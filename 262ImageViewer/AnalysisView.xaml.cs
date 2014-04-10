using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

namespace _262ImageViewer
{
    /// <summary>
    /// Interaction logic for AnalysisView.xaml
    /// </summary>
    public partial class AnalysisView : Page
    {

        public float average;

        public AnalysisView(Bitmap image)
        {
            List<float> brightness = new List<float>();
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    brightness.Add(image.GetPixel(x, y).GetBrightness());
                }
            }
            average = brightness.ToArray().Average();
            brightness.Sort();
            List<int> hist = new List<int>(10);
            // Loop through the List
            foreach(float flt in brightness)
            {
                int bright = (int) Math.Floor(flt * 10) + 1;
                if (bright >= 10)
                {
                    bright = 10;
                }
                hist[bright]++;
            }
            int maxVal = hist.Max();
            Rect01.Height = (290 / maxVal) * hist[1];
            Rect02.Height = (290 / maxVal) * hist[2];
            Rect03.Height = (290 / maxVal) * hist[3];
            Rect04.Height = (290 / maxVal) * hist[4];
            Rect05.Height = (290 / maxVal) * hist[5];
            Rect06.Height = (290 / maxVal) * hist[6];
            Rect07.Height = (290 / maxVal) * hist[7];
            Rect08.Height = (290 / maxVal) * hist[8];
            Rect09.Height = (290 / maxVal) * hist[9];
            Rect10.Height = (290 / maxVal) * hist[10];
        }
    }
}
