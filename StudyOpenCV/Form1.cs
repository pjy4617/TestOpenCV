using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;

namespace StudyOpenCV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Mat src = new Mat("../../lenna.png", ImreadModes.Grayscale);
            Mat dst = new Mat();
            Cv2.Canny(src, dst, 50, 200);
            using(new Window("src image", src))
            using(new Window("dst image", dst))
            {
                Cv2.WaitKey();
            }
        }
    }
}
