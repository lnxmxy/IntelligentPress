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
    public partial class ChangePassword : Form
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             
            if (string.IsNullOrWhiteSpace(this.textBox2.Text))
            {
                MessageBox.Show("密码必须填写");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.textBox3.Text))
            {
                MessageBox.Show("新密码必须填写");
                return;
            }
            if (string.IsNullOrWhiteSpace(this.textBox4.Text))
            {
                MessageBox.Show("旧密码必须填写");
                return;
            }
            if (!this.textBox3.Text.Equals(this.textBox4.Text)) {

                MessageBox.Show("密码不一致");
                return;
            }
            BLL.UserBLL bll = new BLL.UserBLL();
            if (bll.UpdatePassword(this.textBox2.Text, this.textBox3.Text, this.checkBox1.Checked))
            {
                MessageBox.Show("修改密码成功");
            }
            else
            {
                MessageBox.Show("帐号或密码错误");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = this.textBox3.Text = this.textBox4.Text = "";
        }
    }
}
