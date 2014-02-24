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
          
            // Loving that hard codedness there Corban :P
            var basePath = "C:\\Users\\Corban\\GIT\\261-imaging-studies-system\\MedImageViewerStudies\\head_mri";
            Uri link = new Uri(basePath + "\\mri_head17.JPG");

            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = link;
            src.EndInit();

            img.Source = src;
        }
    }
}
