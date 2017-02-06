using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Snake;
using System.Drawing;

namespace Snake
{
    class ColourSchemes
    {
        public void Default()
        {
            Snake.activeScheme.Primary = Color.Black;
            Snake.activeScheme.Secondary = Color.White;
            Snake.activeScheme.Tertiary = Color.Red;
        }

        public void Leaf()
        {
            Snake.activeScheme.Primary = Color.Black;
            Snake.activeScheme.Secondary = Color.White;
            Snake.activeScheme.Tertiary = Color.Green;
        }

        public void Aqua()
        {
            Snake.activeScheme.Primary = Color.Black;
            Snake.activeScheme.Secondary = Color.White;
            Snake.activeScheme.Tertiary = Color.FromArgb(0, 126, 254);
        }

        public void GameBoy()
        {
            Snake.activeScheme.Primary = Color.FromArgb(50, 59, 39);
            Snake.activeScheme.Secondary = Color.FromArgb(156, 171, 137);
            Snake.activeScheme.Tertiary = Color.FromArgb(98, 113, 78);
        }

        public void DarkRed()
        {
            Snake.activeScheme.Primary = Color.Black;
            Snake.activeScheme.Secondary = Color.FromArgb(248, 18, 16);
            Snake.activeScheme.Tertiary = Color.FromArgb(218, 18, 16);
        }
        
        public void Pastel()
        {
            Snake.activeScheme.Primary = Color.FromArgb(29, 80, 194);
            Snake.activeScheme.Secondary = Color.White;
            Snake.activeScheme.Tertiary = Color.FromArgb(253, 113, 96);
        }

        public void Nuclear()
        {
            Snake.activeScheme.Primary = Color.FromArgb(15, 58, 0);
            Snake.activeScheme.Secondary = Color.FromArgb(221, 253, 217);
            Snake.activeScheme.Tertiary = Color.FromArgb(143, 203, 62);
        }

        public void Grayscale()
        {
            Snake.activeScheme.Primary = Color.Black;
            Snake.activeScheme.Secondary = Color.FromArgb(230, 230, 230);
            Snake.activeScheme.Tertiary = Color.FromArgb(110, 110, 110);
        }

        public void OneBit()
        {
            Snake.activeScheme.Primary = Color.Black;
            Snake.activeScheme.Secondary = Color.White;
            Snake.activeScheme.Tertiary = Color.White;
        }

        public void Mars()
        {
            Snake.activeScheme.Primary = Color.FromArgb(97, 0, 29);
            Snake.activeScheme.Secondary = Color.FromArgb(228, 220, 173);
            Snake.activeScheme.Tertiary = Color.FromArgb(233, 69, 86);
        }

        public void Bokju()
        {
            Snake.activeScheme.Primary = Color.FromArgb(70, 70, 70);
            Snake.activeScheme.Secondary = Color.FromArgb(132, 132, 132);
            Snake.activeScheme.Tertiary = Color.FromArgb(1, 1, 1);
        }

        public void Purply()
        {
            Snake.activeScheme.Primary = Color.FromArgb(52, 26, 18);
            Snake.activeScheme.Secondary = Color.FromArgb(70, 200, 253);
            Snake.activeScheme.Tertiary = Color.FromArgb(137, 100, 179);
        }

        public void Vivid()
        {
            Snake.activeScheme.Primary = Color.FromArgb(0, 48, 70);
            Snake.activeScheme.Secondary = Color.FromArgb(254, 246, 254);
            Snake.activeScheme.Tertiary = Color.FromArgb(200, 0, 0);
        }

        public void OldNCold()
        {
            Snake.activeScheme.Primary = Color.FromArgb(4, 30, 55);
            Snake.activeScheme.Secondary = Color.FromArgb(252, 232, 172);
            Snake.activeScheme.Tertiary = Color.FromArgb(105, 204, 238);
        }

