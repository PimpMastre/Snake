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

        Random r = new Random();
        int randomX, randomY;

        bool PBLoaded = false;
        bool edgeScrollingAllowed = true;
        bool gameStopped = false;
        bool leveledUp = false;
        int tick = 0;
        int GOTick;
        int levelTick = 0;
        int gameScore = 0;
        int difficultyMultiplier = 0;
        int level = 1; // luat dintr-o baza de date
        int levelXP = 0; // luat dintr-o baza de date
        int currentXP = 0;
        int start, end;
        int direction = 0; // 0 inseamna nimic, 1- sus, 2- dreapta, 3- jos, 4- stanga
        int newDirection = 0; // pentru a nu da update la directie instantaneu
        int gameSpeed = 100; // default 250
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
        
        
        public Snake()
        {
            InitializeComponent();
        }

        private void Snake_Load(object sender, EventArgs e)
        {
            //colour schemes
            colourSchemes.Default();

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

            labelEasyMP.ForeColor = activeScheme.Tertiary;
            labelMediumMP.ForeColor = activeScheme.Tertiary;
            labelHardMP.ForeColor = activeScheme.Tertiary;
            labelExtremeMP.ForeColor = activeScheme.Tertiary;
            labelEdgeScrollingMP.ForeColor = activeScheme.Tertiary;
            labelReqXP.ForeColor = activeScheme.Tertiary;
            labelGOXPNumberMultiplier.ForeColor = activeScheme.Tertiary;


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
            labelPlay.Visible = true;
            labelHighScores.Visible = true;
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

            timerBlinkXP.Stop();

            //"stergem" XP Bar-ul
            for (int i = 1; i <= pixelDivider; ++i)
                PB[19, i].BackColor = activeScheme.Primary;

            tick = 0;
            timerWriting.Start();
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            labelPlay.Visible = false;
            labelHighScores.Visible = false;
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

            //"stergem" XP Bar-ul
            for (int i = 1; i <= pixelDivider; ++i)
                PB[19, i].BackColor = activeScheme.Primary;

            //"stergem" titlul
            for (int i = 1; i <= 10; ++i)
                for (int j = 1; j <= pixelDivider; ++j)
                    PB[i, j].BackColor = activeScheme.Primary;

            labelDifficultyEasy.Visible = true;
            labelDifficultyMedium.Visible = true;
            labelDifficultyHard.Visible = true;
            labelDifficultyExtreme.Visible = true;
            if (edgeScrollingAllowed == false)
                labelEdgeScrolling.Text = "EDGE SCROLLING DISABLED";
            else
                labelEdgeScrolling.Text = "EDGE SCROLLING ENABLED";

            labelEdgeScrolling.Left = (this.ClientSize.Width - labelEdgeScrolling.Width) / 2;
            labelEdgeScrolling.Update();

            labelEdgeScrolling.Visible = true;
            labelEdgeScrollingMP.Visible = true;
            labelEasyMP.Visible = true;
            labelMediumMP.Visible = true;
            labelHardMP.Visible = true;
            labelExtremeMP.Visible = true;
            labelBack.Visible = true;
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
            if(queueLength > 0)
                checkQueueAdd();
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

        void addNewTailPiece(int dir)
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

        void addNewTailPieceChecks(int dir)
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

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void buttonHighScores_Click(object sender, EventArgs e)
        {

        }

        private void timerBlinkRate_Tick(object sender, EventArgs e)
        {
            if (labelArrowTutorial.Visible == true)
                labelArrowTutorial.Visible = false;
            else
                labelArrowTutorial.Visible = true;
        }

        private void showXPBar()
        {
            int previousProgress, currentProgress; // 27 = max

            previousProgress = ((levelXP % 100) * 27 / 100); // regula de 3 simpla
            currentProgress = currentXP * 27 / 100;

            while (levelXP + currentXP >= 100) // make it better CHANGE
            {
                level++;
                previousProgress = 0;
                currentXP -= (100 - levelXP);
                levelXP = 0;
                currentProgress = currentXP * 27 / 100;
                leveledUp = true;
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

        private void timerGameOver_Tick(object sender, EventArgs e)
        {
            GOTick++;

            if(GOTick == 1)
                labelGameOver.Visible = true;

            if (GOTick == 2)
                labelGOScore.Visible = true;

            if (GOTick == 3)
            {
                labelGOScoreNumber.Text = gameScore.ToString();
                labelGOScoreNumber.Left = (this.ClientSize.Width - labelGOScoreNumber.Width) / 2;
                labelGOScoreNumber.Visible = true;
            }

            if(GOTick == 4)
                labelGOXP.Visible = true;

            if(GOTick == 5)
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

            if(GOTick == 7)
            {
                labelLevel.Text = "LEVEL " + level.ToString();
                labelLevel.Left = (this.ClientSize.Width - labelLevel.Width) / 2;
                labelLevel.Visible = true;

                if (leveledUp == true)
                    timerBlinkLevel.Start();

                leveledUp = false;

                labelReqXP.Text = "(" + (100 - levelXP).ToString() + " REQUIRED TO LEVEL UP)";
                labelReqXP.Left = (this.ClientSize.Width - labelReqXP.Width) / 2;
                labelReqXP.Visible = true;

                labelGOReplay.Visible = true;
                labelGOMainMenu.Visible = true;
            }

            if (GOTick == 8)
                timerGameOver.Stop();
        }

        private void modifyDifficulty(int DIFF)
        {
            if(DIFF == 1) // easy
            {
                difficultyMultiplier = 1;
                gameSpeed = 250;
            }

            if(DIFF == 2) // medium
            {
                difficultyMultiplier = 2;
                gameSpeed = 150;
            }

            if(DIFF == 3) // hard
            {
                difficultyMultiplier = 3;
                gameSpeed = 50;
            }

            if(DIFF == 4) // extreme
            {
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

            labelArrowTutorial.Visible = true;
            timerBlinkRate.Start();

            modifyDifficulty(DIFF);

            //"stergem" titlul
            for (int i = 1; i <= 10; ++i)
                for (int j = 1; j <= pixelDivider; ++j)
                    PB[i, j].BackColor = activeScheme.Primary;

            timerBlinkXP.Stop();
            //"stergem" XP Bar-ul
            for (int i = 1; i <= pixelDivider; ++i)
                PB[19, i].BackColor = activeScheme.Primary;

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
        }

        private void timerBlinkLevel_Tick(object sender, EventArgs e)
        {
            levelTick++;
            if (labelLevel.Visible == false)
                labelLevel.Visible = true;
            else
                labelLevel.Visible = false;

            if (levelTick > 10)
                timerBlinkLevel.Stop();
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
                        if(direction != 4)
                            newDirection = 2;
                        if(direction == 0)
                        {
                            Tail aux = tail[1];
                            tail[1] = tail[3];
                            tail[3] = aux;
                        }
                        break;
                    }
                case Keys.Down:
                    {
                        if(direction != 1)
                            newDirection = 3;
                        break;
                    }
                case Keys.Left:
                    {
                        if(direction != 2)
                            newDirection = 4;
                        break;
                    }
            }
        }
    }
}
