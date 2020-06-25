using QuanLyChamThi.Model;
using QuanLyChamThi.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Diagnostics;
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
    /// Interaction logic for UserControlQuestionList.xaml
    /// </summary>
    public partial class UserControlQuestionList : UserControl
    {
        public UserControlQuestionList()
        {
            InitializeComponent();
            InitializeDataContext();
        }

        // Question list ViewModel //////////////////////////////////////////////////////////////////////////////////////////////////// 
        private void InitializeDataContext()
        {
            this.DataContext = new UCQuestionListViewModel();
        }

        private void DataGridCell_MouseUp(object sender, MouseButtonEventArgs e)
        {
            MarkRowAsDone(sender, e);
        }

        private void MarkRowAsDone(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void dgQuestionList_Loaded(object sender, RoutedEventArgs e)
        {
            if (dgQuestionList.SelectedIndex < 1) return;
            DataGridRow dgRow = dgQuestionList.ItemContainerGenerator.ContainerFromIndex(dgQuestionList.SelectedIndex) as DataGridRow;

            DataGridCell cell = dgQuestionList.Columns[2].GetCellContent(dgRow).Parent as DataGridCell;
            dgRow.Background = new SolidColorBrush(Colors.Blue);
            dgQuestionList.UpdateLayout();
        }
    }
}
