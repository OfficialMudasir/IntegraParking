using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;

namespace VehicleType.WS.Domain.Abstract
{
    public interface IStatelessBaseRepository
    {
        IVehicleTypeTransaction CreateTransaction();

        Type GetEntityType(string sEntityName, string sNamespace = "VehicleType.WS.Domain.Entities");
        IQueryable GetQuery(Type typeEntity, IVehicleTypeTransaction trans = null, int? iTimeoutSeconds = null);
        bool Insert(object oEntity, IVehicleTypeTransaction trans = null);
        bool Update(object oEntity, IVehicleTypeTransaction trans = null);
        bool Delete(object oEntity, IVehicleTypeTransaction trans = null);
        bool ExecuteUpdate(string sQuery, List<QueryParameter> oParameters, IVehicleTypeTransaction trans = null);
        IList GetList(Type oTypeEntity, ICriterion oCriteria = null, Order oOrder = null, VehicleTypeLockMode lockMode = VehicleTypeLockMode.None, IVehicleTypeTransaction trans = null);

        void Dispose();
        bool IsSessionActive();
    }

    public class QueryParameter
    {
        public QueryParameter(string sName, object oValue, Type oType)
        {
            Name = sName;
            Value = oValue;
            Type = oType;
        }

        public string Name { get; private set; }
        public object Value { get; private set; }
        public Type Type { get; private set; }
    }
}
