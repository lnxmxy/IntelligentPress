using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Collections;
using System.Data;

namespace DB
{
    public class AccessDB : BaseDb<OleDbCommand, OleDbParameter, OleDbConnection, OleDbDataReader, OleDbTransaction>
    {
         //数据库连接字符串(web.config来配置)    
        //public  string connectionString = ConfigurationManager.AppSettings["ConnectionString"];    
        // public  string connectionString = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["AccessConnectionString"]);    
            
        public AccessDB()    
        {
            this.connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=RIFD.dll;Jet OLEDB:Database Password=zaq13edc";
        }
        public AccessDB(string connectionString) {
            this.connectionString = connectionString;
        }
  
        #region 公用方法    
  
       
        public override bool Exists(string strSql, params OleDbParameter[] cmdParms)    
        {    
            object obj = this.GetSingle(strSql, cmdParms);    
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
        #endregion  
  
        #region  执行简单SQL语句    
  
        /// <summary>    
        /// 执行SQL语句，返回影响的记录数    
        /// </summary>    
        /// <param name="SQLString">SQL语句</param>    
        /// <returns>影响的记录数</returns>    
        public override int ExecuteSql(string SQLString)    
        {    
            using (OleDbConnection connection = new OleDbConnection(connectionString))    
            {    
                using (OleDbCommand cmd = new OleDbCommand(SQLString, connection))    
                {    
                    try  
                    {    
                        connection.Open();    
                        int rows = cmd.ExecuteNonQuery();    
                        return rows;    
                    }    
                    catch (System.Data.OleDb.OleDbException E)    
                    {    
                        connection.Close();    
                        throw new Exception(E.Message);    
                    }    
                }    
            }    
        }    
  
        /// <summary>    
        /// 执行SQL语句，设置命令的执行等待时间    
        /// </summary>    
        /// <param name="SQLString"></param>    
        /// <param name="Times"></param>    
        /// <returns></returns>    
        public override int ExecuteSqlByTime(string SQLString, int Times)    
        {    
            using (OleDbConnection connection = new OleDbConnection(connectionString))    
            {    
                using (OleDbCommand cmd = new OleDbCommand(SQLString, connection))    
                {    
                    try  
                    {    
                        connection.Open();    
                        cmd.CommandTimeout = Times;    
                        int rows = cmd.ExecuteNonQuery();    
                        return rows;    
                    }    
                    catch (System.Data.OleDb.OleDbException E)    
                    {    
                        connection.Close();    
                        throw new Exception(E.Message);    
                    }    
                }    
            }    
        }    
  
        /// <summary>    
        /// 执行多条SQL语句，实现数据库事务。    
        /// </summary>    
        /// <param name="SQLStringList">多条SQL语句</param>        
        public override void ExecuteSqlTran(ArrayList SQLStringList)    
        {    
            using (OleDbConnection conn = new OleDbConnection(connectionString))    
            {    
                conn.Open();    
                OleDbCommand cmd = new OleDbCommand();    
                cmd.Connection = conn;    
                OleDbTransaction tx = conn.BeginTransaction();    
                cmd.Transaction = tx;    
                try  
                {    
                    for (int n = 0; n < SQLStringList.Count; n++)    
                    {    
                        string strsql = SQLStringList[n].ToString();    
                        if (strsql.Trim().Length > 1)    
                        {    
                            cmd.CommandText = strsql;    
                            cmd.ExecuteNonQuery();    
                        }    
                    }    
                    tx.Commit();    
                }    
                catch (System.Data.OleDb.OleDbException E)    
                {    
                    tx.Rollback();    
                    throw new Exception(E.Message);    
                }    
            }    
        }    
  
        /// <summary>    
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)    
        /// </summary>    
        /// <param name="strSQL">SQL语句</param>    
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>    
        /// <returns>影响的记录数</returns>    
        public override int ExecuteSqlInsertImg(string strSQL, byte[] fs)    
        {    
            using (OleDbConnection connection = new OleDbConnection(connectionString))    
            {    
                OleDbCommand cmd = new OleDbCommand(strSQL, connection);    
                System.Data.OleDb.OleDbParameter myParameter = new System.Data.OleDb.OleDbParameter("@fs", SqlDbType.Image);    
                myParameter.Value = fs;    
                cmd.Parameters.Add(myParameter);    
                try  
                {    
                    connection.Open();    
                    int rows = cmd.ExecuteNonQuery();    
                    return rows;    
                }    
                catch (System.Data.OleDb.OleDbException E)    
                {    
                    throw new Exception(E.Message);    
                }    
                finally  
                {    
                    cmd.Dispose();    
                    connection.Close();    
                }    
            }    
        }    
  
