using DevExpress.Utils.Menu;
using DevExpress.XtraGrid;
using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Menu;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AJLibrary
{
    public class PopupMenuShowDev
    {
        public static void GridViewDev(GridControl view, Dictionary<string, MeasureItemEventHandler> memuItemClickDic)
        {
            // 创建一个右键菜单
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();

            // 将右键菜单与 GridView 关联
            view.ContextMenuStrip = contextMenuStrip;


            foreach (var item in memuItemClickDic)
            {
                // 创建一个菜单项，并添加到右键菜单中
                ToolStripMenuItem menuItem = new ToolStripMenuItem(item.Key);
                // 为菜单项添加点击事件处理程序
                menuItem.Click += (sender, e) => item.Value(sender, (MeasureItemEventArgs)e);

                // 将菜单项添加到右键菜单中
                contextMenuStrip.Items.Add(menuItem);
            }

        }


        public static void TreeListDev(TreeList tree, Dictionary<string, PopupMenuShowingEventHandler> memuItemClickDic = null)
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
    }
}
