using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBBench;

namespace DBBench.Postgres
{
    public interface IDBBenchPostgresRepository : IBaseRepository
    {
        bool InsertTestTable(PgTestTable o);
        
        void EndConnection();
    }
}
