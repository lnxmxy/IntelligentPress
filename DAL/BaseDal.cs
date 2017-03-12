using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace DAL
{
    public abstract class BaseDal<T>
    {
        protected string SelectSql  {get;set;}
        protected string CreateSql { get; set; }
        protected string EditSql { get; set; }
        protected string DeleteSql { get; set; }
        protected DB.AccessDB db { get; set; }
        public BaseDal() {
            this.db = new DB.AccessDB();
        }
        public DataTable GetData(string where = "")
        {
            if (!Utils.IsEmptyOrNull(where) && !where.Trim().StartsWith("and"))
            {
                where = " and " + where;
            }
            DataTable table = new DataTable();

            string sql = SelectSql + where;
            table = db.Query(sql).Tables[0];
            return table;
        }
        public DataTable GetData(string where ,params OleDbParameter[]para)
        {
            if (!where.Trim().StartsWith("and")) {
                where = " and " + where;
            }
            DataTable table = new DataTable();
            string sql = SelectSql + where;
            table = db.Query(sql, para).Tables[0];
            return table;
        }
        public DataTable Query(string sql) {
            return db.Query(sql).Tables[0];
        }
        public DataTable Query(string sql, params OleDbParameter[] para) {
            return db.Query(sql, para).Tables[0];
        }
        public int ExcuteSql(string sql) {
            return db.ExecuteSql(sql);
        }
        public int ExcuteSql(string sql, params  OleDbParameter[] para)
        {
            return db.ExecuteSql(sql,para);
        }
        public abstract T GetModel(string id);
        public abstract int Create(T t);
        public abstract int Edit(T t);
        public abstract int Delete(T t); 
        public abstract T GetModelByRow(DataRow row);
        public int Delete(string Where)
        {
            string sql = DeleteSql + Where;
            int n = db.ExecuteSql(sql);
            return n;
        }
        public T GetModelByWhere(string Where)
        {
            if (!Where.Trim().StartsWith(" and "))
            {
                Where = " and " + Where;
            }
            if (!Where.StartsWith(" "))
            {
                Where = " " + Where;
            }
            DataTable table = db.Query(SelectSql + " " + Where).Tables[0];
            if (table.Rows.Count > 0)
            {
                return GetModelByRow(table.Rows[0]);
            }
            else
            {
                return default(T);
            }
        }
        /// <summary>
        /// 返回指定的实体
        /// </summary>
        /// <param name="Where">And开始的条件语句</param>
        /// <param name="paras">参数值</param>
        /// <returns>符合条件的实体或null</returns>
        public  T GetModelByWhere(string Where,params OleDbParameter[] paras)
        {
            if (!Where.StartsWith(" and ")) {
                Where = " and " + Where;
            }
            if (!Where.StartsWith(" ")) {
                Where = " " + Where;
            }
            DataTable table = db.Query(SelectSql + " " + Where, paras).Tables[0];
            if (table.Rows.Count > 0)
            {
                return GetModelByRow(table.Rows[0]);
            }
            else
            {
                return default(T);
            }

        }
    }
}
