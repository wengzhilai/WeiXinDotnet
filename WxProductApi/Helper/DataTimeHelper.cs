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
        public static long getDateLong(DateTime dt) {
            return Convert.ToInt64(dt.ToString("yyyyMMddHHmmss"));
        }

        public static DateTime getDate(long dataLongStr) {
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyyMMddHHmmss";
            return Convert.ToDateTime(dataLongStr.ToString(), dtFormat);
        }

    }

}