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
            public int xPos, yPos, direction, TPDirection; // TPDirection - 1 - sus, 2 - dreapta, 3 - jos, 4 - stanga
            public bool isTurningPoint;
        }

        int direction = 0; // 0 inseamna nimic, 1- sus, 2- dreapta, 3- jos, 4- stanga
        int gameSpeed = 500; // default 250
        int pictureBoxSize;
        public static PictureBox[,] PB;
        public static int[,] gameBoard;
        Tail[] tail;
        int snakeLength = 3;
        Writing writing = new Writing(); //AESTHETIC, ONLY IMPLEMENT AT THE END
        
        public Snake()
        {
            InitializeComponent();
        }

        private void Snake_Load(object sender, EventArgs e)
        {
            PB = new PictureBox[33, 33];
            gameBoard = new int[33, 33];
            tail = new Tail[201];
            pictureBoxSize = panel1.Size.Height / 32;  // 640x640 e marimea formului, se imparte la 32
            for (int i = 1; i <= 32; ++i)
                for(int j = 1; j <= 32; ++j)
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

            PB[32 / 2, 32 / 2].BackColor = Color.White;
            PB[32 / 2, 32 / 2 - 1].BackColor = Color.White;
            PB[32 / 2, 32 / 2 - 2].BackColor = Color.White;
            gameBoard[32 / 2, 32 / 2] = 1;
            gameBoard[32 / 2, 32 / 2 + 1] = 1;
            gameBoard[32 / 2, 32 / 2 + 2] = 1;
            tail[1].xPos = 32 / 2;
            tail[1].yPos = 32 / 2;
            tail[2].xPos = 32 / 2;
            tail[2].yPos = 32 / 2 - 1;
            tail[2].direction = 4;
            tail[3].xPos = 32 / 2;
            tail[3].yPos = 32 / 2 + 2;
            tail[3].direction = 4;

            timerGameSpeed.Interval = gameSpeed;
            timerGameSpeed.Start();
        }

        private void updateTail()
        {
            Tail aux = new Tail();
            Tail aux2 = new Tail();
            aux = tail[1];
            switch (direction) // schimbam directia capului
            {
                case 1:
                    {
                        tail[1].xPos--;
                        gameBoard[tail[1].xPos, tail[1].yPos] = 1;
                        break;
                    }
                case 2:
                    {
                        tail[1].yPos++;
                        gameBoard[tail[1].xPos, tail[1].yPos] = 1;
                        break;
                    }
                case 3:
                    {
                        tail[1].xPos++;
                        gameBoard[tail[1].xPos, tail[1].yPos] = 1;
                        break;
                    }
                case 4:
                    {
                        tail[1].yPos--;
                        gameBoard[tail[1].xPos, tail[1].yPos] = 1;
                        break;
                    }
            }

            for (int i = 2; i <= snakeLength; ++i)
            {
                aux2 = tail[i];
                gameBoard[tail[i].xPos, tail[i].yPos] = 0;
                tail[i] = aux;
                aux = aux2;
            }

            for (int i = 1; i <= 32; ++i)
                for (int j = 1; j <= 32; ++j)
                    if (gameBoard[i, j] == 0)
                        PB[i, j].BackColor = Color.Black;
                    else
                        PB[i, j].BackColor = Color.White;
        }

        private void timerGameSpeed_Tick(object sender, EventArgs e)
        {
            if(direction != 0)
            {
                updateTail();    
            }
            textBox1.Text = direction.ToString();
        }

        private void Snake_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    {
                        if(direction != 3)
                            direction = 1;
                        break;
                    }
                case Keys.Right:
                    {
                        if(direction != 4)
                            direction = 2;
                        break;
                    }
                case Keys.Down:
                    {
                        if(direction != 1)
                            direction = 3;
                        break;
                    }
                case Keys.Left:
                    {
                        if(direction != 2)
                            direction = 4;
                        break;
                    }
            }
        }
    }
}
