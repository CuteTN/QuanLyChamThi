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
    /// Interaction logic for PageSetting.xaml
    /// </summary>
    public partial class PageSetting : Page
    {

        public PageSetting()
        {
            InitializeComponent();
            InitializeDataContext();
        }

        private void InitializeDataContext()
        {
            DataContext = new SettingViewModel();

        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            bool nothing = ViewExtension.MessageOK(null, "Thiết lập thành công !!!");
        }
    }
}
