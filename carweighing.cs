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

namespace zySoft
{
    public partial class carweighing : Form
    {
        private GridppReport report = new GridppReport();
        private GridppReport report1 = new GridppReport();
        private SqlConnection sqlconn = new SqlConnection();
        private SqlConnection kdconn = new SqlConnection();
        SerialPort CurrentPort = null;
        private int weightCount;
        DataTable dt = new DataTable();
        DataTable partable = new DataTable();
        DataTable partbletwo = new DataTable();
        DataTable invtable = new DataTable();
        DataTable whtable = new DataTable();
        public carweighing()
        {
            InitializeComponent();
        }
        private void carweighing_Load(object sender, EventArgs e)
        {
        
            try
            {

                weightCount = 1;
                
                SqlDataAdapter sqladp = new SqlDataAdapter();
                DataSet sqlds = new DataSet();
                if (sqlconn.State == ConnectionState.Closed)
                {
                    sqlconn.ConnectionString = Utility.DatabaseConnection;
                    sqlconn.Open();
                }

                //将数据插入到dataGridView1
                string str1 = "   select [voucharstate],[voucharcode],[vouchardate],[carcode],[partnernametwo],[cinvname],[unitid],[grossWeight],[tareWeight],[Redundant],[netWeight]  from [weightbills] order by vouchardate desc";
                sqladp = new SqlDataAdapter(str1, sqlconn);
                sqladp.Fill(dt);
                dataGridView1.DataSource = dt;

                //将数据库内值赋值给界面combox控件
      
                partbletwo= this.comboxvalue(comboBox2, label18, " select name,id,code from AA_Partner",sqlconn);
                invtable=  this.comboxvalue(comboBox5, label20, " select name,id,code from AA_Inventory", sqlconn);
                whtable=this.comboxvalue(comboBox4, label19, "select name,id,code from AA_Warehouse", sqlconn);



                //保存相关的参数
                string Current = Directory.GetCurrentDirectory();//获取当前根目录

                // 写入ini
                Ini ini = new Ini(Current + "/config.ini");
             string comset=    ini.ReadValue("Setting", "comset");
             int  baud =Convert.ToInt32( ini.ReadValue("Setting", "baud"));
                string oddeven = ini.ReadValue("Setting", "oddeven");
                string cease = ini.ReadValue("Setting", "cease");
                string data = ini.ReadValue("Setting", "data");

                //读取仪表数据
                CurrentPort = new SerialPort();
                CurrentPort.Close();
                CurrentPort.ReadBufferSize = 512;
                CurrentPort.PortName = comset;  //端口号 
                CurrentPort.BaudRate = baud; //比特率 
                CurrentPort.RtsEnable = true;
                /*
                 * 
                 * NONE
                    EVEN
                    ODD
                    MARK
                    SPACE
                                     * */
                if (oddeven== "NONE")
                {
                    CurrentPort.Parity = Parity.None;//奇偶校验 
                }
                else if (oddeven == "EVEN")
                {
                    CurrentPort.Parity = Parity.Even;//奇偶校验 
                }
                else if (oddeven == "ODD")
                {
                    CurrentPort.Parity = Parity.Odd;//奇偶校验 
                }
                else if (oddeven == "MARK")
                {
                    CurrentPort.Parity = Parity.Mark;//奇偶校验 
                }
                else if (oddeven == "SPACE")
                {
                    CurrentPort.Parity = Parity.Space;//奇偶校验 
                }
             
                //停止位 
                if (cease=="1")
                {
                    CurrentPort.StopBits = StopBits.One;//停止位 
                }
                else if(cease == "2"){
                    CurrentPort.StopBits = StopBits.Two;//停止位 
                }

           

                CurrentPort.DataBits = Convert.ToInt32(data);//数据位
                CurrentPort.ReadTimeout = 500;
                CurrentPort.RtsEnable = true;
                //读超时，即在1000内未读到数据就引起超时异常 
                //绑定数据接收事件，因为发送是被动的，所以你无法主动去获取别人发送的代码，只能通过这个事件来处理
                //   CurrentPort.DataReceived += serialPort1_DataReceived;
                CurrentPort.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                System.Threading.Thread.Sleep(100); //读取速度太慢，加Sleep延长读取时间, 不可缺少
                if (CurrentPort.IsOpen == true)
                {
                    MessageBox.Show("端口" + CurrentPort.PortName + "已经打开或者占用！发货获得地磅数据！", "标签打印", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    CurrentPort.Open();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("所发生错误：" + ex.Message);
            }

        }
       
        //当客户2发生变化时客户id和往来单位code跟随变化啊
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (partbletwo.Rows.Count > 0&& comboBox2.SelectedIndex!=-1)
            {
                label18.Text = partbletwo.Rows[comboBox2.SelectedIndex]["id"].ToString();
                label23.Text = partbletwo.Rows[comboBox2.SelectedIndex]["code"].ToString();

            }
            if (comboBox2.Text!=""&& comboBox5.Text!="")
            {
                monunitprice(label20.Text, label18.Text);
            }
        }
        //当存货发生变化时存货id和存货code 跟随变化啊
        private void comboBox5_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (invtable.Rows.Count > 0&& comboBox5.SelectedIndex!=-1)
            {
                label20.Text = invtable.Rows[comboBox5.SelectedIndex]["id"].ToString();
                label1.Text= invtable.Rows[comboBox5.SelectedIndex]["code"].ToString();
            }
            if (comboBox2.Text != "" && comboBox5.Text != "")
            {
                monunitprice(label20.Text, label18.Text);
            }
        }
        //当仓库发生变化时仓库id和仓库code跟随变化啊
        private void comboBox4_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (whtable.Rows.Count > 0&& comboBox4.SelectedIndex != -1)
            {
                label19.Text = whtable.Rows[comboBox4.SelectedIndex]["id"].ToString();
                label22.Text = whtable.Rows[comboBox4.SelectedIndex]["code"].ToString();
            }
        }
        
      

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && numericUpDown3.Text != "")
            {
                numericUpDown1.Text = (double.Parse(textBox2.Text) * double.Parse(numericUpDown3.Text)).ToString();
            }
            if (textBox2.Text != "" && textBox8.Text != "")
            {
                textBox7.Text = (double.Parse(textBox2.Text) * double.Parse(textBox8.Text)).ToString();
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStrip1_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {

        }
        //保存过磅单
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            //保存过磅单
            SqlDataAdapter sqladp = new SqlDataAdapter();
            SqlCommand sqlcomm = new SqlCommand();
            DataSet sqlds = new DataSet();
            if (sqlconn.State == ConnectionState.Closed)
            {
                sqlconn.ConnectionString = Utility.DatabaseConnection;
                sqlconn.Open();
            }
            string str1 = "";
            SqlTransaction kdtrans = null;
            Boolean bTransBegin = false;
            try {
                //单据状态
                string voucharstate = "部分称重";
                if (textBox5.Text!=""&& textBox1.Text!="")
                {
                    voucharstate = "称重完成";
                }
                string voucharcode = "";
          
              //车牌号
                string carcode = comboBox3.Text;
            
                //客户名称
                string partnernametwo = comboBox2.Text;
                //客户id
                string partneridtwo = label18.Text;
                //客户编号
                string partnercode = label23.Text;
                //存货id
                string cinvid = label20.Text;
                //存货名称
                string cinname = comboBox5.Text;
                //存货编号
                string cincode = label1.Text;
                //仓库名称
                string warehousename = comboBox4.Text;
                //仓库id
                string warehouseid = label19.Text;
                //仓库code
                string warehousecode = label22.Text;

                //毛重
                string grossWeight = textBox5.Text;
                //皮重
                string tareWeight = textBox1.Text;
                //敬重
                string netWeight = textBox2.Text;
                string bdweight = textBox6.Text;
                //扣杂
                string Redundant = textBox3.Text;
               
                //单价
                double unitprice = double.Parse(numericUpDown3.Text);

                //总价
                double totalprice = double.Parse(numericUpDown1.Text);
                //实收
                double realprice=0;
                string vouchartype = "";
                if (toolStripComboBox1.Text != "")
                {
                    vouchartype = toolStripComboBox1.Text;
                }
                else
                {
                    MessageBox.Show("保存失败！ 单据类型不能为空", "过磅单保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                string Proprice = "null";
                string procost = "null";
                if (textBox8.Text!=""&& textBox7.Text!="")
                {
                    Proprice = textBox8.Text;
                    procost = textBox7.Text;
                }
                string omid = "null";
                string omcode = "null";
                if (comboBox1.Text!="")
                {
                    omid = label25.Text;
                    omcode = comboBox1.Text;
                }
                //批号
                string batch = textBox4.Text;
                if (numericUpDown2.Text!="")
                {
                    realprice = double.Parse(numericUpDown2.Text);
                }
                string unit ;
                if(batch != "" && voucharstate != "" && carcode != ""  &&  cinvid != "" && warehousename != ""  && netWeight != "")
                {
                    
                    sqlcomm.Connection = sqlconn;
                    sqlcomm.CommandType = CommandType.Text;
                    sqlcomm.CommandText = "select name from AA_Unit where id =(select idunit from AA_Inventory  where id=" + cinvid + ")";
                    sqladp = new SqlDataAdapter(sqlcomm);
                    dt = new DataTable();
                    sqladp.Fill(dt);
                    unit = dt.Rows[0][0].ToString();
                    if (grossWeight=="" && netWeight == "")
                    {
                        MessageBox.Show("保存失败！ 毛重和皮重不能同时为空", "过磅单保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (netWeight=="")
                    {
                        netWeight = "0";
                    }
                    else if (grossWeight == "")
                    {
                        grossWeight = "0";
                    }
                    else if (Redundant == "")
                    {
                        Redundant = "0";
                    }
                    switch (weightCount)
                    {
                        case 1:

                            //单据编号 
                            sqlcomm.Connection = sqlconn;
                            sqlcomm.CommandType = CommandType.Text;
                            sqlcomm.CommandText = "select count(*)+1 from weightbills";
                            sqladp = new SqlDataAdapter(sqlcomm);
                            dt = new DataTable();
                            sqladp.Fill(dt);
                            string str = dt.Rows[0][0].ToString();
                            if (str.Length < 2)
                            {
                                str = "0" + str;
                            }

                          
                            voucharcode = "LD" + DateTime.Now.ToString("yyyyMMdd") + str;
                            

                            str = "INSERT INTO [dbo].[weightbills] ([voucharstate] ,[voucharcode]  ,[vouchardate]  ,[carcode] ,[partnerIdtwo],[partnernametwo],[warehouseid]   ,[warehousename]   ,[cinvid]   ,[cinvname]  ,[grossWeight]    ,[tareWeight]   ,[netWeight]  ,[Redundant] ,[unitid],[unitprice]  ,[totalprice]  ,[realprice],batch,vouchartype,auditstate,birthorder,partnerIdtwocode,warehousecode,cinvcode,omid,omcode,bdweight,Proprice,procost)"
                                + " VALUES('"+ voucharstate + "'   ,'"+ voucharcode+"'    ,'"+ DateTime.Now.ToString() + "' ,'"+ carcode+"' ," + partneridtwo + "   ,'" + partnernametwo + "'," + warehouseid + " ,'"+ warehousename+"' ,'"+ cinvid + "'   ,'"+ cinname + "'   ,"+ grossWeight + "  ,"+ tareWeight + "  ,"+ netWeight + " ,"+ Redundant + " ,'"+ unit + "' ," + unitprice + " ," + totalprice + " ," + realprice + ",'"+ batch + "','"+ vouchartype + "','未审核','未生成','"+ partnercode + "','"+ warehousecode + "','"+ cincode + "',"+omid+",'"+omcode+"',"+ bdweight + ","+ Proprice + ","+ procost + ")";
                            sqlcomm = new SqlCommand(str, sqlconn);
                            sqlcomm.ExecuteNonQuery();
                            MessageBox.Show("保存成功！", "过磅单保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.toolStripButton2_Click(sender, e);
                            break;
                        case 2:
                            voucharcode = label21.Text;
                            str = "UPDATE [dbo].[weightbills] SET [voucharstate] = '" + voucharstate + "'"
                                    + ",[carcode] = '" + carcode + "'"
                                     + ",[partnerIdtwo] = " + partneridtwo + ""
                                    + ",[partnernametwo] = '" + partnernametwo + "'"
                                    + ",[warehouseid] = " + warehouseid + ""
                                    + ",[warehousename] ='" + warehousename + "'"
                                    + ",[cinvid] = " + cinvid + ""
                                    + ",[cinvname] = '" + cinname + "'"
                                    + ",[grossWeight] = " + grossWeight + ""
                                    + ",[tareWeight] =" + tareWeight + " "
                                    + ",[netWeight] = " + netWeight + ""
                                    + ",[Redundant] = " + Redundant + ""
                                    + ",[unitid] = '" + unit + "'"
                                    + ",totalprice=" + totalprice + ""
                                    + ",unitprice=" + unitprice + ""
                                    + ",realprice=" + realprice + ""
                                    + ",vouchartype='" + vouchartype + "'"
                                    + ",batch='" + batch + "'"
                                    + ",partnerIdtwocode='" + partnercode + "'"
                                    + ",warehousecode='" + warehousecode + "'"
                                    + ",cinvcode='" + cincode + "'"
                                    + ",bdweight=" + bdweight + ""
                                     + ",Proprice=" + Proprice + ""
                                      + ",procost=" + procost + "";
                            if (omid!=null&& omid!="") { 

                                      str = str + ",omid=" + omid + "";
                                   }
                                       str = str + ",omcode='" + omcode + "'"

                                   + "  where voucharcode='" + voucharcode + "' ";
                                    
                            sqlcomm = new SqlCommand(str, sqlconn);
                            sqlcomm.ExecuteNonQuery();
                            this.toolStripButton2_Click(sender, e);
                            break;
                          
                        default:
                            break;
                    }

                    //重新载入数据
                    str1 = "  select  id,[voucharstate] ,[voucharcode]  ,[vouchardate]  ,[carcode]  ,[partnernametwo]     ,[cinvname] ,[unitid] ,[grossWeight]    ,[tareWeight]   ,[netWeight]  ,[Redundant]  from [weightbills] order by vouchardate desc";
                    sqladp = new SqlDataAdapter(str1, sqlconn);
                    dt = new DataTable();
                    sqladp.Fill(dt);
                    dataGridView1.DataSource = dt;

                }



              
            }
            catch (Exception ex)
            {
                if (bTransBegin == true)
                {
                    kdtrans.Rollback();
                }
                MessageBox.Show("发生错误：" + ex.Message, "数据保存", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //新增
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
           
            weightCount = 1;
     
            comboBox2.SelectedIndex = -1;
            comboBox3.Text = "";
            comboBox5.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            textBox5.Clear();
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            numericUpDown3.Text ="";
            numericUpDown2.Text = "";
            numericUpDown1.Text = "";
            textBox4.Text = "";
        }
        //修改
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            SqlDataAdapter sqladp = new SqlDataAdapter();
            SqlCommand sqlcomm = new SqlCommand();
            DataSet sqlds = new DataSet();
            weightCount = 2;
            //选择
            if (dataGridView1.RowCount > 0)
            {
                //for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                //{
                //    DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells["selcol"];
                //    Boolean flag = Convert.ToBoolean(checkCell.Value);
                //    if (flag == true)
                //    {
                //        Utility.voucherid = dataGridView1.Rows[i].Cells["Id"].Value.ToString();
                //        Utility.billno = dataGridView1.Rows[i].Cells["vouchcode"].Value.ToString();
                //        this.Close();
                //    }

                //}
                DataRow dr ;
                string  sss ="";
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    dr = (dataGridView1.SelectedRows[i].DataBoundItem as DataRowView).Row;
                    sss= dr["voucharcode"].ToString();
                    dr.Delete();
                }
              

                    string voucode = sss;
                sqlcomm.Connection = sqlconn;
                sqlcomm.CommandType = CommandType.Text;
                sqlcomm.CommandText = "select* from weightbills  where [voucharcode]='" + voucode + "'";
                sqladp = new SqlDataAdapter(sqlcomm);
                dt = new DataTable();
                sqladp.Fill(dt);
                if (dt.Rows.Count>0)
                {
                    
                    comboBox2.Text = dt.Rows[0]["partnernametwo"].ToString();
                    label18.Text = dt.Rows[0]["partnerIdtwo"].ToString();
                    comboBox3.Text= dt.Rows[0]["carcode"].ToString();
                    comboBox4.Text= dt.Rows[0]["warehousename"].ToString();
                    label19.Text= dt.Rows[0]["warehouseid"].ToString();
                    textBox5.Text= dt.Rows[0]["grossWeight"].ToString();
                    textBox1.Text = dt.Rows[0]["tareWeight"].ToString();
                    textBox2.Text=dt.Rows[0]["netWeight"].ToString();
                    textBox3.Text= dt.Rows[0]["Redundant"].ToString();
                    comboBox5.Text= dt.Rows[0]["cinvname"].ToString();
                    label20.Text= dt.Rows[0]["cinvid"].ToString();
                    numericUpDown3.Text=  dt.Rows[0]["unitprice"].ToString();
                    numericUpDown2.Text= dt.Rows[0]["totalprice"].ToString();
                    numericUpDown1.Text = dt.Rows[0]["realprice"].ToString();
                    textBox4.Text= dt.Rows[0]["batch"].ToString();
                    toolStripComboBox1.Text= dt.Rows[0]["vouchartype"].ToString();
                    comboBox1.Text = dt.Rows[0]["omcode"].ToString();
                    label25.Text = dt.Rows[0]["omid"].ToString();
                    textBox6.Text = dt.Rows[0]["bdweight"].ToString();
                    textBox8.Text = dt.Rows[0]["Proprice"].ToString();
                    textBox7.Text = dt.Rows[0]["procost"].ToString();
                    label21.Text = voucode;
                   //DataRowView drv = dataGridView1.SelectedRows[0].DataBoundItem as DataRowView;
                   // drv.Delete();
                }
              
            }

        }


        //保存过磅单
        private void toolStripButton1_Click(object sender, EventArgs e)
        {   
            SqlDataAdapter sqladp = new SqlDataAdapter();
            SqlCommand sqlcomm = new SqlCommand();
            DataSet sqlds = new DataSet();
            if (sqlconn.State == ConnectionState.Closed)
            {
                sqlconn.ConnectionString = Utility.DatabaseConnection;
                sqlconn.Open();
            }
            string str1 = "";
            //重新载入数据
            str1 = "   select id,[voucharstate] ,[voucharcode]  ,[vouchardate]  ,[carcode]  ,[partnernametwo],[cinvname] ,[unitid],[grossWeight]    ,[tareWeight]   ,[Redundant], [netWeight]  from [weightbills] order by vouchardate desc";
            sqladp = new SqlDataAdapter(str1, sqlconn);
            dt = new DataTable();
            sqladp.Fill(dt);
            int sss= dt.Rows.Count;
            dataGridView1.DataSource = dt;
            sss = dataGridView1.Rows.Count;
        }
        //删除单据
        private void toolStripButton6_Click(object sender, EventArgs e)
        {
      
            string str1 = "";
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            SqlCommand sqlcomm1 = new SqlCommand();
            DataSet sqlds1 = new DataSet();
            try {
                if (dataGridView1.Rows.Count > 0)
                {
                    string voucode = dataGridView1.CurrentRow.Cells["vouchcode"].Value.ToString();
              
                    if (voucode != "")
                    {
                        if (MessageBox.Show("是否真要删除过磅单" + voucode + "？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            str1 = "delete from [weightbills] where [voucharcode]='" + voucode + "' ";
                            sqlcomm1 = new SqlCommand(str1, sqlconn);
                            sqlcomm1.ExecuteNonQuery();
                            MessageBox.Show("单据删除成功！", "单据删除", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            //重新载入数据
                            str1 = "  select id,[voucharstate] ,[voucharcode]  ,[vouchardate]  ,[carcode]  ,[partnernametwo] ,[cinvname] ,[unitid] ,[grossWeight]    ,[tareWeight]   ,[netWeight]  ,[Redundant]  from [weightbills] order by vouchardate desc";
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
        
        public  DataTable  comboxvalue(ComboBox comboBoxemp, Label label, String s,SqlConnection sqlco )
        {
        

            SqlDataAdapter sqladp = new SqlDataAdapter();
            SqlCommand sqlcomm = new SqlCommand();
            DataTable dt = new DataTable();
       
            sqladp = new SqlDataAdapter(s, sqlco);
            sqladp.Fill(dt);
            comboBoxemp.DataSource = dt;
            comboBoxemp.DisplayMember = "name";
            comboBoxemp.DisplayMember = "name";
            comboBoxemp.SelectedIndex = -1;
            return dt;
        }
        //单据类型
        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            /*
             * 出库单
                入库单
                材料出库
                成品入库
             **/
            //if (toolStripComboBox1.Text == "入库单" || toolStripComboBox1.Text == "出库单")
            //{
            //    label26.Visible = false;
            //    label25.Visible = false;
            //    label24.Visible = false;
            //    comboBox1.Visible = false;
            //    button7.Visible = false;
            //}
            //else {
            //    label26.Visible = true;
            //    label25.Visible = true;
            //    label24.Visible = true;
            //    comboBox1.Visible = true;
            //    button7.Visible = true;
            //}
            if (toolStripComboBox1.Text == "入库单" )
            {
                label26.Text = "入库单编号";
                label25.Text = "入库单id";


            }
            else if(toolStripComboBox1.Text == "出库单"){
                label26.Text = "出库单编号";
                label25.Text = "出库单id";
            }

            if (toolStripComboBox1.Text == "入库单" || toolStripComboBox1.Text == "委外入库")
            {
                button6.Visible = false;
            } else if (toolStripComboBox1.Text == "出库单" || toolStripComboBox1.Text == "委外出库") {
                button6.Visible = true;
            }
            if (toolStripComboBox1.Text == "委外入库" || toolStripComboBox1.Text == "委外出库")
            {
                button3.Visible = false;
                button4.Visible = false;
             
                button5.Visible = false;
            }
            else
            {
                button3.Visible = true;
                button4.Visible = true;
                button6.Visible = true;
                button5.Visible = true;
            }
        }
        //更改价格
        private void  monunitprice(string inv, string per)
        {
            SqlDataAdapter sqladpss = new SqlDataAdapter();
            SqlCommand sqlcomm = new SqlCommand();
            DataTable dts ;
            if (sqlconn.State == ConnectionState.Closed)
            {
                sqlconn.ConnectionString = Utility.DatabaseConnection;
                sqlconn.Open();
            }
            string s;
            if (toolStripComboBox1.Text == "出库单" || toolStripComboBox1.Text == "委外出库")
            {
                s = "  select  transactionPrice  from AA_CustomerInventoryPrice where idinventory="+ inv + " and idcustomer="+ per + " ";
                sqlcomm = new SqlCommand(s, sqlconn);
                sqladpss = new SqlDataAdapter(sqlcomm);
                dts = new DataTable();
                sqladpss.Fill(dts);
                int ss = dts.Rows.Count;
                if (dts.Rows.Count > 0) { 
                numericUpDown3.Text = dts.Rows[0][0].ToString();
                }
            }
            else if (toolStripComboBox1.Text == "入库单" || toolStripComboBox1.Text == "委外入库")
            {
                s = "    select  transactionPrice  from AA_VendorInventoryPrice where idinventory=" + inv + " and idvendor=" + per + " ";
                sqladpss = new SqlDataAdapter(s, sqlconn);
                dts = new DataTable();
                sqladpss.Fill(dts);
                int ss = dts.Rows.Count;
                if (dts.Rows.Count>0) { 
                numericUpDown3.Text = dts.Rows[0][0].ToString();
                }
            }
        

        }
       
        private void numericUpDown3_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text!=""&& numericUpDown3.Text!="") { 
            numericUpDown1.Text = (double.Parse(textBox2.Text)*double.Parse(numericUpDown3.Text)).ToString();
            }
        }

        private void tbWeight_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

            numtextBox2();
        }
        //更改敬重
        private void numtextBox2()
        {
            double ss = 0;

            if (textBox5.Text != "")
            {
                ss += double.Parse(textBox5.Text);
            }
            if (textBox1.Text != "")
            {
                ss = ss - double.Parse(textBox1.Text);
            }

            if (textBox3.Text != "")
            {
                ss = ss - double.Parse(textBox3.Text);
            }

            //if (textBox6.Text != "")
            //{
            //    ss = ss - double.Parse(textBox6.Text);
            //}

            textBox2.Text = ss.ToString();
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                //获得地磅数据 地磅型号HT9800
                //每隔100Ms发送一组数据,每组数据有5帧,每帧数据有11位:1位起始位(0),8位数据位(D0-D7),2位停止位(1).其定义如下:
                //第1帧:D0-D7 ------  0FFH(起始位)
                //第2帧:D0-D2 ------  为小数点位置(0-5)//D3-NC  //D4-1 表示称重稳定 0表示称重示稳定 //D5-1 表示重量为负 0表示重量为正 //D6-NC  //D7-1表示超载
                //第3帧:D0-D7 ----- BCD1(称重值)
                //第4帧:D0-D7 ----- BCD2(称重值)
                //第5帧:D0-D7 ----- BCD3(称重值)
                //衡器兼容耀华地磅 12位ASCII为：02 2B 30 30 30 30 30 30 31 31 42 03  
                string strReceive = "";
                SerialPort sp = sender as SerialPort;
                if (sp == null) return;
                byte[] firstBytes = new byte[sp.BytesToRead];
                sp.Read(firstBytes, 0, firstBytes.Length);

                byte firstByte = Convert.ToByte(sp.ReadByte());
                if (firstByte == 0x11)
                {

                    //定义接收数据长度 
                    int bytesRead = sp.ReadBufferSize;
                    //数据接收字节数组
                    byte[] bytesData = new byte[bytesRead];
                    //接收字节 
                    byte byteData;

                    for (int i = 0; i < bytesRead - 1; i++)

                    {
                        byteData = Convert.ToByte(sp.ReadByte());
                        if (byteData == 0xff || byteData == 0xfe)//结束
                        {
                            break;
                        }

                        bytesData[i] = byteData;
                    }
                    //   strReceive = Encoding.Default.GetString(bytesData);
                    strReceive = BitConverter.ToString(bytesData);
                }
                if (this.IsHandleCreated)
                {
                    if (string.IsNullOrEmpty(strReceive) || strReceive.IndexOf("-") < 0 || strReceive.Length < 6)
                    {
                        return;
                    }

                    tbWeight.Invoke(new EventHandler(delegate { tbWeight.Text = GetWeightOfPort(strReceive); }));
                    //  BeginInvoke(interfaceUpdateHandle, strReceive);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.GetType().FullName);
            }
        }

        //private void Gettexts(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        //{
        //    this.Invoke(new Action(() => { MessageBox.Show(this, "text"); }));
        //}


        private string GetWeightOfPort(string weight)
        {
            //0000
            if (string.IsNullOrEmpty(weight) || weight.IndexOf("-") < 0 || weight.Length < 6)
            {
                return "0";
            }
            string[] strs = weight.Split('-');

            weight = strs[2] + strs[1] + strs[0];
            weight = int.Parse(weight.Substring(0, 6)).ToString();
            return weight;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            numtextBox2();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            numtextBox2();
        }

        private void numericUpDown1_TextChanged(object sender, EventArgs e)
        {

        }
        //打印
        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
       

            selectkh frms = new selectkh("往来单位",sqlconn,this);
          // frms.ShowDialog();
            frms.Show();
         
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            selectkh frms = new selectkh("仓库", sqlconn, this);
            // frms.ShowDialog();
            frms.Show();

        }

        private void button5_Click(object sender, EventArgs e)
        {

            selectkh frms = new selectkh("存货", sqlconn, this);
            // frms.ShowDialog();
            frms.Show();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            selectkh frms = new selectkh("批号", sqlconn, this);
            // frms.ShowDialog();
            frms.Show();

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (toolStripComboBox1.Text== "委外出库") { 
            selectkh frms = new selectkh("委外出库", sqlconn, this);
            // frms.ShowDialog();
            frms.Show();
            }
            else if(toolStripComboBox1.Text == "委外入库")
            {
                selectkh frms = new selectkh("委外入库", sqlconn, this);
                // frms.ShowDialog();
                frms.Show();
            }
            else if (toolStripComboBox1.Text == "入库单")
            {
                selectkh frms = new selectkh("入库单", sqlconn, this);
                // frms.ShowDialog();
                frms.Show();
            }
            else if (toolStripComboBox1.Text == "出库单")
            {
                selectkh frms = new selectkh("出库单", sqlconn, this);
                // frms.ShowDialog();
                frms.Show();
            }
        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            numtextBox2();
        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox8.Text != "")
            {
                textBox7.Text = (double.Parse(textBox2.Text) * double.Parse(textBox8.Text)).ToString();
            }
        }
    }
}
