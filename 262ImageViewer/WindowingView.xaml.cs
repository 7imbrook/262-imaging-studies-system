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
        public WindowingView(ImageLoader.Image windowed_img)
        {
            InitializeComponent();
            display_image(windowed_img);
            //display_image() or display_four()
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

        private void display_four(Bitmap image) //Take in processed imgs
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
    }
}
