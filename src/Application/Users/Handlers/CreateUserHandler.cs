using Application.Commands;
using Domain.Entities;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handlers
{
    public class CreateUserHandler : ICommandHandler<CreateUserCommand>
    {

        private readonly IUserRepository _repository;

        public CreateUserHandler(IUserRepository repository)
        {
            this._repository = repository;
        }

        public async Task Handle(CreateUserCommand command)
        {
            if (!command.IsValid())
                return;

            // check if user already exists
            var existentUser = await this._repository.GetByEmail(command.Email);

            if (existentUser != null)
            {
                command.AddError("User Already Exists");
                return;
            }

            var newUser = new User() { Email = command.Email , Password = command.Password};
            await this._repository.Create(newUser);
        }
    }
}
