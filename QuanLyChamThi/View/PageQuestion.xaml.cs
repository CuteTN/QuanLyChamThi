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
    /// Interaction logic for PageQuestion.xaml
    /// </summary>
    public partial class PageQuestion : Page
    {
        public PageQuestion()
        {
            InitializeComponent();
            var questionViewModel = new QuestionViewModel();
            questionViewModel.QuestionListViewModel = ucQuestionList.DataContext as UCQuestionListViewModel;
            DataContext = questionViewModel;
            
            // (ucQuestionList.DataContext as UCQuestionViewModel).listQuestion = (this.DataContext as QuestionViewModel).ListQuestion;
        }

        private void btnHideShow_Click(object sender, RoutedEventArgs e)
        {
            /** UC is VISIBLE **/
            if (ucQuestionList.Visibility == Visibility.Visible)
            {
                ucQuestionList.Visibility = Visibility.Hidden;
                lbDsch.Visibility = Visibility.Hidden;
                Grid.SetColumnSpan(tbContent, 29);
                Grid.SetColumnSpan(cbDifficulty, 26);
                Grid.SetColumnSpan(cbSubject, 26);
                Grid.SetColumn(btnHideShow, 31);
                btnHideShow.Content = new MaterialDesignThemes.Wpf.PackIcon
                { Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowExpandLeft };
            }
            else /** UC is HIDDEN **/
            {
                ucQuestionList.Visibility = Visibility.Visible;
                lbDsch.Visibility = Visibility.Visible;
                Grid.SetColumnSpan(tbContent, 15);
                Grid.SetColumnSpan(cbDifficulty, 12);
                Grid.SetColumnSpan(cbSubject, 12);
                Grid.SetColumn(btnHideShow, 16);
                btnHideShow.Content = new MaterialDesignThemes.Wpf.PackIcon
                { Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowExpandRight };
            }
        }
    }
}
