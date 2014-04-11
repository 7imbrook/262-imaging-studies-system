﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;

namespace _262ImageViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public ReconstructionView reconstructionView;

        /*
         * The main constructor for MainWindow.
         */
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
            // Set the root path, will be the same a the imediatly opened study
            this.rootPath = studySession.imagePath;
        }

        /*
         * Called when the window has loaded. 
         * Used to reload the study's state.
         */
        private void LoadedWindow(object sender, RoutedEventArgs e)
        {
            // Run the previous actions to return to state
            if (this.studySession.rootAction != null)
                this.studySession.rootAction.run(this);

            // Refreash tree
            this.populateTreeView();
        }

        /*
         * Prompt the user to select a study.
         */
        private void openStudyDialog()
        {
            // Create Folder Selection Dialog
            var dlg = new System.Windows.Forms.FolderBrowserDialog();

            // Display the dialog by calling ShowDialog method
            System.Windows.Forms.DialogResult result = dlg.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Uri studyDir = new Uri(dlg.SelectedPath);
                Debug.WriteLine(studyDir.ToString());
                var study = new Study(studyDir);
                this.loadStudy(study);
                this.rootPath = study.imagePath;
            }
            else
            {
                // Close the app if they cancel.
                //Application.Current.Shutdown();
                Process.GetCurrentProcess().Kill();
            }
        }

        /*
         * Use openStudyDialog to prompt the user.
         */
        private void _OpenStudy_Click(object sender, RoutedEventArgs e)
        {
            this.openStudyDialog();
            this.populateTreeView();
        }

        /*
         * Set MainWindow's Frame to the given Page.
         */
        public void setFrameImageView(Page iv)
        {
            this.mainFrame.Content = iv;
        }

        /*
         * Prompt the user to select a new folder.
         */
        private void _NewStudy_Click(object sender, RoutedEventArgs e)
        {
            if (studySession != null)
            {
                this.closeConfirmation();
                this.studySession = null;
            }
            this.openStudyDialog();
        }

        /*
         * Close the window.
         */
        private void _Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /*
         * Handle the close action to prompt to save.
         */
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (studySession != null)
            {
                this.closeConfirmation();
            }
        }

        /*
         * Prompt the user to save the study.
         */
        private void closeConfirmation()
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save your changes?", "Save", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Check if current study is saved, and prompt to save if not
                this.studySession.saveSync();
            }
        }

        /*
         * Toggle between 1- and 4-up display modes.
         */
        private void _View_ToggleImageLayout(object sender, RoutedEventArgs e)
        {
            if (imageView != null)
            {
                var a = new Action.Grid.Toggle();
                this.studySession.addAction(a);
                a.run(this);
            }
        }

        /*
         * Create a new analysis on the current image.
         */
        private void _View_CreateAnalysisView(object sender, RoutedEventArgs e)
        {
            if (studySession != null)
            {
                Bitmap bi = studySession.imageCollection[this.imageView.index].getImage();
                var a = new Action.Analysis.Create(bi);
                a.run(this);
            }
        }

        /*
         * Undo the previous action.
         */
        private void _View_Undo(object sender, RoutedEventArgs e)
        {
            this.studySession.undoAction();
        }

        /*
         * Create a new reconstruction of the current study.
         */
        private void _View_Reconstruction(object sender, RoutedEventArgs e)
        {
            var a = new Action.Reconstruction.Create();
            this.studySession.addAction(a);
            a.run(this);
        }

        /*
         * Begin the windowing process.
         */
        private void _View_Windowing(object sender, RoutedEventArgs e)
        {
            var a = new Action.Windowing.Create(this);
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

        /*
         * Save the open study.
         */
        private void _saveStudy(object sender, RoutedEventArgs e)
        {
            this.studySession.saveSync();
            MessageBox.Show("Saved.");
        }

        /*
         * Save the current study under a new name.
         */
        private void _saveAs(object sender, RoutedEventArgs e)
        {
            // Copy...
        }

        /*
         * Select a new study from the TreeView.
         */
        private void _select_study(object sender, RoutedEventArgs e)
        {
            var treeView = (TreeView)sender;
            if(treeView.SelectedItem != null)
            {
                var item = (TreeViewItem)treeView.SelectedItem;
                Debug.WriteLine("{0}:{1}", item.Header, item.ToolTip);
                var study = new Study((Uri)item.ToolTip);
                this.studySession.saveSync();
                this.loadStudy(study);
                if (this.studySession.rootAction != null)
                    this.studySession.rootAction.run(this);
            }
        }
    }
}





