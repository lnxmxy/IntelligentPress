using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Linq;

namespace RFIDApplication
{
    public partial class Form3 : DevComponents.DotNetBar.Metro.MetroForm
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void comboBoxEx1_TextChanged(object sender, EventArgs e)
        {
            string str = this.comboBoxEx1.Text;
            List<string> list = stringList.Where(p => p.Contains(str)).ToList();
            this.comboBoxEx1.DataSource = list;

        }
        List<string> stringList { get; set; }
        private void Form3_Load(object sender, EventArgs e)
        {
            stringList = new List<string>()
            {
                "张三",
                "李四",
                "王五",
                "张三丰",
                "李世民",
                "王小军",
            };
            this.comboBoxEx1.DataSource = stringList;
            this.comboBox1.DataSource = stringList;
            this.listBox1.DataSource = stringList;
            this.comboBoxEx1.DropDownStyle = ComboBoxStyle.DropDown;
            this.comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
            Project p1 = new Project()
            {
                DisplayOrder = 1,
                ParentProjectId = null,
                ParentProject = null,
                ProjectId = "1",
                ProjectLevel = ProjectLevel.单位工程,
                ProjectName = "梨沟大桥"
            };
            Project p2 = new Project()
            {
                DisplayOrder = 1,
                ParentProjectId =p1.ProjectId,
                ParentProject = p1,
                ProjectId = "1.1",
                ProjectLevel = ProjectLevel.分部工程,
                ProjectName = "基础及下部构造"
            };
            Project p3 = new Project()
            {
                DisplayOrder = 1,
                ParentProjectId = p2.ProjectId,
                ParentProject = p2,
                ProjectId = "1.1.1",
                ProjectLevel = ProjectLevel.子分部工程,
                ProjectName = "右桥1号桥0#台"
            };
            ProjectDetails details = new ProjectDetails()
            {
                Code = "BA01011",
                DisplayOrder = 1,
                Licheng = "K51+422",
                Project = p3,
                Peihebi = "330:766:1149:155:2.31",
                ProjectDetailsId = "1.1.1.1",
                ProjectId = p3.ProjectId,
                Qiangdu = "C25",
                Shuini = "330",
                Title = "1级扩大基础"

            };
            var data = new
            {
                biaoduan = "9标段",
                item = details
            };
            string str = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            this.richTextBox1.Text = str;
            SendData sd = Newtonsoft.Json.JsonConvert.DeserializeObject<SendData>(this.richTextBox1.Text);

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            string str = this.comboBoxEx1.Text;
            List<string> list = stringList.Where(p => p.Contains(str)).ToList();
            this.comboBox1.DataSource = list;

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            this.listBox1.Visible = true;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            this.listBox1.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string str = this.textBox1.Text;
            List<string> list = stringList.Where(p => p.Contains(str)).ToList();
            this.listBox1.DataSource = list;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox1.Text = this.listBox1.SelectedValue.ToString();
            this.listBox1.Visible = false;
        }
    }
    public static class ProjectLevel
    {
        public const int 单位工程 = 1;
        public const int 分部工程 = 2;
        public const int 子分部工程 = 3;
    }

    public class SendData
    {
        public string biaoduan { get; set; }
        public ProjectDetails item { get; set; }
    }
    public class Project
    {
        public string ProjectId { get; set; }
        public string ProjectName { get; set; }
        public int ProjectLevel { get; set; }
        public int DisplayOrder { get; set; }
        public string ParentProjectId { get; set; }
        public Project ParentProject { get; set; }
    }
    public class ProjectDetails
    {
        public string ProjectDetailsId { get; set; }
        public string Title { get; set; }
        public string Code { get; set; }
        public string Licheng { get; set; }
        public string Qiangdu { get; set; }
        public string Shuini { get; set; }
        public string Peihebi { get; set; }
        public string ProjectId { get; set; }
        public int DisplayOrder { get; set; }
        public Project Project { get; set; }
    }
}