using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IRepository;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Repository;
using WxProductApi.Config;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json.Serialization;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WxProductApi
{
    public class Startup
    {

        readonly string MyAllowSpecificOrigins = "AllowSameDomain";


        /// <summary>
        /// 读取配置
        /// </summary>
        /// <value></value>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 加载日志资源
        /// </summary>
        /// <value></value>
        public ILoggerRepository repository { get; set; }
        /// <summary>
        /// 环境
        /// </summary>
        /// <value></value>
        public IWebHostEnvironment WebHostEnvironment { get; }


        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment;
            //log4net
            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("Config/log4net.config"));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //把配置选项注入进来
            services.Configure<AppConfig>(Configuration);
            #region 注入
            services.TryAddSingleton<ILoginRepository, LoginRepository>();
            services.TryAddSingleton<IModuleRepository, ModuleRepository>();
            services.TryAddSingleton<IQueryRepository, QueryRepository>();
            services.TryAddSingleton<IRoleRepository, RoleRepository>();
            services.TryAddSingleton<IUserRepository, UserRepository>();
            services.TryAddSingleton<IFileRepository, FileRepository>();
            services.TryAddSingleton<IWeiXinRepository, WeiXinRepository>();
            //获取用户IP
            services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();

            services.AddSingleton<IPsAdminRepository,PsAdminRepository>();
            services.AddSingleton<IPsBatchRepository,PsBatchRepository>();
            #endregion

            #region JWT Config

            //nossa key secreta
            var secretKey = "ZGVtby1hcGktand0";

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            #endregion



            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                    });
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                // 忽略循环引用
                // 不使用驼峰
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                // 设置时间格式
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                // 如字段为null值，该字段不会返回到前端
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            #region  添加SwaggerUI
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "用户文档",
                    Version = "v1",
                    Description = "用于前端和后端调用",
                    Contact = new OpenApiContact
                    {
                        Name = "翁志来",
                        Email = "3188894@qq.com"
                    }
                });

                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearer"
                            }
                        },
                        new string[] { }
                    }
                });

                options.IncludeXmlComments(WebHostEnvironment.ContentRootPath + "/WxProductApi.xml");

            });

            #endregion



            services.AddControllers();
            services.AddHttpClient();
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="_appConfig"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<AppConfig> _appConfig)
        {
            //配置数据库连接
            Helper.DapperHelper.mysqlSettings = _appConfig.Value.MysqlSettings;
            //读取配置
            Global.appConfig=_appConfig.Value;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            #region 使用SwaggerUI

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "ETC接口文档");
                // 访问Swagger的路由后缀
                options.RoutePrefix = "sw";
            });

            #endregion

            app.UseRouting();
            // 跨域必须要 routing后面
            app.UseCors(MyAllowSpecificOrigins);
            //启用验证 必须在cors下面
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
