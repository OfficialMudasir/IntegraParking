using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Web.Routing;
using integraMobile.Infrastructure;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using kendoTest.Models;
using kendoTest.Properties;


namespace kendoTest.Controllers
{
    [HandleError]
    [NoCache]
    public class HomeController : Controller
    {

        private ICustomersRepository customersRepository;
        private IInfraestructureRepository infraestructureRepository;


        public HomeController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
        }

        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            Session["Culture"] = new CultureInfo(lang);
            return Redirect(returnUrl);
        }

        public ActionResult Index()
        {
            return RedirectToAction("LogOn", "Home");
        }

        public ActionResult LogOn()
        {
            Session["SUM_USER_DATA"] = null;
            ViewData["lang_for_gCond"] = Session["Culture"].ToString().Replace("-", "_");
            return View();
        }


        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string utcoffset, string returnUrl)
        {
            ViewData["lang_for_gCond"] = Session["Culture"].ToString().Replace("-", "_");
            if (ModelState.IsValid)
            {
                USER oUser=null;
                string strUsername = model.UserName;

                string strAutoPassword="";
                FormAuthMemberShip.MembershipService.GetPassword(strUsername, ref strAutoPassword );
                model.Password = strAutoPassword;

                if (FormAuthMemberShip.MembershipService.ValidateUser(ref strUsername, model.Password))
                {
                    FormAuthMemberShip.FormsService.SignIn(strUsername, model.RememberMe);
                   
                    if (customersRepository.GetUserData(ref oUser,strUsername))
                    {
                        int iUTCOffset = 0;
                        try
                        {
                            iUTCOffset = Convert.ToInt32(utcoffset);
                        }
                        catch
                        {
                            iUTCOffset = 0;
                        }


                        customersRepository.SetUserCultureLangAndUTCOffest(ref oUser, Session["Culture"].ToString(), iUTCOffset);
                        Session["USER_DATA"] = oUser;

                        return RedirectToAction("Operations", "Kendo");

                    }
                    else
                    {
                        ModelState.AddModelError("", Resources.Home_GetUserDataError);
                    }


                }
                else
                {
                    ModelState.AddModelError("", Resources.Home_BadUserNameOrPassword);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        public ActionResult gCond_es_ES()
        {
            Session["Culture"] = new CultureInfo("es-ES");
            return View();
        }

        public ActionResult gCond_en_US()
        {
            Session["Culture"] = new CultureInfo("en-US");
            return View();
        }

        public ActionResult gCond_fr_FR()
        {
            Session["Culture"] = new CultureInfo("fr-FR");
            return View();
        }

        [Authorize]
        public ActionResult LogOff()
        {
            Session["USER_DATA"] = null;
            FormAuthMemberShip.FormsService.SignOut();
            return RedirectToAction("LogOn", "Home");
        }

    }
}
