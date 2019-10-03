using System;
using UserManager;

namespace Interface
{
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new UserManagerFactory().Create(UserManagerFactory.ProviderType.InMemory);

            manager.CreateUser(new User() {Login = "ADMIN", Password = "Qwerty123".GetHashCode(), Name = "Vitalii"});

            Console.WriteLine(manager.CheckUser("ADMIN", "Qwerty123").ToString());
            Console.ReadKey();
        }
    }
}
