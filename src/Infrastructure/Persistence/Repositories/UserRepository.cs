using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Persistency.Repositories;
using Microsoft.VisualBasic;
using Npgsql;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository : PostgresqlRepositoryBase<BlaDBConnectionConfig>, IUserRepository
    {
        public UserRepository(BlaDBConnectionConfig config) : base(config)
        {
        }

        public Task<User> Create(User user)
        {
            List<NpgsqlParameter> parameters = new List<NpgsqlParameter>()
            {
                        GetNamedParameter("p_email", user.Email, NpgsqlTypes.NpgsqlDbType.Text),
                        GetNamedParameter("p_password_hash", user.PasswordHash, NpgsqlTypes.NpgsqlDbType.Text)
            };                       
            return Task.FromResult(this.ExecuteAsSingleOrDefault<User>("sp_insert_user", parameters.ToArray(), Mapper));            
        }

        public Task Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByEmail(string email)
        {
            NpgsqlParameter[] parameters =
            {
                GetNamedParameter("p_email", email, NpgsqlTypes.NpgsqlDbType.Text)
            };
            return Task.FromResult(this.ExecuteAsSingleOrDefault<User>("sp_getbyemail_user", parameters, Mapper));
        }

        public Task<User> GetById(long id)
        {
            NpgsqlParameter[] parameters =
            {
                GetNamedParameter("p_user_id", id, NpgsqlTypes.NpgsqlDbType.Bigint)
            };
            return Task.FromResult(this.ExecuteAsSingleOrDefault<User>("sp_getbyid_user", parameters, Mapper));
        }

        public static User Mapper(NpgsqlDataReader reader)
        {
            User user = new User();
            user.Id = reader.Get<long>("id");
            user.Email = reader.Get<string>("email");
            user.PasswordHash = reader.Get<string>("password_hash");            
            return user;
        }
    }
}
