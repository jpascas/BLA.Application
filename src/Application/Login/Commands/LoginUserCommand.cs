using Application.Commands;
using Application.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands
{
    public class LoginUserCommand : BaseCommand
    {
        public string Email { get; private set; }
        public string Password { get; private set; }

        public LoginUserCommand(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }

        public override bool IsValid()
        {
            var validator = new LoginValidation();
            this.ValidationResult = validator.Validate(this);
            return this.ValidationResult.IsValid;
        }

    }
}
