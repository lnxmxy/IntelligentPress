using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class ProjectDetails
    {
        /// <summary>
        /// 工程详情编号
        /// </summary>
        public string DetailsId { get; set; }

        /// <summary>
        /// 工程详情名称
        /// </summary>
        public string DetailsTilte { get; set; }

        /// <summary>
        /// 工程编号
        /// </summary>
        public string ProjectId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 工程项目编号
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// 采样里程
        /// </summary>
        public string Mileage { get; set; }

        /// <summary>
        /// 设计强度
        /// </summary>
        public string Intensity { get; set; }

        /// <summary>
        /// 每立方水泥用量
        /// </summary>
        public string CementContent { get; set; }

        /// <summary>
        /// 设计混合比
        /// </summary>
        public string MixDesign { get; set; }
        
        /// <summary>
        /// 所属项目
        /// </summary>
        public Project Project { get; set; }


        /// <summary>
        /// 项目详情状态：0 未绑定数据 1 已绑定数据 2 已上传服务器
        /// </summary>
        public int DetailState { get; set; }

     
    }
}
