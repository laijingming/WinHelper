using AJLibrary;
using System.Collections.Generic;


namespace ScriptManagement.Class
{
    public class CommandLog: JsonCacheBase<List<CommandLogModel>>
    {
        public const string comment = "日志文件";

        public const string FILENAME = "./file/log.json";//文件名


        public CommandLog() : base(FILENAME)
        {
        }

        public static CommandLog getIns => SingletonHelper<CommandLog>.GetInstance();

        /// <summary>
        /// 设置置顶
        /// </summary>
        /// <param name="key"></param>
        /// <param name="is_to_up"></param>
        public void SetToUp(string key,bool is_to_up=true) 
        {
            int index = -1;
            int to_up = 0;
            if (is_to_up)
            {
                
                for (int i = 0; i < getIns.data.Count; i++)
                {
                    if (to_up < getIns.data[i].to_up)
                    {
                        to_up = getIns.data[i].to_up;
                    }
                    if (getIns.data[i].name == key)
                    {
                        index = i;
                    }
                }
                to_up++;
            }
            else
            {
                index = getIns.data.FindIndex(x => x.name == key);
            }

            if (index == -1)
            {
                return;
            }
            getIns.data[index].to_up = to_up;
            getIns.MarkUpdated();
        }
    }
}
