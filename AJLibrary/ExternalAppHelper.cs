using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AJLibrary
{
    // 嵌入外部程序辅助类
    public class ExternalAppHelper
    {   
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        private const int GWL_STYLE = -16;
        private const int WS_CHILD = 0x40000000;
        private const int GWL_EXSTYLE = -20;
        private const int WS_CAPTION = 0x00C00000;  // 标题栏
        private const int WS_THICKFRAME = 0x00040000;  // 可调整大小的边框
        private const int WS_SYSMENU = 0x00080000;  // 系统菜单
        private const int WS_MINIMIZEBOX = 0x00020000;  // 最小化按钮
        private const int WS_MAXIMIZEBOX = 0x00010000;  // 最大化按钮

        private static ProcessStartInfo psi = null;
        public Process _process;
        private IntPtr _hWnd;
        public Control _control;
        public string _appPath;

        public void  Init()
        {
            this._control.Resize += _control_Resize;
            this._control.Disposed += _control_Disposed;
            this._control.HandleDestroyed += _control_Disposed;
        }

        public static void MakeProcessStartInfo(string appPath)
        {
            // 创建一个进程启动信息对象
            if (psi == null)
            {
                psi = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Minimized
                };
            }
            psi.FileName = appPath;
        }

        public bool AttachToRunningInstance()
        {
            foreach (Process process in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(_appPath)))
            {
                try
                {
                    _hWnd = FindMainWindowHandle(process.Id);
                    if (_hWnd != IntPtr.Zero)
                    {
                        _process = process;
                        EmbedWindow();
                        return true;
                    }
                }
                catch (Win32Exception)
                {
                    // 忽略访问被拒绝的异常
                    continue;
                }
            }
            return false;
        }

        public Process Embed()
        {
            if (this.AttachToRunningInstance())
            {   
                return _process;
            }

            MakeProcessStartInfo(_appPath);
            _process = Process.Start(psi);
            // 查找主窗口句柄
            _hWnd = IntPtr.Zero;
            int attempts = 10;
            while (_hWnd == IntPtr.Zero && attempts > 0)
            {
                Thread.Sleep(500);
                attempts--;
                _hWnd = FindMainWindowHandle(_process.Id);
            }

            if (_hWnd == IntPtr.Zero)
            {
                throw new Exception("无法找到外部程序的主窗口句柄。");
            }


            // 嵌入窗口
            EmbedWindow();

            return _process;
        }


        private void EmbedWindow()
        {
            // 设置外部窗口为子窗口
            SetParent(_hWnd, _control.Handle);
            UpdateWindowStyle();
            // 调整外部程序窗口大小
            MoveWindow(_hWnd, 0, 0, _control.Width, _control.Height, true);
        }

        private static IntPtr FindMainWindowHandle(int processId)
        {
            IntPtr mainWindowHandle = IntPtr.Zero;

            EnumWindows((hWnd, lParam) =>
            {
                int windowProcessId;
                GetWindowThreadProcessId(hWnd, out windowProcessId);

                if (windowProcessId == processId && IsWindowVisible(hWnd))
                {
                    // 获取窗口标题，用于过滤无关窗口
                    StringBuilder sb = new StringBuilder(256);
                    GetWindowText(hWnd, sb, sb.Capacity);

                    if (!string.IsNullOrEmpty(sb.ToString()))
                    {
                        mainWindowHandle = hWnd;
                        return false; // 停止枚举
                    }
                }

                return true; // 继续枚举
            }, IntPtr.Zero);

            return mainWindowHandle;
        }



        public void UpdateWindowStyle()
        {

            // 获取当前窗口样式
            int style = GetWindowLong(_hWnd, GWL_STYLE);

            // 移除标题栏、边框和系统菜单
            style &= ~WS_CAPTION;    // 移除标题栏
            style &= ~WS_THICKFRAME; // 移除可调整大小的边框
            style &= ~WS_SYSMENU;    // 移除系统菜单
            style &= ~WS_MINIMIZEBOX; // 移除最小化按钮
            style &= ~WS_MAXIMIZEBOX; // 移除最大化按钮

            // 应用新的样式
            SetWindowLong(_hWnd, GWL_STYLE, style | WS_CHILD);

            // 获取当前扩展样式
            int exStyle = GetWindowLong(_hWnd, GWL_EXSTYLE);

            // 应用新的扩展样式（如果有需要，可以在此修改）
            SetWindowLong(_hWnd, GWL_EXSTYLE, exStyle);
        }

        private void _control_Resize(object sender, EventArgs e)
        {
            // 调整嵌入窗口大小
            if (_hWnd != IntPtr.Zero)
            {
                MoveWindow(_hWnd, 0, 0, _control.Width, _control.Height, true);
            }
        }

        private void _control_Disposed(object sender, EventArgs e)
        {
            if (_process != null && !_process.HasExited)
            {
                try
                {
                    _process.Kill(); // 强制终止外部程序
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"关闭外部程序失败: {ex.Message}");
                }
                finally
                {
                    _process = null;
                    _hWnd = IntPtr.Zero;
                }
            }
        }

    }
}
