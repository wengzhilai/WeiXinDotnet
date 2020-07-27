using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Configuration;
using System.Drawing;
using System.Security.Cryptography;

namespace Helper
{
    public class Fun
    {

        /// <summary>
        /// 检测密码的复杂度是否满足
        /// </summary>
        /// <param name="pwdStr"></param>
        /// <returns></returns>
        public static bool CheckPassword(string pwdStr, int PwdComplexity)
        {
            var PwdMinLength = 2;
            Regex r1 = new Regex(@"^(?=.*[a-z])");
            Regex r2 = new Regex(@"^(?=.*[A-Z])");
            Regex r3 = new Regex(@"^(?=.*[0-9])");
            Regex r4 = new Regex(@"^(?=([\x21-\x7e]+)[^a-zA-Z0-9])");
            if (pwdStr.Length < PwdMinLength)
            {
                throw new Exception(string.Format("密码长度不够{0}位", PwdMinLength));
            }
            int reInt = 0;
            if (r1.IsMatch(pwdStr)) reInt++;
            if (r2.IsMatch(pwdStr)) reInt++;
            if (r3.IsMatch(pwdStr)) reInt++;
            if (r4.IsMatch(pwdStr)) reInt++;

            if (reInt < PwdComplexity)
            {
                throw new Exception(string.Format("密码必须包括字母大写、字母小写、数字和特殊字符中的其中{0}种", PwdComplexity));
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetExceptionMessage(Exception e)
        {
            IList<string> message = new List<string>();
            message.Add(e.Message);
            while (e.InnerException != null)
            {
                e = e.InnerException;
                message.Add(e.Message);
            }
            return message[message.Count - 1];
        }


        /// <summary>
        /// 将DataTable数据转换成实体类
        /// 本功能主要用于外导EXCEL
        /// </summary>
        /// <typeparam name="T">MVC的实体类</typeparam>
        /// <param name="dt">输入的DataTable</param>
        /// <returns>实体类的LIST</returns>
        public static IList<T> TableToClass<T>(DataTable dt) where T : new()
        {
            IList<T> outList = new List<T>();
            T tmpClass = new T();
            if (dt.Rows.Count == 0) return outList;
            PropertyInfo[] proInfoArr = tmpClass.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);//得到该类的所有公共属性
            Dictionary<string, string> dic = new Dictionary<string, string>();
            Dictionary<string, string> dic_all = new Dictionary<string, string>();
            foreach (var t in proInfoArr)
            {
                var attrsPro = t.GetCustomAttributes(typeof(DisplayAttribute), true);
                if (attrsPro.Length > 0)
                {
                    DisplayAttribute pro = (DisplayAttribute)attrsPro[0];
                    dic_all.Add(pro.Name, t.Name);
                }
            }
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dic_all.Where(x => x.Key == dt.Columns[i].Caption).Count() != 0)
                {
                    dic.Add(dt.Columns[i].Caption, dic_all[dt.Columns[i].Caption]);
                }
                else if (dic_all.Where(x => x.Value == dt.Columns[i].Caption).Count() != 0)
                {
                    dic.Add(dt.Columns[i].Caption, dt.Columns[i].Caption);
                }
            }

            var rowTmp = dt.Rows[0];

