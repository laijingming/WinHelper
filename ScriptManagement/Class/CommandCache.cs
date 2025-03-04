using AJLibrary;
using DevExpress.XtraBars.Docking2010.Customization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ScriptManagement.Class
{
    public class CommandCache : JsonCacheBase<List<CommandModel>>
    {
        public const string comment = "命令列表";

        public const string FILENAME = "./file/command.json";//文件名

        public CommandCache() : base(FILENAME)
        {
        }

        public static CommandCache getIns => Master.getModel<CommandCache>();

        public void SaveDevop(string auth) 
        {
            //保存auth
            CommandModel model = GetDevop();
            if (model!=null)
            {
                string[] parts = model.command.Split(' ');
                parts[parts.Length - 1] = auth;
                model.command = string.Join(" ", parts);
                MarkUpdated();
            }
        }

        public CommandModel GetDevop() 
        {
            // 重新执行命令
            int index = data.FindIndex(x => x.name == "Devop");
            if (index >= 0)
            {
               return data[index];
            }
            return null;
        }

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
