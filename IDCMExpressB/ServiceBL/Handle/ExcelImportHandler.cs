using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using IDCM.ServiceBL.Common;
using IDCM.ViewLL.Manager;
using IDCM.ControlMBL.Module;
using IDCM.ServiceBL.DataTransfer;
using IDCM.ServiceBL.Handle;
using IDCM.ServiceBL.CmdChannel;

namespace IDCM.ServiceBL.Handle
{
    class ExcelImportHandler:AbsHandler
    {
        public ExcelImportHandler(string fpath,DataGridView dgv,TabControl tc)
        {
            this.xlsPath = System.IO.Path.GetFullPath(fpath);
            this.dgv_data = dgv;
            this.tc = tc;
        }
        /// <summary>
        /// 后台任务执行方法的主体部分，异步执行代码段！
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="args"></param>
        public Object doWork(BackgroundWorker worker, bool cancel, List<Object> args)
        {
            bool res=false;
            string referPath=null;
            ExcelDataImporter edImporter = new ExcelDataImporter(dgv_data);
            referPath = xlsPath;
            res = edImporter.parseExcelData(xlsPath);
            return new object[] { res,referPath};
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
        }
        /// <summary>
        /// 后台任务执行过程中的状态反馈代码段
        /// </summary>
        /// <param name="worker"></param>
        /// <param name="progressPercentage"></param>
        /// <param name="args"></param>
        public void progressChanged(BackgroundWorker worker, int progressPercentage, List<Object> args)
        {
            //if (progressPercentage > 0)
            //    reportProgress(worker, progressPercentage);
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
        private Queue<AbsHandler> nextHandlers = null;
        private string xlsPath = null;
        private DataGridView dgv_data = null;
        private TabControl tc = null;
    }
}
