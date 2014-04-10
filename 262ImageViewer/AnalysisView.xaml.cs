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
        public AnalysisView(Bitmap bi)
        {
            float[] brightness = new float[bi.Width * bi.Height];
            int count = 0;
            for (int x = 0; x < bi.Width; x++)
            {
                for (int y = 0; y < bi.Height; y++)
                {
                    brightness[count] = bi.GetPixel(x, y).GetBrightness();
                    count++;
                }
            }
        }
    }
}
