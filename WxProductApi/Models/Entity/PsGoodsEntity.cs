
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 产品明细
    /// </summary>
    [Table("ps_goods")]
    public class PsGoodsEntity : BaseModel
    {

        /// <summary>
        /// id
        /// </summary>
        [Key]
        [Required]
        [StringLength(32)]
        [Column("id")]
        public string id { get; set; }
        /// <summary>
        /// 产品编码
        /// </summary>
        [Required]
        [Column("pro_num")]
        public int proNum { get; set; }
        /// <summary>
        /// 防伪码
        /// </summary>
        [Required]
        [Column("code")]
        public string code { get; set; }

        /// <summary>
        /// 批次ID
        /// </summary>
        [Required]
        [Column("batch_id")]
        public int batchId { get; set; }

        /// <summary>
        /// 查看次数
        /// </summary>
        [Required]
        [Column("look_num")]
        public int lookNum { get; set; }
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        [Required]
        [StringLength(50)]
        [Column("openid")]
        public string openid { get; set; }
        /// <summary>
        /// 确认时间
        /// </summary>
        [Column("confirm_time")]
        public Int64 confirmTime { get; set; }

        /// <summary>
        /// 查看日志
        /// </summary>
        /// <value></value>
        public List<PsGoodsLogEntity> allLogs { get; set; }

        /// <summary>
        /// 产品批次编号
        /// </summary>
        /// <value></value>
        public string batchCode { get; set; }

    }
}
