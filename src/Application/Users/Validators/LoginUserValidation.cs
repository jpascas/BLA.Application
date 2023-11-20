using Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validations
{
    public class LoginUserValidation : AbstractValidator<LoginUserCommand>
    {
   

        public LoginUserValidation()
        {
            ValidateEmail();
            ValidatePassword();            
        }

        protected void ValidateEmail()
        {
            RuleFor(c => c.Email)
                .NotEmpty()
                .WithMessage("User Email Cannot be Empty")
                .EmailAddress()
                .WithMessage("Email should contain a valid email");
        }

        protected void ValidatePassword()
        {
            RuleFor(p => p.Password).NotEmpty().WithMessage("Your password cannot be empty");          
        }
    }
}
