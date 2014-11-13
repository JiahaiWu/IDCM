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
using IDCM.ServiceBL.Handle;
using IDCM.ServiceBL.CmdChannel;
using IDCM.OverallSC.ShareSync;
using IDCM.SimpleDAL.DBCP;

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
            BackProgressIndicator.addIndicatorBar(homeView.getProgressBar());
        }
        public static HomeViewManager getInstance()
        {
            ManagerI hvm = IDCMAppContext.MainManger.getManager(typeof(HomeViewManager));
            return hvm == null ? null : (hvm as HomeViewManager);
        }
        ~HomeViewManager()
        {
            dispose();
        }
        #endregion
        #region 实例对象保持部分
        public void dispose()
        {
            isDisposed = true;
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
            if (homeView != null && !homeView.IsDisposed)
            {
                homeView.Close();
                homeView.Dispose();
                homeView = null;
            }
        }
        
        private volatile bool isDisposed = false;
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
        public bool IsDisposed()
        {
            return isDisposed;
        }
        #endregion

        public void loadDataSetView(TreeNode tnode)
        {
            datasetBuilder.loadDataSetView();
            updateDataSet(tnode);
        }
        public void loadTreeSet()
        {
            libBuilder.loadTreeSet();
        }
        /// <summary>
        /// 导入数据文档
        /// </summary>
        /// <param name="fpath"></param>
        public void importData(string fpath)
        {
            if (fpath.ToLower().EndsWith("xls") || fpath.ToLower().EndsWith(".xlsx"))
            {
                ExcelImportHandler eih = new ExcelImportHandler(fpath, datasetBuilder.CURRENT_LID, datasetBuilder.CURRENT_PLID);
                eih.addHandler(new UpdateHomeDataViewHandler(libBuilder.SelectedNode_Current, homeView.getItemGridView()));
                UpdateHomeLibCountHandler uhlch = new UpdateHomeLibCountHandler(homeView.getLibTree(), homeView.getBaseTree());
                eih.addHandler(uhlch);
                uhlch.addHandler(new SelectDataRowHandler(homeView.getItemGridView(), homeView.getAttachTabControl()));
                CmdConsole.call(eih, CmdConsole.CmdReqOption.L);
            }
        }
        /// <summary>
        /// 导出数据文档
        /// </summary>
        /// <param name="fpath"></param>
        public void exportData(ExportType etype, string fpath)
        {
            //DataGridView itemDGV = homeView.getItemGridView();
            KeyValuePair<string, int> lastQuery = QueryCmdCache.getLastCDTRQuery();
            AbsHandler handler = null;
            switch (etype)
            {
                case ExportType.Excel:
                    handler = new ExcelExportHandler(fpath, lastQuery.Key,lastQuery.Value);
                    CmdConsole.call(handler);
                    break;
                case ExportType.JSONList:
                    handler = new JSONListExportHandler(fpath, lastQuery.Key, lastQuery.Value);
                    CmdConsole.call(handler);
                    break;
                case ExportType.TSV:
                    handler = new TextExportHandler(fpath, lastQuery.Key, lastQuery.Value, "\t");
                    CmdConsole.call(handler);
                    break;
                case ExportType.CSV:
                    handler = new TextExportHandler(fpath, lastQuery.Key, lastQuery.Value, ",");
                    CmdConsole.call(handler);
                    break;
                default:
                    MessageBox.Show("Unsupport export type!");
                    break;
            }
        }
        /// <summary>
        /// 更新分类目录关联文档数显示
        /// </summary>
        /// <param name="focusNode"></param>
        public void updateLibRecCount(TreeNode focusNode = null)
        {
            UpdateHomeLibCountHandler ulch = null;
            if (focusNode == null)
                ulch = new UpdateHomeLibCountHandler(homeView.getLibTree(),homeView.getBaseTree());
            else
                ulch = new UpdateHomeLibCountHandler(focusNode);
            CmdConsole.call(ulch);
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
           bool needUpdateData= libBuilder.noteCurSelectedNode(node);
           if (needUpdateData)
               updateDataSet(node);
        }
        /// <summary>
        /// 根据指定的数据集合加载数据报表显示
        /// </summary>
        public void updateDataSet(TreeNode filterNode)
        {
            datasetBuilder.noteDataSetLib(filterNode); //待考虑顺序问题///////////
            UpdateHomeDataViewHandler uhdvh = new UpdateHomeDataViewHandler(filterNode, homeView.getItemGridView());
            uhdvh.addHandler(new SelectDataRowHandler(homeView.getItemGridView(), homeView.getAttachTabControl()));
            uhdvh.addHandler(new UpdateHomeLibCountHandler(filterNode));
            CmdConsole.call(uhdvh);
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
            get { return datasetBuilder.CURRENT_RID; }
            set { datasetBuilder.CURRENT_RID=value; }
        }
        public long CURRENT_LID
        {
            get { return datasetBuilder.CURRENT_LID; }
        }
        public TreeNode RootNode_unfiled
        {
            get
            {
                return libBuilder.RootNode_unfiled;
            }
        }
    }
}
