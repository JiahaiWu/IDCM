using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.SimpleDAL.DBCP;
using IDCM.SimpleDAL.POO;
using Dapper;

namespace IDCM.SimpleDAL.DAM
{
    class AuthInfoDAM:DAMBase
    {
        public static AuthInfo queryLastAuthInfo()
        {
            AuthInfo lastAuthInfo = null;
            string cmd = "select * from " + typeof(AuthInfo).Name + " order by timestamp desc limit 1;";
            using (SQLiteConnPicker picker = new SQLiteConnPicker(ConnectStr))
            {
                lastAuthInfo = picker.getConnection().Query<AuthInfo>(cmd).FirstOrDefault<AuthInfo>();
            }
            return lastAuthInfo;
        }
        public static void updateLastAuthInfo(AuthInfo lastAuthInfo)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("insert Or Replace into ").Append(typeof(AuthInfo).Name).Append("(username,password,loginFlag,autoLogin,timestamp) values(");
            strBuilder.Append("'").Append(lastAuthInfo.Username).Append("',");
            strBuilder.Append("'").Append(lastAuthInfo.Password).Append("',");
            strBuilder.Append(lastAuthInfo.LoginFlag?1:0).Append(",");
            strBuilder.Append(lastAuthInfo.autoLogin ? 1 : 0).Append(",");
            strBuilder.Append(DateTime.Now.Ticks).Append(");");
            using (SQLiteConnPicker picker = new SQLiteConnPicker(ConnectStr))
            {
                picker.getConnection().Execute(strBuilder.ToString());
            }
        }
    }
}
