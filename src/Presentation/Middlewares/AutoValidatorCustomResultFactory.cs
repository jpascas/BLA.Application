using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Presentation.ViewModels;

namespace Presentation.Middlewares
{

    public class CustomResultFactory : IFluentValidationAutoValidationResultFactory
    {
        public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails validationProblemDetails)
        {
            return new BadRequestObjectResult(new ValidationErrorResult("Validation errors", validationProblemDetails?.Errors));
        }
    }
}
