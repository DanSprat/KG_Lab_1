using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGraph
{

    class Rotate :Filters
    {
        int y0;
        int x0;
        double phi;

        public Rotate( int x0 , int y0, double phi) {

            this.x0 = x0;
            this.y0 = y0;
            this.phi = phi;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            double newX = (x - x0) * Math.Cos(phi) - (y - y0) * Math.Sin(phi) + x0;
            double newY = (x - x0) * Math.Sin(phi) + (y - y0) * Math.Cos(phi) + y0;
            Color newColor;
            if ((int)newX >= sourceImage.Width || (int)newX < 0 || (int)newY >= sourceImage.Height || (int)newY < 0)
            {
                newColor = Color.Black;
            }
            else {
                newColor = sourceImage.GetPixel((int)newX, (int)newY);
            }
            return newColor;
        }
    }
}
