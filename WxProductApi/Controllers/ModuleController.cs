using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;
using System.Linq;
using System.Security.Claims;
using IRepository;
using System;
using Helper;
using Microsoft.AspNetCore.Cors;
using Models.Entity;

namespace WxProductApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("user/[controller]/[action]")]
    [ApiController]
    [EnableCors]
    [Authorize]
    public class ModuleController : ControllerBase, IModuleController
    {
        IModuleRepository _respoitory;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="module"></param>
        public ModuleController(IModuleRepository module)
        {
            this._respoitory = module;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        async public Task<ResultObj<SysModuleEntity>> getUserMenu()
        {
            var reObj = new ResultObj<SysModuleEntity>();
            try
            {
                var userId=User.userId();
                reObj = await this._respoitory.GetMGetMenuByUserId(Convert.ToInt32(userId));
            }
            catch (Exception e)
            {
                LogHelper.WriteErrorLog(this.GetType(), "获取用户详情失败", e);
                reObj.success = false;
                reObj.msg = e.Message;
            }
            return reObj;
        }

        /// <summary>
        /// 保存Query
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultObj<int>> save(DtoSave<SysModuleEntity> inEnt)
        {
            ResultObj<int> reObj = new ResultObj<int>();
            try
            {
                reObj = await _respoitory.Save(inEnt);

            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(typeof(QueryController), ex.ToString());
                reObj.msg = ex.Message;
                reObj.success = false;
            }
            return reObj;
        }

        /// <summary>
        /// 查找单条
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultObj<SysModuleEntity>> singleByKey(DtoDo<int> inEnt)
        {
            ResultObj<SysModuleEntity> reObj = new ResultObj<SysModuleEntity>();
            try
            {
                reObj.data = await _respoitory.SingleByKey(inEnt.Key);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(typeof(QueryController), ex.ToString());
                reObj.msg = ex.Message;
                reObj.success = false;
            }
            return reObj;
        }

        /// <summary>
        /// 删除单条
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultObj<int>> delete(DtoDo<int> inEnt)
        {
            ResultObj<int> reObj = new ResultObj<int>();
            try
            {
                reObj = await _respoitory.Delete(inEnt.Key);

            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(typeof(QueryController), ex.ToString());
                reObj.msg = ex.Message;
                reObj.success = false;
            }
            return reObj;
        }
    }
}