using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using Trinet.Core.IO.Ntfs;

namespace JFileHider
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listBox1.DisplayMember = "Name";
            listBox2.DisplayMember = "Name";
            listBox3.DisplayMember = "Name";
            comboBox1.DisplayMember = "Item1";
            comboBox1.ValueMember = "Item2";
        }

        private void Fill_Drive()
        {
            comboBox1.Items.Clear();
            DriveInfo[] dris = DriveInfo.GetDrives();
            foreach (var dri in dris)
            {
                if (dri.IsReady && dri.DriveFormat.ToUpper() == "NTFS")
                {
                    string name = dri.Name + "[" + dri.VolumeLabel + "]";
                    Tuple<string, string> tp = new Tuple<string, string>(name,dri.Name);
                    comboBox1.Items.Add(tp);
                }
            }
            comboBox1.SelectedIndex = 0;
        }

        private void Fill_DirList(string path)
        {
            listBox1.Items.Clear();
            string[] strs = Directory.GetDirectories(path);
            

            var tmp = new DirectoryInfo(path);
            if (tmp.Parent != null)
            {
                FakeDirInfo ff = new FakeDirInfo();
                ff.Name = "..";
                ff.FullName = tmp.Parent.FullName;
                listBox1.Items.Add(ff);
            }
            
            foreach (string s in strs)
            {
                listBox1.Items.Add(new DirectoryInfo(s));
            }
        }

        private void Fill_FileList(string path)
        {
            listBox2.Items.Clear();
            string[] strs = Directory.GetFiles(path);
            foreach (string s in strs)
            {
                listBox2.Items.Add(new FileInfo(s));
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Fill_Drive();
            
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox1.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                string new_path = "";
                object obj = listBox1.Items[index];
                FakeDirInfo f = obj as FakeDirInfo;
                if (f != null)
                {
                    
                    Fill_DirList(f.FullName);
                }
                else
                {
                    DirectoryInfo di = (DirectoryInfo)obj;
                    Fill_DirList(di.FullName);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index >= 0)
            {
                DirectoryInfo di = listBox1.Items[index] as DirectoryInfo;
                if (di != null)
                {
                    Fill_FileList(di.FullName);
                    textBox1.Text = di.FullName;
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox2.SelectedIndex;
            if (index >= 0)
            {
                listBox3.Items.Clear();
                FileInfo fi = listBox2.Items[index] as FileInfo;
                if (fi != null)
                {
                    var ais = ADSHelper.ListADSFile(fi.FullName);
                    if (ais != null)
                    {
                        foreach (var ai in ais)
                            listBox3.Items.Add(ai);
                    }
                       
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
                return;
            string path = ((Tuple<string, string>)comboBox1.SelectedItem).Item2;
            Fill_DirList(path);
            Fill_FileList(path);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex < 0 || listBox2.SelectedIndex < 0)
            {
                MessageBox.Show("请选择带有隐藏信息的文件！");
                return;
            }
            FileInfo fi = listBox2.SelectedItem as FileInfo;
            AlternateDataStreamInfo ai = listBox3.SelectedItem as AlternateDataStreamInfo;
            string fullname = fi.FullName + ":" + ai.Name;
            ADSHelper.ExecADSFile(fullname);
            
        }



      
    }

    class FakeDirInfo
    {
        public string Name { get; set; }
        public string FullName { get; set; }
    }
}
