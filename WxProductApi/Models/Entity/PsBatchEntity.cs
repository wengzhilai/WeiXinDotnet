
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 产品批次
    /// </summary>
    [Table("ps_batch")]
    public class PsBatchEntity : BaseModel
    {

        /// <summary>
        /// id
        /// </summary>
        [Key]
        [Required]
        [Column("id")]
        public int id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        [Column("create_time")]
        public Int64 createTime { get; set; }
        /// <summary>
        /// 介绍
        /// </summary>
        [StringLength(1000)]
        [Column("introduce")]
        public string introduce { get; set; }
        /// <summary>
        /// 产品数量
        /// </summary>
        [Required]
        [Column("goods_num")]
        public int goodsNum { get; set; }

        /// <summary>
        /// 下载次数
        /// </summary>
        [Required]
        [Column("down_num")]
        public int downNum { get; set; }


    }
}
