using Application.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class DeletePrescriptionCommand : BaseCommand
    {
        public Guid Id { get; private set; }

        public DeletePrescriptionCommand(Guid id)
        {
            this.Id = id;
        }

        public override bool IsValid()
        {
            var validator = new DeletePrescriptionValidation();
            this.ValidationResult = validator.Validate(this);
            return this.ValidationResult.IsValid;
        }
    }
}
