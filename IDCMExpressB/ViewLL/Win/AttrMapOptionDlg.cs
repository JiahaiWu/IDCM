using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.ServiceBL.Common;
using IDCM.ServiceBL.Common.Converter;
using IDCM.ControlMBL.Utilities;

namespace IDCM.ViewLL.Win
{
    public partial class AttrMapOptionDlg : Form
    {
        public AttrMapOptionDlg()
        {
            InitializeComponent();
        }
        public void setInitCols(List<string> xlscols,List<string> dbList,ref Dictionary<string,string> mapping)
        {
            this.srcCols = xlscols;
            this.destCols = dbList;
            this.mapping = mapping;
            setSimilarMapping();
        }
        public void setSimilarMapping(double threshold=0.8)
        {
            Dictionary<ObjectPair<string, string>, double> mappingEntries = new Dictionary<ObjectPair<string, string>, double>();
            List<string> baseList = new List<string>();
            foreach (string str in destCols)
            {
                baseList.Add(CVNameConverter.toViewName(str));
            }
            StringSimilarity.computeSimilarMap(srcCols, baseList, ref mappingEntries);
            mapping.Clear();
            foreach (KeyValuePair<ObjectPair<string, string>, double> kvpair in mappingEntries)
            {
                if (kvpair.Value >= threshold)
                {
                    mapping[kvpair.Key.Val] = CVNameConverter.toDBName(kvpair.Key.Key);
                }
            }
            foreach (string col in srcCols)
            {
                if (!mapping.ContainsKey(col))
                {
                    mapping[col] = null;
                }
            }
            this.dataGridView_map.Rows.Clear();
            foreach (KeyValuePair<string, string> mappair in mapping)
            {
                this.dataGridView_map.Rows.Add(new string[] { mappair.Key, null, null, mappair.Value, null });
            }
            radioButton_similarity.Checked = true;
        }
        public void setExtractMapping()
        {
            HashSet<string> baseSet = new HashSet<string>(destCols);
            mapping.Clear();
            foreach (string col in srcCols)
            {
                if (baseSet.Contains(CVNameConverter.toDBName(col)))
                {
                    mapping[col] = CVNameConverter.toDBName(col);
                }
                else
                {
                    mapping[col] = null;
                }
            }
            this.dataGridView_map.Rows.Clear();
            foreach (KeyValuePair<string, string> mappair in mapping)
            {
                this.dataGridView_map.Rows.Add(new string[] { mappair.Key, null, null, CVNameConverter.toViewName(mappair.Value), null });
            }
            radioButton_exact.Checked = true;
        }
        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button_confirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void radioButton_similarity_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButton_similarity.Checked)
                setSimilarMapping();
        }

        private void radioButton_exact_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_exact.Checked)
                setExtractMapping();
        }


        private void dataGridView_map_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView_map.RowHeadersWidth - 4, e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), dataGridView_map.RowHeadersDefaultCellStyle.Font, rectangle,
                dataGridView_map.RowHeadersDefaultCellStyle.ForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
            dataGridView_map.AutoResizeRowHeadersWidth(DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders);
        }

        private void dataGridView_map_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.ColumnIndex < 0 || e.RowIndex < 0)
                    return;
                if (dataGridView_map.Columns[e.ColumnIndex].HeaderText.Equals("Unbound"))
                {
                    DataGridViewCell dgvcell = dataGridView_map.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dgvcell.Value = null;
                    string col = dataGridView_map.Rows[e.RowIndex].Cells[0].Value.ToString();
                    mapping[col] = null;
                    dataGridView_map.Rows[e.RowIndex].Cells[3].Value = null;
                    radioButton_custom.Checked = true;
                }
                else if (dataGridView_map.Columns[e.ColumnIndex].HeaderText.Equals("Config"))
                {
                    toolStripComboBox_dest.Items.AddRange(unboundDestCols());
                    toolStripComboBox_dest.SelectedIndex = 0;
                    ControlUtil.ClearEvent(toolStripComboBox_dest, "SelectedIndexChanged");
                    toolStripComboBox_dest.Click += delegate(object tsender, EventArgs te) { toolStripComboBox_dest_Changed(tsender, te, e.ColumnIndex, e.RowIndex); };
                    contextMenuStrip_destList.Show(MousePosition);
                }
            }
        }
        private void toolStripComboBox_dest_Changed(object sender, EventArgs e, int columnIndex, int rowIndex)
        {
            string stext=toolStripComboBox_dest.SelectedText;
            if (stext!=null)
            {
                dataGridView_map.Rows[rowIndex].Cells[3].Value =stext;
                mapping[dataGridView_map.Rows[rowIndex].Cells[0].Value.ToString()] = CVNameConverter.toDBName(stext);
            }
        }
        private string[] unboundDestCols()
        {
            List<string> res = new List<string>();
            HashSet<string> dests=new HashSet<string>(mapping.Values);
            foreach (string col in destCols)
            {
                if (!dests.Contains(col))
                    res.Add(col);
            }
            return res.ToArray();
        }
        private List<string> srcCols = null;
        private List<string> destCols = null;
        private Dictionary<string, string> mapping = null;
    }
}
