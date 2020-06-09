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
    public partial class inventory : Form
    {
        public inventory()
        {
            InitializeComponent();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //保存数据
            string str1 = "";

            SqlConnection sqlconn = new SqlConnection();
            SqlCommand sqlcomm = new SqlCommand();
            SqlDataAdapter sqladp = new SqlDataAdapter();
            DataSet sqlds = new DataSet();

                sqlconn.ConnectionString = Utility.DatabaseConnection;
                if (sqlconn.State == ConnectionState.Closed)
                {
                    sqlconn.Open();
                }
                if  (textBox1.Text !="" && textBox2.Text !="")
                {
                    str1 = "insert into AA_inventory(wlcode,wlname) values('" + textBox1.Text + "','" + textBox2.Text+ "')";
                    sqlcomm = new SqlCommand(str1, sqlconn);
                    sqlcomm.ExecuteNonQuery();
                }
                str1 = "select wlcode,wlname from AA_inventory";
                sqladp = new SqlDataAdapter(str1, sqlconn);
                sqladp.Fill(sqlds, "inv");
                if (sqlds.Tables["inv"].Rows.Count > 0)
                {
                    dataGridView1.DataSource = sqlds.Tables["inv"];
                    //dataGridView1.Rows[i].Cells["RFName"].Value = sqlds.Tables["rftype"].Rows[i]["wlname"].ToString();

                }
                //for (int i = 0; i <= sqlds.Tables["rftype"].Rows.Count - 1; i++)
                //{
                //    dataGridView1.Rows.Add();
                //    dataGridView1.Rows[i].Cells["RFID"].Value = sqlds.Tables["rftype"].Rows[i]["wlcode"].ToString();
                //    dataGridView1.Rows[i].Cells["RFName"].Value = sqlds.Tables["rftype"].Rows[i]["wlname"].ToString();

                //}
                MessageBox.Show("保存成功！", "提示");
                textBox1.Text = "";
                textBox2.Text = ""; 
        }

        private void inventory_Load(object sender, EventArgs e)
        {
            //载入表中的数据
            string str1 = "";
            SqlConnection sqlconn = new SqlConnection();
            SqlDataAdapter sqladp = new SqlDataAdapter();
            DataSet sqlds = new DataSet();
            sqlconn.ConnectionString = Utility.DatabaseConnection;
            if (sqlconn.State == ConnectionState.Closed)
            {
                sqlconn.Open();
            }
            str1 = "select wlcode,wlname from AA_inventory order by wlcode";
            sqladp = new SqlDataAdapter(str1, sqlconn);
            sqladp.Fill(sqlds, "rftype");
            if  (sqlds.Tables["rftype"].Rows.Count>0)
            {
                dataGridView1.DataSource = sqlds.Tables["rftype"];
                //dataGridView1.Rows[i].Cells["RFName"].Value = sqlds.Tables["rftype"].Rows[i]["wlname"].ToString();
                
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                textBox1.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["RFID"].Value.ToString();
                textBox2.Text = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["RFName"].Value.ToString();
            }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string str1 = "";
            SqlConnection sqlconn = new SqlConnection();
            SqlDataAdapter sqladp = new SqlDataAdapter();
            DataSet sqlds = new DataSet();
            sqlconn.ConnectionString = Utility.DatabaseConnection;
            if (sqlconn.State == ConnectionState.Closed)
            {
                sqlconn.Open();
            }
            if (textBox2.Text == "") 
            {
                MessageBox.Show("无法获得要删除的物料！", "删除提示：", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }
            if (MessageBox.Show("是否真要删除物料" + textBox2.Text + "？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                str1 = "delete  from aa_inventory where wlcode='" + textBox1.Text + "'";
                SqlCommand sqlcomm = new SqlCommand(str1, sqlconn);
                sqlcomm.ExecuteNonQuery();
            }
            str1 = "select wlcode,wlname from AA_inventory order by wlcode";
            sqladp = new SqlDataAdapter(str1, sqlconn);
            sqladp.Fill(sqlds, "rftype");
            if (sqlds.Tables["rftype"].Rows.Count > 0)
            {
                dataGridView1.DataSource = sqlds.Tables["rftype"];
                //dataGridView1.Rows[i].Cells["RFName"].Value = sqlds.Tables["rftype"].Rows[i]["wlname"].ToString();

            }
            MessageBox.Show("删除成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);
  
        }
    }
}
