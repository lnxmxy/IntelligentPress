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
    public delegate bool Yanzheng(string text);
    public partial class Form5 : Form
    {
        public event Yanzheng yanzhengHanlder;
        public Form5()
        { 
            InitializeComponent();
        } 
        private void Form5_Load(object sender, EventArgs e)
        {
            this.ShowIcon = false;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }
         
        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.yanzhengHanlder != null) {
               bool b =  this.yanzhengHanlder(this.textBox1.Text);
               this.DialogResult = b ? DialogResult.OK : DialogResult.Cancel;
            }
        }
    }
}
