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


        private void dataGridView_map_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return;
            if (dataGridView_map.Columns[e.ColumnIndex].HeaderText.Equals("Unbound"))
            {
                DataGridViewCell dgvcell = dataGridView_map.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dgvcell.Value = null;
                string col = dataGridView_map.Rows[e.RowIndex].Cells[0].Value.ToString();
                mapping[col] = null;
                radioButton_custom.Checked = true;
            }
            else if (dataGridView_map.Columns[e.ColumnIndex].HeaderText.Equals("Config"))
            {
                //弹出映射选择列表
            }
        }

        private List<string> srcCols = null;
        private List<string> destCols = null;
        private Dictionary<string, string> mapping = null;
    }
}
