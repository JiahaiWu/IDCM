using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;
using IDCM.OverallSC.Commons;
using IDCM.ViewLL.Manager;
using IDCM.SimpleDAL.DBCP;
using IDCM.SimpleDAL.DAM;
using IDCM.SimpleDAL.POO;

namespace IDCM.AddedEI.Debug
{
    class DBReseter:DAMBase
    {
        public static void resetDataSource(object sender, EventArgs e)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            //clear LibraryNode
            SQLiteHelper.ExecuteNonQuery(ConnectStr, "delete from LibraryNode;"
                , "delete from CustomTColDef;"
                ,"delete from CTDRecord;"
                ,"delete from CustomTColMap;");
            Console.WriteLine("reset DataSource end!");
            System.Environment.Exit(0);
        }
    }
}
