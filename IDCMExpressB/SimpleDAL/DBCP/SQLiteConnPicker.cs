using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Threading;
using System.Collections.Concurrent;
using IDCM.OverallSC.ShareSync;

namespace IDCM.SimpleDAL.DBCP
{
    /// <summary>
    /// 单点数据库连接访问的串行保护类,封装SQLiteConnection用于多线程串行共享。
    /// 内置全局多点数据库连接的连接池,对多数据库实例提供支持。
    /// @author JiahaiWu 2014-11-06
    /// </summary>
    class SQLiteConnPicker : IDisposable
    {
        /// <summary>
        /// 获取单点数据库连接的构造方法
        /// </summary>
        /// <param name="connStr"></param>
        public SQLiteConnPicker(string connStr)
        {
#if DEBUG
            if (connStr == null || connStr.Length == 0)
                throw new ArgumentNullException("connStr is NULL for SQLiteConnPicker(string)!");
#endif
            this.connectionStr = connStr;
            SQLiteConnHolder holder = null;
            connectPool.TryGetValue(connectionStr, out holder);
            if (holder == null)
            {
                holder = new SQLiteConnHolder(connectionStr);
                lock (ShareSyncLockers.SQLiteConnPicker_Lock)
                {
                    bool SQLiteConnAdded = connectPool.TryAdd(connectionStr, holder);
#if DEBUG
                    System.Diagnostics.Debug.Assert(SQLiteConnAdded);
#endif
                }
            }
            //尝试获取并打开数据库连接
            holder.tryOpen(connectionStr);
        }
        /// <summary>
        /// 销毁连接资源
        /// </summary>
        public void Dispose()
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(connectionStr != null && connectionStr.Length > 0);
#endif
            SQLiteConnHolder holder = null;
            connectPool.TryGetValue(connectionStr, out holder);
            if (holder != null)
            {
                holder.release();
            }
        }
        /// <summary>
        /// 解开对象封装获得SQLite连接对象。
        /// 请注意该方法仅用于一次性SQL事务处理流程，且不需要外部的连接释放管理操作。
        /// @author JiahaiWu 2014-11-06
        /// @Note 请注意安全使用本方法获取的连接实例，更进一步的全封装实现尚未实现。
        /// </summary>
        /// <returns></returns>
        public SQLiteConnection getConnection()
        {
            SQLiteConnHolder holder = null;
            connectPool.TryGetValue(connectionStr, out holder);
            return holder!=null?holder.Sconn:null;
        }
        /// <summary>
        /// 开启数据库连接
        /// </summary>
        /// <returns></returns>
        public bool open()
        {
            SQLiteConnHolder holder = null;
            connectPool.TryGetValue(connectionStr, out holder);
            if (holder != null)
            {
                return holder.tryOpen(connectionStr);
            }
            return false;
        }
        /// <summary>
        /// 销毁连接资源
        /// </summary>
        internal static void closeAll()
        {
            lock (ShareSyncLockers.SQLiteConnPicker_Lock)
            {
                foreach (SQLiteConnHolder holder in connectPool.Values)
                {
                    holder.kill();
                }
                connectPool.Clear();
            }
        }
        /// <summary>
        /// 数据库连接串
        /// </summary>
        private volatile string connectionStr = null;
        #region 静态持有域
        /// <summary>
        /// 多点数据库连接的连接池缓存对象
        /// </summary>
        private static volatile ConcurrentDictionary<string, SQLiteConnHolder> connectPool = new ConcurrentDictionary<string, SQLiteConnHolder>();
        /// <summary>
        /// 最长等待毫秒数（默认为5000ms）
        /// </summary>
        protected static int MAX_WAIT_TIME_OUT = 5000;
        #endregion
        /// <summary>
        /// Inner Class for Connection Holding Obeject Definition
        /// 单点数据库连接访问保持句柄类
        /// @author JiahaiWu 2014-11-06
        /// </summary>
        protected class SQLiteConnHolder
        {
            public SQLiteConnHolder(string connString)
            {
                sconn = new SQLiteConnection();
                sconn.ConnectionString = connString;
                semaphore = new Semaphore(1, 1);
            }
            /// <summary>
            /// 数据库连接句柄
            /// </summary>
            private SQLiteConnection sconn = null;

            public SQLiteConnection Sconn
            {
                get { return sconn; }
            }
            /// <summary>
            /// 同步信号量
            /// </summary>
            private Semaphore semaphore;
            /// <summary>
            /// 尝试打开单点数据库连接，
            /// 借助于信号量机制实现串行获取连接过程。
            /// </summary>
            /// <param name="connectionStr"></param>
            /// <returns></returns>
            internal bool tryOpen(string connectionStr=null)
            {
                if (semaphore.WaitOne(MAX_WAIT_TIME_OUT, true))
                {
                    try{
                        if (sconn != null)
                        {
                            if (!sconn.State.Equals(ConnectionState.Open)&&!sconn.State.Equals(ConnectionState.Closed))
                            {
                                sconn.Close();
                            }
                        }
                        else
                        {
                            sconn = new SQLiteConnection();
                            sconn.ConnectionString = connectionStr;
                        }
                        if(!sconn.State.Equals(ConnectionState.Open))              
                            sconn.Open();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new SQLiteException(ex.Message, ex);
                    }
                }
                return false;
            }
            /// <summary>
            /// 销毁连接资源
            /// </summary>
            internal void release()
            {
                if (sconn != null)
                {
                    if (!sconn.State.Equals(ConnectionState.Closed))
                        sconn.Close();
                }
                //释放信号量控制
                semaphore.Release();
            }
            /// <summary>
            /// 彻底销毁连接资源
            /// </summary>
            internal void kill()
            {
                if (sconn != null && sconn.State != ConnectionState.Closed)
                {
                    sconn.Close();
                }
                sconn.Dispose();
                sconn = null;
                semaphore.Close();
                semaphore.Dispose();
            }
        }
    }
}
