using Parallelprogrammeringseksamen.Pipelines.DTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parallelprogrammeringseksamen.Pipelines.PipelineStages
{
    public class Pipeline_WriteResult
    {
        public void WriteResultToConsole(BlockingCollection<DTOImgAnalyzationResultWithFrequency> input)
        {
            foreach (var colorFreq in input.GetConsumingEnumerable())
            {
                Console.WriteLine($"File at: {colorFreq.FilePath} is done");
                Console.WriteLine($"Top 10 color results:");
                foreach (var colorFrequency in colorFreq.DtoColorFrequencies.Take(10))
                {
                    Console.WriteLine(String.Format("{0,-10} : {1,5} | {2,10} : {3,10}", "The color ", colorFrequency.ColorKey, "Appears ", colorFrequency.Frequency));
                }

            }
        }
    }
}
