using System;
using System.Windows.Controls;
using System.Windows;

namespace CDMTest
{
    internal class CDMTest : Window
    {
        private Label label1;

        public CDMTest()
        {
            Width = 800;
            Height = 600;

            Grid grid = new Grid();
            Content = grid;

            var cc = new CDM.UserControls.CDMUserControl();
            grid.Children.Add(cc);
        }

        [STAThread]
        public static void Main()
        {
            Application app = new Application();

            app.Run(new CDMTest());
        }
    }
}
