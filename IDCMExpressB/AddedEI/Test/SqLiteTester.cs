using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Data.SQLite;
using System.Data.SQLite.Generic;
using System.Data;
using IDCM.SimpleDAL.DBCP;
using IDCM.SimpleDAL.DAM;
using System.Data.Common;

namespace IDCM.AddedEI.Test
{
    class SqLiteTester
    {
        public static void doTest(object sender, EventArgs e)
        {
            string datasource = "F:\\sqltest.db";
            if (!File.Exists(datasource))
            {
                SQLiteConnection.CreateFile(datasource);
                //连接数据库
                SQLiteConnection conn = new SQLiteConnection();
                SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
                connstr.DataSource = datasource;
                connstr.Password = "admin";//设置密码，SQLite ADO.NET实现了数据库密码保护
                conn.ConnectionString = connstr.ToString();
                conn.Open();
                //创建表
                SQLiteCommand cmd = new SQLiteCommand();
                string sql = "CREATE TABLE test(username varchar(20),password varchar(20))";
                cmd.CommandText = sql;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();
                //插入数据
                sql = "INSERT INTO test VALUES('a','b')";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                sql = "INSERT INTO test VALUES('c','d')";
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            
            Thread ta = new Thread(new ParameterizedThreadStart(testBase));
            ta.Name = "ta";
            Thread tb = new Thread(new ParameterizedThreadStart(testBase));
            tb.Name = "tb";
            ta.Start(datasource);
            tb.Start(datasource);
        }

        public static void testBase(object datasource)
        {
            //连接数据库
            SQLiteConnectionStringBuilder sqlCSB = new SQLiteConnectionStringBuilder();
            sqlCSB.SyncMode = SynchronizationModes.Full;
            sqlCSB.DataSource = datasource.ToString();
            sqlCSB.Password = "admin";//设置密码，SQLite ADO.NET实现了数据库密码保护
            string cmd = "SELECT * FROM test";
            
            for (int i = 0; i < 20; i++)
            {
                DataTable table = SQLiteHelper.ExecuteDataTable(sqlCSB.ToString(), cmd);
                StringBuilder sb = new StringBuilder();
                DataRowCollection drc = table.Rows;
                for (int k = 0; k < drc.Count; k++)
                {
                    sb.Append("username:").Append(drc[k][0]).Append("\n")
                .Append("password:").Append(drc[k][1]).Append("\n");
                }
                Console.WriteLine(Thread.CurrentThread.Name + "@"+i+"#" + sb.ToString());
            }
        }
    }
}
