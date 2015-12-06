
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
namespace 攒机助手
{
    public partial class Form1 : Form
    {
        PrintDocument pdDocument = new PrintDocument();

        string pricedate = "";
        long sum;
        long difference;
        bool needupdate = false;
        //string cpusr2 = "";
        Thread threadUpdate;
        Thread threadReport;
        Thread threadShowAd;
        Thread threadPriceUpdate;
        String adlink;
        private readonly String releaseurl = "http://bbs.luobotou.org/app/cjzs.txt";
        delegate void set_Text(string s); //定义委托
        set_Text Set_Text;
        SettingFile sf = new SettingFile();

        HardWareKind hwCpu;
        HardWareKind hwMb;
        HardWareKind hwRam;
        HardWareKind hwHdd;
        HardWareKind hwSsd;
        HardWareKind hwGpu;
        HardWareKind hwLcd;
        HardWareKind hwBox;
        HardWareKind hwFan;
        HardWareKind hwPower;
        HardWareKind hwCdrom;
        HardWareKind hwKb;
        List<HardWareKind> allHwKind = new List<HardWareKind>();


        public Form1()
        {
            InitializeComponent();
            InitailizeHWKind();

            allHwKind.Add(hwCpu);
            allHwKind.Add(hwMb);
            allHwKind.Add(hwRam);
            allHwKind.Add(hwHdd);
            allHwKind.Add(hwSsd);
            allHwKind.Add(hwGpu);
            allHwKind.Add(hwLcd);
            allHwKind.Add(hwBox);
            allHwKind.Add(hwFan);
            allHwKind.Add(hwPower);
            allHwKind.Add(hwCdrom);
            allHwKind.Add(hwKb);

        }

        private void InitailizeHWKind()
        {
            hwCpu = new HardWareKind("CPU", "CPU", comboBoxcpu, textBoxcpu, textBoxcpu1, textBoxcpusr);
            hwMb = new HardWareKind("主板", "MB", comboBoxmb, textBoxmb, textBoxmb1, textBoxmbsr);
            hwRam = new HardWareKind("内存", "RAM", comboBoxram, textBoxram, textBoxram1, textBoxramsr);
            hwHdd = new HardWareKind("硬盘", "HDD", comboBoxhdd, textBoxhdd, textBoxhdd1, textBoxhddsr);
            hwSsd = new HardWareKind("固态硬盘", "SSD", comboBoxssd, textBoxssd, textBoxssd1, textBoxssdsr);
            hwGpu = new HardWareKind("显卡", "GPU", comboBoxgpu, textBoxgpu, textBoxgpu1, textBoxgpusr);
            hwLcd = new HardWareKind("显示器", "LCD", comboBoxlcd, textBoxlcd, textBoxlcd1, textBoxlcdsr);
            hwBox = new HardWareKind("机箱", "BOX", comboBoxbox, textBoxbox, textBoxbox1, textBoxboxsr);
            hwFan = new HardWareKind("散热器", "FAN", comboBoxfan, textBoxfan, textBoxfan1, textBoxfansr);
            hwPower = new HardWareKind("电源", "POW", comboBoxpower, textBoxpower, textBoxpower1, textBoxpowersr);
            hwCdrom = new HardWareKind("光驱", "CDROM", comboBoxcdrom, textBoxcdrom, textBoxcdrom1, textBoxcdromsr);
            hwKb = new HardWareKind("键鼠", "KB", comboBoxkb, textBoxkb, textBoxkb1, textBoxkbsr);
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
            pdDocument.PrintPage += new PrintPageEventHandler(OnPrintPage);
            Set_Text = new set_Text(set_textboxText);
            if (!File.Exists(Application.StartupPath + "\\config.ini")) { MessageBox.Show("配置文件错误！config.ini不存在！"); Application.Exit(); }
            this.Text = "攒机助手免费版 " + Application.ProductVersion;

            LoadConfigFile();


            threadReport = new Thread(report);
            threadReport.Start();
            threadShowAd = new Thread(ad);
            threadShowAd.Start();
            if (sf.GetValue("CheckUpdateOnStartUp") != "0" && Assembly.GetExecutingAssembly().GetName().Version.Revision != 0)
            {
                threadUpdate = new Thread(update);
                threadUpdate.Start();
            }

        }

        private void LoadConfigFile()
        {
            try
            {        ////
                StreamReader objReader = new StreamReader(Application.StartupPath + "\\config.ini");
                string sLine = objReader.ReadLine();

                if (sLine.Contains("#"))
                {
                    Console.WriteLine(sLine);
                    pricedate = sLine.Substring(1);
                    labeldate.Text = "价格更新日期：" + pricedate;
                    if (!string.IsNullOrEmpty(pricedate))
                    {
                        needupdate = true;
                    }
                    sLine = objReader.ReadLine();
                }

                foreach (var item in allHwKind)
                {
                    item.HW.Add(new HardWare("自动选择", 0));
                }
                hwGpu.HW.Add(new HardWare("核心显卡", 0));
                hwPower.HW.Add(new HardWare("机箱自带", 0));
                hwFan.HW.Add(new HardWare("盒装自带", 0));

                while (sLine != null)
                {
                    if (sLine.Contains("[END]")) break;

                    //Console.WriteLine(sLine); ;
                    foreach (var item in allHwKind)
                    {
                        if (sLine.Contains("[" + item.KindNameShort + "]"))
                        {
                            Console.WriteLine(sLine);
                            sLine = objReader.ReadLine();
                            while (!sLine.Contains("["))
                            {
                                string hwname = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                                int hwprice = Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                                HardWare hw = new HardWare(hwname, hwprice);
                                item.HW.Add(hw);
                                sLine = objReader.ReadLine();
                            }
                            item.AddToComboBox(item.HW);
                            item.CBB.SelectedIndex = 0;
                        }
                        //Console.WriteLine(sLine);
                    }


                }
                objReader.Close();

                if (needupdate)
                {
                    threadPriceUpdate = new Thread(configdownload);
                    threadPriceUpdate.Start();
                }
                ////
                //hwCpu.HWChoice.Name = "";


            }
            catch (Exception err)
            {
                //throw err;
                MessageBox.Show("配置文件存在错误!\n" + err.ToString());
            }
        }
        private void 启动时自动检查更新ToolStripMenuItem_Checked(object sender, EventArgs e)
        {

            if (启动时自动检查更新ToolStripMenuItem.Checked)
            {
                sf.SetValue("CheckUpdateOnStartup", "1");
                //启动时自动检查更新ToolStripMenuItem.Checked = false;
                //WTRegedit("nevercheckupdate", "0");
            }

            if (!启动时自动检查更新ToolStripMenuItem.Checked)
            {
                sf.SetValue("CheckUpdateOnStartup", "0");
                //启动时自动检查更新ToolStripMenuItem.Checked = true;
                //WTRegedit("nevercheckupdate", "1");
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TextBox1.Text.Trim()))
            {
                MessageBox.Show("请输入预算金额！"); return;
            }
            foreach (var item in allHwKind)
            {
                item.TxtName.Text = string.Empty;
                item.TxtPrice.Text = string.Empty;
            }

