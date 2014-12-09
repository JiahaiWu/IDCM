﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;
using IDCM.ServiceBL.Common;
using IDCM.SimpleDAL.POO;
using IDCM.ViewLL.Manager;

namespace IDCM.ServiceBL.NetTransfer
{
    class StrainViewQueryExecutor
    {

        public static StrainView strainListQuery(string id, int timeout = 10000)
        {
            AuthInfo authInfo = AuthenticationRetainer.getInstance().getLoginAuthInfo();
            if (authInfo != null && id != null)
            {
                string signInUri = ConfigurationManager.AppSettings["StrainListUri"];
                string url = string.Format(signInUri, new string[] { authInfo.Jsessionid,id});
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                log.Info("StrainViewQueryExecutor Request Url=" + url);
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
                log.Info("StrainViewQueryExecutor Response=" + resStr);
                StrainView sv = parserToListPageInfo(resStr);
                if (sv != null)
                {
                    sv.Jsessionid = authInfo.Jsessionid;
                }
                return sv;
            }
            return null;
        }

        protected static StrainView parserToListPageInfo(string jsonStr)
        {
            StrainView sv = JsonConvert.DeserializeObject<StrainView>(jsonStr);
            return sv;
        }
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();
    }
}
