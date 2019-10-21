using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManager.Interfaces;

namespace UserManager.Providers
{
    internal class DatabaseProvider : DbContext, IProvider
    {
        public DatabaseProvider(DbContextOptions options) : base(options) =>
            Database.EnsureCreated();

        public DbSet<User> Users { get; set; }

        public IEnumerable<User> GetUsers()
        {
            return Users;
        } 

        public User CreateOrUpdate(User user)
        {
            var tryGetUser = Users.Find(user.Login);
            if(tryGetUser == null)
            {
                Users.Add(user);
                tryGetUser = user;
            }
            else
            {
                tryGetUser.Password = user.Password;
                tryGetUser.Name = user.Name;
                tryGetUser.Role = tryGetUser.Role;
                Users.Update(tryGetUser);
            }

            this.SaveChanges();
            return tryGetUser;
        }

        public async Task<User> CreateOrUpdateAsync(User user)
        {
            var tryGetUser = await Users.FindAsync(user.Login);
            if (tryGetUser == null)
            {
                Users.Add(user);
                tryGetUser = user;
            }
            else
            {
                tryGetUser.Password = user.Password;
                tryGetUser.Name = user.Name;
                tryGetUser.Role = user.Role;
                tryGetUser.Status = user.Status;
                tryGetUser.Password = user.Password;
                tryGetUser.FailedLoginCount = user.FailedLoginCount;
                Users.Update(tryGetUser);
            }

            await this.SaveChangesAsync();
            return tryGetUser;
        }

        public void Delete(User user)
        {
            Users.Remove(user);
            this.SaveChanges();
        }

        public async Task DeleteAsync(User user)
        {
            Users.Remove(user);
            await this.SaveChangesAsync();
        }

        public User Read(Func<User, bool> predicate)
        {
            return Users.AsNoTracking().FirstOrDefault(x => predicate(x));
        }

        public async Task<User> ReadAsync(Func<User, bool> predicate)
        {
            return await Users.AsNoTracking().FirstOrDefaultAsync(x => predicate(x));
        }
    }
}
