﻿using System;
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
        public string BmpImgPath { get; set; }
        public List<string> BmpImgPaths { get; set; }
        public List<string> BmpImgPaths2 { get; set; }
        public List<string> BmpImgPaths3 { get; set; }
        public Bitmap Img { get; set; }


        public void ProcessImage()
        {
            BmpImgPath = @"TomAndJerry.bmp";
            BmpImgPaths = GetStringAsList(@"TomAndJerry.bmp", 9);
            BmpImgPaths2 = GetStringAsList(@"./ImagesToWorkOn/TheWorld.bmp", 9);
            BmpImgPaths3 = GetStringAsList(@"f153065664.bmp", 9);
            
            ColorLoader cl = new ColorLoader();
            //MultipleImages using tasks
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
            Console.WriteLine("Multiple images - Sequential - total time - " + sequentialSwElapsed);
            Console.WriteLine("****************************************************");
            Console.WriteLine("******************* - RESULT - *********************");
            Console.WriteLine("****************************************************");
            Console.WriteLine("Load Multiple images - Async tasks - total time - " + asyncSwElapsed);
            Console.WriteLine("Load Multiple images - Sequential - total time - " + sequentialSwElapsed);
            Console.WriteLine("****************************************************");
            Console.WriteLine("****************************************************");
            //ConcurrentBag<Color> multipleImagesColors = cl.GetColourCollection_SequentialForLoop_ManyImages(BmpImgPaths2);
            //colorsSequential
            //ConcurrentBag<Color> colorsSequential = cl.GetColourCollection_SequentialForLoop(BmpImgPath);
            //colorParallel
            //ConcurrentBag<Color> colorParallel = cl.GetColourCollection_ParallelForLoop(BmpImgPath);
            var cpu = new CPU();
            cpu.Initialize(multipleImagesColors, 2);

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

        public void ProcessImg_LINQ()
        {

        }

    }
}
