﻿using System;
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
using System.Diagnostics;

namespace _262ImageViewer
{
    /// <summary>
    /// Interaction logic for ImageView.xaml
    /// </summary>
    public partial class ImageView : Page
    {
        public bool modeSelect;
        public int index;    //Counter of image array position
        public ImageLoader localImage;

        public ImageView()
        {
            InitializeComponent();
            //Application.Current.MainWindow.Height = study.Current.Height + 200;
            //Application.Current.MainWindow.Width = study.Current.Width;
            index = 0;
            modeSelect = true; //True is one, False is four
            localImage = null;
            //display_counter();
        }

        public ImageView(ImageLoader imgLdr)
        {
            InitializeComponent();
            //Application.Current.MainWindow.Height = study.Current.Height + 200;
            //Application.Current.MainWindow.Width = study.Current.Width;
            index = 0;
            modeSelect = true; //True is one, False is four
            localImage = imgLdr;
            //display_counter();
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
            if (modeSelect && localImage != null)
            {
                if (index < localImage.Count())
                {
                    index++;
                    display_image(localImage[index]);
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
            if (modeSelect && localImage != null)
            {
                if (index < localImage.Count())
                {
                    index--;
                    display_image(localImage[index]);
                }
            }
        }

        public void switchMode()
        {
            if (modeSelect)
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
                else if (index > localImage.Count())
                {
                    index = localImage.Count();
                }

                display_image(localImage[index]);

                prev_button.IsEnabled = true;
                next_button.IsEnabled = true;
                modeSelect = true;
            }

        }

    }
}
