using System;
using System.Collections.Generic;
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

        private void OpenStudy_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".stud";
            dlg.Filter = "Studies (.stud)|*.stud";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {
                // Open document
                Uri loadedStudyName = new Uri(dlg.FileName);
                if (loadedStudyName.AbsolutePath.ToLower().EndsWith(".stud"))
                {
                    MessageBox.Show("Send to Timbrook to deserialize: " + loadedStudyName);
                    StudySession loadedStudy = new StudySession(loadedStudyName);
                    //Study loadedStudy = Study(loadedStudyName);
                }
            }
        }

        private void NewStudy_Click(object sender, RoutedEventArgs e)
        {
            // Create a new study
            MessageBox.Show("Hey there. Let's make a new study.");
            var newImageView = new ImageView();
            newImageView.Show();
        }
    }
}
