using Application.Abstractions;
using Application.Commands;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class LoginUserHandler : ICommandHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository repository;
        private readonly IJwtProvider jwtProvider;
        private readonly IPasswordHasher passwordHasher;

        public LoginUserHandler(IUserRepository repository, IJwtProvider jwtProvider, IPasswordHasher passwordHasher)
        {
            this.repository = repository;
            this.jwtProvider = jwtProvider;
            this.passwordHasher = passwordHasher;
        }

        public async Task<OperationResult<string>> Handle(LoginUserCommand command)
        {
            if (!command.IsValid())
                return OperationResult<string>.FailureResult("Input is Invalid");

            // check if user already exists
            var existentUser = await this.repository.GetByEmail(command.Email);

            if (existentUser == null)
                return OperationResult<string>.FailureResult("User doesnt exists", 401);

            if (!this.passwordHasher.VerifyHashedPassword(existentUser.PasswordHash, command.Password))            
                return OperationResult<string>.FailureResult("Password is incorrect", 401);

            return OperationResult<string>.SuccessResult(jwtProvider.Generate(existentUser)); // return JWT
        }
    }
}
