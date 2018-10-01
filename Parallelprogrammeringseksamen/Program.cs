using System;

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
            ImgProcessor imgp = new ImgProcessor();
            imgp.ProcessImage();
        }
    }
}
