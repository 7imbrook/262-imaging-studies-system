/* 
 * MainWindow.cs
 * 
 * Version: 
 *     $Id$ 
 * 
 * Revisions: 
 *     $Log$ 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace _262ImageViewer
{
    /*
     * A partial definition for MainWindow
     */
    public partial class MainWindow
    {
        /*
         * The current ImageView
         */
        private ImageView imageView;

        /*
         * The current StudySession
         */
        public StudySession studySession;

        /*
         * Given a study, make an image loader and view for it.
         */
        public void loadStudy(StudySession session)
        {
            // Associate objects together
            ImageLoader imageLoader = new LocalImages(session.imagePath);
            imageView = new ImageView(imageLoader, session.imageIndex, session.imageMode);
            studySession = session;
            setFrameImageView(imageView);
        }
    }
}
