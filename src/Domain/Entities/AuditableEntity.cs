using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public abstract class AuditableEntity
    {
        public Int16 Status { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
