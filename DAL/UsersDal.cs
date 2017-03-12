using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Models;
using System.Data.OleDb;

namespace DAL
{
    public class UsersDal : BaseDal<Users>
    { 
        public UsersDal():base()
        {
            this.SelectSql = " select * from [User] where 1=1";
            this.CreateSql = " insert into [User] values (@UserId,@UserName,@UserPassword,@PasswordRemembered) ";
            this.EditSql = " update [User] set UserName=@UserName,UserPassword=@UserPassword,PasswordRemembered=@PasswordRemembered where UserId=@UserId ";
            this.DeleteSql = "Delete from [user] where 1=1 "; 
        }

        public override Users GetModel(string id)
        { 
            DataTable table = new DataTable();
            string sql = SelectSql + " and UserId=@UserId ";
            table = db.Query(sql, new OleDbParameter("@UserId", id)).Tables[0];
            if (table.Rows.Count > 0)
            {
                Users user = new Users();
                user.UserId = table.Rows[0]["UserId"].ToString();
                user.UserName = table.Rows[0]["UserName"].ToString();
                user.UserPassword = table.Rows[0]["UserPassword"].ToString();
                user.PasswordRemembered = table.Rows[0]["PasswordRemembered"].ToString();
                return user;
            }
            else
            {
                return null;
            }
        }
        public override int Create(Users user)
        {
            OleDbParameter[] para = { 
                new OleDbParameter("@UserId",user.UserId),
                new OleDbParameter("@UserName",user.UserName),
                new OleDbParameter("@UserPassword",user.UserPassword),
                new OleDbParameter("@PasswordRemembered",user.PasswordRemembered)
                                    };
            int n = db.ExecuteSql(CreateSql, para);
            return n;
        }
        public override int Edit(Users user)
        {
            OleDbParameter[] para = { 
                new OleDbParameter("@UserName",user.UserName),
                new OleDbParameter("@UserPassword",user.UserPassword),
                new OleDbParameter("@PasswordRemembered",user.PasswordRemembered),
                new OleDbParameter("@UserId",user.UserId)
                                    };
            int n = db.ExecuteSql(EditSql, para);
            return n;
        }
        public override int Delete(Users user)
        {
            string sql = DeleteSql + " and UserId=@UserId ";
            int n = db.ExecuteSql(sql, new OleDbParameter("@UserId", user.UserId));
            return n;
        }

        public override Users GetModelByRow(DataRow row)
        {
            try
            {
                Users user = new Users();
                user.UserId = row["UserId"].ToString();
                user.UserName = row["UserName"].ToString();
                user.UserPassword = row["UserPassword"].ToString();
                user.PasswordRemembered = row["PasswordRemembered"].ToString();
                return user;
            }
            catch {
                return null;
            }
        }
    }
}
