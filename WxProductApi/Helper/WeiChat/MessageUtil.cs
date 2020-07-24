using System;
using System.Collections.Generic;
using System.Text;

namespace Helper.WeiChat
{
    public class MessageUtil
    {
        public const String CLICK_ETC_BIND = "etcBind";
        public const String CLICK_ETC = "etc";
        public const String CLICK_ETC_INSTALL = "etc_install";
        public const String CLICK_GetMoney = "getMoney";
        public const String CLICK_DOWNURL = "get56Url";
        public const String MESSAGE_EVENT = "event";
        public const String EVENT_SUB = "subscribe";

        public static string downloadApp()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" 物流帮手下载地址:");
            sb.Append(" <a href=\"");
            sb.Append("https://gzhzc.56bs.cn/s");
            sb.Append(" \">点击下载</a>");
            return sb.ToString();
        }


        public static string etcInstallPlace()
        {
            return "ETC安装激活地点：<a href='http://wechatetc.56bs.cn/defaultReplyPage'>点击查询</a>，回复“省份/城市”名称进行精确查询";
        }

        public static string etcGetMoney(string fromUserName)
        {
            return "推广赚钱：<a href='http://t4.ngrok.wjbjp.cn/promote/index.html?openid=" + fromUserName + "'>点击推广</a>";
        }

        /// <summary>
        /// 生成文本信息
        /// </summary>
        /// <param name="toUserName"></param>
        /// <param name="fromUserName"></param>
        /// <param name="replay"></param>
        /// <returns></returns>
        public static string initText(string toUserName, string fromUserName, string replay)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("FromUserName", toUserName);
            dic.Add("ToUserName", fromUserName);
            dic.Add("MsgType", "text");
            dic.Add("CreateTime", DateTime.Now.Ticks.ToString());
            dic.Add("Content", replay);
            return TypeChange.DictToXml(dic);
        }
    }
}
