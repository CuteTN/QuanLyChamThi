﻿using System;
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
using System.Windows.Shapes;

namespace QuanLyChamThi.View
{
    /// <summary>
    /// Interaction logic for WindowQuestionList.xaml
    /// </summary>
    public partial class WindowQuestionList : Window
    {
        public WindowQuestionList()
        {
            InitializeComponent();
        }


        private void btnHideShow_Click(object sender, RoutedEventArgs e)
        {
            if (lbXtch.Visibility == Visibility.Visible)
            {
                lbXtch.Visibility = Visibility.Hidden;
                tbContent.Visibility = Visibility.Hidden;
                Grid.SetColumnSpan(questionList, 29);
                Grid.SetColumnSpan(btnAddRemove, 29);
                Grid.SetColumn(btnHideShow, 31);
                btnHideShow.Content = new MaterialDesignThemes.Wpf.PackIcon
                { Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowExpandLeft };
            }
            else /** UC is HIDDEN **/
            {
                lbXtch.Visibility = Visibility.Visible;
                tbContent.Visibility = Visibility.Visible;
                Grid.SetColumnSpan(questionList, 15);
                Grid.SetColumnSpan(btnAddRemove, 15);
                Grid.SetColumn(btnHideShow, 16);
                btnHideShow.Content = new MaterialDesignThemes.Wpf.PackIcon
                { Kind = MaterialDesignThemes.Wpf.PackIconKind.ArrowExpandRight };
            }
        }
    }
}
