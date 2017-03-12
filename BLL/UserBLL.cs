using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.Security.Cryptography;

namespace BLL
{
    public class UserBLL:BaseBll<Models.Users>
    {
        public UserBLL() {
            this.Context = new DAL.UsersDal();
        }
        protected DAL.UsersDal Dal = new DAL.UsersDal();
        /// <summary>
        /// 根据用户名密码登录
        /// </summary>
        /// <param name="user">用户类，应包含用户名和密码</param>
        /// <param name="Remember">是否记住密码</param>
        /// <returns>登录是否成功</returns>
        public bool Login(Models.Users user,bool Remember){
            string sqlWhere = " UserName =@UserName and UserPassword =@UserPassword ";
            System.Data.OleDb.OleDbParameter[] para ={
                                                  new OleDbParameter("@UserName",user.UserName),
                                                  new OleDbParameter("@UserPassword",user.UserPassword),
                                                  };
            Models.Users UserTemp = Dal.GetModelByWhere(sqlWhere, para);
            if (UserTemp != null) {
                if (Remember && !UserTemp.UserPassword.Equals(user.PasswordRemembered))
                {
                    UserTemp.PasswordRemembered = user.UserPassword;
                    Dal.Edit(UserTemp);
                }
                LoginInfo.UserInfo = UserTemp;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据记住的密码登录
        /// </summary>
        /// <returns>登录状态</returns>
        public bool Login() {
            DataTable table = Dal.GetData();
            if (table.Rows.Count == 0)
                return false;
            Models.Users user = Dal.GetModelByRow(table.Rows[0]);
            if (user.UserPassword.Equals(user.PasswordRemembered))
            {

                LoginInfo.UserInfo = user;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据通用密码登录
        /// </summary>
        /// <param name="DefaultPassword">由管理员提供的密码</param>
        /// <returns>登录状态</returns>
        public bool Login(string DefaultPassword) {
            string str = Utils.GetDefaultPassword();
            if (str.Equals(DefaultPassword))
            {
                DataTable table = Dal.GetData();
                if (table.Rows.Count == 0)
                    return false;
                Models.Users user = Dal.GetModelByRow(table.Rows[0]);
                LoginInfo.UserInfo = user;
                return true;
            }
            else {
                return false;
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="oldPassword">原密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        public bool UpdatePassword(string oldPassword,string newPassword,bool ifRemember) {
            string userId = LoginInfo.UserInfo.UserId;
            Models.Users user = Dal.GetModel(userId);
            if (user.UserPassword.Equals(oldPassword) ||this.Login(oldPassword) )
            {
                user.UserPassword = newPassword;
                if (ifRemember) {
                    user.PasswordRemembered = newPassword;
                }
                return Dal.Edit(user) > 0;
            } 
            return false;
        }

        public override List<ValidataError> Validata(Models.Users Model)
        {
            return new List<ValidataError>();
        }
    }
}
