using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using backOffice.Infrastructure;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace backOffice.Models.Dashboard
{
    public class DashboardDataModel
    {        
        [LocalizedDisplayNameBundle("DashboardDataModel_Installations", AssemblyName = "PBPPlugin", Filename = "PBPPlugin")]        
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        public object[] Installations { get; set; }
        
        [LocalizedDisplayNameBundle("DashboardDataModel_DateGroup", AssemblyName = "PBPPlugin", Filename = "PBPPlugin")]                
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        public DateGroupType DateGroup { get; set; }
        
        [LocalizedDisplayNameBundle("DashboardDataModel_DateGroupPattern", AssemblyName = "PBPPlugin", Filename = "PBPPlugin")]
        public bool DateGroupPattern { get; set; }
        
        [LocalizedDisplayNameBundle("DashboardDataModel_DateFilter", AssemblyName = "PBPPlugin", Filename = "PBPPlugin")]        
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        public DateFilterType DateFilter { get; set; }

        [LocalizedDisplayNameBundle("DashboardDataModel_CustomIniDate", AssemblyName = "PBPPlugin", Filename = "PBPPlugin")]        
        public DateTime CustomIniDate { get; set; }

        [LocalizedDisplayNameBundle("DashboardDataModel_CustomEndDate", AssemblyName = "PBPPlugin", Filename = "PBPPlugin")]
        public DateTime CustomEndDate { get; set; }

        [LocalizedDisplayNameBundle("DashboardDataModel_CustomIniTime", AssemblyName = "PBPPlugin", Filename = "PBPPlugin")]
        public string CustomIniTime { get; set; }

        [LocalizedDisplayNameBundle("DashboardDataModel_CustomEndTime", AssemblyName = "PBPPlugin", Filename = "PBPPlugin")]
        public string CustomEndTime { get; set; }

        [LocalizedDisplayNameBundle("DashboardDataModel_Currency", AssemblyName = "PBPPlugin", Filename = "PBPPlugin")]
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        public decimal CurrencyId { get; set; }

        [LocalizedDisplayNameBundle("DashboardDataModel_UserInstallation", AssemblyName = "PBPPlugin", Filename = "PBPPlugin")]
        [RequiredBundle(ErrorMessageResourceName = "ErrorsMsg_RequiredField", AssemblyName = "PBPPlugin", Filename = "PBPPlugin", DefaultStr = "Field {0} required")]
        public UserInstallationType UserInstallation { get; set; }

        public string CurrencyIsoCode { get; set; }

        public string InstallationsInit { get; set; }

        public string[] InstallationsIds
        {
            get
            {
                string[] installations = null;
                if (Installations != null)
                {
                    installations = Installations.Select(i => i.ToString()).ToArray();
                }
                return installations;
            }
        }

        public void GetDateRange(out DateTime iniDate, out DateTime endDate)
        {
            DateTime xNow = DateTime.Now;

            endDate = xNow;

            switch (this.DateFilter)
            {
                case DateFilterType.currentWeek:
                    iniDate = xNow.Date.AddDays(-Conversions.Date2DayOfWeek(xNow));
                    break;
                case DateFilterType.currentMonth:
                    iniDate = new DateTime(xNow.Year, xNow.Month, 1);
                    break;
                case DateFilterType.currentQuarter:
                    iniDate = new DateTime(DateTime.Now.Year, (((xNow.Month - 1) / 3) * 3) + 1, 1);
                    break;
                case DateFilterType.currentHalf:
                    iniDate = new DateTime(DateTime.Now.Year, (((xNow.Month - 1) / 6) * 6) + 1, 1);
                    break;
                case DateFilterType.currentYear:
                    iniDate = new DateTime(DateTime.Now.Year, 1, 1);
                    break;

                case DateFilterType.lastWeek:
                    iniDate = xNow.AddDays(-7);
                    break;
                case DateFilterType.lastMonth:
                    iniDate = xNow.AddMonths(-1);
                    break;
                case DateFilterType.lastQuarter:
                    iniDate = xNow.AddMonths(-3);
                    break;
                case DateFilterType.lastHalf:
                    iniDate = xNow.AddMonths(-6);
                    break;
                case DateFilterType.lastYear:
                    iniDate = xNow.AddYears(-1);
                    break;

                case DateFilterType.custom:                    
                    iniDate = this.CustomIniDate;                    
                    endDate = this.CustomEndDate;
                    break;

                default:
                    iniDate = xNow.Date;
                    endDate = xNow;
                    break;
            }

        }

    }
}