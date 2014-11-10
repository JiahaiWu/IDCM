using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.ServiceBL.Common.Converter
{
    public sealed class CVNameConverter
    {
        public static bool isViewWrapName(string attr)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(attr!= null);
#endif
            return (attr.StartsWith("[") && attr.StartsWith("]"));
        }
        /// <summary>
        /// toViewName
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string toViewName(string attr)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(attr!= null);
#endif
            if (attr.StartsWith("[") && attr.EndsWith("]"))
                return attr;
            return "[" + attr + "]";
        }
        /// <summary>
        /// toDBName
        /// </summary>
        /// <param name="attr"></param>
        /// <returns></returns>
        public static string toDBName(string attr)
        {
#if DEBUG
            System.Diagnostics.Debug.Assert(attr!= null);
#endif
            if (attr.StartsWith("[") && attr.EndsWith("]"))
                return attr.Substring(1, attr.Length - 2);
            return attr;
        }
    }
}
