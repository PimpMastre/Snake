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
            public int xPos, yPos, TPDirection; // TPDirection - 1 - sus, 2 - dreapta, 3 - jos, 4 - stanga
            public bool isTurningPoint;
        }

        int direction = 0; // 0 inseamna nimic, 1- sus, 2- dreapta, 3- jos, 4- stanga
        int pictureBoxSize;
        public static PictureBox[,] PB;
        public static int[,] gameBoard;
        Tail[] tail;
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
            gameBoard[32 / 2, 32 / 2] = 1;
            tail[1].xPos = 32 / 2;
            tail[1].yPos = 32 / 2;
            tail[1].isTurningPoint = false;

            timerGameSpeed.Start();
        }

        private void timerGameSpeed_Tick(object sender, EventArgs e)
        {
            if(direction != 0)
            {
                if(direction == 1) // sus
                {

                }
                else
                    if(direction == 2) // dreapta
                    {

                    }
                    else
                    {
                        if(direction == 3) // jos
                        {

                        }
                        else
                        {
                            if(direction == 4) // stanga
                            {

                            }
                        }
                    }
            }
        }
    }
}
