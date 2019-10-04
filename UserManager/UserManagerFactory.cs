using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UserManager.Interfaces;
using UserManager.Providers;

namespace UserManager
{
    public class UserManagerFactory
    {
        public enum ProviderType 
        {
            Database,
            InMemory
        }

        public IUserManager Create(ProviderType type, string connectionString = null)
        {
            IProvider provider;

            switch (type)
            {
                case ProviderType.Database:
                    provider = new DatabaseProvider(new DbContextOptionsBuilder<DatabaseProvider>().UseSqlServer(connectionString).EnableSensitiveDataLogging().Options);
                    break;
                case ProviderType.InMemory:
                    provider = new InMemoryProvider();
                    break;
                default:
                    throw new ArgumentException("no such provider type supported");
            }

            return new UserManager(provider);
        }
    }
}
