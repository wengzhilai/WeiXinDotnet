﻿using System;
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
        public ConfigWebConfig WebConfig { get; set; } = new ConfigWebConfig();
        public WeiXin WeiXin { get; set; } = new WeiXin();

        public MysqlSettings MysqlSettings { get; set; }

        /// <summary>
        /// 基本配置
        /// </summary>
        /// <returns></returns>
        public BaseConfig BaseConfig { get; set; }
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
    public class MysqlSettings
    {
        public string server { get; set; }
        public string userid { get; set; }
        public string pwd { get; set; }
        public string port { get; set; } 
        public string database { get; set; }
        public string sslmode { get; set; }


    }

    /// <summary>
    ///  系统验证
    /// </summary>
    public class BaseConfig{
        /// <summary>
        /// 是否需要验证码
        /// </summary>
        /// <value></value>
        public bool VerifyCode { get; set; }
        /// <summary>
        /// 短信验证码有效时间
        /// </summary>
        /// <value></value>
        public int  VerifyExpireMinute{ get; set; }=30;

        /// <summary>
        /// 密码复杂度
        /// </summary>
        /// <value></value>
        public int PwdComplexity{get;set;}

    }
}
