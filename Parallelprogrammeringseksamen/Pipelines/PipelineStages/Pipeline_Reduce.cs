using Parallelprogrammeringseksamen.Pipelines.DTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Parallelprogrammeringseksamen.Pipelines.PipelineStages
{
    public class Pipeline_Reduce
    {
        public void MapReduceColor(BlockingCollection<DTOImgColorsFromFilepath> input, BlockingCollection<DTOImgAnalyzationResultWithFrequency> output)
        {
            try
            {

                foreach (var imgColors in input.GetConsumingEnumerable())
                {

                    Console.WriteLine("MapReducing: " + imgColors.ImgAnalyzedFilePath);
                    var candidates =
                            imgColors
                                .ListOfColorsFromFilepath
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

                    output.Add(new DTOImgAnalyzationResultWithFrequency { DtoColorFrequencies = candidates, FilePath = imgColors.ImgAnalyzedFilePath });

                }
            }
            finally
            {
                output.CompleteAdding();
            }
        }
    }
}
