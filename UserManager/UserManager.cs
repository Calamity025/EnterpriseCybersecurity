using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UserManager
{
    class UserManager : IUserManager
    {
        private readonly IProvider _provider;
        public UserManager(IProvider provider) =>
            _provider = provider;

        public User CheckUser(string login, string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException();
            }
            var user = _provider.Read(x => x.Login.Equals(login));
            if (user == null)
            {
                throw new KeyNotFoundException("No user with such login");
            }

            if (user.Password != password.GetHashCode())
            {
                throw new ArgumentException("Password is incorrect");
            }

            return user;
        }

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
                throw new ArgumentException("Password is incorrect");
            }

            return user;
        }

        public void ChangePassword(string login, string oldPassword, string newPassword)
        {
            if (oldPassword == null || newPassword == null)
            {
                throw new ArgumentNullException();
            }
            var user = CheckUser(login, oldPassword);
            if (oldPassword.GetHashCode() == newPassword.GetHashCode())
            {
                throw new ArgumentException("New password cannot be the same as the old one");
            }
            user.Password = newPassword.GetHashCode();
            _provider.CreateOrUpdate(user);
        }

        public async Task ChangePasswordAsync(string login, string oldPassword, string newPassword)
        {
            if (oldPassword == null || newPassword == null)
            {
                throw new ArgumentNullException();
            }
            var user = await CheckUserAsync(login, oldPassword);
            if (oldPassword.GetHashCode() == newPassword.GetHashCode())
            {
                throw new ArgumentException("New password cannot be the same as the old one");
            }
            user.Password = newPassword.GetHashCode();
            await _provider.CreateOrUpdateAsync(user);
        }

        public User CreateUser(User user)
        {
            var tryGetUser = _provider.Read(x => x.Login.Equals(user.Login));
            if (tryGetUser != null)
            {
                throw new ArgumentException("User with such login already exists");
            }
            user.Role = User.Roles.User;

            return _provider.CreateOrUpdate(user);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var tryGetUser = await _provider.ReadAsync(x => x.Login.Equals(user.Login));
            if (tryGetUser != null)
            {
                throw new ArgumentException("User with such login already exists");
            }
            user.Role = User.Roles.User;

            return await _provider.CreateOrUpdateAsync(user);
        }

        public void ChangePermission(string issuerLogin, string issuerPassword, string targetLogin, User.Roles role)
        {
            var admin = CheckUser(issuerLogin, issuerPassword);
            if (!admin.Role.Equals(User.Roles.Admin))
            {
                throw new ArgumentException("Not enough permissions");
            }
            var user = _provider.Read(x => x.Login.Equals(targetLogin));
            if (user == null)
            {
                throw new KeyNotFoundException("No user with such login");
            }
            if (user.Role == role)
            {
                return;
            }

            user.Role = role;
            _provider.CreateOrUpdate(user);
        }

        public async Task ChangePermissionAsync(string issuerLogin, string issuerPassword, string targetLogin, User.Roles role)
        {
            var admin = await CheckUserAsync(issuerLogin, issuerPassword);
            if (!admin.Role.Equals(User.Roles.Admin))
            {
                throw new ArgumentException("Not enough permissions");
            }
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

        public void DeleteUser(string issuerLogin, string issuerPassword, string targetLogin)
        {
            var admin = CheckUser(issuerLogin, issuerPassword);
            if (!admin.Role.Equals(User.Roles.Admin))
            {
                throw new ArgumentException("Not enough permissions");
            }
            var user = _provider.Read(x => x.Login.Equals(targetLogin));
            if (user == null)
            {
                throw new KeyNotFoundException("No user with such login");
            }
            _provider.Delete(user);
        }

        public async Task DeleteUserAsync(string issuerLogin, string issuerPassword, string targetLogin)
        {
            var admin = await CheckUserAsync(issuerLogin, issuerPassword);
            if (!admin.Role.Equals(User.Roles.Admin))
            {
                throw new ArgumentException("Not enough permissions");
            }
            var user = await _provider.ReadAsync(x => x.Login.Equals(targetLogin));
            if (user == null)
            {
                throw new KeyNotFoundException("No user with such login");
            }
            await _provider.DeleteAsync(user);
        }

        public void Seed(IEnumerable<User> users)
        {
            foreach(var user in users)
            {
                _provider.CreateOrUpdate(user);
            }
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
