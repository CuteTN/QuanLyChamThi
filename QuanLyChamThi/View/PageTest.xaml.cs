using QuanLyChamThi.Model;
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
            List<QuestionModel> selectedQuestions = new List<QuestionModel>();
            var windowQuestionList = new WindowQuestionList(selectedQuestions);
            windowQuestionList.ShowDialog();
            (DataContext as TestViewModel).AcceptData(selectedQuestions);
        }

        private void dgReport_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
