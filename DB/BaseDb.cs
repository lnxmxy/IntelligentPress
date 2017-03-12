using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace DB
{
    public abstract class BaseDb<dbCommond, dbParameter, dbConnection, dbReader, dbTransaction> : IBaseDB<dbCommond, dbParameter, dbConnection, dbReader, dbTransaction>

        where dbCommond : DbCommand
        where dbParameter : DbParameter
        where dbConnection : DbConnection
        where dbReader : DbDataReader
        where dbTransaction : DbTransaction
    {

        public string connectionString = "";
        
        public int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = this.GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        public bool Exists(string strSql)
        {
            object obj = this.GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public abstract bool Exists(string strSql, params dbParameter[] cmdParms);

        public abstract  int ExecuteSql(string SQLString); 

        public abstract int ExecuteSqlByTime(string SQLString, int Times);

        public abstract void ExecuteSqlTran(System.Collections.ArrayList SQLStringList);

        public abstract int ExecuteSqlInsertImg(string strSQL, byte[] fs);

        public abstract object GetSingle(string SQLString);

        public abstract dbReader ExecuteReader(string strSQL);

        public abstract System.Data.DataSet Query(string SQLString);

        public abstract DataSet Query(string SQLString, int Times);

        public abstract int ExecuteSql(string SQLString, params dbParameter[] cmdParms);

        public abstract void ExecuteSqlTran(System.Collections.Hashtable SQLStringList);

        public abstract object GetSingle(string SQLString, params dbParameter[] cmdParms);

        public abstract dbReader ExecuteReader(string SQLString, params dbParameter[] cmdParms);

        public abstract System.Data.DataSet Query(string SQLString, params dbParameter[] cmdParms);

        public abstract void PrepareCommand(dbCommond cmd, dbConnection conn, dbTransaction trans, string cmdText, dbParameter[] parameters);

        public string getPageListCounts(string _ID, string _tbName, string _strCondition, int _Dist)
        {
            //---存放取得查询结果总数的查询语句                        
            //---对含有DISTINCT的查询进行SQL构造    
            //---对含有DISTINCT的总数查询进行SQL构造    
            string strTmp = "", SqlSelect = "", SqlCounts = "";

            if (_Dist == 0)
            {
                SqlSelect = "Select ";
                SqlCounts = "COUNT(*)";
            }
            else
            {
                SqlSelect = "Select DISTINCT ";
                SqlCounts = "COUNT(DISTINCT " + _ID + ")";
            }
            if (_strCondition == string.Empty)
            {
                strTmp = SqlSelect + " " + SqlCounts + " FROM " + _tbName;
            }
            else
            {
                strTmp = SqlSelect + " " + SqlCounts + " FROM " + " Where (1=1) " + _strCondition;
            }
            return strTmp;    
        }

        public string getPageListSql(string primaryKey, string queryFields, string tableName, string condition, string orderBy, int pageSize, int pageIndex)
        {
            string strTmp = ""; //---strTmp用于返回的SQL语句    
            string SqlSelect = "", SqlPrimaryKeySelect = "", strOrderBy = "", strWhere = " where 1=1 ", strTop = "";
            //0：分页数量    
            //1:提取字段    
            //2:表    
            //3:条件    
            //4:主键不存在的记录    
            //5:排序    
            SqlSelect = " select top {0} {1} from {2} {3} {4} {5}";
            //0:主键    
            //1:TOP数量,为分页数*(排序号-1)    
            //2:表    
            //3:条件    
            //4:排序    
            SqlPrimaryKeySelect = " and {0} not in (select {1} {0} from {2} {3} {4}) ";
            if (orderBy != "")
                strOrderBy = " order by " + orderBy;
            if (condition != "")
                strWhere += " and " + condition;
            int pageindexsize = (pageIndex - 1) * pageSize;
            if (pageindexsize > 0)
            {
                strTop = " top " + pageindexsize.ToString();

                SqlPrimaryKeySelect = String.Format(SqlPrimaryKeySelect, primaryKey, strTop, tableName, strWhere, strOrderBy);

                strTmp = String.Format(SqlSelect, pageSize.ToString(), queryFields, tableName, strWhere, SqlPrimaryKeySelect, strOrderBy);

            }
            else
            {
                strTmp = String.Format(SqlSelect, pageSize.ToString(), queryFields, tableName, strWhere, "", strOrderBy);

            }
            return strTmp;    
        }

        public System.Data.DataSet GetPagingList(string primaryKey, string queryFields, string tableName, string condition, string orderBy, int pageSize, int pageIndex)
        {
            string sql = getPageListSql(primaryKey, queryFields, tableName, condition, orderBy, pageSize, pageIndex);

            return Query(sql);  
        }

        public string GetPagingListSQL(string primaryKey, string queryFields, string tableName, string condition, string orderBy, int pageSize, int pageIndex)
        {
            string sql = getPageListSql(primaryKey, queryFields, tableName, condition, orderBy, pageSize, pageIndex);

            return sql;    
        }

        public int GetRecordCount(string _ID, string _tbName, string _strCondition, int _Dist)
        {
            string sql = getPageListCounts(_ID, _tbName, _strCondition, _Dist);

            object obj = this.GetSingle(sql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }    
        }
    }
}
