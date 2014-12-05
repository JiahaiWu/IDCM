using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;
using IDCM.OverallSC.ShareSync;


namespace IDCM.AppContext
{
    /// <summary>
    /// 内部线程实例化句柄统一监管器，用于明确进程内线程资源的跟踪记录与安全退出验证服务
    /// @author JiahaiWu 
    /// </summary>
    class LongTermHandleNoter
    {
        /// <summary>
        /// 获取特定类型的Form实例化对象
        /// </summary>
        /// <param name="formType"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Form getForm(Type formType)
        {
            int count = handleList.Count;
            int idx = 0;
            while (idx < count)
            {
                Object handle = handleList.ElementAt(idx);
                if (handle is Form && !(handle as Form).IsDisposed && handle.GetType().Equals(formType))
                {
                    return handle as Form;
                }
                ++idx;
            }
            return null;
        }
        /// <summary>
        /// 获取线程句柄对象
        /// </summary>
        /// <param name="start"></param>
        /// <param name="tname"></param>
        /// <param name="maxStackSize"></param>
        /// <returns></returns>
        public static Thread getThread(string threadName = null)
        {
            int count = handleList.Count;
            int idx = 0;
            while (idx < count)
            {
                Object handle = handleList.ElementAt(idx);
                if (handle is Thread && (handle as Thread).IsAlive && (handle as Thread).Name.Equals(threadName))
                {
                    return handle as Thread;
                }
                ++idx;
            }
            return null;
        }

        /// <summary>
        /// 记录实例化对象
        /// </summary>
        /// <param name="formType"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static Form note(Form formView)
        {
            lock (ShareSyncLockers.BackendHandleMonitor_Lock)
            {
                handleList.AddLast(formView);
            }

            formView.Disposed += OnHandleDisposed;

            if (formView is IDCM.ViewLL.Win.StackInfoView)
            {
                Console.Write("");
            }
            
            return formView;
        }

        /// <summary>
        /// 记录带参线程句柄对象
        /// </summary>
        /// <param name="start"></param>
        /// <param name="tname"></param>
        /// <param name="maxStackSize"></param>
        /// <returns></returns>
        public static Thread note(Thread thread)
        {
            if (thread != null && thread.IsAlive)
            {
                lock (ShareSyncLockers.BackendHandleMonitor_Lock)
                {
                    handleList.AddLast(thread);
                }
            }
            return thread;
        }
        /// <summary>
        /// 记录异步线程句柄对象
        /// </summary>
        /// <returns></returns>
        public static BackgroundWorker note(BackgroundWorker worker)
        {
            if (worker != null)
            {
                lock (ShareSyncLockers.BackendHandleMonitor_Lock)
                {
                    handleList.AddLast(worker);
                    
                }
                worker.Disposed += OnHandleDisposed;
            }
            return worker;
        }
        /// <summary>
        /// 句柄对象释放时事件处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OnHandleDisposed(object sender, EventArgs e)
        {
            lock (ShareSyncLockers.BackendHandleMonitor_Lock)
            {
                handleList.Remove(sender);
            }
        }
        /// <summary>
        /// 查询句柄记录集，验证当前空闲状态
        /// </summary>
        /// <returns></returns>
        public static bool checkForIdle()
        {
            lock (ShareSyncLockers.BackendHandleMonitor_Lock)
            {
                int count = handleList.Count;
                int idx = 0;
                while (idx < count)
                {
                    Object handle = handleList.ElementAt(idx);
                    if (handle == null)
                        handleList.Remove(idx);
                    else
                    {
                        if (handle is Form && (handle as Form).IsDisposed)
                        {
                            handleList.Remove(idx);
                        }
                        else if (handle is BackgroundWorker && !(handle as BackgroundWorker).IsBusy)
                        {
                            handleList.Remove(idx);
                        }
                        else if (handle is Thread && !(handle as Thread).IsAlive)
                        {
                            handleList.Remove(idx);
                        }
                        else
                            break;
                    }
                    ++idx;
                }
            }
            return handleList.Count < 1;
        }

        /// <summary>
        /// 获取后台任务明细
        /// </summary>
        /// <returns></returns>
        public static Dictionary<String, String> getStackDetails()
        {
            Dictionary<String, String> dictionary = new Dictionary<String, String>();
            foreach (Object obj in handleList)
            {
                //if (obj is Form)
                //{
                //    Form tempForm = (obj as Form);
                //    String key = tempForm.Name;
                //    String status = "未运行";
                //    if (!tempForm.IsDisposed)
                //    {
                //        status = "运行中";
                //    }
                //    dictionary.Add(key, status);
                //}
                //else 
                if (obj is Thread)
                {
                    Thread thread = (obj as Thread);
                    String key = thread.Name;
                    String status = "未运行";
                    if (thread.IsAlive)
                    {
                        status = "运行中";
                    }
                    dictionary.Add(key, status);
                }
            }
            return dictionary;
        }
        /// <summary>
        /// 后台线程对象缓冲池
        /// @author JiahaiWu 2014-11-07
        /// </summary>
        private static LinkedList<Object> handleList = new LinkedList<object>();
    }
}
