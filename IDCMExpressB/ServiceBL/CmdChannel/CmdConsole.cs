using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using IDCM.ServiceBL.Common;
using System.Windows.Forms;
using IDCM.ServiceBL.ServBuf;
using IDCM.ServiceBL.Handle;

namespace IDCM.ServiceBL.CmdChannel
{
    class CmdConsole
    {

        /// <summary>
        /// 向指定的对象实例和关联方法传递参数，并请求执行
        /// </summary>
        /// <param name="servInstance"></param>
        /// <param name="option"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool call(AbsHandler servInstance,CmdReqOption option=CmdReqOption.L, MethodInfo method=null, Object[] args=null)
        {
            switch (option)
            {
                case CmdReqOption.L:
                    if(method!=null)
                        method.Invoke(servInstance, args);
                    BGWorkerPool.pushHandler(servInstance);
                    break;
                case CmdReqOption.N:
                    break;
                default:
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 向指定的对象实例和关联方法传递参数，并中断执行
        /// </summary>
        /// <param name="servInstance"></param>
        /// <param name="option"></param>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool kill(AbsHandler servInstance, CmdReqOption option = CmdReqOption.L, MethodInfo method = null, Object[] args = null)
        {
            switch (option)
            {
                case CmdReqOption.L:
                    if (method != null)
                        method.Invoke(servInstance, args);
                    BGWorkerPool.abortHandlerByType(servInstance.GetType());
                    break;
                case CmdReqOption.N:
                    break;
                default:
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 请求操作命令的参数选项定义
        /// </summary>
        public enum CmdReqOption{
            /// <summary>
            /// local implement
            /// </summary>
            L=0,
            /// <summary>
            /// Network implement
            /// </summary>
            N=1};
    }
}
