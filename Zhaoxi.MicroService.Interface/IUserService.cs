using System;
using System.Collections.Generic;
using Zhaoxi.MicroService.Model;

namespace Zhaoxi.MicroService.Interface
{
    public interface IUserService
    {
        User FindUser(int id);

        IEnumerable<User>  UserAll();

    }
}
