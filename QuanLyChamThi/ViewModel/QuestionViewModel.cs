using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.ViewModel
{
    class QuestionViewModel : ViewModelBase, UserModelBase
    {
        

        public void Receive(object sender, List<DatabaseCommand> commands)
        {
            throw new NotImplementedException();
        }

        public QuestionViewModel()
        {
            ViewModelMediator.Ins.AddUserModel(this);
        }
    }
}
