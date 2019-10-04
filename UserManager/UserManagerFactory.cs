using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

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
                    provider = new DatabaseProvider(new DbContextOptionsBuilder<DatabaseProvider>().UseSqlServer(connectionString).Options);
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
