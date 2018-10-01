using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;

namespace Parallelprogrammeringseksamen
{
    /// <summary>
    /// This class will hold all the LINQ and PLINQ experiments made on the CPU.
    /// </summary>
    public class CPU
    {
        public void ProcessImg_MapReduce_PLINQ(Collection<Color> colors)
        {
            //colors.AsParallel()
            //    .GroupBy(c => c)
            //    .Select(colourGroup => new IDMultiSetItem(
            //        colorGroup., colorGroup.Count()));
        }
    }
}
