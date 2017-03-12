using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class DeviceData
    {
        /// <summary>
        /// 数据编号
        /// </summary>
        public string DataId { get; set; }

        /// <summary>
        /// 分组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string DeviceValue { get; set; }

        /// <summary>
        /// 工程项目编号
        /// </summary>
        public string ProjectDetailId { get; set; }

        /// <summary>
        /// 是否上传
        /// </summary>
        public int IfUpload { get; set; }

        /// <summary>
        /// 未上传原因
        /// </summary>
        public string NotUploadReason { get; set; }

        /// <summary>
        /// 龄期
        /// </summary>
        public string Instar { get; set; }

        /// <summary>
        /// 养生方式
        /// </summary>
        public string YangSheng { get; set; }

        /// <summary>
        /// 采样日期
        /// </summary>
        public string SamplingDate { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreateDate { get; set; }

        public ProjectDetails ProjectDetail { get; set; }
    }
}
