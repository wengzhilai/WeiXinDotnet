
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 产品明细
    /// </summary>
    [Table("ps_goods_log")]
    public class PsGoodsLogEntity : BaseModel
    {

            /// <summary>
            /// id
            /// </summary>
            [Key]
            [Required]
            [Column("id")]
            public int id { get; set; }
            /// <summary>
            /// goods_guid
            /// </summary>
            [Required]
            [StringLength(32)]
            [Column("goods_guid")]
            public string goodsGuid { get; set; }
            /// <summary>
            /// openid
            /// </summary>
            [Required]
            [StringLength(100)]
            [Column("openid")]
            public string openid { get; set; }
            /// <summary>
            /// create_time
            /// </summary>
            [Required]
            [Column("create_time")]
            public Int64 createTime { get; set; }
            /// <summary>
            /// ip
            /// </summary>
            [Required]
            [StringLength(16)]
            [Column("ip")]
            public string ip { get; set; }
            
       
    }
}
