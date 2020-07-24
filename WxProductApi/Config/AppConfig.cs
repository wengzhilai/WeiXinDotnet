using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WxProductApi.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class AppConfig
    {
        /// <summary>
        /// 配置
        /// </summary>
        public static ConfigWebConfig WebConfig { get; set; } = new ConfigWebConfig();
        public static WeiXin WeiXin { get; set; } = new WeiXin();
    }

    /// <summary>
    /// 配置
    /// </summary>
    public class ConfigWebConfig
    {
        /// <summary>
        /// 推广一个客户的费用
        /// </summary>
        public int ClientPrice { get; set; }
    }

    public class WeiXin
    {
        public string Appid { get; set; }
        public string Secret { get; set; }
        public string Token { get; set; }
    }


}
