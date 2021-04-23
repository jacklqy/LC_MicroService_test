using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Zhaoxi.MicroService.AuthenticationCenter
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

            //�ͻ���ģʽ--��ôִ��Ids4
            services.AddIdentityServer()//��ô����
              .AddDeveloperSigningCredential()//��ʱ���ɵ�֤��--��ʱ���ɵ�
              .AddInMemoryClients(ClientInitConfig.GetClients())//InMemory �ڴ�ģʽ
              .AddInMemoryApiResources(ClientInitConfig.GetApiResources());//�ܷ���ɶ��Դ
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();//ʹ��Ids4 ����ᴫ��Ids4���м��

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
    /// <summary>
    /// �ͻ���ģʽ
    /// </summary>
    public class ClientInitConfig
    {
        /// <summary>
        /// ����ApiResource   
        /// �������Դ��Resources��ָ�ľ��ǹ����API
        /// </summary>
        /// <returns>���ApiResource</returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource("UserApi", "�û���ȡAPI")
            };
        }

        /// <summary>
        /// ������֤������Client
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                new Client
                {
                    ClientId = "AspNetCore31.AuthDemo",//�ͻ���Ωһ��ʶ
                    ClientSecrets = new [] { new Secret("jack123456".Sha256()) },//�ͻ������룬�����˼���
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    //��Ȩ��ʽ���ͻ�����֤��ֻҪClientId+ClientSecrets
                    AllowedScopes = new [] { "UserApi" },//������ʵ���Դ
                    Claims=new List<Claim>(){
                        new Claim(IdentityModel.JwtClaimTypes.Role,"Admin"),
                        new Claim(IdentityModel.JwtClaimTypes.NickName,"̽lu��"),
                        new Claim("eMail","529987528@qq.com")
                    }
                }
            };
        }
    }

}
