using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace KGraph
{
    class MaximumFilter :Filters
    {
        int radiusX;
        int radiusY;

        public MaximumFilter(int radiusX_ = 3, int radiusY_ = 3)
        {
            radiusX = radiusX_;
            radiusY = radiusY_;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color_x_y = sourceImage.GetPixel(x, y);

            int maxR = 0;
            int maxG = 0;
            int maxB = 0;
            for (int l = -radiusY; l <= radiusY; ++l)
                for (int k = -radiusX; k <= radiusX; ++k)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    if (neighborColor.R > maxR)
                        maxR = neighborColor.R;
                    if (neighborColor.G> maxG)
                        maxG = neighborColor.G;
                    if (neighborColor.B > maxB)
                        maxB = neighborColor.B;
                }
            
          

            return Color.FromArgb(
            Clamp((int)maxR, 0, 255),
            Clamp((int)maxG, 0, 255),
            Clamp((int)maxB, 0, 255)
            );
        }
    }
}
