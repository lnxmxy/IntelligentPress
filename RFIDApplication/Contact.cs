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
    public partial class Contact : Form
    {
        public Contact()
        {
            InitializeComponent();
        }

        private void Contact_Load(object sender, EventArgs e)
        {
            this.label1.Text = "【单位名称】：交通运输部公路科学研究院  ";
            this.label2.Text = "【单位地址】：北京市海淀区西土路8号";
            this.label3.Text = "【联系电话】：010-62079598  18701128988";
            this.label4.Text = "【联  系人】：李思李";
            this.label5.Text = "【电子邮箱】：decoli27@gmail.com";
        }
    }
}
