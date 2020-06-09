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
    public partial class WeightSelect : Form
    {
        public WeightSelect()
        {
            InitializeComponent();
        }

        private void WeightSelect_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //过滤任务单
                 SqlConnection sqlconn = new SqlConnection();
                 SqlConnection kdconn = new SqlConnection();
                SqlDataAdapter sqladp = new SqlDataAdapter();
                DataSet sqlds = new DataSet();

                    if (sqlconn.State == ConnectionState.Closed)
                    {
                        sqlconn.ConnectionString = Utility.DatabaseConnection;
                        sqlconn.Open();
                    }
                    string str1 = "select vouchcode,vouchdate,carno,partnerName,cinvname,grossWeight,tareWeight from weightbills  where 1=1 and (isnull(grossWeight,0)=0 or isnull(tareWeight,0)=0) ";
                         string start = Convert.ToDateTime(startdate.Text).ToString("yyyy-MM-dd");
                         if (start != "") { str1 = str1 + " and CONVERT(nvarchar(10),vouchdate,120)='" + start + "'"; }
                         if (textBox2.Text != "") { str1 = str1 + " and carno like'%" + textBox2.Text + "%'"; }
                         if (textBox1.Text != "") { str1 = str1 + " and partnerName like'%" + textBox1.Text + "%'"; }
                         sqladp = new SqlDataAdapter(str1, sqlconn);
                    sqladp.Fill(sqlds, "ICMO");
                    dataGridView1.DataSource = sqlds.Tables["ICMO"];
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询过程中发生错误：" + ex.Message, "查询提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //选择
            if (dataGridView1.RowCount > 0)
            {
                for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                {
                    DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells["selcol"];
                    Boolean flag = Convert.ToBoolean(checkCell.Value);
                    if (flag == true)
                    {
                        Utility.billno = dataGridView1.Rows[i].Cells["voucode"].Value.ToString();
                        this.Close();
                    }

                }
            }
        }
    }
}
