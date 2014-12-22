using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.ServiceBL.Common.GCMStrainElement;
using System.Windows.Forms;
using IDCM.ControlMBL.AsyncInvoker;

namespace IDCM.ServiceBL.NetTransfer
{
    class GCMDataLoader
    {
        /// <summary>
        /// 通过网络请求，载入数据显示
        /// </summary>
        public static bool loadData(DataGridView itemDGV, TreeView recordTree,ListView recordView, Dictionary<string, int> loadedNoter)
        {
            int curPage = 1;
            StrainListPage slp = StrainListQueryExecutor.strainListQuery(curPage);
            showDataItems(slp, itemDGV, loadedNoter);
            while (hasNextPage(slp, curPage))
            {
                curPage++;
                slp = StrainListQueryExecutor.strainListQuery(curPage);
                showDataItems(slp,itemDGV,loadedNoter);
            }
            if (loadedNoter.Count > 0)
            {
                string strainid = loadedNoter.First().Key;
                StrainView sv = StrainViewQueryExecutor.strainViewQuery(strainid);
                showRecordView(sv,recordTree,recordView);
            }
            return true;
        }
        /// <summary>
        /// 将网络获取数据显示到itemDGV，并予以标记缓存
        /// </summary>
        /// <param name="slp"></param>
        private static void showDataItems(StrainListPage slp, DataGridView itemDGV,Dictionary<string, int> loadedNoter)
        {
            if (slp == null || slp.list == null)
                return;
            if (!itemDGV.Columns.Contains("id"))
            {
                DataGridViewTextBoxColumn dgvtbc = new DataGridViewTextBoxColumn();
                dgvtbc.Name = "id";
                dgvtbc.HeaderText = "id";
                dgvtbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                DGVAsyncUtil.syncAddCol(itemDGV, dgvtbc);  ///itemDGV.Columns.Add(dgvtbc);
            }
            foreach (Dictionary<string, string> valMap in slp.list)
            {
                //add valMap note Tag into loadedNoter Map
                int dgvrIdx = -1;
                loadedNoter.TryGetValue(valMap["id"], out dgvrIdx);
                if (dgvrIdx <= 0)
                {
                    dgvrIdx = itemDGV.RowCount;
                    DGVAsyncUtil.syncAddRow(itemDGV, null, dgvrIdx); //dgvrIdx = itemDGV.Rows.Add();
                    loadedNoter.Add(valMap["id"], dgvrIdx);
                }
                foreach (KeyValuePair<string, string> entry in valMap)
                {
                    //if itemDGV not contains Column of entry.key
                    //   add Column named with entry.key
                    //then merge data into itemDGV View.
                    //(if this valMap has exist in loadedNoter Map use Update Method else is append Method.) 
                    if (!itemDGV.Columns.Contains(entry.Key))
                    {
                        DataGridViewTextBoxColumn dgvtbc = new DataGridViewTextBoxColumn();
                        dgvtbc.Name = entry.Key;
                        dgvtbc.HeaderText = entry.Key;
                        dgvtbc.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                        DGVAsyncUtil.syncAddCol(itemDGV, dgvtbc);  ///itemDGV.Columns.Add(dgvtbc);
                    }
                    DataGridViewCell dgvc = itemDGV.Rows[dgvrIdx].Cells[entry.Key];
                    if (dgvc != null)
                    {
                        DGVAsyncUtil.syncValue(itemDGV, dgvc, entry.Value);  ////dgvc.Value = entry.Value;
                    }
                }
            }
        }
        /// <summary>
        /// 判断分页请求是否存在下一页内容
        /// </summary>
        /// <param name="slp"></param>
        /// <param name="reqPage"></param>
        /// <returns></returns>
        private static bool hasNextPage(StrainListPage slp, int reqPage)
        {
            if (slp != null && slp.totalpage > slp.pageNumber && slp.totalpage > reqPage)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 将目标记录数据显示到细览控件中
        /// </summary>
        /// <param name="sv"></param>
        /// <param name="recordTab"></param>
        public static void showRecordView(StrainView sv, TreeView recordTree,ListView recordView)
        {
            if (sv == null)
                return;
            foreach (KeyValuePair<string, object> svEntry in sv.ToDictionary())
            {
                TreeNode node = new TreeNode(svEntry.Key);
                node.Name = svEntry.Key;
                if (svEntry.Value is string)
                {
                    node.Tag = svEntry.Value;
                }
                else if (svEntry.Value is Dictionary<string, dynamic>)
                {
                    foreach (KeyValuePair<string, dynamic> subEntry in svEntry.Value as Dictionary<string, dynamic>)
                    {
                        TreeNode subNode = new TreeNode(subEntry.Key);
                        subNode.Name = subEntry.Key;
                        subNode.Tag =Convert.ToString(subEntry.Value);
                        node.Nodes.Add(subNode);
                    }
                }
                TreeViewAsyncUtil.syncAddNode(recordTree, node); //recordTree.Nodes.Add(node);
            }
        }
    }
}
