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
using System.Text;
using System.Web;

namespace WxProductApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("ps/[controller]/[action]")]
    [ApiController]
    [EnableCors]
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
        /// <param name="_appConfig"></param>
        /// <param name="httpClientFactory"></param>
        /// <param name="weiXin"></param>
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
        [Authorize]
        [HttpPost]
        public Task<ResultObj<int>> Save(DtoSave<PsBatchEntity> inEnt)
        {
            return _respoitory.Save(inEnt);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">主键 ID</param>
        /// <returns></returns>

        [Authorize]
        [HttpPost]
        public Task<ResultObj<int>> Delete(int id)
        {
            return _respoitory.Delete(id);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> downCsv(string key)
        {
            var url=Request.Path.Value.Replace("downCsv","GoodsDetail");
            var hostAddress="t1.ngrok.wjbjp.cn";
            // hostAddress=Request.Host.Host;
            var reByte = await _respoitory.MakeCsvByte(int.Parse(key),$"http://{hostAddress}{url}?state=");
            return File(reByte, "application/octet-stream", $"{DateTime.Now.ToString("yyyyMMdd")}.csv");
        }


        /// <summary>
        /// 产品详情
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state">是产品的GUID</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task GoodsDetail(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                string ip = this.httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                state = $"{state}|{ip}";
                string rebackUrl = $"http://{Request.Host.Host}{Request.Path}";
                rebackUrl=HttpUtility.UrlEncode(rebackUrl);
                var url = Helper.WeiChat.Utility.GetWebpageAuthorization(appConfig.WeiXin.Appid, rebackUrl,Fun.Base64Encode(state), true);
                Response.Redirect(url);
            }
            else
            {
                var stateList = Fun.Base64Decode(state).Split('|');
                state = stateList[0];
                WxUserEntity wxUser = new WxUserEntity();
                try
                {
                    //出错后，返回重新查看
                    wxUser = Helper.WeiChat.Utility.GetWebpageUserInfo(appConfig.WeiXin.Appid, appConfig.WeiXin.Secret, code);
                }
                catch
                {
                    Response.Redirect($"GoodsDetail?state={state}");
                    return;
                }
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
                var reObj = await _respoitory.GoodsDetail(inLog);
                StringBuilder htmlStringBuilder = new StringBuilder();

                if (reObj.success)
                {
                    htmlStringBuilder.Append($"该产品是正品，已查阅{reObj.data.lookNum}次<br />");
                    if (reObj.data.confirmTime == 0)
                    {
                        htmlStringBuilder.Append($"还未被确认，<a onclick=\"checkGoods()\" href=\"#\">点击确认</a>");
                        var jsStr = @"
<script>
    function checkGoods(){
        var word = prompt('请输入产品编码',"""");
        if(word){
            alert('{state}_'+word)
            window.location='GoodsCheck?state={state}_'+word
        }
        else{
            alert('不能为空')
        }
    }
</script>
                        ";
                        jsStr = jsStr.Replace("{state}", state);
                        htmlStringBuilder.Append(jsStr);
                    }
                    else
                    {
                        htmlStringBuilder.Append($"确认时间：{Helper.DataTimeHelper.getDate(reObj.data.confirmTime).ToString()}\r\n<br />");
                        htmlStringBuilder.Append($"产品编号：{reObj.data.code}");
                    }
                }
                else
                {
                    htmlStringBuilder.Append(reObj.msg);
                }
                await ShowHtml(htmlStringBuilder.ToString());
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
        public async Task GoodsCheck(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
            {
                string ip = this.httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
                state = $"{state}|{ip}";
                var url = Helper.WeiChat.Utility.GetWebpageAuthorization(appConfig.WeiXin.Appid, $"http://{Request.Host.Host}{Request.Path}", Fun.Base64Encode(state), false);
                Response.Redirect(url);
            }
            else
            {
                var stateList = Fun.Base64Decode(state).Split('|');
                state = stateList[0];
                var goodsGuid = state.Split("_")[0];
                var goodsCode = state.Split("_")[1];

                WxUserEntity wxUser = new WxUserEntity();
                try
                {
                    //出错后，返回重新查看
                    wxUser = Helper.WeiChat.Utility.GetWebpageUserInfo(appConfig.WeiXin.Appid, appConfig.WeiXin.Secret, code);
                }
                catch
                {
                    await ShowHtml($"微信用户有误。<a href=\"GoodsDetail?state={goodsGuid}\">点击返回</a>");
                    return;
                }

                wxUser.ip = stateList[1];
                PsGoodsLogEntity inLog = new PsGoodsLogEntity()
                {
                    goodsGuid = goodsGuid,
                    ip = wxUser.ip,
                    openid = wxUser.openid
                };

                var reObj = await _respoitory.GoodsCheck(inLog, goodsCode);
                if (reObj.success)
                {
                    Response.Redirect($"GoodsDetail?state={goodsGuid}");
                }
                else
                {
                    await ShowHtml($"产品码有误。<a href=\"GoodsDetail?state={goodsGuid}\">点击返回</a>");
                }
            }
        }

        /// <summary>
        /// 显示网页
        /// </summary>
        /// <param name="htmlStr"></param>
        /// <returns></returns>
        private async Task ShowHtml(string htmlStr)
        {
            StringBuilder htmlStringBuilder = new StringBuilder();
            htmlStringBuilder.Append("<html>");
            htmlStringBuilder.Append("<head><meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" /> </head>");//支持中文
            htmlStringBuilder.Append("<body>");
            htmlStringBuilder.Append("<spen style=\"font-size: 300%\">");//让字体变大
            htmlStringBuilder.Append(htmlStr);
            htmlStringBuilder.Append("</spen>");
            htmlStringBuilder.Append("</body>");
            htmlStringBuilder.Append("</html>");
            var result = htmlStringBuilder.ToString();
            var data = Encoding.UTF8.GetBytes(result);
            Response.ContentType = "text/html";
            await Response.Body.WriteAsync(data, 0, data.Length);
        }
    }
}