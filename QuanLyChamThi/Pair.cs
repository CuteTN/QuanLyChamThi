using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyChamThi
{
    class Pair<T,U>
    {
        T First { get; set; }
        U Second { get; set; }

        Pair(T first, U second)
        {
            First = first;
            Second = second;
        }
    }
}
