using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace IDCM.ServiceBL.Common.Converter
{
    class StrViewConverter
    {
        /// <summary>
        /// 将目标字符串尾部填充特定字符，返回为填充为固定长度的字符串
        /// </summary>
        /// <param name="rawStr"></param>
        /// <param name="len"></param>
        /// <param name="appendChar"></param>
        /// <returns></returns>
        public static string toFixLenString(string rawStr,int len,char appendChar=' ')
        {
            string str = rawStr;
            while (str.Length < len)
            {
                str += appendChar;
            }
            return str;
        }
        /// <summary>
        /// 将目标字符串左右拼接，中间填充特定字符，返回为固定长度的字符串
        /// </summary>
        /// <param name="rawStr"></param>
        /// <param name="len"></param>
        /// <param name="appStr"></param>
        /// <param name="appendChar"></param>
        /// <returns></returns>
        public static string extendFixLenString(string rawStr, int len,string appStr, char appendChar = ' ')
        {
            string str = rawStr;
            int llen = len - appStr.Length;
            while (str.Length < llen)
            {
                str += appendChar;
            }
            return str+appStr;
        }

        /// <summary>
        /// 将目标字符串尾部填充特定字符，返回为填充为固定长宽度的字符串
        /// </summary>
        /// <param name="rawStr"></param>
        /// <param name="len"></param>
        /// <param name="appendChar"></param>
        /// <returns></returns>
        public static string toFixWidthString(string rawStr, float len,Font font, char appendChar = ' ')
        {
            string str = rawStr;
            while (TextRenderer.MeasureText(str, font).Width < len)
            {
                str += appendChar;
            }
            return str;
        }
        /// <summary>
        /// 将目标字符串左右拼接，中间填充特定字符，返回为固定宽度的字符串
        /// </summary>
        /// <param name="rawStr"></param>
        /// <param name="len"></param>
        /// <param name="appStr"></param>
        /// <param name="appendChar"></param>
        /// <returns></returns>
        public static string extendFixWidthString(string rawStr, float len, string appStr,Font font, char appendChar = ' ')
        {
            string str = rawStr;
            float llen = len - TextRenderer.MeasureText(appStr, font).Width;
            while (TextRenderer.MeasureText(str, font).Width < llen)
            {
                str += appendChar;
            }
            return str + appStr;
        }
    }
}
