
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 微信请求日记
    /// </summary>
    [Table("wx_user")]
    public class WxUserEntity : BaseModel
    {

        /// <summary>
        /// id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("openid")]
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 用户昵称
        /// </summary>
        [Column("nickname")]
        public string nickname { get; set; }

        /// <summary>
        /// sex	用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        [Column("sex")]
        public int sex { get; set; }

        /// <summary>
        /// 	用户个人资料填写的省份
        /// </summary>
        [Column("province")]
        public string province { get; set; }
        
        /// <summary>
        /// 普通用户个人资料填写的城市
        /// </summary>
        [Column("city")]
        public string city { get; set; }

        /// <summary>
        ///  国家，如中国为CN
        /// </summary>
        [Column("country")]
        public string country { get; set; }

        /// <summary>
        ///  用户头像
        /// </summary>
        [Column("headimgurl")]
        public string headimgurl { get; set; }

        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。
        /// </summary>
        [Column("unionid")]
        public string unionid { get; set; }


    }
}
