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
    public partial class checkvouch : Form
    {
        private SqlConnection sqlconn = new SqlConnection();
        private SqlConnection kdconn = new SqlConnection();
        public checkvouch()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "合格") { CommonClass.checkid = "0"; }
            else { CommonClass.checkid = "1"; }
            CommonClass.stockid = textBox1.Text;
            CommonClass.stockname = comboBox2.Text;
            CommonClass.saleAreaid = textBox2.Text;
            CommonClass.saleAreaName = saleName.Text;
            this.Close();
        }

        private void checkvouch_Load(object sender, EventArgs e)
        {

            SqlDataAdapter sqladp = new SqlDataAdapter();
            SqlCommand sqlcomm = new SqlCommand();
            DataSet sqlds = new DataSet();
            if (kdconn.State == ConnectionState.Closed)
            {
                kdconn.ConnectionString = Utility.kdConnection;
                kdconn.Open();
            }
            comboBox1.Items.Clear();
            comboBox1.Items.Add("合格");
            comboBox1.Items.Add("不合格");
            comboBox1.SelectedIndex = 0;
            string str1 = "select FItemID,FName  from t_Item where FItemClassID='3001'";
            sqladp = new SqlDataAdapter(str1, kdconn);
            sqladp.Fill(sqlds, "sale");
            saleName.Items.Clear();
            for (int i = 0; i < sqlds.Tables["sale"].Rows.Count; i++)
            {
                saleName.Items.Add(sqlds.Tables["sale"].Rows[i]["FName"].ToString());
            }
            saleName.SelectedIndex = 0; 
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDataAdapter sqladp = new SqlDataAdapter();
            SqlCommand sqlcomm = new SqlCommand();
            DataSet sqlds = new DataSet();
            if (comboBox1.Text == "合格")
            {
                string str1 = "select FItemID,Fname  from t_Stock where FTypeID='500' and FProperty='10'";
                sqladp = new SqlDataAdapter(str1, kdconn);
                sqlds = new DataSet();
                sqladp.Fill(sqlds, "stock");
                comboBox2.Items.Clear();
                for (int i = 0; i <= sqlds.Tables["Stock"].Rows.Count - 1; i++)
                {
                    textBox1.Text = sqlds.Tables["stock"].Rows[0]["FItemID"].ToString();
                    comboBox2.Items.Add(sqlds.Tables["stock"].Rows[i]["Fname"].ToString());
                }
            }
            if (comboBox1.Text == "不合格")
            {
                string str1 = "select FItemID,Fname  from t_Stock where FTypeID='500' and FProperty='12'";
                sqladp = new SqlDataAdapter(str1, kdconn);
                sqlds = new DataSet();
                sqladp.Fill(sqlds, "stock");
                for (int i = 0; i <= sqlds.Tables["Stock"].Rows.Count - 1; i++)
                {
                    comboBox2.Items.Clear();
                    textBox1.Text = sqlds.Tables["stock"].Rows[0]["FItemID"].ToString();
                    comboBox2.Items.Add(sqlds.Tables["stock"].Rows[i]["Fname"].ToString());
                }
            }
            comboBox2.SelectedIndex = 0;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //选择仓库更新id
                SqlConnection kdconn = new SqlConnection();        
                SqlDataAdapter sqladp = new SqlDataAdapter();
                DataSet sqlds = new DataSet();
                if (kdconn.State == ConnectionState.Closed)
                {
                    kdconn.ConnectionString = Utility.kdconnection;
                    kdconn.Open();
                }
                string str1 = "";
                str1 = "select FItemID,FName  from t_Stock where FName ='" + comboBox2.Text + "'";
                sqladp = new SqlDataAdapter(str1, kdconn);
                sqlds = new DataSet();
                sqladp.Fill(sqlds, "stock");
                if (sqlds.Tables["stock"].Rows.Count > 0)
                {
                    textBox1.Text = sqlds.Tables["stock"].Rows[0]["FItemID"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获得仓库发生错误！"+ex.Message , "退出提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //选择仓库更新id
                SqlConnection kdconn = new SqlConnection();
                SqlDataAdapter sqladp = new SqlDataAdapter();
                DataSet sqlds = new DataSet();
                if (kdconn.State == ConnectionState.Closed)
                {
                    kdconn.ConnectionString = Utility.kdconnection;
                    kdconn.Open();
                }
                string str1 = "select FItemID,FName  from t_Item where FItemClassID='3001' and FName ='" + saleName.Text + "'";
            sqladp = new SqlDataAdapter(str1, kdconn);
            sqladp.Fill(sqlds, "sale");
            if (sqlds.Tables["sale"].Rows.Count>0)
            {
                textBox2.Text= sqlds.Tables["sale"].Rows[0]["FItemID"].ToString();
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show("获得销售范围发生错误！" + ex.Message, "退出提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
