using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows.Forms;
using IDCM.ViewLL.Manager;
using IDCM.ViewLL.Win;

namespace IDCM.AppContext
{
    /// <summary>
    /// The class that handles the creation of the application windows
    /// 本类作为IDCM的窗口任务、后台任务、线程任务的创建管理类。
    /// 本实例只允许一次有效实例化，启动初始化窗口线程和心跳检测任务。
    /// 心跳检测任务会定期地判断当前进程是否活动任务为空，准备退出。
    /// @author JiahaiWu 2014-10
    /// 基础初始化操作包含三个部分：
    /// 1.工作空间独享验证checkWorkSpaceSingleton()
    /// 2.基本数据库初始化验证 startForm()
    /// 3.载入IDCM主窗口对象实例
    /// </summary>
    class IDCMAppContext : ApplicationContext
    {
        public IDCMAppContext(string workspacePath=null)
        {
            if (!hasInited)
            {
                hasInited = true;
                // Handle the ApplicationExit event to know when the application is exiting.
                Application.ApplicationExit += new EventHandler(this.OnApplicationExit);
                //初始化检测...
                //检查当前目录下的进程实例是否已存在，如果存在执行退出操作;
                //检查目标工作空间的文档是否已占用，如果被占用则执行退出操作;
                WorkSpaceHolder.checkWorkSpaceSingleton(workspacePath);

                // Create both application forms and handle the Closed event
                // to know when both forms are closed.
                Form startForm = new StartForm(workspacePath);
                mainManger = new IDCMVeiwManger();
                DialogResult res = startForm.ShowDialog();
                if (res == DialogResult.OK)
                {
                    startForm.Dispose();
                    mainManger.initForm(true);
                }
                else
                {
                    if (res != DialogResult.Cancel)
                    {
                        MessageBox.Show("Failed to load and open the Workspace!", "Error", MessageBoxButtons.OK);
                    }
                    startForm.Dispose();
                    mainManger.Dispose();
                }
                LongTermHandleNoter.checkForIdle();
                //Run HandleInstanceMonitor
                monitor.Interval = 3000;
                monitor.Tick += OnHeartBreak;
                monitor.Start();
            }
        }
        private void OnApplicationExit(object sender, EventArgs e)
        {

        }

        private void OnFormClosing(object sender, CancelEventArgs e)
        {

        }

        private void OnFormClosed(object sender, EventArgs e)
        {
            // When a form is closed, decrement the count of open forms.

            // When the count gets to 0, exit the app by calling
            // ExitThread().
            if (LongTermHandleNoter.checkForIdle())
            {
                ExitThread();
                this.Dispose();
            }
        }
        /// <summary>
        /// 心跳检测
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHeartBreak(object sender, EventArgs e)
        {
#if DEBUG
            //Console.WriteLine("* Heart Break For checkForIdle()");
#endif
            if (LongTermHandleNoter.checkForIdle())
            {
                monitor.Stop();
                ExitThread();
                this.Dispose();
            }
        }
        /// <summary>
        /// HandleInstanceMonitors
        /// </summary>
        private static System.Windows.Forms.Timer monitor = new System.Windows.Forms.Timer();
        private static volatile bool hasInited = false;
        private static IDCMVeiwManger mainManger = null;

        public static IDCMVeiwManger MainManger
        {
            get
            {
                if (hasInited)
                    return IDCMAppContext.mainManger;
                else
                    return null;
            }
        }
    }
}
