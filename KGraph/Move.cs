using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGraph
{
    class Move : Filters
    {
        int k;
        int l;
        public Move(int k = 40,int l = 40)
        {
            this.k = k;
            this.l = l;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor;
            if (x + k >= sourceImage.Width)
                sourceColor = Color.Black;
            else {
                if (x + k < 0)
                {
                    sourceColor = Color.Black;
                }
                else {
                    if (y + l < 0 || y + l >= sourceImage.Height)
                    {
                        sourceColor = Color.Black;
                    }
                    else {
                        sourceColor = sourceImage.GetPixel(x + k, y + l);
                    }
                   
                }
            }
            return sourceColor;
        }
    }
}
