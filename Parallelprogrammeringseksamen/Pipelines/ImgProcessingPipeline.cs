using Parallelprogrammeringseksamen.Pipelines.DTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Parallelprogrammeringseksamen.Pipelines.PipelineStages;
using static Parallelprogrammeringseksamen.Pipelines.PipelineStages.Pipeline_LoadImagePaths;

namespace Parallelprogrammeringseksamen.Pipelines
{
    public class ImgProcessingPipeline
    {
        public void Initialize(string FolderLocationOrOnlineLocation = "") //if "" it uses default filepath or URL.
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
            var stage1 = f.StartNew(() => pplsLoadImage.LoadImagePaths(CallingMode.Online, buffer1)); //Load image paths from local or online source.

            //Use extra stages for load balancing to minimize bottleneck.
            var stage2_0 = f.StartNew(() => pplsProcessImage.LoadImageColors(buffer1, buffer2)); //Load colours from each image into collection.
            var stage2_1 = f.StartNew(() => pplsProcessImage.LoadImageColors(buffer1, buffer2)); //Load colours from each image into collection.
            var stage2_2 = f.StartNew(() => pplsProcessImage.LoadImageColors(buffer1, buffer2)); //Load colours from each image into collection.
            var stage2_3 = f.StartNew(() => pplsProcessImage.LoadImageColors(buffer1, buffer2)); //Load colours from each image into collection.
            Task.WaitAll(stage1, stage2_0, stage2_1, stage2_2, stage2_3);
            buffer2.CompleteAdding();

            var stage3 = f.StartNew(() => pplsMapReduce.MapReduceColor(buffer2, buffer3)); //MapReduce.

            var stage4 = f.StartNew(() => pplsWriteResult.WriteResultToConsole(buffer3)); //Print result.

            //Wait for all pipeline stages/tasks to finish.
            Task.WaitAll(stage3, stage4);
        }
    }
}
