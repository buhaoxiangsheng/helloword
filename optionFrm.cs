using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace zySoft
{
    public partial class optionFrm : Form
    {
        public optionFrm()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                //测试连接
                SqlConnection sqlconn = new SqlConnection();
                SqlDataAdapter sqladp = new SqlDataAdapter();
                sqlconn.ConnectionString = CommonClass.connstring = "Data Source=" + servertxt.Text.Trim() + ";Initial Catalog=" + dbtxt.Text + ";User Id=sa;Password=" + satxt.Text.Trim() + ";";
                if (sqlconn.State == ConnectionState.Closed)
                {
                    sqlconn.Open();
                    MessageBox.Show("连接数据库成功！", "登陆提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接参数设置错误！请调整,"+ex.Message, "登陆提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void optionFrm_Load(object sender, EventArgs e)
        {
            //载入默认值
            radioButton1.Checked = true;
            //载入相关的参数
            string Current = Directory.GetCurrentDirectory();//获取当前根目录
            Ini ini = new Ini(Current + "/config.ini");
            if (File.Exists(Current + "/config.ini"))
            {
                //存在配置文件，读取相关信息
                servertxt.Text = ini.ReadValue("DbInterface", "ServerName");
                satxt.Text = ini.ReadValue("DbInterface", "Password");
                dbtxt.Text = ini.ReadValue("DbInterface", "dbName");
                usertxt.Text = ini.ReadValue("DbInterface", "UserName");
            }
            comboBox1.Items.Add("宁波柯力D2008 D2009 XK3118");
            comboBox2.Text = "8";

  
        }

        private void button1_Click(object sender, EventArgs e)
        {
             //保存相关的参数
            string  Current = Directory.GetCurrentDirectory();//获取当前根目录
            //Console.WriteLine("Current directory {0}", Current);
            // 写入ini
            Ini ini = new Ini(Current + "/config.ini");

            CommonClass.ztcode = satxt.Text;
            CommonClass.iyear = dbtxt.Text;
            ini.Writue("DbInterface", "ServerName", servertxt.Text.Trim());
            ini.Writue("DbInterface", "UserName", usertxt.Text.Trim());
            ini.Writue("DbInterface", "Password", satxt.Text.Trim());
            ini.Writue("DbInterface", "dbName", dbtxt.Text.Trim());
            MessageBox.Show("连接参数保存成功！", "登陆提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);

}

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
