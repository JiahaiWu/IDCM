using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.ServiceBL.Common;
using System.Reflection;
using IDCM.SimpleDAL.DBCP;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Data;
using IDCM.SimpleDAL.DAM;
using IDCM.SimpleDAL.POO;

namespace IDCM.ServiceBL.DataTransfer
{
    class ColumnMappingHolder
    {
        public static ColumnMapping getBaseMapping()
        {
            return attrMapping.Clone() as ColumnMapping;
        }
        /// <summary>
        /// 存储表属性映射位序条件语句
        /// </summary>
        public static void noteDefaultColMap(List<string> noteCmds)
        {
            try
            {
                SQLiteHelper.ExecuteNonQuery(DAMBase.ConnectStr, CommandType.Text, noteCmds.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:\r\n" + ex.Message + "\n" + ex.StackTrace);
                Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            getCacheAttrDBMap();
        }
        public static void noteDefaultColMap(string attr,int dbOrder,int viewOrder)
        {
            CustomTColMapDAM.noteDefaultColMap(attr, dbOrder, viewOrder);
            getCacheAttrDBMap();
        }
        public static void clearColMap()
        {
            CustomTColMapDAM.clearColMap();
            attrMapping.Clear();
        }
        /// <summary>
        /// 缓存数据字段映射关联关系
        /// </summary>
        public static void getCacheAttrDBMap()
        {
            List<CustomTColMap> ctcms = CustomTColMapDAM.findAllByOrder();
            lock (attrMapping)
            {
                attrMapping.Clear();
                foreach (CustomTColMap dr in ctcms)
                {
                    attrMapping.Add(dr.Attr, new ObjectPair<int, int>(dr.MapOrder, dr.ViewOrder));
                }
            }
            ////Debug///
        }
        /// <summary>
        /// 获取存储字段序列值(如查找失败返回-1)
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static int getDBOrder(string attr)
        {
            if (attrMapping.Count < 1)
                getCacheAttrDBMap();
            ObjectPair<int, int> kvpair = null;
            attrMapping.TryGetValue(attr, out kvpair);
            return kvpair==null?-1:kvpair.Key;
        }

        /// <summary>
        /// 获取预览字段集序列
        /// </summary>
        /// <returns></returns>
        public static List<string> getViewAttrs()
        {
            if (attrMapping.Count < 1)
                getCacheAttrDBMap();
            return attrMapping.Keys.ToList<string>();
        }
        /// <summary>
        /// 获取预览字段位序值(如查找失败返回-1)
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static int getViewOrder(string attr)
        {
            if (attrMapping.Count < 1)
                getCacheAttrDBMap();
            ObjectPair<int, int> kvpair = null;
            attrMapping.TryGetValue(attr, out kvpair);
            return kvpair==null?-1:kvpair.Val;
        }
        /// <summary>
        /// 更新预览字段位序值
        /// </summary>
        /// <param name="attr"></param>
        /// <param name="viewOrder"></param>
        public static void updateViewOrder(string attr, int viewOrder,bool isRequired)
        {
            int vOrder = viewOrder;
            if (isRequired == false && viewOrder < MaxMainViewCount)
                vOrder = viewOrder + MaxMainViewCount;
            else if(viewOrder > MaxMainViewCount)
                vOrder = viewOrder - MaxMainViewCount;
            updateViewOrder(attr, vOrder);
        }
        public static void updateViewOrder(string attr, int viewOrder)
        {
            int ic=CustomTColMapDAM.updateViewOrder(attr, viewOrder);
            if (ic > 0)
                attrMapping[attr] = new ObjectPair<int, int>(attrMapping[attr].Key, viewOrder);
        }
        /// <summary>
        /// 数据字段名与[数据存储，预览界面]的映射关系
        /// </summary>
        protected static ColumnMapping attrMapping = new ColumnMapping();
        /// <summary>
        /// 主表域最大显示字段数
        /// </summary>
        public const int MaxMainViewCount = 1000;
    }
}
