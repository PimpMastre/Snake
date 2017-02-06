using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Snake;

/* AR TREBUI SA ARATE ASA:
 * PENTRU LITERA A
 *  ##   S>E
 * #  #  D  D
 * ####   >E
 * #  #
 * #  #  E  E
 *  */

namespace Snake
{
    public class Writing
    {
        public void drawALetter(int locationX, int locationY, int tick)
        {
            int sJ = 4, dJ = 4, sA = 2, jA = 2;

            if (tick <= sJ)
                Snake.PB[locationX + tick, locationY].BackColor = Snake.activeScheme.Tertiary;

            if(tick <= dJ)
                Snake.PB[locationX + tick, locationY + 3].BackColor = Snake.activeScheme.Tertiary;

            if(tick <= sA)
                Snake.PB[locationX, locationY + tick].BackColor = Snake.activeScheme.Tertiary;

            if(tick <= jA)
                Snake.PB[locationX + 2, locationY + tick].BackColor = Snake.activeScheme.Tertiary;
        }

        public void drawELetter(int locationX, int locationY, int tick)
        {
            int sJ = 5, sA = 3, mA = 2, jA = 3;

            if (tick <= sJ)
                Snake.PB[locationX + tick - 1, locationY].BackColor = Snake.activeScheme.Tertiary;

            if (tick <= sA)
                Snake.PB[locationX, locationY + tick].BackColor = Snake.activeScheme.Tertiary;

            if (tick <= mA)
                Snake.PB[locationX + 2, locationY + tick].BackColor = Snake.activeScheme.Tertiary;

            if (tick <= jA)
                Snake.PB[locationX + 4, locationY + tick].BackColor = Snake.activeScheme.Tertiary;
        }

        public void drawKLetter(int locationX, int locationY, int tick)
        {
            int sJ = 5, P1 = 2, P2 = 3, P3 = 4;

            if (tick <= sJ)
                Snake.PB[locationX + tick - 1, locationY].BackColor = Snake.activeScheme.Tertiary;

            if(tick == P1)
                Snake.PB[locationX + 2, locationY + 1].BackColor = Snake.activeScheme.Tertiary;

            if(tick == P2)
            {
                Snake.PB[locationX + 1, locationY + 2].BackColor = Snake.activeScheme.Tertiary;
                Snake.PB[locationX + 3, locationY + 2].BackColor = Snake.activeScheme.Tertiary;
            }
            
            if(tick == P3)
            {
                Snake.PB[locationX, locationY + 3].BackColor = Snake.activeScheme.Tertiary;
                Snake.PB[locationX + 4, locationY + 3].BackColor = Snake.activeScheme.Tertiary;
            }
        }

        public void drawNLetter(int locationX, int locationY, int tick)
        {
            int sJ = 5, dJ = 5, P1 = 2, P2 = 3;

            if(tick <= sJ)
                Snake.PB[locationX + tick - 1, locationY].BackColor = Snake.activeScheme.Tertiary;
            
            if(tick <= dJ)
                Snake.PB[locationX + tick - 1, locationY + 3].BackColor = Snake.activeScheme.Tertiary;
            
            if(tick == P1)
                Snake.PB[locationX + P1, locationY + 1].BackColor = Snake.activeScheme.Tertiary;

            if(tick == P2)
                Snake.PB[locationX + P2, locationY + 2].BackColor = Snake.activeScheme.Tertiary;
        }

        public void drawSLetter(int locationX, int locationY, int tick)
        {
            int sA = 3, mA = 2, jA = 3, P1 = 2, P2 = 3;

            if(tick <= sA)
                Snake.PB[locationX, locationY + tick].BackColor = Snake.activeScheme.Tertiary;

            if(tick <= mA)
                Snake.PB[locationX + 2, locationY + tick].BackColor = Snake.activeScheme.Tertiary;

            if(tick <= jA)
                Snake.PB[locationX + 4, locationY + tick - 1].BackColor = Snake.activeScheme.Tertiary;

            if(tick == P1)
                Snake.PB[locationX + 1, locationY].BackColor = Snake.activeScheme.Tertiary;

            if(tick == P2)
                Snake.PB[locationX + 3, locationY + 3].BackColor = Snake.activeScheme.Tertiary;
        }
    }
}
