
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Entity;

namespace Models
{
    /// <summary>
    /// 用户扩展
    /// </summary>
    [Table(@"sys_role_module a left join 
             sys_role b on a.role_id=b.id left join 
             sys_module c on a.module_id=c.id")]
    public class FaRoleModuleEntityView
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [Display(Name = "角色名")]
        [Column("b.name")]
        public string roleName { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        /// <value></value>
        [Display(Name = "角色ID")]
        [Column("a.role_id")]
        public int roleId { get; set; }


        /// <summary>
        /// 模块ID
        /// </summary>
        /// <value></value>
        [Display(Name = "模块ID")]
        [Column("a.module_id")]
        public int id { get; set; }

                /// <summary>
        /// 上级
        /// </summary>
        [Range(0, 2147483647)]
        [Display(Name = "上级")]
        [Column("c.parent_id")]
        public int parentId { get; set; }
        /// <summary>
        /// 模块名
        /// </summary>
        [StringLength(60)]
        [Display(Name = "模块名")]
        [Column("c.name")]
        public string name { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [StringLength(2000)]
        [Display(Name = "地址")]
        [Column("c.location")]
        public string location { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        [StringLength(20)]
        [Display(Name = "代码")]
        [Column("c.code")]
        public string code { get; set; }
        /// <summary>
        /// 调试
        /// </summary>
        [Required]
        [Range(0, 2147483647)]
        [Display(Name = "调试")]
        [Column("c.is_debug")]
        public Int16 isDebug { get; set; }
        /// <summary>
        /// 隐藏
        /// </summary>
        [Required]
        [Range(0, 2147483647)]
        [Display(Name = "隐藏")]
        [Column("c.is_hide")]
        public Int16 isHide { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        [Range(0, 2147483647)]
        [Display(Name = "排序")]
        [Column("c.show_order")]
        public Int16 showOrder { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(2000)]
        [Display(Name = "描述")]
        [Column("c.description")]
        public string description { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        [StringLength(2000)]
        [Display(Name = "图片")]
        [Column("c.image_url")]
        public string imageUrl { get; set; }
        /// <summary>
        /// 桌面角色
        /// </summary>
        [StringLength(200)]
        [Display(Name = "桌面角色")]
        [Column("c.desktop_role")]
        public string desktopRole { get; set; }
        /// <summary>
        /// 宽
        /// </summary>
        [Range(0, 2147483647)]
        [Display(Name = "宽")]
        [Column("c.w")]
        public Nullable<int> w { get; set; }
        /// <summary>
        /// 高
        /// </summary>
        [Range(0, 2147483647)]
        [Display(Name = "高")]
        [Column("c.h")]
        public Nullable<int> h { get; set; }
    }
}
