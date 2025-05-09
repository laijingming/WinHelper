using DevExpress.XtraEditors;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;

namespace AutoLogin
{
    public class RetryChromeDriverWrapper
    {
        private ChromeDriver _driver;
        private readonly string _userDataDir;
        private readonly int _port;
        private readonly ChromeDriverService _service;
        private readonly string _logFilePath;
        public Action<string> OnLog = Console.WriteLine; // 默认输出到控制台

        public RetryChromeDriverWrapper(string userDataDir, int port) 
        {
            _userDataDir = userDataDir;
            _port = port;
            _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "file/autologin.log");

            _service = ChromeDriverService.CreateDefaultService();
            _service.HideCommandPromptWindow = true;

            CleanZombieChromeDrivers();  // 新增：清理 zombie driver 进程
        }

        public IWebDriver GetDriver()
        {
            if (_driver != null)
            {
                try
                {
                    var url = _driver.Url;
                    return _driver;
                }
                catch
                {   
                    Log("已失效的 ChromeDriver 实例，准备重启...");
                    DisposeDriver();
                }
            }

            InitDriver();
            return _driver;
        }

        private void InitDriver()
        {
            if (IsPortOpen("127.0.0.1", _port, 1000))
            {
                try
                {
                    Log($"尝试复用 Chrome 调试端口 {_port}");
                    var options = new ChromeOptions();
                    options.DebuggerAddress = $"127.0.0.1:{_port}";
                    _driver = new ChromeDriver(_service, options);
                    return;
                }
                catch (WebDriverException)
                {
                    Log("复用失败，Chrome 会话已失效，准备重新启动...");
                    DisposeDriver();
                }
            }

            LaunchNewChrome();
        }

        private void LaunchNewChrome()
        {
            Log($"启动新的 Chrome，端口: {_port}");

            if (!Directory.Exists(_userDataDir))

            {
                Directory.CreateDirectory(_userDataDir);
            }

            var options = new ChromeOptions();
            options.AddArgument($"user-data-dir={_userDataDir}");
            options.AddArgument("--start-maximized");
            options.AddArgument($"--remote-debugging-port={_port}");

            _driver = new ChromeDriver(_service, options);
        }


        private void DisposeDriver()
        {
            try
            {
                _driver?.Quit();
                _driver?.Dispose();
            }
            catch(Exception e) { XtraMessageBox.Show(e.ToString()); }
            _driver = null;
        }

        private bool IsPortOpen(string host, int port, int timeoutMs)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var result = client.BeginConnect(host, port, null, null);
                    bool success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(timeoutMs));
                    if (!success) return false;
                    client.EndConnect(result);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 自动清理旧的 chromedriver 进程（Zombie 进程）
        /// </summary>
        private void CleanZombieChromeDrivers()
        {
            try
            {
                var processes = Process.GetProcessesByName("chromedriver");
                foreach (var proc in processes)
                {
                    if (!proc.HasExited)
                    {
                        Log($"终止旧的 chromedriver 进程 PID: {proc.Id}");
                        proc.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                Log($"清理 chromedriver 异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 写入日志（带时间戳）
        /// </summary>
        private void Log(string message)
        {
            string log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
            Console.WriteLine(log);

            try
            {
                File.AppendAllText(_logFilePath, log + Environment.NewLine);
                OnLog.Invoke(message);
            }
            catch { }
        }
    }
}
