using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Queries
{
    public interface IUserQueryService
    {
        Task<User> FindById(long id);
        Task<User> FindByEmail(string email);
    }
}
