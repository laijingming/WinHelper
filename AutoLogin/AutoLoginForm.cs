using AJLibrary;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using static DevExpress.Utils.Drawing.Helpers.NativeMethods;

namespace AutoLogin
{
    public partial class AutoLoginForm : DevExpress.XtraEditors.XtraForm
    {
        AutologinModel autologinModel = AutologinModel.getIns;
        public AutoLoginForm()
        {
            InitializeComponent();
            InitData();
            autologinModel.OnLog = AppendLog;
            this.FormClosing += AutoLoginForm_FormClosing;
        }

        private void AutoLoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            autologinModel.CloseDriver();
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
            SplashScreenManager.ShowDefaultWaitForm("正在登录", "请等待...");
            autologinModel.Run((DomainModel)cardView1.GetFocusedRow());
            SplashScreenManager.CloseForm();
            // BringToFront 使窗口在 z 顺序上置顶
            //this.BringToFront();
            // Activate 使窗口获得焦点
            //this.Activate();
        }

        private void AppendLog(string message) 
        {
            if (memoLog.InvokeRequired)
            {
                memoLog.Invoke(new Action(()=>AppendLog(message)));
            }
            else
            {
                memoLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {message}{Environment.NewLine}");
            }

        }

    }
    public class AutologinModel
    {
        public Action<string> OnLog = Console.WriteLine; // 默认输出到控制台
        public List<DomainModel> data { get;}

        public static AutologinModel getIns = SingletonHelper<AutologinModel>.GetInstance();

        public AutologinModel() 
        {   
            _driverWrapper.OnLog = Log;
            data = JsonHelper.DeserializeJsonFileToType<List<DomainModel>>("./file/autologindata.json");
        }

        private void Log(string msg)
        {
            OnLog?.Invoke(msg);
        }

        public DomainModel GetDomainByName(string name) 
        {
            return data.Find(x => x.name == name);
        }

        #region chromedriver
        private RetryChromeDriverWrapper _driverWrapper =
    new RetryChromeDriverWrapper(ConfigCache.GetIns.GetAutoLoginDir(), Convert.ToInt32(ConfigCache.GetIns.GetAutoLoginPort()));
        private IWebDriver driver => (ChromeDriver)_driverWrapper.GetDriver();
        public void CloseDriver() 
        {
            //if (driver != null)
            //{
            //    driver.Quit();
            //    driver.Dispose();
            //    driver = null;
            //}
        }
        //private void SetDriver()
        //{
        //    if (driver != null)
        //    {
        //        try
        //        {
        //            var hand = driver.Url;
        //            return;
        //        }
        //        catch (Exception)
        //        {
        //            try
        //            {
        //                driver.Quit();
        //                driver.Dispose();
        //            }
        //            catch { }
        //            driver = null;
        //        }
        //    };

        //    ChromeOptions options = new ChromeOptions();
        //    ChromeDriverService service = ChromeDriverService.CreateDefaultService();
        //    service.HideCommandPromptWindow = true;

        //    string port = ConfigCache.GetIns.GetAutoLoginPort();
        //    string dir = ConfigCache.GetIns.GetAutoLoginDir();
        //    if (!Directory.Exists(dir))
        //    {
        //        Directory.CreateDirectory(dir);
        //    }

        //    bool reused = false;

        //    // 判断本地9222端口是否开放
        //    if (IsPortOpen("127.0.0.1", Convert.ToInt16(port), 1000))
        //    {
        //        try
        //        {
        //            options.DebuggerAddress = "127.0.0.1:" + port;
        //            Console.WriteLine("检测到Chrome调试实例，尝试复用中...");
        //            driver = new ChromeDriver(service, options);
        //            reused = true;
        //        }
        //        catch (WebDriverException)
        //        {
        //            XtraMessageBox.Show("连接已断开，重启浏览器...");
        //        }
        //    }

        //    if (!reused)
        //    {
        //        options.DebuggerAddress = null;
        //        options.AddArgument("user-data-dir=" + dir);
        //        options.AddArgument("--start-maximized");
        //        options.AddArgument("--remote-debugging-port=" + port);
        //        driver = new ChromeDriver(service, options);
        //    }
        //}

        ///// <summary>
        ///// 检测指定主机和端口是否开放
        ///// </summary>
        //private bool IsPortOpen(string host, int port, int timeoutMs)
        //{
        //    try
        //    {
        //        using (var client = new TcpClient())
        //        {
        //            var result = client.BeginConnect(host, port, null, null);
        //            bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(timeoutMs));
        //            if (!success)
        //                return false;
        //            client.EndConnect(result);
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
        #endregion


        #region 自动登录逻辑


        public void Run(DomainModel domain)
        {
            Log($"准备登录：{domain.name} @ {domain.url}");

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
                    Log("已登录");
                    return ;
                }
                switchButton.Click();

                // 如果用户仍然未输入有效数据，则提示后退出
                if (string.IsNullOrWhiteSpace(domain.account) || string.IsNullOrWhiteSpace(domain.password))
                {
                    Log("账号或密码未填写，无法继续登录！");
                    return ;
                }

                //等待输入框加载
                var usernameField = FindElement(By.XPath("//input[@placeholder='输入账号或手机号码']"));
                var passwordField = FindElement(By.XPath("//input[@placeholder='输入密码']"));
                var captchaField = FindElement(By.XPath("//input[@placeholder='输入验证码']"),false);

                Log("开始填充账号、密码");
                //填充数据
                usernameField.SendKeys(domain.account);
                passwordField.SendKeys(domain.password);

                if (captchaField != null)
                {
                    Log("正在获取验证码...");
                    //发送验证码
                    FindElement(By.ClassName("input-link")).Click();
                    //获取验证码，并输入
                    captchaField.SendKeys(GetDingTalkNotification(domain));
                }

                var loginButton = FindElement(By.CssSelector("button.submit"));
                loginButton.Click();


                // 忽略提示（如果有）
                var ignoreButton = FindElement(By.XPath("//button[span[normalize-space(text())='忽略']]"));
                if (ignoreButton != null)
                {
                    ignoreButton.Click();
                }
                Log("登录完成！");
            }
            catch (Exception ex)
            {
                Log("位置错误：" + ex.Message);
            }
        }

        private IWebElement FindElement(By by, bool isTryMoreTimes = true)
        {
            IWebElement input = null;
            int num = 0;
            while (input == null && num < 3)
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
                if (!isTryMoreTimes)
                {
                    break;
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
        [DllImport("user32.dll")]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        #region 鼠标事件常量
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;
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

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public Point ptMinPosition;
            public Point ptMaxPosition;
            public RECT rcNormalPosition;
        }
        #endregion
        private bool IsWindowMaximized(IntPtr hWnd)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = Marshal.SizeOf(placement);
            GetWindowPlacement(hWnd, ref placement);

            // 判断窗口是否已经最大化
            return placement.showCmd == SW_SHOWMAXIMIZED;
        }
        private bool IsWindowForeground(IntPtr hWnd)
        {
            // 获取当前前台窗口的句柄
            IntPtr foregroundWindow = GetForegroundWindow();

            // 判断目标窗口是否已经是前台窗口
            return foregroundWindow == hWnd;
        }

        private IntPtr GetDingTalk(DomainModel domain)
        {
            // 查找“钉钉”窗口（窗口标题为“钉钉”）
            IntPtr hWnd = FindWindow(null, "钉钉");
            if (hWnd == IntPtr.Zero)
            {
                Console.WriteLine("未找到钉钉窗口");
                return IntPtr.Zero;
            }

            // 保存当前鼠标位置，确保在方法结束时恢复
            Point originalCursor = Cursor.Position;

            try
            {
                // 判断是否需要最大化窗口
                if (!IsWindowMaximized(hWnd))
                {
                    ShowWindow(hWnd, SW_SHOWMAXIMIZED);
                }

                // 判断是否需要将窗口置于前台
                if (!IsWindowForeground(hWnd))
                {
                    SetForegroundWindow(hWnd);
                }

                Thread.Sleep(1000); // 等待窗口最大化稳定

                // 获取窗口坐标
                GetWindowRect(hWnd, out RECT rect);
                Console.WriteLine($"钉钉窗口位置: 左上({rect.Left},{rect.Top}), 右下({rect.Right},{rect.Bottom})");

                // 计算各个按钮的绝对坐标（相对于屏幕）
                int unreadButtonX = rect.Left + domain.unreadButtonX; // 未读按钮的 X 坐标
                int unreadButtonY = rect.Top + domain.unreadButtonY;   // 未读按钮的 Y 坐标

                int firstChatX = rect.Left + domain.firstChatX; // 第一个未读会话的 X 坐标
                int firstChatY = rect.Top + domain.firstChatY;   // 第一个未读会话的 Y 坐标

                int allButtonX = rect.Left + domain.allButtonX; // 全部按钮的 X 坐标
                int allButtonY = rect.Top + domain.allButtonY;   // 全部按钮的 Y 坐标

                // 模拟鼠标点击操作
                // 点击未读按钮
                ClickMouse(unreadButtonX, unreadButtonY);
                Thread.Sleep(2500); // 等待会话列表加载

                // 点击第一个未读会话
                ClickMouse(firstChatX, firstChatY);
                Thread.Sleep(500);

                // 点击“全部”按钮
                ClickMouse(allButtonX, allButtonY);

                // 返回钉钉窗口句柄
                return hWnd;
            }
            finally
            {
                // 无论操作是否成功，都恢复鼠标到原来的位置
                SetCursorPos(originalCursor.X, originalCursor.Y);
            }
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
        private string GetDingTalkNotification(DomainModel domain)
        {
            tryNum++;
            AutomationElement dingTalk = AutomationElement.FromHandle(GetDingTalk(domain));
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
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
            if (tryNum < 3)
            {
                return GetDingTalkNotification(domain);

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

        public string fiddlerUrl { get; set; }

        //未读按钮坐标
        public int unreadButtonX {  get; set; }
        public int unreadButtonY {  get; set; }

        //第一个未读会话坐标
        public int firstChatX { get; set; }
        public int firstChatY { get; set; }

        //全部按钮相对坐标
        public int allButtonX {  get; set; }
        public int allButtonY {  get; set; }

    }
}
