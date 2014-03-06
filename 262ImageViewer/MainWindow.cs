using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace _262ImageViewer
{
    public partial class MainWindow
    {

        private ImageView imageView;

        /**
         * Given a study, make an image loader and view for it.
         */
        public void loadStudy(StudySession session)
        {
            this.session = session;
            // Associate the things
            ImageLoader imageLoader = new LocalImages(session.imagePath);
            Debug.WriteLine("Print an image? {0}", imageLoader.Count());
            imageView = new ImageView(imageLoader);
            setFrameImageView(imageView);
        }

    }
}
