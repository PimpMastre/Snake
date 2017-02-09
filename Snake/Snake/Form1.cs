using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Snake
{
    public partial class Snake : Form
    {
        struct Tail
        {
            public int xPos, yPos; // pozitia pe coloana = x, pozitia pe linie = y
        }

        public struct HighScore
        {
            public int score;
            public string name;
        }

        public struct colourScheme
        {
            public Color Primary, Secondary, Tertiary, Level;
        }

        public string[] paletteList = { "", "default_", "leaf", "aqua", "gameboy", "pastel", "darkred", "grayscale", "nuclear", "onebit", "bokju", "purply", "glow", "oldncold", "mossy", "lavender", "forest", "jungle", "vivid", "winter", "antique", "dirtsnow", "mars", "sleepy" };
        public static int selectedPalette = 1;
        public static int userPalette = 1;

        Random r = new Random();
        int randomX, randomY;
        int randomXSpawn, randomYSpawn;

        bool PBLoaded = false;
        bool edgeScrollingAllowed = true;
        bool needsEdgeScrolling = false;
        bool gameStopped = false;
        bool gameStarted = false;
        bool leveledUp = false;
        bool loggedIn = false;
        bool gamePaused = false;
        bool levelUnlocked = false;
        bool paletteUnlocked = false;
        int selectedHSMenu = 1;
        int selectedHSMenuLevel = 1;
        int selectedGameLevel = 1;
        int tick = 0;
        int GOTick;
        int levelTick = 0;
        int gameScore = 0;
        int currentDifficulty = 0;
        int difficultyMultiplier = 0;
        int levelMultiplier = 0;
        public static  int level = 1;
        public static int levelXP = 0;
        string currentUser;
        public int levelRequiredXP;
        int currentXP = 0;
        int start, end;
        int direction = 0; // 0 inseamna nimic, 1- sus, 2- dreapta, 3- jos, 4- stanga
        int newDirection = 0; // pentru a nu da update la directie instantaneu
        int gameSpeed;
        int pixelDivider = 32; // default 32
        int pictureBoxSize;
        int snakeLength = 1;
        int queueLength = 0;
        public static PictureBox[,] PB;
        public static char[,] GB;
        Tail[] tail;
        Tail pickupLocation;
        Tail aux = new Tail();
        Tail aux2 = new Tail();
        public static colourScheme activeScheme;
        public static HighScore[] hsTable = new HighScore[11];
        Writing writing = new Writing();
        ColourSchemes colourSchemes = new ColourSchemes();
        XPSystem xps = new XPSystem();
        DatabaseHandler dbHandler = new DatabaseHandler();
        ImageHandler imageHandler = new ImageHandler();
        
        public Snake()
        {
            InitializeComponent();
            dbHandler.DBInitialisation();
        }

        //--------------FORM LOAD EVENT--------------\\

        private void Snake_Load(object sender, EventArgs e)
        {
            // schimbam color scheme-ul
            if(loggedIn == false)
                colourSchemes.Calls(paletteList[selectedPalette]);

            if(loggedIn == false)
            {
                labelPlay.Text = "LOG IN";
                labelPlay.Left = (this.ClientSize.Width - labelPlay.Width) / 2;
            }
            else
            {
                labelPlay.Text = "PLAY";
                labelPlay.Left = (this.ClientSize.Width - labelPlay.Width) / 2;
            }

            // ascundem tot
            textBoxUserName.Text = "user";
            textBoxPassword.Text = "password";
            labelArrowTutorial.Visible = false;
            labelGameOver.Visible = false;
            labelGOScore.Visible = false;
            labelGOScoreNumber.Visible = false;
            labelGOReplay.Visible = false;
            labelGOMainMenu.Visible = false;
            labelGOXP.Visible = false;
            labelGOXPNumber.Visible = false;
            labelGOXPNumberMultiplier.Visible = false;
            labelReqXP.Visible = false;
            labelLevel.Visible = false;
            labelDifficultyEasy.Visible = false;
            labelDifficultyMedium.Visible = false;
            labelDifficultyHard.Visible = false;
            labelDifficultyExtreme.Visible = false;
            labelEasyMP.Visible = false;
            labelMediumMP.Visible = false;
            labelHardMP.Visible = false;
            labelExtremeMP.Visible = false;
            labelEdgeScrolling.Visible = false;
            labelEdgeScrollingMP.Visible = false;
            labelBack.Visible = false;
            labelBackPalette.Visible = false;
            labelUserName.Visible = false;
            labelPassword.Visible = false;
            textBoxUserName.Visible = false;
            textBoxPassword.Visible = false;
            labelLogIn.Visible = false;
            labelNewAccount.Visible = false;
            labelChoosePalette.Visible = false;
            labelResetProgress.Visible = false;
            labelPalette1.Visible = false;
            labelPalette2.Visible = false;
            labelPalette3.Visible = false;
            labelRPQuestion.Visible = false;
            labelRPYes.Visible = false;
            labelRPNo.Visible = false;
            labelPausedMainMenu.Visible = false;
            labelPauseGP.Visible = false;
            labelPauseRestart.Visible = false;
            labelPauseResume.Visible = false;
            labelPGRestartQ.Visible = false;
            labelPGRestartYes.Visible = false;
            labelPGRestartNo.Visible = false;
            labelPGMainMenuQ.Visible = false;
            labelPGMainMenuYes.Visible = false;
            labelPGMainMenuNo.Visible = false;
            labelMainQuitQ.Visible = false;
            labelMainQuitYes.Visible = false;
            labelMainQuitNo.Visible = false;
            labelHighScoresEasy.Visible = false;
            labelHighScoresMedium.Visible = false;
            labelHighScoresHard.Visible = false;
            labelHighScoresExtreme.Visible = false;
            labelChoosePaletteI.Visible = false;
            labelLevelUpUnlocks.Visible = false;
            labelLoginError.Visible = false;
            labelLogOut.Visible = false;
            labelDeleteAccountQ.Visible = false;
            labelDeleteAccountYes.Visible = false;
            labelDeleteAccountNo.Visible = false;
            labelRemoveAccount.Visible = false;
            labelVerifyIdentityPass.Visible = false;
            textBoxVerifyIdentityPass.Visible = false;
            labelVerifyIdentityQ.Visible = false;
            labelVerifyIdentitySure.Visible = false;
            labelVerifyIdentityYes.Visible = false;
            labelVerifyIdentityNo.Visible = false;
            labelVerifyIdentityIncorrectPass.Visible = false;
            labelHighScoreName.Visible = false;
            labelHighScoreScore.Visible = false;
            labelName1.Visible = false;
            labelName2.Visible = false;
            labelName3.Visible = false;
            labelName4.Visible = false;
            labelName5.Visible = false;
            labelName6.Visible = false;
            labelName7.Visible = false;
            labelName8.Visible = false;
            labelName9.Visible = false;
            labelName10.Visible = false;
            labelScore1.Visible = false;
            labelScore2.Visible = false;
            labelScore3.Visible = false;
            labelScore4.Visible = false;
            labelScore5.Visible = false;
            labelScore6.Visible = false;
            labelScore7.Visible = false;
            labelScore8.Visible = false;
            labelScore9.Visible = false;
            labelScore10.Visible = false;
            labelHSLevel1.Visible = false;
            labelHSLevel2.Visible = false;
            labelHSLevel3.Visible = false;
            labelHSLevel4.Visible = false;
            labelHSLevel5.Visible = false;
            labelHSLevel6.Visible = false;
            labelHSLevel7.Visible = false;
            labelLevelSelectTop.Visible = false;
            pictureBoxLevelSelect1.Visible = false;
            pictureBoxLevelSelect2.Visible = false;
            pictureBoxLevelSelect3.Visible = false;
            labelBackDifficulty.Visible = false;
            labelLSDifficultyTop.Visible = false;
            labelLSDifficultyChange.Visible = false;
            labelLSMultiplierBonusTop.Visible = false;
            labelLSMultiplierBonusChange.Visible = false;
            labelPaletteUnlocked.Visible = false;
            labelLevelUnlocked.Visible = false;
            labelPaletteUnlockedOptions.Visible = false;

            if (loggedIn == true)
                labelOptions.Visible = true;
            else
                labelOptions.Visible = false;

            labelPlay.Visible = true;
            labelHighScores.Visible = true;
            labelExit.Visible = true;

            if (levelUnlocked == true && loggedIn == true)
                labelLevelUnlocked.Visible = true;
            if (paletteUnlocked == true && loggedIn == true)
                labelPaletteUnlocked.Visible = true;

            //picture box-uri
            if (PBLoaded == false)
            {
                PB = new PictureBox[pixelDivider + 5, pixelDivider + 5];
                GB = new char[pixelDivider + 5, pixelDivider + 5];
                tail = new Tail[1100];
                pictureBoxSize = 640 / pixelDivider;  // 640x640 e marimea formului, se imparte la pixelDivider
                for (int i = 1; i <= pixelDivider; ++i)
                    for (int j = 1; j <= pixelDivider; ++j)
                    {
                        PB[i, j] = new PictureBox();
                        PB[i, j].Height = pictureBoxSize;
                        PB[i, j].Width = pictureBoxSize;
                        PB[i, j].Top = (pictureBoxSize) * (i - 1);
                        PB[i, j].Left = (pictureBoxSize) * (j - 1);
                        PB[i, j].BackColor = activeScheme.Primary;
                        PB[i, j].Parent = panel1;
                    }
                PBLoaded = true;
            }

            gameStarted = false;

            timerBlinkXP.Stop();

            removeXPBar();

            tick = 0;
            timerWriting.Start();

            changeColourScheme();
        }

        //--------------MY METHODS--------------\\

        public void changeColourScheme()
        {
            labelArrowTutorial.BackColor = activeScheme.Primary;
            labelPlay.BackColor = activeScheme.Primary;
            labelHighScores.BackColor = activeScheme.Primary;
            labelExit.BackColor = activeScheme.Primary;
            labelDifficultyEasy.BackColor = activeScheme.Primary;
            labelDifficultyMedium.BackColor = activeScheme.Primary;
            labelDifficultyHard.BackColor = activeScheme.Primary;
            labelDifficultyExtreme.BackColor = activeScheme.Primary;
            labelEasyMP.BackColor = activeScheme.Primary;
            labelMediumMP.BackColor = activeScheme.Primary;
            labelHardMP.BackColor = activeScheme.Primary;
            labelExtremeMP.BackColor = activeScheme.Primary;
            labelEdgeScrolling.BackColor = activeScheme.Primary;
            labelEdgeScrollingMP.BackColor = activeScheme.Primary;
            labelGameOver.BackColor = activeScheme.Primary;
            labelGOMainMenu.BackColor = activeScheme.Primary;
            labelGOReplay.BackColor = activeScheme.Primary;
            labelGOScore.BackColor = activeScheme.Primary;
            labelGOScoreNumber.BackColor = activeScheme.Primary;
            labelGOXP.BackColor = activeScheme.Primary;
            labelGOXPNumber.BackColor = activeScheme.Primary;
            labelGOXPNumberMultiplier.BackColor = activeScheme.Primary;
            labelLevel.BackColor = activeScheme.Primary;
            labelReqXP.BackColor = activeScheme.Primary;
            labelBack.BackColor = activeScheme.Primary;
            labelOptions.BackColor = activeScheme.Primary;
            labelUserName.BackColor = activeScheme.Primary;
            textBoxUserName.BackColor = activeScheme.Primary;
            labelPassword.BackColor = activeScheme.Primary;
            textBoxPassword.BackColor = activeScheme.Primary;
            labelLogIn.BackColor = activeScheme.Primary;
            labelNewAccount.BackColor = activeScheme.Primary;
            labelChoosePalette.BackColor = activeScheme.Primary;
            labelResetProgress.BackColor = activeScheme.Primary;
            labelBackPalette.BackColor = activeScheme.Primary;
            labelPalette1.BackColor = activeScheme.Primary;
            labelPalette2.BackColor = activeScheme.Primary;
            labelPalette3.BackColor = activeScheme.Primary;
            labelPausedMainMenu.BackColor = activeScheme.Primary;
            labelPauseGP.BackColor = activeScheme.Primary;
            labelPauseRestart.BackColor = activeScheme.Primary;
            labelPauseResume.BackColor = activeScheme.Primary;
            labelPGRestartQ.BackColor = activeScheme.Primary;
            labelPGRestartYes.BackColor = activeScheme.Primary;
            labelPGRestartNo.BackColor = activeScheme.Primary;
            labelPGMainMenuQ.BackColor = activeScheme.Primary;
            labelPGMainMenuYes.BackColor = activeScheme.Primary;
            labelPGMainMenuNo.BackColor = activeScheme.Primary;
            labelMainQuitQ.BackColor = activeScheme.Primary;
            labelMainQuitYes.BackColor = activeScheme.Primary;
            labelMainQuitNo.BackColor = activeScheme.Primary;
            labelHighScoresEasy.BackColor = activeScheme.Primary;
            labelHighScoresMedium.BackColor = activeScheme.Primary;
            labelHighScoresHard.BackColor = activeScheme.Primary;
            labelHighScoresExtreme.BackColor = activeScheme.Primary;
            labelChoosePaletteI.BackColor = activeScheme.Primary;
            labelLevelUpUnlocks.BackColor = activeScheme.Primary;
            labelLoginError.BackColor = activeScheme.Primary;
            labelLogOut.BackColor = activeScheme.Primary;
            labelRemoveAccount.BackColor = activeScheme.Primary;
            labelDeleteAccountQ.BackColor = activeScheme.Primary;
            labelDeleteAccountYes.BackColor = activeScheme.Primary;
            labelDeleteAccountNo.BackColor = activeScheme.Primary;
            labelVerifyIdentityPass.BackColor = activeScheme.Primary;
            textBoxVerifyIdentityPass.BackColor = activeScheme.Primary;
            labelVerifyIdentityQ.BackColor = activeScheme.Primary;
            labelVerifyIdentitySure.BackColor = activeScheme.Primary;
            labelVerifyIdentityYes.BackColor = activeScheme.Primary;
            labelVerifyIdentityNo.BackColor = activeScheme.Primary;
            labelVerifyIdentityIncorrectPass.BackColor = activeScheme.Primary;
            labelRPQuestion.BackColor = activeScheme.Primary;
            labelRPYes.BackColor = activeScheme.Primary;
            labelRPNo.BackColor = activeScheme.Primary;
            labelHighScoreName.BackColor = activeScheme.Primary;
            labelHighScoreScore.BackColor = activeScheme.Primary;
            labelName1.BackColor = activeScheme.Primary;
            labelName2.BackColor = activeScheme.Primary;
            labelName3.BackColor = activeScheme.Primary;
            labelName4.BackColor = activeScheme.Primary;
            labelName5.BackColor = activeScheme.Primary;
            labelName6.BackColor = activeScheme.Primary;
            labelName7.BackColor = activeScheme.Primary;
            labelName8.BackColor = activeScheme.Primary;
            labelName9.BackColor = activeScheme.Primary;
            labelName10.BackColor = activeScheme.Primary;
            labelScore1.BackColor = activeScheme.Primary;
            labelScore2.BackColor = activeScheme.Primary;
            labelScore3.BackColor = activeScheme.Primary;
            labelScore4.BackColor = activeScheme.Primary;
            labelScore5.BackColor = activeScheme.Primary;
            labelScore6.BackColor = activeScheme.Primary;
            labelScore7.BackColor = activeScheme.Primary;
            labelScore8.BackColor = activeScheme.Primary;
            labelScore9.BackColor = activeScheme.Primary;
            labelScore10.BackColor = activeScheme.Primary;
            labelLevelSelectTop.BackColor = activeScheme.Primary;
            pictureBoxLevelSelect1.BackColor = activeScheme.Tertiary;
            pictureBoxLevelSelect2.BackColor = activeScheme.Tertiary;
            pictureBoxLevelSelect3.BackColor = activeScheme.Tertiary;
            labelBackDifficulty.BackColor = activeScheme.Primary;
            labelLSDifficultyTop.BackColor = activeScheme.Primary;
            labelLSDifficultyChange.BackColor = activeScheme.Primary;
            labelLSMultiplierBonusTop.BackColor = activeScheme.Primary;
            labelLSMultiplierBonusChange.BackColor = activeScheme.Primary;
            labelHSLevel1.BackColor = activeScheme.Primary;
            labelHSLevel2.BackColor = activeScheme.Primary;
            labelHSLevel3.BackColor = activeScheme.Primary;
            labelHSLevel4.BackColor = activeScheme.Primary;
            labelHSLevel5.BackColor = activeScheme.Primary;
            labelHSLevel6.BackColor = activeScheme.Primary;
            labelHSLevel7.BackColor = activeScheme.Primary;
            labelLevelUnlocked.BackColor = activeScheme.Primary;
            labelPaletteUnlocked.BackColor = activeScheme.Primary;
            labelPaletteUnlockedOptions.BackColor = activeScheme.Primary;

            labelPlay.ForeColor = activeScheme.Secondary;
            labelHighScores.ForeColor = activeScheme.Secondary;
            labelExit.ForeColor = activeScheme.Secondary;
            labelArrowTutorial.ForeColor = activeScheme.Secondary;
            labelDifficultyEasy.ForeColor = activeScheme.Secondary;
            labelDifficultyMedium.ForeColor = activeScheme.Secondary;
            labelDifficultyHard.ForeColor = activeScheme.Secondary;
            labelDifficultyExtreme.ForeColor = activeScheme.Secondary;
            labelEdgeScrolling.ForeColor = activeScheme.Secondary;
            labelGameOver.ForeColor = activeScheme.Secondary;
            labelGOMainMenu.ForeColor = activeScheme.Secondary;
            labelGOReplay.ForeColor = activeScheme.Secondary;
            labelGOScore.ForeColor = activeScheme.Secondary;
            labelGOScoreNumber.ForeColor = activeScheme.Secondary;
            labelGOXP.ForeColor = activeScheme.Secondary;
            labelGOXPNumber.ForeColor = activeScheme.Secondary;
            labelLevel.ForeColor = activeScheme.Secondary;
            labelReqXP.ForeColor = activeScheme.Secondary;
            labelBack.ForeColor = activeScheme.Secondary;
            labelOptions.ForeColor = activeScheme.Secondary;
            labelUserName.ForeColor = activeScheme.Secondary;
            labelPassword.ForeColor = activeScheme.Secondary;
            labelLogIn.ForeColor = activeScheme.Secondary;
            labelNewAccount.ForeColor = activeScheme.Secondary;
            labelChoosePalette.ForeColor = activeScheme.Secondary;
            labelResetProgress.ForeColor = activeScheme.Secondary;
            labelPalette1.ForeColor = activeScheme.Secondary;
            labelPalette3.ForeColor = activeScheme.Secondary;
            labelBackPalette.ForeColor = activeScheme.Secondary;
            labelPausedMainMenu.ForeColor = activeScheme.Secondary;
            labelPauseGP.ForeColor = activeScheme.Secondary;
            labelPauseRestart.ForeColor = activeScheme.Secondary;
            labelPauseResume.ForeColor = activeScheme.Secondary;
            labelPGRestartQ.ForeColor = activeScheme.Secondary;
            labelPGRestartYes.ForeColor = activeScheme.Secondary;
            labelPGRestartNo.ForeColor = activeScheme.Secondary;
            labelPGMainMenuQ.ForeColor = activeScheme.Secondary;
            labelPGMainMenuYes.ForeColor = activeScheme.Secondary;
            labelPGMainMenuNo.ForeColor = activeScheme.Secondary;
            labelMainQuitQ.ForeColor = activeScheme.Secondary;
            labelMainQuitYes.ForeColor = activeScheme.Secondary;
            labelMainQuitNo.ForeColor = activeScheme.Secondary;
            labelHighScoresEasy.ForeColor = activeScheme.Secondary;
            labelHighScoresMedium.ForeColor = activeScheme.Secondary;
            labelHighScoresHard.ForeColor = activeScheme.Secondary;
            labelHighScoresExtreme.ForeColor = activeScheme.Secondary;
            labelChoosePaletteI.ForeColor = activeScheme.Secondary;
            labelLogOut.ForeColor = activeScheme.Secondary;
            labelRemoveAccount.ForeColor = activeScheme.Secondary;
            labelDeleteAccountQ.ForeColor = activeScheme.Secondary;
            labelDeleteAccountYes.ForeColor = activeScheme.Secondary;
            labelDeleteAccountNo.ForeColor = activeScheme.Secondary;
            labelVerifyIdentityPass.ForeColor = activeScheme.Secondary;
            labelVerifyIdentityQ.ForeColor = activeScheme.Secondary;
            labelVerifyIdentitySure.ForeColor = activeScheme.Secondary;
            labelVerifyIdentityYes.ForeColor = activeScheme.Secondary;
            labelVerifyIdentityNo.ForeColor = activeScheme.Secondary;
            labelRPQuestion.ForeColor = activeScheme.Secondary;
            labelRPYes.ForeColor = activeScheme.Secondary;
            labelRPNo.ForeColor = activeScheme.Secondary;
            labelHighScoreName.ForeColor = activeScheme.Secondary;
            labelHighScoreScore.ForeColor = activeScheme.Secondary;
            labelName1.ForeColor = activeScheme.Secondary;
            labelName2.ForeColor = activeScheme.Secondary;
            labelName3.ForeColor = activeScheme.Secondary;
            labelName4.ForeColor = activeScheme.Secondary;
            labelName5.ForeColor = activeScheme.Secondary;
            labelName6.ForeColor = activeScheme.Secondary;
            labelName7.ForeColor = activeScheme.Secondary;
            labelName8.ForeColor = activeScheme.Secondary;
            labelName9.ForeColor = activeScheme.Secondary;
            labelName10.ForeColor = activeScheme.Secondary;
            labelScore1.ForeColor = activeScheme.Secondary;
            labelScore2.ForeColor = activeScheme.Secondary;
            labelScore3.ForeColor = activeScheme.Secondary;
            labelScore4.ForeColor = activeScheme.Secondary;
            labelScore5.ForeColor = activeScheme.Secondary;
            labelScore6.ForeColor = activeScheme.Secondary;
            labelScore7.ForeColor = activeScheme.Secondary;
            labelScore8.ForeColor = activeScheme.Secondary;
            labelScore9.ForeColor = activeScheme.Secondary;
            labelScore10.ForeColor = activeScheme.Secondary;
            labelLevelSelectTop.ForeColor = activeScheme.Secondary;
            pictureBoxLevelSelect1.ForeColor = activeScheme.Secondary;
            pictureBoxLevelSelect2.ForeColor = activeScheme.Secondary;
            pictureBoxLevelSelect3.ForeColor = activeScheme.Secondary;
            labelBackDifficulty.ForeColor = activeScheme.Secondary;
            labelLSDifficultyTop.ForeColor = activeScheme.Secondary;
            labelLSMultiplierBonusTop.ForeColor = activeScheme.Secondary;
            labelHSLevel1.ForeColor = activeScheme.Secondary;
            labelHSLevel2.ForeColor = activeScheme.Secondary;
            labelHSLevel3.ForeColor = activeScheme.Secondary;
            labelHSLevel4.ForeColor = activeScheme.Secondary;
            labelHSLevel5.ForeColor = activeScheme.Secondary;
            labelHSLevel6.ForeColor = activeScheme.Secondary;
            labelHSLevel7.ForeColor = activeScheme.Secondary;

            labelEasyMP.ForeColor = activeScheme.Tertiary;
            labelMediumMP.ForeColor = activeScheme.Tertiary;
            labelHardMP.ForeColor = activeScheme.Tertiary;
            labelExtremeMP.ForeColor = activeScheme.Tertiary;
            labelEdgeScrollingMP.ForeColor = activeScheme.Tertiary;
            labelReqXP.ForeColor = activeScheme.Tertiary;
            labelGOXPNumberMultiplier.ForeColor = activeScheme.Tertiary;
            labelPalette2.ForeColor = activeScheme.Tertiary;
            labelLevelUpUnlocks.ForeColor = activeScheme.Tertiary;
            labelLoginError.ForeColor = activeScheme.Tertiary;
            textBoxPassword.ForeColor = activeScheme.Tertiary;
            textBoxUserName.ForeColor = activeScheme.Tertiary;
            textBoxVerifyIdentityPass.ForeColor = activeScheme.Tertiary;
            labelVerifyIdentityIncorrectPass.ForeColor = activeScheme.Tertiary;
            labelScore1.ForeColor = activeScheme.Tertiary;
            labelScore2.ForeColor = activeScheme.Tertiary;
            labelScore3.ForeColor = activeScheme.Tertiary;
            labelScore4.ForeColor = activeScheme.Tertiary;
            labelScore5.ForeColor = activeScheme.Tertiary;
            labelScore6.ForeColor = activeScheme.Tertiary;
            labelScore7.ForeColor = activeScheme.Tertiary;
            labelScore8.ForeColor = activeScheme.Tertiary;
            labelScore9.ForeColor = activeScheme.Tertiary;
            labelScore10.ForeColor = activeScheme.Tertiary;
            labelLSDifficultyChange.ForeColor = activeScheme.Tertiary;
            labelLSMultiplierBonusChange.ForeColor = activeScheme.Tertiary;
            labelLevelUnlocked.ForeColor = activeScheme.Tertiary;
            labelPaletteUnlocked.ForeColor = activeScheme.Tertiary;
            labelPaletteUnlockedOptions.ForeColor = activeScheme.Tertiary;

            for (int i = 1; i <= pixelDivider; ++i)
                for (int j = 1; j <= pixelDivider; ++j)
                    PB[i, j].BackColor = activeScheme.Primary;
        }

        private void newPickup(bool isFirst)
        {
            bool placed = false;
            bool ok = true;

            if (isFirst == false)
            {
                while (placed == false)
                {
                    ok = true;
                    randomX = r.Next(1, pixelDivider);
                    randomY = r.Next(1, pixelDivider);

                    for (int i = 1; i <= snakeLength; ++i)
                        if (randomX == tail[i].xPos && randomY == tail[i].yPos)
                            ok = false;

                    if (GB[randomX, randomY] == '#')
                        ok = false;

                    if (ok == true)
                    {
                        pickupLocation.xPos = randomX;
                        pickupLocation.yPos = randomY;
                        PB[randomX, randomY].BackColor = activeScheme.Tertiary;
                        placed = true;
                        timerPickupBlink.Start();
                    }
                }
            }
            else
            {
                while (placed == false)
                {
                    ok = true;
                    randomX = r.Next(5, pixelDivider);
                    randomY = r.Next(5, pixelDivider);

                    for (int i = 1; i <= snakeLength; ++i)
                        if (randomX == tail[i].xPos && randomY == tail[i].yPos)
                            ok = false;

                    if (GB[randomX, randomY] == '#')
                        ok = false;

                    if (ok == true)
                    {
                        pickupLocation.xPos = randomX;
                        pickupLocation.yPos = randomY;
                        PB[randomX, randomY].BackColor = activeScheme.Tertiary;
                        placed = true;
                        timerPickupBlink.Start();
                    }
                }
            }
        }

        private void gameOver()
        {
            timerPickupBlink.Stop();

            //facem tot primary
            for (int i = 1; i <= pixelDivider; ++i)
                for (int j = 1; j <= pixelDivider; ++j)
                    PB[i, j].BackColor = activeScheme.Primary;

            currentXP = gameScore * difficultyMultiplier;

            gameStarted = false;

            //SALVAM HIGH SCORE-UL SI PROGRESUL
            dbHandler.saveHighScore(currentUser, currentDifficulty, gameScore, selectedGameLevel);
            dbHandler.saveProgress(currentUser);

            //pornim timerul de game over
            timerGameOver.Start();
            GOTick = 0;
        }

        private void updateTail()
        {
            bool needPickup = false;
            aux = tail[1]; // retinem capul intr-un aux, le interschimbam dupa
            switch (direction) // schimbam directia capului
            {
                case 1:
                    {
                        tail[1].xPos--;
                        break;
                    }
                case 2:
                    {
                        tail[1].yPos++;
                        break;
                    }
                case 3:
                    {
                        tail[1].xPos++;
                        break;
                    }
                case 4:
                    {
                        tail[1].yPos--;
                        break;
                    }
            }

            //perform checks
            if ((edgeScrollingAllowed == false && (tail[1].xPos < 1 || tail[1].xPos > pixelDivider || tail[1].yPos < 1 || tail[1].yPos > pixelDivider)) || (GB[tail[1].xPos, tail[1].yPos] == '#'))
            {
                timerGameSpeed.Stop();
                //facem tot primary
                for (int i = 1; i <= pixelDivider; ++i)
                    for (int j = 1; j <= pixelDivider; ++j)
                        PB[i, j].BackColor = activeScheme.Primary;
                            
                gameStopped = true;
                gameOver();
            }
            else
                if(tail[1].xPos < 1)
                {
                    tail[1].xPos = pixelDivider;
                }
                else
                    if(tail[1].xPos > pixelDivider)
                    {
                        tail[1].xPos = 1;
                    }
                    else
                        if(tail[1].yPos < 1)
                        {
                            tail[1].yPos = pixelDivider;
                        }
                        else
                            if(tail[1].yPos > pixelDivider)
                            {
                                tail[1].yPos = 1;
                            }

            //verificam daca am ajuns la pickup
            if (pickupLocation.xPos == tail[1].xPos && pickupLocation.yPos == tail[1].yPos)
            {
                needPickup = true;
            }

            if (gameStopped == false)
            {
                // mutam toata coada catre cap
                for (int i = 2; i <= snakeLength; ++i)
                {
                    aux2 = tail[i];
                    PB[tail[i].xPos, tail[i].yPos].BackColor = activeScheme.Primary;
                    tail[i] = aux;
                    aux = aux2;
                    if (tail[1].xPos == tail[i].xPos && tail[1].yPos == tail[i].yPos) // verificam daca am intrat in coada
                    {
                        timerGameSpeed.Stop();
                        gameStopped = true;
                        gameOver();
                    }
                }

                if (gameStopped == false)
                {
                    // recoloram sarpele
                    for (int i = 1; i <= snakeLength; ++i)
                        if (!(tail[i].xPos < 1 || tail[i].xPos > pixelDivider || tail[i].yPos < 1 || tail[i].yPos > pixelDivider))
                            PB[tail[i].xPos, tail[i].yPos].BackColor = activeScheme.Secondary;

                    if (needPickup == true)
                    {
                        timerPickupBlink.Stop();
                        addNewTailPieceCheck(tail[snakeLength], tail[snakeLength - 1]);
                        newPickup(false);
                        gameScore++;
                    }
                }
            }
        }

        private void checkQueueAdd()
        {
            addNewTailPieceCheck(tail[snakeLength], tail[snakeLength - 1]);
            queueLength--;
        }

        private bool checkIfPossible(int dirX, int dirY)
        {
            for (int i = 1; i < snakeLength; ++i)
                if ((tail[i].xPos == dirX && tail[i].yPos == dirY) || (GB[tail[i].xPos, tail[i].yPos] == '#'))
                    return false;

            return true;
        }

        private void addToQueue()
        {
            queueLength++;
        }

        private void addNewTailPiece(int dir)
        {
            snakeLength++;
            tail[snakeLength].xPos = tail[snakeLength - 1].xPos;
            tail[snakeLength].yPos = tail[snakeLength - 1].yPos;
            
            switch(dir)
            {
                case 1:
                    {
                        tail[snakeLength].xPos--;
                        break;
                    }
                case 2:
                    {
                        tail[snakeLength].yPos++;
                        break;
                    }
                case 3:
                    {
                        tail[snakeLength].xPos++;
                        break;
                    }
                case 4:
                    {
                        tail[snakeLength].yPos--;
                        break;
                    }
            }

            PB[tail[snakeLength].xPos, tail[snakeLength].yPos].BackColor = activeScheme.Secondary;
        }

        private void addNewTailPieceCheck(Tail last, Tail secondLast)
        {
            if (last.xPos - 1 == secondLast.xPos)
            {
                if (last.xPos + 1 < 1 || last.xPos + 1 > pixelDivider || last.yPos < 1 || last.yPos > pixelDivider)
                    addToQueue();
                else
                    if (checkIfPossible(last.xPos + 1, last.yPos) == false)
                        addToQueue();
                    else
                        addNewTailPiece(1);
            }
            else
                if (last.xPos + 1 == secondLast.xPos)
                {
                    if (last.xPos - 1 < 1 || last.xPos - 1 > pixelDivider || last.yPos < 1 || last.yPos > pixelDivider)
                        addToQueue();
                    else
                        if (checkIfPossible(last.xPos - 1, last.yPos) == false)
                            addToQueue();
                        else
                            addNewTailPiece(3);
                }
            else
                if (last.yPos - 1 == secondLast.yPos)
                {
                    if (last.xPos < 1 || last.xPos > pixelDivider || last.yPos + 1 < 1 || last.yPos + 1 > pixelDivider)
                        addToQueue();
                    else
                        if (checkIfPossible(last.xPos, last.yPos + 1) == false)
                            addToQueue();
                        else
                            addNewTailPiece(4);
                }
            else
                if (last.yPos + 1 == secondLast.yPos)
                {
                    if (last.xPos < 1 || last.xPos > pixelDivider || last.yPos - 1 < 1 || last.yPos - 1 > pixelDivider)
                        addToQueue();
                    else
                        if (checkIfPossible(last.xPos, last.yPos - 1) == false)
                            addToQueue();
                        else
                            addNewTailPiece(2);
            }
        }

        private void showXPBar()
        {
            //req xp
            levelRequiredXP = xps.requiredXP(level);

            int previousProgress, currentProgress; // 27 = max

            previousProgress = ((levelXP % levelRequiredXP) * 27 / levelRequiredXP); // regula de 3 simpla
            currentProgress = currentXP * 27 / levelRequiredXP;

            while (levelXP + currentXP >= levelRequiredXP)
            {
                level++;
                previousProgress = 0;
                currentXP -= (levelRequiredXP - levelXP);
                levelXP = 0;
                currentProgress = currentXP * 27 / levelRequiredXP;
                leveledUp = true;
                levelRequiredXP = xps.requiredXP(level);
                paletteUnlocked = true;
                if (level == 3 || level == 7 || level == 11 || level == 14 || level == 18 || level == 21)
                    levelUnlocked = true;
            }

            for (int i = 6; i < previousProgress + 4 && i <= 27; ++i)
                PB[19, i].BackColor = activeScheme.Secondary;

            start = previousProgress + 4;
            end = currentProgress + start;

            if (start < 6)
                start = 6;

            levelXP += currentXP;

            timerBlinkXP.Start();
        }

        private void modifyDifficulty(int DIFF)
        {
            if(DIFF == 1) // easy
            {
                currentDifficulty = 1;
                difficultyMultiplier = 1;
                gameSpeed = 250;
            }

            if(DIFF == 2) // medium
            {
                currentDifficulty = 2;
                difficultyMultiplier = 2;
                gameSpeed = 150;
            }

            if(DIFF == 3) // hard
            {
                currentDifficulty = 3;
                difficultyMultiplier = 3;
                gameSpeed = 50;
            }

            if(DIFF == 4) // extreme
            {
                currentDifficulty = 4;
                difficultyMultiplier = 5;
                gameSpeed = 30;
            }


            if (edgeScrollingAllowed == false || needsEdgeScrolling == true)
                difficultyMultiplier *= 2;

            difficultyMultiplier += levelMultiplier;
        }

        private void placeLevel(int level)
        {
            //citire din fisier
            StreamReader fin = new StreamReader("levels/level" + level + ".txt");
            for (int i = 1; i <= pixelDivider; ++i)
            {
                string linie = fin.ReadLine();
                for (int j = 1; j <= pixelDivider; ++j)
                {
                    GB[i, j] = Convert.ToChar(linie[j - 1]);
                }
            }

            //facem elementele din fisier culoarea level
            for(int i = 1; i <= pixelDivider; ++i)
                for(int j = 1; j <= pixelDivider; ++j)
                {
                    if (GB[i, j] == '#')
                        PB[i, j].BackColor = activeScheme.Level;
                }
        }

        private void startGame(int DIFF)
        {
            labelDifficultyEasy.Visible = false;
            labelDifficultyMedium.Visible = false;
            labelDifficultyHard.Visible = false;
            labelDifficultyExtreme.Visible = false;
            labelEasyMP.Visible = false;
            labelMediumMP.Visible = false;
            labelHardMP.Visible = false;
            labelExtremeMP.Visible = false;
            labelEdgeScrolling.Visible = false;
            labelEdgeScrollingMP.Visible = false;
            labelBack.Visible = false;
            labelBackPalette.Visible = false;
            labelBackDifficulty.Visible = false;

            labelArrowTutorial.Visible = true;
            timerBlinkRate.Start();

            modifyDifficulty(DIFF);

            removeTitle();
            timerBlinkXP.Stop();
            removeXPBar();

            //resetam totul la default
            direction = 0;
            newDirection = 0;
            gameScore = 0;
            tick = 0;
            levelTick = 0;
            snakeLength = 1;
            queueLength = 0;
            gameStopped = false;

            //plasam elementele din nivel
            placeLevel(selectedGameLevel);

            getSpawnLocation(selectedGameLevel);

            PB[randomXSpawn, randomYSpawn].BackColor = activeScheme.Secondary;
            tail[1].xPos = randomXSpawn;
            tail[1].yPos = randomYSpawn;
            addNewTailPiece(2);
            PB[tail[snakeLength].xPos, tail[snakeLength].yPos].BackColor = activeScheme.Secondary;
            addNewTailPiece(2);
            PB[tail[snakeLength].xPos, tail[snakeLength].yPos].BackColor = activeScheme.Secondary;
            
            //plasam primul pickup
            newPickup(true);

            // timer pentru dificultate
            timerGameSpeed.Interval = gameSpeed;
            timerGameSpeed.Start();
            gameStarted = true;
        }

        private void removeTitle()
        {
            for (int i = 1; i <= 10; ++i)
                for (int j = 1; j <= pixelDivider; ++j)
                    PB[i, j].BackColor = activeScheme.Primary;
        }

        private void removeXPBar()
        {
            for (int i = 1; i <= pixelDivider; ++i)
                PB[19, i].BackColor = activeScheme.Primary;
        }

        private void showPauseMenu()
        {
            timerGameSpeed.Stop();
            timerPickupBlink.Stop();
            if(direction == 0)
            {
                timerBlinkRate.Stop();
                labelArrowTutorial.Visible = false;
            }

            //ascundem sarpele si pickup-ul
            for (int i = 1; i <= pixelDivider; ++i)
                for (int j = 1; j <= pixelDivider; ++j)
                    PB[i, j].BackColor = activeScheme.Primary;

            gamePaused = true;

            labelPausedMainMenu.Visible = true;
            labelPauseGP.Visible = true;
            labelPauseRestart.Visible = true;
            labelPauseResume.Visible = true;
        }

        private void hidePauseMenu()
        {
            if (direction == 0)
            {
                timerBlinkRate.Start();
                labelArrowTutorial.Visible = true;
            }

            timerPickupBlink.Start();

            //recoloram sarpele si pickup-ul
            for (int i = 2; i <= snakeLength; ++i)
                PB[tail[i].xPos, tail[i].yPos].BackColor = activeScheme.Secondary;

            PB[pickupLocation.xPos, pickupLocation.yPos].BackColor = activeScheme.Tertiary;

            for (int i = 1; i <= pixelDivider; ++i)
                for (int j = 1; j <= pixelDivider; ++j)
                    if (GB[i, j] == '#')
                        PB[i, j].BackColor = activeScheme.Level;

            gamePaused = false;

            labelPausedMainMenu.Visible = false;
            labelPauseGP.Visible = false;
            labelPauseRestart.Visible = false;
            labelPauseResume.Visible = false;
            labelPGRestartQ.Visible = false;
            labelPGRestartYes.Visible = false;
            labelPGRestartNo.Visible = false;
            labelPGMainMenuNo.Visible = false;
            labelPGMainMenuYes.Visible = false;
            labelPGMainMenuQ.Visible = false;

            timerGameSpeed.Start();
        }

        private void showHighScores()
        {
            labelHighScoreName.Visible = true;
            labelHighScoreScore.Visible = true;

            for(int i = 1; i < 10; ++i)
                for(int j = i + 1; j <= 10; ++j)
                    if(hsTable[i].score < hsTable[j].score)
                    {
                        HighScore aux = new HighScore();
                        aux = hsTable[i];
                        hsTable[i] = hsTable[j];
                        hsTable[j] = aux;
                    }

            labelName1.Text = "1. " + hsTable[1].name;
            labelScore1.Text = hsTable[1].score.ToString();
            labelName1.Visible = true;
            labelScore1.Visible = true;

            labelName2.Text = "2. " + hsTable[2].name;
            labelScore2.Text = hsTable[2].score.ToString();
            labelName2.Visible = true;
            labelScore2.Visible = true;

            labelName3.Text = "3. " + hsTable[3].name;
            labelScore3.Text = hsTable[3].score.ToString();
            labelName3.Visible = true;
            labelScore3.Visible = true;

            labelName4.Text = "4. " + hsTable[4].name;
            labelScore4.Text = hsTable[4].score.ToString();
            labelName4.Visible = true;
            labelScore4.Visible = true;

            labelName5.Text = "5. " + hsTable[5].name;
            labelScore5.Text = hsTable[5].score.ToString();
            labelName5.Visible = true;
            labelScore5.Visible = true;

            labelName6.Text = "6. " + hsTable[6].name;
            labelScore6.Text = hsTable[6].score.ToString();
            labelName6.Visible = true;
            labelScore6.Visible = true;

            labelName7.Text = "7. " + hsTable[7].name;
            labelScore7.Text = hsTable[7].score.ToString();
            labelName7.Visible = true;
            labelScore7.Visible = true;

            labelName8.Text = "8. " + hsTable[8].name;
            labelScore8.Text = hsTable[8].score.ToString();
            labelName8.Visible = true;
            labelScore8.Visible = true;

            labelName9.Text = "9. " + hsTable[9].name;
            labelScore9.Text = hsTable[9].score.ToString();
            labelName9.Visible = true;
            labelScore9.Visible = true;

            labelName10.Text = "10. " + hsTable[10].name;
            labelScore10.Text = hsTable[10].score.ToString();
            labelName10.Visible = true;
            labelScore10.Visible = true;
        }

        private void getSpawnLocation(int level)
        {
            if(level == 1)
            {
                randomXSpawn = r.Next(4, 28);
                randomYSpawn = r.Next(4, 28);
            }
            
            if(level == 2)
            {
                randomXSpawn = r.Next(5, 27);
                randomYSpawn = r.Next(5, 27);
            }

            if(level == 3)
            {
                int k = r.Next(1, 2);
                switch(k)
                {
                    case 1:
                        {
                            randomXSpawn = 16;
                            randomYSpawn = 8;
                            break;
                        }
                    case 2:
                        {
                            randomXSpawn = 16;
                            randomYSpawn = 24;
                            break;
                        }
                }
            }

            if(level == 4)
            {
                int k = r.Next(1, 2);
                switch (k)
                {
                    case 1:
                        {
                            randomXSpawn = 16;
                            randomYSpawn = 8;
                            break;
                        }
                    case 2:
                        {
                            randomXSpawn = 16;
                            randomYSpawn = 24;
                            break;
                        }
                }
            }

            if(level == 5)
            {
                int k = r.Next(1, 4);
                switch (k)
                {
                    case 1:
                        {
                            randomXSpawn = 8;
                            randomYSpawn = 8;
                            break;
                        }
                    case 2:
                        {
                            randomXSpawn = 8;
                            randomYSpawn = 24;
                            break;
                        }
                    case 3:
                        {
                            randomXSpawn = 24;
                            randomYSpawn = 8;
                            break;
                        }
                    case 4:
                        {
                            randomXSpawn = 24;
                            randomYSpawn = 24;
                            break;
                        }
                }
            }

            if(level == 6)
            {
                randomXSpawn = r.Next(3, 29);
                while(randomXSpawn % 2 != 0)
                    randomXSpawn = r.Next(3, 29);

                randomYSpawn = r.Next(3, 28);
            }

            if(level == 7)
            {
                int k = r.Next(1, 5);
                switch (k)
                {
                    case 1:
                        {
                            randomXSpawn = 8;
                            randomYSpawn = 8;
                            break;
                        }
                    case 2:
                        {
                            randomXSpawn = 8;
                            randomYSpawn = 24;
                            break;
                        }
                    case 3:
                        {
                            randomXSpawn = 24;
                            randomYSpawn = 8;
                            break;
                        }
                    case 4:
                        {
                            randomXSpawn = 24;
                            randomYSpawn = 24;
                            break;
                        }
                    case 5:
                        {
                            randomXSpawn = 16;
                            randomYSpawn = 15;
                            break;
                        }
                }
            }
        }

        //--------------KEY DOWN EVENT--------------\\

        private void Snake_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    {
                        if (direction != 3)
                            newDirection = 1;
                        break;
                    }
                case Keys.Right:
                    {
                        if (direction != 4)
                            newDirection = 2;
                        if (direction == 0)
                        {
                            Tail aux = tail[1];
                            tail[1] = tail[3];
                            tail[3] = aux;
                        }
                        break;
                    }
                case Keys.Down:
                    {
                        if (direction != 1)
                            newDirection = 3;
                        break;
                    }
                case Keys.Left:
                    {
                        if (direction != 2)
                            newDirection = 4;
                        break;
                    }
                case Keys.Escape:
                    {
                        if (gameStarted == true)
                        {
                            if (gamePaused == true)
                                hidePauseMenu();
                            else
                                showPauseMenu();
                        }
                        break;
                    }
            }
        }

        //--------------CLICK EVENTS--------------\\

        private void buttonExit_Click(object sender, EventArgs e)
        {
            timerWriting.Stop();
            removeTitle();

            labelPlay.Visible = false;
            labelOptions.Visible = false;
            labelHighScores.Visible = false;
            labelExit.Visible = false;
            labelPaletteUnlocked.Visible = false;
            labelLevelUnlocked.Visible = false;

            labelMainQuitQ.Visible = true;
            labelMainQuitYes.Visible = true;
            labelMainQuitNo.Visible = true;
        }

        private void labelMainQuitYes_Click(object sender, EventArgs e)
        {
            if(loggedIn == true)
            {
                dbHandler.saveProgress(currentUser);
            }

            this.Close();
        }

        private void labelMainQuitNo_Click(object sender, EventArgs e)
        {
            Snake_Load(sender, e);
        }

        private void buttonHighScores_Click(object sender, EventArgs e)
        {
            timerWriting.Stop();
            removeTitle();

            labelPlay.Visible = false;
            labelHighScores.Visible = false;
            labelOptions.Visible = false;
            labelExit.Visible = false;
            labelPaletteUnlocked.Visible = false;
            labelLevelUnlocked.Visible = false;

            labelHSLevel1.Visible = true;
            labelHSLevel2.Visible = true;
            labelHSLevel3.Visible = true;
            labelHSLevel4.Visible = true;
            labelHSLevel5.Visible = true;
            labelHSLevel6.Visible = true;
            labelHSLevel7.Visible = true;

            labelHighScoresEasy.Visible = true;
            labelHighScoresMedium.Visible = true;
            labelHighScoresHard.Visible = true;
            labelHighScoresExtreme.Visible = true;
            labelBack.Visible = true;
            selectedHSMenu = 1;
            labelHighScoresEasy.ForeColor = activeScheme.Tertiary;
            labelHSLevel1.ForeColor = activeScheme.Tertiary;

            labelHighScoresEasy_Click(sender, e);
        }

        private void labelHSLevel1_Click(object sender, EventArgs e)
        {
            selectedHSMenuLevel = 1;

            labelHSLevel1.ForeColor = activeScheme.Tertiary;

            labelHSLevel2.ForeColor = activeScheme.Secondary;
            labelHSLevel3.ForeColor = activeScheme.Secondary;
            labelHSLevel4.ForeColor = activeScheme.Secondary;
            labelHSLevel5.ForeColor = activeScheme.Secondary;
            labelHSLevel6.ForeColor = activeScheme.Secondary;
            labelHSLevel7.ForeColor = activeScheme.Secondary;

            dbHandler.readHighScores(selectedHSMenu, selectedHSMenuLevel);
            showHighScores();
        }

        private void labelHSLevel2_Click(object sender, EventArgs e)
        {
            selectedHSMenuLevel = 2;

            labelHSLevel2.ForeColor = activeScheme.Tertiary;

            labelHSLevel1.ForeColor = activeScheme.Secondary;
            labelHSLevel3.ForeColor = activeScheme.Secondary;
            labelHSLevel4.ForeColor = activeScheme.Secondary;
            labelHSLevel5.ForeColor = activeScheme.Secondary;
            labelHSLevel6.ForeColor = activeScheme.Secondary;
            labelHSLevel7.ForeColor = activeScheme.Secondary;

            dbHandler.readHighScores(selectedHSMenu, selectedHSMenuLevel);
            showHighScores();
        }

        private void labelHSLevel3_Click(object sender, EventArgs e)
        {
            selectedHSMenuLevel = 3;

            labelHSLevel3.ForeColor = activeScheme.Tertiary;

            labelHSLevel1.ForeColor = activeScheme.Secondary;
            labelHSLevel2.ForeColor = activeScheme.Secondary;
            labelHSLevel4.ForeColor = activeScheme.Secondary;
            labelHSLevel5.ForeColor = activeScheme.Secondary;
            labelHSLevel6.ForeColor = activeScheme.Secondary;
            labelHSLevel7.ForeColor = activeScheme.Secondary;

            dbHandler.readHighScores(selectedHSMenu, selectedHSMenuLevel);
            showHighScores();
        }

        private void labelHSLevel4_Click(object sender, EventArgs e)
        {
            selectedHSMenuLevel = 4;

            labelHSLevel4.ForeColor = activeScheme.Tertiary;

            labelHSLevel1.ForeColor = activeScheme.Secondary;
            labelHSLevel2.ForeColor = activeScheme.Secondary;
            labelHSLevel3.ForeColor = activeScheme.Secondary;
            labelHSLevel5.ForeColor = activeScheme.Secondary;
            labelHSLevel6.ForeColor = activeScheme.Secondary;
            labelHSLevel7.ForeColor = activeScheme.Secondary;

            dbHandler.readHighScores(selectedHSMenu, selectedHSMenuLevel);
            showHighScores();
        }

        private void labelHSLevel5_Click(object sender, EventArgs e)
        {
            selectedHSMenuLevel = 5;

            labelHSLevel5.ForeColor = activeScheme.Tertiary;

            labelHSLevel1.ForeColor = activeScheme.Secondary;
            labelHSLevel2.ForeColor = activeScheme.Secondary;
            labelHSLevel3.ForeColor = activeScheme.Secondary;
            labelHSLevel4.ForeColor = activeScheme.Secondary;
            labelHSLevel6.ForeColor = activeScheme.Secondary;
            labelHSLevel7.ForeColor = activeScheme.Secondary;

            dbHandler.readHighScores(selectedHSMenu, selectedHSMenuLevel);
            showHighScores();
        }

        private void labelHSLevel6_Click(object sender, EventArgs e)
        {
            selectedHSMenuLevel = 6;

            labelHSLevel6.ForeColor = activeScheme.Tertiary;

            labelHSLevel1.ForeColor = activeScheme.Secondary;
            labelHSLevel2.ForeColor = activeScheme.Secondary;
            labelHSLevel3.ForeColor = activeScheme.Secondary;
            labelHSLevel4.ForeColor = activeScheme.Secondary;
            labelHSLevel5.ForeColor = activeScheme.Secondary;
            labelHSLevel7.ForeColor = activeScheme.Secondary;

            dbHandler.readHighScores(selectedHSMenu, selectedHSMenuLevel);
            showHighScores();
        }

        private void labelHSLevel7_Click(object sender, EventArgs e)
        {
            selectedHSMenuLevel = 7;

            labelHSLevel7.ForeColor = activeScheme.Tertiary;

            labelHSLevel1.ForeColor = activeScheme.Secondary;
            labelHSLevel2.ForeColor = activeScheme.Secondary;
            labelHSLevel3.ForeColor = activeScheme.Secondary;
            labelHSLevel4.ForeColor = activeScheme.Secondary;
            labelHSLevel5.ForeColor = activeScheme.Secondary;
            labelHSLevel6.ForeColor = activeScheme.Secondary;

            dbHandler.readHighScores(selectedHSMenu, selectedHSMenuLevel);
            showHighScores();
        }

        private void labelHighScoresEasy_Click(object sender, EventArgs e)
        {
            selectedHSMenu = 1;
            labelHighScoresMedium.ForeColor = activeScheme.Secondary;
            labelHighScoresHard.ForeColor = activeScheme.Secondary;
            labelHighScoresExtreme.ForeColor = activeScheme.Secondary;
            dbHandler.readHighScores(selectedHSMenu, selectedHSMenuLevel);
            showHighScores();
        }

        private void labelHighScoresMedium_Click(object sender, EventArgs e)
        {
            selectedHSMenu = 2;
            labelHighScoresEasy.ForeColor = activeScheme.Secondary;
            labelHighScoresHard.ForeColor = activeScheme.Secondary;
            labelHighScoresExtreme.ForeColor = activeScheme.Secondary;
            dbHandler.readHighScores(selectedHSMenu, selectedHSMenuLevel);
            showHighScores();
        }

        private void labelHighScoresHard_Click(object sender, EventArgs e)
        {
            selectedHSMenu = 3;
            labelHighScoresEasy.ForeColor = activeScheme.Secondary;
            labelHighScoresMedium.ForeColor = activeScheme.Secondary;
            labelHighScoresExtreme.ForeColor = activeScheme.Secondary;
            dbHandler.readHighScores(selectedHSMenu, selectedHSMenuLevel);
            showHighScores();
        }

        private void labelHighScoresExtreme_Click(object sender, EventArgs e)
        {
            selectedHSMenu = 4;
            labelHighScoresEasy.ForeColor = activeScheme.Secondary;
            labelHighScoresMedium.ForeColor = activeScheme.Secondary;
            labelHighScoresHard.ForeColor = activeScheme.Secondary;
            dbHandler.readHighScores(selectedHSMenu, selectedHSMenuLevel);
            showHighScores();
        }

        private void labelDifficultyEasy_Click(object sender, EventArgs e)
        {
            startGame(1);
        }

        private void labelDifficultyMedium_Click(object sender, EventArgs e)
        {
            startGame(2);
        }

        private void labelDifficultyHard_Click(object sender, EventArgs e)
        {
            startGame(3);
        }

        private void labelDifficultyExtreme_Click(object sender, EventArgs e)
        {
            startGame(4);
        }

        private void label6_Click(object sender, EventArgs e)
        {
            if (edgeScrollingAllowed == false)
            {
                edgeScrollingAllowed = true;
                labelEdgeScrolling.Text = "EDGE SCROLLING ENABLED";
            }
            else
            {
                edgeScrollingAllowed = false;
                labelEdgeScrolling.Text = "EDGE SCROLLING DISABLED";
            }

            labelEdgeScrolling.Left = (this.ClientSize.Width - labelEdgeScrolling.Width) / 2;
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (loggedIn == true) // daca user-ul este logat
            {
                labelPlay.Visible = false;
                labelHighScores.Visible = false;
                labelOptions.Visible = false;
                labelExit.Visible = false;
                labelGameOver.Visible = false;
                labelGOScore.Visible = false;
                labelGOScoreNumber.Visible = false;
                labelGOReplay.Visible = false;
                labelGOMainMenu.Visible = false;
                labelGOXP.Visible = false;
                labelGOXPNumber.Visible = false;
                labelLevel.Visible = false;
                labelReqXP.Visible = false;
                labelGOXPNumberMultiplier.Visible = false;
                labelPaletteUnlocked.Visible = false;
                labelLevelUnlocked.Visible = false;

                levelUnlocked = false;

                timerWriting.Stop();
                timerBlinkXP.Stop();

                removeXPBar();
                removeTitle();

                labelLevelSelectTop.Visible = true;
                pictureBoxLevelSelect1.Visible = true;
                pictureBoxLevelSelect2.Visible = true;
                pictureBoxLevelSelect3.Visible = true;
                selectedGameLevel = 1;
                pictureBoxLevelSelect1.Visible = false;
                labelBack.Visible = true;

                labelLSDifficultyTop.Visible = true;
                labelLSDifficultyChange.Visible = true;
                labelLSMultiplierBonusTop.Visible = true;
                labelLSMultiplierBonusChange.Visible = true;

                labelLSDifficultyChange.Text = "NONE";
                labelLSMultiplierBonusChange.Text = "+0";
                labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
                labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);

                pictureBoxLevelSelect2.BackgroundImage = Image.FromFile(imageHandler.GetImage(selectedGameLevel));
                pictureBoxLevelSelect3.BackgroundImage = Image.FromFile(imageHandler.GetImage(selectedGameLevel + 1));
            }
            else
            {
                labelPlay.Visible = false;
                labelHighScores.Visible = false;
                labelOptions.Visible = false;
                labelExit.Visible = false;
                labelGameOver.Visible = false;
                labelGOScore.Visible = false;
                labelGOScoreNumber.Visible = false;
                labelGOReplay.Visible = false;
                labelGOMainMenu.Visible = false;
                labelGOXP.Visible = false;
                labelGOXPNumber.Visible = false;
                labelLevel.Visible = false;
                labelReqXP.Visible = false;
                labelGOXPNumberMultiplier.Visible = false;

                timerWriting.Stop();
                timerBlinkXP.Stop();

                removeXPBar();
                removeTitle();

                labelUserName.Visible = true;
                textBoxUserName.Visible = true;
                labelPassword.Visible = true;
                textBoxPassword.Visible = true;
                labelLogIn.Visible = true;
                labelNewAccount.Visible = true;
                labelBack.Visible = true;
            }
        }

        private void labelOptions_Click(object sender, EventArgs e)
        {
            timerWriting.Stop();
            removeTitle();
 
            //hide menu buttons
            labelPlay.Visible = false;
            labelHighScores.Visible = false;
            labelOptions.Visible = false;
            labelExit.Visible = false;
            labelPaletteUnlocked.Visible = false;
            labelLevelUnlocked.Visible = false;

            if (paletteUnlocked == true)
                labelPaletteUnlockedOptions.Visible = true;
            else
                labelPaletteUnlockedOptions.Visible = false;

            labelChoosePalette.Visible = true;
            labelResetProgress.Visible = true;
            labelRemoveAccount.Visible = true;
            labelLogOut.Visible = true;
            labelBack.Visible = true;
        }

        private void labelLogOut_Click(object sender, EventArgs e)
        {
            dbHandler.saveProgress(currentUser);
            loggedIn = false;
            currentUser = "";
            labelPlay.Text = "LOG IN";
            labelPlay.Left = (this.ClientSize.Width - labelPlay.Width) / 2;

            selectedPalette = 1;
            Snake_Load(sender, e);
        }

        private void labelChoosePalette_Click(object sender, EventArgs e)
        {
            labelChoosePalette.Visible = false;
            labelResetProgress.Visible = false;
            labelBack.Visible = false;
            labelLogOut.Visible = false;
            labelRemoveAccount.Visible = false;
            labelPaletteUnlockedOptions.Visible = false;

            paletteUnlocked = false;

            labelBackPalette.Visible = true;
            labelChoosePaletteI.Visible = true;

            if(selectedPalette > 1)
                labelPalette1.Text = paletteList[selectedPalette - 1];
            labelPalette1.Visible = true;

            labelPalette2.Text = paletteList[selectedPalette];
            labelPalette2.Visible = true;

            if(selectedPalette < 23)
                labelPalette3.Text = paletteList[selectedPalette + 1];
            labelPalette3.Visible = true;

            if (selectedPalette == 1)
                labelPalette1.Visible = false;

            if (selectedPalette == 23)
                labelPalette3.Visible = false;

            if (level == selectedPalette)
                labelPalette3.Visible = false;

            //center them
            labelPalette1.Left = (this.ClientSize.Width - labelPalette1.Width) / 2;
            labelPalette2.Left = (this.ClientSize.Width - labelPalette2.Width) / 2 + 8;
            labelPalette3.Left = (this.ClientSize.Width - labelPalette3.Width) / 2;
        }

        private void labelPalette1_Click(object sender, EventArgs e)
        {
            selectedPalette--;

            if(selectedPalette >= 1)
                labelPalette1.Text = paletteList[selectedPalette - 1];

            labelPalette2.Text = paletteList[selectedPalette];
            labelPalette3.Text = paletteList[selectedPalette + 1];

            if (selectedPalette == 1)
                labelPalette1.Visible = false;

            if (selectedPalette == 22)
                labelPalette3.Visible = true;

            if (level == selectedPalette)
                labelPalette3.Visible = false;
            else
                labelPalette3.Visible = true;

            //center them
            labelPalette1.Left = (this.ClientSize.Width - labelPalette1.Width) / 2;
            labelPalette2.Left = (this.ClientSize.Width - labelPalette2.Width) / 2 + 8;
            labelPalette3.Left = (this.ClientSize.Width - labelPalette3.Width) / 2;

            colourSchemes.Calls(paletteList[selectedPalette]);
            changeColourScheme();

            labelPalette1_MouseEnter(sender, e);
        }

        private void labelPalette3_Click(object sender, EventArgs e)
        {
            selectedPalette++;

            labelPalette1.Text = paletteList[selectedPalette - 1];
            labelPalette2.Text = paletteList[selectedPalette];

            if(selectedPalette < 23)
                labelPalette3.Text = paletteList[selectedPalette + 1];

            if (selectedPalette == 2)
                labelPalette1.Visible = true;

            if (selectedPalette == 23)
                labelPalette3.Visible = false;

            labelPalette2.ForeColor = activeScheme.Tertiary;

            if (level == selectedPalette)
                labelPalette3.Visible = false;
            else
                labelPalette3.Visible = true;

            //center them
            labelPalette1.Left = (this.ClientSize.Width - labelPalette1.Width) / 2;
            labelPalette2.Left = (this.ClientSize.Width - labelPalette2.Width) / 2 + 8;
            labelPalette3.Left = (this.ClientSize.Width - labelPalette3.Width) / 2;

            colourSchemes.Calls(paletteList[selectedPalette]);
            changeColourScheme();

            labelPalette3_MouseEnter(sender, e);
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
            labelBack.Visible = false;
            labelResetProgress.Visible = false;
            labelChoosePalette.Visible = false;
            labelLogOut.Visible = false;
            labelRemoveAccount.Visible = false;

            labelRPQuestion.Visible = true;
            labelRPYes.Visible = true;
            labelRPNo.Visible = true;
        }

        private void labelRPNo_Click(object sender, EventArgs e)
        {
            labelRPQuestion.Visible = false;
            labelRPYes.Visible = false;
            labelRPNo.Visible = false;
            labelOptions_Click(sender, e);
        }

        private void labelRPYes_Click(object sender, EventArgs e)
        {
            level = 1;
            levelXP = 0;
            selectedPalette = 1;
            userPalette = 1;

            dbHandler.resetProgress(currentUser);

            labelRPQuestion.Visible = false;
            labelRPYes.Visible = false;
            labelRPNo.Visible = false;
            labelOptions_Click(sender, e);
        }

        private void labelRemoveAccount_Click(object sender, EventArgs e)
        {
            labelBack.Visible = false;
            labelResetProgress.Visible = false;
            labelChoosePalette.Visible = false;
            labelLogOut.Visible = false;
            labelRemoveAccount.Visible = false;

            labelDeleteAccountQ.Visible = true;
            labelDeleteAccountYes.Visible = true;
            labelDeleteAccountNo.Visible = true;
        }

        private void labelDeleteAccountNo_Click(object sender, EventArgs e)
        {
            labelDeleteAccountQ.Visible = false;
            labelDeleteAccountYes.Visible = false;
            labelDeleteAccountNo.Visible = false;
            labelOptions_Click(sender, e);
        }

        private void labelDeleteAccountYes_Click(object sender, EventArgs e)
        {
            labelDeleteAccountYes.Visible = false;
            labelDeleteAccountNo.Visible = false;
            labelDeleteAccountQ.Visible = false;

            labelVerifyIdentityPass.Visible = true;
            textBoxVerifyIdentityPass.Text = "password";
            textBoxVerifyIdentityPass.Visible = true;
            labelVerifyIdentityQ.Visible = true;
            labelVerifyIdentitySure.Visible = true;
            labelVerifyIdentityYes.Visible = true;
            labelVerifyIdentityNo.Visible = true;
        }

        private void labelVerifyIdentityYes_Click(object sender, EventArgs e)
        {
            if(dbHandler.deleteAccount(currentUser, textBoxVerifyIdentityPass.Text) == 1)
            {
                labelVerifyIdentityIncorrectPass.Visible = true;
                timerVIPassError.Start();
            }
            else
            {
                labelVerifyIdentityPass.Visible = false;
                textBoxVerifyIdentityPass.Visible = false;
                labelVerifyIdentityQ.Visible = false;
                labelVerifyIdentitySure.Visible = false;
                labelVerifyIdentityYes.Visible = false;
                labelVerifyIdentityNo.Visible = false;

                selectedPalette = 1;
                userPalette = 1;
                currentUser = "";
                colourSchemes.Calls(paletteList[selectedPalette]);
                changeColourScheme();

                loggedIn = false;
                dbHandler.DBInitialisation();
                Snake_Load(sender, e);
            }
        }

        private void labelVerifyIdentityNo_Click(object sender, EventArgs e)
        {
            labelVerifyIdentityPass.Visible = false;
            textBoxVerifyIdentityPass.Visible = false;
            labelVerifyIdentityQ.Visible = false;
            labelVerifyIdentitySure.Visible = false;
            labelVerifyIdentityYes.Visible = false;
            labelVerifyIdentityNo.Visible = false;

            labelOptions_Click(sender, e);
        }

        private void labelPauseResume_Click(object sender, EventArgs e)
        {
            hidePauseMenu();
        }

        private void labelPauseRestart_Click(object sender, EventArgs e)
        {
            labelPauseResume.Visible = false;
            labelPauseGP.Visible = false;
            labelPausedMainMenu.Visible = false;
            labelPauseRestart.Visible = false;

            labelPGRestartQ.Visible = true;
            labelPGRestartNo.Visible = true;
            labelPGRestartYes.Visible = true;
        }

        private void labelPGRestartYes_Click(object sender, EventArgs e)
        {
            labelPGRestartQ.Visible = false;
            labelPGRestartYes.Visible = false;
            labelPGRestartNo.Visible = false;

            gameStarted = false;

            startGame(currentDifficulty);
        }

        private void labelPGRestartNo_Click(object sender, EventArgs e)
        {
            labelPGRestartQ.Visible = false;
            labelPGRestartYes.Visible = false;
            labelPGRestartNo.Visible = false;

            labelPausedMainMenu.Visible = true;
            labelPauseGP.Visible = true;
            labelPauseRestart.Visible = true;
            labelPauseResume.Visible = true;
        }

        private void labelPausedMainMenu_Click(object sender, EventArgs e)
        {
            labelPauseResume.Visible = false;
            labelPauseGP.Visible = false;
            labelPausedMainMenu.Visible = false;
            labelPauseRestart.Visible = false;

            labelPGMainMenuQ.Visible = true;
            labelPGMainMenuYes.Visible = true;
            labelPGMainMenuNo.Visible = true;
        }

        private void labelPGMainMenuNo_Click(object sender, EventArgs e)
        {
            labelPGMainMenuQ.Visible = false;
            labelPGMainMenuYes.Visible = false;
            labelPGMainMenuNo.Visible = false;

            labelPausedMainMenu.Visible = true;
            labelPauseGP.Visible = true;
            labelPauseRestart.Visible = true;
            labelPauseResume.Visible = true;
        }

        private void labelPGMainMenuYes_Click(object sender, EventArgs e)
        {
            Snake_Load(sender, e);
        }

        private void textBoxUserName_Click(object sender, EventArgs e)
        {
           textBoxUserName.Text = "";
        }

        private void textBoxPassword_Click(object sender, EventArgs e)
        {
            textBoxPassword.Text = "";
        }

        private void textBoxVerifyIdentityPass_Click(object sender, EventArgs e)
        {
            textBoxVerifyIdentityPass.Text = "";
        }

        private void labelLogIn_Click(object sender, EventArgs e)
        {
            if (textBoxUserName.Text == "user" || textBoxUserName.Text == "")
            {
                labelLoginError.Text = "INVALID USER NAME";
                labelLoginError.Left = (this.ClientSize.Width - labelLoginError.Width) / 2;
                labelLoginError.Visible = true;
                timerLabelError.Start();
            }
            else
            {
                int returnvalue = dbHandler.checkLoginCredentials(textBoxUserName.Text, textBoxPassword.Text);

                if (returnvalue == 1)
                {
                    labelLoginError.Text = "NO USER FOUND";
                    labelLoginError.Left = (this.ClientSize.Width - labelLoginError.Width) / 2;
                    labelLoginError.Visible = true;
                    timerLabelError.Start();
                }
                else
                    if (returnvalue == 2)
                {
                    labelLoginError.Text = "INCORRECT PASSWORD";
                    labelLoginError.Left = (this.ClientSize.Width - labelLoginError.Width) / 2;
                    labelLoginError.Visible = true;
                    timerLabelError.Start();
                }
                else
                    if (returnvalue == 3)
                    {
                        loggedIn = true;
                        currentUser = textBoxUserName.Text;

                        selectedPalette = userPalette;

                        colourSchemes.Calls(paletteList[userPalette]);
                        changeColourScheme();
                        Snake_Load(sender, e);
                    }
            }
        }

        private void labelNewAccount_Click(object sender, EventArgs e)
        {
            if (textBoxUserName.Text == "user" || textBoxUserName.Text == "")
            {
                labelLoginError.Text = "ILLEGAL USER NAME";
                labelLoginError.Left = (this.ClientSize.Width - labelLoginError.Width) / 2;
                labelLoginError.Visible = true;
                timerLabelError.Start();
            }
            else
            {
                int returnvalue = dbHandler.checkLoginCredentials(textBoxUserName.Text, textBoxPassword.Text);
                if (returnvalue == 1)
                {
                    if (textBoxPassword.Text != null)
                    {
                        dbHandler.addNewAccount(textBoxUserName.Text, textBoxPassword.Text);
                        loggedIn = true;
                        dbHandler.DBInitialisation();
                        currentUser = textBoxUserName.Text;
                        Snake_Load(sender, e);
                    }
                }
                else
                {
                    labelLoginError.Text = "USER NAME TAKEN";
                    labelLoginError.Left = (this.ClientSize.Width - labelLoginError.Width) / 2;
                    labelLoginError.Visible = true;
                    timerLabelError.Start();
                }
            }
        }

        private void pictureBoxLevelSelect1_Click(object sender, EventArgs e)
        {
            selectedGameLevel--;
            pictureBoxLevelSelect3.Visible = true;
            if (selectedGameLevel == 1)
            {
                pictureBoxLevelSelect2.BackgroundImage = Image.FromFile(imageHandler.GetImage(selectedGameLevel));
                pictureBoxLevelSelect3.BackgroundImage = Image.FromFile(imageHandler.GetImage(selectedGameLevel + 1));
                pictureBoxLevelSelect1.Visible = false;
            }
            else
            {
                pictureBoxLevelSelect1.BackgroundImage = Image.FromFile(imageHandler.GetImage(selectedGameLevel - 1));
                pictureBoxLevelSelect2.BackgroundImage = Image.FromFile(imageHandler.GetImage(selectedGameLevel));
                pictureBoxLevelSelect3.BackgroundImage = Image.FromFile(imageHandler.GetImage(selectedGameLevel + 1));
            }

            if (selectedGameLevel == 1)
            {
                labelLSDifficultyChange.Text = "NONE";
                labelLSMultiplierBonusChange.Text = "+0";
                labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
                labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);
            }
            else
                if(selectedGameLevel == 2)
                {
                    labelLSDifficultyChange.Text = "EASY";
                    labelLSMultiplierBonusChange.Text = "+1";
                    labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
                    labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);
                }
                else
                    if(selectedGameLevel == 3)
                    {
                        labelLSDifficultyChange.Text = "MEDIUM";
                        labelLSMultiplierBonusChange.Text = "+2";
                        labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
                        labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);
                    }
                    else
                        if (selectedGameLevel == 4)
                        {
                            labelLSDifficultyChange.Text = "MEDIUM";
                            labelLSMultiplierBonusChange.Text = "+2";
                            labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
                            labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);
                        }
                        else
                            if(selectedGameLevel == 5)
                            {
                                labelLSDifficultyChange.Text = "HARD";
                                labelLSMultiplierBonusChange.Text = "+3";
                                labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
                                labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);
                            }
                            else
                                if(selectedGameLevel == 6)
                                {
                                    labelLSDifficultyChange.Text = "EXTREME";
                                    labelLSMultiplierBonusChange.Text = "+4";
                                    labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
                                    labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);
                                }
                                else
                                    if (selectedGameLevel == 7)
                                    {
                                        labelLSDifficultyChange.Text = "EXTREME";
                                        labelLSMultiplierBonusChange.Text = "+4";
                                        labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
                                        labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);
                                    }
        }

        private void pictureBoxLevelSelect2_Click(object sender, EventArgs e)
        {
            if (imageHandler.verifyIsLocked(selectedGameLevel) == false) // if it is unlocked
            {
                labelPlay.Visible = false;
                labelHighScores.Visible = false;
                labelOptions.Visible = false;
                labelExit.Visible = false;
                labelGameOver.Visible = false;
                labelGOScore.Visible = false;
                labelGOScoreNumber.Visible = false;
                labelGOReplay.Visible = false;
                labelGOMainMenu.Visible = false;
                labelGOXP.Visible = false;
                labelGOXPNumber.Visible = false;
                labelLevel.Visible = false;
                labelReqXP.Visible = false;
                labelGOXPNumberMultiplier.Visible = false;

                timerWriting.Stop();
                timerBlinkXP.Stop();

                removeXPBar();
                removeTitle();

                labelLevelSelectTop.Visible = false;
                pictureBoxLevelSelect1.Visible = false;
                pictureBoxLevelSelect2.Visible = false;
                pictureBoxLevelSelect3.Visible = false;
                labelBack.Visible = false;

                labelLSDifficultyTop.Visible = false;
                labelLSDifficultyChange.Visible = false;
                labelLSMultiplierBonusTop.Visible = false;
                labelLSMultiplierBonusChange.Visible = false;

                labelDifficultyEasy.Visible = true;
                labelDifficultyMedium.Visible = true;
                labelDifficultyHard.Visible = true;
                labelDifficultyExtreme.Visible = true;

                labelEdgeScrolling.Visible = true;
                labelEdgeScrollingMP.Visible = true;
                labelEasyMP.Visible = true;
                labelMediumMP.Visible = true;
                labelHardMP.Visible = true;
                labelExtremeMP.Visible = true;
                labelBackDifficulty.Visible = true;

                if (selectedGameLevel == 4 || selectedGameLevel == 5) // if it NEEDS edge scrolling
                {
                    labelEdgeScrolling.Visible = false;
                    labelEdgeScrollingMP.Visible = false;
                    edgeScrollingAllowed = true;
                    needsEdgeScrolling = true;
                }
                else
                {
                    if (selectedGameLevel == 2) // if you can't go over the edges
                    {
                        labelEdgeScrolling.Visible = false;
                        labelEdgeScrollingMP.Visible = false;
                        edgeScrollingAllowed = false;
                        needsEdgeScrolling = false;
                    }
                    else
                    {
                        if (edgeScrollingAllowed == false)
                            labelEdgeScrolling.Text = "EDGE SCROLLING DISABLED";
                        else
                            labelEdgeScrolling.Text = "EDGE SCROLLING ENABLED";

                        needsEdgeScrolling = false;
                        labelEdgeScrolling.Left = (this.ClientSize.Width - labelEdgeScrolling.Width) / 2;
                    }
                }

                if (selectedGameLevel == 1)
                    levelMultiplier = 0;
                else
                    if (selectedGameLevel == 2)
                    levelMultiplier = 1;
                else
                    if (selectedGameLevel == 3)
                    levelMultiplier = 2;
                else
                    if (selectedGameLevel == 4)
                    levelMultiplier = 2;
                else
                    if (selectedGameLevel == 5)
                    levelMultiplier = 3;
                else
                    if (selectedGameLevel == 6)
                    levelMultiplier = 4;
                else
                    if (selectedGameLevel == 7)
                    levelMultiplier = 4;
                else
                    levelMultiplier = 0;
            }
        }

        private void pictureBoxLevelSelect3_Click(object sender, EventArgs e)
        {
            selectedGameLevel++;
            pictureBoxLevelSelect1.Visible = true;

            if (selectedGameLevel == 7)
            {
                pictureBoxLevelSelect1.BackgroundImage = Image.FromFile(imageHandler.GetImage(selectedGameLevel - 1));
                pictureBoxLevelSelect2.BackgroundImage = Image.FromFile(imageHandler.GetImage(selectedGameLevel));
                pictureBoxLevelSelect3.Visible = false;
            }
            else
            {
                pictureBoxLevelSelect1.BackgroundImage = Image.FromFile(imageHandler.GetImage(selectedGameLevel - 1));
                pictureBoxLevelSelect2.BackgroundImage = Image.FromFile(imageHandler.GetImage(selectedGameLevel));
                pictureBoxLevelSelect3.BackgroundImage = Image.FromFile(imageHandler.GetImage(selectedGameLevel + 1));
            }

            if (selectedGameLevel == 1)
            {
                labelLSDifficultyChange.Text = "NONE";
                labelLSMultiplierBonusChange.Text = "+0";
                labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
                labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);
            }
            else
                if(selectedGameLevel == 2)
                {
                    labelLSDifficultyChange.Text = "EASY";
                    labelLSMultiplierBonusChange.Text = "+1";
                    labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
                    labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);
                }
                else
                    if(selectedGameLevel == 3)
                    {
                        labelLSDifficultyChange.Text = "MEDIUM";
                        labelLSMultiplierBonusChange.Text = "+2";
                        labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
                        labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);
                    }
                    else
                        if (selectedGameLevel == 4)
                        {
                            labelLSDifficultyChange.Text = "MEDIUM";
                            labelLSMultiplierBonusChange.Text = "+2";
                            labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
                            labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);
                        }
                        else
                            if(selectedGameLevel == 5)
                            {
                                labelLSDifficultyChange.Text = "HARD";
                                labelLSMultiplierBonusChange.Text = "+3";
                                labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
                                labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);
                            }
                            else
                                if (selectedGameLevel == 6)
                                {
                                    labelLSDifficultyChange.Text = "EXTREME";
                                    labelLSMultiplierBonusChange.Text = "+4";
                                    labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
                                    labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);
                                }
                                else
                                    if(selectedGameLevel == 7)
                                    {
                                        labelLSDifficultyChange.Text = "EXTREME";
                                        labelLSMultiplierBonusChange.Text = "+4";
                                        labelLSDifficultyChange.Left = labelLSDifficultyTop.Left + ((labelLSDifficultyTop.Width - labelLSDifficultyChange.Width) / 2);
                                        labelLSMultiplierBonusChange.Left = labelLSMultiplierBonusTop.Left + ((labelLSMultiplierBonusTop.Width - labelLSMultiplierBonusChange.Width) / 2);
                                    }
        }

        private void labelBackDifficulty_Click(object sender, EventArgs e)
        {
            labelDifficultyEasy.Visible = false;
            labelDifficultyMedium.Visible = false;
            labelDifficultyHard.Visible = false;
            labelDifficultyExtreme.Visible = false;
            labelEdgeScrolling.Visible = false;
            labelEdgeScrollingMP.Visible = false;
            labelEasyMP.Visible = false;
            labelMediumMP.Visible = false;
            labelHardMP.Visible = false;
            labelExtremeMP.Visible = false;
            labelBackDifficulty.Visible = false;

            labelBack.Visible = true;
            labelLevelSelectTop.Visible = true;
            pictureBoxLevelSelect1.Visible = true;
            pictureBoxLevelSelect2.Visible = true;
            pictureBoxLevelSelect3.Visible = true;
            if (selectedGameLevel == 6)
                pictureBoxLevelSelect3.Visible = false;
            if (selectedGameLevel == 1)
                pictureBoxLevelSelect1.Visible = false;

            labelLSDifficultyTop.Visible = true;
            labelLSDifficultyChange.Visible = true;
            labelLSMultiplierBonusTop.Visible = true;
            labelLSMultiplierBonusChange.Visible = true;
        }

        //--------------TIMER TICK EVENTS--------------\\

        private void timerGameSpeed_Tick(object sender, EventArgs e)
        {
            // directia este modificata la fiecare tick pentru a preveni un bug (mersul inapoi in coada)
            if (direction == 1 && newDirection != 3)
                direction = newDirection;
            else if (direction == 2 && newDirection != 4)
                direction = newDirection;
            else if (direction == 3 && newDirection != 1)
                direction = newDirection;
            else if (direction == 4 && newDirection != 2)
                direction = newDirection;
            else
            {
                direction = newDirection;
            }

            // la inceputul jocului directia va fi 0, sarpele nu se misca
            if (direction != 0)
            {
                labelArrowTutorial.Visible = false;
                timerBlinkRate.Stop();
                updateTail();
            }

            //daca avem de adaugat din lista de asteptare, incercam
            if (queueLength > 0)
                checkQueueAdd();
        }

        private void timerWriting_Tick(object sender, EventArgs e)
        {
            tick++;
            //SNAKE
            writing.drawSLetter(3, 5, tick);
            writing.drawNLetter(3, 10, tick);
            writing.drawALetter(3, 15, tick);
            writing.drawKLetter(3, 20, tick);
            writing.drawELetter(3, 25, tick);

            if (tick > 5)
                timerWriting.Stop();
        }

        private void timerBlinkRate_Tick(object sender, EventArgs e)
        {
            if (labelArrowTutorial.Visible == true)
                labelArrowTutorial.Visible = false;
            else
                labelArrowTutorial.Visible = true;
        }

        private void timerBlinkLevel_Tick(object sender, EventArgs e)
        {
            levelTick++;
            if (labelLevel.Visible == false)
                labelLevel.Visible = true;
            else
                labelLevel.Visible = false;

            if (levelTick > 10)
            {
                labelLevel.Visible = true;
                timerBlinkLevel.Stop();
            }
            
        }

        private void timerBlinkXP_Tick(object sender, EventArgs e)
        {
            if (PB[19, start + 1].BackColor == activeScheme.Primary)
            {
                for (int i = start; i <= end && i <= 27; ++i)
                    PB[19, i].BackColor = activeScheme.Tertiary;
            }
            else
            {
                for (int i = start; i <= end && i <= 27; ++i)
                    PB[19, i].BackColor = activeScheme.Primary;
            }
        }

        private void timerGameOver_Tick(object sender, EventArgs e)
        {
            GOTick++;

            if (GOTick == 1)
                labelGameOver.Visible = true;

            if (GOTick == 2)
                labelGOScore.Visible = true;

            if (GOTick == 3)
            {
                labelGOScoreNumber.Text = gameScore.ToString();
                labelGOScoreNumber.Left = (this.ClientSize.Width - labelGOScoreNumber.Width) / 2;
                labelGOScoreNumber.Visible = true;
            }

            if (GOTick == 4)
                labelGOXP.Visible = true;

            if (GOTick == 5)
            {
                labelGOXPNumber.Text = currentXP.ToString();
                labelGOXPNumber.Left = (this.ClientSize.Width - labelGOXPNumber.Width) / 2;
                labelGOXPNumber.Visible = true;

                labelGOXPNumberMultiplier.Text = "(X" + difficultyMultiplier + ")";
                labelGOXPNumberMultiplier.Left = labelGOXPNumber.Left + labelGOXPNumber.Width + 3;
                labelGOXPNumberMultiplier.Visible = true;
            }

            if (GOTick == 6)
            {
                if(level == 23)
                {
                    labelLevel.Text = "LEVEL " + level.ToString();
                    labelLevel.Left = (this.ClientSize.Width - labelLevel.Width) / 2;
                    labelLevel.Visible = true;
                    leveledUp = false;
                    labelLevelUpUnlocks.Visible = false;
                    labelGOReplay.Visible = true;
                    labelGOMainMenu.Visible = true;

                    labelReqXP.Visible = false;
                    for (int i = 6; i <= 27; ++i)
                        PB[19, i].BackColor = activeScheme.Secondary;
                }
                else
                {
                    showXPBar();
                    labelLevel.Text = "LEVEL " + level.ToString();
                    labelLevel.Left = (this.ClientSize.Width - labelLevel.Width) / 2;
                    labelLevel.Visible = true;

                    if (leveledUp == true)
                    {
                        timerBlinkLevel.Start();
                        if (level == 3 || level == 7 || level == 11 || level == 14 || level == 18 || level == 21)
                            labelLevelUpUnlocks.Text = "NEW PALETTE AND LEVEL UNLOCKED";
                        else
                            labelLevelUpUnlocks.Text = "NEW PALETTE UNLOCKED";

                        labelLevelUpUnlocks.Left = (this.ClientSize.Width - labelLevelUpUnlocks.Width) / 2;
                        labelLevelUpUnlocks.Visible = true;
                        leveledUp = false;
                    }

                    labelReqXP.Text = "(" + (levelRequiredXP - levelXP).ToString() + " REQUIRED TO LEVEL UP)";
                    labelReqXP.Left = (this.ClientSize.Width - labelReqXP.Width) / 2;
                    labelReqXP.Visible = true;

                    labelGOReplay.Visible = true;
                    labelGOMainMenu.Visible = true;
                }
            }

            if (GOTick == 7)
                timerGameOver.Stop();
        }

        private void timerRPYesBlink_Tick(object sender, EventArgs e)
        {
            if (labelRPYes.ForeColor == activeScheme.Tertiary)
                labelRPYes.ForeColor = activeScheme.Secondary;
            else
                labelRPYes.ForeColor = activeScheme.Tertiary;
        }

        private void timerDAYesBlink_Tick(object sender, EventArgs e)
        {
            if (labelDeleteAccountYes.ForeColor == activeScheme.Tertiary)
                labelDeleteAccountYes.ForeColor = activeScheme.Secondary;
            else
                labelDeleteAccountYes.ForeColor = activeScheme.Tertiary;
        }

        private void timerVIBlink_Tick(object sender, EventArgs e)
        {
            if (labelVerifyIdentityYes.ForeColor == activeScheme.Tertiary)
                labelVerifyIdentityYes.ForeColor = activeScheme.Secondary;
            else
                labelVerifyIdentityYes.ForeColor = activeScheme.Tertiary;
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

        private void timerPickupBlink_Tick(object sender, EventArgs e)
        {
            if (PB[pickupLocation.xPos, pickupLocation.yPos].BackColor == activeScheme.Tertiary)
                PB[pickupLocation.xPos, pickupLocation.yPos].BackColor = activeScheme.Secondary;
            else
                PB[pickupLocation.xPos, pickupLocation.yPos].BackColor = activeScheme.Tertiary;
        }

        //--------------MOUSE ENTER / LEAVE EVENTS--------------\\

        private void labelPlay_MouseLeave(object sender, EventArgs e)
        {
            labelPlay.ForeColor = activeScheme.Secondary;
        }

        private void labelHighScores_MouseEnter(object sender, EventArgs e)
        {
            labelHighScores.ForeColor = activeScheme.Tertiary;
        }

        private void labelPlay_MouseEnter(object sender, EventArgs e)
        {
            labelPlay.ForeColor = activeScheme.Tertiary;
        }

        private void labelHighScores_MouseLeave(object sender, EventArgs e)
        {
            labelHighScores.ForeColor = activeScheme.Secondary;
        }

        private void labelOptions_MouseEnter(object sender, EventArgs e)
        {
            labelOptions.ForeColor = activeScheme.Tertiary;
        }

        private void labelOptions_MouseLeave(object sender, EventArgs e)
        {
            labelOptions.ForeColor = activeScheme.Secondary;
        }

        private void labelExit_MouseEnter(object sender, EventArgs e)
        {
            labelExit.ForeColor = activeScheme.Tertiary;
        }

        private void labelExit_MouseLeave(object sender, EventArgs e)
        {
            labelExit.ForeColor = activeScheme.Secondary;
        }

        private void labelDifficultyEasy_MouseEnter(object sender, EventArgs e)
        {
            labelDifficultyEasy.ForeColor = activeScheme.Tertiary;
        }

        private void labelDifficultyEasy_MouseLeave(object sender, EventArgs e)
        {
            labelDifficultyEasy.ForeColor = activeScheme.Secondary;
        }

        private void labelDifficultyMedium_MouseEnter(object sender, EventArgs e)
        {
            labelDifficultyMedium.ForeColor = activeScheme.Tertiary;
        }

        private void labelDifficultyMedium_MouseLeave(object sender, EventArgs e)
        {
            labelDifficultyMedium.ForeColor = activeScheme.Secondary;
        }

        private void labelDifficultyHard_MouseEnter(object sender, EventArgs e)
        {
            labelDifficultyHard.ForeColor = activeScheme.Tertiary;
        }

        private void labelDifficultyHard_MouseLeave(object sender, EventArgs e)
        {
            labelDifficultyHard.ForeColor = activeScheme.Secondary;
        }

        private void labelDifficultyExtreme_MouseEnter(object sender, EventArgs e)
        {
            labelDifficultyExtreme.ForeColor = activeScheme.Tertiary;
        }

        private void labelDifficultyExtreme_MouseLeave(object sender, EventArgs e)
        {
            labelDifficultyExtreme.ForeColor = activeScheme.Secondary;
        }

        private void labelEdgeScrolling_MouseEnter(object sender, EventArgs e)
        {
            labelEdgeScrolling.ForeColor = activeScheme.Tertiary;
        }

        private void labelEdgeScrolling_MouseLeave(object sender, EventArgs e)
        {
            labelEdgeScrolling.ForeColor = activeScheme.Secondary;
        }

        private void labelGOMainMenu_MouseEnter(object sender, EventArgs e)
        {
            labelGOMainMenu.ForeColor = activeScheme.Tertiary;
        }

        private void labelGOMainMenu_MouseLeave(object sender, EventArgs e)
        {
            labelGOMainMenu.ForeColor = activeScheme.Secondary;
        }

        private void labelBack_MouseEnter(object sender, EventArgs e)
        {
            labelBack.ForeColor = activeScheme.Tertiary;
        }

        private void labelBack_MouseLeave(object sender, EventArgs e)
        {
            labelBack.ForeColor = activeScheme.Secondary;
        }

        private void labelNewAccount_MouseEnter(object sender, EventArgs e)
        {
            labelNewAccount.ForeColor = activeScheme.Tertiary;
        }

        private void labelNewAccount_MouseLeave(object sender, EventArgs e)
        {
            labelNewAccount.ForeColor = activeScheme.Secondary;
        }

        private void labelLogIn_MouseEnter(object sender, EventArgs e)
        {
            labelLogIn.ForeColor = activeScheme.Tertiary;
        }

        private void labelLogIn_MouseLeave(object sender, EventArgs e)
        {
            labelLogIn.ForeColor = activeScheme.Secondary;
        }

        private void labelResetProgress_MouseEnter(object sender, EventArgs e)
        {
            labelResetProgress.ForeColor = activeScheme.Tertiary;
        }

        private void labelResetProgress_MouseLeave(object sender, EventArgs e)
        {
            labelResetProgress.ForeColor = activeScheme.Secondary;
        }

        private void labelChoosePalette_MouseEnter(object sender, EventArgs e)
        {
            labelChoosePalette.ForeColor = activeScheme.Tertiary;
        }

        private void labelChoosePalette_MouseLeave(object sender, EventArgs e)
        {
            labelChoosePalette.ForeColor = activeScheme.Secondary;
        }

        private void labelBackPalette_MouseEnter(object sender, EventArgs e)
        {
            labelBackPalette.ForeColor = activeScheme.Tertiary;
        }

        private void labelBackPalette_MouseLeave(object sender, EventArgs e)
        {
            labelBackPalette.ForeColor = activeScheme.Secondary;
        }

        private void labelPalette1_MouseEnter(object sender, EventArgs e)
        {
            labelPalette1.ForeColor = activeScheme.Tertiary;
        }

        private void labelPalette1_MouseLeave(object sender, EventArgs e)
        {
            labelPalette1.ForeColor = activeScheme.Secondary;
        }

        private void labelPalette3_MouseEnter(object sender, EventArgs e)
        {
            labelPalette3.ForeColor = activeScheme.Tertiary;
        }

        private void labelPalette3_MouseLeave(object sender, EventArgs e)
        {
            labelPalette3.ForeColor = activeScheme.Secondary;
        }

        private void labelRPYes_MouseEnter(object sender, EventArgs e)
        {
            timerRPYesBlink.Start();
        }

        private void labelRPYes_MouseLeave(object sender, EventArgs e)
        {
            timerRPYesBlink.Stop();
            labelRPYes.ForeColor = activeScheme.Secondary;
        }

        private void labelRPNo_MouseEnter(object sender, EventArgs e)
        {
            labelRPNo.ForeColor = activeScheme.Tertiary;
        }

        private void labelRPNo_MouseLeave(object sender, EventArgs e)
        {
            labelRPNo.ForeColor = activeScheme.Secondary;
        }

        private void labelPauseResume_MouseEnter(object sender, EventArgs e)
        {
            labelPauseResume.ForeColor = activeScheme.Tertiary;
        }

        private void labelPauseResume_MouseLeave(object sender, EventArgs e)
        {
            labelPauseResume.ForeColor = activeScheme.Secondary;
        }

        private void labelPauseRestart_MouseEnter(object sender, EventArgs e)
        {
            labelPauseRestart.ForeColor = activeScheme.Tertiary;
        }

        private void labelPauseRestart_MouseLeave(object sender, EventArgs e)
        {
            labelPauseRestart.ForeColor = activeScheme.Secondary;
        }

        private void labelPausedMainMenu_MouseEnter(object sender, EventArgs e)
        {
            labelPausedMainMenu.ForeColor = activeScheme.Tertiary;
        }

        private void labelPausedMainMenu_MouseLeave(object sender, EventArgs e)
        {
            labelPausedMainMenu.ForeColor = activeScheme.Secondary;
        }

        private void labelPGRestartYes_MouseEnter(object sender, EventArgs e)
        {
            labelPGRestartYes.ForeColor = activeScheme.Tertiary;
        }

        private void labelPGRestartYes_MouseLeave(object sender, EventArgs e)
        {
            labelPGRestartYes.ForeColor = activeScheme.Secondary;
        }

        private void labelPGRestartNo_MouseEnter(object sender, EventArgs e)
        {
            labelPGRestartNo.ForeColor = activeScheme.Tertiary;
        }

        private void labelPGRestartNo_MouseLeave(object sender, EventArgs e)
        {
            labelPGRestartNo.ForeColor = activeScheme.Secondary;
        }

        private void labelPGMainMenuYes_MouseEnter(object sender, EventArgs e)
        {
            labelPGMainMenuYes.ForeColor = activeScheme.Tertiary;
        }

        private void labelPGMainMenuYes_MouseLeave(object sender, EventArgs e)
        {
            labelPGMainMenuYes.ForeColor = activeScheme.Secondary;
        }

        private void labelPGMainMenuNo_MouseEnter(object sender, EventArgs e)
        {
            labelPGMainMenuNo.ForeColor = activeScheme.Tertiary;
        }

        private void labelPGMainMenuNo_MouseLeave(object sender, EventArgs e)
        {
            labelPGMainMenuNo.ForeColor = activeScheme.Secondary;
        }

        private void labelMainQuitYes_MouseEnter(object sender, EventArgs e)
        {
            labelMainQuitYes.ForeColor = activeScheme.Tertiary;
        }

        private void labelMainQuitYes_MouseLeave(object sender, EventArgs e)
        {
            labelMainQuitYes.ForeColor = activeScheme.Secondary;
        }

        private void labelMainQuitNo_MouseEnter(object sender, EventArgs e)
        {
            labelMainQuitNo.ForeColor = activeScheme.Tertiary;
        }

        private void labelMainQuitNo_MouseLeave(object sender, EventArgs e)
        {
            labelMainQuitNo.ForeColor = activeScheme.Secondary;
        }

        private void labelHighScoresEasy_MouseEnter(object sender, EventArgs e)
        {
            labelHighScoresEasy.ForeColor = activeScheme.Tertiary;
        }

        private void labelHighScoresEasy_MouseLeave(object sender, EventArgs e)
        {
            if(selectedHSMenu != 1)
                labelHighScoresEasy.ForeColor = activeScheme.Secondary;
        }

        private void labelHighScoresMedium_MouseEnter(object sender, EventArgs e)
        {
            labelHighScoresMedium.ForeColor = activeScheme.Tertiary;
        }

        private void labelHighScoresMedium_MouseLeave(object sender, EventArgs e)
        {
            if (selectedHSMenu != 2)
                labelHighScoresMedium.ForeColor = activeScheme.Secondary;
        }

        private void labelHighScoresHard_MouseEnter(object sender, EventArgs e)
        {
            labelHighScoresHard.ForeColor = activeScheme.Tertiary;
        }

        private void labelHighScoresHard_MouseLeave(object sender, EventArgs e)
        {
            if (selectedHSMenu != 3)
                labelHighScoresHard.ForeColor = activeScheme.Secondary;
        }

        private void labelHighScoresExtreme_MouseEnter(object sender, EventArgs e)
        {
            labelHighScoresExtreme.ForeColor = activeScheme.Tertiary;
        }

        private void labelHighScoresExtreme_MouseLeave(object sender, EventArgs e)
        {
            if (selectedHSMenu != 4)
                labelHighScoresExtreme.ForeColor = activeScheme.Secondary;
        }

        private void labelLogOut_MouseEnter(object sender, EventArgs e)
        {
            labelLogOut.ForeColor = activeScheme.Tertiary;
        }

        private void labelLogOut_MouseLeave(object sender, EventArgs e)
        {
            labelLogOut.ForeColor = activeScheme.Secondary;
        }

        private void labelDeleteAccountYes_MouseEnter(object sender, EventArgs e)
        {
            timerDAYesBlink.Start();
        }

        private void labelDeleteAccountYes_MouseLeave(object sender, EventArgs e)
        {
            timerDAYesBlink.Stop();
            labelDeleteAccountYes.ForeColor = activeScheme.Secondary;
        }

        private void labelDeleteAccountNo_MouseEnter(object sender, EventArgs e)
        {
            labelDeleteAccountNo.ForeColor = activeScheme.Tertiary;
        }

        private void labelDeleteAccountNo_MouseLeave(object sender, EventArgs e)
        {
            labelDeleteAccountNo.ForeColor = activeScheme.Secondary;
        }

        private void labelRemoveAccount_MouseEnter(object sender, EventArgs e)
        {
            labelRemoveAccount.ForeColor = activeScheme.Tertiary;
        }

        private void labelRemoveAccount_MouseLeave(object sender, EventArgs e)
        {
            labelRemoveAccount.ForeColor = activeScheme.Secondary;
        }

        private void labelVerifyIdentityYes_MouseEnter(object sender, EventArgs e)
        {
            timerVIBlink.Start();
        }

        private void labelVerifyIdentityYes_MouseLeave(object sender, EventArgs e)
        {
            timerVIBlink.Stop();
            labelVerifyIdentityYes.ForeColor = activeScheme.Secondary;
        }

        private void labelVerifyIdentityNo_MouseEnter(object sender, EventArgs e)
        {
            labelVerifyIdentityNo.ForeColor = activeScheme.Tertiary;
        }

        private void labelVerifyIdentityNo_MouseLeave(object sender, EventArgs e)
        {
            labelVerifyIdentityNo.ForeColor = activeScheme.Secondary;
        }

        private void labelBackDifficulty_MouseEnter(object sender, EventArgs e)
        {
            labelBackDifficulty.ForeColor = activeScheme.Tertiary;
        }

        private void labelBackDifficulty_MouseLeave(object sender, EventArgs e)
        {
            labelBackDifficulty.ForeColor = activeScheme.Secondary;
        }

        private void labelHSLevel1_MouseEnter(object sender, EventArgs e)
        {
            labelHSLevel1.ForeColor = activeScheme.Tertiary;
        }

        private void labelHSLevel1_MouseLeave(object sender, EventArgs e)
        {
            if (selectedHSMenuLevel != 1)
                labelHSLevel1.ForeColor = activeScheme.Secondary;
        }

        private void labelHSLevel2_MouseEnter(object sender, EventArgs e)
        {
            labelHSLevel2.ForeColor = activeScheme.Tertiary;
        }

        private void labelHSLevel2_MouseLeave(object sender, EventArgs e)
        {
            if (selectedHSMenuLevel != 2)
                labelHSLevel2.ForeColor = activeScheme.Secondary;
        }

        private void labelHSLevel3_MouseEnter(object sender, EventArgs e)
        {
            labelHSLevel3.ForeColor = activeScheme.Tertiary;
        }

        private void labelHSLevel3_MouseLeave(object sender, EventArgs e)
        {
            if (selectedHSMenuLevel != 3)
                labelHSLevel3.ForeColor = activeScheme.Secondary;
        }

        private void labelHSLevel4_MouseEnter(object sender, EventArgs e)
        {
            labelHSLevel4.ForeColor = activeScheme.Tertiary;
        }

        private void labelHSLevel4_MouseLeave(object sender, EventArgs e)
        {
            if (selectedHSMenuLevel != 4)
                labelHSLevel4.ForeColor = activeScheme.Secondary;
        }

        private void labelHSLevel5_MouseEnter(object sender, EventArgs e)
        {
            labelHSLevel5.ForeColor = activeScheme.Tertiary;
        }

        private void labelHSLevel5_MouseLeave(object sender, EventArgs e)
        {
            if (selectedHSMenuLevel != 5)
                labelHSLevel5.ForeColor = activeScheme.Secondary;
        }

        private void labelHSLevel6_MouseEnter(object sender, EventArgs e)
        {
            labelHSLevel6.ForeColor = activeScheme.Tertiary;
        }

        private void labelHSLevel6_MouseLeave(object sender, EventArgs e)
        {
            if (selectedHSMenuLevel != 6)
                labelHSLevel6.ForeColor = activeScheme.Secondary;
        }

        private void labelHSLevel7_MouseEnter(object sender, EventArgs e)
        {
            labelHSLevel7.ForeColor = activeScheme.Tertiary;
        }

        private void labelHSLevel7_MouseLeave(object sender, EventArgs e)
        {
            if (selectedHSMenuLevel != 7)
                labelHSLevel7.ForeColor = activeScheme.Secondary;
        }

        private void labelGOReplay_MouseEnter(object sender, EventArgs e)
        {
            labelGOReplay.ForeColor = activeScheme.Tertiary;
        }

        private void labelGOReplay_MouseLeave(object sender, EventArgs e)
        {
            labelGOReplay.ForeColor = activeScheme.Secondary;
        }
    }
}
