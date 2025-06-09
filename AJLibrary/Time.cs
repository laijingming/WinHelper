using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AJLibrary
{
    public class Time
    {
        public static long Now()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        }

        public static long NowMs()
        {
            return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }

        public static DateTime NowDate()
        {
            return UnixTimeStampToDateTime(Now());
        }

        /// <summary>
        /// 日期转时间戳
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isSec"></param>
        /// <returns></returns>
        public static string DateTimeToUnixTimeStamp(string input, bool isSec = true)
        {
            DateTime dateTime;
            if (!DateTime.TryParse(input, out dateTime))
            {
                return "Invalid input";
            }
            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = dateTime - epoch;
            double res = Convert.ToInt64(timeSpan.TotalSeconds);
            if (!isSec)
            {
                res *= 1000;
            }
            return res.ToString();
        }

        /// <summary>
        /// 时间戳转日期
        /// </summary>
        /// <param name="input"></param>
        /// <param name="isSec"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime(long unixTimeStamp, bool isSec = true)
        {
            if (!isSec) { unixTimeStamp /= 1000; }
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
