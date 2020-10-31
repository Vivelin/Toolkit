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

        public int Quality { get; set; }
            = 90;

        public int MinimumQuality { get; set; }
            = 50;
    }
}
