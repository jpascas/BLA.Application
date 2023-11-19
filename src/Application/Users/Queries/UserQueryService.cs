using Domain.Entities;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Application.Queries
{
    public class UserQueryService : IUserQueryService
    {
        private readonly IUserRepository _repository;

        public UserQueryService(IUserRepository repository)
        {
            this._repository = repository;
        }

        public async Task<User> FindByEmail(string email)
        {
            var entity = await this._repository.GetByEmail(email);

            return entity;
        }

        public async Task<User> FindById(long id)
        {
            var entity = await this._repository.GetById(id);

            return entity;
        }
    }
}
