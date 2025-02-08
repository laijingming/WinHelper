using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraBars.Docking2010.Views;
using DevExpress.XtraEditors;
using DevExpress.XtraBars.Docking2010;
using AJLibrary;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System;
using System.Diagnostics;

namespace ExternalApp
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        public Form1()
        {
            InitializeComponent();
            windowsUIView1.AddTileWhenCreatingDocument = DevExpress.Utils.DefaultBoolean.False;
            CreateLayout();
        }

        private Dictionary<string, List<string>> categories = new Dictionary<string, List<string>>()
        {
            { "工具类应用", new List<string> { @"F:\CloudMusic\cloudmusic.exe", "D:\\CSharp\\Helper\\ImageToIco\\bin\\Debug\\ImageToIco.exe" } },
            { "办公类应用", new List<string> { @"D:\CSharp\devop\DXApplication1\bin\Release\Devop.exe", @"D:\work\PhpStorm 2023.3.6\bin\phpstorm64.exe" } }
        };


        void CreateLayout()
        {
            InitializeMainTileContainerWithPageGroup();
        }



        private void InitializeMainTileContainerWithPageGroup()
        {
            // 清空现有内容
            //windowsUIView1.ContentContainers.Clear();
            //mainTileContainer.Items.Clear();

            foreach (var category in categories)
            {

                foreach (var appPath in category.Value)
                {
                    Page page = new Page
                    {
                        Parent = mainTileContainer,
                        Caption = System.IO.Path.GetFileNameWithoutExtension(appPath),
                    };
                    windowsUIView1.ContentContainers.Add(page);
                    //创建一个 Tile 对应 PageGroup
                    CreateTile(appPath).ActivationTarget = page;

                }
            }
            //windowsUIView1.PageProperties.ShowCaption = false;
            windowsUIView1.ActivateContainer(mainTileContainer);
        }


        private Tile CreateTile(string appPath)
        {
            Tile tile = new Tile();
            tile.Properties.ItemSize = TileItemSize.Small;
            // 提取应用程序图标
            Icon appIcon = ExtractAppIcon(appPath);
            if (appIcon != null)
            {
                // 将图标转为 Bitmap，用于展示在 Tile 上
                Bitmap iconBitmap = appIcon.ToBitmap();

                // 添加图标到 Tile 元素
                tile.Elements.Add(new TileItemElement
                {
                    Image = iconBitmap,
                    ImageAlignment = TileItemContentAlignment.TopCenter, // 图标位置
                    ImageScaleMode = TileItemImageScaleMode.ZoomInside // 保持缩放比例
                });
                XtraUserControl userControl = new XtraUserControl();
                userControl.Dock = System.Windows.Forms.DockStyle.Fill;
                ExternalAppHelper externalAppHelper = new ExternalAppHelper()
                {
                    _appPath = appPath,
                    _control = userControl
                };
                externalAppHelper.Init();
                BaseDocument document = windowsUIView1.AddDocument(userControl);
                document.Caption = System.IO.Path.GetFileNameWithoutExtension(appPath);
                tile.Document = document as Document;
                tile.Click += (s, e) =>
                {   
                    if (externalAppHelper._process==null||externalAppHelper._process.HasExited)
                    {
                        try
                        {
                            externalAppHelper.Embed();

                        }
                        catch (Exception ex)
                        {
                           XtraMessageBox.Show(ex.Message);
                        }
                    }
                };
            }

            // 添加应用程序标题
            tile.Elements.Add(new TileItemElement
            {
                Text = System.IO.Path.GetFileNameWithoutExtension(appPath), // 应用标题
                TextAlignment = TileItemContentAlignment.BottomLeft, // 标题位置
            });
            tile.Appearances.Selected.BackColor = tile.Appearances.Hovered.BackColor = tile.Appearances.Normal.BackColor = Color.Transparent;
            tile.Appearances.Selected.BorderColor = tile.Appearances.Hovered.BorderColor = tile.Appearances.Normal.BorderColor = Color.Transparent;
            mainTileContainer.Items.Add(tile);
            windowsUIView1.Tiles.Add(tile);
            return tile;
        }

        private Icon ExtractAppIcon(string appPath)
        {
            try
            {
                // 提取程序图标
                return Icon.ExtractAssociatedIcon(appPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"无法提取应用图标: {ex.Message}");
                return null;
            }
        }
    }
}