using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using RFIDManageSystem;
using System.IO;
using System.Threading.Tasks;

namespace RFIDApplication
{
    public partial class QueryForm : Form
    {
        public QueryForm()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBox2.DisplayMember = "ProjectName";
            this.comboBox2.ValueMember = "ProjectId";
            this.comboBox2.DataSource = GetDefaultList("请选择分部工程", 2, this.comboBox1.SelectedValue.ToString());
          
            if (this.comboBox1.SelectedIndex == 0)
            {
                this.comboBox2.SelectedIndex = this.comboBox3.SelectedIndex = this.comboBox4.SelectedIndex = 0;
                this.comboBox2.Enabled = this.comboBox3.Enabled = this.comboBox4.Enabled = false;
            }
            else {
                this.comboBox2.Enabled = true;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBox3.DisplayMember = "ProjectName";
            this.comboBox3.ValueMember = "ProjectId";
            this.comboBox3.DataSource = GetDefaultList("请选择子分部工程", 3, this.comboBox2.SelectedValue.ToString());
             if (this.comboBox2.SelectedIndex == 0)
            {
                this.comboBox3.SelectedIndex = this.comboBox4.SelectedIndex = 0;
                this.comboBox3.Enabled = this.comboBox4.Enabled = false;
            }
            else
            {
                this.comboBox3.Enabled = true;
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBox4.DisplayMember = "DetailsTilte";
            this.comboBox4.ValueMember = "DetailsId";
            this.comboBox4.DataSource = GetDefaultDetailsList("请选择分项工程", this.comboBox3.SelectedValue.ToString());
            
            if (this.comboBox3.SelectedIndex == 0)
            {
                this.comboBox4.SelectedIndex = 0;
                this.comboBox4.Enabled = false;
            }
            else
            {
                this.comboBox4.Enabled = true;
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private List<Models.Project> GetDefaultList(string text, int level = 1, string parentProjectId = null)
        {
            BLL.ProjectBLL bll = new BLL.ProjectBLL();
            string Where = " ProjectLevel=@ProjectLevel ";

            List<OleDbParameter> Paralist = new List<OleDbParameter>();
            Paralist.Add(new OleDbParameter("ProjectLevel", level));
            if (!Utils.IsEmptyOrNull(parentProjectId))
            {
                Where += " and parentProjectId=@parentProjectId";
                Paralist.Add(new OleDbParameter("@parentProjectId", parentProjectId));
            }
            Where += " order by  ProjectId";
            DataTable table = bll.GetData(Where, Paralist.ToArray());
            List<Models.Project> list = new List<Models.Project>() { 
                new Models.Project(){
                    ProjectId = "",
                     ProjectName=text
                }
            };
            list.AddRange(ListSupport.ToList<Models.Project>(table));
            return list;
        }
        private List<Models.ProjectDetails> GetDefaultDetailsList(string text, string projectId)
        {
            BLL.ProjectDetailsBLL bll = new BLL.ProjectDetailsBLL();
            DataTable table = null;
            //if (!Utils.IsEmptyOrNull(projectId))
            //{
            //    table = bll.GetData(" ProjectId=@ProjectId  ", new System.Data.OleDb.OleDbParameter("@parentProjectId", projectId));
            //}
            //else {
            //    table = bll.GetData();
            //}
            table = bll.GetDataByProjectId(this.comboBox1.SelectedValue.ToString(), this.comboBox2.SelectedValue.ToString(), this.comboBox3.SelectedValue.ToString());
            List<Models.ProjectDetails> list = new List<Models.ProjectDetails>() { 
                new Models.ProjectDetails(){
                    DetailsId="",
                    ProjectId = "",
                   DetailsTilte=text
                }
            };
            list.AddRange(ListSupport.ToList<Models.ProjectDetails>(table));
            return list;
        }

        private void QueryForm_Load(object sender, EventArgs e)
        {
            this.comboBox1.DisplayMember = "ProjectName";
            this.comboBox1.ValueMember = "ProjectId";
            this.comboBox1.DataSource = GetDefaultList("请选择项目工程");
            this.comboBox5.SelectedIndex = 0;
            this.dataGridView1.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);

        }

        void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // var a = this.dataGridView1.Columns[0].Width;
            this.dataGridView1.Columns[8].Width = 120;
            this.dataGridView1.Columns[11].Width = 120;
            this.dataGridView1.Columns[12].Width = 160;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            bindData();
        }
        void bindData()
        {
            BLL.DeviceDataBLL bll = new BLL.DeviceDataBLL();
            DataTable table = bll.Query(this.comboBox1.SelectedValue.ToString(), this.comboBox2.SelectedValue.ToString(), this.comboBox3.SelectedValue.ToString(), this.comboBox4.SelectedValue.ToString(), this.dateTimePicker1.Value.ToString("yyyy-MM-dd"), this.dateTimePicker2.Value.ToString("yyyy-MM-dd"), txtDeviceValue.Text, this.comboBox5.SelectedIndex);

            this.dataGridView1.DataSource = table; 

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            string title = "";
            title = "打印RFID卡信息";
            PrintDGV.Print_DataGridView(this.dataGridView1, title, 4);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                try
                {
                    BLL.DeviceDataBLL bll = new BLL.DeviceDataBLL();
                    DataTable table = bll.Query(this.comboBox1.SelectedValue.ToString(), this.comboBox2.SelectedValue.ToString(), this.comboBox3.SelectedValue.ToString(), this.comboBox4.SelectedValue.ToString(), this.dateTimePicker1.Value.ToString("yyyy-MM-dd"), this.dateTimePicker2.Value.ToString("yyyy-MM-dd"),txtDeviceValue.Text,this.comboBox5.SelectedIndex);
                    byte[] byteArray = Office.ExcelExport.StreamExport(table, null);
                    string name = dialog.SelectedPath + "\\导出数据.xls";
                    FileStream fs = File.Open(name, FileMode.Create);
                    fs.Write(byteArray, 0, byteArray.Length);
                    fs.Flush();
                    fs.Close();
                    MessageBox.Show("导出完成");
                    System.Diagnostics.Process.Start("Explorer.exe", dialog.SelectedPath);
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bindData();
        }
    }
}
