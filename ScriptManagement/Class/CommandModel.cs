using AJLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptManagement.Class
{
    /// <summary>
    /// 任务实体
    /// </summary>
    public class CommandModel
    {
        public string name { get; set; }
        public string parrent_command { get; set; }
        public string command { get; set; }

        public string type { get; set; } = "";
        public List<CommandModel> children { get; set; }
    }


    /// <summary>
    /// 任务实体
    /// </summary>
    public class CommandLogModel
    {
        public string name { get; set; }
        /// <summary>
        /// 执行次数
        /// </summary>
        public int num { get; set; }
        /// <summary>
        /// 置顶
        /// </summary>
        public int to_up { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public long time { get; set; } = Time.Now();
    }
}
