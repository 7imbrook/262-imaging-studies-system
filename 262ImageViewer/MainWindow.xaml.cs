﻿using System;
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
            string fileName = "startup.b";
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
                    var session = new StudySession(new Uri(stud));
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
                this.closeConfirmation();
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
                        var loadedStudy = new StudySession(loadedStudyName);
                        this.loadStudy(loadedStudy);
                    }
                    catch (IOException)
                    {
                        MessageBox.Show("There was a issue with your study, it may be corrupted.");
                    }
                }
            }
        }

        public void setFrameImageView(ImageView iv)
        {
            //this.mainFrame.Source = new Uri("ImageView.xaml", UriKind.Relative);
            //this.mainFrame.ClearValue(Content);
            this.mainFrame.Content = iv;
        }

        private void _NewStudy_Click(object sender, RoutedEventArgs e)
        {
            if (studySession != null)
                this.closeConfirmation();
            this.saveConfirmation();
        }

        //Needed a helper fuction without arguments
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
                    var study = new StudySession(new Uri(path), name);
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
            closeConfirmation();
        }

        private void closeConfirmation()
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save your changes?", "Team Olaf", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Check if current study is saved, and prompt to save if not
                this.studySession.updateState(imageView.index, imageView.modeSelect);
            }/*
            if (result == MessageBoxResult.No)
            {
                // Close main window using "exit" menuItem 
                Close();
            }
            if (result == MessageBoxResult.Cancel)
            {
            }*/
        }

        private void _View_Click(object sender, RoutedEventArgs e)
        {
            if (imageView != null)
            {
                imageView.switchMode();
            }
        }

        private void set_current_default(object sender, RoutedEventArgs e)
        {
            string settingsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!Directory.Exists(settingsDirectory))
                Directory.CreateDirectory(settingsDirectory);
            string fileName = "startup.b";
            var format = new BinaryFormatter();
            Stream stream = new FileStream(settingsDirectory + "/" + fileName, FileMode.Create, FileAccess.Write, FileShare.None);
            format.Serialize(stream, this.studySession.imagePath);
            stream.Close();
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
                    var study = new StudySession(new Uri(path), name);
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





