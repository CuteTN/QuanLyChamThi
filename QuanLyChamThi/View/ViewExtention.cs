using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLyChamThi.View
{
    static class ViewExtension
    {
        public static bool Message(Window sender, string message, string detail, float stayTime)
        {
            WindowNotification windowNotification = new WindowNotification(message, detail, stayTime);
            return (windowNotification.ShowDialog() == true);
        } 
    }
}
