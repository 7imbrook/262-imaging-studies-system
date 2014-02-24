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
        //Placeholer "Study"
        BitmapImage[] image_list = new BitmapImage[4];

        public ImageView()
        {
            InitializeComponent();
            //Placeholder pictures for "Study"
            /* Old code remains I don't want to chuck yet
            BitmapImage image_0 = new BitmapImage(new Uri("picture.jpg", UriKind.Relative));
            BitmapImage image_1 = new BitmapImage(new Uri("picture2.jpg", UriKind.Relative));
            BitmapImage image_2 = new BitmapImage(new Uri("picture3.jpg", UriKind.Relative));
            BitmapImage image_3 = new BitmapImage(new Uri("picture4.jpg", UriKind.Relative));
            */
            BitmapImage image_0 = new BitmapImage();
            BitmapImage image_1 = new BitmapImage();
            BitmapImage image_2 = new BitmapImage();
            BitmapImage image_3 = new BitmapImage();

            image_list[0] = image_0;
            image_list[1] = image_1;
            image_list[2] = image_2;
            image_list[3] = image_3;
            //
            counter = 1;
            display_counter();
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
            current_img.Text = counter_string + "/4";
            current_img.TextWrapping = TextWrapping.Wrap;
            image_counter.Children.Add(current_img);
        }

        /**
         * Displays image based on the array position given by the counter.
         **/
        private void display_image(int position)
        {
            image_display.Children.Clear(); //Clear current image(s)
            Image i = new Image();
            
            //Source of the image is located in the URI referenced by the BitmapImage...
            BitmapImage img_src = image_list[position - 1];
            i.Width = 500; //Placeholder, need to check requirements
            i.Source = img_src;
            i.Stretch = Stretch.Uniform;
            //int x = img_src.PixelWidth; //Renders image
            image_display.Children.Add(i);
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
                display_image(counter);
            }

            else
            {
                counter++;
                display_counter();
                display_image(counter);
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
                display_image(counter);
            }
            else
            {
                counter--;
                display_counter();
                display_image(counter);
            }
        }
    }
}
