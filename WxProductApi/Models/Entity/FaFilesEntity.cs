
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 登录
    /// </summary>
    [Table("fa_files")]
    public class FaFilesEntity : BaseModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "ID")]
        [Column]
        public int id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(50)]
        [Display(Name = "名称")]
        [Column]
        public string name { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        [StringLength(200)]
        [Display(Name = "路径")]
        [Column]
        public string path { get; set; }
        /// <summary>
        /// USER_ID
        /// </summary>
        [Range(0, 2147483647)]
        [Display(Name = "USER_ID")]
        [Column]
        public Nullable<int> userId { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        [Range(0, 2147483647)]
        [Display(Name = "大小")]
        [Column]
        public Int64 length { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Display(Name = "添加时间")]
        [Column]
        public Nullable<DateTime> uploadTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(2000)]
        [Display(Name = "备注")]
        [Column]
        public string remark { get; set; }
        /// <summary>
        /// 相对路径
        /// </summary>
        [StringLength(254)]
        [Display(Name = "相对路径")]
        [Column]
        public string url { get; set; }
        /// <summary>
        /// 文件类型
        /// </summary>
        [StringLength(50)]
        [Display(Name = "文件类型")]
        [Column]
        public string fileType { get; set; }


    }
}
