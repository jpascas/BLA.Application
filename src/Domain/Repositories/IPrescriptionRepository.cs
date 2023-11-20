using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Repositories
{
    public interface IPrescriptionRepository
    {
        Task<Prescription> GetById(Guid id);
        Task<List<Prescription>> GetByUserId(long userId);
        Task Delete(Guid Id);
        Task<Prescription> Create(Prescription prescription);
        Task<Prescription> Update(Prescription prescription);
    }
}
