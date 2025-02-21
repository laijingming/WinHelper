using AJLibrary;
using System.Collections.Generic;

namespace ScriptManagement.Class
{
    public class CommandCache : JsonCacheBase<List<CommandModel>>
    {
        public const string comment = "命令列表";

        public const string FILENAME = "./file/command.json";//文件名

        public CommandCache() : base(FILENAME)
        {
        }

        public static CommandCache getIns => SingletonHelper<CommandCache>.GetInstance();
    }

    public class CommandModel
    {
        public string name { get; set; }
        public string parrent_command { get; set; }
        public string command { get; set; }

        public string type { get; set; } = "";
        public List<CommandModel> children { get; set; }
    }
}
