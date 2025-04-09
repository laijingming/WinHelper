using AJLibrary;
using System.Collections.Generic;

namespace Office
{
    internal class OfficeCache:JsonCacheBase<List<OfficCacheModel>>
    {
        public const string comment = "命令列表";

        public const string FILENAME = "./file/officecache.json";//文件名

        public static OfficeCache getIns => Master.getModel<OfficeCache>();

        public OfficeCache() : base(FILENAME)
        {

        }



    }

    public class OfficCacheModel
    {   
        public string path {  get; set; }
        public string name { get; set; }
    }
}
