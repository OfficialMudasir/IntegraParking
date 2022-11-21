using System;
using System.Collections.Generic;
using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Globalization;
using System.Configuration;
using integraMobile.Infrastructure;
using System.Threading;


namespace integraMobile
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new OutputCacheAttribute
            {
                VaryByParam = "*",
                Duration = 0,
                NoStore = true,
            });

            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
     
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "LogOn", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "IndividualsRegistration",                         // Route name
                "{controller}/{action}/{id}",      // URL with parameters 
                new
                {
                    controller = "IndividualsRegistration",     // Parameter defaults
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );



        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Use LocalDB for Entity Framework by default
            //Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");
            log4net.Config.BasicConfigurator.Configure();
            log4net.Config.XmlConfigurator.Configure();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

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
                        if (langName != "en-US" && langName != "es-ES") langName = ConfigurationManager.AppSettings["DefaultLanguage"] ?? "es-ES";
                    }
                    ci = new CultureInfo(langName);
                    this.Session["Culture"] = ci;
                }
                //Finally setting culture for each request
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }

        }

    }
}