using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

using Vivelin.Toolkit;

namespace Toolkit
{
    public static class Program
    {
        private static readonly ImageOptimizer s_optimizer
            = new ImageOptimizer();

        [STAThread]
        public static int Main(string[] args)
        {
            /*if (args.Length > 0)
            {
                var path = args[0];
                if (!File.Exists(path))
                {
                    ShowError($"Cannot find '{path}'. Make sure you typed the name correctly, and then try again.");
                    return ExitCode.FileNotFound;
                }

                using var file = File.OpenRead(path);
                using var buffer = new MemoryStream();
                try
                {
                    OptimizeAsync(file, buffer).GetAwaiter().GetResult();
                    ClipboardHelpers.SetImageFromStream(buffer);
                    return ExitCode.Success;
                }
                catch (ArgumentException)
                {
                    ShowError($"'{path}' is not a valid or supported image file. Please select another image file and then try again.");
                    return ExitCode.InvalidArguments;
                }
            }
            else if (Clipboard.ContainsImage())
            {
                using var buffer = new MemoryStream();
                var image = Clipboard.GetImage();
                image.Save(buffer);

                using var target = new MemoryStream();
                try
                {
                    OptimizeAsync(buffer, target).GetAwaiter().GetResult();
                    ClipboardHelpers.SetImageFromStream(target);
                    return ExitCode.Success;
                }
                catch (ArgumentException)
                {
                    ShowError("The clipboard contains an unsupported image file. Please choose another image and then try again.");
                    return ExitCode.InvalidArguments;
                }
            }
            */
            var wpfApp = new App();
            wpfApp.InitializeComponent();
            return wpfApp.Run();
        }

        private static void ShowError(string message)
        {
            MessageBox.Show(message, "Toolkit", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private static async Task OptimizeAsync(Stream source, Stream target)
        {
            if (source.CanSeek)
                source.Seek(0, SeekOrigin.Begin);

            await s_optimizer.OptimizeAsync(source, target);
        }

        private static class ExitCode
        {
            public const int Success = 0;

            public const int InvalidArguments = 400;

            public const int FileNotFound = 404;
        }
    }
}