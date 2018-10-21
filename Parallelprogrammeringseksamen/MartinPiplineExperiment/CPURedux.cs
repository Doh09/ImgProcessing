using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Parallelprogrammeringseksamen.MartinPiplineExperiment
{
    public class CPURedux
    {
        public void Initialize(List<Color> colors)
        {
            Console.WriteLine("RESULT *-*-*-*- CPU Map-Reduce time results");
            var plinqColours = ProcessImg_MapReduce_PLINQ(colors);
            Console.WriteLine("");
            Console.WriteLine("RESULT *-*-*-*- CPU Map-Reduce color results");
            PrintListWithColours(plinqColours);
            Console.WriteLine("");
        }

        public List<CPU.ColorFrequency> ProcessImg_MapReduce_PLINQ(IEnumerable<Color> colors)
        {
            Console.WriteLine("Starting PLINQ Map-Reduce...");
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            var candidates =
                colors
                    .AsParallel()
                    .GroupBy(color => color)
                    .Select(
                        intermediate => new CPU.ColorFrequency
                        {
                            ColorKey = intermediate.Key,
                            Frequency = intermediate.Sum(c => 1)
                        })
                    .OrderBy(c => c.Frequency);
            sw.Stop();

            Console.WriteLine($"Parallel CPU: {sw.Elapsed}");

            return candidates.ToList();
        }

        private void PrintListWithColours(List<CPU.ColorFrequency> colorFrequencies, int topCandidatesToPrint = 10)
        {
            for (int i = topCandidatesToPrint; i > 0; i--)
            {
                Thread.Sleep(100);
                CPU.ColorFrequency cf = colorFrequencies[colorFrequencies.Count - i];
                Color c = cf.ColorKey; //The color used for the text in the console.
                Color colorInfo = cf.ColorKey; //The color data, RGB.
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
                Colorful.Console.WriteLine($"Colour placement: {i} - Colour {colorInfo} - Frequency: {cf.Frequency}", c);
            }
        }

        public class ColorFrequency
        {
            public Color ColorKey { get; set; }
            public int Frequency { get; set; }

        }
    }
}
