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
using System.Diagnostics;


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

        /*
         * The cutoff values for windowing.
         */
        public float high, low;

        /*
         * The ImageLoader in use.
         */
        private List<ImageLoader.Image> imageLoader;

        /*
         * The main window.
         */
        MainWindow main;


        /*
         * Constructor
         */
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

        /*
         * Displays the given image by processing it first along with the high and low
         * cutoff values.
         */
        private void display_image(ImageLoader.Image image, float high, float low)
        {
            image_display.Children.Clear();
            System.Windows.Controls.Image i = new System.Windows.Controls.Image();

            //Take the image and create a windowed BitmapSource, then set i's source as that BitmapSource.
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

        /*
         * Display the next processed image upon click.
         */
        private void nextImage_Click(object sender, RoutedEventArgs e)
        {
            var a = new Action.Windowing.Next();
            MainWindow mw = (MainWindow)Application.Current.MainWindow;

            mw.studySession.addAction(a);

            a.run(mw);
        }

        /*
         * Increases the image position counter by one, then displays the
         * new processed image located at the new index in imageLoader.
         */
        public void nextImage()
        {
            if (imageLoader != null)
            {
                if (isValidIndex(index + 1))
                {
                    index++;
                    display_image(imageLoader[index], high, low);
                }
            }
            buttonCheck();
        }

        private void prevImage_Click(object sender, RoutedEventArgs e)
        {
            // create the action
            Debug.WriteLine("Other Test");
            var a = new Action.Windowing.Previous();
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            mw.studySession.addAction(a);
            a.run(mw);
        }

        /*
         * Decreases the index by one, then displays the new processed image
         * located at the new index in the imageLoader.
         */
        public void prevImage()
        {
            if (imageLoader != null)
            {
                if (isValidIndex(index - 1))
                {
                    index--;
                    display_image(imageLoader[index], high, low);
                }
            }

            buttonCheck();
        }

        /*
         * Run the close action. Add to action chain.
         */
        public void close_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            var a = new Action.Reconstruction.Close();
            main.studySession.addAction(a);
            a.run(mw);
        }

        /*
         * Helper function to check if an index is within bounds.
         */
        private bool isValidIndex(int i)
        {
            return (0 <= i && i < imageLoader.Count());
        }

        /*
         * Helper function to redraw a bitmap to its windowed form.
         * Returns as a BitmapSource.
         */
        private BitmapSource windowedImage(float high, float low, Bitmap to_be_processed)
        {
            var image_height = to_be_processed.Height;
            var image_width = to_be_processed.Width;
            Bitmap processed_image = new Bitmap(image_width, image_height);

            for (int i = 0; i < image_width; i++)
            {
                for (int j = 0; j < image_height; j++)
                {
                    System.Drawing.Color pixel = to_be_processed.GetPixel(i, j);
                    //If the pixel's intensity is less than the low cutoff value,
                    //make the pixel black.
                    if (pixel.GetBrightness() < low)
                    {
                        processed_image.SetPixel(i, j, System.Drawing.Color.Black);
                    }
                    //If the pixel's intensity is more than the high cutoff value,
                    //make the pixel white.
                    else if (pixel.GetBrightness() > high)
                    {
                        processed_image.SetPixel(i, j, System.Drawing.Color.White);
                    }
                    //If neither, the pixel will be a new intensity determined by the
                    //average scaled intensity.
                    else
                    {
                        float sf = equate_scale_factor(high, low, pixel);
                        int color_value = (int)(sf * 255);
                        processed_image.SetPixel(i, j, System.Drawing.Color.FromArgb(color_value, color_value, color_value));
                        
                    }

                }
            }
            Debug.WriteLine(processed_image.GetHbitmap());
            //Convert the Bitmap to BitmapSource
            BitmapSource windowedImage = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                processed_image.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(processed_image.Width, processed_image.Height));

            return windowedImage;
        }

        /*
         * Helper function that determines a pixel's new intensity.
         */
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
