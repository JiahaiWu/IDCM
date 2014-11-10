using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading;

namespace IDCM.SimpleDAL.DBCP
{
    /// <summary>
    /// 本类为SQLite数据库帮助静态类,使用时只需直接调用即可,无需实例化
    /// </summary>
    class SQLiteHelper
    {

        #region ExecuteNonQuery

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="commandTexts">执行语句或存储过程名</param>
        /// <returns>所受影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, params string[] commandTexts)
        {
            return ExecuteNonQuery(connectionString,CommandType.Text, commandTexts);
        }
        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="commandType">执行类型</param>
        /// <param name="commandTexts">执行语句或存储过程名</param>
        /// <returns>所受影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType = CommandType.Text, params string[] commandTexts)
        {
            int result = 0;
            if (commandTexts == null || commandTexts.Length == 0)
                throw new ArgumentNullException("commandText");
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnPicker picker = new SQLiteConnPicker(connectionString))
            {
                SQLiteConnection con = picker.getConnection();
                cmd.Connection = con;
                SQLiteTransaction trans = con.BeginTransaction(IsolationLevel.ReadCommitted);
                cmd.Transaction = trans;
                cmd.CommandType = commandType;
                try
                {
                    foreach (string commandText in commandTexts)
                    {
                        cmd.CommandText = commandText;
#if DEBUG
                        Console.WriteLine("Info: @CommandText=" + cmd.CommandText);
#endif
                        result = cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error:" + ex.Message + "\r\n[StackTrace]=" + ex.StackTrace);
                    if (trans != null)
                        trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>所受影响的行数</returns>
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, SQLiteParameter[] cmdParms,params string[] commandTexts)
        {
            int result = 0;
            if (commandTexts == null || commandTexts.Length == 0)
                throw new ArgumentNullException("commandText");

            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnPicker picker = new SQLiteConnPicker(connectionString))
            {
                SQLiteConnection conn = picker.getConnection();
                SQLiteTransaction trans = null;
                cmd.Connection = conn;
                cmd.CommandType = commandType;
                trans = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                cmd.Transaction = trans;
                if (cmdParms != null)
                {
                    foreach (SQLiteParameter parm in cmdParms)
                        cmd.Parameters.Add(parm);
                }
                try
                {
                    foreach (string commandText in commandTexts)
                    {
                        cmd.CommandText = commandText;
#if DEBUG
                        Console.WriteLine("Info: @CommandText=" + cmd.CommandText);
#endif
                        result = cmd.ExecuteNonQuery();
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error:" + ex.Message + "\r\n[StackTrace]=" + ex.StackTrace);
                    if (trans != null)
                        trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }
        /// <summary>
        /// 执行数据库操作(新增、更新或删除)
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="cmds">SqlCommand对象</param>
        /// <returns>所受影响的行数</returns>
        private static int ExecuteNonQuery(string connectionString, params SQLiteCommand[] cmds)
        {
            int result = 0;
            using (SQLiteConnPicker picker = new SQLiteConnPicker(connectionString))
            {
                SQLiteTransaction trans = null;
                try
                {
                    trans = picker.getConnection().BeginTransaction(IsolationLevel.ReadCommitted);
                    foreach (SQLiteCommand cmd in cmds)
                    {
                        cmd.Connection = picker.getConnection();
                        cmd.Transaction = trans;
#if DEBUG
                        Console.WriteLine("Info: @CommandText=" + cmd.CommandText);
#endif
                        result = cmd.ExecuteNonQuery();
                        trans.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error:" + ex.Message + "\r\n[StackTrace]=" + ex.StackTrace);
                    if (trans != null)
                        trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// 执行数据库操作(新增、更新或删除)同时返回执行后查询所得的第1行第1列数据
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <returns>查询所得的第1行第1列数据</returns>
        public static object ExecuteScalar(string connectionString, string commandText, CommandType commandType=CommandType.Text)
        {
            return ExecuteScalar(connectionString, commandText, CommandType.Text, null);
        }

        /// <summary>
        /// 执行数据库操作(新增、更新或删除)同时返回执行后查询所得的第1行第1列数据
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>查询所得的第1行第1列数据</returns>
        public static object ExecuteScalar(string connectionString, string commandText, CommandType commandType, params SQLiteParameter[] cmdParms)
        {
            object result = 0;
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            SQLiteCommand cmd = new SQLiteCommand();
            using (SQLiteConnPicker picker = new SQLiteConnPicker(connectionString))
            {
                SQLiteTransaction trans = null;
                try{
                    cmd.Connection = picker.getConnection();
                    cmd.CommandText = commandText;
                    cmd.CommandType = commandType;
                    trans = cmd.Connection.BeginTransaction(IsolationLevel.ReadCommitted);
                    cmd.Transaction = trans;
                    if (cmdParms != null)
                    {
                        foreach (SQLiteParameter parm in cmdParms)
                            cmd.Parameters.Add(parm);
                    }
    #if DEBUG
                    Console.WriteLine("Info: @CommandText=" + cmd.CommandText);
    #endif
                    result = cmd.ExecuteScalar();
                    trans.Commit();
                }catch (Exception ex)
                {
                    if (trans != null)
                        trans.Rollback();
                    throw ex;
                }
            }
            return result;
        }
        #endregion


        #region ExecuteDataTable
        /// <summary>
        /// 执行数据库查询，返回DataTable对象
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <returns>DataTable对象</returns>
        public static DataTable ExecuteDataTable(string connectionString, string commandText, CommandType commandType = CommandType.Text)
        {
            return ExecuteDataTable(connectionString, commandText, commandType,null);
        }

        /// <summary>
        /// 执行数据库查询，返回DataTable对象
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <param name="cmdParms">SQL参数对象</param>
        /// <returns>DataTable对象</returns>
        public static DataTable ExecuteDataTable(string connectionString, string commandText, CommandType commandType, params SQLiteParameter[] cmdParms)
        {
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            DataSet ds = new DataSet();
            using (SQLiteConnPicker picker = new SQLiteConnPicker(connectionString))
            {
                SQLiteConnection conn = picker.getConnection();
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = conn;
                cmd.CommandText = commandText;
                cmd.CommandType = commandType;
                if (cmdParms != null)
                {
                    foreach (SQLiteParameter parm in cmdParms)
                        cmd.Parameters.Add(parm);
                }
                try
                {
#if DEBUG
                    Console.WriteLine("Info: @CommandText=" + cmd.CommandText);
#endif
                    SQLiteDataAdapter sda = new SQLiteDataAdapter(cmd);
                    sda.Fill(ds);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }
        #endregion

        /// <summary>
        /// 通用分页查询方法
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <param name="tableName">表名</param>
        /// <param name="strColumns">查询字段名</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="strOrder">排序条件</param>
        /// <param name="pageSize">每页数据数量</param>
        /// <param name="currentIndex">当前页数</param>
        /// <param name="recordOut">数据总量</param>
        /// <returns>DataTable数据表</returns>
        public static DataTable SelectPaging(string connString, string tableName, string strColumns, string strWhere, string strOrder, int pageSize, int currentIndex, out int recordOut)
        {
            DataTable dt = new DataTable();
            recordOut = Convert.ToInt32(ExecuteScalar(connString, "select count(*) from " + tableName, CommandType.Text));
            string pagingTemplate = "select {0} from {1} where {2} order by {3} limit {4} offset {5} ";
            int offsetCount = (currentIndex - 1) * pageSize;
            string commandText = String.Format(pagingTemplate, strColumns, tableName, strWhere, strOrder, pageSize.ToString(), offsetCount.ToString());
            try{
                using (SQLiteConnPicker picker = new SQLiteConnPicker(connString))
                {
                    using (DbDataReader reader = executeReader(picker.getConnection(), commandText, CommandType.Text))
                    {
                        if (reader != null)
                        {
                            dt.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message + "\r\n[StackTrace]=" + ex.StackTrace);
                throw ex;
            }
            return dt;
        }
        /// <summary>
        /// 执行数据库查询，返回SqlDataReader对象
        /// </summary>
        /// <param name="SQLiteConnection">连接句柄</param>
        /// <param name="commandText">执行语句或存储过程名</param>
        /// <param name="commandType">执行类型</param>
        /// <returns>SqlDataReader对象</returns>
        private static DbDataReader executeReader(SQLiteConnection conn, string commandText, CommandType commandType)
        {
            DbDataReader reader = null;
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            SQLiteCommand cmd = new SQLiteCommand();
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = commandText;
            cmd.CommandType = commandType;
#if DEBUG
            Console.WriteLine("Info: @CommandText=" + cmd.CommandText);
#endif
            reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }
    }
}
