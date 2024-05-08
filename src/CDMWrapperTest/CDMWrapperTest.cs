using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using System.Windows;

namespace CDMWrapperTest
{
    internal class CDMWrapperTest : Window
    {
        public CDMWrapperTest()
        {
            Width = 800;
            Height = 600;
            this.Loaded += CDMWrapperTest_Loaded;
        }

        private void CDMWrapperTest_Loaded(object sender, RoutedEventArgs e)
        {
            var wih = new System.Windows.Interop.WindowInteropHelper(this);
            var hWnd = wih.Handle;
            var w = new CDMWrapper.CDMWrapper();
            w.showCDM(hWnd);
        }

        [STAThread]
        public static void Main()
        {
            Application app = new Application();
            var wt = new CDMWrapperTest();
            app.Run(wt);
        }
    }
}
