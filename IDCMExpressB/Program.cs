using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using IDCM.AppContext;

namespace IDCM
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string ws=null;
            if (args.Length > 0)
            {
                for (int i=0;i<args.Length-1;i++)
                {
                    if (args[i].Equals("-ws"))
                    {
                        ws = args[i + 1].Trim(new char[]{'"'});
                    }
                }
            }
            Application.Run(new IDCMAppContext(ws));
        }
    }
}
