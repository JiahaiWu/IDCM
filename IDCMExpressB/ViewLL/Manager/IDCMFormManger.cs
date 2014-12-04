using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.ViewLL.Win;
using System.Windows.Forms;
using IDCM.AppContext;
using IDCM.ServiceBL.Handle;
using IDCM.ServiceBL.CmdChannel;

namespace IDCM.ViewLL.Manager
{
    /// <summary>
    /// IDCM主界面封装主管控制器实现
    /// @author JiahaiWu  2014-10-30
    /// </summary>
    public class IDCMFormManger
    {
        #region 构造&析构
        public IDCMFormManger()
        {
            mainForm = new IDCMForm();
            LongTermHandleNoter.note(mainForm);
            mainForm.setManager(this);
            subManagers = new Dictionary<Type, ManagerI>();
        }
        public static IDCMFormManger getInstance()
        {
            return IDCMAppContext.MainManger;
        }
        ~IDCMFormManger()
        {
            Dispose();
        }
        public void Dispose()
        {
            if (mainForm != null && !mainForm.IsDisposed)
            {
                mainForm.Close();
                mainForm.Dispose();
                mainForm = null;
            }
            subManagers = null;
        }
        #endregion
        #region 实例对象保持部分
        //主页面窗口实例
        internal IDCMForm mainForm = null;
        internal Dictionary<Type, ManagerI> subManagers = null;
        #endregion

        /// <summary>
        /// 主窗体初始化方法
        /// </summary>
        /// <param name="activeShow"></param>
        /// <returns></returns>
        public bool initForm(bool activeShow = true)
        {
            if (WorkSpaceHolder.InWorking || WorkSpaceHolder.verifyForLoad())
            {
                mainForm.WindowState = FormWindowState.Maximized;
                if (activeShow)
                {
                    mainForm.Show();
                }
                else
                    mainForm.Hide();
                return true;
            }
            Dispose();
            return false;
        }
        /// <summary>
        /// 获取子窗体实例
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        internal ManagerI getManager(Type manager)
        {
            ManagerI obj = null;
            subManagers.TryGetValue(manager, out obj);
            if (obj == null || obj.isDisposed())
            {
                obj = Activator.CreateInstance(manager) as ManagerI;
                subManagers[manager] = obj;
                if (obj.initView(false))
                {
                    obj.setMdiParent(this.mainForm);
                }
            }
            return obj;
        }
        /// <summary>
        /// 激活直属视图实例，及其必要的窗口显示操作。
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="activeFront"></param>
        /// <returns></returns>
        public bool activeChildView(Type manager, bool activeFront = false)
        {
            if (typeof(ManagerI).IsAssignableFrom(manager))
            {
                ManagerI view = getManager(manager);
                if (activeFront)
                {
                    if(manager.IsSubclassOf(typeof(RetainerA)))
                    {
                        view.initView(true);
                    }else{
                        foreach (ManagerI ma in subManagers.Values)
                        {
                            ma.setMaxToNormal();
                        }
                        view.setToMaxmize(activeFront);
                    }
                }
                return view != null;
            }
            return false;
        }
        /// <summary>
        /// 显示等待提示页面，并隐式地激活直属视图实例及其必要的窗口显示操作。
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="activeFront"></param>
        /// <returns></returns>
        public void activeChildViewAwait(Type manager, bool activeFront = false)
        {
            Form startForm = new StartForm();
            startForm.Show();
            startForm.Update();
            bool res = activeChildView(manager, activeFront);
            startForm.Close();
            startForm.Dispose();
        }
        /// <summary>
        /// 关闭当前工作空间，仅保留主框架窗口
        /// </summary>
        /// <returns></returns>
        public bool closeWorkSpaceHolder()
        {
            foreach (ManagerI ma in subManagers.Values)
            {
                ma.dispose();
            }
            subManagers.Clear();
            WorkSpaceHolder.close();
            return true;
        }
        /// <summary>
        /// 更新用户身份认证状态
        /// </summary>
        /// <param name="uname"></param>
        public void updateUserStatus(string uname = null)
        {
            string tip = "Off Line";
            if (uname != null)
                tip = "On Line: " + uname;
            mainForm.setLoginTip(tip);
        }
        public void showDBDataSearch()
        {
            HomeViewManager hvManager = (HomeViewManager)getManager(typeof(HomeViewManager));
            if (hvManager != null)
            {
                if (hvManager.isActive())
                {
                    hvManager.showDBDataSearch();
                }
            }
        }
        public void frontDataSearch()
        {
            HomeViewManager hvManager = (HomeViewManager)getManager(typeof(HomeViewManager));
            if (hvManager != null)
            {
                if (hvManager.isActive())
                {
                    hvManager.frontDataSearch();
                }
            }
        }
        public void frontSearchNext()
        {
            HomeViewManager hvManager = (HomeViewManager)getManager(typeof(HomeViewManager));
            if (hvManager != null)
            {
                if (hvManager.isActive())
                {
                    hvManager.frontSearchNext();
                }
            }
        }
        public void frontSearchPrev()
        {
            HomeViewManager hvManager = (HomeViewManager)getManager(typeof(HomeViewManager));
            if (hvManager != null)
            {
                if (hvManager.isActive())
                {
                    hvManager.frontSearchPrev();
                }
            }
        }
    }
}