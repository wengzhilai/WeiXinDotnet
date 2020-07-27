using System.Threading.Tasks;

namespace Repository
{
    public class RedisRepository
    {
        private static string _userTokenKey = "UserToken_{0}";

        /// <summary>
        /// 保存，用户登录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool UserTokenSet(int userId, string key)
        {
            try
            {
                return Helper.RedisWriteHelper.HashSetKey<Models.Redis.UserRedis>(string.Format(_userTokenKey, userId), "Token", key);
            }
            catch
            {
                return true;
            }
        }



        /// <summary>
        /// 删除用户是的Token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static bool UserTokenDelete(int userId)
        {
            try
            {
                return Helper.RedisWriteHelper.KeyDelete(string.Format(_userTokenKey, userId));

            }
            catch
            {
                return true;
            }
        }

    }
}