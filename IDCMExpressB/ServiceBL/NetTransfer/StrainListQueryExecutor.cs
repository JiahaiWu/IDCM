using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using IDCM.ViewLL.Manager;
using IDCM.ServiceBL.Common;
using IDCM.SimpleDAL.POO;

namespace IDCM.ServiceBL.NetTransfer
{
    class StrainListQueryExecutor
    {

        public static StrainListPage strainListQuery(int currentPage, string strainnumber = "", string strainname = "", int timeout = 10000)
        {
            AuthInfo authInfo = AuthenticationRetainer.getInstance().getLoginAuthInfo();
            if (authInfo != null && currentPage != null)
            {
                string signInUri = ConfigurationManager.AppSettings["StrainListUri"];
                string url = string.Format(signInUri, new object[] { authInfo.Jsessionid, currentPage, strainnumber, strainname });
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "*/*";
                request.KeepAlive = true;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1)";
                request.Timeout = timeout;
                request.ReadWriteTimeout = timeout;
                Stream myRequestStream = request.GetRequestStream();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("UTF-8"));
                string resStr = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                StrainListPage slp = parserToListPageInfo(resStr);
                if (slp != null)
                {
                    slp.Jsessionid = authInfo.Jsessionid;
                }
                return slp;
            }
            return null;
        }

        protected static StrainListPage parserToListPageInfo(string jsonStr)
        {
            StrainListPage slp = JsonConvert.DeserializeObject<StrainListPage>(jsonStr);
            return slp;
        }
    }
}
