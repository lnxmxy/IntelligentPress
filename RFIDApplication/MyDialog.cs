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
    public partial class MyDialog : Form
    {
        string message { get; set; }
        string ok { get; set; }
        string cancle { get; set; }
        public MyDialog(string title,string message,string OKButtonTex,string CancleButtonText)
        {
            InitializeComponent();
            this.Text = title;
            this.label1.Text = message;
            this.button1.Text = OKButtonTex;
            this.button2.Text = CancleButtonText;
            this.ControlBox = false;
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
        }

        private void MyDialog_Load(object sender, EventArgs e)
        {
            this.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
    public static class MyDialogOption {
        public static DialogResult show(string title, string message, string OKButtonTex = "确定", string CancleButtonText = "取消") {
            MyDialog dialog = new MyDialog(title, message, OKButtonTex, CancleButtonText);
            dialog.StartPosition = FormStartPosition.CenterParent;
            dialog.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            dialog.ShowInTaskbar = false;
            return dialog.ShowDialog();
        }
    }
}
