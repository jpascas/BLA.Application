using Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validations
{
    public class DeletePrescriptionValidation : AbstractValidator<DeletePrescriptionCommand>
    {
   

        public DeletePrescriptionValidation()
        {
            ValidateId();            
        }

        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("Id Cannot be Empty");                
        }
    }
}
