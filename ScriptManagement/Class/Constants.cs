using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptManagement.Class
{
    internal class Constants
    {
        /// <summary>
        /// 类型 
        ///     0 并发命令
        ///     1 并发命令，子节点为父节点参数
        ///     2 阻塞脚本
        ///     3 阻塞脚本，子节点为父节点参数
        /// </summary>
        public const int COMMAND_ASYNC_TYPE = 0;
        public const int COMMAND_ASYNC_PARAMS_TYPE = 1;
        public const int COMMAND_SYNC_TYPE = 2;
        public const int COMMAND_SYNC_PARAMS_TYPE = 3;

    }
}
