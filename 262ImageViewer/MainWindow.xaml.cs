using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

namespace _262ImageViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //InitializeComponent();
            //Study studyTest = new Study();
        }

        private void _OpenStudy_Click(object sender, RoutedEventArgs e)
        {
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
                    StudySession loadedStudy = new StudySession(loadedStudyName);
                    this.loadStudy(loadedStudy);
                }
            }
        }

        private void _NewStudy_Click(object sender, RoutedEventArgs e)
        {
            // Prompt where to save the images
            var savePromt = new Microsoft.Win32.SaveFileDialog();

            savePromt.DefaultExt = "";
            savePromt.Filter = "";

            Nullable<bool> result = savePromt.ShowDialog();
            if ((bool)result)
            {
                var path = savePromt.FileName;
                var name = path.Split('\\').Last();

                var study = new StudySession(new Uri(path), name);
                
            }
        }

        private void _Close_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save your changes?", "Team Olaf", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                // Check if current study is saved, and prompt to save if not
                var savePromt = new Microsoft.Win32.SaveFileDialog();

                savePromt.DefaultExt = "";
                savePromt.Filter = "";

                Nullable<bool> answer = savePromt.ShowDialog();
                if ((bool)answer)
                {
                    var path = savePromt.FileName;
                    var name = path.Split('\\').Last();

                    var study = new StudySession(new Uri(path), name);

                }
            }
            if (result == MessageBoxResult.No)
            {
                // Close main window using "exit" menuItem 
                Close();
            }
            if (result == MessageBoxResult.Cancel)
            {

            }
        }
    }
}
