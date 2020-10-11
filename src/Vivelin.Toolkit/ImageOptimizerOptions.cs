using System;
using System.Collections.Generic;
using System.Text;

namespace Vivelin.Toolkit
{
    public class ImageOptimizerOptions
    {
        public FileSize TargetSize { get; set; }
            = FileSize.MB(8);
    }
}
