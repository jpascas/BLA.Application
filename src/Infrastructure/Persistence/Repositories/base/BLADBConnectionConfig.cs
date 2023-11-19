using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistency.Repositories
{
    public class BLADBConnectionConfig : IDBConnectionConfig
    {
        public BLADBConnectionConfig(string connectionString)
        {
            ConnectionString = connectionString;
        }
        public string ConnectionString { get; }
    }    
}
