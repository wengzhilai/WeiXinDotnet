
using Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using System.Linq;
using System.Threading.Tasks;
using WxProductApi.Config;

namespace WxProductApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        IOptions<AppConfig> appConfig;
        public TestController(IOptions<AppConfig> _appConfig){
            appConfig=_appConfig;
        }
        /// <summary>
        /// 测试超级管理员用户
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "superadmin")]
        public Result TestUser(DtoKey inEnt )
        {
            Result reEnt = new Result();
            reEnt.success = true;
            reEnt.msg = "User.Identity:" + TypeChange.ObjectToStr(User.Identity);
            reEnt.msg += "User.Claims:" + TypeChange.ObjectToStr(from c in User.Claims select new { c.Type, c.Value });

            return reEnt;
        }
        /// <summary>
        /// 接口测试
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        public Result Test(DtoKey inEnt)
        {
            Result reEnt = new Result();
            reEnt.success = true;
            reEnt.msg = "接口测试成功："+TypeChange.ObjectToStr(appConfig);
            return reEnt;
        }
        [HttpPost]
        public async Task<Result> TestDBAsync(DtoKey inEnt)
        {
            int i= await new DapperHelper().Exec("SELECT 1 ");
            Result reEnt = new Result();
            reEnt.success = true;
            reEnt.msg = "接口测试成功：数字"+i;
            return reEnt;
        }
        /// <summary>
        /// 权限接口测试
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public Result TestAuth(DtoKey inEnt)
        {
            Result reEnt = new Result();
            reEnt.success = true;
            reEnt.msg = "权限接口测试成功";
            return reEnt;
        }
    }
}