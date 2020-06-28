using QuanLyChamThi.ViewModel;
using System.Windows;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace QuanLyChamThi.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            /** CREATE GRID COLUMNS AND ROWS **/


            DataContext = MainWindowViewModel.Ins;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.D1)
            //    mainScreen.Content = listPage[0];
            //else if (e.Key == Key.D2)
            //    mainScreen.Content = listPage[1];
            //else if (e.Key == Key.D3)
            //    mainScreen.Content = listPage[2];
            //else if (e.Key == Key.D4)
            //    mainScreen.Content = listPage[3];
            //else if (e.Key == Key.D5)
            //    mainScreen.Content = listPage[4];
            //else if (e.Key == Key.D6)
            //    mainScreen.Content = listPage[5];
            //else if (e.Key == Key.D7)
            //    mainScreen.Content = listPage[6];

            if (e.Key == Key.Escape)
                this.Close();
        }

        private void canvasExtendedSideBar_MouseEnter(object sender, MouseEventArgs e)
        {
            canvasExtendedSideBar.Visibility = Visibility.Visible;
            Grid.SetColumn(mainScreen, 6);
            sprtPnl.Visibility = Visibility.Hidden;
        }

        private void canvasExtendedSideBar_MouseLeave(object sender, MouseEventArgs e)
        {
            canvasExtendedSideBar.Visibility = Visibility.Hidden;
            Grid.SetColumn(mainScreen, 1);
            sprtPnl.Visibility = Visibility.Visible;
        }


        /**
          
        EFFECT

        //DoubleAnimation moveIn = new DoubleAnimation { From = -150, To = 0, FillBehavior = FillBehavior.Stop, BeginTime = TimeSpan.FromSeconds(0), Duration = new Duration(TimeSpan.FromSeconds(0.5)) };
        DoubleAnimation moveOut = new DoubleAnimation { From = 0, To = -150, FillBehavior = FillBehavior.Stop, BeginTime = TimeSpan.FromSeconds(0), Duration = new Duration(TimeSpan.FromSeconds(0.5)) };
        
        private void EffectMoveIn()
        {
            PathGeometry animationPath = new PathGeometry();
            PathFigure path = new PathFigure();
            path.StartPoint = new Point(-150, 0);
            path.Segments.Add(new LineSegment(new Point(0, 0), false));
            animationPath.Figures.Add(path);

            DoubleAnimationUsingPath moveIn = new DoubleAnimationUsingPath();
            moveIn.PathGeometry = animationPath;
            moveIn.FillBehavior = FillBehavior.Stop;
            moveIn.BeginTime = TimeSpan.FromSeconds(0);
            moveIn.Duration = TimeSpan.FromSeconds(5);
            moveIn.Source = PathAnimationSource.X;


            Storyboard storyboard = new Storyboard();

            storyboard.Children.Add(moveIn);
            foreach (DoubleAnimationUsingPath child in storyboard.Children)
            {
                Storyboard.SetTarget(child, canvasExtendedSideBar);
                Storyboard.SetTargetProperty(child, new PropertyPath(TranslateTransform.XProperty));
            }

            storyboard.Begin();
        }

        private void EffectMoveOut()
        {
            Storyboard storyboard = new Storyboard();

            storyboard.Children.Add(moveOut);
            foreach (DoubleAnimation child in storyboard.Children)
            {
                Storyboard.SetTarget(child, canvasExtendedSideBar);
                Storyboard.SetTargetProperty(child, new PropertyPath("TranslateTransform.XProperty"));
            }
            storyboard.Completed += delegate { canvasExtendedSideBar.Visibility = Visibility.Hidden; };

            storyboard.Begin();
        }
        **/
    }
}
