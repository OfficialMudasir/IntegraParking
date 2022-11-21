using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using backOffice.Infrastructure;

namespace PBPPlugin
{
    public class RouteConfig : BaseRouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            BasePath = "~/";
            RelBasePath = "";

        }

        public static string BasePath = "~/Plugins/PBPPlugin/";
        public static string RelBasePath = "Plugins/PBPPlugin/";

        public static string GetRelativePath(string sPath)
        {
            return GetRelativePath(sPath, RelBasePath);
        }

    }
}