using System;
using System.Diagnostics;
using System.IO;

namespace Parallelprogrammeringseksamen
{
    /// <summary>
    /// Starts the program.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting Image Processor...");
            //ImgProcessor imgp = new ImgProcessor();
            //imgp.Initialize(new ColorLoader());

            var imgProcessorRedux = new ImgProcessorRedux();
            var files = Directory.GetFiles(
                @"C:\Users\marti\source\repos\ImgProcessing\Parallelprogrammeringseksamen\ImagesToWorkOn", "*.bmp");


            imgProcessorRedux.Initialize(new ColorLoader(), files);


            Console.ReadLine();
        }
    }
}
