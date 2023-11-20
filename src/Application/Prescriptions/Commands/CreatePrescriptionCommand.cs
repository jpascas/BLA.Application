using Application.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class CreatePrescriptionCommand : BaseCommand
    {
        public string Drug { get; private set; }
        public string Dosage { get; private set; }
        public string Notes { get; private set; }


        public CreatePrescriptionCommand(string drug, string dosage, string notes)
        {
            this.Drug = drug;
            this.Dosage = dosage;
            this.Notes = notes;
        }

        public override bool IsValid()
        {
            var validator = new CreatePrescriptionValidation();
            this.ValidationResult = validator.Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
