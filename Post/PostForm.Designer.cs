namespace Post
{
    partial class PostForm
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
            this.txtPath = new DevExpress.XtraEditors.ComboBoxEdit();
            this.layoutControl2 = new DevExpress.XtraLayout.LayoutControl();
            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
            this.comContentType = new DevExpress.XtraEditors.ComboBoxEdit();
            this.comMethod = new DevExpress.XtraEditors.ComboBoxEdit();
            this.txtTimeOut = new DevExpress.XtraEditors.TextEdit();
            this.txtLoopCount = new DevExpress.XtraEditors.TextEdit();
            this.txtParam = new DevExpress.XtraEditors.MemoEdit();
            this.txtUids = new DevExpress.XtraEditors.MemoEdit();
            this.txtProtocolFilePath = new DevExpress.XtraEditors.TextEdit();
            this.btnProtpcolFileOpen = new DevExpress.XtraEditors.SimpleButton();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem8 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem9 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem10 = new DevExpress.XtraLayout.LayoutControlItem();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
            this.btnExec = new DevExpress.XtraEditors.SimpleButton();
            this.richTextBoxResult = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.txtPath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).BeginInit();
            this.layoutControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comContentType.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.comMethod.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeOut.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoopCount.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParam.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUids.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProtocolFilePath.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPath
            // 
            this.txtPath.Location = new System.Drawing.Point(90, 12);
            this.txtPath.Name = "txtPath";
            this.txtPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.txtPath.Size = new System.Drawing.Size(610, 20);
            this.txtPath.StyleController = this.layoutControl2;
            this.txtPath.TabIndex = 1;
            // 
            // layoutControl2
            // 
            this.layoutControl2.Controls.Add(this.treeList1);
            this.layoutControl2.Controls.Add(this.comContentType);
            this.layoutControl2.Controls.Add(this.comMethod);
            this.layoutControl2.Controls.Add(this.txtTimeOut);
            this.layoutControl2.Controls.Add(this.txtPath);
            this.layoutControl2.Controls.Add(this.txtLoopCount);
            this.layoutControl2.Controls.Add(this.txtParam);
            this.layoutControl2.Controls.Add(this.txtUids);
            this.layoutControl2.Controls.Add(this.txtProtocolFilePath);
            this.layoutControl2.Controls.Add(this.btnProtpcolFileOpen);
            this.layoutControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutControl2.Location = new System.Drawing.Point(2, 23);
            this.layoutControl2.Name = "layoutControl2";
            this.layoutControl2.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(904, 327, 650, 400);
            this.layoutControl2.Root = this.layoutControlGroup1;
            this.layoutControl2.Size = new System.Drawing.Size(712, 703);
            this.layoutControl2.TabIndex = 3;
            this.layoutControl2.Text = "layoutControl2";
            // 
            // treeList1
            // 
            this.treeList1.Location = new System.Drawing.Point(12, 368);
            this.treeList1.Name = "treeList1";
            this.treeList1.Size = new System.Drawing.Size(688, 323);
            this.treeList1.TabIndex = 8;
            // 
            // comContentType
            // 
            this.comContentType.Location = new System.Drawing.Point(435, 36);
            this.comContentType.Name = "comContentType";
            this.comContentType.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comContentType.Size = new System.Drawing.Size(265, 20);
            this.comContentType.StyleController = this.layoutControl2;
            this.comContentType.TabIndex = 5;
            // 
            // comMethod
            // 
            this.comMethod.Location = new System.Drawing.Point(90, 36);
            this.comMethod.Name = "comMethod";
            this.comMethod.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comMethod.Size = new System.Drawing.Size(263, 20);
            this.comMethod.StyleController = this.layoutControl2;
            this.comMethod.TabIndex = 5;
            // 
            // txtTimeOut
            // 
            this.txtTimeOut.Location = new System.Drawing.Point(434, 60);
            this.txtTimeOut.Name = "txtTimeOut";
            this.txtTimeOut.Size = new System.Drawing.Size(266, 20);
            this.txtTimeOut.StyleController = this.layoutControl2;
            this.txtTimeOut.TabIndex = 4;
            // 
            // txtLoopCount
            // 
            this.txtLoopCount.EditValue = "";
            this.txtLoopCount.Location = new System.Drawing.Point(90, 60);
            this.txtLoopCount.Name = "txtLoopCount";
            this.txtLoopCount.Size = new System.Drawing.Size(262, 20);
            this.txtLoopCount.StyleController = this.layoutControl2;
            this.txtLoopCount.TabIndex = 4;
            // 
            // txtParam
            // 
            this.txtParam.Location = new System.Drawing.Point(90, 165);
            this.txtParam.Name = "txtParam";
            this.txtParam.Size = new System.Drawing.Size(610, 199);
            this.txtParam.StyleController = this.layoutControl2;
            this.txtParam.TabIndex = 4;
            // 
            // txtUids
            // 
            this.txtUids.Location = new System.Drawing.Point(90, 84);
            this.txtUids.Name = "txtUids";
            this.txtUids.Size = new System.Drawing.Size(610, 51);
            this.txtUids.StyleController = this.layoutControl2;
            this.txtUids.TabIndex = 4;
            // 
            // txtProtocolFilePath
            // 
            this.txtProtocolFilePath.Location = new System.Drawing.Point(90, 139);
            this.txtProtocolFilePath.Name = "txtProtocolFilePath";
            this.txtProtocolFilePath.Size = new System.Drawing.Size(489, 20);
            this.txtProtocolFilePath.StyleController = this.layoutControl2;
            this.txtProtocolFilePath.TabIndex = 6;
            // 
            // btnProtpcolFileOpen
            // 
            this.btnProtpcolFileOpen.Location = new System.Drawing.Point(583, 139);
            this.btnProtpcolFileOpen.Name = "btnProtpcolFileOpen";
            this.btnProtpcolFileOpen.Size = new System.Drawing.Size(117, 22);
            this.btnProtpcolFileOpen.StyleController = this.layoutControl2;
            this.btnProtpcolFileOpen.TabIndex = 7;
            this.btnProtpcolFileOpen.Text = "载入";
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2,
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.layoutControlItem4,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7,
            this.layoutControlItem8,
            this.layoutControlItem9,
            this.layoutControlItem10});
            this.layoutControlGroup1.Name = "Root";
            this.layoutControlGroup1.OptionsItemText.TextToControlDistance = 1;
            this.layoutControlGroup1.Size = new System.Drawing.Size(712, 703);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.txtPath;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.MaxSize = new System.Drawing.Size(692, 24);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(692, 24);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(692, 24);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.Text = "接口地址";
            this.layoutControlItem2.TextSize = new System.Drawing.Size(77, 14);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.comMethod;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 24);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(345, 24);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(345, 24);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(345, 24);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.Text = "请求方式";
            this.layoutControlItem1.TextSize = new System.Drawing.Size(77, 14);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.txtLoopCount;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 48);
            this.layoutControlItem3.MaxSize = new System.Drawing.Size(344, 24);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(344, 24);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(344, 24);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.Text = "请求次数";
            this.layoutControlItem3.TextSize = new System.Drawing.Size(77, 14);
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.txtTimeOut;
            this.layoutControlItem4.Location = new System.Drawing.Point(344, 48);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(348, 24);
            this.layoutControlItem4.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.SupportHorzAlignment;
            this.layoutControlItem4.Text = "超时(ms)";
            this.layoutControlItem4.TextSize = new System.Drawing.Size(77, 14);
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.comContentType;
            this.layoutControlItem5.Location = new System.Drawing.Point(345, 24);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(347, 24);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.SupportHorzAlignment;
            this.layoutControlItem5.Text = "Content-Type";
            this.layoutControlItem5.TextSize = new System.Drawing.Size(77, 14);
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.txtUids;
            this.layoutControlItem6.CustomizationFormText = "用户";
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 72);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(692, 55);
            this.layoutControlItem6.Text = "用户";
            this.layoutControlItem6.TextSize = new System.Drawing.Size(77, 14);
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.txtProtocolFilePath;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 127);
            this.layoutControlItem7.MaxSize = new System.Drawing.Size(571, 26);
            this.layoutControlItem7.MinSize = new System.Drawing.Size(571, 26);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(571, 26);
            this.layoutControlItem7.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem7.Text = "协议地址";
            this.layoutControlItem7.TextSize = new System.Drawing.Size(77, 14);
            // 
            // layoutControlItem8
            // 
            this.layoutControlItem8.Control = this.btnProtpcolFileOpen;
            this.layoutControlItem8.Location = new System.Drawing.Point(571, 127);
            this.layoutControlItem8.Name = "layoutControlItem8";
            this.layoutControlItem8.Size = new System.Drawing.Size(121, 26);
            this.layoutControlItem8.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem8.TextVisible = false;
            // 
            // layoutControlItem9
            // 
            this.layoutControlItem9.Control = this.txtParam;
            this.layoutControlItem9.Location = new System.Drawing.Point(0, 153);
            this.layoutControlItem9.MaxSize = new System.Drawing.Size(692, 203);
            this.layoutControlItem9.MinSize = new System.Drawing.Size(692, 203);
            this.layoutControlItem9.Name = "layoutControlItem9";
            this.layoutControlItem9.Size = new System.Drawing.Size(692, 203);
            this.layoutControlItem9.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem9.Text = "请求协议";
            this.layoutControlItem9.TextSize = new System.Drawing.Size(77, 14);
            // 
            // layoutControlItem10
            // 
            this.layoutControlItem10.Control = this.treeList1;
            this.layoutControlItem10.Location = new System.Drawing.Point(0, 356);
            this.layoutControlItem10.Name = "layoutControlItem10";
            this.layoutControlItem10.Size = new System.Drawing.Size(692, 327);
            this.layoutControlItem10.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem10.TextVisible = false;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.layoutControl2);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl1.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(716, 728);
            this.groupControl1.TabIndex = 4;
            this.groupControl1.Text = "请求设置";
            // 
            // groupControl2
            // 
            this.groupControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupControl2.Controls.Add(this.checkEdit1);
            this.groupControl2.Controls.Add(this.btnExec);
            this.groupControl2.GroupStyle = DevExpress.Utils.GroupStyle.Light;
            this.groupControl2.Location = new System.Drawing.Point(722, 0);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(401, 728);
            this.groupControl2.TabIndex = 5;
            this.groupControl2.Text = "请求结果";
            // 
            // checkEdit1
            // 
            this.checkEdit1.Location = new System.Drawing.Point(100, 26);
            this.checkEdit1.Name = "checkEdit1";
            this.checkEdit1.Properties.Caption = "清理";
            this.checkEdit1.Size = new System.Drawing.Size(75, 20);
            this.checkEdit1.TabIndex = 8;
            // 
            // btnExec
            // 
            this.btnExec.Location = new System.Drawing.Point(6, 25);
            this.btnExec.Name = "btnExec";
            this.btnExec.Size = new System.Drawing.Size(88, 22);
            this.btnExec.TabIndex = 7;
            this.btnExec.Text = "运行";
            // 
            // richTextBoxResult
            // 
            this.richTextBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxResult.Location = new System.Drawing.Point(724, 52);
            this.richTextBoxResult.Name = "richTextBoxResult";
            this.richTextBoxResult.Size = new System.Drawing.Size(397, 674);
            this.richTextBoxResult.TabIndex = 9;
            this.richTextBoxResult.Text = "";
            // 
            // PostForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1123, 728);
            this.Controls.Add(this.richTextBoxResult);
            this.Controls.Add(this.groupControl2);
            this.Controls.Add(this.groupControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "PostForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.txtPath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl2)).EndInit();
            this.layoutControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comContentType.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.comMethod.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTimeOut.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoopCount.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtParam.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUids.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtProtocolFilePath.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.ComboBoxEdit txtPath;
        private DevExpress.XtraLayout.LayoutControl layoutControl2;
        private DevExpress.XtraEditors.TextEdit txtTimeOut;
        private DevExpress.XtraEditors.TextEdit txtLoopCount;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.MemoEdit txtUids;
        private DevExpress.XtraEditors.ComboBoxEdit comContentType;
        private DevExpress.XtraEditors.ComboBoxEdit comMethod;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.TextEdit txtProtocolFilePath;
        private DevExpress.XtraEditors.SimpleButton btnProtpcolFileOpen;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem8;
        private DevExpress.XtraEditors.MemoEdit txtParam;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem9;
        private DevExpress.XtraTreeList.TreeList treeList1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem10;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton btnExec;
        private System.Windows.Forms.RichTextBox richTextBoxResult;
        private DevExpress.XtraEditors.CheckEdit checkEdit1;
    }
}

