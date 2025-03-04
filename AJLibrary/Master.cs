using System;
using System.Collections.Generic;

namespace AJLibrary
{
    public class Master
    {
        // 定义一个字典来存储对象实例
        public static Dictionary<string, object> _caches = new Dictionary<string, object>();


        /// <summary>
        /// 固化缓存
        /// </summary>
        public static void destory()
        {
            //缓存固化
            foreach (dynamic item in Master._caches)
            {
                item.Value.Save();
            }
        }

        public static T getModel<T>()
        {   
            string key = typeof(T).Name;
            if (!_caches.ContainsKey(key))
            {
                _caches.Add(key, Activator.CreateInstance(typeof(T)));
            }
            return (T)_caches[key];// 返回实例
        }

    }
}
