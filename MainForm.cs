using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace zySoft
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Form1 frm1 = new Form1();
            frm1.FormBorderStyle = FormBorderStyle.None;
            frm1.TopLevel = false;
            int index = tabControl1.TabCount; 
            TabPage Page = new TabPage();
            Page.Name = "Page" + (index+1).ToString();
            Page.Text = "tabPage" + (index+1).ToString();
            Page.TabIndex = index;
            this.tabControl1.Controls.Add(Page);

            #region 三种设置某个选项卡为当前选项卡的方法
            //this.tabControl1.SelectedIndex = index;  
            //this.tabControl1.SelectedTab = Page;  
            //panel1.Controls.Add(frm1);
            //frm1.Show();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否真的要退出系统？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            About  about = new About();
            about.FormBorderStyle = FormBorderStyle.None;
            about.TopLevel = false;
            panel1.Controls.Clear();
            panel1.Controls.Add(about);
            about.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            toolStripStatusLabel3.Text = "登录用户：" + CommonClass.userName;
            toolStripStatusLabel4.Text = "登录日期：" + DateTime.Now.ToString();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
          //标签打印
            Form2 frm2 = new Form2();
            frm2.FormBorderStyle = FormBorderStyle.None;
            frm2.TopLevel = false;
            panel1.Controls.Clear();
            frm2.Top = 0;
            frm2.Left = 0;
            frm2.Width = this.Width;
            frm2.Height = this.Height - toolStrip1.Height - statusStrip1.Height - 5;
            panel1.Controls.Add(frm2);
            frm2.Show();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            //设置接口连接参数
            optionFrm optfrm = new optionFrm();
            optfrm.FormBorderStyle = FormBorderStyle.None;
            optfrm.TopLevel = false;
            panel1.Controls.Clear();
            panel1.Controls.Add(optfrm);
            optfrm.Show();

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Form3 optfrm = new Form3();
            optfrm.FormBorderStyle = FormBorderStyle.None;
            optfrm.Top=0;
            optfrm.Left = 0;
            optfrm.Width = this.Width;
            optfrm.Height = this.Height - toolStrip1.Height - statusStrip1.Height-5;
            optfrm.TopLevel = false;
            panel1.Controls.Clear();
            panel1.Controls.Add(optfrm);
            optfrm.Show();
        }

        private void toolStripButton7_Click_1(object sender, EventArgs e)
        {
            userFrm optfrm = new userFrm();
            optfrm.FormBorderStyle = FormBorderStyle.None;
            optfrm.Top = 0;
            optfrm.Left = 0;
            optfrm.Width = this.Width;
            optfrm.Height = this.Height - toolStrip1.Height - statusStrip1.Height - 5;
            optfrm.TopLevel = false;
            panel1.Controls.Clear();
            panel1.Controls.Add(optfrm);
            optfrm.Show();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            About optfrm = new About();
            optfrm.FormBorderStyle = FormBorderStyle.None;
            optfrm.TopLevel = false;
            panel1.Controls.Clear();
            panel1.Controls.Add(optfrm);
            optfrm.Show();
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            inventory optfrm = new inventory();
            optfrm.FormBorderStyle = FormBorderStyle.None;
            optfrm.TopLevel = false;
            panel1.Controls.Clear();
            panel1.Controls.Add(optfrm);
            optfrm.Show();
        }
    }
}
            #endregion