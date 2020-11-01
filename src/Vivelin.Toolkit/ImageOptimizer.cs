using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Vivelin.Toolkit
{
    public class ImageOptimizer
    {
        private Configuration _configuration;

        public ImageOptimizer()
        {
            _configuration = Configuration.Default;
        }

        public Task<OptimizedImage> OptimizeAsync(Stream source,
            CancellationToken cancellationToken = default)
        {
            return OptimizeAsync(source, new ImageOptimizerOptions(), cancellationToken);
        }

        public async Task<OptimizedImage> OptimizeAsync(Stream source,
            ImageOptimizerOptions options,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var originalFormat = await Image.DetectFormatAsync(_configuration, source);

                using var image = await Image.LoadAsync(_configuration, source, cancellationToken);
                if (image.Width > options.TargetWidth || image.Height > options.TargetHeight)
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(options.TargetWidth, options.TargetHeight),
                        Mode = ResizeMode.Max
                    }));
                }

                var buffer = new MemoryStream();
                IImageEncoder encoder = null;
                do
                {
                    buffer.SetLength(0); // Reset the buffer
                    encoder = SelectEncoder(encoder, originalFormat, options);
                    await image.SaveAsync(buffer, encoder, cancellationToken);
                }
                while (buffer.Length > options.TargetSize.Bytes);

                return new OptimizedImage(buffer, GetImageFormat(encoder));
            }
            catch (UnknownImageFormatException ex)
            {
                throw new ArgumentException("The source does not contain a valid image or the image is of an unsupported format.", ex);
            }
        }

        /// <summary>
        /// Selects an encoder that should result in a lower image file size.
        /// </summary>
        /// <param name="previousEncoder">The previously used encoder, or <c>null</c>.</param>
        /// <param name="originalFormat">The format of the original image.</param>
        /// <param name="options">The options.</param>
        /// <returns>A new or changed encoder.</returns>
        private IImageEncoder SelectEncoder(IImageEncoder previousEncoder, IImageFormat originalFormat, ImageOptimizerOptions options)
        {
            if (previousEncoder is null)
            {
                return originalFormat switch
                {
                    JpegFormat _ => new JpegEncoder
                    {
                        Quality = options.InitialJpegQuality
                    },
                    PngFormat _ => new PngEncoder(),
                    _ => new PngEncoder()
                };
            }

            if (previousEncoder is PngEncoder)
            {
                return new JpegEncoder
                {
                    Quality = options.InitialJpegQuality
                };
            }

            if (previousEncoder is JpegEncoder jpegEncoder)
            {
                jpegEncoder.Quality -= 5;
                if (jpegEncoder.Quality < options.MinimumJpegQuality)
                    throw new InvalidOperationException("Unable to optimize image without reducing quality below minimum.");
                return jpegEncoder;
            }

            throw new NotSupportedException("Unable to select encoder settings based on previous encoder.");
        }

        private ImageFormat GetImageFormat(IImageEncoder encoder)
        {
            return encoder switch
            {
                JpegEncoder _ => ImageFormat.Jpeg,
                PngEncoder _ => ImageFormat.Png,
                _ => ImageFormat.None
            };
        }
    }
}