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
        #region ImgPaths
        public string BmpImgPath { get; set; }
        public List<string> BmpImgPaths { get; set; }
        public List<string> BmpImgPaths2 { get; set; }
        public List<string> BmpImgPaths3 { get; set; }
        #endregion
        #region Dependency Injections
        private IColorLoader cl { get; set; }
        #endregion
        #region Results
        public delegate void Results(string s);
        public Results ImgProcessorResults;
        #endregion

        public void Initialize(IColorLoader cl) {
            this.cl = cl;
            SetUpImagePaths();
            var colors = LoadImage();
            ProcessImage(colors);
        }

        private void SetUpImagePaths()
        {
            BmpImgPath = @"TomAndJerry.bmp";
            BmpImgPaths = GetStringAsList(@"TomAndJerry.bmp", 9);
            BmpImgPaths2 = GetStringAsList(@"./ImagesToWorkOn/TheWorld.bmp", 9);
            BmpImgPaths3 = GetStringAsList(@"f153065664.bmp", 9);
        }

        private List<Color> LoadImage()
        {
            ColorLoader cl = new ColorLoader();
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            List<Color> multipleImagesColors = cl.GetColourCollection_SequentialForLoop_ManyImages_UsingTasksAsync(BmpImgPaths).Result;
            sw.Stop();
            string asyncSwElapsed = sw.Elapsed.ToString();
            Console.WriteLine("Multiple images - Async tasks - total time - " + asyncSwElapsed);
            sw.Restart();
            List<Color> multipleImagesColors2 = cl.GetColourCollection_SequentialForLoop_ManyImages(BmpImgPaths);
            sw.Stop();
            string sequentialSwElapsed = sw.Elapsed.ToString();
            //Colorful.Console.WriteAscii();

            //ImgProcessorResults = new Results(PrintAscii(parameter => parameter = ""));
            Console.WriteLine("RESULT *-*-*-*- Image loading results");
            Console.WriteLine("Load Multiple images - Async tasks - total time - " + asyncSwElapsed);
            Console.WriteLine("Load Multiple images - Sequential - total time - " + sequentialSwElapsed);
            Console.WriteLine("");

            return multipleImagesColors;
        }

        public void ProcessImage(List<Color> colorsToMapReduce)
        {
            var cpu = new CPU();
            cpu.Initialize(colorsToMapReduce, 2);
            Console.WriteLine("All results loaded, press >ENTER< twice to exit.");
            Console.ReadLine();
            Console.ReadLine();
        }

        public List<string> GetStringAsList(string stringToMultiply, int lengthOfList)
        {
            List<string> stringList = new List<string>();
            for (int i = 0; i < lengthOfList; i++)
            {
                stringList.Add(stringToMultiply);
            }
            return stringList;
        }

        void PrintAscii(string s) {
            Colorful.Console.WriteAscii(s);
        }

        void PrintLine(string s) {
            Console.WriteLine(s);
        }

    }
}
