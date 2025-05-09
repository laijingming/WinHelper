using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageToIco
{
    public partial class PictureForm : DevExpress.XtraEditors.XtraForm
    {
        Form1 form1 = new Form1();
        public PictureForm()
        {
            InitializeComponent();
            LoadPictureCfg();
        }

        private void LoadPictureCfg()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                // 加载图片逻辑
                pictureEdit1.LoadAsync(args[1]);
            }

            //自适应图片
            pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            //缩放图片
            pictureEdit1.Properties.ShowZoomSubMenu = DevExpress.Utils.DefaultBoolean.True;
            //允许ctrl+鼠标缩放图片
            pictureEdit1.Properties.ShowScrollBars = true;
            //添加编辑
            pictureEdit1.Properties.ShowEditMenuItem = DevExpress.Utils.DefaultBoolean.True;

            pictureEdit1.Properties.PopupMenuShowing += Properties_PopupMenuShowing;
        }

        private void Properties_PopupMenuShowing(object sender, DevExpress.XtraEditors.Events.PopupMenuShowingEventArgs e)
        {
            if (e.PopupMenu == null) return;

            // 检查菜单项里是否已经有“我的自定义菜单”
            foreach (DXMenuItem item in e.PopupMenu.Items)
            {
                if (item.Caption == "ico")
                {
                    return; // 已存在，不再添加
                }
            }
            // 创建一个新的菜单项
            var customMenuItem = new DXMenuCheckItem("ico");
            customMenuItem.ImageOptions.Image = Image.FromFile("./file/picture_menu_ico.png");
            // 加点击事件
            customMenuItem.Click += (s, args) =>
            {
                form1.LoadImage(pictureEdit1.Image);
                form1.ShowDialog(this);
            };

            // 把自定义菜单项加到右键菜单里
            e.PopupMenu.Items.Add(customMenuItem);

        }
    }
}