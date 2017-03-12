using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms; 

namespace RFIDApplication
{
    public partial class Login :  Form 
    {
        public BLL.UserBLL bll = new BLL.UserBLL();
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {

            if (bll.Login())
            {
                this.DialogResult = DialogResult.OK;
            }
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
          //  this.ShowIcon = false;
            this.Text = "欢迎使用水泥混凝土强度智能监控系统";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if(bll.Login()){
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }else{
            string str1 = this.textBox1.Text;
            string str2 = this.textBox2.Text;
            Models.Users user = new Models.Users()
            {
                UserName = this.textBox1.Text,
                UserPassword = this.textBox2.Text
            };
                if(Utils.IsEmptyOrNull(user.UserName)){
                    MessageBox.Show("请填写用户名", "登录失败");
                    this.textBox1.Focus();
                }
                else if (Utils.IsEmptyOrNull(user.UserPassword))
                {
                    MessageBox.Show("请填写密码", "登录失败");
                    this.textBox2.Focus();
                }
                else
                {
                    if (bll.Login(user, this.checkBox1.Checked))
                    {
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("用户名或密码错误", "登录失败");
                    }
                }
            }
             
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Form5 form5 = new Form5( );

            form5.StartPosition = FormStartPosition.CenterParent;

            form5.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            form5.yanzhengHanlder += new Yanzheng(form5_yanzhengHanlder);
            DialogResult result = form5.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
            }
            else {

                MessageBox.Show("临时密码错误，请与管理员联系", "登录失败");
            }
        }

        bool form5_yanzhengHanlder(string text)
        {
            return bll.Login(text);
        }
    }
}
