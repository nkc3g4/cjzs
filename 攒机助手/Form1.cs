
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Management;
using System.Net;
//using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
namespace 攒机助手
{

    public partial class Form1 : Form
    {
     
        PrintDocument pdDocument = new PrintDocument();
        string clipdata = "";
       string[] cpu=new string[50];int[] cpu1 = new int[50];
       string[] acpu = new string[50];int[] acpu1 = new int[50];
       string[] mb = new string[50];int[] mb1 = new int[50];
        string [] gpu=new string [50];int[] gpu1 = new int[50];
        string[] ram = new string[50];int[] ram1 = new int[50];
        string[] power = new string[50]; int[] power1 = new int[50];
        string[] lcd = new string[50]; int[] lcd1 = new int[50];
        string[] box = new string[50]; int[] box1 = new int[50];
        string[] hdd = new string[50]; int[] hdd1 = new int[50];
        string[] ssd = new string[50]; int[] ssd1 = new int[50];
        string[] fan = new string[50]; int[] fan1 = new int[50];
        string[] cdrom = new string[50]; int[] cdrom1 = new int[50];
        string[] kb = new string[50]; int[] kb1 = new int[50];
        string cpusr;
        string mbsr;
        string ramsr;
        string hddsr;
        string gpusr;
        string ssdsr;
        string lcdsr;
        string boxsr;
        string fansr;
        string powersr;
        string cdromsr;
        string kbsr;
        string pricedate="";
        long sum;
        long difference;
        int z=0;
        bool needupdate=false ;
        string cpusr2="";
        Thread threadupdate;
        Thread threadreport;
        Thread threadad;
        Thread priceupdate;
        String adlink;
        String releaseurl = "http://bbs.luobotou.org/app/cjzs.txt";
        delegate void set_Text(string s); //定义委托
        set_Text Set_Text;
        //cpu[1]="";
        public Form1()
        {
            InitializeComponent();
         
        }
        private void set_textboxText(string s)
        {
            labelad.Text = s;
            labelad.Visible = true;
           // linkLabel1.Text = s;
            //label4.Visible = true;
           // linkLabel1.Visible = true;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
            this.Width = this.Width - (int)(((double)this.Width) * 0.4444);
            tableLayoutPanel1.ColumnStyles[2].Width = 0;
            pdDocument.PrintPage += new PrintPageEventHandler(OnPrintPage);
            Set_Text = new set_Text(set_textboxText);
            if (!System.IO.File.Exists(Application.StartupPath + "\\config.ini")) { MessageBox.Show("配置文件错误！config.ini不存在！"); Application.Exit(); }
           // this.Width = 440;
            this.Text = Application.ProductName + " " + Application.ProductVersion;
            try
            {        ////
                StreamReader objReader = new StreamReader(Application.StartupPath + "\\config.ini");
                string sLine = objReader.ReadLine();
                if (sLine.Contains("#"))
                {
                    pricedate = sLine.Substring(1);
                    labeldate.Text = "价格更新日期：" + pricedate;
                    needupdate = true;
                    sLine = objReader.ReadLine();
                }
                int cpuloop = 0;
                int acpul = 0;
                int mbloop = 0;
                int ramloop = 0;
                int hddloop = 0;
                int gpuloop = 1;
                int boxloop = 0;
                int powloop = 1;
                int lcdloop = 0;
                int fanloop = 1;
                int cdromloop = 0;
                int kbloop = 0;
                int ssdloop = 0;
                gpu[1] = "核心显卡";
                power[1] = "机箱自带";
                fan[1] = "盒装自带";
                ComboBoxcpu.Items.Add("自动选择");
                ComboBoxmb.Items.Add("自动选择");
                ComboBoxram.Items.Add("自动选择");
                ComboBoxhdd.Items.Add("自动选择");
                ComboBoxgpu.Items.Add("自动选择");
                ComboBoxgpu.Items.Add(gpu[1]);
                ComboBoxlcd.Items.Add("自动选择");
                ComboBoxbox.Items.Add("自动选择");
                ComboBoxfan.Items.Add("自动选择");
                ComboBoxfan.Items.Add(fan[1]);
                comboBoxssd.Items.Add("自动选择");
                ComboBoxpower.Items.Add("自动选择");
                ComboBoxpower.Items.Add(power[1]);
                ComboBoxcdrom.Items.Add("自动选择");
                comboBoxkb.Items.Add("自动选择");

                ComboBoxcpu.SelectedIndex = 0;
                ComboBoxmb.SelectedIndex = 0;
                ComboBoxram.SelectedIndex = 0;
                ComboBoxhdd.SelectedIndex = 0;
                ComboBoxgpu.SelectedIndex = 0;
                ComboBoxlcd.SelectedIndex = 0;
                ComboBoxbox.SelectedIndex = 0;
                ComboBoxfan.SelectedIndex = 0;
                ComboBoxpower.SelectedIndex = 0;
                ComboBoxcdrom.SelectedIndex = 0;
                comboBoxkb.SelectedIndex = 0;
                comboBoxssd.SelectedIndex = 0;

                //ArrayList LineList = new ArrayList();
                while (sLine != null)
                {

                    //try
                    //{
                    //if (sLine.Substring(0, 1) == "'") { continue; }
                    if (sLine.Contains("[CPU]"))
                    {
                        sLine = objReader.ReadLine();
                        while (!sLine.Contains("["))
                        {
                            cpu[++cpuloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                            cpu1[cpuloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                            ComboBoxcpu.Items.Add(cpu[cpuloop] + CreateSpace(27 - System.Text.Encoding.Default .GetBytes(cpu[cpuloop]).Length ) + "￥" + cpu1[cpuloop]);
                            sLine = objReader.ReadLine();
                        }
                        // cpu[++cpuloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                        //cpu1[cpuloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",")+1));
                        //MessageBox.Show(cpu[cpuloop]);
                    }
                    else if (sLine.Contains("[ACPU]"))
                    {
                        sLine = objReader.ReadLine();
                        while (!sLine.Contains("["))
                        {
                            acpu[++acpul] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                            acpu1[acpul] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                            sLine = objReader.ReadLine();
                        }

                    }
                    else if (sLine.Contains("[MB]"))
                    {
                        sLine = objReader.ReadLine();
                        while (!sLine.Contains("["))
                        {
                            mb[++mbloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                            mb1[mbloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                            ComboBoxmb.Items.Add(mb[mbloop] + CreateSpace(27 - System.Text.Encoding.Default.GetBytes(mb[mbloop]).Length) + "￥" + mb1[mbloop]);

                            //ComboBoxmb.Items.Add(mb[mbloop] + "  ￥" + mb1[mbloop]);
                            sLine = objReader.ReadLine();
                        }

                    }
                    else if (sLine.Contains("[RAM]"))
                    {
                        sLine = objReader.ReadLine();
                        while (!sLine.Contains("["))
                        {
                            ram[++ramloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                            ram1[ramloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                            ComboBoxram.Items.Add(ram[ramloop] + CreateSpace(30 - System.Text.Encoding.Default.GetBytes(ram[ramloop]).Length) + "￥" + ram1[ramloop]);

                            //ComboBoxram.Items.Add(ram[ramloop] + "  ￥" + ram1[ramloop]);
                            sLine = objReader.ReadLine();
                        }

                    }
                    else if (sLine.Contains("HDD"))
                    {
                        sLine = objReader.ReadLine();
                        while (!sLine.Contains("["))
                        {
                            hdd[++hddloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                            hdd1[hddloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                            ComboBoxhdd.Items.Add(hdd[hddloop] + CreateSpace(30 - System.Text.Encoding.Default.GetBytes(hdd[hddloop]).Length) + "￥" + hdd1[hddloop]);

                            //ComboBoxhdd.Items.Add(hdd[hddloop] + "  ￥" + hdd1[hddloop]);
                            sLine = objReader.ReadLine();
                        }

                    }
                    else if (sLine.Contains("[GPU]"))
                    {
                        sLine = objReader.ReadLine();
                        while (!sLine.Contains("["))
                        {
                            gpu[++gpuloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                            gpu1[gpuloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                            ComboBoxgpu.Items.Add(Regex.Replace(gpu[gpuloop], @"\([^\(]*\)", "") + CreateSpace(27 - System.Text.Encoding.Default.GetBytes(Regex.Replace(gpu[gpuloop], @"\([^\(]*\)", "")).Length) + "￥" + gpu1[gpuloop]);
                            sLine = objReader.ReadLine();
                        }

                        //MessageBox.Show(sLine);
                        //MessageBox.Show(gpuloop.ToString());
                    }
                    else if (sLine.Contains("[BOX]"))
                    {
                        sLine = objReader.ReadLine();
                        while (!sLine.Contains("["))
                        {
                            box[++boxloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                            box1[boxloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                            ComboBoxbox.Items.Add(box[boxloop] + CreateSpace(27 - System.Text.Encoding.Default.GetBytes(box[boxloop]).Length) + "￥" + box1[boxloop]);
                            sLine = objReader.ReadLine();
                        }

                    }
                    else if (sLine.Contains("[POW]"))
                    {
                        sLine = objReader.ReadLine();
                        while (!sLine.Contains("["))
                        {
                            power[++powloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                            power1[powloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                            ComboBoxpower.Items.Add(power[powloop] + CreateSpace(27 - System.Text.Encoding.Default.GetBytes(power[powloop]).Length) + "￥" + power1[powloop]);
                            sLine = objReader.ReadLine();
                        }

                    }
                    else if (sLine.Contains("[LCD]"))
                    {
                        sLine = objReader.ReadLine();
                        while (!sLine.Contains("["))
                        {
                            lcd[++lcdloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                            lcd1[lcdloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                            ComboBoxlcd.Items.Add(lcd[lcdloop] + CreateSpace(27 - System.Text.Encoding.Default.GetBytes(lcd[lcdloop]).Length) + "￥" + lcd1[lcdloop]);
                            sLine = objReader.ReadLine();
                        }

                    }
                    else if (sLine.Contains("[FAN]"))
                    {
                        sLine = objReader.ReadLine();
                        while (!sLine.Contains("["))
                        {
                            fan[++fanloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                            fan1[fanloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                            ComboBoxfan.Items.Add(fan[fanloop] + CreateSpace(27 - System.Text.Encoding.Default.GetBytes(fan[fanloop]).Length) + "￥" + fan1[fanloop]);
                            sLine = objReader.ReadLine();
                        }

                    }
                    else if (sLine.Contains("[CDROM]"))
                    {
                        sLine = objReader.ReadLine();
                        while (!sLine.Contains("["))
                        {
                            cdrom[++cdromloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                            cdrom1[cdromloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                            ComboBoxcdrom.Items.Add(cdrom[cdromloop] + CreateSpace(27 - System.Text.Encoding.Default.GetBytes(cdrom[cdromloop]).Length) + "￥" + cdrom1[cdromloop]);
                            sLine = objReader.ReadLine();
                        }

                    }
                    else if (sLine.Contains("[KB]"))
                    {
                        sLine = objReader.ReadLine();
                        while (!sLine.Contains("["))
                        {
                            kb[++kbloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                            kb1[kbloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                            comboBoxkb.Items.Add(kb[kbloop] + CreateSpace(27 - System.Text.Encoding.Default.GetBytes(kb[kbloop]).Length) + "￥" + kb1[kbloop]);
                            sLine = objReader.ReadLine();
                        }

                    }
                    else if (sLine.Contains("[SSD]"))
                    {
                        sLine = objReader.ReadLine();
                        while (!sLine.Contains("["))
                        {
                            ssd[++ssdloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                            ssd1[ssdloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                            comboBoxssd.Items.Add(ssd[ssdloop] + CreateSpace(27 - System.Text.Encoding.Default.GetBytes(ssd[ssdloop]).Length) + "￥" + ssd1[ssdloop]);
                            sLine = objReader.ReadLine();
                        }

                    }
                    else { break; }
                    //}
                    //catch (Exception ex) { MessageBox.Show(ex.ToString()); }
                    //sLine = objReader.ReadLine();
                    //MessageBox.Show(sLine);
                    // if (sLine != null && !sLine.Equals(""))
                    //   LineList.Add(sLine);
                }
                objReader.Close();
                //if (needupdate)
                //{
                //    priceupdate = new Thread(configdownload);
                //    priceupdate.Start();
                //}
                //////
                //cpu[0] = "";


            }
            catch(Exception err)
            {
                MessageBox.Show("配置文件存在错误!\n" + err.ToString());
            }

        threadreport = new Thread(report);
        threadreport.Start();
        threadad = new Thread(ad);
        threadad.Start();
        if (IsRegeditExit(Application.ProductName) == true) { if ((GetRegistData("nevercheckupdate")) == "1") { 启动时自动检查更新ToolStripMenuItem.Checked = false; return; } }
        if (Application.ProductVersion.Substring(6) != "0") { return; }
        threadupdate = new Thread(update);
        threadupdate.Start();

        //MessageBox.Show(gpu.Length.ToString());

        //ComboBox1.SelectedIndex = 0;
        
          //ComboBox1.SelectedIndex = 0;
        }
        private void 启动时自动检查更新ToolStripMenuItem_Checked(object sender, EventArgs e)
        {

            if (启动时自动检查更新ToolStripMenuItem.Checked)
            {
                //启动时自动检查更新ToolStripMenuItem.Checked = false;
                WTRegedit("nevercheckupdate", "0");
            }

            if (!启动时自动检查更新ToolStripMenuItem.Checked)
            {
                //启动时自动检查更新ToolStripMenuItem.Checked = true;
                WTRegedit("nevercheckupdate", "1");
            }
        }
        private void buttonexpand_Click(object sender, EventArgs e)
        {
            if (buttonexpand.Text.Contains("<"))
            {
                this.Width = this.Width - (int)(((double)this.Width)*0.4444);
                tableLayoutPanel1.ColumnStyles[2].Width = 0;
                buttonexpand.Text = ">";
            }
            else 
            {
                this.Width = (int)(((double)this.Width) / 0.555556);
                tableLayoutPanel1.ColumnStyles[2].Width = 44.444444f;
                buttonexpand.Text = "<";
                Point a = new Point(groupBox3.Size.Width - 43, 0);
                label31.Location = a;

            }

            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (TextBox1.Text == "") { MessageBox.Show("请输入预算金额！"); return; }
            TextBoxcpu.Text = "";
            TextBoxcpu1.Text = "";

            TextBoxgpu.Text = "";
            TextBoxgpu1.Text = "";
            TextBoxmb.Text = "";
            TextBoxmb1.Text = "";
            TextBoxram.Text = "";
            TextBoxram1.Text = "";
            TextBoxhdd.Text = "";
            TextBoxhdd1.Text = "";
            TextBoxlcd.Text = "";
            TextBoxlcd1.Text = "";
            TextBoxfan.Text = "";
            TextBoxfan1.Text = "";
            TextBoxpower.Text = "";
            TextBoxpower1.Text = "";
            TextBoxbox.Text = "";
            TextBoxbox1.Text = "";
            textBoxssd.Text = "";
            textBoxssd1.Text = "";
            textBoxcdrom.Text = "";
            textBoxcdrom1.Text = "";
            textBoxkb.Text = "";
            textBoxkb1.Text = "";

            ////////////////////////
            cpu[0] = "";
            cpu1[0] = 0;
            gpu[0] = "";
            gpu1[0] = 0;
            mb[0] = "";
            mb1[0] = 0;
            ram[0] = "";
            ram1[0] = 0;
            box[0] = "";
            box1[0] = 0;
            power[0] = "";
            power1[0] = 0;
            lcd[0] = "";
            lcd1[0] = 0;
            fan[0] = "";
            fan1[0] = 0;
            hdd[0] = "";
            hdd1[0] = 0;
            ssd[0] = "";
            ssd1[0] = 0;
            cdrom[0] = "";
            cdrom1[0] = 0;
            kb[0] = "";
            kb1[0] = 0;
            //////////////////////
            cpusr = cpusrbox.Text;
            mbsr = mbsrbox.Text;
            ramsr = ramsrbox.Text;
            hddsr = hddsrbox.Text;
            gpusr = gpusrbox.Text;
            ssdsr = ssdsrBox.Text;
            lcdsr = lcdsrBox.Text;
            boxsr = boxsrbox.Text;
            fansr = fansrBox.Text;
            powersr = powersrBox.Text;
            cdromsr = cdromsrbox.Text;
            kbsr = kbsrbox.Text;

            ///////////////////////
            long choice;
            int mbchoice = 0;
            long money;
            long realmoney = 0;
            difference = 0;
            choice = 0;
            realmoney = long.Parse(TextBox1.Text);
            if (!CheckBoxlcd.Checked) 
            {
                if (realmoney < 4000) { money = realmoney + 800; }
                else if (realmoney >= 4000 && realmoney < 8000) { money = realmoney + 800; }
                else { money = realmoney + 1000; }
            }
            else { money = realmoney; }
            if (!checkBoxhdd.Checked) { money = money + 300; }
            if (checkBoxssd.Checked) { money = money - (int)(((float)money) * 0.1); }
            ////////////////////////
            if (money > 500000) { MessageBox.Show("您输入的金额过大，请重新输入！"); return; }
            ////////////////////////////////////////////////////////
            //CPU
            if (!CheckBoxamd.Checked)
            {
                if (ComboBoxcpu.SelectedIndex != 0)
                {
                    choice = ComboBoxcpu.SelectedIndex;

                }
                else
                {
                    if (money <= 2500)
                    {
                        if (checkBoxcpu.Checked)
                        {
                            choice = cauto(cpu, cpu1, money / 5, cpusr2, cpusr,"*");
                            // choice = closest(cpu1, money / 5);
                        }
                        else
                        {
                            choice = cauto(cpu, cpu1, money / 6, cpusr2, cpusr, "*");
                            //choice = closest(cpu1, money / 6); 
                        }
                    }
                    else if (money > 2500 && money <= 5500)
                    {
                        if (checkBoxcpu.Checked || cpusrbox.Text == "K")
                        {
                            choice = cauto(cpu, cpu1, money / 4, cpusr2, cpusr);
                            //  choice = closest(cpu1, money / 4); 
                        }
                        else
                        {
                            choice = cauto(cpu, cpu1, money / 5, cpusr2, cpusr, "K");
                            //   choice = closest(cpu1, money / 5); 
                        }
                    }
                    //else if 
                    else if (money > 5500 && money <= 9000) 
                    {
                        if (checkBoxcpu.Checked || cpusrbox.Text == "K")
                        {
                            choice = cauto(cpu, cpu1, money / 3, cpusr2, cpusr);
                            //  choice = closest(cpu1, money / 4); 
                        }
                        else
                        {
                            choice = cauto(cpu, cpu1, money / 4, cpusr2, cpusr, "K");
                            //   choice = closest(cpu1, money / 5); 
                        }

                    }
                    else 
                    {
                        if (checkBoxcpu.Checked) { choice = cauto(cpu, cpu1, money / 3, "", cpusr, "*"); }
                        else { choice = cauto(cpu, cpu1, (int)(((float)money) / 3.5), "", cpusr, "*"); }
                        //if (checkBoxcpu.Checked || cpusrbox.Text == "K")
                        //{//(int)(((float)money) / 3.5)
                            
                            //  choice = closest(cpu1, money / 4); 
                        //}
                        //else
                        //{
                        //    choice = cauto(cpu, cpu1, money / 4, "", cpusr ,"K");
                        //    //   choice = closest(cpu1, money / 5); 
                        //}
                    }



                }
                if (choice == 0 || choice == -1) { cpu[0] = "没有找到符合要求的配件。"; cpu1[0] = 0; }
                else
                {
                    cpu[0] = cpu[choice];
                    cpu1[0] = cpu1[choice];
                }
            }
            else
            {
                string notcontainpart = "*";
                if (checkBoxhtpc.Checked) { notcontainpart = "FX"; }
                if (ComboBoxcpu.SelectedIndex != 0)
                {
                    choice = ComboBoxcpu.SelectedIndex;

                }
                else
                {
                    if (checkBoxcpu.Checked)
                    {
                        choice = cauto(acpu, acpu1, money / 5, cpusr2, cpusr, notcontainpart);
                        //      choice = closest(acpu1, money / 5);
                    }
                    else
                    {
                        choice = cauto(acpu, acpu1, money / 6, cpusr2, cpusr, notcontainpart);
                        //   choice = closest(acpu1, money / 6);
                    }



                }
                if (choice == 0 || choice == -1) { cpu[0] = "没有找到符合要求的配件。"; cpu1[0] = 0; }
                else
                {
                    cpu[0] = acpu[choice];
                    cpu1[0] = acpu1[choice];
                }
            }


            //////////////////////////////////////////////////////////
            //主板
            int ivb = 0;
            // ivb = cpu[choice].IndexOf("7");
            if (!CheckBoxamd.Checked)
            {
                if (ComboBoxmb.SelectedIndex != 0)
                {
                    mbchoice = ComboBoxmb.SelectedIndex;
                }
                else
                {

                    if (cpu[choice].Contains("赛扬") || cpu[choice].Contains("奔腾"))
                    {
                        if (cpu[choice].Contains("1820") || cpu[choice].Contains("1830") || cpu[choice].Contains("1810"))
                        {
                            mbchoice = cauto(mb, mb1, 300, "H81", mbsr);
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 300, "B85", mbsr); }

                        }
                        else if (cpu[choice].Contains("1610") || cpu[choice].Contains("1620") || cpu[choice].Contains("1630"))
                        {
                            mbchoice = cauto(mb, mb1, 300, "H61", mbsr);
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 300, "B75", mbsr); }
                        }
                        //////下面奔腾
                        else if (cpu[choice].Contains("3220") || cpu[choice].Contains("3230") || cpu[choice].Contains("3240") || cpu[choice].Contains("3420"))
                        {
                            mbchoice = cauto(mb, mb1, 300, "H81", mbsr);
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 350, "B85", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 400, "H87", mbsr); }

                        }
                        else if (cpu[choice].Contains("2010") || cpu[choice].Contains("2020") || cpu[choice].Contains("2100") || cpu[choice].Contains("2120") || cpu[choice].Contains("2130"))
                        {
                            mbchoice = cauto(mb, mb1, 350, "H61", mbsr);
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 300, "B75", mbsr); }
                        }
                        else if (cpu[choice].Contains("3258")) 
                        {
                            mbchoice = cauto(mb, mb1, 600, "Z97", mbsr);

                        }
                        else { mbchoice = cauto(mb, mb1, 300, "H61", mbsr); }

                        // mbchoice = 1;
                    }
                    else if (cpu[choice].Contains("i3"))
                    {
                        ivb = cpu[choice].IndexOf("3");
                        if (cpu[choice].Substring(ivb + 2, 2) == "32")
                        {
                            mbchoice = cauto(mb, mb1, 500, "B75", mbsr);
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 500, "H77", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 500, "H61", mbsr); }
                        }
                        else if (cpu[choice].Substring(ivb + 2, 2) == "41" || cpu[choice].Substring(ivb + 2, 2) == "43")
                        {
                            if (checkBoxcpu.Checked||checkBoxgpu.Checked) { mbchoice = cauto(mb, mb1, 450, "H81", mbsr); }
                            else { mbchoice = cauto(mb, mb1, 500, "B85", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 500, "H81", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 500, "H87", mbsr); }
                        }
                        else { mbchoice = cauto(mb, mb1, 400, "H61", mbsr); }
                        //mbchoice = 2;
                    }
                    else if (cpu[choice].Contains("i5") && !cpu[choice].Contains("K") && !cpu[choice].Contains("X"))  //I5 !K !X
                    {
                        ivb = cpu[choice].IndexOf("5");

                        if (cpu[choice].Substring(ivb + 2, 2) == "34" || cpu[choice].Substring(ivb + 2, 2) == "35" || cpu[choice].Substring(ivb + 2, 2) == "33")
                        {
                            if (checkBoxcpu.Checked || checkBoxgpu.Checked) { mbchoice = cauto(mb, mb1, 500, "B75", mbsr); }
                            else { mbchoice = cauto(mb, mb1, 600, "B75", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 600, "H77", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 600, "Z77", mbsr); }
                        }
                        else if (cpu[choice].Substring(ivb + 2, 2) == "44" || cpu[choice].Substring(ivb + 2, 2) == "45" || cpu[choice].Substring(ivb + 2, 2) == "46")
                        {
                            if (checkBoxcpu.Checked || checkBoxgpu.Checked) { mbchoice = cauto(mb, mb1, 500, "B85", mbsr); }
                            else { mbchoice = cauto(mb, mb1, 600, "B85", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 600, "H87", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 600, "H81", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 600, "Z87", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 800, "Z97", mbsr); }

                        }
                        else //2 gen i5
                        {
                            if (checkBoxcpu.Checked) { mbchoice = cauto(mb, mb1, 500, "H61", mbsr); }
                            else { mbchoice = cauto(mb, mb1, 700, "H67", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 500, "H61", mbsr); }
                        }
                        //mbchoice = 3;
                    }
                    else if (cpu[choice].Contains("i7") && !cpu[choice].Contains("K") && !cpu[choice].Contains("X"))
                    {
                        //i7 !X !K
                        // int ivb = 0;
                        ivb = cpu[choice].IndexOf("7");
                        if (cpu[choice].Substring(ivb + 2, 2) == "38") { mbchoice = cauto(mb, mb1, 1500, "X79", mbsr); }
                        else if (cpu[choice].Substring(ivb + 2, 2) == "37") //3 gen i7
                        {
                            mbchoice = cauto(mb, mb1, 800, "B75", mbsr);
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 800, "H77", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 600, "Z77", mbsr); }
                        }
                        else if (cpu[choice].Substring(ivb + 2, 2) == "47") //4 gen i7
                        {
                            if (checkBoxcpu.Checked || checkBoxgpu.Checked) { mbchoice = cauto(mb, mb1, 650, "B85", mbsr); }

                            else { mbchoice = cauto(mb, mb1, 750, "B85", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 800, "H87", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 600, "H81", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 800, "Z87", mbsr); }
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 800, "Z97", mbsr); }

                        }
                            //2 gen i7
                        else if (checkBoxcpu.Checked) { mbchoice = cauto(mb, mb1, 700, "H67", mbsr); }
                        else { mbchoice = cauto(mb, mb1, 1000, "H67", mbsr); }
                    }
                    else if (cpu[choice].Contains("i7") && cpu[choice].Contains("K"))//i7 K
                    {
                        //  int ivb = 0;
                        ivb = cpu[choice].IndexOf("7");
                        if (cpu[choice].Substring(ivb + 2, 2) == "48" || cpu[choice].Substring(ivb + 2, 2) == "49" || cpu[choice].Substring(ivb + 2, 2) == "39") 
                        {
                            mbchoice = cauto(mb, mb1, 1500, "X79", mbsr); 
                        }
                        else if (cpu[choice].Substring(ivb + 2, 2) == "37") // 3 gen i7 k
                        {
                            mbchoice = cauto(mb, mb1, 1000, "Z77", mbsr); 
                        }
                        else if (cpu[choice].Substring(ivb + 2, 2) == "47")  //4 gen i7 k
                        {
                            if (money < 10000)
                            {
                                if (cpu[choice].Contains("4770K"))
                                {
                                    mbchoice = cauto(mb, mb1, 1000, "Z87", mbsr);
                                    if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 1000, "Z97", mbsr); }
                                }
                                else 
                                {
                                    mbchoice = cauto(mb, mb1, 1000, "Z97", mbsr);
                                }
                            }
                            else
                            {
                                 mbchoice = cauto(mb, mb1, 1500, "Z97", mbsr); 

                            }
                        }
                        else if (checkBoxcpu.Checked) //2 gen i7 k
                        {
                            mbchoice = cauto(mb, mb1, 1000, "Z68", mbsr); 
                        }
                        else 
                        {
                            mbchoice = cauto(mb, mb1, 1500, "Z68", mbsr); 
                        }
                    }
                    //else if (choice == 6) { mbchoice = 7; }
                    else if (cpu[choice].Contains("980X") || cpu[choice].Contains("990X") || cpu[choice].Contains("995X")) 
                    {
                        mbchoice = cauto(mb, mb1, 4000, "X58", mbsr); 
                    }
                    else if (cpu[choice].Contains("3960X") || cpu[choice].Contains("3970X") || cpu[choice].Contains("4960X") || cpu[choice].Contains("4970X")) 
                    {
                        mbchoice = cauto(mb, mb1, 3600, "X79", mbsr); 
                    }
                    else if (cpu[choice].Contains("i5") && cpu[choice].Contains("K")) //I5 K
                    {
                        //  int ivb = 0;
                        ivb = cpu[choice].IndexOf("5");
                        if (cpu[choice].Substring(ivb + 2, 2) == "35") // 3 gen i5 K
                        {
                            mbchoice = cauto(mb, mb1, 800, "Z77", mbsr); 
                        }
                        else if (cpu[choice].Contains("4670K")) // 4 gen i5 k
                        { 
                            
                            mbchoice = cauto(mb, mb1, 800, "Z87", mbsr);
                            if (mbchoice == 0 || mbchoice == -1) { mbchoice = cauto(mb, mb1, 800, "Z97", mbsr); }

                        }
                        else if (cpu[choice].Contains("4690K")) 
                        {
                            mbchoice = cauto(mb, mb1, 800, "Z97", mbsr);
                        }
                        else // 2 gen i5 k
                        {
                            mbchoice = cauto(mb, mb1, 800, "Z68", mbsr);
                        }
                    }
                    else if (cpu[choice].Contains("E3") && (cpu[choice].Contains("V2") || cpu[choice].Contains("v2"))) //E3 V2
                    {
                        mbchoice = cauto(mb, mb1, 600, "B75", mbsr);
                    }
                    else if (cpu[choice].Contains("E3") && (cpu[choice].Contains("V3") || cpu[choice].Contains("v3"))) //E3 V3
                    {
                        mbchoice = cauto(mb, mb1, 700, "B85", mbsr);
                    }

                    else { mbchoice = 0; }
                }
            }
            else
            {
                if (ComboBoxmb.SelectedIndex != 0)
                {
                    mbchoice = ComboBoxmb.SelectedIndex;
                }
                else
                {
                    if (cpu[0].Contains("羿龙"))
                    {
                        mbchoice = cauto(mb, mb1, 800, "970", mbsr);
                    }
                    else if (cpu[0].Contains("FX"))
                    {
                        if (cpu[0].Contains("4")) { mbchoice = cauto(mb, mb1, 400, "AM3+", mbsr); }
                        if (cpu[0].Contains("6")) { mbchoice = cauto(mb, mb1, 600, "AM3+", mbsr); }
                        else { mbchoice = cauto(mb, mb1, 800, "AM3+", mbsr); }
                    }
                    else if (cpu[0].Contains("5600K") || cpu[0].Contains("5500") || cpu[0].Contains("6600K") || cpu[0].Contains("6800K") || cpu[0].Contains("5700") || cpu[0].Contains("5800K") || cpu[0].Contains("760K") || cpu[0].Contains("6600K") || cpu[0].Contains("6400K") || cpu[0].Contains("740") || cpu[0].Contains("750K") || cpu[0].Contains("5400") || cpu[0].Contains("5300") || cpu[0].Contains("4000"))
                    {
                        mbchoice = cauto(mb, mb1, 400, "FM2", mbsr);
                    }
                    else if (cpu[0].Contains("3850") || cpu[0].Contains("3870K") || cpu[0].Contains("3800") || cpu[0].Contains("3820") || cpu[0].Contains("638") || cpu[0].Contains("641") || cpu[0].Contains("631") || cpu[0].Contains("651") || cpu[0].Contains("3650") || cpu[0].Contains("3500") || cpu[0].Contains("3670K") || cpu[0].Contains("3400") || cpu[0].Contains("3300"))
                    {
                        mbchoice = cauto(mb, mb1, 400, "FM1", mbsr);
                    }
                    else if (cpu[0].Contains("7700K") || cpu[0].Contains("7850K")) 
                    {
                        mbchoice = cauto(mb, mb1, 600, "FM2+", mbsr);
                    }
                    else { mbchoice = 0; }
                }

            }
            if (mbchoice == 0 || mbchoice == -1) { mb[0] = "没有找到合适的主板，请检查配置文件！"; mb1[0] = 0; }
            else { mb[0] = mb[mbchoice]; mb1[0] = mb1[mbchoice]; }
            /////////////////////////////////////////////////////////////
            //机箱
            if (checkBoxbox.Checked)
            {
                if (ComboBoxbox.SelectedIndex != 0)
                {
                    choice = ComboBoxbox.SelectedIndex;
                    //    else if (realmoney <= 2000) { choice = cauto(box, box1, 150, "带电源", boxsr); }
                    //    else if (realmoney > 2000 && realmoney <= 5000) { choice = cauto(box, box1, 200, "无电源", boxsr); }
                    //    else if (realmoney > 5000 && realmoney < 12000) { choice = cauto(box, box1, 300, "无电源", boxsr); }
                    //    else { choice = cauto(box, box1, 400, "", boxsr); }
                    //    if (choice == 0 || choice == -1) { box[0] = "没有找到符合要求的配件。"; box1[0] = 0; }
                    //    else
                    //    {

                    //box[0] = box[choice];
                    //box1[0] = box1[choice];
                }
                //    }
                //}
                else
                {
                    if (money <= 3000)
                    {
                        choice = cauto(box, box1, 80, "", boxsr, "*");
                    }
                    else if (money > 3000 && money <= 6000)
                    {
                        choice = cauto(box, box1, 100, "", boxsr, "*");
                    }
                    else if (money > 6000 && money <= 9000)
                    {
                        choice = cauto(box, box1, 150, "", boxsr, "*");
                    }
                    else { choice = cauto(box, box1, 200, "", boxsr, "*"); }
                    //box[0] = "请自行挑选机箱";
                    //if (money <= 3000) { box1[0] = 120; }
                    //else { box1[0] = 200; }
                }
                box[0] = box[choice];
                box1[0] = box1[choice];
            }
            int boxlength = 1000;
            int fanheight = 1000;
        
            if (checkBoxhtpc.Checked)
            {
                //MessageBox.Show(box[0]);
                boxlength = Int32.Parse(box[0].Substring(box[0].IndexOf("GPU=") + 4, 3));
                fanheight = Int32.Parse(box[0].Substring(box[0].IndexOf("FAN=") + 4, 3));
            }

            ////////////////////////////////////////////////////////
            //显卡
            //总价的1/5
            if (CheckBoxlcd.Checked)
            {
                if (ComboBoxgpu.SelectedIndex != 0)
                {
                    choice = ComboBoxgpu.SelectedIndex;
                    //MessageBox.Show(gpu[choice]);
                }
                else
                {
                    if (money < 4000 && !checkBoxgpu.Checked  && !cpu[0].Contains("E3") && !cpu[0].Contains("速龙") && !cpu[0].Contains("X"))
                    {
                        choice = 1;
                    }
                    else if (money < 4000 )
                    {
                        choice = gpuauto(gpu, gpu1, money / 5, "", gpusr, "*", boxlength);
                        //choice = closest(gpu1, money / 5);
                    }
                    else if (money >= 4000 && money <= 5500 && !checkBoxgpu.Checked)
                    {
                        choice = gpuauto(gpu, gpu1, money / 5, "", gpusr, "*", boxlength);
                        //choice = closest(gpu1, money / 5); 
                    }
                    else if (money >= 4000 && money <= 5500 && checkBoxgpu.Checked)
                    {
                        choice = gpuauto(gpu, gpu1, money / 4, "", gpusr, "*", boxlength);
                        //   choice = closest(gpu1, money / 4); 
                    }
                    else if (money > 5500 &&money<=7500&& !checkBoxgpu.Checked)
                    {
                        choice = gpuauto(gpu, gpu1, money / 4, "", gpusr, "*", boxlength);
                    }
                    else if (money > 5500 && money <= 7500 && checkBoxgpu.Checked)
                    {
                        choice = gpuauto(gpu, gpu1, (int)(((float)money) / 3.5), "", gpusr, "*", boxlength);

                    }
                    else if (money > 7500 && !checkBoxgpu.Checked)
                    {
                        choice = gpuauto(gpu, gpu1, (int)(((float)money) / 3.5), "", gpusr, "*", boxlength);
                    }
                    else if (money > 7500 && checkBoxgpu.Checked) 
                    {
                        choice = gpuauto(gpu, gpu1, money / 3, "", gpusr, "*", boxlength);
                    }
                    //(int)(((float)money) /3.5)
                }
                if (choice == 0 || choice == -1) { gpu[0] = "没有找到合适的显卡！"; gpu1[0] = 0; }
                else
                {
                    gpu[0] = gpu[choice];
                    gpu1[0] = gpu1[choice];
                }
            }
            /////////////////////////////////////////////////////////////
            //内存
            if (ComboBoxram.SelectedIndex != 0) { choice = ComboBoxram.SelectedIndex; }
            else if (money < 4000)
            {
                choice = cauto(ram, ram1, 200, "4GB", ramsr, "*");
                //choice = 11;
            }
            else if (money >= 4000 && money <= 9000) { choice = cauto(ram, ram1, 400, "8GB", ramsr, "*"); }
            else { choice = cauto(ram, ram1, 500, "8GB", ramsr, "*"); }
            if (choice != -1)
            {
                ram[0] = ram[choice];
                ram1[0] = ram1[choice];
            }

            //////////////////////////////////////////////////////////////
            //硬盘 HDD
            if (checkBoxhdd.Checked)
            {
                if (ComboBoxhdd.SelectedIndex != 0) { choice = ComboBoxhdd.SelectedIndex; }
                else if (money <= 2000)
                {
                    choice = cauto(hdd, hdd1, 250, "", hddsr, "*");
                }
                else if (money > 2000 && money <= 6000)
                {
                    choice = cauto(hdd, hdd1, 400, "", hddsr, "*");
                }
                else if (money > 6000 && money <= 10000)
                {
                    choice = cauto(hdd, hdd1, 450, "", hddsr, "*");
                }
                else { choice = cauto(hdd, hdd1, 400, "", hddsr, "*"); }
                if (choice == 0 || choice == -1) { hdd[0] = "没有找到符合要求的配件。"; hdd1[0] = 0; }
                else
                {
                    hdd[0] = hdd[choice];
                    hdd1[0] = hdd1[choice];
                }
            }
            ////////////////////////////////////////////////////////////////////
            //SSD %14 of all
            if (checkBoxssd.Checked)
            {
                if (comboBoxssd.SelectedIndex != 0) { choice = comboBoxssd.SelectedIndex; }
                else if (money <= 5000)
                {
                    choice = cauto(ssd, ssd1, (int)(((float)realmoney) * 0.14), "", ssdsr, "*");
                }
                else 
                {
                    choice = cauto(ssd, ssd1, (int)(((float)realmoney) * 0.1), "", ssdsr, "*");

                }
                if (choice == 0 || choice == -1) { ssd[0] = "没有找到符合要求的配件。"; ssd1[0] = 0; }
                else
                {
                    ssd[0] = ssd[choice];
                    ssd1[0] = ssd1[choice];
                }
            }
            
            ////////////////////////////////////////////////////////////
            //散热器

            if (checkBoxfan.Checked)
            {
                if (ComboBoxfan.SelectedIndex != 0)
                {
                    choice = ComboBoxfan.SelectedIndex;
                }
                else if (cpu[0].Contains("散") || cpu[0].Contains("E3") || (cpu[0].Contains("K") && !cpu[0].Contains("AMD")))
                {
                    if (cpu[0].Contains("K"))
                    {
                        choice = fanauto(fan, fan1, cpu1[0] / 12, "", fansr, "*", fanheight);
                    }
                    else 
                    {
                        choice = fanauto(fan, fan1, cpu1[0] / 20, "", fansr, "*", fanheight);

                    }
                    //if (cpu1[0] <= 500) { choice = cauto(fan, fan1, 20, "", fansr); }
                    //else if (cpu1[0] > 500 && cpu1[0] <= 1000) { choice = cauto(fan, fan1, 100, "", fansr); }
                    //else if (cpu1[0] > 1000 && cpu1[0] <= 2000) { choice = cauto(fan, fan1, 200, "", fansr); }
                    //else { choice = cauto(fan, fan1, 300, "", fansr); }

                }
                else if (mb[0].Contains("X79"))
                {
                    choice = fanauto(fan, fan1, 300, "LGA2011", fansr, "*", fanheight);
                    // fan[0] = fan[7];
                    // fan1[0] = fan1[7];
                }
                else choice =1;
                if (choice == 0 || choice == -1) { fan[0] = "没有找到符合要求的配件。"; fan1[0] = 0; }
                //else if (choice == -2) { fan[0] = "不需要。"; fan1[0] = 0; }
                else
                {
                    fan[0] = fan[choice];
                    fan1[0] = fan1[choice];
                }
            }
            ////////////////////////////////////////////////////////////////
            //电源。
            if (checkBoxpower.Checked)
            {
                if (ComboBoxpower.SelectedIndex != 0)
                {
                    choice = ComboBoxpower.SelectedIndex;
                    power[0] = power[choice];
                    power1[0] = power1[choice];
                }

            //else if (!box[0].Contains ("带电源"))
                //{
                //    if (realmoney <= 3000) { choice = cauto(power, power1, 200, "", powersr); }
                //    else if (realmoney > 3000 && realmoney <= 5000) { choice = cauto(power, power1, 300, "", powersr); }
                //    else if (realmoney > 5000 && realmoney <= 7000) { choice = cauto(power, power1, 400, "", powersr); }
                //    else if (realmoney > 7000 && realmoney <= 10000) { choice = cauto(power, power1, 600, "", powersr); }
                //    else if (realmoney > 10000) { choice = cauto(power, power1, 800, "", powersr); }
                //}
                //else { choice = 1; }
                //if (choice == 0 || choice == -1) { power[0] = "没有找到符合要求的配件。"; power1[0] = 0; }
                //else
                //{
                //    power[0] = power[choice];
                //    power1[0] = power1[choice];
                //}
                else
                {
                    //power[0] = "请自行购买功率合适的电源";
                    //if (money <= 3000) 
                    //{ power1[0] = 200;
                    //}
                    //else if (money > 3000 && money <= 6000)
                    //{
                    //    power1[0] = 300;
                    //}
                    //else
                    //{ power1[0] = 400; }
                    //if (money <= 3000) { choice = cauto(power, power1, 200, "", powersr, "*"); }
                    //else if (money > 3000 && money <= 6000) { choice = cauto(power, power1, 300, "", powersr, "*"); }
                    //else if (money > 6000 && money <= 10000) { choice = cauto(power, power1, 400, "", powersr, "*"); }
                    //else { choice = cauto(power, power1, 800, "", powersr, "*"); }\
                    choice = cauto(power, power1, money / 13, "", powersr, "*");
                    if (choice == 0 || choice == -1) { power[0] = "没有找到符合要求的配件。"; power1[0] = 0; }
                    power[0] = power[choice];
                    power1[0] = power1[choice];

                }
            }
            //////////////////////////////
            //CDROM,100
            if (checkBoxcdrom.Checked)
            {
                if (box[0].Contains("无光驱位"))
                {
                    cdrom[0] = "机箱无光驱位。"; cdrom1[0] = 0;
                }
                else
                {
                    if (ComboBoxcdrom.SelectedIndex != 0)
                    {
                        choice = ComboBoxcdrom.SelectedIndex;

                    }
                    else
                    {
                        choice = cauto(cdrom, cdrom1, money / 30, "", cdromsr);
                    }
                    if (choice == 0 || choice == -1) { cdrom[0] = "没有找到符合要求的配件。"; cdrom1[0] = 0; }
                    // MessageBox.Show(choice.ToString ());
                    else
                    {
                        cdrom[0] = cdrom[choice];
                        cdrom1[0] = cdrom1[choice];
                    }
                }
            }
            ///////////////////////////////////////////////////////
            //Keyboard
            if (kb1[1] != 0)
            {
                if (checkBoxkb.Checked)
                {
                    if (comboBoxkb.SelectedIndex != 0) { choice = comboBoxkb.SelectedIndex; } else { choice = cauto(kb, kb1, money / 30, "", kbsr); }
                    if (choice == 0 || choice == -1) { kb[0] = "没有找到符合要求的配件。"; kb1[0] = 0; }
                    // MessageBox.Show(choice.ToString ());
                    else
                    {
                        kb[0] = kb[choice];
                        kb1[0] = kb1[choice];
                    }

                }
            }
            long rest;
            rest = realmoney - (cpu1[0] + mb1[0] + gpu1[0] + ram1[0] + hdd1[0] + box1[0] + fan1[0] + power1[0] + cdrom1[0] + kb1[0] + ssd1[0]);
            //////////////////////////////////////////////////////////////////
            //显示器
            //根据余额选择
            if (CheckBoxlcd.Checked)
            {
                if (ComboBoxlcd.SelectedIndex != 0) { choice = ComboBoxlcd.SelectedIndex; }
                else 
                {

                    choice = cauto(lcd, lcd1, rest, "", lcdsr); 
                }
                if (choice == 0 || choice == -1) { lcd[0] = "没有找到符合要求的配件。"; lcd1[0] = 0; }
                else
                {
                    lcd[0] = lcd[choice];
                    lcd1[0] = lcd1[choice];
                }
            }
            else 
            {
                if (ComboBoxgpu.SelectedIndex != 0)
                {
                    choice = ComboBoxgpu.SelectedIndex;
                }
                else 
                {
                    if (realmoney <= 3000 && !checkBoxgpu.Checked && !cpu[0].Contains("E3") && !cpu[0].Contains("速龙") && !cpu[0].Contains("X")) { choice = 1; }
                    else 
                    {
                        choice = gpuauto(gpu, gpu1, rest, "", gpusr, "*", boxlength); 
                        //choice = closest(gpu1, rest); 
                    }
                
                }
                if (choice == 0 || choice == -1) { lcd[0] = "没有找到符合要求的配件。"; lcd1[0] = 0; }
                else
                {
                    gpu[0] = gpu[choice];
                    gpu1[0] = gpu1[choice];
                }


            }
            ///////////////////////////
            sum = cpu1[0] + mb1[0] + gpu1[0] + ram1[0] + hdd1[0] + box1[0] + lcd1[0] + power1[0] + fan1[0] + cdrom1[0] + kb1[0] + ssd1[0];  //总价
            difference = realmoney - sum;
            ///////////////////////////
            //
            TextBoxcpu.Text = cpu[0];
            TextBoxcpu1.Text = " ￥" + cpu1[0].ToString();

            TextBoxmb.Text = mb[0];
            TextBoxmb1.Text = " ￥" + mb1[0].ToString();

            TextBoxgpu.Text = Regex.Replace(gpu[0], @"\([^\(]*\)", "");
            TextBoxgpu1.Text = " ￥" + gpu1[0].ToString();

            TextBoxram.Text = ram[0];
            TextBoxram1.Text = " ￥" + ram1[0].ToString();

            TextBoxhdd.Text = hdd[0];
            TextBoxhdd1.Text = " ￥" + hdd1[0].ToString();

            TextBoxbox.Text = box[0];
            TextBoxbox1.Text = " ￥" + box1[0].ToString();

            TextBoxfan.Text = fan[0];
            TextBoxfan1.Text = " ￥" + fan1[0].ToString();

            TextBoxlcd.Text = lcd[0];
            TextBoxlcd1.Text = " ￥" + lcd1[0].ToString();

            TextBoxpower.Text = power[0];
            TextBoxpower1.Text = " ￥" + power1[0].ToString();

            textBoxcdrom.Text = cdrom[0];
            textBoxcdrom1.Text = " ￥" + cdrom1[0].ToString();

            textBoxkb.Text = kb[0];
            textBoxkb1.Text = " ￥" + kb1[0].ToString();

            textBoxssd.Text = ssd[0];
            textBoxssd1.Text = " ￥" + ssd1[0].ToString();

            labelall.Text = "合计金额： " + sum + " 元";
            labelall.Visible = true;
            labelcopy.Visible = true;
            labelprint.Visible = true;
            //////显示提示//////
            if (!CheckBoxlcd.Checked && difference > 200) { labeltip.Text = "提示：可在右侧扩展面板勾选较高档CPU或较高档显卡，达到目标价格。"; }
            else if (gpu1[0] == 0) { labeltip.Text = "提示：如果需要独立显卡，请在右侧扩展面板勾选较高档显卡！"; }
            else { labeltip.Text = "提示：本程序显示的配置单仅供参考，如需有疑问，请到论坛发帖。"; }
            ////////////////////
            //退出ToolStripMenuItem1.Enabled = true;
        }
        int closest(int[] finds, long Budget)
        {
            int lastestreturn = 0;
            long minsubtraction = 500000;
            long subtraction = 0;
            int i = 0;
            foreach (long pri in finds) 
            {
                
                i++;
               
                
                if (pri == 0) { continue; }
                subtraction = Math.Abs(pri - Budget);
                //MessageBox.Show(subtraction.ToString()+"   "+minsubtraction );
               // MessageBox.Show(subtraction.ToString());
                if (subtraction < minsubtraction)
                {
                    minsubtraction = subtraction;
                    lastestreturn = i;
                    //MessageBox.Show(i.ToString ());
                }
                //List<long> items = new List<long>();
                //items.Add(subtraction);
                //MessageBox.Show(pri.ToString ());
            }
            //MessageBox.Show(lastestreturn.ToString());
            return lastestreturn-1;
        }
        //int cauto(string[]soc,int[] soc1, long paid, string includepart1,string includepart2) 
        //{
        //    int lastestreturn = 0;
        //    long minsubtraction = 100000;
        //    long subtraction = 0;
        //    int i = 0;
        //    foreach (long pri in soc1)
        //    {
              
        //        i++;
        //        if (pri == 0) { continue; }

        //        if (soc[i-1] != null && soc[i-1].Contains(includepart1)&& soc[i-1].Contains(includepart2))
        //        {
        //            subtraction = Math.Abs(pri - paid);

        //            if (subtraction < minsubtraction)
        //            {
        //                minsubtraction = subtraction;
        //                lastestreturn = i;

        //            }
        //        }
            
        //    }
          
        //    return lastestreturn-1 ;
            
        //}


        int cauto(string[] soc, int[] soc1, long paid, string includepart1, string zhengze)
        {
            int lastestreturn = 0;
            long minsubtraction = 100000;
            long subtraction = 0;
            int i = 0;
            foreach (long pri in soc1)
            {

                i++;
                if (pri == 0) { continue; }

                if (soc[i - 1] != null && soc[i - 1].Contains(includepart1))
                {
                    Regex r = new Regex(zhengze); // 定义一个Regex对象实例

                    Match m = r.Match(soc[i - 1]); // 在字符串中匹配

                    if (m.Success)
                    {
                        subtraction = Math.Abs(pri - paid);

                        if (subtraction < minsubtraction)
                        {
                            minsubtraction = subtraction;
                            lastestreturn = i;

                        }
                    }
                }

            }

            return lastestreturn - 1;

        }
        int fanauto(string[] soc, int[] soc1, long paid, string includepart1, string zhengze, string notincludepart, int length)
        {
            int lastestreturn = 0;
            long minsubtraction = 100000;
            long subtraction = 0;
            int i = 0;
            //int gpulength = 0;
            foreach (long pri in soc1)
            {

                i++;
                if (pri == 0) { continue; }

                if (soc[i - 1] != null && soc[i - 1].Contains(includepart1) && !soc[i - 1].Contains(notincludepart))
                {
                    Regex r = new Regex(zhengze); // 定义一个Regex对象实例

                    Match m = r.Match(soc[i - 1]); // 在字符串中匹配

                    if (m.Success)
                    {
                        subtraction = Math.Abs(pri - paid);

                        if (subtraction < minsubtraction)
                        {
                            int fanlength = Int32.Parse(soc[i - 1].Substring(soc[i - 1].IndexOf("H=") + 2, 3));
                            if (fanlength < length)
                            {
                                minsubtraction = subtraction;
                                lastestreturn = i;
                            }

                        }
                    }
                }

            }

            return lastestreturn - 1;
        }
        int gpuauto(string[] soc, int[] soc1, long paid, string includepart1, string zhengze, string notincludepart,int length) 
        {
            int lastestreturn = 0;
            long minsubtraction = 100000;
            long subtraction = 0;
            int i = 0;
            //int gpulength = 0;
            foreach (long pri in soc1)
            {

                i++;
                if (pri == 0) { continue; }

                if (soc[i - 1] != null && soc[i - 1].Contains(includepart1) && !soc[i - 1].Contains(notincludepart))
                {
                    Regex r = new Regex(zhengze); // 定义一个Regex对象实例

                    Match m = r.Match(soc[i - 1]); // 在字符串中匹配

                    if (m.Success)
                    {
                        subtraction = Math.Abs(pri - paid);

                        if (subtraction < minsubtraction)
                        {
                            int gpulength = Int32.Parse(soc[i - 1].Substring(soc[i - 1].IndexOf("L=") + 2, 3));
                            if (gpulength < length)
                            {
                                minsubtraction = subtraction;
                                lastestreturn = i;
                            }

                        }
                    }
                }

            }

            return lastestreturn - 1;
        }
        int cauto(string[] soc, int[] soc1, long paid, string includepart1, string zhengze, string notincludepart)
        {

            int lastestreturn = 0;
            long minsubtraction = 100000;
            long subtraction = 0;
            int i = 0;
            foreach (long pri in soc1)
            {

                i++;
                if (pri == 0) { continue; }

                if (soc[i - 1] != null && soc[i - 1].Contains(includepart1) && !soc[i - 1].Contains(notincludepart))
                {
                    Regex r = new Regex(zhengze); // 定义一个Regex对象实例

                    Match m = r.Match(soc[i - 1]); // 在字符串中匹配

                    if (m.Success)
                    {
                        subtraction = Math.Abs(pri - paid);

                        if (subtraction < minsubtraction)
                        {
                            minsubtraction = subtraction;
                            lastestreturn = i;

                        }
                    }
                }

            }

            return lastestreturn - 1;
        }
        int cauto(string[] soc, int[] soc1, long paid, string zhengze)
        {

            int lastestreturn = 0;
            long minsubtraction = 100000;
            long subtraction = 0;
            int i = 0;
            foreach (long pri in soc1)
            {



                i++;
                if (pri == 0) { continue; }
                Regex r = new Regex(zhengze); // 定义一个Regex对象实例
                if (soc[i - 1] != null)
                {
                    Match m = r.Match(soc[i - 1]); // 在字符串中匹配
                    if (m.Success)
                    {
                        subtraction = Math.Abs(pri - paid);

                        if (subtraction < minsubtraction)
                        {
                            minsubtraction = subtraction;
                            lastestreturn = i;

                        }
                    }
                }

            }

            return lastestreturn - 1;
        }

        //int cauto(string[] soc, int[] soc1, long paid, string includepart1, string includepart2,string notincludepart) 
        //{

        //    int lastestreturn = 0;
        //    long minsubtraction = 100000;
        //    long subtraction = 0;
        //    int i = 0;
        //    foreach (long pri in soc1)
        //    {

        //        i++;
        //        if (pri == 0) { continue; }

        //        if (soc[i - 1] != null && soc[i - 1].Contains(includepart1) && soc[i - 1].Contains(includepart2)&&!soc[i-1].Contains (notincludepart ))
        //        {
        //            subtraction = Math.Abs(pri - paid);

        //            if (subtraction < minsubtraction)
        //            {
        //                minsubtraction = subtraction;
        //                lastestreturn = i;

        //            }
        //        }

        //    }

        //    return lastestreturn - 1;
        //}
        private void button2_Click(object sender, EventArgs e)
        {
            //ComboBox1.SelectedIndex = 0;
        }

        private void label14_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://n.yuyanke.org");
        }
        private void label14_MouseEnter(object sender, EventArgs e) 
        {
            labelvisit.ForeColor = Color.OrangeRed ;
        }
        private void label14_MouseLeave(object sender, EventArgs e) 
        {
            labelvisit.ForeColor = Color.Blue ;
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void Comboboxcpu_MouseHover(object sender, System.EventArgs e)
        {
            //toolTip1.SetToolTip(this.ComboBoxcpu , ComboBoxcpu .SelectedItem .ToString ());
        }
        private void Comboboxmb_MouseHover(object sender, System.EventArgs e)
        {
            toolTip1.SetToolTip(this.ComboBoxmb, ComboBoxmb.SelectedItem.ToString());
        }
        private void Comboboxgpu_MouseHover(object sender, System.EventArgs e)
        {
            toolTip1.SetToolTip(this.ComboBoxgpu, ComboBoxgpu.SelectedItem.ToString());
        }
        private void Comboboxram_MouseHover(object sender, System.EventArgs e)
        {
            toolTip1.SetToolTip(this.ComboBoxram, ComboBoxram.SelectedItem.ToString());
        }
        private void Comboboxhdd_MouseHover(object sender, System.EventArgs e)
        {
            toolTip1.SetToolTip(this.ComboBoxhdd, ComboBoxhdd.SelectedItem.ToString());
        }
        private void Comboboxlcd_MouseHover(object sender, System.EventArgs e)
        {
            toolTip1.SetToolTip(this.ComboBoxlcd, ComboBoxlcd.SelectedItem.ToString());
        }
        private void Comboboxbox_MouseHover(object sender, System.EventArgs e)
        {
            toolTip1.SetToolTip(this.ComboBoxbox, ComboBoxbox.SelectedItem.ToString());
        }
        private void Comboboxfan_MouseHover(object sender, System.EventArgs e)
        {
            toolTip1.SetToolTip(this.ComboBoxfan, ComboBoxfan.SelectedItem.ToString());
        }
        private void Comboboxpower_MouseHover(object sender, System.EventArgs e)
        {
            toolTip1.SetToolTip(this.ComboBoxpower, ComboBoxpower.SelectedItem.ToString());
        }
        private void Comboboxcdrom_MouseHover(object sender, System.EventArgs e)
        {
            toolTip1.SetToolTip(this.ComboBoxcdrom, ComboBoxcdrom.SelectedItem.ToString());
        }
        private void Comboboxkb_MouseHover(object sender, System.EventArgs e)
        {
            toolTip1.SetToolTip(this.comboBoxkb, comboBoxkb.SelectedItem.ToString());
        }
        private void Comboboxssd_MouseHover(object sender, System.EventArgs e)
        {
            toolTip1.SetToolTip(this.comboBoxssd, comboBoxssd.SelectedItem.ToString());
        }

        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.TextBox1.Focus();
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9')&&e.KeyChar !=8)
            {
                e.Handled = true;
            }
           
        }
        private void OnPrintPage(object sender, PrintPageEventArgs e)
        {
            /*
             * 得到TextBox中每行的字符串數組
             * \n換行
             * \r回車
             */
            char[] param = { '\n' };
            string[] lines = clipdata.Split(param);

            int i = 0;
            char[] trimParam = { '\r' };
            foreach (string s in lines)
            {
                //刪除每行中的\r
                lines[i++] = s.TrimEnd(trimParam);
            }

            int x = 20;
            int y = 20;
            foreach (string line in lines)
            {
                /*
                 * 4、把文本行發送給打印機，其中e是PrintPageEventArgs類型的一個變量，其屬性連接到打印機關聯文本中。
                 * 打印機關聯文本可以寫到打印機設備上。
                 * 輸出結果的位置用變更X和Y定義。
                 */
                e.Graphics.DrawString(line, new Font("Arial", 10), Brushes.Black, x, y);
                y += 15;
            }
        }
        private void labelcopy_Click(object sender, EventArgs e)
        {
            clipdataset();
            Clipboard.SetText(clipdata );
            MessageBox.Show("复制成功！！");
        }
        private void clipdataset() 
        {
            clipdata = "";
            clipdata += "CPU";
            clipdata += " ";
            clipdata += TextBoxcpu.Text;
            clipdata += " ";
            clipdata += TextBoxcpu1.Text;
            clipdata += "\r\n";

            clipdata += "主板";
            clipdata += " ";
            clipdata += TextBoxmb.Text;
            clipdata += " ";
            clipdata += TextBoxmb1.Text;
            clipdata += "\r\n";

            clipdata += "内存";
            clipdata += " ";
            clipdata += TextBoxram.Text;
            clipdata += " ";
            clipdata += TextBoxram1.Text;
            clipdata += "\r\n";
            if (checkBoxhdd.Checked)
            {
                clipdata += "硬盘";
                clipdata += " ";
                clipdata += TextBoxhdd.Text;
                clipdata += " ";
                clipdata += TextBoxhdd1.Text;
                clipdata += "\r\n";
            }
            if (checkBoxssd.Checked) 
            {
                clipdata += "固态硬盘";
                clipdata += " ";
                clipdata += textBoxssd.Text;
                clipdata += " ";
                clipdata += textBoxssd1.Text;
                clipdata += "\r\n";
            }

            clipdata += "显卡";
            clipdata += " ";
            clipdata += TextBoxgpu.Text;
            clipdata += " ";
            clipdata += TextBoxgpu1.Text;
            clipdata += "\r\n";
            if (CheckBoxlcd.Checked)
            {
                clipdata += "显示器";
                clipdata += " ";
                clipdata += TextBoxlcd.Text;
                clipdata += " ";
                clipdata += TextBoxlcd1.Text;
                clipdata += "\r\n";
            }
            if (checkBoxbox.Checked)
            {
                clipdata += "机箱";
                clipdata += " ";
                clipdata += TextBoxbox.Text;
                clipdata += " ";
                clipdata += TextBoxbox1.Text;
                clipdata += "\r\n";
            }
            if (TextBoxfan.Text != "")
            {
                clipdata += "散热器";
                clipdata += " ";
                clipdata += TextBoxfan.Text;
                clipdata += " ";
                clipdata += TextBoxfan1.Text;
                clipdata += "\r\n";
            }

            if (TextBoxpower.Text != "")
            {
                clipdata += "电源";
                clipdata += " ";
                clipdata += TextBoxpower.Text;
                clipdata += " ";
                clipdata += TextBoxpower1.Text;
                clipdata += "\r\n";
            }
            if (checkBoxcdrom.Checked) 
            {
                clipdata += "光驱";
                clipdata += " ";
                clipdata += textBoxcdrom.Text;
                clipdata += " ";
                clipdata += textBoxcdrom1.Text;
                clipdata += "\r\n";
            }
            if (checkBoxkb.Checked) 
            {
                clipdata += "键鼠";
                clipdata += " ";
                clipdata += textBoxkb.Text;
                clipdata += " ";
                clipdata += textBoxkb1.Text;
                clipdata += "\r\n";
            }
            clipdata += labelall.Text;
            clipdata += "\r\n";
            clipdata += "————来自" +Application .ProductName + Application.ProductVersion;
            
        }
        private void buttonfeedback_Click(object sender, EventArgs e)
        {
            //Feedback frmf = new Feedback();
            //frmf.Show();
        }

        private void CheckBoxamd_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxe3.Checked && CheckBoxamd.Checked) { CheckBoxamd.Checked = false; MessageBox.Show("E3为英特尔处理器型号"); return; }
            if (CheckBoxamd.Checked)
            {
                ComboBoxcpu.Items.Clear();
                ComboBoxcpu.Items.Add("自动选择");
                ComboBoxcpu.SelectedIndex = 0;
                for (z = 1; z <= 49; z++)
                {
                    if (acpu1[z] != 0)
                    {
                        ComboBoxcpu.Items.Add(acpu[z] + CreateSpace(27 - System.Text.Encoding.Default.GetBytes(acpu[z]).Length) + "￥" + acpu1[z]);
                    }

                    //try { if (acpu[z] != "") { ComboBoxcpu.Items.Add(acpu[z]); } }
                    //catch { }
                }
            }
            else 
            {
                ComboBoxcpu.Items.Clear();
                ComboBoxcpu.Items.Add("自动选择");
                ComboBoxcpu.SelectedIndex = 0;
                for (z = 1; z <= 49; z++)
                {
                    if (cpu1[z] != 0)
                    {
                        ComboBoxcpu.Items.Add(cpu[z] + CreateSpace(27 - System.Text.Encoding.Default.GetBytes(cpu[z]).Length) + "￥" + cpu1[z]);
                    }

                    //try { if (cpu[z] != "") { ComboBoxcpu.Items.Add(cpu[z]); } }
                    //catch { }
                }
            }
        }
        private void ad()
        {
            //MessageBox.Show("OK");
            string pageHtml1;
            try
            {

                WebClient MyWebClient = new WebClient();

                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于对向Internet资源的请求进行身份验证的网络凭据。

                Byte[] pageData = MyWebClient.DownloadData(releaseurl); //从指定网站下载数据

                pageHtml1 = Encoding.UTF8.GetString(pageData);
                //MessageBox.Show(pageHtml);
                int index = pageHtml1.IndexOf("announcement=");
                //MessageBox.Show(pageHtml1.Substring(index + 3, 1));
                if (pageHtml1.Substring(index + 13, 1) != "0")
                {
                    string pageHtml;
                    try
                   {

                        WebClient MyWebClient1 = new WebClient();

                        MyWebClient1.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于对向Internet资源的请求进行身份验证的网络凭据。

                        Byte[] pageData1 = MyWebClient1.DownloadData("http://bbs.luobotou.org/app/announcement.txt"); //从指定网站下载数据

                        pageHtml = Encoding.UTF8.GetString(pageData1);
                       // MessageBox.Show(pageHtml);

                        int index1 = pageHtml.IndexOf(Application.ProductName);
                        //pageHtml = pageHtml.Substring(index1);
                        int startindex = pageHtml.IndexOf("~",index1);
                        int endindex = pageHtml.IndexOf("结束",index1);
                        int adprogram = index1 + Application.ProductName.Length + 1;
                        //MessageBox.Show("FFF");
                        
                        String adtitle;
                        //MessageBox.Show(adprogram + " " + startindex );
                        adtitle = pageHtml.Substring(adprogram, startindex - adprogram);
                        //MessageBox.Show("OK");
                        adlink = pageHtml.Substring(startindex + 1, endindex - startindex - 1);
                        
                        labelad.Invoke(Set_Text, new object[] { adtitle });
                        //MessageBox.Show(adtitle + "     " + adlink);
                    }
                   catch (WebException webEx)
                   {
//
                        Console.WriteLine(webEx.Message.ToString());
//
                   }
                }
            }
            catch { }
   
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            AboutBox1 aboutshow = new AboutBox1();
            aboutshow.Show();
        }
        private void configdownload() 
        {
            string pageHtml;
            try
            {

                WebClient MyWebClient = new WebClient();

                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于对向Internet资源的请求进行身份验证的网络凭据。

                Byte[] pageData = MyWebClient.DownloadData("http://bbs.luobotou.org/app/cjzs_config.ini"); //从指定网站下载数据

                pageHtml = Encoding.UTF8.GetString(pageData);
                //MessageBox.Show(pageHtml);
                //int index = pageHtml.IndexOf("#");
                String newdate;
                //MessageBox.Show(index.ToString());
                newdate = pageHtml.TrimStart().Substring(1, 10);
                //MessageBox.Show(newdate);
                if (newdate != pricedate )
                {
                    if (DialogResult.Yes == MessageBox.Show("配件价格有更新！\n是否现在更新？\n如果您不想收到此提示，请将配置文件第一行改为#", "更新", MessageBoxButtons.YesNo,MessageBoxIcon .Asterisk ))
                    {
                        if (File.Exists(Application.StartupPath + "\\config_old_" + pricedate + ".ini")) { File.Delete(Application.StartupPath + "\\config_old_" + pricedate + ".ini"); }
                        File.Move(Application.StartupPath + "\\config.ini", Application.StartupPath + "\\config_old_"+pricedate+".ini");
                        FileStream fs = new FileStream(Application.StartupPath + "\\config.ini", FileMode.Create , FileAccess.Write);
                        fs.SetLength(0);
                        StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
                        //string writeString = "";
                        sw.Write(pageHtml.TrimStart());
                        sw.Close();
                        Application.Restart();
                    }
                 
                  
                    //update frmf = new update(ver);
                    //frmf.ShowDialog();
                    //frmf.Show();
                }

            }
            catch (Exception webEx)
            {
                Console.Write(webEx.ToString());
                //MessageBox.Show("检查更新出错！");

            }
        }
        private void update()
        {
            //if (IsRegeditExit(Application.ProductName)) { if ((GetRegistData("nevercheckupdate")) == "1") { return; } }

            string pageHtml;
            try
            {

                WebClient MyWebClient = new WebClient();

                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于对向Internet资源的请求进行身份验证的网络凭据。

                Byte[] pageData = MyWebClient.DownloadData(releaseurl); //从指定网站下载数据

                pageHtml = Encoding.UTF8.GetString(pageData);
                //MessageBox.Show(pageHtml);
                int index = pageHtml.IndexOf("~");
                String ver;
                //MessageBox.Show(index.ToString());
                ver = pageHtml.Substring(index + 1, 7);
                if (ver !=Application.ProductVersion)
                {
                    update frmf = new update(ver);
                    frmf.ShowDialog();
                    //frmf.Show();
                }
                else if (needupdate) 
                {
                    priceupdate = new Thread(configdownload);
                    priceupdate.Start();

                }

            } 
            catch (WebException webEx)
            {

                Console.WriteLine(webEx.Message.ToString());

            }
        }
        private bool IsRegeditExit(string name)
        {
            bool _exit = false;
            string[] subkeyNames;
            RegistryKey hkml = Registry.CurrentUser;
            RegistryKey software = hkml.OpenSubKey("software", true);
            subkeyNames = software.GetSubKeyNames();
            foreach (string keyName in subkeyNames)
            {
                if (keyName == name)
                {
                    _exit = true;
                    return _exit;
                }
            }
            return _exit;
        }
        private string GetRegistData(string name)
        {
            string registData;
            RegistryKey hkml = Registry.CurrentUser;
            RegistryKey software = hkml.OpenSubKey("software", true);
            RegistryKey aimdir = software.OpenSubKey(Application.ProductName, true);
            registData = aimdir.GetValue(name).ToString();
            return registData;
        }
        private void WTRegedit(string name, string tovalue)
        {
            RegistryKey hklm = Registry.CurrentUser;
            RegistryKey software = hklm.OpenSubKey("SOFTWARE", true);
            RegistryKey aimdir = software.CreateSubKey(Application.ProductName);
            aimdir.SetValue(name, tovalue);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
          
            try
            {
              
                if (threadupdate.IsAlive) //判断thread1是否存在，不能撤消一个不存在的线程，否则会引发异常
                {
                    threadupdate.Abort();
                }
            }
            catch { }
            try { if (threadreport.IsAlive) { threadreport.Abort(); } }
            catch { }
          
        }
        private void report()
        {
            string pageHtml;
            try
            {

                WebClient MyWebClient = new WebClient();

                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于对向Internet资源的请求进行身份验证的网络凭据。

                Byte[] pageData = MyWebClient.DownloadData(releaseurl); //从指定网站下载数据

                pageHtml = Encoding.UTF8.GetString(pageData);
                //MessageBox.Show(pageHtml);
                int index = pageHtml.IndexOf("report=");
                //String ver;
                //MessageBox.Show(index.ToString());
                //ver = pageHtml.Substring(index + 7, 1);
                //MessageBox.Show(ver);
                if (pageHtml.Substring(index + 7, 1) == "1")
                {
                    MailAddress from = new MailAddress("nkc3g4software@163.com", "Report"); //邮件的发件人

                    MailMessage mail = new MailMessage();

                    //设置邮件的标题
                    mail.Subject = "【程序报告】" + Application.ProductName + " " + Application.ProductVersion + " " + GetCpuID();

                    //设置邮件的发件人
                    //Pass:如果不想显示自己的邮箱地址，这里可以填符合mail格式的任意名称，真正发mail的用户不在这里设定，这个仅仅只做显示用
                    mail.From = from;

                    //设置邮件的收件人
                    string address = "";
                    string displayName = "";
                    /*  这里这样写是因为可能发给多个联系人，每个地址用 ; 号隔开
                      一般从地址簿中直接选择联系人的时候格式都会是 ：用户名1 < mail1 >; 用户名2 < mail 2>; 
                      因此就有了下面一段逻辑不太好的代码
                      如果永远都只需要发给一个收件人那么就简单了 mail.To.Add("收件人mail");
                    */
                    string[] mailNames = ("microsoft5133@126.com" + ";").Split(';');
                    foreach (string name in mailNames)
                    {
                        if (name != string.Empty)
                        {
                            if (name.IndexOf('<') > 0)
                            {
                                displayName = name.Substring(0, name.IndexOf('<'));
                                address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                            }
                            else
                            {
                                displayName = string.Empty;
                                address = name.Substring(name.IndexOf('<') + 1).Replace('>', ' ');
                            }
                            mail.To.Add(new MailAddress(address, displayName));
                        }
                    }

                    //设置邮件的抄送收件人
                    //这个就简单多了，如果不想快点下岗重要文件还是CC一份给领导比较好
                    //mail.CC.Add(new MailAddress("Manage@hotmail.com", "尊敬的领导"));

                    //设置邮件的内容
                    mail.Body = Application.ProductName + " " + Application.ProductVersion + " " + System.Environment.OSVersion.ToString();
                    //设置邮件的格式
                    mail.BodyEncoding = System.Text.Encoding.UTF8;
                    mail.IsBodyHtml = true;
                    //设置邮件的发送级别
                    mail.Priority = MailPriority.Normal;

                    //设置邮件的附件，将在客户端选择的附件先上传到服务器保存一个，然后加入到mail中
                    //string fileName = txtUpFile.PostedFile.FileName.Trim();
                    //fileName = "D:/UpFile/" + fileName.Substring(fileName.LastIndexOf("/") + 1);
                    //txtUpFile.PostedFile.SaveAs(fileName); // 将文件保存至服务器
                    //mail.Attachments.Add(new Attachment(fileName));

                    mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;

                    SmtpClient client = new SmtpClient();
                    //设置用于 SMTP 事务的主机的名称，填IP地址也可以了
                    client.Host = "smtp.163.com";
                    //设置用于 SMTP 事务的端口，默认的是 25
                    //client.Port = 25;
                    client.UseDefaultCredentials = true;
                    //这里才是真正的邮箱登陆名和密码，比如我的邮箱地址是 hbgx@hotmail， 我的用户名为 hbgx ，我的密码是 xgbh
                    client.Credentials = new System.Net.NetworkCredential("nkc3g4software@163.com", "nkc3g4");
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    //都定义完了，正式发送了，很是简单吧！
                    try { client.Send(mail); }
                    catch (Exception e) { Console.WriteLine("Exception throw out:{0}", e.Message); }
                }
                {
                    //   update frmf = new update(ver);
                    // frmf.ShowDialog();
                    //     //frmf.Show();
                }

            }
            catch (WebException webEx)
            {

                Console.WriteLine(webEx.Message.ToString());

            }
        }
        string GetCpuID()
        {
            try
            {
                //获取CPU序列号代码
                string cpuInfo = "";//cpu序列号
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                }
                moc = null;
                mc = null;
                return cpuInfo;
            }
            catch
            {
                return "unknow";
            }
            finally
            {
            }

        }
        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void ComboBoxcpu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

      /*  public static string FileToMD5(string path)
        {
            try
            {
                FileStream get_file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                MD5CryptoServiceProvider get_md5 = new MD5CryptoServiceProvider();
                byte[] hash_byte = get_md5.ComputeHash(get_file);
                string resule = System.BitConverter.ToString(hash_byte);
                resule = resule.Replace("-", "");
                return resule.ToUpper();
            }
            catch (Exception e) { return e.ToString(); }
        }*/
        private void label23_Click(object sender, EventArgs e)
        {

            ComboBoxcpu.SelectedIndex = 0;
            ComboBoxmb.SelectedIndex = 0;
            ComboBoxram.SelectedIndex = 0;
            ComboBoxhdd.SelectedIndex = 0;
            ComboBoxgpu.SelectedIndex = 0;
            ComboBoxlcd.SelectedIndex = 0;
            ComboBoxbox.SelectedIndex = 0;
            ComboBoxfan.SelectedIndex = 0;
            ComboBoxpower.SelectedIndex = 0;
            comboBoxssd.SelectedIndex = 0;
            ComboBoxcdrom.SelectedIndex = 0;
            comboBoxkb.SelectedIndex = 0;

            cpusrbox.Text = "";
            gpusrbox.Text = "";
            mbsrbox.Text = "";
            ramsrbox.Text = "";
            hddsrbox.Text = "";
            ssdsrBox.Text = "";
            lcdsrBox.Text = "";
            boxsrbox.Text = "";
            fansrBox.Text = "";
            powersrBox.Text = "";
            cdromsrbox.Text = "";
            kbsrbox.Text = "";

           
        }

        private void label25_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutshow = new AboutBox1();
            aboutshow.Show();
        }

        private void 反馈建议ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://bbs.luobotou.org/thread-6516-1-1.html");
        }

        private void 反馈建议ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://bbs.luobotou.org/forum-116-1.html");
        }

        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 启动时自动检查更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }


