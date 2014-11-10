using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using IDCM.SimpleDAL.POO;
using IDCM.SimpleDAL.DAM;

namespace IDCM.ServiceBL.Handle
{
    /// <summary>
    /// 节点数量更新事件处理
    /// </summary>
    class UpdateHomeLibCountHandler:AbsHandler
    {
        public UpdateHomeLibCountHandler(TreeView libTree,TreeView baseTree)
        {
            this.libTree = libTree;
            this.baseTree = baseTree;
        }
        public UpdateHomeLibCountHandler(TreeNode focusNode)
        {
            this.focusNode = focusNode;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res = false;
            if (!processing)
            {
                processing = true;
                Dictionary<int, long> counts = CTDRecordDAM.countCTDRecord(null);
                if (libTree != null)
                {
                    foreach (TreeNode pnode in libTree.Nodes)
                    {
                        long c_pnode = 0;
                        if (pnode.Nodes != null)
                        {
                            foreach (TreeNode node in pnode.Nodes)
                            {
                                long lc = 0;
                                counts.TryGetValue(Convert.ToInt32(node.Name), out lc);
                                node.Tag = lc;
                                c_pnode += lc;
                            }
                        }
                        pnode.Tag = c_pnode;
                    }
                }
                if (baseTree != null)
                {
                    foreach (TreeNode node in baseTree.Nodes)
                    {
                        long lc = 0;
                        counts.TryGetValue(Convert.ToInt32(node.Name), out lc);
                        node.Tag = lc;
                    }
                }
                if (focusNode != null)
                {
                    Dictionary<int, long> fcounts = CTDRecordDAM.countCTDRecord(focusNode.Name);
                    long lc = 0;
                    fcounts.TryGetValue(Convert.ToInt32(focusNode.Name), out lc);
                    focusNode.Tag = lc;
                }
                res = true;
                processing = false;
            }
            return new object[] { res };
        }
        /// <summary>
        /// 后台任务执行结束，回调代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public void complete(BackgroundWorker worker, bool canceled, Exception error, List<Object> args)
        {
            if (canceled)
                return;
            if (error != null)
            {
                MessageBox.Show("ERROR::" + error.Message + "\n" + error.StackTrace);
                return;
            }
            if (baseTree != null)
            {
                baseTree.ExpandAll();
                baseTree.Refresh();
            }
            if (libTree != null)
            {
                libTree.ExpandAll();
                libTree.Refresh();
            }
            if (focusNode != null)
            {
                focusNode.TreeView.Refresh();
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
            return null;
        }
        private TreeView libTree=null;
        private TreeView baseTree=null;
        private TreeNode focusNode = null;
        private volatile bool processing=false;
    }
}
