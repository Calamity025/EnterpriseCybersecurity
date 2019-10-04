using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using UserManager;
using UserManager.Interfaces;

namespace Frontpage
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateWebHostBuilder(args).Build();

            using(var scope = builder.Services.CreateScope()){
                Seed(scope.ServiceProvider.GetRequiredService<IUserManager>());
            }

            builder.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void Seed(IUserManager manager)
        {
            var users = new List<User>
            {
                new User { Login = "ADMIN", Password = "Qwerty123".GetHashCode(), Name = "Vitalii", Role = User.Roles.Admin },
                new User { Login = "Calamity", Password = "AnotherQwerty".GetHashCode(), Name = "Another Vitalii", Role = User.Roles.User }
            };

            manager.Seed(users);
        }
    }
}
