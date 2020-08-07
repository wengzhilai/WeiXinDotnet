using System.Linq;
namespace System.Security.Claims
{
    /// <summary>
    /// 
    /// </summary>
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int userId(this ClaimsPrincipal source)
        {
            int reInt = 0;
            var item = source.Claims.FirstOrDefault(x => x.Type == "id");
            if (int.TryParse(item.Value, out reInt))
            {
                return reInt;
            }
            return 0;
        }
    }
}