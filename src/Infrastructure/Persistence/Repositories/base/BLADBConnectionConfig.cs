using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistency.Repositories
{
    public class BlaDBConnectionConfig : IDBConnectionConfig
    {
        public BlaDBConnectionConfig(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public string ConnectionString { get; }
    }    
}
