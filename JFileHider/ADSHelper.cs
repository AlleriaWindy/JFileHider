using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Trinet.Core.IO.Ntfs;

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

        public static List<AlternateDataStreamInfo> ListADSFile(string fname)
        {
            if (!CheckNTFS(fname))
                return null;
            List<AlternateDataStreamInfo> lst = new List<AlternateDataStreamInfo>();
            var ais = (new FileInfo(fname)).ListAlternateDataStreams();
            foreach (var ai in ais)
                lst.Add(ai);
            if (lst.Count <= 0)
                return null;
            else
                return lst;
        }

        private static void RunCmd(string arg, bool wait = true, string start_pos = null)
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = " /c " + arg;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.CreateNoWindow = true;
            if (start_pos != null)
                proc.StartInfo.WorkingDirectory = start_pos;

            proc.Start();//test
            if (wait)
                proc.WaitForExit();
        }

        public static bool CreateADSFile(string fname, string adsname, string hidfile)
        {
            string arg = string.Format("type \"{0}\" > \"{1}:{2}\"", hidfile, fname, adsname);

            RunCmd(arg);

            return true;
        }

        public static void ExecADSFile(string fullpath, string arg = null)
        {
            string tmpfile = System.Windows.Forms.Application.ExecutablePath;
            tmpfile = Path.GetDirectoryName(tmpfile) + "/temp.ttt";
            if (File.Exists(tmpfile))
                File.Delete(tmpfile);
            RunCmd(string.Format("mklink \"{0}\" \"{1}\"", tmpfile, fullpath));

            //RunCmd(tmpfile, false,Path.GetDirectoryName(fullpath));

            FileManager.ShellAPIHelper sh = new FileManager.ShellAPIHelper(@"D:\VisualStudioProjects\JFileHider\JFileHider\bin\Debug\temp.ttt");
            sh.ShowOpenWithDialog();
            //Process.Start(@"D:\VisualStudioProjects\JFileHider\JFileHider\bin\Debug\temp.ttt");
        }
    }
}
