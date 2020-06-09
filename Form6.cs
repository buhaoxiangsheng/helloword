using Chanjet.TP.OpenAPI;
using grproLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ufida.T.EAP.Net;

namespace zySoft
{
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }
        private GridppReport report = new GridppReport();
        private GridppReport report1 = new GridppReport();
        private SqlConnection sqlconn = new SqlConnection();
        private SqlConnection kdconn = new SqlConnection();
        SerialPort CurrentPort = null;
        private int weightCount;
        string caccNums;
        string privateKeyPaths;
        string cmServerURLs;
        private void Form6_Load(object sender, EventArgs e)
        {
            try
            {

                weightCount = 1;
               
              
           
            //将数据插入到dataGridView1
            SqlDataAdapter sqladp = new SqlDataAdapter();
            DataTable dt = new DataTable();
            if (sqlconn.State == ConnectionState.Closed)
            {
                sqlconn.ConnectionString = Utility.DatabaseConnection;
                sqlconn.Open();
            }
            string str1 = "   select [voucharstate],[voucharcode],vouchartype,[vouchardate],[carcode],omcode,[partnernametwo],warehousename,[cinvname],batch,[unitid],[grossWeight],[tareWeight],[Redundant],[netWeight],auditstate,birthorder  from [weightbills] order by vouchardate desc";
            sqladp = new SqlDataAdapter(str1, sqlconn);
            sqladp.Fill(dt);
            dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("所发生错误：" + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            selectkh frms = new selectkh("存货", sqlconn, this);
            // frms.ShowDialog();
            frms.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            selectkh frms = new selectkh("往来单位", sqlconn, this);
            // frms.ShowDialog();
            frms.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            selectkh frms = new selectkh("批号", sqlconn, this);
            // frms.ShowDialog();
            frms.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            selectkh frms = new selectkh("仓库", sqlconn, this);
            // frms.ShowDialog();
            frms.Show();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            //全选
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == false)|| (Convert.ToString(dataGridView1.Rows[i].Cells[0].Value) == null))
                    {
                        dataGridView1.Rows[i].Cells[0].Value = "true";
                    }
                    else
                        continue;
                }
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {

            string str1 = "";
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            SqlCommand sqlcomm1 = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                List<string> list = new List<string>();
                if (dataGridView1.Rows.Count > 0)
                {
                    string voucode = "";
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true))
                        {
                            voucode = voucode+"'"+ Convert.ToString(dataGridView1.Rows[i].Cells["voucode"].Value)+"',";   
                        }
                    }
                 
                    if (voucode != "")
                    {
                        voucode = voucode.Substring(0, voucode.Length - 1);
                        if (MessageBox.Show("是否真要删除过磅单" + voucode + "？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            str1 = "delete from [weightbills] where [voucharcode] in (" + voucode + ") ";
                            sqlcomm1 = new SqlCommand(str1, sqlconn);
                            sqlcomm1.ExecuteNonQuery();
                            MessageBox.Show("单据删除成功！", "单据删除", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //重新载入数据
                            str1 = "  select [voucharstate],[voucharcode],vouchartype,[vouchardate],[carcode],omcode,[partnernametwo],warehousename,[cinvname],batch,[unitid],[grossWeight],[tareWeight],[Redundant],[netWeight],auditstate,birthorder  from [weightbills] order by vouchardate desc";
                            sqladp1 = new SqlDataAdapter(str1, sqlconn);
                            dt = new DataTable();
                            sqladp1.Fill(dt);
                            dataGridView1.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除过磅单时发生错误！" + ex.Message, "单据删除", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string str1 = "";
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            SqlCommand sqlcomm1 = new SqlCommand();
            DataTable dt = new DataTable();
            str1 = "select [voucharstate],[voucharcode],vouchartype,[vouchardate],[carcode],omcode,[partnernametwo],warehousename,[cinvname],batch,[unitid],[grossWeight],[tareWeight],[Redundant],[netWeight],auditstate,birthorder  from [weightbills]  where 1=1 ";
            if (dateTimePicker1.Value.ToString("yyyy-MM-dd")!=""&& dateTimePicker2.Value.ToString("yyyy-MM-dd") != "")
            {
                str1 += " and [vouchardate] between  '"+ dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'  " ;
            }
            if (textBox1.Text!="")
            {
                str1 += " and voucharcode like '%"+ textBox1.Text + "%'";
            }
            if (comboBox1.Text!="")
            {
                str1 += " AND voucharstate ='"+ comboBox1.Text + "'";
            }
            if (comboBox2.Text!="")
            {
                str1 += " and  auditstate='"+ comboBox2.Text + "'";
            }
            if (comboBox3.Text!="")
            {
                str1 += " and birthorder='"+ comboBox3.Text + "'";
            }
            if (comboBox4.Text!="") 
            {
                str1 += " and vouchartype='"+ comboBox4.Text + "'";
            }
            if (textBox2.Text!="")
            {
                str1 += " and  [carcode]='"+ textBox2.Text + "'";
            }
            if (comboBox7.Text!="")
            {
                str1 += " and [cinvname] like '%" + comboBox7.Text + "%'";
            }
            if (comboBox5.Text!="")
            {
                str1 += " and [partnernametwo]  like '%"+ comboBox5 .Text+ "%'";
            }
            if (textBox4.Text!="")
            {
                str1 += " and  batch  like '%"+ textBox4.Text + "%' ";
            }
            if (comboBox6.Text!="")
            {
                str1 += " and warehousename like'%"+ comboBox6.Text + "%'";
            }
            if (comboBox8.Text!="")
            {
                str1 += " and omcode like'%" + comboBox8.Text + "%'";
            }

            sqladp1 = new SqlDataAdapter(str1, sqlconn);
            dt = new DataTable();
            sqladp1.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            string str1 = "";
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            SqlCommand sqlcomm1 = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                List<string> list = new List<string>();
                if (dataGridView1.Rows.Count > 0)
                {
                    string voucode = "";
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true)&& (Convert.ToString(dataGridView1.Rows[i].Cells[16].Value).Equals("未审核")))
                        {
                            voucode = voucode + "'" + Convert.ToString(dataGridView1.Rows[i].Cells["voucode"].Value) + "',";
                        }
                    }

                    if (voucode != "")
                    {
                        voucode = voucode.Substring(0, voucode.Length - 1);
                        if (MessageBox.Show("是否审核过磅单" + voucode + "？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            str1 = "update   [weightbills] set  auditstate  ='已审核'  where [voucharcode] in (" + voucode + ") ";
                            sqlcomm1 = new SqlCommand(str1, sqlconn);
                            sqlcomm1.ExecuteNonQuery();
                            MessageBox.Show("单据审核成功！", "单据审核", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //重新载入数据
                            str1 = "  select [voucharstate],[voucharcode],vouchartype,[vouchardate],[carcode],omcode,[partnernametwo],warehousename,[cinvname],batch,[unitid],[grossWeight],[tareWeight],[Redundant],[netWeight],auditstate,birthorder  from [weightbills] order by vouchardate desc";
                            sqladp1 = new SqlDataAdapter(str1, sqlconn);
                            dt = new DataTable();
                            sqladp1.Fill(dt);
                            dataGridView1.DataSource = dt;
                        } 
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("审核过磅单时发生错误！" + ex.Message, "单据删除", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            string str1 = "";
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            SqlCommand sqlcomm1 = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                List<string> list = new List<string>();
                if (dataGridView1.Rows.Count > 0)
                {
                    string voucode = "";
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true)&&(Convert.ToString(dataGridView1.Rows[i].Cells[16].Value).Equals("已审核")))
                        {
                            voucode = voucode + "'" + Convert.ToString(dataGridView1.Rows[i].Cells["voucode"].Value) + "',";
                        }
                    }

                    if (voucode != "")
                    {
                        voucode = voucode.Substring(0, voucode.Length - 1);
                        if (MessageBox.Show("是否弃审过磅单" + voucode + "？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            str1 = "update   [weightbills] set  auditstate  ='未审核'  where [voucharcode] in (" + voucode + ") ";
                            sqlcomm1 = new SqlCommand(str1, sqlconn);
                            sqlcomm1.ExecuteNonQuery();
                            MessageBox.Show("单据弃审成功！", "单据弃审", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //重新载入数据
                            str1 = "   select [voucharstate],[voucharcode],vouchartype,[vouchardate],[carcode],omcode,[partnernametwo],warehousename,[cinvname],batch,[unitid],[grossWeight],[tareWeight],[Redundant],[netWeight],auditstate,birthorder  from [weightbills] order by vouchardate desc";
                            sqladp1 = new SqlDataAdapter(str1, sqlconn);
                            dt = new DataTable();
                            sqladp1.Fill(dt);
                            dataGridView1.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("弃审过磅单时发生错误！" + ex.Message, "单据删除", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //全取消
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true) || (Convert.ToString(dataGridView1.Rows[i].Cells[0].Value) == null))
                    {
                        dataGridView1.Rows[i].Cells[0].Value = "false";
                    }
                    else
                        continue;
                }
            }
        }
        string mess = "";
        //采购入库
        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

            string str1 = "";
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            SqlCommand sqlcomm1 = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {
                //获取自定义项
                str1 = "   	  select FieldName,Type from Eap_UserDefineItems  where VoucherNames like '%采购订单%' and VoucherNames like '%采购入库单%' ";
                sqladp1 = new SqlDataAdapter(str1, sqlconn);
                DataTable data = new DataTable();
                sqladp1.Fill(data);
                string pubstrhe = "";
                string pubstrhecs = "";
                string pubstrbd = "";
                string pubstrbdcs = "";
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    if (data.Rows[i]["Type"].ToString() == "1")
                    {
                        pubstrbd += "\"" + data.Rows[i]["FieldName"].ToString() + "\",";
                       
                    }
                    else if (data.Rows[i]["Type"].ToString() == "2")
                    {
                        pubstrhe += "\""+ data.Rows[i]["FieldName"].ToString() + "\",";
                      
                    }

                }
                if (pubstrhe.Length>0) { 
                pubstrhe = pubstrhe.Substring(0, pubstrhe.Length - 1);
                }
                if (pubstrbd.Length>0)
                {
                    pubstrbd = pubstrbd.Substring(0, pubstrbd.Length - 1);
                }
            




                List<string> list = new List<string>();
                if (dataGridView1.Rows.Count > 0)
                {
                    string voucode = "";
                    string omcode = "";
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        string ss = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value);
                        Boolean booleans = Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value);

                        if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true) && (Convert.ToString(dataGridView1.Rows[i].Cells[3].Value).Equals("入库单")) && (Convert.ToString(dataGridView1.Rows[i].Cells[17].Value).Equals("未生成")) && (Convert.ToString(dataGridView1.Rows[i].Cells[16].Value).Equals("已审核")))
                        {
                            voucode = voucode + "'" + Convert.ToString(dataGridView1.Rows[i].Cells["voucode"].Value) + "',";
                            omcode = omcode + "'" + Convert.ToString(dataGridView1.Rows[i].Cells["omcode"].Value) + "',";
                        }
                    }
                    if (voucode != "")
                    {

                        
                        voucode = voucode.Substring(0, voucode.Length - 1);
                        omcode = omcode.Substring(0, omcode.Length - 1);
                        str1 = "   select  *  from [weightbills]   where   [voucharcode] in(" + voucode + ") ";
                        sqladp1 = new SqlDataAdapter(str1, sqlconn);
                        dt = new DataTable();
                        sqladp1.Fill(dt);
                   
                        insertvouchar insertvouchar = new insertvouchar();
                     



                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string temppub = "";
                            if (pubstrhe.Length>0)
                            {
                                str1 = "  select " + pubstrhe + "  from SA_SaleOrder where id=" + dt.Rows[i]["omid"].ToString() + "";
                                sqladp1 = new SqlDataAdapter(str1, sqlconn);
                                DataTable dts = new DataTable();
                                sqladp1.Fill(dts);
                                if (dts.Rows.Count > 0)
                                {
                                for (int j =0;j<dts.Columns.Count;j++)
                                {
                                 
                                    if (dts.Rows[0][j].ToString()=="")
                                    {
                                        temppub += "\"" + "0" + "\",";
                                    }
                                  
                                    else { temppub += "\"" + dts.Rows[0][j].ToString() + "\","; }
                                    
                                }
                                
                                temppub = temppub.Substring(0, temppub.Length - 1);
                                }
                                else
                                {
                                    temppub += "\"" + "0" + "\",";
                                }
                            }

                            string temppubbd = "";
                            if (pubstrbd.Length > 0)
                            {
                                str1 = "   select   (" + pubstrbd + ") from SA_SaleOrder_b where idSaleOrderDTO=" + dt.Rows[i]["omid"].ToString() + "  and idinventory ='" + dt.Rows[i]["cinvid"].ToString() + "'";
                                sqladp1 = new SqlDataAdapter(str1, sqlconn);
                                DataTable dtse = new DataTable();
                                sqladp1.Fill(dtse);
                                if (dtse.Rows.Count> 0)
                                {

                              
                                
                                for (int j = 0; j < dtse.Columns.Count; j++)
                                {
                              
                                    if (dtse.Rows[0][j].ToString()=="")
                                    {
                                        temppubbd += "\"" + "0" + "\",";
                                    }
                                    else { 
                                    temppubbd += "\"" + dtse.Rows[0][j].ToString() + "\",";
                                    }

                                }
                                }
                                else
                                {
                                    temppubbd += "\"" + "0" + "\",";
                                }

                                temppubbd = temppubbd.Substring(0, temppubbd.Length - 1);
                            }

                         

                            string argss = "{dto:{" +
                              "VoucherDate: \"" + DateTime.Now.ToString("yyyy-MM-dd") + "\"," +
                              "  Code: \"" + dt.Rows[i]["omcode"].ToString() + "\"," +
                            "ExternalCode:\"" + dt.Rows[i]["voucharcode"].ToString() + "\"," +
                              "Partner: {Code: \"" + dt.Rows[i]["partnerIdtwocode"].ToString() + "\"}, " +
                              "VoucherType:{ code: \"ST1001\"}," +
                              "BusiType: {Code: \"01\"}," +
                              " Department: {Code:\"0200\"}," +
                              "Project: {Code: \"0101\"},";

                            if (pubstrhe.Length>0&& pubstrhe.Length>0)
                            {
                                argss += " DynamicPropertyKeys:[" + pubstrhe + "]," +
                           " DynamicPropertyValues:[" + temppub + "],";
                            }



                            argss += "RDRecordDetails: [{" +
                              "  Code:\"0001\"," +
                              "Inventory:{Code: \"" + dt.Rows[i]["cinvcode"].ToString() + "\"}," +
                                              "Unit: {Name:\"" + dt.Rows[i]["unitid"].ToString() + "\"}," +
                                              "BaseQuantity: " + dt.Rows[i]["netWeight"].ToString() + "," +
                                              " Price: " + dt.Rows[i]["unitprice"].ToString() + "," +
                                               " Amount: " + dt.Rows[i]["totalprice"].ToString() + "," +
                                              "Batch:\"" + dt.Rows[i]["batch"].ToString() + "\", ";
                            if(pubstrbd.Length>0&& temppubbd.Length>0){ 
                            argss += " DynamicPropertyKeys:[" + pubstrbd + "]," +
                            " DynamicPropertyValues:[" + temppubbd + "],";
                            }
                            argss += "Warehouse:{Code: \"" + dt.Rows[i]["warehousecode"].ToString() + "\"}" +

                                               "" +
                                              "}]" +
                                              "} }";
                            //    str1 = "   select  omcode,voucharcode,partnerIdtwocode  from [weightbills]   where   omcode in(" + omcode + ") ";
                         //str1 = "   select  omcode,voucharcode,partnerIdtwocode  from [weightbills]   where   omcode ="+ dt.Rows[i]["omcode"]+ " and voucharcode in ("+ voucode + ")  ";

                         //   sqladp1 = new SqlDataAdapter(str1, sqlconn);
                         // DataTable  data = new DataTable();
                         //   sqladp1.Fill(data);
                         //   for (int j = 0; j < data.Rows.Count; j++)
                         //   { 
                         //       argss += "

                           // eaptim = dt.Rows[i]["cinvcode"].ToString();

                            insertvouchar.insertdocument(CommonClass.usertoken, privateKeyPaths, cmServerURLs, "purchaseReceive/Create", argss, "入库单");
                            str1 = "  update   [weightbills] set  birthorder  ='已生成'  where [omcode]='"+dt.Rows[i]["omcode+" +
                                ""].ToString()+"'";
                            sqlcomm1 = new SqlCommand(str1, sqlconn);
                            sqlcomm1.ExecuteNonQuery();

                        }
                        MessageBox.Show("生成成功" + voucode + "？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                        
                        this.button1_Click(sender, e);
                    }
                }
                else
                {
                    MessageBox.Show("过磅单生成入库单时发生错误！ 请选择有效单据", "入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("过磅单生成入库单时发生错误！" + ex.Message, "入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        //销售出库
        private void toolStripTextBox4_Click(object sender, EventArgs e)
        {

            string str1 = "";
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            SqlCommand sqlcomm1 = new SqlCommand();
            DataTable dt = new DataTable();
            try
            {     //获取自定义项
                str1 = "   	  select FieldName,Type from Eap_UserDefineItems  where VoucherNames like '%销售订单%' and VoucherNames like '%销售出库单%' ";
                sqladp1 = new SqlDataAdapter(str1, sqlconn);
                DataTable data = new DataTable();
                sqladp1.Fill(data);
                string pubstrhe = "";
                string pubstrhecs = "";
                string pubstrbd = "";
                string pubstrbdcs = "";
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    if (data.Rows[i]["Type"].ToString() == "1")
                    {
                        pubstrbd += "\"" + data.Rows[i]["FieldName"].ToString() + "\",";

                    }
                    else if (data.Rows[i]["Type"].ToString() == "2")
                    {
                        pubstrhe += "\"" + data.Rows[i]["FieldName"].ToString() + "\",";

                    }

                }
                if (pubstrhe.Length > 0)
                {
                    pubstrhe = pubstrhe.Substring(0, pubstrhe.Length - 1);
                }
                if (pubstrbd.Length > 0)
                {
                    pubstrbd = pubstrbd.Substring(0, pubstrbd.Length - 1);
                }





                List<string> list = new List<string>();
                if (dataGridView1.Rows.Count > 0)
                {
                    string voucode = "";
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        string ss = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value);
                        Boolean booleans = Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value);

                        if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true) && (Convert.ToString(dataGridView1.Rows[i].Cells[4].Value).Equals("出库单")) && (Convert.ToString(dataGridView1.Rows[i].Cells[18].Value).Equals("未生成")) && (Convert.ToString(dataGridView1.Rows[i].Cells[17].Value).Equals("已审核")))
                        {
                            voucode = voucode + "'" + Convert.ToString(dataGridView1.Rows[i].Cells["voucode"].Value) + "',";
                        }
                    }
                    if (voucode != "")
                    {
                        voucode = voucode.Substring(0, voucode.Length - 1);
                        str1 = "   select  *  from [weightbills]   where [voucharcode] in (" + voucode + ") ";
                        sqladp1 = new SqlDataAdapter(str1, sqlconn);
                        dt = new DataTable();
                        sqladp1.Fill(dt);
                        insertvouchar insertvouchar = new insertvouchar();

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {

                            string eaptim = dt.Rows[i]["voucharcode"].ToString();


                            string temppub = "";
                            if (pubstrhe.Length > 0)
                            {
                                str1 = "  select " + pubstrhe + "  from PU_PurchaseOrder where id=" + dt.Rows[i]["omid"].ToString() + "";
                                sqladp1 = new SqlDataAdapter(str1, sqlconn);
                                DataTable dts = new DataTable();
                                sqladp1.Fill(dts);
                                if (dts.Rows.Count>0)
                                {
                                for (int j = 0; j < dts.Columns.Count; j++)
                                {
                                    if (dts.Rows[0][j].ToString() == "")
                                    {
                                        temppub += "\"" + "0" + "\",";
                                    }

                                    else { temppub += "\"" + dts.Rows[0][j].ToString() + "\","; }

                                }
                                temppub = temppub.Substring(0, temppub.Length - 1);
                                }
                                else
                                {
                                    temppub += "\"" + "0" + "\",";
                                }
                            }

                            string temppubbd = "";
                            if (pubstrbd.Length > 0)
                            {
                                str1 = "   select   (" + pubstrbd + ") from PU_PurchaseOrder_b where idPurchaseOrderDTO=" + dt.Rows[i]["omid"].ToString() + "  and idinventory ='" + dt.Rows[i]["cinvid"].ToString() + "'";
                                sqladp1 = new SqlDataAdapter(str1, sqlconn);
                                DataTable dtse = new DataTable();
                                sqladp1.Fill(dtse);
                                if (dtse.Rows.Count > 0)
                                {
                                    for (int j = 0; j < dtse.Columns.Count; j++)
                                    {
                                        if (dtse.Rows[0][j].ToString() == "")
                                        {
                                            temppubbd += "\"" + "0" + "\",";
                                        }
                                        else
                                        {
                                            temppubbd += "\"" + dtse.Rows[0][j].ToString() + "\",";
                                        }

                                    }

                                    temppubbd = temppubbd.Substring(0, temppubbd.Length - 1);
                                }
                                else
                                {
                                    temppubbd += "\"" + "0" + "\",";
                                }
                            }




                            string argss = "{" +
                                     "dto:{" +
                            " ExternalCode: \"" + dt.Rows[i]["voucharcode"].ToString() + "\"," +
                             "     Code:\"" + dt.Rows[i]["voucharcode"].ToString() + "\"," +
                               "   VoucherType: { Code: \"ST1021\"}," +
                                  "Partner: { Code: \"" + dt.Rows[i]["partnerIdtwocode"].ToString() + "\"}," +
                                    "Customer: { Code: \"" + dt.Rows[i]["partnerIdtwocode"].ToString() + "\"}," +
                                 " VoucherDate: \"" + DateTime.Now.ToString("yyyy-MM-dd") + "\"," +
                                "  BusiType: { Code: \"15\"},";
                            if (pubstrbd.Length > 0 && temppubbd.Length > 0)
                            {
                                argss += " DynamicPropertyKeys:[" + pubstrbd + "]," +
                                " DynamicPropertyValues:[" + temppubbd + "],";
                            }
                            argss +="Warehouse: { Code: \"" + dt.Rows[i]["warehousecode"].ToString() + "\"}," +
                              "    RDRecordDetails: [{" +
                                "    Code: \"0001\"," +
                               "    Inventory: { Code: \"" + dt.Rows[i]["cinvcode"].ToString() + "\"}," +

                                              "BaseQuantity: " + dt.Rows[i]["netWeight"].ToString() + "," +
                                              " Price: " + dt.Rows[i]["unitprice"].ToString() + "," +
                                               " Amount: " + dt.Rows[i]["totalprice"].ToString() + ",";
                            if (pubstrhe.Length > 0 && pubstrhe.Length > 0)
                            {
                                argss += " DynamicPropertyKeys:[" + pubstrhe + "]," +
                           " DynamicPropertyValues:[" + temppub + "],";
                            }



                            argss += "Batch:\"" + dt.Rows[i]["batch"].ToString() + "\" " +
                             "     }]" +
                             "     } }";
                                                       


                 

                            insertvouchar.insertdocument(CommonClass.usertoken, privateKeyPaths, cmServerURLs, "saleDispatch/Create", argss, "出库单");
                            str1 = "  update   [weightbills] set  birthorder  ='已生成'  where [voucharcode]='" + dt.Rows[i]["voucharcode"].ToString() + "'";
                            sqlcomm1 = new SqlCommand(str1, sqlconn);
                            sqlcomm1.ExecuteNonQuery();

                        }
                        MessageBox.Show("生成成功" + voucode + "？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                        this.button1_Click(sender, e);
                    }
                }
                else
                {
                    MessageBox.Show("过磅单生成出库单时发生错误！ 请选择有效单据", "出库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("过磅单生成出库单时发生错误！" + ex.Message, "出库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            selectkh frms = new selectkh("委外订单号", sqlconn, this);
            // frms.ShowDialog();
            frms.Show();

        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }
        //委外出库单
        private void toolStripTextBox2_Click(object sender, EventArgs e)
        {
            SqlTransaction tran = sqlconn.BeginTransaction();
         
            string str1 = "";
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            SqlCommand sqlcomm1 = new SqlCommand();
            sqlcomm1.Connection = sqlconn;
            DataTable dt = new DataTable();
            sqlcomm1.Transaction = tran;
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    string voucode = "";
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        string ss = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value);
                        Boolean booleans = Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value);

                        if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true) && (Convert.ToString(dataGridView1.Rows[i].Cells[4].Value).Equals("委外出库")) && (Convert.ToString(dataGridView1.Rows[i].Cells[18].Value).Equals("未生成")) && (Convert.ToString(dataGridView1.Rows[i].Cells[17].Value).Equals("已审核")))
                        {
                            voucode = voucode + "'" + Convert.ToString(dataGridView1.Rows[i].Cells["voucode"].Value) + "',";
                        }
                    }
                    if (voucode != "")
                    {
                        voucode = voucode.Substring(0, voucode.Length - 1);
                        str1 = "   select  omcode  from [weightbills]   where [voucharcode] in (" + voucode + ")  group by omcode ";
                        sqladp1 = new SqlDataAdapter(str1, sqlconn);
                        dt = new DataTable();
                        sqladp1.Fill(dt);


                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //获取最新编号
                            string selectsql = "  select COUNT(*)+1 from ST_RDRecord where   code like '%OD-"+ DateTime.Now.ToString("yyyy-MM") + "%'";
                            sqladp1 = new SqlDataAdapter(selectsql, sqlconn);
                            DataTable d1 = new DataTable();
                            sqladp1.Fill(d1);
                            string odcode = "000" + d1.Rows[0][0].ToString();
                            if (odcode.Length > 4)
                            {
                                odcode = odcode.Substring(odcode.Length - 4, odcode.Length);
                            }
                        odcode= "OD-"  + DateTime.Now.ToString("yyyy-MM") +"-"+  odcode;


                        //插入表头信息
                        selectsql = "	 select warehouseid,cinvid,partnerIdtwo,sum(totalprice) amount,omid,omcode from  weightbills  where omcode='" + dt.Rows[i][0].ToString() + "'  group by   warehouseid,cinvid,partnerIdtwo,omid,omcode";
                            sqladp1 = new SqlDataAdapter(selectsql, sqlconn);
                            d1 = new DataTable();
                            sqladp1.Fill(d1);

                            string sql = " insert into ST_RDRecord (code,sourceVoucherCode,amount,accountingperiod,accountingyear,VoucherYear,VoucherPeriod,ManufactureOrderCode,idbusitype,idpartner,idsettleCustomer,idrdstyle,voucherState,sourceVoucherId,idsourcevouchertype,idvouchertype,voucherdate,madedate,price2) values " +
                                        "('" + odcode + "','" + d1.Rows[0]["omcode"].ToString() + "'," + d1.Rows[0]["amount"].ToString() + ",5,'" + DateTime.Now.ToString("yyyy") + "','" + DateTime.Now.ToString("yyyy") + "',5,'" + d1.Rows[0]["omcode"].ToString() + "',10003," +
                                        "                      " + d1.Rows[0]["partnerIdtwo"].ToString() + "," + d1.Rows[0]["partnerIdtwo"].ToString() + ",10001,181,'" + d1.Rows[0]["omid"].ToString() + "',50002,50003,'" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "',1)";
                            sqlcomm1.CommandText = sql;

                             //sqlcomm1 = new SqlCommand(sql, sqlconn);
                            sqlcomm1.ExecuteNonQuery();
                          
                            str1 = "  select  * from weightbills  where omcode='" + dt.Rows[i][0].ToString() + "' vouchartype='委外出库'";
                            sqladp1 = new SqlDataAdapter(str1, sqlconn);
                            DataTable db = new DataTable();
                            sqladp1.Fill(db);



                            for (int j = 0; j < db.Rows.Count; j++)
                            {                                                                                     //x
                                string sql1 = " insert into ST_RDRecord_b(code,quantity,compositionQuantity,baseQuantity,price,amount,sourceVoucherCode,SourceVoucherCodeByMergedFlow,ManufactureOrderCode,idbusiTypeByMergedFlow,idinventory,idbaseunit,idunit,idwarehouse,sourceVoucherId,ManufactureOrderId,ManufactureOrderDetailId,sourceVoucherDetailId,ManufactureOrderMaterialDetailId,SourceVoucherIdByMergedFlow,SourceVoucherDetailIdByMergedFlow,idsourcevouchertype,idsourceVoucherTypeByMergedFlow,idRDRecordDTO,createdtime,CostAccountStatus,AccountTime,batch) values " +
                                    "( '"+"000"+j+"',"+db.Rows[j]["netWeight"].ToString() + ",'" + db.Rows[j]["netWeight"].ToString() + db.Rows[j]["unitid"].ToString() + "'," + db.Rows[j]["bdweight"].ToString() + "," + db.Rows[j]["unitprice"].ToString() + "," + db.Rows[j]["totalprice"].ToString() + ", '" + db.Rows[j]["omcode"].ToString() + "','" + db.Rows[j]["omcode"].ToString() + "','" + db.Rows[j]["omcode"].ToString() + "'," +
                                                                                                                                                                                                        "10003,"+ db.Rows[j]["cinvid"].ToString() + ",(select top 1 id from aa_unit where name ='" + db.Rows[j]["unitid"].ToString() + "'),(select top 1 id from aa_unit where name ='" + db.Rows[j]["unitid"].ToString() + "')," + db.Rows[j]["warehouseid"].ToString() + ","+ db.Rows[j]["omid"].ToString() + " ," + db.Rows[j]["omid"].ToString() + " ," + db.Rows[j]["omid"].ToString() + " ," +
                                                                                                                                                                                                                                                                                                                                              "(select b.id   from  OM_OutSourceOrder a join  OM_OutSourceOrder_Material  b on a.id=b.voucherId   where a.id="+ db.Rows[j]["omid"].ToString() + " and b.idinventory="+ db.Rows[j]["cinvid"].ToString() + "),(select b.id   from  OM_OutSourceOrder a join  OM_OutSourceOrder_Material  b on a.id=b.voucherId   where a.id=" + db.Rows[j]["omid"].ToString() + " and b.idinventory=" + db.Rows[j]["cinvid"].ToString() + ")," +
                                                                                                                                                                                                                                                                                                                                                                                                                                                      "" + db.Rows[j]["omid"].ToString() + ",(select b.id   from  OM_OutSourceOrder a join  OM_OutSourceOrder_Material  b on a.id=b.voucherId   where a.id=" + db.Rows[j]["omid"].ToString() + " and b.idinventory=" + db.Rows[j]["cinvid"].ToString() + "),50002,50002,(select max(id) from ST_RDRecord where sourceVoucherCode ='" + db.Rows[j]["omcode"].ToString() + "' and code like '%od%'),'" + DateTime.Now.ToString("yyyy-MM-dd") + "',51022,'" + DateTime.Now.ToString("yyyy-MM-dd") + "','"+ db.Rows[j]["batch"].ToString() + "') ";
                                //sqlcomm1 = new SqlCommand(sql1, sqlconn);
                                sqlcomm1.CommandText = sql1;
                                sqlcomm1.ExecuteNonQuery();
                            //if (db.Rows[j]["batch"].ToString() != null && db.Rows[j]["batch"].ToString() != "")
                            //{
                            //    str1 = "  update   [ST_CurrentStock] set  BaseQuantity  =(select BaseQuantity-" + db.Rows[j]["netWeight"].ToString() + "   from ST_CurrentStock  where idwarehouse=" + db.Rows[j]["warehouseid"].ToString() + " and idinventory=" + db.Rows[j]["cinvid"].ToString() + " and batch=" + db.Rows[j]["batch"].ToString() + " ),CanuseBaseQuantity  =(select CanuseBaseQuantity-" + db.Rows[j]["netWeight"].ToString() + "   from ST_CurrentStock  where idwarehouse=" + db.Rows[j]["warehouseid"].ToString() + " and idinventory=" + db.Rows[j]["cinvid"].ToString() + " and batch=" + db.Rows[j]["batch"].ToString() + " )    where  idwarehouse=" + db.Rows[j]["warehouseid"].ToString() + " and idinventory=" + db.Rows[j]["cinvid"].ToString() + " and batch=" + db.Rows[j]["batch"].ToString() + "";
                            //    sqlcomm1 = new SqlCommand(str1, sqlconn);
                            //    sqlcomm1.ExecuteNonQuery();
                            //}
                            str1 = "  update   [weightbills] set  birthorder  ='已生成'  where [voucharcode]='" + db.Rows[j]["voucharcode"].ToString() + "'";
                                //  sqlcomm1 = new SqlCommand(str1, sqlconn);
                                sqlcomm1.CommandText = str1;
                                sqlcomm1.ExecuteNonQuery();
                            }
                         
                        }
                        }
                        tran.Commit();
                        MessageBox.Show("生成成功" + voucode + "？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                        this.button1_Click(sender, e);
                    }
                
                else
                {
                    tran.Rollback();
                    MessageBox.Show("过磅单生成委外出料单时发生错误！ 请选择有效单据", "委外出料单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

             }
            catch (Exception ex)
            {
                tran.Rollback();
                MessageBox.Show("过磅单生成委外出料单时发生错误！" + ex.Message, "委外出料单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        //委外入库单
        private void toolStripTextBox3_Click(object sender, EventArgs e)
        {
            SqlTransaction tran = sqlconn.BeginTransaction();
            string str1 = "";
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            SqlCommand sqlcomm1 = new SqlCommand();
            DataTable dt = new DataTable();
            sqlcomm1.Transaction = tran;

            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    string voucode = "";
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        string ss = Convert.ToString(dataGridView1.Rows[i].Cells[3].Value);
                        Boolean booleans = Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value);

                        if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true) && (Convert.ToString(dataGridView1.Rows[i].Cells[4].Value).Equals("委外入库")) && (Convert.ToString(dataGridView1.Rows[i].Cells[18].Value).Equals("未生成")) && (Convert.ToString(dataGridView1.Rows[i].Cells[17].Value).Equals("已审核")))
                        {
                            voucode = voucode + "'" + Convert.ToString(dataGridView1.Rows[i].Cells["voucode"].Value) + "',";
                        }
                    }
                    if (voucode != "")
                    {
                        voucode = voucode.Substring(0, voucode.Length - 1);
                        str1 = "   select  omcode  from [weightbills]   where [voucharcode] in (" + voucode + ")  group by omcode ";
                        sqladp1 = new SqlDataAdapter(str1, sqlconn);
                        dt = new DataTable();
                        sqladp1.Fill(dt);


                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            //获取最新编号
                            string selectsql = "  select COUNT(*)+1 from ST_RDRecord where   code like '%WR-" + DateTime.Now.ToString("yyyy-MM") + "%'";
                            sqladp1 = new SqlDataAdapter(selectsql, sqlconn);
                            DataTable d1 = new DataTable();
                            sqladp1.Fill(d1);
                            string odcode = "000" + d1.Rows[0][0].ToString();
                            if (odcode.Length > 4)
                            {
                                odcode = odcode.Substring(odcode.Length - 4, odcode.Length);
                            }
                            odcode = "WR-" + DateTime.Now.ToString("yyyy-MM") +"-"+ odcode;


                            //插入表头信息
                            selectsql = "	 select warehouseid,cinvid,partnerIdtwo,sum(totalprice)amount,realprice,omid,omcode,voucharcode from  weightbills  where omcode='" + dt.Rows[i][0].ToString() + "'  group by   warehouseid,cinvid,partnerIdtwo,omid,omcode,realprice,voucharcode";
                            sqladp1 = new SqlDataAdapter(selectsql, sqlconn);
                            d1 = new DataTable();
                            sqladp1.Fill(d1);

                            string sql = " insert into ST_RDRecord (code,sourceVoucherCode,amount,accountingperiod,accountingyear,VoucherYear,VoucherPeriod,ManufactureOrderCode,idbusitype,idpartner,idsettleCustomer,idrdstyle,voucherState,sourceVoucherId,idsourcevouchertype,idvouchertype,voucherdate,madedate,rdDirectionFlag,idwarehouse,accountState,settleStatus ,TotalManuAmount,TotalOrigManuAmount) values " +
                                        "('" + odcode + "','" + d1.Rows[0]["omcode"].ToString() + "'," + d1.Rows[0]["amount"].ToString() + ",5,'" + DateTime.Now.ToString("yyyy") + "','" + DateTime.Now.ToString("yyyy") + "',5,'" + d1.Rows[0]["omcode"].ToString() + "',10001," +
                                        "                      " + d1.Rows[0]["partnerIdtwo"].ToString() + "," + d1.Rows[0]["partnerIdtwo"].ToString() + ",16,181,'" + d1.Rows[0]["omid"].ToString() + "',50002,50004,'" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "',1,"+ d1.Rows[0]["warehouseid"].ToString()   + ",338,599,( select  realprice*netWeight from weightbills  where [voucharcode]='" + d1.Rows[0]["voucharcode"].ToString() + "' ),( select  realprice*netWeight from weightbills  where [voucharcode]='" + d1.Rows[0]["voucharcode"].ToString() + "' ))";
                            // sqlcomm1 = new SqlCommand(sql, sqlconn);
                            sqlcomm1.CommandText = sql;
                            sqlcomm1.ExecuteNonQuery();

                            str1 = "  select  * from weightbills  where omcode='" + dt.Rows[i][0].ToString() + "' and vouchartype='委外入库' ";
                            sqladp1 = new SqlDataAdapter(str1, sqlconn);
                            DataTable db = new DataTable();
                            sqladp1.Fill(db);



                            for (int j = 0; j < db.Rows.Count; j++)
                            {                                                                                     //x
                                string sql1 = " insert into ST_RDRecord_b(code,quantity,compositionQuantity,baseQuantity,price,amount,sourceVoucherCode,SourceVoucherCodeByMergedFlow,ManufactureOrderCode,idbusiTypeByMergedFlow,idinventory,idbaseunit,idunit,idwarehouse,sourceVoucherId,ManufactureOrderId,ManufactureOrderDetailId,sourceVoucherDetailId,ManufactureOrderMaterialDetailId,SourceVoucherIdByMergedFlow,SourceVoucherDetailIdByMergedFlow,idsourcevouchertype,idsourceVoucherTypeByMergedFlow,idRDRecordDTO,createdtime,CostAccountStatus,AccountTime,price2,OrigManuPrice, OrigManuAmount,batch,pubuserdefdecm1,OrigManuPrice,OrigManuAmount) values " +
                                    "( '" + "000" + j + "'," + db.Rows[j]["netWeight"].ToString() + ",'" + db.Rows[j]["netWeight"].ToString() + db.Rows[j]["unitid"].ToString() + "'," + db.Rows[j]["netWeight"].ToString() + "," + db.Rows[j]["unitprice"].ToString() + "," + db.Rows[j]["totalprice"].ToString() + ", '" + db.Rows[j]["omcode"].ToString() + "','" + db.Rows[j]["omcode"].ToString() + "','" + db.Rows[j]["omcode"].ToString() + "'," +
                                                                                                                                                                                                        "10001," + db.Rows[j]["cinvid"].ToString() + ",(select top 1 id from aa_unit where name ='" + db.Rows[j]["unitid"].ToString() + "'),(select top 1 id from aa_unit where name ='" + db.Rows[j]["unitid"].ToString() + "')," + db.Rows[j]["warehouseid"].ToString() + "," + db.Rows[j]["omid"].ToString() + " ," + db.Rows[j]["omid"].ToString() + " ," + db.Rows[j]["omid"].ToString() + " ," +
                                                                                                                                                                                                                                                                                                                                              "(select b.id   from  OM_OutSourceOrder a join  OM_OutSourceOrder_Material  b on a.id=b.voucherId   where a.id=" + db.Rows[j]["omid"].ToString() + " and b.idinventory=" + db.Rows[j]["cinvid"].ToString() + "),(select b.id   from  OM_OutSourceOrder a join  OM_OutSourceOrder_Material  b on a.id=b.voucherId   where a.id=" + db.Rows[j]["omid"].ToString() + " and b.idinventory=" + db.Rows[j]["cinvid"].ToString() + ")," +
                                                                                                                                                                                                                                                                                                                                                                                                                                                      "" + db.Rows[j]["omid"].ToString() + ",(select b.id   from  OM_OutSourceOrder a join  OM_OutSourceOrder_Material  b on a.id=b.voucherId   where a.id=" + db.Rows[j]["omid"].ToString() + " and b.idinventory=" + db.Rows[j]["cinvid"].ToString() + "),50002,50002,(select max(id) from ST_RDRecord where sourceVoucherCode ='" + db.Rows[j]["omcode"].ToString() + "' and code like '%wr%'),'" + DateTime.Now.ToString("yyyy-MM-dd") + "',51022,'" + DateTime.Now.ToString("yyyy-MM-dd") + "',1,( select  realprice  from weightbills  where [voucharcode]='" + db.Rows[j]["voucharcode"].ToString() + "' ),( select  realprice*netWeight from weightbills  where [voucharcode]='" + db.Rows[j]["voucharcode"].ToString() + "' ),'"+ db.Rows[j]["batch"].ToString() + "'," +
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  "  " + db.Rows[j]["netWeight"].ToString() + ", " + db.Rows[j]["Proprice"].ToString() + "," + db.Rows[j]["procost"].ToString() + ",) ";
                                //    sqlcomm1 = new SqlCommand(sql1, sqlconn);
                                sqlcomm1.CommandText = sql1;
                                sqlcomm1.ExecuteNonQuery();
                                //if (db.Rows[j]["batch"].ToString() != null && db.Rows[j]["batch"].ToString() != "")
                                //{
                                //    str1 = "  update   [ST_CurrentStock] set  BaseQuantity  =(select BaseQuantity-" + db.Rows[j]["netWeight"].ToString() + "   from ST_CurrentStock  where idwarehouse=" + db.Rows[j]["warehouseid"].ToString() + " and idinventory=" + db.Rows[j]["cinvid"].ToString() + " and batch=" + db.Rows[j]["batch"].ToString() + " ),CanuseBaseQuantity  =(select CanuseBaseQuantity-" + db.Rows[j]["netWeight"].ToString() + "   from ST_CurrentStock  where idwarehouse=" + db.Rows[j]["warehouseid"].ToString() + " and idinventory=" + db.Rows[j]["cinvid"].ToString() + " and batch=" + db.Rows[j]["batch"].ToString() + " )    where  idwarehouse=" + db.Rows[j]["warehouseid"].ToString() + " and idinventory=" + db.Rows[j]["cinvid"].ToString() + " and batch=" + db.Rows[j]["batch"].ToString() + "";
                                //    sqlcomm1 = new SqlCommand(str1, sqlconn);
                                //    sqlcomm1.ExecuteNonQuery();
                                //}
                                str1 = "  update   [weightbills] set  birthorder  ='已生成'  where [voucharcode]='" + db.Rows[j]["voucharcode"].ToString() + "'";
                                //   sqlcomm1 = new SqlCommand(str1, sqlconn);
                                sqlcomm1.CommandText = str1;
                                sqlcomm1.ExecuteNonQuery();



                            }

                        }
                    }
                    tran.Commit();
                    MessageBox.Show("生成成功" + voucode + "？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);

                    this.button1_Click(sender, e);
                }

                else
                {
                    tran.Rollback();
                    MessageBox.Show("过磅单生成委外入库单时发生错误！ 请选择有效单据", "委外入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                tran.Rollback();
                MessageBox.Show("过磅单生成委外入库单时发生错误！" + ex.Message, "委外入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
