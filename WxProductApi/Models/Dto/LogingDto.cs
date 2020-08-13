
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// 用户登录实体
    /// </summary>
    public class LogingDto
    {
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string userName { get; set; }
        /// <summary>
        /// 登录名
        /// </summary>
        public string loginName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }
        
    }
}