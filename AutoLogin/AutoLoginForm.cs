using AJLibrary;
using DevExpress.Utils;
using DevExpress.Utils.Automation;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
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
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;

namespace AutoLogin
{
    public partial class AutoLoginForm : DevExpress.XtraEditors.XtraForm
    {
        AutologinModel autologinModel = null;
        public AutoLoginForm()
        {
            InitializeComponent();
            InitData();
            autologinModel  = new AutologinModel();
        }

        private void InitData()
        {
            gridControl1.RefreshDataSource();
            gridControl1.DataSource = autologinModel.data;
        }

        /// <summary>
        /// 一键登录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cardView1_DoubleClick(object sender, EventArgs e)
        {
            autologinModel.Run((DomainModel)cardView1.GetFocusedRow());
        }
    }
    public class AutologinModel
    {   
        public List<DomainModel> data { get;}

        public AutologinModel() 
        {
            data = JsonHelper.DeserializeJsonFileToType<List<DomainModel>>("./cfg/data.json");
        }

        public string RunByName(string name)
        {
            return Run(data.Find(x => x.name == name));
        }


        #region 自动登录逻辑
        private IWebDriver driver;
        private void SetDriver()
        {
            if (driver != null)
            {
                try
                {
                    // 尝试获取当前标签页的窗口句柄
                    var handles = driver.WindowHandles;
                    // 如果能正常获取，说明实例依然有效，直接返回
                    return;
                }
                catch
                {
                    // 如果捕获到异常，则认为 driver 无效，置空以便重新初始化
                    driver.Quit();
                    driver.Dispose();
                    driver = null;
                }
            }
            ChromeOptions options = new ChromeOptions();
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            // 判断本地9222端口是否开放
            if (IsPortOpen("127.0.0.1", 9222, 1000))
            {
                // 已有 Chrome 调试实例
                options.DebuggerAddress = "127.0.0.1:9222";
                Console.WriteLine("检测到已启动的Chrome实例，将复用该浏览器。");
                driver = new ChromeDriver(service, options);
            }
            else
            {
                options.DebuggerAddress = null;
                // 未检测到调试实例，则启动新的 Chrome 实例
                Console.WriteLine("未检测到Chrome调试实例，启动新的浏览器。");
                // 如果需要让新启动的Chrome也启用调试模式，可以添加以下参数：
                options.AddArgument("user-data-dir=D:\\SeleniumChromeProfile");
                options.AddArgument("--start-maximized");
                options.AddArgument("--remote-debugging-port=9222");

                driver = new ChromeDriver(service, options);
            }
        }

        /// <summary>
        /// 检测指定主机和端口是否开放
        /// </summary>
        private bool IsPortOpen(string host, int port, int timeoutMs)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var result = client.BeginConnect(host, port, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(timeoutMs));
                    if (!success)
                        return false;
                    client.EndConnect(result);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Run(DomainModel domain)
        {
            SetDriver();
            // 检查是否已有标签页的 URL 包含目标地址
            bool found = false;
            foreach (string handle in driver.WindowHandles)
            {
                driver.SwitchTo().Window(handle);
                // 根据实际情况，可以使用 Equals 或 Contains 来判断
                if (!string.IsNullOrEmpty(driver.Url) && driver.Url.Contains(domain.url))
                {
                    found = true;
                    break;
                }
            }
            // 如果没有找到，则新建标签页
            if (!found)
            {
                driver.SwitchTo().NewWindow(WindowType.Tab);
                driver.Navigate().GoToUrl(domain.url);
            }
            else
            {
                // 如果找到了，则刷新或继续操作
                driver.Navigate().Refresh();
            }


            try
            {

                //切换账号密码登录
                var switchButton = FindElement(By.ClassName("vui-login-modal-toggler"));
                if (switchButton == null)
                {
                    return "已登录";
                }
                switchButton.Click();

                // 如果用户仍然未输入有效数据，则提示后退出
                if (string.IsNullOrWhiteSpace(domain.account) || string.IsNullOrWhiteSpace(domain.password))
                {
                    return "账号或密码未填写，无法继续登录！";
                }

                //等待输入框加载
                var usernameField = FindElement(By.XPath("//input[@placeholder='输入账号或手机号码']"));
                var passwordField = FindElement(By.XPath("//input[@placeholder='输入密码']"));
                var captchaField = FindElement(By.XPath("//input[@placeholder='输入验证码']"));
                var captchaButton = FindElement(By.ClassName("input-link"));

                //填充数据
                usernameField.SendKeys(domain.account);
                passwordField.SendKeys(domain.password);

                //发送验证码
                captchaButton.Click();

                //获取验证码，并输入
                captchaField.SendKeys(GetDingTalkNotification());

                //点击登录按钮
                var loginButton = FindElement(By.CssSelector("button.submit"));
                loginButton.Click();


                // 忽略提示（如果有）
                var ignoreButton = FindElement(By.XPath("//button[span[normalize-space(text())='忽略']]"));
                if (ignoreButton != null)
                {
                    ignoreButton.Click();
                }
                return "登录成功";
            }
            catch (Exception ex)
            {
                return "发生错误: " + ex.Message;
            }
            finally
            {
                // 10. 关闭浏览器（可选）
                //driver.Quit();
            }
        }



        private IWebElement FindElement(By by)
        {
            IWebElement input = null;
            int num = 0;
            while (input == null && num < 5)
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
            IntPtr hWnd = FindWindow(null, "钉钉");
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

            int allButtonX = rect.Left + 100; // 全部按钮相对位置
            int allButtonY = rect.Top + 58;

            int currentX = Cursor.Position.X;//保存当前鼠标坐标
            int currentY = Cursor.Position.Y;



            //模拟鼠标点击未读按钮
            ClickMouse(unreadButtonX, unreadButtonY);
            Thread.Sleep(4000);

            //模拟鼠标点击第一个未读会话
            ClickMouse(firstChatX, firstChatY);
            Thread.Sleep(500);

            //模拟鼠标点击全部按钮
            ClickMouse(allButtonX, allButtonY);

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

        private int tryNum = 0;
        /// <summary>
        /// 通过钉钉获取验证码信息
        /// </summary>
        /// <param name="dingTalk"></param>
        /// <returns></returns>
        private string GetDingTalkNotification()
        {
            
            tryNum++;
            AutomationElement dingTalk = AutomationElement.FromHandle(GetDingTalk());
            try
            {   
                //定位验证码所在类名
                var element = dingTalk.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "DingChatWnd"));
                element = element.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "CefBrowserWindow"));
                element = element.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "Chrome_WidgetWin_0"));
                element = element.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.ClassNameProperty, "Chrome_RenderWidgetHostHWND"));
                element = element.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, "列表"));
                var messageList = element.FindAll(TreeScope.Children, Condition.TrueCondition);
                var messageRow = messageList[messageList.Count - 1].FindAll(TreeScope.Descendants, new PropertyCondition(AutomationElement.ControlTypeProperty, System.Windows.Automation.ControlType.Text));
                string message = messageRow[messageRow.Count - 1].Current.Name;
                Console.WriteLine("检测到消息：" + message);
                // 使用正则表达式提取验证码
                Match match = Regex.Match(message, @"\b\d{6}\b");
                if (match.Success)
                {
                    return match.Value;
                }
            }
            catch (Exception)
            {
                if (tryNum < 3)
                {
                    return GetDingTalkNotification();

                }
            }
            return "未找到验证码";
        }

        #endregion
        #endregion
    }

    public class DomainModel
    {   
        public string name {  get; set; }
        public string account {  get; set; }
        public string password { get; set; }
        public string url { get; set; }

    }
}
