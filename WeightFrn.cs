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
using System.IO.Ports;
using grproLib;

namespace zySoft
{
    public partial class WeightFrn : Form
    {
        private GridppReport report = new GridppReport();
        private GridppReport report1 = new GridppReport();
        private SqlConnection sqlconn = new SqlConnection();
        private SqlConnection kdconn = new SqlConnection();
        SerialPort CurrentPort = null;
        private int weightCount;
        public WeightFrn()
        {
            InitializeComponent();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            try
            {
                dateTimePicker1.Text = DateTime.Now.ToString();
                radioButton1.Checked = true;
                radioButton2.Checked = false;
                weightCount = 0;
                ////读取kd连接
                //string Current = Directory.GetCurrentDirectory();//获取当前根目录
                //Ini ini = new Ini(Current + "/config.ini");
                //if (File.Exists(Current + "/config.ini"))
                //{
                //    //存在配置文件，读取相关信息
                //    string servertxt = ini.ReadValue("DbInterface", "ServerName");
                //    string satxt = ini.ReadValue("DbInterface", "Password");
                //    string dbtxt = ini.ReadValue("DbInterface", "dbName");
                //    string usertxt = ini.ReadValue("DbInterface", "UserName");
                //    Utility.kdconnection = "Data Source=" + servertxt + ";User ID=" + usertxt + ";pwd=" + satxt + ";Initial Catalog=" + dbtxt + ";Pooling=true;Max Pool Size=300;Min Pool Size=0;Connection Lifetime=300;";
                //}
                //else
                //{
                //    MessageBox.Show("请先进行连接配置！", "登陆提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    this.Close();

                //}              
                //SqlDataAdapter sqladp = new SqlDataAdapter();
                //DataSet sqlds = new DataSet();
                //if (sqlconn.State == ConnectionState.Closed)
                //{
                //    sqlconn.ConnectionString = Utility.DatabaseConnection;
                //    sqlconn.Open();
                //}
                //startDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");  
                //string str1 = "select vouchcode,vouchdate,carno,partnerId,partnerName,cinvid,cinvname,grossWeight,tareWeight,netWeight,water,dhValue,netValue,cmemo from weightbills order by vouchdate desc";
                //sqladp = new SqlDataAdapter(str1, sqlconn);
                //sqladp.Fill(sqlds, "vouch");
                //dataGridView1.DataSource = sqlds.Tables["vouch"]; 
                //axGRPrintViewer1.Visible = false;
                //dataGridView1.Columns["partnerId"].Visible = false;
                //dataGridView1.Columns["cinvid"].Visible = false;
                //if (CommonClass.userCode == "admin")
                //{
                //    checkBox1.Enabled = true;
                //}
                //else
                //{
                //    checkBox1.Enabled = false;
                //}
                if (File.Exists(Application.StartupPath + "\\Weightbill.grf") == false)
                {
                    MessageBox.Show("************************************************************/r/n" +
                           "没有Weightbill.grf！请确认板件明细文件是否存在！/r/n" +
                           "************************************************************");
                }
                report.LoadFromFile(Application.StartupPath + "\\Weightbill.grf");
                report.DetailGrid.Recordset.ConnectionString = Utility.reportConnection;
                if (File.Exists(Application.StartupPath + "\\Weightbill1.grf") == false)
                {
                    MessageBox.Show("************************************************************/r/n" +
                           "没有Weightbill1.grf！请确认板件明细文件是否存在！/r/n" +
                           "************************************************************");
                }
                report1.LoadFromFile(Application.StartupPath + "\\Weightbill1.grf");
                report1.DetailGrid.Recordset.ConnectionString = Utility.reportConnection;
                //关联报表取数事件
                report.Initialize += new _IGridppReportEvents_InitializeEventHandler(ReportInitialize);
                //读取仪表数据
          
                CurrentPort = new SerialPort();
                CurrentPort.Close();
                CurrentPort.ReadBufferSize = 512;
                CurrentPort.PortName = "COM3";  //端口号 
                CurrentPort.BaudRate = 4800; //比特率 
                CurrentPort.Parity = Parity.None;//奇偶校验 
                CurrentPort.StopBits = StopBits.One;//停止位 
                CurrentPort.DataBits = Convert.ToInt32("8");//数据位
                CurrentPort.ReadTimeout = 500;
                //准备就绪              
          //  CurrentPort.DtrEnable = true;
                CurrentPort.RtsEnable = true;
                //设置数据读取超时为1秒

                //读超时，即在1000内未读到数据就引起超时异常 
                //绑定数据接收事件，因为发送是被动的，所以你无法主动去获取别人发送的代码，只能通过这个事件来处理
         
                    CurrentPort.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                
                    System.Threading.Thread.Sleep(100); //读取速度太慢，加Sleep延长读取时间, 不可缺少

                //if (CurrentPort.IsOpen == false)
                //{
                //    CurrentPort.Open();
                //}
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
        //获取查询月份
        private void ReportInitialize()
        {
            if (radioButton1.Checked == true)
            {
                report.DetailGrid.Recordset.QuerySQL = "select * from weightbills where vouchcode='" + textBox1.Text + "'";
                // report.ParameterByName("strWhere").AsString = strwhere;
            }
            else
            {
                report1.DetailGrid.Recordset.QuerySQL = "select * from weightbills where vouchcode='" + textBox1.Text + "'";
            }

        }


     
        //private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        //{
        //    try
        //    {
        //        //获得地磅数据 地磅型号HT9800
        //        //每隔100Ms发送一组数据,每组数据有5帧,每帧数据有11位:1位起始位(0),8位数据位(D0-D7),2位停止位(1).其定义如下:
        //        //第1帧:D0-D7 ------  0FFH(起始位)
        //        //第2帧:D0-D2 ------  为小数点位置(0-5)//D3-NC  //D4-1 表示称重稳定 0表示称重示稳定 //D5-1 表示重量为负 0表示重量为正 //D6-NC  //D7-1表示超载
        //        //第3帧:D0-D7 ----- BCD1(称重值)
        //        //第4帧:D0-D7 ----- BCD2(称重值)
        //        //第5帧:D0-D7 ----- BCD3(称重值)
        //        //衡器兼容耀华地磅 12位ASCII为：02 2B 30 30 30 30 30 30 31 31 42 03  
        //        string strReceive = "";
        //        SerialPort sp = sender as SerialPort;
     
        //        if (CurrentPort == null) return;


        //        byte[] firstBytes = new byte[sp.BytesToRead];
        //        CurrentPort.Read(firstBytes, 0, firstBytes.Length);

        //        byte firstByte = Convert.ToByte(sp.ReadByte());


            

        //        if (firstByte == 0x11)
        //        {
        //            //定义接收数据长度 
        //            int bytesRead = sp.ReadBufferSize;
        //            //数据接收字节数组
        //            byte[] bytesData = new byte[bytesRead];
        //            //接收字节 
        //            byte byteData;

        //            for (int i = 0; i < bytesRead - 1; i++)
        //            {
        //                byteData = Convert.ToByte(sp.ReadByte());


        //                if (byteData == 0xFF)//结束
        //                {
        //                    break;
        //                }
        //                bytesData[i] = byteData;
        //            }
        //            strReceive = Encoding.Default.GetString(bytesData);
        //        }
            
        //        if (this.IsHandleCreated)
        //        {
        //            tbWeight.Invoke(new EventHandler(delegate { tbWeight.Text = GetWeightOfPort(strReceive); }));
        //            //  BeginInvoke(interfaceUpdateHandle, strReceive);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message + ex.GetType().FullName);
        //    }
        //}
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
                        if (byteData == 0xff|| byteData == 0xfe)//结束
                        {
                            break;
                        }
                        
                         bytesData[i] = byteData;
                    }
                    //   strReceive = Encoding.Default.GetString(bytesData);
                    strReceive =  BitConverter.ToString(bytesData);
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
            string[] strs= weight.Split('-');

            weight = strs[2] + strs[1] + strs[0];
            weight = int.Parse(weight.Substring(0, 6)).ToString() ;
            return weight;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            //新增单据
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            DataSet sqlds1 = new DataSet();
            if (kdconn.State == ConnectionState.Closed)
            {
                kdconn.ConnectionString = Utility.kdconnection;
                kdconn.Open();
            }
            weightCount = 1;
           
            string str1 = "";
            if (radioButton1.Checked == true)
            {
                textBox1.Text = "YL000001";
                str1 = "select  (case when MAX(vouchcode)=null then 1 else  convert(int,substring(MAX(vouchcode),3,6)+1) end) voucode  from weightbills where vouchtype='采购'";
                sqladp1 = new SqlDataAdapter(str1, sqlconn);
                sqladp1.Fill(sqlds1, "voucode");
                string voucode = sqlds1.Tables["voucode"].Rows[0]["voucode"].ToString();
                if (voucode != "")
                {
                    textBox1.Text = "YL000000".Substring(0, 8 - voucode.Length) + voucode;
                }
            }
            else
            {
                textBox1.Text = "CP000001";
                str1 = "select  (case when MAX(vouchcode)=null then 1 else  convert(int,substring(MAX(vouchcode),3,6)+1) end) voucode  from weightbills where vouchtype='销售'";
                sqladp1 = new SqlDataAdapter(str1, sqlconn);
                sqladp1.Fill(sqlds1, "voucode");
                string voucode = sqlds1.Tables["voucode"].Rows[0]["voucode"].ToString();
                if (voucode != "")
                {
                    textBox1.Text = "CP000000".Substring(0, 8 - voucode.Length) + voucode;
                }
            }

            if (radioButton1.Checked == true)
            {
                str1 = "select FItemID,FName,FModel from t_ICItem where left(FNumber,2) ='01'";
            }
            else
            {
                str1 = "select FItemID,FName,FModel from t_ICItem where left(FNumber,2) ='02'";
            }
            sqladp1 = new SqlDataAdapter(str1, kdconn);
            sqladp1.Fill(sqlds1, "inv");
            comboBox2.Items.Clear();
            for (int i = 0; i <= sqlds1.Tables["inv"].Rows.Count - 1; i++)
            {
               // invid.Text = sqlds1.Tables["inv"].Rows[0]["FName"].ToString();
                comboBox2.Items.Add(sqlds1.Tables["inv"].Rows[i]["FName"].ToString());
                //comboBox2.SelectedIndex = 0;
            }
            if (radioButton1.Checked == true)
            {
                str1 = "select FItemID,FName from t_Supplier";
                sqladp1 = new SqlDataAdapter(str1, kdconn);
                sqladp1.Fill(sqlds1, "Supplier");
                comboBox1.Items.Clear();
                for (int i = 0; i <= sqlds1.Tables["Supplier"].Rows.Count - 1; i++)
                {
                    //suppid.Text = sqlds1.Tables["Supplier"].Rows[0]["FName"].ToString();
                    comboBox1.Items.Add(sqlds1.Tables["Supplier"].Rows[i]["FName"].ToString());
                    //comboBox1.SelectedIndex = 0;
                }

            }
            else
            {
                str1 = "select FItemID,FName  from t_Organization ";
                sqladp1 = new SqlDataAdapter(str1, kdconn);
                sqladp1.Fill(sqlds1, "customer");
                comboBox1.Items.Clear();
                for (int i = 0; i <= sqlds1.Tables["customer"].Rows.Count - 1; i++)
                {
                   // suppid.Text = sqlds1.Tables["customer"].Rows[0]["FName"].ToString();
                    comboBox1.Items.Add(sqlds1.Tables["customer"].Rows[i]["FName"].ToString());
                   // comboBox1.SelectedIndex = 0;
                }
            }
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str1 = "";
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            DataSet sqlds1 = new DataSet();
            if (kdconn.State == ConnectionState.Closed)
            {
                kdconn.ConnectionString = Utility.kdconnection;
                kdconn.Open();
            }
            if (comboBox1.Text !="")
            {
                if (radioButton1.Checked == true)
                {
                    str1 = "select FItemID,FName  from t_Supplier  where FName='" + comboBox1.Text + "'";
                    sqladp1 = new SqlDataAdapter(str1, kdconn);
                    sqladp1.Fill(sqlds1, "Supplier");
                    if (sqlds1.Tables["Supplier"].Rows.Count > 0)
                    {
                        suppid.Text = sqlds1.Tables["Supplier"].Rows[0]["FItemID"].ToString(); 
                    }
                }
                else
                {
                    str1 = "select FItemID,FName  from t_Organization  where FName='" + comboBox1.Text + "'";
                    sqladp1 = new SqlDataAdapter(str1, kdconn);
                    sqladp1.Fill(sqlds1, "customer");
                    if (sqlds1.Tables["customer"].Rows.Count > 0)
                    {
                        suppid.Text = sqlds1.Tables["customer"].Rows[0]["FItemID"].ToString();
                    }
                }

            }
        }

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
            try
            {
                string voucode=textBox1.Text;
                string voudate=System.DateTime.Now.ToString("yyyy-MM-dd")  ;
                string venid=suppid.Text;
                string venname=comboBox1.Text;
                string carno=textBox2.Text;
                string cinvid=invid.Text;
                string cinvname=comboBox2.Text;
                string cinvstd=textBox3.Text;
                string gWeight=textBox4.Text;
                string pWeight=textBox5.Text;
                string jWeight=textBox6.Text;
                string water=textBox7.Text;
                string dhvalue=textBox8.Text;
                string netvalue=textBox9.Text;
                string bz=textBox10.Text;
                string vouchtype="";
                string stockid="";
                if (voucode != "" && voudate != "" && venid != "" && venname != "" && carno != "" && cinvid != "" && cinvname != "")
                {
                    switch (weightCount)
                    {
                        case 1:
                                if (weightCount == 1 && (gWeight != "" || pWeight != ""))
                                {
                                    if (radioButton1.Checked == true) { vouchtype = "采购"; }
                                    if (radioButton2.Checked == true) { vouchtype = "销售"; }
                                    str1 = "insert into weightbills(vouchtype,vouchcode,vouchdate,carno,partnerId,partnerName,cinvid,cinvname,cinvstd,grossWeight,tareWeight,netWeight,water,dhValue,netValue,cmemo,WeightTime1,cmaker,unitID) values('" + vouchtype + "','" + voucode + "','" + voudate + "','" + carno
                                             + "','" + venid + "','" + venname + "','" + cinvid + "','" + cinvname + "','" + cinvstd + "','" + gWeight + "','" + pWeight + "','" + jWeight + "','" + water + "','" + dhvalue + "','" + netvalue + "','" + bz + "','" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "','" + CommonClass.userName + "','" + unitID.Text + "')";
                                    sqlcomm = new SqlCommand(str1, sqlconn);
                                    sqlcomm.ExecuteNonQuery();
                                    MessageBox.Show("保存成功！", "过磅单保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            break; 
                        case 2:
                            if (gWeight != "" && pWeight != "" && Convert.ToInt32(gWeight) >0 && Convert.ToInt32(pWeight) >0)
                            {
                                if (radioButton1.Checked == true) { vouchtype = "采购"; }
                                if (radioButton2.Checked == true) { vouchtype = "销售"; }
                                str1 = "update  weightbills set grossWeight='" + textBox4.Text + "',tareWeight='" + textBox5.Text + "',netWeight='" + textBox6.Text + "',water='" + textBox7.Text + "',dhValue='" + textBox8.Text + "',netValue='" + textBox9.Text + "',cmemo='" + textBox10.Text + "',WeightTime2='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where vouchcode='" + textBox1.Text + "'";
                                sqlcomm = new SqlCommand(str1, sqlconn);
                                sqlcomm.ExecuteNonQuery();
                                MessageBox.Show("保存成功！", "过磅单保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                kdtrans = kdconn.BeginTransaction();
                                bTransBegin = true;
                                //完成二次过磅后生成金蝶的单据
                                if (jWeight != "" && Convert.ToInt32(pWeight) > 0)
                                {
                                    string vouchcode = "";
                                    string dbvoucode = "";
                                    string userid = "16393";
                                    string kdname = "";
                                    string ffmanager = "3105";
                                    string fsmanager = "3105";
                                    string finterid = "";
                                    str1 = "select  top 1 kduserid,kdusername from aa_user where username = '" + CommonClass.userName + "'";
                                    sqladp = new SqlDataAdapter(str1, sqlconn);
                                    sqlds = new DataSet();
                                    sqladp.Fill(sqlds, "user");
                                    if (sqlds.Tables["user"].Rows.Count > 0)
                                    {
                                        userid = sqlds.Tables["user"].Rows[0]["kduserid"].ToString();
                                        kdname = sqlds.Tables["user"].Rows[0]["kdusername"].ToString();

                                    }
                                    if (userid == "")
                                    {
                                        MessageBox.Show("无法获得用户对应的金蝶id，请进行用户设置", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    //保管员
                                    str1 = "select top 1 FItemID  from t_Emp where FName='" + kdname + "'";
                                    sqladp = new SqlDataAdapter(str1, kdconn);
                                    sqladp.SelectCommand.Transaction = kdtrans;
                                    sqlds = new DataSet();
                                    sqladp.Fill(sqlds, "Emp");
                                    if (sqlds.Tables["Emp"].Rows.Count > 0)
                                    {
                                        ffmanager = sqlds.Tables["Emp"].Rows[0]["FItemID"].ToString();
                                        fsmanager = sqlds.Tables["Emp"].Rows[0]["FItemID"].ToString();

                                    }
                                    if (userid == "")
                                    {
                                        MessageBox.Show("无法获得用户对应的金蝶id，请进行用户设置", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    str1 = "select top 1 fitemid,fname from t_stock where fitemid in ( select fdefaultloc from t_icitem where fitemid='" + cinvid + "')";
                                    sqladp = new SqlDataAdapter(str1, kdconn);
                                    sqladp.SelectCommand.Transaction = kdtrans;
                                    sqlds = new DataSet();
                                    sqladp.Fill(sqlds, "stock");
                                    if (sqlds.Tables["stock"].Rows.Count > 0)
                                    {
                                        stockid = sqlds.Tables["stock"].Rows[0]["fitemid"].ToString();
                                    }
                                    if (stockid == "")
                                    {
                                        MessageBox.Show("单据没有入库仓库！请调整", "采购入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                    str1 = "declare @p2 int   set @p2=NULL  exec GetICMaxNum 'ICStockBill',@p2 output,1," + userid + "  select @p2";
                                    SqlDataAdapter sqladp2 = new SqlDataAdapter(str1, kdconn);
                                    DataSet sqlds2 = new DataSet();
                                    sqladp2.SelectCommand.Transaction = kdtrans;
                                    sqladp2.Fill(sqlds2, "FInterID");
                                    if (sqlds2.Tables["FInterID"].Rows.Count > 0)
                                    {
                                        finterid = sqlds2.Tables["FInterID"].Rows[0][0].ToString();
                                    }
                                   
                                    if (vouchtype == "采购")
                                    {
                                        //获得单据号
                                        //str1 = "select val1+SUBSTRING('000000',1,flength2-LEN(val2))+val2 from  (select FProjectVal val1,flength flength1 from t_billcoderule where fbilltypeid='1' and FProjectID=1) a1, (select FProjectVal val2,flength flength2 from t_billcoderule where fbilltypeid='1' and FProjectID=3) a2";
                                        //sqladp2 = new SqlDataAdapter(str1, kdconn);
                                        //sqlds2 = new DataSet();
                                        //sqladp2.SelectCommand.Transaction = kdtrans;
                                        //sqladp2.Fill(sqlds2, "FInterID");
                                        //if (sqlds2.Tables["FInterID"].Rows.Count > 0)
                                        //{
                                        //    vouchcode = sqlds2.Tables["FInterID"].Rows[0][0].ToString();
                                        //}

                                        string fuuid = System.Guid.NewGuid().ToString();
                                        dbvoucode = vouchcode;
                                        //插入主表数据
                                        str1 = "insert into ICStockBill(FBrNo,FInterID,FTranType,FDate,FBillNo,FDeptID,FSupplyID,FFManagerID,FSManagerID,FBillerID,FPosted,FROB,FStatus,FUpStockWhenSave,FPOStyle,FUUID,FMarketingStyle,FSettleDate,FsourceType,FEmpID,FVchInterID,FRelateBrID,FBrID,FManageType,FOrderAffirm,FHeadSelfA0140) select top 1 '0','" + finterid + "','1','" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "',vouchcode,'0',partnerId,'" + ffmanager + "','" + fsmanager + "','" + userid + "','0','1','0','0','252','"
                                               + fuuid + "','12530','" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "','37521',0,0,0,0,0,0,carno from zyWeight..WeightBills where vouchcode in ('" + textBox1.Text + "')";
                                        sqlcomm = new SqlCommand(str1, kdconn);
                                        sqlcomm.Transaction = kdtrans;
                                        sqlcomm.ExecuteNonQuery();
                                        //单位
                                        str1 = "insert into ICStockBillEntry(fbrno,FInterID,FEntryID,FItemID,FQty,FUnitID,FAuxQty,FDCStockID,FReProduceType,FPlanMode,FChkPassItem,FComplexQty,FEntrySelfA0159,FEntrySelfA0160) select  '0','" + finterid + "',ROW_NUMBER() over (order by vouchcode) rownum,cinvid,convert(float,netWeight)/1000,'" + unitID.Text + "',convert(float,netWeight)/1000,'" + stockid + "','0','14036','1058',CONVERT(nvarchar(20),convert(float,netWeight)/1000)+'吨',convert(float,grossWeight)/1000,convert(float,tareWeight)/1000" +
                                                "  from zyWeight..weightbills where vouchcode in ('" + textBox1.Text + "')";
                                        sqlcomm = new SqlCommand(str1, kdconn);
                                        sqlcomm.Transaction = kdtrans;
                                        sqlcomm.ExecuteNonQuery();
                                        //检查是否插入成功
                                        str1 = "select * from ICStockBill join ICStockBillEntry on ICStockBill.FInterID= ICStockBillEntry.FInterID where ICStockBill.FInterID='" + finterid + "'";
                                        sqladp2 = new SqlDataAdapter(str1, kdconn);
                                        sqlds2 = new DataSet();
                                        sqladp2.SelectCommand.Transaction = kdtrans;
                                        sqladp2.Fill(sqlds2, "checkvouch");
                                        if (sqlds2.Tables["checkvouch"].Rows.Count == 0)
                                        {
                                            MessageBox.Show("没有生成成功！", "入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                        else
                                        {
                                            //str1 = "update t_billcoderule set FProjectVal =FProjectVal+1 from t_billcoderule where fbilltypeid='1' and FProjectID=3";
                                            //sqlcomm = new SqlCommand(str1, kdconn);
                                            //sqlcomm.Transaction = kdtrans;
                                            //sqlcomm.ExecuteNonQuery();
                                            //str1 = "update ICBillNo set FCurNo=FCurNo+1,FDesc='CIN+'+substring(FFormat,1,LEN(FFormat)-LEN(fcurno+1))+convert(varchar(10),FCurNo+1)  FROM ICBillNo where FBillID=1";
                                            //sqlcomm = new SqlCommand(str1, kdconn);
                                            //sqlcomm.Transaction = kdtrans;
                                            //sqlcomm.ExecuteNonQuery();

                                            if (sqlconn.State == ConnectionState.Closed)
                                            {
                                                sqlconn.ConnectionString = Utility.DatabaseConnection;
                                                sqlconn.Open();
                                            }
                                            str1 = "update WeightBillS set KDVouchcode='" + vouchcode + "' where vouchcode in ('" + textBox1.Text + "')";
                                            sqlcomm = new SqlCommand(str1, sqlconn);
                                            //sqlcomm.Transaction = trans;
                                            sqlcomm.ExecuteNonQuery();
                                            kdtrans.Commit();
                                            MessageBox.Show("生成外购入库单：" + textBox1.Text + " 1张！", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }

                                    }
                                    if (vouchtype == "销售")
                                    {
                                        //获得单据号
                                        //str1 = "select val1+SUBSTRING('000000',1,flength2-LEN(val2))+val2 from  (select FProjectVal val1,flength flength1 from t_billcoderule where fbilltypeid='24' and FProjectID=1) a1, (select FProjectVal val2,flength flength2 from t_billcoderule where fbilltypeid='24' and FProjectID=3) a2";
                                        //sqladp2 = new SqlDataAdapter(str1, kdconn);
                                        //sqlds2 = new DataSet();
                                        //sqladp2.SelectCommand.Transaction = kdtrans;
                                        //sqladp2.Fill(sqlds2, "FInterID");
                                        //if (sqlds2.Tables["FInterID"].Rows.Count > 0)
                                        //{
                                        //    vouchcode = sqlds2.Tables["FInterID"].Rows[0][0].ToString();
                                        //}
                                        string fuuid = System.Guid.NewGuid().ToString();
                                        dbvoucode = vouchcode;
                                        //插入主表数据
                                        str1 = "insert into ICStockBill(FBrNo,FInterID,FTranType,FDate,FBillNo,FDeptID,FFManagerID,FSManagerID,FBillerID,FROB,FUpStockWhenSave,FUUID,FMarketingStyle,FSourceType,FVchInterID,FPurposeID,FManageType,FSupplyID,FHeadSelfB0159) select top 1 '0','" + finterid + "','21','" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "',vouchcode,'1186','" + ffmanager + "','" + fsmanager + "','" + userid + "','1','0','"
                                               + fuuid + "','12530','37521','0','1200','0',partnerId,carno from zyWeight..WeightBills where vouchcode in ('" + textBox1.Text + "')";
                                        sqlcomm = new SqlCommand(str1, kdconn);
                                        sqlcomm.Transaction = kdtrans;
                                        sqlcomm.ExecuteNonQuery();
                                        //单位吨
                                        str1 = "insert into ICStockBillEntry(fbrno,FInterID,FEntryID,FItemID,FQty,FUnitID,FAuxQty,FDCStockID,FReProduceType,FPlanMode,FChkPassItem,FComplexQty,FEntrySelfB0167,FEntrySelfB0168) select  '0','" + finterid + "',ROW_NUMBER() over (order by vouchcode) rownum,cinvid,convert(float,netWeight)/1000,unitId,convert(float,netWeight)/1000,'" + stockid + "','0','14036','1058',CONVERT(nvarchar(20),convert(float,netWeight)/1000)+'吨',convert(float,grossWeight)/1000,convert(float,tareWeight)/1000" +
                                                "  from zyWeight..weightbills where vouchcode in ('" + textBox1.Text + "')";
                                        sqlcomm = new SqlCommand(str1, kdconn);
                                        sqlcomm.Transaction = kdtrans;
                                        sqlcomm.ExecuteNonQuery();
                                        //检查是否插入成功
                                        str1 = "select * from ICStockBill join ICStockBillEntry on ICStockBill.FInterID= ICStockBillEntry.FInterID where ICStockBill.FInterID='" + finterid + "'";
                                        sqladp2 = new SqlDataAdapter(str1, kdconn);
                                        sqlds2 = new DataSet();
                                        sqladp2.SelectCommand.Transaction = kdtrans;
                                        sqladp2.Fill(sqlds2, "checkvouch");
                                        if (sqlds2.Tables["checkvouch"].Rows.Count == 0)
                                        {
                                            MessageBox.Show("没有生成成功！", "入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                        else
                                        {
                                            //str1 = "update t_billcoderule set FProjectVal =FProjectVal+1 from t_billcoderule where fbilltypeid='24' and FProjectID=3";
                                            //sqlcomm = new SqlCommand(str1, kdconn);
                                            //sqlcomm.Transaction = kdtrans;
                                            //sqlcomm.ExecuteNonQuery();
                                            //str1 = "update ICBillNo set FCurNo=FCurNo+1,FDesc='SOUT+'+substring(FFormat,1,LEN(FFormat)-LEN(fcurno+1))+convert(varchar(10),FCurNo+1)  FROM ICBillNo where FBillID=24";
                                            //sqlcomm = new SqlCommand(str1, kdconn);
                                            //sqlcomm.Transaction = kdtrans;
                                            //sqlcomm.ExecuteNonQuery();

                                            if (sqlconn.State == ConnectionState.Closed)
                                            {
                                                sqlconn.ConnectionString = Utility.DatabaseConnection;
                                                sqlconn.Open();
                                            }
                                            str1 = "update WeightBills set KDVouchcode='" + vouchcode + "' where vouchcode in ('" + textBox1.Text + "')";
                                            sqlcomm = new SqlCommand(str1, sqlconn);
                                            //sqlcomm.Transaction = trans;
                                            sqlcomm.ExecuteNonQuery();
                                            kdtrans.Commit();
                                            MessageBox.Show("生成销售出库单：" + textBox1.Text + " 1张！", "销售出库单生成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }

                                    }

                                }
                       
                             }
                                else
                                {
                                    MessageBox.Show("皮重和毛重不能同时为空！", "数据录入：", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                                }
                            break;
                        default:
                            break;
                    }
                    //重新载入数据
                    str1 = "select vouchcode,vouchdate,carno,partnerId,partnerName,cinvid,cinvname,grossWeight,tareWeight,netWeight,water,dhValue,netValue,cmemo from weightbills order by vouchdate desc";
                    sqladp = new SqlDataAdapter(str1, sqlconn);
                    sqladp.Fill(sqlds, "vouch");
                    dataGridView1.DataSource = sqlds.Tables["vouch"]; 
                }
                else
                {
                    MessageBox.Show("录入的信息不全！", "数据保存", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                }
            }
            catch (Exception ex)
            {
                if (bTransBegin == true)
                {
                    kdtrans.Rollback();
                }
                MessageBox.Show("发生错误："+ex.Message, "数据保存", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            //打印单据
            if (radioButton1.Checked == true)
            {
                axGRPrintViewer1.Stop();
                axGRPrintViewer1.Start();
                report.PrintPreview(true);
            }
            else
            {
                report1.DetailGrid.Recordset.QuerySQL = "select * from weightbills where vouchcode='" + textBox1.Text + "'";
                axGRPrintViewer1.Stop();
                axGRPrintViewer1.Start();
                report1.PrintPreview(true);
            }

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            textBox9.Text = "";
            textBox10.Text = "";
            comboBox1.Text = "";
            comboBox2.Text = "";
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            //皮重
            decimal grossWeight = 0;
            decimal pWeight = 0;
            if (Convert.ToInt32(tbWeight.Text) != 0)
            {
                textBox5.Text = tbWeight.Text.ToString();
            }
            if (textBox4.Text != "")
            { grossWeight = Convert.ToDecimal(textBox4.Text); }
            if (textBox5.Text != "")
            { pWeight = Convert.ToDecimal(textBox5.Text); }
            textBox6.Text = (grossWeight - pWeight).ToString();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            //毛重
            decimal grossWeight = 0;
            decimal pWeight = 0;
            if (Convert.ToInt32(tbWeight.Text) != 0)
            {
                textBox4.Text = tbWeight.Text.ToString();
            }

            if (textBox4.Text != "")
            { grossWeight = Convert.ToDecimal(textBox4.Text); }
            if (textBox5.Text != "")
            { pWeight = Convert.ToDecimal(textBox5.Text); }
            textBox6.Text = (grossWeight - pWeight).ToString();

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str1 = "";
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            DataSet sqlds1 = new DataSet();
            if (kdconn.State == ConnectionState.Closed)
            {
                kdconn.ConnectionString = Utility.kdconnection;
                kdconn.Open();
            }
            if (comboBox2.Text != "")
            {
                str1 = "select FItemID,FName,FModel,FUnitID from t_ICItem  where FName='" + comboBox2.Text + "'";
                sqladp1 = new SqlDataAdapter(str1, kdconn);
                sqlds1 = new DataSet();  
                sqladp1.Fill(sqlds1, "ICItem");
                if (sqlds1.Tables["ICItem"].Rows.Count > 0)
                {
                    invid.Text = sqlds1.Tables["ICItem"].Rows[0]["FItemID"].ToString();
                    unitID.Text = sqlds1.Tables["ICItem"].Rows[0]["FUnitID"].ToString();
                }              
            }

        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            //选单
            string str1 = "";
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            DataSet sqlds1 = new DataSet();
            WeightSelect selectbill = new WeightSelect();
            selectbill.ShowDialog();
            if (Utility.billno != "")
            {
                weightCount = 2;
                str1 = "select vouchtype,vouchcode,vouchdate,carno,partnerId,partnerName,cinvid,cinvname,grossWeight,tareWeight,netWeight,water,dhValue,netValue,cmemo from weightbills  where vouchcode='" + Utility.billno + "'";
                sqladp1 = new SqlDataAdapter(str1, sqlconn);
                sqladp1.Fill(sqlds1, "ICItem");
                if (sqlds1.Tables["ICItem"].Rows.Count > 0)
                {
                    string voutype = sqlds1.Tables["ICItem"].Rows[0]["vouchtype"].ToString();
                    if (voutype == "采购") { radioButton1.Checked = true; }
                    if (voutype == "销售") { radioButton2.Checked = true; label3.Text = "客户"; }
                    comboBox1.Items.Clear();
                    comboBox2.Items.Clear();
                    textBox1.Text = sqlds1.Tables["ICItem"].Rows[0]["vouchcode"].ToString();
                    dateTimePicker1.Text = sqlds1.Tables["ICItem"].Rows[0]["vouchdate"].ToString();
                    suppid.Text = sqlds1.Tables["ICItem"].Rows[0]["partnerId"].ToString();
                    comboBox1.Items.Add(sqlds1.Tables["ICItem"].Rows[0]["partnerName"].ToString());
                    textBox2.Text = sqlds1.Tables["ICItem"].Rows[0]["carno"].ToString();
                    invid.Text = sqlds1.Tables["ICItem"].Rows[0]["cinvid"].ToString();
                    comboBox2.Items.Add(sqlds1.Tables["ICItem"].Rows[0]["cinvname"].ToString());
                    textBox4.Text = sqlds1.Tables["ICItem"].Rows[0]["grossWeight"].ToString();
                    textBox5.Text = sqlds1.Tables["ICItem"].Rows[0]["tareWeight"].ToString();
                    comboBox1.SelectedIndex = 0;
                    comboBox2.SelectedIndex = 0;
                }
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
            }
        }

        private void WeightFrn_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CurrentPort.IsOpen)
            {
                CurrentPort.DiscardInBuffer();
                CurrentPort.Close();
            }
            axGRPrintViewer1.Stop(); 
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            string str1 = "";
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            DataSet sqlds1 = new DataSet();
            label3.Text ="客户";
            if (weightCount == 1)
            {
                str1 = "select FItemID,FName  from t_Organization ";
                sqladp1 = new SqlDataAdapter(str1, kdconn);
                sqladp1.Fill(sqlds1, "customer");
                comboBox1.Items.Clear();
                for (int i = 0; i <= sqlds1.Tables["customer"].Rows.Count - 1; i++)
                {
                    // suppid.Text = sqlds1.Tables["customer"].Rows[0]["FName"].ToString();
                    comboBox1.Items.Add(sqlds1.Tables["customer"].Rows[i]["FName"].ToString());
                    // comboBox1.SelectedIndex = 0;
                }
 
            }
        }

        private void WeightFrn_Resize(object sender, EventArgs e)
        {
            if (this.Width > groupBox1.Width)
            {
                //MessageBox.Show(this.Width + "/" + groupBox1.Left + "/" + groupBox1.Width);
               // groupBox1.Left =658;
                //   dataGridView1.Width = this.Width - groupBox1.Width - 5;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            string str1 = "";
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            DataSet sqlds1 = new DataSet();
            label3.Text = "供应商";
            if (weightCount == 1)
            {
                str1 = "select FItemID,FName from t_Supplier";
                sqladp1 = new SqlDataAdapter(str1, kdconn);
                sqladp1.Fill(sqlds1, "Supplier");
                comboBox1.Items.Clear();
                for (int i = 0; i <= sqlds1.Tables["Supplier"].Rows.Count - 1; i++)
                {
                    //suppid.Text = sqlds1.Tables["Supplier"].Rows[0]["FName"].ToString();
                    comboBox1.Items.Add(sqlds1.Tables["Supplier"].Rows[i]["FName"].ToString());
                    //comboBox1.SelectedIndex = 0;
                }
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            //删除单据
            string str1 = "";
            SqlDataAdapter sqladp1 = new SqlDataAdapter();
            SqlCommand sqlcomm1=new SqlCommand(); 
            DataSet sqlds1 = new DataSet();
            try
            {
                if (sqlconn.State == ConnectionState.Closed)
                {
                    sqlconn.ConnectionString = Utility.DatabaseConnection;
                    sqlconn.Open();
                }
                if (kdconn.State == ConnectionState.Closed)
                {
                    kdconn.ConnectionString = Utility.kdconnection;
                    kdconn.Open();
                }
                if (dataGridView1.Rows.Count > 0)
                {
                    string voucode = dataGridView1.CurrentRow.Cells["vouchcode"].Value.ToString();
                    if (voucode != "")
                    {
                        if (MessageBox.Show("是否真要删除过磅单" + voucode + "？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            str1 = "select FCheckerID from ICStockBill where FBillNo='" + voucode + "' ";
                            sqladp1 = new SqlDataAdapter(str1, kdconn);
                            sqlds1 = new DataSet();
                            sqladp1.Fill(sqlds1, "delvouch");
                            if (sqlds1.Tables["delvouch"].Rows.Count == 0)
                            {
                                str1 = "delete from weightbills where vouchcode='" + voucode + "' ";
                                sqlcomm1 = new SqlCommand(str1, sqlconn);
                                sqlcomm1.ExecuteNonQuery();
                                MessageBox.Show("单据删除成功！", "单据删除", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                str1 = "select vouchcode,vouchdate,carno,partnerId,partnerName,cinvid,cinvname,grossWeight,tareWeight,netWeight,water,dhValue,netValue,cmemo from weightbills order by vouchdate desc";
                                sqladp1 = new SqlDataAdapter(str1, sqlconn);
                                sqladp1.Fill(sqlds1, "vouch");
                                dataGridView1.DataSource = sqlds1.Tables["vouch"];
                            }
                            else
                            {
                                if (sqlds1.Tables["delvouch"].Rows[0]["FCheckerID"].ToString() != "")
                                {

                                    MessageBox.Show("金蝶的单据已经审核不能删除！", "单据删除", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                else
                                {
                                    str1 = "delete from ICStockBill where FCheckerID is null and FBillNo='" + voucode + "' ";
                                    sqlcomm1 = new SqlCommand(str1, kdconn);
                                    sqlcomm1.ExecuteNonQuery();
                                    str1 = "delete from weightbills where vouchcode='" + voucode + "' ";
                                    sqlcomm1 = new SqlCommand(str1, sqlconn);
                                    sqlcomm1.ExecuteNonQuery();
                                    MessageBox.Show("单据删除成功！", "单据删除", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    str1 = "select vouchcode,vouchdate,carno,partnerId,partnerName,cinvid,cinvname,grossWeight,tareWeight,netWeight,water,dhValue,netValue,cmemo from weightbills order by vouchdate desc";
                                    sqladp1 = new SqlDataAdapter(str1, sqlconn);
                                    sqladp1.Fill(sqlds1, "vouch");
                                    dataGridView1.DataSource = sqlds1.Tables["vouch"];
                                }
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除过磅单时发生错误！" + ex.Message, "单据删除", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //查询
            try
            {
                SqlDataAdapter sqladp = new SqlDataAdapter();
                DataSet sqlds = new DataSet();
                if (sqlconn.State == ConnectionState.Closed)
                {
                    sqlconn.ConnectionString = Utility.DatabaseConnection;
                    sqlconn.Open();
                }
                startDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
                string str1 = "select vouchcode,vouchdate,carno,partnerId,partnerName,cinvid,cinvname,grossWeight,tareWeight,netWeight,water,dhValue,netValue,cmemo from weightbills where 1=1";
                if (startDate.Text != "") { str1 += " and vouchdate=convert(datetime,'" + startDate.Text + "')"; }
                if (partner.Text != "") { str1 += " and partnerName like '%" + partner.Text + "%'"; }
                if (carcode.Text != "") { str1 += " and carno like '%" + carcode.Text + "%'"; }
                str1 += " order by vouchdate desc";
                sqladp = new SqlDataAdapter(str1, sqlconn);
                sqladp.Fill(sqlds, "vouch");
                dataGridView1.DataSource = sqlds.Tables["vouch"]; 

            }
            catch (Exception ex)
            {
                MessageBox.Show("查询过磅单时发生错误！"+ex.Message, "单据过滤", MessageBoxButtons.OK, MessageBoxIcon.Error);
 
            }
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox4.Enabled = true;
                textBox4.ReadOnly = false;
                textBox5.Enabled = true;
                textBox5.ReadOnly = false;
            }
            else
            {
                textBox4.ReadOnly = true;
                textBox5.ReadOnly = true;
            }
        }

        private void tbWeight_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
