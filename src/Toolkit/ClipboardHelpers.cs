using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Toolkit
{
    public static class ClipboardHelpers
    {
        public static void SetImageFromStream(Stream target)
        {
            if (target.CanSeek)
                target.Seek(0, SeekOrigin.Begin);

            var decoder = new PngBitmapDecoder(target,
                BitmapCreateOptions.None,
                BitmapCacheOption.Default);
            var frame = decoder.Frames.Single();

            Clipboard.SetImage(frame);
        }
    }
}
