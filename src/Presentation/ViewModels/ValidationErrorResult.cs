using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Presentation.ViewModels
{
    public class ValidationErrorResult
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
        [JsonPropertyName("validationErrors")]
        public IDictionary<string, string[]> ValidationErrors { get; set; } = new Dictionary<string, string[]>();
        public ValidationErrorResult()
        {
        }
        public ValidationErrorResult(string title, IDictionary<string, string[]> validationErrors)
        {
            this.Title = title;
            this.ValidationErrors = validationErrors;
        }
    }
}
