using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;

namespace Common
{
    public class Internet
    {
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(int Description, int ReservedValue);

        #region 方法一
        /// <summary>
        /// 用于检查网络是否可以连接互联网,true表示连接成功,false表示连接失败 
        /// </summary>
        /// <returns></returns>
        public static bool IsConnectInternet()
        {
            int Description = 0;
            return InternetGetConnectedState(Description, 0);
        }
        #endregion

        #region 方法二
        /// <summary>
        /// 用于检查IP地址或域名是否可以使用TCP/IP协议访问(使用Ping命令),true表示Ping成功,false表示Ping失败 
        /// </summary>
        /// <param name="strIpOrDName">输入参数,表示IP地址或域名</param>
        /// <returns></returns>
        public static bool PingIpOrDomainName(string strIpOrDName)
        {
            try
            {
                Ping objPingSender = new Ping();
                PingOptions objPinOptions = new PingOptions();
                objPinOptions.DontFragment = true;
                string data = "";
                byte[] buffer = Encoding.UTF8.GetBytes(data);
                int intTimeout = 120;
                PingReply objPinReply = objPingSender.Send(strIpOrDName, intTimeout, buffer, objPinOptions);
                string strInfo = objPinReply.Status.ToString();
                if (strInfo == "Success")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        #region 方法三
        public static bool GetHtmlPage(string url) {
            try
            {
                string str = Utils.HttpGet(url);
                return "Error_IDE".Equals(str);
            }
            catch (Exception ex) {
                return false;
            }
        }
        #endregion 
    }
    public delegate void InternetStateChangedDelegate(bool InternetState);
    public class InternetStateListener
    {
        private bool internetState = true;
        protected string ServerIp{get;set;}
        protected int Interval{get;set;}
        protected bool IfDebug { get; set; }
        public event InternetStateChangedDelegate InternetStateChanged;
        public bool InternetState
        {
            get { return internetState; }
            set
            {
                if (internetState != value)
                {
                    try
                    {
                        if (this.InternetStateChanged != null)
                        {
                            this.InternetStateChanged(value);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        internetState = value;
                    }
                }
                internetState = value;
            }
        }
        public InternetStateListener(string ip, int Interval, bool ifDebug = false)
        {
            this.ServerIp = ip;
            this.Interval = Interval;
            this.IfDebug = ifDebug;
        }
        public void Start() {

            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (this.IfDebug)
                    {
                        Random random = new Random();
                        this.InternetState = random.Next(10) % 2 == 0;
                    }
                    else
                    {
                       // this.InternetState = Internet.PingIpOrDomainName(ServerIp);
                        this.InternetState = Internet.GetHtmlPage("http://www.intellconst.com/wbs/YLJMenuRFID.aspx");
                    }
                    System.Threading.Thread.Sleep(Interval);
                }
            }).ContinueWith(t => {
                throw t.Exception;
            },System.Threading.Tasks.TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}
