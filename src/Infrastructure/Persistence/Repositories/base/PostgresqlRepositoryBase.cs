using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistency.Repositories
{
    public class PostgresqlRepositoryBase<TConfig> where TConfig : IDBConnectionConfig
    {        
        private readonly string ConnectionString;        
        public PostgresqlRepositoryBase(IDBConnectionConfig config)
        {
            this.ConnectionString = config.ConnectionString;            
        }

        private NpgsqlConnection GetConnection()
        {
            NpgsqlConnection conn = new NpgsqlConnection(this.ConnectionString);
            return conn;
        }

        private NpgsqlCommand GetCommand(string functionName, NpgsqlConnection conn, NpgsqlParameter[] parameters, NpgsqlTransaction transaction = null)
        {
            var cmd = new NpgsqlCommand(functionName, conn);
            cmd.Transaction = transaction;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;           
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            return cmd;
        }

        private T GetSingle<T>(NpgsqlDataReader reader, Func<NpgsqlDataReader, T> converter) where T : class
        {
            if (reader.Read())
            {
                return converter(reader);
            }
            return null;
        }

        private List<T> GetList<T>(NpgsqlDataReader reader, Func<NpgsqlDataReader, T> converter) where T : class
        {
            List<T> result = new List<T>();
            while (reader.Read())
            {
                result.Add(converter(reader));
            }
            return result;
        }

        private delegate T ExecuteReaderAndAction<T>(NpgsqlDataReader reader);

        private T ExecuteActionFromReader<T>(string functionName, NpgsqlParameter[] parameters, ExecuteReaderAndAction<T> action)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                NpgsqlCommand cmd = GetCommand(functionName, conn, parameters);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    return action(reader);
                }
            }
        }

        public List<T> ExecuteAsList<T>(string functionName, NpgsqlParameter[] parameters, Func<NpgsqlDataReader, T> converter) where T : class
        {
            return ExecuteActionFromReader(functionName, parameters, (reader) => this.GetList<T>(reader, converter));
        }

        public T ExecuteAsSingleOrDefault<T>(string functionName, NpgsqlParameter[] parameters, Func<NpgsqlDataReader, T> converter) where T : class
        {
            return ExecuteActionFromReader(functionName, parameters, (reader) => this.GetSingle<T>(reader, converter));
        }

        public void ExecuteAsNonQuery(string functionName, NpgsqlParameter[] parameters)
        {
            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                NpgsqlCommand cmd = GetCommand(functionName, conn, parameters);
                cmd.ExecuteNonQuery();
            }
        }

        public T ExecuteScalar<T>(string functionName, NpgsqlParameter[] parameters)
        {

            using (NpgsqlConnection conn = GetConnection())
            {
                conn.Open();
                NpgsqlCommand cmd = GetCommand(functionName, conn, parameters);
                return (T)cmd.ExecuteScalar();
            }
        }

        public NpgsqlParameter GetNamedParameter(string name, object value, NpgsqlTypes.NpgsqlDbType type)
        {
            var nameParam = new NpgsqlParameter("@" + name, value);
            nameParam.NpgsqlDbType = type;
            return nameParam;
        }
    }
}
