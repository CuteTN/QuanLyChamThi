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
            for (int i = 0; i < 31; i++)
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < 17; i++)
                mainGrid.RowDefinitions.Add(new RowDefinition());
        }

        private void btnHideShowQuestionList_Click(object sender, RoutedEventArgs e)
        {
            /** LIST IS HIDDEN **/
            if (pnlQuestionList.Visibility == Visibility.Hidden)
            {
                pnlQuestionList.Visibility = Visibility.Visible;
                Grid.SetColumnSpan(svGrid, 15);
                btnHideShowQuestionList.Text = "Ẩn";
            }
            /** LIST IS VISIBLE **/
            else  //pnlQuestionList.Visibility == Visibility.Visible
            {
                pnlQuestionList.Visibility = Visibility.Hidden;
                Grid.SetColumnSpan(svGrid, 28);
                btnHideShowQuestionList.Text = "Hiện";
            }
        }
    }
}
