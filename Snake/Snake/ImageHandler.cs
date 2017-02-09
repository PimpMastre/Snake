using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    class ImageHandler
    {
        public bool verifyIsLocked(int levelno)
        {
            //levels unlock at 3, 7, 11, 14, 18, 21
            if (levelno == 1)
                return false;

            if (levelno == 2)
                if (Snake.level < 3)
                    return true;
                else
                    return false;

            if (levelno == 3)
                if (Snake.level < 7)
                    return true;
                else
                    return false;

            if (levelno == 4)
                if (Snake.level < 11)
                    return true;
                else
                    return false;

            if (levelno == 5)
                if(Snake.level < 14)
                return true;
            else
                return false;

            if (levelno == 6)
                if (Snake.level < 18)
                return true;
            else
                return false;

            if (levelno == 7)
                if (Snake.level < 21)
                    return true;
                else
                    return false;

            return true;
        }

        public string GetImage(int levelno)
        {
            if (levelno == 1)
                return "img/one.png";
            else
                if (levelno == 2)
                if (verifyIsLocked(2) == true)
                    return "img/twoLocked.png";
                else
                    return "img/two.png";
            else
                if (levelno == 3)
                if (verifyIsLocked(3) == true)
                    return "img/threeLocked.png";
                else
                    return "img/three.png";
            else
                if (levelno == 4)
                if (verifyIsLocked(4) == true)
                    return "img/fourLocked.png";
                else
                    return "img/four.png";
            else
                if (levelno == 5)
                if (verifyIsLocked(5) == true)
                    return "img/fiveLocked.png";
                else
                    return "img/five.png";
            else
                if (levelno == 6)
                if (verifyIsLocked(6) == true)
                    return "img/sixLocked.png";
                else
                    return "img/six.png";
            else
                if (levelno == 7)
                if (verifyIsLocked(7) == true)
                    return "img/sevenLocked.png";
                else
                    return "img/seven.png";
            return "";
        }
    }
}
