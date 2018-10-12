using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Parallelprogrammeringseksamen
{
    /// <summary>
    /// This class will hold all the LINQ and PLINQ experiments made on the GPU with CUDA.
    /// </summary>
    public class GPU
    {
        public static void Run(int N, double[] a, double[] b)
        {
            Parallel.For(0, N, i => { a[i] += b[i]; });
        }

        public void ProcessImg_MapReduce_PLINQ(IEnumerable<Color> colors)
        {
            

            //Stopwatch sw = new Stopwatch();
            //sw.Restart();
            //var candidates =
            //    colors
            //        //.AsParallel()
            //        .GroupBy(color => color)
            //        .Select(
            //            intermediate => new
            //            {
            //                ColorKey = intermediate.Key,
            //                Frequency = intermediate.Sum(c => 1)
            //            })
            //        .OrderBy(c => c.Frequency);
            //sw.Stop();

            //Console.WriteLine(sw.Elapsed);

            //foreach (var result in candidates)
            //{
            //    var localR = result.ColorKey.R;
            //    var localG = result.ColorKey.G;
            //    var localB = result.ColorKey.B;
            //    var colorToConsole = Color.FromArgb(localR, localG, localB);

            //    Console.WriteLine(
            //        $"Red: {localR} Green: {localG} Blue: {localB} Appeared: {result.Frequency}",
            //       colorToConsole); //FromArgb(result.ColorKey.R, result.ColorKey.G, result.ColorKey.B));
            //                        //Console.WriteLine($"Red: {result.ColorKey.R} Green: {result.ColorKey.G} Blue: {result.ColorKey.B} Appeared: {result.Frequency}");

            //    Thread.Sleep(1000);
            //}

            //Console.ReadLine();


            //.Select(foafGroup => new IDMultisetItem(foafGroup.Key,
            //    foafGroup.Count()));
            //return Multiset.MostNumerous(candidates, maxCandidates);
        }
    }
}
