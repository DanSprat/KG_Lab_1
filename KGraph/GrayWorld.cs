using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGraph
{

    class GrayWorld : Filters
    {
        double R;
        double G;
        double B;
        double avg;

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color current = sourceImage.GetPixel(x, y);
            int newR = (int)(current.R * avg / R);
            int newG = (int)(current.G * avg / G);
            int newB = (int)(current.B * avg / B);
            return Color.FromArgb(Clamp(newR,0,255),Clamp(newG,0,255),Clamp(newB, 0, 255));
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; i++) {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++) {
                    Color current = sourceImage.GetPixel(i, j);
                    R += current.R;
                    G += current.G;
                    B += current.B;
                }
            }
            avg = (R + G + B) / 3;


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
            return resultImage;
        }
    }
}
