
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
        /// <summary>
        /// 手机串号
        /// </summary>
        public string imei { get; set; }
        /// <summary>
        /// 版本号
        /// </summary>
        public string version { get; set; }
        /// <summary>
        /// 短信验证码
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public string userId { get; set; }
        /// <summary>
        /// 推荐码
        /// </summary>
        public string pollCode { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public int type { get; set; }
    }
}