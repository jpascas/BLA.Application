using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.ViewModels
{
    public class ErrorResult
    {
        public ErrorResult()
        {            
        }
        public List<string> Messages { get; set; } = new List<string>();
    }
}
