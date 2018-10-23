using Parallelprogrammeringseksamen.Pipelines.DTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Parallelprogrammeringseksamen.Pipelines.PipelineStages;

namespace Parallelprogrammeringseksamen.Pipelines
{
    public class ImgProcessingPipeline
    {
        public void Initialize(IEnumerable<string> bmpImgPaths)
        {

            //Buffer for the filepaths
            var buffer1 = new BlockingCollection<string>();
            //Buffer for the colors
            var buffer2 = new BlockingCollection<DTOImgColorsFromFilepath>();
            //Buffer for the mapped/reduced color
            var buffer3 = new BlockingCollection<DTOImgAnalyzationResultWithFrequency>();

            //Establish pipeline objects
            var pplsLoadImage = new Pipeline_LoadImagePaths();
            var pplsProcessImage = new Pipeline_ProcessImage();
            var pplsMapReduce = new Pipeline_MapReduce();
            var pplsWriteResult = new Pipeline_WriteResult();

            //Set TaskFactory, let it be long running so it will work with pipeline working queues & tasks demanding a lot of time.
            var f = new TaskFactory(TaskCreationOptions.LongRunning,
                TaskContinuationOptions.None);

            //Use TaskFactory to run all pipeline stages.
            var stage1 = f.StartNew(() => pplsLoadImage.LoadImagePaths(bmpImgPaths, buffer1)); //Load image paths.

            var stage2 = f.StartNew(() => pplsProcessImage.LoadImageColors(buffer1, buffer2)); //Load colours from each image into collection.

            var stage3 = f.StartNew(() => pplsMapReduce.MapReduceColor(buffer2, buffer3)); //MapReduce.

            var stage4 = f.StartNew(() => pplsWriteResult.WriteResultToConsole(buffer3)); //Print result.

            //Wait for all pipeline stages/tasks to finish.
            Task.WaitAll(stage1, stage2, stage3, stage4);
        }
    }
}
