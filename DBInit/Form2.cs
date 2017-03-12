using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DBInit
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                this.toolStripStatusLabel1.Text = "";
                string sql = this.richTextBox1.SelectedText;
                if (string.IsNullOrWhiteSpace(sql))
                    sql = this.richTextBox1.Text;
                BLL.ProjectBLL bll = new BLL.ProjectBLL();
                if (sql.ToLower().Trim().StartsWith("select"))
                {
                    DataTable table = bll.Query(sql);
                    this.dataGridView1.DataSource = table;
                    this.tabControl1.SelectedIndex = 0;
                }
                else
                {
                    int n = bll.Excute(sql);
                    this.richTextBox2.Clear();
                    this.richTextBox2.Text = string.Format("{0}行受影响", n);
                    this.tabControl1.SelectedIndex = 1;
                }
            }
            catch (Exception ex) {
                this.toolStripStatusLabel1.Text = ex.Message;
            }
        }
    }
}