            for (int a = 0; a < dt.Rows.Count; a++)
            {
                var row = dt.Rows[a];
                tmpClass = new T();
                proInfoArr = tmpClass.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);//得到该类的所有公共属性
                foreach (var t in dic)
                {
                    PropertyInfo outproInfo = tmpClass.GetType().GetProperty(t.Value);
                    if (outproInfo != null)
                    {
                        if (row[t.Key] == null || string.IsNullOrEmpty(row[t.Key].ToString()))
                        {
                            row[t.Key] = rowTmp[t.Key];
                        }
                        outproInfo.SetValue(tmpClass, Convert.ChangeType(row[t.Key], outproInfo.PropertyType, CultureInfo.CurrentCulture), null);
                    }
                }
                outList.Add(tmpClass);
                rowTmp = dt.Rows[a];
            }
            return outList;
        }

        /// <summary>
        /// 复制一个类里所有属性到别一个类
        /// </summary>
        /// <typeparam name="inT">传入的类型</typeparam>
        /// <typeparam name="outT">输出类型</typeparam>
        /// <param name="inClass">传入的类</param>
        /// <param name="outClass">输入的类</param>
        /// <returns>复制结果的类</returns>
        public static outT ClassToCopy<inT, outT>(inT inClass, outT outClass, IList<string> allPar = null)
        {
            if (inClass == null) return outClass;
            PropertyInfo[] proInfoArr = inClass.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);//得到该类的所有公共属性
            for (int a = 0; a < proInfoArr.Length; a++)
            {
                if (allPar != null && !allPar.Contains(proInfoArr[a].Name)) continue;
                PropertyInfo outproInfo = outClass.GetType().GetProperty(proInfoArr[a].Name);
                if (outproInfo != null)
                {
                    var type = outproInfo.PropertyType;
                    object objValue = proInfoArr[a].GetValue(inClass, null);
                    if (null != objValue)
                    {
                        if (!outproInfo.PropertyType.IsGenericType)
                        {
                            objValue = Convert.ChangeType(objValue, outproInfo.PropertyType);
                        }
                        else
                        {
                            Type genericTypeDefinition = outproInfo.PropertyType.GetGenericTypeDefinition();
                            if (genericTypeDefinition == typeof(Nullable<>))
                            {
                                objValue = Convert.ChangeType(objValue, Nullable.GetUnderlyingType(outproInfo.PropertyType));
                            }
                        }
                    }
                    outproInfo.SetValue(outClass, objValue, null);
                }
            }
            return outClass;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="inT"></typeparam>
        /// <typeparam name="outT"></typeparam>
        /// <param name="inClass"></param>
        /// <returns></returns>
        public static outT ClassToCopy<inT, outT>(inT inClass) where outT : new()
        {
            if (inClass == null) return default(outT);
            outT outClass = new outT();
            return ClassToCopy(inClass, outClass);
        }
        /// <summary>
        /// 转换IList内的所有属性
        /// </summary>
        /// <typeparam name="inT"></typeparam>
        /// <typeparam name="outT"></typeparam>
        /// <param name="inClass"></param>
        /// <returns></returns>
        public static IList<outT> ClassListToCopy<inT, outT>(IList<inT> inClass) where outT : new()
        {
            if (inClass == null) return default(IList<outT>);
            IList<outT> outClass = new List<outT>();
            for (int a = 0; a < inClass.Count; a++)
            {
                outClass.Add(ClassToCopy<inT, outT>(inClass[a]));
            }
            return outClass;
        }

        /// <summary>
        /// 计算MD5
        /// </summary>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public static string Md5Hash(byte[] fileContent)
        {
            if (fileContent == null) return null;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(fileContent);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
                sb.Append(retVal[i].ToString("x2"));
            return sb.ToString();
        }

        /// <summary>
        /// 32位MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5Hash(string input)
        {
            return Md5Hash(Encoding.Default.GetBytes(input));
        }



        public static string GetUpMoth(string yyyyMM, int numMoth)
        {
            DateTime dt = new DateTime(Convert.ToInt32(yyyyMM.Substring(0, 4)), Convert.ToInt32(yyyyMM.Substring(4, 2)), 1);
            return dt.AddMonths(numMoth).ToString("yyyyMM");
        }


        public static DataTable JsonToDataTable(string strJson)
        {
     

            //取出表名  
            Regex rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(strJson).Value;
            DataTable tb = null;
            //去除表名  
            strJson = strJson.Substring(strJson.IndexOf("[") + 1);
            strJson = strJson.Substring(0, strJson.IndexOf("]"));

            //获取数据  
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split(',');

                //创建表  
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        DataColumn dc = new DataColumn();
                        string[] strCell = str.Split(':');
                        dc.ColumnName = strCell[0].ToString().Replace("\"", "");
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }

                //增加内容  
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split(':')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }

            return tb;
        }
        public static string EvalExpression(string formula)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Result").Expression = formula;
            dt.Rows.Add(dt.NewRow());

            var result = dt.Rows[0]["Result"];
            return result.ToString();
        }
        /// <summary>
        /// 获取类的备注信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetClassDescription<T>()
        {

            object[] peroperties = typeof(T).GetCustomAttributes(typeof(DescriptionAttribute), true);
            if (peroperties.Length > 0)
            {
                return ((DescriptionAttribute)peroperties[0]).Description;
            }
            return "";
        }
        /// <summary>
        /// 获取类的属性说明
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string GetClassProperDescription<T>(string properName)
        {
            PropertyInfo[] peroperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in peroperties)
            {
                if (property.Name == properName)
                {
                    object[] objs = property.GetCustomAttributes(typeof(DescriptionAttribute), true);
                    if (objs.Length > 0)
                    {
                        return ((DescriptionAttribute)objs[0]).Description;
                    }
                }
            }
            return "";
        }

        public static string GetClassProperType<T>(string properName)
        {
            PropertyInfo[] peroperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in peroperties)
            {
                if (property.Name == properName)
                {
                    var propertyType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                    return propertyType.Name;
                }
            }
            return "";
        }



        /// <summary>
        /// 产生一组不重复的随机数
        /// </summary>
        public static IList<int> RandomIntList(int MinValue, int MaxValue, int Length)
        {
            if (MaxValue - MinValue + 1 < Length)
            {
                return null;
            }
            Random R = new Random();
            Int32 SuiJi = 0;
            IList<int> suijisuzu = new List<int>();
            int min = MinValue - 1;
            int max = MaxValue + 1;
            for (int i = 0; i < Length; i++)
            {
                suijisuzu.Add(min);
            }
            for (int i = 0; i < Length; i++)
            {
                while (true)
                {
                    SuiJi = R.Next(min, max);
                    if (!suijisuzu.Contains(SuiJi))
                    {
                        suijisuzu[i] = SuiJi;
                        break;
                    }
                }
            }
            return suijisuzu;
        }

        #region 通过两个点的经纬度计算距离

        private const double EARTH_RADIUS = 6378.137; //地球半径

        public static object LockRunScript { get; set; }

        private static double rad(double d)
        {
            return d * Math.PI / 180.0;
        }
        /// <summary>
        /// 通过两个点的经纬度计算距离(米)
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lng1"></param>
        /// <param name="lat2"></param>
        /// <param name="lng2"></param>
        /// <returns></returns>
        public static double GetDistance(double lat1, double lng1, double lat2, double lng2)
        {
            double radLat1 = rad(lat1);
            double radLat2 = rad(lat2);
            double a = radLat1 - radLat2;
            double b = rad(lng1) - rad(lng2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * EARTH_RADIUS;
            s = Math.Round(s * 10000) / 10;
            return s;
        }
        /// <summary>
        /// 通过两个点的经纬度计算距离(米)
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="lng1"></param>
        /// <param name="lat2"></param>
        /// <param name="lng2"></param>
        /// <returns></returns>
        public static double GetDistance(string lat1, string lng1, string lat2, string lng2)
        {
            return Fun.GetDistance(Convert.ToDouble(lat1), Convert.ToDouble(lng1), Convert.ToDouble(lat2), Convert.ToDouble(lng2));
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetSelectScript(string p)
        {
            // @[00-9]  -5   @[00-23,24-40]
            StringBuilder str = new StringBuilder();
            int begin = p.IndexOf("@");//0
            int end = p.IndexOf("]");//6

            //取得参数占位符中的参数
            string temp = p.Substring(begin + 2, end - begin - 2);

            string[] mc = temp.Split(',');

            foreach (var a in mc)
            {
                string[] c = a.Split('-');

                int i1 = 0, i2 = 0, size = 0;
                try
                {
                    i1 = Int32.Parse(c[0]);
                    i2 = Int32.Parse(c[1]);
                }
                catch (Exception e)
                {
                    LogHelper.WriteErrorLog<Fun>(e.ToString());

                    //如果不是数字，返回空字符串
                    return str.ToString();
                }
                //是否不超过第二个数
                for (int i = i1; i <= i2; i++)
                {
                    size = c[0].Length;
                    size = size - i.ToString().Length;
                    if (size > 0)
                    {

                        //补齐占位符
                        switch (size)
                        {
                            case 1:
                                str.Append(p.Replace(p.Substring(begin, end - begin + 1), "0" + i.ToString()));
                                break;
                            case 2:
                                str.Append(p.Replace(p.Substring(begin, end - begin + 1), "00" + i.ToString()));
                                break;
                            case 3:
                                str.Append(p.Replace(p.Substring(begin, end - begin + 1), "000" + i.ToString()));
                                break;
                            default:
                                str.Append(p.Replace(p.Substring(begin, end - begin + 1), "0000" + i.ToString()));
                                break;

                        }

                    }
                    else
                    {
                        str.Append(p.Replace(p.Substring(begin, end - begin + 1), i.ToString()));
                    }
                    str.Append(";");
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// 去除HTML标记
        /// </summary>
        /// <param name="NoHTML">包括HTML的源码 </param>
        /// <returns>已经去除后的文字</returns>
        public static string NoHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring)) return Htmlstring;
            //删除脚本
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            //Htmlstring=HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
            return Htmlstring;

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="server_addr"></param>
        /// <param name="postStr"></param>
        /// <param name="cookieList"></param>
        /// <returns></returns>
        public static string ExecutePostJson(string server_addr, string postStr, CookieContainer cookieList = null)
        {
            string content = string.Empty;
            try
            {
                DateTime startTime = new DateTime(1970, 1, 1);
                var cdt = (int)(DateTime.Now - startTime).TotalSeconds;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(server_addr);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Timestamp", Convert.ToString(cdt));
                request.ReadWriteTimeout = 30 * 1000;
                request.CookieContainer = cookieList;
                if (!string.IsNullOrEmpty(postStr))
                {
                    byte[] data = Encoding.UTF8.GetBytes(postStr);
                    request.ContentLength = data.Length;
                    Stream myRequestStream = request.GetRequestStream();
                    myRequestStream.Write(data, 0, data.Length);
                    myRequestStream.Close();
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader myStreamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                content = myStreamReader.ReadToEnd();
                myStreamReader.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog<Fun>(ex.ToString());

                return "{\"Message\":\"" + ex.Message + "\",\"IsError\":true}";
            }
            return content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server_addr"></param>
        /// <returns></returns>
        public static string HttpGetJson(string server_addr)
        {
            string content = string.Empty;
            try
            {
                Console.WriteLine("请求地址："+server_addr);
                DateTime startTime = new DateTime(1970, 1, 1);
                var cdt = (int)(DateTime.Now - startTime).TotalSeconds;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(server_addr);
                request.Method = "get";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader myStreamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                content = myStreamReader.ReadToEnd();
                Console.WriteLine("返回内容：" + content);
                myStreamReader.Close();
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog<Fun>(ex.ToString());
                return "{\"Message\":\"" + ex.Message + "\",\"IsError\":true}";
            }
            return content;
        }


        public static string HttpPostJson(string Url, string postDataStr)
        {
            string reStr = "";
            byte[] dataArray = Encoding.UTF8.GetBytes(postDataStr);
            // Console.Write(Encoding.UTF8.GetString(dataArray));

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "POST";
            //request.CookieContainer = cookie;

            try
            {
                Console.WriteLine("请求地址：" + Url);
                Console.WriteLine("请求参数：" + postDataStr);

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(dataArray, 0, dataArray.Length);
                dataStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                reStr = reader.ReadToEnd();
                reader.Close();
                Console.WriteLine("返回：" + reStr);
                return reStr;
            }
            catch (Exception e)
            {
                LogHelper.WriteErrorLog<Fun>(e.ToString());
                return reStr;
            }
        }

        // public static CookieContainer ExecuteGetCookieAndPic(string url,ref Bitmap sourcebm)
        // {
        //     string content = string.Empty;
        //     try
        //     {
        //         CookieContainer myCookieContainer = new CookieContainer();
        //         HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
        //         HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //         string cookieStr = response.Headers["set-cookie"];
        //         Stream resStream = response.GetResponseStream();//得到验证码数据流
        //         sourcebm = new Bitmap(resStream);//初始化Bitmap图片
        //         CookieContainer reList = new CookieContainer();
        //         var cookieVal = Fun.Substring(cookieStr, "=", ";");
        //         reList.Add(new Cookie("PHPSESSID", cookieVal, "/", "panda.xmxing.net"));
        //         return reList;
        //     }
        //     catch (Exception ex)
        //     {
        //         return null;
        //     }
        // }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inStr"></param>
        /// <param name="startStr"></param>
        /// <param name="endStr"></param>
        /// <returns></returns>
        public static string Substring(string inStr, string startStr, string endStr)
        {
            if (string.IsNullOrEmpty(inStr)) return "";
            int s = inStr.IndexOf(startStr);
            if (s > 0)
            {
                var tmp = inStr.Substring(s + startStr.Length);
                int e = tmp.IndexOf(endStr);
                if (e > 0)
                {
                    return tmp.Substring(0, e);
                }
            }
            return "";
        }

        /// <summary>
        /// 下载
        /// </summary>
        public static bool DownLoadSoft(string DownloadPath, string url, string FileName)
        {
            string path = DownloadPath.Remove(DownloadPath.Length - 1);
            bool flag = false;
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (FileStream fs = new FileStream(DownloadPath + FileName, FileMode.Create, FileAccess.Write))
                {
                    //创建请求
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    //接收响应
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    //输出流
                    Stream responseStream = response.GetResponseStream();
                    byte[] bufferBytes = new byte[10000];//缓冲字节数组
                    int bytesRead = -1;
                    while ((bytesRead = responseStream.Read(bufferBytes, 0, bufferBytes.Length)) > 0)
                    {
                        fs.Write(bufferBytes, 0, bytesRead);
                    }
                    if (fs.Length > 0)
                    {
                        flag = true;
                    }
                    //关闭写入
                    fs.Flush();
                    fs.Close();
                }

            }
            catch (Exception exp)
            {
                LogHelper.WriteErrorLog<Fun>(exp.ToString());
                //返回错误消息
            }
            return flag;
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="powerInt"></param>
        /// <returns></returns>
        public static List<int> GetPowerList(string powerInt)
        {
            switch (powerInt)
            {
                case "7":
                    return new List<int>() { 1, 2, 4 };
                case "6":
                    return new List<int>() { 2, 4 };
                case "5":
                    return new List<int>() { 1, 4 };
                case "4":
                    return new List<int>() { 4 };
                case "3":
                    return new List<int>() { 1, 2 };
                case "2":
                    return new List<int>() { 2 };
                case "1":
                    return new List<int>() { 1 };
                default:
                    return new List<int>() { 4 };
            }
        }
        /// <summary>
        /// 格式华农历时间
        /// </summary>
        /// <param name="inTime"></param>
        /// <returns></returns>
        public static string FormatLunlarTime(DateTime? inTime)
        {

            var reStr = "";

            var arrList = new List<string>() { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };

            if (inTime == null) return reStr;
            reStr += FormatNumToChinese(inTime.Value.Year) + "年";
            #region 格式化月

            if (inTime.Value.Month < 10)
            {
                reStr += FormatNumToChinese(inTime.Value.Month) + "月";
            }
            else if (inTime.Value.Month == 10)
            {
                reStr += "十月";
            }
            else if (inTime.Value.Month == 11)
            {
                reStr += "十一月";
            }
            else if (inTime.Value.Month == 12)
            {
                reStr += "十二月";
            }
            #endregion

            if (inTime.Value.Day < 10)
            {
                reStr += "初" + arrList[inTime.Value.Day];
            }
            else if (inTime.Value.Day == 10)
            {
                reStr += "十";
            }
            else if (inTime.Value.Day < 20)
            {
                reStr += "十" + arrList[inTime.Value.Day - 10];
            }
            else if (inTime.Value.Day == 20)
            {
                reStr += "廿";
            }
            else if (inTime.Value.Day < 30)
            {
                reStr += "廿" + arrList[inTime.Value.Day - 20];
            }
            else if (inTime.Value.Day == 30)
            {
                reStr += "三十";
            }
            else if (inTime.Value.Day <= 31)
            {
                reStr += "三十" + arrList[inTime.Value.Day - 30];
            }

            if (inTime.Value.Hour != 0)
            {
                reStr += HourToShiChen(inTime.Value.Hour) + "时";
            }
            
            return reStr;
        }

        public static string FormatNumToChinese(int? inV)
        {
            if (inV == null) return "";
            return FormatNumToChinese(inV.ToString());
        }

        public static string FormatNumToChinese(string inV)
        {
            if (inV == null) return "";
            var arrList = new List<string>() { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九", "十" };
            StringBuilder sb = new StringBuilder();
            foreach (var item in inV)
            {
                if (item.ToString().IsInt32())
                {
                    sb.Append(arrList[Convert.ToInt32(item.ToString())]);
                }
                else
                {
                    sb.Append(item);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 数字转中文
        /// </summary>
        /// <param name="number">eg: 22</param>
        /// <returns></returns>
        public static string NumberToChinese(int number)
        {
            string res = string.Empty;
            string str = number.ToString();
            string schar = str.Substring(0, 1);
            switch (schar)
            {
                case "1":
                    res = "一";
                    break;
                case "2":
                    res = "二";
                    break;
                case "3":
                    res = "三";
                    break;
                case "4":
                    res = "四";
                    break;
                case "5":
                    res = "五";
                    break;
                case "6":
                    res = "六";
                    break;
                case "7":
                    res = "七";
                    break;
                case "8":
                    res = "八";
                    break;
                case "9":
                    res = "九";
                    break;
                default:
                    res = "零";
                    break;
            }
            if (str.Length > 1)
            {
                switch (str.Length)
                {
                    case 2:
                    case 6:
                        res += "十";
                        break;
                    case 3:
                    case 7:
                        res += "百";
                        break;
                    case 4:
                        res += "千";
                        break;
                    case 5:
                        res += "万";
                        break;
                    default:
                        res += "";
                        break;
                }
                res += NumberToChinese(int.Parse(str.Substring(1, str.Length - 1)));
            }
            res = res.Trim('零');
            return res;
        }

        public static string HourToShiChen(int hour)
        {
            if (hour >= 23) return "子";
            string nameList = "子丑寅卯辰巳午未申酉戌亥";
            return nameList[(hour + 1) / 2].ToString();
        }




        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string HashEncrypt(string Text, string sKey = "")
        {

            if (string.IsNullOrEmpty(sKey))
            {
                sKey = ";fg;(&*?>\":;dSnUoMO,3zy6";
                //sKey = HashEncrypt(Text, "lfi8&%9lJYU6^%\"?>KR");
            }
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream();
            byte[] inputByteArray;
            inputByteArray = Encoding.UTF8.GetBytes(Text);
            des.Key = ASCIIEncoding.ASCII.GetBytes(Fun.Md5Hash(sKey).ToUpper().Substring(0, 8));
            des.IV = des.Key;
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }

        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string HashDecrypt(string Text, string sKey = "")
        {
            try
            {
                if (Text == null)
                {
                    return "";
                }
                if (string.IsNullOrEmpty(sKey))
                {
                    sKey = ";fg;(&*?>\":;dSnUoMO,3zy6";

                    //sKey = HashDecrypt("C350FB38CF25BB01CAE1936AD3B6926938C85DE82DF42C936EE969DBD0BC2C27", "lfi8&%9lJYU6^%\"?>KR");
                }
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                int len;
                len = Text.Length / 2;
                byte[] inputByteArray = new byte[len];
                int x, i;
                for (x = 0; x < len; x++)
                {
                    i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                    inputByteArray[x] = (byte)i;
                }
                des.Key = ASCIIEncoding.ASCII.GetBytes(Fun.Md5Hash(sKey).ToUpper().Substring(0, 8));
                des.IV = des.Key;
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                des.Mode = CipherMode.CBC;
                des.Padding = PaddingMode.PKCS7;
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                LogHelper.WriteErrorLog(typeof(Fun), e.ToString());
                return "";
            }

        }
    }

}