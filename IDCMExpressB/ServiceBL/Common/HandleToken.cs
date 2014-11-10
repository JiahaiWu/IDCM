using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.OverallSC.ShareSync;

namespace IDCM.ServiceBL.Common
{
    class HandleToken
    {
        /// <summary>
        /// 获取唯一序列生成ID值
        /// </summary>
        /// <returns></returns>
        public static long nextTempID()
        {
            lock (ShareSyncLockers.processSerialIncrement_Lock)
            {
                ++processIncrementNum;
            }
            return processIncrementNum;
        }
        /// <summary>
        /// 自动增长Process ID计数值
        /// </summary>
        private volatile static int processIncrementNum = 1;
    }
}
