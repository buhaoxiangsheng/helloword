using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace zySoft
{
    class CommonClass
    {
        public static string connstring { get; set; }
        public static string ztconnstring { get; set; }
        public static string accountDB { get; set; }
        public static string ztcode { get; set; } //帐套号
        public static string iyear { get; set; } //年度
        public static string imonth { get; set; } //年度
        public static string maker { get; set; }
        public static bool badd { get; set; }
        public static bool bedit { get; set; }
        public static string ccode { get; set; } //查询单据号
        public static string filePath { get; set; }
        public static string reportconnstring { get; set; }
        public static string checkid { get; set; } //合格
        public static string stockid { get; set; } //仓库
        public static string stockname { get; set; } //入库数量
        public static string userCode { get; set; } //
        public static string userName { get; set; } //
        public static string teamName { get; set; } //
        public static string saleAreaid { get; set; } //仓库
        public static string saleAreaName { get; set; } //入库数量
        public static bool pass { get; set; }//通过验证
        public static string usertoken { get; set; }//获取token
    }
    public class Ini
    {
        // 声明INI文件的写操作函数 WritePrivateProfileString()

        [System.Runtime.InteropServices.DllImport("kernel32")]

        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        // 声明INI文件的读操作函数 GetPrivateProfileString()

        [System.Runtime.InteropServices.DllImport("kernel32")]

        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);


        private string sPath = null;
        public Ini(string path)
        {
            this.sPath = path;
        }

        public void Writue(string section, string key, string value)
        {

            // section=配置节，key=键名，value=键值，path=路径

            WritePrivateProfileString(section, key, value, sPath);

        }

        public string ReadValue(string section, string key)
        {

            // 每次从ini中读取多少字节

            System.Text.StringBuilder temp = new System.Text.StringBuilder(255);

            // section=配置节，key=键名，temp=上面，path=路径

            GetPrivateProfileString(section, key, "", temp, 255, sPath);

            return temp.ToString();

        }

        public string ExportExcel(DataSet ds, string saveFileName)
        {
            try
            {
                if (ds == null) return "数据库为空";
                bool fileSaved = false;
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)
                { return "无法创建Excel对象，可能您的机子未安装Excel"; }
                xlApp.Visible = false;
                xlApp.DisplayAlerts = false;
                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
                //取得sheet1 //写入字段
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                { worksheet.Cells[1, i + 1] = ds.Tables[0].Columns[i].ColumnName; }
                //写入数值
                for (int r = 0; r < ds.Tables[0].Rows.Count; r++)
                {
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    { worksheet.Cells[r + 2, i + 1] = ds.Tables[0].Rows[r][i]; }
                    System.Windows.Forms.Application.DoEvents();
                }
                //worksheet.Columns.EntireColumn.AutoFit();
                //列宽自适应。
                if (saveFileName != "")
                {
                    try
                    {
                        workbook.Saved = true;
                        worksheet.Name = "sheet";
                        workbook.SaveCopyAs(saveFileName);
                        fileSaved = true;
                    }
                    catch (Exception ex)
                    {
                        fileSaved = false;
                        //MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + ex.Message);
                    }
                }
                else
                {
                    fileSaved = false;
                }
                xlApp.Workbooks.Close(); 
                xlApp.Quit();
                worksheet = null;
                workbooks = null;
                xlApp=null;
                GC.Collect();
                //强行销毁
                //if (fileSaved && System.IO.File.Exists(saveFileName))
                //    System.Diagnostics.Process.Start(saveFileName);
                ////打开EXCEL 
                return "true";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
