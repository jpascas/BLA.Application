using Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validations
{
    public class UpdatePrescriptionValidation : AbstractValidator<UpdatePrescriptionCommand>
    {
   

        public UpdatePrescriptionValidation()
        {
            ValidateId();
            ValidateDosage();
            ValidateNotes();
        }

        protected void ValidateId()
        {
            RuleFor(c => c.Id)
                .NotEmpty()
                .WithMessage("Id Cannot be Empty");                
        }

        protected void ValidateDosage()
        {
            RuleFor(p => p.Dosage).NotEmpty().WithMessage("Dosage Cannot be Empty");          
        }
        protected void ValidateNotes()
        {
            RuleFor(p => p.Notes).NotEmpty().WithMessage("Notes Cannot be Empty");
        }
    }
}
