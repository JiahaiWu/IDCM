using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.ViewLL.Manager
{
    public abstract class RetainerA:ManagerI
    {

        public void setMaxToNormal()
        {
        }
        public void setToMaxmize(bool activeFront = false)
        {
        }
        public void setMdiParent(Form pForm)
        {
        }
        public virtual bool initView(bool activeShow = true)
        {
            return true;
        }
        public virtual bool IsDisposed()
        {
            return isDisposed;
        }
        public virtual void dispose()
        {
            isDisposed = true;
        }
        protected volatile bool isDisposed = false;
    }
}
