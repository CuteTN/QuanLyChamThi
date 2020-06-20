using QuanLyChamThi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.ViewModel
{
    // Author: CuteTN
    class ViewModelMediator
    {
        List<UserModelBase> UserModels = new List<UserModelBase>();
        DataProvider DataProvider = DataProvider.Ins;

        public void AddUserModel(UserModelBase umb)
        {
            UserModels.Add(umb);
        }

        public void Receive(Object sender, Object args)
        {
            foreach (var u in UserModels)
                u.Receive(sender, args);
            // DataProvider.Receive(sender, args);
        }

        // singleton pattern
        private ViewModelMediator() { }
        private static ViewModelMediator _ins = null;

        public static ViewModelMediator Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new ViewModelMediator();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }



    }
}
