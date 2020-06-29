using QuanLyChamThi.ViewModel;
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
        LoginViewModel vm = new LoginViewModel();

        public WindowLogin()
        {
            InitializeComponent();
            DataContext = vm;
            access = "denied";
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            DoLogin();
        }

        private void DoLogin()
        {
            bool loginResult = vm.Login(tbUsername.Text, tbPassword.Password);
            if (loginResult == true)
            {
                access = "accepted";
                ViewExtension.Message(this, "Đăng nhập thành công", "Chào mừng " + tbUsername.Text, ViewExtension.MessageType.Notification);
                this.Close();
            }
            else
            {
                ViewExtension.Message(this, "Đăng nhập thất bại", "Sai tên tài khoản hoặc mật khẩu", 0);
            }
        }

        public string getAccessState()
        {
            return access;
        }

        private void mainGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DoLogin();
            }
        }
    }
}
