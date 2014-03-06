﻿using System;
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

        /**
         * Given a study, make an image loader and view for it.
         */
        public void loadStudy(StudySession session)
        {
            // Associate the things
            ImageLoader imageloader = new LocalImages(session.imagePath);
            Debug.WriteLine("Print an image? {0}", imageloader.Count());
            ImageView imageView = new ImageView();
            imageView.addImages(imageloader);
            setFrameImageView();
        }

    }
}
