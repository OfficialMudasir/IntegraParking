using System.Data;

namespace DBBench.Postgres
{ 
    public interface IPostgresTransaction
    {
        bool BeginTransaction();
        bool BeginTransaction(IsolationLevel isolationLevel);
        bool FinishTransaction(bool bCommit);
    }
}
