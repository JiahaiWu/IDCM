using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.SimpleDAL.DBCP;
using Dapper;
using IDCM.SimpleDAL.POO;

namespace IDCM.SimpleDAL.DAM
{
    class BaseInfoNoteDAM:DAMBase
    {
        /// <summary>
        /// 获取唯一序列生成ID值(数据库同步)
        /// </summary>
        /// <returns></returns>
        public static long nextSeqID()
        {
            lock (incrementLock)
            {
                ++autoIncrementNum;
                if (autoIncrementNum % 10 == 0)//如果是10的整数
                {
                    string cmd = "update BaseInfoNote set seqId=" + autoIncrementNum;//更新BaseInfoNote seqId
                    SQLiteHelper.ExecuteNonQuery(ConnectStr, cmd);
                }
            }
            return autoIncrementNum;
        }
        /// <summary>
        /// 创建基础序号自增长
        /// </summary>
        public static void loadBaseInfo()
        {
            using (SQLiteConnPicker picker = ConnectPicker())
            {
                List<long> seqIds = picker.getConnection().Query<long>("SELECT seqId FROM BaseInfoNote").ToList<long>();
                if (seqIds.Count == 0)
                {
                    DBVersionNote dbvn = new DBVersionNote();
                    string icmd = "insert into BaseInfoNote(seqId) values(" + dbvn.StartNo + ");";
                    picker.getConnection().Execute(icmd);
                    autoIncrementNum = dbvn.StartNo;
                }
                else
                {
                    autoIncrementNum = seqIds[0] + 10;
                    string cmd = "update BaseInfoNote set seqId=" + autoIncrementNum;
                    picker.getConnection().Execute(cmd);
                }
            }
        }

        /// <summary>
        /// 自增长对象锁
        /// </summary>
        private static Object incrementLock = new Object();
        /// <summary>
        /// 自动增长ID计数值
        /// </summary>
        private static long autoIncrementNum = 0;
    }
}
