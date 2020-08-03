
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 登录
    /// </summary>
    [Table("sys_login")]
    public class SysLoginEntity : BaseModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required]
        [Display(Name = "ID")]
        [Column("id")]
        public int id { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [Display(Name = "登录名")]
        [Column("login_name")]
        public String loginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [Column("password")]
        public String password { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [Display(Name = "联系电话")]
        [Column("phone_no")]
        public String phoneNo { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        [Display(Name = "电子邮件")]
        [Column("email_addr")]
        public String emailAddr { get; set; }

        /// <summary>
        /// 验证码
        /// </summary>
        [Display(Name = "验证码")]
        [Column("verify_code")]
        public String verifyCode { get; set; }

        /// <summary>
        /// 验证码时间
        /// </summary>
        [Display(Name = "验证码时间")]
        [Column("verify_time")]
        public Int64 verifyTime { get; set; }

        /// <summary>
        /// 是否禁用
        /// </summary>
        [Display(Name = "是否禁用")]
        [Column("is_locked")]
        public int isLocked { get; set; }

        /// <summary>
        /// 密码修改时间
        /// </summary>
        [Display(Name = "密码修改时间")]
        [Column("pass_update_date")]
        public Int64 passUpdateDate { get; set; }

        /// <summary>
        /// 禁用原因
        /// </summary>
        [Display(Name = "禁用原因")]
        [Column("locked_reason")]
        public String lockedReason { get; set; }

        /// <summary>
        /// 失败次数
        /// </summary>
        [Display(Name = "失败次数")]
        [Column("fail_count")]
        public int failCount { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        [Display(Name = "登录次数")]
        [Column("login_count")]
        public int loginCount { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Display(Name = "最后登录时间")]
        [Column("last_login_time")]
        public Int64 lastLoginTime { get; set; }

        /// <summary>
        /// 最后登出时间
        /// </summary>
        [Display(Name = "最后登出时间")]
        [Column("last_logout_time")]
        public Int64 lastLogoutTime { get; set; }

        /// <summary>
        /// 最后活动时间
        /// </summary>
        [Display(Name = "最后活动时间")]
        [Column("last_active_time")]
        public Int64 lastActiveTime { get; set; }


    }

}
