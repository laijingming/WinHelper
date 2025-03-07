using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using AJLibrary;
using System.Drawing;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using System.Windows.Forms;
using System.IO;
using DevExpress.Utils.Menu;
using DevExpress.XtraTreeList.Menu;
using ScriptManagement.Class;
using DevExpress.XtraGrid.Views.Tile;
using DevExpress.Utils;
using AutoLogin;
using System.Text.RegularExpressions;
using static AJLibrary.Cmd;

namespace ScriptManagement
{
    public partial class Form1 : XtraForm
    {
        private string layoutFilePath = "./file/dockManagerLayout.xml"; // 保存布局的文件路径
        FiddlerHelper fiddlerHelper = FiddlerHelper.getIns;
        AutologinModel autologinModel = new AutologinModel();
        public TaskManage taskManage = new TaskManage();
        DevTreeListInit treeListInitial;
        DomainModel domain;

        public Form1()
        {
            InitializeComponent();
            Cmd.displayPartialResult = DisplayPartialResult;
            // 检查是否有保存的布局文件
            if (File.Exists(layoutFilePath))
            {
                try
                {
                    // 从 XML 文件中恢复布局
                    dockManager1.RestoreLayoutFromXml(layoutFilePath);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("无法加载布局：" + ex.Message);
                }
            }
            InitTree();
            InitGrid();
            InitMemo();
            InitFiddler();
        }
        #region 事件


        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            memoEdit1.Focus();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {  
            try
            {
                fiddlerHelper.CloseFiddler();
                autologinModel.CloseDriver();
                // 将当前布局保存为 XML
                dockManager1.SaveLayoutToXml(layoutFilePath);

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("无法保存布局：" + ex.Message);
            }
        }
        #endregion

        #region tileGrid

