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
using System.Windows.Controls;
using System.Windows.Forms;
using System.IO;

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
         * The root path from open
         */
        public Uri rootPath;

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

        private void populateTreeView()
        {
            this.studyTree.Items.Clear();
            this.studyTree.Items.Add(treeAtPath(this.rootPath.LocalPath));  
        }

        private TreeViewItem treeAtPath(string path)
        {
            string[] pathComp = path.Split('\\');
            string rootStudyName = pathComp[pathComp.Length - 1];
            TreeViewItem item = new TreeViewItem() { Header = rootStudyName, ToolTip = new Uri(path) };
            string[] subs = Directory.GetDirectories(path);
            foreach(string ssub in subs)
            {
                item.Items.Add(treeAtPath(ssub));
            }
            return item;
        }

    }
}
