
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// 用户登录实体
    /// </summary>
    public class ResetPasswordDto
    {
        /// <summary>
        /// 构造
        /// </summary>
        public ResetPasswordDto()
        {
        }
        /// <summary>
        /// 验证码
        /// </summary>
        public string VerifyCode { get; set; }
        /// <summary>
        /// 登录码
        /// </summary>
        /// <value></value>
        public string LoginName { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        /// <value></value>
        public string NewPwd { get; set; }

        /// <summary>
        /// 用于短信验证码
        /// </summary>
        public string msg_id { get; set; }
        
    }
}