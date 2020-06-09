using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Data.SqlClient;
//using grproLib;
using System.IO;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;
using System.Threading;

namespace zySoft
{
       public partial class Form2 : Form
    {
        //private GridppReport report = new GridppReport();
        private SqlConnection sqlconn = new SqlConnection();
        private SqlConnection kdconn = new SqlConnection();
        private StringBuilder builder = new StringBuilder();
        SerialPort CurrentPort = null;
        private delegate void HandleInterfaceUpdateDelegate(string astring);
        HandleInterfaceUpdateDelegate interfaceUpdateHandle;
        static string strReceive;

        public Form2()
        {
            InitializeComponent();
        }
        public static string ByteToHexStr(byte[] bytes) //函数,字节数组转16进制字符串
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            if (CurrentPort !=null)
            {
                CurrentPort.DiscardInBuffer();
                CurrentPort.Close();
            }
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //载入数据
            try
            {
               // interfaceUpdateHandle = new HandleInterfaceUpdateDelegate(showText); 
                //载入打印标签
                //读取kd连接
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

                sqlconn.ConnectionString = Utility.DatabaseConnection;
                SqlDataAdapter sqladp = new SqlDataAdapter();
                DataSet sqlds = new DataSet();
                if (sqlconn.State == ConnectionState.Closed)
                {
                    sqlconn.Open();
                }
                string str1 = "select RFName from FRtype";
                sqladp = new SqlDataAdapter(str1, sqlconn);
                sqladp.Fill(sqlds, "rftype");
                comboBox1.Items.Clear(); 
                for (int i = 0; i <= sqlds.Tables["rftype"].Rows.Count - 1; i++)
                {
                    comboBox1.Items.Add(sqlds.Tables["rftype"].Rows[i]["RFName"].ToString()); 
                }
                comboBox1.SelectedIndex = 0;
                str1 = "select wlname from aa_inventory";
                sqladp = new SqlDataAdapter(str1, sqlconn);
                sqladp.Fill(sqlds, "inv");
                WlName.Items.Clear();
                for (int i = 0; i <= sqlds.Tables["inv"].Rows.Count - 1; i++)
                {
                    WlName.Items.Add(sqlds.Tables["inv"].Rows[i]["wlname"].ToString());
                }
                WlName.SelectedIndex = 0;
                SqlConnection kdconn = new SqlConnection();
                kdconn.ConnectionString = Utility.kdconnection;
                SqlDataAdapter sqladp1 = new SqlDataAdapter();
                DataSet sqlds1 = new DataSet();
                if (kdconn.State == ConnectionState.Closed)
                {
                    kdconn.Open();
                }
                str1 = "select  Bh    as 包号,Bar_code as 编号, PH_barcode  as  批号,Wl_code as 品名编码,Wlname as 品名,Gsm  as 克重,PB    as 配比,F_width  as  幅宽厘米,Net_wt  as  净重,Gross_wt as 毛重,Bc     as  班次,D_Datetime as 打印日期,J_length  as  卷长,Mjjs   as     每件卷数,Jt_sum   as 接头数,jY_code as    检验员,Jj      as    卷径,MJh    as 母卷号, Cj       as   车间,CZy_code  as  操作员,stockName as 仓库 from WeightBill where D_Datetime>=DateAdd(dd,-1,getdate())";
                sqladp = new SqlDataAdapter(str1, sqlconn);
                sqladp.Fill(sqlds, "label");
                if (sqlds.Tables["label"].Rows.Count > 0)
                {
                    dataGridView1.DataSource = sqlds.Tables["label"];
                }
                if (CommonClass.teamName != "")
                {
                    textBox18.Text = CommonClass.teamName;
                }
                else
                {
                    textBox18.Text = "甲班";
                }
                tbWeight.ReadOnly = true;
                FInterID.Visible = false;
                deptId.Visible = false;
                WLId.Visible = false;
                unitID.Visible = false;
                textBox4.ReadOnly = true; 
                dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd");

                CurrentPort = new SerialPort();
                CurrentPort.Close();
                CurrentPort.ReadBufferSize = 512;
                CurrentPort.PortName = "COM3";  //端口号 
                CurrentPort.BaudRate = 9600; //比特率 
                CurrentPort.Parity = Parity.None;//奇偶校验 
                CurrentPort.StopBits = StopBits.One;//停止位 
                CurrentPort.DataBits = Convert.ToInt32("8");//数据位
                CurrentPort.ReadTimeout = 500; //读超时，即在1000内未读到数据就引起超时异常 
                //绑定数据接收事件，因为发送是被动的，所以你无法主动去获取别人发送的代码，只能通过这个事件来处理
                CurrentPort.DataReceived += serialPort1_DataReceived;
                System.Threading.Thread.Sleep(100); //读取速度太慢，加Sleep延长读取时间, 不可缺少
                if (CurrentPort.IsOpen == true)
                {
                    MessageBox.Show("端口" + CurrentPort.PortName + "已经打开或者占用！发货获得地磅数据！", "标签打印", MessageBoxButtons.OK, MessageBoxIcon.Error); 
                }
                else
                {
                    CurrentPort.Open();
                }
                axGRPrintViewer1.Visible = false; 
              
            }
            catch (Exception ex)
            {
                MessageBox.Show("所发生错误：" + ex.Message);
            }

        }
        private void showText(string astring)
        {
            tbWeight.Text = astring;
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
                //// byte[] readBuffer = new byte[sp.ReadBufferSize];
                //// sp.Read(readBuffer, 0, readBuffer.Length);
                //// this.Invoke((EventHandler)(delegate  
                //// { 
                //// //直接按ASCII规则转换成字符串  
                ////// builder.Append(Encoding.ASCII.GetString(readBuffer));
                ////  builder.Append(ByteToHexStr(readBuffer)); //用到函数，作用：转换16进制
                //// //追加的形式添加到文本框末端，并滚动到最后。  
                //// this.textBox1.AppendText(builder.ToString());  
                //// }));
                byte firstByte = Convert.ToByte(sp.ReadByte());
                if (firstByte == 0x02)
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
                        if (byteData == 0x03)//结束
                        {
                            break;
                        }
                        bytesData[i] = byteData;
                    }
                    strReceive = Encoding.Default.GetString(bytesData);
                }
                tbWeight.Invoke(new EventHandler(delegate { tbWeight.Text = GetWeightOfPort(strReceive); }));
              //  BeginInvoke(interfaceUpdateHandle, strReceive);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + ex.GetType().FullName);
            }
        }

         private string GetWeightOfPort(string weight)
         {
             if (string.IsNullOrEmpty(weight) || weight.IndexOf("+") < 0 || weight.Length < 6)
             {
                 return "0.0";
             }
             weight = weight.Replace("+", "");
             weight = int.Parse(weight.Substring(0, 5)).ToString() + "." + weight.Substring(5, 1);
             return weight;
         }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //新增时参照
            string str1 = "";
            SqlDataAdapter sqladp = new SqlDataAdapter();
            DataSet sqlds = new DataSet(); 
            MoSelect moselect = new MoSelect();
            moselect.ShowDialog();
            //获得选择的fitemid
            if (Utility.voucherid != "")
            {
                if (kdconn.State == ConnectionState.Closed)
                {
                    kdconn.ConnectionString = Utility.kdConnection ;
                    kdconn.Open();
                }
                if (sqlconn.State == ConnectionState.Closed)
                {
                    sqlconn.ConnectionString = Utility.databaseConnection;
                    sqlconn.Open();
                }
                string fitemid = Utility.voucherid;
                string billcode = "";
                str1 = "select max(isnull(voucode,'')) billcode from WeightBill ";
                sqladp = new SqlDataAdapter(str1, sqlconn);
                sqladp.Fill(sqlds, "billcode");
                if (sqlds.Tables["billcode"].Rows.Count > 0)
                { 
                    string txtbill= sqlds.Tables["billcode"].Rows[0]["billcode"].ToString();
                    if (txtbill != "")
                    {
                        int xuhao=Convert.ToInt32(txtbill.Substring(2,6))+1;
                        billcode = "WB" + "000000".Substring(1,6-xuhao.ToString().Length)+xuhao;
                    }
                    else
                    {
                        billcode = "WB000001";
                    }

                }
                textBox20.Text = billcode;
                //str1 = "select FItemID,FName  from t_Item where FItemClassID='3001'"; 
                //sqladp = new SqlDataAdapter(str1, kdconn);
                //sqladp.Fill(sqlds, "sale");
                //saleName.Items.Clear(); 
                // for (int i = 0; i < sqlds.Tables["sale"].Rows.Count; i++)
                //{
                //    saleName.Items.Add(sqlds.Tables["sale"].Rows[i]["FName"].ToString());
                //}
                // saleName.SelectedIndex = 0; 
                str1 = "select v1.FInterID,FBillNo,FWorkShop,t8.FName cdepname,v1.FItemID,t9.FNumber,t9.FName,FModel,FGMPBatchNo,FQty,FPlanCommitDate,FPlanFinishDate,t9.FUnitID from ICMO v1 LEFT OUTER JOIN t_SubMessage t5 ON   v1.FMRP = t5.FInterID  AND t5.FInterID<>0  LEFT OUTER JOIN t_Department t8 ON   v1.FWorkShop = t8.FItemID  AND t8.FItemID<>0 INNER JOIN t_ICItem t9 ON   v1.FItemID = t9.FItemID  AND t9.FItemID<>0 and v1.FStatus=1 " +
                              "  where  v1.FInterID='" + fitemid + "'";

                sqladp = new SqlDataAdapter(str1, kdconn);
                sqladp.Fill(sqlds, "ICMO");
                if (sqlds.Tables["ICMO"].Rows.Count>0)
                {
                textBox1.Text = sqlds.Tables["ICMO"].Rows[0]["FBillNo"].ToString();
                textBox13.Text = sqlds.Tables["ICMO"].Rows[0]["cdepname"].ToString();
                textBox12.Text = sqlds.Tables["ICMO"].Rows[0]["FQty"].ToString();
                FInterID.Text =sqlds.Tables["ICMO"].Rows[0]["FInterID"].ToString();
                deptId.Text = sqlds.Tables["ICMO"].Rows[0]["FWorkShop"].ToString();
                WLId.Text = sqlds.Tables["ICMO"].Rows[0]["FItemID"].ToString();
                textBox2.Text = sqlds.Tables["ICMO"].Rows[0]["FNumber"].ToString();
                textBox3.Text = sqlds.Tables["ICMO"].Rows[0]["FName"].ToString();
                textBox8.Text = sqlds.Tables["ICMO"].Rows[0]["FGMPBatchNo"].ToString();
                unitID.Text = sqlds.Tables["ICMO"].Rows[0]["FUnitID"].ToString();
                }
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    //生成入库单
            //    SqlCommand sqlcomm = new SqlCommand();
            //    if (kdconn.State == ConnectionState.Closed)
            //    {
            //        kdconn.ConnectionString = Utility.kdConnection;
            //        kdconn.Open();
            //    }
            //    //需要对应仓库
            //    if (comboBox2.Text == "")
            //    {
            //        MessageBox.Show("仓库不能为空！", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //    //检查是否已经质检rkCheck

            //    string str1 = " select * from rkCheck where FBillNo='" + textBox1.Text + "'";
            //    SqlDataAdapter sqladp = new SqlDataAdapter(str1, kdconn);
            //    DataSet sqlds = new DataSet();
            //    sqladp.Fill(sqlds, "rkCheck");
            //    if (sqlds.Tables["rkCheck"].Rows.Count == 0)
            //    {
            //        MessageBox.Show("任务单：" + textBox1.Text + " 没有完成产品检验或其他！", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }

            //    string vouchcode = "";
            //    string dbvoucode = "";
            //    string userid = "16393";
            //    string ffmanager = "3105";
            //    string fsmanager = "3105";
            //    string finterid = "";
            //    //
            //    str1 = "select  top 1 kduserid from aa_user where username = '" + CommonClass.userName + "'";
            //    sqladp = new SqlDataAdapter(str1, sqlconn);
            //    sqladp.Fill(sqlds, "user");
            //    if (sqlds.Tables["user"].Rows.Count > 0)
            //    {
            //        userid = sqlds.Tables["user"].Rows[0]["kduserid"].ToString();
            //        ffmanager = userid;
            //        fsmanager = userid;
            //    }

            //    //获得finterID
            //    str1 = "declare @p2 int   set @p2=NULL  exec GetICMaxNum 'ICStockBill',@p2 output,1," + userid + "  select @p2";
            //    SqlDataAdapter sqladp2 = new SqlDataAdapter(str1, kdconn);
            //    DataSet sqlds2 = new DataSet();
            //    sqladp2.Fill(sqlds2, "FInterID");
            //    if (sqlds2.Tables["FInterID"].Rows.Count > 0)
            //    {
            //        finterid = sqlds2.Tables["FInterID"].Rows[0][0].ToString();
            //    }
            //    //获得单据号
            //    str1 = "select val1+SUBSTRING('000000',1,flength2-LEN(val2))+val2 from  (select FProjectVal val1,flength flength1 from t_billcoderule where fbilltypeid='2' and FProjectID=1) a1, (select FProjectVal val2,flength flength2 from t_billcoderule where fbilltypeid='2' and FProjectID=3) a2";
            //    sqladp2 = new SqlDataAdapter(str1, kdconn);
            //    sqlds2 = new DataSet();
            //    sqladp2.Fill(sqlds2, "FInterID");
            //    if (sqlds2.Tables["FInterID"].Rows.Count > 0)
            //    {
            //        vouchcode = sqlds2.Tables["FInterID"].Rows[0][0].ToString();
            //    }
            //    string fuuid = System.Guid.NewGuid().ToString();
            //    dbvoucode = textBox20.Text.ToString();
            //    //插入主表数据
            //    str1 = "insert into ICStockBill(FBrNo,FInterID,FTranType,FDate,FBillNo,FDeptID,FFManagerID,FSManagerID,FBillerID,FPosted,FROB,FStatus,FUpStockWhenSave,FUUID,FManageType,FSelTranType,FsourceType,FVchInterID,FEntrySelfA0245) select top 1 '0','" + finterid + "','2','" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "','" + vouchcode + "',deptId,'" + ffmanager + "','" + fsmanager + "','" + userid + "','0','1','0','1','"
            //           + fuuid + "','0','85','37521','0',saleAreaId from zyBarcode..WeightBill where voucode='" + dbvoucode + "'";
            //    sqlcomm = new SqlCommand(str1, kdconn);
            //    sqlcomm.ExecuteNonQuery();

            //    //取得入库单仓库
            //    string stockid = "";
            //    str1 = "select FItemID  from t_Stock where FName ='" + comboBox2.Text + "'";
            //    sqladp2 = new SqlDataAdapter(str1, kdconn);
            //    sqlds2 = new DataSet();
            //    sqladp2.Fill(sqlds2, "stock");
            //    if (sqlds2.Tables["stock"].Rows.Count > 0)
            //    {
            //        stockid = sqlds2.Tables["stock"].Rows[0]["FItemID"].ToString();
            //    }
            //    str1 = "insert into ICStockBillEntry(FBrNo,FInterID,FEntryID,FItemID,FQty,FAuxQty,FUnitID,FDCStockID,FPlanMode,FChkPassItem,FBatchNo,FSourceTranType,FSourceInterId,FSourceBillNo,FICMOBillNo,FICMOInterID,FEntrySelfA0245) select  '0','" + finterid + "',ROW_NUMBER() over (order by bh) rownum,wlid,Net_wt,Net_wt,unitId,stockId,'14036','1058',ph_barcode,'85',sourceID,sourceVoucode,sourceVoucode" +
            //           ",sourceID,Gross_wt from zyBarcode..WeightBill where voucode='" + dbvoucode + "'";
            //    sqlcomm = new SqlCommand(str1, kdconn);
            //    sqlcomm.ExecuteNonQuery();
            //    //检查是否插入成功
            //    str1 = "select * from ICStockBill join ICStockBillEntry on ICStockBill.FInterID= ICStockBillEntry.FInterID where ICStockBill.FInterID='" + finterid + "'";
            //    sqladp2 = new SqlDataAdapter(str1, kdconn);
            //    sqlds2 = new DataSet();
            //    sqladp2.Fill(sqlds2, "checkvouch");
            //    if (sqlds2.Tables["checkvouch"].Rows.Count == 0)
            //    {
            //        MessageBox.Show("没有生成成功！", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //    else
            //    {
            //        str1 = "update t_billcoderule set FProjectVal =FProjectVal+1 from t_billcoderule where fbilltypeid='2' and FProjectID=3";
            //        sqlcomm = new SqlCommand(str1, kdconn);
            //        sqlcomm.ExecuteNonQuery();
            //        str1 = "update ICBillNo set FCurNo=FCurNo+1,FDesc='CIN+'+substring(FFormat,1,LEN(FFormat)-LEN(fcurno+1))+convert(varchar(10),FCurNo+1)  FROM ICBillNo where FBillID=2";
            //        sqlcomm = new SqlCommand(str1, kdconn);
            //        sqlcomm.ExecuteNonQuery();
            //        str1 = "update WeightBill set rkVouchcode='" + vouchcode + "' where voucode='" + textBox20.Text + "'";
            //        sqlcomm = new SqlCommand(str1, sqlconn);
            //        sqlcomm.ExecuteNonQuery();
            //        MessageBox.Show("生成入库单：" + vouchcode + " 1张！", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("生成入库单的过程中发生错误：" + ex.Message , "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //}

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            //保存过磅单
            string str1="";
           SqlDataAdapter sqladp = new SqlDataAdapter();
           SqlCommand sqlcomm = new SqlCommand();
            DataSet sqlds = new DataSet();
            try
            {
                if (sqlconn.State == ConnectionState.Closed)
                {
                    sqlconn.ConnectionString = Utility.DatabaseConnection;
                    sqlconn.Open();
                }

                //检查单据
                if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox7.Text == "" || textBox17.Text =="")
                {
                    MessageBox.Show("录入的信息不全", "保存检查", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if(CommonClass.userName=="")
                {
                    MessageBox.Show("无法获得登录用户名", "保存检查", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }
                    //str1 = "if not exists(select * from sysobjects where xtype='u' and name='WeightBill') create table WeightBill()";
                    //SqlCommand sqlcomm = new SqlCommand(str1, sqlconn);
                    //sqlcomm.ExecuteNonQuery();
                    if (MessageBox.Show("是否要保存过磅单" + textBox20.Text.ToString() + "？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                         str1 = "select * from WeightBill where sourceVoucode='" + textBox1.Text + "' and bh='" + textBox17.Text + "'";
                    sqladp  = new SqlDataAdapter(str1,sqlconn);
                    sqladp.Fill(sqlds, "labelcheck");
                    if (sqlds.Tables["labelcheck"].Rows.Count == 0)
                    {
                        str1 = "insert into WeightBill(bar_code,Ph_barcode,WLId,WLname,Gsm,Pb,F_width,Net_wt,Gross_wt,Bc,d_datetime,Bh,J_length,Mjjs,Wl_code, Jt_sum, jY_code, Jj,Mjh,Cj,CZy_code,voucode,ddate,deptId,deptName,moQty,sourceID,sourceVoucode,unitId,invCode,invName,DA_code,Style_No,Dinfine1,Dinfine2,bz) values('" + textBox1.Text + "_" + textBox17.Text + "','" + textBox8.Text + "','" + WLId.Text + "','" + WlName.Text + "','" + textBox7.Text + "" + "','"
                            + textBox9.Text + "','" + textBox21.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox18.Text + "','" + DateTime.Now + "','" + textBox17.Text + "','" + textBox15.Text + "','" + textBox14.Text + "','" + textBox25.Text + "','" + textBox24.Text + "','" + textBox19.Text + "','" + textBox23.Text + "','" + textBox22.Text + "','" + textBox13.Text + "','" + CommonClass.userName + "','" +
                             textBox20.Text + "','" + dateTimePicker1.Text + "','" + deptId.Text + "','" + textBox13.Text + "','" + textBox12.Text + "','" + FInterID.Text + "','" + textBox1.Text + "','" + unitID.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox26.Text + "','" + textBox10.Text + "','" + textBox27.Text + "','" + textBox28.Text + "','" + textBox11.Text + "')";
                        sqlcomm = new SqlCommand(str1, sqlconn);
                        sqlcomm.ExecuteNonQuery();
                    }
                    else
                    {
                        if (MessageBox.Show("任务单：" + textBox1.Text + ",包号为" + textBox17.Text + "已经存在是否覆盖？", "保存", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                        {
                            str1="delete from WeightBill where sourceVoucode='" + textBox1.Text + "' and bh='" + textBox17.Text + "'";
                            sqlcomm = new SqlCommand(str1, sqlconn);
                            sqlcomm.ExecuteNonQuery();
                            str1 = "insert into WeightBill(bar_code,Ph_barcode,WLId,WLname,Gsm,Pb,F_width,Net_wt,Gross_wt,Bc,d_datetime,Bh,J_length,Mjjs,Wl_code, Jt_sum, jY_code, Jj,Mjh,Cj,CZy_code,voucode,ddate,deptId,deptName,moQty,sourceID,sourceVoucode,unitId,invCode,invName,DA_code,Style_No,Dinfine1,Dinfine2,bz) values('" + textBox1.Text + "_" + textBox17.Text + "','" + textBox8.Text + "','" + WLId.Text + "','" + WlName.Text + "','" + textBox7.Text + "" + "','"
                                + textBox9.Text + "','" + textBox21.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox18.Text + "','" + DateTime.Now + "','" + textBox17.Text + "','" + textBox15.Text + "','" + textBox14.Text + "','" + textBox25.Text + "','" + textBox24.Text + "','" + textBox19.Text + "','" + textBox23.Text + "','" + textBox22.Text + "','" + textBox13.Text + "','" + CommonClass.userName + "','" +
                                 textBox20.Text + "','" + dateTimePicker1.Text + "','" + deptId.Text + "','" + textBox13.Text + "','" + textBox12.Text + "','" + FInterID.Text + "','" + textBox1.Text + "','" + unitID.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox26.Text + "','" + textBox10.Text + "','" + textBox27.Text + "','" + textBox28.Text + "','" + textBox11.Text + "')";
                            sqlcomm = new SqlCommand(str1, sqlconn);
                             sqlcomm.ExecuteNonQuery();
 
                        }
                        else 
                        {
                            return;
                        }
                    }
                    str1 = "select  Bh    as 包号,Bar_code as 编号, PH_barcode  as  批号,Wl_code as 品名编码,Wlname as 品名,Gsm  as 克重,PB    as 配比,F_width  as  幅宽厘米,Net_wt  as  净重,Gross_wt as 毛重,Bc     as  班次,D_Datetime as 打印日期,J_length  as  卷长,Mjjs   as     每件卷数,Jt_sum   as 接头数,jY_code as    检验员,Jj      as    卷径,MJh    as 母卷号, Cj       as   车间,CZy_code  as  操作员,stockName as 仓库 from WeightBill where D_Datetime>=DateAdd(dd,-1,getdate())";
                    sqladp = new SqlDataAdapter(str1, sqlconn);
                    sqladp.Fill(sqlds, "label");
                    if (sqlds.Tables["label"].Rows.Count > 0)
                    {
                        dataGridView1.DataSource = sqlds.Tables["label"];
                    }

                    MessageBox.Show("保存过磅单成功", "过磅单保存", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存过磅单的过程中发生错误："+ex.Message, "过磅单保存", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            try
            {

                //标签打印//自动按过磅单号和包号保存
                string str1 = "";
                SqlDataAdapter sqladp = new SqlDataAdapter();
                SqlCommand sqlcomm = new SqlCommand();
                DataSet sqlds = new DataSet();
                if (sqlconn.State == ConnectionState.Closed)
                {
                    sqlconn.Open();
                }

                //if (File.Exists(Application.StartupPath + "\\printmodel.grf") == false)
                //{
                // MessageBox.Show ("************************************************************/r/n"  +
                //        "没有printmodel.grf！请确认板件明细文件是否存在！/r/n" +
                //        "************************************************************");
                //}
                //report.LoadFromFile(Application.StartupPath + "\\printmodel.grf");
                //report.DetailGrid.Recordset.ConnectionString = Utility.reportConnection;
                //axGRPrintViewer1.Report = report;
                if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox7.Text == "")
                {
                    MessageBox.Show("录入的信息不全", "保存检查", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (textBox20.Text != "" && textBox17.Text != "")
                {
                    //str1 = "if not exists(select * from sysobjects where xtype='u' and name='barcodeLable') CREATE TABLE [dbo].[barcodeLable]([bar_code] [nvarchar](60) NOT NULL,	[Ph_barcode] [nvarchar](25) NOT NULL,	[Code] [nvarchar](20) NOT NULL,	[name] [nvarchar](50) NOT NULL,	[Gsm] [decimal](18, 2) NULL,"+
                    //      "[Pb] [nvarchar](20) NULL,	[F_width] [decimal](18, 2) NULL,[Net_wt] [decimal](18, 2) NULL,	[Gross_wt] [decimal](18, 2) NULL,[Bc] [nvarchar](10) NULL,	[d_datetime] [datetime] NULL,	[Bh] [decimal](18, 2) NULL,	[J_length] [decimal](18, 2) NULL,[Mjjs] [int] NULL,	[Wl_code] [nvarchar](20) NULL,"+
                    //      "[DA_code] [nvarchar](20) NULL,[Jt_sum] [int] NULL,[jY_code] [nvarchar](20) NULL,	[Jj] [nvarchar](10) NULL,[Mjh] [int] NULL,[Cj] [nvarchar](10) NOT NULL,	[zfbs] [int] NULL,[CZy_code] [nvarchar](20) NOT NULL,[Dinfine1] [nvarchar](50) NULL,[Dinfine2] [nvarchar](50) NULL,[Dinfine3] [nvarchar](50) NULL,"+
                    //      "[Dinfine4] [nvarchar](50) NULL,	[Dinfine5] [nvarchar](50) NULL,	[Dinfine6] [nvarchar](50) NULL,[Style_No] [nvarchar](30) NULL,[bz] [nvarchar](50) NULL)";
                    //SqlCommand sqlcomm = new SqlCommand(str1, sqlconn);
                    //sqlcomm.ExecuteNonQuery();
                    str1 = "select * from WeightBill where sourceVoucode='" + textBox1.Text + "' and bh='" + textBox17.Text + "'";
                    sqladp = new SqlDataAdapter(str1, sqlconn);
                    sqladp.Fill(sqlds, "labelcheck");
                    if (sqlds.Tables["labelcheck"].Rows.Count == 0)
                    {
                        str1 = "insert into WeightBill(bar_code,Ph_barcode,WLId,WLname,Gsm,Pb,F_width,Net_wt,Gross_wt,Bc,d_datetime,Bh,J_length,Mjjs,Wl_code, Jt_sum, jY_code, Jj,Mjh,Cj,CZy_code,voucode,ddate,deptId,deptName,moQty,sourceID,sourceVoucode,unitId,invCode,invName,DA_code,Style_No,Dinfine1,Dinfine2,bz) values('" + textBox1.Text + "_" + textBox17.Text + "','" + textBox8.Text + "','" + WLId.Text + "','" + WlName.Text + "','" + textBox7.Text + "" + "','"
                            + textBox9.Text + "','" + textBox21.Text + "','" + textBox4.Text + "','" + textBox5.Text + "','" + textBox18.Text + "','" + DateTime.Now + "','" + textBox17.Text + "','" + textBox15.Text + "','" + textBox14.Text + "','" + textBox25.Text + "','" + textBox24.Text + "','" + textBox19.Text + "','" + textBox23.Text + "','" + textBox22.Text + "','" + textBox13.Text + "','" + CommonClass.userName + "','" +
                             textBox20.Text + "','" + dateTimePicker1.Text + "','" + deptId.Text + "','" + textBox13.Text + "','" + textBox12.Text + "','" + FInterID.Text + "','" + textBox1.Text + "','" + unitID.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox26.Text + "','" + textBox10.Text + "','" + textBox27.Text + "','" + textBox28.Text + "','" + textBox11.Text + "')";
                        sqlcomm = new SqlCommand(str1, sqlconn);
                        sqlcomm.ExecuteNonQuery();
                    }

                    //
                    //开始打印

                    str1 = "select  Bh as 包号,Bar_code as 编号, PH_barcode  as  批号,Wl_code as 品名编码,Wlname as 品名,Gsm  as 克重,PB    as 配比,F_width  as  幅宽厘米,Net_wt  as  净重,Gross_wt as 毛重,Bc     as  班次,D_Datetime as 打印日期,J_length  as  卷长,Mjjs   as     每件卷数,Jt_sum   as 接头数,jY_code as    检验员,Jj  as    卷径,MJh   as 母卷号, Cj as   车间,CZy_code  as  操作员,stockName as 仓库 from WeightBill where D_Datetime>=DateAdd(dd,-1,getdate())";
                    sqladp = new SqlDataAdapter(str1, sqlconn);
                    DataSet sqlds2 = new DataSet();
                    sqladp.Fill(sqlds2, "label");
                    if (sqlds2.Tables["label"].Rows.Count > 0)
                    {
                        dataGridView1.DataSource = sqlds2.Tables["label"];
                    }
                    string printvouch = textBox1.Text + "_" + textBox17.Text;
                    if (printvouch == "")
                    {
                        if (dataGridView1.Rows.Count > 0 && dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["单据号"].Value != null)
                        {
                            printvouch = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["编号"].Value.ToString();
                        }
                        else
                        {
                            MessageBox.Show("服务获得打印的单据号，请选择打印的单据", "标签打印", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    Ini ini = new Ini("d:\\barcode\\excel\\Hplabele.xls");
                    //excel 必须安装2003版本
                    if (File.Exists("d:\\barcode\\excel\\Hplabele.xls"))
                    {
                        str1 = "SELECT     bar_code AS 编号, Ph_barcode AS 批号, voucode AS 单据号, ddate AS 日期, deptName AS 部门名, plandate AS 计划日期, moQty AS 计划量, Wl_code AS 物料代码,invcode AS 品名编码, Wlname AS 品名, Gsm AS 克重, Pb AS 配比, F_width AS 幅宽厘米, ISNULL(F_width, 0) * 10 AS 幅宽毫米, Net_wt AS 净重, Gross_wt AS 毛重," +
                                     "Bc AS 班次, convert(nvarchar(10),d_datetime,120) AS 打印日期, Bh AS 包号, J_length AS 卷长, Mjjs AS 每件卷数, DA_code AS 订单号, Jt_sum AS 接头数, jY_code AS 检验员, Jj AS 卷径,Mjh AS 母卷号, Cj AS 车间, CZy_code AS 操作员, Dinfine1 AS 自定义项1, Dinfine2 AS 自定义项2, Dinfine3 AS 自定义项3, Dinfine4 AS 自定义项4," +
                                     "Dinfine5 AS 自定义项5, Dinfine6 AS 自定义项6, Style_No AS 设计风格, bz AS 备注, '' AS R, '' AS T  FROM   WeightBill where bar_code='" + printvouch + "'";
                        sqladp = new SqlDataAdapter(str1, sqlconn);
                        DataSet sqlds3 = new DataSet();
                        sqladp.Fill(sqlds3, "label1");
                        if (sqlds3.Tables["label1"].Rows.Count > 0)
                        {
                            string bsuccuess = ini.ExportExcel(sqlds3, "d:\\barcode\\excel\\Hplabele.xls");
                            if (bsuccuess != "true")
                            {
                                MessageBox.Show("导出文件时出错,文件可能正被打开！\n");
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("找不到文件D:\\barcode\\excel\\Hplabele.xls！", "标签打印", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    //int a = 2;
                    //var templateFullPath =  btFile;
                    //btFormat = btApp.Formats.Open(templateFullPath, false, "");
                    //btFormat.PrintSetup.IdenticalCopiesOfLabel = 1;
                    //btFormat.PrintSetup.NumberSerializedLabels = a;
                    //btFormat.SetNamedSubStringValue("SN", this.textBox10.Text);//向模版传递变量。
                    //btFormat.PrintOut(false, true);
                    //axGRPrintViewer1.Stop();
                    //axGRPrintViewer1.Start();
                    //report.Print(true);
                    string btwFile = "";
                    str1 = "select RFPath from FRtype where RFName='" + comboBox1.Text + "'";
                    sqladp = new SqlDataAdapter(str1, sqlconn);
                    sqladp.Fill(sqlds, "RFlable");
                    if (sqlds.Tables["RFlable"].Rows.Count > 0)
                    {
                        btwFile = sqlds.Tables["RFlable"].Rows[0]["RFPath"].ToString();
                    }

                    textBox5.Text = "";
                    textBox17.Text = (Convert.ToInt16(textBox17.Text) + 1).ToString();

                    Process myprocess = new Process();
                    string exefilePath = "c:\\Program Files \\Seagull\\BarTender Suite";
                    myprocess.StartInfo.FileName = "bartend.exe";//需要启动的程序名    
                    if (System.IO.File.Exists(Path.Combine(exefilePath, "bartend.exe")))
                    {
                        myprocess.StartInfo.FileName = "bartend.exe";//需要启动的程序名       
                        myprocess.StartInfo.Arguments = "\"/f=" + btwFile + "\" /p /x";//启动参数 
                        myprocess.StartInfo.WorkingDirectory = exefilePath;//需要启动的程序所在文件夹
                        myprocess.Start();//启动 
                        if (myprocess.HasExited)//判断进程是否终止
                        {
                            Process[] KillmyProcess = Process.GetProcessesByName("bartend.exe");
                            foreach (Process process in KillmyProcess)
                            {
                                process.Kill();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("请确保BarTend应用程序和模版是否存在！", "提示：");
                    }
                }
                else
                {
                    MessageBox.Show("请新增单据或者录入包号？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("在标签打印过程中发生错误！"+ex.Message , "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
  
            }

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
           // btApp.Quit(BarTender.BtSaveOptions.btSaveChanges);
        }

        //private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //选择仓库更新id
        //        SqlConnection kdconn = new SqlConnection();
        //        kdconn.ConnectionString = Utility.kdconnection;
        //        SqlDataAdapter sqladp = new SqlDataAdapter();
        //        DataSet sqlds = new DataSet();
        //        if (kdconn.State == ConnectionState.Closed)
        //        {
        //            kdconn.Open();
        //        }
        //        string str1 = "";
        //        str1 = "select FItemID  from t_Stock where FName ='" + WlName.Text + "'";
        //        sqladp = new SqlDataAdapter(str1, kdconn);
        //        sqlds = new DataSet();
        //        sqladp.Fill(sqlds, "stock");
        //        if (sqlds.Tables["stock"].Rows.Count > 0)
        //        {
        //            stockId.Text = sqlds.Tables["stock"].Rows[0]["FItemID"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("获得仓库发生错误！", "退出提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            decimal grossWeight = 0;
            decimal netWeight = 0;
            if (textBox5.Text != "") 
             { netWeight = Convert.ToDecimal(textBox5.Text); }
            if (textBox6.Text != "") 
            { grossWeight = Convert.ToDecimal(textBox6.Text); }
            textBox4.Text = (netWeight - grossWeight).ToString();
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键  
            if ((e.KeyChar == 0x2D) && (((System.Windows.Forms.TextBox)sender).Text.Length == 0)) return;   //处理负数  
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((System.Windows.Forms.TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符  
                }
            }  
      
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            decimal grossWeight = 0;
            decimal netWeight = 0;
            if (textBox5.Text != "")
            { netWeight=Convert.ToDecimal(textBox5.Text); }
            if (textBox6.Text != "")
             { grossWeight = Convert.ToDecimal(textBox6.Text); }
            textBox4.Text = (netWeight - grossWeight).ToString();
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x20) e.KeyChar = (char)0;  //禁止空格键  
            if ((e.KeyChar == 0x2D) && (((System.Windows.Forms.TextBox)sender).Text.Length == 0)) return;   //处理负数  
            if (e.KeyChar > 0x20)
            {
                try
                {
                    double.Parse(((System.Windows.Forms.TextBox)sender).Text + e.KeyChar.ToString());
                }
                catch
                {
                    e.KeyChar = (char)0;   //处理非法字符  
                }
            }  
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否真要删除过磅单" + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["编号"].Value.ToString() + "？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                if (dataGridView1.Rows[0].Cells["编号"].Value != null)
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
                    str1 = "delete  from WeightBill where bar_code='" + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["编号"].Value.ToString() + "'";
                    SqlCommand sqlcomm = new SqlCommand(str1, sqlconn);
                    sqlcomm.ExecuteNonQuery();

                    str1 = "select  Bh    as 包号,Bar_code as 编号, PH_barcode  as  批号,Wl_code as 品名编码,Wlname as 品名,Gsm  as 克重,PB    as 配比,F_width  as  幅宽厘米,Net_wt  as  净重,Gross_wt as 毛重,Bc     as  班次,D_Datetime as 打印日期,J_length  as  卷长,Mjjs   as     每件卷数,Jt_sum   as 接头数,jY_code as    检验员,Jj      as    卷径,MJh    as 母卷号, Cj       as   车间,CZy_code  as  操作员,stockName as 仓库 from WeightBill where D_Datetime>=DateAdd(dd,-1,getdate())";
                    sqladp = new SqlDataAdapter(str1, sqlconn);
                    sqladp.Fill(sqlds, "label");
                    if (sqlds.Tables["label"].Rows.Count > 0)
                    {
                        dataGridView1.DataSource = sqlds.Tables["label"];
                    }
                    else
                    {
                        dataGridView1.DataSource = null;
                    }
                    MessageBox.Show("删除成功！", "提示：",MessageBoxButtons.OK,MessageBoxIcon.Information);
  
                }
            }
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            toolStripButton6_Click(null,null);
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
         }

        private void textBox21_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {
                if(e.KeyChar=='.')
               {
                 e.Handled = false;
               } 
                else
                {
                e.Handled = true;
                }
            }
        }
        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CurrentPort.IsOpen)
            {
                CurrentPort.DiscardInBuffer();
                CurrentPort.Close();
            }
        }

        //private void saleName_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //销售范围id
        //        SqlConnection kdconn = new SqlConnection();
        //        kdconn.ConnectionString = Utility.kdconnection;
        //        SqlDataAdapter sqladp = new SqlDataAdapter();
        //        DataSet sqlds = new DataSet();
        //        if (kdconn.State == ConnectionState.Closed)
        //        {
        //            kdconn.Open();
        //        }
        //        string str1 = "";
        //        str1 = "select FItemID,FName  from t_Item where FItemClassID='3001' and FName ='" + saleName.Text + "'";
        //        sqladp = new SqlDataAdapter(str1, kdconn);
        //        sqlds = new DataSet();
        //        sqladp.Fill(sqlds, "Item");
        //        if (sqlds.Tables["Item"].Rows.Count > 0)
        //        {
        //            saleId.Text = sqlds.Tables["Item"].Rows[0]["FItemID"].ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("获得销售范围ID错误！", "退出提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

    }
}
