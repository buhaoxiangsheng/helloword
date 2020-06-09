using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using grproLib;
using System.IO;


namespace zySoft
{
    public partial class Form3 : Form
    {
        private GridppReport report = new GridppReport();
        private SqlConnection sqlconn = new SqlConnection();
        private SqlConnection kdconn = new SqlConnection();
        public string strwhere = "1=1";
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
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
            SqlDataAdapter sqladp = new SqlDataAdapter();
            DataSet sqlds = new DataSet();
            if (sqlconn.State == ConnectionState.Closed)
            {
                sqlconn.ConnectionString = Utility.DatabaseConnection;
                sqlconn.Open();
            }
            startDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            endDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            dataGridView1.Width = this.Width - 40;
            dataGridView1.Height = this.Height - toolStrip1.Height-40;
            string str1 = "select wlname from aa_inventory";
             sqladp = new SqlDataAdapter(str1, sqlconn);
            sqladp.Fill(sqlds, "inv");
            invname.Items.Clear();
            if (sqlds.Tables["inv"].Rows.Count > 0)
            {
                for (int i = 0; i <= sqlds.Tables["inv"].Rows.Count - 1; i++)
                {
                    invname.Items.Add(sqlds.Tables["inv"].Rows[i]["wlname"].ToString());
                }
                invname.SelectedIndex = 0;

            }
            if (File.Exists(Application.StartupPath + "\\exportModel.grf") == false)
            {
                MessageBox.Show("************************************************************/r/n" +
                       "没有exportModel.grf！请确认板件明细文件是否存在！/r/n" +
                       "************************************************************");
            }
            report.LoadFromFile(Application.StartupPath + "\\exportModel.grf");
            report.DetailGrid.Recordset.ConnectionString = Utility.reportConnection;
            //关联报表取数事件
            report.Initialize += new _IGridppReportEvents_InitializeEventHandler(ReportInitialize);
            axGRPrintViewer1.Visible = false; 
        }
        //获取查询月份
        private void ReportInitialize()
        {
            report.DetailGrid.Recordset.QuerySQL = strwhere;
           // report.ParameterByName("strWhere").AsString = strwhere;

        }
       

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            //查询数据
            string str1 = "";
            string invcode = "";
            SqlConnection sqlconn = new SqlConnection();
            SqlDataAdapter sqladp = new SqlDataAdapter();
            DataSet sqlds = new DataSet();
            
            if (sqlconn.State == ConnectionState.Closed)
            {sqlconn.ConnectionString = Utility.DatabaseConnection;
                sqlconn.Open();
            }
            if (kdconn.State == ConnectionState.Closed)
            {
                kdconn.ConnectionString = Utility.kdConnection;
                kdconn.Open();
            }
            //更新产品的默认默认仓库
            str1 = "select WLId from WeightBill where isnull(stockId,'')=''";
            SqlDataAdapter sqladp2 = new SqlDataAdapter(str1, sqlconn);
            DataSet sqlds2 = new DataSet();
            sqladp2.Fill(sqlds2, "stock");
            for (int i = 0; i <= sqlds2.Tables["stock"].Rows.Count - 1; i++)
            {
                invcode  = sqlds2.Tables["stock"].Rows[0][0].ToString();
                str1 = "select top 1 fitemid,fname from t_stock where fitemid in ( select fdefaultloc from t_icitem where fitemid='" + invcode + "')";
                sqladp = new SqlDataAdapter(str1, kdconn);
                sqlds = new DataSet();
                sqladp.Fill(sqlds, "fitem");
                if (sqlds.Tables["fitem"].Rows.Count >0 )
                {
                    string stockid = sqlds.Tables["fitem"].Rows[0]["fitemid"].ToString();
                    string stockname = sqlds.Tables["fitem"].Rows[0]["fname"].ToString();
                    if (stockid != "")
                    {
                        str1 = "update WeightBill set stockid='" + stockid + "',stockname='" + stockname + "' where  WLId='" + invcode + "'";
                        SqlCommand sqlcomm = new SqlCommand(str1,sqlconn);
                        sqlcomm.ExecuteNonQuery(); 
                    }
                }
            }

