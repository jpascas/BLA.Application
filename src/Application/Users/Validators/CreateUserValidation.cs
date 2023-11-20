using Application.Commands;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Validations
{
    public class CreateUserValidation : AbstractValidator<CreateUserCommand>
    {
   

        public CreateUserValidation()
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
            RuleFor(p => p.Password).NotEmpty().WithMessage("Your password cannot be empty")
                .MinimumLength(8).WithMessage("Your password length must be at least 8.")
                .MaximumLength(16).WithMessage("Your password length must not exceed 16.")
                .Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
                .Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
                .Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
                .Matches(@"[\!\?\*\.]+").WithMessage("Your password must contain at least one (!? *.).");
        }
    }
}
