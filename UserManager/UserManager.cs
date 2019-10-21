using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UserManager.Interfaces;

namespace UserManager
{
    class UserManager : IUserManager
    {
        private readonly IProvider _provider;
        public UserManager(IProvider provider) =>
            _provider = provider;

        public async Task<User> CheckUserAsync(string login, string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException();
            }
            var user = await _provider.ReadAsync(x => x.Login.Equals(login));
            if (user == null)
            {
                throw new KeyNotFoundException("No user with such login");
            }
            if (user.Password != password.GetHashCode())
            {
                if (user.FailedLoginCount >= 3 && user.Role != User.Roles.Admin)
                {
                    await Suspend(user);
                    throw new ArgumentException(
                        "Too many failed login attempts. Account is suspended. Contact admins if you think this is a mistake");
                }
                user.FailedLoginCount = user.FailedLoginCount+1;
                await _provider.CreateOrUpdateAsync(user);
                throw new ArgumentException("Password is incorrect");
            }

            return user;
        }

        public async Task ChangePasswordAsync(string login, string oldPassword, string newPassword)
        {
            if (oldPassword == null || newPassword == null)
            {
                throw new ArgumentNullException();
            }
            var user = await CheckUserAsync(login, oldPassword);
            if (user.Password != oldPassword.GetHashCode())
            {
                throw new ArgumentException("Old password is incorrect");
            }
            if (oldPassword.GetHashCode() == newPassword.GetHashCode())
            {
                throw new ArgumentException("New password cannot be the same as the old one");
            }
            user.Password = newPassword.GetHashCode();
            await _provider.CreateOrUpdateAsync(user);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var tryGetUser = await _provider.ReadAsync(x => x.Login.Equals(user.Login));
            if (tryGetUser != null)
            {
                throw new ArgumentException("User with such login already exists");
            }
            user.Role = User.Roles.User;
            user.Status = User.Statuses.Active;
            return await _provider.CreateOrUpdateAsync(user);
        }

        public async Task ChangePermissionAsync(string targetLogin, User.Roles role)
        {
            var user = await _provider.ReadAsync(x => x.Login.Equals(targetLogin));
            if (user == null)
            {
                throw new KeyNotFoundException("No user with such login");
            }
            if (user.Role == role)
            {
                return;
            }
            user.Role = role;
            await _provider.CreateOrUpdateAsync(user);
        }


        public async Task ChangeStatus(string login, User.Statuses status)
        {
            var user = await _provider.ReadAsync(x => x.Login.Equals(login));
            if (user == null)
            {
                throw new KeyNotFoundException("No user with such login");
            }
            if (user.Role == User.Roles.Admin)
            {
                return;
            }
            user.Status = status;
            await _provider.CreateOrUpdateAsync(user);
        }

        public async Task Suspend(User user)
        {
            if (user.Role == User.Roles.Admin)
            {
                return;
            }
            user.Status = User.Statuses.Suspended;
            await _provider.CreateOrUpdateAsync(user);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await Task.Run(() => _provider.GetUsers());
        }

        public async Task SeedAsync(IEnumerable<User> users)
        {
            foreach (var user in users)
            {
                await _provider.CreateOrUpdateAsync(user);
            }
        }
    }
}
