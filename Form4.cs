using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace zySoft
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string softName = textBox1.Text.ToString();
            string strKeyName = string.Empty;
            string softPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\";
            RegistryKey regKey = Registry.LocalMachine;
            RegistryKey regSubKey = regKey.OpenSubKey(softPath + softName + ".exe", false);

            object objResult = regSubKey.GetValue(strKeyName);
            RegistryValueKind regValueKind = regSubKey.GetValueKind(strKeyName);
            if (regValueKind == Microsoft.Win32.RegistryValueKind.String)
            {
                this.textBox1.Text = objResult.ToString();
            }  
        }
    }
}
