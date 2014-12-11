using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.ServiceBL.Common;
using System.Windows.Forms;

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
            foreach (Dictionary<string, string> valMap in slp.list)
            {
                //add valMap note Tag into loadedNoter Map
                int dgvrIdx = -1;
                loadedNoter.TryGetValue(valMap["id"], out dgvrIdx);
                if (dgvrIdx < 0)
                {
                    dgvrIdx = itemDGV.Rows.Add();
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
                        itemDGV.Columns.Add(dgvtbc);
                    }
                    DataGridViewCell dgvc = itemDGV.Rows[dgvrIdx].Cells[entry.Key];
                    if (dgvc != null)
                    {
                        dgvc.Value = entry.Value;
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
            foreach (KeyValuePair<string, string> svEntry in sv)
            {
                TreeNode node = new TreeNode(svEntry.Key);
                node.Name = svEntry.Key;
                node.Tag = svEntry.Value;
                recordTree.Nodes.Add(node);
            }
        }
    }
}
