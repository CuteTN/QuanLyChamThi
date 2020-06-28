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
using QuanLyChamThi.Model;
using QuanLyChamThi.ViewModel;

namespace QuanLyChamThi.View
{
    /// <summary>
    /// Interaction logic for PageTestResult.xaml
    /// </summary>
    public partial class PageTestResultDetailed : Page
    {
        public PageTestResultDetailed()
        {
            InitializeComponent();

            DataContext = new TestResultDetailedViewModel();
        }
    }
}
