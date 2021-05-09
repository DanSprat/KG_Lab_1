using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace KGraph
{
    class Sepia : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y) {
            Color sourceColor = sourceImage.GetPixel(x, y);
            float intensity =(float)(sourceColor.R * 0.299 + sourceColor.G * 0.587 + 0.114 * sourceColor.B);
            float k = 30f;
            float colorR = intensity + 2 * k;
            float colorG = intensity +(float)( 0.5 * k);
            float colorB = intensity - k;
            return Color.FromArgb(Clamp((int)colorR, 0, 255), Clamp((int)colorG, 0, 255), Clamp((int)colorB, 0, 255));
        }
    }
}
