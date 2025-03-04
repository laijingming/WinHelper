using AJLibrary;
using System;
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


            String skin = ConfigCache.getIns.Get("Skin");
            if (skin != null)
            {
                DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(skin);
            }

            Application.Run(new MainForm());
        }


        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            //皮肤存储
            if (ConfigCache.getIns.Get("Skin") != DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName)
            {
                ConfigCache.getIns.Set("Skin", DevExpress.LookAndFeel.UserLookAndFeel.Default.SkinName);
            }
            Master.destory();
        }
    }
}
