using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AJLibrary
{
    public class JsonCacheBase<T> where T : class, new()
    {
        public string name;
        public bool _update = false;//存在更新标识
        public T data;
        public JsonCacheBase(string fileName)
        {
            name = fileName;
            data = LoadCacheFromFile() ?? new T();
        }

        private T LoadCacheFromFile()
        {
            try
            {
                string json = FileTool.LoadFromFile(name);
                if (json.Length > 0)
                {
                    return JsonConvert.DeserializeObject<T>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"从文件加载缓存时发生错误：{ex.Message}");
            }

            return null;
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            if (_update)
            {
                SaveCacheToFile();
                _update = false;
            }
        }

        public void SaveCacheToFile()
        {
            try
            {
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                FileTool.SaveToFile(name, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存缓存到文件时发生错误：{ex.Message}");
            }
        }

        /// <summary>
        /// 标记为已更新
        /// </summary>
        public void MarkUpdated()
        {
            _update = true;
        }
    }
}
