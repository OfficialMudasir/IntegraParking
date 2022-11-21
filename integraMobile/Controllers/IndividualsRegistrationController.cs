using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Globalization;
using System.Configuration;
using System.Threading;
using System.Web.Security;
using System.Security.Cryptography;
using System.IO;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Models;
using integraMobile.Web.Resources;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;


namespace integraMobile.Controllers
{
    [HandleError]
    [NoCache]
    public class IndividualsRegistrationController : Controller
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(CreditCallController));

        string[] UserDeviceLangs = { "es-ES", "en-US", "fr-FR", "ca-ES", "es-MX", "eu-ES", "it-IT" };

        private ICustomersRepository customersRepository;
        private IInfraestructureRepository infraestructureRepository;

        private const long BIG_PRIME_NUMBER = 2147483647;
        private const long BIG_PRIME_NUMBER2 = 624159837;


        public IndividualsRegistrationController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
        }

        
        public ActionResult Index()
        {
            Session["CustomerInscriptionModelStep1"] = null;
            ViewData["SelectedMainPhoneNumberPrefix"] = "";
            ViewData["SelectedAlternativePhoneNumberPrefix"] = "";
            ViewData["CountriesOptionList"] = infraestructureRepository.Countries.ToArray();
            ViewData["UsernameEqualsEmail"] = ConfigurationManager.AppSettings["UsernameEqualsToEmail"];
            return View("Step1");

        }


        public ActionResult Step1()
        {
            if (Session["CustomerInscriptionModelStep1"] == null)
                return Index();
            else
            {
                CustomerInscriptionModelStep1 model = (CustomerInscriptionModelStep1)Session["CustomerInscriptionModelStep1"];
                ViewData["CountriesOptionList"] = infraestructureRepository.Countries.ToArray();
                ViewData["SelectedMainPhoneNumberPrefix"] = model.MainPhoneNumberPrefix;
                ViewData["SelectedAlternativePhoneNumberPrefix"] = model.AlternativePhoneNumberPrefix;
                ViewData["UsernameEqualsEmail"] = ConfigurationManager.AppSettings["UsernameEqualsToEmail"];


                return View(model);
            }
        }


        public ActionResult signup(string country, string lang, string bin)
        {

            string ua = Request.UserAgent;
                
            try
            {                             
                Logger_AddLogMessage(string.Format("Signup?country={0}&lang={1} | Agent={2}",country, lang, ua), LogLevels.logINFO);


                decimal dSourceApp = infraestructureRepository.GetDefaultSourceApp();


                if (!string.IsNullOrEmpty(bin))
                {
                    string json = DecryptCryptResult(bin, ConfigurationManager.AppSettings["CryptKey"]);
                    dynamic oResponse = JsonConvert.DeserializeObject(json);


                    if (oResponse["appid"] != null)
                    {
                        dSourceApp = Convert.ToDecimal(oResponse["appid"].ToString());
                        Logger_AddLogMessage(string.Format("Source APP ID {0}", infraestructureRepository.GetSourceAppCode(dSourceApp)), LogLevels.logINFO);

                    }
                }

                SOURCE_APPS_CONFIGURATION oConf = infraestructureRepository.GetSourceAppsConfiguration(dSourceApp);

                string strCulture = "en-US";

                if (Session["Culture"] == null)
                {
                    if (!string.IsNullOrEmpty(lang))
                    {
                        try
                        {
                            int iLangIndex = Convert.ToInt32(lang);
                            if (iLangIndex <= UserDeviceLangs.Length)
                            {
                                strCulture = UserDeviceLangs[iLangIndex - 1];
                            }
                        }
                        catch
                        { }

                    }
                }
                else
                    strCulture = Session["Culture"].ToString();
                
                CultureInfo ci = new CultureInfo(strCulture);
                Session["Culture"] = ci;

                SetCulture();
               

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
                    string strURL = string.Format("{3}://signup?country={0}&lang={1}&bin={2}", country, lang, bin, oConf.SOAPC_ANDROID_APPSCHEMA);
                    Logger_AddLogMessage(string.Format("Redirecting to: {0}", strURL), LogLevels.logINFO);
                    ViewData["AndroidGooglePlayURL"] = oConf.SOAPC_ANDROID_GOOGLE_PLAY_URL;
                    ViewData["AndroidDeepLink"] = strURL;
                    return View("signupAndroid");
                }                
                /*else
                {
                    Logger_AddLogMessage(string.Format("User Agent not supported"), LogLevels.logERROR);
                }
                

                ViewData["culture"] = strCulture;*/
                string pbin = bin;     

                integraMobile.Response.ResponseSignUpStep1 oResponseSignUpStep1 = new integraMobile.Response.ResponseSignUpStep1()
                {
                    bin= pbin,
                    ccode = country,
                    r = integraMobile.ExternalWS.ResultType.Result_OK,                   
                };

                Session["ResponseSignUpStep1"] = oResponseSignUpStep1;

            }
            catch (Exception e)
            {
                Logger_AddLogException(e,"Excepcion Signup",LogLevels.logERROR);
            }

            return RedirectToAction("Step2", "Registration");
        }



        public ActionResult signuprikips(string country, string lang, string bin)
        {

            string ua = Request.UserAgent;

            try
            {
                Logger_AddLogMessage(string.Format("Signuprikips?country={0}&lang={1} | Agent={2}", country, lang, ua), LogLevels.logINFO);


                decimal dSourceApp = infraestructureRepository.GetDefaultSourceApp();


                if (!string.IsNullOrEmpty(bin))
                {
                    string json = DecryptCryptResult(bin, ConfigurationManager.AppSettings["CryptKey"]);
                    dynamic oResponse = JsonConvert.DeserializeObject(json);


                    if (oResponse["appid"] != null)
                    {
                        dSourceApp = Convert.ToDecimal(oResponse["appid"].ToString());
                        Logger_AddLogMessage(string.Format("Source APP ID {0}", infraestructureRepository.GetSourceAppCode(dSourceApp)), LogLevels.logINFO);

                    }
                }

                SOURCE_APPS_CONFIGURATION oConf = infraestructureRepository.GetSourceAppsConfiguration(dSourceApp);

                string strCulture = "en-US";

                if (Session["Culture"] == null)
                {
                    if (!string.IsNullOrEmpty(lang))
                    {
                        try
                        {
                            int iLangIndex = Convert.ToInt32(lang);
                            if (iLangIndex <= UserDeviceLangs.Length)
                            {
                                strCulture = UserDeviceLangs[iLangIndex - 1];
                            }
                        }
                        catch
                        { }

                    }
                }
                else
                    strCulture = Session["Culture"].ToString();

                CultureInfo ci = new CultureInfo(strCulture);
                Session["Culture"] = ci;

                SetCulture();


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
                    string strURL = string.Format("{3}://signuprikips?country={0}&lang={1}&bin={2}", country, lang, bin, oConf.SOAPC_ANDROID_APPSCHEMA);
                    Logger_AddLogMessage(string.Format("Redirecting to: {0}", strURL), LogLevels.logINFO);
                    ViewData["AndroidGooglePlayURL"] = oConf.SOAPC_ANDROID_GOOGLE_PLAY_URL;
                    ViewData["AndroidDeepLink"] = strURL;
                    return View("signupAndroid");
                }
                /*else
                {
                    Logger_AddLogMessage(string.Format("User Agent not supported"), LogLevels.logERROR);
                }
                

                ViewData["culture"] = strCulture;*/
                string pbin = bin;

                integraMobile.Response.ResponseSignUpStep1 oResponseSignUpStep1 = new integraMobile.Response.ResponseSignUpStep1()
                {
                    bin = pbin,
                    ccode = country,
                    r = integraMobile.ExternalWS.ResultType.Result_OK,
                };

                Session["ResponseSignUpStep1"] = oResponseSignUpStep1;

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "Excepcion Signup", LogLevels.logERROR);
            }

            return RedirectToAction("Step2", "Registration");
        }




        public ActionResult Redirect(int? iSourceApp)
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
            
            SetCulture();

            if (oConf != null)
            {

                string ua = Request.UserAgent;

                try
                {
                    Logger_AddLogMessage(string.Format("Redirect| Agent={0} SourceApp={1}", ua, infraestructureRepository.GetSourceAppCode(dSourceApp)), LogLevels.logINFO);


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
                        string strURL = oConf.SOAPC_ANDROID_GOOGLE_PLAY_URL;
                        Logger_AddLogMessage(string.Format("Redirecting to: {0}", strURL), LogLevels.logINFO);
                        return Redirect(strURL);
                    }
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "Excepcion Redirect", LogLevels.logERROR);
                }
            }

            string strURLFinal = ConfigurationManager.AppSettings["WebBaseURL"].ToString();
            Logger_AddLogMessage(string.Format("Redirecting to: {0}", strURLFinal), LogLevels.logINFO);
            return Redirect(strURLFinal);
        }


        [HttpPost]
        [Validation.ValidateCaptcha()]
        public ActionResult Step1(CustomerInscriptionModelStep1 model, bool captchaIsValid)
        {
            SetCulture();

            Session["CustomerInscriptionModelStep1"] = model;
            ViewData["CountriesOptionList"] = infraestructureRepository.Countries.ToArray();
            ViewData["SelectedMainPhoneNumberPrefix"] = model.MainPhoneNumberPrefix;
            ViewData["SelectedAlternativePhoneNumberPrefix"] = model.AlternativePhoneNumberPrefix;
            ViewData["UsernameEqualsEmail"] = ConfigurationManager.AppSettings["UsernameEqualsToEmail"];

            if (!captchaIsValid)
            {
                ModelState.AddModelError("recaptcha", ResourceExtension.GetLiteral("ErrorsMsg_CaptchaNotValid"));
            }            

            if (ModelState.IsValid)
            {

                CUSTOMER_INSCRIPTION custInsc =GetDomainCustomerInscriptionFromStep1ViewModel(model);

                if (custInsc == null)
                {
                    ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                    return View(model);
                }


                if (customersRepository.ExistMainTelephone(Convert.ToInt32(custInsc.CUSINS_MAIN_TEL_COUNTRY), custInsc.CUSINS_MAIN_TEL))
                {
                    ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorTelephoneAlreadyExist"));
                    return View(model);
                }


                if (customersRepository.ExistEmail( custInsc.CUSINS_EMAIL))
                {
                    ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorEmailAlreadyExist"));
                    return View(model);
                }


                if (!customersRepository.AddCustomerInscription(ref custInsc))
                {
                    ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                    return View(model);
                }

                if (!SendEmailAndSMS(custInsc))
                {
                    ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorSendingActivation"));
                    return View(model);
                }

                customersRepository.UpdateActivationRetries(ref custInsc);
                Session["CustomerInscriptionID"] = custInsc.CUSINS_ID;

                return (RedirectToAction("Step1End"));
               
            }
            else
            {
                return View(model);
            }
   
           
        }
       
        
        
        public ActionResult Step1End()
        {
            SetCulture();

            Session["CustomerInscriptionModelStep1"] = null;
            CUSTOMER_INSCRIPTION custInsc = GetCustomerInscriptionFromSession();
            if (custInsc != null)
            {
                ViewData["email"] = custInsc.CUSINS_EMAIL;
                string strMaskedPhone = "";

                for (int i = 0; i < custInsc.CUSINS_MAIN_TEL.Length; i++)
                {
                    if ((i == 0) || (i == custInsc.CUSINS_MAIN_TEL.Length - 2) || (i == custInsc.CUSINS_MAIN_TEL.Length - 1))
                    {
                        strMaskedPhone += custInsc.CUSINS_MAIN_TEL[i];
                    }
                    else
                    {
                        strMaskedPhone += "X";
                    }
                }


                ViewData["maskedTelephone"] = "+" + custInsc.COUNTRy.COU_TEL_PREFIX + " " + strMaskedPhone;

                return View("Step1End");
            }
            else
            {
                return (RedirectToAction("Step1"));
            }
            
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Step1End(string submitButton)
        {
            SetCulture();

            CUSTOMER_INSCRIPTION custInsc = GetCustomerInscriptionFromSession();

            if (custInsc != null)
            {

                
                if (SendEmailAndSMS(custInsc))
                {
                    customersRepository.UpdateActivationRetries(ref custInsc);
                    ViewData["ActivationRetries"] = string.Format(ResourceExtension.GetLiteral("CustomerInscriptionModel_EmailAndSMSReSent"), custInsc.CUSINS_ACTIVATION_RETRIES);
                    Session["CustomerInscriptionID"] =  custInsc.CUSINS_ID;
                }

                ViewData["email"] = custInsc.CUSINS_EMAIL;
                string strMaskedPhone = "";

                for (int i = 0; i < custInsc.CUSINS_MAIN_TEL.Length; i++)
                {
                    if ((i == 0) || (i == custInsc.CUSINS_MAIN_TEL.Length - 2) || (i == custInsc.CUSINS_MAIN_TEL.Length - 1))
                    {
                        strMaskedPhone += custInsc.CUSINS_MAIN_TEL[i];
                    }
                    else
                    {
                        strMaskedPhone += "X";
                    }
                }


                ViewData["maskedTelephone"] = "+" + custInsc.COUNTRy.COU_TEL_PREFIX + " " + strMaskedPhone;

                return View("Step1End");
                
            }
            else
                return (RedirectToAction("Step1"));


          
        }

        public ActionResult Step2()
        {
            SetCulture();

            Session["SUM_USER_DATA_ID"] = null;
            ViewData["CodeExpired"] = false;
            ViewData["CodeAlreadyUsed"] = false;
            Session["CustomerInscriptionModelStep1"] = null;
            int iNumSecondsTimeoutActivationSMS = Convert.ToInt32(ConfigurationManager.AppSettings["NumSecondsTimeoutActivationSMS"]);
            ViewData["NumMinutesTimeoutActivationSMS"] = iNumSecondsTimeoutActivationSMS / 60;
            if (Session["ValidCustomerInscription"] == null)
            {
                Session["ValidCustomerInscription"] = false;
            }
            else
            {
                if ((bool)Session["ValidCustomerInscription"])
                {
                    return (RedirectToAction("Step3"));
                }

            }

            if (Session["CustomerInscriptionID"] == null)
            {
                string urlCode = Request.QueryString["code"];
               
                CUSTOMER_INSCRIPTION custInsc = customersRepository.GetCustomerInscriptionData(urlCode);


                if (custInsc != null)
                {
                    string culture = custInsc.CUISINS_CULTURE;
                    CultureInfo ci = new CultureInfo(culture);
                    Session["Culture"] = ci;
                    Thread.CurrentThread.CurrentUICulture = ci;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                    integraMobile.Properties.Resources.Culture = ci;


                    Session["CustomerInscriptionID"] =  custInsc.CUSINS_ID;

                    if (customersRepository.IsCustomerInscriptionExpired(custInsc))
                    {
                        ModelState.AddModelError("CodeExpired", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeExpired"));
                        ViewData["CodeExpired"] = true;
                    }
                    else if (customersRepository.IsCustomerInscriptionAlreadyUsed(custInsc))
                    {
                        ModelState.AddModelError("CodeAlreadyUsed", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeAlreadyUsed"));
                        ViewData["CodeAlreadyUsed"] = true;
                    }


                    ViewData["EndSufixMainPhone"] = custInsc.CUSINS_MAIN_TEL.Substring(custInsc.CUSINS_MAIN_TEL.Length - 2, 2)
                                                    .PadLeft(custInsc.CUSINS_MAIN_TEL.Length , '*');

                }
                else
                {
                    ModelState.AddModelError("ConfirmationCodeError", ResourceExtension.GetLiteral("ErrMsg_ActivationURLIncorrect"));
                }
            }
            else
            {
                CUSTOMER_INSCRIPTION custInsc = GetCustomerInscriptionFromSession();
                ViewData["EndSufixMainPhone"] = custInsc.CUSINS_MAIN_TEL.Substring(custInsc.CUSINS_MAIN_TEL.Length - 2, 2)
                                                .PadLeft(custInsc.CUSINS_MAIN_TEL.Length , '*'); ;

                if (customersRepository.IsCustomerInscriptionExpired(custInsc))
                {
                    ModelState.AddModelError("CodeExpired", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeExpired"));
                    ViewData["CodeExpired"] = true;
                }
                else if (customersRepository.IsCustomerInscriptionAlreadyUsed(custInsc))
                {
                    ModelState.AddModelError("CodeAlreadyUsed", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeAlreadyUsed"));
                    ViewData["CodeAlreadyUsed"] = true;
                }

                

            }

            return View("Step2");

        }



        [HttpPost]//[AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Step2(string type,string confirmationcode)
        {
            SetCulture();

            CUSTOMER_INSCRIPTION custInsc = GetCustomerInscriptionFromSession();
            ViewData["EndSufixMainPhone"] = custInsc.CUSINS_MAIN_TEL.Substring(custInsc.CUSINS_MAIN_TEL.Length - 2, 2)
                                            .PadLeft(custInsc.CUSINS_MAIN_TEL.Length, '*'); ;
            int iNumSecondsTimeoutActivationSMS = Convert.ToInt32(ConfigurationManager.AppSettings["NumSecondsTimeoutActivationSMS"]);
            ViewData["NumMinutesTimeoutActivationSMS"] = iNumSecondsTimeoutActivationSMS / 60;
            Session["ValidCustomerInscription"] = false;

            if (!customersRepository.IsCustomerInscriptionExpired(custInsc))
            {
                if (!customersRepository.IsCustomerInscriptionAlreadyUsed(custInsc))
                {


                    if (type == "confirmcode")
                    {
                        if (String.IsNullOrEmpty(confirmationcode))
                        {
                            ModelState.AddModelError("ConfirmationCodeError", ResourceExtension.GetLiteral("ErrMsg_ValidationCodeIsRequired"));
                        }
                        else
                        {


                            if (custInsc.CUSINS_ACTIVATION_CODE != confirmationcode)
                            {
                                ModelState.AddModelError("ConfirmationCodeError", ResourceExtension.GetLiteral("ErrMsg_ValidationCodeIsIncorrect"));
                            }
                            else
                            {
                                Session["ValidCustomerInscription"] = true;
                                return (RedirectToAction("Step3"));
                            }

                        }
                    }
                    else if (type == "resendsms")
                    {

                        if (ReSendSMS(custInsc))
                        {
                            customersRepository.UpdateActivationRetries(ref custInsc);
                            ViewData["ActivationRetries"] = string.Format(ResourceExtension.GetLiteral("CustomerInscriptionModel_SMSReSent"), custInsc.CUSINS_ACTIVATION_RETRIES);
                            Session["CustomerInscriptionID"] =  custInsc.CUSINS_ID;

                        }

                    }
                }
                else
                {
                    ModelState.AddModelError("CodeAlreadyUsed", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeAlreadyUsed"));
                    ViewData["CodeAlreadyUsed"] = true;

                }
            }
            else
            {
                ModelState.AddModelError("CodeExpired", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeExpired"));
                ViewData["CodeExpired"] = true;
            }

            return View("Step2");
            

        }

        public ActionResult Step3()
        {
            SetCulture();

            if ((Session["ValidCustomerInscription"]==null)||
                (Session["CustomerInscriptionID"]==null)||
                (!((bool)Session["ValidCustomerInscription"])))
            {

                Session["ValidCustomerInscription"]=null;
                Session["CustomerInscriptionID"]=null;
                Session["CustomerInscriptionModelStep3"] = null;
                Session["CustomerInscriptionModelStep4"] = null;
                return (RedirectToAction("Index", "Home"));

            }
           

            if (Session["CustomerInscriptionModelStep3"] == null)
            {
                Session["CustomerInscriptionModelStep3"] = null;
                ViewData["SelectedCountry"] = "";
                ViewData["CountriesOptionList"] = infraestructureRepository.Countries.ToArray();
                return View("Step3");
            }
            else
            {
                CustomerInscriptionModelStep3 model = (CustomerInscriptionModelStep3)Session["CustomerInscriptionModelStep3"];
                ViewData["CountriesOptionList"] = infraestructureRepository.Countries.ToArray();
                ViewData["SelectedCountry"] = model.Country;
                return View(model);
            }    
        }



        [HttpPost]
        public ActionResult Step3(CustomerInscriptionModelStep3 model)
        {
            SetCulture();


            if ((Session["ValidCustomerInscription"] == null) ||
                (Session["CustomerInscriptionID"] == null) ||
                (!((bool)Session["ValidCustomerInscription"])))
            {

                Session["ValidCustomerInscription"] = null;
                Session["CustomerInscriptionID"] = null;
                Session["CustomerInscriptionModelStep3"] = null;
                Session["CustomerInscriptionModelStep4"] = null;
                return (RedirectToAction("Index", "Home"));

            }

            Session["CustomerInscriptionModelStep3"] = model;
            ViewData["CountriesOptionList"] = infraestructureRepository.Countries.ToArray();
            ViewData["SelectedCountry"] = model.Country;


            if (ModelState.IsValid)
            {
                model.Valid = true;
                Session["CustomerInscriptionModelStep3"] = model;
                return (RedirectToAction("Step4"));

            }
            else
            {
                return View(model);
            }


        }


        public ActionResult Step4()
        {
            SetCulture();

            ViewData["lang_for_gCond"] = Session["Culture"].ToString().Replace("-", "_");
            ViewData["UsernameEqualsEmail"] = ConfigurationManager.AppSettings["UsernameEqualsToEmail"];
            bool bUsernameEqualsEmail = (ViewData["UsernameEqualsEmail"].ToString() == "1");


            if ((Session["ValidCustomerInscription"] == null) ||
                (Session["CustomerInscriptionID"] == null) ||
                (Session["CustomerInscriptionModelStep3"] == null)||
                (!((bool)Session["ValidCustomerInscription"]))||
                (!((CustomerInscriptionModelStep3)Session["CustomerInscriptionModelStep3"]).Valid))
            {

                Session["ValidCustomerInscription"] = null;
                Session["CustomerInscriptionID"] = null;
                Session["CustomerInscriptionModelStep3"] = null;
                Session["CustomerInscriptionModelStep4"] = null;
                return (RedirectToAction("Index", "Home"));

            }


            CustomerInscriptionModelStep4 model = null;

            if (Session["CustomerInscriptionModelStep4"] == null)
            {
                Session["CustomerInscriptionModelStep4"] = null;
                model = new CustomerInscriptionModelStep4();

            }
            else
            {               
                model = (CustomerInscriptionModelStep4)Session["CustomerInscriptionModelStep4"];
            }


            CUSTOMER_INSCRIPTION custInsc = GetCustomerInscriptionFromSession();
            model.Email = custInsc.CUSINS_EMAIL;    
            if (bUsernameEqualsEmail)
            {
                model.Username = "XXXXXXXXXXX";
            }
            return View(model);

        }



        [HttpPost]
        public ActionResult Step4(CustomerInscriptionModelStep4 model)
        {
            SetCulture();

            ViewData["UsernameEqualsEmail"] = ConfigurationManager.AppSettings["UsernameEqualsToEmail"];
            bool bUsernameEqualsEmail = (ViewData["UsernameEqualsEmail"].ToString() == "1");

            try
            {
                ViewData["lang_for_gCond"] = Session["Culture"].ToString().Replace("-", "_");

              

                if ((Session["ValidCustomerInscription"] == null) ||
                    (Session["CustomerInscriptionID"] == null) ||
                    (Session["CustomerInscriptionModelStep3"] == null) ||
                    (!((bool)Session["ValidCustomerInscription"])) ||
                    (!((CustomerInscriptionModelStep3)Session["CustomerInscriptionModelStep3"]).Valid))
                {

                    Session["ValidCustomerInscription"] = null;
                    Session["CustomerInscriptionID"] = null;
                    Session["CustomerInscriptionModelStep3"] = null;
                    Session["CustomerInscriptionModelStep4"] = null;
                    return (RedirectToAction("Index", "Home"));

                }

                CUSTOMER_INSCRIPTION custInsc = GetCustomerInscriptionFromSession();

                Session["CustomerInscriptionModelStep4"] = model;

                if (ModelState.IsValid)
                {

                    if (bUsernameEqualsEmail)
                    {
                        model.Username = model.Email;
                    }

                    if (customersRepository.ExistUsername(model.Username))
                    {
                        ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrMsg_UsernameAlreadyExist"));
                        if (bUsernameEqualsEmail)
                        {
                            model.Username = "XXXXXXXXXXX";
                        }
                        return View(model);
                    }
                    else
                    {
                        model.Valid = true;
                        Session["CustomerInscriptionModelStep4"] = model;
                        /*return (RedirectToAction("Step5"));*/


                        if (model.ConfirmServiceCondictions)
                        {
                            if (!customersRepository.IsCustomerInscriptionAlreadyUsed(custInsc))
                            {

                                USER oUser = null;
                                if (GetDomainCustomerAndUserFromViewsModels(custInsc,
                                    (CustomerInscriptionModelStep3)Session["CustomerInscriptionModelStep3"],
                                    (CustomerInscriptionModelStep4)Session["CustomerInscriptionModelStep4"],
                                    ref oUser))
                                {

                                    string password = ((CustomerInscriptionModelStep4)Session["CustomerInscriptionModelStep4"]).Password;
                                    FormAuthMemberShip.MembershipService.DeleteUser(oUser.USR_USERNAME);


                                    MembershipCreateStatus createStatus = FormAuthMemberShip.MembershipService.CreateUser(oUser.USR_USERNAME, password, oUser.USR_EMAIL);
                                    if (createStatus != MembershipCreateStatus.Success)
                                    {
                                        FormAuthMemberShip.MembershipService.DeleteUser(oUser.USR_USERNAME);
                                        createStatus = FormAuthMemberShip.MembershipService.CreateUser(oUser.USR_USERNAME, password, oUser.USR_EMAIL);
                                    }



                                    if (createStatus != MembershipCreateStatus.Success)
                                    {
                                        FormAuthMemberShip.MembershipService.DeleteUser(oUser.USR_USERNAME);
                                        ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                                        if (bUsernameEqualsEmail)
                                        {
                                            model.Username = "XXXXXXXXXXX";
                                        }
                                        return View(model);
                                    }
                                    else
                                    {
                                        if (!customersRepository.AddUser(ref oUser,
                                                            ((decimal)Session["CustomerInscriptionID"])))
                                        {
                                            FormAuthMemberShip.MembershipService.DeleteUser(oUser.USR_USERNAME);
                                            ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                                            if (bUsernameEqualsEmail)
                                            {
                                                model.Username = "XXXXXXXXXXX";
                                            }
                                            return View(model);
                                        }
                                        else
                                        {

                                            Session["ValidCustomerInscription"] = null;
                                            Session["CustomerInscriptionID"] = null;
                                            Session["CustomerInscriptionModelStep3"] = null;
                                            Session["CustomerInscriptionModelStep4"] = null;
                                            Session["SUM_USER_DATA_ID"] = oUser.USR_ID;


                                            long lSenderId = infraestructureRepository.SendEmailTo(oUser.USR_EMAIL, ResourceExtension.GetLiteral("CustomerInscriptionModel_SummaryWelcomeEmail_Subject"),
                                                string.Format(GetLiteralByUserCountry("CustomerInscriptionModel_SummaryWelcomeEmail_Body", ref oUser), getBaseUrl(Request),
                                                oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1, oUser.USR_USERNAME, GetEmailFooter(ref oUser)),null);

                                            if (lSenderId > 0)
                                            {
                                                if (customersRepository.InsertUserEmail(ref oUser, oUser.USR_EMAIL,
                                                    ResourceExtension.GetLiteral("CustomerInscriptionModel_SummaryWelcomeEmail_Subject"), string.Format(GetLiteralByUserCountry("CustomerInscriptionModel_SummaryWelcomeEmail_Body", ref oUser),
                                                    getBaseUrl(Request),
                                                    oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1,
                                                    oUser.USR_USERNAME,
                                                    GetEmailFooter(ref oUser)), lSenderId))
                                                {
                                                    Session["SUM_USER_DATA_ID"] = oUser.USR_ID;
                                                }

                                            }

                                            customersRepository.AssignPendingInvitationsToAccept(ref oUser);

                                            return (RedirectToAction("Summary"));

                                        }
                                    }
                                }
                                else
                                {
                                    ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrorsMsg_ErrorAddindInformationToDB"));
                                    if (bUsernameEqualsEmail)
                                    {
                                        model.Username = "XXXXXXXXXXX";
                                    }
                                    return View(model);

                                }

                            }
                            else
                            {
                                ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrMsg_ActivationCodeAlreadyUsed"));
                                if (bUsernameEqualsEmail)
                                {
                                    model.Username = "XXXXXXXXXXX";
                                }
                                return View(model);
                            }

                        }
                        else
                        {
                            ModelState.AddModelError("customersDomainError", ResourceExtension.GetLiteral("ErrMsg_UseConditionsMustBeAccepted"));
                            if (bUsernameEqualsEmail)
                            {
                                model.Username = "XXXXXXXXXXX";
                            }
                            return View(model);
                        }
                    }
                }
                else
                {
                    if (bUsernameEqualsEmail)
                    {
                        model.Username = "XXXXXXXXXXX";
                    }
                    return View(model);
                }

            }
            catch (Exception e)
            {
                ModelState.AddModelError("customersDomainError", e.Message);
                if (bUsernameEqualsEmail)
                {
                    model.Username = "XXXXXXXXXXX";
                }                    
                return View(model);


            }

        }

        public ActionResult gCond_es_ES()
        {
            return View();
        }

        public ActionResult gCond_ca_ES()
        {
            return View();
        }

        public ActionResult gCond_en_US()
        {
            return View();
        }

        public ActionResult gCond_fr_FR()
        {
            return View();
        }

        public ActionResult gCond_fr_CA()
        {
            return View();
        }


        public ActionResult Summary()
        {
            SetCulture();

            if (Session["SUM_USER_DATA_ID"] == null) 
            {
                return (RedirectToAction("Index", "Home"));
            }

            USER oUser=GetUserFromSession();

            ViewData["email"] = oUser.USR_EMAIL;
            ViewData["UserNameAndSurname"] = oUser.CUSTOMER.CUS_FIRST_NAME + " " + oUser.CUSTOMER.CUS_SURNAME1;

            return View();
        }



        private CUSTOMER_INSCRIPTION GetDomainCustomerInscriptionFromStep1ViewModel(CustomerInscriptionModelStep1 model)
        {
            CUSTOMER_INSCRIPTION custInsc = null;

            try
            {
                custInsc = new CUSTOMER_INSCRIPTION
                {
                    CUSINS_NAME = model.Name,
                    CUSINS_SURNAME1 = model.Surname1,
                    CUSINS_SURNAME2 = model.Surname2,
                    CUSINS_DOC_ID = model.DocId,
                    CUSINS_MAIN_TEL_COUNTRY = Convert.ToInt32(model.MainPhoneNumberPrefix),
                    CUSINS_MAIN_TEL = model.MainPhoneNumber,
                    CUSINS_SECUND_TEL_COUNTRY = Convert.ToInt32(model.AlternativePhoneNumberPrefix),
                    CUSINS_SECUND_TEL = model.AlternativePhoneNumber,
                    CUSINS_EMAIL = model.Email,
                    CUISINS_CULTURE = ((CultureInfo)Session["Culture"]).Name
                };
            }
            catch
            {
                custInsc=null;

            }

            return custInsc;
        }


        private bool  GetDomainCustomerAndUserFromViewsModels(CUSTOMER_INSCRIPTION custInsc,
                                                              CustomerInscriptionModelStep3 models3, 
                                                              CustomerInscriptionModelStep4 models4,
                                                              ref USER oUser)
        {
            bool bRes = true;
            
            oUser = null;

            try
            {

                    oUser=new USER
                    {

                        CUSTOMER = new CUSTOMER
                        {
                            CUS_TYPE = (int)CustomerType.Individual,
                            CUS_COU_ID = Convert.ToDecimal(models3.Country),
                            CUS_DOC_ID = custInsc.CUSINS_DOC_ID,
                            CUS_DOC_ID_TYPE = 0,//Undefined for now
                            CUS_FIRST_NAME = custInsc.CUSINS_NAME,
                            CUS_SURNAME1 = custInsc.CUSINS_SURNAME1,
                            CUS_SURNAME2 = custInsc.CUSINS_SURNAME2,
                            CUS_STREET = models3.StreetName,
                            CUS_STREE_NUMBER = Convert.ToInt32(models3.StreetNumber),
                            CUS_LEVEL_NUM = (models3.LevelInStreetNumber!=null && 
                                            models3.LevelInStreetNumber.Length > 0) ? Convert.ToInt32(models3.LevelInStreetNumber) : (int?)null,
                            CUS_DOOR = models3.DoorInStreetNumber ,
                            CUS_LETTER = models3.LetterInStreetNumber,
                            CUS_STAIR = models3.StairInStreetNumber,
                            CUS_CITY = models3.City,
                            CUS_STATE = models3.State,
                            CUS_ZIPCODE = models3.ZipCode,
                            CUS_ENABLED = 1,
                            CUS_INSERT_UTC_DATE = DateTime.UtcNow,
                            CUS_NAME="",
                        },

                        USR_COU_ID = Convert.ToDecimal(models3.Country),
                        USR_EMAIL = custInsc.CUSINS_EMAIL,
                        USR_MAIN_TEL_COUNTRY = custInsc.CUSINS_MAIN_TEL_COUNTRY,
                        USR_MAIN_TEL = custInsc.CUSINS_MAIN_TEL,
                        USR_SECUND_TEL_COUNTRY = custInsc.CUSINS_SECUND_TEL_COUNTRY,
                        USR_SECUND_TEL = custInsc.CUSINS_SECUND_TEL,
                        USR_USERNAME = models4.Username  ,
                        USR_BALANCE = 0,
                        USR_CUR_ID = infraestructureRepository.GetCountryCurrency(Convert.ToInt32(models3.Country)),
                        USR_CULTURE_LANG = custInsc.CUISINS_CULTURE,
                        USR_ENABLED = 1,
                        USR_INSERT_UTC_DATE = DateTime.UtcNow,
                        USR_PAYMETH = (int)PaymentMeanTypeStatus.pmsWithoutPaymentMean,
                        USR_SIGNUP_OS =  (int)MobileOS.Web,
                        USR_REFUND_BALANCE_TYPE = (int)RefundBalanceType.rbtAmount,
                    };
                 

                    oUser.USER_PLATEs.Add(new USER_PLATE
                                            {
                                                USRP_PLATE=models4.Plate,
                                                USRP_IS_DEFAULT=1,
                                                USRP_ENABLED = 1
                                            });
    
               
            }
            catch(Exception)
            {
                bRes = false;
               
                oUser = null;

            }

            return bRes;
        }

        private bool SendEmailAndSMS(CUSTOMER_INSCRIPTION custInsc)
        {
            bool bRes = true;
            try
            {
                string requrl = "";
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebBaseURL"]))
                {
                    requrl = Request.Url.ToString();
                }
                else
                {
                    requrl = ConfigurationManager.AppSettings["WebBaseURL"] + "/IndividualsRegistration/";
                }

                string url = requrl.Substring(0, requrl.ToString().LastIndexOf('/') + 1) + "Step2";
                string urlWithParam = url + "?code=" + custInsc.CUSINS_URL_PARAMETER;
                string strEmailSubject=ResourceExtension.GetLiteral("Activation_EmailHeader");
                string strEmailBody = string.Format(ResourceExtension.GetLiteral("Activation_EmailBody"),urlWithParam,url);
                string strSMS = string.Format(ResourceExtension.GetLiteral("Activation_SMS"), custInsc.CUSINS_ACTIVATION_CODE);

                long lSenderId = infraestructureRepository.SendEmailTo(custInsc.CUSINS_EMAIL, strEmailSubject, strEmailBody, null);

                if (lSenderId>0)
                {
                    string strCompleteTelephone="";
                    customersRepository.InsertCustomerEmail(custInsc, custInsc.CUSINS_EMAIL, strEmailSubject, strEmailBody, lSenderId);
                    lSenderId = infraestructureRepository.SendSMSTo(Convert.ToInt32(custInsc.CUSINS_MAIN_TEL_COUNTRY), custInsc.CUSINS_MAIN_TEL, strSMS, null, ref strCompleteTelephone);

                    if (lSenderId > 0)
                    {
                        customersRepository.InsertCustomerSMS(custInsc, strCompleteTelephone, strSMS, lSenderId);
                    }
                 
                }


            }
            catch
            {
                bRes = false;

            }

            return bRes;
        }


        private bool ReSendSMS(CUSTOMER_INSCRIPTION custInsc)
        {
            bool bRes = true;
            try
            {
                string strSMS = string.Format(ResourceExtension.GetLiteral("Activation_SMS"), custInsc.CUSINS_ACTIVATION_CODE);
                string strCompleteTelephone = "";

                long lSenderId = infraestructureRepository.SendSMSTo(Convert.ToInt32(custInsc.CUSINS_MAIN_TEL_COUNTRY), custInsc.CUSINS_MAIN_TEL, strSMS, null, ref strCompleteTelephone);

                if (lSenderId > 0)
                {
                    customersRepository.InsertCustomerSMS(custInsc, strCompleteTelephone, strSMS, lSenderId);
                }
            }
            catch
            {
                bRes = false;

            }

            return bRes;
        }

        private string getBaseUrl(HttpRequestBase request)
        {
            StringBuilder url = new StringBuilder();
            url.Append(request.Url.Scheme);
            url.Append("://");
            url.Append(request.Url.Host);
            if (request.Url.Port != 80)
            {
                url.Append(":");
                url.Append(request.Url.Port);
            }
            url.Append(request.ApplicationPath);
            if (url[url.Length - 1] != '/')
            {
                url.Append("/");
            }
            return url.ToString();
        }


        public USER GetUserFromSession()
        {
            USER oUser = null;
            try
            {
                if (Session["SUM_USER_DATA_ID"] != null)
                {
                    decimal dUserId = (decimal)Session["SUM_USER_DATA_ID"];
                    if (!customersRepository.GetUserDataById(ref oUser, dUserId))
                    {
                        oUser = null;

                    }

                }

            }
            catch
            {
                oUser = null;
            }

            return oUser;

        }


        public CUSTOMER_INSCRIPTION GetCustomerInscriptionFromSession()
        {
            CUSTOMER_INSCRIPTION ocustInsc = null;
            try
            {
                if (Session["CustomerInscriptionID"] != null)
                {
                    decimal dcuscInstId = (decimal)Session["CustomerInscriptionID"];
                    if (!customersRepository.GetCustomerInscription(ref ocustInsc, dcuscInstId))
                    {
                        ocustInsc = null;

                    }

                }

            }
            catch
            {
                ocustInsc = null;
            }

            return ocustInsc;

        }

        private string GetEmailFooter(ref USER oUser)
        {
            string strFooter = "";

            try
            {
                strFooter = ResourceExtension.GetLiteral(string.Format("footer_CUR_{0}_{1}", oUser.CURRENCy.CUR_ISO_CODE, oUser.COUNTRy.COU_CODE));
                if (string.IsNullOrEmpty(strFooter))
                {
                    strFooter = ResourceExtension.GetLiteral(string.Format("footer_CUR_{0}", oUser.CURRENCy.CUR_ISO_CODE));
                }
            }
            catch
            {

            }

            return strFooter;
        }

        private string GetLiteralByUserCountry(string strLiteralName, ref USER oUser)
        {
            string strLiteral = "";

            try
            {
                strLiteral = ResourceExtension.GetLiteral(string.Format("{0}_COU_{1}", strLiteralName, oUser.COUNTRy.COU_CODE));
                if (string.IsNullOrEmpty(strLiteral))
                {
                    strLiteral = ResourceExtension.GetLiteral(strLiteralName);
                }

            }
            catch
            {

            }

            return strLiteral;
        }


        protected void SetCulture()
        {

            if (Session["Culture"] == null)
            {
                Session["Culture"] = new CultureInfo("en-US");
            }

            integraMobile.Properties.Resources.Culture = (CultureInfo)Session["Culture"];
            Thread.CurrentThread.CurrentUICulture = (CultureInfo)Session["Culture"];
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(((CultureInfo)Session["Culture"]).Name);
            integraMobile.Properties.Resources.Culture = (CultureInfo)Session["Culture"];

        }

        private string DecryptCryptResult(string strHexByteArray, string strHashSeed)
        {
            string strRes = "";
            try
            {

                byte[] _normKey = null;

                int iKeyLength = 32;

                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _normKey = new byte[iKeyLength];
                int iSum = 0;

                for (int i = 0; i < iKeyLength; i++)
                {
                    if (i < keyBytes.Length)
                    {
                        iSum += keyBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _normKey[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER) % (Byte.MaxValue + 1));

                }


                byte[] _iv = null;

                int iIVLength = 16;

                byte[] ivBytes = System.Text.Encoding.UTF8.GetBytes(strHashSeed);
                _iv = new byte[iIVLength];
                iSum = 0;

                for (int i = 0; i < iIVLength; i++)
                {
                    if (i < ivBytes.Length)
                    {
                        iSum += ivBytes[i];
                    }
                    else
                    {
                        iSum += i;
                    }
                    _iv[i] = Convert.ToByte((iSum * BIG_PRIME_NUMBER2) % (Byte.MaxValue + 1));

                }

                strRes = DecryptStringFromBytes_Aes(StringToByteArray(strHexByteArray), _normKey, _iv);



            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "DecryptCryptResult::Exception", LogLevels.logERROR);

            }
            return strRes;
        }


        static string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        private static void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }

        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:X2}", b);
            return hex.ToString();
        }

        private static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private static void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }

    }
}
