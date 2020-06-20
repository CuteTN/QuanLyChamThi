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
using System.Windows.Shapes;

namespace QuanLyChamThi.View
{
    /// <summary>
    /// Interaction logic for WindowLogin.xaml
    /// </summary>
    public partial class WindowLogin : Window
    {

        public WindowLogin()
        {
            InitializeComponent();
            /** CREATE GRID COLUMNS AND ROWS **/
            for (int i = 0; i < 8; i++)
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < 8; i++)
                mainGrid.RowDefinitions.Add(new RowDefinition());
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Animation.Message(this, "Đăng nhập thành công", "Chào mừng abcxyz", 1) == false)
                this.Close();
        }
    }
}
