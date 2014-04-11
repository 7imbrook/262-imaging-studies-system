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
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
         * The current index of the reconstruction slice
        */
        public int sliceIndex = 0;

        /*
         * The ImageLoader in use.
         */
        private List<ImageLoader.Image> imageLoader;

        /*
         * The Current Study
         */
        private Study studySession;

        /*
         * Constructor that creates the ReconstructionView with defined state.
         */
        public ReconstructionView(List<ImageLoader.Image> imgLdr, int i, bool mode, Study session)
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
        private void display_image(ImageLoader.Image image)
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

        /*
         * Creates a 2x2 grid and adds the images starting from top left
         * being the lowest numbered image of the set to bottom right
         * being the highest numbered image.
         */
        private void display_four(List<ImageLoader.Image> imageList, int index)
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
                    System.Windows.Controls.Image to_display = new System.Windows.Controls.Image();
                    ImageLoader.Image source = imageList[index];
                    to_display.Source = source.getSource();
                    to_display.Stretch = Stretch.Uniform;
                    int x = source.getWidth();
                    Grid.SetRow(to_display, 0);
                    Grid.SetColumn(to_display, position);
                    four_grid.Children.Add(to_display);
                    index++;
                }
            }
            //Vertical Reconstruction Image
            //-----------------------------
            System.Windows.Controls.Image vertical = new System.Windows.Controls.Image();
            Bitmap vSource = reconstructor(studySession, true);

            BitmapSource vBS = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                vSource.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(vSource.Width, vSource.Height));

            vertical.Source = vBS;
            vertical.Stretch = Stretch.Uniform;
            int y = vSource.Width;
            Grid.SetRow(vertical, 0);
            Grid.SetColumn(vertical, 1);
            four_grid.Children.Add(vertical);
            //-------------------------------
            //Horizontal Reconstruction Image
            //-------------------------------
            System.Windows.Controls.Image horizontal = new System.Windows.Controls.Image();
            Bitmap hSource = reconstructor(studySession, false);

            BitmapSource hBS = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                hSource.GetHbitmap(),
                IntPtr.Zero,
                System.Windows.Int32Rect.Empty,
                BitmapSizeOptions.FromWidthAndHeight(hSource.Width, hSource.Height));

            horizontal.Source = hBS;
            horizontal.Stretch = Stretch.Uniform;
            int z = hSource.Width;
            Grid.SetRow(horizontal, 1);
            Grid.SetColumn(horizontal, 0);
            four_grid.Children.Add(horizontal);
            //---------------------------------

            //Add the grid of images to the image_display.
            image_display.Children.Add(four_grid);

        }
        
        /*
         * Make a bitmap of the reconstruction of a study.
         */
        Bitmap reconstructor(Study studySession, bool axis)
        {
            int height = studySession.imageCollection[0].getHeight();
            int width = studySession.imageCollection[0].getWidth();
            int numImages = studySession.imageCollection.Count();

            Bitmap reconst = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(reconst))
            {
                for (int i = 0; i < numImages; i++)
                {
                    if (axis)
                    {
                        g.DrawImage(studySession.imageCollection[i].getSlice(sliceIndex, true), i, 0);
                    }
                    else
                    {
                        g.DrawImage(studySession.imageCollection[i].getSlice(sliceIndex, false), 0, i);
                    }
                }
            }
            return reconst;
        }
        

        /*
         * Create the action to increase the image position counter by one.
         */
        private void nextImage_Click(object sender, RoutedEventArgs e)
        {
            // create the action
            var a = new Action.Reconstruction.Next();
            studySession.addAction(a);
            // Need the current study
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            a.run(mw);
        }

        /*
         * Increases the image position counter by one, then displays
         * the new image located in the array position determined by
         * the counter. Displays counter number as well.
         */
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
         * Create the action to decrease the image position counter by one.
         */
        private void prevImage_Click(object sender, RoutedEventArgs e)
        {
            // create the action
            var a = new Action.Reconstruction.Previous();
            studySession.addAction(a);
            // Need the current study
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            a.run(mw);
        }

        /*
         * Decreases the image position counter by one, then displays
         * the new image located in the array position determined by
         * the counter. Displays counter number as well.
         */
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
         * Create the action to go to the next reconstruction slice.
         */
        private void nextReconstruction_Click(object sender, RoutedEventArgs e)
        {
            // create the action
            var a = new Action.Reconstruction.NextReconstruction();
            studySession.addAction(a);
            // Need the current study
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            a.run(mw);
        }

        /*
         * Go to the next reconstruction slice.
         */
        public void nextReconstruction()
        {
            if (isValidIndex(index + 1))
            {
                sliceIndex++;
                display_four(imageLoader, index);
            }
            buttonCheck();
        }

        /*
         * Create the action to go to the previous reconstruction slice.
         */
        private void previousReconstruction_Click(object sender, RoutedEventArgs e)
        {
            // create the action
            var a = new Action.Reconstruction.PreviousReconstruction();
            studySession.addAction(a);
            // Need the current study
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            a.run(mw);
        }

        /*
         * Go to the previous reconstruction slice.
         */
        public void previousReconstruction()
        {
            if (isValidIndex(index - 1))
            {
                sliceIndex--;
                display_four(imageLoader, index);
            }
            buttonCheck();
        }

        /*
         * Create the action to close the reconstruction.
         */
        private void close_Click(object sender, RoutedEventArgs e)
        {
            // create the action
            MainWindow mw = (MainWindow)Application.Current.MainWindow;
            var a = new Action.Reconstruction.Close();
            studySession.addAction(a);
            // Need the current study
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
