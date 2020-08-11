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
using Microsoft.AspNetCore.Http;
using WxProductApi.Config;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Collections.Generic;

namespace WxProductApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("ps/[controller]/[action]")]
    [ApiController]
    [EnableCors]
    [Authorize]
    public class PsGoodsController : ControllerBase
    {
        IWeiXinRepository weiXin;

        IPsBatchRepository _respoitory;
        AppConfig appConfig;
        /// <summary>
        /// 用于获取IP地址
        /// </summary>
        IHttpContextAccessor httpContextAccessor;
        private IHttpClientFactory httpClientFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="module"></param>
        /// <param name="httpContextAccessor"></param>
        public PsGoodsController(IPsBatchRepository module, IHttpContextAccessor httpContextAccessor, IOptions<AppConfig> _appConfig, IHttpClientFactory httpClientFactory, IWeiXinRepository weiXin)
        {
            this._respoitory = module;
            this.httpContextAccessor = httpContextAccessor;
            this.appConfig = _appConfig.Value;
            this.httpClientFactory = httpClientFactory;
            this.weiXin = weiXin;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="inEnt"></param>
        /// <returns></returns>
        public Task<ResultObj<int>> Save(DtoSave<PsBatchEntity> inEnt)
        {
            return _respoitory.Save(inEnt);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键 ID</param>
        /// <returns></returns>

        public Task<ResultObj<int>> Delete(int id)
        {
            return _respoitory.Delete(id);
        }

        /// <summary>
        /// 生成批次csv文件
        /// </summary>
        /// <param name="batchId"></param>
        /// <returns></returns>
        public Task<byte[]> MakeCsvByte(int batchId)
        {
            return _respoitory.MakeCsvByte(batchId);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<IActionResult> downCsv(DtoKey code)
        {
            var reByte = await _respoitory.MakeCsvByte(int.Parse(code.Key));
            return File(reByte, "application/octet-stream", $"{DateTime.Now.ToString()}.csv");
        }


        /// <summary>
        /// 产品详情
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state">是产品的GUID</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<ResultObj<PsGoodsEntity>> GoodsDetail(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                string ip = this.httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                state = $"{state}|{ip}";
                var url = Helper.WeiChat.Utility.GetWebpageAuthorization(appConfig.WeiXin.Appid, $"http://{Request.Host.Host}{Request.Path}", state, false);
                Response.Redirect(url);
                return null;
            }
            else
            {
                var stateList = state.Split('|');
                state = stateList[0];
                var wxUser = Helper.WeiChat.Utility.GetWebpageUserInfo(appConfig.WeiXin.Appid, appConfig.WeiXin.Secret, code);
                wxUser.ip = stateList[1];
                var addressList = await httpClientFactory.CreateClient().GetAddressAsync(wxUser.ip);
                wxUser.address = string.Join("", addressList);
                var opNum = await weiXin.saveUser(wxUser, new List<string> { "nickname", "headimgurl", "lastTime", "ip", "address", "subscribe" });
                PsGoodsLogEntity inLog = new PsGoodsLogEntity()
                {
                    goodsGuid = state,
                    ip = wxUser.ip,
                    openid = wxUser.openid
                };
                return await _respoitory.GoodsDetail(inLog);
            }

        }
        /// <summary>
        /// 检测产品码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state">是产品的{guid}_{code}</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ResultObj<PsGoodsEntity>> GoodsCheck(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                string ip = this.httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                state = $"{state}|{ip}";
                var url = Helper.WeiChat.Utility.GetWebpageAuthorization(appConfig.WeiXin.Appid, $"http://{Request.Host.Host}{Request.Path}", state, false);
                Response.Redirect(url);
                return null;
            }
            else
            {
                var stateList = state.Split('|');
                state = stateList[0];
                var goodsGuid=state.Split("_")[0];
                var goodsCode=state.Split("_")[1];
                var wxUser = Helper.WeiChat.Utility.GetWebpageUserInfo(appConfig.WeiXin.Appid, appConfig.WeiXin.Secret, code);
                wxUser.ip = stateList[1];
                PsGoodsLogEntity inLog = new PsGoodsLogEntity()
                {
                    goodsGuid = goodsGuid,
                    ip = wxUser.ip,
                    openid = wxUser.openid
                };

                return await _respoitory.GoodsCheck(inLog,goodsCode);
            }
        }
    }
}