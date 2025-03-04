using DevExpress.XtraEditors;
using System.Collections.Generic;

namespace Main
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraEditors.TileItemElement tileItemElement1 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            DevExpress.XtraEditors.TileItemElement tileItemElement3 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer dockingContainer1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer();
            DevExpress.XtraEditors.TileItemElement tileItemElement4 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement5 = new DevExpress.XtraEditors.TileItemElement();
            this.documentGroup1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup(this.components);
            this.document2 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
            this.tileControl1 = new DevExpress.XtraEditors.TileControl();
            this.tileGroup2 = new DevExpress.XtraEditors.TileGroup();
            this.tileItem3 = new DevExpress.XtraEditors.TileItem();
            this.tileItem7 = new DevExpress.XtraEditors.TileItem();
            this.dockManager1 = new DevExpress.XtraBars.Docking.DockManager(this.components);
            this.dockPanel1 = new DevExpress.XtraBars.Docking.DockPanel();
            this.dockPanel1_Container = new DevExpress.XtraBars.Docking.ControlContainer();
            this.documentManager1 = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
            this.tabbedView1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.ribbonStatusBar1 = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.tileItem2 = new DevExpress.XtraEditors.TileItem();
            this.tileItem1 = new DevExpress.XtraEditors.TileItem();
            this.skinRibbonGalleryBarItem1 = new DevExpress.XtraBars.SkinRibbonGalleryBarItem();
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.document2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).BeginInit();
            this.dockPanel1.SuspendLayout();
            this.dockPanel1_Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // documentGroup1
            // 
            this.documentGroup1.Items.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document[] {
            this.document2});
            // 
            // document2
            // 
            this.document2.Caption = "主页";
            this.document2.ControlName = "dockPanel1";
            this.document2.FloatLocation = new System.Drawing.Point(410, 143);
            this.document2.FloatSize = new System.Drawing.Size(475, 324);
            this.document2.Pinned = true;
            this.document2.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.False;
            this.document2.Properties.AllowFloat = DevExpress.Utils.DefaultBoolean.True;
            this.document2.Properties.AllowFloatOnDoubleClick = DevExpress.Utils.DefaultBoolean.True;
            // 
            // tileControl1
            // 
            this.tileControl1.BackColor = System.Drawing.Color.Transparent;
            this.tileControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tileControl1.Groups.Add(this.tileGroup2);
            this.tileControl1.ImeMode = System.Windows.Forms.ImeMode.Hiragana;
            this.tileControl1.Location = new System.Drawing.Point(0, 0);
            this.tileControl1.MaxId = 39;
            this.tileControl1.Name = "tileControl1";
            this.tileControl1.Size = new System.Drawing.Size(1014, 609);
            this.tileControl1.TabIndex = 0;
            this.tileControl1.Text = "tileControl1";
            this.tileControl1.VerticalContentAlignment = DevExpress.Utils.VertAlignment.Center;
            // 
            // tileGroup2
            // 
            this.tileGroup2.Items.Add(this.tileItem3);
            this.tileGroup2.Name = "tileGroup2";
            this.tileGroup2.Visible = false;
            // 
            // tileItem3
            // 
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
            tileItemElement1.Text = "Header";
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.Manual;
            tileItemElement1.TextLocation = new System.Drawing.Point(75, 0);
            tileItemElement2.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("resource.Image")));
            tileItemElement2.ImageOptions.ImageAlignment = DevExpress.XtraEditors.TileItemContentAlignment.Manual;
            tileItemElement2.ImageOptions.ImageLocation = new System.Drawing.Point(4, 8);
            tileItemElement2.ImageOptions.ImageScaleMode = DevExpress.XtraEditors.TileItemImageScaleMode.ZoomOutside;
            tileItemElement2.ImageOptions.ImageSize = new System.Drawing.Size(64, 64);
            this.tileItem3.Elements.Add(tileItemElement1);
            this.tileItem3.Elements.Add(tileItemElement2);
            this.tileItem3.Id = 26;
            this.tileItem3.ItemSize = DevExpress.XtraEditors.TileItemSize.Wide;
            this.tileItem3.Name = "tileItem3";
            // 
            // tileItem7
            // 
            tileItemElement3.Text = "tileItem7";
            this.tileItem7.Elements.Add(tileItemElement3);
            this.tileItem7.Id = 6;
            this.tileItem7.ItemSize = DevExpress.XtraEditors.TileItemSize.Medium;
            this.tileItem7.Name = "tileItem7";
            // 
            // dockManager1
            // 
            this.dockManager1.DockingOptions.SnapMode = ((DevExpress.Utils.Controls.SnapMode)((((DevExpress.Utils.Controls.SnapMode.OwnerControl | DevExpress.Utils.Controls.SnapMode.OwnerForm) 
            | DevExpress.Utils.Controls.SnapMode.Screens) 
            | DevExpress.Utils.Controls.SnapMode.SnapForms)));
            this.dockManager1.Form = this;
            this.dockManager1.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
            this.dockPanel1});
            this.dockManager1.Style = DevExpress.XtraBars.Docking2010.Views.DockingViewStyle.Light;
            this.dockManager1.TopZIndexControls.AddRange(new string[] {
            "DevExpress.XtraBars.BarDockControl",
            "DevExpress.XtraBars.StandaloneBarDockControl",
            "System.Windows.Forms.MenuStrip",
            "System.Windows.Forms.StatusStrip",
            "System.Windows.Forms.StatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
            "DevExpress.XtraBars.Ribbon.RibbonControl",
            "DevExpress.XtraBars.Navigation.OfficeNavigationBar",
            "DevExpress.XtraBars.Navigation.TileNavPane",
            "DevExpress.XtraBars.TabFormControl",
            "DevExpress.XtraBars.FluentDesignSystem.FluentDesignFormControl",
            "DevExpress.XtraBars.ToolbarForm.ToolbarFormControl"});
            // 
            // dockPanel1
            // 
            this.dockPanel1.Controls.Add(this.dockPanel1_Container);
            this.dockPanel1.DockedAsTabbedDocument = true;
            this.dockPanel1.FloatLocation = new System.Drawing.Point(410, 143);
            this.dockPanel1.FloatSize = new System.Drawing.Size(475, 324);
            this.dockPanel1.ID = new System.Guid("a0b8a589-6e41-4b2f-8bb6-8428b685832c");
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Options.ShowCloseButton = false;
            this.dockPanel1.OriginalSize = new System.Drawing.Size(200, 200);
            this.dockPanel1.SavedIndex = 1;
            this.dockPanel1.SavedMdiDocument = true;
            this.dockPanel1.SavedMdiDocumentIndex = 1;
            this.dockPanel1.Text = "主页";
            // 
            // dockPanel1_Container
            // 
            this.dockPanel1_Container.Controls.Add(this.tileControl1);
            this.dockPanel1_Container.Location = new System.Drawing.Point(0, 0);
            this.dockPanel1_Container.Name = "dockPanel1_Container";
            this.dockPanel1_Container.Size = new System.Drawing.Size(1014, 609);
            this.dockPanel1_Container.TabIndex = 0;
            // 
            // documentManager1
            // 
            this.documentManager1.ContainerControl = this;
            this.documentManager1.View = this.tabbedView1;
            this.documentManager1.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
            this.tabbedView1});
            // 
            // tabbedView1
            // 
            this.tabbedView1.DocumentGroups.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup[] {
            this.documentGroup1});
            this.tabbedView1.Documents.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseDocument[] {
            this.document2});
            dockingContainer1.Element = this.documentGroup1;
            this.tabbedView1.RootContainer.Nodes.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer[] {
            dockingContainer1});
            this.tabbedView1.Style = DevExpress.XtraBars.Docking2010.Views.DockingViewStyle.Light;
            // 
            // ribbonControl1
            // 
            this.ribbonControl1.ExpandCollapseItem.Id = 0;
            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbonControl1.ExpandCollapseItem,
            this.ribbonControl1.SearchEditItem,
            this.skinRibbonGalleryBarItem1});
            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
            this.ribbonControl1.MaxItemId = 4;
            this.ribbonControl1.Name = "ribbonControl1";
            this.ribbonControl1.QuickToolbarItemLinks.Add(this.skinRibbonGalleryBarItem1);
            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            this.ribbonControl1.ShowDisplayOptionsMenuButton = DevExpress.Utils.DefaultBoolean.True;
            this.ribbonControl1.ShowExpandCollapseButton = DevExpress.Utils.DefaultBoolean.True;
            this.ribbonControl1.ShowMoreCommandsButton = DevExpress.Utils.DefaultBoolean.True;
            this.ribbonControl1.ShowPageHeadersInFormCaption = DevExpress.Utils.DefaultBoolean.True;
            this.ribbonControl1.ShowPageHeadersMode = DevExpress.XtraBars.Ribbon.ShowPageHeadersMode.Hide;
            this.ribbonControl1.ShowToolbarCustomizeItem = false;
            this.ribbonControl1.Size = new System.Drawing.Size(1014, 27);
            this.ribbonControl1.StatusBar = this.ribbonStatusBar1;
            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
            // 
            // ribbonStatusBar1
            // 
            this.ribbonStatusBar1.Location = new System.Drawing.Point(0, 660);
            this.ribbonStatusBar1.Name = "ribbonStatusBar1";
            this.ribbonStatusBar1.Ribbon = this.ribbonControl1;
            this.ribbonStatusBar1.Size = new System.Drawing.Size(1014, 31);
            // 
            // ribbonPage2
            // 
            this.ribbonPage2.Name = "ribbonPage2";
            this.ribbonPage2.Text = "ribbonPage2";
            // 
            // tileItem2
            // 
            tileItemElement4.Text = "tileItem2";
            this.tileItem2.Elements.Add(tileItemElement4);
            this.tileItem2.Id = 23;
            this.tileItem2.ItemSize = DevExpress.XtraEditors.TileItemSize.Medium;
            this.tileItem2.Name = "tileItem2";
            // 
            // tileItem1
            // 
            tileItemElement5.Appearance.Hovered.Font = new System.Drawing.Font("Segoe UI Light", 17F);
            tileItemElement5.Appearance.Hovered.Options.UseFont = true;
            tileItemElement5.Appearance.Hovered.Options.UseTextOptions = true;
            tileItemElement5.Appearance.Hovered.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            tileItemElement5.Appearance.Hovered.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            tileItemElement5.Appearance.Normal.Font = new System.Drawing.Font("Segoe UI Light", 17F);
            tileItemElement5.Appearance.Normal.Options.UseFont = true;
            tileItemElement5.Appearance.Normal.Options.UseTextOptions = true;
            tileItemElement5.Appearance.Normal.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            tileItemElement5.Appearance.Normal.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            tileItemElement5.Appearance.Selected.Font = new System.Drawing.Font("Segoe UI Light", 17F);
            tileItemElement5.Appearance.Selected.Options.UseFont = true;
            tileItemElement5.Appearance.Selected.Options.UseTextOptions = true;
            tileItemElement5.Appearance.Selected.TextOptions.Trimming = DevExpress.Utils.Trimming.EllipsisCharacter;
            tileItemElement5.Appearance.Selected.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            tileItemElement5.MaxWidth = 600;
            tileItemElement5.Text = "The quick brown fox jumps over the lazy dog.";
            tileItemElement5.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.Manual;
            tileItemElement5.TextLocation = new System.Drawing.Point(4, 4);
            this.tileItem1.Elements.Add(tileItemElement5);
            this.tileItem1.Id = 22;
            this.tileItem1.ItemSize = DevExpress.XtraEditors.TileItemSize.Wide;
            this.tileItem1.Name = "tileItem1";
            // 
            // skinRibbonGalleryBarItem1
            // 
            this.skinRibbonGalleryBarItem1.Caption = "skinRibbonGalleryBarItem1";
            this.skinRibbonGalleryBarItem1.Id = 3;
            this.skinRibbonGalleryBarItem1.Name = "skinRibbonGalleryBarItem1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1014, 691);
            this.Controls.Add(this.ribbonStatusBar1);
            this.Controls.Add(this.ribbonControl1);
            this.IconOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("MainForm.IconOptions.LargeImage")));
            this.Name = "MainForm";
            this.Ribbon = this.ribbonControl1;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.StatusBar = this.ribbonStatusBar1;
            this.Text = "辅助工具";
            this.Load += new System.EventHandler(this.XtraForm2_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.XtraForm2_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.documentGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.document2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dockManager1)).EndInit();
            this.dockPanel1.ResumeLayout(false);
            this.dockPanel1_Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.documentManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tabbedView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TileControl tileControl1;
        private DevExpress.XtraEditors.TileItem tileItem7;
        private DevExpress.XtraBars.Docking.DockManager dockManager1;
        private DevExpress.XtraBars.Docking.DockPanel dockPanel1;
        private DevExpress.XtraBars.Docking.ControlContainer dockPanel1_Container;
        private DevExpress.XtraBars.Docking2010.DocumentManager documentManager1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView tabbedView1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup documentGroup1;
        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document document2;
        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private DevExpress.XtraEditors.TileItem tileItem2;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar1;
        private DevExpress.XtraEditors.TileItem tileItem1;
        private DevExpress.XtraEditors.TileGroup tileGroup2;
        private DevExpress.XtraEditors.TileItem tileItem3;
        private DevExpress.XtraBars.SkinRibbonGalleryBarItem skinRibbonGalleryBarItem1;

        public class TileDataSocure
        {
            public List<TileTpl> data;
            public XtraForm main;
            public TileDataSocure(XtraForm parrent)
            {
                main = parrent;
                data = new List<TileTpl>
                {
                    new TileTpl()
                    {
                        name = "脚本管理",
                        description = "脚本管理",
                        newForm = ()=> setFormCfg(new ScriptManagement.Form1()),
                        iconPath ="命令行管理工具.exe"
                    },
                    new TileTpl()
                    {
                        name = "一键登录",
                        description = "一键登录",
                        newForm = () => setFormCfg(new AutoLogin.AutoLoginForm()),
                        iconPath ="一键登录.exe"
                    },
                    new TileTpl()
                    {
                        name = "图片转ICO",
                        description = "图片转ICO",
                        newForm = () =>setFormCfg( new ImageToIco.Form1()),
                        iconPath = "图片转ICO.exe",
                    }
                };
            }

            private XtraForm setFormCfg(XtraForm form) 
            {
                form.TopLevel = false;
                form.Parent = main;
                return form;
            }

        }

        public class TileTpl
        {
            public string name { get; set; }
            public string iconPath { get; set; }
            public string description { get; set; }
            public NewFormFun newForm { get; set; }
        }

        public delegate XtraForm NewFormFun(); // 委托：返回 XtraForm
    }
}