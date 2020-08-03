using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Helper;
using IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Models;
using Newtonsoft.Json.Linq;

namespace WxProductApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("user/[controller]/[action]")]
    [EnableCors]
    [ApiController]
    [Authorize]
    public class LoginController : ControllerBase, ILoginController
    {
        ILoginRepository _login;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientFactory"></param>
        /// <param name="login"></param>
        public LoginController( ILoginRepository login)
        {
            _login = login;
        }

        /// <summary>
        /// 密码登录
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<ResultObj<String>> userLogin(LogingDto inEnt)
        {
            ResultObj<String> reobj = new ResultObj<String>();

            var loginResult = await _login.UserLogin(inEnt);
            if (loginResult.success)
            {
                var client = new HttpClient();

                // var paras = new Dictionary<string, string>();
                // paras.Add("userObjJson", TypeChange.ObjectToStr(loginResult.data));
                // var tokeStr=Fun.HashEncrypt($"{DataTimeHelper.getDateLong(DateTime.Now)}|{loginResult.data.id}|{loginResult.data.loginName}|{loginResult.data.name}");
                var tokeStr=Helper.AuthHelper.GenerateToken(loginResult.data);
                reobj.success = true;
                reobj.code = tokeStr;
                reobj.data = TypeChange.ObjectToStr(reobj);
                
            }
            else
            {
                reobj.success=false;
                reobj.msg=loginResult.msg;
            }
            return reobj;
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public  Result loginOut(LogingDto inEnt)
        {
            var reObj = new Result();
            reObj.success = true;
            return reObj;
        }


        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultObj<int>> loginReg(LogingDto inEnt)
        {
            var reObj = new ResultObj<int>();
            try
            {
                return await _login.LoginReg(inEnt);
            }
            catch (Exception e)
            {
                reObj.success = false;
                reObj.msg = e.Message;
            }
            return reObj;
        }


        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<Result> deleteUser(DtoKey userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修改用户 名
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultObj<bool>> userEditPwd(EditPwdDto inEnt)
        {
            var reObj = new ResultObj<bool>();
            try
            {
                reObj = await this._login.UserEditPwd(inEnt);
            }
            catch (Exception e)
            {
                reObj.success = false;
                reObj.msg = e.Message;
            }
            return reObj;
        }

        /// <summary>
        /// 更新账号
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<Result> changeLoginName(ChangeLoginNameDto inEnt)
        {
            return _login.ChangeLoginName(inEnt);
        }
    }


}