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
        public GridView imageView;

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
            imageView = new GridView(session.imageCollection, 0, true);
            studySession = session;
            studySession.mainWindow = this;
            setFrameImageView(imageView);
        }
    }
}
