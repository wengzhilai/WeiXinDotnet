using System.Collections.Generic;

namespace Helper.WeiChat.Entities
{
    public class WebpageUserInfo
    {
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// sex	用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public int sex { get; set; }
        /// <summary>
        /// 	用户个人资料填写的省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        public string city { get; set; }

        /// <summary>
        ///  国家，如中国为CN
        /// </summary>
        public string country { get; set; }

        /// <summary>
        ///  用户头像
        /// </summary>
        public string headimgurl { get; set; }

        /// <summary>
        /// 用户特权信息
        /// </summary>
        public List<string> privilege { get; set; }



        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。
        /// </summary>
        public string unionid { get; set; }

        
    }
}
