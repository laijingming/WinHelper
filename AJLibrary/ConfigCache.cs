using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJLibrary
{
    public class ConfigCache : JsonDicCacheBase
    {
        public const string comment = "基础配置";

        public const string FILENAME = "./file/config.json";//文件名

        public ConfigCache() : base(FILENAME)
        {
        }

        public static ConfigCache getIns => SingletonHelper<ConfigCache>.GetInstance();
    }
}
