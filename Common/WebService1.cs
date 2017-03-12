using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace Common
{
    public class WebService2
    {
        private ServiceReference1.SendsmsWebServiceClient client;
        private string userName = "7SDK-LHW-0588-QCZTQ";
        private string userPassWord = "896510";
        #region 私有方法
        public ServiceReference1.SendsmsWebServiceClient Client
        {
            get {
                if (client == null) {
                    client = new ServiceReference1.SendsmsWebServiceClient();
                }
                return client; 
            }
            set { client = value; }
        }
        public string sendMessage(string message, string mobileNo) {
            string result = "";
            string str = sendMessage(this.userName, this.userPassWord, mobileNo, message, "", "", "", "0");
            result = getResult(str);
            return result;
        }
        /// <summary>
        /// 发送普通短信
        /// </summary>
        /// <param name="username">用户名（必填）</param>
        /// <param name="password">密码（必填）</param>
        /// <param name="oldMobile">手机号，多个手机号为用半角 , 分开，如13899999999,13688888888(最多200个，必填)</param>
        /// <param name="content">发送内容（必填）</param>
        /// <param name="product_number">产品ID</param>
        /// <param name="dstime">定时时间，为空时表示立即发送（选填）格式：20130202120212</param>
        /// <param name="xh">扩展的小号,必须为数字，没有请留空</param>
        /// <param name="repeat">发送不可重复的短信, 1是可以重复，0不能重复</param>
        /// <returns>发送状态</returns>
        private string sendMessage(String username, String password, String oldMobile, String content, String product_number, String dstime, String xh, String repeat)
        {
            try
            {
                string result = "";
                string str = Client.sendSms(username, password, oldMobile, content, product_number, dstime, xh, repeat);
                result = getResult(str);
                return result;
            }
            catch (Exception ex) {
                return ex.Message;
            }
        }
        /// <summary>
        /// 获取发送状态
        /// </summary>
        /// <param name="str">表示状态的字符串</param>
        /// <returns>发送状态</returns>
        private string getResult(string str) {
            string result = "";
            try
            {
                switch (str)
                {
                    case "-1": result = "用户名或者密码不正确"; break;
                    case "2": result = "余额不够"; break;
                    case "3": result = "扣费失败（请联系客服）"; break;
                    case "6": result = "有效号码为空"; break;
                    case "7": result = "短信内容为空"; break;
                    case "8": result = "无签名，必须，格式：【签名】"; break;
                    case "9": result = "没有Url提交权限"; break;
                    case "10": result = "发送号码过多,最多支持200个号码"; break;
                    case "11": result = "产品ID异常"; break;
                    case "12": result = "参数异常"; break;
                    case "13": result = "参数异常"; break;
                    case "15": result = "Ip验证失败"; break;
                    case "19": result = "短信内容过长，最多支持500个"; break;
                    case "20": result = "定时时间不正确：格式：20130202120212(14位数字)"; break;
                    default: string[] strArray = str.Split(',');
                        string str1 = strArray[0];
                        string messageId = strArray[1];
                        if (str1 == "0")
                        {
                            result = "编号为：“" + messageId + "”的短信发送失败"; break;
                        }
                        else if (str1 == "1")
                        {
                            result = "编号为：“" + messageId + "”的短信发送成功"; break;
                        }
                        else if (str1 == "5")
                        {
                            result = "编号为：“" + messageId + "”的短信定时成功"; break;
                        }
                        else
                        {

                        }
                        break;
                }
            }
            catch (Exception ex) {
                result = ex.Message;
            }
             return result;
        }
        #endregion
    }
}
