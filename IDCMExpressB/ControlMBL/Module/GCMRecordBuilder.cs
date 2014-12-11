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
        public GCMRecordBuilder(TreeView recordTree,ListView recordList)
        {
            this.recordTree = recordTree;
            this.recordList = recordList;
        }
        ~GCMRecordBuilder()
        {
            Dispose();
        }
        public void Dispose()
        {
            this.recordTree = null;
            this.recordList = null;
        }
        #endregion
        #region 实例对象保持部分
        private TreeView recordTree = null;
        private ListView recordList = null;
        #endregion
    }
}
