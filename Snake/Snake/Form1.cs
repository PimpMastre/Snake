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

        Random r = new Random();
        int randomX, randomY;

        int direction = 0; // 0 inseamna nimic, 1- sus, 2- dreapta, 3- jos, 4- stanga
        int newDirection = 0; // pentru a nu da update la directie instantaneu
        int gameSpeed = 50; // default 250
        int pixelDivider = 32; // default 32
        int pictureBoxSize;
        public static PictureBox[,] PB;
        Tail[] tail;
        Tail pickupLocation;
        int snakeLength = 1;
        int queueLength = 0;
        Writing writing = new Writing(); //AESTHETIC, ONLY IMPLEMENT AT THE END

        public Snake()
        {
            InitializeComponent();
        }

        private void Snake_Load(object sender, EventArgs e)
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
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            labelTitle.Visible = false; // ascundem butoanele/labelurile
            buttonPlay.Visible = false;
            buttonHighScores.Visible = false;
            buttonExit.Visible = false;

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
                randomX = r.Next(1, 32);
                randomY = r.Next(1, 32);

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
                    tail[1].xPos = 32;
                }
                else
                    if(tail[1].xPos > 32)
                    {
                        tail[1].xPos = 1;
                    }
                    else
                        if(tail[1].yPos < 1)
                        {
                            tail[1].yPos = 32;
                        }
                        else
                            if(tail[1].yPos > 32)
                            {
                                tail[1].yPos = 1;
                            }
            for (int i = 2; i <= snakeLength; ++i)
                if (tail[1].xPos == tail[i].xPos && tail[1].yPos == tail[i].yPos)
                {
                    timerGameSpeed.Stop();
                    MessageBox.Show("Game over, man!");
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
                direction = newDirection;

            // la inceputul jocului directia va fi 0, sarpele nu se misca
            if (direction != 0)
                updateTail();

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
                case Keys.Space: // just for tests, should be removed
                    {
                        addNewTailPieceChecks(r.Next(1, 4));
                        break;
                    }
            }
        }
    }
}
