using Dev.Challenge.Application.Models;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Challenge.Crosscutting.Migration
{
    public static class UserMigration
    {
        public static void InitializeUsers(IMemoryCache memoryCache)
        {
            var users = new List<User>
        {
            new User { Id = Guid.NewGuid(), Username = "admin", Password = "admin", Role = "ADMIN" },
            new User { Id = Guid.NewGuid(), Username = "entregador", Password = "entregador", Role = "ENTREGADOR" }
        };

            memoryCache.Set("Users", users);
        }
    }

}
