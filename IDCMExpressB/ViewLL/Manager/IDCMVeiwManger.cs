using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.ViewLL.Win;
using System.Windows.Forms;
using IDCM.AppContext;


namespace IDCM.ViewLL.Manager
{
    /// <summary>
    /// IDCM主界面封装主管控制器实现
    /// @author JiahaiWu  2014-10-30
    /// </summary>
    public class IDCMVeiwManger
    {
        #region 构造&析构
        public IDCMVeiwManger()
        {
            mainForm = new IDCMForm();
            LongTermHandleNoter.note(mainForm);
            mainForm.setManager(this);
            subManagers = new Dictionary<Type, ManagerI>();
        }
        ~IDCMVeiwManger()
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
        internal volatile IDCMForm mainForm = null;
        internal volatile Dictionary<Type, ManagerI> subManagers = null;
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
            if (obj == null || obj.IsDisposed())
            {
                obj = Activator.CreateInstance(manager) as ManagerI;
                subManagers[manager] = obj;
                obj.initView(false);
                obj.setMdiParent(this.mainForm);
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
                    foreach (ManagerI ma in subManagers.Values)
                    {
                        ma.setMaxToNormal();
                    }
                    view.setToMaxmize(activeFront);
                }
                return view != null;
            }
            return false;
        }
    }
}
