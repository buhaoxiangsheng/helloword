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

namespace zySoft
{
    public partial class MoSelect : Form
    {
        public MoSelect()
        {
            InitializeComponent();
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
                    //获得金蝶数据
                    string Current = Directory.GetCurrentDirectory();//获取当前根目录
                    Ini ini = new Ini(Current + "/config.ini");
                    if (File.Exists(Current + "/config.ini"))
                    {
                        //存在配置文件，读取相关信息
                      string  servertxt = ini.ReadValue("DbInterface", "ServerName");
                      string  satxt = ini.ReadValue("DbInterface", "Password");
                      string  dbtxt = ini.ReadValue("DbInterface", "dbName");
                      string  usertxt = ini.ReadValue("DbInterface", "UserName");
                      if (kdconn.State == ConnectionState.Closed)
                      {
                          kdconn.ConnectionString = "Data Source=" + servertxt + ";User ID=" + usertxt + ";pwd=" + satxt + ";Initial Catalog=" + dbtxt + ";";
                          kdconn.Open();
                          Utility.kdConnection = "Data Source=" + servertxt + ";User ID=" + usertxt + ";pwd=" + satxt + ";Initial Catalog=" + dbtxt + ";";
                      }

  
                   //string str1 = "Select top 2000 v1.FTranType as FTranType,v1.FInterID as FInterID,v1.Fauxqty as Fauxqty,v1.FQty as FBaseQty,case when t9.FProductUnitID=0 then 0  else v1.FQty/t50.FCoEfficient end as FCUUnitQty,v1.FAuxQtyFinish as FAuxQtyFinish,v1.FAuxStockQty as FAuxStockQty,v1.FAuxQtyPass as FAuxQtyPass,v1.FAuxQtyScrap as FAuxQtyScrap,v1.FAuxQtyForItem as FAuxQtyForItem,v1.FAuxQtyLost as FAuxQtyLost,v1.FFinishTime as FFinishTime,v1.FReadyTime as FReadyTime,v1.FfixTime as FfixTime,t9.FQtyDecimal as FQtyDecimal,t9.FPriceDecimal as FPriceDecimal,v1.FAuxInHighLimitQty as FAuxInHighLimitQty,v1.FInHighLimitQty as FInHighLimitQty,case when t9.FProductUnitID=0 then 0  else v1.FInHighLimitQty/(t50.FCoEfficient) end  as FInHighLimitCuQty,v1.FAuxInLowLimitQty as FAuxInLowLimitQty,v1.FInLowLimitQty as FInLowLimitQty,case when t9.FProductUnitID=0 then 0  else v1.FInLowLimitQty/(t50.FCoEfficient) end  as FInLowLimitCuQty,v1.FHRReadyTime as FHRReadyTime,v1.FAuxCheckCommitQty as FAuxCheckCommitQty,v1.FCheckCommitQty as FCheckCommitQty,v1.FReprocessedAuxQty as FReprocessedAuxQty,v1.FReprocessedQty as FReprocessedQty,v1.FSelDiscardStockInAuxQty as FSelDiscardStockInAuxQty,v1.FSelDiscardStockInQty as FSelDiscardStockInQty,v1.FDiscardStockInAuxQty as FDiscardStockInAuxQty,v1.FDiscardStockInQty as FDiscardStockInQty,v1.FSampleBreakAuxQty as FSampleBreakAuxQty,v1.FSampleBreakQty as FSampleBreakQty from ICMO v1 LEFT OUTER JOIN t_SubMessage t5 ON   v1.FMRP = t5.FInterID  AND t5.FInterID<>0 "+
                   //     " LEFT OUTER JOIN t_WorkType t7 ON   v1.FWorktypeID = t7.FInterID  AND t7.FInterID<>0  LEFT OUTER JOIN t_Department t8 ON   v1.FWorkShop = t8.FItemID  AND t8.FItemID<>0  INNER JOIN t_ICItem t9 ON   v1.FItemID = t9.FItemID  AND t9.FItemID<>0 "+
                   //      "INNER JOIN t_MeasureUnit t12 ON   v1.FUnitID = t12.FItemID  AND t12.FItemID<>0  LEFT OUTER JOIN t_User t30 ON   v1.FBillerID = t30.FUserID  AND t30.FUserID<>0  LEFT OUTER JOIN t_User t32 ON   v1.FConveyerID = t32.FUserID  AND t32.FUserID<>0  LEFT OUTER JOIN t_User t31 ON   v1.FCheckerID = t31.FUserID  AND t31.FUserID<>0 "+
                   //       "LEFT OUTER JOIN SEOrder t22 ON   v1.FOrderInterID = t22.FInterID  AND t22.FInterID<>0  LEFT OUTER JOIN t_Routing t40 ON   v1.FRoutingID = t40.FInterID  AND t40.FInterID<>0  LEFT OUTER JOIN t_MeasureUnit t15 ON   t9.FUnitID = t15.FMeasureUnitID  AND t15.FMeasureUnitID<>0  LEFT OUTER JOIN ICMO t20 ON   v1.FParentInterID = t20.FInterID  AND t20.FInterID<>0 "+
                   //       "LEFT OUTER JOIN CBCostObj t35 ON   v1.FCostObjID = t35.FItemID  AND t35.FItemID<>0  LEFT OUTER JOIN ICBom t36 ON   v1.FBomInterID = t36.FInterID  AND t36.FInterID<>0  LEFT OUTER JOIN t_MeasureUnit t50 ON   t9.FProductUnitID = t50.FItemID  AND t50.FItemID<>0  LEFT OUTER JOIN ICMrpResult t37 ON   v1.FPlanOrderInterID = t37.FPlanOrderInterID  AND t37.FPlanOrderInterID<>0 "+
                   //       "LEFT OUTER JOIN PPOrder t38 ON   v1.FPPOrderInterID = t38.FInterID  AND t38.FInterID<>0  LEFT OUTER JOIN t_Organization t10 ON   v1.FCustID = t10.FItemID  AND t10.FItemID<>0  LEFT OUTER JOIN PPBOM t45 ON   v1.FInterID = t45.FICMOInterID  AND t45.FICMOInterID<>0  LEFT OUTER JOIN v_ICTransType t1 ON   v1.FTranType = t1.FID  AND t1.FID<>0  LEFT OUTER JOIN t_User t33 ON   v1.FConfirmerID = t33.FUserID  AND t33.FUserID<>0 "+
                   //       "INNER JOIN t_SubMessage t244 ON   v1.FCardClosed = t244.FInterID  AND t244.FInterID<>0  LEFT OUTER JOIN t_User t51 ON   v1.FFinCloseer = t51.FUserID  AND t51.FUserID<>0  LEFT OUTER JOIN vw_ICMOStatus t60 ON   v1.FInterID = t60.FInterID  AND t60.FInterID<>0  LEFT OUTER JOIN t_Submessage t52 ON   v1.FStockFlag = t52.FInterID  AND t52.FInterID<>0  LEFT OUTER JOIN v_ICPlanCategoryEntry t9521 ON   v1.FPlanCategory = t9521.FID  AND t9521.FID<>0 "+
                   //       "LEFT OUTER JOIN t_SubMessage t682 ON   v1.FBomCategory = t682.FInterID  AND t682.FInterID<>0   1=1 AND (v1.FPlanFinishDate = '"+startdate.Value +"') AND (( v1.FSuspend = 0 AND ( SELECT fvalue FROM t_systemprofile WHERE fcategory='sh' AND fkey='ITEMSTOCK_FROMTYPE')<0  AND (t9.FProChkMde = 352 OR (((SELECT fvalue FROM t_systemprofile WHERE fcategory='SH' AND fkey='ITEMSTOCK_FROMTYPE_CHECK')=0 AND (v1.FQtyPass>0 OR v1.FQtyScrap+v1.FQtyForItem+v1.FSampleBreakQty>0)))) "+
                   //       "AND v1.FFinClosed = 0) AND  v1.FStatus in(1,2)  AND (v1.FInHighLimitQty>v1.FCommitQty or (Select Count(*) From PPBomEntry t where t.FInterID=t45.FInterID and t.FMaterielType<>371 AND t.FQtyMust-t.FQty>0)>0)  AND ISNULL(v1.FStatus,0) <> 3 AND v1.FTranType = 85)";
                     string str1 = "select v1.FInterID,FBillNo,FWorkShop,t8.FName cdepname,v1.FItemID,t9.FName,FModel,FGMPBatchNo,FQty,FPlanCommitDate,FPlanFinishDate from ICMO v1 LEFT OUTER JOIN t_SubMessage t5 ON   v1.FMRP = t5.FInterID  AND t5.FInterID<>0  LEFT OUTER JOIN t_Department t8 ON   v1.FWorkShop = t8.FItemID  AND t8.FItemID<>0 INNER JOIN t_ICItem t9 ON   v1.FItemID = t9.FItemID  AND t9.FItemID<>0  "+
                                   "  where    v1.FStatus in(1,2) AND v1.FFinClosed = 0   ";
                         string start = Convert.ToDateTime(startdate.Text).ToString("yyyy-MM-dd");
                         string end = Convert.ToDateTime(enddate.Text).ToString("yyyy-MM-dd");
                     if (textBox1.Text != "") 
                     {
                         str1 = str1 + "AND (v1.FPlanCommitDate between '" + start + "'AND '" + end + "' and v1.FBillNo like '%" + textBox1.Text.Trim() + "%')";
                     }
                     else
                     {

                         str1 = str1 + "AND (v1.FPlanCommitDate between '" + start + "'AND '" + end + "')";
                     }

                    sqladp = new SqlDataAdapter(str1, kdconn);
                    sqladp.Fill(sqlds, "ICMO");
                    dataGridView1.DataSource = sqlds.Tables["ICMO"];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("查询过程中发生错误：" + ex.Message, "查询提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void MoSelect_Load(object sender, EventArgs e)
        {
            //载入默认只
            enddate.Text = DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //选择
            if (dataGridView1.RowCount > 0)
            {
                for(int i=0;i<=dataGridView1.Rows.Count-1;i++)
                {
                    DataGridViewCheckBoxCell checkCell = (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells["selcol"];
                    Boolean flag = Convert.ToBoolean(checkCell.Value);
                    if (flag == true)
                    {
                        Utility.voucherid = dataGridView1.Rows[i].Cells["Id"].Value.ToString();
                        Utility.billno = dataGridView1.Rows[i].Cells["vouchcode"].Value.ToString();
                        this.Close();
                    }
                     
                }
            }
        }
    }
}
