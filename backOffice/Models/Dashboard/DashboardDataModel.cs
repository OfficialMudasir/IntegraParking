using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using backOffice.Properties;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;

namespace backOffice.Models.Dashboard
{
    public class DashboardDataModel
    {
        [LocalizedDisplayName("DashboardDataModel_Installations", NameResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        public object[] Installations { get; set; }

        [LocalizedDisplayName("DashboardDataModel_DateGroup", NameResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        public DateGroupType DateGroup { get; set; }

        [LocalizedDisplayName("DashboardDataModel_DateGroupPattern", NameResourceType = typeof(Resources))]        
        public bool DateGroupPattern { get; set; }

        [LocalizedDisplayName("DashboardDataModel_DateFilter", NameResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        public DateFilterType DateFilter { get; set; }

        [LocalizedDisplayName("DashboardDataModel_CustomIniDate", NameResourceType = typeof(Resources))]
        public DateTime CustomIniDate { get; set; }

        [LocalizedDisplayName("DashboardDataModel_CustomEndDate", NameResourceType = typeof(Resources))]
        public DateTime CustomEndDate { get; set; }

        [LocalizedDisplayName("DashboardDataModel_CustomIniTime", NameResourceType = typeof(Resources))]
        public string CustomIniTime { get; set; }

        [LocalizedDisplayName("DashboardDataModel_CustomEndTime", NameResourceType = typeof(Resources))]
        public string CustomEndTime { get; set; }

        [LocalizedDisplayName("DashboardDataModel_Currency", NameResourceType = typeof(Resources))]
        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorsMsg_RequiredField")]
        public decimal CurrencyId { get; set; }

        public string CurrencyIsoCode { get; set; }

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