
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entity
{
    /// <summary>
    /// 
    /// </summary>
    [Table("sequence")]
    public class SequenceEntity
    {

        /// <summary>
        /// 表名
        /// </summary>
        [Key]
        [Required]
        [Display(Name = "表名")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(20)]
        [Column]
        public string seq_name { get; set; }
        /// <summary>
        /// 当前值
        /// </summary>
        [Required]
        [Display(Name = "当前值")]
        [Column]
        public int current_val { get; set; }

        /// <summary>
        /// 增加值
        /// </summary>
        [Required]
        [StringLength(20)]
        [Display(Name = "增加值")]
        [Column]
        public int increment_val { get; set; }


    }
}