﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO;
using IDCM.OverallSC.ShareSync;
using System.Data;
using IDCM.SimpleDAL.DAM;
using IDCM.ServiceBL.Common.Converter;

namespace IDCM.ServiceBL.DataTransfer
{
    class JSONListExporter
    {
        public bool exportJSONList(string filepath, string cmdstr, int tcount)
        {
            try
            {
                StringBuilder strbuilder = new StringBuilder();
                int count = 0;
                using (FileStream fs = new FileStream(filepath, FileMode.Create))
                {
                    Dictionary<string, int> maps = ColumnMappingHolder.getCustomViewDBMapping();
                    //填写内容////////////////////
                    int offset = 0;
                    int stepLen = SysConstants.EXPORT_PAGING_COUNT;
                    while (offset < tcount)
                    {
                        int lcount = tcount - offset > stepLen ? stepLen : tcount - offset;
                        DataTable table = CTDRecordDAM.queryCTDRecordByHistSQL(cmdstr, lcount, offset);
                        foreach(DataRow row in table.Rows)
                        {
                            string jsonStr = convertToJsonStr(maps, row);
                            strbuilder.Append(jsonStr).Append("\n\r");
                            /////////////
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
                        offset += lcount;
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
        protected string convertToJsonStr(Dictionary<string, int> maps, DataRow row)
        {
            Dictionary<string, string> record = ConvertToRecDict(maps, row);
            return JsonConvert.SerializeObject(record);
        }
        protected Dictionary<string, string> ConvertToRecDict(Dictionary<string, int> maps,DataRow row)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach(KeyValuePair<string,int> kvpair in maps)
            {
                if (kvpair.Value > 0)
                {
                    string key = CVNameConverter.toViewName(kvpair.Key);
                    int k = kvpair.Value > SysConstants.Max_Attr_Count ? kvpair.Value - SysConstants.Max_Attr_Count : kvpair.Value;
                    dict[key] = row[k].ToString();
                }
            }
            return dict;
        }
    }
}
