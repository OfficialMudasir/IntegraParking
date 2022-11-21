using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Configuration;
using integraMobile.Infrastructure;

namespace backOffice.Controllers
{
    [HandleError]
    //[NoCache]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            //return View();
            string sController = "Operation";
            string sAction = "Index";
            string sOptions = ConfigurationManager.AppSettings["MenuOptionsEnabled"];
            if (!string.IsNullOrEmpty(sOptions))
            {
                foreach (string sOption in sOptions.Split(';')) {
                    if (!string.IsNullOrEmpty(sOption))
                    {
                        sController = sOption.Split('#')[0];
                        sAction = sOption.Split('#')[1];
                        break;
                    }
                }
            }
            return RedirectToAction(sAction, sController);
        }

        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            Session["Culture"] = new CultureInfo(lang);
            return Redirect(returnUrl);
        }

        public ActionResult BlankPage()
        {
            return View();
        }

        public ActionResult SetTimeZoneOffset(string redirectUrl)
        {
            return View((object)redirectUrl);
        }
        
        public ActionResult SetTimeZoneOffsetAction(string utcoffset, string redirectUrl)
        {
            Session["utcOffset"] = utcoffset;
            return Redirect(redirectUrl);
        }

    }
}
