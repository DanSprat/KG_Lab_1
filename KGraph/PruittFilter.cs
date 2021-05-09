using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
namespace KGraph
{
    class PruittFilter : MatrixFilter
    {
        protected float[,] kernelY = null;

        public PruittFilter() { 
            kernelY = new float[3,3]  { {-1,-1,-1 },{0,0,0 },{ 1,1,1} };
            kernel = new float[3, 3] { {-1,0,1 }, { -1, 0, 1 }, { -1, 0, 1 } };
        }

         public Bitmap processImage1(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage1 = new Bitmap(sourceImage.Width, sourceImage.Height);
            Bitmap resultImage2 = new Bitmap(sourceImage.Width, sourceImage.Height);
            Bitmap resultImage3 = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; ++i)
            {
                worker.ReportProgress((int)((float)i / resultImage1.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color colorX = calculateNewPixelColorX(sourceImage, i, j);
                    Color colorY = calculateNewPixelColorY(sourceImage, i, j);
                    double sqR = Math.Sqrt(colorX.R * colorX.R + colorY.R * colorY.R);
                    double sqG = Math.Sqrt(colorX.G * colorX.G + colorY.G * colorY.G);
                    double sqB = Math.Sqrt(colorX.B * colorX.B + colorY.B * colorY.B);
                    resultImage3.SetPixel(i, j, Color.FromArgb(Clamp((int)sqR, 0, 255), Clamp((int)sqG, 0, 255), Clamp((int)sqB, 0, 255)));

                }
            }

            return resultImage3;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int radiusX = kernel.GetLength(0) / 2;
            int radiusY = kernel.GetLength(1) / 2;
            float resultRX = 0;
            float resultGX = 0;
            float resultBX = 0;
            float resultRY = 0;
            float resultGY = 0;
            float resultBY = 0;
            for (int l = -radiusY; l <= radiusY; ++l)
                for (int k = -radiusX; k <= radiusX; ++k)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    resultRX += neighborColor.R * kernel[k + radiusX, l + radiusY];
                    resultGX += neighborColor.G * kernel[k + radiusX, l + radiusY];
                    resultBX += neighborColor.B * kernel[k + radiusX, l + radiusY];
                    resultRY += neighborColor.R * kernelY[k + radiusX, l + radiusY];
                    resultGY += neighborColor.G * kernelY[k + radiusX, l + radiusY];
                    resultBY += neighborColor.B * kernelY[k + radiusX, l + radiusY];
                }
            return Color.FromArgb(
            Clamp((int)Math.Sqrt(resultRX * resultRX + resultRY * resultRY), 0, 255),
            Clamp((int)Math.Sqrt(resultGX * resultGX + resultGY * resultGY), 0, 255),
            Clamp((int)Math.Sqrt(resultBX * resultBX + resultBY * resultBY), 0, 255)
            );
        }
        protected  Color calculateNewPixelColorX(Bitmap sourceImage, int x, int y)
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
            return Color.FromArgb(Clamp((int)resultR, 0, 255), Clamp((int)resultG, 0, 255), Clamp((int)resultB, 0, 255));

        }

        protected Color calculateNewPixelColorY(Bitmap sourceImage, int x, int y)
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
                    resultR += neighborColor.R * kernelY[k + radiusX, l + radiusY];
                    resultG += neighborColor.G * kernelY[k + radiusX, l + radiusY];
                    resultB += neighborColor.B * kernelY[k + radiusX, l + radiusY];

                }
            }
            return Color.FromArgb(Clamp((int)resultR, 0, 255), Clamp((int)resultG, 0, 255), Clamp((int)resultB, 0, 255));

        }
    }
}
