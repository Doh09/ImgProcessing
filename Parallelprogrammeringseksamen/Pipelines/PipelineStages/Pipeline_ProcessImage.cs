using Parallelprogrammeringseksamen.Pipelines.DTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text;

namespace Parallelprogrammeringseksamen.Pipelines.PipelineStages
{
    public class Pipeline_ProcessImage
    {
        public void LoadImageColors(BlockingCollection<string> input, BlockingCollection<DTOImgColorsFromFilepath> output)
        {
            try
            {
                foreach (var filePath in input.GetConsumingEnumerable()) //Use GetConsumingEnumerable to acquire list to work on. Also allows parallel working on collection if needed, we don't use that though.
                {

                    Console.WriteLine("Loading colors for analyzing into buffer from: " + filePath);
                    Bitmap bmpFile = null;
                    //If online image
                    if (filePath.ToLower().StartsWith("http"))
                    {
                        var request = WebRequest.Create("http://www.gravatar.com/avatar/6810d91caff032b202c50701dd3af745?d=identicon&r=PG");

                        using (var response = request.GetResponse())
                        using (var stream = response.GetResponseStream())
                        {
                            bmpFile = (Bitmap) Bitmap.FromStream(stream);
                        }
                    }
                    else //Else if image from local directory.
                    {
                        bmpFile = new Bitmap(filePath);
                    }
                    DTOImgColorsFromFilepath colors = new DTOImgColorsFromFilepath { ImgAnalyzedFilePath = filePath, ListOfColorsFromFilepath = new List<Color>() };

                    for (int i = 0; i < bmpFile.Width; i++)
                    {
                        for (int j = 0; j < bmpFile.Height; j++)
                        {
                            colors.ListOfColorsFromFilepath.Add(bmpFile.GetPixel(i, j)); //Set to sequential, as it's only one line of code
                        }
                    }

                    output.Add(colors);
                }
            }
            finally
            {
                //output.CompleteAdding();
            }
        }
    }
}
