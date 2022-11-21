using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;

namespace DBBench.Postgres
{
    public enum PostgresLockMode
    {
        None = 0,
        Read = 1,
        Upgrade = 2,
        UpgradeNoWait = 3,
        Write = 4
    }

    public interface IBaseRepository
    {
        IPostgresTransaction CreateTransaction();
        IQueryable GetQuery(Type typeEntity, IPostgresTransaction trans = null, int? iTimeoutSeconds = null);
        void Dispose();
        bool IsSessionActive();
    }

    public class PostgresStoredProcedureParameter
    {
        public string ParamName { get; set; }
        public object ParamValue { get; set; }

        public PostgresStoredProcedureParameter(string sParamName, object oParamValue)
        {
            ParamName = sParamName;
            ParamValue = oParamValue;
        }
    }
}
