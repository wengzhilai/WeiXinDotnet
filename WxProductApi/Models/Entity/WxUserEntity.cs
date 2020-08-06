
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// IP地址
        /// </summary>
        [Column("ip")]
        public string ip { get; set; }

        /// <summary>
        /// 真实地址
        /// </summary>
        [Column("address")]
        public string address { get; set; }

        /// <summary>
        /// 真实地址
        /// </summary>
        [Column("create_time")]
        public long createTime { get; set; }

        /// <summary>
        /// 真实地址
        /// </summary>
        [Column("last_time")]
        public long lastTime { get; set; }




        /// <summary>
        /// language
        /// </summary>
        [Required]
        [StringLength(20)]
        [Column("language")]
        public string language { get; set; }
        /// <summary>
        /// subscribe
        /// </summary>
        [Required]
        [Column("subscribe")]
        public int subscribe { get; set; }
        /// <summary>
        /// subscribe_time
        /// </summary>
        [Required]
        [Column("subscribe_time")]
        public Int64 subscribe_time { get; set; }
        /// <summary>
        /// remark
        /// </summary>
        [Required]
        [StringLength(100)]
        [Column("remark")]
        public string remark { get; set; }
        /// <summary>
        /// groupid
        /// </summary>
        [Required]
        [Column("groupid")]
        public int groupid { get; set; }
        /// <summary>
        /// tagid_list_str
        /// </summary>
        [Required]
        [StringLength(100)]
        [Column("tagid_list_str")]
        public string tagidListStr { get {return string.Join(",",tagid_list);}}
        /// <summary>
        /// subscribe_scene
        /// </summary>
        [Required]
        [StringLength(30)]
        [Column("subscribe_scene")]
        public string subscribe_scene { get; set; }
        /// <summary>
        /// qr_scene
        /// </summary>
        [Required]
        [StringLength(10)]
        [Column("qr_scene")]
        public string qr_scene { get; set; }
        /// <summary>
        /// qr_scene_str
        /// </summary>
        [Required]
        [StringLength(10)]
        [Column("qr_scene_str")]
        public string qr_scene_str { get; set; }

        /// <summary>
        /// 用户被打上的标签ID列表
        /// </summary>
        /// <value></value>
        public List<int> tagid_list{get;set;}=new List<int>();
        
    }
}
