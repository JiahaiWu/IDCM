using IDCM.AppContext;
using IDCM.ServiceBL.ServBuf;
using IDCM.ViewLL.Win;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace IDCM.ViewLL.Manager
{
    class StackInfoManager : RetainerA
    {
        private volatile StackInfoView stackView = null;
        private Timer timer = new Timer();
        /// <summary>
        /// 任务信息构造方法
        /// </summary>
        public StackInfoManager() {
            stackView = new StackInfoView();
            stackView.Disposed += disposeEvent;
            stackView.setManager(this);
            LongTermHandleNoter.note(stackView);

            //计时器每隔三秒就会重新调用loadStackDate()方法加载后台任务信息，重绘dataGridView，这样会不会太耗费资源了？
            timer.Interval = 3000;
            timer.Tick += loadStackDataEvent;
                
        }

        private void loadStackDataEvent(object sender, EventArgs e){
            loadStackData();
        }
        /// <summary>
        /// 加载后台任务信息
        /// </summary>
        public void loadStackData()
        {
            if (stackView == null) return;
            DataTable dataTable = new DataTable();
            DataColumn dataColumn = new DataColumn("Name",typeof(string));
            DataColumn dataColumn1 = new DataColumn("RunningState", typeof(string));
            DataColumn dataColumn2 = new DataColumn("StartTime", typeof(string));
            DataColumn dataColumn3 = new DataColumn("RunningTime", typeof(string));
            dataTable.Columns.Add(dataColumn);
            dataTable.Columns.Add(dataColumn1);
            dataTable.Columns.Add(dataColumn2);
            dataTable.Columns.Add(dataColumn3);

            Dictionary<string, string> dictionary = LongTermHandleNoter.getStackDetails();
            Dictionary<string, KeyValuePair<string,string>> dictionary2 = BGWorkerPool.getStackInfo();

            
            foreach (KeyValuePair<string, string> kvp in dictionary)
            {
                DataRow dr = dataTable.NewRow();
                //sb.Append(kvp.Key).Append(":").Append(kvp.Value).Append("\n");
                dr[0] = kvp.Key;
                dr[1] = kvp.Value;
                dr[2] = "";
                dataTable.Rows.Add(dr);
            }

            foreach(KeyValuePair<string,KeyValuePair<string,string>> kvp in dictionary2){
                DataRow dr = dataTable.NewRow();
                dr[0] = kvp.Key;
                dr[1] = kvp.Value.Key;
                dr[2] = kvp.Value.Value;
                
                DateTime date = Convert.ToDateTime(kvp.Value.Value);

                dr[3] = date.Subtract(DateTime.Now).ToString(@"hh\:mm\:ss");
                dataTable.Rows.Add(dr);
            }
            
            stackView.loadData(dataTable);
            
        }

        public override bool initView(bool activeShow = true)
        {
            if (!timer.Enabled) {
                timer.Start();
            }

            if (stackView == null || stackView.IsDisposed) {
                stackView = new StackInfoView();
                stackView.setManager(this);
                LongTermHandleNoter.note(stackView);
            }
            loadStackData();
            if (activeShow)
            {
                stackView.Show();
                stackView.Activate();
                stackView.WindowState = FormWindowState.Normal;
            }
            else
            {
                stackView.Hide();
            }
            return true;
        }

        public override void dispose()
        {

            stackView.Dispose();
            
            _isDisposed = true;
        }


        private void disposeEvent(object sender, EventArgs e) {
            timer.Stop();//如果窗体销毁，计时器将停止
        }
    }
}
