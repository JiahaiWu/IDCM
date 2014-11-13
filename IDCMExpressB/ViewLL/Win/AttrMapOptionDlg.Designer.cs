namespace IDCM.ViewLL.Win
{
    partial class AttrMapOptionDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttrMapOptionDlg));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Column_src = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_alter = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Column_tag = new System.Windows.Forms.DataGridViewImageColumn();
            this.Column_dest = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column_cancel = new System.Windows.Forms.DataGridViewImageColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column_src,
            this.Column_alter,
            this.Column_tag,
            this.Column_dest,
            this.Column_cancel});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 83);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(710, 429);
            this.dataGridView1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.dataGridView1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(716, 515);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // Column_src
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column_src.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column_src.HeaderText = "From Attr";
            this.Column_src.Name = "Column_src";
            this.Column_src.Width = 150;
            // 
            // Column_alter
            // 
            this.Column_alter.HeaderText = "Config";
            this.Column_alter.Name = "Column_alter";
            this.Column_alter.Text = "...";
            this.Column_alter.ToolTipText = "...";
            this.Column_alter.Width = 60;
            // 
            // Column_tag
            // 
            this.Column_tag.HeaderText = "";
            this.Column_tag.Image = global::IDCM.Properties.Resources.rightArrow;
            this.Column_tag.Name = "Column_tag";
            // 
            // Column_dest
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Column_dest.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column_dest.HeaderText = "To Attr";
            this.Column_dest.Name = "Column_dest";
            this.Column_dest.Width = 150;
            // 
            // Column_cancel
            // 
            this.Column_cancel.HeaderText = "Cancel";
            this.Column_cancel.Image = global::IDCM.Properties.Resources.del_note;
            this.Column_cancel.Name = "Column_cancel";
            // 
            // AttrMapOptionDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(716, 515);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AttrMapOptionDlg";
            this.Text = "AttrMappingOptionDlg";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_src;
        private System.Windows.Forms.DataGridViewButtonColumn Column_alter;
        private System.Windows.Forms.DataGridViewImageColumn Column_tag;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column_dest;
        private System.Windows.Forms.DataGridViewImageColumn Column_cancel;
    }
}