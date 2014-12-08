using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.ViewLL.Manager;

namespace IDCM.ViewLL.Win
{
    public partial class GCMView : Form
    {
        public GCMView()
        {
            InitializeComponent();
            this.splitContainer_left.Panel1Collapsed = true;
            dataGridView_items.AllowDrop = true;
            dataGridView_items.AllowUserToAddRows = false;
            dataGridView_items.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView_items.EditMode = DataGridViewEditMode.EditOnKeystroke;
            ////dataGridView_items.TopLeftHeaderCell = new DataGridViewTopLeftHeaderCell();
            ////dataGridView_items.AdjustedTopLeftHeaderBorderStyle = DataGridViewTopLeftHeaderCell.MeasureTextPreferredSize();

        }

        private GCMViewManager manager=null;

        public ToolStripProgressBar getProgressBar()
        {
            return this.toolStripProgressBar_request;
        }
        public DataGridView getItemGridView()
        {
            return this.dataGridView_items;
        }
        public TabControl getRecordTab()
        {
            return this.tabControl_rec;
        }
        public void setManager(GCMViewManager manager)
        {
            this.manager=manager;
        }
        public TableLayoutPanel getSearchPanel()
        {
            return this.tableLayoutPanel_search;
        }
        public SplitContainer getSearchSpliter()
        {
            return this.splitContainer_left;
        }
        /////////////////////////////////////
        private void toolStripButton_local_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton_down_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton_refresh_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton_search_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton_help_Click(object sender, EventArgs e)
        {

        }

        private void btn_search_Click(object sender, EventArgs e)
        {

        }

        private void btn_options_Click(object sender, EventArgs e)
        {

        }

        private void GCMView_Shown(object sender, EventArgs e)
        {
            activeDataView(true);
        }
        /// <summary>
        /// activeHomeView
        /// </summary>
        /// <param name="refresh"></param>
        public void activeDataView(bool refresh = true)
        {
            //加载默认的数据报表展示
            manager.loadDataSetView();
            manager.updateRecordView();
            //resize for data view
            dataGridView_items.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //dataGridView_items.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.None);
            dataGridView_items.AllowUserToResizeColumns = true;
        }

    }
}
