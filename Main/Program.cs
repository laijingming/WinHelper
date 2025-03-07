using AJLibrary;
using DevExpress.XtraEditors;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Main
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ApplicationExit += Application_ApplicationExit;

            Application.EnableVisualStyles();//防止进程重复开启限制
            Application.SetCompatibleTextRenderingDefault(false);
            Process instance = RuningInstance();
            if (instance != null)
            {
                HandleRunningInstance(instance);
                return;
            }

            String skin = ConfigCache.GetIns.Get("Skin");
            if (skin != null)
            {
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(skin);
            }

            Application.Run(new MainForm());
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            //皮肤存储
            if (ConfigCache.GetIns.Get("Skin") != DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName)
            {
                ConfigCache.GetIns.Set("Skin", DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName);
            }
            Master.destory();
        }

        public static Process RuningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //遍历与当前进程名称相同的进程列表 
            foreach (Process process in processes)
            {
                //如果实例已经存在则忽略当前进程 
                if (process.Id != current.Id)
                {
                    //保证要打开的进程同已经存在的进程来自同一文件路径
                    //if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    //{
                    //返回已经存在的进程
                    return process;
                    //}
                }
            }
            return null;
        }

        //已经有了就把它激活，并将其窗口放置最前端
        public static void HandleRunningInstance(Process instance)
        {
            XtraMessageBox.Show("已经在运行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ShowWindowAsync(instance.MainWindowHandle, 1);  //调用api函数，正常显示窗口
            SetForegroundWindow(instance.MainWindowHandle); //将窗口放置最前端
        }
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(System.IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(System.IntPtr hWnd);


    }
}
