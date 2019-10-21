using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UserManager
{
    public class User
    {
        [Key]
        public string Login { get; set; }
        public string  Name { get; set; }
        public int Password { get; set; }
        public Statuses Status { get; set; }
        public Roles Role { get; set; }
        public DateTime LastFailedLogin { get; set; }

        public enum Roles
        {
            Admin,
            User
        }

        public enum Statuses
        {
            Active,
            Suspended,
            Banned
        }
    }
}
