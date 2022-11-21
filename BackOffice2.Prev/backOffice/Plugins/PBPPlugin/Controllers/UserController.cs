using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

//using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Invoicing;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using backOffice.Infrastructure;
using backOffice.Models;
using backOffice.Helper;
using backOffice.Infrastructure.Security;

namespace PBPPlugin.Controllers
{
    public class UserController : Controller
    {
        private IBackOfficeRepository backOfficeRepository;
        private IInfraestructureRepository infrastructureRepository;

        public UserController()
        {

        }

        public UserController(IBackOfficeRepository _backOfficeRepository, IInfraestructureRepository _infrastructureRepository)
        {            
            this.backOfficeRepository = _backOfficeRepository;
            this.infrastructureRepository = _infrastructureRepository;
        }

        public ActionResult User_Disable(int userId)
        {
            bool bRet = false;
            string sErrorInfo = "";

            var resBundle = ResourceBundle.GetInstance();

            if (FormAuthMemberShip.HelperService.RoleAllowed("USERS_DISABLE"))
            {
                USER oUser = null;

                bRet = backOfficeRepository.SetUserEnabled(userId, false, out oUser);
                if (bRet)
                {
                    try
                    {
                        integraMobile.Infrastructure.FormAuthMemberShip.MembershipService.DeleteUser(oUser.USR_USERNAME);
                    }
                    catch (Exception ex)
                    {
                        if (integraMobile.Infrastructure.FormAuthMemberShip.MembershipService.UserExist(oUser.USR_USERNAME))
                        {
                            backOfficeRepository.SetUserEnabled(userId, true, out oUser);
                            bRet = false;
                        }
                    }
                }
                else
                {
                    sErrorInfo = resBundle.GetString("PBPPlugin", "User_Disable_Error");
                }
            }
            else
                sErrorInfo = resBundle.GetString("PBPPlugin", "UserController.UserDisable.AccessDenied");

            return Json(new { Result = bRet, ErrorInfo = sErrorInfo });
        }

    }
}
