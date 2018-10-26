using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Parallelprogrammeringseksamen.Pipelines.PipelineStages
{
    public class Pipeline_LoadImagePaths
    {
        public enum CallingMode { LocalDirectory, Online }
        List<string> extensions = new List<string> { ".jpg", ".bmp", ".png" };


        public void LoadImagePaths(CallingMode callingMode, BlockingCollection<string> output, string pathOrURL = "")
        {
            var files = new string[0];
            switch (callingMode)
            {
                case CallingMode.LocalDirectory:
                    if (pathOrURL.Equals("")) //Use default filepath
                        files = FilePathsFromDirectory().ToArray();
                    else //Use given filepath
                        files = FilePathsFromDirectory(pathOrURL).ToArray();
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
                string onlineRelativeImgPath = "";
                foreach (var a in link.Attributes["href"].Value)
                {
                    onlineRelativeImgPath += a;
                }

                bool exts = extensions.Exists(x => onlineRelativeImgPath.EndsWith(x));
                if (exts == true)
                {
                    files.Add(url + onlineRelativeImgPath);
                }


            }
            return files;
        }


        private IEnumerable<string> FilePathsFromDirectory(string directoryPath = @"O:\Billeder\Fines Billeder\101NIKON")
        {
            var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories).Where(s => extensions.Contains(Path.GetExtension(s).ToLower()));
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
