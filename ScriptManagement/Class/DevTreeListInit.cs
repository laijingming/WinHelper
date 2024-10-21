using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ScriptManagement
{
    /// <summary>
    /// 初始化TreeList，父子节点选择关联
    /// </summary>
    public class DevTreeListInit
    {
        /// <summary>
        /// 
        /// </summary>
        public List<TreeListNode> treeListNodes;

        TreeList Tree;
        /// <summary>
        /// 初始化TreeList，父子节点选择关联
        /// </summary>
        /// <param name="tree"></param>
        public DevTreeListInit(TreeList tree)
        {
            treeListNodes = new List<TreeListNode>();
            Tree = tree;
            tree.OptionsView.CheckBoxStyle = DefaultNodeCheckBoxStyle.Check;// 启用节点复选框样式为复选框
            tree.BeforeCheckNode += tree_BeforeCheckNode;
            tree.AfterCheckNode += tree_AfterCheckNode;
        }

        public bool AllowCheck = true;

        /// <summary>
        /// 处理节点的复选框选择后事件，确保子节点与父节点关联
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">包含事件数据的参数</param>
        public void tree_AfterCheckNode(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
        {
            SetCheckedChildNodes(e.Node, e.Node.CheckState);
            SetCheckedParentNodes(e.Node, e.Node.CheckState);
            if (!e.Node.HasChildren)
            {
                setTreeListNodels(e.Node);
            }
        }
        /// <summary>
        /// 处理节点的复选框选择前事件，禁止默认选择行为，并处理复选框状态切换
        /// </summary>
        /// <param name="sender">事件发送者</param>
        /// <param name="e">包含事件数据的参数</param>
        void tree_BeforeCheckNode(object sender, DevExpress.XtraTreeList.CheckNodeEventArgs e)
        {
            e.CanCheck = AllowCheck;
            e.State = (e.PrevState == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
        }

        public void SetNodeCheckState(TreeListNode node, CheckState checkState)
        {
            Tree.SetNodeCheckState(node, CheckState.Checked);
            SetCheckedChildNodes(node, CheckState.Checked);
            SetCheckedParentNodes(node, CheckState.Checked);
        }

        void SetCheckedChildNodes(TreeListNode node, CheckState check)
        {
            for (int i = 0; i < node.Nodes.Count; i++)
            {
                node.Nodes[i].CheckState = check;
                SetCheckedChildNodes(node.Nodes[i], check);
                setTreeListNodels(node.Nodes[i]);
            }
        }
        void SetCheckedParentNodes(TreeListNode node, CheckState check)
        {
            if (node.ParentNode != null)
            {
                bool b = false;
                CheckState state;
                for (int i = 0; i < node.ParentNode.Nodes.Count; i++)
                {
                    state = (CheckState)node.ParentNode.Nodes[i].CheckState;
                    if (!check.Equals(state))
                    {
                        b = !b;
                        break;
                    }
                }
                node.ParentNode.CheckState = b ? CheckState.Indeterminate : check;
                SetCheckedParentNodes(node.ParentNode, check);
            }
        }

        /// <summary>
        /// 添加选择指令
        /// </summary>
        /// <param name="node"></param>
        private void setTreeListNodels(TreeListNode node)
        {

            if (treeListNodes.Contains(node))
            {
                treeListNodes.Remove(node);
            }

            if (node.Checked)
            {
                treeListNodes.Add(node);
            }
        }


    }
}
