using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.ServiceBL.Common
{
    public class StrainListPage
    {
        public int pageSize { get; set; }
        public int pageNumber { get; set; }
        public string strainname { get; set; }
        public int strainnumber { get; set; }
        public int totalpage { get; set; }
        public string Jsessionid { get; set; }
        public Dictionary<string,string>[] list { get; set; }
    }
}
