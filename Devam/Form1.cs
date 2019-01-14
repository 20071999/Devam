using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Devam
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Oku();
            string keyName = @"HKEY_CURRENT_USer\Software\Microsoft\Windows\CurrentVersion\Run";
            string valueName = "Devam V2.0";
            if (Registry.GetValue(keyName, valueName, null) == null)
            {
                checkBox1.Checked = false;
            }
            else
            {
                checkBox1.Checked = true;
            }
        }
        //Gerekli WinAPI'leri Importlıyoruz.
        [DllImport("user32.dll")]
        static extern int GetForegroundWindow();

        [DllImport("user32")]
        private static extern UInt32 GetWindowThreadProcessId(Int32 hWnd, out Int32 lpdwProcessId);
       
       
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                Int32 hwnd = 0;
                hwnd = GetForegroundWindow();
                string İslemAdi = Process.GetProcessById(GetWindowProcessID(hwnd)).ProcessName; //Önplanda olan işlemimizin işlem adını çekiyoruz.
                label1.Text = İslemAdi;
            }
            catch (Exception ) {}
        
        }
        private Int32 GetWindowProcessID(Int32 hwnd)
        {
            Int32 pid = 1;
            GetWindowThreadProcessId(hwnd, out pid); 
            return pid;
        }

        public void Oku()
        {
            try
            {
                if(File.Exists(Environment.CurrentDirectory+ "\\Programlar.txt"))
                {
                var satirlar = File.ReadAllLines(Environment.CurrentDirectory+ "\\Programlar.txt");
                foreach (string satir in satirlar)
                {
                    string[] yazilar = satir.Split('|');
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = yazilar[0].ToString();
                    lvi.SubItems.Add(yazilar[1].ToString());
                    lvi.SubItems.Add(yazilar[2].ToString());
                    listView2.Items.Add(lvi);
                }
                }
            }
            catch (Exception) { }
        }
        private void Kaydet(ListView lv, string splitter)
        {
            using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory+ "\\Programlar.txt"))
            {
                foreach (ListViewItem item in lv.Items)
                {
                    try
                    {
                        sw.WriteLine("{0}{1}{2}{3}{4}", item.Text, splitter, item.SubItems[1].Text, splitter, item.SubItems[2].Text);
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
        }

        private void label1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Int32 hwnd = 0;
                hwnd = GetForegroundWindow();
                string İslemAdi = Process.GetProcessById(GetWindowProcessID(hwnd)).ProcessName; //Önplanda olan işlem id'lerimizle istediklerimizi yapıyoruz.
                string ProgramYolu = Process.GetProcessById(GetWindowProcessID(hwnd)).MainModule.FileName;
                string ExecutableAdi = ProgramYolu.Substring(ProgramYolu.LastIndexOf(@"\") + 1);
              
                ListViewItem lvi = new ListViewItem(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(İslemAdi)); //Baş harflerin büyük olması için CulturInfo class'ımızı kullandık.
                lvi.SubItems.Add(ProgramYolu);
                lvi.SubItems.Add(ExecutableAdi);
                ListViewItem item = listView1.FindItemWithText(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(İslemAdi));
                if (item == null)
                {
                    listView1.Items.Add(lvi);
                }
                        if (lvi.Text == "Explorer")
                        {
                            listView1.Items.Remove(lvi);
                        }
                        
                       
                
            }
            catch (Exception) { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Kaydet(listView1, "|");
        }

        private void kaldırToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem itemler in listView1.SelectedItems)
            {
                listView1.Items.Remove(itemler);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                if (listView2.Items.Count >= 1)
                {
                    listView2.Items[0].Selected = true;
                    listView2.Select();
                    Process pc = new Process();
                    pc.StartInfo.FileName = listView2.SelectedItems[0].SubItems[1].Text;
                    pc.Start();
                    listView2.Items.Remove(listView2.Items[0]);
                }

            }
            catch (Exception ) {  }
        }
        /*
         *  try
            {
                File.Delete(Environment.CurrentDirectory + @"\Programlar.txt");
            }
            catch (Exception) { }
         */
        
        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yapımcı: 20071999\nKodlama Dili: C#\nTurkhackteam.org","Hakkında",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

        private void programınAmacıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("PC'yi açınca açıkken kullandığını programlar oto. açılır böylelikle tek tek çalıştığınız programları açmanıza gerek kalmaz.", "Programın Amacı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                File.Delete(Environment.CurrentDirectory + @"\Programlar.txt");
            }
            catch (Exception) { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            button4.Enabled = false;
            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (checkBox1.Checked)
                rk.SetValue("Devam V2.0", Application.ExecutablePath);
            else
                rk.DeleteValue("Devam V2.0", false);            
        }

        private void manuelEkleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog of = new OpenFileDialog();
            of.Filter = "Kısayol - Executable | *.lnk; *.exe";
            if (of.ShowDialog() == DialogResult.OK)
            {
                ListViewItem lvi = new ListViewItem(of.FileName.Substring(of.FileName.LastIndexOf(@"\") + 1).Replace(".lnk",string.Empty).Replace(".exe",string.Empty));
                lvi.SubItems.Add(of.FileName);
                lvi.SubItems.Add(of.FileName.Substring(of.FileName.LastIndexOf(@"\") + 1));
                listView1.Items.Add(lvi);
            }
        }

      
    }
}
