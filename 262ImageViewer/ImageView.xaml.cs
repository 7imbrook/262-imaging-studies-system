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
        private bool modeSelect;
        private int index;    //Counter of image array position
        private ImageLoader localImage;

        public ImageView(ImageLoader imgLdr)
        {
            InitializeComponent();
            index = 0;
            modeSelect = true; //True is one, False is four
            localImage = imgLdr;
            
        }

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
                if (isValidIndex(index + 1))
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
                if (isValidIndex(index - 1))
                {
                    index--;
                    display_image(localImage[index]);
                }
            }
            else
            {
                //display_four(imageList[index]);

            }
        }
        private bool isValidIndex(int i)
        {
            Debug.WriteLine(i);
            return (0 <= i && i < localImage.Count());
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
