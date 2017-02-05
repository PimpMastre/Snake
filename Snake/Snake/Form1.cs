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
        //CULOAREA TEXTULUI = FORM BACKGROUND COLOR
        //CULOAREA FUNDALULUI = PICTUREBOX COLORS
        struct Tail
        {
            public int xPos, yPos; // pozitia pe coloana = x, pozitia pe linie = y
        }

        Random r = new Random();
        int randomX, randomY;

        bool PBLoaded = false;
        int tick = 0;
        int GOTick;
        int gameScore = 0;
        int direction = 0; // 0 inseamna nimic, 1- sus, 2- dreapta, 3- jos, 4- stanga
        int newDirection = 0; // pentru a nu da update la directie instantaneu
        int gameSpeed = 250; // default 250
        int pixelDivider = 32; // default 32
        int pictureBoxSize;
        public static PictureBox[,] PB;
        Tail[] tail;
        Tail pickupLocation;
        int snakeLength = 1;
        int queueLength = 0;
        Writing writing = new Writing();
        
        
        public Snake()
        {
            InitializeComponent();
        }

        private void Snake_Load(object sender, EventArgs e)
        {
            //Game over parts + "tutorial"
            labelArrowTutorial.Visible = false;
            labelGameOver.Visible = false;
            labelGOScore.Visible = false;
            labelGOScoreNumber.Visible = false;
            labelGOReplay.Visible = false;
            labelGOMainMenu.Visible = false;
            labelPlay.Visible = true;
            labelHighScores.Visible = true;
            labelExit.Visible = true;

            //picture box-uri
            if (PBLoaded == false)
            {
                PB = new PictureBox[pixelDivider + 1, pixelDivider + 1];
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
                        PB[i, j].BackColor = Color.Black;
                        PB[i, j].Parent = panel1;
                    }
                PBLoaded = true;
            }
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

            labelArrowTutorial.Visible = true;
            timerBlinkRate.Start();
            timerWriting.Stop();

            //"stergem" titlul
            for (int i = 1; i <= 10; ++i)
                for (int j = 1; j <= pixelDivider; ++j)
                    PB[i, j].BackColor = Color.Black;

            //resetam totul la default
            direction = 0;
            newDirection = 0;
            gameScore = 0;
            tick = 0;
            snakeLength = 1;
            queueLength = 0;

            //sarpele initial, lungime 3
            PB[pixelDivider / 2, pixelDivider / 2 - 1].BackColor = Color.White;
            tail[1].xPos = pixelDivider / 2;
            tail[1].yPos = pixelDivider / 2 - 1;
            addNewTailPiece(2);
            PB[tail[snakeLength].xPos, tail[snakeLength].yPos].BackColor = Color.White;
            addNewTailPiece(2);
            PB[tail[snakeLength].xPos, tail[snakeLength].yPos].BackColor = Color.White;

            //plasam primul pickup
            newPickup();

            // timer pentru dificultate
            timerGameSpeed.Interval = gameSpeed;
            timerGameSpeed.Start();
        }

        private void newPickup()
        {
            bool placed = false;
            bool ok = true;

            while (placed == false)
            {
                ok = true;
                randomX = r.Next(1, pixelDivider);
                randomY = r.Next(1, pixelDivider);

                for (int i = 1; i <= snakeLength; ++i)
                    if (randomX == tail[i].xPos && randomY == tail[i].yPos)
                        ok = false;

                if(ok == true)
                {
                    pickupLocation.xPos = randomX;
                    pickupLocation.yPos = randomY;
                    PB[randomX, randomY].BackColor = Color.LimeGreen;
                    placed = true;
                }
            }
        }

        private void gameOver()
        {
            //facem sarpele si pickup-ul negru
            for (int i = 1; i <= snakeLength; ++i)
                PB[tail[i].xPos, tail[i].yPos].BackColor = Color.Black;

            PB[pickupLocation.xPos, pickupLocation.yPos].BackColor = Color.Black;

            //pornim timerul
            timerGameOver.Start();
            GOTick = 0;
        }

        private void updateTail() // MAYBE ADD EDGE SCROLLING
        {
            Tail aux = new Tail();
            Tail aux2 = new Tail();
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
            if (pickupLocation.xPos == tail[1].xPos && pickupLocation.yPos == tail[1].yPos) // ADD SCORE FUNCTION
            {
                addNewTailPieceChecks(r.Next(1, 4));
                newPickup();
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

            // mutam toata coada catre cap
            for (int i = 2; i <= snakeLength; ++i)
            {
                aux2 = tail[i];
                PB[tail[i].xPos, tail[i].yPos].BackColor = Color.Black;
                tail[i] = aux;
                aux = aux2;
            }

            // recoloram sarpele
            for (int i = 1; i <= snakeLength; ++i)
                PB[tail[i].xPos, tail[i].yPos].BackColor = Color.White;

            for (int i = 2; i <= snakeLength; ++i)
                if (tail[1].xPos == tail[i].xPos && tail[1].yPos == tail[i].yPos)
                {
                    timerGameSpeed.Stop();
                    gameOver();
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

            PB[tail[snakeLength].xPos, tail[snakeLength].yPos].BackColor = Color.White;
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
            {
                labelGOReplay.Visible = true;
                labelGOMainMenu.Visible = true;
            }

            if (GOTick == 5)
                timerGameOver.Stop();
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
