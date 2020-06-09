namespace zySoft
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.startDate = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.endDate = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.selcol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.voutype = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.voucode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.voudate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.carno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partnerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cinvname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grossWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tareWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.netWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.water = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dhValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.netValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmemo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.weightTime2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.kdvouch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.axGRPrintViewer1 = new AxgrproLib.AxGRPrintViewer();
            this.invname = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.axGRPrintViewer1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.startDate,
            this.toolStripLabel2,
            this.endDate,
            this.toolStripLabel3,
            this.invname,
            this.toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton5,
            this.toolStripSeparator4,
            this.toolStripButton6,
            this.toolStripSeparator5,
            this.toolStripButton7,
            this.toolStripSeparator2,
            this.toolStripSeparator6,
            this.toolStripButton3,
            this.toolStripSeparator3,
            this.toolStripButton8,
            this.toolStripButton4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(981, 31);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(56, 28);
            this.toolStripLabel1.Text = "查询日期";
            // 
            // startDate
            // 
            this.startDate.Name = "startDate";
            this.startDate.Size = new System.Drawing.Size(90, 31);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(18, 28);
            this.toolStripLabel2.Text = "--";
            // 
            // endDate
            // 
            this.endDate.Name = "endDate";
            this.endDate.Size = new System.Drawing.Size(90, 31);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(52, 28);
            this.toolStripButton1.Text = "查询";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(76, 28);
            this.toolStripButton5.Text = "信息调整";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(52, 28);
            this.toolStripButton6.Text = "删除";
            this.toolStripButton6.Click += new System.EventHandler(this.toolStripButton6_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton7
            // 
            this.toolStripButton7.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton7.Image")));
            this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton7.Name = "toolStripButton7";
            this.toolStripButton7.Size = new System.Drawing.Size(52, 28);
            this.toolStripButton7.Text = "全选";
            this.toolStripButton7.Click += new System.EventHandler(this.toolStripButton7_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(52, 28);
            this.toolStripButton3.Text = "输出";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 31);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(52, 28);
            this.toolStripButton4.Text = "退出";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selcol,
            this.voutype,
            this.voucode,
            this.voudate,
            this.carno,
            this.partnerName,
            this.cinvname,
            this.grossWeight,
            this.tareWeight,
            this.netWeight,
            this.water,
            this.dhValue,
            this.netValue,
            this.cmemo,
            this.weightTime2,
            this.maker,
            this.kdvouch});
            this.dataGridView1.Location = new System.Drawing.Point(12, 62);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(941, 383);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView1_CurrentCellDirtyStateChanged);
            // 
            // selcol
            // 
            this.selcol.DataPropertyName = "selcol";
            this.selcol.HeaderText = "选择";
            this.selcol.Name = "selcol";
            this.selcol.Width = 60;
            // 
            // voutype
            // 
            this.voutype.DataPropertyName = "vouchtype";
            this.voutype.HeaderText = "单据类型";
            this.voutype.Name = "voutype";
            this.voutype.Width = 80;
            // 
            // voucode
            // 
            this.voucode.DataPropertyName = "vouchcode";
            this.voucode.HeaderText = "单据号";
            this.voucode.Name = "voucode";
            this.voucode.Width = 90;
            // 
            // voudate
            // 
            this.voudate.DataPropertyName = "vouchdate";
            this.voudate.HeaderText = "日期";
            this.voudate.Name = "voudate";
            // 
            // carno
            // 
            this.carno.DataPropertyName = "carno";
            this.carno.HeaderText = "车号";
            this.carno.Name = "carno";
            this.carno.Width = 90;
            // 
            // partnerName
            // 
            this.partnerName.DataPropertyName = "partnerName";
            this.partnerName.HeaderText = "客商名称";
            this.partnerName.Name = "partnerName";
            this.partnerName.Width = 80;
            // 
            // cinvname
            // 
            this.cinvname.DataPropertyName = "cinvname";
            this.cinvname.HeaderText = "物料";
            this.cinvname.Name = "cinvname";
            this.cinvname.Width = 90;
            // 
            // grossWeight
            // 
            this.grossWeight.DataPropertyName = "grossWeight";
            this.grossWeight.HeaderText = "毛重";
            this.grossWeight.Name = "grossWeight";
            // 
            // tareWeight
            // 
            this.tareWeight.DataPropertyName = "tareWeight";
            this.tareWeight.HeaderText = "皮重";
            this.tareWeight.Name = "tareWeight";
            // 
            // netWeight
            // 
            this.netWeight.DataPropertyName = "netWeight";
            this.netWeight.HeaderText = "净重";
            this.netWeight.Name = "netWeight";
            // 
            // water
            // 
            this.water.DataPropertyName = "water";
            this.water.HeaderText = "水杂";
            this.water.Name = "water";
            // 
            // dhValue
            // 
            this.dhValue.DataPropertyName = "dhValue";
            this.dhValue.HeaderText = "油品";
            this.dhValue.Name = "dhValue";
            // 
            // netValue
            // 
            this.netValue.DataPropertyName = "netValue";
            this.netValue.HeaderText = "皂化值";
            this.netValue.Name = "netValue";
            // 
            // cmemo
            // 
            this.cmemo.DataPropertyName = "cmemo";
            this.cmemo.HeaderText = "碘值";
            this.cmemo.Name = "cmemo";
            // 
            // weightTime2
            // 
            this.weightTime2.DataPropertyName = "weightTime2";
            this.weightTime2.HeaderText = "二次过磅时间";
            this.weightTime2.Name = "weightTime2";
            // 
            // maker
            // 
            this.maker.DataPropertyName = "cmaker";
            this.maker.HeaderText = "过磅人";
            this.maker.Name = "maker";
            // 
            // kdvouch
            // 
            this.kdvouch.DataPropertyName = "kdvouchcode";
            this.kdvouch.HeaderText = "金蝶单据号";
            this.kdvouch.Name = "kdvouch";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(162, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "客商";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(195, 32);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(99, 21);
            this.textBox1.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(300, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 17;
            this.label2.Text = "单据号";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(347, 32);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(106, 21);
            this.textBox2.TabIndex = 21;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 35);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 27;
            this.label7.Text = "车号";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(51, 30);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(109, 21);
            this.textBox7.TabIndex = 28;
            // 
            // axGRPrintViewer1
            // 
            this.axGRPrintViewer1.Enabled = true;
            this.axGRPrintViewer1.Location = new System.Drawing.Point(900, 392);
            this.axGRPrintViewer1.Name = "axGRPrintViewer1";
            this.axGRPrintViewer1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axGRPrintViewer1.OcxState")));
            this.axGRPrintViewer1.Size = new System.Drawing.Size(229, 221);
            this.axGRPrintViewer1.TabIndex = 14;
            this.axGRPrintViewer1.Visible = false;
            // 
            // invname
            // 
            this.invname.Name = "invname";
            this.invname.Size = new System.Drawing.Size(95, 31);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(32, 28);
            this.toolStripLabel3.Text = "品种";
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton8.Image")));
            this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton8.Name = "toolStripButton8";
            this.toolStripButton8.Size = new System.Drawing.Size(88, 28);
            this.toolStripButton8.Text = "清除单据号";
            this.toolStripButton8.Click += new System.EventHandler(this.toolStripButton8_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(981, 457);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.axGRPrintViewer1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form3";
            this.Text = "查询输出";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.axGRPrintViewer1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox startDate;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox endDate;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private AxgrproLib.AxGRPrintViewer axGRPrintViewer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.ToolStripButton toolStripButton7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selcol;
        private System.Windows.Forms.DataGridViewTextBoxColumn voutype;
        private System.Windows.Forms.DataGridViewTextBoxColumn voucode;
        private System.Windows.Forms.DataGridViewTextBoxColumn voudate;
        private System.Windows.Forms.DataGridViewTextBoxColumn carno;
        private System.Windows.Forms.DataGridViewTextBoxColumn partnerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cinvname;
        private System.Windows.Forms.DataGridViewTextBoxColumn grossWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn tareWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn netWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn water;
        private System.Windows.Forms.DataGridViewTextBoxColumn dhValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn netValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn cmemo;
        private System.Windows.Forms.DataGridViewTextBoxColumn weightTime2;
        private System.Windows.Forms.DataGridViewTextBoxColumn maker;
        private System.Windows.Forms.DataGridViewTextBoxColumn kdvouch;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox invname;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
    }
}