using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Zhaoxi.MicroService.Interface;
using Zhaoxi.MicroService.Model;

namespace Zhaoxi.MicroService.ServiceInstance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        private IConfiguration _iConfiguration;

        public HealthController(IConfiguration configuration)
        {
            this._iConfiguration = configuration;
        }

        [HttpGet]
        [Route("Index")]
        public IActionResult Index()
        {
            Console.WriteLine($"This is HealthController  {this._iConfiguration["port"]} Invoke");
            
            return Ok();//只是个200 
        }

    }
}