using System;

namespace Toolkit
{
    public static class Program
    {
        [STAThread]
        public static int Main(string[] args)
        {
            var wpfApp = new App();
            wpfApp.InitializeComponent();
            return wpfApp.Run();
        }
    }
}