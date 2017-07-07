using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Collections;

namespace HostsEditor
{
    public partial class HostsEditorForm : Form
    {
        public HostsEditorForm()
        {
            InitializeComponent();
            ReadFile();
        }
        Regex myReg = new Regex(@"\.(ru|com|ua|su|net|me|info|md)\b");
        string path = @"C:\Windows\System32\drivers\etc\hosts";
        async private void removeUrl_Click(object sender, EventArgs e)
        {
            string strUrl;
            if (listBox1.SelectedItem != null)
            {
                strUrl = listBox1.SelectedItem.ToString();
                listBox1.Items.Remove(listBox1.SelectedItem);

                var myList = new ArrayList();
                File.SetAttributes(path, FileAttributes.Normal);
                using (StreamReader sr = new StreamReader(path))
                {
                    string str;
                    var myReg1 = new Regex(strUrl);
                    while (true)
                    {
                        str = await sr.ReadLineAsync();
                        if (str == null) { break; }

                        if (myReg1.IsMatch(str) == false)
                        {
                            myList.Add(str);
                        }
                    }
                    sr.Close();
                }
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    foreach (var item in myList)
                    {
                        sw.WriteLine(item);
                    }
                    sw.Close();
                }
                ReadFile();
                File.SetAttributes(path, FileAttributes.ReadOnly);
            }
            if (listBox1.SelectedItem == null)
                unlockUrl.Visible = false;
        }
        async private void addUrl_Click(object sender, EventArgs e)
        {
            if (urlString.Text != null && myReg.IsMatch(urlString.Text) == true)
            {
                File.SetAttributes(path, FileAttributes.Normal);
                StreamWriter sw = new StreamWriter(path, true);
                await sw.WriteLineAsync("127.0.0.1 " + urlString.Text);
                sw.Close();
                File.SetAttributes(path, FileAttributes.ReadOnly);
                ReadFile();
                if (listBox1.SelectedItem == null)
                    unlockUrl.Visible = false;
                urlString.Text = "";
            }
            else
            {
                MessageBox.Show("Введите правельный адресс", "Ошибка");
            }
        }
        async private void ReadFile()
        {
            listBox1.Items.Clear();
            StreamReader sr = new StreamReader(path);
            string str = await sr.ReadToEndAsync();
            foreach (Match m in Regex.Matches(str, @"127.0.0.1\s+.+\.(ru|com|ua|su|net|me|info)\b"))
                listBox1.Items.Add(Regex.Match(m.Value, @"\b\S+\.(ru|com|ua|su|net|me|info)\b"));
            sr.Close();
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            unlockUrl.Visible = true;
        }
        private void поУмолчаниюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string str1 = "# Copyright (c) 1993-2009 Microsoft Corp.\n#\n# This is a sample HOSTS file used by Microsoft TCP/IP for Windows.\n#\n# This file contains the mappings of IP addresses to host names. Each\n# entry should be kept on an individual line. The IP address should\n# be placed in the first column followed by the corresponding host name.\n# The IP address and the host name should be separated by at least one\n# space.\n#\n# Additionally, comments (such as these) may be inserted on individual\n# lines or following the machine name denoted by a '#' symbol.\n#\n# For example:\n#\n#      102.54.94.97     rhino.acme.com          # source server\n#       38.25.63.10     x.acme.com              # x client host\n\n# localhost name resolution is handled within DNS itself.\n#	127.0.0.1       localhost\n#	::1             localhost\n\n";
            File.SetAttributes(path, FileAttributes.Normal);
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                sw.Write(str1);
                sw.Close();
            }
            ReadFile();
            File.SetAttributes(path, FileAttributes.ReadOnly);
        }
        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            urlString.Text = "vk.com";
        }
    }
}
