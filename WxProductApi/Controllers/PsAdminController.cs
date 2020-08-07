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
    [Route("ps/[controller]/[action]")]
    [ApiController]
    [EnableCors]
    [Authorize]
    public class PsAdminController : ControllerBase
    {
        IPsAdminRepository _respoitory;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="module"></param>
        public PsAdminController(IPsAdminRepository module)
        {
            this._respoitory = module;
        }

        /// <summary>
        /// 保存Query
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultObj<int>> save(DtoSave<PsAdminEntity> inEnt)
        {
            ResultObj<int> reObj = new ResultObj<int>();
            try
            {
                inEnt.data.userId=User.userId();
                reObj = await _respoitory.Save(inEnt);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(this.GetType(), ex.ToString());
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
        public async Task<ResultObj<int>> delete(DtoDo<string> inEnt)
        {
            ResultObj<int> reObj = new ResultObj<int>();
            try
            {
                reObj = await _respoitory.Delete(inEnt.Key);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog(this.GetType(), ex.ToString());
                reObj.msg = ex.Message;
                reObj.success = false;
            }
            return reObj;
        }
    }
}