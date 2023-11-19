using Application.Validations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Presentation.ViewModels.Validations
{
    public class LoginRequestModelValidation : AbstractValidator<LoginRequestModel>
    {
        public LoginRequestModelValidation()
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
