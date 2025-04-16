using AJLibrary;
using System.Collections.Generic;

namespace Post
{
    public class PostConfigCache: JsonDicCacheBase
    {
        public const string comment = "配置";

        public const string FILENAME = "./file/postconfig.json";//文件名

        public static PostConfigCache getIns => Master.getModel<PostConfigCache>();

        public PostConfigCache() : base(FILENAME)
        {

        }



    }

}
