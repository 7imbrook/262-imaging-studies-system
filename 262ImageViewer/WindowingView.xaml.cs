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
        /*
         * Selects which display mode the ImageView is in.
         * True = 1-up
         * False = 4-up
         */
        public bool modeSelect
        {
            get;
            private set;
        }

        /*
         * The current index of the BitmapImage list.
         */
        public int index;
        public float high, low;


        private List<ImageLoader.Image> imageLoader;

        MainWindow main;

        public WindowingView(float h, float l, MainWindow win)
        {
            InitializeComponent();
            main = win;
            index = win.imageView.index;
            modeSelect = win.imageView.modeSelect;
            imageLoader = win.studySession.imageCollection;
            high = h;
            low = l;
            
            if(isValidIndex(index))
            {
                display_image(imageLoader[index], h, l);
            }
        }

        private void display_image(ImageLoader.Image image, float high, float low) //Take in processed img
        {
            image_display.Children.Clear();

            System.Windows.Controls.Image i = new System.Windows.Controls.Image();

            BitmapSource wSource = windowedImage(high, low, image.getImage());


            i.Source = wSource;
            // If the image won't fit at native resolution, scale it.
            if (Application.Current.MainWindow.ActualHeight < image.getHeight() ||
                Application.Current.MainWindow.ActualWidth < image.getWidth())
            {
                i.Stretch = Stretch.Uniform;
            }
            else
            {
                i.Stretch = Stretch.None;
            }
            image_display.Children.Add(i);
            
        }

        private void display_four(List<ImageLoader.Image> imageList, int index) //Take in processed imgs
        {
            //Clear any leftover images.
            image_display.Children.Clear();

            var four_grid = new Grid();

            //Create the 2x2 grid.
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

        }

        private void nextImage_Click(object sender, RoutedEventArgs e)
        {
            var a = new Action.Windowing.Next(this);
            MainWindow mw = (MainWindow)Application.Current.MainWindow;

            mw.studySession.addAction(a);

            a.run(mw);
        }

        public void nextImage()
        {
            if (modeSelect && imageLoader != null)
            {
                if (isValidIndex(index + 1))
                {
                    index++;
                    display_image(imageLoader[index], high, low);
                }
            }
            else
            {
                if (isValidIndex(index + 4))
                {
                    index += 4;
                    display_four(imageLoader, index);
                }
            }
            buttonCheck();
        }

        private void prevImage_Click(object sender, RoutedEventArgs e)
        {
            // create the action
            var a = new Action.Grid.Previous();
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            mw.studySession.addAction(a);
            a.run(mw);
        }

        public void prevImage()
        {
            if (modeSelect && imageLoader != null)
            {
                if (isValidIndex(index - 1))
                {
                    index--;
                    display_image(imageLoader[index], high, low);
                }
            }
            else
            {
                if (isValidIndex(index - 4))
                {
                    index -= 4;
                    display_four(imageLoader, index);
                }
            }
            buttonCheck();
        }

        /*
         * Helper function to check if an index is within bounds.
         */
        private bool isValidIndex(int i)
        {
            return (0 <= i && i < imageLoader.Count());
        }


        private BitmapSource windowedImage(float high, float low, Bitmap to_be_processed)
        {
            var image_height = to_be_processed.Height;
            var image_width = to_be_processed.Width;
            Bitmap processed_image = new Bitmap(image_width, image_height);

            for (int i = 0; i < image_height; i++)
            {
                for (int j = 0; j < image_width; j++)
                {
                    System.Drawing.Color pixel = to_be_processed.GetPixel(i, j);
                    if (pixel.GetBrightness() < low)
                    {
                        System.Drawing.Color new_pixel = System.Drawing.Color.Black;
                        processed_image.SetPixel(i, j, new_pixel);
                    }
                    else if (pixel.GetBrightness() > high)
                    {
                        System.Drawing.Color new_pixel = System.Drawing.Color.White;
                        processed_image.SetPixel(i, j, new_pixel);
                    }
                    else
                    {
                        float sf = equate_scale_factor(high, low, pixel);
                        int color_value = (int)(sf * 255);
                        System.Drawing.Color new_pixel = System.Drawing.Color.FromArgb(color_value, color_value, color_value);
                        processed_image.SetPixel(i, j, pixel);
                    }

                }
            }

            BitmapSource windowedImage = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                processed_image.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(processed_image.Width, processed_image.Height));

            return windowedImage;
        }

        private float equate_scale_factor(float high, float low, System.Drawing.Color pixel)
        {
            float output = ((pixel.GetBrightness() - low) * high) / (high - low);
            return output;
        }

        /*
         * Check if the current index is at either end of the list.
         * If so, disable the corresponding button.
         */
        private void buttonCheck()
        {
            // Check and disable prev.
            if (!isValidIndex(index - 1))
            {
                prev_button.IsEnabled = false;
            }

            // Check and disable prev.
            if (!isValidIndex(index + 1) && modeSelect == true)
            {
                next_button.IsEnabled = false;
            }

            // Check and enable prev.
            if (isValidIndex(index - 1))
            {
                prev_button.IsEnabled = true;
            }

            // Check and enable next
            if (isValidIndex(index + 1) && modeSelect == true)
            {
                next_button.IsEnabled = true;
            }

            // Check and enable next
            if (isValidIndex(index + 4) && modeSelect == false)
            {
                next_button.IsEnabled = true;
            } // Check and disable next
            else if (!isValidIndex(index + 4) && modeSelect == false)
            {
                next_button.IsEnabled = false;
            }
        }

    }
}
