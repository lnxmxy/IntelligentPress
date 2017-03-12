using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Common
{
    public static class Setting
    {
        public static string getAppSettingString(string name) { 
            string str = ConfigurationManager.AppSettings[name]; 
            return str;
        }
    }
}
