using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLyChamThi.View
{
    public static class ViewExtension
    {
        public enum MessageType
        {
            Error,
            Notification,
            Warning,
        }

        public static bool Message(Window sender, string message, string detail, MessageType messageType)
        {
            if (sender != null) sender.Opacity = 0.5;
            WindowNotification windowNotification = new WindowNotification(message, detail, messageType);
            bool result = (windowNotification.ShowDialog() == true);
            if (sender != null) sender.Opacity = 1;

            return result;
        }

        /// <summary>
        /// Return value: 0: Cancel, 1: Confirm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int Confirm(Window sender, string message)
        {
            if (sender != null) sender.Opacity = 0.5;
            WindowConfirm windowConfirm = new WindowConfirm(message);
            windowConfirm.ShowDialog();
            if (sender != null) sender.Opacity = 1;

            return windowConfirm.GetConfirmState();
        }

        public static bool MessageOK(Window sender, string message, MessageType messageType) 
        {
            if (sender != null) sender.Opacity = 0.5;
            WindowNotiOK windowNotiOK = new WindowNotiOK(message, messageType);
            bool result = (windowNotiOK.ShowDialog() == true);
            if (sender != null) sender.Opacity = 1;

            return result;
        }
    }
}
