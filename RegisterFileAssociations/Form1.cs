using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;

namespace RegisterFileAssociations
{   
    /// <summary>
    /// 支持注册+取消注册
    /// </summary>
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
            EnsureRunAsAdministrator();
            RegisterFileAssociations();
        }

        /// <summary>
        /// 是否管理员运行
        /// </summary>
        private void EnsureRunAsAdministrator()
        {
            if (!IsAdministrator())
            {
                var exeName = Process.GetCurrentProcess().MainModule.FileName;
                var startInfo = new ProcessStartInfo(exeName)
                {
                    Verb = "runas",
                    UseShellExecute = true
                };
                try
                {
                    Process.Start(startInfo);
                }
                catch
                {
                    MessageBox.Show("需要以管理员权限运行才能进行文件关联操作。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 是否管理员
        /// </summary>
        /// <returns></returns>
        private bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        private readonly string progId = "ImageViewerApp"; // 自定义的ProgId
        private readonly string[] extensions = { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };

        /// <summary>
        /// 设置为默认图片查看器
        /// </summary>
        /// <param name="appPath"></param>
        private void RegisterFileAssociations(
            string appPath= "D:\\CSharp\\Helper\\ImageToIco\\bin\\Release\\图片转ICO.exe",
            string description= "图片查看器",
            string _iconPath = ""
            )
        {
            string iconPath = _iconPath;
            if (string.IsNullOrEmpty(_iconPath))
            {
                iconPath = appPath;
            }
            iconPath = $"\"{iconPath}\",0";

            foreach (var ext in extensions)
            {
                Registry.SetValue($@"HKEY_CLASSES_ROOT\{ext}", "", progId);
            }

            using (var key = Registry.ClassesRoot.CreateSubKey(progId))
            {
                if (key == null) return;

                key.SetValue("", description);

                using (var defaultIcon = key.CreateSubKey("DefaultIcon"))
                {
                    defaultIcon?.SetValue("", iconPath);
                }

                using (var shell = key.CreateSubKey(@"shell\open\command"))
                {
                    shell?.SetValue("", $"\"{appPath}\" \"%1\"");
                }
            }

            MessageBox.Show("文件关联成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 取消关联，恢复原状
        private void UnregisterFileAssociations()
        {
            foreach (var ext in extensions)
            {
                try
                {
                    // 清除扩展名指向
                    Registry.ClassesRoot.DeleteSubKeyTree(ext, false);
                }
                catch { /* 有可能没权限或者不存在，忽略 */ }
            }

            try
            {
                // 删除自定义的ProgId信息
                Registry.ClassesRoot.DeleteSubKeyTree(progId, false);
            }
            catch { /* 忽略异常 */ }

            MessageBox.Show("文件关联已取消！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
