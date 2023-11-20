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
    public class DeletePrescriptionHandler : ICommandHandler<DeletePrescriptionCommand, Nothing>
    {

        private readonly IPrescriptionRepository repository;
        private readonly IContextProvider contextProvider;

        public DeletePrescriptionHandler(IPrescriptionRepository repository, IContextProvider contextProvider)
        {
            this.repository = repository;            
            this.contextProvider = contextProvider;
        }

        public async Task<OperationResult<Nothing>> Handle(DeletePrescriptionCommand command)
        {
            if (!command.IsValid())
                return OperationResult<Nothing>.FailureResult("Input is Invalid");

            var currentUserId = this.contextProvider.GetCurrentUserId();            
            // check if the prescription exists and belongs to the user
            var existentPrescription = await this.repository.GetById(command.Id);
            if (existentPrescription == null || existentPrescription.UserId != currentUserId)
                return OperationResult<Nothing>.FailureResult("Prescription Not Found", 404);

            await this.repository.Delete(command.Id);
            return OperationResult<Nothing>.SuccessResult(Nothing.AtAll);
        }
    }
}
