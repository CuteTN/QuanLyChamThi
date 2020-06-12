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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using QuanLyChamThi.ViewModel;
using QuanLyChamThi.Command;

namespace QuanLyChamThi.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel vm;
        private NhapKQChamThi nhapKQChamThi;
        public MainWindow()
        {
            InitializeComponent();
            vm = new MainWindowViewModel();
            vm.CloseCommand = new RelayCommand(param => this.Close());
            this.DataContext = vm;

            nhapKQChamThi = new NhapKQChamThi();
            Main.Content = nhapKQChamThi;
        }
    }
}
