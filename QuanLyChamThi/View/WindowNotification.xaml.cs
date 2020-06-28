using Microsoft.Expression.Interactivity.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace QuanLyChamThi.View
{
    /// <summary>
    /// Interaction logic for WindowNotification.xaml
    /// </summary>
    public partial class WindowNotification : Window
    {
        DoubleAnimation fadeIn = new DoubleAnimation { From = 0, To = 1, FillBehavior = FillBehavior.Stop, BeginTime = TimeSpan.FromSeconds(0), Duration = new Duration(TimeSpan.FromSeconds(0.25)) };
        DoubleAnimation hold = new DoubleAnimation { From = 1, To = 1, FillBehavior = FillBehavior.Stop, BeginTime = TimeSpan.FromSeconds(0.25), Duration = new Duration(TimeSpan.FromSeconds(1)) };
        DoubleAnimation fadeOut = new DoubleAnimation { From = 1, To = 0, FillBehavior = FillBehavior.Stop, BeginTime = TimeSpan.FromSeconds(1.25), Duration = new Duration(TimeSpan.FromSeconds(0.25)) };

        public WindowNotification(string notification, string detail, int messageType)
        {
            InitializeComponent();

            /** GET NOTIFICATION **/
            tbNotification.Text = notification;
            tbDetail.Text = detail;

            if (messageType == 0)
            {
                imgIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Cancel;
                border.Background = new SolidColorBrush(Colors.Red);
            }

            FadeInThenOut();
        }

        private void FadeInThenOut()
        {

            Storyboard storyboard = new Storyboard();

            storyboard.Children.Add(fadeIn);
            storyboard.Children.Add(hold);
            storyboard.Children.Add(fadeOut);

            foreach (DoubleAnimation child in storyboard.Children) 
            {
                Storyboard.SetTarget(child, this);
                Storyboard.SetTargetProperty(child, new PropertyPath(OpacityProperty));
            }

            storyboard.Completed += delegate { this.Close(); };
            storyboard.Begin();
        }

        private void mainGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
