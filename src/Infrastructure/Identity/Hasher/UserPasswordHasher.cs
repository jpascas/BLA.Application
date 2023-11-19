using Application.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Identity
{
    public class UserPasswordHasher : PasswordHasher<User>, IPasswordHasher<User>, IPasswordHasher
    {
        public UserPasswordHasher(
            IOptions<PasswordHasherOptions> optionsAccessor = null)
            : base(optionsAccessor) { }

        public string Hash(string password)
        {
            return this.HashPassword(null, password);
        }
        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            var verifyResult = this.VerifyHashedPassword(null, hashedPassword, providedPassword);
            return verifyResult == PasswordVerificationResult.Success;
        }
        /// </summary>
        public override PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null)
            {
                throw new ArgumentNullException(nameof(hashedPassword));
            }
            if (providedPassword == null)
            {
                throw new ArgumentNullException(nameof(providedPassword));
            }            

            var verifyResult = base.VerifyHashedPassword(user, hashedPassword, providedPassword);

            return verifyResult;
        }
    }

}
