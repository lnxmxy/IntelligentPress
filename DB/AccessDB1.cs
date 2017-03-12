using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace DB
{
    public class AccessDB1 : BaseDb<OleDbCommand, OleDbParameter, OleDbConnection, OleDbDataReader, OleDbTransaction>
    {
        #region 变量
        protected static OleDbConnection conn = new OleDbConnection();
        protected static OleDbCommand comm = new OleDbCommand();

        protected static string connectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=RIFD.dll;Jet OLEDB:Database Password=zaq13edc";
        //protected static string connectionString ="";// @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=ahwildlife.mdb;Persist Security Info=False;Jet OLEDB:Database Password=sa;";
        #endregion
        #region 关闭数据库
        /// <summary>
        /// 关闭数据库
        /// </summary>
        private static void closeConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                conn.Dispose();
                comm.Dispose();
            }
        }
        #endregion
        #region 打开数据库
        /// <summary>
        /// 打开数据库
        /// </summary>
        private static void openConnection()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = connectionString;
                comm.Connection = conn;
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion
        public override bool Exists(string strSql, params OleDbParameter[] cmdParms)
        {
            throw new NotImplementedException();
        }

        public override int ExecuteSql(string SQLString)
        {
            try
            {
                openConnection();
                PrepareCommand(comm, conn, null, SQLString, null);  
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                closeConnection();
            }
        }

        public override int ExecuteSqlByTime(string SQLString, int Times)
        {
            throw new NotImplementedException();
        }

        public override void ExecuteSqlTran(System.Collections.ArrayList SQLStringList)
        {
            throw new NotImplementedException();
        }

        public override int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            throw new NotImplementedException();
        }

        public override object GetSingle(string SQLString)
        {
            throw new NotImplementedException();
        }

        public override OleDbDataReader ExecuteReader(string strSQL)
        {
            throw new NotImplementedException();
        }

        public override System.Data.DataSet Query(string SQLString)
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter();
            try
            {
                openConnection();
                PrepareCommand(comm, conn, null, SQLString, null);  
                da.SelectCommand = comm;
                da.Fill(ds);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                closeConnection();
            }
            return ds;
        }

        public override System.Data.DataSet Query(string SQLString, int Times)
        {
            throw new NotImplementedException();
        }

        public override int ExecuteSql(string SQLString, params OleDbParameter[] cmdParms)
        {
            try
            {
                openConnection();
                PrepareCommand(comm, conn, null, SQLString, cmdParms);   
                return comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                closeConnection();
            }
        }

        public override void ExecuteSqlTran(System.Collections.Hashtable SQLStringList)
        {
            throw new NotImplementedException();
        }

        public override object GetSingle(string SQLString, params OleDbParameter[] cmdParms)
        {
            throw new NotImplementedException();
        }

        public override OleDbDataReader ExecuteReader(string SQLString, params OleDbParameter[] cmdParms)
        {
            throw new NotImplementedException();
        }

        public override System.Data.DataSet Query(string SQLString, params OleDbParameter[] cmdParms)
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter();
            try
            {
                openConnection();
                PrepareCommand(comm, conn, null, SQLString, cmdParms);    
             
                da.SelectCommand = comm;
                da.Fill(ds);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                closeConnection();
            }
            return ds;
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
  
    }
}
