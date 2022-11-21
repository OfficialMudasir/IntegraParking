using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Concrete;
using integraMobile.ExternalWS;
using integraMobile.Helper;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Models;
using integraMobile.Properties;
using integraMobile.Request;
using integraMobile.Response;
using Microsoft.Web.Mvc.Controls;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Globalization;
using System.Threading;
using integraMobile.Web.Resources;

namespace integraMobile.Controllers
{
    [HandleError]
    [NoCache]
    public class RegistrationController : Controller
    {
        #region Constants
        //string[] UserDeviceLangs = { "es-ES", "en-US", "fr-FR", "ca-ES", "es-MX", "eu-ES", "it-IT" };
        List<decimal> LIST_COUNTRIES = new List<decimal> { 12, 39, 51, 99, 135, 162, 198 };
        #endregion

        #region Properties
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(RegistrationController));
        private ICustomersRepository customersRepository;
        private IInfraestructureRepository infraestructureRepository;
        private IGeograficAndTariffsRepository geograficRepository;
       


        #endregion

        #region Constructor
        public RegistrationController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository, IGeograficAndTariffsRepository geograficRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;
            this.geograficRepository = geograficRepository;
            
        }
        #endregion

        #region Step1
        public ActionResult Step1(string culture, string urlreferer, string couid)
        {

            if (!string.IsNullOrEmpty(culture))
            {
                CultureInfo ci = new CultureInfo(culture);

                if (ci == null)
                {
                    //Sets default culture to english invariant
                    string langName = "en-US";
                    ci = new CultureInfo(langName);
                }
                Session["Culture"] = ci;


                SetCulture();


                urlreferer = urlreferer.Replace(culture, "{0}");
                if (!string.IsNullOrEmpty(couid))
                {
                    Session["CouIdRegistration"] = couid;
                }

                if (!string.IsNullOrEmpty(urlreferer))
                {
                    Session["URLReferer"] = urlreferer;
                }

                return RedirectToAction("Step1", "Registration");

            }
            else
            {
                if (!string.IsNullOrEmpty(couid))
                {
                    Session["CouIdRegistration"] = couid;
                }
                RegistrationModelSignUpStep1 model = new RegistrationModelSignUpStep1();
                return View(model);
            }
        }

        [HttpPost]
        public ActionResult Step1(RegistrationModelSignUpStep1 model)
        {
            SetCulture();

            Session["RegistrationModelStep1"] = model;
            if (ModelState.IsValid)
            {
                Int32 iLang = GetCulture();
                String strAuthHash = String.Empty;
                if (!string.IsNullOrEmpty((string)Session["CouIdRegistration"]))
                {
                    model.ccode = (string)Session["CouIdRegistration"];
                }

                SignUpStep1Request oSignUpStep1Request = new SignUpStep1Request();
                oSignUpStep1Request = oSignUpStep1Request.getRequest(model, iLang, Tools.CONST_OSID);
                string sInJson = Tools.ToJsonRequest(oSignUpStep1Request);

                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(sInJson);
                m_Log.LogMessage(LogLevels.logINFO, String.Format(Tools.PrettyXml(doc.InnerXml)));

                try
                {
                    List<COUNTRIES_REDIRECTION> oCountriesRedirectList = infraestructureRepository.GetCountriesRedirections();
                    string strOut = string.Empty;
                    ResponseSignUpStep1 oResponseSignUpStep1 = null;
                    bool userExist = false;

                    //Verificar si el usuario existe en algun servidor externo
                    COUNTRIES_REDIRECTION oCountryRedirection = null;
                    foreach (COUNTRIES_REDIRECTION countriesRedirection in oCountriesRedirectList)
                    {
                        strOut = CallServiceIntegraMobileWS(Tools.METHODS_SIGNUP_STEP1_JSON, sInJson, countriesRedirection);
                        oResponseSignUpStep1 = JsonConvert.DeserializeObject<ResponseSignUpStep1>(strOut);
                        if (oResponseSignUpStep1.r != ResultType.Result_OK)
                        {
                            oCountryRedirection = countriesRedirection;
                            userExist = true;
                            break;
                        }
                    }

                    //En caso de encontrar el usuario en los servidores externos
                    //Verificar que no existe en el servidor principal
                    if (!userExist)
                    {
                        oCountryRedirection = null;
                        strOut = CallServiceIntegraMobileWS(Tools.METHODS_SIGNUP_STEP1_JSON, sInJson, null);
                        oResponseSignUpStep1 = JsonConvert.DeserializeObject<ResponseSignUpStep1>(strOut);
                    }

                    if (oResponseSignUpStep1 != null && oResponseSignUpStep1.r == ResultType.Result_OK)
                    {
                        Session["ResponseSignUpStep1"] = oResponseSignUpStep1;
                        return (RedirectToAction("Step2"));
                    }
                    else
                    {
                        if (oResponseSignUpStep1.r == ResultType.Result_Validate_Email)
                        {
                            model.Message = Resources.signup2_title;
                        }
                        else if (oResponseSignUpStep1.r == ResultType.Result_Error_Email_Already_Exist)
                        {
                            
                            if (oCountryRedirection != null)
                            {
                             //   this.ModelState.AddModelError(oResponseSignUpStep1.r.ToString(), Tools.Message(oResponseSignUpStep1.r));
                                
                            }
                            else
                            {
                                Session["NewUserEmail"] = model.Email;
                                //LogOnModel oLogOnModel = new LogOnModel();
                                //oLogOnModel.UserName = model.Email;
                                return (RedirectToAction("Index", "Home"));
                            }
                        }
                        if (oResponseSignUpStep1.r != ResultType.Result_OK && oResponseSignUpStep1.r != ResultType.Result_Validate_Email)
                        {
                            model.Message = String.Empty;
                            this.ModelState.AddModelError(oResponseSignUpStep1.r.ToString(), Tools.Message(oResponseSignUpStep1.r));
                        }
 
                    }
                    return View(model);
                }
                catch (Exception)
                {
                    return View(model);
                }
               
            }
            else
            {
                return View(model);
            }

        }
        #endregion

        #region Step2
        public ActionResult Step2()
        {
            SetCulture();

            RegistrationModelSignUpStep2 model = new RegistrationModelSignUpStep2();
            if (Session["CouIdRegistration"] != null)
            {
                model.Country = (String)Session["CouIdRegistration"];
                Session["CouIdRegistration"] = null;
            }
            model = GetQuestionsByCountry(model);
            return View(model);
        }


        [HttpPost]
        public ActionResult Step2(RegistrationModelSignUpStep2 model, String button, string utcoffset)
        {
            SetCulture();

            Session["RegistrationModelStep2"] = model;
           
            model = GetQuestionsByCountry(model);
            if (ModelState.IsValid)
            {
                if (!String.IsNullOrEmpty(button))
                {
                    if (ValidStep2(model, button))
                    {
                        ResponseSignUpStep1 oResponseSignUpStep1 = (ResponseSignUpStep1)Session["ResponseSignUpStep1"];
                        SortedList parametersIn = new SortedList();

                        String strXmlQuestions = String.Empty;
                        List<CheckBoxListQuestionsModel> questionsChecked = new List<CheckBoxListQuestionsModel>();
                        Request.Questions oQuestions = new Request.Questions();
                        foreach (String sChecked in model.SelectedQuestions)
                        {
                            CheckBoxListQuestionsModel oCheckBoxListQuestionsModel = model.ListQuestions.FirstOrDefault(x => x.Id == Convert.ToInt32(sChecked));
                            if (oCheckBoxListQuestionsModel != null)
                            {
                                questionsChecked.Add(oCheckBoxListQuestionsModel);
                            }
                        }

                        if (!GenerateXMLQuestions(questionsChecked, model, ref strXmlQuestions, ref oQuestions))
                        {
                            //xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                            //Logger_AddLogMessage(string.Format("Step2::GenerateXMLQuestions::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            //return xmlOut;
                        }

                        SignUpStep2Request oSignUpStep2Request = SignUpStep2Request.getRequest(oResponseSignUpStep1.bin, model.Password, Convert.ToInt32(model.Country), Tools.CONST_OSID, Tools.CONST_APPVERS, oQuestions);
                        string  sInJson = Tools.ToJsonRequest(oSignUpStep2Request);

                        oResponseSignUpStep1.ccode = model.Country;
                        Session["ResponseSignUpStep1"] = oResponseSignUpStep1;

                        XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(sInJson);
                        m_Log.LogMessage(LogLevels.logINFO, String.Format(Tools.PrettyXml(doc.InnerXml)));

                        try
                        {
                            COUNTRIES_REDIRECTION oCountriesRedirect = infraestructureRepository.GetCountriesRedirectionsByCountryId(Convert.ToDecimal(model.Country));
                            string strOut = string.Empty;
                            ResponseSignUpStep2 oResponseSignUpStep2 = null;

                            if (oCountriesRedirect != null)
                            {
                                strOut = CallServiceIntegraMobileWS(Tools.METHODS_SIGNUP_STEP2_JSON, sInJson, oCountriesRedirect);
                                oResponseSignUpStep2 = JsonConvert.DeserializeObject<ResponseSignUpStep2>(strOut);
                            }
                            else
                            {
                                strOut = CallServiceIntegraMobileWS(Tools.METHODS_SIGNUP_STEP2_JSON, sInJson, null);
                                oResponseSignUpStep2 = JsonConvert.DeserializeObject<ResponseSignUpStep2>(strOut);
                            }

                            if (oResponseSignUpStep2 != null && oResponseSignUpStep2.r == ResultType.Result_OK)
                            {
                                Session["ResponseSignUpStep2"] = oResponseSignUpStep2;
                                return AutoLogin(utcoffset, oCountriesRedirect);
                            }
                            else
                            {
                                this.ModelState.AddModelError(oResponseSignUpStep2.r.ToString(), Tools.Message(oResponseSignUpStep2.r));
                            }
                            return View(model);
                        }
                        catch (Exception)
                        {
                            return View(model);
                        }
                    }
                }
            }
            return View(model);
        }
        #endregion

        #region  Private Methods

        private ActionResult AutoLogin(string utcoffset, COUNTRIES_REDIRECTION oCountriesRedirect)
        {
            SetCulture();

            
            LogOnModel model = new LogOnModel();
            RegistrationModelSignUpStep1 oRegistrationModelSignUpStep1 = (RegistrationModelSignUpStep1)Session["RegistrationModelStep1"];
            RegistrationModelSignUpStep2 oRegistrationModelSignUpStep2 = (RegistrationModelSignUpStep2)Session["RegistrationModelStep2"];
            model.UserName = oRegistrationModelSignUpStep1.Email;
            model.Password = oRegistrationModelSignUpStep2.Password;

            if (string.IsNullOrEmpty(model.culture))
            {
                ViewData["lang_for_gCond"] = Session["Culture"].ToString().Replace("-", "_");
            }
            else
            {
                ViewData["lang_for_gCond"] = model.culture.Replace("-", "_");
                CultureInfo ci = new CultureInfo(model.culture);
                model.urlreferer = model.urlreferer.Replace(model.culture, "{0}");

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

            }


            if (ModelState.IsValid)
            {

                USER oUser = null;
                string strUsername = model.UserName;

                if (oCountriesRedirect != null)
                {
                    string sInJsonVerify = Tools.CreatingJsonInVerifyLoginExistsRequest(model.UserName, model.Password);

                    XmlDocument docVerif = (XmlDocument)JsonConvert.DeserializeXmlNode(sInJsonVerify);
                    m_Log.LogMessage(LogLevels.logINFO, String.Format(Tools.PrettyXml(docVerif.InnerXml)));

                    string strOut = CallServiceIntegraMobileWS(Tools.METHODS_VERIFY_LOGIN_EXISTS_JSON, sInJsonVerify, oCountriesRedirect);
                    VerifyLoginExistsResponse oVerifyLoginExistsResponse = JsonConvert.DeserializeObject<VerifyLoginExistsResponse>(strOut);

                    if (oVerifyLoginExistsResponse.r == ResultType.Result_OK)
                    {
                        NameValueCollection data = new NameValueCollection();
                        data.Add("UserName", model.UserName);
                        data.Add("Password", model.Password);
                        Tools.RedirectWithData(data, string.Format(@"{0}{1}{2}", oCountriesRedirect.COURE_COUNTRY_REDIRECTION_WEBAPP_URL, Tools.PATH_HOME, Tools.PATH_LOGON));
                    }
                    else
                    {
                        this.ModelState.AddModelError(oVerifyLoginExistsResponse.r.ToString(), Tools.Message(oVerifyLoginExistsResponse.r));
                    }
                }
                else
                {
                    if (FormAuthMemberShip.MembershipService.ValidateUser(ref strUsername, model.Password))
                    {
                        FormAuthMemberShip.FormsService.SignIn(strUsername, model.RememberMe);

                        if (customersRepository.GetUserData(ref oUser, strUsername))
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
                            if (string.IsNullOrEmpty(model.urlreferer))
                            {
                                ModelState.AddModelError("", ResourceExtension.GetLiteral("Home_GetUserDataError"));
                            }
                            else
                            {
                                return Redirect(string.Format("{0}?UserName={1}&ErrorMessage={2}",
                                    string.Format(model.urlreferer, Thread.CurrentThread.CurrentCulture.Name), HttpUtility.UrlEncode(model.UserName),
                                    HttpUtility.UrlEncode(ResourceExtension.GetLiteral("Home_GetUserDataError"))));
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(model.urlreferer))
                        {
                            ModelState.AddModelError("", ResourceExtension.GetLiteral("Home_BadUserNameOrPassword"));
                        }
                        else
                        {
                            return Redirect(string.Format("{0}?UserName={1}&ErrorMessage={2}",
                                string.Format(model.urlreferer, Thread.CurrentThread.CurrentCulture.Name), HttpUtility.UrlEncode(model.UserName),
                                HttpUtility.UrlEncode(ResourceExtension.GetLiteral("Home_BadUserNameOrPassword"))));
                        }
                    }
                }
            }
            return View(model);
        }

        private bool GenerateXMLQuestions(List<CheckBoxListQuestionsModel> questionsChecked, RegistrationModelSignUpStep2 model, ref string strXmlQuestions, ref integraMobile.Request.Questions oQuestions)
        {
            bool bRes = true;
            try
            {
                StringBuilder sb = new StringBuilder();
                strXmlQuestions = "";
                foreach (CheckBoxListQuestionsModel oCheck in model.ListQuestions)
                {
                    //XML
                    Boolean bFound = questionsChecked.Any(x => x.Id.Equals(oCheck.Id));
                    sb.Append("<question json:Array='true'>");
                    sb.AppendFormat("<idversion>{0}</idversion>", oCheck.Id);
                    sb.AppendFormat("<value>{0}</value>", Convert.ToInt32(bFound));
                    sb.Append("</question>");
                    
                    //OBJECT
                    integraMobile.Request.Question oQuestionModel = new integraMobile.Request.Question();
                    oQuestionModel.idversion = oCheck.Id;
                    oQuestionModel.value = Convert.ToInt32(bFound);

                    oQuestions.question.Add(oQuestionModel);
                }
                
                strXmlQuestions = sb.ToString();
            }
            catch (Exception e)
            {
                bRes = false;
                m_Log.LogMessage(LogLevels.logERROR, string.Format("GenerateXMLQuestions::Error: "), e);
            }
            return bRes;
        }

        private Boolean ValidStep2(RegistrationModelSignUpStep2 model, String buttonNextStep2)
        {
            Boolean bIsValid = true;
            if (!String.IsNullOrEmpty(buttonNextStep2) && buttonNextStep2.Equals("buttonNextStep2"))
            {
                List<CheckBoxListQuestionsModel> questionsNoFount = new List<CheckBoxListQuestionsModel>();

                if (model.SelectedQuestions.Count > 0)
                {
                    List<CheckBoxListQuestionsModel> questionsChecked = new List<CheckBoxListQuestionsModel>();
                    foreach (String sChecked in model.SelectedQuestions)
                    {
                        CheckBoxListQuestionsModel oCheckBoxListQuestionsModel = model.ListQuestions.FirstOrDefault(x => x.Id == Convert.ToInt32(sChecked));
                        if (oCheckBoxListQuestionsModel != null)
                        {
                            questionsChecked.Add(oCheckBoxListQuestionsModel);
                        }
                    }
                    if (questionsChecked.Count > 0)
                    {
                        questionsNoFount = model.ListQuestions.Except(questionsChecked).ToList();
                        if (questionsNoFount.Count > 0)
                        {
                            foreach (CheckBoxListQuestionsModel sError in questionsNoFount)
                            {
                                if (sError.Mandatory == 1)
                                {
                                    bIsValid = false;
                                    this.ModelState.AddModelError("Questions", Resources.SignUp3_Option_Mandatory + " " + sError.QuestionName.Replace("$", ""));
                                }
                            }
                        }
                    }
                }
                else if (model.SelectedQuestions.Count == 0 && model.ListQuestions.Count>0)
                {
                    CheckBoxListQuestionsModel oError =  model.ListQuestions.FirstOrDefault(x => x.Mandatory == Convert.ToInt32(1));
                    if (oError.Mandatory == 1)
                    {
                        bIsValid = false;
                        this.ModelState.AddModelError("Questions", Resources.SignUp3_Option_Mandatory + " " + oError.QuestionName.Replace("$", ""));
                    }
                }
                if (String.IsNullOrEmpty(model.Password))
                {
                    this.ModelState.AddModelError("Password", String.Format(Resources.ErrorsMsg_RequiredField, "PassWord"));
                    bIsValid = false;
                }
            }
            return bIsValid;
        }
      
        private RegistrationModelSignUpStep2 GetQuestionsByCountry(RegistrationModelSignUpStep2 model)
        {
            ResponseSignUpStep1 oResponseSignUpStep1 = new ResponseSignUpStep1();
            oResponseSignUpStep1 = (ResponseSignUpStep1)Session["ResponseSignUpStep1"];
            if (String.IsNullOrEmpty(model.Country) && !String.IsNullOrEmpty(oResponseSignUpStep1.ccode))
            {
                model.Country = oResponseSignUpStep1.ccode;
            }
            if (oResponseSignUpStep1.countries == null || oResponseSignUpStep1.countries.country.Count == 0)
            {
                CultureInfo ci = (CultureInfo)Session["Culture"];
                if (ci == null)
                {
                    //Sets default culture to english invariant
                    string langName = "en-US";
                    ci = new CultureInfo(langName);
                    Session["Culture"] = ci;
                }
                oResponseSignUpStep1.countries = (LoadCountries(ci.Name));
            }
            
            List<COUNTRy> oCountries = new List<COUNTRy>();
            
            foreach (decimal co in LIST_COUNTRIES)
            {
                oCountries.Add(infraestructureRepository.Countries.FirstOrDefault(x => x.COU_ID.Equals(co)));
            }
            COUNTRy oCOUNTRy = new COUNTRy();
            if (oCountries.Count > 0)
            {
                model.ListCountries = (List<CountryModel>)model.GetCountriesList(oCountries);

                if (String.IsNullOrEmpty(model.Country) || (model.Country=="-1"))
                {
                    model.Country = Convert.ToString(oCountries[0].COU_ID);
                    oCOUNTRy = infraestructureRepository.Countries.FirstOrDefault(x => x.COU_DESCRIPTION.Equals(oCountries[0].COU_DESCRIPTION));
                }
                else
                {
                    oCOUNTRy = infraestructureRepository.Countries.FirstOrDefault(x => x.COU_ID == Convert.ToInt32(model.Country));
                }
            }

            integraMobile.Models.CountriesModel.Country countryQuestions = oResponseSignUpStep1.countries.country.FirstOrDefault(x => x.idcountry == Convert.ToInt32(oCOUNTRy.COU_ID));
            if (countryQuestions != null && countryQuestions.questions.Count > 0)
            {
                model.ListQuestions = (List<CheckBoxListQuestionsModel>)model.GetCheckBoxListQuestionsList(countryQuestions.questions);
            }
            return model;
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

        private CountriesModel LoadCountries(String strCulture)
        {
            CountriesModel oCountriesModel = new CountriesModel();

            decimal dSourceApp = geograficRepository.GetDefaultSourceApp();

            IEnumerable<stVersionsLiterals> versionsLiterals = customersRepository.GetQuestions(strCulture, dSourceApp);

            List<decimal> oCountryValidate = new List<decimal>();
            foreach (stVersionsLiterals vl in versionsLiterals.OrderBy(r => r.countryId))
            {
                if (!oCountryValidate.Contains(vl.countryId))
                {
                    
                    CountriesModel.Country oCountry = new CountriesModel.Country();
                    oCountry.idcountry = Convert.ToInt32(vl.countryId);
                    

                    List<stVersionsLiterals> lista = versionsLiterals.Where(r => r.countryId.Equals(vl.countryId)).ToList();

                    oCountry.questions = new List<CountriesModel.Questions>();
                    CountriesModel.Questions oQuestions = new CountriesModel.Questions();
                    oCountry.questions.Add(oQuestions); 
                    foreach (stVersionsLiterals vl2 in lista.OrderBy(x => x.IdVersion))
                    {
                        CountriesModel.Question oQuestion = new CountriesModel.Question();
                        oQuestion.idversion = Convert.ToInt32(vl2.IdVersion);
                        oQuestion.literal = vl2.strLiteral;
                        oQuestion.mandatory = Convert.ToInt32(vl2.Mandatory);

                        oQuestion.urls = new CountriesModel.Urls();
                        oQuestion.urls.url = new List<string>();

                        if (vl2.liststUrlLiteral.Count > 0)
                        {
                            
                            foreach (stUrlLiteral sUl in vl2.liststUrlLiteral)
                            {
                                oQuestion.urls.url.Add(sUl.URL);
                            }
                        }
                        oCountry.questions[0].question.Add(oQuestion);
                    }
                    oCountriesModel.country.Add(oCountry);
                }
            }
            return oCountriesModel;
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

        private string CallServiceIntegraMobileWS(string methods, string sInJson, COUNTRIES_REDIRECTION countriesRedirection)
        {
            string jsonOut = string.Empty;
            try
            {
            integraMobileWS.integraMobileWS oIntegraMobileWS = Start(countriesRedirection);
                switch (methods)
                {
                    case Tools.METHODS_SIGNUP_STEP1_JSON:
                        jsonOut = oIntegraMobileWS.SignUpStep1JSON(sInJson);
                        jsonOut = Tools.RemeveTagIparkOut(jsonOut);
                        break;

                    case Tools.METHODS_SIGNUP_STEP2_JSON:
                        jsonOut = oIntegraMobileWS.SignUpStep2JSON(sInJson);
                        jsonOut = Tools.RemeveTagIparkOut(jsonOut);
                        break;

                    case Tools.METHODS_QUERY_LOGIN_CITY_JSON:
                        jsonOut = oIntegraMobileWS.QueryLoginCityJSON(sInJson);
                        jsonOut = Tools.RemeveTagIparkOut(jsonOut);
                        break;

                    case Tools.METHODS_VERIFY_LOGIN_EXISTS_JSON:
                        jsonOut = oIntegraMobileWS.VerifyLoginExistsJSON(sInJson);
                        jsonOut = Tools.RemeveTagIparkOut(jsonOut);
                        break;
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return jsonOut;
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
        #endregion
    }
}