            str1 = "select 'false' selcol,vouchtype,vouchcode,vouchdate,carno,partnerName,cinvname,cinvstd,grossWeight,tareWeight,netWeight,water,dhValue,netValue,cmemo,weightTime2 ,cmaker,kdvouchcode from weightbills where 1=1";
            strwhere = "select vouchtype,vouchcode,vouchdate,carno,partnerName,cinvname,cinvstd,grossWeight,tareWeight,netWeight,water,dhValue,netValue,cmemo,weightTime2 ,cmaker,kdvouchcode from weightbills where 1=1";
            if (startDate.Text != "")
            {
                str1 = str1 + " and vouchdate>='" + startDate.Text + "' and vouchdate<='" + endDate.Text + "'";
                strwhere = strwhere + " and vouchdate>='" + startDate.Text + "' and vouchdate<='" + endDate.Text + "'";
            }
            if (invname.Text.Trim() != "")
            {
                str1 = str1 + "  and cinvname like '%" + invname.Text.Trim() + "%'";
                strwhere = strwhere + "and cinvname like '%" + invname.Text.Trim() + "%'";
            }
            if (textBox1.Text.Trim() != "") 
            {
                str1 = str1 + "  and partnerName like'%" + textBox1.Text.Trim() + "%'";
                strwhere = strwhere + "and partnerName like '%" + textBox1.Text.Trim() + "%'";
            }
            if (textBox2.Text.Trim() != "")
            {
                str1 = str1 + "  and vouchcode like'" + textBox2.Text.Trim() + "%'";
                strwhere = strwhere + "and vouchcode like'%" + textBox2.Text.Trim() + "%'";
            }

