using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace FileManager
{
    class ShellAPIHelper
    {

        public enum ShowWindowCommands : int
        {

            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,    //用最近的大小和位置显示，激活
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_MAX = 10
        }
        [DllImport("shell32.dll")]
        public static extern IntPtr ShellExecute(
            IntPtr hwnd,
            string lpszOp,
            string lpszFile,
            string lpszParams,
            string lpszDir,
            ShowWindowCommands FsShowCmd
            );

        public string FileName { get; set; }
        public string ExeParam { get; set;   }
        public ShellAPIHelper(string fname, string param = null)
        {
            FileName = fname;
            ExeParam = param;
        }

        public int Execute()
        {
            IntPtr rt = IntPtr.Zero;
            string workpath = Path.GetDirectoryName(FileName);
            rt = ShellExecute(IntPtr.Zero, "explore", FileName, ExeParam, workpath, ShowWindowCommands.SW_SHOWNORMAL);
            int rrr = rt.ToInt32();
            if (rrr < 32)
                throw new Exception("API执行错误，错误代号：" + rrr.ToString());
            else
                return rrr;
        }

        public void ShowOpenWithDialog()
        {
            var args = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shell32.dll");
            args += ",OpenAs_RunDLL " + FileName;
            Process.Start("rundll32.exe", args);
        }
    }
}
