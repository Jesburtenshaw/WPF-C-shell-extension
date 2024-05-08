using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CDM.Models
{
    internal class FileFolderModel
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public DateTime LastModifiedDateTime { get; set; }
        public BitmapSource IconSource { get; set; }
    }
}
