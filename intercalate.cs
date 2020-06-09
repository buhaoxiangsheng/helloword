using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace zySoft
{
    public partial class intercalate : Form
    {
        public intercalate()
        {
            InitializeComponent();
        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            //保存相关的参数
          string  Current = Directory.GetCurrentDirectory();//获取当前根目录
            
             // 写入ini
            Ini ini = new Ini(Current + "/config.ini");
            ini.Writue("Setting", "comset", comboBox2.Text.Trim());
            ini.Writue("Setting", "baud", comboBox3.Text.Trim());
            ini.Writue("Setting", "oddeven", comboBox4.Text.Trim());
            ini.Writue("Setting", "cease", comboBox5.Text.Trim());
            ini.Writue("Setting", "data", comboBox6.Text.Trim());

            MessageBox.Show("保存成功");
        }

        private void intercalate_Load(object sender, EventArgs e)
        {
            string Current = Directory.GetCurrentDirectory();//获取当前根目录
            Ini ini = new Ini(Current + "/config.ini");

            if (File.Exists(Current + "/config.ini"))
            {
                comboBox2.Text = ini.ReadValue("Setting", "comset");
                comboBox3.Text = ini.ReadValue("Setting", "baud");
                comboBox4.Text = ini.ReadValue("Setting", "oddeven");
                comboBox5.Text = ini.ReadValue("Setting", "cease");
                comboBox6.Text = ini.ReadValue("Setting", "data");


            }
            else
            {
                MessageBox.Show("请先进行连接配置！", "登陆提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }
        }
    }
}
