using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using IDCM.ServiceBL.Common;
using System.Windows.Forms;
using IDCM.ServiceBL.DataTransfer;
using IDCM.ServiceBL.Handle;

namespace IDCM.ServiceBL.ServBuf
{
    /// <summary>
    /// 本地后台运行服务池，为后台执行线程
    /// </summary>
    class BGWorkerPool
    {
        /// <summary>
        /// 加载后台执行任务袋，并立即提交异步执行操作
        /// </summary>
        /// <param name="handler"></param>
        public static void pushHandler(AbsHandler handler, Object args = null, Queue<AbsHandler> cascadeHandlers = null)
        {
            if (handler == null)
                return;
            LocalHandlerProxy proxy = new LocalHandlerProxy(handler, cascadeHandlers);
            BackgroundWorker bgworker = new BackgroundWorker();
            bgworker.WorkerReportsProgress = true;
            bgworker.WorkerSupportsCancellation = true;
            //bgworker.DoWork += proxy.worker_DoWork;
            //bgworker.ProgressChanged += proxy.worker_ProgressChanged;
            //bgworker.RunWorkerCompleted += proxy.worker_RunWorkerCompleted;
            bgworker.DoWork += new DoWorkEventHandler(proxy.worker_DoWork);
            bgworker.ProgressChanged += new ProgressChangedEventHandler(proxy.worker_ProgressChanged);
            bgworker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(proxy.worker_RunWorkerCompleted);
            bgworker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(proxy.worker_cascadeProcess);
            lock (workerPool)
            {
                workerPool.AddLast(new KeyValuePair<BackgroundWorker, LocalHandlerProxy>(bgworker, proxy));
            }
            if (args == null)
                bgworker.RunWorkerAsync();
            else
                bgworker.RunWorkerAsync(args);
        }
        /// <summary>
        /// 移除特定的BackgroundWorker实例
        /// </summary>
        /// <param name="worker"></param>
        public static void removeWorker(BackgroundWorker worker)
        {
            if (worker == null)
                return;
            KeyValuePair<BackgroundWorker, LocalHandlerProxy> _kvpair = new KeyValuePair<BackgroundWorker, LocalHandlerProxy>();
            lock (workerPool)
            {
                foreach (KeyValuePair<BackgroundWorker, LocalHandlerProxy> kvpair in workerPool)
                {
                    if (kvpair.Key.Equals(worker))
                    {
                        _kvpair = kvpair;
                    }
                }
                workerPool.Remove(_kvpair);
            }
        }
        public static void abortHandlerByType(Type handlerType)
        {
            KeyValuePair<BackgroundWorker, LocalHandlerProxy> _kvpair=new KeyValuePair<BackgroundWorker,LocalHandlerProxy>();
            lock (workerPool)
            {
                foreach (KeyValuePair<BackgroundWorker, LocalHandlerProxy> kvpair in workerPool)
                {
                    if (kvpair.Value.getHandler().GetType().IsEquivalentTo(handlerType))
                    {
                        _kvpair = kvpair;
                    }
                }
                workerPool.Remove(_kvpair);
            }
            if (_kvpair.Key!=null && !_kvpair.Key.CancellationPending)
            {
                _kvpair.Key.CancelAsync();
                //_kvpair.Key.Dispose();
            }
        }

        /// <summary>
        /// 提供BackgroudWorker任务信息
        /// </summary>
        /// <returns></returns>
        public static Dictionary<String, KeyValuePair<string,string>> getStackInfo() {
            Dictionary<String, KeyValuePair<string, string>> dictionary = new Dictionary<String, KeyValuePair<string, string>>();
            foreach(KeyValuePair<BackgroundWorker, LocalHandlerProxy> kvp in workerPool){
                KeyValuePair<string, string> kvp0 = kvp.Value.getStackInfo();
                KeyValuePair<string, string> kvp1;
                if (kvp.Key.IsBusy)
                {
                    kvp1 = new KeyValuePair<string, string>("运行中", kvp0.Value);
                }
                else {
                    kvp1 = new KeyValuePair<string, string>("未运行", kvp0.Value);
                }
                
                dictionary.Add(kvp0.Key, kvp1);
            }
            return dictionary;
        } 
        /// <summary>
        /// 后台统一任务管控的后台任务执行缓存池
        /// </summary>
        private static LinkedList<KeyValuePair<BackgroundWorker, LocalHandlerProxy>> workerPool = new LinkedList<KeyValuePair<BackgroundWorker, LocalHandlerProxy>>();
    }
}
