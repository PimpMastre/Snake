using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Snake
{
    public partial class Snake : Form
    {
        // Static accessors for the Writing class (title screen animation)
        public static PictureBox[,] GridControls { get; private set; }
        public static ColourScheme ActiveScheme { get; private set; }

        // ---- UI state (non-game) ----
        private string[] paletteList;
        private int paletteIndex;
        private bool isLoggedIn;
        // SnakeClass.GetCurrentUser() is managed via SnakeClass.GetCurrentUser()
        private int selectedHSMenu = 1;
        private int selectedHSMenuLevel = 1;
        private int selectedGameLevel = 1;
        private bool edgeScrollingAllowed = true;
        private bool needsEdgeScrolling;
        private bool paletteUnlocked;
        private bool levelUnlocked;
        private bool gameStarted;
        private bool gamePaused;
        private int currentDifficulty;
        private int currentXP;
        private bool leveledUp;

        // ---- Core objects ----
        private PictureBox[,] PB;
        private readonly Writing writing = new Writing();
        private readonly ImageHandler imageHandler = new ImageHandler();
        private readonly DatabaseHandler dbHandler = new DatabaseHandler();
        private readonly XPSystem xpSystem = new XPSystem();
        private readonly HighScore[] hsTable = new HighScore[11];
        private int tick;
        private int GOTick;
        private int levelTick;
        private int start, end;
        private bool PBLoaded;
        private readonly ColourScheme colourScheme = new ColourScheme();
        private readonly ColourSchemes colourSchemes;
        private int gameScore;
        private int scoreMultiplier;
        private int gameSpeed;
        private int levelMultiplier;
        private int pixelDivider = 32;
        private int pictureBoxSize;

        private readonly GameEngine gameEngine;

        // ---- UI label arrays (for bulk operations) ----
        private readonly Label[] mainMenuLabels;
        private readonly Label[] difficultyLabels;
        private readonly Label[] highScoreNameLabels = new Label[10];
        private readonly Label[] highScoreScoreLabels = new Label[10];
        private readonly Label[] hsLevelLabels = new Label[7];
        private readonly Label[] hsDifficultyLabels = new Label[4];
        private readonly Label[] levelSelectLabels;
        private readonly Label[] pauseMenuLabels;
        private readonly Label[] confirmMenuLabels;

        public Snake()
        {
            paletteList = new[] { "", "default_", "leaf", "aqua", "gameboy", "pastel", "darkred", "grayscale", "nuclear", "onebit", "bokju", "purply", "glow", "oldncold", "mossy", "lavender", "forest", "jungle", "vivid", "winter", "antique", "dirtsnow", "mars", "sleepy" };
            paletteIndex = 1;

            colourSchemes = new ColourSchemes(colourScheme);
            gameEngine = new GameEngine(colourScheme);

            InitializeComponent();

            // Build label arrays for bulk operations (must be after InitializeComponent — panel1 doesn't exist before then)
            mainMenuLabels = FindControlsByPrefix("labelPlay", "labelHighScores", "labelExit", "labelOptions");
            difficultyLabels = FindControlsByPrefix("labelDifficultyEasy", "labelDifficultyMedium", "labelDifficultyHard", "labelDifficultyExtreme");
            hsLevelLabels = FindControlsByPrefix("labelHSLevel1", "labelHSLevel2", "labelHSLevel3", "labelHSLevel4", "labelHSLevel5", "labelHSLevel6", "labelHSLevel7");
            hsDifficultyLabels = FindControlsByPrefix("labelHighScoresEasy", "labelHighScoresMedium", "labelHighScoresHard", "labelHighScoresExtreme");
            levelSelectLabels = FindControlsByPrefix("labelStartGame");
            pauseMenuLabels = FindControlsByPrefix("labelPausedMainMenu", "labelPauseGP", "labelPauseRestart", "labelPauseResume");
            confirmMenuLabels = FindControlsByPrefix("labelPGRestartYes", "labelPGRestartNo", "labelPGMainMenuYes", "labelPGMainMenuNo", "labelMainQuitYes", "labelMainQuitNo", "labelRPYes", "labelRPNo", "labelDeleteAccountYes", "labelDeleteAccountNo", "labelVerifyIdentityYes", "labelVerifyIdentityNo");

            for (int i = 0; i < 10; i++)
            {
                highScoreNameLabels[i] = FindLabelByName("labelName" + (i + 1));
                highScoreScoreLabels[i] = FindLabelByName("labelScore" + (i + 1));
            }

            dbHandler.Initialise();
        }

        private Label[] FindControlsByPrefix(params string[] prefixes)
        {
            var result = new System.Collections.Generic.List<Label>();
            foreach (string prefix in prefixes)
            {
                Control ctrl = null;
                if (panel1 != null)
                    ctrl = panel1.Controls.Find(prefix, false).FirstOrDefault();
                if (ctrl == null)
                    ctrl = Controls.Find(prefix, false).FirstOrDefault();
                if (ctrl is Label c) result.Add(c);
            }
            return result.ToArray();
        }

        private Label FindLabelByName(string name)
        {
            Control ctrl = null;
            if (panel1 != null)
                ctrl = panel1.Controls.Find(name, false).FirstOrDefault();
            if (ctrl == null)
                ctrl = Controls.Find(name, false).FirstOrDefault();
            return ctrl as Label;
        }

        // ===== COLOUR SCHEME =====

        private void ApplyColourScheme()
        {
            // Populate the scheme object from the saved palette selection
            ColourScheme.ApplyById(SnakeClass.GetSelectedPalette(), colourScheme);

            var primary = colourScheme.Primary;
            var secondary = colourScheme.Secondary;
            var tertiary = colourScheme.Tertiary;

            // Set all labels' back/foreground colors in one pass (search all children recursively)
            foreach (var lbl in GetAllControls<Label>())
            {
                lbl.BackColor = primary;
            }
            foreach (var lbl in GetAllControls<Label>())
            {
                // Determine foreground based on the label's role
                lbl.ForeColor = GetLabelColor(lbl.Name);
            }

            // Panel and other containers
            panel1.BackColor = primary;
            foreach (var pb in GetAllControls<PictureBox>().Where(p => p.Name.StartsWith("pictureBoxLevelSelect")))
            {
                pb.BackColor = colourScheme.Level;
                pb.ForeColor = secondary;
            }
        }

        private Color GetLabelColor(string name)
        {
            // Tertiary colors for special labels
            var tertiaryLabels = new[] {
                "labelEasyMP", "labelMediumMP", "labelHardMP", "labelExtremeMP",
                "labelEdgeScrollingMP", "labelReqXP", "labelGOXPNumberMultiplier",
                "labelPalette2", "labelLevelUpUnlocks", "labelLoginError",
                "labelVerifyIdentityIncorrectPass", "labelLSMultiplierBonusChange", "labelLSDifficultyChange",
                "labelPaletteUnlocked", "labelPaletteUnlockedOptions", "labelLevelUnlocked"
            };
            if (tertiaryLabels.Contains(name)) return colourScheme.Tertiary;

            // Tertiary for text boxes
            if (name.StartsWith("textBox")) return colourScheme.Tertiary;

            // Secondary is default for most labels
            return colourScheme.Secondary;
        }

        // ===== FORM LOAD =====

        private void Snake_Load(object sender, EventArgs e)
        {
            ApplyColourScheme();

            labelPlay.Text = isLoggedIn ? "PLAY" : "LOG IN";
            labelPlay.Left = (ClientSize.Width - labelPlay.Width) / 2;

            HideAll();
            ShowMainMenu();

            if (!PBLoaded)
            {
                PB = new PictureBox[pixelDivider + 5, pixelDivider + 5];
                pictureBoxSize = 640 / pixelDivider;
                for (int i = 1; i <= pixelDivider; i++)
                    for (int j = 1; j <= pixelDivider; j++)
                    {
                        PB[i, j] = new PictureBox
                        {
                            Height = pictureBoxSize,
                            Width = pictureBoxSize,
                            Top = pictureBoxSize * (i - 1),
                            Left = pictureBoxSize * (j - 1),
                            BackColor = colourScheme.Primary,
                            Parent = panel1
                        };
                    }
                gameEngine.SetGridControls(PB);
                PBLoaded = true;
            }

            // Update static accessors for Writing class
            GridControls = PB;
            ActiveScheme = colourScheme;

            gameStarted = false;
            timerBlinkXP.Stop();
            RemoveXPBar();
            tick = 0;
            timerWriting.Start();
        }

        // ===== HELPERS =====

        private IEnumerable<T> GetAllControls<T>() where T : Control
        {
            var result = new System.Collections.Generic.List<T>();
            void Walk(System.Windows.Forms.Control parent)
            {
                foreach (Control c in parent.Controls)
                {
                    if (c is T t) result.Add(t);
                    if (c.Controls.Count > 0) Walk(c);
                }
            }
            Walk(panel1);
            return result;
        }

        private void HideAll()
        {
            foreach (var c in GetAllControls<Label>()) c.Visible = false;
            foreach (var c in GetAllControls<PictureBox>()) c.Visible = false;
            foreach (var c in GetAllControls<TextBox>()) c.Visible = false;
        }

        private void ShowMainMenu()
        {
            foreach (var l in mainMenuLabels) l.Visible = true;
            labelPlay.Visible = true;
            labelOptions.Visible = isLoggedIn;

            if (levelUnlocked && isLoggedIn) labelLevelUnlocked.Visible = true;
            if (paletteUnlocked && isLoggedIn) labelPaletteUnlocked.Visible = true;

            // Ensure level select picture boxes are hidden on main menu
            var pic1 = panel1.Controls.Find("pictureBoxLevelSelect1", false).FirstOrDefault() as PictureBox
                       ?? Controls.Find("pictureBoxLevelSelect1", false).FirstOrDefault() as PictureBox;
            var pic2 = panel1.Controls.Find("pictureBoxLevelSelect2", false).FirstOrDefault() as PictureBox
                       ?? Controls.Find("pictureBoxLevelSelect2", false).FirstOrDefault() as PictureBox;
            var pic3 = panel1.Controls.Find("pictureBoxLevelSelect3", false).FirstOrDefault() as PictureBox
                       ?? Controls.Find("pictureBoxLevelSelect3", false).FirstOrDefault() as PictureBox;
            if (pic1 != null) pic1.Visible = false;
            if (pic2 != null) pic2.Visible = false;
            if (pic3 != null) pic3.Visible = false;
        }

        private void HideLabels(Label[] labels)
        {
            foreach (var l in labels) l.Visible = false;
        }

        private void ShowLabels(Label[] labels)
        {
            foreach (var l in labels) l.Visible = true;
        }

        private void ClearHSLabels()
        {
            for (int i = 0; i < 10; i++)
            {
                highScoreNameLabels[i].Visible = false;
                highScoreScoreLabels[i].Visible = false;
            }
        }

        private void ClearHSLevelLabels()
        {
            HideLabels(hsLevelLabels);
        }

        private void ClearHSDifficultyLabels()
        {
            HideLabels(hsDifficultyLabels);
        }

        private void RemoveTitle()
        {
            for (int i = 1; i <= 10; i++)
                for (int j = 1; j <= pixelDivider; j++)
                    PB[i, j].BackColor = colourScheme.Primary;
        }

        private void RemoveXPBar()
        {
            for (int i = 1; i <= pixelDivider; i++)
                PB[19, i].BackColor = colourScheme.Primary;
        }

        private void ShowXPBar()
        {
            var reqXP = xpSystem.GetRequiredXP(SnakeClass.GetPlayerLevel());
            int previousProgress, currentProgress;

            previousProgress = ((SnakeClass.GetLevelXP() % reqXP) * 27 / reqXP);
            currentProgress = currentXP * 27 / reqXP;

            while (SnakeClass.GetLevelXP() + currentXP >= reqXP)
            {
                SnakeClass.PlayerLevel++;
                previousProgress = 0;
                currentXP -= (reqXP - SnakeClass.GetLevelXP());
                SnakeClass.LevelXP = 0;
                currentProgress = currentXP * 27 / reqXP;
                leveledUp = true;
                reqXP = xpSystem.GetRequiredXP(SnakeClass.GetPlayerLevel());
                paletteUnlocked = true;
                int curLevel = SnakeClass.GetPlayerLevel();
                if (curLevel == 3 || curLevel == 7 || curLevel == 11 || curLevel == 14 || curLevel == 18 || curLevel == 21)
                    levelUnlocked = true;
            }

            for (int i = 6; i < previousProgress + 4 && i <= 27; i++)
                PB[19, i].BackColor = colourScheme.Secondary;

            start = previousProgress + 4;
            end = currentProgress + start;
            if (start < 6) start = 6;

            SnakeClass.LevelXP += currentXP;
            timerBlinkXP.Start();
        }

        // ===== GAME =====

        private void StartGame(int diff)
        {
            HideLabels(difficultyLabels);
            labelEdgeScrolling.Visible = false;
            labelEdgeScrollingMP.Visible = false;
            HideLabels(new[] { labelEasyMP, labelMediumMP, labelHardMP, labelExtremeMP, labelBack });

            labelArrowTutorial.Visible = true;
            timerBlinkRate.Start();

            // Set difficulty
            switch (diff)
            {
                case 1: scoreMultiplier = 1; gameSpeed = 250; break;
                case 2: scoreMultiplier = 2; gameSpeed = 150; break;
                case 3: scoreMultiplier = 3; gameSpeed = 50; break;
                case 4: scoreMultiplier = 5; gameSpeed = 30; break;
                default: scoreMultiplier = 1; gameSpeed = 250; break;
            }
            currentDifficulty = diff;

            gameEngine.SetEdgeScrollingAllowed(edgeScrollingAllowed);
            gameEngine.SetNeedsEdgeScrolling(needsEdgeScrolling);
            if (!edgeScrollingAllowed || needsEdgeScrolling) scoreMultiplier *= 2;
            gameEngine.SetDifficultyMultiplier(scoreMultiplier + levelMultiplier);

            RemoveTitle();
            RemoveXPBar();
            timerBlinkXP.Stop();

            gameScore = 0;
            tick = 0;
            levelTick = 0;

            gameEngine.StartGame((GameEngine.Difficulty)diff, selectedGameLevel, levelMultiplier, edgeScrollingAllowed);
            gameEngine.OnScoreChanged += OnScoreChanged;
            gameEngine.OnGameOver += OnGameOver;

            // Set initial snake from GameEngine spawn
            var engine = gameEngine;
            PB[engine.GetSpawnX(), engine.GetSpawnY()].BackColor = colourScheme.Secondary;
            PB[engine.GetSpawnX(), engine.GetSpawnY() + 1].BackColor = colourScheme.Secondary;
            PB[engine.GetSpawnX(), engine.GetSpawnY() + 2].BackColor = colourScheme.Secondary;

            timerGameSpeed.Interval = gameSpeed;
            timerGameSpeed.Start();
            gameStarted = true;
        }

        private void OnScoreChanged(int score)
        {
            gameScore = score;
        }

        private void OnGameOver()
        {
            gameStarted = false;

            // Save data
            if (isLoggedIn)
            {
                dbHandler.SaveHighScore(SnakeClass.GetCurrentUser(), currentDifficulty, gameScore, selectedGameLevel);
                dbHandler.SaveProgress(SnakeClass.GetCurrentUser());
            }

            // Recolor grid to primary
            for (int i = 1; i <= pixelDivider; i++)
                for (int j = 1; j <= pixelDivider; j++)
                    PB[i, j].BackColor = colourScheme.Primary;

            currentXP = gameScore * scoreMultiplier;
            GOTick = 0;
            timerGameOver.Start();
        }

        // ===== KEY DOWN =====

        private void Snake_KeyDown(object sender, KeyEventArgs e)
        {
            if (!gameStarted) return;

            switch (e.KeyCode)
            {
                case Keys.Up when gameEngine.CurrentDirection != GameEngine.Direction.Down:
                    gameEngine.ChangeDirection(GameEngine.Direction.Up); break;
                case Keys.Right when gameEngine.CurrentDirection != GameEngine.Direction.Left:
                    gameEngine.ChangeDirection(GameEngine.Direction.Right); break;
                case Keys.Down when gameEngine.CurrentDirection != GameEngine.Direction.Up:
                    gameEngine.ChangeDirection(GameEngine.Direction.Down); break;
                case Keys.Left when gameEngine.CurrentDirection != GameEngine.Direction.Right:
                    gameEngine.ChangeDirection(GameEngine.Direction.Left); break;
                case Keys.Escape:
                    if (gamePaused) HidePauseMenu(); else ShowPauseMenu();
                    break;
            }
        }

        // ===== TIMER TICKS =====

        private void timerGameSpeed_Tick(object sender, EventArgs e)
        {
            gameEngine.Update();
        }

        private void timerWriting_Tick(object sender, EventArgs e)
        {
            tick++;
            writing.DrawSLetter(3, 5, tick);
            writing.DrawNLetter(3, 10, tick);
            writing.DrawALetter(3, 15, tick);
            writing.DrawKLetter(3, 20, tick);
            writing.DrawELetter(3, 25, tick);
            if (tick > 5) timerWriting.Stop();
        }

        private void timerBlinkRate_Tick(object sender, EventArgs e)
        {
            labelArrowTutorial.Visible = !labelArrowTutorial.Visible;
        }

        private void timerBlinkXP_Tick(object sender, EventArgs e)
        {
            for (int i = start; i <= end && i <= 27; i++)
                PB[19, i].BackColor = PB[19, i].BackColor == colourScheme.Primary ? colourScheme.Tertiary : colourScheme.Primary;
        }

        private void timerGameOver_Tick(object sender, EventArgs e)
        {
            GOTick++;
            switch (GOTick)
            {
                case 1: labelGameOver.Visible = true; break;
                case 2: labelGOScore.Visible = true; break;
                case 3:
                    labelGOScoreNumber.Text = gameScore.ToString();
                    labelGOScoreNumber.Left = (ClientSize.Width - labelGOScoreNumber.Width) / 2;
                    labelGOScoreNumber.Visible = true; break;
                case 4: labelGOXP.Visible = true; break;
                case 5:
                    labelGOXPNumber.Text = currentXP.ToString();
                    labelGOXPNumber.Left = (ClientSize.Width - labelGOXPNumber.Width) / 2;
                    labelGOXPNumber.Visible = true;
                    labelGOXPNumberMultiplier.Text = $"(X{scoreMultiplier})";
                    labelGOXPNumberMultiplier.Left = labelGOXPNumber.Left + labelGOXPNumber.Width + 3;
                    labelGOXPNumberMultiplier.Visible = true; break;
                case 6:
                    if (SnakeClass.GetPlayerLevel() == 23)
                    {
                        labelLevel.Text = $"LEVEL {SnakeClass.GetPlayerLevel()}";
                        labelLevel.Left = (ClientSize.Width - labelLevel.Width) / 2;
                        labelLevel.Visible = true;
                        labelLevelUpUnlocks.Visible = false;
                        labelGOReplay.Visible = true;
                        labelGOMainMenu.Visible = true;
                        labelReqXP.Visible = false;
                        for (int i = 6; i <= 27; i++) PB[19, i].BackColor = colourScheme.Secondary;
                    }
                    else
                    {
                        ShowXPBar();
                        labelLevel.Text = $"LEVEL {SnakeClass.GetPlayerLevel()}";
                        labelLevel.Left = (ClientSize.Width - labelLevel.Width) / 2;
                        labelLevel.Visible = true;
                        if (leveledUp)
                        {
                            timerBlinkLevel.Start();
                            int hsLevel = SnakeClass.GetPlayerLevel();
                            labelLevelUpUnlocks.Text = (hsLevel == 3 || hsLevel == 7 || hsLevel == 11 || hsLevel == 14 || hsLevel == 18 || hsLevel == 21)
                                ? "NEW PALETTE AND LEVEL UNLOCKED"
                                : "NEW PALETTE UNLOCKED";
                            labelLevelUpUnlocks.Left = (ClientSize.Width - labelLevelUpUnlocks.Width) / 2;
                            labelLevelUpUnlocks.Visible = true;
                            leveledUp = false;
                        }
                        labelReqXP.Text = $"({(xpSystem.GetRequiredXP(SnakeClass.GetPlayerLevel()) - SnakeClass.GetLevelXP())} REQUIRED TO LEVEL UP)";
                        labelReqXP.Left = (ClientSize.Width - labelReqXP.Width) / 2;
                        labelReqXP.Visible = true;
                        labelGOReplay.Visible = true;
                        labelGOMainMenu.Visible = true;
                    }
                    break;
                case 7: timerGameOver.Stop(); break;
            }
        }

        private void timerPickupBlink_Tick(object sender, EventArgs e)
        {
            if (gameEngine.IsPickupActive())
            {
                var bg = PB[gameEngine.GetPickupX(), gameEngine.GetPickupY()].BackColor;
                PB[gameEngine.GetPickupX(), gameEngine.GetPickupY()].BackColor = bg == colourScheme.Tertiary ? colourScheme.Secondary : colourScheme.Tertiary;
            }
        }

        private void timerBlinkLevel_Tick(object sender, EventArgs e)
        {
            levelTick++;
            labelLevel.Visible = !labelLevel.Visible;
            if (levelTick > 10)
            {
                labelLevel.Visible = true;
                timerBlinkLevel.Stop();
            }
        }

        // ===== MENU HELPERS =====

        private void ShowPauseMenu()
        {
            timerGameSpeed.Stop();
            gameEngine.HideSnake();
            gamePaused = true;
            ShowLabels(pauseMenuLabels);
        }

        private void HidePauseMenu()
        {
            if (gameEngine.CurrentDirection == GameEngine.Direction.None)
            {
                timerBlinkRate.Start();
                labelArrowTutorial.Visible = true;
            }

            // Redraw full game state (snake, obstacles, pickup)
            gameEngine.Render();

            gamePaused = false;
            timerPickupBlink.Start();
            HideLabels(pauseMenuLabels);
            timerGameSpeed.Start();
        }

        private void ShowHighScores()
        {
            ClearHSLabels();
            ClearHSLevelLabels();
            ClearHSDifficultyLabels();

            // Sort high scores
            for (int i = 1; i <= 10; i++)
                for (int j = i + 1; j <= 10; j++)
                    if (hsTable[i].Score < hsTable[j].Score)
                    {
                        var tmp = hsTable[i]; hsTable[i] = hsTable[j]; hsTable[j] = tmp;
                    }

            for (int i = 0; i < 10; i++)
            {
                highScoreNameLabels[i].Text = $"{i + 1}. {hsTable[i + 1].Name}";
                highScoreScoreLabels[i].Text = hsTable[i + 1].Score.ToString();
                highScoreNameLabels[i].Visible = true;
                highScoreScoreLabels[i].Visible = true;
            }
        }

        // ===== BUTTON CLICKS =====

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (isLoggedIn)
            {
                ShowLevelSelect();
            }
            else
            {
                ShowLogin();
            }
        }

        private void ShowLevelSelect()
        {
            HideMainMenu();
            timerWriting.Stop();
            timerBlinkXP.Stop();
            RemoveXPBar();
            RemoveTitle();
            levelUnlocked = false;

            ShowLabels(new[] { labelLevelSelectTop, labelBack, labelStartGame });
            ShowLabels(levelSelectLabels);
            labelLSDifficultyTop.Visible = true;
            labelLSDifficultyChange.Visible = true;
            labelLSMultiplierBonusTop.Visible = true;
            labelLSMultiplierBonusChange.Visible = true;

            var pic1 = panel1.Controls.Find("pictureBoxLevelSelect1", false).FirstOrDefault() as PictureBox
                       ?? Controls.Find("pictureBoxLevelSelect1", false).FirstOrDefault() as PictureBox;
            var pic2 = panel1.Controls.Find("pictureBoxLevelSelect2", false).FirstOrDefault() as PictureBox
                       ?? Controls.Find("pictureBoxLevelSelect2", false).FirstOrDefault() as PictureBox;
            var pic3 = panel1.Controls.Find("pictureBoxLevelSelect3", false).FirstOrDefault() as PictureBox
                       ?? Controls.Find("pictureBoxLevelSelect3", false).FirstOrDefault() as PictureBox;

            if (pic1 != null) pic1.Visible = selectedGameLevel > 1;
            if (pic2 != null) pic2.Visible = true;
            if (pic3 != null) pic3.Visible = selectedGameLevel < 7;

            UpdateLevelImages();
        }

        private void UpdateLevelImages()
        {
            var pic1 = panel1.Controls.Find("pictureBoxLevelSelect1", false).FirstOrDefault() as PictureBox
                       ?? Controls.Find("pictureBoxLevelSelect1", false).FirstOrDefault() as PictureBox;
            var pic2 = panel1.Controls.Find("pictureBoxLevelSelect2", false).FirstOrDefault() as PictureBox
                       ?? Controls.Find("pictureBoxLevelSelect2", false).FirstOrDefault() as PictureBox;
            var pic3 = panel1.Controls.Find("pictureBoxLevelSelect3", false).FirstOrDefault() as PictureBox
                       ?? Controls.Find("pictureBoxLevelSelect3", false).FirstOrDefault() as PictureBox;

            if (pic1 != null && selectedGameLevel > 1)
                pic1.BackgroundImage = Image.FromFile(imageHandler.GetImage(selectedGameLevel - 1));
            if (pic2 != null)
                pic2.BackgroundImage = Image.FromFile(imageHandler.GetImage(selectedGameLevel));
            if (pic3 != null && selectedGameLevel < 7)
                pic3.BackgroundImage = Image.FromFile(imageHandler.GetImage(selectedGameLevel + 1));

            UpdateLevelDifficultyDisplay();
        }

        private void UpdateLevelDifficultyDisplay()
        {
            string diffText;
            string multText;
            switch (selectedGameLevel)
            {
                case 1: diffText = "NONE"; multText = "+0"; break;
                case 2: diffText = "EASY"; multText = "+1"; break;
                case 3:
                case 4: diffText = "MEDIUM"; multText = "+2"; break;
                case 5: diffText = "HARD"; multText = "+3"; break;
                case 6:
                case 7: diffText = "EXTREME"; multText = "+4"; break;
                default: diffText = "NONE"; multText = "+0"; break;
            }
            labelLSDifficultyChange.Text = diffText;
            labelLSMultiplierBonusChange.Text = multText;
            labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
            labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);
        }

        private void buttonHighScores_Click(object sender, EventArgs e)
        {
            HideMainMenu();
            timerWriting.Stop();
            RemoveTitle();

            ShowLabels(hsLevelLabels);
            ShowLabels(hsDifficultyLabels);
            labelBack.Visible = true;

            selectedHSMenu = 1;
            hsLevelLabels[0].ForeColor = colourScheme.Tertiary;
            hsDifficultyLabels[0].ForeColor = colourScheme.Tertiary;

            ReadAndDisplayScores(selectedHSMenu, selectedHSMenuLevel);
        }

        private void ReadAndDisplayScores(int diff, int level)
        {
            if (isLoggedIn)
            {
                dbHandler.ReadHighScores(diff, level, hsTable);
            }
            else
            {
                // Clear scores for non-logged-in users
                for (int i = 1; i <= 10; i++)
                {
                    hsTable[i].Name = "---";
                    hsTable[i].Score = 0;
                }
            }
            ShowHighScores();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            timerWriting.Stop();
            RemoveTitle();
            HideMainMenu();
            labelMainQuitQ.Visible = true;
            labelMainQuitYes.Visible = true;
            labelMainQuitNo.Visible = true;
        }

        private void labelMainQuitYes_Click(object sender, EventArgs e)
        {
            if (isLoggedIn) dbHandler.SaveProgress(SnakeClass.GetCurrentUser());
            Close();
        }

        private void labelMainQuitNo_Click(object sender, EventArgs e)
        {
            Snake_Load(sender, e);
        }

        private void labelBack_Click(object sender, EventArgs e)
        {
            HideLabels(new[] { labelBack });
            ShowMainMenu();
        }

        private void labelOptions_Click(object sender, EventArgs e)
        {
            timerWriting.Stop();
            RemoveTitle();
            HideMainMenu();

            labelPaletteUnlockedOptions.Visible = paletteUnlocked;
            ShowLabels(new[] { labelChoosePalette, labelResetProgress, labelRemoveAccount, labelLogOut, labelBack });
        }

        private void labelLogOut_Click(object sender, EventArgs e)
        {
            dbHandler.SaveProgress(SnakeClass.GetCurrentUser());
            SnakeClass.SetLoggedOut();
            labelPlay.Text = "LOG IN";
            labelPlay.Left = (ClientSize.Width - labelPlay.Width) / 2;
            paletteIndex = 1;
            Snake_Load(sender, e);
        }

        private void labelChoosePalette_Click(object sender, EventArgs e)
        {
            HideLabels(new[] { labelChoosePalette, labelResetProgress, labelBack, labelLogOut, labelRemoveAccount, labelPaletteUnlockedOptions });
            paletteUnlocked = false;

            labelBackPalette.Visible = true;
            labelChoosePaletteI.Visible = true;

            UpdatePaletteDisplay();
        }

        private void UpdatePaletteDisplay()
        {
            labelPalette1.Visible = false;
            labelPalette3.Visible = false;

            if (paletteIndex > 1)
            {
                labelPalette1.Text = paletteList[paletteIndex - 1];
                labelPalette1.Visible = true;
            }

            labelPalette2.Text = paletteList[paletteIndex];
            labelPalette2.Visible = true;
            labelPalette2.ForeColor = colourScheme.Tertiary;

            if (paletteIndex < 23 && !imageHandler.IsLocked(paletteIndex + 1))
            {
                labelPalette3.Text = paletteList[paletteIndex + 1];
                labelPalette3.Visible = true;
            }

            CenterPalettes();
        }

        private void CenterPalettes()
        {
            labelPalette1.Left = (ClientSize.Width - labelPalette1.Width) / 2;
            labelPalette2.Left = (ClientSize.Width - labelPalette2.Width) / 2 + 8;
            labelPalette3.Left = (ClientSize.Width - labelPalette3.Width) / 2;
        }

        private void labelPalette1_Click(object sender, EventArgs e)
        {
            paletteIndex--;
            ApplyPalette();
            labelPalette1_MouseEnter(sender, e);
        }

        private void labelPalette3_Click(object sender, EventArgs e)
        {
            if (paletteIndex >= 23 || imageHandler.IsLocked(paletteIndex + 1))
                return;
            paletteIndex++;
            ApplyPalette();
            labelPalette3_MouseEnter(sender, e);
        }

        private void ApplyPalette()
        {
            colourSchemes.ApplyScheme(paletteList[paletteIndex]);
            ApplyColourScheme();

            labelPalette1.Visible = false;
            labelPalette3.Visible = false;

            if (paletteIndex > 1)
            {
                labelPalette1.Text = paletteList[paletteIndex - 1];
                labelPalette1.Visible = true;
            }

            labelPalette2.Text = paletteList[paletteIndex];

            if (paletteIndex < 23 && !imageHandler.IsLocked(paletteIndex + 1))
            {
                labelPalette3.Text = paletteList[paletteIndex + 1];
                labelPalette3.Visible = true;
            }

            CenterPalettes();
        }

        private void labelBackPalette_Click(object sender, EventArgs e)
        {
            labelBackPalette.Visible = false;
            labelBack.Visible = true;
            labelPalette1.Visible = false;
            labelPalette2.Visible = false;
            labelPalette3.Visible = false;
            labelChoosePaletteI.Visible = false;
            labelOptions_Click(sender, e);
        }

        private void labelResetProgress_Click(object sender, EventArgs e)
        {
            HideLabels(new[] { labelBack, labelResetProgress, labelChoosePalette, labelLogOut, labelRemoveAccount });
            ShowLabels(new[] { labelRPQuestion, labelRPYes, labelRPNo });
        }

        private void labelRPNo_Click(object sender, EventArgs e)
        {
            HideLabels(new[] { labelRPQuestion, labelRPYes, labelRPNo });
            labelOptions_Click(sender, e);
        }

        private void labelRPYes_Click(object sender, EventArgs e)
        {
            SnakeClass.PlayerLevel = 1;
            SnakeClass.LevelXP = 0;
            SnakeClass.SelectedPalette = 1;
            paletteIndex = 1;
            dbHandler.ResetProgress(SnakeClass.GetCurrentUser());
            HideLabels(new[] { labelRPQuestion, labelRPYes, labelRPNo });
            labelOptions_Click(sender, e);
        }

        private void labelRemoveAccount_Click(object sender, EventArgs e)
        {
            HideLabels(new[] { labelBack, labelResetProgress, labelChoosePalette, labelLogOut, labelRemoveAccount });
            ShowLabels(new[] { labelDeleteAccountQ, labelDeleteAccountYes, labelDeleteAccountNo });
        }

        private void labelDeleteAccountNo_Click(object sender, EventArgs e)
        {
            HideLabels(new[] { labelDeleteAccountQ, labelDeleteAccountYes, labelDeleteAccountNo });
            labelOptions_Click(sender, e);
        }

        private void labelDeleteAccountYes_Click(object sender, EventArgs e)
        {
            HideLabels(new[] { labelDeleteAccountQ, labelDeleteAccountYes, labelDeleteAccountNo });
            labelVerifyIdentityPass.Visible = true;
            textBoxVerifyIdentityPass.Text = "password";
            textBoxVerifyIdentityPass.Visible = true;
            ShowLabels(new[] { labelVerifyIdentityQ, labelVerifyIdentitySure, labelVerifyIdentityYes, labelVerifyIdentityNo });
        }

        private void labelVerifyIdentityYes_Click(object sender, EventArgs e)
        {
            var result = dbHandler.DeleteAccount(SnakeClass.GetCurrentUser(), textBoxVerifyIdentityPass.Text);
            if (result == 1)
            {
                labelVerifyIdentityIncorrectPass.Visible = true;
                timerVIPassError.Start();
            }
            else
            {
                HideVerifyFields();
                SnakeClass.SelectedPalette = 1;
                paletteIndex = 1;
                SnakeClass.SetLoggedOut();
                colourSchemes.ApplyScheme(paletteList[1]);
                ApplyColourScheme();
                dbHandler.Initialise();
                Snake_Load(sender, e);
            }
        }

        private void labelVerifyIdentityNo_Click(object sender, EventArgs e)
        {
            HideVerifyFields();
            labelOptions_Click(sender, e);
        }

        private void HideVerifyFields()
        {
            labelVerifyIdentityPass.Visible = false;
            textBoxVerifyIdentityPass.Visible = false;
            HideLabels(new[] { labelVerifyIdentityQ, labelVerifyIdentitySure, labelVerifyIdentityYes, labelVerifyIdentityNo });
        }

        private void labelLogIn_Click(object sender, EventArgs e)
        {
            if (textBoxUserName.Text == "user" || textBoxUserName.Text == "")
            {
                ShowLoginError("INVALID USER NAME");
                return;
            }

            var result = dbHandler.CheckLoginCredentials(textBoxUserName.Text, textBoxPassword.Text,
                out var playerLevel, out var levelXP, out var userPalette);

            switch (result)
            {
                case 1: ShowLoginError("NO USER FOUND"); break;
                case 2: ShowLoginError("INCORRECT PASSWORD"); break;
                case 3:
                    isLoggedIn = true;
                    SnakeClass.SetLoggedIn(textBoxUserName.Text);
                    SnakeClass.PlayerLevel = playerLevel;
                    SnakeClass.LevelXP = levelXP;
                    SnakeClass.SelectedPalette = userPalette;
                    paletteIndex = userPalette;
                    colourSchemes.ApplyScheme(paletteList[userPalette]);
                    ApplyColourScheme();
                    Snake_Load(sender, e);
                    break;
            }
        }

        private void labelNewAccount_Click(object sender, EventArgs e)
        {
            if (textBoxUserName.Text == "user" || textBoxUserName.Text == "")
            {
                ShowLoginError("ILLEGAL USER NAME");
                return;
            }

            var result = dbHandler.CheckLoginCredentials(textBoxUserName.Text, textBoxPassword.Text, out _, out _, out _);
            if (result == 1)
            {
                if (textBoxPassword.Text != null)
                {
                    dbHandler.AddNewAccount(textBoxUserName.Text, textBoxPassword.Text);
                    isLoggedIn = true;
                    dbHandler.Initialise();
                    SnakeClass.SetLoggedIn(textBoxUserName.Text);
                    Snake_Load(sender, e);
                }
            }
            else
            {
                ShowLoginError("USER NAME TAKEN");
            }
        }

        private void ShowLogin()
        {
            HideMainMenu();
            timerWriting.Stop();
            timerBlinkXP.Stop();
            RemoveXPBar();
            RemoveTitle();

            ShowLabels(new[] { labelUserName, labelPassword, labelLogIn, labelNewAccount, labelBack });
            textBoxUserName.Visible = true;
            textBoxPassword.Visible = true;
        }

        private void ShowLoginError(string message)
        {
            labelLoginError.Text = message;
            labelLoginError.Left = (ClientSize.Width - labelLoginError.Width) / 2;
            labelLoginError.Visible = true;
            timerLabelError.Start();
        }

        private void HideMainMenu()
        {
            HideLabels(mainMenuLabels);
            labelPaletteUnlocked.Visible = false;
            labelLevelUnlocked.Visible = false;
        }

        // ===== LEVEL SELECT =====

        private void pictureBoxLevelSelect1_Click(object sender, EventArgs e)
        {
            if (selectedGameLevel > 1)
            {
                selectedGameLevel--;
                UpdateLevelImages();
            }
        }

        private void pictureBoxLevelSelect2_Click(object sender, EventArgs e)
        {
            if (!imageHandler.IsLocked(selectedGameLevel))
            {
                HideLevelSelect();
                ShowDifficultySelect();
            }
        }

        private void pictureBoxLevelSelect3_Click(object sender, EventArgs e)
        {
            if (selectedGameLevel < 7)
            {
                selectedGameLevel++;
                UpdateLevelImages();
            }
            else
            {
                // Already at last level — do nothing
            }
        }

        private void HideLevelSelect()
        {
            var pic1 = panel1.Controls.Find("pictureBoxLevelSelect1", false).FirstOrDefault() as PictureBox
                       ?? Controls.Find("pictureBoxLevelSelect1", false).FirstOrDefault() as PictureBox;
            var pic2 = panel1.Controls.Find("pictureBoxLevelSelect2", false).FirstOrDefault() as PictureBox
                       ?? Controls.Find("pictureBoxLevelSelect2", false).FirstOrDefault() as PictureBox;
            var pic3 = panel1.Controls.Find("pictureBoxLevelSelect3", false).FirstOrDefault() as PictureBox
                       ?? Controls.Find("pictureBoxLevelSelect3", false).FirstOrDefault() as PictureBox;

            HideLabels(new[] { labelLevelSelectTop, labelStartGame });
            HideLabels(levelSelectLabels);
            if (pic1 != null) pic1.Visible = false;
            if (pic2 != null) pic2.Visible = false;
            if (pic3 != null) pic3.Visible = false;
            HideLabels(new[] { labelBack, labelLSDifficultyTop, labelLSDifficultyChange, labelLSMultiplierBonusTop, labelLSMultiplierBonusChange });
        }

        private void ShowDifficultySelect()
        {
            ShowLabels(difficultyLabels);
            ShowLabels(new[] { labelBack });
            labelEdgeScrolling.Visible = true;
            labelEdgeScrollingMP.Visible = true;
            HideLabels(new[] { labelEasyMP, labelMediumMP, labelHardMP, labelExtremeMP });
            labelBackDifficulty.Visible = true;
            labelLSDifficultyTop.Visible = true;
            labelLSDifficultyChange.Visible = true;
            labelLSMultiplierBonusTop.Visible = true;
            labelLSMultiplierBonusChange.Visible = true;
            labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
            labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);

            // Level-specific settings
            switch (selectedGameLevel)
            {
                case 4:
                case 5:
                    labelEdgeScrolling.Visible = false;
                    labelEdgeScrollingMP.Visible = false;
                    edgeScrollingAllowed = true;
                    needsEdgeScrolling = true;
                    break;
                case 2:
                    labelEdgeScrolling.Visible = false;
                    labelEdgeScrollingMP.Visible = false;
                    edgeScrollingAllowed = false;
                    needsEdgeScrolling = false;
                    break;
                default:
                    needsEdgeScrolling = false;
                    labelEdgeScrolling.Left = (ClientSize.Width - labelEdgeScrolling.Width) / 2;
                    break;
            }

            switch (selectedGameLevel)
            {
                case 1: levelMultiplier = 0; break;
                case 2: levelMultiplier = 1; break;
                case 3: levelMultiplier = 2; break;
                case 4: levelMultiplier = 2; break;
                case 5: levelMultiplier = 3; break;
                case 6: levelMultiplier = 4; break;
                case 7: levelMultiplier = 4; break;
                default: levelMultiplier = 0; break;
            }
        }

        private void labelDifficultyEasy_Click(object sender, EventArgs e) => StartGame(1);
        private void labelDifficultyMedium_Click(object sender, EventArgs e) => StartGame(2);
        private void labelDifficultyHard_Click(object sender, EventArgs e) => StartGame(3);
        private void labelDifficultyExtreme_Click(object sender, EventArgs e) => StartGame(4);

        private void label6_Click(object sender, EventArgs e)
        {
            edgeScrollingAllowed = !edgeScrollingAllowed;
            labelEdgeScrolling.Text = edgeScrollingAllowed ? "EDGE SCROLLING ENABLED" : "EDGE SCROLLING DISABLED";
            labelEdgeScrolling.Left = (ClientSize.Width - labelEdgeScrolling.Width) / 2;
        }

        private void labelBackDifficulty_Click(object sender, EventArgs e)
        {
            HideLabels(new[] { labelBackDifficulty });
            HideLabels(difficultyLabels);
            HideLabels(new[] { labelEdgeScrolling, labelEdgeScrollingMP, labelEasyMP, labelMediumMP, labelHardMP, labelExtremeMP });

            ShowLabels(new[] { labelBack, labelStartGame });
            ShowLabels(new[] { labelLevelSelectTop });
            ShowLabels(levelSelectLabels);
            var pic1 = panel1.Controls.Find("pictureBoxLevelSelect1", false).FirstOrDefault() as PictureBox
                       ?? Controls.Find("pictureBoxLevelSelect1", false).FirstOrDefault() as PictureBox;
            var pic2 = panel1.Controls.Find("pictureBoxLevelSelect2", false).FirstOrDefault() as PictureBox
                       ?? Controls.Find("pictureBoxLevelSelect2", false).FirstOrDefault() as PictureBox;
            var pic3 = panel1.Controls.Find("pictureBoxLevelSelect3", false).FirstOrDefault() as PictureBox
                       ?? Controls.Find("pictureBoxLevelSelect3", false).FirstOrDefault() as PictureBox;
            if (pic1 != null) pic1.Visible = selectedGameLevel > 1;
            if (pic2 != null) pic2.Visible = true;
            if (pic3 != null) pic3.Visible = selectedGameLevel < 7;
            ShowLabels(new[] { labelLSDifficultyTop, labelLSDifficultyChange, labelLSMultiplierBonusTop, labelLSMultiplierBonusChange });
        }

        // ===== PAUSE MENU =====

        private void labelPauseResume_Click(object sender, EventArgs e) => HidePauseMenu();

        private void labelPauseRestart_Click(object sender, EventArgs e)
        {
            HideLabels(pauseMenuLabels);
            ShowLabels(new[] { labelPGRestartQ, labelPGRestartNo, labelPGRestartYes });
        }

        private void labelPGRestartYes_Click(object sender, EventArgs e)
        {
            HideLabels(new[] { labelPGRestartQ, labelPGRestartYes, labelPGRestartNo });
            gameStarted = false;
            StartGame(currentDifficulty);
        }

        private void labelPGRestartNo_Click(object sender, EventArgs e)
        {
            HideLabels(new[] { labelPGRestartQ, labelPGRestartYes, labelPGRestartNo });
            ShowLabels(pauseMenuLabels);
        }

        private void labelPausedMainMenu_Click(object sender, EventArgs e)
        {
            HideLabels(pauseMenuLabels);
            ShowLabels(new[] { labelPGMainMenuQ, labelPGMainMenuNo, labelPGMainMenuYes });
        }

        private void labelPGMainMenuNo_Click(object sender, EventArgs e)
        {
            HideLabels(new[] { labelPGMainMenuQ, labelPGMainMenuYes, labelPGMainMenuNo });
            ShowLabels(pauseMenuLabels);
        }

        private void labelPGMainMenuYes_Click(object sender, EventArgs e) => Snake_Load(sender, e);

        // ===== HS LEVEL / DIFFICULTY CLICKS =====

        private void SetActiveHSLevel(int idx)
        {
            selectedHSMenuLevel = idx;
            for (int i = 0; i < hsLevelLabels.Length; i++)
                hsLevelLabels[i].ForeColor = i == idx - 1 ? colourScheme.Tertiary : colourScheme.Secondary;
            ReadAndDisplayScores(selectedHSMenu, selectedHSMenuLevel);
        }

        private void SetActiveHSDifficulty(int idx)
        {
            selectedHSMenu = idx;
            for (int i = 0; i < hsDifficultyLabels.Length; i++)
                hsDifficultyLabels[i].ForeColor = i == idx - 1 ? colourScheme.Tertiary : colourScheme.Secondary;
            ReadAndDisplayScores(selectedHSMenu, selectedHSMenuLevel);
        }

        private void labelHSLevel1_Click(object sender, EventArgs e) => SetActiveHSLevel(1);
        private void labelHSLevel2_Click(object sender, EventArgs e) => SetActiveHSLevel(2);
        private void labelHSLevel3_Click(object sender, EventArgs e) => SetActiveHSLevel(3);
        private void labelHSLevel4_Click(object sender, EventArgs e) => SetActiveHSLevel(4);
        private void labelHSLevel5_Click(object sender, EventArgs e) => SetActiveHSLevel(5);
        private void labelHSLevel6_Click(object sender, EventArgs e) => SetActiveHSLevel(6);
        private void labelHSLevel7_Click(object sender, EventArgs e) => SetActiveHSLevel(7);

        private void labelHighScoresEasy_Click(object sender, EventArgs e) => SetActiveHSDifficulty(1);
        private void labelHighScoresMedium_Click(object sender, EventArgs e) => SetActiveHSDifficulty(2);
        private void labelHighScoresHard_Click(object sender, EventArgs e) => SetActiveHSDifficulty(3);
        private void labelHighScoresExtreme_Click(object sender, EventArgs e) => SetActiveHSDifficulty(4);

        // ===== SHARED MOUSE HANDLERS =====

        private void SharedMouseEnter(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null)
            {
                lbl.ForeColor = colourScheme.Tertiary;

                // Start blink timers for confirmation buttons
                if (lbl.Name == "labelRPYes")
                {
                    timerRPYesBlink.Start();
                }
                else if (lbl.Name == "labelDeleteAccountYes")
                {
                    timerDAYesBlink.Start();
                }
                else if (lbl.Name == "labelVerifyIdentityYes")
                {
                    timerVIBlink.Start();
                }
            }
        }

        private void SharedMouseLeave(object sender, EventArgs e)
        {
            Label lbl = sender as Label;
            if (lbl != null)
            {
                lbl.ForeColor = colourScheme.Secondary;

                // Stop blink timers
                if (lbl.Name == "labelRPYes")
                {
                    timerRPYesBlink.Stop();
                }
                else if (lbl.Name == "labelDeleteAccountYes")
                {
                    timerDAYesBlink.Stop();
                }
                else if (lbl.Name == "labelVerifyIdentityYes")
                {
                    timerVIBlink.Stop();
                }
            }
        }

        // ===== TEXT BOX CLICKS =====

        private void textBoxUserName_Click(object sender, EventArgs e) => textBoxUserName.Text = "";
        private void textBoxPassword_Click(object sender, EventArgs e) => textBoxPassword.Text = "";
        private void textBoxVerifyIdentityPass_Click(object sender, EventArgs e) => textBoxVerifyIdentityPass.Text = "";

        // ===== TIMER ERROR HANDLERS =====

        private void timerRPYesBlink_Tick(object sender, EventArgs e)
        {
            labelRPYes.ForeColor = labelRPYes.ForeColor == colourScheme.Tertiary ? colourScheme.Secondary : colourScheme.Tertiary;
        }

        private void timerDAYesBlink_Tick(object sender, EventArgs e)
        {
            labelDeleteAccountYes.ForeColor = labelDeleteAccountYes.ForeColor == colourScheme.Tertiary ? colourScheme.Secondary : colourScheme.Tertiary;
        }

        private void timerVIBlink_Tick(object sender, EventArgs e)
        {
            labelVerifyIdentityYes.ForeColor = labelVerifyIdentityYes.ForeColor == colourScheme.Tertiary ? colourScheme.Secondary : colourScheme.Tertiary;
        }

        private void timerUserTaken_Tick(object sender, EventArgs e)
        {
            labelLoginError.Visible = false;
            timerLabelError.Stop();
        }

        private void timerVIPassError_Tick(object sender, EventArgs e)
        {
            labelVerifyIdentityIncorrectPass.Visible = false;
            timerVIPassError.Stop();
        }

        // ===== SHARED MOUSE WRAPPERS =====
        // These wrapper methods match the Designer's existing event subscriptions
        // and delegate to the shared handlers.

        private void labelPlay_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelPlay_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelHighScores_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelHighScores_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelOptions_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelOptions_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelExit_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelExit_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelDifficultyEasy_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelDifficultyEasy_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelDifficultyMedium_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelDifficultyMedium_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelDifficultyHard_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelDifficultyHard_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelDifficultyExtreme_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelDifficultyExtreme_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelEdgeScrolling_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelEdgeScrolling_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelGOMainMenu_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelGOMainMenu_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelBack_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelBack_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelNewAccount_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelNewAccount_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelLogIn_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelLogIn_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelResetProgress_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelResetProgress_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelChoosePalette_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelChoosePalette_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelBackPalette_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelBackPalette_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelPalette1_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelPalette1_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelPalette3_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelPalette3_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelRPYes_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelRPYes_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelRPNo_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelRPNo_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelPauseResume_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelPauseResume_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelPauseRestart_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelPauseRestart_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelPausedMainMenu_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelPausedMainMenu_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelPGRestartYes_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelPGRestartYes_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelPGRestartNo_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelPGRestartNo_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelPGMainMenuYes_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelPGMainMenuYes_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelPGMainMenuNo_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelPGMainMenuNo_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelMainQuitYes_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelMainQuitYes_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelMainQuitNo_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelMainQuitNo_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelHighScoresEasy_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelHighScoresEasy_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelHighScoresMedium_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelHighScoresMedium_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelHighScoresHard_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelHighScoresHard_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelHighScoresExtreme_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelHighScoresExtreme_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelLogOut_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelLogOut_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelDeleteAccountYes_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelDeleteAccountYes_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelDeleteAccountNo_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelDeleteAccountNo_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelRemoveAccount_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelRemoveAccount_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelVerifyIdentityYes_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelVerifyIdentityYes_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelVerifyIdentityNo_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelVerifyIdentityNo_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelBackDifficulty_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelBackDifficulty_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelHSLevel1_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelHSLevel1_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelHSLevel2_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelHSLevel2_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelHSLevel3_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelHSLevel3_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelHSLevel4_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelHSLevel4_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelHSLevel5_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelHSLevel5_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelHSLevel6_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelHSLevel6_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelHSLevel7_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelHSLevel7_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
        private void labelGOReplay_MouseEnter(object sender, EventArgs e) => SharedMouseEnter(sender, e);
        private void labelGOReplay_MouseLeave(object sender, EventArgs e) => SharedMouseLeave(sender, e);
    }
}
