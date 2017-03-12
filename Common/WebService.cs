using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Common.ServiceReference2;
namespace Common
{
    public class WebService
    {
        private SDKClientClient client;
        private string userName = "7SDK-LHW-0588-QCZTQ";
        private string userPassWord = "896510";
        #region 私有方法
        public SDKClientClient Client
        {
            get {
                if (client == null)
                {
                    client = new SDKClientClient();
                }
                return client; 
            }
            set { client = value; }
        }
        public string sendMessage(string message, string mobileNo) {
            string result = "";
            string[] array = { mobileNo };
            result = sendMessage(array, message);
            return result;
        }
        /// <summary>
        /// sendMessage 发送短信
        /// </summary>
        /// <param name="softwareSerialNo">软件序列号</param>
        /// <param name="key">key</param>
        /// <param name="sendTime">定时短信</param>
        /// <param name="mobiles">手机号码</param>
        /// <param name="smsContent">短信内容</param>
        /// <param name="addSerial">扩展号码</param>
        /// <param name="srcCharset">字符编码</param>
        /// <param name="smsPriority">短信等级</param>
        /// <param name="smsID">短信ID</param>
        /// <returns></returns>
        private string sendMessage( String[] mobiles, String smsContent,long smsID=0)
        {
            try
            {
                string result = "";
                // softwareSerialNo,  key,  sendTime,  mobiles,  smsContent,  addSerial, srcCharset,  smsPriority,  smsID
                int n = Client.sendSMS(userName, userPassWord, "", mobiles, smsContent, "", "GBK", 1, smsID);
                result = getResult(n);
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
        private string getResult(int str) {
            string result = "";
            try
            {
                switch (str)
                {
                    case 0	    : result="短信发送成功";break;
                    case 305	: result="服务器端返回错误，错误的返回值（返回值不是数字字符串）";break;
                    case 101	: result="客户端网络故障";break;
                    case 303	: result="客户端网络故障";break;
                    case 307	: result="目标电话号码不符合规则，电话号码必须是以0、1开头";break;
                    case 997	: result="平台返回找不到超时的短信，该信息是否成功无法确定";break;
                    case 998	: result="由于客户端网络问题导致信息发送超时，该信息是否成功下发无法确定";break;
                    case -1	    : result="系统异常";break;
                    case -2	    : result="客户端异常";break;
                    case -101	: result="命令不被支持";break;
                    case -104	: result="请求超过限制";break;
                    case -117	: result="发送短信失败";break;
                    case -1104	: result="路由失败，请联系系统管理员";break;
                    case -9016	: result="发送短信包大小超出范围";break;
                    case -9017	: result="发送短信内容格式错误";break;
                    case -9018	: result="发送短信扩展号格式错误";break;
                    case -9019	: result="发送短信优先级格式错误";break;
                    case -9020	: result="发送短信手机号格式错误";break;
                    case -9021	: result="发送短信定时时间格式错误";break;
                    case -9022	: result="发送短信唯一序列值错误";break;
                    case -9001	: result="序列号格式错误";break;
                    case -9002	: result="密码格式错误";break;
                    case -9003	: result="客户端Key格式错误";break;
                    case -9025: result = "客户端请求sdk5超时"; break;
                    default: result = "未知错误"; break;

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
