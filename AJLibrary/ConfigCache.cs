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

        public static ConfigCache GetIns => Master.getModel<ConfigCache>();

        /// <summary>
        /// 一键登录端口
        /// </summary>
        /// <returns></returns>
        public string GetAutoLoginPort() 
        {
            return Get("autoLoginPort", "9222");
        }
        /// <summary>
        /// 一键登录端口
        /// </summary>
        /// <returns></returns>
        public string GetAutoLoginDir()
        {
            return Get("autoLoginDir", "D:\\SeleniumChromeProfile");
        }

    }
}
