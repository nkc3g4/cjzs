﻿using System;
using System.Windows.Forms;
using Microsoft.Win32;
namespace 攒机助手
{
    public partial class update : Form
    {
        string args = null;
        public update(string args)
        {
            InitializeComponent();
            this.args = args;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) { button1.Enabled = false; }
            if (checkBox1.Checked == false) { button1.Enabled = true; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                SettingFile sf = new SettingFile();
                sf.SetValue("CheckUpdateOnStartup", "0");
                //WTRegedit("nevercheckupdate", "1");
            }
            this.Close();
        }
      

        private void update_Load(object sender, EventArgs e)
        {
            label1.Text += args;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://bbs.luobotou.org/thread-59-1-1.html");
            this.Close();
        }
    }
}
