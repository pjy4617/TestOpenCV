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
using OpenCvSharp.Extensions;

namespace Camera
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            comboBox1.DataSource = Enum.GetValues(typeof(ThresholdType));
            
        }
        private void DrawImage()
        {
            string solutionPath = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
            solutionPath += "\\Image\\ROIEdgeHor.bmp";
            using (IplImage src = Cv.LoadImage(solutionPath, LoadMode.AnyColor))
            {
                pictureBoxIpl1.ImageIpl = src;
                pictureBoxIpl1.SizeMode = PictureBoxSizeMode.StretchImage;

//                 IplImage src = new IplImage(src1.Size, BitDepth.U8, 3);
//                 double gamma_value = 2.0;
//                 byte[] lut = new byte[256];
//                 for(int i = 0; i < lut.Length; i++)
//                 {
//                     lut[i] = (byte)(Math.Pow(i / 255.0, 1.0 / gamma_value) * 255.0);
//                 }
//                 Cv.LUT(src1, src, lut);

//                 pictureBoxIpl2.ImageIpl = src;
//                 pictureBoxIpl2.SizeMode = PictureBoxSizeMode.StretchImage;

                IplImage bin = new IplImage(src.Size, BitDepth.U8, 1);
                Cv.CvtColor(src, bin, ColorConversion.RgbToGray);
                Cv.Smooth(bin, bin, SmoothType.Gaussian);
                ThresholdType thresholdType = (ThresholdType)comboBox1.SelectedValue;
                Cv.Threshold(bin, bin, trackBar1.Value, 255, thresholdType);
                
                pictureBoxIpl2.ImageIpl = bin;
                pictureBoxIpl2.SizeMode = PictureBoxSizeMode.StretchImage;

                IplImage dil = new IplImage(src.Size, BitDepth.U8, 1);
                IplConvKernel element = new IplConvKernel(4, 4, 2, 2, ElementShape.Custom, new int[3, 3]);
                Cv.Erode(bin, dil, element, 1);

                pictureBoxIpl3.ImageIpl = dil;
                pictureBoxIpl3.SizeMode = PictureBoxSizeMode.StretchImage;







                IplImage con = new IplImage(src.Size, BitDepth.U8, 3);
                Cv.Copy(src, con);

                CvMemStorage storage = new CvMemStorage();
                CvSeq<CvPoint> contours;
                Cv.FindContours(dil, storage, out contours, CvContour.SizeOf, ContourRetrieval.List, ContourChain.ApproxSimple);
                try
                {
                    Cv.DrawContours(con, contours, CvColor.Red, CvColor.Red, 1, 1, LineType.AntiAlias);
                    Cv.ClearSeq(contours);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                
                
                
                Cv.ReleaseMemStorage(storage);

                pictureBoxIpl4.ImageIpl = con;
                pictureBoxIpl4.SizeMode = PictureBoxSizeMode.StretchImage;

                if (dil != null) Cv.ReleaseImage(bin);
                if (bin != null) Cv.ReleaseImage(bin);
                if (con != null) Cv.ReleaseImage(con);
            }
        }
        private void HoughLine()
        {
            using (IplImage src = new IplImage("ROIEdgeHor.bmp", LoadMode.AnyColor))
            {
                pictureBoxIpl1.ImageIpl = src;
                pictureBoxIpl1.SizeMode = PictureBoxSizeMode.StretchImage;

                IplImage bin = new IplImage(src.Size, BitDepth.U8, 1);
                Cv.CvtColor(src, bin, ColorConversion.RgbToGray);
                Cv.Smooth(bin, bin, SmoothType.Gaussian);
                ThresholdType thresholdType = (ThresholdType)comboBox1.SelectedValue;
                Cv.Threshold(bin, bin, trackBar1.Value, 255, thresholdType);
                

                pictureBoxIpl2.ImageIpl = bin;
                pictureBoxIpl2.SizeMode = PictureBoxSizeMode.StretchImage;


                IplImage canny = new IplImage(src.Size, BitDepth.U8, 1);
                Cv.Canny(bin, canny, trackBar2.Value, trackBar3.Value);

                IplImage houline = new IplImage(src.Size, BitDepth.U8, 3);
                Cv.CvtColor(canny, houline, ColorConversion.GrayToBgr);

                CvMemStorage storage = new CvMemStorage();
                CvSeq lines = canny.HoughLines2(storage, HoughLinesMethod.Standard, 1, Math.PI / 180, 50, 0, 0);

                for(int i=0;i<Math.Min(lines.Total, trackBar4.Value); i++)
                {
                    CvLineSegmentPolar element = lines.GetSeqElem<CvLineSegmentPolar>(i).Value;
                    float r = element.Rho;
                    float theta = element.Theta;
                    double a = Math.Cos(theta);
                    double b = Math.Sin(theta);
                    double x0 = r * a;
                    double y0 = r * b;
                    int scale = src.Size.Width + src.Size.Height;
                    CvPoint pt1 = new CvPoint(Convert.ToInt32(x0 - scale * b), Convert.ToInt32(y0 + scale * a));
                    CvPoint pt2 = new CvPoint(Convert.ToInt32(x0 + scale * b), Convert.ToInt32(y0 - scale * a));

                    houline.Circle(new CvPoint((int)x0, (int)y0), 2, CvColor.Yellow, -1);
                    houline.Line(pt1, pt2, CvColor.Red, 1, LineType.AntiAlias);
                }

                pictureBoxIpl3.ImageIpl = houline;
                pictureBoxIpl3.SizeMode = PictureBoxSizeMode.StretchImage;

                Cv.ReleaseMemStorage(storage);
                Cv.ClearSeq(lines);
                if (bin != null) Cv.ReleaseImage(bin);
                if (houline != null) Cv.ReleaseImage(houline);
                if (canny != null) Cv.ReleaseImage(canny);
            }
        }
        private void BinarizerMethod()
        {
            using (IplImage src = new IplImage("ROIEdgeHor.bmp", LoadMode.AnyColor))
            {
                pictureBoxIpl1.ImageIpl = src;
                pictureBoxIpl1.SizeMode = PictureBoxSizeMode.StretchImage;

                IplImage bin = new IplImage(src.Size, BitDepth.U8, 1);
                Cv.CvtColor(src, bin, ColorConversion.RgbToGray);
                Cv.Smooth(bin, bin, SmoothType.Gaussian);
                ThresholdType thresholdType = (ThresholdType)comboBox1.SelectedValue;
                Cv.Threshold(bin, bin, trackBar1.Value, 255, thresholdType);

                pictureBoxIpl2.ImageIpl = bin;
                pictureBoxIpl2.SizeMode = PictureBoxSizeMode.StretchImage;

                IplImage bina = new IplImage(src.Size, BitDepth.U8, 1);
//                 Binarizer.Nick(bin, bina, 61, 0.3);
//                 pictureBoxIpl3.ImageIpl = bina;
//                 pictureBoxIpl3.SizeMode = PictureBoxSizeMode.StretchImage;
                Binarizer.Sauvola(bin, bina, 77, 0.2, 64);
                pictureBoxIpl4.ImageIpl = bina;
                pictureBoxIpl4.SizeMode = PictureBoxSizeMode.StretchImage;
                Binarizer.Niblack(bin, bina, 61, -0.5);
                pictureBoxIpl5.ImageIpl = bina;
                pictureBoxIpl5.SizeMode = PictureBoxSizeMode.StretchImage;




                if (bina != null) Cv.ReleaseImage(bina);
                if (bin != null) Cv.ReleaseImage(bin);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DrawImage();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawImage();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            DrawImage();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            DrawImage();
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            DrawImage();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            DrawImage();
        }
    }
}
