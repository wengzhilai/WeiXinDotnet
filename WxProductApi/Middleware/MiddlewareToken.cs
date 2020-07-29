using System.Threading.Tasks;
using Helper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Models;

namespace WxProductApi
{
    /// <summary>
    /// token验证中间件
    /// </summary>
    public class MiddlewareToken{
        private readonly RequestDelegate _next;
        public MiddlewareToken(RequestDelegate next){
            _next=next;
        }

        public async Task InvokeAsync(HttpContext context){
            if(context.Request.Headers.ContainsKey("Authorization")){
                var token=context.Request.Headers["Authorization"];
                if(token.Count>0){
                    string tokenStr=token[0].Replace("Bearer ","");
                    // Result reEnt = new Result(){
                    //     success=false,
                    //     msg="token无效"
                    // };
                    // await context.Response.WriteAsync(TypeChange.ObjectToStr(reEnt));
                    // return;
                }
            }
            await _next(context);
        }

    }
}