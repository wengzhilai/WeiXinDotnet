using System;
using System.Collections.Generic;
using System.Text;

namespace Helper.WeiChat.Entities
{
    public class JsApiModel
    {
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string noncestr { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public string timestamp { get; set; }
        /// <summary>
        /// 当前网页的URL
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string signature { get; set; }
        /// <summary>
        /// appid
        /// </summary>
        public string appid { get; set; }
    }
}
