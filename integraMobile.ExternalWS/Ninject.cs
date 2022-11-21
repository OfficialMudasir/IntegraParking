using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Concrete;
using Ninject;

namespace integraMobile.ExternalWS
{
    public class integraMobileThirdPartyConfirmModule : Ninject.Modules.NinjectModule
    {

        public override void Load()
        {
            Bind<ICustomersRepository>()
                            .To<SQLCustomersRepository>()
                            .WithConstructorArgument("connectionString",
                                System.Configuration.ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                            );

            Bind<IInfraestructureRepository>()
                           .To<SQLInfraestructureRepository>()
                           .WithConstructorArgument("connectionString",
                               ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                           );

            Bind<IGeograficAndTariffsRepository>()
                           .To<SQLGeograficAndTariffsRepository>()
                           .WithConstructorArgument("connectionString",
                               ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                           );

            Bind<IRetailerRepository>()
                           .To<SQLRetailerRepository>()
                           .WithConstructorArgument("connectionString",
                               ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                           );    

        }

    }
}
