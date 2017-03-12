using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Xml;
using System.Collections;

namespace Common
{
    public class Method
    {
        public static string GetUserIP()
        {
            string result = "IP地址未知";
            if (System.Web.HttpContext.Current.Request.UserHostAddress != null)
            {
                result = System.Web.HttpContext.Current.Request.UserHostAddress.ToString();
            }
            return result;
        }

        #region//ShowPager:显示分页效果.
        /// <summary>
        /// 显示分页效果.
        /// </summary>
        /// <param name="nPage">当前页.</param>
        /// <param name="maxPage">最大页.</param>
        /// <returns>返回字符串.</returns>
        public static string ShowPager(int nPage, int maxPage)
        {
            int endpage = 1;
            string qdkPager = "";
            if (maxPage <= 11)
            {
                if (nPage > 1)
                {
                    qdkPager = "<a href=\"?page=1\">首页<</a> ";
                }
                else
                {
                    qdkPager = "<a class=\"noLink\">首页<</a> ";
                }
                //qdkPager += "<div>上10页</div> ";
                endpage = maxPage;
                for (int ii = 1; ii <= maxPage; ii++)
                {
                    if (nPage != ii)
                    {
                        qdkPager += "<a href=\"?page=" + ii.ToString() + "\">" + ii.ToString() + "</a> ";
                    }
                    else
                    {
                        qdkPager += "<a class=\"nowPage\">" + ii.ToString() + "</a> ";
                    }
                }
                //qdkPager += "<div>下10页</div> ";
                if (nPage < maxPage)
                {
                    qdkPager += "<a href=\"?page=" + maxPage + "\">>尾页</a> ";
                }
                else
                {
                    qdkPager += "<a class=\"noLink\">>尾页</a> ";
                }
            }
            else
            {
                int maxCount = 0;
                if (maxPage % 10 == 0)
                {
                    maxCount = maxPage / 10;
                }
                else
                {
                    maxCount = maxPage / 10 + 1;
                }
                if (nPage > 1)
                {
                    qdkPager = "<a href=\"?page=1\">首页<</a> ";
                }
                else
                {
                    qdkPager = "<a class=\"noLink\">首页<</a> ";
                }

                int nowpagecount = 0;
                if (nPage % 10 == 0)
                {
                    nowpagecount = nPage / 10;
                }
                else
                {
                    nowpagecount = nPage / 10 + 1;
                }
                if (nowpagecount > 1)
                {
                    qdkPager += "<a href=\"?page=" + ((nowpagecount - 2) * 10 + 1) + "\">上10页</a> ";
                }
                else
                {
                    qdkPager += "<a class=\"noLink\">上10页</a> ";
                }
                endpage = ((nowpagecount - 1) * 10 + 11) < maxPage ? ((nowpagecount - 1) * 10 + 11) : maxPage;
                for (int ii = ((nowpagecount - 1) * 10) > 0 ? ((nowpagecount - 1) * 10) : 1; ii <= endpage; ii++)
                {
                    if (nPage != ii)
                    {
                        qdkPager += "<a href=\"?page=" + ii.ToString() + "\">" + ii.ToString() + "</a> ";
                    }
                    else
                    {
                        qdkPager += "<a class=\"nowPage\">" + ii.ToString() + "</a> ";
                    }
                }
                if (nowpagecount < maxCount)
                {
                    qdkPager += "<a href=\"?page=" + (nowpagecount * 10 + 1) + "\">下10页</a> ";
                }
                else
                {
                    qdkPager += "<a class=\"noLink\">下10页</a> ";
                }
                if (nPage < maxPage)
                {
                    qdkPager += "<a href=\"?page=" + maxPage + "\">>尾页</a> ";
                }
                else
                {
                    qdkPager += "<a class=\"noLink\">>尾页</a> ";
                }
            }
            return qdkPager;
        }

