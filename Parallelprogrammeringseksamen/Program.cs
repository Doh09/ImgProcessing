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

            #region pipeline
            var imgProcessingPipeline = new ImgProcessingPipeline();
            imgProcessingPipeline.Initialize();
            #endregion
            #region non-pipeline
            //ImgProcessor imgp = new ImgProcessor();
            //imgp.Initialize(new ColorLoader());
            #endregion
            Console.WriteLine("Finished... Hit >ENTER< to quit...");
            Console.ReadLine();
        }
    }
}
