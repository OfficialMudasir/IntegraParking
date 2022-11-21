using System.Data;

namespace DBBench.Oracle
{
    public interface IOracleTransaction
    {
        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns></returns>
        bool BeginTransaction();

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        /// <returns></returns>
        bool BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// Finishes the transaction.
        /// </summary>
        /// <param name="bCommit">if set to <c>true</c> [b commit].</param>
        /// <returns></returns>
        bool FinishTransaction(bool bCommit);
    }
}
