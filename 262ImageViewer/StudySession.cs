﻿/* 
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
    /**
     * StudyMetadata will besaved and restored from disk. Any data kept
     * in the StudySession object will have to be recreated.
     */
    [Serializable]
    class StudyMetadata
    {
        public string fileName = "";
        public int index = 0;
        public bool mode = true;
    }

    public class Study
    {
        // The root Action
        public Action.Action rootAction;

        //
        public MainWindow mainWindow;

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

            // Debugging stuff
            string debug = this.rootAction.ToString();
            Debug.WriteLine(debug);
        }

        public void undoAction()
        {
            if (this.rootAction == null)
            {
                return;
            }
            // Edge case
            if(this.rootAction.next() == null)
            {
                this.rootAction.undo(this.mainWindow);
                this.rootAction = null;
                return;
            }
            Action.Action p = rootAction;
            Action.Action r = rootAction;
            while (r.next() != null) { 
                p = r;
                r = r.next();
            }
            r.undo(this.mainWindow);
            p.removeNext();

            // Debugging stuff
            string debug = this.rootAction.ToString();
            Debug.WriteLine(debug);
        }

        // Study Metadata object
        private StudyMetadata metadata;
        // The filename stored in the metadata object
        private string fileName
        {
            get
            {
                return this.metadata.fileName;
            }
            set
            {
                this.metadata.fileName = value;
            }
        }
        // The current image index stored in metadata
        public int imageIndex
        {
            get
            {
                return this.metadata.index;
            }
            private set
            {
                
            }
        }
        // The current mode stored in metadata
        public bool imageMode
        {
            get
            {
                return this.metadata.mode;
            }
            private set
            {

            }
        }
        // Image locations
        public Uri imagePath;
        // The current active path of the stud file
        public string currentPath
        {
            get
            {
                return (this.imagePath.AbsolutePath + "/" + this.fileName + ".stud");
            }
            private set {}
        }

        /**
         * Create a study and initialize with a name and directory path
         * this will create a folder title fileName and inside that folder
         * will be all the images associated with the study and a metadata
         * file that contains layout information and other references.
         */
        public Study(Uri filePath, string fileName)
        {
            this.metadata = new StudyMetadata();
            this.metadata.index = 0;
            this.metadata.mode = true;
            this.fileName = fileName;
            this.imagePath = new Uri(filePath, fileName + "/");
            if (Directory.Exists(this.imagePath.AbsolutePath))
            {
               throw new IOException("File exists");
            }
            else
            {
                Directory.CreateDirectory(this.imagePath.AbsolutePath);
                this.saveSync();
            }
        }

        /**
         * Given a .stud file path, creates a studySession with that information
         */
        public Study(Uri studPath)
        {
            this.imagePath = new Uri(System.IO.Path.GetDirectoryName(studPath.AbsolutePath));
            var format = new BinaryFormatter();
            var dataStream = new FileStream(studPath.AbsolutePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            try
            {
                this.metadata = (StudyMetadata)format.Deserialize(dataStream);
            }
            catch
            {
                throw new IOException("Invalid file");
            }
            dataStream.Close();
        }

        /**
         * Updates the state and save the file.
         */
        public void updateState(int index, bool mode)
        {
            this.metadata.index = index;
            this.metadata.mode = mode;
            this.saveSync();
        }

        /**
         * Save the session information to disk
         */
        public bool saveSync()
        {
            BinaryFormatter format = new BinaryFormatter();
            Stream stream = new FileStream(this.imagePath.AbsolutePath + "/" + this.fileName + ".stud", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            format.Serialize(stream, this.metadata);
            stream.Close();
            return File.Exists(this.imagePath.AbsolutePath);
        }

        public override string ToString()
        {
            return this.fileName;
        }

    }
}
