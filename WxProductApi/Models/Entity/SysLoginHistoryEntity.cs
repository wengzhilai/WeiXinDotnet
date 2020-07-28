
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 登录历史
    /// </summary>
    [Table("sys_login_history")]
    public class SysLoginHistoryEntity : BaseModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required]
        [Display(Name = "ID")]
        [Column("id")]
        public int ID { get; set; }
        /// <summary>
        /// USER_ID
        /// </summary>
        [Column("user_id")]
        public int? USER_ID { get; set; }
        /// <summary>
        /// LOGIN_TIME
        /// </summary>
        [Column("login_time")]
        public Int64 LOGIN_TIME { get; set; }
        /// <summary>
        /// LOGIN_HOST
        /// </summary>
        [StringLength(255)]
        [Column("login_host")]
        public string LOGIN_HOST { get; set; }
        /// <summary>
        /// LOGOUT_TIME
        /// </summary>
        [Column("logout_time")]
        public Int64 LOGOUT_TIME { get; set; }
        /// <summary>
        /// LOGIN_HISTORY_TYPE
        /// </summary>
        [Column("login_history_type")]
        public Nullable<int> LOGIN_HISTORY_TYPE { get; set; }
        /// <summary>
        /// MESSAGE
        /// </summary>
        [StringLength(255)]
        [Column("message")]
        public string MESSAGE { get; set; }
    }
}