            if (textBox7.Text.Trim() != "")
            {
                str1 = str1 + "  and carno like '%" + textBox7.Text.Trim() + "%'";
                strwhere = strwhere + "and carno like '%" + textBox7.Text.Trim() + "%'";
            }
            str1 += "  order by vouchcode,vouchdate";
            strwhere += " order by vouchcode,vouchdate";
            sqladp = new SqlDataAdapter(str1, sqlconn);
            sqladp.Fill(sqlds, "label");
            dataGridView1.DataSource = sqlds.Tables["label"];

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {  
            int l = 1;
            string moCode = "";
            string moId = "";
            string stockid = "";
            string rkCode = "";
            //获得任务单号
            string str1 = "";
            string vouchcode = "";
            string dbvoucode = "";
            string userid = "16393";
            string ffmanager = "3105";
            string fsmanager = "3105";
            string finterid = "";
            decimal ysQty = 0;
            SqlTransaction trans = null;
            try
            {

                //获得finterID
                SqlCommand sqlcomm = new SqlCommand();
                if (kdconn.State == ConnectionState.Closed)
                {
                    kdconn.ConnectionString = Utility.kdConnection;
                    kdconn.Open();
                }
                str1 = "select  top 1 kduserid from aa_user where username = '" + CommonClass.userName + "'";
                SqlDataAdapter sqladp = new SqlDataAdapter(str1, sqlconn);
                DataSet sqlds = new DataSet();
                sqladp.Fill(sqlds, "user");
                if (sqlds.Tables["user"].Rows.Count > 0)
                {
                    userid = sqlds.Tables["user"].Rows[0]["kduserid"].ToString();
                    ffmanager = userid;
                    fsmanager = userid;
                }
                if (userid == "")
                {
                    MessageBox.Show("无法获得用户对应的金蝶id，请进行用户设置", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                trans = kdconn.BeginTransaction();
                for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                {
                    DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells["selcol"];
                    if (checkCell.Value == DBNull.Value) { checkCell.Value = false; }
                    Boolean flag = Convert.ToBoolean(checkCell.Value);
                    if (flag == true)
                    {
                        if (l == 1)
                        {
                            moCode = dataGridView1.Rows[i].Cells["sourceVoucode"].Value.ToString();
                            stockid = dataGridView1.Rows[i].Cells["stockid"].Value.ToString();
                            moId = "'" + dataGridView1.Rows[i].Cells["bar_code"].Value.ToString() + "'";
                            if (dataGridView1.Rows[i].Cells["rkvouchcode"].Value == null)
                            { rkCode = ""; }
                            else
                            { rkCode = dataGridView1.Rows[i].Cells["rkvouchcode"].Value.ToString(); }
                            if (rkCode != "")
                            {
                                MessageBox.Show("有单据已经生成入库单！请调整", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            if (stockid == "")
                            {
                                MessageBox.Show("单据没有入库仓库！请调整", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            l = l + 1;
                        }
                        else
                        {
                            stockid = dataGridView1.Rows[i].Cells["stockid"].Value.ToString();
                            string moCode1 = dataGridView1.Rows[i].Cells["sourceVoucode"].Value.ToString();
                            moId = moId + "," + "'" + dataGridView1.Rows[i].Cells["bar_code"].Value.ToString() + "'";
                            if (dataGridView1.Rows[i].Cells["rkvouchcode"].Value != null)
                            { rkCode = dataGridView1.Rows[i].Cells["rkvouchcode"].Value.ToString(); }
                            if (moCode1 != moCode)
                            {
                                MessageBox.Show("选择的单据不是相同的任务单！请调整", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            if (rkCode != "")
                            {
                                MessageBox.Show("有单据已经生成入库单！请调整", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            if (stockid == "")
                            {
                                MessageBox.Show("单据没有入库仓库！请调整", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            l = l + 1;
                        }
                    }
                }
                if (moId == "" || moCode == "") { return; }
                str1 = "declare @p2 int   set @p2=NULL  exec GetICMaxNum 'ICStockBill',@p2 output,1," + userid + "  select @p2";
                SqlDataAdapter sqladp2 = new SqlDataAdapter(str1, kdconn);
                DataSet sqlds2 = new DataSet();
                sqladp2.SelectCommand.Transaction = trans;
                sqladp2.Fill(sqlds2, "FInterID");
                if (sqlds2.Tables["FInterID"].Rows.Count > 0)
                {
                    finterid = sqlds2.Tables["FInterID"].Rows[0][0].ToString();
                }

                str1 = " Select v1.FBillNo,v1.FQty,v1.FCommitQty,v1.FQtyFinish,ISNULL(v1.FQtyFinish,0)-ISNULL(v1.FCommitQty,0) ysQty  from ICMO v1 where  v1.FBillNo = '" + moCode + "'  AND  v1.FStatus in(1,2)  AND (v1.FInHighLimitQty>v1.FCommitQty)  and FQtyFinish>0 ";
                sqladp2 = new SqlDataAdapter(str1, kdconn);
                sqlds2 = new DataSet();
                sqladp2.SelectCommand.Transaction = trans;
                sqladp2.Fill(sqlds2, "icmo");
                if (sqlds2.Tables["icmo"].Rows.Count > 0)
                {
                    ysQty =Convert.ToDecimal(sqlds2.Tables["icmo"].Rows[0]["ysQty"].ToString());
                }
                else
                {
                    MessageBox.Show("发生错误！,单据还没有报检", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //获得单据号
                str1 = "select val1+SUBSTRING('000000',1,flength2-LEN(val2))+val2 from  (select FProjectVal val1,flength flength1 from t_billcoderule where fbilltypeid='2' and FProjectID=1) a1, (select FProjectVal val2,flength flength2 from t_billcoderule where fbilltypeid='2' and FProjectID=3) a2";
                sqladp2 = new SqlDataAdapter(str1, kdconn);
                sqlds2 = new DataSet();
                sqladp2.SelectCommand.Transaction = trans;
                sqladp2.Fill(sqlds2, "FInterID");
                if (sqlds2.Tables["FInterID"].Rows.Count > 0)
                {
                    vouchcode = sqlds2.Tables["FInterID"].Rows[0][0].ToString();
                }
                string fuuid = System.Guid.NewGuid().ToString();
                dbvoucode = vouchcode;
                //插入主表数据
                str1 = "insert into ICStockBill(FBrNo,FInterID,FTranType,FDate,FBillNo,FDeptID,FFManagerID,FSManagerID,FBillerID,FPosted,FROB,FStatus,FUpStockWhenSave,FUUID,FManageType,FSelTranType,FsourceType,FVchInterID) select top 1 '0','" + finterid + "','2','" + DateTime.Now.ToString("yyyy-MM-dd 00:00:00") + "','" + vouchcode + "',deptId,'" + ffmanager + "','" + fsmanager + "','" + userid + "','0','1','0','0','"
                       + fuuid + "','0','85','37521','0' from zyBarcode..WeightBill where bar_code in (" + moId + ")";
                sqlcomm = new SqlCommand(str1, kdconn);
                sqlcomm.Transaction = trans;
                sqlcomm.ExecuteNonQuery();

                //取得入库单仓库
                //string stockid = "";
                ////str1 = "select FItemID  from t_Stock where FName ='" + comboBox2.Text + "'";
                //sqladp2 = new SqlDataAdapter(str1, kdconn);
                //sqlds2 = new DataSet();
                //sqladp2.Fill(sqlds2, "stock");
                //if (sqlds2.Tables["stock"].Rows.Count > 0)
                //{
                //    stockid = sqlds2.Tables["stock"].Rows[0]["FItemID"].ToString();
                //}
                str1 = "insert into ICStockBillEntry(FBrNo,FInterID,FEntryID,FItemID,FQty,FAuxQty,FUnitID,FDCStockID,FPlanMode,FChkPassItem,FBatchNo,FSourceTranType,FSourceInterId,FSourceBillNo,FICMOBillNo,FICMOInterID,FEntrySelfA0245,FDCSPID,FSnListID,FSecQty,FSecCoefficient,FEntrySelfA0240,FQtyMust,FAuxQtyMust) select  '0','" + finterid + "',ROW_NUMBER() over (order by bh) rownum,wlid,Net_wt,Net_wt,unitId,stockId,'14036','1058',ph_barcode,'85',sourceID,sourceVoucode,sourceVoucode" +
                       ",sourceID,Gross_wt,0,0,1,Net_wt,bh," + ysQty + " as ysqty," + ysQty + " as  ysqty  from zyBarcode..WeightBill where bar_code in (" + moId + ")";
                sqlcomm = new SqlCommand(str1, kdconn);
                sqlcomm.Transaction = trans;
                sqlcomm.ExecuteNonQuery();
                //检查是否插入成功
                str1 = "select * from ICStockBill join ICStockBillEntry on ICStockBill.FInterID= ICStockBillEntry.FInterID where ICStockBill.FInterID='" + finterid + "'";
                sqladp2 = new SqlDataAdapter(str1, kdconn);
                sqlds2 = new DataSet();
                sqladp2.SelectCommand.Transaction = trans;
                sqladp2.Fill(sqlds2, "checkvouch");
                if (sqlds2.Tables["checkvouch"].Rows.Count == 0)
                {

                    MessageBox.Show("没有生成成功！", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    //更新单据应收数量FQtyMust,FAuxQtyMust
                    //str1 = "update ICStockBillEntry set FQtyMust=avgnetweight,FAuxQtyMust=avgnetweight from (select round(sum(FQty)/COUNT(*),4) avgnetweight from ICStockBillEntry where FInterID='" + finterid + "') netweight where FInterID='" + finterid + "'";
                    //sqlcomm = new SqlCommand(str1, kdconn);
                    //sqlcomm.Transaction = trans;
                    //sqlcomm.ExecuteNonQuery();
                    str1 = " update ICMO set FCommitQty=isnull(FCommitQty,0)+(select sum(FQty) from ICStockBillEntry where FInterID='" + finterid + "'),FAuxCommitQty=isnull(FCommitQty,0)+(select sum(FQty) from ICStockBillEntry where FInterID='" + finterid + "') where  FBillNo='" + moCode + "'";
                    sqlcomm = new SqlCommand(str1, kdconn);
                    sqlcomm.Transaction = trans;
                    sqlcomm.ExecuteNonQuery();
                    str1 = "update t_billcoderule set FProjectVal =FProjectVal+1 from t_billcoderule where fbilltypeid='2' and FProjectID=3";
                    sqlcomm = new SqlCommand(str1, kdconn);
                    sqlcomm.Transaction = trans;
                    sqlcomm.ExecuteNonQuery();
                    str1 = "update ICBillNo set FCurNo=FCurNo+1,FDesc='CIN+'+substring(FFormat,1,LEN(FFormat)-LEN(fcurno+1))+convert(varchar(10),FCurNo+1)  FROM ICBillNo where FBillID=2";
                    sqlcomm = new SqlCommand(str1, kdconn);
                    sqlcomm.Transaction = trans;
                    sqlcomm.ExecuteNonQuery();

                    if (sqlconn.State == ConnectionState.Closed)
                    {
                        sqlconn.ConnectionString = Utility.DatabaseConnection;
                        sqlconn.Open();
                    }
                    str1 = "update WeightBill set rkVouchcode='" + vouchcode + "' where bar_code in (" + moId + ")";
                    sqlcomm = new SqlCommand(str1, sqlconn);
                    //sqlcomm.Transaction = trans;
                    sqlcomm.ExecuteNonQuery();
                    trans.Commit(); 
                    toolStripButton1_Click(null,null);
                    MessageBox.Show("生成入库单：" + vouchcode + " 1张！", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                MessageBox.Show("发生错误！"+ex.Message , "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (this.dataGridView1.IsCurrentCellDirty) //有未提交的更//改
            {
                this.dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            string moId = "";
            string rkCode = "";
            int l=0;
            if (sqlconn.State == ConnectionState.Closed)
            {
                sqlconn.ConnectionString = Utility.DatabaseConnection;
                sqlconn.Open();
            }
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i <= dataGridView1.Rows.Count-1; i++)
                {
                    DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells["selcol"];
                    if (checkCell.Value == DBNull.Value) { checkCell.Value = false; }
                    Boolean  flag = Convert.ToBoolean(checkCell.Value);
                    if (flag == true)
                    {
                        if (l== 0)
                        {
                            moId = "'" + dataGridView1.Rows[i].Cells["bar_code"].Value.ToString() + "'";
                            if (dataGridView1.Rows[i].Cells["rkvouchcode"].Value == null)
                            { rkCode =""; }
                            else
                            { rkCode = dataGridView1.Rows[i].Cells["rkvouchcode"].Value.ToString(); }
                            if (rkCode != "")
                            {
                                MessageBox.Show("有单据已经生成入库单！请调整", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            l = l + 1;
                        }
                        else
                        {
                              moId = moId + "," + "'" + dataGridView1.Rows[i].Cells["bar_code"].Value.ToString() + "'";
                              if (dataGridView1.Rows[i].Cells["rkvouchcode"].Value != null) 
                               { rkCode = rkCode + "," + dataGridView1.Rows[i].Cells["rkvouchcode"].Value.ToString(); }
                              if (rkCode != "")
                              {
                                  MessageBox.Show("已经生成入库单的不能调整！请调整", "产成品入库单生成", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                  return;
                              }
                              l = l + 1;
                       }
                      
                    }


                }

            }
            if (moId != "")
            {
                checkvouch checkfrm = new checkvouch();
                checkfrm.ShowDialog();
                if (CommonClass.checkid != null)
                {
                    string str1 = "update WeightBill set checkFlag='" + CommonClass.checkid + "'  where bar_code in (" + moId + ")";
                    SqlCommand sqlcomm = new SqlCommand(str1, sqlconn);
                    sqlcomm.ExecuteNonQuery();
                }
                if (CommonClass.stockid != null  && CommonClass.stockname != null)
                {
                    string str1 = "update WeightBill set checkFlag='" + CommonClass.checkid + "',stockId='" + CommonClass.stockid + "',stockName='" + CommonClass.stockname + "'  where bar_code in (" + moId + ")";
                    SqlCommand sqlcomm = new SqlCommand(str1, sqlconn);
                    sqlcomm.ExecuteNonQuery();
                  
                }
                if (CommonClass.saleAreaid != null  && CommonClass.saleAreaName != null)
                {
                    string str1 = "update WeightBill set saleAreaId='" + CommonClass.saleAreaid + "',saleAreaName='" + CommonClass.saleAreaName + "'  where bar_code in (" + moId + ")";
                    SqlCommand sqlcomm = new SqlCommand(str1, sqlconn);
                    sqlcomm.ExecuteNonQuery();
                  
                }
                toolStripButton1_Click(null, null);
                MessageBox.Show("调整成功！", "检验调整", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                {
                    MessageBox.Show("请先选择调整的行！", "检验调整", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                //axGRPrintViewer1.Stop();
                //axGRPrintViewer1.Start();
                report.PrintPreview(true);
            }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            //删除数据
            for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
            {
                DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells["selcol"];
                if (checkCell.Value == DBNull.Value) { checkCell.Value = false; }
                Boolean flag = Convert.ToBoolean(checkCell.Value);
                if (flag == true)
                {
                    if (MessageBox.Show("是否真要删除过磅单" + dataGridView1.Rows[i].Cells["bar_code"].Value.ToString() + "？", "退出提示：", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        if (dataGridView1.Rows[i].Cells["bar_code"].Value != null)
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
                            str1 = "delete  from WeightBills where vouchcode='" + dataGridView1.Rows[i].Cells["vouchcode"].Value.ToString() + "'";
                            SqlCommand sqlcomm = new SqlCommand(str1, sqlconn);
                            sqlcomm.ExecuteNonQuery();
                            toolStripButton1_Click(null, null);
                            MessageBox.Show("删除成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }

                    }
                }
            }

        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            //全选
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if ((Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == false))
                    {
                        dataGridView1.Rows[i].Cells[0].Value = "True";
                    }
                    else
                        continue;
                }
            }
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            int l=0;
            //清除入库号
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i <= dataGridView1.Rows.Count - 1; i++)
                {
                    DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells["selcol"];
                    if (checkCell.Value == DBNull.Value) { checkCell.Value = false; }
                    Boolean flag = Convert.ToBoolean(checkCell.Value);
                    if (flag == true)
                    {
                        if (l == 1)
                        {
                            password passfrm = new password();
                            passfrm.ShowDialog();
                            if (CommonClass.pass == true)
                            {
                                string str1 = "update WeightBill set rkVouchcode =null where bar_code='" + dataGridView1.Rows[i].Cells["bar_code"].Value.ToString() + "'"; ;
                                SqlCommand sqlcomm = new SqlCommand(str1, sqlconn);
                                sqlcomm.ExecuteNonQuery();

                            }
                            else
                            {
                                break; 
                            }
                        }
                        else
                        {
                            string str1 = "update WeightBill set rkVouchcode =null where bar_code='" + dataGridView1.Rows[i].Cells["bar_code"].Value.ToString() + "'"; ;
                            SqlCommand sqlcomm = new SqlCommand(str1, sqlconn);
                            sqlcomm.ExecuteNonQuery();
 
                        }
                    }
                    toolStripButton1_Click(null, null);
                    MessageBox.Show("更新成功！", "提示：", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }       

            }
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {

        }
    }
}

