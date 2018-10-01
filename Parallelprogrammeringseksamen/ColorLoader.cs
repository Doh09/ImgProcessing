using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Parallelprogrammeringseksamen
{
    /// <summary>
    /// This class loads in the colours from an image into a collection.
    /// This collection can then be used with LINQ and PLINQ to check speed differences.
    /// </summary>
    public class ColorLoader
    {

        public ConcurrentBag<Color> GetColourCollection_SequentialForLoop(string BmpImgPath)
        {
            ConcurrentBag<Color> colors = new ConcurrentBag<Color>();
            Console.WriteLine("Loading img from path: " + BmpImgPath);
            Bitmap bmp = new Bitmap(BmpImgPath);
            int width = bmp.Width;
            int height = bmp.Height;
            Console.WriteLine("Image size: " + width + "x" + height);
            Console.WriteLine("Loading colours into collection so they can be worked with...");

            Stopwatch sw = new Stopwatch();
            sw.Start();
            //Run code
            int newHeight = height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++) //Man kan lave en cache optimering ved at tjekke om længde eller bredde er størst og så bytte rundt på loopsne. Men så skal man også bytte rundt på i og j da x skal stå først og y nr. 2.
                {
                    Color clr = bmp.GetPixel(i, j); // Get the color of pixel at position i,j
                    colors.Add(clr); // <-- crasher den den ikke er thread safe.
                }
            }
            sw.Stop();
            Console.WriteLine("Time taken to load colours sequential: " + sw.Elapsed);
            Console.WriteLine("Colours loaded, press >ENTER< to continue...");
            Console.ReadLine();
            return colors;
        }

        public ConcurrentBag<Color> GetColourCollection_ParallelForLoop(string BmpImgPath)
        {
            ConcurrentBag<Color> colors = new ConcurrentBag<Color>();
            Console.WriteLine("Loading img from path: " + BmpImgPath);
            Bitmap img = new Bitmap(BmpImgPath);
            int width = img.Width;
            int height = img.Height;
            Console.WriteLine("Image size: " + width + "x" + height);
            Console.WriteLine("Loading colours into collection so they can be worked with...");

            Stopwatch sw = new Stopwatch();
            sw.Start();
            Parallel.For(0, width, i => //width+1 as it's exclusive.
            {
                Bitmap bmp;
                lock (new object())
                {
                    bmp = new Bitmap(BmpImgPath);
                }
                //Run code
                int newHeight = height;
                for (int j = 0; j < newHeight; j++) //Man kan lave en cache optimering ved at tjekke om længde eller bredde er størst og så bytte rundt på loopsne. Men så skal man også bytte rundt på i og j da x skal stå først og y nr. 2.
                {
                    Color clr = bmp.GetPixel(i, j); // Get the color of pixel at position i,j
                    colors.Add(clr); // <-- crasher den den ikke er thread safe.
                }
            });
            sw.Stop();
            Console.WriteLine("Time taken to load colours parallel: " + sw.Elapsed);
            Console.WriteLine("Colours loaded, press >ENTER< to continue...");
            Console.ReadLine();
            return colors;
        }
    }
}
