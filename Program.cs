using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace zySoft
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new login());
        }
    }
    public class Utility
    {
        public static string databaseConnection = "";
        public static string reportConnection = "";
        public static string kdconnection = "";
        public static string voucherid = "";
        public static string billno = "";
        public static string GetReportPath()
        {
            string FileName = Application.StartupPath.ToLower();
            return FileName ;
        }
               
        //获取数据连接串
        public static string DatabaseConnection
        {
            get {return databaseConnection; }
            set { databaseConnection = value; }

        }
       public static string ReportConnection
        {
            get { return reportConnection; }
            set { reportConnection = value; }

        }
       public static string kdConnection
       {
           get { return kdconnection; }
           set { kdconnection = value; }

       }
       public static string VoucherId
       {
           get { return voucherid; }
           set { voucherid = value; }

       }
       public static string BillNo
       {
           get { return billno; }
           set { billno = value; }

       }
     
    }
}
