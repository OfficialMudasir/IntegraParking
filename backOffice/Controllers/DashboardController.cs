using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using backOffice.Properties;
using backOffice.Models;
using backOffice.Models.Dashboard;

namespace backOffice.Controllers
{
    public class DashboardController : Controller
    {
        private IBackOfficeRepository backOfficeRepository;

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
            if (Helper.Helper.MenuOptionEnabled("Dashboard#Dashboard"))
            {
                CURRENCy oCurrency = backOfficeRepository.GetCurrencies(PredicateBuilder.True<CURRENCy>().And(c => c.CUR_ISO_CODE == "EUR")).FirstOrDefault();
                DashboardDataModel oModel = new DashboardDataModel()
                {
                    Installations = backOfficeRepository.GetInstallations(PredicateBuilder.True<INSTALLATION>()).Select(i => new { id = "I" + i.INS_ID.ToString(), Name = i.INS_DESCRIPTION }).ToArray<object>(),
                    DateGroup = DateGroupType.day,
                    DateGroupPattern = false,
                    DateFilter = DateFilterType.currentYear,
                    CustomIniDate = DateTime.Now.AddDays(-7),
                    CustomEndDate = DateTime.Now,
                    CustomIniTime = "00:00",
                    CustomEndTime = "23:59",
                    CurrencyId = oCurrency.CUR_ID,
                    CurrencyIsoCode = oCurrency.CUR_ISO_CODE
                };                
                return View(oModel);
            }
            else
                return RedirectToAction("BlankPage", "Home");            
        }

        public ActionResult Read_Today(string opeType, string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            var data = ChartsRepository.TodayData(backOfficeRepository, opeType, oFilters.InstallationsIds);
            List<TodayData> oRet = new List<TodayData>();
            oRet.Add(data);
            return Json(oRet);
        }

        public ActionResult Read_OperationsToday(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.OperationsTodayData(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate));
        }

        public ActionResult Read_Operations(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel) Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.OperationsDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public ActionResult Read_OperationsTotals(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.OperationsTotalsDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public ActionResult Read_OperationsAvg(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.OperationsAvgDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CurrencyId, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public ActionResult Read_OperationsAvgTotals(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.OperationsAvgTotalsDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CurrencyId, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public ActionResult Read_Recharges(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.RechargesDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CurrencyId, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public ActionResult Read_RechargesAvg(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.RechargesAvgDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CurrencyId, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public ActionResult Read_Inscriptions(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.InscriptionsDataViews(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public ActionResult Read_InscriptionsAvg(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.InscriptionsAvgDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CurrencyId, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public ActionResult Read_OperationsUser(string filters)
        {
            /*Newtonsoft.Json.JsonSerializerSettings _jsonSettings = new Newtonsoft.Json.JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Error,
                DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Unspecified
            };*/
            DashboardDataModel oFilters = (DashboardDataModel)Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.OperationsUserDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public ActionResult Read_OperationsUserAll(string filters)
        {
            DashboardDataModel oFilters = (DashboardDataModel)Conversions.JsonDeserializeFromString(filters, typeof(DashboardDataModel));
            return Json(ChartsRepository.OperationsUserAllDataViews2(backOfficeRepository, oFilters.InstallationsIds, oFilters.DateGroup, oFilters.DateGroupPattern, oFilters.DateFilter, oFilters.CustomIniDate, oFilters.CustomEndDate, oFilters.CustomIniTime, oFilters.CustomEndTime));
        }

        public JsonResult Read_TreeGroups(string id)
        {
            IQueryable nodes = null;

            if (string.IsNullOrEmpty(id))
            {
                nodes = (from i in backOfficeRepository.GetInstallations(PredicateBuilder.True<INSTALLATION>().And(i => i.INS_ENABLED == 1))
                         orderby i.INS_DESCRIPTION
                         select new
                         {
                             id = "I" + i.INS_ID.ToString(),
                             Name = i.INS_DESCRIPTION,
                             hasChildren = i.GROUPs.Any(),
                             @checked = true
                         });
            }
            else
            {
                if (id.StartsWith("I"))
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
            var types = (from d in Enum.GetValues(typeof(DateGroupType)).Cast<DateGroupType>()
                         select new 
                         {
                             id = (int)d,
                             Name = Resources.ResourceManager.GetString("DateGroupType_" + (d).ToString())
                         })
                         .OrderBy(e => e.id)
                         .AsQueryable();
            return Json(types, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDateFilterTypes()
        {
            var types = (from d in Enum.GetValues(typeof(DateFilterType)).Cast<DateFilterType>()
                         select new
                         {
                             id = (int)d,
                             Name = Resources.ResourceManager.GetString("DateFilterType_" + (d).ToString())
                         })
                         .OrderBy(e => e.id)
                         .AsQueryable();
            return Json(types, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCurrencies()
        {
            string[] IsoCodes = { "EUR", "USD", "CAD", "GBP" };
            var currencies = (from d in backOfficeRepository.GetCurrencies()
                              where IsoCodes.Contains(d.CUR_ISO_CODE)
                              orderby d.CUR_NAME
                              select new
                              {
                                  id = d.CUR_ID,
                                  Name = d.CUR_NAME,
                                  IsoCode = d.CUR_ISO_CODE
                              })
                              .AsQueryable();
            return Json(currencies, JsonRequestBehavior.AllowGet);
        }

    }
}
