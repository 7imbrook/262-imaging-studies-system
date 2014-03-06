﻿using System;
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
        public string fileName;
    }

    public class StudySession
    {
        private StudyMetadata metadata;
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
        public Uri imagePath;


        /**
         * Create a study and initialize with a name and directory path
         * this will create a folder title fileName and inside that folder
         * will be all the images associated with the study and a metadata
         * file that contains layout information and other references.
         */
        public StudySession(Uri filePath, string fileName)
        {
            this.metadata = new StudyMetadata();
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
        public StudySession(Uri studPath)
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

        public bool saveSync()
        {
            var format = new BinaryFormatter();
            Stream stream = new FileStream(this.imagePath.AbsolutePath + this.fileName + ".stud", FileMode.Create, FileAccess.Write, FileShare.None);
            format.Serialize(stream, this.metadata);
            stream.Close();
            return Directory.Exists(this.imagePath.AbsolutePath);
        }

        public override string ToString()
        {
            return this.fileName;
        }

    }
}
