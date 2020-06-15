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

namespace QuanLyChamThi.View
{
    /// <summary>
    /// Interaction logic for PageTestResultDetailed.xaml
    /// </summary>
    public partial class PageTestResultDetailed : Page
    {
        public PageTestResultDetailed()
        {
            InitializeComponent();
            for (int i = 0; i < 31; i++)
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < 17; i++)
                mainGrid.RowDefinitions.Add(new RowDefinition());

            DataContext = new TestResultDetailViewModel();
        }
    }
}
