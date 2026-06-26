using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    /// <summary>
    /// Animated letter drawing for the title screen.
    /// Draws pixel-art letters frame-by-frame on the grid.
    /// </summary>
    public class Writing
    {
        private void DrawPixel(int x, int y, Color color)
        {
            if (x >= 1 && x <= 32 && y >= 1 && y <= 32)
            {
                var pb = SnakeForm.GridControls[x, y];
                if (pb != null) pb.BackColor = color;
            }
        }

        public void DrawSLetter(int locationX, int locationY, int tick)
        {
            var tertiary = SnakeForm.ActiveScheme?.Tertiary ?? Color.Red;
            int sA = 3, mA = 2, jA = 3, P1 = 2, P2 = 3;

            if (tick <= sA) DrawPixel(locationX, locationY + tick, tertiary);
            if (tick <= mA) DrawPixel(locationX + 2, locationY + tick, tertiary);
            if (tick <= jA) DrawPixel(locationX + 4, locationY + tick - 1, tertiary);
            if (tick == P1) DrawPixel(locationX + 1, locationY, tertiary);
            if (tick == P2) DrawPixel(locationX + 3, locationY + 3, tertiary);
        }

        public void DrawNLetter(int locationX, int locationY, int tick)
        {
            var tertiary = SnakeForm.ActiveScheme?.Tertiary ?? Color.Red;
            int sJ = 5, dJ = 5, P1 = 2, P2 = 3;

            if (tick <= sJ) DrawPixel(locationX + tick - 1, locationY, tertiary);
            if (tick <= dJ) DrawPixel(locationX + tick - 1, locationY + 3, tertiary);
            if (tick == P1) DrawPixel(locationX + P1, locationY + 1, tertiary);
            if (tick == P2) DrawPixel(locationX + P2, locationY + 2, tertiary);
        }

        public void DrawALetter(int locationX, int locationY, int tick)
        {
            var tertiary = SnakeForm.ActiveScheme?.Tertiary ?? Color.Red;
            int sJ = 4, dJ = 4, sA = 2, jA = 2;

            if (tick <= sJ) DrawPixel(locationX + tick, locationY, tertiary);
            if (tick <= dJ) DrawPixel(locationX + tick, locationY + 3, tertiary);
            if (tick <= sA) DrawPixel(locationX, locationY + tick, tertiary);
            if (tick <= jA) DrawPixel(locationX + 2, locationY + tick, tertiary);
        }

        public void DrawKLetter(int locationX, int locationY, int tick)
        {
            var tertiary = SnakeForm.ActiveScheme?.Tertiary ?? Color.Red;
            int sJ = 5, P1 = 2, P2 = 3, P3 = 4;

            if (tick <= sJ) DrawPixel(locationX + tick - 1, locationY, tertiary);
            if (tick == P1) DrawPixel(locationX + 2, locationY + 1, tertiary);
            if (tick == P2)
            {
                DrawPixel(locationX + 1, locationY + 2, tertiary);
                DrawPixel(locationX + 3, locationY + 2, tertiary);
            }
            if (tick == P3)
            {
                DrawPixel(locationX, locationY + 3, tertiary);
                DrawPixel(locationX + 4, locationY + 3, tertiary);
            }
        }

        public void DrawELetter(int locationX, int locationY, int tick)
        {
            var tertiary = SnakeForm.ActiveScheme?.Tertiary ?? Color.Red;
            int sJ = 5, sA = 3, mA = 2, jA = 3;

            if (tick <= sJ) DrawPixel(locationX + tick - 1, locationY, tertiary);
            if (tick <= sA) DrawPixel(locationX, locationY + tick, tertiary);
            if (tick <= mA) DrawPixel(locationX + 2, locationY + tick, tertiary);
            if (tick <= jA) DrawPixel(locationX + 4, locationY + tick, tertiary);
        }
    }
}
