using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

using Microsoft.Win32;

using Vivelin.Toolkit;

namespace Toolkit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ImageSource _sourceImageSource;
        private ImageSource _resultImageSource;
        private ImageSourceFactory _imageSourceFactory;
        private string _sourceImageProperties;

        public MainWindow()
        {
            _imageSourceFactory = new ImageSourceFactory();

            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ImageSource SourceImageSource
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

        public ImageSource ResultImageSource
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

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (propertyName is null)
                throw new ArgumentNullException(nameof(propertyName));

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PasteSource_Click(object sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsImage())
                SourceImageSource = Clipboard.GetImage();
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
                var image = _imageSourceFactory.LoadImage(file, extension);
                SourceImageSource = image;
                SourceImageProperties = $"{image.Width}x{image.Height} ({extension}, {fileSize.Megabytes:F1} MB)";
            }
        }
    }
}