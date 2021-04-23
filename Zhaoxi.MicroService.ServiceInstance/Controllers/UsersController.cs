using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _iUserService = null;
        private IConfiguration _iConfiguration;

        public UsersController(ILogger<UsersController> logger, IUserService userService, IConfiguration configuration)
        {
            _logger = logger;
            this._iUserService = userService;
            this._iConfiguration = configuration;
        }
        [HttpGet]
        [Route("Get")]
        public User Get(int id)
        {
            return this._iUserService.FindUser(id);
        }

        [HttpGet]
        [Route("All")]
        //[Authorize]//需要授权
        public IEnumerable<User> Get()
        {
            Console.WriteLine($"This is UsersController {this._iConfiguration["port"]} Invoke");

            return this._iUserService.UserAll().Select(u => new User()
            {
                Id = u.Id,
                Account = u.Account,
                Name = u.Name,
                Role = $"{ this._iConfiguration["ip"]}{ this._iConfiguration["port"]}",
                Email = u.Email,
                LoginTime = u.LoginTime,
                Password = u.Password
            });
        }

        [HttpGet]
        [Route("Timeout")]
        public IEnumerable<User> Timeout()
        {
            Thread.Sleep(5000);//休息5000ms
            Console.WriteLine($"This is UsersController {this._iConfiguration["port"]} Invoke Timeout");
            return this._iUserService.UserAll().Select(u => new User()
            {
                Id = u.Id,
                Account = u.Account,
                Name = u.Name,
                Role = $"{ this._iConfiguration["ip"]}:{ this._iConfiguration["port"]}",
                Email = u.Email,
                LoginTime = u.LoginTime,
                Password = u.Password
            });
        }

        [HttpGet]
        [Route("Exception")]
        public IEnumerable<User> Exception()
        {
            Console.WriteLine($"This is UsersController {this._iConfiguration["port"]} Invoke Timeout");
            throw new Exception("UsersController Custom Exception");
        }
    }
}