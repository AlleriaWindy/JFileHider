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
            Fill_DirList("I:/");
            Fill_FileList("I:/");
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
                }
            }
        }



      
    }

    class FakeDirInfo
    {
        public string Name { get; set; }
        public string FullName { get; set; }
    }
}
