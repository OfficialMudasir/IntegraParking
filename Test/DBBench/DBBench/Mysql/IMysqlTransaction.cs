using System.Data;

namespace DBBench.Mysql
{
    public interface IMysqlTransaction
    {
        bool BeginTransaction();
        bool BeginTransaction(IsolationLevel isolationLevel);
        bool FinishTransaction(bool bCommit);
    }
}
