﻿using QuanLyChamThi.view;
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

namespace QuanLyChamThi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Page[] listPage = { new PageMain(), new PageReport(), new PageTestResult()};
        public MainWindow()
        {
            InitializeComponent();
            /** CREATE GRID COLUMNS AND ROWS **/
            for (int i = 0; i < 32; i++)
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < 18; i++)
                mainGrid.RowDefinitions.Add(new RowDefinition());

            mainGrid.ColumnDefinitions[0].MinWidth = 40;
            mainGrid.RowDefinitions[0].MinHeight = 40;

            mainScreen.Content = new PageMain();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D1)
                mainScreen.Content = listPage[0];
            else if (e.Key == Key.D2)
                mainScreen.Content = listPage[1];
            else if (e.Key == Key.D3)
                mainScreen.Content = listPage[2];

            if (e.Key == Key.Escape)
                this.Close();
        }

        private void canvasExtendedSideBar_MouseEnter(object sender, MouseEventArgs e)
        {
            canvasExtendedSideBar.Visibility = Visibility.Visible;
        }

        private void canvasExtendedSideBar_MouseLeave(object sender, MouseEventArgs e)
        {
            canvasExtendedSideBar.Visibility = Visibility.Hidden;
        }
    }
}