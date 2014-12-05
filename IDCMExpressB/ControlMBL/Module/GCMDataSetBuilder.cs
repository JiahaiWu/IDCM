using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.ControlMBL.Module
{
    class GCMDataSetBuilder
    {
        #region 构造&析构
        public GCMDataSetBuilder(DataGridView dgv)
        {
            this.itemDGV = dgv;
        }
        ~GCMDataSetBuilder()
        {
            Dispose();
        }
        public void Dispose()
        {
            itemDGV = null;
        }
        #endregion
        #region 实例对象保持部分
        private DataGridView itemDGV=null;
        #endregion
    }
}
