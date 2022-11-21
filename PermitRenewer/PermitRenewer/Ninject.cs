using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Concrete;
using Ninject;

namespace PermitRenewer
{
    public class PermitRenewerModule : Ninject.Modules.NinjectModule
    {
        public override void Load()
        {
            Bind<IBackOfficeRepository>()
                            .To<SQLBackOfficeRepository>()
                            .WithConstructorArgument("connectionString",
                                System.Configuration.ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                            );
            Bind<ICustomersRepository>()
                            .To<SQLCustomersRepository>()
                            .WithConstructorArgument("connectionString",
                                System.Configuration.ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                            );
            Bind<IInfraestructureRepository>()
                            .To<SQLInfraestructureRepository>()
                            .WithConstructorArgument("connectionString",
                                System.Configuration.ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                            );
            Bind<SQLGeograficAndTariffsRepository>()
                            .To<SQLGeograficAndTariffsRepository>()
                            .WithConstructorArgument("connectionString",
                                System.Configuration.ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                            );
        }
    }
}