using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Presentation.ViewModels
{
    public class ErrorResult
    {
        public ErrorResult()
        {            
        }
        [JsonPropertyName("messages")]
        public List<string> Messages { get; set; } = new List<string>();
    }
}
