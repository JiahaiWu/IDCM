using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.OverallSC.Commons;
using IDCM.ViewLL.Manager;
using System.Threading;
using System.Configuration;
using IDCM.ServiceBL;
using IDCM.ServiceBL.Common;
using IDCM.AppContext;
using IDCM.ControlMBL.AsyncInvoker;

namespace IDCM.ViewLL.Win
{
    public partial class IDCMForm : Form
    {
        private IDCMVeiwManger manager = null;
        internal void setManager(IDCMVeiwManger manager)
        {
            this.manager = manager;
        }

        public IDCMForm()
        {
            InitializeComponent();
            
        }

        private void IDCMForm_Load(object sender, EventArgs e)
        {
#if DEBUG
            ToolStripMenuItem reserMenuItem = new ToolStripMenuItem("Reset DB");
            (MenuStrip_IDCM.Items[1] as ToolStripMenuItem).DropDownItems.Add(reserMenuItem);
            reserMenuItem.Click += new EventHandler(IDCM.AddedEI.Debug.DBReseter.resetDataSource);

            ToolStripMenuItem testMenuItem = new ToolStripMenuItem("Test Sqlite sync");
            (MenuStrip_IDCM.Items[1] as ToolStripMenuItem).DropDownItems.Add(testMenuItem);
            testMenuItem.Click += new EventHandler(IDCM.AddedEI.Test.SqLiteTester.doTest);
#endif
            //Thread.CurrentThread.Name = "IDCMForm" + HandleToken.nextTempID();
        }

        private void MenuStrip_IDCM_ItemAdded(object sender, ToolStripItemEventArgs e)
        {
            //if (e.Item.Text.Length == 0 || e.Item.Text == "还原(&R)" || e.Item.Text == "最小化(&N)")
            //{
            //    e.Item.Visible = false;
            //}
        }

        private void IDCMForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            manager.closeWorkSpaceHolder();
        }
        /// <summary>
        /// 打开或新建一个本地数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WorkSpaceHolder.InWorking)
            {
                DialogResult res = MessageBox.Show("A workspace is in working, you need close it first. Choose \"OK\" to close workspace or choose \"Cancel\" to quit.", "Close Workspace Notice", MessageBoxButtons.OKCancel);
                if (res.Equals(DialogResult.OK))
                {
                    manager.closeWorkSpaceHolder();
                }
                else
                    return;
            }
            if (WorkSpaceHolder.chooseWorkspace())
            {
                WorkSpaceHolder.startInstance();
            }
            manager.activeChildViewAwait(typeof(HomeViewManager), true);
        }
        public void setLoginTip(string tip=null)
        {
            ToolStripItemAsyncUtil.SyncSetText(this.toolStripTextBox_user, tip);
        }
        /// <summary>
        /// 打开或新建一个本地数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WorkSpaceHolder.InWorking)
            {
                DialogResult res = MessageBox.Show("A workspace is in working, you need close it first. Choose \"OK\" to close workspace or choose \"Cancel\" to quit.", "Close Workspace Notice", MessageBoxButtons.OKCancel);
                if (res.Equals(DialogResult.OK))
                {
                    manager.closeWorkSpaceHolder();
                }
                else
                    return;
            }
            if (WorkSpaceHolder.chooseWorkspace())
            {
                WorkSpaceHolder.startInstance();
            }
            manager.activeChildViewAwait(typeof(HomeViewManager), true);
        }
        /// <summary>
        /// 关闭一个本地数据库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WorkSpaceHolder.InWorking)
            {
                manager.closeWorkSpaceHolder();
            }
        }
        /// <summary>
        /// 更新字段模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void templatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.activeChildView(typeof(LibFieldManager), true);
        }
        /// <summary>
        /// IDCMForm主界面第一次显示后，启动默认的数据页面展示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IDCMForm_Shown(object sender, EventArgs e)
        {
            manager.activeChildViewAwait(typeof(HomeViewManager), true);
            manager.activeChildView(typeof(AuthenticationRetainer), false);
        }
        /// <summary>
        /// 身份认证信息配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void authToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.activeChildView(typeof(AuthenticationRetainer), true);
        }
    }
}
