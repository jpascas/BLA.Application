using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetById(long id);
        Task<User> GetByEmail(string email);
        Task<User> Create(User user);        
        Task Delete(Guid id);
    }
}
