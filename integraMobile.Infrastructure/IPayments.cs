using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace integraMobile.Infrastructure
{
    public interface IPayments
    {
        bool RefundEnabled { get; }
        bool PartialRefundEnabled { get; }
    }
}
