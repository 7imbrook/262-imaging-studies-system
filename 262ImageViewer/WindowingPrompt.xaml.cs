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
        /*
         * User inputted low and high cutoff values for pixel intensity.
         */
        public float low_cut, high_cut;
        
        //WindowingView windowingView;

        MainWindow main;

        public WindowingPrompt(MainWindow win)
        {
            InitializeComponent();
            main = win;
        }

        /*
         * Upon click, evaluate if the numbers are in between 0 ~ 100. If they
         * are valid, create a windowingView with the values.
         */
        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            String l = LowCut.Text;
            String h = HighCut.Text;
            bool lowBool = float.TryParse(l, out low_cut);
            bool highBool = float.TryParse(h, out high_cut);
            if (lowBool && highBool && isValidCuts(low_cut, high_cut)) 
            {
                low_cut = float.Parse(LowCut.Text) * 0.01f;
                high_cut = float.Parse(HighCut.Text) * 0.01f;

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

        /*
         * Helper function to determine if the cuts are valid.
         */
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

        /*
         * Close upon click.
         */
        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
