using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistency.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
using Npgsql;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Persistence.Repositories
{
    public class PrescriptionRepository : PostgresqlRepositoryBase<BLADBConnectionConfig>, IPrescriptionRepository
    {
        public PrescriptionRepository(BLADBConnectionConfig config) : base(config)
        {
        }

        public Task<Prescription> Create(Prescription prescription)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
            {
                        GetNamedParameter("p_user_id", prescription.UserId, NpgsqlTypes.NpgsqlDbType.Bigint),
                        GetNamedParameter("p_drug", prescription.Drug, NpgsqlTypes.NpgsqlDbType.Text),
                        GetNamedParameter("p_dosage", prescription.Dosage, NpgsqlTypes.NpgsqlDbType.Text),
                        GetNamedParameter("p_notes", prescription.Notes, NpgsqlTypes.NpgsqlDbType.Text),
                        GetNamedParameter("p_created_by", prescription.CreatedBy, NpgsqlTypes.NpgsqlDbType.Bigint),                        
            };                       
            return Task.FromResult(this.ExecuteAsSingleOrDefault<Prescription>("sp_insert_prescription", parameters.ToArray(), Mapper));            
        }

        public Task<Prescription> Update(Prescription prescription)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
            {
                        GetNamedParameter("p_id", prescription.Id, NpgsqlTypes.NpgsqlDbType.Uuid),                        
                        GetNamedParameter("p_dosage", prescription.Dosage, NpgsqlTypes.NpgsqlDbType.Text),
                        GetNamedParameter("p_notes", prescription.Notes, NpgsqlTypes.NpgsqlDbType.Text),
                        GetNamedParameter("p_modified_by", prescription.CreatedBy, NpgsqlTypes.NpgsqlDbType.Bigint),
            };
            return Task.FromResult(this.ExecuteAsSingleOrDefault<Prescription>("sp_update_prescription", parameters.ToArray(), Mapper));
        }

        public Task Delete(Guid id)
        {
            NpgsqlParameter[] parameters =
           {
                GetNamedParameter("p_id", id, NpgsqlTypes.NpgsqlDbType.Uuid),                
            };
            this.ExecuteAsNonQuery("sp_deletebyid_prescription", parameters);
            return Task.CompletedTask;
        }
        public Task<Prescription> GetById(Guid id)
        {
            NpgsqlParameter[] parameters =
            {
                GetNamedParameter("p_id", id, NpgsqlTypes.NpgsqlDbType.Uuid)
            };
            return Task.FromResult(this.ExecuteAsSingleOrDefault<Prescription>("sp_getbyid_prescription", parameters, Mapper));
        }

        public Task<List<Prescription>> GetByUserId(long userId)
        {
            NpgsqlParameter[] parameters =
            {
                GetNamedParameter("p_user_id", userId, NpgsqlTypes.NpgsqlDbType.Bigint)
            };
            return Task.FromResult(this.ExecuteAsList<Prescription>("sp_getbyuserid_prescription", parameters, Mapper));
        }

        public Prescription Mapper(NpgsqlDataReader reader)
        {
            Prescription prescription = new Prescription();
            prescription.Id = reader.Get<Guid>("id");
            prescription.UserId = reader.Get<long>("user_id");
            prescription.Drug = reader.Get<string>("drug");
            prescription.Dosage = reader.Get<string>("dosage");
            prescription.Notes = reader.Get<string>("notes");
            prescription.CreatedBy = reader.Get<long>("created_by");
            prescription.CreatedAt = reader.Get<DateTime>("created_at");
            prescription.ModifiedBy = reader.Get<long>("modified_by");
            prescription.ModifiedAt = reader.Get<DateTime>("modified_at");
            return prescription;
        }        
    }
}
