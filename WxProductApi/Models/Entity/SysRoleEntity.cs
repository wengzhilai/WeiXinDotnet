
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 系统角色
    /// </summary>
    [Table("sys_role")]
    public class SysRoleEntity : BaseModel
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
        /// 角色名
        /// </summary>
        [Display(Name = "角色名")]
        [Column("name")]
        public String name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [Display(Name = "备注")]
        [Column("remark")]
        public String remark { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        [Display(Name = "类型")]
        [Column("type")]
        public int type { get; set; }



        /// <summary>
        /// 模块ID集合
        /// </summary>
        /// <value></value>
        public int[] moduleIdStr { get; set; }
    }
}
