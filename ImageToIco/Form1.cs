using AJLibrary;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ImageToIco
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
        } 


        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            string inputImagePath = FileTool.OpenFile(FileTool.ImageFileFilter);
            if (inputImagePath != null )
            {   
                pictureEdit1.Image = Image.FromFile(inputImagePath);
                string outputIconPath = Path.ChangeExtension(inputImagePath, ".ico");
                ConvertImageToIcon(inputImagePath, outputIconPath, new Size(64, 64));// 设置图标尺寸
                pictureEdit2.Image = Image.FromFile(outputIconPath);
            }

        }

        private void ConvertImageToIcon(string inputPath, string outputPath, Size iconSize)
        {
            using (Bitmap bitmap = new Bitmap(inputPath))
            {
                // 调整图片尺寸
                Bitmap resizedBitmap = new Bitmap(bitmap, iconSize);

                // 将调整后的图片保存为 ICO 文件
                using (MemoryStream ms = new MemoryStream())
                {
                    resizedBitmap.Save(ms, ImageFormat.Png);
                    using (FileStream fs = new FileStream(outputPath, FileMode.Create))
                    {
                        Icon.FromHandle(resizedBitmap.GetHicon()).Save(fs);
                    }
                }
            }
        }
    }
}
