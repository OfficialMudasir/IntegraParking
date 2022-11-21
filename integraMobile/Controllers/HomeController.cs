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
using integraMobile.Models;
using integraMobile.Web.Resources;
using System.Configuration;
using System.Threading;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Helper;
using System.Xml;
using Newtonsoft.Json;
using integraMobile.Response;
using integraMobile.ExternalWS;
using System.Collections.Specialized;


namespace integraMobile.Controllers
{
    [HandleError]
    [NoCache]
    public class HomeController : Controller
    {
        #region Constants
        private const string PATH_HOME = "Home/";
        private const string PATH_LOGON = "LogOn";
        #endregion

        #region Properties
        private ICustomersRepository customersRepository;
        private IInfraestructureRepository infraestructureRepository;
        private IGeograficAndTariffsRepository geograficAndTariffsRepository;
        protected static CLogWrapper m_Log;
        #endregion

        #region Constructor
        public HomeController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository, IGeograficAndTariffsRepository geograficAndTariffsRepository )
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
            this.geograficAndTariffsRepository = geograficAndTariffsRepository;
            m_Log = new CLogWrapper(typeof(PermitsController));
        }
        #endregion

        #region Public methods
        public ActionResult ChangeCulture(string lang, string returnUrl)
        {
            Session["ChangeCulture"] = true;
            Session["Culture"] = new CultureInfo(lang);
            integraMobile.Properties.Resources.Culture = (CultureInfo)Session["Culture"];
            //New Changes
            if (returnUrl.Contains("Appeal"))
            {
                TempData.Keep("AppealModelData");
                TempData.Keep("AppealTicketSearchData");
                TempData.Keep("AppealPersonalInfoData");
                TempData.Keep("AppealPITicketSearchData");
                TempData.Keep("AppealAllTicketData");
                var model = new Models.AppealReturnModel();
                if (returnUrl.Contains("Appeal/PersonalInformation") && TempData["AppealPersonalInfoData"] != null)
                {
                    var _modeldata = TempData["AppealPersonalInfoData"];
                    model = JsonConvert.DeserializeObject<Models.AppealReturnModel>(_modeldata.ToString());
                    TempData.Keep("AppealPersonalInfoData");
                   // return this.RedirectAndPost(returnUrl, model);
                }
            }
            // End

            return Redirect(returnUrl);
        }

        public ActionResult Index()
        {           
            return RedirectToAction("LogOn", "Home");
        }

        public ActionResult LogOn()
        {

            Session["SUM_USER_DATA_ID"] = null;
            Session["FinanOperator"] = null;
            ViewData["lang_for_gCond"] = Session["Culture"].ToString().Replace("-", "_");
            Session["RequestOpId"] = null;
            Session["RequestInstallationId"] = null;
            if (Session["URLReferer"] != null)
            {
                string strReferer = "";
                if (Session["NewUserEmail"] != null)
                {
                    strReferer = string.Format(Session["URLReferer"].ToString(), Thread.CurrentThread.CurrentCulture.Name) + "?UserName=" +
                        HttpUtility.UrlEncode(Session["NewUserEmail"].ToString());
                }
                else
                {
                    strReferer = string.Format(Session["URLReferer"].ToString(), Thread.CurrentThread.CurrentCulture.Name);
                }
                Session["URLReferer"] = null;
                Session["NewUserEmail"] = null;
                return Redirect(strReferer);
            }
            else
            {
                if (Session["NewUserEmail"] != null)
                {
                    LogOnModel model = new LogOnModel()
                    {
                        UserName = Session["NewUserEmail"].ToString()
                    };
                    Session["NewUserEmail"] = null;
                    return View(model);
                }
                else
                {
                    return View();
                }
            }
        }


        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string utcoffset, string returnUrl )
        {
            try
            {
                if (Session != null && (Session["USER_ID"] != null || Session["CurrentModel"] != null))
                {
                    FormAuthMemberShip.FormsService.SignOut();
                    Session["USER_ID"] = null;
                    Session["CurrentModel"] = null;
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("HomeController > LogOn > Session cleared before login"));
                }
                else 
                {
                    m_Log.LogMessage(LogLevels.logINFO, string.Format("HomeController > LogOn > Session was already null at LogOn"));
                }
            }
            catch (Exception ex)
            {
                m_Log.LogMessage(LogLevels.logERROR, string.Format("HomeController > LogOn > {0}", ex.Message));
            }

            if (string.IsNullOrEmpty(model.culture))
            {
                ViewData["lang_for_gCond"] = Session["Culture"].ToString().Replace("-", "_");
            }
            else
            {
                ViewData["lang_for_gCond"] = model.culture.Replace("-", "_");
                CultureInfo ci = new CultureInfo(model.culture);
                model.urlreferer=model.urlreferer.Replace(model.culture, "{0}");
                
                if (ci == null)
                {
                    //Sets default culture to english invariant
                    string langName = "en-US";
                    ci = new CultureInfo(langName);
                }
                Session["Culture"] = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                integraMobile.Properties.Resources.Culture = ci;
                Session["InstallationID"] = null;

            }
          
            if (ModelState.IsValid)
            {
                USER oUser=null;
                string strUsername = model.UserName;

                if (FormAuthMemberShip.MembershipService.ValidateUser(ref strUsername, model.Password))
                {
                    FormAuthMemberShip.FormsService.SignIn(strUsername, model.RememberMe);

                    if (customersRepository.GetUserData(ref oUser, strUsername))
                    {
                        decimal dDefaultSourceApp = geograficAndTariffsRepository.GetDefaultSourceApp();

                        if (oUser.USR_SIGNUP_SOAPP_ID == dDefaultSourceApp)
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

                            string strSuscriptionType = "";
                            RefundBalanceType eRefundBalType = RefundBalanceType.rbtAmount;
                            customersRepository.GetUserPossibleSuscriptionTypes(ref oUser, infraestructureRepository, out strSuscriptionType, out eRefundBalType);

                            customersRepository.SetUserCultureLangAndUTCOffest(ref oUser, Session["Culture"].ToString(), iUTCOffset);
                            Session["USER_ID"] = oUser.USR_ID;
                            Session["OVERWRITE_CARD"] = false;
                            Session["SuscriptionTypeConf"] = strSuscriptionType;

                            if (!string.IsNullOrEmpty(model.urlreferer))
                            {
                                Session["URLReferer"] = model.urlreferer;
                            }


                            if (!string.IsNullOrEmpty(model.finan_operator))
                            {
                                Session["FinanOperator"] = Convert.ToDecimal(model.finan_operator);
                            }

                            if (oUser.USR_SUSCRIPTION_TYPE != null)
                            {
                                return RedirectToAction("SelectPayMethod", "Account");
                            }
                            else
                            {
                                return RedirectToAction("SelectSuscriptionType", "Account");
                            }
                        }
                        else
                        {
                            VerifyLoginExistsServiceExternal(model);

                        }

                    }
                    else
                    {
                        VerifyLoginExistsServiceExternal(model);
                    }

                }
                else
                {
                    VerifyLoginExistsServiceExternal(model);
                }
                
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult mainmenu(int? iSourceApp)
        {

            decimal dSourceApp = infraestructureRepository.GetDefaultSourceApp();
            SOURCE_APPS_CONFIGURATION oConf = null;

            if (iSourceApp.HasValue)
            {
                oConf = infraestructureRepository.GetSourceAppsConfiguration(Convert.ToInt32(iSourceApp.Value));
                dSourceApp = Convert.ToInt32(iSourceApp.Value);
            }

            if (oConf == null)
            {
                oConf = infraestructureRepository.GetSourceAppsConfiguration(dSourceApp);
            }

            string ua = Request.UserAgent;

            try
            {
                Logger_AddLogMessage(string.Format("mainmenu | Agent={0}", ua), LogLevels.logINFO);            


                if ((ua.Contains("iPad")) ||
                    (ua.Contains("iPod")) ||
                    (ua.Contains("iPhone")))
                {
                    string strURL = oConf.SOAPC_IOS_APPSTORE_URL;
                    Logger_AddLogMessage(string.Format("Redirecting to: {0}", strURL), LogLevels.logINFO);
                    return Redirect(strURL);
                }
                else if (ua.Contains("Android"))
                {
                    string strURL = string.Format("{0}://mainmenu", oConf.SOAPC_ANDROID_APPSCHEMA);
                    Logger_AddLogMessage(string.Format("Redirecting to: {0}", strURL), LogLevels.logINFO);
                    ViewData["AndroidGooglePlayURL"] = oConf.SOAPC_ANDROID_GOOGLE_PLAY_URL;
                    ViewData["AndroidDeepLink"] = strURL;
                    return View("mainmenuAndroid");
                }
                          
            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "Excepcion Signup", LogLevels.logERROR);
            }

            return RedirectToAction("LogOn", "Home");
        }

        public ActionResult gCond_es_ES()
        {
            Session["Culture"] = new CultureInfo("es-ES");
            return View();
        }

        public ActionResult gCond_ca_ES()
        {
            Session["Culture"] = new CultureInfo("ca-ES");
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

        public ActionResult gCond_fr_CA()
        {
            Session["Culture"] = new CultureInfo("fr-CA");
            return View();
        }
        #endregion

        #region Private Methods
        private static void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }

        private static void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }

        private integraMobileWS.integraMobileWS Start(COUNTRIES_REDIRECTION countriesRedirection)
        {

            integraMobileWS.integraMobileWS oIntegraMobileWS = null;
            if (countriesRedirection == null)
            {
                oIntegraMobileWS = new integraMobileWS.integraMobileWS();
                oIntegraMobileWS.Url = ConfigurationManager.AppSettings["WSUrl"];
                oIntegraMobileWS.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["WSTimeout"]);

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["WSHttpUser"]))
                {
                    oIntegraMobileWS.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["WSHttpUser"], ConfigurationManager.AppSettings["WSHttpPassword"]);
                }
            }
            else
            {
                oIntegraMobileWS = new integraMobileWS.integraMobileWS();
                int iwsurl = countriesRedirection.COURE_COUNTRY_REDIRECTION_WS_URL.IndexOf('/', countriesRedirection.COURE_COUNTRY_REDIRECTION_WS_URL.Length - 1);
                string ws_url = string.Empty;
                if (iwsurl != -1)
                {
                    ws_url = countriesRedirection.COURE_COUNTRY_REDIRECTION_WS_URL.Remove(countriesRedirection.COURE_COUNTRY_REDIRECTION_WS_URL.Length - 1, 1);
                }
                else
                {
                    ws_url = countriesRedirection.COURE_COUNTRY_REDIRECTION_WS_URL;
                }
                oIntegraMobileWS.Timeout = Convert.ToInt32(ConfigurationManager.AppSettings["WSTimeout"]);

                if (!string.IsNullOrEmpty(countriesRedirection.COURE_COUNTRY_REDIRECTION_HTTP_USER) && !string.IsNullOrEmpty(countriesRedirection.COURE_COUNTRY_REDIRECTION_PASSWORD))
                {
                    oIntegraMobileWS.Url = ws_url;
                    oIntegraMobileWS.Credentials = new System.Net.NetworkCredential(countriesRedirection.COURE_COUNTRY_REDIRECTION_HTTP_USER, countriesRedirection.COURE_COUNTRY_REDIRECTION_PASSWORD);
                }
            }
            return oIntegraMobileWS;
        }

        private Int32 GetCulture()
        {
            if (Session["Culture"] == null)
            {
                Session["Culture"] = "en-US";
            }
            decimal dLang = infraestructureRepository.GetLanguage(Session["Culture"].ToString());

            return Convert.ToInt32(dLang);
        }

        private void VerifyLoginExistsServiceExternal(LogOnModel model)
        {
            VerifyLoginExistsResponse oVerifyLoginExistsResponse = null;
            List<COUNTRIES_REDIRECTION> oCountriesRedirectionList = infraestructureRepository.GetCountriesRedirections();
            int iLan = GetCulture();
            if (oCountriesRedirectionList != null)
            {
                foreach (COUNTRIES_REDIRECTION countriesRedirection in oCountriesRedirectionList)
                {
                    integraMobileWS.integraMobileWS oIntegraMobileWS = Start(countriesRedirection);
                    string sInJson = Tools.CreatingJsonInVerifyLoginExistsRequest(model.UserName, model.Password);
                    XmlDocument docVerif = (XmlDocument)JsonConvert.DeserializeXmlNode(sInJson);
                    m_Log.LogMessage(LogLevels.logINFO, String.Format(Tools.PrettyXml(docVerif.InnerXml)));

                    string jsonOut = oIntegraMobileWS.VerifyLoginExistsJSON(sInJson);
                    jsonOut = Tools.RemeveTagIparkOut(jsonOut);
                    oVerifyLoginExistsResponse = JsonConvert.DeserializeObject<VerifyLoginExistsResponse>(jsonOut);

                    if (oVerifyLoginExistsResponse.r == ResultType.Result_OK)
                    {
                        NameValueCollection data = new NameValueCollection();
                        data.Add("UserName", model.UserName);
                        data.Add("Password", model.Password);
                        Tools.RedirectWithData(data, string.Format(@"{0}{1}{2}", countriesRedirection.COURE_COUNTRY_REDIRECTION_WEBAPP_URL, PATH_HOME, PATH_LOGON));
                        oVerifyLoginExistsResponse = null;
                        break;
                    }
                    else if (oVerifyLoginExistsResponse.r == ResultType.Result_Error_User_Is_Not_Activated)
                    {
                        break;
                    }
                    else if (oVerifyLoginExistsResponse.r == ResultType.Result_Error_InvalidAuthentication)
                    {
                        break;
                    }
                }
                if (oVerifyLoginExistsResponse != null && (oVerifyLoginExistsResponse.r != ResultType.Result_OK))
                {
                    this.ModelState.AddModelError(oVerifyLoginExistsResponse.r.ToString(), Tools.Message(oVerifyLoginExistsResponse.r));
                }
            }
            else
            {
                this.ModelState.AddModelError(ResultType.Result_Error_User_Not_Logged.ToString(), Tools.Message(ResultType.Result_Error_User_Not_Logged));
            }
        }

        #endregion
    }
}
