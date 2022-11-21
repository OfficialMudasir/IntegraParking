using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBBench;

namespace DBBench.Oracle
{
    public interface IDBBenchOracleRepository : IBaseRepository
    {
        bool InsertTestTable(OraTestTable o);

        void EndConnection();
    }
}
