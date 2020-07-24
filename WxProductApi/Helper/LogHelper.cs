using System;
using log4net;

namespace Helper
{
    public class LogHelper
    {
        private static ILog logger = LogManager.GetLogger("NETCoreRepository", typeof(LogHelper));

        private static ILog MakeLog(Type type)
        {
            return LogManager.GetLogger("NETCoreRepository", type);
        }

        #region 输出错误日志到Log4Net

        /// <summary>
        /// 输出错误日志到Log4Net
        /// </summary>
        /// <param name="ex"></param>
        public static void WriteErrorLog<T>(string msg,Exception ec=null)
        {
            WriteErrorLog(typeof(T), msg, ec);
        }

        public static void WriteErrorLog(Type type,string msg,Exception ec=null)
        {
            logger = MakeLog(type);
            logger.Error(msg, ec);
        }

        public static void WriteDebugLog<T>(string msg, Exception ec = null)
        {
            WriteDebugLog(typeof(T), msg, ec);
        }

        public static void WriteDebugLog(Type type, string msg, Exception ec = null)
        {
            logger = MakeLog(type);
            logger.Debug(msg, ec);
        }


        public static void WriteFatalLog<T>(string msg, Exception ec = null)
        {
            WriteFatalLog(typeof(T), msg, ec);
        }

        public static void WriteFatalLog(Type type, string msg, Exception ec = null)
        {
            logger = MakeLog(type);
            logger.Fatal(msg, ec);
        }

        /// <summary>
        /// 输出记录日志到Log4Net
        /// </summary>
        /// <param name="msg"></param>
        public static void WriteLog<T>(string msg)
        {
            WriteLog(typeof(T), msg);
        }
        public static void WriteLog(Type type,string msg)
        {
            logger = MakeLog(type);
            logger.Info(msg);
        }


        #endregion
    }
}