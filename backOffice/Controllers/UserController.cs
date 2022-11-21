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
    public class UserController : Controller
    {
        private IBackOfficeRepository backOfficeRepository;

        public UserController(IBackOfficeRepository _backOfficeRepository)
        {            
            this.backOfficeRepository = _backOfficeRepository;            
        }

        #region Actions

        public ActionResult Index()
        {
            return RedirectToAction("Users");
        }

        public ActionResult Users()
        {
            if (Helper.Helper.MenuOptionEnabled("User#Users"))
            {
                backOffice.Helper.Helper.PopulateCountries(ViewData, backOfficeRepository);
                backOffice.Helper.Helper.PopulateCurrencies(ViewData, backOfficeRepository);            
                backOffice.Helper.Helper.PopulatePaymentMeanTypes(ViewData);
                backOffice.Helper.Helper.PopulatePaymentMeanSubTypes(ViewData);
                backOffice.Helper.Helper.PopulatePaymentSuscryptionTypes(ViewData, true);
                backOffice.Helper.Helper.PopulateBooleans(ViewData);
                var predicate = PredicateBuilder.False<USER>();
                return View(UserDataModel.List(backOfficeRepository, predicate, false));
            }
            else
                return RedirectToAction("BlankPage", "Home");

        }

        public ActionResult Users_Read([DataSourceRequest] DataSourceRequest request)
        {
            var predicate = PredicateBuilder.True<USER>();
            if (Request.Params["gridInitialized"] == "false") predicate = predicate.And(o => false);
            return Json(UserDataModel.List(backOfficeRepository, predicate, false).ToDataSourceResult(request));
        }

        public ActionResult User_Disable(int userId)
        {
            bool bRet = false;
            string sErrorInfo = "";

            USER oUser = null;

            bRet = backOfficeRepository.SetUserEnabled(userId, false, out oUser);
            if (bRet)
            {
                try
                {
                    FormAuthMemberShip.MembershipService.DeleteUser(oUser.USR_USERNAME);
                }
                catch (Exception ex)
                {
                    if (FormAuthMemberShip.MembershipService.UserExist(oUser.USR_USERNAME))
                    {
                        backOfficeRepository.SetUserEnabled(userId, true, out oUser);
                        bRet = false;
                    }
                }
            }
            else
            {
                sErrorInfo = Resources.User_Disable_Error;
            }

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo });
        }

        public ActionResult UserSecurityOperations()
        {
            if (Helper.Helper.MenuOptionEnabled("User#UserSecurityOperations"))
            {

                backOffice.Helper.Helper.PopulateSecurityOperationType(ViewData);
                backOffice.Helper.Helper.PopulateSecurityOperationStatus(ViewData);
                backOffice.Helper.Helper.PopulateCountries(ViewData, backOfficeRepository);
                var predicate = PredicateBuilder.False<USERS_SECURITY_OPERATION>();
                return View(UserSecurityOperationDataModel.List(backOfficeRepository, predicate, false));
            }
            else
                return RedirectToAction("BlankPage", "Home");
        }

        public ActionResult UserSecurityOperations_Read([DataSourceRequest] DataSourceRequest request)
        {
            var predicate = PredicateBuilder.True<USERS_SECURITY_OPERATION>();
            if (Request.Params["gridInitialized"] == "false") predicate = predicate.And(o => false);
            return Json(UserSecurityOperationDataModel.List(backOfficeRepository, predicate, false).ToDataSourceResult(request));
        }

        #endregion

        #region Methods

        #endregion

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonNetResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

    }
}
