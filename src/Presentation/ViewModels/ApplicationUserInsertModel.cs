using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace Presentation.ViewModels
{    
    public class ApplicationUserIResultModel
    {        
        [JsonPropertyName("email")]
        public string Email { get; set; }        
    }
}
