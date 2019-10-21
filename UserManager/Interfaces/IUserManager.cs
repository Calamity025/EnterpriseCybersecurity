using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManager.Interfaces
{
    public interface IUserManager
    {
        User CheckUser(string login, string password);
        Task<User> CheckUserAsync(string login, string password);
        void ChangePassword(string login, string oldPassword, string newPassword);
        Task ChangePasswordAsync(string login, string oldPassword, string newPassword);
        User CreateUser(User user);
        Task<User> CreateUserAsync(User user);
        void ChangePermission(string issuerLogin, string issuerPassword, string targetLogin, User.Roles role);
        Task ChangePermissionAsync(string issuerLogin, string issuerPassword, string targetLogin, User.Roles role);
        void DeleteUser(string issuerLogin, string issuerPassword, string targetLogin);
        Task DeleteUserAsync(string issuerLogin, string issuerPassword, string targetLogin);
        Task ChangeStatus(string login, User.Statuses status);
        Task Suspend(string login);
        void Seed(IEnumerable<User> users);
        Task SeedAsync(IEnumerable<User> users);
    }
}