        public static string ShowPager(int nPage, int maxPage, string strLink)
        {
            int endpage = 1;
            string qdkPager = "";
            if (maxPage <= 11)
            {
                if (nPage > 1)
                {
                    qdkPager = "<a href=\"?page=1&" + strLink + "\">首页<</a> ";
                }
                else
                {
                    qdkPager = "<a class=\"noLink\">首页<</a> ";
                }
                //qdkPager += "<div>上10页</div> ";
                endpage = maxPage;
                for (int ii = 1; ii <= maxPage; ii++)
                {
                    if (nPage != ii)
                    {
                        qdkPager += "<a href=\"?page=" + ii.ToString() + "&" + strLink + "\">" + ii.ToString() + "</a> ";
                    }
                    else
                    {
                        qdkPager += "<a class=\"nowPage\">" + ii.ToString() + "</a> ";
                    }
                }
                //qdkPager += "<div>下10页</div> ";
                if (nPage < maxPage)
                {
                    qdkPager += "<a href=\"?page=" + maxPage + "&" + strLink + "\">>尾页</a> ";
                }
                else
                {
                    qdkPager += "<a class=\"noLink\">>尾页</a> ";
                }
            }
            else
            {
                int maxCount = 0;
                if (maxPage % 10 == 0)
                {
                    maxCount = maxPage / 10;
                }
                else
                {
                    maxCount = maxPage / 10 + 1;
                }
                if (nPage > 1)
                {
                    qdkPager = "<a href=\"?page=1&" + strLink + "\">首页<</a> ";
                }
                else
                {
                    qdkPager = "<a class=\"noLink\">首页<</a> ";
                }

                int nowpagecount = 0;
                if (nPage % 10 == 0)
                {
                    nowpagecount = nPage / 10;
                }
                else
                {
                    nowpagecount = nPage / 10 + 1;
                }
                if (nowpagecount > 1)
                {
                    qdkPager += "<a href=\"?page=" + ((nowpagecount - 2) * 10 + 1) + "&" + strLink + "\">上10页</a> ";
                }
                else
                {
                    qdkPager += "<a class=\"noLink\">上10页</a> ";
                }
                endpage = ((nowpagecount - 1) * 10 + 11) < maxPage ? ((nowpagecount - 1) * 10 + 11) : maxPage;
                for (int ii = ((nowpagecount - 1) * 10) > 0 ? ((nowpagecount - 1) * 10) : 1; ii <= endpage; ii++)
                {
                    if (nPage != ii)
                    {
                        qdkPager += "<a href=\"?page=" + ii.ToString() + "&" + strLink + "\">" + ii.ToString() + "</a> ";
                    }
                    else
                    {
                        qdkPager += "<a class=\"nowPage\">" + ii.ToString() + "</a> ";
                    }
                }
                if (nowpagecount < maxCount)
                {
                    qdkPager += "<a href=\"?page=" + (nowpagecount * 10 + 1) + "&" + strLink + "\">下10页</a> ";
                }
                else
                {
                    qdkPager += "<a class=\"noLink\">下10页</a> ";
                }
                if (nPage < maxPage)
                {
                    qdkPager += "<a href=\"?page=" + maxPage + "&" + strLink + "\">>尾页</a> ";
                }
                else
                {
                    qdkPager += "<a class=\"noLink\">>尾页</a> ";
                }
            }
            return qdkPager;
        }
        #endregion

        #region//sltListBox:将下拉框中等于指定值的选项选中.
        /// <summary>
        /// 将下拉框中等于指定值的选项选中.
        /// </summary>
        /// <param name="MyDD">要设置的下拉框.</param>
        /// <param name="sltValue">要检查的值.</param>
        public static void sltListBox(ref System.Web.UI.WebControls.DropDownList MyDD, string sltValue)
        {
            int ii = 0;
            MyDD.SelectedItem.Selected = false;
            for (ii = 0; ii < MyDD.Items.Count; ii++)
            {
                if (MyDD.Items[ii].Value == sltValue)
                {
                    MyDD.Items[ii].Selected = true;
                }
            }
        }
        #endregion

        #region GetChecked:用于在绑定数据时选定CheckBox的checked状态.
        /// <summary>
        /// 用于在绑定数据时选定CheckBox的checked状态.
        /// </summary>
        /// <param name="val">绑定的数据.</param>
        /// <returns>返回一个字符串.</returns>
        public static string GetChecked(object val)
        {
            string result = "";
            if (Convert.ToBoolean(val))
            {
                result = "checked";
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 验证签名
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public static bool CheckSignature(String signature, String timestamp, String nonce)
        {
            String[] arr = new String[] { "ipoo11ja", timestamp, nonce };//token:ipoo11ja
            // 将token、timestamp、nonce三个参数进行字典序排序  
            Array.Sort<String>(arr);

            StringBuilder content = new StringBuilder();
            for (int i = 0; i < arr.Length; i++)
            {
                content.Append(arr[i]);
            }

            String tmpStr = SHA1_Encrypt(content.ToString());


            // 将sha1加密后的字符串可与signature对比，标识该请求来源于微信  
            return tmpStr != null ? tmpStr.Equals(signature) : false;
        }
        /// <summary>
        /// 使用缺省密钥给字符串加密
        /// </summary>
        /// <param name="Source_String"></param>
        /// <returns></returns>
        public static string SHA1_Encrypt(string Source_String)
        {
            byte[] StrRes = Encoding.Default.GetBytes(Source_String);
            HashAlgorithm iSHA = new SHA1CryptoServiceProvider();
            StrRes = iSHA.ComputeHash(StrRes);
            StringBuilder EnText = new StringBuilder();
            foreach (byte iByte in StrRes)
            {
                EnText.AppendFormat("{0:x2}", iByte);
            }
            return EnText.ToString();
        }
        /// <summary>
        /// 将xml文件转换成Hashtable
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static Hashtable ParseXml(String xml)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            XmlNode bodyNode = xmlDocument.ChildNodes[0];
            Hashtable ht = new Hashtable();
            if (bodyNode.ChildNodes.Count > 0)
            {
                foreach (XmlNode xn in bodyNode.ChildNodes)
                {
                    ht.Add(xn.Name, xn.InnerText);
                }
            }
            return ht;
        }
        
    }
}
