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
            DataContext = MainWindowViewModel.Ins;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
