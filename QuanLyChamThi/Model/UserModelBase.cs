using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.ViewModel
{
    // Author: CuteTN
    interface UserModelBase
    {
        void Receive(object sender, List<DatabaseCommand> commands);
    }
}
