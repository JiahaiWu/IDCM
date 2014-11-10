using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.SimpleDAL.POO;
using IDCM.ServiceBL.DataTransfer;
using IDCM.SimpleDAL.DAM;

namespace IDCM.ServiceBL.ServProcessor
{
    class TemplateUpdater
    {
        public bool doUpdateProcess(LinkedList<CustomTColDef> newCtcds)
        {
            LinkedList<CustomTColDef> curCtcds = CustomTColDefDAM.loadCustomAll();
            Dictionary<CustomTColDef, bool> updateDict = new Dictionary<CustomTColDef, bool>();
            if (structChanged(curCtcds, newCtcds, updateDict))
            {
                //备份旧有的数据记录表
                string bakSuffix = CustomTColMapDAM.renameCustomTColDefAll();
                //重写自定义数据表
                CTableSetting.overwriteAllCustomTColDef(newCtcds.ToList());
                //重建数据记录表
                CustomTColMapDAM.buildCustomTable();
                //启用数据转录事务
                noteTranscribeCTDRecord(CTDRecordDAM.table_name + bakSuffix, CTDRecordDAM.table_name, typeof(CustomTColDef).Name + bakSuffix, typeof(CustomTColDef).Name);
            }
            else
            {
                //添加需要追加的字段
                foreach (KeyValuePair<CustomTColDef, bool> kvpair in updateDict)
                {
                    if (kvpair.Value)
                        CustomTColDefDAM.appendCustomTColDef(kvpair.Key);
                }
                //更新字段属性定义
                foreach (KeyValuePair<CustomTColDef, bool> kvpair in updateDict)
                {
                    if (!kvpair.Value)
                        CustomTColDefDAM.updateCustomTColDef(kvpair.Key);
                }
            }
            return true;
        }
        /// <summary>
        /// 验证对象结构是否发生属性变更
        /// </summary>
        /// <param name="curCtcds"></param>
        /// <param name="newCtcds"></param>
        /// <returns></returns>
        protected bool structChanged(LinkedList<CustomTColDef> curCtcds, LinkedList<CustomTColDef> newCtcds, Dictionary<CustomTColDef, bool> updateDict)
        {
            HashSet<string> ctcdcodes = new HashSet<string>();
            Dictionary<string, CustomTColDef> attrs = new Dictionary<string, CustomTColDef>();
            foreach (CustomTColDef ctcd in curCtcds)
            {
                attrs.Add(ctcd.Attr, ctcd);
                ctcdcodes.Add(ctcd.Attr + "." + ctcd.AttrType + "." + ctcd.DefaultVal + "." + ctcd.IsUnique);
            }
            foreach (CustomTColDef ctcd in newCtcds)
            {
                string code = ctcd.Attr + "." + ctcd.AttrType + "." + ctcd.DefaultVal + "." + ctcd.IsUnique;
                if (!ctcdcodes.Contains(code))
                {
                    if (attrs.Keys.Contains(ctcd.Attr))
                    {
                        continue;
                    }
                    else
                    {
                        //for append field
                        updateDict[ctcd] = true;
                    }
                }
                else if (!checkSameDef(attrs[ctcd.Attr], ctcd))
                {
                    //for update field
                    updateDict[ctcd] = false;
                }
            }
            int appendCount = 0;
            foreach (bool st in updateDict.Values)
            {
                if (st)
                    ++appendCount;
            }
            bool res = newCtcds.Count - appendCount != curCtcds.Count;
            return res;
        }
        /// <summary>
        /// 数据转录事务存储
        /// </summary>
        /// <param name="table_source"></param>
        /// <param name="table_target"></param>
        /// <param name="source_config"></param>
        /// <param name="target_config"></param>
        protected void noteTranscribeCTDRecord(string table_source, string table_target, string source_config = null, string target_config = null)
        {
            DelayWorkNote ltwNote = new DelayWorkNote("TranscribeCTDRecord");
            ltwNote.JobSerialInfo = table_source + ";" + table_target + ";" + source_config + ";" + target_config;
            DelayWorkNoteDAM.save(ltwNote);
        }
        protected bool checkSameDef(CustomTColDef octcd, CustomTColDef ctcd)
        {
            string ocode = ctcd.Comments + "." + ctcd.DefaultVal + "." + ctcd.Corder + "." + ctcd.Restrict + "." + ctcd.Attr + "." + ctcd.AttrType + "." + ctcd.DefaultVal + "." + ctcd.IsUnique;
            string code = ctcd.Comments + "." + ctcd.DefaultVal + "." + ctcd.Corder + "." + ctcd.Restrict + "." + ctcd.Attr + "." + ctcd.AttrType + "." + ctcd.DefaultVal + "." + ctcd.IsUnique;
            return ocode.Equals(code);
        }
    }
}
