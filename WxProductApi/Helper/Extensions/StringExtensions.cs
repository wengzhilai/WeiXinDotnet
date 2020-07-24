using Helper;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// 字符串扩展类
    /// </summary>
    public static class StringExtensions
    {
        private static readonly Regex CRegex_PhoneNumber = new Regex(@"^[1]+[3,5,7,8]+\d{9}$");
        private static readonly Regex CRegex_Number = new Regex(@"^[0-9]+$");
        private static readonly Regex CRegex_Word = new Regex(@"^[a-zA-Z]+$");
        private static readonly Regex CRegex_NumberAndString = new Regex(@"(\d+[a-zA-Z])|([a-zA-Z]\d+)");



        public static int ToInt32(this String source)
        {
            int i = 0;
            int.TryParse(source, out i);
            return i;
        }
        public static decimal ToDecimal(this String source)
        {
            decimal i = 0;
            decimal.TryParse(source, out i);
            return i;
        }
        public static DateTime ToDateTime(this String source)
        {
            DateTime i = DateTime.MinValue;
            DateTime.TryParse(source, out i);
            return i;
        }

        public static Char ToChar(this String source)
        {
            Char i = ' ';
            Char.TryParse(source, out i);
            return i;
        }
        public static bool ToBoolean(this String source)
        {
            bool i = false;
            bool.TryParse(source, out i);
            return i;
        }
        

        
        
        

        public static string FormatWith(this String source, params object[] args)
        {
            return string.Format(source, args);
        }
        /// <summary>
        /// 取中文简拼
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>转化后的字符串</returns>
        public static string GetChineseSpell(this String source)
        {
            int len = source.Length;
            string s = string.Empty;
            for (int i = 0; i < len; i++)
                s += GetSpell(source.Substring(i, 1));
            return s;
        }

        private static string GetSpell(string c)
        {
            byte[] ary = Encoding.Default.GetBytes(c);
            if (ary.Length > 1)
            {
                int area = ary[0];
                int pos = ary[1];
                int code = (area << 8) + pos;
                int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                for (int i = 0; i < 26; i++)
                {
                    int max = 55290;
                    if (i != 25)
                        max = areacode[i + 1];
                    if (areacode[i] <= code && code < max)
                        return Encoding.Default.GetString(new[] { (byte)(65 + i) });
                }
                return string.Empty;
            }
            return c;
        }

        /// <summary>
        /// 是否为纯数字字符串(0..9)
        /// </summary>
        /// <param name="source">源字符串</param>
        public static bool IsOnlyNumber(this String source)
        {
            return CRegex_Number.Match(source).Success;
        }
        /// <summary>
        /// 难证是否是电话号码
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(this String source){
            return CRegex_PhoneNumber.Match(source).Success;
        }

        /// <summary>
        /// 是否为数字+字母字符串(0..9,a..z,A..Z)
        /// </summary>
        /// <param name="source">源字符串</param>
        public static bool IsNumberAndString(this String source)
        {
            return CRegex_NumberAndString.Match(source).Success;
        }

        /// <summary>
        /// 是否为纯字母字符串(a..z,A..Z)
        /// </summary>
        /// <param name="source">源字符串</param>
        public static bool IsOnlyWord(this String source)
        {
            return CRegex_Word.Match(source).Success;
        }


        /// <summary>
        /// 比较字符串是否相同(不区分大小写)
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="s">比较的字符串</param>
        /// <returns>是否相同</returns>
        public static bool SameText(this String source, string s)
        {
            return string.Compare(source, s, true) == 0;
        }

        /// <summary>
        /// 是否为有效的整数
        /// </summary>
        public static bool IsInt64(this String source)
        {
            Int64 value;
            return Int64.TryParse(source, out value);
        }

        public static bool IsInt32(this String source)
        {
            int value;
            return Int32.TryParse(source, out value);
        }
        
        /// <summary>
        /// 直接格式化字符串
        /// </summary>
        public static string FormatMe(this String source, params object[] args)
        {
            return string.Format(source, args);
        }

        /// <summary>
        /// 指示指定的 System.String 对象是 null 还是 System.String.Empty 字符串。
        /// </summary>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 取左边字符串
        /// </summary>
        public static string LeftString(this string source, int count)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            int len = source.Length;
            return len > count ? source.Substring(0, count) : source;
        }

        /// <summary>
        /// 取左边字符串
        /// </summary>
        public static string RightString(this string source, int count)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            int len = source.Length;
            if (len > count)
                return source.Substring(len - count, count);
            return source;
        }

        public static string Md5(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return "";
            }
            else
            {
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(source);
                bytes = md5.ComputeHash(bytes);
                md5.Clear();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    sb.Append(bytes[i].ToString("x2"));
                return sb.ToString();
            }


        }

        public static string SHA1(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return "";
            }
            else
            {
                System.Security.Cryptography.SHA1CryptoServiceProvider md5 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(source);
                bytes = md5.ComputeHash(bytes);
                md5.Clear();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                    sb.Append(bytes[i].ToString("x2"));
                return sb.ToString();
            }


        }
    }
}