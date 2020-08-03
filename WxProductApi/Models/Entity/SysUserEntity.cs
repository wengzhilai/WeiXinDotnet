
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 系统用户
    /// </summary>
    [Table("sys_user")]
    public class SysUserEntity : BaseModel
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
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Column("name")]
        public String name { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        [Display(Name = "登录账号")]
        [Column("login_name")]
        public String loginName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [Display(Name = "头像")]
        [Column("icon_files")]
        public String iconFiles { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [Display(Name = "区域")]
        [Column("district_id")]
        public int? districtId { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        [Display(Name = "是否可用")]
        [Column("status")]
        public int status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Display(Name = "创建时间")]
        [Column("create_time")]
        public Int64 createTime { get; set; }

        

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [Column("remark")]
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
