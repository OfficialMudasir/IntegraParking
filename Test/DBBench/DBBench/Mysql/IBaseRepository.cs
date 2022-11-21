using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Criterion;

namespace DBBench.Mysql
{
    public enum MysqlLockMode
    {
        None = 0,
        Read = 1,
        Upgrade = 2,
        UpgradeNoWait = 3,
        Write = 4
    }

    public interface IBaseRepository
    {
        IMysqlTransaction CreateTransaction();
        IQueryable GetQuery(Type typeEntity, IMysqlTransaction trans = null, int? iTimeoutSeconds = null);
        void Dispose();
        bool IsSessionActive();
    }

    public class MysqlStoredProcedureParameter
    {
        public string ParamName { get; set; }
        public object ParamValue { get; set; }

        public MysqlStoredProcedureParameter(string sParamName, object oParamValue)
        {
            ParamName = sParamName;
            ParamValue = oParamValue;
        }
    }
}
