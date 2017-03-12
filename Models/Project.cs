using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace Models
{
    public class Project  
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        public string ProjectId { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }
        /// <summary>
        /// 项目级别
        /// </summary>
        public int ProjectLevel { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int DisplayOrder { get; set; }
        /// <summary>
        /// 上级项目编号
        /// </summary>
        public string ParentProjectId { get; set; }
        /// <summary>
        /// 上级项目
        /// </summary>
        public Project ParentProject { get; set; } 
    }
}
