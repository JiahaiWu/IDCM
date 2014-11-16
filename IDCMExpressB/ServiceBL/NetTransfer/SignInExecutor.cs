using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.SimpleDAL.POO;
using System.Net;
using System.IO;
using System.Configuration;
using Newtonsoft.Json;

namespace IDCM.ServiceBL.NetTransfer
{
    class SignInExecutor
    {
        public static AuthInfo SignIn(string username,string password,int timeout=10000,bool autoLogin=true)
        {
            if (username != null && password != null)
            {
                string signInUri = ConfigurationManager.AppSettings["SignInUri"];
                string url = string.Format(signInUri, new string[] { username, password });
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
                AuthInfo auth= parserToAuthInfo(resStr);
                if (auth != null)
                {
                    auth.Username = username;
                    auth.Password = password;
                    auth.autoLogin = autoLogin;
                }
                return auth;
            }
            return null;
        }

        protected static AuthInfo parserToAuthInfo(string jsonStr)
        {
            AuthInfo auth = new AuthInfo();
            AInfo ai = JsonConvert.DeserializeObject<AInfo>(jsonStr);
            auth.LoginFlag = Boolean.Parse(ai.loginFlag);
            auth.Jsessionid = ai.jsessionid;
            return auth;
        }
        private class AInfo
        {
            public string username { get; set; }
            public string loginFlag { get; set; }
            public string jsessionid { get; set; }
        }
    }
}
