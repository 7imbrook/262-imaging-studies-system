using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace _262ImageViewer
{
    class Study
    {

        public List<BitmapImage> imageRefs;

        private void loadImages()
        {
            var basePath = "C:\\Users\\michael\\Desktop\\head_mri";
            Uri link = new Uri(basePath + "\\mri_head17.JPG");

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = link;
            src.EndInit();
            imageRefs.Add(src);
        }
    }

}
