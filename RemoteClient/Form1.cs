using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace RemoteClient
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private TcpClient client;
        private NetworkStream stream;

        private void InitializeUI()
        {
            this.Text = "Remote Desktop Client";
            this.Width = 1024;
            this.Height = 768;
            pictureEdit1.Properties.ShowZoomSubMenu= DevExpress.Utils.DefaultBoolean.True;
            pictureEdit1.MouseMove += PictureEdit_MouseMove;
            pictureEdit1.MouseClick += PictureEdit_MouseClick;
        }

        private void StartClient()
        {
            InitializeUI();
            //10.1.61.93
            client = new TcpClient("10.1.60.241", 5000);
            //client = new TcpClient("10.1.61.93", 5000);
            stream = client.GetStream();
            Timer timer = new Timer { Interval = 100 }; // 每 100ms 更新屏幕
            timer.Tick += (s, ev) => ReceiveScreen();
            timer.Start();
        }

        private void ReceiveScreen()
        {
            try
            {
                // 接收屏幕数据长度
                byte[] lengthBuffer = new byte[4];
                stream.Read(lengthBuffer, 0, lengthBuffer.Length);
                int length = BitConverter.ToInt32(lengthBuffer, 0);

                // 接收屏幕数据
                byte[] buffer = new byte[length];
                int bytesRead = 0;
                while (bytesRead < length)
                {
                    bytesRead += stream.Read(buffer, bytesRead, length - bytesRead);
                }

                // 显示屏幕
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    Image img = Image.FromStream(ms);
                    pictureEdit1.Image = img;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void PictureEdit_MouseMove(object sender, MouseEventArgs e)
        {
            SendCommand($"MOVE,{e.X},{e.Y}");
        }

        private void PictureEdit_MouseClick(object sender, MouseEventArgs e)
        {
            SendCommand("CLICK");
        }

        private void SendCommand(string command)
        {
            try
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(command);
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendCommand: {ex.Message}");
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            StartClient();
        }
    }
}
