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
    public class LocalImages : IEnumerable, ImageLoader
    {

        public List<string> fileNames = new List<string> { };
        public String folderLocation;
        public int position;

        public LocalImages(String folder)
        {
            folderLocation = folder;
            readFiles(folder);
        }
        private void readFiles(String folder)
        {
            if (Directory.Exists(folder))
            {
                string[] fileArray = Directory.GetFiles(folder);
                foreach (string file in fileArray)
                {
                    if (file.EndsWith(".jpg"))
                    { 
                        fileNames.Add(file);
                    }
                }
                string[] subdirectoryEntries = Directory.GetDirectories(folder);
                foreach (string subdirectory in subdirectoryEntries)
                {
                    readFiles(subdirectory);
                }
            }
            else
            {
                string temp = folder + " is not a valid path.";
                throw new IOException(temp);
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public ImageEnum GetEnumerator()
        {
            return new ImageEnum(this);
        }
            }
    public class ImageEnum
    {
        public LocalImages image;
        public int position = -1;

        public ImageEnum(LocalImages li)
        {
            image = li;
        }

        public bool MoveNext()
        {
            position++;
            return (position < image.fileNames.Count);
        }

        public bool MoveBack()
        {
            position--;
            return (position > 0);
        }

        public void Reset()
        {
            position = -1;
        }

        public BitmapImage Current
        {
            get
            {
                try
                {
                    BitmapImage bi = new BitmapImage();
                    if ( (position >= 0) && (position < image.fileNames.Count))
                    { 
                        bi.BeginInit();
                        bi.UriSource = new Uri(image.folderLocation + image.fileNames[position]);
                        bi.EndInit();
                    }
                    /*else
                    {
                        Bitmap bitmap = (_262ImageViewer.Properties.Resources.blankImage);
                        try
                        {
                            BitmapImage i = Imaging.CreateBitmapSourceFromHBitmap(
                                

                                );
                        }
                        return ;
                    }*/
                    return bi;
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}
