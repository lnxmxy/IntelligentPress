using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;

namespace RFIDApplication
{
    public partial class Form1 : Form
    {
        private BLL.ProjectBLL ProjectBLL = new BLL.ProjectBLL();
        private BLL.ProjectDetailsBLL ProjectDetailsBLL = new BLL.ProjectDetailsBLL();

        List<TextBox> GroupTextlist { get; set; }
        List<Label> GroupTextLabellist { get; set; }
        List<PictureBox> GroupTextPictureBoxlist { get; set; }
        Queue<Models.DeviceData> DetailsForUpload { get; set; }
        Common.InternetStateListener InternetListener { get; set; }
        BLL.ProjectDetailsBLL detailsBLL = new BLL.ProjectDetailsBLL();
        BLL.DeviceDataBLL deviceBLL = new BLL.DeviceDataBLL();
        int DeviceDataTotal { get; set; }
        DateTime dt = DateTime.Now;  //定义一个成员函数用于保存每次的时间点
        bool ifCanConnectToServer { get; set; }
        public Form1()
        {
            InitializeComponent();
            this.GroupTextlist = new List<TextBox>() { 
                this.textBox1,
                this.textBox2,
                this.textBox3,

                this.textBox4,
                this.textBox5,
                this.textBox6,
                
                this.textBox7,
                this.textBox8,
                this.textBox9, 
            };
            this.GroupTextLabellist = new List<Label>() { 
                this.label18,
                this.label19,
                this.label20, 
                this.label21, 
                this.label22, 
                this.label23, 
                this.label24, 
                this.label25, 
                this.label26 
            
            };

            this.GroupTextPictureBoxlist = new List<PictureBox>() { 
            this.pictureBox2, 
                this.pictureBox3, 
                this.pictureBox4, 
                this.pictureBox5,  
                this.pictureBox6,  
                this.pictureBox7,  
                this.pictureBox8,  
                this.pictureBox9,  
                this.pictureBox10   
            };

            this.GroupTextPictureBoxlist.ForEach(p =>
            {
                toolTip1.SetToolTip(p, "点击以清除左侧文本框信息");
                p.Click += (a, b) =>
                {
                    GroupTextlist.ForEach(tt => tt.Enabled = false);
                    int n = this.GroupTextPictureBoxlist.IndexOf(p);
                    TextBox t = this.GroupTextlist[n];
                    t.Text = "";
                    t.Enabled = true;
                    t.Focus();

                };
            });
            GroupTextlist.ForEach(p =>
            {

                p.KeyDown += (a, b) =>
                    {
                        if (b.KeyCode == Keys.Enter)
                        {
                            if (p.TextLength == 15)
                            {
                                BLL.DeviceDataBLL DeviceDataBLL = new BLL.DeviceDataBLL();
                                string message = "";
                                bool ifExist = false;
                                if (DeviceDataBLL.GetModelByWhere(" DeviceValue='" + p.Text + "'") != null)
                                {
                                    ifExist = true;
                                    message = "绑定数据库中已经存在[" + p.Text + "]电子标签请核实！";
                                }
                                int n = this.GroupTextlist.Where(t => t.Text == p.Text).Count();
                                if (n > 1)
                                {
                                    ifExist = true;
                                    message = "编号为[" + p.Text + "]的电子标签已经绑定在其他试块上，请核实确认!";

                                }
                                if (!Regex.IsMatch(p.Text, "^\\d{15}$"))
                                {
                                    ifExist = true;
                                    message = "请填写15位电子标签!";
                                }
                                else
                                {
                                    BLL.DeviceDataAreaBLL bll = new BLL.DeviceDataAreaBLL();
                                    if (!bll.IfInArea(double.Parse(p.Text)))
                                    {
                                        ifExist = true;
                                        message = "填写电子标签编号的值超出范围，请检查!";
                                    }
                                }
                                if (ifExist)
                                {
                                    MessageBox.Show(message);//扫描后马上有回车？看不到弹出
                                    p.Text = "";
                                    p.Enabled = true;
                                    p.Focus();
                                    return;
                                }

                                p.Enabled = false;
                                MoveToNextText();
                            }
                            else
                            {
                                MessageBox.Show("填写电子标签编号不正确！");//扫描后马上有回车？看不到弹出
                                p.Text = "";
                                p.Enabled = true;
                                p.Focus();
                                return;
                            }
                        }
                    };


                p.TextChanged += (a, b) =>
                {

                    if (p.TextLength == 1)
                    {
                        dt = DateTime.Now;
                    }
                    else
                    {
                        //DateTime tempDt = DateTime.Now;        //保存按键按下时刻的时间点
                        //TimeSpan ts = tempDt.Subtract(dt);     //获取时间间隔
                        //if (ts.Milliseconds > 100)              //判断时间间隔，如果时间间隔大于50毫秒，则将TextBox清空
                        //    p.Text = "";
                        //dt = tempDt;
                    }
                };

                resetTextBox();

            });
        }
        private void MoveToNextText()
        {
            TextBox t = this.GroupTextlist.Where(p => string.IsNullOrWhiteSpace(p.Text)).FirstOrDefault();
            if (t != null)
            {
                if (this.GroupTextlist.IndexOf(t) >= (this.GroupInfo.SelectedIndex + 1) * 3)
                {
                    //  this.button1.Focus();
                    return;
                }
                t.Enabled = true;
                t.Focus();
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBox2.DisplayMember = "ProjectName";
            this.comboBox2.ValueMember = "ProjectId";
            this.comboBox2.DataSource = ProjectBLL.GetProjectByParentProjectId(this.comboBox1.SelectedValue.ToString());
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

            this.comboBox3.DisplayMember = "ProjectName";
            this.comboBox3.ValueMember = "ProjectId";
            this.comboBox3.DataSource = ProjectBLL.GetProjectByParentProjectId(this.comboBox2.SelectedValue.ToString());
        }
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.comboBox4.DisplayMember = "DetailsTilte";
            this.comboBox4.ValueMember = "DetailsId";
            this.comboBox4.DataSource = ProjectDetailsBLL.GetDetailsByProjectId(this.comboBox3.SelectedValue.ToString());
        }
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Models.ProjectDetails details = new BLL.ProjectDetailsBLL().GetModel(this.comboBox4.SelectedValue.ToString());
            if (details != null)
            {
                this.Intensity_Label.Text = "【" + details.Intensity + "】";
                this.Mileage_Label.Text = "【" + details.Mileage + "】";
                this.CementContent_Label.Text = "【" + details.CementContent + "】";
                this.MixDesign_Label.Text = "【" + details.MixDesign + "】";
                this.SerialNumber_Label.Text = "【" + details.SerialNumber + "】";

            }
        }
        private void GroupInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.GroupTextlist.Skip((this.GroupInfo.SelectedIndex + 1) * 3).ToList().ForEach(p =>
            {
                p.Text = "";
                p.Enabled = false;
            });
            TextBox box = this.GroupTextlist.Where(p => string.IsNullOrEmpty(p.Text)).First();
            if (box != null)
            {
                int n = this.GroupTextlist.IndexOf(box);
                if (n < (this.GroupInfo.SelectedIndex + 1) * 3)
                {
                    box.Enabled = true;
                    box.Focus();
                }
            }
        }
        void resetTextBox()
        {

            for (int i = 0; i < this.GroupTextlist.Count(); i++)
            {
                if (i == 0)
                {
                    GroupTextlist[i].Enabled = true;
                }
                else
                {
                    GroupTextlist[i].Enabled = false;
                }
                GroupTextlist[i].Text = "";

            }
            GroupTextlist[0].Focus();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //Task.Factory.StartNew(() =>
            //{
            if (yanzhengGroupText())
            {
                Models.ProjectDetails details = detailsBLL.GetModel(this.comboBox4.SelectedValue.ToString());
                //details.DeviceDataList = new List<Models.DeviceData>();
                BLL.DeviceDataBLL DeviceDataBLL = new BLL.DeviceDataBLL();
                List<string> list = new List<string>(){
                    "A","B","C","D","E","F","G","H","I" 
                };
                int n = (this.GroupInfo.SelectedIndex + 1) * 3;
                bool ifSuccess = true;
                string CreateDataStr = DateTime.Now.ToString("yyyyMMddHHmmss");
                string CreateDataStrGroup = "";

                int m = this.GroupTextlist.Where(p => !string.IsNullOrWhiteSpace(p.Text)).Count();
                int a = m / 3;
                int b = m % 3;
                //if (a == 0)
                //{
                //    DialogResult dr = MessageBox.Show("至少需要填写一组数据,是否继续","提示",MessageBoxButtons.OKCancel );
                //    if (dr != DialogResult.OK)
                //    {
                //        MoveToNextText();
                //        return;
                //    }
                //}
                if (b != 0)
                {
                    string messagestr = "";

                    if (this.GroupInfo.SelectedIndex == 0)
                    {
                        if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0)
                        {
                            messagestr += "第一组数据不完整，是否继续提交绑定\r\n\r\n";
                        }
                    }
                    else if (this.GroupInfo.SelectedIndex == 1)
                    {
                        if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0)
                        {
                            messagestr += "第一组数据不完整，是否继续提交绑定\r\n\r\n";
                        }
                        if (textBox4.Text.Length == 0 || textBox5.Text.Length == 0 || textBox6.Text.Length == 0)
                        {
                            messagestr += "第二组数据不完整，是否继续提交绑定\r\n\r\n";
                        }
                    }
                    else if (this.GroupInfo.SelectedIndex == 2)
                    {
                        if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0 || textBox3.Text.Length == 0)
                        {
                            messagestr += "第一组数据不完整，是否继续提交绑定\r\n\r\n";
                        }
                        if (textBox4.Text.Length == 0 || textBox5.Text.Length == 0 || textBox6.Text.Length == 0)
                        {
                            messagestr += "第二组数据不完整，是否继续提交绑定\r\n\r\n";
                        }
                        if (textBox7.Text.Length == 0 || textBox8.Text.Length == 0 || textBox9.Text.Length == 0)
                        {
                            messagestr += "第三组数据不完整，是否继续提交绑定\r\n\r\n";
                        }
                    }



                    DialogResult dr = MyDialogOption.show("提示", messagestr, "是，继续完成绑定", "否，返回补充完整");
                    // DialogResult dr = MessageBoxEx.Show("数据不完整，是否继续提交绑定", "提示", MessageBoxButtons.OKCancel, new string[] { "是，继续完成绑定", "否，返回补充完整" });
                    // DialogResult dr = MessageBox.Show("数据不完整，是否继续提交绑定？", "确认", MessageBoxButtons.OKCancel);
                    if (dr != DialogResult.OK)
                    {
                        MoveToNextText();
                        return;
                    }
                }

                for (int i = 0; i < n; i++)
                {
                    if (list[i] == "A" || list[i] == "B" || list[i] == "C")
                    {
                        CreateDataStrGroup = CreateDataStr + "A";
                    }
                    else if (list[i] == "D" || list[i] == "E" || list[i] == "F")
                    {
                        CreateDataStrGroup = CreateDataStr + "B";
                    }
                    else
                    {
                        CreateDataStrGroup = CreateDataStr + "C";
                    }
                    Models.DeviceData deviceData = new Models.DeviceData()
                      {
                          DataId = this.comboBox4.SelectedValue.ToString() + "_" + list[i] + "_" + Utils.getGUID(),
                          DeviceValue = this.GroupTextlist[i].Text,
                          GroupName = list[i],
                          ProjectDetailId = this.comboBox4.SelectedValue.ToString(),
                          Instar = this.Instar.SelectedItem.ToString(),
                          YangSheng = this.yangsheng.SelectedItem.ToString(),
                          SamplingDate = this.SamplingDate.Value.ToString("yyyy-MM-dd"),
                          CreateDate = CreateDataStrGroup,
                          IfUpload = 0,
                          NotUploadReason = "网络"
                      };

                    if (!Regex.IsMatch(deviceData.DeviceValue, "^\\d{15}$"))
                    {
                        continue;
                    }
                    //判断重复
                    if (DeviceDataBLL.GetModelByWhere(" DeviceValue='" + deviceData.DeviceValue + "'") != null)
                    {
                        MessageBox.Show("绑定数据库中已经存在[" + deviceData.DeviceValue + "]RFID编号请核实！");
                        return;
                    }



                    BLL.FunctionResult result = DeviceDataBLL.Create(deviceData);
                    if (!result.ExcuteState)
                    {
                        ifSuccess = false;
                        break;
                    }
                    this.DeviceDataTotal++;
                    setProcess();

                    this.DetailsForUpload.Enqueue(deviceData);
                }
                if (ifSuccess)
                {
                    details.DetailState = 1;

                    this.Invoke(new EventHandler((aa, bb) =>
                    {
                        MessageBox.Show("绑定数据完成!");
                        resetTextBox();
                        MoveToNextText();
                    }));
                }

            }
            //});
        }
        private bool yanzhengGroupText()
        {



            int n = (this.GroupInfo.SelectedIndex + 1) * 3;
            bool b = false;
            for (int i = 0; i < n; i++)
            {
                string str = this.GroupTextlist[i].Text;
                if (!Utils.IsEmptyOrNull(str) && !Regex.IsMatch(str, "^\\d{15}$"))
                {
                    MessageBox.Show(str + " 数据应为15位数字");
                    return false;
                }
                if (!Utils.IsEmptyOrNull(str) && Regex.IsMatch(str, "^\\d{15}$"))
                {
                    b = true;
                }
            }
            if (!b)
            {
                MessageBox.Show("未检测到芯片信息，请扫描芯片录入信息！");
                return false;
            }

            /*监测文本框信息连续*/
           // List<long> valueList = new List<long>();
            string message = "";
            long target = 1;
            List<TextBox> list = this.GroupTextlist.Where(p => !string.IsNullOrEmpty(p.Text)).ToList();
            var GROUP = this.GroupTextlist.Where(p => !string.IsNullOrEmpty(p.Text)).GroupBy(P => P.Tag);
            foreach (var item in GROUP) {
                string str = item.Key.ToString();
                List<TextBox> tempList = item.OrderBy(p => p.Text).ToList();
                bool b1= false;
                for (int j = 1; j < tempList.Count; j++)
                {
                    long d1 = long.Parse(tempList[j - 1].Text);
                    long d2 = long.Parse(tempList[j].Text);
                    if (d2 - d1 != target)
                    {
                        int n1 = this.GroupTextlist.IndexOf(tempList[j]);
                        string labelText = this.GroupTextLabellist[n1].Text; 
                        b1 = true;
                    }
                }
                //valueList.Add(long.Parse(item.Min(p => p.Text)));
                //valueList.Add(long.Parse(item.Max(p => p.Text)));
                if (b1) {
                    message += "第" + str + "组有数据不连续\r\n\r\n";
                }
            }
            List<string> LabelTextList = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I"};
            List<long> list1 = this.GroupTextlist.Where(p => !string.IsNullOrWhiteSpace(p.Text)).Select(p => long.Parse(p.Text)).OrderBy(p=>p).ToList();
            for (int i = 0; i < list1.Count - 1;i++ )
            {
                if (list1[i + 1] - list1[i] != 1)
                {
                    TextBox t1 = this.GroupTextlist.Where(p => p.Text == list1[i].ToString()).FirstOrDefault();
                    TextBox t2 = this.GroupTextlist.Where(p => p.Text == list1[i+1].ToString()).FirstOrDefault();
                    if (t1.Tag.ToString() != t2.Tag.ToString()) {
                        message += string.Format("第{0}组数据和第{1}组数据不连续\r\n\r\n", t1.Tag.ToString(), t2.Tag.ToString());
                    }
                }
            }
            //for (int i = 1; i+1 < valueList.Count; i+=2) {
            //    if (Math.Abs(valueList[i] - valueList[i + 1]) != 1) {
            //        int n1 = i / 2+1;
            //        int n2 = (i + 1) / 2 + 1;
            //        message += string.Format("第{0}组数据和第{1}组数据不连续\r\n\r\n",n1,n2);
            //    }
            //} 
            if (!string.IsNullOrEmpty(message))
            {
                DialogResult dr = MyDialogOption.show("提示", message + "是否继续绑定！", "是，继续完成绑定", "否，返回检查");
                // DialogResult dr = MessageBox.Show( message+"是否继续绑定！", "提示",MessageBoxButtons.OKCancel);
                return dr == DialogResult.OK;
            }


            return true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;

            this.Intensity_Label.Text = " ";
            this.Mileage_Label.Text = " ";
            this.CementContent_Label.Text = " ";
            this.MixDesign_Label.Text = " ";
            this.SerialNumber_Label.Text = " ";
            this.ifCanConnectToServer = true;
            this.Text += "——" + RFIDApplication.Properties.Settings.Default.BiaoduanName;

            bind();
            bool b = false;

            InternetListener = new Common.InternetStateListener(RFIDApplication.Properties.Settings.Default.ServerIp, 5000, b);
            InternetListener.InternetStateChanged += new Common.InternetStateChangedDelegate(InternetListener_InternetStateChanged);
            InternetListener.Start();
        }
        void InternetListener_InternetStateChanged(bool InternetState)
        {
            this.Invoke(new EventHandler((o, e) =>
            {
                if (InternetState)
                {
                    this.toolStripStatusLabel2.Image = RFIDApplication.Properties.Resources.online;
                    this.toolStripStatusLabel2.Text = "在线";
                }
                else
                {
                    this.toolStripStatusLabel2.Image = RFIDApplication.Properties.Resources.offline;
                    this.toolStripStatusLabel2.Text = "离线";
                }
                this.ifCanConnectToServer = InternetState;
            }));
        }
        void bind()
        {
            try
            {
                this.DetailsForUpload = new Queue<Models.DeviceData>();


                this.backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {

            }
        }
        private void AddEetailsToUpload(Models.DeviceData details)
        {
            this.DetailsForUpload.Enqueue(details);
        }
        private void upload()//修改上传列表校验情况，上传成功更新没成功的情况
        {
            if (this.DetailsForUpload.Count == 0 || !this.ifCanConnectToServer)
            {
                Thread.Sleep(5000);
                upload();
            }
            else
            {
                Models.DeviceData DeviceData = this.DetailsForUpload.Dequeue();
                if (DeviceData.IfUpload == 0)
                {
                    SendProjectDetailsToServer(DeviceData);//上传
                }
                else
                {
                    BLL.FunctionResult result = this.deviceBLL.Edit(DeviceData);
                    if (result.ExcuteState)
                    {

                    }
                    else
                    {
                        this.DetailsForUpload.Enqueue(DeviceData);//上传成功 更新失败
                    }
                }
            }
        }
        private void SendProjectDetailsToServer(Models.DeviceData DeviceData)
        {

            Models.DeviceData dd = deviceBLL.GetModel(DeviceData.DataId);
            ServerOption.ServerDataReceive rec = new ServerOption.ServerDataReceive();
            Models.DataForSend ds = new Models.DataForSend()
            {
                Biaoduan = RFIDApplication.Properties.Settings.Default.BiaoduanCode,
                Item = dd
            };
            string str = Newtonsoft.Json.JsonConvert.SerializeObject(ds);

            string urlstr = "http://www.intellconst.com/wbs/YLJMenuRFID.aspx?username=YLJJK&password=YLJJK123&TypeID=SendYLJDATA";
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("para", str);
            string Str = postSynchronous(urlstr, parameters);

            if (Str.IndexOf("Sucess") >= 0)
            {
                DeviceData.IfUpload = 1;
                BLL.FunctionResult result = this.deviceBLL.Edit(DeviceData);
                if (result.ExcuteState)
                {

                }
                else
                {
                    this.DetailsForUpload.Enqueue(DeviceData);//上传成功 更新失败
                }
            }
            else if (Str.ToLower().Contains("chongfu"))
            {
                dd.NotUploadReason = "RFID重复";
                this.deviceBLL.Edit(dd);
            }
            else
            {
                this.DetailsForUpload.Enqueue(DeviceData);//上传失败 添加到待上传队列
            }
            setProcess();
            upload();
        }
        ///////
        public string dictionaryToPostString(Dictionary<string, string> postVariables)
        {
            string postString = "";
            foreach (KeyValuePair<string, string> pair in postVariables)
            {
                postString += HttpUtility.UrlEncode(pair.Key) + "=" +
                    HttpUtility.UrlEncode(pair.Value) + "&";
            }

            return postString;
        }
        public Dictionary<string, string> postStringToDictionary(string postString)
        {
            char[] delimiters = { '&' };
            string[] postPairs = postString.Split(delimiters);

            Dictionary<string, string> postVariables = new Dictionary<string, string>();
            foreach (string pair in postPairs)
            {
                char[] keyDelimiters = { '=' };
                string[] keyAndValue = pair.Split(keyDelimiters);
                if (keyAndValue.Length > 1)
                {
                    postVariables.Add(HttpUtility.UrlDecode(keyAndValue[0]),
                        HttpUtility.UrlDecode(keyAndValue[1]));
                }
            }

            return postVariables;
        }
        public string postSynchronous(string url, Dictionary<string, string> postVariables)
        {
            string result = null;
            try
            {
                string postString = dictionaryToPostString(postVariables);
                byte[] postBytes = Encoding.ASCII.GetBytes(postString);

                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = postBytes.Length;

                Stream postStream = webRequest.GetRequestStream();
                postStream.Write(postBytes, 0, postBytes.Length);
                postStream.Close();

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                Console.WriteLine(webResponse.StatusCode);
                Console.WriteLine(webResponse.Server);

                Stream responseStream = webResponse.GetResponseStream();
                StreamReader responseStreamReader = new StreamReader(responseStream);
                result = responseStreamReader.ReadToEnd();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return result;
        }


        ///////
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                work(this.backgroundWorker1);
            }
            catch (Exception ex)
            {

            }
        }
        private bool work(BackgroundWorker bk)
        {
            DataTable t = ProjectBLL.GetData(" ProjectLevel = " + Models.ProjectLevels.项目工程);
            this.Invoke(new EventHandler((a, b) =>
            {
                this.comboBox1.DisplayMember = "ProjectName";
                this.comboBox1.ValueMember = "ProjectId";
                this.comboBox1.DataSource = t;
                this.Instar.SelectedIndex = 4;
                this.yangsheng.SelectedIndex = 0;
                this.GroupInfo.SelectedIndex = 0;
            }));
            DataTable table = this.deviceBLL.GetData(" ifUpload = 0 ");
            if (table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    Models.DeviceData detail = deviceBLL.GetModelByRow(row);
                    this.DetailsForUpload.Enqueue(detail);
                }
            }

            setProcess();
            upload();
            return true;
        }
        private void setProcess()
        {
            this.Invoke(new EventHandler((o, e) =>
            {
                this.DeviceDataTotal = this.deviceBLL.GetData().Rows.Count;
                int n = this.deviceBLL.GetData(" IfUpload = 1 ").Rows.Count;
                if (this.DeviceDataTotal == n)
                {
                    this.DataForUploadLabel.Visible = false;
                    SetStateTextAndProcessBar(string.Format("数据库中共有{0}条数据，其中{1}条已上传，0条未上传！", this.DeviceDataTotal, n), this.DeviceDataTotal, n);

                }
                else
                {
                    SetStateTextAndProcessBar(string.Format("数据库中共有{0}条数据，其中{1}条已上传,", this.DeviceDataTotal, n), this.DeviceDataTotal, n);

                    this.DataForUploadLabel.Visible = true;
                }
            }));
        }
        private void SetStateTextAndProcessBar(string message, int total, int cuur)
        {
            this.Invoke(new EventHandler((o, e) =>
            {
                this.toolStripStatusLabel1.Text = message;
                this.DataForUploadLabel.Text = string.Format("{0}条未上传！", total - cuur);
                this.DataForUploadLabel.ForeColor = Color.Red;
            }));
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            QueryForm from = new QueryForm();
            from.WindowState = FormWindowState.Maximized;
            from.StartPosition = FormStartPosition.CenterParent;

            from.ShowDialog();
            from.WindowState = FormWindowState.Maximized;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            Contact contact = new Contact();
            contact.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            contact.StartPosition = FormStartPosition.CenterParent;
            contact.ShowInTaskbar = false;
            contact.ShowDialog();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ChangePassword contact = new ChangePassword();
            contact.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            contact.StartPosition = FormStartPosition.CenterParent;
            contact.ShowInTaskbar = false;
            contact.ShowDialog();

        }
    }
}
