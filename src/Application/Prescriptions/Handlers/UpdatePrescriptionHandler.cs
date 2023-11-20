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
    public class UpdatePrescriptionHandler : ICommandHandler<UpdatePrescriptionCommand, Prescription>
    {

        private readonly IPrescriptionRepository repository;
        private readonly IContextProvider contextProvider;

        public UpdatePrescriptionHandler(IPrescriptionRepository repository, IContextProvider contextProvider)
        {
            this.repository = repository;            
            this.contextProvider = contextProvider;
        }

        public async Task<OperationResult<Prescription>> Handle(UpdatePrescriptionCommand command)
        {
            if (!command.IsValid())
                return OperationResult<Prescription>.FailureResult("Input is Invalid");

            var currentUserId = this.contextProvider.GetCurrentUserId();
            var newPrescription = new Prescription() {
                Id = command.Id,
                Dosage = command.Dosage, Notes = command.Notes, ModifiedBy = currentUserId
            };
            var updatedPrescription = await this.repository.Update(newPrescription);
            return OperationResult<Prescription>.SuccessResult(updatedPrescription);
        }
    }
}
