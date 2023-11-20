using Application.Abstractions;
using Application.Commands;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class CreatePrescriptionHandler : ICommandHandler<CreatePrescriptionCommand, Prescription>
    {

        private readonly IPrescriptionRepository repository;
        private readonly IContextProvider contextProvider;

        public CreatePrescriptionHandler(IPrescriptionRepository repository, IContextProvider contextProvider)
        {
            this.repository = repository;            
            this.contextProvider = contextProvider;
        }

        public async Task<OperationResult<Prescription>> Handle(CreatePrescriptionCommand command)
        {
            if (!command.IsValid())
                return OperationResult<Prescription>.FailureResult("Input is Invalid");

            var currentUserId = this.contextProvider.GetCurrentUserId();
            var newPrescription = new Prescription() { 
                Drug = command.Drug , Dosage = command.Dosage, Notes = command.Notes,
                UserId = currentUserId, CreatedBy = currentUserId
            };
            var createdPrescription = await this.repository.Create(newPrescription);
            return OperationResult<Prescription>.SuccessResult(createdPrescription);
        }
    }
}
