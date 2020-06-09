using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace zySoft
{
    public partial class password : Form
    {
        public password()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text =="admin")
            {
                CommonClass.pass=true;  
                this.Close(); 
            }
            else
            {
                MessageBox.Show("录入密码错误!","提示：");
                CommonClass.pass=false;  
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            CommonClass.pass = false; 
        }
    }
}
