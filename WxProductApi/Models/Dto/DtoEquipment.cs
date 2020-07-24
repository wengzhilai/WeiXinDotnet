
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// 用于设备操作
    /// </summary>
    public class DtoEquipment
    {
        /// <summary>
        /// 构造
        /// </summary>
        public DtoEquipment()
        {

        }
        /// <summary>
        /// 传入的值
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        /// <value></value>
        public int Id { get; set; }

        /// <summary>
        /// 数据内容
        /// </summary>
        /// <value></value>
        public string DataStr { get; set; }
    }
}