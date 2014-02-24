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
using System.Windows.Shapes;

namespace _262ImageViewer
{
    /// <summary>
    /// Interaction logic for ImageView.xaml
    /// </summary>
    public partial class ImageView : Window
    {

        int counter;    //Counter of image array position

        public ImageView()
        {
            InitializeComponent();
            counter = 0;
        }

        /**
         * Displays the current image the user is in. There is only
         * 1~4 images. Displays by #/4. The number is obtained by the
         * counter that keeps track of the position in the array, except +1
         * to the integer.
         **/
        private void display_counter()
        {
            string counter_string = counter.ToString();
            TextBox current_img = new TextBox();
            current_img.Name = "counter";
            current_img.Text = counter_string;
            current_img.TextWrapping = TextWrapping.Wrap;
            image_counter.Children.Add(current_img);
        }

        /**
         * Increases the image position counter by one, then displays
         * the new image located in the array position determined by
         * the counter. Displays counter number as well.
         **/
        private void nextImage_Click(object sender, RoutedEventArgs e)
        {
            if(counter >= 4)
            {
                counter = 4;
                display_counter();
            }

            else
            {
                counter++;
                display_counter();
            }
        }

        /**
         * Decreases the image position counter by one, then displays
         * the new image located in the array position determined by
         * the counter. Displays counter number as well.
         **/
        private void prevImage_Click(object sender, RoutedEventArgs e)
        {
            if (counter <= 1)
            {
                counter = 1;
                display_counter();
            }
            else
            {
                counter--;
                display_counter();
            }
        }
    }
}