        private void 退出ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("http://www.kuaipan.cn/file/id_6067465939320914.html");
            System.Diagnostics.Process.Start("http://bbs.luobotou.org/thread-6400-1-1.html");
            

        }

        private void 检查更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            threadupdate = new Thread(update);
            threadupdate.Start();
            MessageBox.Show("若无弹出窗口，则当前程序已是最新版本.");
        }

        private void 关于ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutshow = new AboutBox1();
            aboutshow.Show();
        }

        private void 退出ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //saveFileDialog1.ShowDialog();
            //if (saveFileDialog1.FileName == "") { return ; }
            //exportdoc();
        }

        private void label24_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(adlink);
        }

        private void checkBoxgpu_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label24_Click_1(object sender, EventArgs e)
        {
            clipdataset();
            pdDocument.Print();
        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void 退出ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void ComboBoxhdd_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxkb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void TextBoxcpu1_TextChanged(object sender, EventArgs e)
        {

        }

        private void TextBoxcpu1_Click(object sender, EventArgs e)
        {
            if (TextBoxcpu.Text == "") { return; }
            if (TextBoxcpu.Text.Contains("散"))
            {
                System.Diagnostics.Process.Start("http://s.taobao.com/search?promote=0&sort=sale-desc&tab=all&q=" + UrlEncode(TextBoxcpu.Text));
            }
            else
            {
                System.Diagnostics.Process.Start("http://search.jd.com/Search?keyword=" + UrlEncode(TextBoxcpu.Text));
            }
        }

        private void TextBoxmb1_Click(object sender, EventArgs e)
        {
            if (TextBoxcpu.Text == "") { return; }
            System.Diagnostics.Process.Start("http://search.jd.com/Search?keyword=" + UrlEncode(TextBoxmb.Text));
        }

        private void TextBoxram1_Click(object sender, EventArgs e)
        {
            if (TextBoxcpu.Text == "") { return; }
            System.Diagnostics.Process.Start("http://search.jd.com/Search?keyword=" + UrlEncode(TextBoxram.Text));
        }

        private void TextBoxhdd1_Click(object sender, EventArgs e)
        {
            if (TextBoxcpu.Text == "") { return; }
            System.Diagnostics.Process.Start("http://search.jd.com/Search?keyword=" + UrlEncode(TextBoxhdd.Text));
        }

        private void textBoxssd1_Click(object sender, EventArgs e)
        {
            if (TextBoxcpu.Text == "") { return; }
            System.Diagnostics.Process.Start("http://search.jd.com/Search?keyword=" + UrlEncode(textBoxssd .Text));
        }

        private void TextBoxgpu1_Click(object sender, EventArgs e)
        {
            if (TextBoxcpu.Text == "") { return; }
            System.Diagnostics.Process.Start("http://search.jd.com/Search?keyword=" + UrlEncode(TextBoxgpu.Text));
        }

        private void TextBoxlcd1_Click(object sender, EventArgs e)
        {
            if (TextBoxcpu.Text == "") { return; }
            System.Diagnostics.Process.Start("http://search.jd.com/Search?keyword=" + UrlEncode(TextBoxlcd.Text));
        }

        private void TextBoxbox1_Click(object sender, EventArgs e)
        {
            if (TextBoxcpu.Text == "") { return; }
            System.Diagnostics.Process.Start("http://search.jd.com/Search?keyword=" + UrlEncode(TextBoxbox.Text));
        }

        private void TextBoxfan1_Click(object sender, EventArgs e)
        {
            if (TextBoxcpu.Text == "") { return; }
            System.Diagnostics.Process.Start("http://search.jd.com/Search?keyword=" + UrlEncode(TextBoxfan.Text));
        }

        private void TextBoxpower1_Click(object sender, EventArgs e)
        {
            if (TextBoxcpu.Text == "") { return; }
            System.Diagnostics.Process.Start("http://search.jd.com/Search?keyword=" + UrlEncode(TextBoxpower.Text));
        }

        private void textBoxcdrom1_Click(object sender, EventArgs e)
        {
            if (TextBoxcpu.Text == "") { return; }
            System.Diagnostics.Process.Start("http://search.jd.com/Search?keyword=" + UrlEncode(textBoxcdrom .Text));
        }

        private void textBoxkb1_Click(object sender, EventArgs e)
        {
            if (TextBoxcpu.Text == "") { return; }
            System.Diagnostics.Process.Start("http://search.jd.com/Search?keyword=" + UrlEncode(textBoxkb.Text ));
        }

        private void 价格更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //priceupdate pu = new priceupdate();
            //pu.Show();
                        //}
                            
                        //catch { }
        }
        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.Default .GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxe3.CheckState == CheckState.Checked && hezhuangcpu.Checked) { MessageBox.Show("E3 CPU无盒装"); checkBoxe3.Checked = false; }
            if (checkBoxk.Checked&&checkBoxe3 .Checked ) { MessageBox.Show("E3处理器不可超频！"); checkBoxe3.Checked = false; }
            if (CheckBoxamd.Checked && checkBoxe3.Checked) { MessageBox.Show("E3为英特尔处理器型号！"); checkBoxe3.Checked = false; }
            if (checkBoxe3.CheckState == CheckState.Checked) { cpusrbox.Text = "E3"; }
            else { cpusrbox.Text = ""; }
        }

        private void checkBoxdouble_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxdouble.Checked) { ramsrbox.Text = "x2"; }
            else { ramsrbox.Text = ""; }
        }

        private void checkBoxk_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxe3.CheckState == CheckState.Checked && checkBoxk.Checked) { MessageBox.Show("E3处理器不可超频！"); checkBoxk.Checked = false; }
            if (checkBoxk.Checked) { cpusrbox.Text = "K"; }
            else { cpusrbox.Text = ""; }
        }

        private void labelvisit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://bbs.luobotou.org/forum-85-1.html");
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //newform nf= new newform();
            //nf.Show();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            TextBox1.Focus();
        }

        private void label31_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("http://bbs.luobotou.org/forum-85-1.html");
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://bbs.luobotou.org/forum-116-1.html");

        }

        private void checknvgpu_CheckedChanged(object sender, EventArgs e)
        {
            if (checkamdgpu.Checked&&checknvgpu .Checked) { checkamdgpu.Checked = false; }
            if (checknvgpu.Checked) { gpusrbox.Text = "GT"; }
            else { gpusrbox.Text = ""; }

        }

        private void checkamdgpu_CheckedChanged(object sender, EventArgs e)
        {
            if (checknvgpu.Checked&&checkamdgpu.Checked ) { checknvgpu.Checked = false; }
            if (checkamdgpu.Checked) { gpusrbox.Text = "HD|R5|R7|R9"; } else { gpusrbox.Text = ""; }
        }

        private void hezhuangcpu_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxe3.CheckState == CheckState.Checked && hezhuangcpu.Checked) { MessageBox.Show("E3 CPU无盒装"); hezhuangcpu .Checked   = false; }
            if (hezhuangcpu.Checked) { cpusr2 = "盒"; } else { cpusr2 = ""; }
            
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Point a =new Point (groupBox3 .Size.Width -43,0);
            label31.Location =a ;
        }
        string CreateSpace (int num)
        {
            string space="";
            for (int i=1;i<=num;i++)
            {
                space += " ";
            }
            return space;
        }

        private void checkBoxhtpc_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxhtpc.Checked) { mbsrbox.Text = "MATX"; boxsrbox.Text = "HTPC"; }
            else { mbsrbox.Text = ""; boxsrbox.Text = ""; }
        }
    }
}
