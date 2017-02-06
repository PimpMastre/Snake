using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Snake : Form
    {
        struct Tail
        {
            public int xPos, yPos; // pozitia pe coloana = x, pozitia pe linie = y
        }

        public struct colourScheme
        {
            public Color Primary, Secondary, Tertiary;
        }

        public string[] paletteList = { "", "default_", "leaf", "aqua", "gameboy", "pastel", "darkred", "grayscale", "nuclear", "onebit", "bokju", "purply", "glow", "oldncold", "mossy", "lavender", "forest", "jungle", "vivid", "winter", "antique", "dirtsnow", "mars", "sleepy" };
        int selectedPalette = 1;

        Random r = new Random();
        int randomX, randomY;

        bool PBLoaded = false;
        bool edgeScrollingAllowed = true;
        bool gameStopped = false;
        bool gameStarted = false;
        bool leveledUp = false;
        bool loggedIn = false;
        bool gamePaused = false;
        int tick = 0;
        int GOTick;
        int levelTick = 0;
        int gameScore = 0;
        int currentDifficulty = 0;
        int difficultyMultiplier = 0;
        int level = 1; // luat dintr-o baza de date
        int levelXP = 0; // luat dintr-o baza de date
        public int levelRequiredXP;
        int currentXP = 0;
        int start, end;
        int direction = 0; // 0 inseamna nimic, 1- sus, 2- dreapta, 3- jos, 4- stanga
        int newDirection = 0; // pentru a nu da update la directie instantaneu
        int gameSpeed;
        int pixelDivider = 32; // default 32
        int pictureBoxSize;
        public static PictureBox[,] PB;
        Tail[] tail;
        Tail pickupLocation;
        Tail aux = new Tail();
        Tail aux2 = new Tail();
        public static colourScheme activeScheme;
        int snakeLength = 1;
        int queueLength = 0;
        Writing writing = new Writing();
        ColourSchemes colourSchemes = new ColourSchemes();
        XPSystem xps = new XPSystem();
        DatabaseHandler dbHandler = new DatabaseHandler();
        
        public Snake()
        {
            InitializeComponent();
        }

        //--------------FORM LOAD EVENT--------------\\

        private void Snake_Load(object sender, EventArgs e)
        {
            // schimbam color scheme-ul
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

            //Game over parts + "tutorial"
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

            labelOptions.Visible = true;
            labelPlay.Visible = true;
            labelHighScores.Visible = true;
            labelOptions.Visible = true;
            labelExit.Visible = true;

            //picture box-uri
            if (PBLoaded == false)
            {
                PB = new PictureBox[pixelDivider + 5, pixelDivider + 5];
                tail = new Tail[201];
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
            textBoxUserName.ForeColor = activeScheme.Tertiary;
            labelPassword.ForeColor = activeScheme.Secondary;
            textBoxPassword.ForeColor = activeScheme.Tertiary;
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

            labelEasyMP.ForeColor = activeScheme.Tertiary;
            labelMediumMP.ForeColor = activeScheme.Tertiary;
            labelHardMP.ForeColor = activeScheme.Tertiary;
            labelExtremeMP.ForeColor = activeScheme.Tertiary;
            labelEdgeScrollingMP.ForeColor = activeScheme.Tertiary;
            labelReqXP.ForeColor = activeScheme.Tertiary;
            labelGOXPNumberMultiplier.ForeColor = activeScheme.Tertiary;
            labelPalette2.ForeColor = activeScheme.Tertiary;
            labelLevelUpUnlocks.ForeColor = activeScheme.Tertiary;

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

                    if (ok == true)
                    {
                        pickupLocation.xPos = randomX;
                        pickupLocation.yPos = randomY;
                        PB[randomX, randomY].BackColor = activeScheme.Tertiary;
                        placed = true;
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

                    if (ok == true)
                    {
                        pickupLocation.xPos = randomX;
                        pickupLocation.yPos = randomY;
                        PB[randomX, randomY].BackColor = activeScheme.Tertiary;
                        placed = true;
                    }
                }
            }
        }

        private void gameOver()
        {
            //facem sarpele si pickup-ul primary
            for (int i = 1; i <= snakeLength; ++i)
                if(!(tail[1].xPos < 1 || tail[1].xPos > pixelDivider || tail[1].yPos < 1 || tail[1].yPos > pixelDivider))
                    PB[tail[i].xPos, tail[i].yPos].BackColor = activeScheme.Primary;

            PB[pickupLocation.xPos, pickupLocation.yPos].BackColor = activeScheme.Primary;

            currentXP = gameScore * difficultyMultiplier;

            gameStarted = false;

            //pornim timerul de game over
            timerGameOver.Start();
            GOTick = 0;
        }

        private void updateTail()
        {
            
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
            if (pickupLocation.xPos == tail[1].xPos && pickupLocation.yPos == tail[1].yPos)
            {
                addNewTailPieceChecks(r.Next(1, 4));
                newPickup(false);
                gameScore++;
            }
            else
                if (edgeScrollingAllowed == false && (tail[1].xPos < 1 || tail[1].xPos > pixelDivider || tail[1].yPos < 1 || tail[1].yPos > pixelDivider))
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

            if (gameStopped == false)
            {
                // mutam toata coada catre cap
                for (int i = 2; i <= snakeLength; ++i)
                {
                    aux2 = tail[i];
                    PB[tail[i].xPos, tail[i].yPos].BackColor = activeScheme.Primary;
                    tail[i] = aux;
                    aux = aux2;
                    if (tail[1].xPos == tail[i].xPos && tail[1].yPos == tail[i].yPos)
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
                }                  
            }
        }

        private void checkQueueAdd()
        {
            if (checkIfPossible(tail[snakeLength].xPos - 1, tail[snakeLength].yPos) == true)
            {
                addNewTailPiece(1);
                queueLength--;
            }
            else
                if (checkIfPossible(tail[snakeLength].xPos, tail[snakeLength].yPos + 1) == true) // verifica 2
                {
                    addNewTailPiece(2);
                    queueLength--;
                }
                else
                    if (checkIfPossible(tail[snakeLength].xPos + 1, tail[snakeLength].yPos) == true) // verifica 3
                    {
                        addNewTailPiece(3);
                        queueLength--;
                    }
                    else
                        if (checkIfPossible(tail[snakeLength].xPos, tail[snakeLength].yPos - 1) == true) // verifica 4
                        {
                            addNewTailPiece(4);
                            queueLength--;
                        }
        }

        private bool checkIfPossible(int dirX, int dirY)
        {
            for (int i = 1; i <= snakeLength; ++i)
                if (tail[i].xPos == dirX && tail[i].yPos == dirY)
                    return false;

            if (dirX < 1 || dirX > pixelDivider || dirY < 1 || dirY > pixelDivider)
                return false;

            return true;
        }

        private void addToQueue(Tail t)
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

        private void addNewTailPieceChecks(int dir)
        {
            switch(dir)
            {
                case 1:
                    {
                        if (checkIfPossible(tail[snakeLength].xPos - 1, tail[snakeLength].yPos) == true)
                            addNewTailPiece(1);
                        else
                            if (checkIfPossible(tail[snakeLength].xPos, tail[snakeLength].yPos + 1) == true) // verifica 2
                                addNewTailPiece(2);
                            else
                                if (checkIfPossible(tail[snakeLength].xPos + 1, tail[snakeLength].yPos) == true) // verifica 3
                                    addNewTailPiece(3);
                                else
                                    if (checkIfPossible(tail[snakeLength].xPos, tail[snakeLength].yPos - 1) == true) // verifica 4
                                        addNewTailPiece(4);
                                    else
                                        addToQueue(tail[snakeLength]);
                        break;
                    }
                case 2:
                    {
                        if (checkIfPossible(tail[snakeLength - 1].xPos, tail[snakeLength - 1].yPos + 1) == true)
                             addNewTailPiece(2);
                        else
                            if (checkIfPossible(tail[snakeLength - 1].xPos - 1, tail[snakeLength - 1].yPos) == true) // verifica 1
                                addNewTailPiece(1);
                            else
                                if (checkIfPossible(tail[snakeLength - 1].xPos + 1, tail[snakeLength - 1].yPos) == true) // verifica 3
                                    addNewTailPiece(3);
                                else
                                    if (checkIfPossible(tail[snakeLength - 1].xPos, tail[snakeLength - 1].yPos - 1) == true) // verifica 4
                                        addNewTailPiece(4);
                                    else
                                        addToQueue(tail[snakeLength]);                     
                        break;
                    }
                case 3:
                    {
                        if (checkIfPossible(tail[snakeLength - 1].xPos + 1, tail[snakeLength - 1].yPos) == true)
                            addNewTailPiece(3);
                        else
                            if (checkIfPossible(tail[snakeLength - 1].xPos - 1, tail[snakeLength - 1].yPos) == true) // verifica 1
                                addNewTailPiece(1);
                            else
                                if (checkIfPossible(tail[snakeLength - 1].xPos, tail[snakeLength - 1].yPos + 1) == true) // verifica 2
                                    addNewTailPiece(2);
                                else
                                    if (checkIfPossible(tail[snakeLength - 1].xPos, tail[snakeLength - 1].yPos - 1) == true) // verifica 4
                                        addNewTailPiece(4);
                                    else
                                        addToQueue(tail[snakeLength]);
                        break;
                    }
                case 4:
                    {
                        if (checkIfPossible(tail[snakeLength - 1].xPos, tail[snakeLength - 1].yPos - 1) == true)
                            addNewTailPiece(4);
                        else
                            if (checkIfPossible(tail[snakeLength - 1].xPos - 1, tail[snakeLength - 1].yPos) == true) // verifica 1
                                addNewTailPiece(1);
                            else
                                if (checkIfPossible(tail[snakeLength - 1].xPos, tail[snakeLength - 1].yPos + 1) == true) // verifica 2
                                    addNewTailPiece(2);
                                else
                                    if (checkIfPossible(tail[snakeLength - 1].xPos + 1, tail[snakeLength - 1].yPos) == true) // verifica 3
                                        addNewTailPiece(3);
                                    else
                                        addToQueue(tail[snakeLength]);
                        break;
                    }
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

            if (edgeScrollingAllowed == false)
                difficultyMultiplier++;
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

            //sarpele initial, lungime 3
            PB[pixelDivider / 2, pixelDivider / 2 - 1].BackColor = activeScheme.Secondary;
            tail[1].xPos = pixelDivider / 2;
            tail[1].yPos = pixelDivider / 2 - 1;
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
            if(direction == 0)
            {
                timerBlinkRate.Stop();
                labelArrowTutorial.Visible = false;
            }

            //ascundem sarpele si pickup-ul
            for (int i = 1; i <= snakeLength; ++i)
                PB[tail[i].xPos, tail[i].yPos].BackColor = activeScheme.Primary;

            PB[pickupLocation.xPos, pickupLocation.yPos].BackColor = activeScheme.Primary;

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

            //recoloram sarpele si pickup-ul
            for (int i = 1; i <= snakeLength; ++i)
                PB[tail[i].xPos, tail[i].yPos].BackColor = activeScheme.Secondary;

            PB[pickupLocation.xPos, pickupLocation.yPos].BackColor = activeScheme.Tertiary;

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

            labelMainQuitQ.Visible = true;
            labelMainQuitYes.Visible = true;
            labelMainQuitNo.Visible = true;
        }

        private void labelMainQuitYes_Click(object sender, EventArgs e)
        {
            //SAVE PROGRESS


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

            labelHighScoresEasy.Visible = true;
            labelHighScoresMedium.Visible = true;
            labelHighScoresHard.Visible = true;
            labelHighScoresExtreme.Visible = true;
            labelBack.Visible = true;
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

                timerWriting.Stop();
                timerBlinkXP.Stop();

                removeXPBar();
                removeTitle();

                labelDifficultyEasy.Visible = true;
                labelDifficultyMedium.Visible = true;
                labelDifficultyHard.Visible = true;
                labelDifficultyExtreme.Visible = true;
                if (edgeScrollingAllowed == false)
                    labelEdgeScrolling.Text = "EDGE SCROLLING DISABLED";
                else
                    labelEdgeScrolling.Text = "EDGE SCROLLING ENABLED";

                labelEdgeScrolling.Left = (this.ClientSize.Width - labelEdgeScrolling.Width) / 2;

                labelEdgeScrolling.Visible = true;
                labelEdgeScrollingMP.Visible = true;
                labelEasyMP.Visible = true;
                labelMediumMP.Visible = true;
                labelHardMP.Visible = true;
                labelExtremeMP.Visible = true;
                labelBack.Visible = true;
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

            labelChoosePalette.Visible = true;
            labelResetProgress.Visible = true;
            labelBack.Visible = true;
        }

        private void labelChoosePalette_Click(object sender, EventArgs e)
        {
            labelChoosePalette.Visible = false;
            labelResetProgress.Visible = false;
            labelBack.Visible = false;

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
            // RESET USER PROGRESS HERE
            level = 1;
            levelXP = 0;

            labelRPQuestion.Visible = false;
            labelRPYes.Visible = false;
            labelRPNo.Visible = false;
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
            if (textBoxUserName.Text == "USER")
                textBoxUserName.Text = "";
        }

        private void textBoxPassword_Click(object sender, EventArgs e)
        {
            if (textBoxPassword.Text == "PASSWORD")
                textBoxPassword.Text = "";
        }

        private void labelLogIn_Click(object sender, EventArgs e) // ADD CREDENTIAL CHECK
        {
            loggedIn = true;
            Snake_Load(sender, e);
        }

        private void labelNewAccount_Click(object sender, EventArgs e) // ADD NEW ACCOUNT
        {
            loggedIn = true;
            Snake_Load(sender, e);
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
                showXPBar();

            if (GOTick == 7)
            {
                labelLevel.Text = "LEVEL " + level.ToString();
                labelLevel.Left = (this.ClientSize.Width - labelLevel.Width) / 2;
                labelLevel.Visible = true;

                if (leveledUp == true)
                {
                    timerBlinkLevel.Start();
                    labelLevelUpUnlocks.Visible = true;
                }

                leveledUp = false;

                labelReqXP.Text = "(" + (levelRequiredXP - levelXP).ToString() + " REQUIRED TO LEVEL UP)";
                labelReqXP.Left = (this.ClientSize.Width - labelReqXP.Width) / 2;
                labelReqXP.Visible = true;

                labelGOReplay.Visible = true;
                labelGOMainMenu.Visible = true;
            }

            if (GOTick == 8)
                timerGameOver.Stop();
        }

        private void timerRPYesBlink_Tick(object sender, EventArgs e)
        {
            if (labelRPYes.ForeColor == activeScheme.Tertiary)
                labelRPYes.ForeColor = activeScheme.Secondary;
            else
                labelRPYes.ForeColor = activeScheme.Tertiary;
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
            labelHighScoresEasy.ForeColor = activeScheme.Secondary;
        }

        private void labelHighScoresMedium_MouseEnter(object sender, EventArgs e)
        {
            labelHighScoresMedium.ForeColor = activeScheme.Tertiary;
        }

        private void labelHighScoresMedium_MouseLeave(object sender, EventArgs e)
        {
            labelHighScoresMedium.ForeColor = activeScheme.Secondary;
        }

        private void labelHighScoresHard_MouseEnter(object sender, EventArgs e)
        {
            labelHighScoresHard.ForeColor = activeScheme.Tertiary;
        }

        private void labelHighScoresHard_MouseLeave(object sender, EventArgs e)
        {
            labelHighScoresHard.ForeColor = activeScheme.Secondary;
        }

        private void labelHighScoresExtreme_MouseEnter(object sender, EventArgs e)
        {
            labelHighScoresExtreme.ForeColor = activeScheme.Tertiary;
        }

        private void labelHighScoresExtreme_MouseLeave(object sender, EventArgs e)
        {
            labelHighScoresExtreme.ForeColor = activeScheme.Secondary;
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
