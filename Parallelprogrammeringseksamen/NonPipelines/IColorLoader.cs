using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Parallelprogrammeringseksamen
{
    public interface IColorLoader
    {
        ConcurrentBag<Color> GetColourCollection_ParallelForLoop(string BmpImgPath);
        List<Color> MapColours_SequentialForLoop(string BmpImgPath);
        List<Color> MapColours_SequentialForLoop_ManyImages(List<string> BmpImgPaths);
        Task<List<Color>> MapColours_SequentialForLoop_ManyImages_UsingTasksAsync(List<string> BmpImgPaths);
        Task<List<Color>> MapColours_UsingTaskAsync(string BmpImgPath);
    }
}