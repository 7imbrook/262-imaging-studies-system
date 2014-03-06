/* 
 * ImageLoader.cs
 * 
 * Version: 
 *     $Id$ 
 * 
 * Revisions: 
 *     $Log$ 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace _262ImageViewer
{
    /*
     * An interface for the loading of images. 
     * Extended by RemoteImages and LocalImages.
     */
    public interface ImageLoader
    {
        /*
         * Return the number of objects in the list.
         */
        int Count();

        /*
         * Allows access of a BitmapImage from an implementation
         * of this interface.
         */
        BitmapImage this[int i]
        {
            get;
            set;
        }
    }
}
