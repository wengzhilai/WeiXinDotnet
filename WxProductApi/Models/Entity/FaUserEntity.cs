
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 系统用户
    /// </summary>
    [Table("fa_user")]
    public class FaUserEntity : BaseModel
    {

        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Display(Name = "ID")]
        [Column("ID")]
        public int id { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Column("NAME")]
        public String name { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        [Display(Name = "登录账号")]
        [Column("LOGIN_NAME")]
        public String loginName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "头像")]
        [Column("ICON_FILES")]
        public String iconFiles { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [Display(Name = "区域")]
        [Column("DISTRICT_ID")]
        public int districtId { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        [Display(Name = "是否可用")]
        [Column("IS_LOCKED")]
        public Decimal isLocked { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Column("CREATE_TIME")]
        public DateTime createTime { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        [Display(Name = "登录次数")]
        [Column("LOGIN_COUNT")]
        public int loginCount { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Display(Name = "最后登录时间")]
        [Column("LAST_LOGIN_TIME")]
        public DateTime lastLoginTime { get; set; }

        /// <summary>
        /// 最后登出时间
        /// </summary>
        [Display(Name = "最后登出时间")]
        [Column("LAST_LOGOUT_TIME")]
        public DateTime lastLogoutTime { get; set; }

        /// <summary>
        /// 最后活动时间
        /// </summary>
        [Display(Name = "最后活动时间")]
        [Column("LAST_ACTIVE_TIME")]
        public DateTime lastActiveTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [Column("REMARK")]
        public String remark { get; set; }


        /// <summary>
        /// 是管理管理员
        /// </summary>
        /// <value></value>
        public bool isAdmin{ get; set; }
        
        /// <summary>
        /// 是普通管理员
        /// </summary>
        /// <value></value>
        public bool isLeader{ get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        /// <value></value>
        public List<int> roleIdList { get; set; }
        /// <summary>
        /// 可编辑的用户ID
        /// </summary>
        /// <value></value>
        public List<int> canEditIdList { get; set; }
    }
}
