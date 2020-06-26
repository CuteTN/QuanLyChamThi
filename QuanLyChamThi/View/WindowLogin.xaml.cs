using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private string access;

        public WindowLogin()
        {
            InitializeComponent();
            access = "denied";
            /** CREATE GRID COLUMNS AND ROWS **/
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            
            access = "accepted";
            //bool nothing = (ViewExtension.Message(this, "Đăng nhập thành công", "Chào mừng abcxyz", 1) == false) ;
            this.Close();
        }

        public string getAccessState()
        {
            return access;
        }
    }
}
