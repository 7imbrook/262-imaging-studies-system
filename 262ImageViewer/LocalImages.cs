using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace _262ImageViewer
{
    public class LocalImages : IEnumerable
    {

        public List<string> fileNames = new List<string> { };
        public String folderLocation;
        public int position;

        public LocalImages(String folder, List<string> files)
        {
            folderLocation = folder;
            fileNames = files;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public ImageEnum GetEnumerator()
        {
            return new ImageEnum(this);
        }
        /*public bool moveNext()
        {
            position++;
            return (position < fileNames.Count);
        }

        public void Reset()
        {
            position = -1;
        }
        public object Current
        {
            get {return new LocalImages(); }
        }*/


    }
    public class ImageEnum
    {
        public LocalImages image;
        int position = -1;

        public ImageEnum(LocalImages li)
        {
            image = li;
        }

        public bool MoveNext()
        {
            position++;
            return (position < image.fileNames.Count);
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
                    bi.BeginInit();
                    bi.UriSource = new Uri(image.folderLocation + image.fileNames[position]);
                    bi.EndInit();
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
