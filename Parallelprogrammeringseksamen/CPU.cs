using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Parallelprogrammeringseksamen
{
    /// <summary>
    /// This class will hold all the LINQ and PLINQ experiments made on the CPU.
    /// </summary>
    public class CPU
    {
        public void Initialize(List<Color> colors, int multiplicationFactor = 0)
        {
            for (int i = 0; i < multiplicationFactor; i++)
            {
                colors.AddRange(colors);
            }
            ProcessImg_MapReduce_PLINQ(colors);
            ProcessImg_MapReduce_LINQ(colors);
            Console.ReadLine();
        }

        public void ProcessImg_MapReduce_PLINQ(IEnumerable<Color> colors)
        {
            //colors.AsParallel()
            //    .GroupBy(c => c)
            //    .Select(colourGroup => new IDMultiSetItem(
            //        colorGroup., colorGroup.Count()));
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            var candidates =
                colors
                    .AsParallel()
                    .GroupBy(color => color)
                    .Select(
                        intermediate => new
                        {
                            ColorKey = intermediate.Key,
                            Frequency = intermediate.Sum(c => 1)
                        })
                    .OrderBy(c => c.Frequency);
            sw.Stop();

            Console.WriteLine($"Parallel CPU: {sw.Elapsed}");

            //foreach (var result in candidates)
            //{
            //    var localR = result.ColorKey.R;
            //    var localG = result.ColorKey.G;
            //    var localB = result.ColorKey.B;

            //    Console.WriteLine(
            //        $"Red: {localR} Green: {localG} Blue: {localB} Appeared: {result.Frequency}"); //FromArgb(result.ColorKey.R, result.ColorKey.G, result.ColorKey.B));
            //                                                 //Console.WriteLine($"Red: {result.ColorKey.R} Green: {result.ColorKey.G} Blue: {result.ColorKey.B} Appeared: {result.Frequency}");

            //}

            //Console.ReadLine();


            //.Select(foafGroup => new IDMultisetItem(foafGroup.Key,
            //    foafGroup.Count()));
            //return Multiset.MostNumerous(candidates, maxCandidates);
        }


        public void ProcessImg_MapReduce_LINQ(IEnumerable<Color> colors)
        {
            //colors.AsParallel()
            //    .GroupBy(c => c)
            //    .Select(colourGroup => new IDMultiSetItem(
            //        colorGroup., colorGroup.Count()));
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            var candidates =
                colors
                    .GroupBy(color => color)
                    .Select(
                        intermediate => new ColorFrequency
                        {
                            ColorKey = intermediate.Key,
                            Frequency = intermediate.Sum(c => 1)
                        })
                    .OrderBy(c => c.Frequency);
            sw.Stop();

            Console.WriteLine($"Sequential CPU: {sw.Elapsed}");

            var candidatesList = candidates.ToList();
            PrintListWithColours(candidatesList);
            //foreach (var result in candidates)
            //{
            //    var localR = result.ColorKey.R;
            //    var localG = result.ColorKey.G;
            //    var localB = result.ColorKey.B;

            //    Console.WriteLine(
            //        $"Red: {localR} Green: {localG} Blue: {localB} Appeared: {result.Frequency}"); //FromArgb(result.ColorKey.R, result.ColorKey.G, result.ColorKey.B));
            //                                                 //Console.WriteLine($"Red: {result.ColorKey.R} Green: {result.ColorKey.G} Blue: {result.ColorKey.B} Appeared: {result.Frequency}");

            //}

             Console.ReadLine();


            //.Select(foafGroup => new IDMultisetItem(foafGroup.Key,
            //    foafGroup.Count()));
            //return Multiset.MostNumerous(candidates, maxCandidates);
        }

        private void PrintListWithColours(List<ColorFrequency> colorFrequencies, int topCandidatesToPrint = 10)
        {
            for (int i = topCandidatesToPrint; i > 0; i--)
            {
                Thread.Sleep(100);
                Color c = colorFrequencies[colorFrequencies.Count - i].ColorKey; //The color used for the text in the console.
                Color colorInfo = colorFrequencies[colorFrequencies.Count - i].ColorKey; //The color data, RGB.
                if (c.R == 0 && c.G == 0 && c.B == 0)
                {
                    //If Colour is black, set background white.
                    Console.BackgroundColor = System.ConsoleColor.White;
                }
                else
                {
                    //If Colour is not black, set background black.
                    Console.BackgroundColor = System.ConsoleColor.Black;
                }
                Colorful.Console.WriteLine($"Colour placement: {i} - Colour {colorInfo}", c);
            }
        }

        public class ColorFrequency
        {
            public Color ColorKey { get; set; }
            public int Frequency { get; set; }

        }
    }
}

