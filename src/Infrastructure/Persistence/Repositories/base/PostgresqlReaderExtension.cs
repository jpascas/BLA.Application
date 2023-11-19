using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistency.Repositories
{
    public static class PostgresqlReaderExtension
    {
        public static T Get<T>(this NpgsqlDataReader reader, string fieldName) where T : notnull
        {
            return (T)reader[fieldName];
        }

        public static T GetNullable<T>(this NpgsqlDataReader reader, string fieldName)
        {
            int ordinal = reader.GetOrdinal(fieldName);
            if (reader.IsDBNull(ordinal))
            {
                return default(T);
            }
            else
            {
                return (T)reader[ordinal];
            }
        }
    }
}
