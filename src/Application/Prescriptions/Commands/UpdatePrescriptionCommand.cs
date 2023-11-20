using Application.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class UpdatePrescriptionCommand : BaseCommand
    {
        public Guid Id { get; private set; }
        public string Dosage { get; private set; }
        public string Notes { get; private set; }


        public UpdatePrescriptionCommand(Guid id, string dosage, string notes)
        {
            this.Id = id;
            this.Dosage = dosage;
            this.Notes = notes;
        }

        public override bool IsValid()
        {
            var validator = new UpdatePrescriptionValidation();
            this.ValidationResult = validator.Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
