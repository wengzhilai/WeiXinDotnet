
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models;
using Models.Entity;

namespace IRepository
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWeiXinRepository
    {
        /// <summary>
        /// 保存微信连接日志
        /// </summary>
        /// <param name="inObj"></param>
        /// <returns></returns>
        Task<ResultObj<int>> saveLog(WxQuestLogEntity inObj);
        /// <summary>
        /// 添加微信用户，
        /// </summary>
        /// <param name="inObj"></param>
        /// <returns></returns>
        Task<ResultObj<int>> saveUser(WxUserEntity inObj);

    }
}
