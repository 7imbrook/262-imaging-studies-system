/* 
 * ReconstructionView.xaml.cs
 * 
 * Version: 
 *     $Id$ 
 * 
 * Revisions: 
 *     $Log$ 
 */

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
    public partial class ReconstructionView : Page
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

        /*
         * The ImageLoader in use.
         */
        private List<BitmapImage> imageLoader;

        /*
         * The Current Study
         */
        private Study studySession;

        /*
         * Constructor that creates the ImageView with defined state.
         */
        public ReconstructionView(List<BitmapImage> imgLdr, int i, bool mode, Study session)
        {
            InitializeComponent();
            studySession = session;
            index = i;
            modeSelect = false; //True is one, False is four
            imageLoader = imgLdr;
            if (isValidIndex(index))
            {
                if (modeSelect == true)
                {
                    display_image(imageLoader[index]);
                }
                else
                {
                    display_four(imageLoader, index);
                }
                buttonCheck();
            }
        }

        /*
         * Displays image based on the array position given by the counter.
         */
        private void display_image(BitmapImage image)
        {
            image_display.Children.Clear();
            Image i = new Image();
            i.Source = image;
            // If the image won't fit at native resolution, scale it.
            if (Application.Current.MainWindow.ActualHeight < image.Height ||
                Application.Current.MainWindow.ActualWidth < image.Width)
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
         * Creates a 2x2 grid and adds the images starting from top left
         * being the lowest numbered image of the set to bottom right
         * being the highest numbered image.
         */
        private void display_four(List<BitmapImage> imageList, int index)
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

            //Add the images belonging in the first row of the grid
            //If there exists no image, leave it blank.
            for (int position = 0; position < 1; position++)
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

            //Add the images belonging in the second row of the grid.
            //If there exists no image, leave it blank.
            for (int position = 1; position < 2; position++)
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
            //Add the grid of images to the image_display.
            image_display.Children.Add(four_grid);

        }
        /*
        Bitmap reconstructor(Study studySession)
        {
            int size = 256;
            boolean vertical = true;
            int sliceIndex = 1;
            int numImages = Study.Count()?

            Bitmap reconst = new Bitmap(size, size);
            using (Graphics g = Graphics.FromImage(reconst))
            {
                for (int i = 0; i < numImages; i++)
                {
                    if (vertical)
                    {
                        g1.DrawImage(studySession.getImage(i).getSlice(sliceIndex, true), i, 0);
                    }
                    else
                    {
                        g1.DrawImage(Study.getImage(i).getSlice(sliceIndex, false), 0, i);
                    }
                }
            }

            return reconst;
        }
        */

        /*
         * Increases the image position counter by one, then displays
         * the new image located in the array position determined by
         * the counter. Displays counter number as well.
         */
        private void nextImage_Click(object sender, RoutedEventArgs e)
        {
            // create the action
            var a = new Action.Reconstruction.Next();

            // Need the current study
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
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

        /*
         * Decreases the image position counter by one, then displays
         * the new image located in the array position determined by
         * the counter. Displays counter number as well.
         */
        private void prevImage_Click(object sender, RoutedEventArgs e)
        {
            // create the action
            var a = new Action.Reconstruction.Previous();

            // Need the current study
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
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

        /*
         * Switch between 4-up and 1-up modes.
         */
        public void switchMode()
        {
            if (modeSelect)
            {
                //Switch from one to four
                double x = (index) / 4;
                int new_index = 4 * (int)Math.Floor(x) + 1;
                index = new_index - 1;
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
