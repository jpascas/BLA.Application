using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace Presentation.ViewModels
{    
    public class PrescriptionResultModel
    {
        public Guid Id { get; set; }        
        public string Drug { get; set; }
        public string Dosage { get; set; }
        public string Notes { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
