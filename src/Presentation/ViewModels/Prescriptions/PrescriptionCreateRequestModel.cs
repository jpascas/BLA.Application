using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Presentation.ViewModels.Prescriptions
{
    public class PrescriptionCreateRequestModel
    {
        [JsonPropertyName("drug")]
        public string Drug { get; set; }
        [JsonPropertyName("dosage")]
        public string Dosage { get; set; }
        [JsonPropertyName("notes")]
        public string Notes { get; set; }
    }
}
