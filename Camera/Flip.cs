using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace Camera
{
    public class Flip : IDisposable
    {
        IplImage img;
        public IplImage FlipX(IplImage src)
        {
            img = new IplImage(src.Size, BitDepth.U8, 3);
            Cv.Flip(src, img, FlipMode.X);
            return img;
        }
        public IplImage FlipY(IplImage src)
        {
            img = new IplImage(src.Size, BitDepth.U8, 3);
            Cv.Flip(src, img, FlipMode.Y);
            return img;
        }
        public IplImage FlipXY(IplImage src)
        {
            img = new IplImage(src.Size, BitDepth.U8, 3);
            Cv.Flip(src, img, FlipMode.XY);
            return img;
        }
        public void Dispose()
        {
            if (img != null) Cv.ReleaseImage(img);
        }
    }
}
