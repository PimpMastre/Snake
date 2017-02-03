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

        int direction = 0; // 0 inseamna nimic, 1- sus, 2- dreapta, 3- jos, 4- stanga
        int newDirection = 0; // pentru a nu da update la directie instantaneu
        int gameSpeed = 500; // default 250
        int pictureBoxSize;
        public static PictureBox[,] PB;
        Tail[] tail;
        int snakeLength = 1;
        Writing writing = new Writing(); //AESTHETIC, ONLY IMPLEMENT AT THE END

        public Snake()
        {
            InitializeComponent();
        }

        private void Snake_Load(object sender, EventArgs e)
        {
            PB = new PictureBox[33, 33];
            tail = new Tail[201];
            pictureBoxSize = panel1.Size.Height / 32;  // 640x640 e marimea formului, se imparte la 32
            for (int i = 1; i <= 32; ++i)
                for (int j = 1; j <= 32; ++j)
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
            PB[32 / 2, 32 / 2 - 1].BackColor = Color.White;
            tail[1].xPos = 32 / 2;
            tail[1].yPos = 32 / 2 - 1;
            addNewTailPiece(2);
            addNewTailPiece(2);

            // timer pentru dificultate
            timerGameSpeed.Interval = gameSpeed;
            timerGameSpeed.Start();
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
        }

        private bool checkIfPossible(int dirX, int dirY)
        {
            for (int i = 1; i <= snakeLength; ++i)
                if (tail[i].xPos == dirX && tail[i].yPos == dirY)
                    return false;
            return true;
        }

        private void addToQueue() // daca nu putem adauga in nici o directie la capatul cozii, punem intr-o lista de asteptare si il adaugam cand putem
        {

        }

        void addNewTailPiece(int dir) // NOT FUNCTIONAL YET
        {
            snakeLength++;
            switch(dir)
            {
                case 1:
                    {
                        if (checkIfPossible(tail[snakeLength - 1].xPos - 1, tail[snakeLength - 1].yPos) == false || tail[snakeLength - 1].xPos - 1 < 1)
                        {
                            if (checkIfPossible(tail[snakeLength - 1].xPos, tail[snakeLength - 1].yPos + 1) == true) // verifica 2
                                addNewTailPiece(2);
                            else
                                if (checkIfPossible(tail[snakeLength - 1].xPos + 1, tail[snakeLength - 1].yPos) == true) // verifica 3
                                    addNewTailPiece(3);
                                else
                                    if (checkIfPossible(tail[snakeLength - 1].xPos, tail[snakeLength - 1].yPos - 1) == true) // verifica 4
                                        addNewTailPiece(4);
                                    else
                                        addToQueue();
                        }
                        else
                        {
                            tail[snakeLength].xPos = tail[snakeLength - 1].xPos - 1;
                            tail[snakeLength].yPos = tail[snakeLength - 1].yPos;
                            PB[tail[snakeLength].xPos, tail[snakeLength].yPos].BackColor = Color.White;
                        }
                        break;
                    }
                case 2:
                    {
                        if (checkIfPossible(tail[snakeLength - 1].xPos, tail[snakeLength - 1].yPos + 1) == false || tail[snakeLength].yPos + 1 >= 32)
                        {
                            if (checkIfPossible(tail[snakeLength - 1].xPos - 1, tail[snakeLength - 1].yPos) == true) // verifica 1
                                addNewTailPiece(1);
                            else
                                if (checkIfPossible(tail[snakeLength - 1].xPos + 1, tail[snakeLength - 1].yPos) == true) // verifica 3
                                    addNewTailPiece(3);
                                else
                                    if (checkIfPossible(tail[snakeLength - 1].xPos, tail[snakeLength - 1].yPos - 1) == true) // verifica 4
                                        addNewTailPiece(4);
                                    else
                                        addToQueue();                     
                        }
                        else
                        {
                            tail[snakeLength].xPos = tail[snakeLength - 1].xPos;
                            tail[snakeLength].yPos = tail[snakeLength - 1].yPos + 1;
                            PB[tail[snakeLength].xPos, tail[snakeLength].yPos].BackColor = Color.White;
                        }
                        break;
                    }
                case 3:
                    {
                        if (checkIfPossible(tail[snakeLength - 1].xPos + 1, tail[snakeLength - 1].yPos) == false || tail[snakeLength].xPos + 1 > 32)
                        {
                            if (checkIfPossible(tail[snakeLength - 1].xPos - 1, tail[snakeLength - 1].yPos) == true) // verifica 1
                                addNewTailPiece(1);
                            else
                                if (checkIfPossible(tail[snakeLength - 1].xPos, tail[snakeLength - 1].yPos + 1) == true) // verifica 2
                                    addNewTailPiece(2);
                                else
                                    if (checkIfPossible(tail[snakeLength - 1].xPos, tail[snakeLength - 1].yPos - 1) == true) // verifica 4
                                        addNewTailPiece(4);
                                    else
                                        addToQueue();
                        }
                        else
                        {
                            tail[snakeLength].xPos = tail[snakeLength - 1].xPos + 1;
                            tail[snakeLength].yPos = tail[snakeLength - 1].yPos;
                            PB[tail[snakeLength].xPos, tail[snakeLength].yPos].BackColor = Color.White;
                        }
                        break;
                    }
                case 4:
                    {
                        if (checkIfPossible(tail[snakeLength - 1].xPos, tail[snakeLength - 1].yPos - 1) == false || tail[snakeLength].yPos - 1 < 1)
                        {
                            if (checkIfPossible(tail[snakeLength - 1].xPos - 1, tail[snakeLength - 1].yPos) == true) // verifica 1
                                addNewTailPiece(1);
                            else
                                if (checkIfPossible(tail[snakeLength - 1].xPos, tail[snakeLength - 1].yPos + 1) == true) // verifica 2
                                    addNewTailPiece(2);
                                else
                                    if (checkIfPossible(tail[snakeLength - 1].xPos + 1, tail[snakeLength - 1].yPos) == true) // verifica 3
                                        addNewTailPiece(3);
                                    else
                                        addToQueue();
                        }
                        else
                        {
                            tail[snakeLength].xPos = tail[snakeLength - 1].xPos;
                            tail[snakeLength].yPos = tail[snakeLength - 1].yPos - 1;
                            PB[tail[snakeLength].xPos, tail[snakeLength].yPos].BackColor = Color.White;
                        }
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
                        addNewTailPiece(2);
                        break;
                    }
            }
        }
    }
}
