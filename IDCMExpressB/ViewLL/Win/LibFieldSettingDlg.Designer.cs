namespace IDCM.ViewLL.Win
{
    partial class LibFieldSettingDlg
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LibFieldSettingDlg));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView_fields = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_submit = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.comboBox_templ = new System.Windows.Forms.ComboBox();
            this.label_template = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.attr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.attrType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.isUnique = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.defaultVal = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.restrict = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.mainField = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.comments = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moveUp = new System.Windows.Forms.DataGridViewButtonColumn();
            this.moveDown = new System.Windows.Forms.DataGridViewButtonColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_fields)).BeginInit();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView_fields);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(6, 56);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(745, 416);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Fields Configuration";
            // 
            // dataGridView_fields
            // 
            this.dataGridView_fields.AllowUserToAddRows = false;
            this.dataGridView_fields.AllowUserToDeleteRows = false;
            this.dataGridView_fields.BackgroundColor = System.Drawing.Color.Gainsboro;
            this.dataGridView_fields.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_fields.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.attr,
            this.attrType,
            this.isUnique,
            this.defaultVal,
            this.restrict,
            this.mainField,
            this.comments,
            this.moveUp,
            this.moveDown});
            this.dataGridView_fields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_fields.Location = new System.Drawing.Point(3, 19);
            this.dataGridView_fields.Name = "dataGridView_fields";
            this.dataGridView_fields.ReadOnly = true;
            this.dataGridView_fields.RowTemplate.Height = 23;
            this.dataGridView_fields.Size = new System.Drawing.Size(739, 394);
            this.dataGridView_fields.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightCyan;
            this.panel1.Controls.Add(this.button_submit);
            this.panel1.Controls.Add(this.button_cancel);
            this.panel1.Controls.Add(this.comboBox_templ);
            this.panel1.Controls.Add(this.label_template);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(757, 50);
            this.panel1.TabIndex = 1;
            // 
            // button_submit
            // 
            this.button_submit.Location = new System.Drawing.Point(532, 13);
            this.button_submit.Name = "button_submit";
            this.button_submit.Size = new System.Drawing.Size(75, 23);
            this.button_submit.TabIndex = 3;
            this.button_submit.Text = "Submit";
            this.button_submit.UseVisualStyleBackColor = true;
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(415, 13);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(75, 23);
            this.button_cancel.TabIndex = 2;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            // 
            // comboBox_templ
            // 
            this.comboBox_templ.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_templ.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox_templ.FormattingEnabled = true;
            this.comboBox_templ.Location = new System.Drawing.Point(107, 15);
            this.comboBox_templ.Name = "comboBox_templ";
            this.comboBox_templ.Size = new System.Drawing.Size(250, 20);
            this.comboBox_templ.TabIndex = 1;
            // 
            // label_template
            // 
            this.label_template.AutoSize = true;
            this.label_template.Location = new System.Drawing.Point(36, 18);
            this.label_template.Name = "label_template";
            this.label_template.Size = new System.Drawing.Size(65, 12);
            this.label_template.TabIndex = 0;
            this.label_template.Text = "Templates:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(757, 478);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // attr
            // 
            this.attr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.attr.DefaultCellStyle = dataGridViewCellStyle1;
            this.attr.HeaderText = "Name";
            this.attr.MaxInputLength = 128;
            this.attr.Name = "attr";
            this.attr.ReadOnly = true;
            this.attr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.attr.ToolTipText = "Field Name";
            this.attr.Width = 49;
            // 
            // attrType
            // 
            this.attrType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.attrType.DefaultCellStyle = dataGridViewCellStyle2;
            this.attrType.HeaderText = "Type";
            this.attrType.Name = "attrType";
            this.attrType.ReadOnly = true;
            this.attrType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.attrType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.attrType.ToolTipText = "Field Type";
            this.attrType.Width = 61;
            // 
            // isUnique
            // 
            this.isUnique.HeaderText = "Unique";
            this.isUnique.Name = "isUnique";
            this.isUnique.ReadOnly = true;
            this.isUnique.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isUnique.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.isUnique.Width = 50;
            // 
            // defaultVal
            // 
            this.defaultVal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.defaultVal.DefaultCellStyle = dataGridViewCellStyle3;
            this.defaultVal.HeaderText = "Default Value";
            this.defaultVal.Name = "defaultVal";
            this.defaultVal.ReadOnly = true;
            this.defaultVal.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.defaultVal.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.defaultVal.Width = 110;
            // 
            // restrict
            // 
            this.restrict.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.restrict.DefaultCellStyle = dataGridViewCellStyle4;
            this.restrict.HeaderText = "Restriction Expression ";
            this.restrict.Name = "restrict";
            this.restrict.ReadOnly = true;
            this.restrict.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.restrict.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.restrict.Width = 150;
            // 
            // mainField
            // 
            this.mainField.HeaderText = "Main";
            this.mainField.Name = "mainField";
            this.mainField.ReadOnly = true;
            this.mainField.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.mainField.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.mainField.Width = 50;
            // 
            // comments
            // 
            this.comments.HeaderText = "Comments";
            this.comments.Name = "comments";
            this.comments.ReadOnly = true;
            // 
            // moveUp
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.moveUp.DefaultCellStyle = dataGridViewCellStyle5;
            this.moveUp.HeaderText = "Up";
            this.moveUp.Name = "moveUp";
            this.moveUp.ReadOnly = true;
            this.moveUp.Width = 50;
            // 
            // moveDown
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            this.moveDown.DefaultCellStyle = dataGridViewCellStyle6;
            this.moveDown.HeaderText = "Down";
            this.moveDown.Name = "moveDown";
            this.moveDown.ReadOnly = true;
            this.moveDown.Width = 50;
            // 
            // LibFieldSettingDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(757, 478);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LibFieldSettingDlg";
            this.Text = "Library Fields Setting";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_fields)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label_template;
        private System.Windows.Forms.ComboBox comboBox_templ;
        private System.Windows.Forms.Button button_submit;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.DataGridView dataGridView_fields;
        private System.Windows.Forms.DataGridViewTextBoxColumn attr;
        private System.Windows.Forms.DataGridViewComboBoxColumn attrType;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isUnique;
        private System.Windows.Forms.DataGridViewComboBoxColumn defaultVal;
        private System.Windows.Forms.DataGridViewComboBoxColumn restrict;
        private System.Windows.Forms.DataGridViewCheckBoxColumn mainField;
        private System.Windows.Forms.DataGridViewTextBoxColumn comments;
        private System.Windows.Forms.DataGridViewButtonColumn moveUp;
        private System.Windows.Forms.DataGridViewButtonColumn moveDown;
    }
}