using System;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Toolkit
{
    public class BitmapSourceFactory
    {
        public BitmapSource LoadImage(Stream source, string extension)
        {
            var decoder = CreateDecoder(source, extension);
            return decoder.Frames.SingleOrDefault();
        }

        private BitmapDecoder CreateDecoder(Stream source, string extension)
        {
            switch (extension.ToLowerInvariant())
            {
                case ".png":
                    return new PngBitmapDecoder(source,
                        BitmapCreateOptions.None,
                        BitmapCacheOption.OnLoad);

                case ".jpg":
                case ".jpeg":
                    return new JpegBitmapDecoder(source,
                        BitmapCreateOptions.None,
                        BitmapCacheOption.OnLoad);

                default:
                    throw new NotSupportedException($"Images with the extension '{extension}' are not supported.");
            }
        }
    }
}