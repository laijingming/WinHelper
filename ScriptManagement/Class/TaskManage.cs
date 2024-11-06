using AJLibrary;
using DevExpress.XtraBars.Docking;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraWaitForm;
using ScriptManagement.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScriptManagement
{
    public class TaskManage
    {

        public List<CommandModel> list = new List<CommandModel>();//任务列表

        /// <summary>
        /// 添加节点命令
        /// </summary>
        /// <param name="node"></param>
        /// <param name="is_check">是否执行选中，默认否</param>
        public void AddTaskListByNode(TreeListNode node, bool is_check = false)
        {
            if (node == null)
            {
                return;
            }
            if (node.HasChildren)
            {
                foreach (TreeListNode item in node.Nodes)
                {
                    //父节点
                    //  isCheck为false所有子节点都执行
                    //  isCheck为true只执行选中的子节点
                    if (!is_check || (is_check && item.Checked))
                    {
                        AddTask(item);
                    }
                }
            }
            else
            {
                AddTask(node);
            }
        }
        /// <summary>
        /// 添加任务
        /// </summary>
        /// <param name="node"></param>
        public void AddTask(TreeListNode node)
        {
            string parrent_command = null;
            if (node.ParentNode != null)
            {
                parrent_command = node.ParentNode.GetValue("command").ToString();
            }
            list.Add(new CommandModel()
            {   
                name = node.GetValue("name").ToString(),
                parrent_command = parrent_command,
                command = node.GetValue("command").ToString(),
                type = node.GetValue("type").ToString(),
            });
        }

        /// <summary>
        /// 清空命令
        /// </summary>
        public void ClearItems()
        {
            list.Clear();
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        public async void Run()
        {
            if (list.Count == 0)
            {   
                return;
            }
            RunLog();
            string commandParams = "";
            string show_name = "";
            int num = 0;

            //重新赋值，防止新的任务加入出现冲突
            List<CommandModel> list_new = list.ToList();
            int allNum = list_new.Count - 1;
            foreach (CommandModel item in list_new)
            {   
                int _type = int.Parse(item.type);
                num++;
                switch (_type)
                {
                    case Constants.COMMAND_ASYNC_TYPE:
                        Cmd.ExecAsync(GetCommond(item));
                        break;
                    case Constants.COMMAND_ASYNC_PARAMS_TYPE:
                    case Constants.COMMAND_SYNC_PARAMS_TYPE:
                        if (commandParams.Length <= 0)
                        {
                            commandParams = string.Format("{0} {1}", item.parrent_command, item.command);
                        }
                        else
                        {
                            commandParams += "," + item.command;
                        }
                        //判断下一项是否同级
                        if (allNum >= num && list_new[num].parrent_command == item.parrent_command)
                        {
                            show_name = item.name + "\n" + show_name;
                            continue;
                        }
                        if (_type== Constants.COMMAND_SYNC_PARAMS_TYPE)
                        {   
                            LoadForm(true, show_name);
                            await Cmd.WaitTaskDone();
                            Cmd.ExecAsync(commandParams);
                            await Cmd.WaitTaskDone();
                            LoadForm(false);
                        }
                        else
                        {
                            Cmd.ExecAsync(commandParams);
                        }
                        commandParams = "";
                        break;
                    case Constants.COMMAND_SYNC_TYPE:
                        LoadForm(true, item.name);
                        await Cmd.WaitTaskDone();
                        Cmd.ExecAsync(GetCommond(item));
                        await Cmd.WaitTaskDone();
                        LoadForm(false);
                        break;
                    default:
                        break;
                }
            }

            ClearItems();
        }

        public void RunLog() 
        {
            string key_name="";
            foreach (CommandModel item in list)
            {
                key_name += item.name+"、";
            }
            key_name = key_name.Trim('、');

            int index = CommandLog.getIns.data.FindIndex(x => x.name == key_name);
            if (index!=-1)
            {
                CommandLog.getIns.data[index].num++;
            }
            else
            {
                CommandLogModel commandLogModel = new CommandLogModel();
                commandLogModel.name = key_name;
                commandLogModel.num++;
                CommandLog.getIns.data.Add(commandLogModel);
            }
            CommandLog.getIns.MarkUpdated();
        }

        private string GetCommond(CommandModel item)
        {
            char separator = ','; // 逗号作为分隔符
            string[] parts = item.command.Split(separator);
            string commond = "";
            foreach (string part in parts)
            {
                commond += item.parrent_command + " " + part + "&&";
            }
            return commond.TrimEnd('&');
        }

        WaitForm1 waitForm = new WaitForm1();
        /// <summary>
        /// 加载浮窗
        /// </summary>
        /// <param name="start"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="millisecond"></param>
        public  void LoadForm(Boolean start = true,string name="")
        {
            if (start)
            {
                waitForm.ShowMe(null, $"正在执行阻塞命令：{name}", "请稍等");
            }
            else
            {
                waitForm.HideMe(null);
            }

        }
    }
}
