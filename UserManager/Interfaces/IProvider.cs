﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Interfaces
{
    internal interface IProvider
    {
        IEnumerable<User> GetUsers();
        User Read(Func<User, bool> predicate);
        Task<User> ReadAsync(Func<User, bool> predicate);
        User CreateOrUpdate(User user);
        Task<User> CreateOrUpdateAsync(User user);
        void Delete(User user);
        Task DeleteAsync(User user);
    }
}
