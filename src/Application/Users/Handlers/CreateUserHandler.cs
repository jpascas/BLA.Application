using Application.Abstractions;
using Application.Commands;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class CreateUserHandler : ICommandHandler<CreateUserCommand, User>
    {

        private readonly IUserRepository _repository;
        private readonly IPasswordHasher passwordHasher;

        public CreateUserHandler(IUserRepository repository, IPasswordHasher passwordHasher)
        {
            this._repository = repository;
            this.passwordHasher = passwordHasher;
        }

        public async Task<OperationResult<User>> Handle(CreateUserCommand command)
        {
            if (!command.IsValid())
                return OperationResult<User>.FailureResult("Input is Invalid");

            // check if user already exists
            var existentUser = await this._repository.GetByEmail(command.Email);

            if (existentUser != null)
            {                
                return OperationResult<User>.FailureResult("User Already Exists");
            }

            var hash = this.passwordHasher.Hash(command.Password);
            var newUser = new User() { Email = command.Email , PasswordHash = hash };
            var createdUser = await this._repository.Create(newUser);
            return OperationResult<User>.SuccessResult(createdUser);
        }
    }
}
