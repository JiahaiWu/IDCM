using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.SimpleDAL.POO;
using IDCM.SimpleDAL.DBCP;
using System.Data.SQLite;
using Dapper;

namespace IDCM.SimpleDAL.DAM
{
    class DelayWorkNoteDAM : DAMBase
    {
        public static long save(DelayWorkNote ltwNote)
        {
            if (ltwNote.JobSerialInfo != null && ltwNote.JobSerialInfo.Length > 0)
            {
                if(ltwNote.Nid<1)
                    ltwNote.Nid= BaseInfoNoteDAM.nextSeqID();
                StringBuilder cmdBuilder = new StringBuilder();
                cmdBuilder.Append("insert or Replace into " + typeof(DelayWorkNote).Name);
                cmdBuilder.Append("(nid,jobType,jobSerialInfo,jobLevel,createTime,startCount,lastResult) values(");
                cmdBuilder.Append(ltwNote.Nid).Append(",'").Append(ltwNote.JobType).Append("','");
                cmdBuilder.Append(ltwNote.JobSerialInfo).Append("',").Append(ltwNote.JobLevel).Append(",").Append(ltwNote.CreateTime).Append(",");
                cmdBuilder.Append(ltwNote.StartCount).Append(",'").Append(ltwNote.LastResult).Append("')");
                SQLiteHelper.ExecuteNonQuery(ConnectStr, cmdBuilder.ToString());
                return ltwNote.Nid;
            }
            return -1;
        }
    }
}
