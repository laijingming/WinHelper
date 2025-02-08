using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraBars.Docking2010.Views;

namespace Main
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private Dictionary<string, Func<XtraForm>> form_dic = new Dictionary<string, Func<XtraForm>>()
        {
            { "脚本管理",()=>new ScriptManagement.Form1() },
            { "图片转ICO", ()=>new ImageToIco.Form1() }
        };

        TileDataSocure dataSocure { get; set; }
        public MainForm()
        {
            InitializeComponent();
            dataSocure = new TileDataSocure();
            dockPanel1.KeyDown += XtraForm2_KeyDown;
            tabbedView1.DocumentClosing += TabbedView1_DocumentClosing; ;
            this.KeyPreview = true; // 确保窗体能够捕获键盘事件
            InitTiles();
        }

        


        private void XtraForm2_Load(object sender, EventArgs e)
        {
        }

        #region event 
        /// <summary>
        /// 关闭panl释放窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TabbedView1_DocumentClosing(object sender, DocumentCancelEventArgs e)
        {
            if (e.Document.Control is DockPanel dockPanel)
            {
                // 获取 DockPanel 内的 XtraForm
                if (dockPanel.Controls.Count > 0 && dockPanel.Controls[0] is XtraForm xtraForm)
                {
                    xtraForm.Close(); // 关闭窗体
                    xtraForm.Dispose(); // 释放资源
                }
            }
        }

        private DateTime lastKeyPressTime = DateTime.MinValue;
        private Keys lastKeyPressed = Keys.Escape;
        private readonly TimeSpan doublePressInterval = TimeSpan.FromMilliseconds(500); // 设定的双击时间间隔

        private void XtraForm2_KeyDown(object sender, KeyEventArgs e)
        {

            DateTime currentTime = DateTime.Now;
            // 检查是否在设定的时间间隔内按下相同的键
            if (e.KeyCode == lastKeyPressed && (currentTime - lastKeyPressTime) <= doublePressInterval)
            {
                // 在此处处理连续两次按下相同按键的逻辑
                tabbedView1.ActivateDocument(dockPanel1);
            }
            // 更新上次按键的时间和键值
            lastKeyPressTime = currentTime;
            lastKeyPressed = e.KeyCode;


            //关闭当前文档
            if (e.Control && e.KeyCode == Keys.W)
            {
                CloseCurrentAndActivatePreviousDocument();
            }

            //添加alt + 上下左右快捷键
            if (e.Alt)
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        PreDocument();
                        break;
                    case Keys.Up:
                        break;
                    case Keys.Right:
                        NextDocument();
                        break;
                    case Keys.Down:
                        break;
                    default:
                        break;
                }
            }
        }


        /// <summary>
        /// 关闭当前文档
        /// </summary>
        private void CloseCurrentAndActivatePreviousDocument()
        {
            // 获取当前活动的文档
            var currentDocument = tabbedView1.ActiveDocument;

            if (currentDocument != null)
            {
                // 获取当前文档的索引
                int currentIndex = tabbedView1.Documents.IndexOf(currentDocument);

                //主页不能关闭
                if (currentIndex == 0)
                {
                    return;
                }

                // 关闭当前文档
                currentDocument.Form.Close();

                // 计算上一个文档的索引
                int previousIndex = currentIndex - 1;

                // 如果存在上一个文档，则激活它
                if (previousIndex >= 0)
                {
                    var previousDocument = tabbedView1.Documents[previousIndex];
                    tabbedView1.Controller.Activate(previousDocument);
                }
                else if (tabbedView1.Documents.Count > 0)
                {
                    // 如果没有上一个文档，但仍有其他文档，激活第一个文档
                    var firstDocument = tabbedView1.Documents[0];
                    tabbedView1.Controller.Activate(firstDocument);
                }
            }
        }

        /// <summary>
        /// 上一个文档
        /// </summary>
        private void PreDocument()
        {
            // 获取当前活动的文档
            var currentDocument = tabbedView1.ActiveDocument;

            if (currentDocument != null)
            {
                // 获取当前文档的索引
                int currentIndex = tabbedView1.Documents.IndexOf(currentDocument);

                // 计算上一个文档的索引
                int newIndex = currentIndex - 1;
                if (newIndex >= 0)
                {
                    var previousDocument = tabbedView1.Documents[newIndex];
                    tabbedView1.Controller.Activate(previousDocument);
                }
                else
                {
                    //激活第一个文档
                    var firstDocument = tabbedView1.Documents[tabbedView1.Documents.Count - 1];
                    tabbedView1.Controller.Activate(firstDocument);
                }
            }
        }

        /// <summary>
        /// 下一个文档
        /// </summary>
        private void NextDocument()
        {
            // 获取当前活动的文档
            var currentDocument = tabbedView1.ActiveDocument;

            if (currentDocument != null)
            {
                // 获取当前文档的索引
                int currentIndex = tabbedView1.Documents.IndexOf(currentDocument);

                // 计算上一个文档的索引
                int newIndex = currentIndex + 1;
                if (tabbedView1.Documents.Count > newIndex)
                {
                    var previousDocument = tabbedView1.Documents[newIndex];
                    tabbedView1.Controller.Activate(previousDocument);
                }
                else
                {
                    //激活第一个文档
                    var firstDocument = tabbedView1.Documents[0];
                    tabbedView1.Controller.Activate(firstDocument);
                }
            }
        }

        #endregion



        #region createform
        private void InitTiles()
        {
            TileGroup tileGroup = new TileGroup();
            foreach (TileTpl tileTpl in dataSocure.data)
            {
                TileItem tile = CreateTileItemElement(tileTpl);
                tileGroup.Items.Add(tile);
                tileControl1.Groups.Add(tileGroup);
                tile.ItemClick += (sender, e) =>
                {
                    ShowFormByName(tileTpl);
                };
            }
        }

        private TileItem CreateTileItemElement(TileTpl tileTpl)
        {
            TileItem tile = new TileItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            TileItemElement tileItemElement1 = new TileItemElement();
            TileItemElement tileItemElement2 = new TileItemElement();
            tileItemElement1.Appearance.Hovered.Font = new System.Drawing.Font("Segoe UI Light", 17F);
            tileItemElement1.Appearance.Hovered.Options.UseFont = true;
            tileItemElement1.Appearance.Hovered.Options.UseTextOptions = true;
            tileItemElement1.Appearance.Hovered.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            tileItemElement1.Appearance.Hovered.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            tileItemElement1.Appearance.Normal.Font = new System.Drawing.Font("Segoe UI Light", 17F);
            tileItemElement1.Appearance.Normal.Options.UseFont = true;
            tileItemElement1.Appearance.Normal.Options.UseTextOptions = true;
            tileItemElement1.Appearance.Normal.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            tileItemElement1.Appearance.Normal.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            tileItemElement1.Appearance.Selected.Font = new System.Drawing.Font("Segoe UI Light", 17F);
            tileItemElement1.Appearance.Selected.Options.UseFont = true;
            tileItemElement1.Appearance.Selected.Options.UseTextOptions = true;
            tileItemElement1.Appearance.Selected.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            tileItemElement1.Appearance.Selected.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            tileItemElement1.MaxWidth = 160;
            tileItemElement1.Text = tileTpl.name;
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.Manual;
            tileItemElement1.TextLocation = new System.Drawing.Point(75, 0);
            if (tileTpl.iconPath != null)
            {
                tileItemElement2.ImageOptions.Image = ExtractAppIcon(tileTpl.iconPath).ToBitmap();
            }
            else
            {
                tileItemElement2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            }
            tileItemElement2.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.Manual;
            tileItemElement2.ImageOptions.ImageLocation = new System.Drawing.Point(4, 8);
            tileItemElement2.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.ZoomOutside;
            tileItemElement2.ImageOptions.ImageSize = new System.Drawing.Size(64, 64);
            tile.Elements.Add(tileItemElement1);
            tile.Elements.Add(tileItemElement2);
            tile.Appearance.BackColor = Color.FromArgb(140, 140, 140);
            tile.Appearance.BorderColor = Color.FromArgb(140, 140, 140);
            tile.ItemSize = DevExpress.XtraEditors.TileItemSize.Wide;
            return tile;
        }

        public void ShowFormByName(TileTpl tileTpl)
        {
            // 检查是否已经打开了相同路径的文档
            DevExpress.XtraBars.Docking.DockPanel dockPanel;
            foreach (var doc in tabbedView1.Documents)
            {
                dockPanel = doc.Control as DevExpress.XtraBars.Docking.DockPanel;
                if (dockPanel != null && dockPanel.Text == tileTpl.name)
                {
                    // 如果已经存在相同路径的文档，则激活该文档并返回
                    tabbedView1.ActivateDocument(doc.Control);
                    return;
                }
            }

            XtraForm frm = tileTpl.NewForm();// 实例化窗体
            frm.TopLevel = false;
            frm.FormBorderStyle = FormBorderStyle.None;
            frm.Dock = DockStyle.Fill;

            dockPanel = new DockPanel();
            dockPanel.Text = tileTpl.name;
            dockPanel.Controls.Add(frm);

            tabbedView1.AddDocument(dockPanel);
            tabbedView1.ActivateDocument(dockPanel);
            frm.Show();

        }



        /// <summary>
        ///  // 提取应用程序图标
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
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
        #endregion


    }
}