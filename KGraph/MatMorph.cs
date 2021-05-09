using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGraph
{
  
    class MatMorph : Filters
    {
        public MatMorph(bool [,] Mstruct,float bord){
            structElem = Mstruct;
            border = bord;
        }

        protected MatMorph() { 
        }

        protected bool[,] structElem;
        protected float border;
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            throw new NotImplementedException();
        }

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            // Перевод в полутон
            bool[,] binImage =new bool [sourceImage.Width,sourceImage.Height];
            GrayScaleFilter GSF = new GrayScaleFilter();
            Bitmap resultImage = GSF.processImage(sourceImage, worker);
            for (int i = 0; i < resultImage.Width; i++) {
                for (int j = 0; j < resultImage.Height; j++) {
                    Color current =resultImage.GetPixel(i, j);
                    if (current.R < border)
                    {
                        binImage[i, j] = true;
                    }
                    else {
                        binImage[i, j] = false;
                    }
                }
            }
            Bitmap resultImage1 = new Bitmap(resultImage);
            int widthX = structElem.GetLength(0) / 2;
            int widthY = structElem.GetLength(1) / 2;
            for (int i = widthX; i < sourceImage.Width-widthX; ++i)
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));
                if (worker.CancellationPending)
                    return null;
                for (int j = widthY; j < sourceImage.Height-widthY; j++)
                {
                    bool current = calculateNewPixelColor(binImage, i, j);
                    if (current == true)
                        resultImage1.SetPixel(i, j, Color.Black);
                    else
                        resultImage1.SetPixel(i, j, Color.White);
                }
            }
            Color color = resultImage.GetPixel(sourceImage.Width / 2, sourceImage.Height-1);
            return resultImage1;
        }




        virtual protected bool  calculateNewPixelColor(bool[,] sourceImage,int x,int y) {
            return false;
        }
      
    }
}
