using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;

namespace DBBench.Oracle
{
    public enum OracleLockMode
    {
        None = 0,
        Read = 1,
        Upgrade = 2,
        UpgradeNoWait = 3,
        Write = 4
    }

    public interface IBaseRepository
    {
        IOracleTransaction CreateTransaction();

        Type GetEntityType(string sEntityName, string sNamespace = "Oracle.Domain.Entities");
        IQueryable GetQuery(Type typeEntity, IOracleTransaction trans = null, int? iTimeoutSeconds = null);
        bool Save(object oEntity, IOracleTransaction trans = null, bool bFlush = true);
        bool SaveOrUpdate(object oEntity, IOracleTransaction trans = null);
        bool Delete(object oEntity, IOracleTransaction trans = null);
        IList GetList(Type oTypeEntity, ICriterion oCriteria = null, Order oOrder = null, OracleLockMode lockMode = OracleLockMode.None, IOracleTransaction trans = null);
        IList GetSQLQuery(Type oTypeEntity, string sQuery, IOracleTransaction trans = null);
        IList GetHQLQuery(Type oTypeEntity, string sQuery, IOracleTransaction trans = null);
        IEnumerable<TOut> ExecuteStoredProcedure<TOut>(string procedureName, IList<OracleStoredProcedureParameter> parameters, IOracleTransaction trans = null);
        TOut ExecuteScalarStoredProcedure<TOut>(string procedureName, IList<OracleStoredProcedureParameter> parameters, IOracleTransaction trans = null);
        bool ExecuteHQLQuery(string sQuery, IList<OracleStoredProcedureParameter> parameters = null, IOracleTransaction trans = null);
        void Flush(IOracleTransaction trans = null);

        void Dispose();
        bool IsSessionActive();
    }

    public class OracleStoredProcedureParameter
    {
        public string ParamName { get; set; }
        public object ParamValue { get; set; }
        public bool IsList { get; set; }

        public OracleStoredProcedureParameter(string sParamName, object oParamValue, bool bIsList = false)
        {
            ParamName = sParamName;
            ParamValue = oParamValue;
            IsList = bIsList;
        }
    }
}
