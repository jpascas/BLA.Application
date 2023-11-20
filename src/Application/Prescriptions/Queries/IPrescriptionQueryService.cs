using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Queries
{
    public interface IPrescriptionQueryService
    {
        Task<Prescription> FindById(Guid id);
        Task<List<Prescription>> FindByCurrentUserId();
    }
}
