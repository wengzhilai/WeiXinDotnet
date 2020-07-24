
using Helper;
using Helper.WeiChat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Linq;
using System.IO;
using System.Text;
using Helper.WeiChat.Entities;
using WxProductApi.Config;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using System.Web;

namespace WxProductApi.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WeiXinController : ControllerBase
    {

        [HttpGet]
        public string index(string echostr)
        {

            PostModel postModel= TypeChange.UrlToEntities<PostModel>(Request.QueryString.Value);
            if (CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, AppConfig.WeiXin.Token))
            {
                return echostr; //返回随机字符串则表示验证通过
            }
            else
            {
                return "failed:" + postModel.Signature + "," +
                    "如果你在浏览器中看到这句话，说明此地址可以被作为微信公众账号后台的Url，请注意保持Token一致。";
            }

        }

        [HttpPost]
        public async Task<string> index()
        {
            PostModel postModel = TypeChange.UrlToEntities<PostModel>(Request.QueryString.Value);
            if (!CheckSignature.Check(postModel.Signature, postModel.Timestamp, postModel.Nonce, AppConfig.WeiXin.Token))
            {
                return "参数错误！";
            }
            else
            {
                Request.EnableBuffering();

                using (var reader = new StreamReader(Request.Body, encoding: Encoding.UTF8))
                {
                    var body =await reader.ReadToEndAsync();
                    var xml = TypeChange.XmlToDict(body);
                    String toUserName = xml.GetValueOrDefault("ToUserName");
                    String fromUserName = xml.GetValueOrDefault("FromUserName");
                    String msgType = xml.GetValueOrDefault("MsgType");
                    String content = xml.GetValueOrDefault("Content");
                    String eventType = xml.GetValueOrDefault("Event");
                    String eventKey = xml.GetValueOrDefault("EventKey");
                    String ticket = xml.GetValueOrDefault("Ticket");


                    if (MessageUtil.MESSAGE_EVENT.Equals(msgType))
                    {
                        switch (eventType.ToLower())
                        {
                            case "subscribe": //订阅
                                if (!string.IsNullOrEmpty(ticket) && !string.IsNullOrEmpty(fromUserName))
                                {
                                    // var saveEnt = new EtcWeixinEntity()
                                    // {
                                    //     openid = fromUserName,
                                    //     createTime = DateTime.Now,
                                    //     parentTicket = ticket,
                                    //     eventKey = eventKey,
                                    // };
                                    // var t= weixin.save(saveEnt);
                                }
                                break;
                            case "unsubscribe": //取消订阅
                                break;
                            case "click": //点击事件
                                String replay = "";
                                switch (eventKey)
                                {
                                    case MessageUtil.CLICK_DOWNURL:
                                        replay = MessageUtil.downloadApp();
                                        break;
      
                                    case MessageUtil.CLICK_ETC_INSTALL:
                                        replay = MessageUtil.etcInstallPlace();
                                        break;
                                    case MessageUtil.CLICK_GetMoney:
                                        replay = MessageUtil.etcGetMoney(fromUserName);
                                        break;
                                }
                                string message = MessageUtil.initText(toUserName, fromUserName, replay);
                                return message;
                            case "view": //url类型
                                break;
                        }

                    }

                    // Do some processing with body…
                    // Reset the request body stream position so the next middleware can read it
                    Request.Body.Position = 0;
                    return "";

                }

            }
        }

        [HttpGet]
        public void GetUserInfo(string key,string code){
            if(string.IsNullOrEmpty(code)){
                Response.Redirect($"https://open.weixin.qq.com/connect/oauth2/authorize?appid={AppConfig.WeiXin.Appid}&redirect_uri={ HttpUtility.UrlEncode("http://t1.ngrok.wjbjp.cn/WeiXin/GetUserInfo?code=%2211%22")}&response_type=code&scope=snsapi_userinfo&state=STATE#wechat_redirect");
            }
            else{
                var i=1;
            }
        }
        /// <summary>
        /// 用于微信绑定时间
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        [HttpPost]
        public Result PostXml(XmlModel inObj)
        {
            var reObj = new Result();
            //表示是有推广人的订阅
            if (inObj.Event.Equals("subscribe") && !string.IsNullOrEmpty(inObj.Ticket) && !string.IsNullOrEmpty(inObj.FromUserName))
            {
                // var saveEnt= new EtcWeixinEntity()
                // {
                //     openid = inObj.FromUserName,
                //     createTime = DateTime.Now,
                //     parentTicket = inObj.Ticket,
                //     eventKey = inObj.EventKey,
                // };
                // return await weixin.save(saveEnt);
            }
            return reObj;
        }

        /// <summary>
        /// 生成JSapi对象
        /// </summary>
        /// <param name="inObj"></param>
        /// <returns></returns>
        [HttpPost]
        public ResultObj<JsApiModel> GetJsApi(DtoKey inObj)
        {
            var reObj = new ResultObj<JsApiModel>();

            var token = Utility.ReadAccessToken(AppConfig.WeiXin.Appid, AppConfig.WeiXin.Secret);

            var jsapiTicket = RedisReadHelper.StringGet("WECHA_JSAPI_TICKET"); ;
            if (string.IsNullOrEmpty(jsapiTicket))
            {
                jsapiTicket = Helper.WeiChat.Utility.GetJsapiTicket(token);
                RedisWriteHelper.SetString("WECHA_JSAPI_TICKET", jsapiTicket, new TimeSpan(2, 0, 0));
            }
            reObj.data = new JsApiModel();
            reObj.data.noncestr = Guid.NewGuid().ToString("n").Substring(10);
            reObj.data.timestamp = TypeChange.DateToInt64().ToString().Substring(0,10);
            reObj.data.url = inObj.Key;
            reObj.data.appid = AppConfig.WeiXin.Appid;

            reObj.data.signature = CheckSignature.GetSignature(new List<string> { "noncestr=" + reObj.data.noncestr, "timestamp=" + reObj.data.timestamp, "url=" + reObj.data.url, "jsapi_ticket="+jsapiTicket },"&");
            return reObj;
        }


        [HttpPost]
        public Result MakeMenu()
        {
            String token = Utility.ReadAccessToken(AppConfig.WeiXin.Appid, AppConfig.WeiXin.Secret);
            MenuModel makeMenu = new MenuModel();
            makeMenu.button = new LinkedList<MenuNodeModel>();
            //makeMenu.button.AddLast(new MenuNodeModel()
            //{
            //    name = "ETC免费办理",
            //    sub_button = new LinkedList<MenuNodeModel>(new[] { 
            //        new MenuNodeModel(){
            //            key=MessageUtil.CLICK_ETC_BIND,
            //            name="绑定申办ETC"
            //        },new MenuNodeModel(){
            //            key=MessageUtil.CLICK_ETC,
            //            name="申办ETC"
            //        },new MenuNodeModel(){
            //            key=MessageUtil.CLICK_ETC_INSTALL,
            //            name="ETC安装激活点"
            //        }
            //    })
            //});

            makeMenu.button.AddLast(new MenuNodeModel()
            {
                name = "ETC安装激活点",
                type= "click",
                key = MessageUtil.CLICK_ETC_INSTALL,
            });
            makeMenu.button.AddLast(new MenuNodeModel()
            {
                name = "推广赚钱",
                type= "click",
                key = MessageUtil.CLICK_GetMoney,
            });
            makeMenu.button.AddLast(new MenuNodeModel()
            {
                name = "获取物流帮手",
                type= "click",
                key = MessageUtil.CLICK_DOWNURL,
            });
            Result result = new Result();
            result.msg = Utility.SetMenu(token, TypeChange.ObjectToStr(makeMenu));
            return result;
        }

        [HttpPost]
        public Result MakeAllTicket()
        {
            var reObj = new Result();
            // var allUser = await staff.getStaffList();
            var token = Utility.ReadAccessToken(AppConfig.WeiXin.Appid, AppConfig.WeiXin.Secret);


            if (!string.IsNullOrEmpty(token))
            {
                // allUser.dataList = allUser.dataList.Where(x => !string.IsNullOrEmpty(x.phone) && string.IsNullOrEmpty(x.ticket)).ToList();
                // foreach (var item in allUser.dataList)
                // {
                //     if (string.IsNullOrEmpty(item.etcNo)) item.etcNo = "87000073";
                //     string postStr = "{\"action_name\": \"QR_LIMIT_STR_SCENE\", \"action_info\": {\"scene\": {\"scene_str\": \"etc_" + item.etcNo + "|" + item.phone + "\"}}}";
                //     item.ticket = Helper.WeiChat.Utility.GetQrCodeTicket(token, postStr);

                //     await staff.updateTicket(item);
                // }
            }
            return reObj;
        }

    }
}