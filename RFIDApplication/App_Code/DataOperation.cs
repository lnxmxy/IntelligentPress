using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Drawing.Printing;
using System.Data;

namespace RFIDManageSystem
{
    class DataOperation
    {
        #region 保存页面设置至XML
        public void SavePageSettings(PrintDocument printDocument)
        {
            //修改xml节
            XmlDocument doc = new XmlDocument();
            doc.Load("PageSettings.xml");

            XmlNode DirectNode = doc.SelectSingleNode(@"//add[@key='PaperDirect']");
            XmlElement DirectEle = (XmlElement)DirectNode;
            DirectEle.SetAttribute("value", printDocument.DefaultPageSettings.Landscape.ToString());

            XmlNode SizeNode = doc.SelectSingleNode(@"//add[@key='PaperSize']");
            XmlElement SizeEle = (XmlElement)SizeNode;
            SizeEle.SetAttribute("value", printDocument.DefaultPageSettings.PaperSize.PaperName.ToString());
            XmlNode UpNode = doc.SelectSingleNode(@"//add[@key='Margin_Up']");
            XmlElement UpEle = (XmlElement)UpNode;
            UpEle.SetAttribute("value", printDocument.DefaultPageSettings.Margins.Top.ToString());

            XmlNode DownNode = doc.SelectSingleNode(@"//add[@key='Margin_Down']");
            XmlElement DownEle = (XmlElement)DownNode;
            DownEle.SetAttribute("value", printDocument.DefaultPageSettings.Margins.Bottom.ToString());

            XmlNode LeftNode = doc.SelectSingleNode(@"//add[@key='Margin_Left']");
            XmlElement LeftEle = (XmlElement)LeftNode;
            LeftEle.SetAttribute("value", printDocument.DefaultPageSettings.Margins.Left.ToString());

            XmlNode RightNode = doc.SelectSingleNode(@"//add[@key='Margin_Right']");
            XmlElement RightEle = (XmlElement)RightNode;
            RightEle.SetAttribute("value", printDocument.DefaultPageSettings.Margins.Right.ToString());

            XmlNode HeightNode = doc.SelectSingleNode(@"//add[@key='PaperHeight']");
            XmlElement HeightEle = (XmlElement)HeightNode;
            HeightEle.SetAttribute("value", printDocument.DefaultPageSettings.PaperSize.Height.ToString());

            XmlNode WidthNode = doc.SelectSingleNode(@"//add[@key='PaperWidth']");
            XmlElement WidthEle = (XmlElement)WidthNode;
            WidthEle.SetAttribute("value", printDocument.DefaultPageSettings.PaperSize.Width.ToString());

            doc.Save("PageSettings.xml");
        }
        #endregion

        #region 读取页面设置信息
        public void ReadPageSettings(PrintDocument printDocument)
        {
            string PaperDirect = "";
            string PaperSize = "";
            string Margin_Up = "";
            string Margin_Down = "";
            string Margin_Left = "";
            string Margin_Right = "";
            string PaperHeight = "";
            string PaperWidth = "";

            XmlDocument doc = new XmlDocument();
            doc.Load("PageSettings.xml");

            XmlNode DirectNode = doc.SelectSingleNode(@"//add[@key='PaperDirect']");
            XmlElement DirectEle = (XmlElement)DirectNode;
            PaperDirect = DirectEle.GetAttribute("value");

            XmlNode SizeNode = doc.SelectSingleNode(@"//add[@key='PaperSize']");
            XmlElement SizeEle = (XmlElement)SizeNode;
            PaperSize = SizeEle.GetAttribute("value");

            XmlNode UpNode = doc.SelectSingleNode(@"//add[@key='Margin_Up']");
            XmlElement UpEle = (XmlElement)UpNode;
            Margin_Up = UpEle.GetAttribute("value");

            XmlNode DownNode = doc.SelectSingleNode(@"//add[@key='Margin_Down']");
            XmlElement DownEle = (XmlElement)DownNode;
            Margin_Down = DownEle.GetAttribute("value");

            XmlNode LeftNode = doc.SelectSingleNode(@"//add[@key='Margin_Left']");
            XmlElement LeftEle = (XmlElement)LeftNode;
            Margin_Left = LeftEle.GetAttribute("value");

            XmlNode RightNode = doc.SelectSingleNode(@"//add[@key='Margin_Right']");
            XmlElement RightEle = (XmlElement)RightNode;
            Margin_Right = RightEle.GetAttribute("value");

            XmlNode HeightNode = doc.SelectSingleNode(@"//add[@key='PaperHeight']");
            XmlElement HeightEle = (XmlElement)HeightNode;
            PaperHeight = HeightEle.GetAttribute("value");

            XmlNode WidthNode = doc.SelectSingleNode(@"//add[@key='PaperWidth']");
            XmlElement WidthEle = (XmlElement)WidthNode;
            PaperWidth = WidthEle.GetAttribute("value");

            if (PaperDirect == "True")
            {
                printDocument.DefaultPageSettings.Landscape = true;//设置横向打印
            }
            else
            {
                printDocument.DefaultPageSettings.Landscape = false;
            }
            printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize(PaperSize, int.Parse(PaperWidth), int.Parse(PaperHeight));
            printDocument.DefaultPageSettings.Margins.Top = int.Parse(Margin_Up);
            printDocument.DefaultPageSettings.Margins.Bottom = int.Parse(Margin_Down);
            printDocument.DefaultPageSettings.Margins.Left = int.Parse(Margin_Left);
            printDocument.DefaultPageSettings.Margins.Right = int.Parse(Margin_Right);
        }
        #endregion

    }
}
