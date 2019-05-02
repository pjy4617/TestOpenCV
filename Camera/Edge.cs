using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace Camera
{
    public class Edge : IDisposable
    {
        IplImage canny;

        public IplImage CannyEdge(IplImage src)
        {
            canny = new IplImage(src.Size, BitDepth.U8, 1);
            Cv.Canny(src, canny, 100, 200);
            return canny;
        }

        public void Dispose()
        {
            if (canny != null) Cv.ReleaseImage(canny);
        }

    }
}
