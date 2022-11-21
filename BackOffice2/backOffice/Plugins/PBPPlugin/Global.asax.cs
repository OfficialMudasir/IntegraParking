using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Globalization;
using System.Threading;
using backOffice.Infrastructure;

using Ninject;
using Ninject.Modules;
using System.Web.Routing;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Concrete;
using System.Configuration;

namespace PBPPlugin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Activator oActivator = new Activator();
            oActivator.Start(null);

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //It's important to check whether session object is ready
            if (HttpContext.Current.Session != null)
            {
                CultureInfo ci = (CultureInfo)this.Session["Culture"];
                //Checking first if there is no value in session 
                //and set default language 
                //this can happen for first user's request
                if (ci == null)
                {
                    //Sets default culture to english invariant
                    string langName = "en-US";

                    //Try to get values from Accept lang HTTP header
                    if (HttpContext.Current.Request.UserLanguages != null &&
                        HttpContext.Current.Request.UserLanguages.Length != 0)
                    {
                        try
                        {
                            //Gets accepted list 
                            langName = HttpContext.Current.Request.UserLanguages[0].Substring(0, 5);
                        }
                        catch { }
                        if (langName != "en-US" && langName != "es-ES" && langName != "ca-ES") langName = "es-ES";
                    }
                    ci = new CultureInfo(langName);
                    this.Session["Culture"] = ci;
                }
                //Finally setting culture for each request
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                //UIShell.OSGi.Utility.Messages.Culture = ci; // new System.Globalization.CultureInfo("en-US"); 
            }

        }
    }

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

                Bind<IBackOfficeRepository>()
                           .To<SQLBackOfficeRepository>()
                           .WithConstructorArgument("connectionString",
                               ConfigurationManager.ConnectionStrings["integraMobile.Domain.Properties.Settings.integraMobileConnectionString"].ConnectionString
                           );

            }
        }
    } 

}