            int money;
            int realmoney = 0;
            difference = 0;
            sum = 0;
            foreach (var item in allHwKind)
            {

                item.HWChoice = null;


            }
            //choice = null;
            realmoney = int.Parse(TextBox1.Text);
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

            #region CoreAutoChoose
            ////////////////////////////////////////////////////////
            //CPU


            hwCpu.AutoChooseHardWare(() =>
            {
                HardWare choice = null;
                List<string> incpart = new List<string>();
                List<string> notincpart = new List<string>();

                //checkboxedcpu.Checked
                if (checkboxedcpu.Checked)
                {
                    incpart.Add("盒");
                }
                notincpart.Add("*");
                if (!checkBoxamd.Checked)
                {
                    incpart.Add("Intel");
                    //incpart.Add(cpusr2);

                    if (checkBoxcoregpu.Checked) { notincpart.Add("E3"); }
                    if (money <= 2500)
                    {
                        if (checkBoxcpu.Checked)
                        {
                            choice = AutoChooseHW(hwCpu, money / 5, incpart, notincpart, hwCpu.HWFilter);
                        }
                        else
                        {
                            choice = AutoChooseHW(hwCpu, money / 6, incpart, notincpart, hwCpu.HWFilter);
                        }
                    }
                    else if (money > 2500 && money <= 5500)
                    {
                        if (checkBoxcpu.Checked || textBoxcpusr.Text == "K")
                        {
                            choice = AutoChooseHW(hwCpu, money / 4, incpart, notincpart, hwCpu.HWFilter);
                        }
                        else
                        {
                            notincpart.Add("K");
                            choice = AutoChooseHW(hwCpu, money / 5, incpart, notincpart, hwCpu.HWFilter);
                        }
                    }
                    else if (money > 5500 && money <= 9000)
                    {
                        if (checkBoxcpu.Checked || textBoxcpusr.Text == "K")
                        {
                            choice = AutoChooseHW(hwCpu, money / 3, incpart, notincpart, hwCpu.HWFilter);
                        }
                        else
                        {
                            notincpart.Add("K");
                            choice = AutoChooseHW(hwCpu, money / 4, incpart, notincpart, hwCpu.HWFilter);
                        }

                    }
                    else
                    {
                        if (checkBoxcpu.Checked)
                        {
                            choice = AutoChooseHW(hwCpu, money / 3, incpart, notincpart, hwCpu.HWFilter);
                        }
                        else
                        {
                            choice = AutoChooseHW(hwCpu, (int)(((float)money) / 3.5), incpart, notincpart, hwCpu.HWFilter);
                        }
                    }


                }
                else
                {
                    incpart.Add("AMD");
                    //List<string> incpart = new List<string>();
                    //incpart.Add(cpusr2);
                    //List<string> notincpart = new List<string>();
                    //notincpart.Add("*");
                    if (checkBoxcoregpu.Checked) { notincpart.Add("速龙"); notincpart.Add("羿龙"); }
                    if (checkBoxhtpc.Checked) { notincpart.Add("FX"); }

                    if (checkBoxcpu.Checked)
                    {
                        choice = AutoChooseHW(hwCpu, money / 5, incpart, notincpart, hwCpu.HWFilter);
                    }
                    else
                    {
                        choice = AutoChooseHW(hwCpu, money / 6, incpart, notincpart, hwCpu.HWFilter);
                    }
                }

                return choice;
            });


            ////////////////////////////////////////////////////////////
            //主板

            hwMb.AutoChooseHardWare(() =>
            {
                int ivb = 0;
                HardWare mbchoice = null;
                if (!hwCpu.HWChoice.Name.Contains("AMD"))
                {

                    if (hwCpu.HWChoice.Name.Contains("赛扬") || hwCpu.HWChoice.Name.Contains("奔腾"))
                    {
                        if (Regex.IsMatch(hwCpu.HWChoice.Name, @"1820|1830|1840|1850"))
                        {
                            mbchoice = AutoChooseHW(hwMb, 300, "H81");
                            //mbchoice = AutoChooseHW(hwMb, 300, "H81");
                            if (mbchoice == null)
                            {
                                mbchoice = AutoChooseHW(hwMb, 300, "B85");
                            }
                        }
                        else if (Regex.IsMatch(hwCpu.HWChoice.Name, @"3900|3920"))
                        {
                            mbchoice = AutoChooseHW(hwMb, 300, "B150");
                            //mbchoice = AutoChooseHW(hwMb, 300, "H81");
                            if (mbchoice == null)
                            {
                                mbchoice = AutoChooseHW(hwMb, 300, "H110");
                            }

                        }
                        else if (hwCpu.HWChoice.Name.Contains("1610") || hwCpu.HWChoice.Name.Contains("1620") || hwCpu.HWChoice.Name.Contains("1630"))
                        {
                            mbchoice = AutoChooseHW(hwMb, 300, "H61");
                            //mbchoice = AutoChooseHW(hwMb, 300, "H81");
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 300, "B75"); }

                            //mbchoice = AutoChooseHW(hwMb, 300, "H61");
                            //if (mbchoice == 0 || mbchoice == -1) { mbchoice = AutoChooseHW(hwMb, 300, "B75"); }
                        }
                        //////下面奔腾
                        else if (Regex.IsMatch(hwCpu.HWChoice.Name, @"3900|3920"))
                        {
                            mbchoice = AutoChooseHW(hwMb, 300, "B150");
                            if (mbchoice == null)
                            {
                                mbchoice = AutoChooseHW(hwMb, 300, "H110");
                            }
                            if (mbchoice == null)
                            {
                                mbchoice = AutoChooseHW(hwMb, 300, "H150");
                            }
                        }
                        //else if (hwCpu.HWChoice.Name.Contains("3220") || hwCpu.HWChoice.Name.Contains("3230") || hwCpu.HWChoice.Name.Contains("3240") || hwCpu.HWChoice.Name.Contains("3420"))
                        else if (Regex.IsMatch(hwCpu.HWChoice.Name, @"3220|3230|3240|3420|3250|3260|3460|3450|3430"))
                        {
                            mbchoice = AutoChooseHW(hwMb, 300, "H81");
                            //mbchoice = AutoChooseHW(hwMb, 300, "H81");
                            if (mbchoice == null)
                            {
                                mbchoice = AutoChooseHW(hwMb, 350, "B85");
                                //mbchoice = AutoChooseHW(hwMb, 350, "B85"); 
                            }
                            if (mbchoice == null)
                            {
                                mbchoice = AutoChooseHW(hwMb, 400, "H87");
                                //mbchoice = AutoChooseHW(hwMb, 400, "H87");
                            }

                        }
                        else if (hwCpu.HWChoice.Name.Contains("2010") || hwCpu.HWChoice.Name.Contains("2020") || hwCpu.HWChoice.Name.Contains("2100") || hwCpu.HWChoice.Name.Contains("2120") || hwCpu.HWChoice.Name.Contains("2130"))
                        {
                            mbchoice = AutoChooseHW(hwMb, 350, "H61");

                            //mbchoice = AutoChooseHW(hwMb, 350, "H61");
                            //if (mbchoice == 0 || mbchoice == -1) { mbchoice = AutoChooseHW(hwMb, 300, "B75"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 300, "B75"); }
                        }
                        else if (hwCpu.HWChoice.Name.Contains("3258"))
                        {
                            mbchoice = AutoChooseHW(hwMb, 600, "Z97");

                        }
                        else { mbchoice = AutoChooseHW(hwMb, 300, "H61"); }

                        // mbchoice = 1;
                    }
                    else if (hwCpu.HWChoice.Name.Contains("i3"))
                    {
                        ivb = hwCpu.HWChoice.Name.IndexOf("3");
                        if (hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "32")
                        {
                            mbchoice = AutoChooseHW(hwMb, 500, "B75");
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 500, "H77"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 500, "H61"); }
                        }
                        else if (hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "41" || hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "43")
                        {
                            if (checkBoxcpu.Checked || checkBoxgpu.Checked) { mbchoice = AutoChooseHW(hwMb, 450, "H81"); }
                            else { mbchoice = AutoChooseHW(hwMb, 500, "B85"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 500, "H81"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 500, "H87"); }
                        }
                        else if (hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "61" || hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "63")
                        {
                            if (checkBoxcpu.Checked || checkBoxgpu.Checked) { mbchoice = AutoChooseHW(hwMb, 450, "H110"); }
                            else { mbchoice = AutoChooseHW(hwMb, 500, "B150"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 500, "H110"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 500, "H170"); }
                        }

