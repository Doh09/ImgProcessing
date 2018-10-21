using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parallelprogrammeringseksamen.MartinPiplineExperiment;

namespace Parallelprogrammeringseksamen
{
    public class ImgProcessorRedux
    {
        private IEnumerable<string> _bmpImgPaths;
        #region Dependency Injections
        private IColorLoader cl { get; set; }
        #endregion

        public void Initialize(IColorLoader cl, IEnumerable<string> bmpImgPaths)
        {
            this.cl = cl;
            _bmpImgPaths = bmpImgPaths;

            //Buffer for the filepaths
            var buffer1 = new BlockingCollection<string>();
            //Buffer for the colors
            var buffer2 = new BlockingCollection<DTOColorPath>();
            //Buffer for the mapped/reduced color
            var buffer3 = new BlockingCollection<DTOColorFrequencyPath>();

            var f = new TaskFactory(TaskCreationOptions.LongRunning,
                TaskContinuationOptions.None);

            var stage1 = f.StartNew(() => LoadImage(_bmpImgPaths, buffer1));

            var stage2 = f.StartNew(() => ProcessImage(buffer1, buffer2));

            var stage3 = f.StartNew(() => MapReduceColor(buffer2, buffer3));

            var stage4 = f.StartNew(() => WriteResultFile(buffer3));
            Task.WaitAll(stage1, stage2, stage3, stage4);
        }

        private void LoadImage(IEnumerable<string> input, BlockingCollection<string> output)
        {
            try
            {
                foreach (var filePath in input)
                {
                    Console.WriteLine("Starting: "+filePath);
                    output.Add(filePath);
                }
            }
            finally
            {
                output.CompleteAdding();
            }
        }

        public void ProcessImage(BlockingCollection<string> input, BlockingCollection<DTOColorPath> output)
        {
            try
            {
                foreach (var filePath in input.GetConsumingEnumerable())
                {

                    Console.WriteLine("Proccesing: " + filePath);
                    Bitmap bmpFile = new Bitmap(filePath);
                    DTOColorPath colors = new DTOColorPath { FilePath = filePath, Colors = new List<Color>() };

                    for (int i = 0; i < bmpFile.Width; i++)
                    {
                        for (int j = 0; j < bmpFile.Height; j++)
                        {
                            colors.Colors.Add(bmpFile.GetPixel(i, j)); //Set to sequential, as it's only one line of code
                        }
                    }

                    output.Add(colors);
                }
            }
            finally
            {
                output.CompleteAdding();
            }
        }

        public void MapReduceColor(BlockingCollection<DTOColorPath> input, BlockingCollection<DTOColorFrequencyPath> output)
        {
            try
            {

                foreach (var colorList in input.GetConsumingEnumerable())
                {

                    Console.WriteLine("Sorting: " + colorList.FilePath);
                    var candidates =
                            colorList
                                .Colors
                                .AsParallel()
                                .GroupBy(color => color)
                                .Select(
                                    intermediate => new DTOColorFrequency
                                    {
                                        ColorKey = intermediate.Key,
                                        Frequency = intermediate.Sum(c => 1)
                                    })
                                .OrderBy(c => c.Frequency)
                                .Reverse();

                    output.Add(new DTOColorFrequencyPath { DtoColorFrequencies = candidates, FilePath = colorList.FilePath });

                }
            }
            finally
            {
                output.CompleteAdding();
            }
        }

        public void WriteResultFile(BlockingCollection<DTOColorFrequencyPath> input)
        {
            foreach (var colorFreq in input.GetConsumingEnumerable())
            {
                Console.WriteLine($"File at: {colorFreq.FilePath} is done");
                Console.WriteLine($"Showing top 10 colors:");
                foreach (var colorFrequency in colorFreq.DtoColorFrequencies.Take(10))
                {
                    Console.WriteLine(String.Format("{0,-10} : {1,5} | {2,10} : {3,10}", "The color ", colorFrequency.ColorKey, "Appears ",colorFrequency.Frequency));
                }

            }
        }

        void PrintAscii(string s)
        {
            Colorful.Console.WriteAscii(s);
        }

        void PrintLine(string s)
        {
            Console.WriteLine(s);
        }

        public class DTOColorFrequency
        {
            public Color ColorKey { get; set; }
            public int Frequency { get; set; }

        }

        public class DTOColorFrequencyPath
        {
            public IEnumerable<DTOColorFrequency> DtoColorFrequencies { get; set; }
            public string FilePath { get; set; }

        }

        public class DTOColorPath
        {
            public IList<Color> Colors { get; set; }
            public string FilePath { get; set; }

        }
    }
}
