using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.ViewLL.Win;
using System.Windows.Forms;
using IDCM.AppContext;
using IDCM.SimpleDAL.DAM;
using IDCM.ControlMBL.Module;
using IDCM.ServiceBL.Common;

namespace IDCM.ViewLL.Manager
{
    /// <summary>
    /// HomeView布局管理器及动态表现事务调度中心
    /// @author JiahaiWu 2014-10-15
    /// </summary>
    class HomeViewManager : ManagerI
    {
        #region 构造&析构
        public HomeViewManager()
        {
            homeView = new HomeView();
            LongTermHandleNoter.note(homeView);
            homeView.setManager(this);
            libBuilder = new LocalLibBuilder(homeView.getBaseTree(), homeView.getLibTree());
            datasetBuilder = new LocalDataSetBuilder(homeView.getItemGridView(), homeView.getAttachTabControl());
        }
        ~HomeViewManager()
        {
            dispose();
        }
        #endregion
        #region 实例对象保持部分
        public void dispose()
        {
            if (homeView != null && !homeView.IsDisposed)
            {
                homeView.Close();
                homeView.Dispose();
                homeView = null;
            }
            if (libBuilder != null)
            {
                libBuilder.Dispose();
                libBuilder = null;
            }
            if (datasetBuilder != null)
            {
                datasetBuilder.Dispose();
                datasetBuilder = null;
            }
        }
        //页面窗口实例
        private volatile HomeView homeView = null;
        private volatile LocalLibBuilder libBuilder = null;
        private volatile LocalDataSetBuilder datasetBuilder = null;
        #endregion
        #region 接口实例化部分
        /// <summary>
        /// 对象实例化初始化方法
        /// </summary>
        /// <returns></returns>
        public bool initView(bool activeShow = true)
        {
            if (homeView == null || homeView.IsDisposed)
            {
                homeView = new HomeView();
                LongTermHandleNoter.note(homeView);
                homeView.setManager(this);
                libBuilder = new LocalLibBuilder(homeView.getBaseTree(), homeView.getLibTree());
                datasetBuilder = new LocalDataSetBuilder(homeView.getItemGridView(), homeView.getAttachTabControl());
            }
            if (CustomTColDefDAM.checkTableSetting())
            {
                if (activeShow)
                {
                    homeView.WindowState = FormWindowState.Maximized;
                    homeView.Show();
                    homeView.Activate();
                }
                else
                {
                    homeView.Hide();
                }
                return true;
            }
            dispose();
            return false;
        }
        public void setMaxToNormal()
        {
            if (homeView.WindowState.Equals(FormWindowState.Maximized))
                homeView.WindowState = FormWindowState.Normal;
        }
        public void setToMaxmize(bool activeFront = false)
        {
            homeView.WindowState = FormWindowState.Maximized;
            if (activeFront)
            {
                homeView.Show();
                homeView.Activate();
            }
        }
        public void setMdiParent(Form pForm)
        {
            homeView.MdiParent = pForm;
        }
        #endregion

        public void loadDataSetView(TreeNode tnode)
        {
            datasetBuilder.loadDataSetView(tnode);
        }
        public void loadTreeSet()
        {
            libBuilder.loadTreeSet();
        }
        public void importData(string fpath)
        {
            datasetBuilder.importData(fpath, homeView.getBaseTree(), homeView.getLibTree());
        }
        public void exportData(ExportType etype, string fpath)
        {
            datasetBuilder.exportData(etype, fpath);
        }
        public void updateLibRecCount(TreeNode node = null)
        {
            libBuilder.updateLibRecCount(node);
        }
        public void selectViewRecord(DataGridViewRow dgvr)
        {
            datasetBuilder.selectViewRecord(dgvr);
        }
        public void trashDataSet(TreeNode filteNode, int newlid = LibraryNodeDAM.REC_UNFILED)
        {
            datasetBuilder.trashDataSet(filteNode, newlid);
        }
        public void deleteNode(TreeNode treeNode)
        {
            libBuilder.deleteNode(treeNode);
        }
        public void addGroup(TreeNode treeNode)
        {
            libBuilder.addGroup(treeNode);
        }
        public void addGroupSet(TreeNode treeNode)
        {
            libBuilder.addGroupSet(treeNode);
        }
        public void renameNode(TreeNode treeNode, string label)
        {
            libBuilder.renameNode(treeNode, label);
        }

        public void noteCurSelectedNode(TreeNode node)
        {
            libBuilder.noteCurSelectedNode(node);
        }
        public void updateDataSet(TreeNode filterNode)
        {

            datasetBuilder.updateDataSet(filterNode);
        }
        public void addNewRecord()
        {
            datasetBuilder.addNewRecord();
        }

        public TreeNode SelectedNode_Current
        {
            get { return libBuilder != null ? libBuilder.SelectedNode_Current : null; }
        }
        public long CURRENT_RID
        {
            get { return LocalDataSetBuilder.CURRENT_RID; }
        }
        public long CURRENT_LID
        {
            get { return datasetBuilder.CURRENT_LID; }
        }
        /// <summary>
        /// 基于总控对象装载容器查找特定控件元素，支持静态化访问形式（仅用于常驻组件访问需要）。
        /// 如果查找成功，则返回目标对象。
        /// @author JiahaiWu 2015-11-01
        /// </summary>
        public static TreeNode RootNode_unfiled
        {
            get
            {
                if (IDCMAppContext.MainManger != null)
                {
                    ManagerI hvm = IDCMAppContext.MainManger.getManager(typeof(HomeViewManager));
                    if (hvm != null)
                    {
                        LocalLibBuilder llb = (hvm as HomeViewManager).libBuilder;
                        if (llb != null)
                            return llb.RootNode_unfiled;
                    }
                }
                return null;
            }
        }
    }
}
