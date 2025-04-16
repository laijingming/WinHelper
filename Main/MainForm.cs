using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraBars.Docking2010.Views;
using AutoLogin;
using DevExpress.XtraSplashScreen;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using AJLibrary;
using System.IO;

namespace Main
{
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        TileDataSocure dataSocure { get; set; }
        public MainForm()
        {
            InitializeComponent();
            dataSocure = new TileDataSocure(this);
            dockPanel1.KeyDown += XtraForm2_KeyDown;
            tabbedView1.DocumentClosing += TabbedView1_DocumentClosing;
            this.FormClosing += MainForm_FormClosing;
            this.KeyPreview = true; // 确保窗体能够捕获键盘事件
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CloseAllDocument();
            SaveTileControlLayout();
        }

        private void XtraForm2_Load(object sender, EventArgs e)
        {
            InitTiles();
            RestoreTileControlLayout();

        }

        private void SaveTileControlLayout()
        {
            try
            {

                // 保存TileControl的布局到XML文件
                tileControl1.SaveLayoutToXml("./file/TileControlLayout.xml");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("保存布局失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestoreTileControlLayout()
        {
            try
            {
                string layoutFilePath = "./file/TileControlLayout.xml";

                // 检查布局文件是否存在
                if (File.Exists(layoutFilePath))
                {   
                    // 从XML文件恢复TileControl的布局
                    tileControl1.RestoreLayoutFromXml(layoutFilePath);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("恢复布局失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


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
            if (e.KeyCode == Keys.Escape && (currentTime - lastKeyPressTime) <= doublePressInterval)
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

        private void CloseAllDocument() 
        {
            for (int i = 0; i < Application.OpenForms.Count; i++)
            {
                XtraForm xtraForm = (XtraForm)Application.OpenForms[i];
                if (xtraForm != this)
                {
                    xtraForm.Close(); // 关闭窗体
                    xtraForm.Dispose(); // 释放资源
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



        #region createform
        private void InitTiles()
        {
            tileControl1.Groups.Clear();
            tileControl1.LookAndFeel.UseDefaultLookAndFeel = true;
            int i = 0;
            foreach (TileTpl tileTpl in dataSocure.data)
            {
                TileGroup tileGroup = new TileGroup();
                TileItem tile = CreateTileItemElement(tileTpl);
                tile.Id = i++; // ❗重要：恢复时靠这个匹配 tile
                tileGroup.Items.Add(tile);
                tileControl1.Groups.Add(tileGroup);
                tile.ItemClick += (sender, e) =>
                {   

                    SplashScreenManager.ShowDefaultWaitForm("正在打开", "请等待......");
                    ShowFormByName(tileTpl);
                    SplashScreenManager.CloseForm();
                };
            }
        }

        private TileItem CreateTileItemElement(TileTpl tileTpl)
        {
            TileItem tile = new TileItem();
            TileItemElement tileItemElement1 = new TileItemElement();
            tileItemElement1.Text = tileTpl.name;
            tileItemElement1.TextAlignment = TileItemContentAlignment.MiddleCenter;
            tile.Elements.Add(tileItemElement1);
            tile.ItemSize = TileItemSize.Default;
            return tile;
        }

        public void ShowFormByName(TileTpl tileTpl)
        {
            // 检查是否已经打开了相同路径的文档
            DockPanel dockPanel;
            foreach (var doc in tabbedView1.Documents)
            {
                dockPanel = doc.Control as DockPanel;
                if (dockPanel != null && dockPanel.Text == tileTpl.name)
                {
                    // 如果已经存在相同路径的文档，则激活该文档并返回
                    tabbedView1.ActivateDocument(doc.Control);
                    return;
                }
            }

            XtraForm frm = tileTpl.newForm();// 实例化窗体
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