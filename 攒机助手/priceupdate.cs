using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace 攒机助手
{
    public partial class priceupdate : Form
    {
        string[] cpu = new string[50]; int[] cpu1 = new int[50];
        string[] acpu = new string[50]; int[] acpu1 = new int[50];
        string[] mb = new string[50]; int[] mb1 = new int[50];
        string[] gpu = new string[50]; int[] gpu1 = new int[50];
        string[] ram = new string[50]; int[] ram1 = new int[50];
        string[] power = new string[50]; int[] power1 = new int[50];
        string[] lcd = new string[50]; int[] lcd1 = new int[50];
        string[] box = new string[50]; int[] box1 = new int[50];
        string[] hdd = new string[50]; int[] hdd1 = new int[50];
        string[] ssd = new string[50]; int[] ssd1 = new int[50];
        string[] fan = new string[50]; int[] fan1 = new int[50];
        string[] cdrom = new string[50]; int[] cdrom1 = new int[50];
        string[] kb = new string[50]; int[] kb1 = new int[50];
        delegate void set_Text(string s); //定义委托
        int allhw = 0;
        int errorhw = 0;

        set_Text Set_Text;
        //Thread pupdate;
        public priceupdate()
        {
            InitializeComponent();
        }
        private void set_textboxText(string s)
        {
            textBox1.AppendText(System.DateTime.Now.ToString() + " " + s + "\r\n");
            textBox1.Focus();
        }
        private void updating()
        {
            textBox1.Invoke(Set_Text, new object[] { "进程已启动" });
            int z = 1;
            for ( z = 1; z <= 49; z++)
            {
                int tempprice = 0 ;
                if (gpu1[z] != 0) 
                {
                    tempprice = update(gpu[z]);
                    if (tempprice != 0) { gpu1[z] = tempprice; }
                }

                if (cpu1[z] != 0)
                {
                    tempprice = update(cpu[z]);
                    if (tempprice != 0) { cpu1[z] = tempprice; }
                }
                if (acpu1[z] != 0)
                {
                    tempprice = update(acpu[z]);
                    if (tempprice != 0) { acpu1[z] = tempprice; }
                }

                if (hdd1[z] != 0)
                {
                    tempprice = update(hdd[z]);
                    if (tempprice != 0) { hdd1[z] = tempprice; }
                }
                if (ram1[z] != 0)
                {
                    tempprice = update(ram[z]);
                    if (tempprice != 0) { ram1[z] = tempprice; }
                }
                if (mb1[z] != 0)
                {
                    tempprice = update(mb[z]);
                    if (tempprice != 0) { mb1[z] = tempprice; }
                }
                if (lcd1[z] != 0)
                {
                    tempprice = update(lcd[z]);
                    if (tempprice != 0) { lcd1[z] = tempprice; }
                }
                if (fan1[z] != 0)
                {
                    tempprice = update(fan[z]);
                    if (tempprice != 0) { fan1[z] = tempprice; }
                }
                if (box1[z] != 0)
                {
                    tempprice = update(box[z]);
                    if (tempprice != 0) { box1[z] = tempprice; }
                }
                if (power1[z] != 0)
                {
                    tempprice = update(power[z]);
                    if (tempprice != 0) { power1[z] = tempprice; }
                }
                if (cdrom1[z] != 0)
                {
                    tempprice = update(cdrom[z]);
                    if (tempprice != 0) { cdrom1[z] = tempprice; }
                }
                if (kb1[z] != 0)
                {
                    tempprice = update(kb[z]);
                    if (tempprice != 0) { kb1[z] = tempprice; }
                }
                if (ssd1[z] != 0)
                {
                    tempprice = update(ssd[z]);
                    if (tempprice != 0) { ssd1[z] = tempprice; }
                }


            }
            FileStream fs = new FileStream(Application.StartupPath + "\\config.ini", FileMode.Open, FileAccess.Write);
            fs.SetLength(0);
            StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
            string writeString = "";
            //try
            //{
                for ( z = 1; z <=49; z++)
                {
                    if (gpu1[z] != 0)
                    {
                        writeString = "[GPU]" + gpu[z] + "," + gpu1[z].ToString();
                        sw.WriteLine(writeString);
                    }
                    
                }
                for (z = 1; z <= 49; z++)
                {
                    if (cpu1[z] != 0)
                    {
                        writeString = "[CPU]" + cpu[z] + "," + cpu1[z].ToString();
                        sw.WriteLine(writeString);
                    }
                    
                }
                for (z = 1; z <= 49; z++)
                {
                    if (acpu1[z] != 0)
                    {
                        writeString = "[ACPU]" + acpu[z] + "," + acpu1[z].ToString();
                        sw.WriteLine(writeString);
                    }

                }
                for (z = 1; z <= 49; z++)
                {
                    if (hdd1[z] != 0)
                    {
                        writeString = "[HDD]" + hdd[z] + "," + hdd1[z].ToString();
                        sw.WriteLine(writeString);
                    }
                    else break;
                }
                for (z = 1; z <= 49; z++)
                {
                    if (ram1[z] != 0)
                    {
                        writeString = "[RAM]" + ram[z] + "," + ram1[z].ToString();
                        sw.WriteLine(writeString);
                    }
                    else break;
                }
                for (z = 1; z <= 49; z++)
                {
                    if (mb1[z] != 0)
                    {
                        writeString = "[MB]" + mb[z] + "," + mb1[z].ToString();
                        sw.WriteLine(writeString);
                    }
                    else break;
                }
                for (z = 1; z <= 49; z++)
                {
                    if (lcd1[z] != 0)
                    {
                        writeString = "[LCD]" + lcd[z] + "," + lcd1[z].ToString();
                        sw.WriteLine(writeString);
                    }
                    //else break;
                }

                for (z = 1; z <= 49; z++)
                {
                    if (box1[z] != 0)
                    {
                        writeString = "[BOX]" + box[z] + "," + box1[z].ToString();
                        sw.WriteLine(writeString);
                    }
                    //else break;
                }

                for (z = 1; z <= 49; z++)
                {
                    if (power1[z] != 0)
                    {
                        writeString = "[POW]" + power[z] + "," + power1[z].ToString();
                        sw.WriteLine(writeString);
                    }
                    //else break;
                }


                for (z = 1; z <= 49; z++)
                {
                    if (fan1[z] != 0)
                    {
                        writeString = "[FAN]" + fan[z] + "," + fan1[z].ToString();
                        sw.WriteLine(writeString);
                    }
                    else break;
                }
                for (z = 1; z <= 49; z++)
                {
                    if (cdrom1[z] != 0)
                    {
                        writeString = "[CDROM]" + cdrom[z] + "," + cdrom1[z].ToString();
                        sw.WriteLine(writeString);
                    }
                    else break;
                }
                for (z = 1; z <= 49; z++)
                {
                    if (kb1[z] != 0)
                    {
                        writeString = "[KB]" + kb[z] + "," + kb1[z].ToString();
                        sw.WriteLine(writeString);
                    }
                    else break;
                }
                for (z = 1; z <= 49; z++)
                {
                    if (ssd1[z] != 0)
                    {
                        writeString = "[SSD]" + ssd[z] + "," + ssd1[z].ToString();
                        sw.WriteLine(writeString);
                    }
                    else break;
                }
                    //writeString = dataGridView1.Rows[i].Cells[0].Value.ToString() + dataGridView1.Rows[i].Cells[1].Value.ToString() + "," + dataGridView1.Rows[i].Cells[2].Value.ToString() + "|" + dataGridView1.Rows[i].Cells[3].Value.ToString() + "#" + dataGridView1.Rows[i].Cells[4].Value.ToString();
                    //sw.WriteLine(writeString);
            //}
            
            //catch { }
            sw.Close();
            MessageBox.Show("更新完成!总共更新" + allhw.ToString() + "个硬件，错误" + errorhw.ToString() + "个。\n请重新启动程序，生效新价格");


        }
        private void button1_Click(object sender, EventArgs e)
        {
            Thread pupdate = new Thread(updating);
            pupdate.Start();
        }
        public static string UrlEncode(string str)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = System.Text.Encoding.Default.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }

        private int update(string pname)
        {
            string pageHtml;
            allhw++;

            try
            {

                WebClient MyWebClient = new WebClient();

                MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于对向Internet资源的请求进行身份验证的网络凭据。

                Byte[] pageData;
                pageData = MyWebClient.DownloadData("http://detail.zol.com.cn/index.php?c=SearchList&keyword=" + UrlEncode(pname)); //从指定网站下载数据

                pageHtml = Encoding.Default.GetString(pageData);
                //MessageBox.Show(pageHtml);
                int list;
                list = pageHtml.IndexOf("class=\"result_list\"");
                int productpage = pageHtml.IndexOf("<a href=", list);
                int pgs = pageHtml.IndexOf("/", productpage);
                int pge = pageHtml.IndexOf("\"", pgs);//productpage end
                string ppu = pageHtml.Substring(pgs, pge - pgs);//productpage url
                //MessageBox.Show(ppu);
                pageData = MyWebClient.DownloadData("http://detail.zol.com.cn" + ppu); //从指定网站下载数据

                pageHtml = Encoding.Default.GetString(pageData);
                if (!pageHtml.ToUpper().Replace(" ", "").Contains(Regex.Replace(pname.ToUpper().Replace(" ", ""), @"\([^\(]*\)", "")))
                {
                    textBox1.Invoke(Set_Text, new object[] { pname + ppu });
                    errorhw++;
                    return 0;
                }

                int pricestart = pageHtml.IndexOf("price-type") + 12;
                int priceend = pageHtml.IndexOf("</b>", pricestart);
                string newprice = pageHtml.Substring(pricestart, priceend - pricestart);
                textBox1.Invoke(Set_Text, new object[] { pname + " " + newprice });
                //log(pname + " " + newprice);
                if (pname.Contains("x2")) { return System.Int32.Parse(newprice) * 2; }
                else { return System.Int32.Parse(newprice); }
            }
            catch (Exception  e)
            {
                textBox1.Invoke(Set_Text, new object[] { pname+e.ToString () });
                errorhw++;

                return 0; 
            }

        }
        private void priceupdate_Load(object sender, EventArgs e)
        {
            Set_Text = new set_Text(set_textboxText);

            StreamReader objReader = new StreamReader(Application.StartupPath + "\\config.ini");
            //       string configfilemd5;
            //   configfilemd5 = FileToMD5(Application.StartupPath + "\\config.ini");
            //  labelmd5.Text  += configfilemd5;
            // if (configfilemd5 == "71E39FF335034B23CA62553EBF4EC560") { labelchange.Text += "否"; } else { labelchange.Text += "是"; }
            //MessageBox.Show(configfilemd5);
            string sLine = "";
            int cpuloop = 0;
            int acpul = 0;
            int mbloop = 0;
            int ramloop = 0;
            int hddloop = 0;
            int gpuloop = 1;
            int boxloop = 0;
            int powloop = 1;
            int lcdloop = 0;
            int fanloop = 0;
            int cdromloop = 0;
            int kbloop = 0;
            int ssdloop = 0;
            //ArrayList LineList = new ArrayList();
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                try
                {
                    //if (sLine.Substring(0, 1) == "'") { continue; }
                    if (sLine.Contains("[CPU]"))
                    {
                        cpu[++cpuloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                        cpu1[cpuloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                    }
                    else if (sLine.Contains("[ACPU]"))
                    {
                        acpu[++acpul] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                        acpu1[acpul] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                    }
                    else if (sLine.Contains("[MB]"))
                    {
                        mb[++mbloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                        mb1[mbloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                    }
                    else if (sLine.Contains("[RAM]"))
                    {
                        ram[++ramloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                        ram1[ramloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                    }
                    else if (sLine.Contains("HDD"))
                    {
                        hdd[++hddloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                        hdd1[hddloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                    }
                    else if (sLine.Contains("[GPU]"))
                    {
                        gpu[++gpuloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                        gpu1[gpuloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                    }
                    else if (sLine.Contains("[BOX]"))
                    {
                        box[++boxloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                        box1[boxloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                    }
                    else if (sLine.Contains("[POW]"))
                    {
                        power[++powloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                        power1[powloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                    }
                    else if (sLine.Contains("[LCD]"))
                    {
                        lcd[++lcdloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                        lcd1[lcdloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                    }
                    else if (sLine.Contains("[FAN]"))
                    {
                        fan[++fanloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                        fan1[fanloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                    }
                    else if (sLine.Contains("[CDROM]"))
                    {
                        cdrom[++cdromloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                        cdrom1[cdromloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                    }
                    else if (sLine.Contains("[KB]"))
                    {
                        kb[++kbloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                        kb1[kbloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                    }
                    else if (sLine.Contains("[SSD]"))
                    {
                        ssd[++ssdloop] = sLine.Substring(sLine.IndexOf("]") + 1, sLine.IndexOf(",") - sLine.IndexOf("]") - 1);
                        ssd1[ssdloop] = System.Int32.Parse(sLine.Substring(sLine.IndexOf(",") + 1));
                    }
                }
                catch { }
                //MessageBox.Show(sLine);
                // if (sLine != null && !sLine.Equals(""))
                //   LineList.Add(sLine);
            }
            objReader.Close();

            //////
            cpu[0] = "";

            gpu[1] = "集成";

            power[1] = "机箱自带";
            log("配置文件加载完毕");
        }
        private void log(string logs) 
        {
            textBox1.AppendText ( System.DateTime.Now.ToString() + " " + logs+"\r\n");
            //textBox1.Text += System.DateTime.Now.ToString() + " " + logs ;
            //textBox1.Text += "\r\n";
            textBox1.Focus();
            //textBox1.SelectionStart = textBox1.TextLength;
            //textBox1.SelectionLength = 0;
            //textBox1.ScrollToCaret();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ScrollToCaret();

        }

        private void label2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.luobotou.pw/thread-6212-1-1.html");
        }
    }
}
