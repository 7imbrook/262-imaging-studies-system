using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace _262ImageViewer
{
    interface ImageLoader
    {
        int Count();
        BitmapImage this[int i]
        {
            get;
            set;
        }
    }
}
