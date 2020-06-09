using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient; 

namespace zySoft
{
    public partial class userclass : Form
    {
        private SqlConnection sqlconn = new SqlConnection();
        private SqlConnection kdconn = new SqlConnection();
        public userclass()
        {
            InitializeComponent();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void userclass_Load(object sender, EventArgs e)
        {
            //载入分组
            try
            {
                sqlconn.ConnectionString = Utility.DatabaseConnection;
                SqlDataAdapter sqladp = new SqlDataAdapter();
                DataSet sqlds = new DataSet();
                if (sqlconn.State == ConnectionState.Closed)
                {
                    sqlconn.Open();
                }
                string str1 = "select classid,classname from aa_userclass ";
                sqladp = new SqlDataAdapter(str1, sqlconn);
                sqladp.Fill(sqlds, "userclass");
                dataGridView1.DataSource = sqlds.Tables["userclass"]; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("所发生错误：" + ex.Message);
            }

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //保存分组
            
            SqlDataAdapter sqladp = new SqlDataAdapter();
            SqlCommand sqlcomm = new SqlCommand(); 
            DataSet sqlds = new DataSet();
            if (sqlconn.State == ConnectionState.Closed)
            {
                sqlconn.ConnectionString = Utility.DatabaseConnection;
                sqlconn.Open();
            }
            if (textBox1.Text !="" && textBox2.Text !=""  )
            {
                string str1 = "select * from aa_userclass where classid='" + textBox1.Text + "'";
                sqladp = new SqlDataAdapter(str1, sqlconn);
                sqladp.Fill(sqlds, "class");
                if (sqlds.Tables["class"].Rows.Count > 0)
                {
                    MessageBox.Show("已存在项目的记录！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
               str1 = "insert into  aa_userclass(classid,classname) values('" + textBox1.Text + "','" + textBox2.Text + "')";
             sqlcomm = new SqlCommand(str1, sqlconn); 
             sqlcomm.ExecuteNonQuery();
             str1 = "select classid,classname from aa_userclass ";
             sqladp = new SqlDataAdapter(str1, sqlconn);
             sqladp.Fill(sqlds, "userclass");
             dataGridView1.DataSource = sqlds.Tables["userclass"]; 
             MessageBox.Show("保存成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("不能为空，请录入数据！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
