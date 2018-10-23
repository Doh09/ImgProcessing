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
            imgProcessingPipeline.Initialize(FilePathsFromURL());

            Console.WriteLine("Finished... Hit >ENTER< to quit...");
            Console.ReadLine();
        }

        public static IList<string> FilePathsFromURL(string url = @"http://www.cs.tau.ac.il/~spike/images/") {
            var files = new List<string>(); //https://html-agility-pack.net/knowledge-base/7760286/how-to-extract-full-url-with-htmlagilitypack---csharp
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(url);
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                string onlineRelativeImgPath = "";
                foreach (var a in link.Attributes["href"].Value)
                {
                    onlineRelativeImgPath += a;
                }
                if (onlineRelativeImgPath.EndsWith(".bmp"))
                    files.Add(url + onlineRelativeImgPath);
            }
            return files;
        }

        public static string[] FilePathsFromDirectory(string directoryPath = @"..\..\..\ImagesToWorkOn") {
            var files = Directory.GetFiles(
                directoryPath, "*.bmp"
                );
            return files;
        }
    }
}
