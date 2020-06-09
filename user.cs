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
    public partial class userFrm : Form
    {
        SqlConnection sqlconn = new SqlConnection();
        SqlConnection kdconn = new SqlConnection();
        public userFrm()
        {
            InitializeComponent();
        }

        private void userFrm_Load(object sender, EventArgs e)
        {
            try
            {
                string Current = Directory.GetCurrentDirectory();//获取当前根目录
                Ini ini = new Ini(Current + "/config.ini");
                if (File.Exists(Current + "/config.ini"))
                {
                    //存在配置文件，读取相关信息
                    string servertxt = ini.ReadValue("DbInterface", "ServerName");
                    string satxt = ini.ReadValue("DbInterface", "Password");
                    string dbtxt = ini.ReadValue("DbInterface", "dbName");
                    string usertxt = ini.ReadValue("DbInterface", "UserName");
                    Utility.kdconnection = "Data Source=" + servertxt + ";User ID=" + usertxt + ";pwd=" + satxt + ";Initial Catalog=" + dbtxt + ";";
                }
                else
                {
                    MessageBox.Show("请先进行连接配置！", "登陆提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();

                }
                string str1 = "";

                SqlDataAdapter sqladp = new SqlDataAdapter();
                DataSet sqlds = new DataSet();

                if (sqlconn.State == ConnectionState.Closed)
                {
                    sqlconn.ConnectionString = Utility.DatabaseConnection;
                    sqlconn.Open();
                }
                str1 = "select classid,classname from aa_userclass";
                sqladp = new SqlDataAdapter(str1, sqlconn);
                sqladp.Fill(sqlds, "userclass");
                treeView1.Nodes.Clear();
                TreeNode mainNodes = new TreeNode("用户分组");
                treeView1.Nodes.Add(mainNodes);
                string userid = "";
                

                for (int i = 0; i < sqlds.Tables["userclass"].Rows.Count; i++)
                {
                    mainNodes.Nodes.Add(sqlds.Tables["userclass"].Rows[i]["classname"].ToString());
                    comboBox2.Items.Add(sqlds.Tables["userclass"].Rows[i]["classname"].ToString());
                    comboBox2.SelectedIndex = 0;
                    useridtxt.Text = sqlds.Tables["userclass"].Rows[0]["classname"].ToString();
                }
                mainNodes.ExpandAll(); 
                str1 = "select usercode ,username ,deptname,teamname,kdusername  from AA_user";
                sqladp = new SqlDataAdapter(str1, sqlconn);
                sqladp.Fill(sqlds, "user");
                dataGridView1.DataSource = sqlds.Tables["user"];

                if (kdconn.State == ConnectionState.Closed)
                {
                    kdconn.ConnectionString = Utility.kdConnection;
                    kdconn.Open();
                }
                dataGridView1.Columns["selcol"].HeaderText = "选择";
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化用户管理发生错误："+ex.Message, "删除提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            string str1 = "";
            DataSet sqlds = new DataSet();
            SqlCommand sqlcomm = new SqlCommand();
            if (sqlconn.State == ConnectionState.Closed)
            {
                sqlconn.ConnectionString = Utility.DatabaseConnection;
                sqlconn.Open();
            }
            SqlDataAdapter sqladp = new SqlDataAdapter();
            str1 = "insert into AA_user( userCode,userName,password,deptname,teamName,kdusername,kduserid,userclassid) values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + comboBox1.Text + "','" + textBox6.Text + "','" + useridtxt.Text + "')";
            sqlcomm = new SqlCommand(str1, sqlconn);
            sqlcomm.ExecuteNonQuery();
            str1 = "select usercode ,username ,deptname,teamname,kdusername  from AA_user";
            sqladp = new SqlDataAdapter(str1, sqlconn);
            sqladp.Fill(sqlds, "user");
            if (sqlds.Tables["user"].Rows.Count > 0)
            {
                dataGridView1.DataSource = sqlds.Tables["user"];
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           //
            if (kdconn.State == ConnectionState.Closed)
            {
                kdconn.ConnectionString = Utility.kdConnection;
                kdconn.Open();
            }
            string str1 = "select FUserID,FName from t_User where FPrimaryGroup=0";
            SqlDataAdapter sqladp = new SqlDataAdapter(str1, kdconn);
            DataSet sqlds = new DataSet();
            sqladp.Fill(sqlds, "user");
            comboBox1.Items.Clear(); 
            for (int i=0;i <=sqlds.Tables["user"].Rows.Count -1;i++)
            {
                comboBox1.Items.Add(sqlds.Tables["user"].Rows[i]["FName"].ToString());

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //选择仓库更新id
                SqlDataAdapter sqladp = new SqlDataAdapter();
                DataSet sqlds = new DataSet();
                if (kdconn.State == ConnectionState.Closed)
                {
                    kdconn.ConnectionString = Utility.kdconnection;
                    kdconn.Open();
                }
                string str1 = "";
                str1 = "select FUserID from t_User where FName ='" + comboBox1.Text + "'";
                sqladp = new SqlDataAdapter(str1, kdconn);
                sqlds = new DataSet();
                sqladp.Fill(sqlds, "user");
                if (sqlds.Tables["user"].Rows.Count > 0)
                {
                    textBox6.Text = sqlds.Tables["user"].Rows[0]["FUserID"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获得金蝶用户发生错误！", "退出提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                textBox1.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["usercode"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["username"].Value.ToString();
                textBox4.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["deptname"].Value.ToString();
                textBox5.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["teamname"].Value.ToString();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
             string str1 = "";
            SqlConnection sqlconn = new SqlConnection();
            SqlDataAdapter sqladp = new SqlDataAdapter();
            DataSet sqlds = new DataSet();
            if (sqlconn.State == ConnectionState.Closed)
            {
                sqlconn.ConnectionString = Utility.DatabaseConnection;
                sqlconn.Open();
            }
            if (textBox2.Text == "") 
            {
                MessageBox.Show("无法获得要删除的用户！", "删除提示：", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }
            if (MessageBox.Show("是否真要删除用户" + textBox2.Text + "？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                str1 = "delete  from AA_user where userCode='" + textBox1.Text + "'";
                SqlCommand sqlcomm = new SqlCommand(str1, sqlconn);
                sqlcomm.ExecuteNonQuery();
            }

             str1 = "select usercode ,username ,deptname,teamname,kdusername  from AA_user";
            sqladp = new SqlDataAdapter(str1, sqlconn);
            sqladp.Fill(sqlds, "user");
            if (sqlds.Tables["user"].Rows.Count > 0)
            {
                dataGridView1.DataSource = sqlds.Tables["user"];
            }
           MessageBox.Show("删除成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //建立分钟
            string str1 = "";
            SqlDataAdapter sqladp = new SqlDataAdapter();
            DataSet sqlds = new DataSet(); 
            userclass classfrm = new userclass();
            classfrm.ShowDialog();
            str1 = "select classid,classname from aa_userclass";
            sqladp = new SqlDataAdapter(str1, sqlconn);
            sqladp.Fill(sqlds, "userclass");
            treeView1.Nodes.Clear();
            for (int i = 0; i < sqlds.Tables["userclass"].Rows.Count; i++)
            {
                treeView1.Nodes.Add(sqlds.Tables["userclass"].Rows[i]["classname"].ToString());
            }
            str1 = "select usercode ,username ,deptname,teamname,kdusername  from AA_user";
            sqladp = new SqlDataAdapter(str1, sqlconn);
            sqladp.Fill(sqlds, "user");
            dataGridView1.DataSource = sqlds.Tables["user"];

        }

        private void userFrm_Resize(object sender, EventArgs e)
        {
            dataGridView1.Left = treeView1.Location.X + treeView1.Width + 1;

        }
      
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                //分类选择
                string str1 = "";
                SqlDataAdapter sqladp = new SqlDataAdapter();
                DataSet sqlds = new DataSet();
                if (treeView1.SelectedNode == null) { return; }
                string nodeName = treeView1.SelectedNode.Text;
                if (nodeName != "")
                {
                    switch (nodeName)
                    {
                        case "用户分组":
                            str1 = "select userCode,userName,deptname,teamName,kdusername from aa_user left join aa_userclass on aa_user.userclassid=aa_userclass.classid";
                            break;
                        default:
                            str1 = "select userCode,userName,deptname,teamName,kdusername from aa_user left join aa_userclass on aa_user.userclassid=aa_userclass.classid where className='" + nodeName + "'";
                            break;
                    }
                    if (str1 != "")
                    {
                        sqladp = new SqlDataAdapter(str1, sqlconn);
                        sqladp.Fill(sqlds, "user");
                        dataGridView1.DataSource = sqlds.Tables["user"];
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //选择仓库更新id
                SqlDataAdapter sqladp = new SqlDataAdapter();
                DataSet sqlds = new DataSet();
                if (kdconn.State == ConnectionState.Closed)
                {
                    kdconn.ConnectionString = Utility.kdconnection;
                    kdconn.Open();
                }
                string str1 = "";
                str1 = "select classid from AA_userClass where className ='" + comboBox2.Text + "'";
                sqladp = new SqlDataAdapter(str1, sqlconn);
                sqlds = new DataSet();
                sqladp.Fill(sqlds, "userclass");
                if (sqlds.Tables["userclass"].Rows.Count > 0)
                {
                    useridtxt.Text = sqlds.Tables["userclass"].Rows[0]["classid"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获得用户组发生错误！", "退出提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
      }
    }

