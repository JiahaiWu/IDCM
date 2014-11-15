using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IDCM.ViewLL.Manager;
using IDCM.ServiceBL;
using IDCM.SimpleDAL.POO;
using IDCM.SimpleDAL.DAM;
using IDCM.AppContext;

namespace IDCM.ViewLL.Win
{
    public partial class StartForm : Form
    {
        public StartForm(string workspacePath=null)
        {
            InitializeComponent();
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
        }

        private void StartForm_Shown(object sender, EventArgs e)
        {
            //检查用户工作空间有效性
            if (WorkSpaceHolder.verifyForLoad(preparepath))
            {
                if (CustomTColDefDAM.checkTableSetting())
                    this.DialogResult = DialogResult.OK;
                else
                    this.DialogResult = DialogResult.No;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
            this.Close();
        }
        private string preparepath=null;
    }
}
