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
                return 100;
            if (userLevel == 2)
                return 150;
            if (userLevel == 3)
                return 200;
            if (userLevel == 4)
                return 300;
            if (userLevel == 5)
                return 350;
            if (userLevel == 6)
                return 350;
            if (userLevel == 7)
                return 370;
            if (userLevel == 8)
                return 390;
            if (userLevel == 9)
                return 400;
            if (userLevel == 10)
                return 410;
            if (userLevel == 11)
                return 420;
            if (userLevel == 12)
                return 450;
            if (userLevel == 13)
                return 460;
            if (userLevel == 14)
                return 470;
            if (userLevel == 15)
                return 490;
            if (userLevel == 16)
                return 500;
            if (userLevel == 17)
                return 500;
            if (userLevel == 18)
                return 500;
            if (userLevel == 19)
                return 510;
            if (userLevel == 20)
                return 520;
            if (userLevel == 21)
                return 550;
            if (userLevel == 22)
                return 600;
            return 9001;
        }
    }
}
