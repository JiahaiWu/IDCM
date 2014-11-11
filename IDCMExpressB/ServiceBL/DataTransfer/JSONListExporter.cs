using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;

namespace IDCM.ServiceBL.DataTransfer
{
    class JSONListExporter
    {
        public bool exportJSONList(string filepath,DataGridView dgv)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                int count = 0;
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    foreach (DataGridViewRow dgvr in dgv.Rows)
                    {
                        if (dgvr.IsNewRow)
                            continue;
                        Dictionary<string, string> record = ConverttoRecDict(dgvr);
                        if (record.Count > 0)
                        {
                            string jsonStr = JsonConvert.SerializeObject(record);
                            strbuilder.Append(jsonStr).Append("\n\r");
                            if (++count % 100 == 0)
                            {
                                Byte[] info = new UTF8Encoding(true).GetBytes(strbuilder.ToString());
                                BinaryWriter bw = new BinaryWriter(fs);
                                fs.Write(info, 0, info.Length);
                                strbuilder.Length = 0;
                            }
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
        protected Dictionary<string, string> ConverttoRecDict(DataGridViewRow dgvr)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach(DataGridViewCell dgvc in dgvr.Cells)
            {
                if (dgvc.Value != null && dgvc.OwningColumn.Name.StartsWith("["))
                {
                    string val=dgvc.Value.ToString();
                    if (val.Length > 0)
                        dict[dgvc.OwningColumn.HeaderText] = val;
                }
            }
            return dict;
        }
    }
}
