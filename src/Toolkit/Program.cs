using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Text;
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

                var optimizer = new ImageOptimizer();
                var options = new ImageOptimizerOptions
                {

                };
                using var target = new MemoryStream();
                optimizer.Optimize(buffer, target, options);
            }

            var wpfApp = new App();
            wpfApp.InitializeComponent();
            return wpfApp.Run();
        }
    }
}
