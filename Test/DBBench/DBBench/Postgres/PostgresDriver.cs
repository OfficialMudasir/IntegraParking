using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostgresDriver.DbDriver
{
    class DevartPgDriver : NHibernate.Driver.ReflectionBasedDriver
    {
        public DevartPgDriver()
            : base(
            "Devart.Data.PostgreSql",
            "Devart.Data.PostgreSql.NHibernate.NHibernatePgSqlConnection",
            "Devart.Data.PostgreSql.NHibernate.NHibernatePgSqlCommand")
        {
        }

        public override string NamedPrefix
        {
            get { return ":"; }
        }

        public override bool UseNamedPrefixInParameter
        {
            get { return false; }
        }

        public override bool UseNamedPrefixInSql
        {
            get { return true; }
        }

        public override bool SupportsMultipleOpenReaders
        {
            get { return false; }
        }

        protected override bool SupportsPreparingCommands
        {
            get { return true; }
        }

        public override NHibernate.Driver.IResultSetsCommand GetResultSetsCommand(NHibernate.Engine.ISessionImplementor session)
        {
            return new NHibernate.Driver.BasicResultSetsCommand(session);
        }

        public override bool SupportsMultipleQueries
        {
            get { return true; }
        }

        protected override void InitializeParameter(System.Data.Common.DbParameter dbParam, string name, NHibernate.SqlTypes.SqlType sqlType)
        {
            base.InitializeParameter(dbParam, name, sqlType);

            // Since the .NET currency type has 4 decimal places, we use a decimal type in PostgreSQL instead of its native 2 decimal currency type.
            if (sqlType.DbType == System.Data.DbType.Currency)
                dbParam.DbType = System.Data.DbType.Decimal;
        }
    }
}