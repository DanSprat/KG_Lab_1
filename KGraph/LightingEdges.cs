using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;


namespace KGraph
{
    class LightingEdges : Filters
    {

        public LightingEdges(int MedRadiusX_ = 3, int MedRadiusY_ = 3, int MaxRadiusX_ = 3, int MaxRadiusY_ = 3) {
            this.MedRadiusX = MedRadiusX_;
            this.MedRadiusY = MedRadiusY_;
            this.MaxRadiusX = MaxRadiusX_;
            this.MaxRadiusY = MedRadiusY_;
            kernelY = new float[3, 3] { { -1, -1, -1 }, { 0, 0, 0 }, { 1, 1, 1 } };
            kernel = new float[3, 3] { { -1, 0, 1 }, { -1, 0, 1 }, { -1, 0, 1 } };
        }
        override public Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            MedainFilter medainFilter = new MedainFilter();
            Bitmap  resultImage = medainFilter.processImage(sourceImage, worker);
            PruittFilter pruitt = new PruittFilter();
            Bitmap resultImageA = pruitt.processImage(resultImage, worker);
            MaximumFilter maximum = new MaximumFilter();
            return maximum.processImage(resultImageA, worker);

            for (int i = 0; i < sourceImage.Width; ++i)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }
            Bitmap resultImage2 = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; ++i)
            {
                worker.ReportProgress((int)((float)i / resultImage2.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage2.SetPixel(i, j, calculateNewPixelColorEdges(resultImage, i, j));
                }
            }
            Bitmap resultImage3 = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; ++i)
            {
                worker.ReportProgress((int)((float)i / resultImage2.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage3.SetPixel(i, j, calculateNewPixelColorMax(resultImage2, i, j));
                }
            }

            return resultImage3;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color_x_y = sourceImage.GetPixel(x, y);
            float[] arr_R = new float[(2 * MedRadiusX + 1) * (2 * MedRadiusY + 1)];
            float[] arr_G = new float[(2 * MedRadiusX + 1) * (2 * MedRadiusY + 1)];
            float[] arr_B = new float[(2 * MedRadiusX + 1) * (2 * MedRadiusY + 1)];

            for (int l = -MedRadiusY; l <= MedRadiusY; ++l)
                for (int k = -MedRadiusX; k <= MedRadiusX; ++k)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    arr_R[(l + MedRadiusY) * (2 * MedRadiusX + 1) + k + MedRadiusX] = neighborColor.R;
                    arr_G[(l + MedRadiusY) * (2 * MedRadiusX + 1) + k + MedRadiusX] = neighborColor.G;
                    arr_B[(l + MedRadiusY) * (2 * MedRadiusX + 1) + k + MedRadiusX] = neighborColor.B;
                }
            Array.Sort(arr_R);
            Array.Sort(arr_G);
            Array.Sort(arr_B);
            float resultR = arr_R[(2 * MedRadiusX + 1) * (2 * MedRadiusY + 1) / 2];
            float resultG = arr_G[(2 * MedRadiusX + 1) * (2 * MedRadiusY + 1) / 2];
            float resultB = arr_B[(2 * MedRadiusX + 1) * (2 * MedRadiusY + 1) / 2];
            return Color.FromArgb(
            Clamp((int)resultR, 0, 255),
            Clamp((int)resultG, 0, 255),
            Clamp((int)resultB, 0, 255)
            );
        }

        protected  Color calculateNewPixelColorEdges(Bitmap sourceImage, int x, int y)
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

        protected Color calculateNewPixelColorMax(Bitmap sourceImage, int x, int y)
        {
            Color color_x_y = sourceImage.GetPixel(x, y);

            int maxR = 0;
            int maxG = 0;
            int maxB = 0;
            for (int l = -MaxRadiusY; l <= MaxRadiusY; ++l)
                for (int k = -MaxRadiusX; k <= MaxRadiusX; ++k)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    if (neighborColor.R > maxR)
                        maxR = neighborColor.R;
                    if (neighborColor.G > maxG)
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

        int MedRadiusX;
        int MedRadiusY;

        int MaxRadiusX;
        int MaxRadiusY;

        protected float[,] kernel = null;
        protected float[,] kernelY = null;



    }
}
