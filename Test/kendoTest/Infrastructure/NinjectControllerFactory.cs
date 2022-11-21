using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ninject;
using Ninject.Modules;
using System.Web.Routing;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Concrete;
using System.Configuration;
using kendoTest.Infrastructure;

namespace integraMobile.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        // A Ninject "kernel" is the thing that can supply object instances
        private IKernel kernel = new StandardKernel(new integraMobileServices());

        // ASP.NET MVC calls this to get the controller for each request
        protected override IController GetControllerInstance(RequestContext context,
                                                             Type controllerType)
        {
            if (controllerType == null)
                return null;
            return (IController)kernel.Get(controllerType);
        }

        // Configures how abstract service types are mapped to concrete implementations
        private class integraMobileServices : NinjectModule
        {
            public override void Load()
            {
                // We'll add some configuration here in a moment
                Bind<ICustomersRepository>()
                           .To<SQLCustomersRepository>()
                           .WithConstructorArgument("connectionString",
                               ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                           );

                Bind<IInfraestructureRepository>()
                           .To<SQLInfraestructureRepository>()
                           .WithConstructorArgument("connectionString",
                               ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                           );

                Bind<SQLDataRepository>()
                           .To<SQLDataRepository>()
                           .WithConstructorArgument("connectionString",
                               ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                           );
            }
        }
    } 
}