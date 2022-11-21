using VehicleType.WS.Infrastructure.Logging.Tools;
using VehicleType.WS.Domain.Entities;
using VehicleType.WS.Domain.Abstract;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace VehicleType.WS.Domain.Concrete
{
    public class OracleVehicleTypeRepository : OracleBaseRepository, IVehicleTypeRepository
    {
        #region Constructor

        public OracleVehicleTypeRepository(bool bOpenSession)
            : base(typeof(OracleVehicleTypeRepository), null, bOpenSession)
        {            
        }

        public OracleVehicleTypeRepository(NHSessionManager.ConnectionConfiguration oConnectionConfig, bool bOpenSession)
            : base(typeof(OracleVehicleTypeRepository), oConnectionConfig, bOpenSession)
        {            
        }

        #endregion

        #region Public methods

        public bool GetVehicleType(string sPlate, out string sVehicleType)
        {
            bool bRet = false;
            sVehicleType = null;

            using (var connection = new OracleConnectionHelper(m_session))
            {
                try
                {
                    connection.BeginConnection();
                    var oVehicleType =
                        connection.Session.Query<Vehicle>()
                            .Where(i => i.VehPlate == sPlate)
                            .FirstOrDefault();

                    if (oVehicleType != null)
                        sVehicleType = oVehicleType.VehType;

                    bRet = true;
                }
                catch (Exception e)
                {
                    Log.LogMessage(LogLevels.logERROR, "GetVehicleType: ", e);
                }
                finally
                {
                    connection.EndConnection();
                }
            }

            return bRet;
        }

        #endregion
    }
}
