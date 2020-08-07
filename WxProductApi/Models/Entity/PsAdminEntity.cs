
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 管理员
    /// </summary>
    [Table("ps_admin")]
    public class PsAdminEntity : BaseModel
    {

        /// <summary>
        /// openid
        /// </summary>
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Key]
        [Column("openid")]
        public string openid { get; set; }
        /// <summary>
        /// user_id
        /// </summary>
        [Column("user_id")]
        public int? userId { get; set; }
        /// <summary>
        /// create_time
        /// </summary>
        [Column("create_time")]
        public Int64? createTime { get; set; }
    }
}
