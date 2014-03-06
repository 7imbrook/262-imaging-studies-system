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
    public partial class ImageView : Page
    {
        public bool modeSelect;
        public int index;    //Counter of image array position
        public List<BitmapImage> imageList;

        public ImageView()
        {
            InitializeComponent();
            //Application.Current.MainWindow.Height = study.Current.Height + 200;
            //Application.Current.MainWindow.Width = study.Current.Width;
            index = 0;
            modeSelect = true; //True is one, False is four
            imageList = null;
            //display_counter();
        }
        public ImageView(List<BitmapImage> list)
        {
            InitializeComponent();
            index = 0;
            modeSelect = true;
            imageList = list;
        }

        /**
         * Displays the current image the user is in. There is only
         * 1~4 images. Displays by #/4. The number is obtained by the
         * counter that keeps track of the position in the array, except +1
         * to the integer.
         **/
        /*private void display_counter()
        {
            //string position_string = study.position.ToString();
            string position_string = "1";
            string counter_total_string = "2";
            TextBox current_img = new TextBox();
            current_img.Name = "counter";
            current_img.Text = position_string + "/" + counter_total_string;
            //current_img.TextWrapping = TextWrapping.Wrap;
            image_counter.Children.Add(current_img);
        }*/

        /**
         * Displays image based on the array position given by the counter.
         **/
        private void display_image(BitmapImage image)
        {
            image_display.Children.Clear();
            Image i = new Image();

            i.Source = image;
            image_display.Children.Add(i);
        }

        private void display_four()
        {
            image_display.Children.Clear();

            var four_grid = new Grid();

            RowDefinition row1 = new RowDefinition();
            row1.Height = new GridLength(0.5, GridUnitType.Star);
            four_grid.RowDefinitions.Add(row1);

            RowDefinition row2 = new RowDefinition();
            row2.Height = new GridLength(0.5, GridUnitType.Star);
            four_grid.RowDefinitions.Add(row2);

            ColumnDefinition col1 = new ColumnDefinition();
            col1.Width = new GridLength(0.5, GridUnitType.Star);
            four_grid.ColumnDefinitions.Add(col1);

            ColumnDefinition col2 = new ColumnDefinition();
            col2.Width = new GridLength(0.5, GridUnitType.Star);
            four_grid.ColumnDefinitions.Add(col2);

            /*
            for (int position = 0; position < 2; position++)
            {

            }
             */
        }

        /**
         * Increases the image position counter by one, then displays
         * the new image located in the array position determined by
         * the counter. Displays counter number as well.
         **/
        public void nextImage_Click(object sender, RoutedEventArgs e)
        {
            if (modeSelect)
            {
                if (index < imageList.Count)
                {
                    index++;
                    display_image(imageList[index]);
                }
            }
        }

        /**
         * Decreases the image position counter by one, then displays
         * the new image located in the array position determined by
         * the counter. Displays counter number as well.
         **/
        public void prevImage_Click(object sender, RoutedEventArgs e)
        {
            if (modeSelect)
            {
                if(index > imageList.Count)
                {
                    index--;
                    display_image(imageList[index]);
                }
            }
        }

        public void switchMode_Click(object sender, RoutedEventArgs e)
        {
            if (modeSelect == true)
            {

                //Switch from one to four
                double x = (index - 1) / 4;
                int new_index = 4 * (int)Math.Floor(x) + 1;
                index = new_index;
                //display_four(imageList[index]);
                prev_button.IsEnabled = false;
                next_button.IsEnabled = false;
                modeSelect = false;
            }
            else
            {
                //Switch from four to one
                index = index - 3;

                if (index < 0)
                {
                    index = 0;
                }
                else if (index > imageList.Count)
                {
                    index = imageList.Count;
                }

                display_image(imageList[index]);

                prev_button.IsEnabled = true;
                next_button.IsEnabled = true;
                modeSelect = true;
            }

        }
    }
}
