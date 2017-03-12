using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common
{
    public delegate void ShowMessageDelget(string message);
    public static class MessageHelper
    {
        public static event ShowMessageDelget showMessageEvent;
        public static void ShowMessage(string message) {
            if (MessageHelper.showMessageEvent != null) {
                showMessageEvent(message);
            }
        }
    }
}
