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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Presentation.Endpoints
{
    [Authorize]
    [Route("api/[controller]")]
    public class PrescriptionsController : ApiController
    {
        private readonly IMapper mapper;
        private readonly ICommandBus commandBus;
        private readonly IUserQueryService userQueryService;

        public PrescriptionsController(ICommandBus commandBus, IMapper mapper)
        {
            this.commandBus = commandBus;            
            this.mapper = mapper;
        }

        [HttpPost("")]
        [FluentValidationAutoValidationAttribute]
        public async Task<ActionResult> Create()
        {
            var currentUserId = this.GetCurrentUserId();
            return Ok();
        }

       
    }
}
