using Application.Validations;
using FluentValidation;
using Presentation.ViewModels.Prescriptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Presentation.ViewModels.Validations
{
    public class UpdatePrescriptionRequestModelValidation : AbstractValidator<UpdatePrescriptionRequestModel>
    {
        public UpdatePrescriptionRequestModelValidation()
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
