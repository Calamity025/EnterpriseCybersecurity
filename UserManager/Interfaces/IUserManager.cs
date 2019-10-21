using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManager.Interfaces
{
    public interface IUserManager
    {
        Task<User> CheckUserAsync(string login, string password);
        Task ChangePasswordAsync(string login, string oldPassword, string newPassword);
        Task<User> CreateUserAsync(User user);
        Task ChangePermissionAsync(string targetLogin, User.Roles role);
        Task ChangeStatus(string login, User.Statuses status);
        Task Suspend(User user);
        Task<IEnumerable<User>> GetUsers();
        Task SeedAsync(IEnumerable<User> users);
    }
}