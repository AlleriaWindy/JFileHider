using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Trinet.Core.IO.Ntfs;
using System.Diagnostics;

namespace JFileHider
{
    static class ADSHelper
    {
        public static bool CheckNTFS(string path)
        {
            DriveInfo di = new DriveInfo(path);
            return di.DriveFormat.ToUpper() == "NTFS";
        }
        public static List<AlternateDataStreamInfo> ListAllADSFile(string path)
        {
            if (!CheckNTFS(path))
                return null;
            List<AlternateDataStreamInfo> lst = new List<AlternateDataStreamInfo>();
            string[] file_names = Directory.GetFiles(path);
            foreach (string s in file_names)
            {
                FileInfo fi = new FileInfo(s);
                var ais = fi.ListAlternateDataStreams();
                if (!(ais == null || ais.Count <= 0))
                {
                    foreach (var ai in ais)
                        lst.Add(ai);
                }
            }
            return lst;
        }

        public static bool CreateADSFile(string fname,string adsname,string hidfile)
        {
            string arg = string.Format(" /c type \"{0}\" > \"{1}:{2}\"", hidfile, fname, adsname);

            Process proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = arg;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.CreateNoWindow = true;

            proc.Start();
            proc.WaitForExit();

            return true;
        }
    }
}
