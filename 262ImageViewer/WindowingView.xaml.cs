using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;

namespace _262ImageViewer
{
    /// <summary>
    /// Interaction logic for WindowingView.xaml
    /// </summary>
    public partial class WindowingView : Page
    {
        public WindowingView()
        {
            InitializeComponent();
            //index = i;
            //Get image from WindowedImage
            //display_image() or display_four()
        }
        /**
        private Bitmap getProcessedImage(int high, int low)
        {
            
            return WindowedImage.processImage(high, low, imge);
        }

        private void display_image(Bitmap image) //Take in processed img
        {
            image_display.Children.Clear();
            System.Windows.Controls.Image i = new System.Windows.Controls.Image();
            i.Source = image;

            
        }

        private void display_four(Bitmap image) //Take in processed imgs
        {
            image_display.Children.Clear();
        }
        **/
    }
}
