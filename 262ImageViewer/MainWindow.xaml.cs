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
            InitializeComponent();
            Image img = this.testImage;
            Study studyTest = new Study();

            var basePath = "C:\\Users\\Steven\\MedImageViewerStudies\\head_mri";
            Uri link = new Uri(basePath + "\\mri_head17.JPG");

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = link;
            src.EndInit();
            List<string> fileNames = new List<string>();
            fileNames.Add("\\mri_head14.JPG");
            fileNames.Add("\\mri_head41.JPG");
            LocalImages images = new LocalImages("C:\\Users\\Steven\\MedImageViewerStudies\\head_mri", fileNames);
            foreach (BitmapImage b in images)
            {
                img.Source = b;
            }
            //img.Source = src;
        }
    }
}
