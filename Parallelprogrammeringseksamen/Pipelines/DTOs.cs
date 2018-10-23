using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Parallelprogrammeringseksamen.Pipelines.DTOs
{
    public class DTOColorFrequency
    {
        public Color ColorKey { get; set; }
        public int Frequency { get; set; }

    }

    public class DTOImgAnalyzationResultWithFrequency
    {
        public IEnumerable<DTOColorFrequency> DtoColorFrequencies { get; set; }
        public string FilePath { get; set; }

    }

    public class DTOImgColorsFromFilepath
    {
        public IList<Color> ListOfColorsFromFilepath { get; set; }
        public string ImgAnalyzedFilePath { get; set; }

    }
}
