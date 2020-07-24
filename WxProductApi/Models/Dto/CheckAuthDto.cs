
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// 用于检测用户权限
    /// </summary>
    public class CheckAuthDto
    {
        /// <summary>
        /// 检测的用户
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 权限代码，最大值是:777，
        /// </summary>
        public string Authority { get; set; }
        /// <summary>
        /// 要检测的权限,1添加，2修改，4查看
        /// </summary>
        public int PowerNum { get; set; }
        /// <summary>
        /// 是否是创建者
        /// </summary>
        public bool IsCreater { get; set; }
    }
}