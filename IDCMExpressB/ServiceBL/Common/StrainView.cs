using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.ServiceBL.Common
{
    public class StrainView:Dictionary<string,string>
    {
        public string Jsessionid { get; set; }
    }
}
