using System;
using System.Collections.Generic;
using System.Text;

namespace Vivelin.Toolkit
{
    public class ImageOptimizerOptions
    {
        public int TargetHeight { get; set; }
            = 2048;

        public int TargetWidth { get; set; }
            = 2048;

        public FileSize TargetSize { get; set; }
            = FileSize.MB(8);

        public int InitialJpegQuality { get; set; }
            = 95;

        public int MinimumJpegQuality { get; set; }
            = 50;
    }
}
