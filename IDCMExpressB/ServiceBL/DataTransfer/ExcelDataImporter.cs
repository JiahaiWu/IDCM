using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Collections;
using System.Data.OleDb;
using System.Threading;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using IDCM.OverallSC.Commons;
using IDCM.ControlMBL.Utilities;
using IDCM.ControlMBL.AsyncInvoker;
using IDCM.ServiceBL.DataTransfer;
using IDCM.SimpleDAL.DAM;
using IDCM.SimpleDAL.POO;
using IDCM.ServiceBL.Common.Converter;

namespace IDCM.ServiceBL.DataTransfer
{
    class ExcelDataImporter
    {
        /// <summary>
        /// 线程锁对象
        /// </summary>
        private object objLock = new object();
        /// <summary>
        /// 初始化关注的DataGridView
        /// </summary>
        /// <param name="dgv_sample"></param>
        public ExcelDataImporter(DataGridView dgv_sample)
        {
            this.dgv_data = dgv_sample;
        }

        /// <summary>
        /// 解析指定的Excel文档，执行数据转换.
        /// 本方法调用对类功能予以线程包装，用于异步调用如何方法。
        /// 在本线程调用下的控件调用，需通过UI控件的Invoke/BegainInvoke方法更新。
        /// </summary>
        /// <param name="fpath"></param>
        /// <returns>返回请求流程是否执行完成</returns>
        public bool parseExcelData(string fpath)
        {
            if (fpath == null || fpath.Length < 1)
                return false;
            string fullPath = System.IO.Path.GetFullPath(fpath);
            lock (objLock)
            {
                IWorkbook workbook = null;
                FileStream fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                try
                {
                    workbook = WorkbookFactory.Create(fs);
                    ISheet dataSheet = workbook.GetSheet("Core Datasets");
                    if (dataSheet == null)
                        dataSheet = workbook.GetSheetAt(1);
                    parseSheetInfo(dataSheet, dgv_data);
                }
                catch (Exception ex)
                {
                    log.Info("ERROR: Excel文件导入失败！ ", ex);
                    MessageBox.Show("ERROR: Excel文件导入失败！ " + ex.Message + "\n" + ex.StackTrace);
                }
                finally
                {
                    fs.Close();
                }
            }
            return true;
        }
        /// <summary>
        /// 通过NPOI读取Excel文档，转换可识别内容至本地数据表格视图中
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="dgv"></param>
        public void parseSheetInfo(ISheet sheet, DataGridView dgv)
        {
            int skipIdx = 1;
            if (sheet == null || sheet.LastRowNum < skipIdx) //no data
                return;
            if (dgv.RowCount > 0)
            {
                DataGridViewRow lastRow = dgv.Rows[dgv.RowCount - 1];
                if (lastRow != null && !lastRow.IsNewRow)
                {
                    if (lastRow.Cells[0].Value == null)
                        DGVAsyncUtil.syncRemoveRow(dgv, dgv.Rows[dgv.RowCount - 1]);
                }
            }
            /////////////////////////////////////////////////////////
            IRow titleRow = sheet.GetRow(skipIdx - 1);
            int columnSize = titleRow.LastCellNum;
            int rowSize = sheet.LastRowNum;
            List<string> xlscols = new List<string>(columnSize);
            for (int i = titleRow.FirstCellNum; i < columnSize; i++)
            {
                if (titleRow.GetCell(i) != null) //同理，没有数据的单元格都默认是null
                {
                    string cellData = titleRow.GetCell(i).ToString();
                    xlscols.Add(CVNameConverter.toViewName(cellData.Trim().ToLower()));
                }
                else
                {
                    xlscols.Add(null);
                }
            }
            ///////////////////////////////////////////
            int dgvColCount = DGVUtil.getTextualColumnCount(dgv);
            for (int i = skipIdx; i <= rowSize; ++i)
            {
                IRow row = sheet.GetRow(i);
                if (row == null) continue; //没有数据的行默认是null　
                string[] cellvalues = new string[dgvColCount];
                Dictionary<string, string> mapValues = new Dictionary<string, string>();
                
                ICell headCell = row.GetCell(row.FirstCellNum);
                if (headCell == null || headCell.ToString().Length == 0 || headCell.ToString().Equals("end!"))
                    break;
                for (int j = row.FirstCellNum; j < columnSize; j++)
                {
                    if (row.GetCell(j) != null) //同理，没有数据的单元格都默认是null
                    {
                        string cellData = row.GetCell(j).ToString().Trim();
                        if(ColumnMappingHolder.getDBOrder(xlscols[j])>-1)
                        {
                            mapValues[xlscols[j]]=cellData;
                            int idx = ColumnMappingHolder.getViewOrder(xlscols[j]);
                            if (idx > -1 && idx < dgvColCount)
                            {
                                cellvalues[idx] = cellData;
                            }
                            //else if(idx/ColumnMappingHolder.MaxMainViewCount!=1)
                            //    FileUtil.simpleLogLine("ERROR: ViewOrder overflow！ @ViewOrder=" + idx + "\n@dgvColCount=" + dgvColCount);
                        }
                    }
                }
                //store
                long nuid = CTDRecordDAM.mergeRecord(mapValues);
                if (nuid > 0)
                {
                    DGVAsyncUtil.syncAddRow(dgv, cellvalues);
                }
            }
        }

        private DataGridView dgv_data;
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
