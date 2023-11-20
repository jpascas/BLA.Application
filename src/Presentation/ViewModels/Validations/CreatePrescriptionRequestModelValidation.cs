using Application.Validations;
using FluentValidation;
using Presentation.ViewModels.Prescriptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Presentation.ViewModels.Validations
{
    public class CreatePrescriptionRequestModelValidation : AbstractValidator<CreatePrescriptionRequestModel>
    {
        public CreatePrescriptionRequestModelValidation()
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
