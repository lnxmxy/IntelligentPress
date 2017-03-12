using System;
using System.Data;
using System.Configuration;
using System.Data.OleDb;

namespace RepEXE
{
    public class AccessHelper
    {
        #region 变量
        protected static OleDbConnection conn = new OleDbConnection();
        protected static OleDbCommand comm = new OleDbCommand();

        //protected static string connectionString = @"Provider=Microsoft.Jet.OleDb.4.0;Data Source=Data\1.mdb";
        //protected static string connectionString ="";// @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=ahwildlife.mdb;Persist Security Info=False;Jet OLEDB:Database Password=sa;";
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public AccessHelper()
        {
             
        }
        #endregion


        private static string connectionString = string.Empty;  

        public static string ConnString  

        {

            get { return connectionString; }

            set { connectionString = value; }  

        }  





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

        #region 执行sql语句
        /// <summary>
        /// 执行sql语句
        /// </summary>
        public static void ExecuteSql(string sqlstr)
        {
            try
            {
                openConnection();
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                comm.ExecuteNonQuery();
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
        #endregion

        #region 返回指定sql语句的OleDbDataReader对象,使用时请注意关闭这个对象。
        /// <summary>
        /// 返回指定sql语句的OleDbDataReader对象,使用时请注意关闭这个对象。
        /// </summary>
        public static OleDbDataReader DataReader(string sqlstr)
        {
            OleDbDataReader dr = null;
            try
            {
                openConnection();
                comm.CommandText = sqlstr;
                comm.CommandType = CommandType.Text;

                dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                try
                {
                    dr.Close();
                    closeConnection();
                }
                catch { }
            }
            return dr;
        }
        #endregion

        #region 返回指定sql语句的OleDbDataReader对象,使用时请注意关闭
        /// <summary>
        /// 返回指定sql语句的OleDbDataReader对象,使用时请注意关闭
        /// </summary>
        public static void DataReader(string sqlstr, ref OleDbDataReader dr)
        {
            try
            {
                openConnection();
                comm.CommandText = sqlstr;
                comm.CommandType = CommandType.Text;
                dr = comm.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                try
                {
                    if (dr != null && !dr.IsClosed)
                        dr.Close();
                }
                catch
                {
                }
                finally
                {
                    closeConnection();
                }
            }
        }
        #endregion

        #region 返回指定sql语句的DataSet
        /// <summary>
        /// 返回指定sql语句的DataSet
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public static DataSet DataSet(string sqlstr)
        {
            DataSet ds = new DataSet();
            OleDbDataAdapter da = new OleDbDataAdapter();
            try
            {
                openConnection();
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
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
        #endregion

        #region 返回指定sql语句的DataSet
        /// <summary>
        /// 返回指定sql语句的DataSet
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="ds"></param>
        public static void DataSet(string sqlstr, ref DataSet ds)
        {
            OleDbDataAdapter da = new OleDbDataAdapter();
            try
            {
                openConnection();
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
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
        }
        #endregion

        #region 返回指定sql语句的DataTable
        /// <summary>
        /// 返回指定sql语句的DataTable
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public static DataTable DataTable(string sqlstr)
        {
            DataTable dt =   new DataTable();
            dt.TableName = "T";
                OleDbDataAdapter da = new OleDbDataAdapter();
                try
                {
                    using (OleDbConnection conn = new OleDbConnection())
                    {
                        conn.ConnectionString = connectionString;
                        conn.Open();
                        using (OleDbCommand comm = new OleDbCommand())
                        {
                            comm.Connection = conn;
                            comm.CommandType = CommandType.Text;
                            comm.CommandText = sqlstr;
                            da.SelectCommand = comm;
                            da.Fill(dt);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    closeConnection();
                }
                
                return dt;
            
        }
        #endregion

        #region 返回指定sql语句的DataTable
        /// <summary>
        /// 返回指定sql语句的DataTable
        /// </summary>
        public static void DataTable(string sqlstr, ref DataTable dt)
        {
            OleDbDataAdapter da = new OleDbDataAdapter();
            try
            {
                openConnection();
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                da.SelectCommand = comm;
                da.Fill(dt);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                closeConnection();
            }
        }
        #endregion

        #region 返回指定sql语句的DataView
        /// <summary>
        /// 返回指定sql语句的DataView
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public static DataView DataView(string sqlstr)
        {
            OleDbDataAdapter da = new OleDbDataAdapter();
            DataView dv = new DataView();
            DataSet ds = new DataSet();
            try
            {
                openConnection();
                comm.CommandType = CommandType.Text;
                comm.CommandText = sqlstr;
                da.SelectCommand = comm;
                da.Fill(ds);
                dv = ds.Tables[0].DefaultView;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                closeConnection();
            }
            return dv;
        }
        #endregion

    }
}
