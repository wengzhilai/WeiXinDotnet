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


            //这个例子只是修改一下response的header
            context.Response.OnStarting(state => {
                var httpContext = (HttpContext)state;
                httpContext.Response.Headers.Add("test2", "testvalue2");
                return Task.FromResult(0);
            }, context);
            //处理结束转其它中间组件去处理


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