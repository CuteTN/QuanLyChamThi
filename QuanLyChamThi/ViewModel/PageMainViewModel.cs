using QuanLyChamThi.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.ViewModel
{
    class PageMainViewModel: ViewModelBase, UserModelBase
    {

        private ObservableCollection<SUBJECT> _listSubject;
        public ObservableCollection<SUBJECT> ListSubject
        {
            get
            {
                if(_listSubject == null)
                {
                    _listSubject = new ObservableCollection<SUBJECT>(DataProvider.Ins.DB.SUBJECT.ToList());
                }
                return _listSubject;
            }
            set { _listSubject = value; OnPropertyChange("ListSubject"); }
        }

        private ObservableCollection<CLASS> _listClass;
        public ObservableCollection<CLASS> ListClass
        {
            get
            {
                if (_listClass == null)
                {
                    _listClass = new ObservableCollection<CLASS>(DataProvider.Ins.DB.CLASS.Where(param => param.Year == DateTime.Now.Year).ToList());
                }
                return _listClass;
            }
            set { _listClass = value; OnPropertyChange("ListSubject"); }
        }

        void Refresh()
        {
            ListSubject = null;
            ListClass = null;
        }

        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            Refresh();
        }

        public PageMainViewModel()
        {
            ViewModelMediator.Ins.AddUserModel(this);
        }
    }
}