                        else { mbchoice = AutoChooseHW(hwMb, 400, "H61"); }
                        //mbchoice = 2;
                    }
                    else if (hwCpu.HWChoice.Name.Contains("i5") && !hwCpu.HWChoice.Name.Contains("K") && !hwCpu.HWChoice.Name.Contains("X"))  //I5 !K !X
                    {
                        ivb = hwCpu.HWChoice.Name.IndexOf("5");

                        if (hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "34" || hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "35" || hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "33")
                        {
                            if (checkBoxcpu.Checked || checkBoxgpu.Checked) { mbchoice = AutoChooseHW(hwMb, 500, "B75"); }
                            else { mbchoice = AutoChooseHW(hwMb, 600, "B75"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 600, "H77"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 600, "Z77"); }
                        }
                        else if (hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "44" || hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "45" || hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "46")
                        {
                            if (checkBoxcpu.Checked || checkBoxgpu.Checked) { mbchoice = AutoChooseHW(hwMb, 500, "B85"); }
                            else { mbchoice = AutoChooseHW(hwMb, 600, "B85"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 600, "H87"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 600, "H81"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 600, "Z87"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 800, "Z97"); }

                        }
                        else if (hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "64" || hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "65" || hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "66")
                        {
                            if (checkBoxcpu.Checked || checkBoxgpu.Checked) { mbchoice = AutoChooseHW(hwMb, 500, "B150"); }
                            else { mbchoice = AutoChooseHW(hwMb, 600, "B150"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 600, "H170"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 600, "H110"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 800, "Z170"); }

                        }
                        else //2 gen i5
                        {
                            if (checkBoxcpu.Checked) { mbchoice = AutoChooseHW(hwMb, 500, "H61"); }
                            else { mbchoice = AutoChooseHW(hwMb, 700, "H67"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 500, "H61"); }
                        }
                        //mbchoice = 3;
                    }
                    else if (hwCpu.HWChoice.Name.Contains("i7") && !hwCpu.HWChoice.Name.Contains("K") && !hwCpu.HWChoice.Name.Contains("X"))
                    {
                        //i7 !X !K
                        // int ivb = 0;
                        ivb = hwCpu.HWChoice.Name.IndexOf("7");
                        if (hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "38") { mbchoice = AutoChooseHW(hwMb, 1500, "X79"); }
                        else if (hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "37") //3 gen i7
                        {
                            mbchoice = AutoChooseHW(hwMb, 800, "B75");
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 800, "H77"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 600, "Z77"); }
                        }
                        else if (hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "47") //4 gen i7
                        {
                            if (checkBoxcpu.Checked || checkBoxgpu.Checked) { mbchoice = AutoChooseHW(hwMb, 650, "B85"); }

                            else { mbchoice = AutoChooseHW(hwMb, 750, "B85"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 800, "H87"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 600, "H81"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 800, "Z87"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 800, "Z97"); }

                        }
                        else if (hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "67") //6 gen i7
                        {
                            if (checkBoxcpu.Checked || checkBoxgpu.Checked) { mbchoice = AutoChooseHW(hwMb, 650, "B150"); }

                            else { mbchoice = AutoChooseHW(hwMb, 750, "B150"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 800, "H170"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 600, "H110"); }
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 800, "Z170"); }

                        }
                        //2 gen i7
                        else if (checkBoxcpu.Checked) { mbchoice = AutoChooseHW(hwMb, 700, "H67"); }
                        else { mbchoice = AutoChooseHW(hwMb, 1000, "H67"); }
                    }
                    else if (hwCpu.HWChoice.Name.Contains("i7") && hwCpu.HWChoice.Name.Contains("K"))//i7 K
                    {
                        //  int ivb = 0;
                        ivb = hwCpu.HWChoice.Name.IndexOf("7");
                        if (hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "48" || hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "49" || hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "39")
                        {
                            mbchoice = AutoChooseHW(hwMb, 1500, "X79");
                        }
                        else if (hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "37") // 3 gen i7 k
                        {
                            mbchoice = AutoChooseHW(hwMb, 1000, "Z77");
                        }
                        else if (hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "47")  //4 gen i7 k
                        {
                            if (money < 10000)
                            {
                                if (hwCpu.HWChoice.Name.Contains("4770K"))
                                {
                                    mbchoice = AutoChooseHW(hwMb, 1000, "Z87");
                                    if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 1000, "Z97"); }
                                }
                                else
                                {
                                    mbchoice = AutoChooseHW(hwMb, 1000, "Z97");
                                }
                            }

                            else if (checkBoxcpu.Checked) //2 gen i7 k
                            {
                                mbchoice = AutoChooseHW(hwMb, 1000, "Z68");
                            }
                            else
                            {
                                mbchoice = AutoChooseHW(hwMb, 1500, "Z68");
                            }
                        }
                        else if (hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "67")  //6 gen i7 k
                        {


                            mbchoice = AutoChooseHW(hwMb, 1500, "Z170");

                        }
                        //else if (choice == 6) { mbchoice = 7; }
                        else if (hwCpu.HWChoice.Name.Contains("980X") || hwCpu.HWChoice.Name.Contains("990X") || hwCpu.HWChoice.Name.Contains("995X"))
                        {
                            mbchoice = AutoChooseHW(hwMb, 4000, "X58");
                        }
                        else if (hwCpu.HWChoice.Name.Contains("3960X") || hwCpu.HWChoice.Name.Contains("3970X") || hwCpu.HWChoice.Name.Contains("4960X") || hwCpu.HWChoice.Name.Contains("4970X"))
                        {
                            mbchoice = AutoChooseHW(hwMb, 3600, "X79");
                        }
                    }
                    else if (hwCpu.HWChoice.Name.Contains("i5") && hwCpu.HWChoice.Name.Contains("K")) //I5 K
                    {
                        //  int ivb = 0;
                        ivb = hwCpu.HWChoice.Name.IndexOf("5");
                        if (hwCpu.HWChoice.Name.Substring(ivb + 2, 2) == "35") // 3 gen i5 K
                        {
                            mbchoice = AutoChooseHW(hwMb, 800, "Z77");
                        }
                        else if (hwCpu.HWChoice.Name.Contains("4670K")) // 4 gen i5 k
                        {

                            mbchoice = AutoChooseHW(hwMb, 800, "Z87");
                            if (mbchoice == null) { mbchoice = AutoChooseHW(hwMb, 800, "Z97"); }

                        }
                        else if (hwCpu.HWChoice.Name.Contains("6600K")) //6 gen i5 k
                        {

                            mbchoice = AutoChooseHW(hwMb, 800, "Z170");

                        }
                        else if (hwCpu.HWChoice.Name.Contains("4690K"))
                        {
                            mbchoice = AutoChooseHW(hwMb, 800, "Z97");
                        }
                        else // 2 gen i5 k
                        {
                            mbchoice = AutoChooseHW(hwMb, 800, "Z68");
                        }
                    }
                    else if (hwCpu.HWChoice.Name.Contains("E3") && (hwCpu.HWChoice.Name.Contains("V2") || hwCpu.HWChoice.Name.Contains("v2"))) //E3 V2
                    {
                        mbchoice = AutoChooseHW(hwMb, 600, "B75");
                    }
                    else if (hwCpu.HWChoice.Name.Contains("E3") && (hwCpu.HWChoice.Name.Contains("V3") || hwCpu.HWChoice.Name.Contains("v3"))) //E3 V3
                    {
                        mbchoice = AutoChooseHW(hwMb, 700, "B85");
                    }

                    else { mbchoice = null; }
                        //}
                    
                }
                else
                {
                    //if (comboBoxmb.SelectedIndex != 0)
                    //{
                    //    mbchoice = comboBoxmb.SelectedItem as HardWare;
                    //}
                    //else
                    //{
                    if (hwCpu.HWChoice.Name.Contains("羿龙"))
                    {
                        mbchoice = AutoChooseHW(hwMb, 800, "970");
                    }
                    else if (hwCpu.HWChoice.Name.Contains("FX"))
                    {
                        if (hwCpu.HWChoice.Name.Contains("4")) { mbchoice = AutoChooseHW(hwMb, 400, "AM3+"); }
                        if (hwCpu.HWChoice.Name.Contains("6")) { mbchoice = AutoChooseHW(hwMb, 600, "AM3+"); }
                        else { mbchoice = AutoChooseHW(hwMb, 800, "AM3+"); }
                    }
                    else if (hwCpu.HWChoice.Name.Contains("5600K") || hwCpu.HWChoice.Name.Contains("5500") || hwCpu.HWChoice.Name.Contains("6600K") || hwCpu.HWChoice.Name.Contains("6800K") || hwCpu.HWChoice.Name.Contains("5700") || hwCpu.HWChoice.Name.Contains("5800K") || hwCpu.HWChoice.Name.Contains("760K") || hwCpu.HWChoice.Name.Contains("6600K") || hwCpu.HWChoice.Name.Contains("6400K") || hwCpu.HWChoice.Name.Contains("740") || hwCpu.HWChoice.Name.Contains("750K") || hwCpu.HWChoice.Name.Contains("5400") || hwCpu.HWChoice.Name.Contains("5300") || hwCpu.HWChoice.Name.Contains("4000"))
                    {
                        mbchoice = AutoChooseHW(hwMb, 400, "FM2");
                    }
                    else if (hwCpu.HWChoice.Name.Contains("3850") || hwCpu.HWChoice.Name.Contains("3870K") || hwCpu.HWChoice.Name.Contains("3800") || hwCpu.HWChoice.Name.Contains("3820") || hwCpu.HWChoice.Name.Contains("638") || hwCpu.HWChoice.Name.Contains("641") || hwCpu.HWChoice.Name.Contains("631") || hwCpu.HWChoice.Name.Contains("651") || hwCpu.HWChoice.Name.Contains("3650") || hwCpu.HWChoice.Name.Contains("3500") || hwCpu.HWChoice.Name.Contains("3670K") || hwCpu.HWChoice.Name.Contains("3400") || hwCpu.HWChoice.Name.Contains("3300"))
                    {
                        mbchoice = AutoChooseHW(hwMb, 400, "FM1");
                    }
                    else if (hwCpu.HWChoice.Name.Contains("7700K") || hwCpu.HWChoice.Name.Contains("7850K"))
                    {
                        mbchoice = AutoChooseHW(hwMb, 600, "FM2+");
                    }
                    else { mbchoice = null; }
                    //}

                }
                //MessageBox.Show(mbchoice .Name );
                return mbchoice;

            });

            /////////////////////////////////////////////////////////////
            //机箱
            if (checkBoxbox.Checked)
            {
                hwBox.AutoChooseHardWare(() =>
                {
                    HardWare choice = null;

                    if (money <= 3000)
                    {
                        choice = AutoChooseHW(hwBox, 80);
                    }
                    else if (money > 3000 && money <= 6000)
                    {
                        choice = AutoChooseHW(hwBox, 100);
                    }
                    else if (money > 6000 && money <= 9000)
                    {
                        choice = AutoChooseHW(hwBox, 150);
                    }
                    else
                    {
                        choice = AutoChooseHW(hwBox, 200);
                    }
                    return choice;
                });

            }
            //////////
            int boxLength = 1000;
            int fanHeight = 1000;

            if (checkBoxhtpc.Checked)
            {
                //MessageBox.Show(box[0]);
                boxLength = Int32.Parse(hwBox.HWChoice.Name.Substring(hwBox.HWChoice.Name.IndexOf("GPU=") + 4, 3));
                fanHeight = Int32.Parse(hwBox.HWChoice.Name.Substring(hwBox.HWChoice.Name.IndexOf("FAN=") + 4, 3));
            }

            ////////////////////////////////////////////////////////
            //显卡
            //总价的1/5


            if (CheckBoxlcd.Checked)
            {
                hwGpu.AutoChooseHardWare(() =>
                {
                    HardWare choice = null;

                    if (checkBoxcoregpu.Checked || (money < 4000 && !checkBoxgpu.Checked && !hwCpu.HWChoice.Name.Contains("E3") && !hwCpu.HWChoice.Name.Contains("速龙") && !hwCpu.HWChoice.Name.Contains("X")))
                    {
                        choice = hwGpu.HW[1];
                    }
                    else if (money < 4000)
                    {
                        choice = AutoChooseHW(hwGpu, money / 5, string.Empty, "*", (x) =>
                        {
                            if (x.Contains("L="))
                            {
                                return int.Parse(x.Substring(x.IndexOf("L=") + 2, 3)) < boxLength;
                            }
                            else
                            {
                                return !checkBoxhtpc.Checked;
                            }
                        });
                        //choice = closest(gpu1, money / 5);
                    }
                    else if (money >= 4000 && money <= 5500 && !checkBoxgpu.Checked)
                    {
                        choice = AutoChooseHW(hwGpu, money / 5, string.Empty, "*", (x) =>
                        {
                            if (x.Contains("L="))
                            {
                                return int.Parse(x.Substring(x.IndexOf("L=") + 2, 3)) < boxLength;
                            }
                            else
                            {
                                return !checkBoxhtpc.Checked;
                            }
                        });


                        //choice = AutoChooseHW(hwGpu, money / 5, "", gpusr, "*", boxlength);
                        //choice = closest(gpu1, money / 5); 
                    }
                    else if (money >= 4000 && money <= 5500 && checkBoxgpu.Checked)
                    {
                        choice = AutoChooseHW(hwGpu, money / 4, string.Empty, "*", (x) =>
                        {
                            if (x.Contains("L="))
                            {
                                return int.Parse(x.Substring(x.IndexOf("L=") + 2, 3)) < boxLength;
                            }
                            else
                            {
                                return !checkBoxhtpc.Checked;
                            }
                        });

                        //choice = AutoChooseHW(hwGpu, money / 4, "", gpusr, "*", boxlength);
                        //   choice = closest(gpu1, money / 4); 
                    }
                    else if (money > 5500 && money <= 7500 && !checkBoxgpu.Checked)
                    {
                        choice = AutoChooseHW(hwGpu, money / 4, string.Empty, "*", (x) =>
                        {
                            if (x.Contains("L="))
                            {
                                return int.Parse(x.Substring(x.IndexOf("L=") + 2, 3)) < boxLength;
                            }
                            else
                            {
                                return !checkBoxhtpc.Checked;
                            }
                        });

                        //choice = AutoChooseHW(hwGpu, money / 4, "", gpusr, "*", boxlength);
                    }
                    else if (money > 5500 && money <= 7500 && checkBoxgpu.Checked)
                    {
                        choice = AutoChooseHW(hwGpu, (int)(((float)money) / 3.5), string.Empty, "*", (x) =>
                        {
                            if (x.Contains("L="))
                            {
                                return int.Parse(x.Substring(x.IndexOf("L=") + 2, 3)) < boxLength;
                            }
                            else
                            {
                                return !checkBoxhtpc.Checked;
                            }
                        });

                        //choice = AutoChooseHW(hwGpu, (int)(((float)money) / 3.5), "", gpusr, "*", boxlength);

                    }
                    else if (money > 7500 && !checkBoxgpu.Checked)
                    {
                        choice = AutoChooseHW(hwGpu, (int)(((float)money) / 3.5), string.Empty, "*", (x) =>
                        {
                            if (x.Contains("L="))
                            {
                                return int.Parse(x.Substring(x.IndexOf("L=") + 2, 3)) < boxLength;
                            }
                            else
                            {
                                return !checkBoxhtpc.Checked;
                            }
                        });

                        //choice = AutoChooseHW(hwGpu, (int)(((float)money) / 3.5), "", gpusr, "*", boxlength);
                    }
                    else if (money > 7500 && checkBoxgpu.Checked)
                    {
                        choice = AutoChooseHW(hwGpu, money / 3, string.Empty, "*", (x) =>
                        {
                            if (x.Contains("L="))
                            {
                                return int.Parse(x.Substring(x.IndexOf("L=") + 2, 3)) < boxLength;
                            }
                            else
                            {
                                return !checkBoxhtpc.Checked;
                            }
                        });

                        //choice = AutoChooseHW(hwGpu, money / 3, "", gpusr, "*", boxlength);
                    }
                    return choice;
                });


            }
            ///////////////////////////////////////////////////////////////
            //内存
            hwRam.AutoChooseHardWare(() =>
                {
                    HardWare choice = null;
                    string incPart = "DDR3";
                    if (Regex.IsMatch(hwMb.HWChoice.Name, @"Z170|B150|H110|H170"))
                    {
                        incPart = "DDR4";
                    }
                    if (money < 4000)
                    {
                        choice = AutoChooseHW(hwRam, 200, incPart);
                    }
                    else if (money >= 4000 && money <= 9000)
                    {
                        choice = AutoChooseHW(hwRam, 300, incPart);
                    }
                    else { choice = AutoChooseHW(hwRam, 500, incPart); }
                    return choice;
                });

            ////////////////////////////////////////////////////////////////
            ////硬盘 HDD
            if (checkBoxhdd.Checked)
            {


                hwHdd.AutoChooseHardWare(() =>
                {
                    HardWare choice = null;

                    if (money <= 2000)
                    {
                        choice = AutoChooseHW(hwHdd, 250);
                    }
                    else if (money > 2000 && money <= 6000)
                    {
                        choice = AutoChooseHW(hwHdd, 300);
                    }
                    else if (money > 6000 && money <= 10000)
                    {
                        choice = AutoChooseHW(hwHdd, 400);
                    }
                    else { choice = AutoChooseHW(hwHdd, 500); }
                    return choice;
                });
            }
            //////////////////////////////////////////////////////////////////////
            ////SSD %14 of all
            if (checkBoxssd.Checked)
            {
                hwSsd.AutoChooseHardWare(() =>
                {
                    HardWare choice = null;

                    if (money <= 5000)
                    {
                        choice = AutoChooseHW(hwSsd, (int)(((float)realmoney) * 0.1));
                    }
                    else
                    {
                        choice = AutoChooseHW(hwSsd, (int)(((float)realmoney) * 0.08));

                    }
                    return choice;
                });
            }

            //////////////////////////////////////////////////////////////
            ////散热器



            if (checkBoxfan.Checked)
            {
                hwFan.AutoChooseHardWare(() =>
                {
                    HardWare choice = null;
                    if (hwCpu.HWChoice.Name.Contains("散") || hwCpu.HWChoice.Name.Contains("E3") || (hwCpu.HWChoice.Name.Contains("K") && !hwCpu.HWChoice.Name.Contains("AMD")))
                    {
                        if (hwCpu.HWChoice.Name.Contains("K"))
                        {
                            choice = AutoChooseHW(hwFan, hwCpu.HWChoice.Price / 12, string.Empty, "*", x => Int32.Parse(x.Substring(x.IndexOf("H=") + 2, 3)) < fanHeight);
                        }
                        else
                        {
                            choice = AutoChooseHW(hwFan, hwCpu.HWChoice.Price / 20, string.Empty, "*", x => Int32.Parse(x.Substring(x.IndexOf("H=") + 2, 3)) < fanHeight);


                        }
                    }
                    else if (hwMb.HWChoice.Name.Contains("X79"))
                    {
                        choice = AutoChooseHW(hwFan, 300, "LGA2011", "*", x => Int32.Parse(x.Substring(x.IndexOf("H=") + 2, 3)) < fanHeight);
                    }
                    else choice = hwFan.HW[1];
                    return choice;
                });
            }

            //////////////////////////////////////////////////////////////////
            ////电源。
            if (checkBoxpower.Checked)
            {

                hwPower.AutoChooseHardWare(() =>
                {
                    HardWare choice = null;
                    choice = AutoChooseHW(hwPower, money / 13);
                    if (choice == null) { hwPower.HWChoice = new HardWare("没有找到合适的散热器", 0); }
                    else
                    {
                        hwPower.HWChoice = choice;
                    }
                    return choice;
                });

            }
            ////////////////////////////////
            ////CDROM,100
            if (checkBoxcdrom.Checked)
            {
                //HardWare choice = null;

                if (hwBox.HWChoice.Name.Contains("无光驱位"))
                {
                    hwCdrom.HWChoice = new HardWare("机箱无光驱位", 0);
                }
                else
                {
                    hwCdrom.AutoChooseHardWare(() =>
                    {

                        HardWare choice = null;
                        choice = AutoChooseHW(hwCdrom, money / 30);

                        return choice;
                    });

                }
            }
            /////////////////////////////////////////////////////////
            //Keyboard
            //if (kb1[1] != 0)
            //{
            if (checkBoxkb.Checked)
            {

                hwKb.AutoChooseHardWare(() =>
                {
                    HardWare choice = null;

                    choice = AutoChooseHW(hwKb, money / 30);
                    return choice;
                });


            }
            //}
            int rest = realmoney;
            foreach (var item in allHwKind)
            {
                if (item == hwLcd)
                {
                    continue;
                }
                if (item.HWChoice == null) continue;
                rest -= item.HWChoice.Price;
                //Console.WriteLine(item.HWChoice.Name + item.HWChoice.Price);
            }
            //rest = realmoney - (hwCpu.HWChoice .Price  + mb1[0] + gpu1[0] + ram1[0] + hdd1[0] + box1[0] + fan1[0] + power1[0] + cdrom1[0] + kb1[0] + ssd1[0]);
            ////////////////////////////////////////////////////////////////////
            ////显示器
            ////根据余额选择
            if (CheckBoxlcd.Checked)
            {
                hwLcd.AutoChooseHardWare(() =>
                {
                    HardWare choice = null;
                    choice = AutoChooseHW(hwLcd, rest);
                    return choice;
                });

            }
            else//进行显卡余额配置
            {
                HardWare choice = null;

                if (comboBoxgpu.SelectedIndex != 0)
                {
                    choice = comboBoxgpu.SelectedItem as HardWare;
                }
                else
                {
                    if (realmoney <= 3000 && !checkBoxgpu.Checked && !hwCpu.HWChoice.Name.Contains("E3") && !hwCpu.HWChoice.Name.Contains("速龙") && !hwCpu.HWChoice.Name.Contains("X"))
                    {
                        choice = hwGpu.HW[1];
                    }
                    else
                    {
                        choice = AutoChooseHW(hwGpu, rest, string.Empty, "*", (x) =>
                        {
                            if (x.Contains("L="))
                            {
                                return int.Parse(x.Substring(x.IndexOf("L=") + 2, 3)) < boxLength;
                            }
                            else
                            {
                                return !checkBoxhtpc.Checked;
                            }
                        });

                        //choice = AutoChooseHW(hwGpu, rest, "", gpusr, "*", boxlength);
                        //choice = closest(gpu1, rest); 
                    }

                }
                if (choice == null)
                {
                    choice = HardWareKind.NoResult;
                }
                hwGpu.HWChoice = choice;

            }
            #endregion
            /////////////////////////////
            foreach (var item in allHwKind)
            {
                if (item.HWChoice == null) continue;
                sum += item.HWChoice.Price;
            }
            difference = realmoney - sum;
            //sum = cpu1[0] + mb1[0] + gpu1[0] + ram1[0] + hdd1[0] + box1[0] + lcd1[0] + power1[0] + fan1[0] + cdrom1[0] + kb1[0] + ssd1[0];  //总价
            //difference = realmoney - sum;
            /////////////////////////////
            ////
            foreach (var item in allHwKind)
            {
                item.ShowHardWareToTextBox();
            }

            labelall.Text = "合计金额： " + sum + " 元";
            labelall.Visible = true;
            labelcopy.Visible = true;
            labelprint.Visible = true;
            //////显示提示//////
            if (!CheckBoxlcd.Checked && difference > 200) { labeltip.Text = "提示：可在右侧扩展面板勾选较高档CPU或较高档显卡，达到目标价格。"; }
            else if (hwGpu.HWChoice == HardWareKind.NoResult) { labeltip.Text = "提示：如果需要独立显卡，请在右侧扩展面板勾选较高档显卡！"; }
            else { labeltip.Text = "提示：本程序显示的配置单仅供参考，如需有疑问，请到论坛发帖。"; }
            //////////////////
            //退出ToolStripMenuItem1.Enabled = true;
        }
        private bool DetectHTPCGPULength(string x, int boxLength)
        {
            if (x.Contains("L="))
            {
                return int.Parse(x.Substring(x.IndexOf("L=") + 2, 3)) < boxLength;
            }
            else
            {
                return true;
            }
        }
        //private long AutoChooseHW(HardWareKind hwCpu, long p1, List<string> incpart, List<string> notincpart, string p2)
        //{
        //    throw new NotImplementedException();
        //}

        #region AutoChoose
        private HardWare AutoChooseHW(HardWareKind hwk, int paidMoney, string inculdePart, string regex)
        {
            return AutoChooseHW(hwk, paidMoney, new List<string> { inculdePart }, null, regex);
        }
        private HardWare AutoChooseHW(HardWareKind hwk, int paidMoney)
        {
            return AutoChooseHW(hwk, paidMoney, null, new List<string> { "*" }, hwk.HWFilter);
        }

        private HardWare AutoChooseHW(HardWareKind hwk, int paidMoney, string includedPart)
        {
            return AutoChooseHW(hwk, paidMoney, new List<string> { includedPart }, new List<string> { "*" }, hwk.HWFilter);
        }
        //private HardWare AutoChooseHW(HardWareKind hwk, int paidMoney, string includedPart, string notincludedPart)
        //{
        //    return AutoChooseHW(hwk, paidMoney, new List<string> { includedPart }, new List<string> { notincludedPart }, hwk.HWFilter);
        //}
        private HardWare AutoChooseHW(HardWareKind hwk, int paidMoney, string includedPart, string notincludedPart, Func<string, bool> func)
        {
            return AutoChooseHW(hwk, paidMoney, new List<string> { includedPart }, new List<string> { notincludedPart }, new List<Regex> { new Regex(hwk.HWFilter) }, func);
        }
        private HardWare AutoChooseHW(HardWareKind hwk, int paidMoney, IList<string> includedPart, string regex)
        {
            return AutoChooseHW(hwk, paidMoney, includedPart, null, regex);
        }
        private HardWare AutoChooseHW(HardWareKind hwk, int paidMoney, IList<string> includedPart)
        {
            return AutoChooseHW(hwk, paidMoney, includedPart, null, string.Empty);
        }
        private HardWare AutoChooseHW(HardWareKind hwk, int paidMoney, IList<string> includedPart, IList<string> notIncludedPart)
        {
            return AutoChooseHW(hwk, paidMoney, includedPart, notIncludedPart, string.Empty);
        }
        private HardWare AutoChooseHW(HardWareKind hwk, int paidMoney, IList<string> includedPart, IList<string> notIncludedPart, string regex)
        {
            return AutoChooseHW(hwk, paidMoney, includedPart, notIncludedPart, new List<Regex> { new Regex(regex) });
        }
        private HardWare AutoChooseHW(HardWareKind hwk, int paidMoney, IList<string> includedPart, IList<string> notIncludedPart, IList<Regex> regexes)
        {
            return AutoChooseHW(hwk, paidMoney, includedPart, notIncludedPart, regexes, null);
        }
        /// <summary>
        /// 返回符合要求的硬件
        /// </summary>
        /// <param name="hwk">硬件种类对象</param>
        /// <param name="paidMoney">预算</param>
        /// <param name="includedPart">包含部分</param>
        /// <param name="notIncludedPart">不包含部分</param>
        /// <param name="regex">正则表达式</param>
        /// <returns></returns>
        private HardWare AutoChooseHW(HardWareKind hwk, int paidMoney, IList<string> includedPart, IList<string> notIncludedPart, IList<Regex> regexes, Func<string, bool> func)
        {
            if (hwk.HW == null) return null;
            else
            {
                int choicenum = 0;//所有条件符合的第几个硬件
                int minsubtraction = 0;
                HardWare lastestReturn = null;
                int subtraction = 0;

                //Regex r = new Regex(regex);
                foreach (var item in hwk.HW)
                {
                    if (item.Price == 0) continue;
                    bool isgood = true;
                    if (includedPart != null)
                    {
                        foreach (var includedItem in includedPart)
                        {
                            if (!item.Name.Contains(includedItem)) { isgood = false; break; }
                        }
                    }
                    if (notIncludedPart != null)
                    {
                        foreach (var notIncludedItem in notIncludedPart)
                        {
                            if (item.Name.Contains(notIncludedItem)) { isgood = false; break; }
                        }
                    }
                    if (regexes != null)
                    {
                        foreach (var regexItem in regexes)
                        {
                            if (!regexItem.IsMatch(item.Name)) { isgood = false; break; }
                        }
                    }
                    if (func != null)
                    {
                        if (!func(item.Name)) { isgood = false; }
                    }
                    if (!isgood) { continue; }

                    choicenum++;
                    //MessageBox.Show("Test");
                    subtraction = Math.Abs(item.Price - paidMoney);
                    if (choicenum == 1)
                    {
                        minsubtraction = subtraction;
                        lastestReturn = item;
                    }

                    else
                    {
                        if (subtraction < minsubtraction)
                        {
                            minsubtraction = subtraction;
                            lastestReturn = item;
                        }

                    }
                }
                return lastestReturn;
            }
        }
        #endregion



        private void button2_Click(object sender, EventArgs e)
        {
            AutoChooseHW(hwCpu, 100, new List<string> { "", "" }, new List<string> { "", "" }, new List<Regex> { new Regex("") }, x => (x.Length > 2));
            //ComboBox1.SelectedIndex = 0;
        }


        private void label14_MouseEnter(object sender, EventArgs e)
        {
            labelvisit.ForeColor = Color.OrangeRed;
        }
        private void label14_MouseLeave(object sender, EventArgs e)
        {
            labelvisit.ForeColor = Color.Blue;
        }
        private void ComboBoxMouseHover(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if (cb != null)
            {
                toolTip1.SetToolTip(cb, cb.SelectedItem.ToString());
            }
        }


        private void TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.TextBox1.Focus();
            }
            else if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != 8)
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
            string[] lines = ClipDataSet().Split(param);

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
            //clipdataset();
            Clipboard.SetText(ClipDataSet());
            MessageBox.Show("复制成功！！");
        }
        private string ClipDataSet()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var item in allHwKind)
            {
                if (!string.IsNullOrEmpty(item.TxtName.Text.Trim()) && !string.IsNullOrEmpty(item.TxtPrice.Text.Trim()))
                {
                    sb.AppendFormat(item.KindNameCh + " {0} {1}\r\n", item.TxtName.Text.Trim(), item.TxtPrice.Text.Trim());
                }
            }

            sb.AppendLine(labelall.Text);
            sb.Append("————来自" + Application.ProductName + Application.ProductVersion);

            return sb.ToString();
        }


        private void CheckBoxamd_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxe3.Checked && checkBoxamd.Checked)
            {
                checkBoxamd.Checked = false;
                MessageBox.Show("E3为英特尔处理器型号"); return;
            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            AboutBox1 aboutshow = new AboutBox1();
            aboutshow.Show();
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
                        int startindex = pageHtml.IndexOf("~", index1);
                        int endindex = pageHtml.IndexOf("结束", index1);
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
        private void configdownload()
        {
            string pageHtml;
            try
            {

                WebClient MyWebClient = new WebClient();

                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于对向Internet资源的请求进行身份验证的网络凭据。

                Byte[] pageData = MyWebClient.DownloadData("http://bbs.luobotou.org/app/cjzs_config.txt"); //从指定网站下载数据

                pageHtml = Encoding.UTF8.GetString(pageData);
                //MessageBox.Show(pageHtml);
                //int index = pageHtml.IndexOf("#");
                String newdate;
                //MessageBox.Show(index.ToString());
                newdate = pageHtml.TrimStart().Substring(1, 10);
                //MessageBox.Show(newdate);
                if (newdate != pricedate)
                {
                    if (DialogResult.Yes == MessageBox.Show("配件价格有更新！\n是否现在更新？\n如果您不想收到此提示，请将配置文件第一行改为#", "更新", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk))
                    {
                        if (File.Exists(Application.StartupPath + "\\config_old_" + pricedate + ".ini")) { File.Delete(Application.StartupPath + "\\config_old_" + pricedate + ".ini"); }
                        File.Move(Application.StartupPath + "\\config.ini", Application.StartupPath + "\\config_old_" + pricedate + ".ini");
                        FileStream fs = new FileStream(Application.StartupPath + "\\config.ini", FileMode.Create, FileAccess.Write);
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
                if (ver != Application.ProductVersion)
                {
                    update frmf = new update(ver);
                    frmf.ShowDialog();
                    //frmf.Show();
                }
                else if (needupdate)
                {
                    threadPriceUpdate = new Thread(configdownload);
                    threadPriceUpdate.Start();

                }

            }
            catch (WebException webEx)
            {

                Console.WriteLine(webEx.Message.ToString());

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

            try
            {

                if (threadUpdate != null && threadUpdate.IsAlive) //判断thread1是否存在，不能撤消一个不存在的线程，否则会引发异常
                {
                    threadUpdate.Abort();
                }
            }
            catch
            {

            }
            try
            {
                if (threadReport != null && threadReport.IsAlive)
                {
                    threadReport.Abort();
                }
            }
            catch
            {

            }

        }
        private void report()
        {
            string pageHtml;
            try
            {

                WebClient MyWebClient = new WebClient();

                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于对向Internet资源的请求进行身份验证的网络凭据。

                Byte[] pageData = MyWebClient.DownloadData("http://bbs.luobotou.org/app/cjzs.txt"); //从指定网站下载数据

                pageHtml = Encoding.Default.GetString(pageData);
                int index = pageHtml.IndexOf("webreport=");

                if (pageHtml.Substring(index + 10, 1) == "1")
                {
                    string strURL = "http://myapp.luobotou.org/statistics.aspx?name=cjzs&ver=" + Application.ProductVersion;
                    System.Net.HttpWebRequest request;
                    // 创建一个HTTP请求
                    request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
                    System.Net.HttpWebResponse response;
                    response = (System.Net.HttpWebResponse)request.GetResponse();
                    System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    string responseText = myreader.ReadToEnd();
                    myreader.Close();
                }

            }
            catch (WebException webEx)
            {
                Console.WriteLine(webEx.Message.ToString());
            }
        }

        private void label23_Click(object sender, EventArgs e)
        {
            foreach (var item in allHwKind)
            {
                item.CBB.SelectedIndex = 0;
                item.TxtFilter.Text = string.Empty;
            }
            //foreach (var item in comboboxList)
            //{
            //    item.SelectedIndex = 0;
            //}
            //foreach (var item in tBFilter)
            //{
            //    item.Text = string.Empty;
            //}


        }

        private void label25_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutshow = new AboutBox1();
            aboutshow.Show();
        }



        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://bbs.luobotou.org/thread-6516-1-1.html");
        }

        private void 反馈建议ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://bbs.luobotou.org/forum-116-1.html");
        }



        private void 退出ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("http://www.kuaipan.cn/file/id_6067465939320914.html");
            System.Diagnostics.Process.Start("http://bbs.luobotou.org/thread-6400-1-1.html");


        }

        private void 检查更新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            threadUpdate = new Thread(update);
            threadUpdate.Start();
            MessageBox.Show("若无弹出窗口，则当前程序已是最新版本.");
        }

        private void 关于ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutshow = new AboutBox1();
            aboutshow.Show();
        }



        private void label24_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(adlink);
        }



        private void label24_Click_1(object sender, EventArgs e)
        {
            //clipdataset();
            pdDocument.Print();
        }

        private void 退出ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void TextBoxcpu1_Click(object sender, EventArgs e)
        {
            if (textBoxcpu.Text == "") { return; }
            if (textBoxcpu.Text.Contains("散"))
            {
                System.Diagnostics.Process.Start("http://s.taobao.com/search?promote=0&sort=sale-desc&tab=all&q=" + UrlEncode(textBoxcpu.Text));
            }
            else
            {
                System.Diagnostics.Process.Start("http://search.jd.com/Search?keyword=" + UrlEncode(textBoxcpu.Text));
            }
        }
        private void TextBoxPriceClick(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                if (!string.IsNullOrEmpty(tb.Text))
                {
                    Process.Start("http://search.jd.com/Search?keyword=" + UrlEncode(tb.Text));
                }
            }
        }


        private string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.Default.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxe3.CheckState == CheckState.Checked && checkboxedcpu.Checked) { MessageBox.Show("E3 CPU无盒装"); checkBoxe3.Checked = false; }
            if (checkBoxk.Checked && checkBoxe3.Checked) { MessageBox.Show("E3处理器不可超频！"); checkBoxe3.Checked = false; }
            if (checkBoxamd.Checked && checkBoxe3.Checked) { MessageBox.Show("E3为英特尔处理器型号！"); checkBoxe3.Checked = false; }
            if (checkBoxe3.CheckState == CheckState.Checked) { textBoxcpusr.Text = "E3"; }
            else { textBoxcpusr.Text = ""; }
        }

        private void checkBoxdouble_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxdouble.Checked) { textBoxramsr.Text = "x2"; }
            else { textBoxramsr.Text = ""; }
        }

        private void checkBoxk_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxe3.CheckState == CheckState.Checked && checkBoxk.Checked) { MessageBox.Show("E3处理器不可超频！"); checkBoxk.Checked = false; }
            if (checkBoxk.Checked) { textBoxcpusr.Text = "K"; }
            else { textBoxcpusr.Text = ""; }
        }

        private void labelvisit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://bbs.luobotou.org/forum-85-1.html");
        }



        private void Form1_Activated(object sender, EventArgs e)
        {
            TextBox1.Focus();
        }


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://bbs.luobotou.org/forum-116-1.html");

        }

        private void checknvgpu_CheckedChanged(object sender, EventArgs e)
        {
            if (checkamdgpu.Checked && checknvgpu.Checked) { checkamdgpu.Checked = false; }
            if (checknvgpu.Checked) { textBoxgpusr.Text = "GT"; }
            else { textBoxgpusr.Text = ""; }

        }

        private void checkamdgpu_CheckedChanged(object sender, EventArgs e)
        {
            if (checknvgpu.Checked && checkamdgpu.Checked) { checknvgpu.Checked = false; }
            if (checkamdgpu.Checked) { textBoxgpusr.Text = "HD|R5|R7|R9"; } else { textBoxgpusr.Text = ""; }
        }

        private void hezhuangcpu_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxe3.CheckState == CheckState.Checked && checkboxedcpu.Checked) { MessageBox.Show("E3 CPU无盒装"); checkboxedcpu.Checked = false; }
            //if (checkboxedcpu.Checked) { cpusr2 = "盒"; } else { cpusr2 = ""; }

        }


        private string CreateSpace(int num)
        {
            string space = "";
            for (int i = 1; i <= num; i++)
            {
                space += " ";
            }
            return space;
        }

        private void checkBoxhtpc_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxhtpc.Checked) { textBoxmbsr.Text = "MATX"; textBoxboxsr.Text = "HTPC"; }
            else { textBoxmbsr.Text = ""; textBoxboxsr.Text = ""; }
        }



        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://bbs.luobotou.org/thread-3057-1-1.html");

        }


        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://bbs.luobotou.org/thread-10008-1-1.html");

        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath);
        }
    }

}
