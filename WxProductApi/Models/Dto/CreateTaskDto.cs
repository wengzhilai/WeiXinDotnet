
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// 创建任务
    /// </summary>
    public class CreateTaskDto
    {
        /// <summary>
        /// 任务类型，0或null表示是非流程任务
        /// </summary>
        /// <value></value>
        [Required, MaxLength(20)]
        public int? TaskFlowId { get; set; }


        /// <summary>
        /// 任务名称
        /// </summary>
        /// <value></value>
        [Required, MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        /// <value></value>
        public string KeyId { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        /// <value></value>
        public string Remark { get; set; }
        
    }
}