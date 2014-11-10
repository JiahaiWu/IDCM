using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;
using System.Data;
using System.IO;

namespace IDCM.ServiceBL.DataTransfer
{
    class ExcelExporter
    {
        public bool exportExcel(string filepath, DataGridView dgv)
        {
            try
            {
                IWorkbook workbook = null;
                string suffix = Path.GetExtension(filepath).ToLower();
                if (suffix.Equals(".xlsx"))
                    workbook = new XSSFWorkbook();
                else
                    workbook = new HSSFWorkbook();
                ISheet sheet= workbook.CreateSheet("Core Datasets");
                IRow rowHead = sheet.CreateRow(0);
                HashSet<int> excludes = new HashSet<int>();
                //填写表头
                ICellStyle headStyle = workbook.CreateCellStyle();
                for (int i = 0; i < dgv.ColumnCount; i++)
                {
                    if (dgv.Columns[i].Name.StartsWith("["))
                    {
                        ICell cell = rowHead.CreateCell(i, CellType.String);
                        cell.SetCellValue(dgv.Columns[i].HeaderText.ToString());
                        cell.CellStyle.FillBackgroundColor = IndexedColors.Green.Index;
                    }
                    else
                    {
                        excludes.Add(i);
                    }
                }
                CellRangeAddress cra = CellRangeAddress.ValueOf("A1:" + numToExcelIndex(dgv.ColumnCount)+"1");
                sheet.SetAutoFilter(cra);
                //填写内容
                int ridx = 1;
                foreach (DataGridViewRow dgvr in dgv.Rows)
                {
                    if (dgvr.IsNewRow)
                        continue;
                    IRow row = sheet.CreateRow(ridx);
                    for (int i = 0; i < dgv.ColumnCount; i++)
                    {
                        if(excludes.Contains(i))
                            continue;
                        DataGridViewCell dcell=dgvr.Cells[i];
                        if (dcell != null && dcell.Value != null)
                        {
                            row.CreateCell(i).SetCellValue(dcell.Value.ToString());
                        }
                    }
                }
                using (FileStream fs = File.Create(filepath))
                {
                    workbook.Write(fs);
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
        /// <summary>
        /// 用于针对Excel的列名转换实现，1->A
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected string numToExcelIndex(int value)
        {
            if (value < 1 || value > 18277)
            {
#if DEBUG
                System.Diagnostics.Debug.Assert(value>0 && value<18278);
#endif
                return null;
            }
            string rtn = string.Empty;
            List<int> iList = new List<int>();
            //To single Int
            while (value / 26 != 0 || value % 26 != 0)
            {
                iList.Add(value % 26);
                value /= 26;
            }
            //Change 0 To 26
            for (int j = 0; j < iList.Count - 1; j++)
            {
                if (iList[j] == 0)
                {
                    iList[j + 1] -= 1;
                    iList[j] = 26;
                }
            }
            //Remove 0 at last
            if (iList[iList.Count - 1] == 0)
            {
                iList.Remove(iList[iList.Count - 1]);
            }
            //To String
            for (int j = iList.Count - 1; j >= 0; j--)
            {
                char c = (char)(iList[j] + 64);
                rtn += c.ToString();
            }
            return rtn;
        }
    }
}
