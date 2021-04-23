using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Zhaoxi.MicroService.ClientDemo.Models;
using Zhaoxi.MicroService.Interface;
using Zhaoxi.MicroService.Model;

namespace Zhaoxi.MicroService.ClientDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _iUserService = null;
        public HomeController(ILogger<HomeController> logger, IUserService userService)
        {
            _logger = logger;
            this._iUserService = userService;
        }

        #region MyRegion
        public IActionResult Index1()
        {
            #region 单体架构
            //base.ViewBag.Users = this._iUserService.UserAll();//三层架构
            #endregion

            #region 分布式架构
            //把数据的获取换成调用服务
            //代码的一小步，架构的一大步---从单体步入分布式的时代
            string url = null;
            //url = "http://47.95.2.2:5726/api/users/all";
            //url = "http://47.95.2.2:5727/api/users/all";
            //url = "http://47.95.2.2:5728/api/users/all";

            //怎么写代码？三个地址怎么维护？多启动一个就来改代码？
            #region Nginx
            url = "http://47.95.2.2:8086/api/users/all";
            #endregion

            #region Consul
            //url = "http://ZhaoxiService/api/users/all";

            //ConsulClient client = new ConsulClient(c =>
            //{
            //    c.Address = new Uri("http://47.95.2.2:11500/");//consul地址
            //    c.Datacenter = "dc1";
            //});

            //var res = client.Agent.Checks().Result;
            //var res1 = client.Agent.Self().Result;
            //var resul = client.Agent.Services().Result;
            //var response = client.Agent.Services().Result.Response;
            //foreach (var item in response)
            //{
            //    Console.WriteLine("***************************************");
            //    Console.WriteLine(item.Key);
            //    var service = item.Value;
            //    Console.WriteLine($"{service.Address}--{service.Port}--{service.Service}");
            //    Console.WriteLine("***************************************");
            //}

            //Uri uri = new Uri(url);
            //string groupName = uri.Host;
            //AgentService agentService = null;

            //var serviceDictionary = response.Where(s => s.Value.Service.Equals(groupName, StringComparison.OrdinalIgnoreCase)).ToArray();
            //{
            //    agentService = serviceDictionary[0].Value;//写死
            //}
            //{
            //    //long index = iTotalCount++ % serviceDictionary.Length;
            //    //agentService = serviceDictionary[index].Value;
            //    //轮询
            //}
            //{
            //    //long index = new Random(DateTime.Now.Millisecond).Next(0, 999999) % serviceDictionary.Length;
            //    //agentService = serviceDictionary[index].Value;
            //    //随机--平均
            //}
            //{
            //    //权重：不同的端口号 任务的比重不同--你得告诉我
            //    //serviceDictionary[0].Value.Tags[0]  1/2/3/4
            //    //ip port 权重值   分配时按照权重来分配
            //    //大家找小助教获取下代码，自己动手试试
            //}
            ////超时自动切换---重试---Polly
            //url = $"{uri.Scheme}://{agentService.Address}:{agentService.Port}{uri.PathAndQuery}";
            #endregion

            string content = InvokeApi(url);
            base.ViewBag.Users = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<User>>(content);
            Console.WriteLine($"This is {url} Invoke");
            #endregion
            return View();
        }
        #endregion

        public IActionResult Index()
        {
            #region 单体架构
            //base.ViewBag.Users = this._iUserService.UserAll();//三层架构
            #endregion

            #region 分布式架构
            string url = null;
            //url = "http://localhost:5726/api/users/all";
            //url = "http://47.95.2.2:5726/api/users/all";
            //url = "http://47.95.2.2:5727/api/users/all";
            //url = "http://47.95.2.2:5728/api/users/all";

            //url = "http://47.95.2.2:8086/api/users/all";

            //url = "http://ZhaoxiService/api/users/all";

            url = "http://localhost:6299/T/users/all";

            //ConsulClient client = new ConsulClient(c =>
            //{
            //    c.Address = new Uri("http://47.95.2.2:11500/");//consul地址
            //    c.Datacenter = "dc1";
            //});
            //var response = client.Agent.Services().Result.Response;
            //foreach (var item in response)
            //{
            //    Console.WriteLine("***************************************");
            //    Console.WriteLine(item.Key);
            //    var service = item.Value;
            //    Console.WriteLine($"{service.Address}--{service.Port}--{service.Service}");
            //    Console.WriteLine("***************************************");
            //}

            //Uri uri = new Uri(url);
            //string groupName = uri.Host;
            //AgentService agentService = null;

            //var serviceDictionary = response.Where(s => s.Value.Service.Equals(groupName, StringComparison.OrdinalIgnoreCase)).ToArray();//获取的全部服务实例信息 5726/5727/5728
            //{
            //    agentService = serviceDictionary[0].Value;//写死--死心眼，怼一个
            //}
            ////{
            ////    //雨露均沾--轮询策略
            ////    agentService = serviceDictionary[iTotalCount++ % serviceDictionary.Length].Value;
            ////}
            ////{
            ////    //看RP--随机策略
            ////    agentService = serviceDictionary[new Random().Next(0, 1000) % serviceDictionary.Length].Value;
            ////}
            //{
            //    //权重策略--不同的服务器处理能力不同，按能力分配
            //    //serviceDictionary[0].Value.Tags//获取权重1
            //    //大家找小助教获取下代码，自己动手试试
            //}

            //url = $"{uri.Scheme}://{agentService.Address}:{agentService.Port}{uri.PathAndQuery}";

            string content = InvokeApi(url);
            base.ViewBag.Users = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<User>>(content);
            Console.WriteLine($"This is {url} Invoke");
            #endregion

            return View();
        }

        /// <summary>
        /// 自增变量--不考虑线程安全
        /// </summary>
        private static long iTotalCount = 0;

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static string InvokeApi(string url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Get;
                message.RequestUri = new Uri(url);
                var result = httpClient.SendAsync(message).Result;
                string content = result.Content.ReadAsStringAsync().Result;
                return content;
            }
        }
    }
}
