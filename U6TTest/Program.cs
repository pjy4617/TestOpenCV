using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;

namespace U6TTest
{
    class Program
    {
        static void Main(string[] args)
        {
            VideoCapture capture = new VideoCapture(3);
            Mat frame = new Mat();
            capture.Set(CaptureProperty.FrameWidth, 1920);
            capture.Set(CaptureProperty.FrameHeight, 1080);
            while(true)
            {
                if (capture.IsOpened())
                {
                    capture.Read(frame);
                    frame.SaveImage("c:\\test.bmp");
                    Cv2.ImShow("frame", frame);
                    if (Cv2.WaitKey(33) == 'q') break;
                }
                
            }
            capture.Release();
            Cv2.DestroyAllWindows();
        }
    }
}
