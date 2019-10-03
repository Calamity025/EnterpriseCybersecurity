using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserManager
{
    internal class DatabaseProvider : IProvider
    {
        public User CreateOrUpdate(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> CreateOrUpdateAsync(User user)
        {
            throw new NotImplementedException();
        }

        public void Delete(User user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(User user)
        {
            throw new NotImplementedException();
        }

        public User Read(Func<User, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<User> ReadAsync(Func<User, bool> predicate)
        {
            throw new NotImplementedException();
        }
    }
}
