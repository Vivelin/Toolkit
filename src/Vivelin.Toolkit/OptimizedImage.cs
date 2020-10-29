using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Vivelin.Toolkit
{
    public class OptimizedImage : IDisposable
    {
        public OptimizedImage(Stream stream, ImageFormat imageFormat)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (stream.CanSeek)
                stream.Seek(0, SeekOrigin.Begin);

            Stream = stream;
            ImageFormat = imageFormat;
        }

        public Stream Stream { get; }

        public ImageFormat ImageFormat { get; }

        public string DefaultExtension 
            => ImageFormat.GetDefaultExtension();

        public void Dispose()
        {
            Stream.Dispose();
        }
    }
}
