using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Drawing;

namespace _262ImageViewer
{
    public class LocalImages : ImageLoader
    {
        /*
         * The list of jpgs in the directory.
         */
        private List<Uri> fileNames = new List<Uri>();

        /*
         * The current index of the list.
         */
        private int position = 0;

        /*
         * Make a new LocalImages based on the given directory Uri.
        public LocalImages(Uri folder)
        {
            readFiles(folder);
        }

        /*
         * Find in all of the jpgs inside the given directory Uri 
         * and add them to the list.
         */
        private void readFiles(Uri folder)
        {
            if (Directory.Exists(folder.AbsolutePath))
            {
                string[] fileArray = Directory.GetFiles(folder.AbsolutePath);
                foreach (string file in fileArray)
                {
                    if (file.EndsWith(".jpg"))
                    {
                        fileNames.Add(new Uri(file));
                    }
                }
            }
            else
            {
                throw new IOException(folder.AbsolutePath + " is not a valid path.");
            }
        }

        /*
         * Verify that the given index is within the bounds of the filename list.
         */
        private bool isValidIndex(int index) 
        {
            return (index < fileNames.Count && index >= 0);
        }

        /*
         * Get the next given number of images, as a List of BitmapImages.
         * If there are no more images, return an empty list.
         */
        public List<BitmapImage> GetNext(int numImages) 
        {
            List<BitmapImage> returnList = new List<BitmapImage>();
            // Check if the next index is valid
            if (isValidIndex(position + 1))
            {
                for (int i = 0; i < numImages; i++)
                {
                    if (isValidIndex(position + 1))
                    {
                        position++;
                        returnList.Add(new BitmapImage(fileNames[position]));
                    }
                    else
                    {
                        // Pad the list with blanks, using the resolution of the first image
                        if (returnList.Count > 0)
                        {
                            //Bitmap b = new Bitmap(1, 1);
                            //b.SetPixel(0, 0, Color.White);
                            //b = new Bitmap(b, returnList[0].PixelWidth, returnList[0].PixelHeight);
                            // DOESN'T WORK YET
                            // TODO: Fix this.
                            // For now, just duplicate the last image.
                            returnList.Add(returnList[0]);
                        }
                    }
                }
            }
            return returnList;
        }

        /*
         * Get the previous given number of images, as a List of BitmapImages.
         * If there are no more images, return an empty list.
         */
        public List<BitmapImage> GetPrev(int numImages)
        {
            List<BitmapImage> returnList = new List<BitmapImage>();
            // Check if the previous index is valid
            if (isValidIndex(position - 1))
            {
                for (int i = 0; i < numImages; i++)
                {
                    if (isValidIndex(position - 1))
                    {
                        position--;
                        returnList.Add(new BitmapImage(fileNames[position]));
                    }
                    else
                    {
                        // Pad the list with blanks, using the resolution of the first image
                        if (returnList.Count > 0)
                        {
                            //Bitmap b = new Bitmap(1, 1);
                            //b.SetPixel(0, 0, Color.White);
                            //b = new Bitmap(b, returnList[0].PixelWidth, returnList[0].PixelHeight);
                            // DOESN'T WORK YET
                            // TODO: Fix this.
                            // For now, just duplicate the last image.
                            returnList.Add(returnList[0]);
                        }
                    }
                }
            }
            return returnList;
        }
    }
}
