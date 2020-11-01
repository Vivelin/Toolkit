using System;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
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
                var format = await Image.DetectFormatAsync(_configuration, source);

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
                if (format is JpegFormat)
                {
                    var encoder = new JpegEncoder { Quality = options.Quality };
                    await image.SaveAsJpegAsync(buffer, cancellationToken);
                    return new OptimizedImage(buffer, ImageFormat.Jpeg);
                }

                await image.SaveAsPngAsync(buffer, cancellationToken);
                return new OptimizedImage(buffer, ImageFormat.Png);
            }
            catch (UnknownImageFormatException ex)
            {
                throw new ArgumentException("The source does not contain a valid image or the image is of an unsupported format.", ex);
            }
        }
    }
}