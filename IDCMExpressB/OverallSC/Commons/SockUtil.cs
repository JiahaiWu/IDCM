using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace IDCM.OverallSC.Commons
{
    class SockUtil
    {
        private static void CallBackMethod(IAsyncResult asyncresult)
        {
            TimeoutObject.Set();
        }
        /// <summary>
        /// 连接使用tcp协议的服务端
        /// </summary>
        /// <param name="ip">服务端的ip</param>
        /// <param name="port">服务端的端口号</param>
        /// <returns></returns>
        public static Socket ConnectServer(string ip, int port)
        {
            Socket s = null;
            try
            {
                lock (TimeoutObject)
                {
                    TimeoutObject.Reset();
                    IPAddress ipAddress = IPAddress.Parse(ip);
                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
                    s = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    s.BeginConnect(ipEndPoint, CallBackMethod, s);
                    if (TimeoutObject.WaitOne(5000, false) && s.Connected == true)
                    {
                        return s;
                    }
                    else
                        s = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message + "\n" + ex.StackTrace);
            }
            return null;
        }
        /// <summary>
        /// 向远程主机发送数据
        /// </summary>
        /// <param name="socket">连接到远程主机的socket</param>
        /// <param name="buffer">待发送数据</param>
        /// <param name="outTime">发送超时时长，单位是秒(为-1时，将一直等待直到有数据需要发送)</param>
        /// <returns>0:发送成功；-1:超时；-2:出现错误；-3:出现异常</returns>
        public static int SendData(Socket socket, byte[] buffer, int outTime)
        {
            if (socket == null || socket.Connected == false)
            {
                throw new ArgumentException("参数socket为null，或者未连接到远程计算机");
            }
            if (buffer == null || buffer.Length == 0)
            {
                throw new ArgumentException("参数buffer为null ,或者长度为 0");
            }

            int flag = 0;
            try
            {
                int left = buffer.Length;
                int sndLen = 0;
                int hasSend = 0;

                while (true)
                {
                    if ((socket.Poll(outTime * 1000, SelectMode.SelectWrite) == true))
                    {
                        // 收集了足够多的传出数据后开始发送
                        sndLen = socket.Send(buffer, hasSend, left, SocketFlags.None);
                        left -= sndLen;
                        hasSend += sndLen;

                        // 数据已经全部发送
                        if (left == 0)
                        {
                            flag = 0;
                            break;
                        }
                        else
                        {
                            // 数据部分已经被发送
                            if (sndLen > 0)
                            {
                                continue;
                            }
                            else // 发送数据发生错误
                            {
                                flag = -2;
                                break;
                            }
                        }
                    }
                    else // 超时退出
                    {
                        flag = -1;
                        break;
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Error: " + ex.Message + "\n" + ex.StackTrace);
                flag = -3;
            }
            return flag;
        }
        /// <summary>
        /// 向远程主机发送数据
        /// </summary>
        /// <param name="socket">连接到远程主机的socket</param>
        /// <param name="buffer">待发送的字符串</param>
        /// <param name="outTime">发送数据的超时时间，单位是秒(为-1时，将一直等待直到有数据需要发送)</param>
        /// <returns>0:发送数据成功；-1:超时；-2:错误；-3:异常</returns>
        public static int SendData(Socket socket, string buffer, int outTime)
        {
            if (buffer == null || buffer.Length == 0)
            {
                throw new ArgumentException("buffer为null或则长度为0.");
            }
            return SendData(socket, System.Text.Encoding.UTF8.GetBytes(buffer), outTime);
        }
        /// <summary>
        /// 接收远程主机发送的数据
        /// </summary>
        /// <param name="socket">要接收数据且已经连接到远程主机的</param>
        /// <param name="buffer">接收数据的缓冲区(需要接收的数据的长度，由 buffer 的长度决定)</param>
        /// <param name="outTime">接收数据的超时时间，单位秒(指定为-1时，将一直等待直到有数据需要接收)</param>
        /// <returns></returns>
        public static int RecvData(Socket socket, byte[] buffer, int outTime)
        {
            if (socket == null || socket.Connected == false)
            {
                throw new ArgumentException("socket为null，或者未连接到远程计算机");
            }
            if (buffer == null || buffer.Length == 0)
            {
                throw new ArgumentException("buffer为null ,或者长度为 0");
            }

            buffer.Initialize();
            int left = buffer.Length;
            int curRcv = 0;
            int hasRecv = 0;
            int flag = 0;

            try
            {
                while (true)
                {
                    if (socket.Poll(outTime * 1000, SelectMode.SelectRead) == true)
                    {
                        // 已经有数据等待接收
                        curRcv = socket.Receive(buffer, hasRecv, left, SocketFlags.None);
                        left -= curRcv;
                        hasRecv += curRcv;

                        // 数据已经全部接收 
                        if (left == 0)
                        {
                            flag = 0;
                            break;
                        }
                        else
                        {
                            // 数据已经部分接收
                            if (curRcv > 0)
                            {
                                continue;
                            }
                            else  // 出现错误
                            {
                                flag = -2;
                                break;
                            }
                        }
                    }
                    else // 超时退出
                    {
                        flag = -1;
                        break;
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Error: " + ex.Message + "\n" + ex.StackTrace);
                flag = -3;
            }
            return flag;
        }
        private static ManualResetEvent TimeoutObject = new ManualResetEvent(false);
    }
}
