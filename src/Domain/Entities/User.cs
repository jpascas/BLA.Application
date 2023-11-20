using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class User
    {
        public long Id { get; set; }                
        public string Email { get; set; }

        public string PasswordHash { get; set; }
    }
}
