using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGraph
{
    class Opening : MatMorph
    {
        public Opening(bool[,] Mstruct, float bord)
        {
            structElem = Mstruct;
            border = bord;
        }
        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Erosion erosion = new Erosion(structElem,border);
            Bitmap res1 = erosion.processImage(sourceImage,worker);
            Dilation dilation = new Dilation(structElem,border);
            Bitmap res2 = dilation.processImage(res1, worker);
            return res2;
        }
    }
}
