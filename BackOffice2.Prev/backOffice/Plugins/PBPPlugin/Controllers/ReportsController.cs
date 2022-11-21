using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Telerik.Reporting.Cache;
using Telerik.Reporting.Cache.Interfaces;
using Telerik.Reporting.Cache.Database;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.Engine;
using Telerik.Reporting.Services.WebApi;

namespace PBPPlugin.Controllers
{
    public class ReportsController : ReportsControllerBase
    {
        static ReportServiceConfiguration preservedConfiguration;

        static IReportServiceConfiguration PreservedConfiguration
        {
            get
            {
                if (null == preservedConfiguration)
                {
                    preservedConfiguration = new ReportServiceConfiguration
                    {
                        HostAppId = "BackOfficePBP",
                        //Storage = new DatabaseStorage("MsSql", "Data Source=HBUSQUE-LAPTOP\\SQLEXPRESS2012;Initial Catalog=ReportingState;Integrated Security=True"), 
                        Storage = new DatabaseStorage("MsSql", "integraMobile.Reports.Properties.Settings.integraMobileReports.Cache"),
                        //Storage = new FileStorage(),
                        ReportResolver = CreateReportResolver(), 
                        ReportSharingTimeout = 0,
                        // ClientSessionTimeout = 15,
                    };
                }
                return preservedConfiguration;
            }
        }

        public ReportsController()
        {
            this.ReportServiceConfiguration = PreservedConfiguration;
        }

        //protected override IReportResolver CreateReportResolver()
        static IReportResolver CreateReportResolver()
        {
            var appPath = HttpContext.Current.Server.MapPath(RouteConfig.BasePath + "/");
            var reportsPath = Path.Combine(appPath, @"Reports");

            return new ReportFileResolver(reportsPath)
                .AddFallbackResolver(new ReportTypeResolver());
        }

        /*protected override ICache CreateCache()
        {
            return Telerik.Reporting.Services.Engine.CacheFactory.CreateFileCache();
        }*/
        /*protected override ICache CreateCache()
        {
            return Telerik.Reporting.Services.Engine.CacheFactory.CreateDatabaseCache("MsSql", "integraMobile.Reports.Properties.Settings.integraMobileReports.Cache");
            //new MsSqlServerStorage("integraMobile.Reports.Properties.Settings.integraMobileReports.Cache");
        }*/
    }
}
