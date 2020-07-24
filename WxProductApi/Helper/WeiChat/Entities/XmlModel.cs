using System;
using System.Collections.Generic;
using System.Text;

namespace Helper.WeiChat.Entities
{
    /// <summary>
    /// 微信的Xml结构
    /// </summary>
    public class XmlModel
    {
        /// <summary>
        /// 公众号的微信号
        /// </summary>
        public string ToUserName { get; set; }
        /// <summary>
        /// 用户的微信号
        /// </summary>
        public string FromUserName { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public Int64 CreateTime { get; set; }
        /// <summary>
        /// 消息类型
        ///  text, image, voice, video, link, location, event
        /// </summary>
        public string MsgType { get; set; }
        /// <summary>
        /// 事件
        /// subscribe,unsubscribe,CLICK,VIEW
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string EventKey { get; set; }
        /// <summary>
        /// 获取的二维码ticket
        /// </summary>
        public string Ticket { get; set; }
    }

}
