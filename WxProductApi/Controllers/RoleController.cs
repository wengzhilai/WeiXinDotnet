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
    [Route("[controller]/[action]")]
    [ApiController]
    [EnableCors]
    [Authorize]
    public class RoleController : ControllerBase, IRoleController
    {
        IRoleRepository _respoitory;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Role"></param>
        public RoleController(IRoleRepository Role)
        {
            this._respoitory = Role;
        }


        /// <summary>
        /// 保存Query
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultObj<int>> save(DtoSave<SysRoleEntity> inEnt)
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
        public async Task<ResultObj<SysRoleEntity>> singleByKey(DtoDo<int> inEnt)
        {
            ResultObj<SysRoleEntity> reObj = new ResultObj<SysRoleEntity>();
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