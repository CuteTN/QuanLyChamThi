using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using QuanLyChamThi.Command;

namespace QuanLyChamThi.ViewModel
{
    class MainWindowViewModel: ViewModelBase
    {
        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                {
                    _closeCommand = new RelayCommand(param => CloseWindow());
                }
                return _closeCommand;
            }
            set
            {
                _closeCommand = value;
            }
        }

        private void CloseWindow()
        {

        }
    }
}
