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
        public static bool Message(Window sender, string message, string detail, int messageType)
        {
            sender.Opacity = 0.5;
            WindowNotification windowNotification = new WindowNotification(message, detail, messageType);
            bool result = (windowNotification.ShowDialog() == true);
            sender.Opacity = 1;

            return result;
        }

        public static int Confirm(Window sender, string message) //return 0 if user cacel, 1 if confirm
        {
            sender.Opacity = 0.5;
            WindowConfirm windowConfirm = new WindowConfirm(message);
            windowConfirm.ShowDialog();
            sender.Opacity = 1;

            return windowConfirm.GetConfirmState();
        }
    }
}
