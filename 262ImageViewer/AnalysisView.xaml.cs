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
        public AnalysisView(Bitmap image)
        {
            InitializeComponent();
            List<float> brightness = new List<float>();
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    brightness.Add(image.GetPixel(x, y).GetBrightness());
                }
            }
            AverageLBL.Content = (brightness.ToArray().Average() * 100).ToString() + "%";
            brightness.Sort();
            List<int> hist = new List<int>(new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0});
            // Loop through the List
            foreach(float flt in brightness)
            {
                int bright = (int) Math.Floor(flt * 10) + 1;
                if (bright >= 10)
                {
                    bright = 10;
                }
                hist[bright] += 1;
            }
            int maxVal = hist.Max();

            Prog01.Maximum = maxVal;
            Prog02.Maximum = maxVal;
            Prog03.Maximum = maxVal;
            Prog04.Maximum = maxVal;
            Prog05.Maximum = maxVal;
            Prog06.Maximum = maxVal;
            Prog07.Maximum = maxVal;
            Prog08.Maximum = maxVal;
            Prog09.Maximum = maxVal;
            Prog10.Maximum = maxVal;

            Prog01.Value = hist[1];
            Prog02.Value = hist[2];
            Prog03.Value = hist[3];
            Prog04.Value = hist[4];
            Prog05.Value = hist[5];
            Prog06.Value = hist[6];
            Prog07.Value = hist[7];
            Prog08.Value = hist[8];
            Prog09.Value = hist[9];
            Prog10.Value = hist[10];
        }
    }
}
