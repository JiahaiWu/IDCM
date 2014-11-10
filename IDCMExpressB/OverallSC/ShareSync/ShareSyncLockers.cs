using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.OverallSC.ShareSync
{
    /// <summary>
    /// 多线程共享锁预定义类
    /// </summary>
    class ShareSyncLockers
    {
        /// <summary>
        /// 进程工作目录空间独占保持的共享锁对象
        /// </summary>
        public static object WorkSpaceHolder_Lock = new object();
        /// <summary>
        /// 用于保持串行获取数据库连接的共享锁对象
        /// </summary>
        public static object SQLiteConnPicker_Lock=new object();
        /// <summary>
        /// 后台线程任务监控器独占保持的共享锁对象
        /// </summary>
        public static object BackendHandleMonitor_Lock = new object();
        /// <summary>
        /// 本地表单数据视图控件的独占保持的共享锁对象
        /// </summary>
        public static object LocalDataGridView_Lock = new object();
        /// <summary>
        /// 用于内存处理过程中所需唯一标识的共享锁对象
        /// </summary>
        public static object processSerialIncrement_Lock = new object();
    }
}
