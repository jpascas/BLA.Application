using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class User : AuditableEntity
    {
        public long Id { get; set; }                
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
