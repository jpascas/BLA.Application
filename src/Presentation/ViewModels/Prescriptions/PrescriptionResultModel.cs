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
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("drug")]
        public string Drug { get; set; }
        [JsonPropertyName("dosage")]
        public string Dosage { get; set; }
        [JsonPropertyName("notes")]
        public string Notes { get; set; }
        [JsonPropertyName("createdby")]
        public long CreatedBy { get; set; }
        [JsonPropertyName("createdat")]
        public DateTime CreatedAt { get; set; }
        [JsonPropertyName("modifiedby")]
        public long ModifiedBy { get; set; }
        [JsonPropertyName("modifiedat")]
        public DateTime ModifiedAt { get; set; }
    }
}
