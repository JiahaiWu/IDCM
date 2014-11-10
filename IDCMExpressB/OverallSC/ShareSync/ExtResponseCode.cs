using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IDCM.OverallSC.ShareSync
{
    class ExtResponseCode
    {
        /**
         * 1xx 本地错误 
         */
        public const int WORKERROR = 121;
        /**
	     * 2xx 请求成功
	     */
        /**请求操作成功*/
        public const int OK = 220;
        /**
         * 4xx 客户端错误
         */
        /**请求参数不合格*/
        public const int BADREQUEST = 420;
        /**用户不可用*/
        public const int DUPLICATEINFO = 421;
        /**权限不允许*/
        public const int NOPERMISSION = 425;
        /**不可用或禁止的请求功能*/
        public const int UNAVAILABLEREQUEST = 435;
        /**服务器不支持的Web API版本，无法完成处理*/
        public const int ILLEGALAPIVERSION = 436;
        /**
	     * 服务器错误
	     */
        /**请求服务发生常规错误，中断无法完成的请求*/
        public const int SERVICEWORKERROR = 520;
        /**请求服务解析异常，无法完成请求*/
        public const int SERVICEPARSEEXCEPTION = 521;
        /**服务器业务处理异常，无法完成请求*/
        public const int SERVICEWORKEXCEPTION = 522;
        /**依赖服务网络请求异常，无法完成请求*/
        public const int SERVICENETWORKERROR = 523;
    }
}
