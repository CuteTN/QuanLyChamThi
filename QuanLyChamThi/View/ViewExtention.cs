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
            if (sender != null) sender.Opacity = 0.5;
            WindowNotification windowNotification = new WindowNotification(message, detail, messageType);
            bool result = (windowNotification.ShowDialog() == true);
            if (sender != null) sender.Opacity = 0.5;

            return result;
        }

        public static int Confirm(Window sender, string message) //return 0 if user cacel, 1 if confirm
        {
            if (sender != null) sender.Opacity = 0.5;
            WindowConfirm windowConfirm = new WindowConfirm(message);
            windowConfirm.ShowDialog();
            if (sender != null) sender.Opacity = 0.5;

            return windowConfirm.GetConfirmState();
        }

        public static bool MessageOK(Window sender, string message, int messageType) //messageType: 0: falure, 1: notification, 2: warning
        {
            if (sender != null) sender.Opacity = 0.5;
            WindowNotiOK windowNotiOK = new WindowNotiOK(message, messageType);
            bool result = (windowNotiOK.ShowDialog() == true);
            if (sender != null) sender.Opacity = 1;

            return result;
        }
    }
}
