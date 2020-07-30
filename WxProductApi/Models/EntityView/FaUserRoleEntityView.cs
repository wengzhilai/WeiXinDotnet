
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 系统角色
    /// </summary>
    [Table("sys_user_role ur LEFT JOIN sys_user u on u.ID=ur.user_id LEFT JOIN sys_role r on r.ID=ur.role_id ")]
    public class FaUserRoleEntityView
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [StringLength(80)]
        [Column("r.`name`")]
        public string roleName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Column("r.`type`")]
        public Nullable<int> roleType { get; set; }

        /// <summary>
        /// ROLE_ID
        /// </summary>
        [Required]
        [Column("ur.role_id")]
        public int roleId { get; set; }
        /// <summary>
        /// USER_ID
        /// </summary>
        [Required]
        [Key]
        [Column("ur.user_id")]
        public int userId { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [StringLength(80)]
        [Column("u.`name`")]
        public string userName { get; set; }
    }
}
