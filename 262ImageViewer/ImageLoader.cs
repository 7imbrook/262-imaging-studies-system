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
        /*
         * Get the next given number of images, as a List of BitmapImages.
         * If there are no more images, return an empty list.
         */
        List<BitmapImage> GetNext(int numImages);

        /*
         * Get the previous given number of images, as a List of BitmapImages.
         * If there are no more images, return an empty list.
         */
        List<BitmapImage> GetPrev(int numImages);
    }
}
