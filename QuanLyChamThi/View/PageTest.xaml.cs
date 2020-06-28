using QuanLyChamThi.ViewModel;
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

namespace QuanLyChamThi.View
{
    /// <summary>
    /// Interaction logic for PageTest.xaml
    /// </summary>
    public partial class PageTest : Page
    {
        public PageTest()
        {
            InitializeComponent();

            DataContext = new TestViewModel();
        }

        private void btnChooseQuestion_Click(object sender, RoutedEventArgs e)
        {
            var windowQuestionList = new WindowQuestionList();
            windowQuestionList.ShowDialog();
        }

        private void dgReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
