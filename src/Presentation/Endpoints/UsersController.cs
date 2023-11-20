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
using Application.Abstractions;

namespace Presentation.Endpoints
{
    [Route("api/[controller]")]
    public class UsersController : ApiController
    {
        private readonly IMapper mapper;
        private readonly ICommandBus commandBus;        

        public UsersController(ICommandBus commandBus, IMapper mapper)
        {
            this.commandBus = commandBus;                        
            this.mapper = mapper;
        }

        [HttpPost("")]
        [FluentValidationAutoValidationAttribute]
        public async Task<ActionResult> Create([FromBody] CreateUserRequestModel createRequestModel)
        {            
            var command = new CreateUserCommand(createRequestModel.Email, createRequestModel.Password);

            var userResult = await this.commandBus.Send<CreateUserCommand, User>(command);

            if (userResult.Success)
            {
                UserResultModel userModel = this.mapper.Map<User, UserResultModel>(userResult.Result);
                return Ok(userModel);
            }
            else
            {                
                return ToFailureResult(userResult);
            }
        }

        [HttpPost("login")]
        [FluentValidationAutoValidationAttribute]
        public async Task<ActionResult> Login([FromBody] LoginUserRequestModel loginRequestModel)
        {
            var command = new LoginUserCommand(loginRequestModel.Email, loginRequestModel.Password);

            var tokenResult = await this.commandBus.Send<LoginUserCommand, string>(command);

            if (tokenResult.Success)
            {                
                return Ok(tokenResult.Result);
            }
            else
            {
                return ToFailureResult(tokenResult);
            }
        }      
    }
}
