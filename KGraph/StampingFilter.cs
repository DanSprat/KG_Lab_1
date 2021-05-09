using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace KGraph
{
    class StampingFilter : MatrixFilter
    {

        public StampingFilter() {
           const int sizeX = 3;
           const int sizeY = 3;
           kernel = new float[sizeX, sizeY] { {0,1,0 },{1,0,-1 },{0,-1,0 } };
        }
        public override Bitmap processImage(Bitmap sourctImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourctImage.Width, sourctImage.Height);
            for (int i = 0; i < sourctImage.Width; ++i)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourctImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColorGray(sourctImage, i, j));
                }
            }

            Bitmap resultImage2 = new Bitmap(sourctImage.Width, sourctImage.Height);
            for (int i = 0; i < sourctImage.Width; ++i)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourctImage.Height; j++)
                {
                    resultImage2.SetPixel(i, j, calculateNewPixelColor(resultImage, i, j));
                }
            }

            return resultImage2;
        }
        protected Color calculateNewPixelColorGray(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int intensity = (int)(sourceColor.R * 0.299 + sourceColor.G * 0.587 + 0.114 * sourceColor.B);
            return Color.FromArgb(intensity, intensity, intensity);

        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;
            float resultR = 0;
            float resultG = 0;
            float resultB = 0;
            for (int l = -radiusY; l <= radiusY; l++)
            {
                for (int k = -radiusX; k <= radiusX; k++)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    resultR += neighborColor.R * kernel[k + radiusX, l + radiusY];
                    resultG += neighborColor.G * kernel[k + radiusX, l + radiusY];
                    resultB += neighborColor.B * kernel[k + radiusX, l + radiusY];

                }
            }
            return Color.FromArgb(Clamp(((int)resultR+255)/2, 0, 255), Clamp(((int)resultG+255)/2, 0, 255), Clamp(((int)resultB+255)/2, 0, 255));

        }
    }

  
}