        private ImageCollection imageCollection;
        private ContextMenuStrip contextMenuStrip;
        ToolStripMenuItem topMenuItem;
        ToolStripMenuItem unTopMenuItem;
        ToolStripMenuItem setNicknameMenuItem;
        private void InitGrid()
        {
            gridControl1.DataSource = CommandLog.getIns.data;


            // 设置 TileView 为卡片模式
            tileView1.OptionsTiles.RowCount = 0; // 自动计算行数
            tileView1.OptionsTiles.Orientation = Orientation.Vertical; // 自动计算行数
            tileView1.OptionsTiles.ItemSize = new Size(200, 100); // 卡片大小

            //排序
            tileView1.Columns["to_up"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            tileView1.Columns["num"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
            tileView1.Columns["time"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;

            // **定义 TileView 模板**
            tileView1.TileTemplate.Clear();

            // 定义卡片模板
            tileView1.TileTemplate.Add(new TileViewItemElement()
            {
                Column = tileView1.Columns["name"],
                TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter
            });


            // 创建 ImageCollection 存储图标
            imageCollection = new ImageCollection();
            imageCollection.AddImage(Image.FromFile("./file/Upload_32x32.png"));  // 置顶图标


            // 创建右键菜单
            contextMenuStrip = new ContextMenuStrip();
            topMenuItem = new ToolStripMenuItem("置顶");
            topMenuItem.Click += TopMenuItem_Click;
            contextMenuStrip.Items.Add(topMenuItem);

            unTopMenuItem = new ToolStripMenuItem("取消置顶");
            unTopMenuItem.Click += UnTopMenuItem_Click; ;
            contextMenuStrip.Items.Add(unTopMenuItem);

            setNicknameMenuItem = new ToolStripMenuItem("设置昵称");
            setNicknameMenuItem.Click += SetNicknameMenuItem_Click;
            contextMenuStrip.Items.Add(setNicknameMenuItem);  

            tileView1.DoubleClick += TileView1_DoubleClick;
            tileView1.ItemRightClick += TileView1_ItemRightClick;
            tileView1.ItemCustomize += TileView1_ItemCustomize;
        }

        /// <summary>
        /// 设置昵称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetNicknameMenuItem_Click(object sender, EventArgs e)
        {
            CommandLogModel model = (CommandLogModel)tileView1.GetFocusedRow();
            string nickname = XtraInputBox.Show("请输入昵称", "设置昵称", "");
            if (string.IsNullOrWhiteSpace(nickname))
            {
                return;
            }
            CommandLog.getIns.SetNickname(model.name,nickname);
            tileView1.RefreshData();
        }

        /// <summary>
        /// 处理取消置顶逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnTopMenuItem_Click(object sender, EventArgs e)
        {
            CommandLogModel model = (CommandLogModel)tileView1.GetFocusedRow();
            CommandLog.getIns.SetToUp(model.name, false);
            tileView1.RefreshData();
        }

        /// <summary>
        /// 处理组合命令置顶逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void TopMenuItem_Click(object sender, EventArgs e)
        {
            CommandLogModel model = (CommandLogModel)tileView1.GetFocusedRow();
            CommandLog.getIns.SetToUp(model.name);
            tileView1.RefreshData();
        }

        /// <summary>
        /// 右键显示置顶菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TileView1_ItemRightClick(object sender, TileViewItemClickEventArgs e)
        {
            CommandLogModel model = (CommandLogModel)tileView1.GetFocusedRow();
            if (model != null)
            {
                if (model.to_up>0)
                {
                    unTopMenuItem.Visible = true;
                }
                else
                {
                    unTopMenuItem.Visible = false;
                }
                contextMenuStrip.Show(Cursor.Position);//当前鼠标所在位置
            }
        }



        /// <summary>
        /// 动态加载置顶图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TileView1_ItemCustomize(object sender, TileViewItemCustomizeEventArgs e)
        {

            //添加昵称
            var nickname = (string)tileView1.GetRowCellValue(e.RowHandle, "nickname");
            if (nickname!=null)
            {
                // 定义卡片模板
                e.Item.Elements.Add(new TileViewItemElement()
                {
                    Text = nickname,
                    TextAlignment = TileItemContentAlignment.TopCenter
                });
            }

            var isPinned = Convert.ToInt16(tileView1.GetRowCellValue(e.RowHandle, "to_up"));
            if (isPinned > 0)
            {   
                e.Item.Elements.Add(new TileViewItemElement()
                {
                    Image = imageCollection.Images[0], // 默认图标
                    ImageAlignment = TileItemContentAlignment.TopLeft,
                });
            }

        }

        /// <summary>
        /// 双击执行组合任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TileView1_DoubleClick(object sender, EventArgs e)
        {
            CommandLogModel model = (CommandLogModel)tileView1.GetFocusedRow();
            string[] name_arr = model.name.Split('、');
            foreach (string name in name_arr)
            {
                TreeListNode node = treeList1.FindNode(x => x.GetValue("name").ToString() == name);
                taskManage.AddTaskListByNode(node);
            }
            TaskRun();
        }

        #endregion

        #region treelist



        private void InitTree()
        {   
            treeListInitial = new DevTreeListInit(treeList1);
            List<CommandModel> commandModels = CommandCache.getIns.data;
            treeList1.DataSource = commandModels;
            treeList1.ChildListFieldName = "children";

            //type文本显示
            treeList1.CustomColumnDisplayText += TreeList1_CustomColumnDisplayText;
            //改变type颜色
            treeList1.NodeCellStyle += TreeList1_NodeCellStyle;

            repositoryItemButtonEdit1.ButtonClick += ButtonEdit_ButtonClick;

            //treeList1.Columns["name"].BestFit();
            treeList1.ExpandAll();

            TreeListDev(treeList1, new Dictionary<string, DevExpress.XtraTreeList.PopupMenuShowingEventHandler>()
            {
                {"执行选择", (se, e2) => //按照选中顺序执行节点
                    {
                        Cmd.TaskCompletionSourceList.Clear();//清空上一次执行任务结果
                        foreach (TreeListNode item in treeListInitial.treeListNodes)
                        {
                            taskManage.AddTaskListByNode(item);
                        }
                        TaskRun();
                    }
                },
                {"取消所有选中状态", (se, e2) =>
                    {
                        treeList1.UncheckAll();
                        treeListInitial.treeListNodes.Clear();
                        Cmd.TaskCompletionSourceList.Clear();
                    }
                },
            });
            
        }


        private void ButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            taskManage.AddTaskListByNode(treeList1.FocusedNode);
            TaskRun();
        }


        private void TreeList1_NodeCellStyle(object sender, DevExpress.XtraTreeList.GetCustomNodeCellStyleEventArgs e)
        {
            if (!e.Node.HasChildren&&e.Column.FieldName == "type")
            {
                e.Node.GetValue(e.Column).ToString();
                switch (int.Parse(e.Node.GetValue(e.Column).ToString()))
                {
                    case 0:
                    case 1:
                        e.Appearance.BackColor = Color.LightBlue;
                        e.Appearance.ForeColor = Color.Black;
                        break;
                    case 2:
                    case 3:
                        e.Appearance.BackColor = Color.LightCoral;
                        e.Appearance.ForeColor = Color.Black;
                        break;
                    default:
                        e.Appearance.BackColor = Color.LightGray;
                        e.Appearance.ForeColor = Color.Black;
                        break;
                }
            }
        }

        private void TreeList1_CustomColumnDisplayText(object sender, DevExpress.XtraTreeList.CustomColumnDisplayTextEventArgs e)
        {
            if (e.Column.FieldName == "type")
            {
                if (e.Node.HasChildren)
                {
                    e.DisplayText = "";
                    return;
                }
                int val = int.Parse(e.Value.ToString());
                switch (val)
                {
                    case 0:
                    case 1:
                        e.DisplayText = "异步";
                        break;
                    case 2:
                    case 3:
                        e.DisplayText = "阻塞";
                        break;
                    default:
                        e.DisplayText = "未知类型";
                        break;
                }
            }
        }

        public void TreeListDev(TreeList tree, Dictionary<string, PopupMenuShowingEventHandler> memuItemClickDic = null)
        {
            tree.PopupMenuShowing += (sender, e) => {
                // 获取右键菜单
                if (e.Menu is TreeListMenu menu)
                {
                    foreach (DXMenuItem item in menu.Items)
                    {
                        if (item.Caption == "Full Expand")
                        {
                            item.Caption = "全部展开";
                        }
                        else if (item.Caption == "Full Collapse")
                        {
                            item.Caption = "全部折叠";
                        }
                        else if (item.Caption == "Collapse")
                        {
                            item.Caption = "折叠";
                        }
                        else if (item.Caption == "Expand")
                        {
                            item.Caption = "展开";
                        }

                    }
                    if (memuItemClickDic != null) foreach (var item in memuItemClickDic)
                        {
                            // 添加自定义按钮
                            DXMenuItem execSelectedItem = new DXMenuItem(item.Key);
                            execSelectedItem.Click += (se, e2) => item.Value(se, e);
                            // 将自定义按钮插入到菜单的末尾
                            menu.Items.Add(execSelectedItem);
                        }

                }
            };
        }

        public void TaskRun() 
        {
            isAuth = false;
            taskManage.Run();
            tileView1.RefreshData();
        } 
        #endregion

        #region memoedit
        private int promptStartIndex;//记录提示符的位置
        private void InitMemo()
        {
            memoEdit1.Properties.ScrollBars = ScrollBars.Both;
            memoEdit1.Properties.ContextMenuStrip = new ContextMenuStrip();
        }


        private void ShowPrompt(bool is_init=false)
        {   
            if (is_init)
            {
                memoEdit1.AppendText(Cmd.work_path+">");
                promptStartIndex = memoEdit1.Text.Length; // 记录提示符的位置
                memoEdit1.Select(promptStartIndex, 0);
            }
            else
            {
                memoEdit1.BeginInvoke(new Action(() => {
                    memoEdit1.AppendText(Environment.NewLine + Cmd.work_path + "> ");
                    promptStartIndex = memoEdit1.Text.Length;
                    memoEdit1.Select(promptStartIndex, 0);
                }));
            }
        }

        private void MemoEdit1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; //阻止默认的回车行为（换行）
                string command = memoEdit1.Text.Substring(promptStartIndex).Trim();
                if (!string.IsNullOrEmpty(command))
                {
                    Cmd.ExecCommandCom(command);
                    memoEdit1.AppendText(Environment.NewLine);

                }
                else
                {
                    ShowPrompt();
                }
            }// 确保用户不能移动光标到提示符之前的区域
            else if (e.KeyCode == Keys.Back && memoEdit1.SelectionStart <= promptStartIndex)
            {
                e.SuppressKeyPress = true; // 阻止删除提示符之前的内容
            }// 确保用户不能剪切光标到提示符之前的区域
            else if (e.KeyCode == Keys.X && e.Modifiers==Keys.Control && memoEdit1.SelectionStart < promptStartIndex) 
            {
                e.SuppressKeyPress = true; // 阻止剪切提示符之前的内容
            }
        }

