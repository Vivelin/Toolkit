using System;
using System.Windows;

namespace Toolkit
{
    /// <summary>
    /// Interaction logic for TrayIconHost.xaml
    /// </summary>
    public partial class TrayIconHost : Window
    {
        public TrayIconHost()
        {
            InitializeComponent();
        }

        private void Tray_Exit_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void Tray_OptimizeImage_Click(object sender, RoutedEventArgs e)
        {
            var window = new OptimizeImageWindow();
            window.Show();
        }
    }
}