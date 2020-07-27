
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 登录历史
    /// </summary>
    [Table("fa_module")]
    public class FaModuleEntity : BaseModel
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        [Display(Name = "id")]
        [Column("ID")]
        public int id { get; set; }

        /// <summary>
        /// 父ID
        /// </summary>
        [Display(Name = "父ID")]
        [Column("PARENT_ID")]
        public int parentId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        [Display(Name = "模块名称")]
        [Column("NAME")]
        public String name { get; set; }

        /// <summary>
        /// 连接地址
        /// </summary>
        [Display(Name = "连接地址")]
        [Column("LOCATION")]
        public String location { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        [Display(Name = "代码")]
        [Column("CODE")]
        public String code { get; set; }

        /// <summary>
        /// 调试
        /// </summary>
        [Display(Name = "调试")]
        [Column("IS_DEBUG")]
        public Decimal isDebug { get; set; }

        /// <summary>
        /// 隐藏
        /// </summary>
        [Display(Name = "隐藏")]
        [Column("IS_HIDE")]
        public Decimal isHide { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [Display(Name = "排序")]
        [Column("SHOW_ORDER")]
        public Decimal showOrder { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [Display(Name = "描述")]
        [Column("DESCRIPTION")]
        public String description { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        [Display(Name = "图片地址")]
        [Column("IMAGE_URL")]
        public String imageUrl { get; set; }

        /// <summary>
        /// 桌面
        /// </summary>
        [Display(Name = "桌面")]
        [Column("DESKTOP_ROLE")]
        public String desktopRole { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        [Display(Name = "宽")]
        [Column("W")]
        public int w { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        [Display(Name = "高")]
        [Column("H")]
        public int h { get; set; }


        /// <summary>
        /// 所有子项
        /// </summary>
        /// <value></value>
        public List<FaModuleEntity> children { get; set; }
    }
}
