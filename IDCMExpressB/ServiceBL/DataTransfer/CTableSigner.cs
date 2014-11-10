using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.ServiceBL.DataTransfer;
using IDCM.OverallSC.Commons;
using IDCM.SimpleDAL.POO;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using System.Reflection;
using IDCM.OverallSC.Commons.Generator;
using IDCM.SimpleDAL.DAM;
using IDCM.ViewLL.Manager;
namespace IDCM.ServiceBL.DataTransfer
{
    /// <summary>
    /// 用于用户定义表的登记访问管理器
    /// </summary>
    class CTableSigner
    {

        protected static bool getNeedUpdateCTCD(LinkedList<CustomTColDef> curCtcds, LinkedList<CustomTColDef> newCtcds)
        {
            Dictionary<string, KeyValuePair<CustomTColDef, bool>> ctcdDict = new Dictionary<string, KeyValuePair<CustomTColDef, bool>>();
            HashSet<string> attrs = new HashSet<string>();
            foreach(CustomTColDef ctcd in curCtcds)
            {
                attrs.Add(ctcd.Attr);
            }
            foreach (CustomTColDef ctcd in newCtcds)
            {
                string code=ctcd.Attr + "." + ctcd.AttrType + "." + ctcd.DefaultVal + "." + ctcd.IsUnique;
                if (attrs.Contains(ctcd.Attr))
                {
                    return true;
                }
            }
            return false;
        }
        
    }
}
