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
        }

        private void btnHideShow_Click(object sender, RoutedEventArgs e)
        {
            /** UC is VISIBLE **/
            if (ucQuestionList.Visibility == Visibility.Visible)
            {
                ucQuestionList.Visibility = Visibility.Hidden;
                Grid.SetColumnSpan(svGrid, 29);
                btnHideShow.Content = "<";
            }
            else /** UC is HIDDEN **/
            {
                ucQuestionList.Visibility = Visibility.Visible;
                Grid.SetColumnSpan(svGrid, 15);
                btnHideShow.Content = ">";
            }
        }
    }
}
