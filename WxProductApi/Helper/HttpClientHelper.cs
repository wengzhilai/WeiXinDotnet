using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Helper
{
    /// <summary>
    /// 
    /// </summary>
    public static class HttpClientHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async static Task<T> Get<T>(this HttpClient httpClient, string url) where T : new()
        {
            T result=new T();
            using (var response = await httpClient.GetAsync(url))
            {
                if (response.IsSuccessStatusCode)
                {
                    using (Stream stream = await response.Content.ReadAsStreamAsync())
                    {
                        using (StreamReader streamReader = new StreamReader(stream, Encoding.UTF8))
                        {
                           string reStr = await streamReader.ReadToEndAsync();
                           result=JsonConvert.DeserializeObject<T>(reStr);
                        }
                    }

                }
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async static Task<byte[]> MyGetFile(this HttpClient httpClient, string url)
        {
            byte[] result = null;
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url))
            {
                using (var response = await httpClient.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        result = await response.Content.ReadAsByteArrayAsync();
                    }
                }
            }
            return result ?? new byte[0];
        }
        /// <summary>
        /// 获取IP详细地址
        /// </summary>
        /// <param name="httpClient"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static async Task<List<string>> GetAddressAsync(this HttpClient httpClient,string ip)
        {
            JObject jo =await httpClient.Get<JObject>($"http://api.map.baidu.com/location/ip?ak=rg3c2fj4QBZwa6v3h1w95Sp9&ip={ip}");
            if (jo["status"].ToString() == "0")
            {
                return new List<string> { jo["content"]["address_detail"]["province"].ToString(), jo["content"]["address_detail"]["city"].ToString() };
            }
            else
            {
                return new List<string> { "未知", "未知" };
            }
        }

    }
}