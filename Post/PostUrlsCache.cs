using AJLibrary;
using System.Collections.Generic;

namespace Post
{
    internal class PostUrlsCache:JsonCacheBase<List<Testing>>
    {
        public const string comment = "配置";

        public const string FILENAME = "./file/posturls.json";//文件名

        public static PostUrlsCache getIns => Master.getModel<PostUrlsCache>();

        public PostUrlsCache() : base(FILENAME)
        {

        }



    }

    internal class Testing
    {
        public string Name { get; set; }
        public string Url { get; set; }

        /// <summary>
        /// 用于combox对外展示数据
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + ":" + Url;
        }
    }
}