        /// <summary>    
        /// 执行一条计算查询结果语句，返回查询结果（object）。    
        /// </summary>    
        /// <param name="SQLString">计算查询结果语句</param>    
        /// <returns>查询结果（object）</returns>    
        public override object GetSingle(string SQLString)    
        {    
            using (OleDbConnection connection = new OleDbConnection(connectionString))    
            {    
                using (OleDbCommand cmd = new OleDbCommand(SQLString, connection))    
                {    
                    try  
                    {    
                        connection.Open();    
                        object obj = cmd.ExecuteScalar();    
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))    
                        {    
                            return null;    
                        }    
                        else  
                        {    
                            return obj;    
                        }    
                    }    
                    catch (System.Data.OleDb.OleDbException e)    
                    {    
                        connection.Close();    
                        throw new Exception(e.Message);    
                    }    
                }    
            }    
        }      
  
        /// <summary>    
        /// 执行查询语句，返回SqlDataReader(使用该方法切记要手工关闭SqlDataReader和连接)    
        /// </summary>    
        /// <param name="strSQL">查询语句</param>    
        /// <returns>SqlDataReader</returns>    
        public override OleDbDataReader ExecuteReader(string strSQL)    
        {    
            OleDbConnection connection = new OleDbConnection(connectionString);    
            OleDbCommand cmd = new OleDbCommand(strSQL, connection);    
            try  
            {    
                connection.Open();    
                OleDbDataReader myReader = cmd.ExecuteReader();    
                return myReader;    
            }    
            catch (System.Data.OleDb.OleDbException e)    
            {    
                throw new Exception(e.Message);    
            }    
            //finally //不能在此关闭，否则，返回的对象将无法使用    
            //{    
            //  cmd.Dispose();    
            //  connection.Close();    
            //}      
        }    
        /// <summary>    
        /// 执行查询语句，返回DataSet    
        /// </summary>    
        /// <param name="SQLString">查询语句</param>    
        /// <returns>DataSet</returns>    
        public override DataSet Query(string SQLString)    
        {    
            using (OleDbConnection connection = new OleDbConnection(connectionString))    
            {    
                DataSet ds = new DataSet();    
                try  
                {    
                    connection.Open();    
                    OleDbDataAdapter command = new OleDbDataAdapter(SQLString, connection);    
                    command.Fill(ds, "ds");    
                }    
                catch (System.Data.OleDb.OleDbException ex)    
                {    
                    throw new Exception(ex.Message);    
                }    
                return ds;    
            }    
        }    
        /// <summary>    
        /// 执行查询语句，返回DataSet,设置命令的执行等待时间    
        /// </summary>    
        /// <param name="SQLString"></param>    
        /// <param name="Times"></param>    
        /// <returns></returns>    
        public override  DataSet Query(string SQLString, int Times)    
        {    
            using (OleDbConnection connection = new OleDbConnection(connectionString))    
            {    
                DataSet ds = new DataSet();    
                try  
                {    
                    connection.Open();    
                    OleDbDataAdapter command = new OleDbDataAdapter(SQLString, connection);    
                    command.SelectCommand.CommandTimeout = Times;    
                    command.Fill(ds, "ds");    
                }    
                catch (System.Data.OleDb.OleDbException ex)    
                {    
                    throw new Exception(ex.Message);    
                }    
                return ds;    
            }    
        }  
  
        #endregion  
  
        #region 执行带参数的SQL语句    
  
        /// <summary>    
        /// 执行SQL语句，返回影响的记录数    
        /// </summary>    
        /// <param name="SQLString">SQL语句</param>    
        /// <returns>影响的记录数</returns>    
        public override int ExecuteSql(string SQLString, params OleDbParameter[] cmdParms)    
        {    
            using (OleDbConnection connection = new OleDbConnection(connectionString))    
            {    
                using (OleDbCommand cmd = new OleDbCommand())    
                {    
                    try  
                    {    
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);    
                        int rows = cmd.ExecuteNonQuery();    
                        cmd.Parameters.Clear();    
                        return rows;    
                    }    
                    catch (System.Data.OleDb.OleDbException E)    
                    {    
                        throw new Exception(E.Message);    
                    }    
                }    
            }    
        }      
  
        /// <summary>    
        /// 执行多条SQL语句，实现数据库事务。    
        /// </summary>    
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的OleDbParameter[]）</param>    
        public override void ExecuteSqlTran(Hashtable SQLStringList)    
        {    
            using (OleDbConnection conn = new OleDbConnection(connectionString))    
            {    
                conn.Open();    
                using (OleDbTransaction trans = conn.BeginTransaction())    
                {    
                    OleDbCommand cmd = new OleDbCommand();    
                    try  
                    {    
                        //循环    
                        foreach (DictionaryEntry myDE in SQLStringList)    
                        {    
                            string cmdText = myDE.Key.ToString();    
                            OleDbParameter[] cmdParms = (OleDbParameter[])myDE.Value;    
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);    
                            int val = cmd.ExecuteNonQuery();    
                            cmd.Parameters.Clear();    
  
                            trans.Commit();    
                        }    
                    }    
                    catch  
                    {    
                        trans.Rollback();    
                        throw;    
                    }    
                }    
            }    
        }      
  
        /// <summary>    
        /// 执行一条计算查询结果语句，返回查询结果（object）。    
        /// </summary>    
        /// <param name="SQLString">计算查询结果语句</param>    
        /// <returns>查询结果（object）</returns>    
        public override object GetSingle(string SQLString, params OleDbParameter[] cmdParms)    
        {    
            using (OleDbConnection connection = new OleDbConnection(connectionString))    
            {    
                using (OleDbCommand cmd = new OleDbCommand())    
                {    
                    try  
                    {    
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);    
                        object obj = cmd.ExecuteScalar();    
                        cmd.Parameters.Clear();    
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))    
                        {    
                            return null;    
                        }    
                        else  
                        {    
                            return obj;    
                        }    
                    }    
                    catch (System.Data.OleDb.OleDbException e)    
                    {    
                        throw new Exception(e.Message);    
                    }    
                }    
            }    
        }    
  
        /// <summary>    
        /// 执行查询语句，返回SqlDataReader (使用该方法切记要手工关闭SqlDataReader和连接)    
        /// </summary>    
        /// <param name="strSQL">查询语句</param>    
        /// <returns>SqlDataReader</returns>    
        public override  OleDbDataReader ExecuteReader(string SQLString, params OleDbParameter[] cmdParms)    
        {    
            OleDbConnection connection = new OleDbConnection(connectionString);    
            OleDbCommand cmd = new OleDbCommand();    
            try  
            {    
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);    
                OleDbDataReader myReader = cmd.ExecuteReader();    
                cmd.Parameters.Clear();    
                return myReader;    
            }    
            catch (System.Data.OleDb.OleDbException e)    
            {    
                throw new Exception(e.Message);    
            }    
            //finally //不能在此关闭，否则，返回的对象将无法使用    
            //{    
            //  cmd.Dispose();    
            //  connection.Close();    
            //}      
        }    
  
        /// <summary>    
        /// 执行查询语句，返回DataSet    
        /// </summary>    
        /// <param name="SQLString">查询语句</param>    
        /// <returns>DataSet</returns>    
        public override DataSet Query(string SQLString, params OleDbParameter[] cmdParms)    
        {    
            using (OleDbConnection connection = new OleDbConnection(connectionString))    
            {    
                OleDbCommand cmd = new OleDbCommand();    
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);    
                using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))    
                {    
                    DataSet ds = new DataSet();    
                    try  
                    {    
                        da.Fill(ds, "ds");    
                        cmd.Parameters.Clear();    
                    }    
                    catch (System.Data.OleDb.OleDbException ex)    
                    {    
                        throw new Exception(ex.Message);    
                    }    
                    return ds;    
                }    
            }    
        }

        public override void PrepareCommand(OleDbCommand cmd, OleDbConnection conn, OleDbTransaction trans, string cmdText, OleDbParameter[] cmdParms)    
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;    
            if (cmdParms != null)
            {


                foreach (OleDbParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }    
        }  
  
        #endregion  
       
    }
}
