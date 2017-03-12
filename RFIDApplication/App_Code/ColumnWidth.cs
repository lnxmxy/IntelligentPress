using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RFIDApplication.App_Code
{
    public static class ColumnWidth
    {
        public static Dictionary<string, int> Column_Name_Width = new Dictionary<string, int> {
            // 单位工程 分部工程 子分部工程 项目工程 实验分组 实验数据 龄期 取样时间 分项工程编号 取样里程 设计强度 每立方水泥用量 设计混合比 ;
               {"试验分组",34},
               {"龄期",40},
               {"分项工程编号",55},
               {"取样时间",70},
               {"RFID卡号",100},
               {"取样里程",55},//
               {"设计强度",34},//
               {"立方水泥用量",48},
               {"设计混合比",190}//
               
        };
    }
}
