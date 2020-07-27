

namespace Models.Redis
{
    /// <summary>
    /// 用户缓存信息
    /// </summary>
    public class UserRedis : Models.Entity.FaUserEntity
    {
        /// <summary>
        ///  用户令牌
        /// </summary>
        /// <value></value>
        public string Token { get; set; }
    }
}