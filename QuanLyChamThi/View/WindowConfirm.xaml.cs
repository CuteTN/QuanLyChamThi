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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLyChamThi.View
{
    /// <summary>
    /// Interaction logic for WindowConfirm.xaml
    /// </summary>
    public partial class WindowConfirm : Window
    {
        private int ConfirmState = 0; //0: canceled, 1: confirmed

        public WindowConfirm(string message)
        {
            InitializeComponent();
            tbContent.Text = message;
            FadeIn();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            ConfirmState = 1;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ConfirmState = 0;
            this.FadeOut();
        }

        public int GetConfirmState()
        {
            return ConfirmState;
        }

        DoubleAnimation fadeIn = new DoubleAnimation { From = 0, To = 1, FillBehavior = FillBehavior.Stop, BeginTime = TimeSpan.FromSeconds(0), Duration = new Duration(TimeSpan.FromSeconds(0.25)) };
        DoubleAnimation fadeOut = new DoubleAnimation { From = 1, To = 0, FillBehavior = FillBehavior.Stop, BeginTime = TimeSpan.FromSeconds(0), Duration = new Duration(TimeSpan.FromSeconds(0.25)) };
        private void FadeIn()
        {
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeIn);
            foreach (DoubleAnimation child in storyboard.Children)
            {
                Storyboard.SetTarget(child, this);
                Storyboard.SetTargetProperty(child, new PropertyPath(OpacityProperty));
            }
            storyboard.Begin();
        }

        private void FadeOut()
        {
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(fadeOut);
            foreach (DoubleAnimation child in storyboard.Children)
            {
                Storyboard.SetTarget(child, this);
                Storyboard.SetTargetProperty(child, new PropertyPath(OpacityProperty));
            }
            storyboard.Completed += delegate { this.Close(); };
            storyboard.Begin();
        }
    }
}
