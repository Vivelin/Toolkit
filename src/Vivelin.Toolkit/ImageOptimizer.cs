using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using SixLabors.ImageSharp;
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

        public Task OptimizeAsync(Stream source, Stream target,
            CancellationToken cancellationToken = default)
        {
            return OptimizeAsync(source, target, new ImageOptimizerOptions(), cancellationToken);
        }

        public async Task OptimizeAsync(Stream source, Stream target,
            ImageOptimizerOptions options,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var image = await Image.LoadAsync(_configuration, source, cancellationToken);
                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(options.TargetWidth, options.TargetHeight),
                    Mode = ResizeMode.Max
                }));
                await image.SaveAsPngAsync(target, cancellationToken);
            }
            catch (UnknownImageFormatException ex)
            {
                throw new ArgumentException("The source does not contain a valid image or the image is of an unsupported format.", ex);
            }
        }
    }
}