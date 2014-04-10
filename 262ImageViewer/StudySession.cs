/* 
 * ImageLoader.cs
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
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace _262ImageViewer
{
    public class Study
    {
        // The root Action
        public Action.Action rootAction;
        // using in the undoing of actions
        public MainWindow mainWindow;

        // Image locations
        public Uri imagePath;

        // The Image Collection
        public List<BitmapImage> imageCollection;

        /**
         * Given a path looks for a .studyinfo file to load study data ->  open images
         */
        public Study(Uri studPath)
        {
            this.imagePath = studPath;

            // Check if there's a saved state and load if there is
            string fileName = ".studyinfo";
            string settingsPath = System.IO.Path.Combine(studPath.AbsolutePath, fileName);
            if (File.Exists(settingsPath))
            {     
                try
                {
                    var format = new BinaryFormatter();
                    var dataStream = new FileStream(studPath.AbsolutePath + "/" + fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                    this.rootAction = (Action.Action)format.Deserialize(dataStream);
                    dataStream.Close();
                }
                catch
                {
                    throw new IOException("Invalid file");
                }
            }
            // Load image proxies
            this.imageCollection = new List<BitmapImage>();

            string[] fileArray = Directory.GetFiles(this.imagePath.AbsolutePath);
            foreach (string file in fileArray)
            {
                string lf = file.ToLower();
                if (lf.EndsWith(".jpg") || lf.EndsWith(".acr"))
                {
                    var img = new Uri(file);
                    BitmapImage bmp = new BitmapImage(img);
                    this.imageCollection.Add(bmp);
                }
            }
        }

        /**
         * Save the session information to disk
         */
        public bool saveSync()
        {
            if (this.rootAction == null)
                return false;
            Debug.WriteLine("Saving chain: {0}", this.rootAction);
            BinaryFormatter format = new BinaryFormatter();
            Stream stream = new FileStream(this.imagePath.AbsolutePath + "/.studyinfo", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            format.Serialize(stream, this.rootAction);
            stream.Close();
            return File.Exists(this.imagePath.AbsolutePath);
        }

        /**
        * adds an action to the chain
        */
        public void addAction(Action.Action action)
        {
            if (this.rootAction == null)
            {
                this.rootAction = action;
                return;
            }
            Action.Action l = rootAction;
            while (l.next() != null)
                l = l.next();
            l.setNext(action);
        }

        /**
         * undoes and pop an action
         */
        public void undoAction()
        {
            if (this.rootAction == null)
            {
                return;
            }
            // Edge case
            if (this.rootAction.next() == null)
            {

                this.rootAction.undo(mainWindow);
                this.rootAction = null;
                return;
            }
            Action.Action p = rootAction;
            Action.Action r = rootAction;
            while (r.next() != null)
            {
                p = r;
                r = r.next();
            }

            r.undo(mainWindow);
            p.removeNext();

            // Debugging stuff
            string debug = this.rootAction.ToString();
            Debug.WriteLine(debug);
        }

    }
}
