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
        private GridView imageView;
        /*
         * The current StudySession
         */
        public Study studySession;

        /*
         * Given a study, make an image loader and view for it.
         */
        public void loadStudy(Study session)
        {
            // Associate objects together
            ImageLoader imageLoader = new LocalImages(session.imagePath);
            imageView = new GridView(imageLoader, session.imageIndex, session.imageMode);
            studySession = session;
            setFrameImageView(imageView);
        }
    }
}
