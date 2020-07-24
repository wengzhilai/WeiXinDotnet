
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// 修改用户密码
    /// </summary>
    public class EditPwdDto
    {
        /// <summary>
        /// 构造
        /// </summary>
        public EditPwdDto()
        {
        }

        /// <summary>
        /// 用户登录名
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPwd { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        /// <value></value>
        public string NewPwd { get; set; }
        /// <summary>
        /// 重置新密码
        /// </summary>
        /// <value></value>
        public string ReNewPwd { get; set; }
        
    }
}