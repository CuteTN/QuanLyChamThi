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
            ucQuestionList.btnEdit.Visibility = Visibility.Hidden;
            ucQuestionList.btnRemove.Visibility = Visibility.Hidden;
        }
    }
}
