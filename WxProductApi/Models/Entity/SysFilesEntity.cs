
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 登录
    /// </summary>
    [Table("sys_files")]
    public class SysFilesEntity : BaseModel
    {
        /// <summary>
        /// ID
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
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
        /// 大小
        /// </summary>
        [Range(0, 2147483647)]
        [Display(Name = "大小")]
        [Column]
        public long length { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [Display(Name = "添加时间")]
        [Column("upload_time")]
        public long uploadTime { get; set; }
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
        [Column("file_type")]
        public string fileType { get; set; }

        /// <summary>
        /// 文件md5
        /// </summary>
        /// <value></value>
        [StringLength(32)]
        [Display(Name = "文件md5")]
        [Column("md5_str")]
        public string md5Str{get;set;}
        /// <summary>
        /// 文件base64
        /// </summary>
        [Display(Name = "文件base64")]
        [Column("base64_str")]
        public string base64Str{get;set;}
        /// <summary>
        /// 是否在用,0给吗没用，1表示在用
        /// </summary>
        [Column("is_use")]
        public int isUse{get;set;}
    }
}
