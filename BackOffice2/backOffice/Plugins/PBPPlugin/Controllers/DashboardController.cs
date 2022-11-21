using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

//using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using backOffice.Infrastructure;
using backOffice.Infrastructure.Security;
using backOffice.Models;
using backOffice.Models.Dashboard;

namespace PBPPlugin.Controllers
{
    public class DashboardController : Controller
    {
        private IBackOfficeRepository backOfficeRepository;

        public DashboardController()
        {

        }

        public DashboardController(IBackOfficeRepository _backOfficeRepository)
        {
            this.backOfficeRepository = _backOfficeRepository;
        }

        public ActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        public ActionResult Dashboard()
        {
            if (FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {
                ResourceBundle resBundle = ResourceBundle.GetInstance();

                var instAllowed = FormAuthMemberShip.HelperService.InstallationsRoleAllowed("DASHBOARD_READ");
                //CURRENCy oCurrency = backOfficeRepository.GetCurrencies(PredicateBuilder.True<CURRENCy>().And(c => c.CUR_ISO_CODE == "EUR")).FirstOrDefault();
                string[] IsoCodes = { "EUR", "USD", "CAD", "GBP", "MXN" };
                CURRENCy oCurrency = backOfficeRepository.GetInstallations(PredicateBuilder.True<INSTALLATION>())
                                                         .Select(i => i.CURRENCy)
                                                         .Where(c => /*IsoCodes.Contains(c.CUR_ISO_CODE) &&*/ c.CUR_ISO_CODE == ConfigurationManager.AppSettings["ApplicationCurrencyISOCode"].ToString())
                                                         .Distinct()
                                                         .OrderBy(c => c.CUR_NAME)
                                                         .ToList()
                                                         .FirstOrDefault();
                List<int> oInsUnknown = new List<int>() { 0 };
                DashboardDataModel oModel = new DashboardDataModel()
                {
                    Installations = backOfficeRepository.GetInstallations(PredicateBuilder.True<INSTALLATION>())
                                                        .Where(i => instAllowed.Contains(Convert.ToInt32(i.INS_ID)))
                                                        .Select(i => new { id = "I" + i.INS_ID.ToString(), Name = i.INS_DESCRIPTION })
                                                        .ToList().AsQueryable<object>()
                                                        .Union(from i in oInsUnknown
                                    select new
                                    {
                                        id = "I" + i.ToString(),
                                        Name = resBundle.GetString("PBPPlugin", "Dashboard_UnknownInstallation", "Unknown"),
                                        hasChildren = false,
                                        @checked = true
                                    })
                                                        .ToArray<object>(),
                    DateGroup = DateGroupType.day,
                    DateGroupPattern = false,
                    DateFilter = DateFilterType.lastMonth,
                    CustomIniDate = DateTime.Now.AddDays(-7),
                    CustomEndDate = DateTime.Now,
                    CustomIniTime = "00:00",
                    CustomEndTime = "23:59",
                    CurrencyId = oCurrency.CUR_ID,
                    CurrencyIsoCode = oCurrency.CUR_ISO_CODE
                };
                                      

                oModel.InstallationsInit = "[";
                for (int i = 0; i < oModel.Installations.Length; i++)
                {
                    oModel.InstallationsInit += Newtonsoft.Json.JsonConvert.SerializeObject(((dynamic) oModel.Installations[i]).id);
                    if (i < oModel.Installations.Length - 1) oModel.InstallationsInit += ",";
                }
                oModel.InstallationsInit += "]";
                
                return View(oModel);
            }
            else
                //return RedirectToAction("BlankPage", "Home");
                return Redirect(FormAuthMemberShip.HelperService.AccessDeniedUrl());
        }

        public ActionResult Read_Today(string opeType, string filters)
        {
            List<TodayData> oRet = new List<TodayData>();
            if (FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
            {
                DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
                var data = ChartsRepository.TodayData(backOfficeRepository, opeType, oFilters.InstallationsIds);
                oRet.Add(data);
            }
            return Json(oRet);
        }

        public ActionResult Read_OperationsToday(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            IList<ChartDataItem> oData = new List<ChartDataItem>();
            if (FormAuthMemberShip.HelperService.RoleAllowed("DASHBOARD_READ"))
                oData = ChartsRepository.OperationsTodayData(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate);
            return Json(oData);
        }

        public ActionResult Read_Operations(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.OperationsDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Read_OperationsTotals(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.OperationsTotalsDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Read_OperationsAvg(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.OperationsAvgDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CurrencyId, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public ActionResult Read_OperationsAvgTotals(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.OperationsAvgTotalsDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CurrencyId, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public ActionResult Read_Recharges(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.RechargesDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CurrencyId, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public ActionResult Read_RechargesAvg(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.RechargesAvgDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CurrencyId, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public ActionResult Read_Inscriptions(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.InscriptionsDataViews(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime, oFilters.UserInstallation));
        }

        public ActionResult Read_InscriptionsAvg(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.InscriptionsAvgDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CurrencyId, oFilters.CustomIniTime, oFilters.CustomEndTime, oFilters.UserInstallation));
        }

        public ActionResult Read_InscriptionsPlatform(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.InscriptionsPlatformDataViews(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime, oFilters.UserInstallation));
        }

        public ActionResult Read_InscriptionsPlatformTotals(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.InscriptionsPlatformTotalsDataViews(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime, oFilters.UserInstallation), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Read_InscriptionsPlatformAcums(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.InscriptionsPlatformDataAcums(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime, oFilters.UserInstallation));
        }

        public ActionResult Read_UsersInstallations(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.UsersInstallationsData(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Read_UsersInstallationsFirstOperation(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.UsersInstallationsFirstOperationData(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime), JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Read_OperationsUser(string filters)
        {
            /*Newtonsoft.Json.JsonSerializerSettings _jsonSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Error,
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Unspecified
            };*/
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.OperationsUserDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public ActionResult Read_OperationsUserAll(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.OperationsUserAllDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public ActionResult Read_Invitations(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.InvitationsDataViews(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Read_InvitationsAcums(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.InvitationsDataAcums(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Read_RechargeCoupons(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.RechargeCouponsDataViews(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime), JsonRequestBehavior.AllowGet);
        }
        public ActionResult Read_RechargeCouponsAcums(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)integraMobile.Infrastructure.Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.RechargeCouponsDataAcums(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime), JsonRequestBehavior.AllowGet);
        }

        public JsonResult Read_TreeGroups(string id)
        {
            IQueryable nodes = null;

            if (string.IsNullOrEmpty(id))
            {
                ResourceBundle resBundle = ResourceBundle.GetInstance();
                nodes = new List<dynamic>() {
                    new 
                    {
                        id = "R",
                        Name = resBundle.GetString("PBPPlugin", "Dashboard_TreeTitle", "Installations and Groups"),
                        hasChildren = true,
                        @checked = true
                    }
                }.AsQueryable();

            }
            else
            {
                if (id.StartsWith("R"))
                {
                    List<int> oInsUnknown = new List<int>() { 0 };
                    ResourceBundle resBundle = ResourceBundle.GetInstance();

                    var instAllowed = FormAuthMemberShip.HelperService.InstallationsRoleAllowed("DASHBOARD_READ");
                    nodes = (from i in backOfficeRepository.GetInstallations(PredicateBuilder.True<INSTALLATION>().And(i => i.INS_ENABLED == 1 && instAllowed.Contains(Convert.ToInt32(i.INS_ID))))
                             orderby i.INS_DESCRIPTION
                             select new
                             {
                                 id = "I" + i.INS_ID.ToString(),
                                 Name = i.INS_DESCRIPTION,
                                 hasChildren = i.GROUPs.Any(),
                                 @checked = true
                             }).ToList().AsQueryable()
                             .Union(from i in oInsUnknown
                                    select new
                                    {
                                        id = "I" + i.ToString(),
                                        Name = resBundle.GetString("PBPPlugin", "Dashboard_UnknownInstallation", "Unknown"),
                                        hasChildren = false,
                                        @checked = true
                                    });
                    
                }
                else if (id.StartsWith("I"))
                {
                    nodes = (from g in backOfficeRepository.GetGroups(PredicateBuilder.True<GROUP>()
                                                                                      .And(g => g.GRP_INS_ID == decimal.Parse(id.Substring(1)))
                                                                                      .And(g => g.GROUPS_HIERARCHies.Where(gh => !gh.GRHI_GPR_ID_PARENT.HasValue &&
                                                                                                                                 gh.GRHI_INI_APPLY_DATE <= DateTime.Now &&
                                                                                                                                 gh.GRHI_END_APPLY_DATE >= DateTime.Now).Any()))
                             orderby g.GRP_DESCRIPTION
                             select new
                             {
                                 id = "G" + g.GRP_ID.ToString(),
                                 Name = g.GRP_DESCRIPTION,
                                 hasChildren = g.GROUPS_HIERARCHies1.Any()
                             });

                }
                else if (id.StartsWith("G"))
                {
                    nodes = (from g in backOfficeRepository.GetGroups(PredicateBuilder.True<GROUP>()
                                                                                      .And(g => g.GROUPS_HIERARCHies.Where(gh => gh.GRHI_GPR_ID_PARENT.HasValue &&
                                                                                                                           gh.GRHI_GPR_ID_PARENT == decimal.Parse(id.Substring(1)) &&
                                                                                                                           gh.GRHI_INI_APPLY_DATE <= DateTime.Now &&
                                                                                                                           gh.GRHI_END_APPLY_DATE >= DateTime.Now).Any()))
                             orderby g.GRP_DESCRIPTION
                             select new
                             {
                                 id = "G" + g.GRP_ID.ToString(),
                                 Name = g.GRP_DESCRIPTION,
                                 hasChildren = g.GROUPS_HIERARCHies1.Any()
                             });

                }
            }

            return Json(nodes, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDateGroupTypes()
        {
            ResourceBundle resBundle = ResourceBundle.GetInstance();
            var types = (from d in Enum.GetValues(typeof(DateGroupType)).Cast<DateGroupType>()
                         select new
                         {
                             id = (int)d,
                             Name = resBundle.GetString("PBPPlugin", "DateGroupType_" + (d).ToString())
                         })
                         .OrderBy(e => e.id)
                         .AsQueryable();
            return Json(types, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDateFilterTypes()
        {
            ResourceBundle resBundle = ResourceBundle.GetInstance();
            var types = (from d in Enum.GetValues(typeof(DateFilterType)).Cast<DateFilterType>()
                         select new
                         {
                             id = (int)d,
                             Name = resBundle.GetString("PBPPlugin", "DateFilterType_" + (d).ToString())
                         })
                         .OrderBy(e => e.id)
                         .AsQueryable();
            return Json(types, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCurrencies()
        {
            string[] IsoCodes = { "EUR", "USD", "CAD", "GBP", "MXN" };
            var currencies = backOfficeRepository.GetCountries(PredicateBuilder.True<COUNTRy>())
                                                 .Where(t => t.CURRENCy != null)
                                                 .Select(t => t.CURRENCy)
                                                 .Distinct()
                                                 .OrderBy(t => t.CUR_NAME)
                                                 .Select(t => new
                                                 {
                                                     id = t.CUR_ID,
                                                     Name = t.CUR_NAME,
                                                     IsoCode = t.CUR_ISO_CODE
                                                 })
                                                 .AsQueryable();
            /*var currencies = (from d in backOfficeRepository.GetInstallations(PredicateBuilder.True<INSTALLATION>()).Select(i => i.CURRENCy).Distinct() // backOfficeRepository.GetCurrencies()
                              where IsoCodes.Contains(d.CUR_ISO_CODE)
                              orderby d.CUR_NAME
                              select new
                              {
                                  id = d.CUR_ID,
                                  Name = d.CUR_NAME,
                                  IsoCode = d.CUR_ISO_CODE
                              })
                              .AsQueryable();*/
            
            return Json(currencies, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserInstallationTypes()
        {
            ResourceBundle resBundle = ResourceBundle.GetInstance();
            var types = (from d in Enum.GetValues(typeof(UserInstallationType)).Cast<UserInstallationType>()
                         select new
                         {
                             id = (int)d,
                             Name = resBundle.GetString("PBPPlugin", "UserInstallationType_" + (d).ToString(), (d).ToString())
                         })
                         .OrderBy(e => e.id)
                         .AsQueryable();
            return Json(types, JsonRequestBehavior.AllowGet);
        }

    }
}
