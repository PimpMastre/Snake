using System.Collections.Generic;
using System.Drawing;

namespace Snake
{
    public class ColourScheme
    {
        public Color Primary { get; set; }
        public Color Secondary { get; set; }
        public Color Tertiary { get; set; }
        public Color Level { get; set; }

        private ColourScheme() { }

        private static readonly Dictionary<string, Action<ColourScheme>> SchemeSetters = new Dictionary<string, Action<ColourScheme>>
        {
            { "default_", s => { s.Primary = Color.Black; s.Secondary = Color.White; s.Tertiary = Color.Red; s.Level = Color.FromArgb(205, 205, 205); } },
            { "leaf", s => { s.Primary = Color.Black; s.Secondary = Color.White; s.Tertiary = Color.Green; s.Level = Color.FromArgb(205, 205, 205); } },
            { "aqua", s => { s.Primary = Color.Black; s.Secondary = Color.White; s.Tertiary = Color.FromArgb(0, 126, 254); s.Level = Color.FromArgb(205, 205, 205); } },
            { "gameboy", s => { s.Primary = Color.FromArgb(50, 59, 39); s.Secondary = Color.FromArgb(156, 171, 137); s.Tertiary = Color.FromArgb(98, 113, 78); s.Level = Color.FromArgb(106, 121, 87); } },
            { "pastel", s => { s.Primary = Color.FromArgb(29, 80, 194); s.Secondary = Color.White; s.Tertiary = Color.FromArgb(253, 113, 96); s.Level = Color.FromArgb(205, 205, 205); } },
            { "darkred", s => { s.Primary = Color.Black; s.Secondary = Color.FromArgb(248, 18, 16); s.Tertiary = Color.FromArgb(218, 18, 16); s.Level = Color.FromArgb(198, 0, 0); } },
            { "grayscale", s => { s.Primary = Color.Black; s.Secondary = Color.FromArgb(230, 230, 230); s.Tertiary = Color.FromArgb(110, 110, 110); s.Level = Color.FromArgb(180, 180, 180); } },
            { "nuclear", s => { s.Primary = Color.FromArgb(15, 58, 0); s.Secondary = Color.FromArgb(221, 253, 217); s.Tertiary = Color.FromArgb(143, 203, 62); s.Level = Color.FromArgb(171, 203, 167); } },
            { "onebit", s => { s.Primary = Color.Black; s.Secondary = Color.White; s.Tertiary = Color.White; s.Level = Color.FromArgb(205, 205, 205); } },
            { "bokju", s => { s.Primary = Color.FromArgb(70, 70, 70); s.Secondary = Color.FromArgb(132, 132, 132); s.Tertiary = Color.FromArgb(1, 1, 1); s.Level = Color.FromArgb(82, 82, 82); } },
            { "purply", s => { s.Primary = Color.FromArgb(52, 26, 18); s.Secondary = Color.FromArgb(70, 200, 253); s.Tertiary = Color.FromArgb(137, 100, 179); s.Level = Color.FromArgb(20, 150, 203); } },
            { "glow", s => { s.Primary = Color.Black; s.Secondary = Color.Red; s.Tertiary = Color.White; s.Level = Color.FromArgb(205, 0, 0); } },
            { "oldncold", s => { s.Primary = Color.FromArgb(4, 30, 55); s.Secondary = Color.FromArgb(252, 232, 172); s.Tertiary = Color.FromArgb(105, 204, 238); s.Level = Color.FromArgb(202, 182, 122); } },
            { "mossy", s => { s.Primary = Color.FromArgb(85, 47, 0); s.Secondary = Color.FromArgb(119, 239, 151); s.Tertiary = Color.FromArgb(83, 133, 253); s.Level = Color.FromArgb(69, 189, 101); } },
            { "lavender", s => { s.Primary = Color.FromArgb(96, 93, 107); s.Secondary = Color.FromArgb(201, 230, 184); s.Tertiary = Color.FromArgb(145, 154, 221); s.Level = Color.FromArgb(151, 180, 134); } },
            { "forest", s => { s.Primary = Color.FromArgb(0, 0, 52); s.Secondary = Color.FromArgb(125, 103, 49); s.Tertiary = Color.FromArgb(45, 92, 53); s.Level = Color.FromArgb(75, 53, 0); } },
            { "jungle", s => { s.Primary = Color.FromArgb(0, 57, 38); s.Secondary = Color.FromArgb(253, 198, 169); s.Tertiary = Color.FromArgb(154, 52, 76); s.Level = Color.FromArgb(203, 148, 119); } },
            { "vivid", s => { s.Primary = Color.FromArgb(0, 48, 70); s.Secondary = Color.FromArgb(254, 246, 254); s.Tertiary = Color.FromArgb(200, 0, 0); s.Level = Color.FromArgb(204, 196, 204); } },
            { "winter", s => { s.Primary = Color.FromArgb(60, 98, 124); s.Secondary = Color.FromArgb(210, 253, 253); s.Tertiary = Color.FromArgb(101, 253, 253); s.Level = Color.FromArgb(160, 203, 203); } },
            { "antique", s => { s.Primary = Color.FromArgb(49, 41, 22); s.Secondary = Color.FromArgb(166, 90, 53); s.Tertiary = Color.FromArgb(238, 139, 85); s.Level = Color.FromArgb(116, 40, 3); } },
            { "dirtsnow", s => { s.Primary = Color.FromArgb(164, 164, 164); s.Secondary = Color.FromArgb(75, 75, 75); s.Tertiary = Color.FromArgb(195, 59, 59); s.Level = Color.FromArgb(25, 25, 25); } },
            { "mars", s => { s.Primary = Color.FromArgb(97, 0, 29); s.Secondary = Color.FromArgb(228, 220, 173); s.Tertiary = Color.FromArgb(233, 69, 86); s.Level = Color.FromArgb(178, 170, 123); } },
            { "sleepy", s => { s.Primary = Color.FromArgb(27, 49, 77); s.Secondary = Color.FromArgb(210, 232, 218); s.Tertiary = Color.FromArgb(224, 79, 127); s.Level = Color.FromArgb(160, 182, 168); } }
        };

        public static void Apply(string paletteId, ColourScheme scheme)
        {
            if (SchemeSetters.TryGetValue(paletteId, out var setter))
            {
                setter(scheme);
            }
        }
    }
}
