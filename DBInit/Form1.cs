using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace DBInit
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public Models.Project ProjectLevel1 = new Models.Project();
        public Models.Project ProjectLevel2 = new Models.Project();
        public Models.Project ProjectLevel3 = new Models.Project();
        List<Models.ProjectDetails> Details = new List<Models.ProjectDetails>();
        List<Models.Project> Level_1_List = new List<Models.Project>();
        List<Models.Project> Level_2_List = new List<Models.Project>();
        List<Models.Project> Level_3_List = new List<Models.Project>();
        string ProjectCode = DBInit.Properties.Settings.Default.ProjectCode;
        string biaoduanCode = DBInit.Properties.Settings.Default.BiaoduanCode;
        private void button2_Click(object sender, EventArgs e)
        {
            BLL.UserBLL bll = new BLL.UserBLL();
            if (bll.GetData().Rows.Count == 0)
            {
                Models.Users user = new Models.Users()
                {
                    UserId = Utils.getGUID(),
                    UserName = this.textBox1.Text,
                    UserPassword = this.textBox2.Text
                };
                BLL.FunctionResult result = bll.Create(user);
                if (result.ExcuteState)
                {
                    MessageBox.Show("添加用户成功");
                    bind();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bind();
            this.button4.Text = "清空数据库";
            this.button5.Text = "sql测试";
            Common.MessageHelper.showMessageEvent += new Common.ShowMessageDelget(MessageHelper_showMessageEvent);
        }

        void MessageHelper_showMessageEvent(string message)
        {
            this.Invoke(new EventHandler((o, e) =>
            {
                this.richTextBox1.AppendText(message + "\r\n");
            }));
        }
        private void bind()
        {
            BLL.UserBLL bll = new BLL.UserBLL();
            this.dataGridView2.DataSource = bll.GetData();
            BLL.DeviceDataAreaBLL bll1 = new BLL.DeviceDataAreaBLL();
            DataTable table = bll1.GetData();
            Models.DeviceDataArea area = null;
            if (table.Rows.Count > 0)
            {
                area = bll1.GetModelByRow(table.Rows[0]);
                this.textBox6.Text = area.MaxValue.ToString();
                this.textBox5.Text = area.MinValue.ToString(); 
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.Details = new List<Models.ProjectDetails>();
                this.backgroundWorker1.RunWorkerAsync();
                this.textBox4.Text = dialog.FileName;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            work(this.backgroundWorker1);
        }
        private bool work(BackgroundWorker bk)
        {
            try
            {
                BLL.ProjectBLL Bll_Project = new BLL.ProjectBLL();
                string fileName = this.textBox4.Text;
                IWorkbook workbook = null;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0) // 2007版本
                    workbook = new XSSFWorkbook(fs);
                else if (fileName.IndexOf(".xls") > 0) // 2003版本
                    workbook = new HSSFWorkbook(fs);
                int n = workbook.NumberOfSheets;
                showMessage("发现" + n + "个sheet页");

                for (int i = 0; i < n; i++)
                {
                    int order = 1;
                    showMessage("正在处理" + (i + 1) + "个sheet页");
                    ISheet sheet = workbook.GetSheetAt(i);

                    int start = sheet.FirstRowNum;
                    int end = sheet.LastRowNum;
                    for (int r = 2; r <= end; r++)
                    {
                        IRow row = sheet.GetRow(r);
                        if (row == null || row.FirstCellNum == -1)
                            continue;
                        try
                        {
                            #region

                            string p1Name = row.GetCell(0).StringCellValue;
                            string p2Name = row.GetCell(1).StringCellValue;
                            string p3Name = row.GetCell(2).StringCellValue;
                            if (!Utils.IsEmptyOrNull(p1Name))
                            {
                                AddProject(p1Name, 1, i);
                            }
                            if (!Utils.IsEmptyOrNull(p2Name))
                            {
                                AddProject(p2Name, 2, i, Level_1_List.Last().ProjectId);
                            }

                            if (!Utils.IsEmptyOrNull(p3Name))
                            {
                                AddProject(p3Name, 3, i, Level_2_List.Last().ProjectId);
                            }
                            int CIndex = 3;
                            Models.Project tempProject = Level_3_List.Last();
                            int index = order++;
                            Models.ProjectDetails details = new Models.ProjectDetails()
                            {
                                DetailsTilte = row.GetCell(3).GetValue(out CIndex),//.StringCellValue,
                                SerialNumber = row.GetCell(CIndex).GetValue(out CIndex),//.StringCellValue,
                                Mileage = row.GetCell(CIndex).GetValue(out CIndex),//.StringCellValue,
                                Intensity = row.GetCell(CIndex).GetValue(out CIndex),//.StringCellValue,
                                CementContent = row.GetCell(CIndex).GetValue(out CIndex),//.StringCellValue.ToString(),
                                MixDesign = row.GetCell(CIndex).GetValue(out CIndex),//.NumericCellValue.ToString(),
                                DetailsId = ProjectCode + "_" + biaoduanCode + "_M4_" + index + "_" + Utils.getGUID(),// Utils.getGUID(),
                                DisplayOrder = index,
                                ProjectId = tempProject.ProjectId,
                                Project = tempProject
                            };
                            this.Details.Add(details);
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            showMessage("第" + i + "页，第" + r + "行：" + ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                showMessage(ex.Message);
            }
            finally
            {
                this.Invoke(new EventHandler((a, b) =>
                {
                    this.dataGridView1.DataSource = Details.Select(p => new
                    {
                        一级菜单 = p.Project.ParentProject.ParentProject.ProjectName,
                        二级菜单 = p.Project.ParentProject.ProjectName,
                        三级菜单 = p.Project.ProjectName,
                        四级菜单 = p.DetailsTilte,
                        分项工程编号 = p.SerialNumber,
                        取样里程 = p.Mileage,
                        设计强度 = p.Intensity,
                        每立方水泥用量 = p.CementContent,
                        设计配合比 = p.MixDesign

                    }).ToList();
                }));

            }
            return true;
        }

        private void showMessage(string message)
        {
            this.Invoke(new EventHandler((a, b) =>
            {
                this.richTextBox1.AppendText(message);
                this.richTextBox1.AppendText("\r\n");
            }));
        }
        private void AddProject(string p1Name, int level, int sheetIndex, string parentProjectId = null)
        {
            List<List<Models.Project>> myList = new List<List<Models.Project>>() {
                null,
                this.Level_1_List,
                this.Level_2_List,
                this.Level_3_List
            };
            List<Models.Project> proList = myList[level];
            if (proList.Where(p => p.ProjectName == p1Name && p.ParentProjectId == parentProjectId).Count() == 0)
            {
                Models.Project project = new Models.Project()
                {
                    ProjectName = p1Name,
                    DisplayOrder = proList.Count + 1,
                    ProjectId = ProjectCode + "_" + biaoduanCode + "_M" + level + "_" + (proList.Count + 1),
                    ParentProject = level == 1 ? null : myList[level - 1].Last(),
                    ParentProjectId = parentProjectId,
                    ProjectLevel = level
                };
                proList.Add(project);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (this.Details.Where(p => string.IsNullOrWhiteSpace(p.Project.ParentProject.ParentProject.ProjectName)).Count() > 0)
                {
                    ShowMessageAndSetButtonEnabled("有一级菜单为空", false);
                    return;
                }
                if (this.Details.Where(p => string.IsNullOrWhiteSpace(p.Project.ParentProject.ProjectName)).Count() > 0)
                {
                    ShowMessageAndSetButtonEnabled("有二级菜单为空", false);
                    return;
                }
                if (this.Details.Where(p => string.IsNullOrWhiteSpace(p.Project.ProjectName)).Count() > 0)
                {
                    ShowMessageAndSetButtonEnabled("有三级菜单为空", false);
                    return;
                }
                if (this.Details.Where(p => string.IsNullOrWhiteSpace(p.CementContent)).Count() > 0)
                {
                    ShowMessageAndSetButtonEnabled("每立方米水泥用量为空", false);
                    return;
                }
                if (this.Details.Where(p => string.IsNullOrWhiteSpace(p.DetailsTilte)).Count() > 0)
                {
                    ShowMessageAndSetButtonEnabled("有四级菜单为空", false);
                    return;
                }
                if (this.Details.Where(p => string.IsNullOrWhiteSpace(p.SerialNumber)).Count() > 0)
                {
                    ShowMessageAndSetButtonEnabled("有项目工程编号为空", false);
                    return;
                }

                Models.ProjectDetails details = this.Details.Where(p => string.IsNullOrWhiteSpace(p.Mileage)).FirstOrDefault();
                if (this.Details.Where(p => string.IsNullOrWhiteSpace(p.Mileage)).Count() > 0)
                {
                    ShowMessageAndSetButtonEnabled("有取样里程为空", false);
                    return;
                }
                if (this.Details.Where(p => string.IsNullOrWhiteSpace(p.MixDesign)).Count() > 0)
                {
                    ShowMessageAndSetButtonEnabled("有混合比例为空", false);
                    return;
                }
                if (this.Details.Where(p => string.IsNullOrWhiteSpace(p.Intensity)).Count() > 0)
                {
                    ShowMessageAndSetButtonEnabled("有设计强度为空", false);
                    return;
                }
            }
            catch (Exception ex)
            {
                ShowMessageAndSetButtonEnabled("数据异常", false);
            }
            ShowMessageAndSetButtonEnabled("读取数据成功", true);
        }
        private void ShowMessageAndSetButtonEnabled(string message, bool Enabled)
        {
            MessageBox.Show(message);
            this.button3.Enabled = Enabled;
        }
        bool IfSaving = false;
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Task.Factory.StartNew(() =>
                {
                    IfSaving = true;
                    BLL.ProjectDetailsBLL bll = new BLL.ProjectDetailsBLL();
                    bll.SaveList(this.Details);

                    IfSaving = false;
                    MessageBox.Show("保存完成");
                }).ContinueWith(a =>
                {
                    MessageBox.Show("保存到数据库失败");
                }, TaskContinuationOptions.OnlyOnFaulted);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存到数据库失败");
            }

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string sql1 = "delete from [user]";
            string sql2 = " delete from project ";
            string sql3 = " delete from projectDetails ";
            string sql4 = " delete from deviceData ";
            string sql5 = " delete from DeviceDataArea ";

            if (!excute(sql5, "删除数据范围失败")) return;
            if (!excute(sql4, "删除绑定数据失败")) return;
            if (!excute(sql3, "删除项目详情失败")) return;
            if (!excute(sql2, "删除项目失败")) return;
            if (!excute(sql1, "删除用户失败")) return;
            MessageBox.Show("数据库清理完成");

        }
        private bool excute(string sql, string errorMessage)
        {
            BLL.ProjectBLL bll = new BLL.ProjectBLL();
            try
            {
                bll.Excute(sql);
            }
            catch
            {
                MessageBox.Show(errorMessage);
            }
            return true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.StartPosition = FormStartPosition.CenterParent;
            f.Show();
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

            BLL.DeviceDataAreaBLL bll = new BLL.DeviceDataAreaBLL();
            DataTable table = bll.GetData();
            Models.DeviceDataArea area = null;
            string str = this.textBox5.Text;
            if (!Regex.IsMatch(str, "^\\d{15}$")) {
                MessageBox.Show("最小值应为十五位数字");
                return;
            }
            str = this.textBox6.Text;
            if (!Regex.IsMatch(str, "^\\d{15}$"))
            {
                MessageBox.Show("最大值应为十五位数字");
                return;
            }
            double min = double.Parse(this.textBox5.Text);
            double max = double.Parse(this.textBox6.Text);
            if (min >= max) {

                MessageBox.Show("最小值应小于最大值");
                return;
            }
            if (table.Rows.Count > 0)
            {
                area = bll.GetModelByRow(table.Rows[0]);
                area.MaxValue = double.Parse(this.textBox6.Text);
                area.MinValue = double.Parse(this.textBox5.Text);
                bll.Edit(area);
            }
            else
            {
                area = new Models.DeviceDataArea()
                {
                    AreaId = Utils.getGUID(),
                    MaxValue = double.Parse(this.textBox6.Text),
                    MinValue = double.Parse(this.textBox5.Text)
                };
                bll.Create(area);
            }
        }
    }
    public static class CellEx
    {
        public static string GetValue(this ICell cell, out int NextIndex)
        {
            string value = "";
            if (cell == null || cell.ColumnIndex == cell.Row.LastCellNum)
            {
                NextIndex = -1;
                return "";
            }

            switch (cell.CellType)
            {
                case CellType.String: value = cell.StringCellValue; break;
                case CellType.Boolean: value = cell.BooleanCellValue.ToString(); break;
                case CellType.Error: value = cell.ErrorCellValue.ToString(); break;
                case CellType.Numeric: value = cell.NumericCellValue.ToString(); break;
                case CellType.Blank:
                    if (cell.Row.Sheet.GetRow(1).GetCell(cell.ColumnIndex).CellType == CellType.Blank)
                        return cell.Row.GetCell(cell.ColumnIndex + 1).GetValue(out NextIndex);
                    else
                    {
                        NextIndex = cell.ColumnIndex + 1;
                        return "";
                    }
                case CellType.Formula:
                case CellType.Unknown:
                default: value = ""; break;
            }
            //NextIndex = cell.ColumnIndex + 1; 
            //return value;
            if (Utils.IsEmptyOrNull(value))
            {
                return cell.Row.GetCell(cell.ColumnIndex + 1).GetValue(out NextIndex);
            }
            else
            {
                NextIndex = cell.ColumnIndex + 1;
                return value;
            }
        }
    }
}
