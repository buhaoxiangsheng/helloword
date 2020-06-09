using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace zySoft
{

    public partial class mainForm1 : DevExpress.XtraEditors.XtraForm
    {
        public mainForm1()
        {
            InitializeComponent();
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            //  
            barStaticItem1.Width = this.Width;
            barStaticItem4.Caption = "登录日期：" + DateTime.Now.ToString("yyyy-MM-dd");
            barStaticItem5.Caption = "登录用户：" + CommonClass.userName;

            navBarItem6.Visible = false;
            navBarItem11.Visible = false;
            navBarGroup3.Visible = false;
            navBarItem1.Visible = false;
            navBarItem3.Visible = false;
            navBarItem4.Visible = false;
            navBarItem9.Visible = false;
        }

        private void navBarItem7_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //调用用户管理
            xtraTabbedMdiManager1.MdiParent = this;   //设置控件的父表单..
            userFrm frm = new userFrm();
            frm.MdiParent = this;    //设置新建窗体的父表单为当前活动窗口
            frm.Show();
            xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
        }

        private void navBarItem1_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //标签格式
            xtraTabbedMdiManager1.MdiParent = this;   //设置控件的父表单..
            Form1 frm = new Form1();
            frm.MdiParent = this;    //设置新建窗体的父表单为当前活动窗口
            frm.Show();
            xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口

        }

        private void navBarItem2_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //过磅管理
            xtraTabbedMdiManager1.MdiParent = this;   //设置控件的父表单..
                                                      // WeightFrn frm = new WeightFrn();
            carweighing frm = new carweighing();
            frm.MdiParent = this;    //设置新建窗体的父表单为当前活动窗口
            frm.Show();
            xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口

        }

        private void navBarItem3_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //物料档案
            xtraTabbedMdiManager1.MdiParent = this;   //设置控件的父表单..
            inventory frm = new inventory();
            frm.MdiParent = this;    //设置新建窗体的父表单为当前活动窗口
            frm.Show();
            xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
        }

        private void navBarItem4_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //供应商


        }

        private void navBarItem5_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //输出打印
            xtraTabbedMdiManager1.MdiParent = this;   //设置控件的父表单..
            Form6 frm = new Form6();
            frm.MdiParent = this;    //设置新建窗体的父表单为当前活动窗口
            frm.Show();
            xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
        }

        private void navBarItem6_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            //标签打印
            xtraTabbedMdiManager1.MdiParent = this;   //设置控件的父表单..
            Form2 frm = new Form2();
            frm.MdiParent = this;    //设置新建窗体的父表单为当前活动窗口
            frm.Show();
            xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //退出系统
            if (MessageBox.Show("是否真的要退出系统？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        private void navBarItem10_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            xtraTabbedMdiManager1.MdiParent = this;   //设置控件的父表单..
            intercalate frm = new intercalate();
            frm.MdiParent = this;    //设置新建窗体的父表单为当前活动窗口
            frm.Show();
            xtraTabbedMdiManager1.SelectedPage = xtraTabbedMdiManager1.Pages[frm];    //使得标签的选择为当前新建的窗口
        }

    }
}