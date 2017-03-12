using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace RFIDApplication
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //MessageBox.Show("1、登录页整合\r\n2、修改密码页取消用户名，用户ID由登录获得\r\n3、确认龄期的字段\r\n");
            //Application.Run(new Form1());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Login login = new Login();
            if (login.bll.Login())
            {

                Application.Run(new Form1());
            }
            else
            {
                DialogResult result = login.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Application.Run(new Form1());
                }
            }
        }
    }
}
