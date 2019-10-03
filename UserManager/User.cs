﻿using System;
using System.Collections.Generic;
using System.Text;

namespace UserManager
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string  Name { get; set; }
        public int Password { get; set; }
        public Roles Role { get; set; }

        public enum Roles
        {
            Admin,
            User
        }

        public override string ToString()
        {
            return $"Welcome back!\r\nLogin: {Login}\r\nName: {Name}\r\nRole: {Role.ToString()}";
        }
    }
}
