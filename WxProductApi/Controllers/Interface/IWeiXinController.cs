using Models;
using Models.Entity;
using System.Threading.Tasks;

namespace WxProductApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWeiXinController
    {
        /// <summary>
        /// 保存微信连接日志
        /// </summary>
        /// <param name="inObj"></param>
        /// <returns></returns>
        Task<ResultObj<int>> saveLog(WxQuestLogEntity inObj);

        Task<ResultObj<int>> saveUser(WxQuestLogEntity inObj);

    }
}
