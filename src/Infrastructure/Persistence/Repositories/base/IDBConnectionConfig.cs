using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistency.Repositories
{
    public interface IDBConnectionConfig
    {
        string ConnectionString { get; }
    }    
}
