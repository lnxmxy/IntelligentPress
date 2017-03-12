using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models
{
    public class Users
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string UserPassword { get; set; }
        /// <summary>
        /// 记住的密码
        /// </summary>
        public string PasswordRemembered { get; set; }
    }
}
