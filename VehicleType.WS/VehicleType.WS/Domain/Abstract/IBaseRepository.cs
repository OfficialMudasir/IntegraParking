using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;

namespace VehicleType.WS.Domain.Abstract
{
    public enum VehicleTypeLockMode
    {
        None = 0,
        Read = 1,
        Upgrade = 2,
        UpgradeNoWait = 3,
        Write = 4
    }

    public interface IBaseRepository
    {
        IVehicleTypeTransaction CreateTransaction();

        Type GetEntityType(string sEntityName, string sNamespace = "iParkTicket.Domain.Entities");
        IQueryable GetQuery(Type typeEntity, IVehicleTypeTransaction trans = null, int? iTimeoutSeconds = null);
        bool Save(object oEntity, IVehicleTypeTransaction trans = null, bool bFlush = true);
        bool SaveOrUpdate(object oEntity, IVehicleTypeTransaction trans = null);
        bool Delete(object oEntity, IVehicleTypeTransaction trans = null);
        IList GetList(Type oTypeEntity, ICriterion oCriteria = null, Order oOrder = null, VehicleTypeLockMode lockMode = VehicleTypeLockMode.None, IVehicleTypeTransaction trans = null);
        IList GetSQLQuery(Type oTypeEntity, string sQuery, IVehicleTypeTransaction trans = null);
        IList GetHQLQuery(Type oTypeEntity, string sQuery, IVehicleTypeTransaction trans = null);
        IEnumerable<TOut> ExecuteStoredProcedure<TOut>(string procedureName, IList<StoredProcedureParameter> parameters, IVehicleTypeTransaction trans = null);
        TOut ExecuteScalarStoredProcedure<TOut>(string procedureName, IList<StoredProcedureParameter> parameters, IVehicleTypeTransaction trans = null);

        void Dispose();
        bool IsSessionActive();
    }

    public class StoredProcedureParameter
    {
        public string ParamName { get; set; }
        public object ParamValue { get; set; }

        public StoredProcedureParameter(string sParamName, object oParamValue)
        {
            ParamName = sParamName;
            ParamValue = oParamValue;
        }
    }
}
