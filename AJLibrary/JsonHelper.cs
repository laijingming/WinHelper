using Newtonsoft.Json;
using System.IO;


namespace AJLibrary
{
    public class JsonHelper
    {
        public static object Deserialize(string json) { 
            return JsonConvert.DeserializeObject(json);
        }

        public static string Serialize(object obj) { 
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 解析json文件转化为object
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static object DeserializeJsonFile(string path) { 
            return JsonConvert.SerializeObject(File.ReadAllText(path)); 
        }

        /// <summary>
        /// 解析json文件转化为指定类型数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T DeserializeJsonFileToType<T>(string path) { 
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path)); 
        }
    }
}
