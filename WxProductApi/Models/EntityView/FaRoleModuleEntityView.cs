
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Entity;

namespace Models
{
    /// <summary>
    /// 用户扩展
    /// </summary>
    [Table(@"fa_role_module a left join 
             fa_role b on a.ROLE_ID=b.ID left join 
             fa_module c on a.MODULE_ID=c.ID")]
    public class FaRoleModuleEntityView
    {
        /// <summary>
        /// 角色名
        /// </summary>
        [Display(Name = "角色名")]
        [Column("b.NAME RoleName")]
        public string RoleName { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        /// <value></value>
        [Display(Name = "角色ID")]
        [Column("a.ROLE_ID")]
        public int ROLE_ID { get; set; }


        /// <summary>
        /// 模块ID
        /// </summary>
        /// <value></value>
        [Display(Name = "模块ID")]
        [Column("a.MODULE_ID")]
        public int MODULE_ID { get; set; }

                /// <summary>
        /// 上级
        /// </summary>
        [Range(0, 2147483647)]
        [Display(Name = "上级")]
        [Column("c.PARENT_ID")]
        public Nullable<int> PARENT_ID { get; set; }
        /// <summary>
        /// 模块名
        /// </summary>
        [StringLength(60)]
        [Display(Name = "模块名")]
        [Column("c.NAME")]
        public string NAME { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [StringLength(2000)]
        [Display(Name = "地址")]
        [Column("c.LOCATION")]
        public string LOCATION { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        [StringLength(20)]
        [Display(Name = "代码")]
        [Column("c.CODE")]
        public string CODE { get; set; }
        /// <summary>
        /// 调试
        /// </summary>
        [Required]
        [Range(0, 2147483647)]
        [Display(Name = "调试")]
        [Column("c.IS_DEBUG")]
        public Int16 IS_DEBUG { get; set; }
        /// <summary>
        /// 隐藏
        /// </summary>
        [Required]
        [Range(0, 2147483647)]
        [Display(Name = "隐藏")]
        [Column("c.IS_HIDE")]
        public Int16 IS_HIDE { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        [Range(0, 2147483647)]
        [Display(Name = "排序")]
        [Column("c.SHOW_ORDER")]
        public Int16 SHOW_ORDER { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(2000)]
        [Display(Name = "描述")]
        [Column("c.DESCRIPTION")]
        public string DESCRIPTION { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        [StringLength(2000)]
        [Display(Name = "图片")]
        [Column("c.IMAGE_URL")]
        public string IMAGE_URL { get; set; }
        /// <summary>
        /// 桌面角色
        /// </summary>
        [StringLength(200)]
        [Display(Name = "桌面角色")]
        [Column("c.DESKTOP_ROLE")]
        public string DESKTOP_ROLE { get; set; }
        /// <summary>
        /// 宽
        /// </summary>
        [Range(0, 2147483647)]
        [Display(Name = "宽")]
        [Column("c.W")]
        public Nullable<int> W { get; set; }
        /// <summary>
        /// 高
        /// </summary>
        [Range(0, 2147483647)]
        [Display(Name = "高")]
        [Column("c.H")]
        public Nullable<int> H { get; set; }
    }
}
