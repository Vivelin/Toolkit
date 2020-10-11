using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

using Vivelin.Toolkit;

namespace Toolkit
{
    public static class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            // TODO: Refactor into ToolFactory?
            if (args.Length > 0)
            {
                // TODO: If file is an image, resize it and add it to the clipboard

                // Else: error (unsupported file type)

                // Else: error (unsupported argument)
            }
            else if (Clipboard.ContainsImage())
            {
                using var buffer = new MemoryStream();
                var image = Clipboard.GetImage();
                image.Save(buffer);
                buffer.Seek(0, SeekOrigin.Begin);

                var optimizer = new ImageOptimizer();
                using var target = new MemoryStream();
                optimizer.OptimizeAsync(buffer, target).GetAwaiter().GetResult();
                target.Seek(0, SeekOrigin.Begin);

                var decoder = new PngBitmapDecoder(target,
                    BitmapCreateOptions.None,
                    BitmapCacheOption.Default);
                var frame = decoder.Frames.Single();
                Clipboard.SetImage(frame);
                
                return ExitCode.Success;
            }

            var wpfApp = new App();
            wpfApp.InitializeComponent();
            return wpfApp.Run();
        }

        private class ExitCode
        {
            public const int Success = 0;
        }
    }
}