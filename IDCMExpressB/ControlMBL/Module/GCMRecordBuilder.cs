using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.ControlMBL.Module
{
    class GCMRecordBuilder
    {
        
        #region 构造&析构
        public GCMRecordBuilder(TabControl tab)
        {
            this.recTab = tab;
        }
        ~GCMRecordBuilder()
        {
            Dispose();
        }
        public void Dispose()
        {
            recTab = null;
        }
        #endregion
        #region 实例对象保持部分
        private TabControl recTab = null;
        #endregion
    }
}
