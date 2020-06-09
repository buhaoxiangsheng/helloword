using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zySoft
{
    public partial class selectkh : Form
    {
        public selectkh()
        {
            InitializeComponent();
        }
        carweighing carweighing;
        Form6 form6;
        int a = 0;

        //过磅单
        public selectkh(string v, SqlConnection sqlconn ,carweighing car)
        {
            a = 1;
            InitializeComponent();
            this.type = v;
            this.sqlconn = sqlconn;
            this.carweighing = car;
        }

        //查询单
        public selectkh(string v, SqlConnection sqlconn, Form6 car)
        {
            a = 2;
            InitializeComponent();
            this.type = v;
            this.sqlconn = sqlconn;
            this.form6 = car;
        }

        string s = "";
        SqlDataAdapter sqladpss = new SqlDataAdapter();
        SqlCommand sqlcomm = new SqlCommand();
        DataTable dts;
        SqlConnection sqlconns;
        private string type;
        private SqlConnection sqlconn;


        private void selectkh_Load(object sender, EventArgs e)
        {
            label3.Text = "分类";
            label1.Text = "编号";
            label2.Text = "名称";

            if (a==1) {
            if(carweighing.toolStripComboBox1.Text == "委外出库"&& carweighing.label25.Text != "")
            {
                    string typeid = carweighing.label25.Text;
                    if (type == "委外出库")
                    {
                        setvwck(sqlconn);
                        label4.Text = "委外出库";
                        label5.Visible = true;
                        dateTimePicker1.Visible = true;
                        label6.Visible = true;
                        dateTimePicker2.Visible = true;
                        label1.Text = "客户编号";
                        label2.Text = "存货编号";
                        label3.Text = "加工单编号";
                    }else  if (type == "往来单位")
                    {
                        setPartner(sqlconn, typeid);
                        label4.Text = "往来单位";
                    }
                    else if (type == "存货")
                    {
                        serinvInventory(sqlconn, typeid);
                        label4.Text = "存货";
                    }
                    else if (type == "仓库")
                    {
                        setWarehouse(sqlconn, typeid);
                        label4.Text = "仓库";
                    }
                    else if(type == "批号")
                    {
                        setbatch(sqlconn);
                        label4.Text = "批号";
                    }
                    sqlconns = sqlconn;
                    return ;
                }
                else     if (carweighing.toolStripComboBox1.Text == "委外入库" && carweighing.label25.Text != "")
                {
                    if (type == "批号")
                    {
                        setbatch(sqlconn);
                        label4.Text = "批号";
                    }
                    setvwrk(sqlconn);
                    label4.Text = "委外入库";
                    label5.Visible = true;
                    dateTimePicker1.Visible = true;
                    label6.Visible = true;
                    dateTimePicker2.Visible = true;
                    label1.Text = "供应商编号";
                    label2.Text = "存货编号";
                    label3.Text = "加工单编号";
                    sqlconns = sqlconn;
                    return;

                }
                else if (carweighing.toolStripComboBox1.Text == "入库单" && carweighing.label25.Text != "")
                {
                    if (type == "批号")
                    {
                        setbatch(sqlconn);
                        label4.Text = "批号";
                        return;
                    }
                    //   setvwrk(sqlconn);
                    setcgvouchar(sqlconn);
                    label4.Text = "入库单";
                    label5.Visible = true;
                    dateTimePicker1.Visible = true;
                    label6.Visible = true;
                    dateTimePicker2.Visible = true;
                    label1.Text = "供应商编号";
                    label2.Text = "存货编号";
                    label3.Text = "采购订单编号";
                    sqlconns = sqlconn;
                    return;

                }
                else if (carweighing.toolStripComboBox1.Text == "出库单" && carweighing.label25.Text != "")
                {
                    if (type == "批号")
                    {
                        setbatch(sqlconn);
                        label4.Text = "批号";
                        return;
                    }

                    setckvouchar(sqlconn);

                    label4.Text = "出库单";
                    label5.Visible = true;
                    dateTimePicker1.Visible = true;
                    label6.Visible = true;
                    dateTimePicker2.Visible = true;
                    label1.Text = "客户编号";
                    label2.Text = "存货编号";
                    label3.Text = "销售订单编号";
                    sqlconns = sqlconn;
                    return;

                }
            }
              if (type == "往来单位")
            {
                setPartner(sqlconn);
                label4.Text = "往来单位";
            }
            else if (type == "存货")
            {
                serinvInventory(sqlconn);
                label4.Text = "存货";
            }
            else if (type == "仓库")
            {
                setWarehouse(sqlconn);
                label4.Text = "仓库";
            }
            else if (type == "批号")
            {
                setbatch(sqlconn);
                label4.Text = "批号";
            }
              else if (type=="委外订单号")
              {
                label5.Visible = true;
                dateTimePicker1.Visible = true;
                label6.Visible = true;
                dateTimePicker2.Visible = true;
                label2.Visible = false;
                textBox2.Visible = false;
                label3.Visible = false;
                comboBox1.Visible = false;
                setomcode(sqlconn);
                label4.Text = "委外订单号";
              }


            sqlconns = sqlconn;
            


        }
     


        //往来单位
        private void setPartner(SqlConnection sqlconn)
        {
            s = "select name  from  AA_PartnerClass ";
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            comboBox1.DataSource = dts;
            comboBox1.DisplayMember = "name";
            comboBox1.DisplayMember = "name";
            comboBox1.SelectedIndex = -1;
            s = "select a.id,  a.code 编号 ,a.name 名称 ,b.name 分类 from AA_Partner  a  left join AA_PartnerClass b on a.idpartnerclass =b.id  where 1=1  ";
            if (a == 1)
            {
                if (carweighing.toolStripComboBox1.Text == "入库单")
                {

                    s = s + " and a. partnerType in(226,228)";

                }
                else if (carweighing.toolStripComboBox1.Text == "出库单")
                {
                    s = s + " and a. partnerType in(211,228)";
                }
            }
            
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            dataGridView1.DataSource = dts;
        }
        //往来单位委外
        private void setPartner(SqlConnection sqlconn,string typeid)
        {
            s = "select name  from  AA_PartnerClass ";
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            comboBox1.DataSource = dts;
            comboBox1.DisplayMember = "name";
            comboBox1.DisplayMember = "name";
            comboBox1.SelectedIndex = -1;
            s = "select a.id,  a.code 编号 ,a.name 名称 ,b.name 分类,c.code 委外单据号 from AA_Partner  a  left join AA_PartnerClass b on a.idpartnerclass =b.id join  OM_OutSourceOrder c on c.idPartner=a.id where 1 =1  ";
            if (typeid!=""&&typeid!=null) 
            {
                s = s + " and c.id="+typeid+ "";
            }
            if (a == 1)
            {
                if (carweighing.toolStripComboBox1.Text == "入库单")
                {

                    s = s + " and a. partnerType in(226,228)";

                }
                else if (carweighing.toolStripComboBox1.Text == "出库单")
                {
                    s = s + " and a. partnerType in(211,228)";
                }
            }

            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            dataGridView1.DataSource = dts;
        }

        //入库单
        private void setcgvouchar(SqlConnection sqlconn)
        {
            s = "select code name  from  PU_PurchaseOrder ";
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            comboBox1.DataSource = dts;
            comboBox1.DisplayMember = "name";
            comboBox1.DisplayMember = "name";
            comboBox1.SelectedIndex = -1;
            s = "select  a.id 采购订单id,a.code 采购订单编号,c.name 供应商名称,d.name 存货名称 ,e.name 仓库名称,b.origDiscountPrice 单价,a.voucherdate 订单时间    from  PU_PurchaseOrder  a join  PU_PurchaseOrder_b b on a.id=b.idPurchaseOrderDTO  join AA_Partner c on a.idpartner=c.id join AA_Inventory d on b.idinventory=d.id join AA_Warehouse e on b.idwarehouse =e.id where VoucherState =  N'189' ";
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            dataGridView1.DataSource = dts;
        }
        //出库单
        private void setckvouchar(SqlConnection sqlconn)
        {
            s = "select code name  from  SA_SaleOrder ";
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            comboBox1.DataSource = dts;
            comboBox1.DisplayMember = "name";
            comboBox1.DisplayMember = "name";
            comboBox1.SelectedIndex = -1;
            s = "select   a.id 销售订单id,a.code 销售订单编号,c.name 客户名称,d.name 存货名称 ,e.name 仓库名称,b.origDiscountPrice 单价,a.voucherdate 订单时间    from  SA_SaleOrder  a join  SA_SaleOrder_b b on a.id=b.idSaleOrderDTO  join AA_Partner c on a.idcustomer=c.id join AA_Inventory d on b.idinventory=d.id join AA_Warehouse e on b.idwarehouse =e.id where VoucherState =  N'189' ";
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            dataGridView1.DataSource = dts;
        }

        //存货
        private void serinvInventory(SqlConnection sqlconn)
        {
            s = "select name  from  AA_InventoryClass ";
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            comboBox1.DataSource = dts;
            comboBox1.DisplayMember = "name";
            comboBox1.DisplayMember = "name";
            comboBox1.SelectedIndex = -1;
            s = "select a.id, a.code 编号 ,a.name 名称 ,b.name 分类 from AA_Inventory  a  left join AA_InventoryClass b on a.idInventoryclass =b.id    ";
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            dataGridView1.DataSource = dts;
        }
        //存货委外
        private void serinvInventory(SqlConnection sqlconn,string typeid)
        {
            s = "select name  from  AA_InventoryClass ";
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            comboBox1.DataSource = dts;
            comboBox1.DisplayMember = "name";
            comboBox1.DisplayMember = "name";
            comboBox1.SelectedIndex = -1;
            s = "select a.id, a.code 编号 ,a.name 名称 ,b.name 分类 from AA_Inventory  a  left join AA_InventoryClass b on a.idInventoryclass =b.id   join OM_OutSourceOrder_Material c on c.idinventory =a.id    where 1=1";
            if (typeid != "" && typeid != null)
            {
                s = s + " and c.voucherId=" + typeid + "";
            }
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            dataGridView1.DataSource = dts;
            
        }
        //仓库
        private void setWarehouse(SqlConnection sqlconn)
        {

            s = "select id, code 编号 ,name 名称, address  发货地址 from AA_Warehouse ";
           
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            dataGridView1.DataSource = dts;

        }
        private void setWarehouse(SqlConnection sqlconn , string typeid)
        {
           
                s = "select a.id, a.code 编号 ,a.name 名称, a.address  发货地址 from AA_Warehouse a join OM_OutSourceOrder_Material c on c.idwarehouse = a.id  where 1=1";
                if (typeid != "" && typeid != null)
                {
                    s = s + " and c.voucherId=" + typeid + "";

                }
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            dataGridView1.DataSource = dts;

        }
        //批号
        private void setbatch(SqlConnection sqlconn)
        {
            string wou = "";
            string wouid = "";
            string inv = "";
            string invid = "";
           
            if (a==1)
            {
                wou= carweighing.comboBox4.Text;
                inv = carweighing.comboBox5.Text;
                wouid = carweighing.label19.Text;
                invid = carweighing.label20.Text;
            }
            else if (a==2)
            {
                wou = form6.comboBox6.Text;
                inv = form6.comboBox7.Text;
                wouid = form6.label19.Text;
                invid = form6.label20.Text;
            }


            if (wou != "" && inv != "")
            {

                s = " select batch from ST_CurrentStock where idwarehouse= "+ wouid + " and idinventory=  " + invid + "";
                sqladpss = new SqlDataAdapter(s, sqlconn);
                dts = new DataTable();
                sqladpss.Fill(dts);
                comboBox1.DataSource = dts;
                comboBox1.DisplayMember = "batch";
                comboBox1.DisplayMember = "batch";
                comboBox1.SelectedIndex = -1;
                label3.Text = "批号";
                label2.Visible = false;
                label1.Visible = false;
                textBox1.Visible = false;
                textBox2.Visible = false;

              
            }
            else {
                MessageBox.Show("仓库或存货不能为空", "查询失败", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SqlDataAdapter sqladp = new SqlDataAdapter();
            DataSet sqlds = new DataSet();
            if (sqlconn.State == ConnectionState.Closed)
            {
                sqlconn.ConnectionString = Utility.DatabaseConnection;
                sqlconn.Open();
            }
            s = "select  b.name 仓库,c.name 存货 ,a.batch 批号,a.CanuseBaseQuantity 可用量  from ST_CurrentStock a  left join AA_Warehouse  b on a.idwarehouse=b.id  join AA_Inventory c on a.idinventory =c.id where a.idwarehouse = " + wouid + " and a.idinventory = " + invid + "   ";
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            dataGridView1.DataSource = dts;

        }
          //委外出库
        private void setvwck(SqlConnection sqlconn){
            s = "select code name  from OM_OutSourceOrder ";
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            comboBox1.DataSource = dts;
            comboBox1.DisplayMember = "name";
            comboBox1.DisplayMember = "name";
            comboBox1.SelectedIndex = -1;


             s = "select a.id id ,a.code 委外加工单编号, d.code 供应商编号 ,d.name 供应商名称 ,c.code 存货编号 ,c.name 存货名称,e.code 仓库编号,e.name 仓库名称,a.voucherdate 单据时间 from OM_OutSourceOrder a join  OM_OutSourceOrder_Material  b on a.id=b.voucherId    join  AA_Inventory c on b.idinventory=c.id  join AA_Partner d on a.idPartner=d.id  join AA_Warehouse e on b.idWarehouse=e.id where VoucherState =  N'189' ";
          //  s = " select code 委外加工单编号,id id,voucherdate 委外加工单时间 from OM_OutSourceOrder";
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            dataGridView1.DataSource = dts;

        }
        //委外入库
        private void setvwrk(SqlConnection sqlconn)
        {
            s = "select code name  from OM_OutSourceOrder ";
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            comboBox1.DataSource = dts;
            comboBox1.DisplayMember = "name";
            comboBox1.DisplayMember = "name";
            comboBox1.SelectedIndex = -1;


                  s = "select a.id id,a.code 委外加工单编号, d.code 供应商编号 ,d.name 供应商名称 ,c.code 存货编号 ,c.name 存货名称,e.code 仓库编号,e.name 仓库名称,a.voucherdate 单据时间 from OM_OutSourceOrder a join  OM_OutSourceOrder_b  b on a.id=b.idOutSourceOrderDTO    join  AA_Inventory c on b.idinventory=c.id  join AA_Partner d on a.idPartner=d.id  join AA_Warehouse e on b.idWarehouse=e.id  where 1 =1 and VoucherState =  N'189'  ";
          //  s = " select code 委外加工单编号,id id,voucherdate 委外加工单时间 from OM_OutSourceOrder";
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            dataGridView1.DataSource = dts;

        }
        //委外单号
        private void setomcode(SqlConnection sqlconn)
        {
            SqlDataAdapter sqladp = new SqlDataAdapter();
            DataSet sqlds = new DataSet();
            if (sqlconn.State == ConnectionState.Closed)
            {
                sqlconn.ConnectionString = Utility.DatabaseConnection;
                sqlconn.Open();
            }
            s = "select code 编号,id from OM_OutSourceOrder  ";
            sqladpss = new SqlDataAdapter(s, sqlconn);
            dts = new DataTable();
            sqladpss.Fill(dts);
            dataGridView1.DataSource = dts;
        }

        //查询
        private void button1_Click(object sender, EventArgs e)
        {
         
            if (label4.Text == "往来单位")
            {
               
                s = "select a.id, a.code 编号 ,a.name 名称 ,b.name 分类 from AA_Partner  a  left join AA_PartnerClass b on a.idpartnerclass =b.id  where 1=1  ";
                if (textBox1.Text != "")
                {
                    s = s + " and  a.code='" + textBox1.Text + "'";
                }
                if (textBox2.Text != "")
                {
                    s = s + " and  a.name like '%" + textBox2.Text + "%'";
                }
                if (comboBox1.Text != "")
                {
                    s = s + " and  b.name = '" + comboBox1.Text + "'";
                }
                if (a == 1)
                {
                    if (carweighing.toolStripComboBox1.Text == "入库单")
                    {

                        s = s + " and a. partnerType in(226,228)";

                    }
                    else if (carweighing.toolStripComboBox1.Text == "出库单")
                    {
                        s = s + " and a. partnerType in(211,228)";
                    }
                }
                sqladpss = new SqlDataAdapter(s, sqlconns);
                dts = new DataTable();
                sqladpss.Fill(dts);
                dataGridView1.DataSource = dts;

            }
            else if (label4.Text == "存货")
            {
                s = "select a.id,a.code 编号 ,a.name 名称 ,b.name 分类 from AA_Inventory  a  left join AA_InventoryClass b on a.idInventoryclass =b.id   where 1=1  ";
                if (textBox1.Text != "")
                {
                    s = s + " and  a.code='" + textBox1.Text + "'";
                }
                if (textBox2.Text != "")
                {
                    s = s + " and  a.name like '%" + textBox2.Text + "%'";
                }
                if (comboBox1.Text != "")
                {
                    s = s + " and  b.name = '" + comboBox1.Text + "'";
                }

                sqladpss = new SqlDataAdapter(s, sqlconns);
                dts = new DataTable();
                sqladpss.Fill(dts);
                dataGridView1.DataSource = dts;
            }
            else if (label4.Text == "仓库")
            {
                s = "select  id, code 编号 ,name 名称,address  发货地址 from AA_Warehouse where 1=1 ";
                if (textBox1.Text != "")
                {
                    s = s + " and  code='" + textBox1.Text + "'";
                }
                if (textBox2.Text != "")
                {
                    s = s + " and name like '%" + textBox2.Text + "%'";
                }
                sqladpss = new SqlDataAdapter(s, sqlconns);
                dts = new DataTable();
                sqladpss.Fill(dts);
                dataGridView1.DataSource = dts;
            }
            else if (label4.Text == "批号")
            {
                s = " select  b.name 仓库,c.name 存货 ,a.batch 批号,a.CanuseBaseQuantity 可用量  from ST_CurrentStock a  left join AA_Warehouse  b on a.idwarehouse=b.id  join AA_Inventory c on a.idinventory =c.id where a.idwarehouse = " + carweighing.label19.Text + " and a.idinventory = " + carweighing.label20.Text + "";
                if (textBox1.Text != "")
                {
                    s = s + " and  a.batch='" + textBox1.Text + "'";
                }

                sqladpss = new SqlDataAdapter(s, sqlconns);
                dts = new DataTable();
                sqladpss.Fill(dts);
                dataGridView1.DataSource = dts;
            }
               else if (label4.Text == "委外出库")
                {
                    s = "select a.id id ,a.code 委外加工单编号, d.code 供应商编号 ,d.name 供应商名称 ,c.code 存货编号 ,c.name 存货名称,e.code 仓库编号,e.name 仓库名称,a.voucherdate 单据时间 from OM_OutSourceOrder a join  OM_OutSourceOrder_Material  b on a.id=b.voucherId    join  AA_Inventory c on b.idinventory=c.id  join AA_Partner d on a.idPartner=d.id  join AA_Warehouse e on b.idWarehouse=e.id where 1=1 and VoucherState =  N'189' ";
                if (textBox1.Text != "")
                {
                    s = s + " and  d.code='" + textBox1.Text + "'";
                }
                if (textBox2.Text != "")
                {
                    s = s + " and  c.code like '%" + textBox2.Text + "%'";
                }
                if (comboBox1.Text != "")
                    {
                        s = s + " and  a.code like '%" + comboBox1.Text + "%'";
                    }
                    if (dateTimePicker1.Value.ToString("yyyy-MM-dd") != "" && dateTimePicker2.Value.ToString("yyyy-MM-dd") != "")
                    {
                        s = s + "  and a.voucherdate between  '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'  ";
                    }

                sqladpss = new SqlDataAdapter(s, sqlconns);
                dts = new DataTable();
                sqladpss.Fill(dts);
                dataGridView1.DataSource = dts;

            }
            else if (label4.Text == "委外入库")
            {
                s = "select a.id id,a.code 委外加工单编号, d.code 供应商编号 ,d.name 供应商名称 ,c.code 存货编号 ,c.name 存货名称,e.code 仓库编号,e.name 仓库名称,a.voucherdate 单据时间 from OM_OutSourceOrder a join  OM_OutSourceOrder_b  b on a.id=b.idOutSourceOrderDTO    join  AA_Inventory c on b.idinventory=c.id  join AA_Partner d on a.idPartner=d.id  join AA_Warehouse e on b.idWarehouse=e.id  where 1 =1  and VoucherState =  N'189'  ";
                if (textBox1.Text != "")
                {
                    s = s + " and  d.code='" + textBox1.Text + "'";
                }
                if (textBox2.Text != "")
                {
                    s = s + " and  c.code like '%" + textBox2.Text + "%'";
                }
                if (comboBox1.Text != "")
                {
                    s = s + " and  a.code like '%" + comboBox1.Text + "%'";
                }
                if (dateTimePicker1.Value.ToString("yyyy-MM-dd") != "" && dateTimePicker2.Value.ToString("yyyy-MM-dd") != "")
                {
                    s = s + "  and a.voucherdate between  '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'  ";
                }
                sqladpss = new SqlDataAdapter(s, sqlconns);
                dts = new DataTable();
                sqladpss.Fill(dts);
                dataGridView1.DataSource = dts;
            }
            else if (label4.Text == "入库单")
            {
                s = "select  a.id 采购订单id,a.code 采购订单编号,c.name 供应商名称,d.name 存货名称 ,e.name 仓库名称,b.origDiscountPrice 单价,a.voucherdate 订单时间   from  PU_PurchaseOrder  a join  PU_PurchaseOrder_b b on a.id=b.idPurchaseOrderDTO  join AA_Partner c on a.idpartner=c.id join AA_Inventory d on b.idinventory=d.id join AA_Warehouse e on b.idwarehouse =e.id where 1 =1 and VoucherState =  N'189'   ";
                if (textBox1.Text != "")
                {
                    s = s + " and  d.name='" + textBox1.Text + "'";
                }
                if (textBox2.Text != "")
                {
                    s = s + " and  c.code like '%" + textBox2.Text + "%'";
                }
                if (comboBox1.Text != "")
                {
                    s = s + " and  a.code like '%" + comboBox1.Text + "%'";
                }
                if (dateTimePicker1.Value.ToString("yyyy-MM-dd") != "" && dateTimePicker2.Value.ToString("yyyy-MM-dd") != "")
                {
                    s = s + "  and a.voucherdate between  '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'  ";
                }
                sqladpss = new SqlDataAdapter(s, sqlconns);
                dts = new DataTable();
                sqladpss.Fill(dts);
                dataGridView1.DataSource = dts;
            }
            else if (label4.Text == "出库单")
            {
                s = "select  a.id 销售订单id,a.code 销售订单编号,c.name 客户名称,d.name 存货名称 ,e.name 仓库名称,b.origDiscountPrice 单价,a.voucherdate 订单时间    from  SA_SaleOrder  a join  SA_SaleOrder_b b on a.id=b.idSaleOrderDTO  join AA_Partner c on a.idcustomer=c.id join AA_Inventory d on b.idinventory=d.id join AA_Warehouse e on b.idwarehouse =e.id where 1 =1 and VoucherState =  N'189'  ";
                if (textBox1.Text != "")
                {
                    s = s + " and  d.name='" + textBox1.Text + "'";
                }
                if (textBox2.Text != "")
                {
                    s = s + " and  c.code like '%" + textBox2.Text + "%'";
                }
                if (comboBox1.Text != "")
                {
                    s = s + " and  a.code like '%" + comboBox1.Text + "%'";
                }
                if (dateTimePicker1.Value.ToString("yyyy-MM-dd") != "" && dateTimePicker2.Value.ToString("yyyy-MM-dd") != "")
                {
                    s = s + "  and a.voucherdate between  '" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dateTimePicker2.Value.ToString("yyyy-MM-dd") + "'  ";
                }
                sqladpss = new SqlDataAdapter(s, sqlconns);
                dts = new DataTable();
                sqladpss.Fill(dts);
                dataGridView1.DataSource = dts;
            }

        }

        //确认
        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
              if (dataGridView1.SelectedRows.Count>0) { 
                   DataRowView drv = dataGridView1.SelectedRows[0].DataBoundItem as DataRowView;
                if (a == 1) {
                       
                    if (label4.Text == "往来单位") {
                        carweighing.label18.Text = drv["id"].ToString();
                        carweighing.comboBox2.Text = drv["名称"].ToString();
                    }
                    else if (label4.Text == "存货") {
                        carweighing.label20.Text = drv["id"].ToString();
                        carweighing.comboBox5.Text = drv["名称"].ToString();
                    }
                    else if (label4.Text == "仓库")
                    {
                        carweighing.label19.Text = drv["id"].ToString();
                        carweighing.comboBox4.Text = drv["名称"].ToString();
                    }
                    else if (label4.Text == "批号")
                    {

                        carweighing.textBox4.Text = drv["批号"].ToString();
                    }
                    else if (label4.Text == "委外出库")
                    {

                            carweighing.comboBox1.Text = drv["委外加工单编号"].ToString();
                            carweighing.label25.Text = drv["id"].ToString();
                            carweighing.comboBox2.Text = drv["供应商名称"].ToString();
                            //     carweighing.label20.Text = drv["存货id"].ToString();
                            carweighing.comboBox5.Text = drv["存货名称"].ToString();
                            //      carweighing.label19.Text = drv["仓库id"].ToString();
                            carweighing.comboBox4.Text = drv["仓库名称"].ToString();


                        }
                        else if (label4.Text == "委外入库")
                        {
                            carweighing.comboBox1.Text = drv["委外加工单编号"].ToString();
                            carweighing.label25.Text = drv["id"].ToString();
                           // carweighing.label18.Text = drv["供应商id"].ToString();
                            carweighing.comboBox2.Text = drv["供应商名称"].ToString();
                       //     carweighing.label20.Text = drv["存货id"].ToString();
                            carweighing.comboBox5.Text = drv["存货名称"].ToString();
                      //      carweighing.label19.Text = drv["仓库id"].ToString();
                            carweighing.comboBox4.Text = drv["仓库名称"].ToString();
                        }
                        else if (label4.Text == "入库单")
                        {
                            carweighing.comboBox1.Text = drv["采购订单编号"].ToString();
                            carweighing.label25.Text = drv["采购订单id"].ToString();
                            // carweighing.label18.Text = drv["供应商id"].ToString();
                            carweighing.comboBox2.Text = drv["供应商名称"].ToString();
                            //     carweighing.label20.Text = drv["存货id"].ToString();
                            carweighing.comboBox5.Text = drv["存货名称"].ToString();
                            //      carweighing.label19.Text = drv["仓库id"].ToString();
                            carweighing.comboBox4.Text = drv["仓库名称"].ToString();
                           carweighing.numericUpDown3.Text=drv["单价"].ToString();
                        }
                        else if (label4.Text == "出库单")
                        {
                            carweighing.comboBox1.Text = drv["销售订单编号"].ToString();
                            carweighing.label25.Text = drv["销售订单id"].ToString();
                            // carweighing.label18.Text = drv["供应商id"].ToString();
                            carweighing.comboBox2.Text = drv["客户名称"].ToString();
                            //     carweighing.label20.Text = drv["存货id"].ToString();
                            carweighing.comboBox5.Text = drv["存货名称"].ToString();
                            //      carweighing.label19.Text = drv["仓库id"].ToString();
                            carweighing.comboBox4.Text = drv["仓库名称"].ToString();
                            carweighing.numericUpDown3.Text = drv["单价"].ToString();
                        }

                    } else if (a==2) {

                    if (label4.Text == "往来单位")
                    {
                        form6.label18.Text = drv["id"].ToString();
                        form6.comboBox5.Text = drv["名称"].ToString();
                    }
                    else if (label4.Text == "存货")
                    {
                        form6.label20.Text = drv["id"].ToString();
                        form6.comboBox7.Text = drv["名称"].ToString();
                    }
                    else if (label4.Text == "仓库")
                    {
                        form6.label19.Text = drv["id"].ToString();
                        form6.comboBox6.Text = drv["名称"].ToString();
                    }
                    else if (label4.Text == "批号")
                    {

                        form6.textBox4.Text = drv["批号"].ToString();
                    }
                   else  if (label4.Text == "委外订单号")
                    {
                            form6.comboBox8.Text = drv["编号"].ToString();
                            form6.label12.Text = drv["id"].ToString();
                    }

                    }

              }
                this.Close();
            }


        }
    }
}
