
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 登录历史
    /// </summary>
    [Table("sys_module")]
    public class SysModuleEntity : BaseModel
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required]
        [Display(Name = "id")]
        [Column("id")]
        public int id { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        [Display(Name = "父ID")]
        [Column("parent_id")]
        public int parentId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        [Display(Name = "模块名称")]
        [Column("name")]
        public String name { get; set; }

        /// <summary>
        /// 连接地址
        /// </summary>
        [Display(Name = "连接地址")]
        [Column("location")]
        public String location { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        [Display(Name = "代码")]
        [Column("code")]
        public String code { get; set; }

        /// <summary>
        /// 调试
        /// </summary>
        [Display(Name = "调试")]
        [Column("is_debug")]
        public Decimal isDebug { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
        [Display(Name = "隐藏")]
        [Column("is_hide")]
        public Decimal isHide { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [Column("show_order")]
        public Decimal showOrder { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        [Column("description")]
        public String description { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        [Display(Name = "图片地址")]
        [Column("image_url")]
        public String imageUrl { get; set; }

        /// <summary>
        /// 桌面
        /// </summary>
        [Display(Name = "桌面")]
        [Column("desktop_role")]
        public String desktopRole { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        [Display(Name = "宽")]
        [Column("w")]
        public int w { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        [Display(Name = "高")]
        [Column("h")]
        public int h { get; set; }


        /// <summary>
        /// 所有子项
        /// </summary>
        /// <value></value>
        public List<SysModuleEntity> children { get; set; }
    }
}
