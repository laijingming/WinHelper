using AJLibrary;
using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.Linq;
using System.Collections.Generic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Windows.Forms;
using DevExpress.Utils.Menu;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ImageToIco
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
            //ExternalConfig();
            ReplaceLoadMenuItem();
        } 

        /// <summary>
        /// 加载转换文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            string inputImagePath = FileTool.OpenFile(FileTool.ImageFileFilter);
            if (inputImagePath != null )
            {
                textEdit2.Text = inputImagePath;
                pictureEdit1.LoadAsync(inputImagePath);
            }
        }

        /// <summary>
        /// 开始转换
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton2_Click(object sender, EventArgs e)
        {
            List<int> sizes = new List<int>();

            int size = getCustomSize();
            
            if (size != 0)
            {
                if (size < 16)
                {
                    textEdit1.ErrorText = "尺寸不能小于16！";
                    textEdit1.Focus();
                    return;
                }
                sizes.Add(size);
            }
            foreach (CheckedListBoxItem item in checkedListBoxControl1.Items)
            {
                if (item.CheckState == System.Windows.Forms.CheckState.Checked)
                {
                    sizes.Add(int.Parse(item.Value.ToString()));
                }
            }
            if (pictureEdit1.Image==null)
            {
                textEdit2.ErrorText = "未上传图片！";
                return;
            }
            if (sizes.Count>0)
            {
                string outputIconPath = Path.GetFullPath("./icon/"); ;
                if (!Directory.Exists(outputIconPath))
                {
                    Directory.CreateDirectory(outputIconPath);
                }
                outputIconPath = outputIconPath+ Time.Now().ToString() + ".ico";
                AJLibrary.IconConverter.SaveIconWithMultipleSizes(pictureEdit1.Image, outputIconPath, sizes.ToArray());
                pictureEdit2.LoadAsync(outputIconPath);
                textEdit3.Text = outputIconPath;
            }
        }

        private void textEdit1_EditValueChanged(object sender, EventArgs e)
        {
            int size = getCustomSize();
            if (size!=0)
            {
                labelControl1.Text = textEdit1.Text + "x" + textEdit1.Text;
            }
            else
            {
                labelControl1.Text = "";
            }
        }

        private int getCustomSize() 
        {
            if (textEdit1.Text.Length > 0 && textEdit1.Text != "0")
            {
                return int.Parse(textEdit1.Text);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void simpleButton3_Click(object sender, EventArgs e)
        {   
            if (string.IsNullOrEmpty(textEdit3.Text))
            {
                textEdit3.ErrorText = "未生成图标！";
                return;
            }
            string dir = Path.GetDirectoryName(textEdit3.Text);
            if (Directory.Exists(dir)) {
                System.Diagnostics.Process.Start("explorer.exe", dir);
            }
        }

        private void ReplaceLoadMenuItem()
        {
            // 遍历当前右键菜单项
            //for (int i = 0; i < pictureEdit1.Properties.ContextMenu.MenuItems.Count; i++)
            //{
                
            //}
        }

        private void pictureEdit1_PopupMenuShowing(object sender, DevExpress.XtraEditors.Events.PopupMenuShowingEventArgs e)
        {
            DXMenuItem execSelectedItem = new DXMenuItem("调用");

            foreach (DXMenuItem item in e.PopupMenu.Items)
            {
                if (item.Caption == "调用")
                {
                    item.Visible = false;
                    execSelectedItem.ImageOptions.Image=item.ImageOptions.Image;
                }
            }
            execSelectedItem.Click += simpleButton1_Click_1;
            e.PopupMenu.Items.Add(execSelectedItem);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ExternalConfig() 
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
        }
    }

}
