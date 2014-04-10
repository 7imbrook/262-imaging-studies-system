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

namespace ImageLoader
{
    /*
     * 
     */
    public interface Image
    {
        /*
         * 
         */
        byte[] getSlice(int sliceIndex, Boolean vertical);

        /*
         * 
         */
        BitmapImage image;
    }
}
