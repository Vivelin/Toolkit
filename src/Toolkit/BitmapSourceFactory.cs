using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Toolkit
{
    public class BitmapSourceFactory
    {
        private const string ExifOrientationQuery = "/app1/ifd/exif:{uint=274}";

        public BitmapSource LoadImage(Stream source, string extension)
        {
            var decoder = CreateDecoder(source, extension);
            return TransformForOrientation(decoder.Frames.SingleOrDefault());
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

        public static BitmapSource TransformForOrientation(BitmapSource source)
        {
            var (angle, mirror) = GetOrientation(source);
            if (angle == 0 && !mirror)
                return source;

            var transform = new TransformGroup();
            if (angle > 0)
                transform.Children.Add(new RotateTransform(angle));
            if (mirror)
                transform.Children.Add(new ScaleTransform(-1, 1));
            return new TransformedBitmap(source, transform);
        }

        private static (int angle, bool mirror) GetOrientation(BitmapSource source)
        {
            if (!((BitmapMetadata)source.Metadata).ContainsQuery(ExifOrientationQuery))
            {
                return (0, false);
            }

            var orientation = (ushort)((BitmapMetadata)source.Metadata).GetQuery(ExifOrientationQuery);
            Debug.WriteLine($"{ExifOrientationQuery}: {orientation}");
            switch (orientation)
            {
                case 1: return (0, false);
                case 2: return (0, true);
                case 3: return (180, false);
                case 4: return (180, true);
                case 5: return (90, true);
                case 6: return (90, false);
                case 7: return (270, true);
                case 8: return (270, false);
                default:
                    throw new NotSupportedException($"EXIF orientation is not supported. Value: '{orientation}'. Source: {ExifOrientationQuery}");
            }
        }
    }
}