using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace RemoteDesktopServer
{
    class Program
    {
        [DllImport("user32.dll")]
        public static extern void SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        const int MOUSEEVENTF_LEFTDOWN = 0x02;
        const int MOUSEEVENTF_LEFTUP = 0x04;

        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();
            Console.WriteLine("Server started...");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client connected...");
                Thread screenThread = new Thread(() => SendScreen(client));
                Thread controlThread = new Thread(() => ReceiveControl(client));
                screenThread.Start();
                controlThread.Start();
            }
        }

        static void SendScreen(TcpClient client)
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
                    Console.WriteLine($"Error in SendScreen: {ex.Message}");
                    break;
                }
            }
        }

         public Bitmap CaptureScreen()
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
                    Console.WriteLine($"Error in ReceiveControl: {ex.Message}");
                    break;
                }
            }
        }
    }
}
