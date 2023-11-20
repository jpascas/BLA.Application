using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public abstract class AuditableEntity
    {        
        public long CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
