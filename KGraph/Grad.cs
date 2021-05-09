using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace KGraph
{
    class Grad : MatMorph
    {
        public Grad(bool[,] structureElement, float threshold = 0.5f)
        {
            this.structElem = structureElement;
            this.border = threshold;
        }

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Erosion erosion = new Erosion(structElem, border);
            Dilation dilation = new Dilation(structElem, border);
            Bitmap img1 = erosion.processImage(sourceImage, worker);
            Bitmap img2 = dilation.processImage(sourceImage, worker);
            Bitmap result = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; ++i)
            {
                for (int j = 0; j < sourceImage.Height; ++j)
                {
                    Color clr1 = img1.GetPixel(i, j);
                    Color clr2 = img2.GetPixel(i, j);
                    Color c = Color.FromArgb(Clamp(clr1.R - clr2.R, 0, 255), Clamp(clr1.G - clr2.G, 0, 255), Clamp(clr1.B - clr2.B, 0, 255));
                    result.SetPixel(i, j, c);
                }
            }
            return result;

        }


    }

}
