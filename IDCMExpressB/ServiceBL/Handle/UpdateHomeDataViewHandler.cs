using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using IDCM.ViewLL.Manager;
using IDCM.ServiceBL.DataTransfer;
using System.Data;
using IDCM.SimpleDAL.POO;
using IDCM.SimpleDAL.DAM;
using IDCM.ControlMBL.AsyncInvoker;

namespace IDCM.ServiceBL.Handle
{
    /// <summary>
    /// 根据选中节点更新数据表记录显示
    /// </summary>
    class UpdateHomeDataViewHandler:AbsHandler
    {
        /// <summary>
        /// 指定查询的节点和目标数据表单，以及是否指定快速失效模式
        /// </summary>
        /// <param name="filterNode"></param>
        /// <param name="dgv"></param>
        /// <param name="failFast"></param>
        public UpdateHomeDataViewHandler(TreeNode filterNode,DataGridView dgv,bool failFast=true)
        {
            this.filterNode = filterNode;
            this.dgv = dgv;
            this.failFast = failFast;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res = false;
            if(!failFast || filterNode.Name.Equals(HomeViewManager.getInstance().CURRENT_LID.ToString()))
            {
                List<string> viewAttrs = ColumnMappingHolder.getViewAttrs();
                long lid = Convert.ToInt64(filterNode.Name);
                DGVAsyncUtil.syncRemoveAllRow(dgv);
                string filterLids = lid.ToString();
                if (lid > 0)
                {
                    long[] lids = LibraryNodeDAM.extractToLids(lid);
                    if (lids != null)
                    {
                        filterLids = "";
                        foreach (long _lid in lids)
                        {
                            filterLids += "," + _lid;
                        }
                        filterLids = filterLids.Substring(1);
                    }
                }
                //数据查询与装载
                DataTable records = CTDRecordDAM.queryCTDRecord(filterLids);
                if (records != null && records.Rows.Count > 0)
                {
                    foreach (DataRow dr in records.Rows)
                    {
                        loadCTableData(dr, viewAttrs);
                    }
                }
                res = true;
            }
            return new object[] { res};
        }
        /// <summary>
        /// 转换数据对象值到列表显示
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pCtd"></param>
        protected void loadCTableData(DataRow dr, List<string> viewAttrs)
        {
            string[] vals = new string[viewAttrs.Count];
            int index = 0;
            foreach (string attr in viewAttrs)
            {
#if DEBUG
                //Console.WriteLine("[DEBUG](loadCTableData) " + attr + "-->" + CustomTColMapDA.getDBOrder(attr) + ">>" + dr[CustomTColMapDA.getDBOrder(attr)].ToString());
#endif
                vals[index] = dr[ColumnMappingHolder.getDBOrder(attr)].ToString();
                ++index;
            }
            DGVAsyncUtil.syncAddRow(dgv, vals);
        }
        /// <summary>
        /// 后台任务执行结束，回调代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public void complete(BackgroundWorker worker, bool canceled, Exception error, List<Object> args)
        {
            if (canceled || (args.Count>0 && args[0].Equals(false)))
            {
                if(nextHandlers!=null)
                    nextHandlers.Clear();
                return;
            }
            if (error != null)
            {
                MessageBox.Show("ERROR::" + error.Message + "\n" + error.StackTrace);
                nextHandlers.Clear();
                return;
            }
        }
        /// <summary>
        /// 后台任务执行过程中的状态反馈代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="progressPercentage"></param>
        /// <param name="args"></param>
        public void progressChanged(BackgroundWorker worker, int progressPercentage, List<Object> args)
        {
        }
        /// <summary>
        /// 后台任务执行结束后的串联执行任务队列。
        /// 单任务情形，返回结果为空即可。
        /// </summary>
        /// <returns></returns>
        public Queue<AbsHandler> cascadeHandlers()
        {
            return nextHandlers;
        }
        public void addHandler(AbsHandler nextHandler)
        {
            if (nextHandler == null)
                return;
            if (nextHandlers == null)
                nextHandlers = new Queue<AbsHandler>();
            nextHandlers.Enqueue(nextHandler);
        }
        private TreeNode filterNode;
        private DataGridView dgv;
        private bool failFast = true;
        private Queue<AbsHandler> nextHandlers = null;
    }
}
