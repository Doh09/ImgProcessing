using HtmlAgilityPack;
using Parallelprogrammeringseksamen.Pipelines;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

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


            var imgProcessingPipeline = new ImgProcessingPipeline();
            imgProcessingPipeline.Initialize();

            Console.WriteLine("Finished... Hit >ENTER< to quit...");
            Console.ReadLine();
        }
    }
}
