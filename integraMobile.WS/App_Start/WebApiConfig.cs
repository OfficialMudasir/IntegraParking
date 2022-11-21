using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
//using System.Web.Mvc;
using integraMobile.WS.WebAPI;
//using integraMobile.WS.WebAPI.Filters;

namespace integraMobile.WS
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            #region Formatters

            // Disabling XML only JSON input/output es enabled.
            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

            // Delete JSON formatter
            config.Formatters.Remove(config.Formatters.JsonFormatter);
            config.Formatters.Add(new JsonFormatter());

            #endregion

            #region Filters

            //config.Filters.Add(new ExceptionFilter());
            //config.Filters.Add(new TariffComputerFilter());

            #endregion

            #region Handlers

            ///config.Services.Replace(typeof(IExceptionHandler),
            ///    new UnhandledExceptionHandler(config.Services.GetExceptionHandler()));
            ///config.MessageHandlers.Add(new BasicAuthenticationHandler());

            #endregion

            #region Routes

            config.MapHttpAttributeRoutes();
            RoutesConfig.RegisterRoutes(config.Routes);

            #endregion

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            //config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            //config.EnableSystemDiagnosticsTracing();
        }
    }
}