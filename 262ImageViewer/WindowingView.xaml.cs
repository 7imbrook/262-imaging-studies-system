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
        public int index
        {
            get;
            private set;
        }

        private List<ImageLoader.Image> imageLoader;

        public WindowingView(List<ImageLoader.Image> imgLdr, int cur_index, bool cur_mode)
        {
            InitializeComponent();
            index = cur_index;
            modeSelect = cur_mode;
            imageLoader = imgLdr;
            if(isValidIndex(index))
            {
                display_image(imageLoader[index]);
            }
        }

        private void display_image(ImageLoader.Image image) //Take in processed img
        {
            image_display.Children.Clear();
            System.Windows.Controls.Image i = new System.Windows.Controls.Image();
            i.Source = image.getSource();
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
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            var a = new Action.Grid.Next();
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
                    display_image(imageLoader[index]);
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
                    display_image(imageLoader[index]);
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
