using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace RemoteSever
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }



        private void simpleButton1_Click(object sender, EventArgs e)
        {

            Task.Run(() =>
            {
                Start();

            });

        }


        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        const int MOUSEEVENTF_LEFTDOWN = 0x02;
        const int MOUSEEVENTF_LEFTUP = 0x04;

        private void Start() 
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();
            AppendText("Server started...");
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                AppendText("Client connected...");
                Thread screenThread = new Thread(() => SendScreen(client));
                Thread controlThread = new Thread(() => ReceiveControl(client));
                screenThread.Start();
                controlThread.Start();
            }
        }

        private void AppendText(string text)
        {
            memoEdit1.BeginInvoke(new Action(() => {
                memoEdit1.AppendText(text + Environment.NewLine);
            }));
        }

        void SendScreen(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            while (true)
            {
                try
                {
                    Bitmap screenshot = CaptureScreen();
                    MemoryStream ms = new MemoryStream();
                    screenshot.Save(ms, ImageFormat.Jpeg);

                    byte[] buffer = ms.ToArray();
                    byte[] length = BitConverter.GetBytes(buffer.Length);

                    stream.Write(length, 0, length.Length);
                    stream.Write(buffer, 0, buffer.Length);

                    Thread.Sleep(100); // 控制帧率
                }
                catch (Exception ex)
                {
                    AppendText($"Error in SendScreen: {ex.Message}");
                    break;
                }
            }
        }

        Bitmap CaptureScreen()
        {
            Rectangle bounds = Screen.PrimaryScreen.Bounds;
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.CopyFromScreen(0, 0, 0, 0, bounds.Size);
            }
            return bitmap;
        }

        void ReceiveControl(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            while (true)
            {
                try
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string command = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        string[] parts = command.Split(',');

                        if (parts[0] == "MOVE")
                        {
                            int x = int.Parse(parts[1]);
                            int y = int.Parse(parts[2]);
                            SetCursorPos(x, y);
                        }
                        else if (parts[0] == "CLICK")
                        {
                            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppendText($"Error in ReceiveControl: {ex.Message}");
                    break;
                }
            }
        }

    }
}
