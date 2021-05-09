using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace KGraph
{
    public partial class Form1 : Form
    {
        Bitmap image;

        public Form1()
        {
            InitializeComponent();
        }

     
        private void фильтрToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void точечныеToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InvertFilter filter = new InvertFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.InitialDirectory = "C:\\Users\\Work\\Pictures\\KG1";
            dialog.Filter = "Image files|*.png;*.jpeg;*.jpg;*.bmp|All files(*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK) {
                image = new Bitmap(dialog.FileName);
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap newImage = ((Filters)e.Argument).processImage(image, backgroundWorker1);
            if (backgroundWorker1.CancellationPending != true)
                image = newImage;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled) {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
            progressBar1.Value = 0;
        }

        private void размытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BlurFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void гауссToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GaussianFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void полутонToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters fillter = new GrayScaleFilter();
            backgroundWorker1.RunWorkerAsync(fillter);
        }

        private void сепияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Sepia();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void тиснениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new StampingFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void медианныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new MedainFilter();
            backgroundWorker1.RunWorkerAsync(filter);

        }

        private void выделениеКраевToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new PruittFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void максимумToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new MaximumFilter();
            backgroundWorker1.RunWorkerAsync(filter);

        }

        private void светящиесяКраяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new LightingEdges(1,1,1,1);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void переносToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter =new Move(-200,100);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void поворотToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new Rotate(300, 422, Math.PI / 4);
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void серыйМирToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new GrayWorld();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        private void линейноеРастяжениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters fiilter = new LinearStretching();
            backgroundWorker1.RunWorkerAsync(fiilter);
        }

        private void расширениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[,] stru = new bool[3,3] { { true, true, true }, { true, true, true }, { true, true, true } };
            Filters fiilter = new Dilation(stru,50);
            backgroundWorker1.RunWorkerAsync(fiilter);

        }

        private void сужениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[,] stru = new bool[3, 3] { { true, true, true }, { true, true, true }, { true, true, true } };
            Filters fiilter = new Erosion(stru, 12);
            backgroundWorker1.RunWorkerAsync(fiilter);
        }

        private void открытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[,] stru = new bool[3, 3] { { true, true, true }, { true, true, true }, { true, true, true } };
            Filters fiilter = new Opening(stru, 12);
            backgroundWorker1.RunWorkerAsync(fiilter);
        }

        private void закрытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[,] stru = new bool[3, 3] { { true, true, true }, { true, true, true }, { true, true, true } };
            Filters fiilter = new Closing(stru, 12);
            backgroundWorker1.RunWorkerAsync(fiilter);
        }

       

        private void gradToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool[,] stru = new bool[3, 3] { { true, true, true }, { true, true, true }, { true, true, true } };
            Filters fiilter = new Grad(stru, 152);
            backgroundWorker1.RunWorkerAsync(fiilter);
        }
    }
}
