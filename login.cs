using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.IO;

namespace zySoft
{
    public partial class login : Form
    {
        private SqlConnection sqlconn = new SqlConnection();
        private SqlConnection kdconn = new SqlConnection();
        DataTable account = new DataTable();

      
        public login()
        {
            InitializeComponent();
        }
        insertvouchar insertvouchar = new insertvouchar(); 
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.SelectedIndex==0)
                {
                    throw new Exception("请选择账套");
                }
                //设置与数据源的连接串，因为在设计时指定的数据库路径是绝对路径。
                if (sqlconn.State == ConnectionState.Closed)
                {
                    sqlconn.ConnectionString = "Data Source=" + servertxt.Text + ";User ID=" + usertxt.Text + ";pwd=" + satxt.Text + ";Initial Catalog=" + label10.Text + ";";
                    Utility.DatabaseConnection = sqlconn.ConnectionString;
                    Utility.ReportConnection = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" + usertxt.Text + ";Password=" + satxt.Text + ";Initial Catalog=" + label10.Text  + ";Data Source=" + servertxt.Text + ";Pooling=true;Max Pool Size=300;Min Pool Size=0;Connection Lifetime=300;";
                    sqlconn.Open();
                    
                }
                if (sqlconn.State == ConnectionState.Open)
                {
                    sqlconn.Close();
                    sqlconn.ConnectionString = "Data Source=" + servertxt.Text + ";User ID=" + usertxt.Text + ";pwd=" + satxt.Text + ";Initial Catalog=" + label10.Text + ";";
                    Utility.DatabaseConnection = sqlconn.ConnectionString;
                    Utility.ReportConnection = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=" + usertxt.Text + ";Password=" + satxt.Text + ";Initial Catalog=" + label10.Text + ";Data Source=" + servertxt.Text + ";Pooling=true;Max Pool Size=300;Min Pool Size=0;Connection Lifetime=300;";
                    sqlconn.Open();

                }

                CommonClass.ztcode = satxt.Text;
                CommonClass.iyear = dbtxt.Text;
                CommonClass.userCode = textBox5.Text.Trim();
                SqlDataAdapter sqladp = new SqlDataAdapter();
                SqlCommand sqlcomm = new SqlCommand();
                DataSet sqlds = new DataSet();

                //通过获取token来判断用户登录成功
                string passwords = insertvouchar.getusertokens(textBox5.Text, textBox6.Text, textBox1.Text, textBox2.Text, textBox3.Text);
            
