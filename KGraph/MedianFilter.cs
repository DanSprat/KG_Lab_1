using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace KGraph
{
    class MedainFilter : Filters
    {
        int radiusX;
        int radiusY;

        public MedainFilter(int radiusX_ = 1, int radiusY_ = 1)
        {
            radiusX = radiusX_;
            radiusY = radiusY_;
        }
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color color_x_y = sourceImage.GetPixel(x, y);
            float[] arr_R = new float[(2 * radiusX + 1) * (2 * radiusY + 1)];
            float[] arr_G = new float[(2 * radiusX + 1) * (2 * radiusY + 1)];
            float[] arr_B = new float[(2 * radiusX + 1) * (2 * radiusY + 1)];

            for (int l = -radiusY; l <= radiusY; ++l)
                for (int k = -radiusX; k <= radiusX; ++k)
                {
                    int idX = Clamp(x + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourceImage.Height - 1);
                    Color neighborColor = sourceImage.GetPixel(idX, idY);
                    arr_R[(l + radiusY) * (2 * radiusX + 1) + k + radiusX] = neighborColor.R;
                    arr_G[(l + radiusY) * (2 * radiusX + 1) + k + radiusX] = neighborColor.G;
                    arr_B[(l + radiusY) * (2 * radiusX + 1) + k + radiusX] = neighborColor.B;
                }
            Array.Sort(arr_R);
            Array.Sort(arr_G);
            Array.Sort(arr_B);
            float resultR = arr_R[(2 * radiusX + 1) * (2 * radiusY + 1) / 2];
            float resultG = arr_G[(2 * radiusX + 1) * (2 * radiusY + 1) / 2];
            float resultB = arr_B[(2 * radiusX + 1) * (2 * radiusY + 1) / 2];
            return Color.FromArgb(
            Clamp((int)resultR, 0, 255),
            Clamp((int)resultG, 0, 255),
            Clamp((int)resultB, 0, 255)
            );
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; ++i)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; ++j)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }
            return resultImage;
        }
    }

}
