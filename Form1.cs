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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //保存数据
            string str1 = "";
 
            SqlConnection sqlconn=new SqlConnection();
            SqlCommand sqlcomm = new SqlCommand();
            DataSet sqlds = new DataSet();
            if (dataGridView1.Rows.Count > 0)
            {
                sqlconn.ConnectionString = Utility.DatabaseConnection;
                if (sqlconn.State == ConnectionState.Closed)
                {
                    sqlconn.Open();
                }
                //删除所有
                str1 = "delete from FRtype  ";
                sqlcomm = new SqlCommand(str1, sqlconn);
                sqlcomm.ExecuteNonQuery(); 

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    str1 = "insert into FRtype(RFID,RFName,RFPath) values('" + dataGridView1.Rows[i].Cells["RFID"].Value.ToString() + "','" + dataGridView1.Rows[i].Cells["RFName"].Value.ToString() + "','" + dataGridView1.Rows[i].Cells["RFPath"].Value.ToString() + "')";
                    sqlcomm = new SqlCommand(str1, sqlconn);
                    sqlcomm.ExecuteNonQuery(); 
                }
                MessageBox.Show("保存成功！", "提示"); 
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //载入表中的数据
            string str1 = "";
            SqlConnection sqlconn = new SqlConnection();
            SqlDataAdapter  sqladp = new SqlDataAdapter();
            DataSet sqlds = new DataSet();
            sqlconn.ConnectionString = Utility.DatabaseConnection;
            if (sqlconn.State == ConnectionState.Closed)
            {
                sqlconn.Open();
            }
            str1 = "select RFID,RFName,RFPath from FRtype";
            sqladp = new SqlDataAdapter(str1, sqlconn);
            sqladp.Fill(sqlds, "rftype");
                for (int i =0;i<=sqlds.Tables["rftype"].Rows.Count-1;i++ )
                {
                    dataGridView1.Rows.Add();
                    dataGridView1.Rows[i].Cells["RFID"].Value = sqlds.Tables["rftype"].Rows[i]["RFID"].ToString();
                    dataGridView1.Rows[i].Cells["RFName"].Value = sqlds.Tables["rftype"].Rows[i]["RFName"].ToString();
                    dataGridView1.Rows[i].Cells["RFPath"].Value = sqlds.Tables["rftype"].Rows[i]["RFPath"].ToString(); 
                }
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           //新增行
            dataGridView1.Rows.Add();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
