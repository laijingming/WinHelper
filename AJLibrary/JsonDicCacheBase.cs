using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AJLibrary
{
    public class JsonDicCacheBase
    {
        public string name;
        public bool _update = false;//存在更新标识
        public Dictionary<string, string> cache;
        public JsonDicCacheBase(string _name)
        {
            name = _name;
            cache = LoadCacheFromFile();
        }

        public void SetFile(string key, string value)
        {
            if (cache.ContainsKey(key))
            {
                // 更新现有键
                cache[key] = value;
            }
            else
            {
                // 添加新键
                cache.Add(key, value);
            }
            SaveCacheToFile();
            _update = false;
        }
        /// <summary>
        /// 临时存储，不存文件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="isSave"></param>
        public void Set(string key, string value)
        {
            if (cache.ContainsKey(key))
            {
                // 更新现有键
                cache[key] = value;
            }
            else
            {
                // 添加新键
                cache.Add(key, value);
            }
            _update = true;
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

        public string Get(string key,string def="")
        {
            if (cache.ContainsKey(key))
            {
                return cache[key];
            }

            // 如果键不存在，返回 空字符串 
            return def;
        }


        private Dictionary<string, string> LoadCacheFromFile()
        {
            try
            {
                string json = FileTool.LoadFromFile(name);
                if (json.Length > 0)
                {
                    return JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"从文件加载缓存时发生错误：{ex.Message}");
            }

            return new Dictionary<string, string>();
        }


        public void SaveCacheToFile()
        {
            try
            {
                string json = JsonConvert.SerializeObject(cache, Formatting.Indented);
                FileTool.SaveToFile(name, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"保存缓存到文件时发生错误：{ex.Message}");
            }
        }
    }

}
