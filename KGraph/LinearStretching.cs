using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGraph
{
    class LinearStretching : Filters
    {
        int yMaxR;
        int yMinR;

        int yMaxG;
        int yMinG;

        int yMaxB;
        int yMinB;

        public LinearStretching() {
            yMaxB = 0;
            yMaxG = 0;
            yMaxR = 0;

            yMinB = 255;
            yMinG = 255;
            yMinR = 255;
        }
        public double func(int y,int yMax,int yMin) {
            double x = ((double)255 / (yMax - yMin)) * (y - yMin);
            return x;
        } 
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    Color current = sourceImage.GetPixel(i, j);
                    if (current.R > yMaxR)
                        yMaxR = current.R;
                    if (current.R < yMinR)
                        yMinR = current.R;

                    if (current.G > yMaxG)
                        yMaxG = current.G;
                    if (current.G < yMinG)
                        yMinG = current.G;

                    if (current.B > yMaxB)
                        yMaxB = current.B;
                    if (current.B < yMinB)
                        yMinB = current.B;
                }
            }

            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }
            return resultImage;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color current = sourceImage.GetPixel(x, y);
            int R = (int)func(current.R, yMaxR, yMinR);
            int G = (int)func(current.G, yMaxG, yMinG);
            int B = (int)func(current.B, yMaxB, yMinB);
            return Color.FromArgb(R, G, B);
        }
    }
}
