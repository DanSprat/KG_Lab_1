using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KGraph
{
    class Dilation : MatMorph
    {
        public Dilation(bool[,] Mstruct, float bord)
        {
            structElem = Mstruct;
            border = bord;
        }
        protected override bool calculateNewPixelColor(bool[,] sourceImage, int x, int y)
        {
            int MH = structElem.GetLength(0) / 2;
            int MW = structElem.GetLength(1) / 2;
            int i, j;
            for (j = -MH ; j <= MH ; j++)
                for (i = -MW ; i <= MW ; i++)
                    if ((structElem[i+MH,j+MW]) && (sourceImage[x + i,y + j] ))
                    {
                        return true;
                    }
            return false;
        }
    }
}
