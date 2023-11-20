using Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validations
{
    public class CreatePrescriptionValidation : AbstractValidator<CreatePrescriptionCommand>
    {
   

        public CreatePrescriptionValidation()
        {
            ValidateDrug();
            ValidateDosage();
            ValidateNotes();
        }

        protected void ValidateDrug()
        {
            RuleFor(c => c.Drug)
                .NotEmpty()
                .WithMessage("Drug Cannot be Empty");                
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
