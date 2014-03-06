using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using _262ImageViewer;

namespace _262ImageViewer_tests
{
    [TestClass]
    public class UnitTest1
    {
        /**
         * Creates a file!
         */
        [TestMethod]
        public void testCreateFile()
        {
            var filePath = new Uri("file:///C:/Users/michael/Documents/Studies/");
            var final = new Uri(filePath, "test_project/");
            // Delete File
            if (Directory.Exists(final.AbsolutePath))
                Directory.Delete(final.AbsolutePath, true);
            // Create File
            StudySession testStudy = new StudySession(filePath, "test_project");
            if (!Directory.Exists(final.AbsolutePath)) throw new Exception("Not created");
        }
        
        /**
         * Test open file
         */
        [TestMethod]
        public void testOpenFile()
        {
            var filePath = new Uri("file:///C:/Users/michael/Documents/Studies/");
            var final = new Uri(filePath, "test_project/");
            // Delete File
            if (Directory.Exists(final.AbsolutePath))
                Directory.Delete(final.AbsolutePath, true);
            // Create File
            StudySession testStudy = new StudySession(filePath, "test_project");
            if (!Directory.Exists(final.AbsolutePath)) throw new Exception("Not created");

            // Open the file
            var studFile = new Uri(final, "test_project.stud");
            var studyLoaded = new StudySession(studFile);
            if (studyLoaded.ToString() != "test_project") throw new Exception("File loaded improperly");
        }

        /**
         * Test bad paths for Studies
         */
        [TestMethod]
        public void testBadPath()
        {
            var path = new Uri("file:///C:/Users/michael/Documents/Studies/not_there.stud");
            try
            {
                var testStudy = new StudySession(path);
                throw new Exception("Study was created");
            }
            catch (FileNotFoundException e)
            { }
        }

    }
}
