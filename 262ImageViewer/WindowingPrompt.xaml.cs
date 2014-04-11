using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _262ImageViewer
{
    /// <summary>
    /// Interaction logic for WindowingPrompt.xaml
    /// </summary>
    public partial class WindowingPrompt : Window
    {

        public float low_cut, high_cut;
        
        //WindowingView windowingView;

        MainWindow main;

        public WindowingPrompt(MainWindow win)
        {
            InitializeComponent();
            main = win;
        }

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            String l = LowCut.Text;
            String h = HighCut.Text;
            bool lowBool = float.TryParse(l, out low_cut);
            bool highBool = float.TryParse(h, out high_cut);
            Debug.WriteLine(lowBool);
            Debug.WriteLine(highBool);
            if (lowBool && highBool && isValidCuts(low_cut, high_cut)) 
            {
                low_cut = float.Parse(LowCut.Text) * 0.01f;
                high_cut = float.Parse(HighCut.Text) * 0.01f;
                /**
                windowingView = new WindowingView(high_cut, low_cut, main);
                main.setFrameImageView(windowingView);
                this.Close();
                **/
                main.winPrompt = this;
                var a = new Action.Windowing.Create();
                main.studySession.addAction(a);
                a.run(main);
                this.Close();
            }
            else
            {
                MessageBox.Show("You've selected invalid values. Please try again.");
            }
        }

        private bool isValidCuts(float low, float high)
        {
            if (low >= 0 && high <= 100)
            {
                if (low < high)
                {
                    return true;
                }
            }
            return false;
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
