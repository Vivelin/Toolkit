using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
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

            LoadFromClipboard();
        }

        private void LoadFromClipboard()
        {
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
                LoadFromFile(openFile.FileName);
            }
        }

        private void LoadFromFile(string path)
        {
            var fileInfo = new FileInfo(path);
            var fileSize = new FileSize(fileInfo.Length);
            var file = File.OpenRead(path);

            var extension = Path.GetExtension(path);
            var image = _bitmapSourceFactory.LoadImage(file, extension);
            SourceImageSource = image;
            SourceImageProperties = $"{image.PixelWidth}x{image.PixelHeight} ({extension}, {fileSize.Megabytes:F1} MB)";
            SourceFileName = path;
            SourceImage.RenderTransform = image.GetExifOrientationRenderTransform();
            SourceImage.RenderTransformOrigin = new Point(0.5, 0.5);
        }

        private async void OptimizeButton_Click(object sender, RoutedEventArgs e)
        {
            using var source = GetSourceStream();
            if (source == null)
                return;

            var options = new ImageOptimizerOptions();
            var result = await _optimizer.OptimizeAsync(source, options);

            var fileSize = new FileSize(result.Stream.Length);
            var image = _bitmapSourceFactory.LoadImage(result.Stream, result.DefaultExtension);
            ResultImageSource = image;
            ResultImageProperties = $"{image.PixelWidth}x{image.PixelHeight} ({result.DefaultExtension}, {fileSize.Megabytes:F1} MB)";

            Clipboard.SetImage(image);
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

        private void SaveResult_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var args = ((App)App.Current).Args;
            if (args.Length > 0 && File.Exists(args[0]))
            {
                LoadFromFile(args[0]);
                OptimizeButton_Click(this, new RoutedEventArgs());
            }
            else if (Clipboard.ContainsImage())
            {
                LoadFromClipboard();
                OptimizeButton_Click(this, new RoutedEventArgs());
            }
        }
    }
}