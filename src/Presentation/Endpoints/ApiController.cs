using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Presentation.ViewModels;

namespace Presentation.Endpoints
{
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        public ApiController() { }

        protected ObjectResult ToFailureResult<T>(OperationResult<T> operationResult, int defaultCode = 500)
        {
            ErrorResult result = new ErrorResult();
            result.Messages.AddRange(operationResult.FailureMessages);
            return StatusCode(operationResult.FailureCode ?? defaultCode, result);
        }
        protected long GetCurrentUserId()
        {
            return  Convert.ToInt64(this.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
    }
}