        public void Mossy()
        {
            Snake.activeScheme.Primary = Color.FromArgb(85, 47, 0);
            Snake.activeScheme.Secondary = Color.FromArgb(119, 239, 151);
            Snake.activeScheme.Tertiary = Color.FromArgb(83, 133, 253);
        }

        public void Glow()
        {
            Snake.activeScheme.Primary = Color.Black;
            Snake.activeScheme.Secondary = Color.Red;
            Snake.activeScheme.Tertiary = Color.White;
        }

        public void Lavender()
        {
            Snake.activeScheme.Primary = Color.FromArgb(96, 93, 107);
            Snake.activeScheme.Secondary = Color.FromArgb(201, 230, 184);
            Snake.activeScheme.Tertiary = Color.FromArgb(145, 154, 221);
        }

        public void Forest()
        {
            Snake.activeScheme.Primary = Color.FromArgb(0, 0, 52);
            Snake.activeScheme.Secondary = Color.FromArgb(125, 103, 49);
            Snake.activeScheme.Tertiary = Color.FromArgb(45, 92, 53);
        }

        public void Winter()
        {
            Snake.activeScheme.Primary = Color.FromArgb(60, 98, 124);
            Snake.activeScheme.Secondary = Color.FromArgb(210, 253, 253);
            Snake.activeScheme.Tertiary = Color.FromArgb(101, 253, 253);
        }

        public void Antique()
        {
            Snake.activeScheme.Primary = Color.FromArgb(49, 41, 22);
            Snake.activeScheme.Secondary = Color.FromArgb(166, 90, 53);
            Snake.activeScheme.Tertiary = Color.FromArgb(238, 139, 85);
        }

        public void DirtSnow()
        {
            Snake.activeScheme.Primary = Color.FromArgb(164, 164, 164);
            Snake.activeScheme.Secondary = Color.FromArgb(75, 75, 75);
            Snake.activeScheme.Tertiary = Color.FromArgb(195, 59, 59);
        }

        public void Jungle()
        {
            Snake.activeScheme.Primary = Color.FromArgb(0, 57, 38);
            Snake.activeScheme.Secondary = Color.FromArgb(253, 198, 169);
            Snake.activeScheme.Tertiary = Color.FromArgb(154, 52, 76);
        }

        public void Sleepy()
        {
            Snake.activeScheme.Primary = Color.FromArgb(27, 49, 77);
            Snake.activeScheme.Secondary = Color.FromArgb(210, 232, 218);
            Snake.activeScheme.Tertiary = Color.FromArgb(224, 79, 127);
        }

        public void Calls(string paletteID)
        {
            if (paletteID == "default_")
                Default();
            else
            if (paletteID == "leaf")
                Leaf();
            else
                if (paletteID == "aqua")
                Aqua();
            else
            if (paletteID == "gameboy")
                GameBoy();
            else
                if (paletteID == "pastel")
                Pastel();
            else
            if (paletteID == "darkred")
                DarkRed();
            else
                if (paletteID == "grayscale")
                Grayscale();
            else
            if (paletteID == "nuclear")
                Nuclear();
            else
                if (paletteID == "onebit")
                OneBit();
            else
            if (paletteID == "bokju")
                Bokju();
            else
                if (paletteID == "purply")
                Purply();
            else
            if (paletteID == "glow")
                Glow();
            else
                if (paletteID == "oldncold")
                OldNCold();
            else
            if (paletteID == "mossy")
                Mossy();
            else
                if (paletteID == "lavender")
                Lavender();
            else
            if (paletteID == "forest")
                Forest();
            else
            if (paletteID == "jungle")
                Jungle();
            else
            if (paletteID == "vivid")
                Vivid();
            else
                if (paletteID == "winter")
                Winter();
            else
            if (paletteID == "antique")
                Antique();
            else
                if (paletteID == "dirtsnow")
                DirtSnow();
            else
            if (paletteID == "mars")
                Mars();
            else
                if (paletteID == "sleepy")
                Sleepy();
        }
    }
}
