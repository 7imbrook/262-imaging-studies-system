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
using System.Windows.Shapes;

namespace _262ImageViewer
{
    /// <summary>
    /// Interaction logic for WindowingPrompt.xaml
    /// </summary>
    public partial class WindowingPrompt : Window
    {

        public float low_cut, high_cut;
        
        WindowingView windowingView;

        MainWindow main;

        public WindowingPrompt(MainWindow win)
        {
            InitializeComponent();
            main = win;
        }

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            low_cut = float.Parse(LowCut.Text) * 0.01f;
            high_cut = float.Parse(HighCut.Text) * 0.01f;
            main.winPrompt = this;

            var a = new Action.Windowing.Create();
            main.studySession.addAction(a);
            a.run(main);

            //windowingView = new WindowingView(high_cut, low_cut, main);
            //main.setFrameImageView(windowingView);
            
            this.Close();
        }

        private void CancelButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
