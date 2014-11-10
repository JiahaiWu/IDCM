using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace IDCM.OverallSC.ShareSync
{
    class ConfigurationHelper
    {
        /// <summary>
        /// SetAppConfig
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="appValue"></param>
        /// <param name="configPath"></param>
        public static void SetAppConfig(string appKey, string appValue,string configPath=null)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(configPath == null ? System.Windows.Forms.Application.ExecutablePath + ".config" : configPath);
            var xNode = xDoc.SelectSingleNode("//appSettings");
            var xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");
            if (xElem != null) xElem.SetAttribute("value", appValue);
            else
            {
                var xNewElem = xDoc.CreateElement("add");
                xNewElem.SetAttribute("key", appKey);
                xNewElem.SetAttribute("value", appValue);
                xNode.AppendChild(xNewElem);
            }
            xDoc.Save(System.Windows.Forms.Application.ExecutablePath + ".config");
        }
        /// <summary>
        /// GetAppConfig
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="configPath"></param>
        /// <returns></returns>
        public static string GetAppConfig(string appKey, string configPath = null)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(configPath == null ? System.Windows.Forms.Application.ExecutablePath + ".config" : configPath);
            var xNode = xDoc.SelectSingleNode("//appSettings");
            var xElem = (XmlElement)xNode.SelectSingleNode("//add[@key='" + appKey + "']");
            if (xElem != null)
            {
                return xElem.Attributes["value"].Value;
            }
            return string.Empty;
        }
    }
}
