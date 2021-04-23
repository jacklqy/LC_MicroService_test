using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Ocelot.Cache;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using Ocelot.Provider.Polly;
using Zhaoxi.MicroService.GatewayDemo.Utility;

namespace Zhaoxi.MicroService.GatewayDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Ids4
            var authenticationProviderKey = "UserGatewayKey";
            services.AddAuthentication("Bearer")
               .AddIdentityServerAuthentication(authenticationProviderKey, options =>
               {
                   //options.Authority = "http://localhost:7200";
                   options.Authority = "http://47.92.132.209:7200";
                   options.ApiName = "UserApi";
                   options.RequireHttpsMetadata = false;
                   options.SupportedTokens = SupportedTokens.Both;
               });
            #endregion

            //services.AddControllers();
            services.AddOcelot()//Ocelot如何处理
                .AddConsul()//支持Consul
                .AddCacheManager(x =>
                {
                    x.WithDictionaryHandle();//默认字典存储
                })
                .AddPolly()
                ;

            services.AddSingleton<IOcelotCache<CachedResponse>, CustomCache>();//自定义缓存--Redis分布式缓存
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseOcelot();//请求交给Ocelot处理

            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseHttpsRedirection();

            //app.UseRouting();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
