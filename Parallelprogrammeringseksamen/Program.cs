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
            ImgProcessor imgp = new ImgProcessor();
            imgp.Initialize(new ColorLoader());



        }
    }
}
