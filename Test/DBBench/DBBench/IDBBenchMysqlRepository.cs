using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBBench;

namespace DBBench.Mysql
{
    public interface IDBBenchMysqlRepository : IBaseRepository
    {
        bool InsertTestTable(MyTestTable o);

        void EndConnection();
    }
}
