using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Parallelprogrammeringseksamen.Pipelines.PipelineStages
{
    public class Pipeline_LoadImagePaths
    {
        public void LoadImagePaths(IEnumerable<string> input, BlockingCollection<string> output)
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
