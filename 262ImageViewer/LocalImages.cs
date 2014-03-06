﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;

namespace _262ImageViewer
{
    public class LocalImages : List<Uri>, ImageLoader
    {
        /*
        * Make a new LocalImages based on the given directory Uri.
        */
        public LocalImages(Uri folder)
        {
            readFiles(folder);
        }

        /*
         * When the list is subscripted, return a new BitmapImage 
         * from the Uri at the index given.
         */
        public new BitmapImage this[int i]
        {
            get
            {
                return new BitmapImage(base[i]);
            }
            set {}
        }

        /*
         * Override count to return the base list's count
         */
        public new int Count()
        {
            return base.Count;
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
                    if (file.ToLower().EndsWith(".jpg"))
                    {
                        base.Add(new Uri(file));
                    }
                }
            }
            else
            {
                throw new IOException(folder.AbsolutePath + " is not a valid path.");
            }
        }
    }
}
