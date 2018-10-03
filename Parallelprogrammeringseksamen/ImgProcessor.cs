using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Parallelprogrammeringseksamen
{
    /// <summary>
    /// This class is the main ImgProcessing class from where we call the other classes, 
    /// it is so to speak the entry point to using the application.
    /// </summary>
    public class ImgProcessor
    {
        public string BmpImgPath { get; set; } = @"f153065664.bmp";
        public Bitmap Img { get; set; }


        public void ProcessImage()
        {
            ColorLoader cl = new ColorLoader();
            //colorsSequential
            ConcurrentBag<Color> colorsSequential = cl.GetColourCollection_SequentialForLoop(BmpImgPath);
            //colorParallel
            ConcurrentBag<Color> colorParallel = cl.GetColourCollection_ParallelForLoop(BmpImgPath);
            

        }

        public void ProcessImg_LINQ()
        {

        }

    }
}
