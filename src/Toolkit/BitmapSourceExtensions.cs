using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Toolkit
{
    internal static class BitmapSourceExtensions
    {
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
    }
}