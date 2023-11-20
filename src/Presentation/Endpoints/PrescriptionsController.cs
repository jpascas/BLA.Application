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
using Presentation.ViewModels.Prescriptions;

namespace Presentation.Endpoints
{
    [Authorize]
    [Route("api/[controller]")]
    public class PrescriptionsController : ApiController
    {
        private readonly IMapper mapper;
        private readonly ICommandBus commandBus;
        private readonly IPrescriptionQueryService prescriptionQueryService;

        public PrescriptionsController(ICommandBus commandBus, IMapper mapper, IPrescriptionQueryService prescriptionQueryService)
        {
            this.commandBus = commandBus;
            this.mapper = mapper;
            this.prescriptionQueryService = prescriptionQueryService;
        }

        [HttpPost("")]
        [FluentValidationAutoValidationAttribute]
        public async Task<ActionResult> Create(CreatePrescriptionRequestModel createRequestModel)
        {
            var command = new CreatePrescriptionCommand(createRequestModel.Drug, createRequestModel.Dosage, createRequestModel.Notes);

            var prescriptionResult = await this.commandBus.Send<CreatePrescriptionCommand, Prescription>(command);

            if (prescriptionResult.Success)
            {
                PrescriptionResultModel prescriptionModel = this.mapper.Map<Prescription, PrescriptionResultModel>(prescriptionResult.Result);
                return Ok(prescriptionModel);
            }
            else
            {
                return ToFailureResult(prescriptionResult);
            }
        }

        [HttpPut("")]
        [FluentValidationAutoValidationAttribute]
        public async Task<ActionResult> Update(UpdatePrescriptionRequestModel updateRequestModel)
        {
            var command = new UpdatePrescriptionCommand(updateRequestModel.Id, updateRequestModel.Dosage, updateRequestModel.Notes);

            var prescriptionResult = await this.commandBus.Send<UpdatePrescriptionCommand, Prescription>(command);

            if (prescriptionResult.Success)
            {
                PrescriptionResultModel prescriptionModel = this.mapper.Map<Prescription, PrescriptionResultModel>(prescriptionResult.Result);
                return Ok(prescriptionModel);
            }
            else
            {
                return ToFailureResult(prescriptionResult);
            }
        }

        [HttpGet("")]
        [FluentValidationAutoValidationAttribute]
        public async Task<ActionResult> GetAllByCurrentUser()
        {
            var prescriptions = await this.prescriptionQueryService.FindByCurrentUserId();
            var prescriptionModels = prescriptions.Select(prescription => this.mapper.Map<Prescription, PrescriptionResultModel>(prescription)).ToList();
            return Ok(prescriptionModels);
        }

        [HttpGet("{id}")]
        [FluentValidationAutoValidationAttribute]
        public async Task<ActionResult> GetById(Guid id)
        {
            var prescription = await this.prescriptionQueryService.FindById(id);
            if (prescription != null)
            {
                var prescriptionModel = this.mapper.Map<Prescription, PrescriptionResultModel>(prescription);
                return Ok(prescriptionModel);
            }
            else
                return NotFound(null);
        }

        [HttpDelete("{id}")]
        [FluentValidationAutoValidationAttribute]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeletePrescriptionCommand(id);

            var prescriptionDeleteResult = await this.commandBus.Send<DeletePrescriptionCommand, Nothing>(command);

            if (prescriptionDeleteResult.Success)
            {                
                return Ok();
            }
            else
            {
                return ToFailureResult(prescriptionDeleteResult);
            }
        }
    }
}
