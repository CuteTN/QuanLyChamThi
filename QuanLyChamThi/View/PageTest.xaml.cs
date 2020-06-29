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
            var viewModel = DataContext as TestViewModel;
            if (!viewModel.ReadyToAcceptData())
            { 
                return;
            }
            List<QuestionModel> selectedQuestions = new List<QuestionModel>();
            foreach (var testDetailModel in viewModel.TempTestDetail)
            {
                selectedQuestions.Add((from q in DataProvider.Ins.DB.QUESTION
                                      where q.IDQuestion == testDetailModel.QuestionID
                                      select new QuestionModel 
                                      {
                                          IDQuestion = q.IDQuestion,
                                          IDDifficulty = q.IDDifficulty,
                                          IDSubject = q.IDSubject,
                                          Content = q.Content
                                      }).ToList()[0]);
            }
            var windowQuestionList = new WindowQuestionList(selectedQuestions);

            windowQuestionList.ShowDialog();
            viewModel.AcceptData(selectedQuestions);
        }
    }
}
