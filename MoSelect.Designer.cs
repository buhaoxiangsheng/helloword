namespace zySoft
{
    partial class MoSelect
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.startdate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.enddate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.selcol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.vouchcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.depname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fmodal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.batchno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.moQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.planDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selcol,
            this.vouchcode,
            this.depname,
            this.fname,
            this.fmodal,
            this.batchno,
            this.moQty,
            this.planDate,
            this.Id});
            this.dataGridView1.Location = new System.Drawing.Point(12, 34);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(764, 421);
            this.dataGridView1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "计划完工日期";
            // 
            // startdate
            // 
            this.startdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startdate.Location = new System.Drawing.Point(88, 5);
            this.startdate.Name = "startdate";
            this.startdate.Size = new System.Drawing.Size(83, 21);
            this.startdate.TabIndex = 2;
            this.startdate.Value = new System.DateTime(2017, 1, 12, 0, 0, 0, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(177, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "----";
            // 
            // enddate
            // 
            this.enddate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.enddate.Location = new System.Drawing.Point(212, 4);
            this.enddate.Name = "enddate";
            this.enddate.Size = new System.Drawing.Size(98, 21);
            this.enddate.TabIndex = 5;
            this.enddate.Value = new System.DateTime(2017, 1, 12, 0, 0, 0, 0);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(331, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "任务单号";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(390, 7);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(506, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(59, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "查询";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(571, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(59, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "确认";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // selcol
            // 
            this.selcol.HeaderText = "选择";
            this.selcol.Name = "selcol";
            this.selcol.Width = 60;
            // 
            // vouchcode
            // 
            this.vouchcode.DataPropertyName = "FBillNo";
            this.vouchcode.HeaderText = "单据号";
            this.vouchcode.Name = "vouchcode";
            this.vouchcode.Width = 80;
            // 
            // depname
            // 
            this.depname.DataPropertyName = "cdepname";
            this.depname.HeaderText = "生产部门";
            this.depname.Name = "depname";
            this.depname.Width = 80;
            // 
            // fname
            // 
            this.fname.DataPropertyName = "FName";
            this.fname.HeaderText = "产品名称";
            this.fname.Name = "fname";
            this.fname.Width = 80;
            // 
            // fmodal
            // 
            this.fmodal.DataPropertyName = "FModel";
            this.fmodal.HeaderText = "规格";
            this.fmodal.Name = "fmodal";
            this.fmodal.Width = 70;
            // 
            // batchno
            // 
            this.batchno.DataPropertyName = "FGMPBatchNo";
            this.batchno.HeaderText = "批号";
            this.batchno.Name = "batchno";
            this.batchno.Width = 70;
            // 
            // moQty
            // 
            this.moQty.DataPropertyName = "FQty";
            this.moQty.HeaderText = "生产数量";
            this.moQty.Name = "moQty";
            this.moQty.Width = 80;
            // 
            // planDate
            // 
            this.planDate.DataPropertyName = "FPlanCommitDate";
            this.planDate.HeaderText = "计划日期";
            this.planDate.Name = "planDate";
            this.planDate.Width = 90;
            // 
            // Id
            // 
            this.Id.DataPropertyName = "FInterID";
            this.Id.HeaderText = "ID";
            this.Id.Name = "Id";
            this.Id.Width = 30;
            // 
            // MoSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(788, 467);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.enddate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.startdate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "MoSelect";
            this.Text = "生产任务单选择";
            this.Load += new System.EventHandler(this.MoSelect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker startdate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker enddate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selcol;
        private System.Windows.Forms.DataGridViewTextBoxColumn vouchcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn depname;
        private System.Windows.Forms.DataGridViewTextBoxColumn fname;
        private System.Windows.Forms.DataGridViewTextBoxColumn fmodal;
        private System.Windows.Forms.DataGridViewTextBoxColumn batchno;
        private System.Windows.Forms.DataGridViewTextBoxColumn moQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn planDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
    }
}