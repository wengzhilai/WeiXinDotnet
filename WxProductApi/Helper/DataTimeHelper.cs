using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Models;


namespace Helper
{

    public class DataTimeHelper
    {
        /// <summary>
        /// 获取时间格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long getDateLong(DateTime dt)
        {
            return Convert.ToInt64(dt.ToString("yyyyMMddHHmmss"));
        }

        public static DateTime getDate(long dataLongStr)
        {
            string dataStr = dataLongStr.ToString();
            var reDt = new DateTime(
                            int.Parse(dataStr.Substring(0, 4)),
                            int.Parse(dataStr.Substring(4, 2)),
                            int.Parse(dataStr.Substring(6, 2)),
                            int.Parse(dataStr.Substring(8, 2)),
                            int.Parse(dataStr.Substring(10, 2)),
                            int.Parse(dataStr.Substring(12, 2))
                            );
            return reDt;
            // DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            // dtFormat.ShortDatePattern = "yyyyMMddHHmmss";
            // return Convert.ToDateTime(dataLongStr.ToString(), dtFormat);
        }
        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static long getDateLongTimestamp(DateTime dt)
        {
            TimeSpan ts = dt - TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1), TimeZoneInfo.Utc, TimeZoneInfo.Local);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        /// 时间戳转为格式
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime StampToDateTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZoneInfo.ConvertTime(new System.DateTime(1970, 1, 1), TimeZoneInfo.Utc, TimeZoneInfo.Local);
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dateTimeStart.Add(toNow);
        }

    }

}