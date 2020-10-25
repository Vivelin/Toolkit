using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Microsoft.Win32;

using Vivelin.Toolkit;

namespace Toolkit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const string ClipboardSource = "Clipboard";

        private readonly BitmapSourceFactory _bitmapSourceFactory;
        private readonly ImageOptimizer _optimizer;

        private BitmapSource _sourceImageSource;
        private BitmapSource _resultImageSource;
        private string _sourceImageProperties;
        private string _resultImageProperties;
        private string _sourceFileName;

        public MainWindow()
        {
            _bitmapSourceFactory = new BitmapSourceFactory();
            _optimizer = new ImageOptimizer();

            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BitmapSource SourceImageSource
        {
            get { return _sourceImageSource; }
            set
            {
                if (_sourceImageSource != value)
                {
                    _sourceImageSource = value;
                    RaisePropertyChanged();
                }
            }
        }

        public BitmapSource ResultImageSource
        {
            get { return _resultImageSource; }
            set
            {
                if (_resultImageSource != value)
                {
                    _resultImageSource = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SourceImageProperties
        {
            get { return _sourceImageProperties; }
            set
            {
                if (_sourceImageProperties != value)
                {
                    _sourceImageProperties = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string ResultImageProperties
        {
            get { return _resultImageProperties; }
            set
            {
                if (_resultImageProperties != value)
                {
                    _resultImageProperties = value;
                    RaisePropertyChanged();
                }
            }
        }

        public string SourceFileName
        {
            get { return _sourceFileName; }
            set
            {
                if (_sourceFileName != value)
                {
                    _sourceFileName = value;
                    RaisePropertyChanged();
                }
            }
        }

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName is null)
                throw new ArgumentNullException(nameof(propertyName));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PasteSource_Click(object sender, RoutedEventArgs e)
        {
            if (!Clipboard.ContainsImage())
                return;

            var clipboardImage = Clipboard.GetImage();
            SourceImageSource = clipboardImage;
            SourceImageProperties = $"{clipboardImage.Width}x{clipboardImage.Height} (???, {double.NaN:F1} MB)";
            SourceFileName = ClipboardSource;
        }

        private void BrowseSource_Click(object sender, RoutedEventArgs e)
        {
            var openFile = new OpenFileDialog();
            openFile.Filter =
                "PNG files (*.png)|*.png|" +
                "JPEG files (*.jpg, *.jpeg)|*.jpg;*.jpeg|" +
                "All image files|*.png;*.jpg;*.jpeg|" +
                "All files|*.*";
            openFile.FilterIndex = 3;
            if (openFile.ShowDialog(this) == true)
            {
                var fileInfo = new FileInfo(openFile.FileName);
                var fileSize = new FileSize(fileInfo.Length);

                using var file = File.OpenRead(openFile.FileName);

                var extension = Path.GetExtension(openFile.FileName);
                var image = _bitmapSourceFactory.LoadImage(file, extension);
                SourceImageSource = image;
                SourceImageProperties = $"{image.Width}x{image.Height} ({extension}, {fileSize.Megabytes:F1} MB)";
                SourceFileName = openFile.FileName;
            }
        }

        private async void OptimizeButton_Click(object sender, RoutedEventArgs e)
        {
            using var source = GetSourceStream();
            if (source == null)
                return;

            using var buffer = new MemoryStream();
            var options = new ImageOptimizerOptions();
            await _optimizer.OptimizeAsync(source, buffer, options);

            buffer.Seek(0, SeekOrigin.Begin);
            var fileSize = new FileSize(buffer.Length);
            var image = _bitmapSourceFactory.LoadImage(buffer, ".png");
            ResultImageSource = image;
            ResultImageProperties = $"{Math.Floor(image.Width)}x{Math.Floor(image.Height)} (.png, {fileSize.Megabytes:F1} MB)";
        }

        private Stream GetSourceStream()
        {
            if (SourceFileName == ClipboardSource)
            {
                var buffer = new MemoryStream();
                SourceImageSource.Save(buffer);
                buffer.Seek(0, SeekOrigin.Begin);
                return buffer;
            }

            if (File.Exists(SourceFileName))
            {
                return File.Open(SourceFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            }

            return null;
        }

        private void CopyResult_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetImage(ResultImageSource);
        }
    }
}