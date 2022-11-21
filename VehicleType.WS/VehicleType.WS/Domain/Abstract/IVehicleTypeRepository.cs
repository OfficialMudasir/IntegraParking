using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VehicleType.WS.Domain.Abstract
{
    public interface IVehicleTypeRepository : IBaseRepository
    {
        bool GetVehicleType(string sPlate, out string sVehicleType);
    }
}