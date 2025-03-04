using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using ScriptManagement.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ScriptManagement
{
    public static class Program
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

            Application.Run(new Form1());
        }

        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            CommandLog.getIns.SaveCacheToFile();
        }
    }
}
