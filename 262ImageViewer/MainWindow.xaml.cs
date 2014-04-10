using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Drawing;


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
                    var study = new Study(path);
                    this.loadStudy(study);
                } 
                catch
                {
                    this.openStudyDialog();
                }
            }
            if (studySession == null)
            {
                _OpenStudy_Click(this, new RoutedEventArgs());
            }

            // Run the previous actions to return to state
            if (this.studySession.rootAction != null)
                this.studySession.rootAction.run(this);

        }

        private void openStudyDialog()
        {
            // Create OpenFileDialog
            var dlg = new System.Windows.Forms.FolderBrowserDialog();

            // Display OpenFileDialog by calling ShowDialog method
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Uri studyDir = new Uri(dlg.SelectedPath);
                Debug.WriteLine(studyDir.ToString());
                var study = new Study(studyDir);
                this.loadStudy(study);
            }
        }

        private void _OpenStudy_Click(object sender, RoutedEventArgs e)
        {
            this.openStudyDialog();
        }

        public void setFrameImageView(Page iv)
        {
            this.mainFrame.Content = iv;
        }

        private void _NewStudy_Click(object sender, RoutedEventArgs e)
        {
            if (studySession != null)
            {
                this.closeConfirmation();
                this.studySession = null;
            }
            this.openStudyDialog();
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
                this.studySession.saveSync();
            }
        }

        // Lets change the name of this...
        private void _View_ToggleImageLayout(object sender, RoutedEventArgs e)
        {
            if (imageView != null)
            {
                var a = new Action.Grid.Toggle();
                this.studySession.addAction(a);
                a.run(this);
            }
        }

        private void _View_CreateAnalysisView(object sender, RoutedEventArgs e)
        {
            if (studySession != null)
            {
                Bitmap bi = null;// new Bitmap();
                var a = new Action.Analysis.Create(bi);
                this.studySession.addAction(a);
                a.run(this);
            }
        }

        private void _View_Undo(object sender, RoutedEventArgs e)
        {
            this.studySession.undoAction();
        }

        private void _View_Reconstruction(object sender, RoutedEventArgs e)
        {
            var a = new Action.Reconstruction.Create(this, studySession);
            this.studySession.addAction(a);
            a.run(this);
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
            this.studySession.saveSync();
            MessageBox.Show("Saved.");
        }

        private void _saveAs(object sender, RoutedEventArgs e)
        {
            // Copy...
        }
    }
}





