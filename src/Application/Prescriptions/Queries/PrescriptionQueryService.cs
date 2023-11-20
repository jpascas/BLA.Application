using Application.Abstractions;
using Domain.Entities;
using Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Application.Queries
{
    public class PrescriptionQueryService : IPrescriptionQueryService
    {
        private readonly IPrescriptionRepository repository;
        private readonly IContextProvider contextProvider;

        public PrescriptionQueryService(IPrescriptionRepository repository, IContextProvider contextProvider)
        {
            this.repository = repository;
            this.contextProvider = contextProvider;
        }

        public async Task<List<Prescription>> FindByCurrentUserId()
        {
            var entities = await this.repository.GetByUserId(this.contextProvider.GetCurrentUserId());
            return entities;
        }

        public async Task<Prescription> FindById(Guid id)
        {
            var entity = await this.repository.GetById(id);
            // check if entity belong to current user and decide what to do
            if (entity != null && entity.UserId == this.contextProvider.GetCurrentUserId())
                return entity;
            return null;
        }
    }
}
