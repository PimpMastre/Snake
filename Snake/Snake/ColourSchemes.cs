using System.Drawing;

namespace Snake
{
    public class ColourSchemes
    {
        private readonly ColourScheme scheme;

        public ColourSchemes(ColourScheme scheme)
        {
            this.scheme = scheme;
        }

        public void ApplyScheme(string paletteId)
        {
            ColourScheme.Apply(paletteId, scheme);
        }
    }
}