                if (passwords!="") { 
                string str1 = "select  top 1 Code, Name,Name from eap_user where name='"+ textBox5.Text + "'";
                    sqladp = new SqlDataAdapter(str1,sqlconn);
                    sqladp.Fill(sqlds, "user");
                int aa = sqlds.Tables["user"].Rows.Count;
                if (sqlds.Tables["user"].Rows.Count>0)
                {
                    CommonClass.userCode = sqlds.Tables["user"].Rows[0]["Code"].ToString();
                    CommonClass.userName = sqlds.Tables["user"].Rows[0]["Name"].ToString();
                    CommonClass.teamName = sqlds.Tables["user"].Rows[0]["Name"].ToString();
                        CommonClass.usertoken = passwords;
                    mainForm1 mainfrm = new mainForm1();
                    mainfrm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("用户或密码错误！", "登录", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("登录时发生错误：" + ex.Message, "系统登录");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //保存win文件
            try
            {//测试连接
                SqlConnection sqlconn;
                string Current;
                //连接数据库
                if (servertxt.Text != "" && satxt.Text != "")
                {
                    CommonClass.connstring = "Data Source=" + servertxt.Text.Trim() + ";Initial Catalog=" + dbtxt.Text + ";User Id=sa;Password=" + satxt.Text.Trim() + ";";
                    sqlconn = new SqlConnection();
                    sqlconn.ConnectionString = CommonClass.connstring;
                    sqlconn.Open();

                    if (sqlconn.State == ConnectionState.Open)
                    {
                            //保存相关的参数
                            Current = Directory.GetCurrentDirectory();//获取当前根目录
                            //Console.WriteLine("Current directory {0}", Current);
                            // 写入ini
                            Ini ini = new Ini(Current + "/config.ini");
                            CommonClass.ztcode = satxt.Text;
                            CommonClass.iyear = dbtxt.Text;
                            ini.Writue("Setting", "ServerName", servertxt.Text.Trim());
                            ini.Writue("Setting", "UserName", usertxt.Text.Trim());
                            ini.Writue("Setting", "Password", satxt.Text.Trim());
                            ini.Writue("Setting", "dbName", dbtxt.Text.Trim());
                            ini.Writue("Setting", "caccNums", textBox1.Text.Trim());
                            ini.Writue("Setting", "privateKeyPaths", textBox3.Text.Trim());
                            ini.Writue("Setting", "cmServerURLs", textBox2.Text.Trim());
                        MessageBox.Show("连接参数保存成功！", "登陆提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }


                    SqlDataAdapter sqladp = new SqlDataAdapter();
                    SqlCommand sqlcomm = new SqlCommand();
                    DataTable dt = new DataTable();
                   
                    account = comboxvalue(comboBox1, label10, "select DsName id ,cAcc_Name name from  EAP_Account",sqlconn);
                }
                else
                {
                    MessageBox.Show("请录入服务器名和接口帐套号！", "登陆提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
            catch (Exception EX)
            {
                MessageBox.Show("不能连接到服务器，请查看相关的参数！,错误：" + EX.Message, "登陆提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (string.IsNullOrEmpty(textBox3.Text) || !File.Exists(textBox3.Text))
            {
                MessageBox.Show("请指定私钥路径！");
            }

        }

        private void login_Load(object sender, EventArgs e)
        {

           
          
            //载入设置参数
            string deptID = "";
            servertxt.Text = "admin";
            panel1.Visible = false;
            //载入相关的参数
            string Current = Directory.GetCurrentDirectory();//获取当前根目录
            Ini ini = new Ini(Current + "/config.ini");
            try { 
            if (File.Exists(Current + "/config.ini"))
            {

                //存在配置文件，读取相关信息
                servertxt.Text = ini.ReadValue("Setting", "ServerName");
                satxt.Text = ini.ReadValue("Setting", "Password");
                dbtxt.Text = ini.ReadValue("Setting", "dbName");
                usertxt.Text = ini.ReadValue("Setting", "UserName");
                textBox1.Text= ini.ReadValue("Setting", "caccNums");
                textBox2.Text = ini.ReadValue("Setting", "cmServerURLs");
                textBox3.Text = ini.ReadValue("Setting", "privateKeyPaths");

                CommonClass.connstring = "Data Source=" + servertxt.Text.Trim() + ";Initial Catalog=" + dbtxt.Text + ";User Id=sa;Password=" + satxt.Text.Trim() + ";";
                sqlconn = new SqlConnection();
                sqlconn.ConnectionString = CommonClass.connstring;
                sqlconn.Open();


              

                SqlDataAdapter sqladp = new SqlDataAdapter();
                SqlCommand sqlcomm = new SqlCommand();
                DataTable dt = new DataTable();

                    

                    account = comboxvalue(comboBox1, label10, "select DsName id ,cAcc_Name name from  EAP_Account",sqlconn);
                    comboBox1.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("请先进行连接配置！", "登陆提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
           }
            catch (Exception  ex )
            {
                MessageBox.Show("数据库配置出错！请重新进行配置"+ex.Message, "数据库连接", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private void button5_Click(object sender, EventArgs e)
        {
            //隐藏panel1
            panel1.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (account.Rows.Count>0) {
                if (comboBox1.SelectedIndex>-1)
                {
                    label10.Text = account.Rows[comboBox1.SelectedIndex]["id"].ToString();
                }
                  
            }
            
          
        }
        public DataTable comboxvalue(ComboBox comboBoxemp, Label label, String s, SqlConnection sqlco)
        {


            SqlDataAdapter sqladp = new SqlDataAdapter();
            SqlCommand sqlcomm = new SqlCommand();
            DataTable dt = new DataTable();

            sqladp = new SqlDataAdapter(s, sqlco);
          

            dt.Columns.Add("id");
            dt.Columns.Add("name");
            DataRow dr = dt.NewRow();
            object[] objs = { "0", "- -请选择账套- -" };
            dr.ItemArray = objs;
            dt.Rows.Add(dr);
            sqladp.Fill(dt);
            comboBoxemp.DataSource = dt;
            comboBoxemp.DisplayMember = "name";
            comboBoxemp.DisplayMember = "name";
            comboBoxemp.SelectedIndex = 0;
            return dt;
        }

    }
}
