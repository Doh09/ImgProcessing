using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Parallelprogrammeringseksamen
{
    /// <summary>
    /// This class loads in the colours from an image into a collection.
    /// This collection can then be used with LINQ and PLINQ to check speed differences.
    /// </summary>
    public class ColorLoader
    {
        public async Task<List<Color>> GetColourCollection_SequentialForLoop_ManyImages_UsingTasksAsync(List<string> BmpImgPaths)
        {
            List<Color> gatheredColours = new List<Color>();
            
            //var taskIndex = -1;
            List<Task<List<Color>>> taskList = new List<Task<List<Color>>>();
            //Task[] allTasks = new Task[BmpImgPaths.Count];
            for (int i = 0; i < BmpImgPaths.Count; i++)
            {
                Console.WriteLine("Starting task with "+ BmpImgPaths[i]);
                taskList.Add(GetColourCollection_UsingTaskAsync(BmpImgPaths[i]));
            }

            // Observe any exceptions that might have occurred.
            try
            {
                //var result = await Task.WhenAll(taskList).ConfigureAwait(false);
                await Task.WhenAll(taskList.ToArray()); //https://stackoverflow.com/questions/25319484/how-do-i-get-a-return-value-from-task-waitall-in-a-console-app
                                                        //From the link - "For best practice, use the new async way of doing things. Instead of
                                                        //Task.WaitAll use await Task.WhenAll"
                foreach (var t in taskList) //Add all the gathered colours to the same collection.
                {
                    gatheredColours.AddRange(t.Result);
                }
                Console.WriteLine("Done loading colours into collection so they can be worked with...");
            }
            catch (AggregateException ae)
            {
                Console.WriteLine(ae);
            }
            finally
            {
  
            }
            return gatheredColours;
        }

        public async Task<List<Color>> GetColourCollection_UsingTaskAsync(string BmpImgPath)
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();

            List<Color> colors = new List<Color>();

            Console.WriteLine();
            Bitmap bmp = new Bitmap(BmpImgPath);
            int width = bmp.Width;
            int height = bmp.Height;
            Console.WriteLine("Scheduled task - Loading img from path: " + BmpImgPath + " - Image size: " + width + "x" + height); //Comment out console writelines for faster parallel use.
            //Run code
            int newHeight = height;
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            Task myTask = Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++) //Man kan lave en cache optimering ved at tjekke om længde eller bredde er størst og så bytte rundt på loopsne. Men så skal man også bytte rundt på i og j da x skal stå først og y nr. 2.
                    {
                        token.ThrowIfCancellationRequested();
                        Color clr = bmp.GetPixel(i, j); // Get the color of pixel at position i,j
                        colors.Add(clr); // <-- crasher den den ikke er thread safe.
                    }
                }
            }, token);
            await myTask;
            sw.Stop();
            Console.WriteLine("Time taken to load colours sequentially in async task: " + sw.Elapsed + " - From path: " + BmpImgPath); //Comment out console writelines for faster parallel use.
            return colors;
        }

        public List<Color> GetColourCollection_SequentialForLoop_ManyImages(List<string> BmpImgPaths)
        {
            List<Color> gatheredColours = new List<Color>();

            //var taskIndex = -1;
            List<Task> taskList = new List<Task>();
            Task[] allTasks = new Task[BmpImgPaths.Count];
            for (int i = 0; i < BmpImgPaths.Count; i++)
            {
                Console.WriteLine("Loading image - " + BmpImgPaths[i]);
                gatheredColours.AddRange(GetColourCollection_SequentialForLoop(BmpImgPaths[i]));
            }
            return gatheredColours;
        }

            public List<Color> GetColourCollection_SequentialForLoop(string BmpImgPath)
        {
            List<Color> colors = new List<Color>();
            Console.WriteLine("Loading img from path: " + BmpImgPath);
            Bitmap bmp = new Bitmap(BmpImgPath);
            int width = bmp.Width;
            int height = bmp.Height;
            Console.WriteLine("Image size: " + width + "x" + height);
            Console.WriteLine("Loading colours into collection so they can be worked with...");

            Stopwatch sw = new Stopwatch();
            sw.Restart();
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
            //Console.WriteLine("Colours loaded, press >ENTER< to continue...");
            //Console.ReadLine();
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
            sw.Restart();
            Parallel.For(0, width, i => //width+1 as it's exclusive.
            {
                //Bitmap bmp;
                //lock (new object())
                //{
                Bitmap bmp = new Bitmap(BmpImgPath);
                //}
                //Run code
                //int newHeight = height;
                for (int j = 0; j < height; j++) //Man kan lave en cache optimering ved at tjekke om længde eller bredde er størst og så bytte rundt på loopsne. Men så skal man også bytte rundt på i og j da x skal stå først og y nr. 2.
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
