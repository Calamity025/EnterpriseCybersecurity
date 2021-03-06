﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Interfaces;

namespace UserManager.Providers
{
    internal class InMemoryProvider : IProvider
    {
        private readonly Dictionary<string, User> _users = new Dictionary<string, User>();
        private readonly object _locker = new object();

        public IEnumerable<User> GetUsers()
        {
            return _users.Values.ToList();
        }

        public User CreateOrUpdate(User user)
        {
            lock (_locker)
            {
                if (_users.ContainsKey(user.Login))
                {
                    _users[user.Login] = user;

                }
                else
                {
                    _users.Add(user.Login, user);
                }
            }

            return user;
        }

        public Task<User> CreateOrUpdateAsync(User user)
        {
            return Task.Run(() => CreateOrUpdate(user));
        }

        public void Delete(User user)
        {
            lock (_locker)
            {
                _users.Remove(user.Login);
            }
        }

        public Task DeleteAsync(User user)
        {
            return Task.Run(() => Delete(user));
        }

        public User Read(Func<User, bool> predicate)
        {
            lock (_locker)
            {
                foreach (var user in _users.Values)
                {
                    if (predicate(user))
                    {
                        return user;
                    }
                }
            }

            return null;
        }

        public Task<User> ReadAsync(Func<User, bool> predicate)
        {
            return Task.Run(() => Read(predicate));
        }
    }
}
