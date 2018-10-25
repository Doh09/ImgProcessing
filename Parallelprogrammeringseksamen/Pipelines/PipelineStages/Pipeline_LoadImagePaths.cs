using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Parallelprogrammeringseksamen.Pipelines.PipelineStages
{
    public class Pipeline_LoadImagePaths
    {
        public enum CallingMode { LocalDirectory, Online }

        public void LoadImagePaths(CallingMode callingMode, BlockingCollection<string> output, string pathOrURL = "")
        {
            var files = new string[0];
            switch (callingMode)
            {
                case CallingMode.LocalDirectory:
                    if (pathOrURL.Equals("")) //Use default filepath
                        files = FilePathsFromDirectory();
                    else //Use given filepath
                        files = FilePathsFromDirectory(pathOrURL);
                    break;
                case CallingMode.Online:
                    if (pathOrURL.Equals("")) //Use default URL
                        files = FilePathsFromURL().ToArray();
                    else //Use given URL
                        files = FilePathsFromURL(pathOrURL).ToArray();
                    break;
            }
            LoadImagePathsIntoBuffer(files, output);
        }

        private List<string> FilePathsFromURL(string url = @"http://www.cs.tau.ac.il/~spike/images/")
        {
            var files = new List<string>(); //https://html-agility-pack.net/knowledge-base/7760286/how-to-extract-full-url-with-htmlagilitypack---csharp
            HtmlWeb hw = new HtmlWeb();
            HtmlDocument doc = hw.Load(url);
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//a[@href]"))
            {
                //For each link
                string onlineRelativeImgPath = "";
                foreach (var a in link.Attributes["href"].Value) //If there is a href value, add the whole path of that href value to the string.
                {
                    onlineRelativeImgPath += a;
                } //if the full link path ends with .bmp, save it to the file path collection as it will be an image.
                if (onlineRelativeImgPath.EndsWith(".bmp"))
                    files.Add(url + onlineRelativeImgPath);
            }
            return files;
        }

        private string[] FilePathsFromDirectory(string directoryPath = @"..\..\..\ImagesToWorkOn")
        {
            var files = Directory.GetFiles(
                directoryPath, "*.bmp"
                );
            return files;
        }

        private void LoadImagePathsIntoBuffer(IEnumerable<string> input, BlockingCollection<string> output)
        {
            try
            {
                foreach (var filePath in input)
                {
                    Console.WriteLine("Adding img filepath to buffer: " + filePath);
                    output.Add(filePath);
                }
            }
            finally
            {
                output.CompleteAdding();
            }
        }
    }
}
