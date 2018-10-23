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
            //ImgProcessor imgp = new ImgProcessor();
            //imgp.Initialize(new ColorLoader());

            //var files = new List<string> { @"http://www.cs.tau.ac.il/~spike/images/ATM.bmp" };

            var files = new List<string>();
            string url = @"http://www.cs.tau.ac.il/~spike/images/";
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(url);
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                string s = link.ToString();
                Console.WriteLine(s);
            }

            //var files = Directory.GetFiles(
            //    @"..\..\..\ImagesToWorkOn", "*.bmp"
            //    );

            var imgProcessingPipeline = new ImgProcessingPipeline();
            imgProcessingPipeline.Initialize(files);

            Console.WriteLine("Finished... Hit >ENTER< to quit...");
            Console.ReadLine();
        }
    }
}
