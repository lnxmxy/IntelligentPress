using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class DeviceDataArea
    {
        /// <summary>
        /// 范围值
        /// </summary>
        public string AreaId { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxValue { get; set; }
        /// <summary>
        /// 最小值
        /// </summary>
        public double MinValue { get; set; }
    }
}
