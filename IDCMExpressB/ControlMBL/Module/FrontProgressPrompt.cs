using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.ViewLL.Win;

namespace IDCM.ControlMBL.Module
{
    class FrontProgressPrompt
    {
        public static void startFrontProgress()
        {
            Nest.processDlg.ShowDialog();
        }
        public static void endFrontProgress()
        {
            Nest.processDlg.Close();
        }
        private static class Nest
        {
            internal static ProcessDlg processDlg = new ProcessDlg();
        }
    }
}
