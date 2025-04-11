using AJLibrary;
using System.Collections.Generic;

namespace Post
{
    internal class PostDataModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int id { get; set; }
        public int pid { get; set; }

        /// <summary>
        /// 用于combox对外展示数据
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name;
        }

    }
}
