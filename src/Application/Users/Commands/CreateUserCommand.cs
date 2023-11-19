using Application.Validations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Commands
{
    public class CreateUserCommand : BaseCommand
    {     
        public string Email { get; private set; }
        public string Password { get; private set; }

        public CreateUserCommand(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }

        public override bool IsValid()
        {
            var validator = new CreateUserValidation();            
            this.ValidationResult = validator.Validate(this);
            return this.ValidationResult.IsValid;
        }

    }
}
