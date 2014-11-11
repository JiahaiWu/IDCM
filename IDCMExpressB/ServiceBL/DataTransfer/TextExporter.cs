using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using IDCM.ControlMBL.Utilities;

namespace IDCM.ServiceBL.DataTransfer
{
    class TextExporter
    {
        public bool exportText(string filepath, DataGridView dgv,string spliter=" ")
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                int count = 0;
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    HashSet<int> excludes = new HashSet<int>();
                    List<string> vals = new List<string>();
                    //填写表头
                    int i = 0;
                    for (i = 0; i < dgv.ColumnCount; i++)
                    {
                        if (dgv.Columns[i].Name.StartsWith("["))
                        {
                            vals.Add(dgv.Columns[i].HeaderText);
                        }
                        else
                        {
                            excludes.Add(i);
                        }
                    }
                    for (i = 0; i < vals.Count - 1; i++)
                    {
                        strbuilder.Append(vals[i]).Append(spliter);
                    }
                    strbuilder.Append(vals[i]);
                    //填写内容
                    foreach (DataGridViewRow dgvr in dgv.Rows)
                    {
                        if (dgvr.IsNewRow)
                            continue;
                        DataGridViewCellCollection cells = dgvr.Cells;
                        int j = 0;
                        if (j < cells.Count)
                        {
                            vals.Clear();
                            for (j = 0; j < cells.Count; j++)
                            {
                                if (excludes.Contains(j))
                                    continue;
                                vals.Add(DGVUtil.getCellValue(cells[j]));
                            }
                            strbuilder.Append("\n\r");
                            for (j = 0; j < vals.Count - 1; j++)
                            {
                                strbuilder.Append(vals[j]).Append(spliter);
                            }
                            strbuilder.Append(vals[j]);
                        }
                        if (++count % 100 == 0)
                        {
                            Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                            BinaryWriter bw = new BinaryWriter(fs);
                            fs.Write(info, 0, info.Length);
                            strbuilder.Length = 0;
                        }
                    }
                    if (strbuilder.Length > 0)
                    {
                        Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                        BinaryWriter bw = new BinaryWriter(fs);
                        fs.Write(info, 0, info.Length);
                        strbuilder.Length = 0;
                    }
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR::" + ex.Message + "\n" + ex.StackTrace);
                return false;
            }
            return true;
        }
        
    }
}
