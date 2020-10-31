using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Toolkit
{
    internal static class BitmapSourceExtensions
    {
        private const string ExifOrientationQuery = "/app1/ifd/exif:{uint=274}";

        /// <summary>
        /// Encodes the bitmap image as PNG to a stream.
        /// </summary>
        /// <param name="source">The bitmap image to encode.</param>
        /// <param name="target">The stream to save the PNG image to.</param>
        public static void Save(this BitmapSource source, Stream target)
        {
            source.Save(target, new PngBitmapEncoder());
        }

        /// <summary>
        /// Encodes the bitmap image to a stream.
        /// </summary>
        /// <param name="source">The bitmap image to encode.</param>
        /// <param name="target">The stream to save the image to.</param>
        /// <param name="encoder">The encoder to use.</param>
        public static void Save(this BitmapSource source, Stream target, BitmapEncoder encoder)
        {
            encoder.Frames.Add(BitmapFrame.Create(source));
            encoder.Save(target);
        }

        public static Transform GetExifOrientationRenderTransform(this BitmapSource source)
        {
            var (angle, mirror) = source.GetOrientation();
            if (angle == 0 && !mirror)
                return null;

            var transform = new TransformGroup();
            if (angle > 0)
                transform.Children.Add(new RotateTransform(angle));
            if (mirror)
                transform.Children.Add(new ScaleTransform(-1, 1));
            return transform;
        }

        private static (int angle, bool mirror) GetOrientation(this BitmapSource source)
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