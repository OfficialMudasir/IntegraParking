using System.Data;

namespace VehicleType.WS.Domain.Abstract
{
    public interface IVehicleTypeTransaction
    {
        bool BeginTransaction();
        bool BeginTransaction(IsolationLevel isolationLevel);
        bool FinishTransaction(bool bCommit);
    }
}
