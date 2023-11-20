using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Prescription : AuditableEntity
    {
        public Guid Id { get; set; }                
        public long UserId { get; set; }

        public string Drug { get; set; }
        public string Dosage { get; set; }
        public string Notes { get; set; }
    }
}
