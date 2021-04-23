using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Zhaoxi.MicroService.Interface;
using Zhaoxi.MicroService.Service;
using Zhaoxi.MicroService.ServiceInstance.Utility;

namespace Zhaoxi.MicroService.ServiceInstance
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
            services.AddControllers();
            services.AddTransient<IUserService, UserService>();

            //services.AddAuthentication("Cookie");

            //services.AddAuthentication("Bearer")//Scheme������ָ������Ϣ�ķ�ʽBearer- 
            //    .AddIdentityServerAuthentication(options =>
            //    {
            //        options.Authority = "http://localhost:7200";//ids4�ĵ�ַ--ר�Ż�ȡ��Կ
            //        options.ApiName = "UserApi";
            //        options.RequireHttpsMetadata = false;
            //    });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthentication();//��Ȩ--�����Ϣ����֤��û���û�
            //app.UseAuthorization();//��Ȩ�������û��Ȩ��

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            #region Consulע�� 
            //վ���������--ִ����ִֻ��һ��
            this.Configuration.ConsulRegist();
            #endregion
        }
    }
}
