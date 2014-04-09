using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace _262ImageViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            string settingsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string fileName = "MedicalImageViewer.bin";
            string settingsPath = System.IO.Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath))
            {
                try
                {
                    var format = new BinaryFormatter();
                    var dataStream = new FileStream(settingsPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    Uri path = (Uri)format.Deserialize(dataStream);
                    dataStream.Close();
                    string stud = "";
                    string[] fileArray = Directory.GetFiles(path.AbsolutePath);
                    foreach (string file in fileArray)
                    {
                        if (file.EndsWith(".stud"))
                        {
                            stud = file;
                            break;
                        }
                    }
                    var session = new Study(new Uri(stud));
                    this.loadStudy(session);
                }
                catch
                {
                }
            }

            if (studySession == null)
            {
                _OpenStudy_Click(this, new RoutedEventArgs());
            }
        }

        private void _OpenStudy_Click(object sender, RoutedEventArgs e)
        {
            if (studySession != null)
            {
                this.closeConfirmation();
            }
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".stud";
            dlg.Filter = "Studies (.stud)|*.stud";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if ((bool)result)
            {
                // Open document
                Uri loadedStudyName = new Uri(dlg.FileName);
                if (loadedStudyName.AbsolutePath.ToLower().EndsWith(".stud"))
                {
                    try
                    {
                        var loadedStudy = new Study(loadedStudyName);
                        this.loadStudy(loadedStudy);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("There was a issue with your study, it may be corrupted.");
                    }
                }
            }
        }

        public void setFrameImageView(GridView iv)
        {
            this.mainFrame.Content = iv;
        }

        private void _NewStudy_Click(object sender, RoutedEventArgs e)
        {
            if (studySession != null)
            {
                this.closeConfirmation();
            }
            this.saveConfirmation();
        }

        // Needed a helper fuction without arguments
        private void saveConfirmation()
        {
            // Prompt where to save the images
            var savePrompt = new Microsoft.Win32.SaveFileDialog();

            savePrompt.DefaultExt = "";
            savePrompt.Filter = "";

            Nullable<bool> result = savePrompt.ShowDialog();
            if ((bool)result)
            {
                var path = savePrompt.FileName;
                var name = path.Split('\\').Last();
                try
                {
                    var study = new Study(new Uri(path), name);
                    this.loadStudy(study);
                }
                catch (IOException)
                {
                    MessageBox.Show("There was a issue with your study, it may be corrupted.");
                }
            }
        }

        private void _Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (studySession != null)
            {
                this.closeConfirmation();
            }
        }

        private void closeConfirmation()
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save your changes?", "Save", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Check if current study is saved, and prompt to save if not
                this.studySession.updateState(imageView.index, imageView.modeSelect);
            }
        }

        // Lets change the name of this...
        private void _View_ToggleImageLayout(object sender, RoutedEventArgs e)
        {
            if (imageView != null)
            {
                var a = new Action.Grid.Toggle(imageView);
                a.run(studySession);
            }
        }

        private void _View_Undo(object sender, RoutedEventArgs e)
        {
            this.studySession.undoAction();
        }

        /*
         * Set the current study as the default startup Study.
         */
        private void set_current_default(object sender, RoutedEventArgs e)
        {
            if (this.studySession != null)
            {
                string settingsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                if (!Directory.Exists(settingsDirectory))
                    Directory.CreateDirectory(settingsDirectory);
                string fileName = "MedicalImageViewer.bin";
                var format = new BinaryFormatter();
                Stream stream = new FileStream(settingsDirectory + "/" + fileName, FileMode.Create, FileAccess.Write, FileShare.None);
                format.Serialize(stream, this.studySession.imagePath);
                stream.Close();
                MessageBox.Show("Default study set.");
            }
            else
            {
                MessageBox.Show("You must first open a study.");
            }
        }

        /*
         * Clear the default startup study.
         */
        private void clear_default_study(object sender, RoutedEventArgs e)
        {
            string settingsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string fileName = "MedicalImageViewer.bin";
            string settingsPath = System.IO.Path.Combine(settingsDirectory, fileName);
            if (File.Exists(settingsPath))
            {
                try
                {
                    File.Delete(settingsPath);
                    MessageBox.Show("Default study cleared.");
                }
                catch
                {
                    MessageBox.Show("An error occured deleting the startup file.\nPlease delete the file:\n\"" +
                        settingsPath + "\"");
                }
            }
        }

        private void _saveStudy(object sender, RoutedEventArgs e)
        {
            this.studySession.updateState(imageView.index, imageView.modeSelect);
        }

        private void _saveAs(object sender, RoutedEventArgs e)
        {
            var savePrompt = new Microsoft.Win32.SaveFileDialog();
            var curretImgPath = studySession.imagePath;
            var i = studySession.imageIndex;
            var s = studySession.imageMode;
            savePrompt.DefaultExt = "";
            savePrompt.Filter = "";

            Nullable<bool> result = savePrompt.ShowDialog();
            if ((bool)result)
            {
                var path = savePrompt.FileName;
                var name = path.Split('\\').Last();
                try
                {
                    var study = new Study(new Uri(path), name);
                    study.updateState(i, s);
                    // Move files
                    string[] fileArray = Directory.GetFiles(curretImgPath.AbsolutePath);
                    foreach (string file in fileArray)
                    {
                        if (file.EndsWith(".jpg"))
                        {
                            System.IO.File.Copy(file, System.IO.Path.Combine(study.imagePath.AbsolutePath, System.IO.Path.GetFileName(file)));
                        }
                    }
                    this.loadStudy(study);
                }
                catch (IOException)
                {
                    MessageBox.Show("There was a issue with your study, it may be corrupted.");
                }
            }
        }
    }
}





