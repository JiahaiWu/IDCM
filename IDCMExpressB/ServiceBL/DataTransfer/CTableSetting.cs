using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using IDCM.OverallSC.Commons;
using IDCM.ServiceBL.Common.Converter;
using System.Windows.Forms;
using IDCM.SimpleDAL.POO;
using IDCM.SimpleDAL.DAM;

namespace IDCM.ServiceBL.DataTransfer
{
    class CTableSetting
    {
        public static bool buildDefaultSetting()
        {
            try
            {
                string cTableDefpath = ConfigurationManager.AppSettings["CTableDef"];
                List<CustomTColDef> ctcds = getCustomTableDef(cTableDefpath);
                overwriteAllCustomTColDef(ctcds);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:\r\n" + ex.Message + "\n" + ex.StackTrace);
                Console.WriteLine("Exception:\r\n" + ex.Message + "\n" + ex.StackTrace);
            }
            return false;
        }
        /// <summary>
        /// 重写用户自定义数据表的字段集定义
        /// </summary>
        /// <param name="ctcds"></param>
        /// <returns></returns>
        public static bool overwriteAllCustomTColDef(List<CustomTColDef> ctcds)
        {
            if (ctcds != null)
            {
                List<CustomTColDef> ictcds = getEmbeddedTableDef();
                ctcds.AddRange(ictcds);
                CustomTColDefDAM.rebuildCustomTColDef();
                CustomTColDefDAM.save(ctcds.ToArray());
                return true;
            }
            return false;
        }
        public static Dictionary<string, List<CustomTColDef>> getTableTemplateDef(string settingPath)
        {
            Dictionary<string, List<CustomTColDef>> templDict = new Dictionary<string, List<CustomTColDef>>();
            if (File.Exists(settingPath))
            {
                List<CustomTColDef> ctcds = new List<CustomTColDef>();
                string[] lines = FileUtil.readAsUTF8Text(settingPath).Split(new char[] { '\n', '\r' });
                foreach (string line in lines)
                {
                    if (line.Length < 1 || line.StartsWith("#"))
                        continue;
                    if (line.StartsWith("[") && line.TrimEnd().EndsWith("]"))
                    {
                        ctcds = new List<CustomTColDef>();
                        string groupName=line.Substring(1,line.LastIndexOf(']')-1);
                        templDict[groupName] = ctcds;
                        continue;
                    }
                    if (line.StartsWith(">>Def"))
                    {
                        string ver = line.Substring(9).Trim();
                        ctcds.Clear();
                        continue;
                    }
                    CustomTColDef ctcd = formatSettingLine(line);
                    if (ctcd != null)
                        ctcds.Add(ctcd);
                }
                return templDict;
            }
            return null;
        }
        public static List<CustomTColDef> getCustomTableDef(string settingPath)
        {
            List<CustomTColDef> ctcds = new List<CustomTColDef>();
            if (File.Exists(settingPath))
            {
                string[] lines = FileUtil.readAsUTF8Text(settingPath).Split(new char[] { '\n', '\r' });
                foreach (string line in lines)
                {
                    if (line.Length < 1 || line.StartsWith("#"))
                        continue;
                    if (line.StartsWith("[") && line.TrimEnd().EndsWith("]"))
                        continue;
                    if (line.StartsWith(">>Def"))
                    {
                        string ver = line.Substring(9).Trim();
                        ctcds.Clear();
                        continue;
                    }
                    CustomTColDef ctcd = formatSettingLine(line);
                    if (ctcd != null)
                        ctcds.Add(ctcd);
                }
                return ctcds;
            }
            else
            {
                log.Fatal("The setting file note exist! @Path=" + settingPath);
                MessageBox.Show("The setting file note exist! @Path=" + settingPath);
            }
            return null;
        }
        public static CustomTColDef formatSettingLine(string line)
        {
            string[] vals = line.Split(new char[] { ',' });
            if (vals.Length > 0)
            {
                CustomTColDef ctcd = new CustomTColDef();
                ctcd.Attr = CVNameConverter.toViewName(vals[0]);
                ctcd.AttrType = vals.Length > 1 ? vals[1] : AttrTypeConverter.IDCM_String;
                ctcd.IsRequire = vals.Length > 2 ? Convert.ToBoolean(vals[2]) : false;
                ctcd.IsUnique = vals.Length > 3 ? Convert.ToBoolean(vals[3]) : false;
                ctcd.Restrict = vals.Length > 4 ? vals[4] : null;
                ctcd.DefaultVal = vals.Length > 5 ? vals[5] : null;
                ctcd.Comments = vals.Length > 6 ? vals[6] : null;
                return ctcd;
            }
            return null;
        }
        public static List<CustomTColDef> getEmbeddedTableDef()
        {
            List<CustomTColDef> ctcds = new List<CustomTColDef>();
            CustomTColDef rid = new CustomTColDef();
            rid.Attr = CTDRecordDAM.CTD_RID;
            rid.AttrType = AttrTypeConverter.IDCM_Integer;
            rid.IsRequire = true;
            rid.IsUnique = true;
            rid.DefaultVal = "-1";
            rid.IsInter = true;
            rid.Comments = "CTDRecordDA.CTD_RID";
            ctcds.Add(rid);
            ///////////////////////////////////
            CustomTColDef plid = new CustomTColDef();
            plid.Attr = CTDRecordDAM.CTD_PLID;
            plid.AttrType = AttrTypeConverter.IDCM_Integer;
            plid.IsRequire = true;
            plid.IsUnique = false;
            plid.IsInter = true;
            plid.DefaultVal = LibraryNodeDAM.REC_ALL.ToString();
            plid.Comments = "CTDRecordDA.CTD_PLID";
            ctcds.Add(plid);
            ///////////////////////////////////////
            CustomTColDef lid = new CustomTColDef();
            lid.Attr = CTDRecordDAM.CTD_LID;
            lid.AttrType = AttrTypeConverter.IDCM_Integer;
            lid.IsRequire = true;
            lid.IsUnique = false;
            lid.IsInter = true;
            lid.DefaultVal = LibraryNodeDAM.REC_ALL.ToString();
            lid.Comments = "CTDRecordDA.CTD_LID";
            ctcds.Add(lid);

            return ctcds;
        }

        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
