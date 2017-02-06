using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake;

namespace Snake
{
    class XPSystem
    {
        public int requiredXP(int userLevel)
        {
            if (userLevel == 1)
                return 10;
            if (userLevel == 2)
                return 25;
            if (userLevel == 3)
                return 40;
            if (userLevel == 4)
                return 60;
            if (userLevel == 5)
                return 80;
            if (userLevel == 6)
                return 90;
            if (userLevel == 7)
                return 100;
            if (userLevel == 8)
                return 110;
            if (userLevel == 9)
                return 125;
            if (userLevel == 10)
                return 125;
            if (userLevel == 11)
                return 130;
            if (userLevel == 12)
                return 140;
            if (userLevel == 13)
                return 150;
            if (userLevel == 14)
                return 150;
            if (userLevel == 15)
                return 155;
            if (userLevel == 16)
                return 160;
            if (userLevel == 17)
                return 170;
            if (userLevel == 18)
                return 170;
            if (userLevel == 19)
                return 180;
            if (userLevel == 20)
                return 185;
            if (userLevel == 21)
                return 190;
            if (userLevel == 22)
                return 200;
            if (userLevel == 23)
                return 210;
            return 999;
        }
    }
}
