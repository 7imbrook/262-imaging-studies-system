using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace _262ImageViewer
{
    [Serializable]
    class StudyMetadata
    {
        public string title;
    }

    public class StudySession
    {

        public Uri workingPath;
        public String fileName;
        private StudyMetadata metadata;

        /**
         * Create a study and initialize with a name and directory path
         * this will create a folder title fileName and inside that folder
         * will be all the images associated with the study and a metadata
         * file that contains layout information and other references.
         */
        public StudySession(Uri filePath, string fileName)
        {
            this.workingPath = new Uri(filePath, fileName + "/");
            this.fileName = fileName;
            if (Directory.Exists(this.workingPath.AbsolutePath))
            {
               throw new IOException("File exists");
            }
            else
            {
                Directory.CreateDirectory(this.workingPath.AbsolutePath);
                this.metadata = new StudyMetadata();
                this.save().Wait();
            }
        }

        public void setTitle(string title)
        {
            this.metadata.title = title;
        }

        async public Task<bool> save()
        {
            var format = new BinaryFormatter();
            Stream stream = new FileStream(this.workingPath.AbsolutePath + this.fileName + ".stud", FileMode.Create, FileAccess.Write, FileShare.None);
            format.Serialize(stream, this.metadata);
            stream.Close();
            return Directory.Exists(this.workingPath.AbsolutePath);
        }
    }
}
