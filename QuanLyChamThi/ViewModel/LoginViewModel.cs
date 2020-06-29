using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi.ViewModel
{
    class LoginViewModel
    {
        public bool Login(string userName, string password)
        {
            return true;
            return (userName == "admin" && password == "admin");
        }
    }
}
