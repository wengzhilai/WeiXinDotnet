
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 微信请求日记
    /// </summary>
    [Table("wx_quest_log")]
    public class WxQuestLogEntity : BaseModel
    {

        /// <summary>
        /// id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required]
        [Column("id")]
        public int id { get; set; }
        /// <summary>
        /// 目标用户
        /// </summary>
        [Required]
        [StringLength(100)]
        [Column("to_user_name")]
        public string toUserName { get; set; }
        /// <summary>
        /// from_user_name
        /// </summary>
        [Required]
        [StringLength(100)]
        [Column("from_user_name")]
        public string fromUserName { get; set; }
        /// <summary>
        /// msg_type
        /// </summary>
        [Required]
        [StringLength(100)]
        [Column("msg_type")]
        public string msgType { get; set; }
        /// <summary>
        /// content
        /// </summary>
        [Required]
        [StringLength(1000)]
        [Column("content")]
        public string content { get; set; }
        /// <summary>
        /// event_type
        /// </summary>
        [Required]
        [StringLength(100)]
        [Column("event_type")]
        public string eventType { get; set; }
        /// <summary>
        /// event_key
        /// </summary>
        [Required]
        [StringLength(100)]
        [Column("event_key")]
        public string eventKey { get; set; }
        /// <summary>
        /// ticket
        /// </summary>
        [StringLength(100)]
        [Column("ticket")]
        public string ticket { get; set; }


    }
}
