using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Domain.Entities;
using Application;
using Presentation.ViewModels;
using Application.Commands;
using Application.Queries;
using System.Reflection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Attributes;

namespace Presentation.Endpoints
{
    [Route("api/[controller]")]
    public class UsersController : ApiController
    {
        private readonly IMapper mapper;
        private readonly ICommandBus commandBus;
        private readonly IUserQueryService userQueryService;

        public UsersController(ICommandBus commandBus, IUserQueryService userQueryService, IMapper mapper)
        {
            this.commandBus = commandBus;            
            this.userQueryService = userQueryService;
            this.mapper = mapper;
        }

        [HttpPost("")]
        [FluentValidationAutoValidationAttribute]
        public async Task<ActionResult> Create([FromBody] ApplicationUserInsertModel userModel)
        {            
            var command = new CreateUserCommand(userModel.Email, userModel.Password);

            await this.commandBus.Send(command);

            if (command.IsCommandValid())
            {
                User user = await this.userQueryService.FindByEmail(userModel.Email);
                ApplicationUserIResultModel applicationUser = this.mapper.Map<User, ApplicationUserIResultModel>(user);
                return Ok(applicationUser);
            }
            else
            {                
                return StatusCode(500, ConvertCommandResultToErrorResult(command));
            }
        }

        private ErrorResult ConvertCommandResultToErrorResult(BaseCommand command)
        {
            ErrorResult result = new ErrorResult();
            command.Errors.ForEach(e => result.Messages.Add(e.Message));
            return result;
        }
    }
}
