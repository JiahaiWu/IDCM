using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using CSharpTest.Net.Collections;

namespace IDCM.SimpleDAL.DBCP
{
    /// <summary>
    /// 特征查询条件缓存类
    /// @author JiahaiWu
    /// </summary>
    class QueryCmdCache
    {
        /// <summary>
        /// 数据表单查询条件语句缓存
        /// </summary>
        /// <param name="cmdstr"></param>
        internal static void cacheCTDRQuery(string cmdstr,int tcount)
        {
            lastUserCTDRQuery = cmdstr;
            lastUserCTDRQueryCount=tcount;
        }
        /// <summary>
        /// 最近的用户发起的数据表单查询条件语句
        /// </summary>
        /// <param name="cmdstr"></param>
        /// <returns></returns>
        public static KeyValuePair<string,int> getLastCDTRQuery(string cmdstr)
        {
            return new KeyValuePair<string, int>(lastUserCTDRQuery, lastUserCTDRQueryCount);
        }
        /// <summary>
        /// 聚合查询数值结果缓存
        /// </summary>
        /// <param name="cmdstr"></param>
        /// <param name="values"></param>
        internal static void cacheAggregateQuery(string cmdstr, params long[] values)
        {
            aggregateQueryStack.AddOrUpdate(cmdstr, values, (key, oldValue) => values);
        }
        /// <summary>
        /// 获取聚合查询数值结果
        /// </summary>
        /// <param name="cmdstr"></param>
        /// <returns></returns>
        public static long[] getAggregateValues(string cmdstr)
        {
            long[] vals=null;
            aggregateQueryStack.TryGetValue(cmdstr, out vals);
            return vals;
        }
        /// <summary>
        /// 最近的用户发起的数据表单查询条件语句
        /// </summary>
        public static string lastUserCTDRQuery = null;
        public static int lastUserCTDRQueryCount = 0;
        /// <summary>
        /// 最近的聚合查询数值结果缓存池，目前仅支持先进先出缓存原则。
        /// </summary>
        public static LurchTable<string, long[]> aggregateQueryStack = new LurchTable<string, long[]>(LurchTableOrder.Insertion,MaxAggregateQueryStackSize);
        /// <summary>
        /// 聚合查询数值结果缓存池大小限定
        /// </summary>
        public static int MaxAggregateQueryStackSize = 256;
    }
}
