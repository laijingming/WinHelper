using DevExpress.XtraTreeList.Nodes;
using System;
using System.Collections.Generic;
using AJLibrary;
using System.Drawing;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList;
using DevExpress.Data.ExpressionEditor;
using System.Windows.Forms;
using System.Diagnostics;
using static AJLibrary.Cmd;
using System.IO;
using DevExpress.Utils.Menu;
using DevExpress.XtraTreeList.Menu;
using ScriptManagement.Class;using DevExpress.Utils.Extensions;
using System.Linq;
using System.Data;


namespace ScriptManagement
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        private string resoure_path = "./file/command.json";
        private string layoutFilePath = "./file/dockManagerLayout.xml"; // 保存布局的文件路径
        public TaskManage taskManage = new TaskManage();
        DevTreeListInit treeListInitial;
        public Form1()
        {
            InitializeComponent();
            treeListInitial = new DevTreeListInit(treeList1);
            Cmd.displayPartialResult = DisplayPartialResult;
            InitTree();
            InitMemo();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            memoEdit1.Focus();
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // 将当前布局保存为 XML
                dockManager1.SaveLayoutToXml(layoutFilePath);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("无法保存布局：" + ex.Message);
            }
        }

        #region treelist
        private void InitTree()
        {
            List<CommandModel> commandModels = JsonHelper.DeserializeJsonFileToType<List<CommandModel>>(resoure_path);
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


            repositoryItemButtonEdit2.ButtonClick += ButtonEdit_ButtonClick2;
            treeList2.DataSource = CommandLog.getIns.data;
            treeList2.Columns["time"].SortOrder = SortOrder.Descending;
            treeList2.Columns["to_up"].SortOrder = SortOrder.Descending;
            treeList2.Columns["num"].SortOrder = SortOrder.Descending;
            treeList2.Columns["time"].Visible = false;
            treeList2.Columns["to_up"].Visible = false;
            treeList2.Columns["num"].Visible = false;
            treeList2.OptionsView.AutoWidth = true;
            treeList2.Columns["name"].BestFit();
            treeList2.CustomDrawNodeCell += TreeList2_CustomDrawNodeCell;

            TreeListDev(treeList2, new Dictionary<string, DevExpress.XtraTreeList.PopupMenuShowingEventHandler>()
            {
                {"置顶", (se, e2) => //按照选中顺序执行节点
                    {   
                        if (treeList2.FocusedNode != null)
                        {
                            string name = treeList2.FocusedNode.GetValue("name").ToString();
                            CommandLog.getIns.SetToUp(name);
                            treeList2.RefreshDataSource();
                        }
                        
                    }
                },
                {"取消置顶", (se, e2) =>
                    {
                        if (treeList2.FocusedNode != null)
                        {
                            string name = treeList2.FocusedNode.GetValue("name").ToString();
                            CommandLog.getIns.SetToUp(name,false);
                            treeList2.RefreshDataSource();
                        }
                        
                    }
                },
            });
        }

        private void TreeList2_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs e)
        {
            // 假设 "to_up" 是表示节点是否置顶的字段
            int isTopNode = Convert.ToInt32(e.Node.GetValue("to_up"));

            if (isTopNode>0 && e.Column.FieldName == "name") // 只在特定列绘制图标
            {
                string originalText = e.CellText;
                string topText = " [置顶]";

                // 使用不同的字体样式和颜色绘制“置顶”标识
                Font boldFont = new Font(e.Appearance.Font, FontStyle.Bold);
                Brush topBrush = Brushes.Red; // 红色刷子来显示“置顶”字样

                // 绘制原始文本
                e.Graphics.DrawString(originalText, e.Appearance.Font, Brushes.Black, e.Bounds.X, e.Bounds.Y+4);

                // 在后面添加显眼的“置顶”标识
                e.Graphics.DrawString(topText, boldFont, topBrush, e.Bounds.X + e.Graphics.MeasureString(originalText, e.Appearance.Font).Width, e.Bounds.Y+4);

                e.Handled = true; // 告诉 TreeList 已经处理
            }
        }

        private void ButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            taskManage.AddTaskListByNode(treeList1.FocusedNode);
            TaskRun();
        }

        private void ButtonEdit_ButtonClick2(object sender, ButtonPressedEventArgs e)
        {
            string name_str = treeList2.FocusedNode.GetValue("name").ToString();
            string[] name_arr = name_str.Split('、');
            foreach (string name in name_arr)
            {
                TreeListNode node = treeList1.FindNode(x => x.GetValue("name").ToString() == name);
                taskManage.AddTaskListByNode(node);
            }
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
                        //e.Appearance.BackColor = Color.LightBlue;
                        e.Appearance.ForeColor = Color.Blue;
                        break;
                    case 1:
                        //e.Appearance.BackColor = Color.LightGreen;
                        e.Appearance.ForeColor = Color.Green;
                        break;
                    case 2:
                        //e.Appearance.BackColor = Color.LightCoral;
                        e.Appearance.ForeColor = Color.Red;
                        break;
                    case 3:
                        //e.Appearance.BackColor = Color.LightCoral;
                        e.Appearance.ForeColor = Color.DarkRed;
                        break;
                    default:
                        //e.Appearance.BackColor = Color.LightGray;
                        e.Appearance.ForeColor = Color.DarkGray;
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
                //if (!e.Node.HasChildren && int.Parse(e.Node.ParentNode.GetValue("type").ToString()) == 1)
                //{
                //    val = 1;
                //}
                //if (!e.Node.HasChildren && int.Parse(e.Node.ParentNode.GetValue("type").ToString()) == 3)
                //{
                //    val = 3;
                //}
                switch (val)
                {
                    case 0:
                        e.DisplayText = "异步";
                        break;
                    case 1:
                        e.DisplayText = "异步参数";
                        break;
                    case 2:
                        e.DisplayText = "阻塞";
                        break;
                    case 3:
                        e.DisplayText = "阻塞参数";
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
            taskManage.Run();
            treeList2.RefreshDataSource();
        } 
        #endregion

        #region memoedit
        private int promptStartIndex;//记录提示符的位置
        private void InitMemo()
        {
            memoEdit1.Properties.ScrollBars = ScrollBars.Both;
            memoEdit1.Properties.ContextMenuStrip = new ContextMenuStrip();

            //memoEdit1.KeyDown += MemoEdit1_KeyDown;
            //Cmd.work_path = Cmd.ExecCommandSync("[Environment]::GetFolderPath('Desktop')").Trim();
            //Cmd.work_path = Cmd.ExecCommandSync("echo %USERPROFILE%\\Desktop").Trim();
            //ShowPrompt(true);


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


        public void DisplayPartialResult(int status, string partialResult ="", string command = "")
        {
            if (Cmd.STATUS_START == status)
            {
                DateTime dt = DateTime.Now;
                memoEdit1.AppendText(dt.ToString() + "： " + partialResult + Environment.NewLine);
            }
            else
            {
                // 在UI上显示部分结果消息
                // 例如，可以将消息追加到一个TextBox或ListBox中
                // 请根据你的UI控件来处理
                BeginInvoke(new Action(() =>
                {
                    memoEdit1.AppendText(partialResult.Replace("\n", Environment.NewLine));
                }));
            }
            
        }

        #endregion

    }
}
