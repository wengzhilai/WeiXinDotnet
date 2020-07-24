using Helper.WeiChat.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Helper.WeiChat
{
    /// <summary>
    /// 微信公众服务器Post过来的加密参数集合（不包括PostData）
    /// <para>如需使用 NeuChar，需要在 MessageHandler 中提供 PostModel 并设置 AppId</para>
    /// </summary>
    public class PostModel : EncryptPostModel
    {
        public string DomainId { get => AppId; set => AppId = value; }

        //以下信息不会出现在微信发过来的信息中，都是微信后台需要设置（获取的）的信息，用于扩展传参使用

        /// <summary>
        /// 当前请求对应的微信 AppId
        /// 如需在 MessageHandler 中使用 NeuChar 进行消息处理，请无比在此处提供 AppId，并对AppId-Secret进行注册，以便自动调用高级接口（如自动发送客服消息）
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 设置服务器内部保密信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="encodingAESKey"></param>
        /// <param name="appId"></param>
        public void SetSecretInfo(string token, string encodingAESKey, string appId)
        {
            Token = token;
            EncodingAESKey = encodingAESKey;
            AppId = appId;
        }
    }
}
