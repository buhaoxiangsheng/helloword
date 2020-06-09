namespace zySoft
{
    partial class WeightSelect
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
            this.queryDate = new System.Windows.Forms.Label();
            this.startdate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.selcol = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.carno = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ddate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.voucode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.partnerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cinvname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pweight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gweight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // queryDate
            // 
            this.queryDate.AutoSize = true;
            this.queryDate.Location = new System.Drawing.Point(2, 16);
            this.queryDate.Name = "queryDate";
            this.queryDate.Size = new System.Drawing.Size(53, 12);
            this.queryDate.TabIndex = 0;
            this.queryDate.Text = "查询日期";
            // 
            // startdate
            // 
            this.startdate.Location = new System.Drawing.Point(61, 13);
            this.startdate.Name = "startdate";
            this.startdate.Size = new System.Drawing.Size(121, 21);
            this.startdate.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(367, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "客商名称";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(428, 15);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(536, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "查询";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selcol,
            this.carno,
            this.ddate,
            this.voucode,
            this.partnerName,
            this.cinvname,
            this.pweight,
            this.gweight});
            this.dataGridView1.Location = new System.Drawing.Point(15, 43);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(699, 349);
            this.dataGridView1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(189, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "车牌号";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(236, 13);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(126, 21);
            this.textBox2.TabIndex = 7;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(617, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "确定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // selcol
            // 
            this.selcol.HeaderText = "选择";
            this.selcol.Name = "selcol";
            this.selcol.Width = 60;
            // 
            // carno
            // 
            this.carno.DataPropertyName = "carno";
            this.carno.HeaderText = "车牌号";
            this.carno.Name = "carno";
            this.carno.Width = 80;
            // 
            // ddate
            // 
            this.ddate.DataPropertyName = "vouchdate";
            this.ddate.HeaderText = "日期";
            this.ddate.Name = "ddate";
            // 
            // voucode
            // 
            this.voucode.DataPropertyName = "vouchcode";
            this.voucode.HeaderText = "单号";
            this.voucode.Name = "voucode";
            this.voucode.Width = 80;
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
            this.cinvname.HeaderText = "物料名称";
            this.cinvname.Name = "cinvname";
            // 
            // pweight
            // 
            this.pweight.DataPropertyName = "tareWeight";
            this.pweight.HeaderText = "皮重";
            this.pweight.Name = "pweight";
            this.pweight.Width = 80;
            // 
            // gweight
            // 
            this.gweight.DataPropertyName = "grossWeight";
            this.gweight.HeaderText = "毛重";
            this.gweight.Name = "gweight";
            this.gweight.Width = 80;
            // 
            // WeightSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(726, 404);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.startdate);
            this.Controls.Add(this.queryDate);
            this.Name = "WeightSelect";
            this.Text = "未完成过磅单选择";
            this.Load += new System.EventHandler(this.WeightSelect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label queryDate;
        private System.Windows.Forms.DateTimePicker startdate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selcol;
        private System.Windows.Forms.DataGridViewTextBoxColumn carno;
        private System.Windows.Forms.DataGridViewTextBoxColumn ddate;
        private System.Windows.Forms.DataGridViewTextBoxColumn voucode;
        private System.Windows.Forms.DataGridViewTextBoxColumn partnerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cinvname;
        private System.Windows.Forms.DataGridViewTextBoxColumn pweight;
        private System.Windows.Forms.DataGridViewTextBoxColumn gweight;
    }
}