        bool isAuth = false;//本次执行是否已经获取权限
        string lastCommand = "";
        public void DisplayPartialResult(int status, string partialResult = "", string fixedCommand = "")
        {
            try
            {
                if (Cmd.STATUS_START == status)
                {
                    lastCommand = partialResult;
                }

                DateTime dt = DateTime.Now;
                //更新 UI
                BeginInvoke(new Action(() =>
                {
                    string newPartialResult = partialResult.Replace("\r\n", Environment.NewLine);
                    if (newPartialResult == partialResult)
                    {
                        newPartialResult += Environment.NewLine;
                    }
                    memoEdit1.AppendText(dt.ToString() + "： " + newPartialResult);

                    // 处理重试逻辑
                    if (partialResult.IndexOf("作业不存在") > -1 && !isAuth)
                    {
                        isAuth = true;
                        AutoLoginAsync(lastCommand);
                    }

                }));

               
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }



        #endregion

        #region 自动登录
        public  void AutoLoginAsync(string lastCommand)
        {
            fiddlerHelper.StartFiddler();
            DisplayPartialResult(STATUS_RUNNING, "开始捕获auth...");
            DisplayPartialResult(STATUS_RUNNING, autologinModel.Run(domain));
            DisplayPartialResult(STATUS_RUNNING, "捕获结束");
            fiddlerHelper.StopFiddler();// 停止 Fiddler 捕获
            this.Show();
            CommandModel model = CommandCache.getIns.GetDevop();
            if (model !=null)
            {
                //重新执行命令
                string[] lastCommandParts = lastCommand.Split(' ');
                ExecAsync(model.command+" "+ lastCommandParts[lastCommandParts.Length - 1]);
            }
        }
        #endregion

        #region fiddler

        private void InitFiddler() 
        {
            domain = autologinModel.GetDomainByName("DEVOPS");
            fiddlerHelper.BeforeRequestFun = (info) =>
            {
                // 如果匹配目标 URL
                if (info.url.IndexOf(domain.fiddlerUrl) > -1)
                {
                    Match match = Regex.Match(info.header, @"Authorization:\s*Bearer\s*(.+)");
                    if (match.Success)
                    {
                        // 输出日志
                        //DisplayPartialResult(STATUS_RUNNING, "匹配到目标请求：" + info.url);
                        //DisplayPartialResult(STATUS_RUNNING, "匹配到Header：" + info.header);

                        // 更新缓存中的命令
                        CommandCache.getIns.SaveDevop(match.Groups[1].Value);
                        DisplayPartialResult(STATUS_RUNNING, "保存auth成功");
                    }
                }
            };
        }

       
        #endregion

    }
}
