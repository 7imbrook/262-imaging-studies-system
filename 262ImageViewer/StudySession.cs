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
        public Uri workingPath;
        public String fileName;
    }

    public class StudySession
    {
        private StudyMetadata metadata;

        public Uri imagePath
        {
            get
            {
                return this.metadata.workingPath;
            }
        }


        /**
         * Create a study and initialize with a name and directory path
         * this will create a folder title fileName and inside that folder
         * will be all the images associated with the study and a metadata
         * file that contains layout information and other references.
         */
        public StudySession(Uri filePath, string fileName)
        {
            this.metadata = new StudyMetadata();
            this.metadata.workingPath = new Uri(filePath, fileName + "/");
            this.metadata.fileName = fileName;
            if (Directory.Exists(this.metadata.workingPath.AbsolutePath))
            {
               throw new IOException("File exists");
            }
            else
            {
                Directory.CreateDirectory(this.metadata.workingPath.AbsolutePath);
                this.saveSync();
            }
        }

        /**
         * Given a .stud file path, creates a studySession with that information
         */
        public StudySession(Uri studPath)
        {
            var format = new BinaryFormatter();
            var dataStream = new FileStream(studPath.AbsolutePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            this.metadata = (StudyMetadata)format.Deserialize(dataStream);
            dataStream.Close();
        }

        public bool saveSync()
        {
            var format = new BinaryFormatter();
            Stream stream = new FileStream(this.metadata.workingPath.AbsolutePath + this.metadata.fileName + ".stud", FileMode.Create, FileAccess.Write, FileShare.None);
            format.Serialize(stream, this.metadata);
            stream.Close();
            return Directory.Exists(this.metadata.workingPath.AbsolutePath);
        }

        public override string ToString()
        {
            return this.metadata.fileName;
        }

    }
}
