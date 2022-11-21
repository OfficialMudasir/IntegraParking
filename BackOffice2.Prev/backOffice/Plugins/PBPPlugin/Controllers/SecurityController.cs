using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Concrete;
//using integraMobile.Domain.Helper;
using backOffice.Infrastructure;
using backOffice.Infrastructure.Maintenances;
using backOffice.Infrastructure.Security;
using SecurityPlugin.Security;

namespace PBPPlugin.Controllers
{
    public class SecurityController : Controller
    {
        private IBackOfficeRepository backOfficeRepository;
        private IInfraestructureRepository infrastructureRepository;

        public SecurityController()
        {

        }

        public SecurityController(IBackOfficeRepository _backOfficeRepository, IInfraestructureRepository _infrastructureRepository)
        {            
            this.backOfficeRepository = _backOfficeRepository;
            this.infrastructureRepository = _infrastructureRepository;
        }

        #region Actions

        public ActionResult Init()
        {
            bool bRet = false;
            string sInfo = "";

            try
            {                
                List<string> oAllRoles = FormAuthMemberShip.MembershipService.GetAllRoles().ToList();

                Group oGroup = null;
                User oUser = null;

                //oUser = new User("eysadmin");
                //bRet = oUser.Delete();
                //oUser = new User("eysadminIBZ");
                //bRet = oUser.Delete();

                if (Group.CreateGroup("Admins", "Admins", null, true, oAllRoles, out oGroup))
                {
                    bRet = SecurityPlugin.Security.User.CreateUser("eysadmin", "eysadmin", "", "", "eysadmin@integraparking.com", "", "eysadmin", "es-ES", oGroup, true, oAllRoles, out oUser);

                    var oMadRoles = oAllRoles.Where(role => role.StartsWith("2#")).ToList();
                    bRet = bRet && SecurityPlugin.Security.User.CreateUser("eysadminMAD", "eysadminMAD", "", "", "eysadminMAD@integraparking.com", "", "eysadminMAD", "es-ES", oGroup, true, oMadRoles, out oUser);

                    var oIbzRoles = oAllRoles.Where(role => role.StartsWith("1#")).ToList();
                    bRet = bRet && SecurityPlugin.Security.User.CreateUser("eysadminIBZ", "eysadminIBZ", "", "", "eysadminIBZ@integraparking.com", "", "eysadminIBZ", "es-ES", oGroup, true, oIbzRoles, out oUser);

                    //oGroup.AllowedRoles.Add("4#DAYEXCEPTIONS_ADMIN");
                    //oGroup.AllowedRoles.Remove("4#DAYEXCEPTIONS_ADMIN");
                    //oGroup.Save();

                    /*Group oGroupChild = null;
                    var oChildRoles = oAllRoles.Where(role => role.StartsWith("1#") || role.StartsWith("5#") || role.StartsWith("2#")).ToList();
                    if (Group.CreateGroup("Admins2", "Admins2", oGroup, true, oChildRoles, out oGroupChild))
                    {
                        SecurityPlugin.Security.User.CreateUser("eysatest1", "eysatest1", "", "", "eysatest1@integraparking.com", "", "eysatest1", "es-ES", oGroupChild, true, oAllRoles, out oUser);
                    }*/                    

                    //bRet = true;
                }

            }
            catch (Exception ex)
            {
                bRet = false;
                sInfo = ex.ToString();
            }

            return Json(new { Ret = bRet, Info = sInfo }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
