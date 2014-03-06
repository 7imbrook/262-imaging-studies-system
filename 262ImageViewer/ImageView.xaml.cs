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
using System.Diagnostics;

namespace _262ImageViewer
{
    /// <summary>
    /// Interaction logic for ImageView.xaml
    /// </summary>
    public partial class ImageView : Page
    {
        public bool modeSelect
        {
            get;
            private set;
        }
        public int index
        {
            get;
            private set;
        }
        private ImageLoader imageLoader;

        public ImageView(ImageLoader imgLdr)
        {
            InitializeComponent();
            index = 0;
            modeSelect = true; //True is one, False is four
            imageLoader = imgLdr;
            if (isValidIndex(index))
            {
                display_image(imageLoader[index]);
            }
        }

        public ImageView(ImageLoader imgLdr, int i, bool mode)
        {
            InitializeComponent();
            index = i;
            modeSelect = mode; //True is one, False is four
            imageLoader = imgLdr;
            if (isValidIndex(index))
            {
                display_image(imageLoader[index]);
            }
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

        private void display_four(ImageLoader imageList, int index)
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

                        
            for (int position = 0; position < 2; position++)
            {
                if (isValidIndex(index))
                {
                    Image to_display = new Image();
                    BitmapImage source = imageList[index];
                    to_display.Source = source;
                    to_display.Stretch = Stretch.Uniform;
                    int x = source.PixelWidth;
                    Grid.SetRow(to_display, 0);
                    Grid.SetColumn(to_display, position);
                    four_grid.Children.Add(to_display);
                    index++;
                }
            }

            for (int position = 0; position < 2; position++)
            {
                if (isValidIndex(index))
                {
                    Image to_display = new Image();
                    BitmapImage source = imageList[index++];
                    to_display.Source = source;
                    to_display.Stretch = Stretch.Uniform;
                    int x = source.PixelWidth;
                    Grid.SetRow(to_display, 1);
                    Grid.SetColumn(to_display, position);
                    four_grid.Children.Add(to_display);
                }
            }

            image_display.Children.Add(four_grid);
            
        }

        /**
         * Increases the image position counter by one, then displays
         * the new image located in the array position determined by
         * the counter. Displays counter number as well.
         **/
        public void nextImage_Click(object sender, RoutedEventArgs e)
        {

            if (modeSelect && imageLoader != null)
            {
                if (isValidIndex(index + 1))
                {
                    index++;
                    display_image(imageLoader[index]);
                }
            }
            else
            {
                index += 4;
                display_four(imageLoader, index);
            }

        }

        /**
         * Decreases the image position counter by one, then displays
         * the new image located in the array position determined by
         * the counter. Displays counter number as well.
         **/
        public void prevImage_Click(object sender, RoutedEventArgs e)
        {
            if (modeSelect && imageLoader != null)
            {
                if (isValidIndex(index - 1))
                {
                    index--;
                    display_image(imageLoader[index]);
                }
            }
            else
            {
                index -= 4;
                display_four(imageLoader, index);
            }
        }
        private bool isValidIndex(int i)
        {
            return (0 <= i && i < imageLoader.Count());
        }

        public void switchMode()
        {
            if (modeSelect)
            {

                //Switch from one to four
                double x = (index) / 4;
                int new_index = 4 * (int)Math.Floor(x) + 1;
                index = new_index;
                display_four(imageLoader, index);
                modeSelect = false;
            }
            else
            {
                //Switch from four to one
                if (index < 0)
                {
                    index = 0;
                }
                else if (index > imageLoader.Count())
                {
                    index = imageLoader.Count();
                }

                display_image(imageLoader[index]);

                modeSelect = true;
            }

        }

    }
}
