using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.OverallSC.ShareSync
{
    class ResponseCodeMap
    {
        private static Dictionary<int, string> codeMap = null;
        private static void init()
        {
            codeMap = new Dictionary<int, string>();
            codeMap.Add(121, "本地操作错误");
            codeMap.Add(220, "请求操作成功");
            codeMap.Add(420, "请求参数不合格");
            codeMap.Add(421, "用户不可用");
            codeMap.Add(425, "权限不允许");
            codeMap.Add(435, "不可用或禁止的请求功能");
            codeMap.Add(436, "服务器不支持的Web API版本，无法完成处理");
            codeMap.Add(520, "请求服务发生常规错误，中断无法完成的请求");
            codeMap.Add(521, "请求服务解析异常，无法完成请求");
            codeMap.Add(522, "服务器业务处理异常，无法完成请求");
            codeMap.Add(523, "依赖服务网络请求异常，无法完成请求");
        }
        public static bool checkStatus(int code)
        {
            if (codeMap == null)
            {
                init();
            }
            return 220 == code;
        }
        public static string getMsg(int code)
        {
            if (codeMap == null)
            {
                init();
            }
            String msg = null;
            codeMap.TryGetValue(code, out msg);
            return msg;
        }
    }
}
