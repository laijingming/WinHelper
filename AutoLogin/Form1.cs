using DevExpress.Utils;
using DevExpress.Utils.Automation;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;

namespace AutoLogin
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        private IWebDriver driver;

        public Form1()
        {
            InitializeComponent();
            //run();

            //string captchaCode = GetDingTalkNotification();
            //string captcha = GetDingTalkNotification();
            //Console.WriteLine("验证码: " + captcha);
            run();
        }

        private void run() 
        {
            string url = "https://ams.om.dianhun.cn/login?url=https%3A%2F%2Fdevops.om.dianhun.cn%2Fauth&id=1153";

            // 1. 设置 Chrome 选项
            ChromeOptions options = new ChromeOptions();
            //options.BinaryLocation = @"C:\Program Files\Google\Chrome\Application\chrome.exe"; // 指定本地 Chrome
            options.AddArgument("--start-maximized"); // 启动时最大化窗口

            // 2. 启动 WebDriver（自动匹配本地 Chrome 版本）
            driver = new ChromeDriver(options);

            // 3. 打开目标网站
            driver.Navigate().GoToUrl(url);
            try
            {

                //切换账号密码登录
                var switchButton = FindElement(By.ClassName("vui-login-modal-toggler")) ;
                switchButton.Click();

                //等待输入框加载
                var usernameField = FindElement(By.XPath("//input[@placeholder='输入账号或手机号码']"));
                var passwordField = FindElement(By.XPath("//input[@placeholder='输入密码']"));
                var captchaField = FindElement(By.XPath("//input[@placeholder='输入验证码']"));
                var captchaButton = FindElement(By.ClassName("input-link"));

                //填充数据
                usernameField.SendKeys("17759513112");
                passwordField.SendKeys("a5232829");
                
                //发送验证码
                captchaButton.Click();

                //拦截验证码，并输入
                captchaField.SendKeys(GetDingTalkNotification());
                //登录

                
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("发生错误: " + ex.Message);
            }
            finally
            {
                // 10. 关闭浏览器（可选）
                //driver.Quit();
            }
        }

        private IWebElement FindElement(By by) 
        {
            IWebElement input=null;
            int num = 0;
            while (input == null&&num<10)
            {
                try
                {
                    input = driver.FindElement(by);
                }
                catch (Exception)
                {
                    num++;
                    Thread.Sleep(1000);
                }
            }
            return input;   
        }


        #region Windows API 导入
        [DllImport("user32.dll")]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        #region 鼠标事件常量
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        const int SW_SHOWMAXIMIZED = 3;
        #endregion

        #region 窗口信息结构体
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        #endregion

        private IntPtr GetDingTalk() 
        {
            //查找钉钉
            IntPtr hWnd = FindWindow(null,"钉钉");
            if (hWnd == IntPtr.Zero)
            {
                Console.WriteLine("未找到钉钉窗口");
                return IntPtr.Zero;
            }
            //最大化窗口
            ShowWindow(hWnd, SW_SHOWMAXIMIZED);
            SetForegroundWindow(hWnd);
            Thread.Sleep(1000); // 等待窗口最大化

            //获取窗口坐标
            GetWindowRect(hWnd, out RECT rect);
            Console.WriteLine($"钉钉窗口位置: 左上({rect.Left},{rect.Top}), 右下({rect.Right},{rect.Bottom})");


            // 坐标调整，根据实际情况填写（你提供具体坐标）
            int unreadButtonX = rect.Left + 182; // 未读按钮相对位置
            int unreadButtonY = rect.Top + 58;

            int firstChatX = rect.Left + 211; // 第一个未读会话相对位置
            int firstChatY = rect.Top + 111;

            int currentX = Cursor.Position.X;//保存当前鼠标坐标
            int currentY = Cursor.Position.Y;

            // 4. 模拟鼠标点击未读按钮
            ClickMouse(unreadButtonX, unreadButtonY);
            Thread.Sleep(5000);

            // 5. 模拟鼠标点击第一个未读会话
            ClickMouse(firstChatX, firstChatY);
            Thread.Sleep(500);

            //还原鼠标位置
            SetCursorPos(currentX, currentY);

            return hWnd;
        }

        /// <summary>
        /// 模拟鼠标点击指定坐标
        /// </summary>
        static void ClickMouse(int x, int y)
        {
            SetCursorPos(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(50);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        /// <summary>
        /// 通过钉钉获取验证码信息
        /// </summary>
        /// <param name="dingTalk"></param>
        /// <returns></returns>
        private string GetDingTalkNotification()
        {
            AutomationElement dingTalk = AutomationElement.FromHandle(GetDingTalk());
            //定位验证码所在类名
            var element = dingTalk.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "DingChatWnd"));
            element = element.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "CefBrowserWindow"));
            element = element.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "Chrome_WidgetWin_0"));
            element = element.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "Chrome_RenderWidgetHostHWND"));
            element = element.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "列表"));
            var messageList = element.FindAll(TreeScope.Children, Condition.TrueCondition);
            var messageRow = messageList[messageList.Count - 1].FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, System.Windows.Automation.ControlType.Text));
            string message = messageRow[messageRow.Count - 1].Current.Name;
            Console.WriteLine("检测到消息："+ message);
            // 使用正则表达式提取验证码
            Match match = Regex.Match(message, @"\b\d{6}\b");
            if (match.Success)
            {
                return match.Value;
            }

            return "未找到验证码";
        }

        #endregion
    }
}
