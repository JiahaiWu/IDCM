using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using IDCM.SimpleDAL.POO;
using IDCM.ServiceBL.ServProcessor;
using System.Windows.Forms;
using IDCM.ViewLL.Manager;
using IDCM.AppContext;
using IDCM.ControlMBL.Module;

namespace IDCM.ServiceBL.Handle
{
    class UpdateTemplateHandler:AbsHandler
    {
        public UpdateTemplateHandler(LinkedList<CustomTColDef> customTCDList)
        {
            this.customTCDList = customTCDList;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res = false;
            TemplateUpdater updater = new TemplateUpdater();
            res=updater.doUpdateProcess(customTCDList);
            return new object[] { res};
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
            if (IDCMAppContext.MainManger != null)
            {
                IDCMAppContext.MainManger.getManager(typeof(HomeViewManager)).dispose();
                IDCMAppContext.MainManger.activeChildView(typeof(HomeViewManager), true);
            }
            else
                MessageBox.Show("ERROR::IDCMAppContext.MainManger is NULL.\n");
        }
        /// <summary>
        /// 后台任务执行过程中的状态反馈代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="progressPercentage"></param>
        /// <param name="args"></param>
        public void progressChanged(BackgroundWorker worker, int progressPercentage, List<Object> args)
        {
            if (progressPercentage == 0)
            {
                FrontProgressPrompt.startFrontProgress();
            }
            if (progressPercentage == 100)
            {
                FrontProgressPrompt.endFrontProgress();
            }
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
        private LinkedList<CustomTColDef> customTCDList = null;
        private Queue<AbsHandler> nextHandlers = null;
    }
}
