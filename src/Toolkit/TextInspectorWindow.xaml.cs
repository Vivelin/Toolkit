using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vivelin.Text;

namespace Toolkit
{
    /// <summary>
    /// Interaction logic for TextInspectorWindow.xaml
    /// </summary>
    public partial class TextInspectorWindow : Window
    {
        public TextInspectorWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //if (!Clipboard.ContainsText())
            //    return;

            //var text = Clipboard.GetText();
            var text = "👻👩‍👩‍👦‍👦";
            GraphemesList.ItemsSource = text.GetGraphemes();
        }
    }
}
