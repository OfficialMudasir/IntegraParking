using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using backOffice.Properties;
using backOffice.Models;

namespace backOffice.Controllers
{
    public class CustomerInscriptionController : Controller
    {
        private IBackOfficeRepository backOfficeRepository;

        public CustomerInscriptionController(IBackOfficeRepository _backOfficeRepository)
        {            
            this.backOfficeRepository = _backOfficeRepository;            
        }

        #region Actions

        public ActionResult Index()
        {
            return RedirectToAction("CustomerInscriptions");
        }

        public ActionResult CustomerInscriptions()
        {
            if (Helper.Helper.MenuOptionEnabled("CustomerInscription#CustomerInscriptions"))
            {
                backOffice.Helper.Helper.PopulateCountries(ViewData, backOfficeRepository);
                var predicate = PredicateBuilder.False<CUSTOMER_INSCRIPTION>();
                return View(CustomerInscriptionDataModel.List(backOfficeRepository, predicate, false));
            }
            else
                return RedirectToAction("BlankPage", "Home");

        }

        public ActionResult CustomerInscriptions_Read([DataSourceRequest] DataSourceRequest request)
        {
            var predicate = PredicateBuilder.True<CUSTOMER_INSCRIPTION>();
            if (Request.Params["gridInitialized"] == "false") predicate = predicate.And(o => false);
            return Json(CustomerInscriptionDataModel.List(backOfficeRepository, predicate, false).ToDataSourceResult(request));
        }

        #endregion

    }
}
