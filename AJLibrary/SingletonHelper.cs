using System;


namespace AJLibrary
{
    public class SingletonHelper<T> where T : class
    {
        private static T instance;                 // 单例实例
        private static readonly object lockObject = new object(); // 用于锁定的对象

        /// <summary>
        /// 获取单例实例，如果尚未创建，则使用指定的参数调用带参数的构造函数。
        /// </summary>
        /// <param name="args">构造函数参数。</param>
        /// <returns>返回单例实例。</returns>
        public static T GetInstance(params object[] args)
        {
            // 第一次检查是否为空，避免不必要的加锁
            if (instance == null)
            {
                lock (lockObject)
                {
                    // 第二次检查是否为空，确保只有一个线程创建实例
                    if (instance == null)
                    {
                        // 使用 Activator.CreateInstance 来创建实例，支持带参数的构造函数
                        instance = (T)Activator.CreateInstance(typeof(T), args);
                    }

                }
            }

            return instance; // 返回单例实例
        }
    }
}
