using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDCM.SimpleDAL.POO;
using IDCM.SimpleDAL.DAM;

namespace IDCM.SimpleDAL.POO
{
    class LibraryNode
    {
        public LibraryNode()
        {
        }
        public LibraryNode(string _name,string _type="Group",string _desc="",int _order=0)
        {
            this.name = _name;
            this.type = _type;
            this.desc = _desc;
            this.lorder = _order;
            this.pid = LibraryNodeDAM.REC_ALL;
            this.pid = LibraryNodeDAM.REC_UNFILED;
            this.updateTime = DateTime.Now.ToFileTimeUtc();
        }
        private long lid;

        public long Lid
        {
            get { return lid; }
            set { lid = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private long pid;

        public long Pid
        {
            get { return pid; }
            set { pid = value; }
        }

        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        private int lorder;

        public int Lorder
        {
            get { return lorder; }
            set { lorder = value; }
        }
        
        private string desc;

        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }
        private long updateTime;

        public long UpdateTime
        {
            get { return updateTime; }
            set { updateTime = value; }
        }

    }
}
