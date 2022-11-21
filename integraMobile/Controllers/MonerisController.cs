using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Mvc;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Web;
using System.Threading;
using integraMobile.Web.Resources;
using integraMobile.Models;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Invoicing;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;
using integraMobile.Helper;

namespace integraMobile.Controllers
{
    [HandleError]
    [NoCache]
    public class MonerisController : BaseCCController
    {

        public MonerisController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;          
        }

        public MonerisController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository, System.Web.HttpRequestBase iRequest, System.Web.HttpServerUtilityBase iServer, System.Web.HttpSessionStateBase iSession, decimal? Config_id)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;            
            _request = iRequest;
            _server = iServer;
            _session = iSession;
            bAvoidHashCheck = true;
            if (Config_id.HasValue)
                Configuration_Id = Config_id!=0?Config_id:null;
            else
                Configuration_Id=null;
        }

        [HttpPost]
        public ActionResult MonerisRequest()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            return MonerisRequest(_request["Guid"],
                                _request["Email"],
                                (_request["Amount"] != null ? Convert.ToInt32(_request["Amount"]) : (int?)null),
                                _request["CurrencyISOCODE"],
                                _request["Description"],
                                _request["UTCDate"],
                                _request["Culture"],
                                _request["ReturnURL"],
                                "",
                                _request["Hash"]);
        }

        [HttpGet]
        public ActionResult MonerisRequest(string Guid, string Email, int? Amount, string CurrencyISOCODE, string Description, string UTCDate,  string Culture, string ReturnURL, string ExternalId, string Hash)
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";

          
            try
            {
                if (_session == null) _session = Session;
                MonerisPayments oPayments = new MonerisPayments();
                string strOrderId = MonerisPayments.UserReference();
                _session["sessionid"] = strOrderId;


                if (_server == null) _server = Server;
                if (_request == null) _request = Request;

                _session["result"] = null;
                _session["errorCode"] = null;
                _session["errorMessage"] = null;
                _session["email"] = null;
                _session["amount"] = null;
                _session["currency"] = null;
                _session["utcdate"] = null;
                _session["MonerisGuid"] = null;
                _session["cardToken"] = null;
                _session["cardScheme"] = null;
                _session["cardPAN"] = null;
                _session["cardExpMonth"] = null;
                _session["cardExpYear"] = null;
                _session["chargeDateTime"] = null;
                _session["HashSeed"] = null;
                _session["ReturnURL"] = null;
                _session["sessionid"] = null;
                _session["orderid"] = null;
                _session["MD"] = null;
                _session["CAVV"] = null;
                _session["ECI"] = null;
                _session["ExternalId"] = null;




                Logger_AddLogMessage(string.Format("MonerisRequest Begin: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}; ReturnURL={6}; culture= {7}; ExternalId={8}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        UTCDate,
                                        Description,
                                        ReturnURL,
                                        ExternalId,
                                        Culture), LogLevels.logINFO);


                _session["ReturnURL"] = ReturnURL;
                _session["ExternalId"] = ExternalId;

                if ((string.IsNullOrEmpty(Guid) && Configuration_Id == null) ||
                    (string.IsNullOrEmpty(Email)) ||
                    (!Amount.HasValue) || (Amount.Value <= 0) ||
                    (string.IsNullOrEmpty(CurrencyISOCODE)) ||
                    (string.IsNullOrEmpty(Description)) ||
                    (string.IsNullOrEmpty(UTCDate)) ||
                    (string.IsNullOrEmpty(Hash) && bAvoidHashCheck == false))
                {
                    result = "error";
                    errorMessage = "Invalid or missing parameter";
                    errorCode = "invalid_parameter";
                }

                else
                {
                    string moneris_lang="en-ca";
                    if (!string.IsNullOrEmpty(Culture))
                    {
                        if (Culture.ToLower().StartsWith("fr"))
                        {
                            moneris_lang = "fr-ca";
                        }

                    }

                    MONERIS_CONFIGURATION oMonerisConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetMonerisConfigurationById(Configuration_Id, out oMonerisConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetMonerisConfiguration(Guid, out oMonerisConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oMonerisConfiguration != null)
                        {

                            _session["HashSeed"] = oMonerisConfiguration.MONCON_HASH_SEED;

                            string strCalcHash = CalculateHash(oMonerisConfiguration.MONCON_GUID, Email, Amount.Value, CurrencyISOCODE, Description, UTCDate, ReturnURL, Culture, ExternalId, oMonerisConfiguration.MONCON_HASH_SEED);

                            if ((oMonerisConfiguration.MONCON_CHECK_DATE_AND_HASH == 0) ||
                                (strCalcHash == Hash) ||
                                (bAvoidHashCheck == true))
                            {
                                DateTime dtUTC = DateTime.UtcNow; ;
                                try
                                {
                                    dtUTC = DateTime.ParseExact(UTCDate, "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    result = "error";
                                    errorMessage = "Invalid DateTime";
                                    errorCode = "invalid_datetime";
                                    Logger_AddLogMessage(string.Format("MonerisRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                       UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                }

                                if (string.IsNullOrEmpty(result))
                                {

                                    if ((oMonerisConfiguration.MONCON_CHECK_DATE_AND_HASH == 0) ||
                                        (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oMonerisConfiguration.MONCON_CONFIRMATION_TIME) ||
                                        (bAvoidHashCheck == true))
                                    {

                                        NumberFormatInfo provider = new NumberFormatInfo();
                                        string strAmount = (Amount.Value / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(CurrencyISOCODE), provider)).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(CurrencyISOCODE), provider);


                                        

                                        string strTicket = "";
                                        string strHPPID = "";
                                        MonerisPayments.MonerisErrorCode eErrorCode;
                                        _session["sessionid"]=strOrderId;
                                        _session["orderid"] = strOrderId;



                                        oPayments.GetTicketFromDataPreload(oMonerisConfiguration.MONCON_MONERIS_FORM_URL, oMonerisConfiguration.MONCON_PS_STORE_ID, oMonerisConfiguration.MONCON_HPP_KEY, strOrderId, strAmount, Email, moneris_lang, 
                                            oMonerisConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_FORM_URL, out eErrorCode, out errorMessage, out strHPPID, out strTicket);
                                        
                                        if (!MonerisPayments.IsError(eErrorCode))                                        
                                        {

                                            ViewData["moneris_form_url"] = oMonerisConfiguration.MONCON_MONERIS_FORM_URL;
                                            ViewData["moneris_hpp_id"] = strHPPID;
                                            ViewData["moneris_ticket"] = strTicket;

                                            _session["email"] = Email;
                                            _session["amount"] = Amount;
                                            _session["currency"] = CurrencyISOCODE;
                                            _session["utcdate"] = dtUTC;
                                            Guid = oMonerisConfiguration.MONCON_GUID;
                                            _session["MonerisGuid"] = Guid;

                                            Logger_AddLogMessage(string.Format("MonerisRequest Begin: Guid={0}, ConfigurationId={1}", _session["MonerisGuid"].ToString(),Configuration_Id), LogLevels.logINFO);


                                        }
                                        else
                                        {
                                            result = "error";
                                            errorCode = eErrorCode.ToString();
                                            if (string.IsNullOrEmpty(errorMessage))
                                            {
                                                errorMessage = MonerisPayments.ErrorMessageDict[eErrorCode];
                                            }
                                        }
                                        
                                    }
                                    else
                                    {
                                        result = "error";
                                        errorMessage = "Invalid DateTime";
                                        errorCode = "invalid_datetime";
                                        Logger_AddLogMessage(string.Format("MonerisRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }
                                }
                            }
                            else
                            {
                                result = "error";
                                errorCode = "invalid_hash";
                                errorMessage = "Invalid Hash";

                                Logger_AddLogMessage(string.Format("MonerisRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                       Hash, strCalcHash), LogLevels.logINFO);


                            }

                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "Moneris configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "Moneris configuration not found";
                    }
                }
            }
            catch
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = "MonerisRequest Method Exception";
            }


            if (!string.IsNullOrEmpty(errorCode))
            {
             
                _session["result"] = result;
                _session["errorCode"] = errorCode;
                _session["errorMessage"] = errorMessage;

                SaveSession(_session["sessionid"].ToString());

                string strRedirectionURLLog = string.Format("MonerisResult?result={0}&errorCode={1}&errorMessage={2}", _server.UrlEncode(result), _server.UrlEncode(errorCode), _server.UrlEncode(errorMessage));
                string strRedirectionURL = string.Format("MonerisResult?r={0}&s={1}", MonerisResultCalc(), _session["sessionid"]);
                Logger_AddLogMessage(string.Format("MonerisRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; ResultURL={4}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        strRedirectionURLLog), LogLevels.logINFO);
                return Redirect(strRedirectionURL);
            }
            else
            {

                SaveSession(_session["sessionid"].ToString());

                Logger_AddLogMessage(string.Format("MonerisRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        UTCDate,
                                        Description), LogLevels.logINFO);
                return View();
            }

        }


        [HttpPost]
        public ActionResult MonerisRequestHT()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            return MonerisRequestHT(_request["Guid"],
                                _request["Email"],
                                (_request["Amount"] != null ? Convert.ToInt32(_request["Amount"]) : (int?)null),
                                _request["CurrencyISOCODE"],
                                _request["Description"],
                                _request["UTCDate"],
                                _request["Culture"],
                                _request["ReturnURL"],
                                (_request["ExternalId"] != null ? _request["ExternalId"].ToString() : ""),
                                _request["Hash"],
                                (_request["ReturnByGet"] != null ? _request["ReturnByGet"].ToString() : "0"));
        }

        [HttpGet]
        public ActionResult MonerisRequestHT(string Guid, string Email, int? Amount, string CurrencyISOCODE, string Description, string UTCDate, string Culture, string ReturnURL, string ExternalId, string Hash, string ReturnByGet = "0")
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";


           
            try
            {
                if (_session == null) _session = Session;
                MonerisPayments oPayments = new MonerisPayments();


                if (_server == null) _server = Server;
                if (_request == null) _request = Request;

                _session["result"] = null;
                _session["errorCode"] = null;
                _session["errorMessage"] = null;
                _session["email"] = null;
                _session["amount"] = null;
                _session["currency"] = null;
                _session["utcdate"] = null;
                _session["MonerisGuid"] = null;
                _session["cardToken"] = null;
                _session["cardScheme"] = null;
                _session["cardPAN"] = null;
                _session["cardExpMonth"] = null;
                _session["cardExpYear"] = null;
                _session["chargeDateTime"] = null;
                _session["HashSeed"] = null;
                _session["ReturnURL"] = null;
                _session["sessionid"] = null;
                _session["orderid"] = null;
                _session["MD"] = null;
                _session["CAVV"] = null;
                _session["ECI"] = null;
                _session["ExternalId"] = null;
                _session["ReturnByGet"] = null;



                Logger_AddLogMessage(string.Format("MonerisRequestHT Begin: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}; ReturnURL={6}; culture= {8};  ExternalId={7}; ReturnByGet={9}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        UTCDate,
                                        Description,
                                        ReturnURL,
                                        ExternalId,
                                        Culture,
                                        ReturnByGet), LogLevels.logINFO);


                _session["ReturnURL"] = ReturnURL;
                _session["ExternalId"] = ExternalId;
                _session["ReturnByGet"] = ReturnByGet;

                string moneris_lang = "en-US";
                if (!string.IsNullOrEmpty(Culture))
                {
                    if (Culture.ToLower().StartsWith("fr"))
                    {
                        moneris_lang = "fr-CA";
                    }

                }

                CultureInfo ci = CultureInfo.GetCultureInfo(moneris_lang);
                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = ci;
                integraMobile.Properties.Resources.Culture = ci;


                Logger_AddLogMessage(string.Format("MonerisRequest : Culture={0}", moneris_lang), LogLevels.logINFO);


                if ((string.IsNullOrEmpty(Guid) && Configuration_Id == null) ||
                    (string.IsNullOrEmpty(Email)) ||
                    (!Amount.HasValue) || 
                    (string.IsNullOrEmpty(CurrencyISOCODE)) ||
                    (string.IsNullOrEmpty(Description)) ||
                    (string.IsNullOrEmpty(UTCDate)) ||
                    (string.IsNullOrEmpty(Hash) && bAvoidHashCheck == false))
                {
                    result = "error";
                    errorMessage = "Invalid or missing parameter";
                    errorCode = "invalid_parameter";
                }

                else
                {

                    MONERIS_CONFIGURATION oMonerisConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetMonerisConfigurationById(Configuration_Id, out oMonerisConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetMonerisConfiguration(Guid, out oMonerisConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oMonerisConfiguration != null)
                        {

                            _session["HashSeed"] = oMonerisConfiguration.MONCON_HASH_SEED;

                            string strCalcHash = CalculateHash(oMonerisConfiguration.MONCON_GUID, Email, Amount.Value, CurrencyISOCODE, Description, 
                                                                    UTCDate, ReturnURL, Culture,  ExternalId, oMonerisConfiguration.MONCON_HASH_SEED);

                            if ((oMonerisConfiguration.MONCON_CHECK_DATE_AND_HASH == 0) ||
                                (strCalcHash == Hash) ||
                                (bAvoidHashCheck == true))
                            {
                                DateTime dtUTC = DateTime.UtcNow; ;
                                try
                                {
                                    dtUTC = DateTime.ParseExact(UTCDate, "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    result = "error";
                                    errorMessage = "Invalid DateTime";
                                    errorCode = "invalid_datetime";
                                    Logger_AddLogMessage(string.Format("MonerisRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                       UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                }

                                if (string.IsNullOrEmpty(result))
                                {

                                    if ((oMonerisConfiguration.MONCON_CHECK_DATE_AND_HASH == 0) ||
                                        (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oMonerisConfiguration.MONCON_CONFIRMATION_TIME) ||
                                        (bAvoidHashCheck == true))
                                    {

                                        NumberFormatInfo provider = new NumberFormatInfo();
                                        string strAmount = (Amount.Value / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(CurrencyISOCODE), provider)).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(CurrencyISOCODE), provider);

                                        ViewData["moneris_hosted_tokenization_url"] = oMonerisConfiguration.MONCON_HOSTED_TOKENIZATION_URL;
                                        ViewData["moneris_hosted_tokenization_id"] = oMonerisConfiguration.MONCON_HOSTED_TOKENIZATION_PROFILE_ID;

                                          string strURLPath = _request.Url.AbsoluteUri.Substring(0, _request.Url.AbsoluteUri.LastIndexOf("/"));
                                        string sSuffix = string.Empty;
                                        if (_session["Suffix"] != null) 
                                        {
                                            sSuffix = _session["Suffix"].ToString();
                                        }
                                        string urlReturn = strURLPath + "/MonerisResponseHT" + sSuffix;
                                        ViewData["response_url"] = urlReturn;


                                        _session["email"] = Email;
                                        _session["amount"] = Amount;
                                        _session["currency"] = CurrencyISOCODE;
                                        _session["utcdate"] = dtUTC;
                                        Guid = oMonerisConfiguration.MONCON_GUID;
                                        _session["MonerisGuid"] = Guid;

                                        Logger_AddLogMessage(string.Format("MonerisRequest Begin: Guid={0}, ConfigurationId={1}", _session["MonerisGuid"].ToString(), Configuration_Id), LogLevels.logINFO);
                                      

                                    }
                                    else
                                    {
                                        result = "error";
                                        errorMessage = "Invalid DateTime";
                                        errorCode = "invalid_datetime";
                                        Logger_AddLogMessage(string.Format("MonerisRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }
                                }
                            }
                            else
                            {
                                result = "error";
                                errorCode = "invalid_hash";
                                errorMessage = "Invalid Hash";

                                Logger_AddLogMessage(string.Format("MonerisRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                       Hash, strCalcHash), LogLevels.logINFO);


                            }

                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "Moneris configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "Moneris configuration not found";
                    }
                }
            }
            catch
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = "MonerisRequest Method Exception";
            }


            if (!string.IsNullOrEmpty(errorCode))
            {

                _session["result"] = result;
                _session["errorCode"] = errorCode;
                _session["errorMessage"] = errorMessage;

            
                string strRedirectionURLLog = string.Format("MonerisResult?result={0}&errorCode={1}&errorMessage={2}", _server.UrlEncode(result), _server.UrlEncode(errorCode), _server.UrlEncode(errorMessage));
                string strRedirectionURL = string.Format("MonerisResult?r={0}&s={1}", MonerisResultCalc(), _session["sessionid"]);
                Logger_AddLogMessage(string.Format("MonerisRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; ResultURL={4}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        strRedirectionURLLog), LogLevels.logINFO);
                return Redirect(strRedirectionURL);
            }
            else
            {

                Logger_AddLogMessage(string.Format("MonerisRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCODE,
                                        UTCDate,
                                        Description), LogLevels.logINFO);
                return View();
            }

        }




        [HttpGet]
        public ActionResult MonerisMPIRequest(string Guid, string id, string MD, string Email, string UTCDate, string Culture, string ReturnURL, string Hash)
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURL = "";


            try
            {
                if (_session == null) _session = Session;
                MonerisPayments oPayments = new MonerisPayments();


                if (_server == null) _server = Server;
                if (_request == null) _request = Request;

                Logger_AddLogMessage(string.Format("MonerisMPIRequest Begin: Guid={0}; id={1} ; MD={2}; Email={3} ;UTCDate = {4} ; culture= {5}; ReturnURL={6}",
                                        Guid,
                                        id,
                                        MD,
                                        Email,
                                        UTCDate,
                                        Culture,
                                        ReturnURL), LogLevels.logINFO);

                _session["result"] = null;
                _session["errorCode"] = null;
                _session["errorMessage"] = null;
                _session["email"] = null;
                _session["amount"] = null;
                _session["currency"] = null;
                _session["utcdate"] = null;
                _session["MonerisGuid"] = null;
                _session["cardToken"] = null;
                _session["cardScheme"] = null;
                _session["cardPAN"] = null;
                _session["cardExpMonth"] = null;
                _session["cardExpYear"] = null;
                _session["chargeDateTime"] = null;
                _session["HashSeed"] = null;
                _session["ReturnURL"] = null;
                _session["sessionid"] = null;
                _session["orderid"] = null;
                _session["MD"] = null;
                _session["CAVV"] = null;
                _session["ECI"] = null;


                _session["ReturnURL"] = ReturnURL;

                if ((string.IsNullOrEmpty(Guid) && Configuration_Id == null) ||
                    (string.IsNullOrEmpty(Email)) ||
                    (string.IsNullOrEmpty(id)) ||
                    (string.IsNullOrEmpty(MD)) ||
                    (string.IsNullOrEmpty(UTCDate)) ||
                    (string.IsNullOrEmpty(Hash) && bAvoidHashCheck == false))
                {
                    result = "error";
                    errorMessage = "Invalid or missing parameter";
                    errorCode = "invalid_parameter";
                }

                else
                {

                    MONERIS_CONFIGURATION oMonerisConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetMonerisConfigurationById(Configuration_Id, out oMonerisConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetMonerisConfiguration(Guid, out oMonerisConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oMonerisConfiguration != null)
                        {

                            _session["HashSeed"] = oMonerisConfiguration.MONCON_HASH_SEED;

                            string strCalcHash = CalculateHash(oMonerisConfiguration.MONCON_GUID, id, MD, Email, UTCDate, Culture, ReturnURL, "", oMonerisConfiguration.MONCON_HASH_SEED);

                            if ((oMonerisConfiguration.MONCON_CHECK_DATE_AND_HASH == 0) ||
                                (strCalcHash == Hash) ||
                                (bAvoidHashCheck == true))
                            {
                                DateTime dtUTC = DateTime.UtcNow; ;
                                try
                                {
                                    dtUTC = DateTime.ParseExact(UTCDate, "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    result = "error";
                                    errorMessage = "Invalid DateTime";
                                    errorCode = "invalid_datetime";
                                    Logger_AddLogMessage(string.Format("MonerisMPIRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                       UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                }

                                if (string.IsNullOrEmpty(result))
                                {

                                    if ((oMonerisConfiguration.MONCON_CHECK_DATE_AND_HASH == 0) ||
                                        (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oMonerisConfiguration.MONCON_CONFIRMATION_TIME) ||
                                        (bAvoidHashCheck == true))
                                    {

                                        string strInlineForm="";
                                        infraestructureRepository.GetMoneris3DSTransactionInlineForm(Convert.ToDecimal(id), MD, Email, out strInlineForm);


                                        if (!string.IsNullOrEmpty(strInlineForm))
                                        {
                                            _session["result"] = result;
                                            _session["errorCode"] = errorCode;
                                            _session["errorMessage"] = errorMessage;
                                            _session["sessionid"] = MD;
                                            _session["email"] = Email;
                                            _session["utcdate"] = dtUTC;
                                            Guid = oMonerisConfiguration.MONCON_GUID;
                                            _session["MonerisGuid"] = Guid;
                                            SaveSession(MD);
                                            return Content(strInlineForm); ;
                                        }
                                        else
                                        {
                                            result = "error";
                                            errorMessage = "empty inline form";
                                            errorCode = "invalid_transaction";
                                            Logger_AddLogMessage(string.Format("MonerisMPIRequest : Empty inline form",
                                                               UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                        }



                                    }
                                    else
                                    {
                                        result = "error";
                                        errorMessage = "Invalid DateTime";
                                        errorCode = "invalid_datetime";
                                        Logger_AddLogMessage(string.Format("MonerisMPIRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }
                                }
                            }
                            else
                            {
                                result = "error";
                                errorCode = "invalid_hash";
                                errorMessage = "Invalid Hash";

                                Logger_AddLogMessage(string.Format("MonerisMPIRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                       Hash, strCalcHash), LogLevels.logINFO);


                            }

                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "Moneris configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "Moneris configuration not found";
                    }
                }
            }
            catch
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = "MonerisRequest Method Exception";
            }

            if (!string.IsNullOrEmpty(errorCode))
            {

                _session["result"] = result;
                _session["errorCode"] = errorCode;
                _session["errorMessage"] = errorMessage;

                SaveSession(_session["sessionid"].ToString());

                string strRedirectionURLLog = string.Format("MonerisResult?result={0}&errorCode={1}&errorMessage={2}", _server.UrlEncode(result), _server.UrlEncode(errorCode), _server.UrlEncode(errorMessage));
                strRedirectionURL = string.Format("MonerisResult?r={0}&s={1}", MonerisResultCalc(), _session["sessionid"]);
                Logger_AddLogMessage(string.Format("MonerisRequest End: Guid={0}; Email={1} ; ResultURL={2}",
                                        Guid,
                                        Email,
                                        strRedirectionURLLog), LogLevels.logINFO);
                return Redirect(strRedirectionURL);
            }

            return View();
        }




        [HttpPost]
        public ActionResult MonerisSuccess()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            if (_session["sessionid"] == null)
            {
                string response_order_id = _request["response_order_id"];
                string[] response_order_id_split = response_order_id.Split('-');
                LoadSession(response_order_id_split[0]);
            }

            Logger_AddLogMessage(string.Format("MonerisSucessPost: Guid={0}; ConfigurationID={1}", _session["MonerisGuid"], Configuration_Id), LogLevels.logINFO);

            return MonerisSuccess(_request["response_order_id"],
                                _request["response_code"],
                                _request["date_stamp"],
                                _request["time_stamp"],
                                _request["eci"],
                                _request["txn_num"],
                                _request["bank_approval_code"],
                                _request["result"],
                                _request["trans_name"],
                                (_request["gcardholder"] != null ? _request["gcardholder"] : _request["cardholder"]).ToString(),
                                _request["charge_total"],
                                _request["card"],
                                _request["f4l4"],
                                _request["message"],
                                _request["iso_code"],
                                _request["bank_transaction_id"],
                                _request["expiry_date"],
                                _request["cvd_response_code"],
                                _request["email"],
                                _request["cust_id"],
                                _request["note"]);
        }

        [HttpPost]
        public ActionResult MonerisFailure()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            if (_session["sessionid"] == null)
            {
                string response_order_id = _request["response_order_id"];
                string[] response_order_id_split = response_order_id.Split('-');
                LoadSession(response_order_id_split[0]);
            }

            Logger_AddLogMessage(string.Format("MonerisFailurePost: Guid={0}; ConfigurationID={1}", _session["MonerisGuid"], Configuration_Id), LogLevels.logINFO);

            return MonerisFailure(_request["response_order_id"],
                                  _request["response_code"],
                                  _request["date_stamp"],
                                  _request["time_stamp"],
                                  _request["message"]);
        }

        [HttpPost]
        public ActionResult MonerisResponseHT()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            return MonerisResponseHT(_request["ResponseCode"],
                                        _request["DataKey"],
                                        _request["ErrorMessage"],
                                        _request["Bin"]);
        }


        [HttpGet]
        public ActionResult MonerisResponseHT(string ResponseCode,
                                              string DataKey,
                                              string ErrorMessage,
                                              string Bin)
        { 
            
            
            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "MonerisResult";
            string strCardHash = "";
            string strCardReference = "";
            string strReference = "";
            string strTransactionId = "";
            string strCardScheme = "";
            string strMonerisDateTime = "";
            string strPAN = "";
            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";
            string strAuthCode = "";
            string strAuthResult = "";
            string result = "";
            string strExpDate = "";
            string strIssuerId = "";
            bool bExecuteFinally = true;

            string strOrderId = MonerisPayments.UserReference();


            try
            {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            _session["sessionid"] = strOrderId;

            
            Logger_AddLogMessage(string.Format("MonerisResponseHT Begin: ErrorCode={0} ; Token={1} ; Email={2} ; Amount={3} ; Currency={4} ; Guid={5}; ConfigurationID={6};ResponseCode={7};DataKey={8}; ErrorMessage={9}; Bin={10}  ",
                                        "succeeded",
                                        strCardReference,
                                        _session["email"],
                                        _session["amount"],
                                        _session["currency"],
                                        _session["MonerisGuid"],
                                        Configuration_Id,
                                        ResponseCode,
                                        DataKey,
                                        ErrorMessage,
                                        Bin
                                        ), LogLevels.logINFO);


                string strGuid = "";

                if (_session["MonerisGuid"] != null)
                {
                    strGuid = _session["MonerisGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid) && Configuration_Id == null)
                {
                    result = "error";
                    errorMessage = "Moneris Guid not found";
                    errorCode = "invalid_configuration";

                }
                else
                {

                    MONERIS_CONFIGURATION oMonerisConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetMonerisConfigurationById(Configuration_Id, out oMonerisConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetMonerisConfiguration(strGuid, out oMonerisConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oMonerisConfiguration == null)
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "Moneris configuration not found";
                        }
                        else
                        {

                            MonerisPayments.MonerisErrorCode eErrorCode;

                            try
                            {
                                eErrorCode = (MonerisPayments.MonerisErrorCode)Convert.ToInt32(ResponseCode);
                            }
                            catch
                            {
                                eErrorCode = MonerisPayments.MonerisErrorCode.InternalError;
                            }

                            errorCode = eErrorCode.ToString();
                            errorMessage = ErrorMessage;

                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                errorMessage = MonerisPayments.ErrorMessageDict[eErrorCode];
                            }
                            

                            /*DateTime dtUTC = (DateTime)_session["utcdate"];


                            if ((oMonerisConfiguration.MONCON_CHECK_DATE_AND_HASH == 1) &&
                                (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) > oMonerisConfiguration.MONCON_CONFIRMATION_TIME))
                            {
                                result = "error";
                                errorMessage = "Invalid DateTime";
                                errorCode = "invalid_datetime";
                                Logger_AddLogMessage(string.Format("MonerisResponse : BeginningDate={0} ; CurrentDate={1}",
                                dtUTC, DateTime.UtcNow), LogLevels.logINFO);
                                

                            }
                            else
                            {*/
                                if (MonerisPayments.IsError(eErrorCode))
                                {
                                    result = "error";


                                    if (eErrorCode != MonerisPayments.MonerisErrorCode.CancelledByCardHolder)
                                    {
                                        errorCode = "transaction_failed";
                                    }

                                }
                                else
                                {
                                    MonerisPayments oPayments = new MonerisPayments();

                                    oPayments.CardVerification(oMonerisConfiguration.MONCON_API_STORE_ID, oMonerisConfiguration.MONCON_API_STORE_KEY, strOrderId, DataKey,
                                        oMonerisConfiguration.MONCON_PROCESING_COUNTRY, _session["email"].ToString(), oMonerisConfiguration.MONCON_TEST_MODE != 0, out eErrorCode, out errorMessage, 
                                        out strIssuerId,
                                        out strTransactionId,
                                        out strAuthCode,
                                        out strAuthResult,
                                        out strMonerisDateTime,
                                        out strPAN,
                                        out strCardScheme,
                                        out strExpDate);
                                    if (MonerisPayments.IsError(eErrorCode))
                                    {
                                        result = "error";
                                        errorCode = "invalid_card";



                                        Logger_AddLogMessage(string.Format("MonerisResponseHT:CardVerification : errorCode={0} ; errorMessage={1}",
                                                    errorCode, errorMessage), LogLevels.logINFO);

                                        if (string.IsNullOrEmpty(errorMessage))
                                        {
                                            errorMessage = MonerisPayments.ErrorMessageDict[eErrorCode];
                                        }

                                    }
                                    else
                                    {

                                        string errorMessage2 = "";

                                        oPayments.GetPermanentToken(oMonerisConfiguration.MONCON_API_STORE_ID, oMonerisConfiguration.MONCON_API_STORE_KEY, DataKey,strIssuerId,
                                            oMonerisConfiguration.MONCON_PROCESING_COUNTRY, _session["email"].ToString(), strExpDate, oMonerisConfiguration.MONCON_TEST_MODE != 0, out eErrorCode, out errorMessage2, 
                                            out strCardReference);
                                        if (MonerisPayments.IsError(eErrorCode))
                                        {
                                            result = "error";
                                            errorCode = "null_token";



                                            Logger_AddLogMessage(string.Format("MonerisResponseHT:CardVerification : errorCode={0} ; errorMessage={1}",
                                                        errorCode, errorMessage2), LogLevels.logINFO);

                                            errorMessage = errorMessage2;

                                            if (string.IsNullOrEmpty(errorMessage))
                                            {
                                                errorMessage = MonerisPayments.ErrorMessageDict[eErrorCode];
                                            }

                                        }
                                        else
                                        {

                                            strReference = strOrderId;
                                            strCardHash = strIssuerId;
                                            strExpirationDateMonth = strExpDate.Substring(2, 2);
                                            strExpirationDateYear = strExpDate.Substring(0, 2);
                                            result = "succeeded";



                                            if ((_session["amount"] != null) && (_session["currency"] != null))
                                            {

                                                int Amount = 0;

                                                try
                                                {
                                                    Amount = Convert.ToInt32(_session["amount"].ToString());
                                                }
                                                catch { }

                                                NumberFormatInfo provider = new NumberFormatInfo();

                                                string CurrencyISOCODE = _session["currency"].ToString();
                                                string strAmount = (Amount / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(CurrencyISOCODE), provider)).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(CurrencyISOCODE), provider);

                                                if (Amount > 0)
                                                {

                                                    string strTransaction2 = "";
                                                    string strAuthCode2 = "";
                                                    string strAuthResult2 = "";
                                                    string strDateTime2 = "";
                                                    string strOrderId2 = MonerisPayments.UserReference();
                                                    DateTime dtExpDate = new DateTime(Convert.ToInt32(strExpirationDateYear) + 2000, Convert.ToInt32(strExpirationDateMonth), 1).AddMonths(1).AddSeconds(-1);
                                                    string strInlineForm = "";



                                                    if (oMonerisConfiguration.MONCON_3DS_TRANSACTIONS.HasValue && oMonerisConfiguration.MONCON_3DS_TRANSACTIONS.Value != 0)
                                                    {

                                                        string strFullURL = _request.Url.AbsoluteUri;
                                                        string strBaseURL = strFullURL.Substring(0, strFullURL.LastIndexOf("/"));
                                                        string strReturnURL = strBaseURL + "/MonerisMPIResponse";
                                                        string strMD = "";


                                                        oPayments.AutomaticTransactionMPIStep1(oMonerisConfiguration.MONCON_API_STORE_ID, oMonerisConfiguration.MONCON_API_STORE_KEY, strOrderId2, strCardReference, strIssuerId, strAmount, oMonerisConfiguration.MONCON_PROCESING_COUNTRY, "",
                                                                                        strCardScheme, dtExpDate, strReturnURL, "Mozilla",
                                                                                        oMonerisConfiguration.MONCON_CHECK_CARD_STATUS != 0, oMonerisConfiguration.MONCON_TEST_MODE != 0, out eErrorCode, out errorMessage,
                                                                                        out strTransaction2, out strAuthCode2, out strAuthResult2, out strDateTime2, out strInlineForm, out strMD);


                                                        if (!string.IsNullOrEmpty(strInlineForm))
                                                        {

                                                            _session["result"] = result;
                                                            _session["errorCode"] = errorCode;
                                                            _session["errorMessage"] = errorMessage;
                                                            _session["cardReference"] = strCardReference;
                                                            _session["cardHash"] = strCardHash;
                                                            _session["cardScheme"] = strCardScheme;
                                                            _session["cardPAN"] = strPAN;
                                                            _session["cardExpMonth"] = strExpirationDateMonth;
                                                            _session["cardExpYear"] = strExpirationDateYear;
                                                            _session["chargeDateTime"] = strMonerisDateTime;
                                                            _session["reference"] = strReference;
                                                            _session["transactionID"] = strTransactionId;
                                                            _session["authCode"] = strAuthCode;
                                                            _session["authResult"] = strAuthResult;
                                                            _session["sessionid"] = strMD;
                                                            _session["DoFinalStep"] = true;
                                                            SaveSession(strMD);
                                                            bExecuteFinally = false;

                                                            Logger_AddLogMessage(string.Format("MonerisResponseHT End (Redirection to MPI): Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                                                                                strCardReference,
                                                                                                _session["email"],
                                                                                                _session["amount"],
                                                                                                _session["currency"],
                                                                                                strRedirectionURLLog), LogLevels.logINFO);


                                                            return Content(strInlineForm); ;
                                                        }

                                                    }
                                                    else
                                                    {
                                                        oPayments.AutomaticTransaction(oMonerisConfiguration.MONCON_API_STORE_ID, oMonerisConfiguration.MONCON_API_STORE_KEY, strOrderId2, strCardReference, strIssuerId, strAmount,oMonerisConfiguration.MONCON_PROCESING_COUNTRY, "",                                                                                        
                                                                                        oMonerisConfiguration.MONCON_CHECK_CARD_STATUS != 0, oMonerisConfiguration.MONCON_TEST_MODE != 0, "", out eErrorCode, out errorMessage2,
                                                                                        out strTransaction2, out strAuthCode2, out strAuthResult2, out strDateTime2);

                                                    }



                                                    if (!MonerisPayments.IsError(eErrorCode))
                                                    {
                                                        DateTime dtNow = DateTime.Now;
                                                        DateTime dtUTCNow = DateTime.UtcNow;

                                                        customersRepository.StartRecharge(oMonerisConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                                                  _session["email"].ToString(),
                                                                                  dtUTCNow,
                                                                                  dtNow,
                                                                                  Amount,
                                                                                  infraestructureRepository.GetCurrencyFromIsoCode(CurrencyISOCODE),
                                                                                  strAuthResult2,
                                                                                  strOrderId2,
                                                                                  strTransaction2,
                                                                                  "",
                                                                                  strDateTime2,
                                                                                  strAuthCode2,
                                                                                  PaymentMeanRechargeStatus.Committed);


                                                        strReference = strOrderId2;
                                                        strMonerisDateTime = strDateTime2;
                                                        strTransactionId = strTransaction2;
                                                        strAuthCode = strAuthCode2;
                                                        strAuthResult = strAuthResult2;
                                                    }
                                                    else
                                                    {
                                                        result = "error";
                                                        errorMessage = errorMessage2;

                                                        if (string.IsNullOrEmpty(errorMessage))
                                                        {
                                                            errorMessage = MonerisPayments.ErrorMessageDict[eErrorCode];
                                                        }
                                                    }



                                                }
                                            }

                                            /* ---- TEST*/
                                            /*
                                            string strTransaction4 = "";
                                            string strAuthCode4 = "";
                                            string strAuthResult4 = "";
                                            string strDateTime4 = "";

                                            oPayments.RefundTransaction(oMonerisConfiguration.MONCON_API_STORE_ID, oMonerisConfiguration.MONCON_API_STORE_KEY, strOrderId2, strTransaction2, "1.00", oMonerisConfiguration.MONCON_PROCESING_COUNTRY, 
                                                                        oMonerisConfiguration.MONCON_TEST_MODE!=0, out eErrorCode, out errorMessage,
                                                                        out strTransaction4, out strAuthCode4, out strAuthResult4, out strDateTime4);


                                            oPayments.DeleteToken(oMonerisConfiguration.MONCON_API_STORE_ID, oMonerisConfiguration.MONCON_API_STORE_KEY, strCardReference, oMonerisConfiguration.MONCON_PROCESING_COUNTRY, 
                                                                    oMonerisConfiguration.MONCON_TEST_MODE!=0, out eErrorCode, out errorMessage);
                                              
                                            */
                                            /**TEST*/

                                        }
                                    }
                                }
                            //}
                            
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "Moneris configuration not found";
                    }


                }
            }
            catch (Exception e)
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = e.Message;
            }
            finally
            {
                if (bExecuteFinally)
                {
                    _session["result"] = result;
                    _session["errorCode"] = errorCode;
                    _session["errorMessage"] = errorMessage;
                    _session["cardReference"] = strCardReference;
                    _session["cardHash"] = strCardHash;
                    _session["cardScheme"] = strCardScheme;
                    _session["cardPAN"] = strPAN;
                    _session["cardExpMonth"] = strExpirationDateMonth;
                    _session["cardExpYear"] = strExpirationDateYear;
                    _session["chargeDateTime"] = strMonerisDateTime;
                    _session["reference"] = strReference;
                    _session["transactionID"] = strTransactionId;
                    _session["authCode"] = strAuthCode;
                    _session["authResult"] = strAuthResult;

                    SaveSession(_session["sessionid"].ToString());

                    strRedirectionURLLog = string.Format("MonerisResult?result={0}" +
                                                        "&errorCode={1}" +
                                                        "&errorMessage={2}" +
                                                        "&cardReference={3}" +
                                                        "&cardHash={4}" +
                                                        "&cardScheme={5}" +
                                                        "&cardPAN={6}" +
                                                        "&cardExpMonth={7}" +
                                                        "&cardExpYear={8}" +
                                                        "&chargeDateTime={9}" +
                                                        "&reference={10}" +
                                                        "&transactionID={11}" +
                                                        "&authCode={12}" +
                                                        "&authResult={13}",
                        _server.UrlEncode(result),
                        _server.UrlEncode(errorCode),
                        _server.UrlEncode(errorMessage),
                        _server.UrlEncode(strCardReference),
                        _server.UrlEncode(strCardHash),
                        _server.UrlEncode(strCardScheme),
                        _server.UrlEncode(strPAN),
                        _server.UrlEncode(strExpirationDateMonth),
                        _server.UrlEncode(strExpirationDateYear),
                        _server.UrlEncode(strMonerisDateTime),
                        _server.UrlEncode(strReference),
                        _server.UrlEncode(strTransactionId),
                        _server.UrlEncode(strAuthCode),
                        _server.UrlEncode(strAuthResult));

                    Logger_AddLogMessage(string.Format("MonerisResponseHT End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                            strCardReference,
                                            _session["email"],
                                            _session["amount"],
                                            _session["currency"],
                                            strRedirectionURLLog), LogLevels.logINFO);

                    strRedirectionURL = string.Format("MonerisResult?r={0}&s={1}", MonerisResultCalc(), _session["sessionid"]);
                }

            }

            return Redirect(strRedirectionURL);
        }


        [HttpGet]
        public ActionResult MonerisSuccess(string response_order_id,
                                           string response_code,
                                           string date_stamp,
                                           string time_stamp,
                                           string eci,
                                           string txn_num,
                                           string bank_approval_code,
                                           string result,
                                           string trans_name,
                                           string gcardholder,
                                           string charge_total,
                                           string card,
                                           string f4l4,
                                           string message,
                                           string iso_code,
                                           string bank_transaction_id,
                                           string expiry_date,
                                           string cvd_response_code,
                                           string email,
                                           string cust_id,
                                           string note)
        {
            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "MonerisResult";
            string strCardHash = "";
            string strCardReference = "";
            string strReference = "";
            string strTransactionId = "";
            string strCardScheme = "";
            string strMonerisDateTime = "";
            string strPAN = "";
            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";
            string strAuthCode = "";
            string strAuthResult = "";


            /*bool testing = false;
            if (testing) { 
                response_order_id = "20180621115941463736";
                date_stamp = "2018-06-21";
                time_stamp = "07:59:51";
                bank_transaction_id="664514240010050490";
                charge_total = "60.00";
                bank_approval_code="713073";
                response_code="027";
                iso_code="01";
                message="APPROVED           *                    =";
                trans_name="purchase";
                gcardholder="TEST TEST";
                f4l4="4273***8010";
                card="V";
                expiry_date="2007";
                result="1";
                eci="7";
                txn_num="77-0_130";
                cvd_response_code="M";
                email="michelinho80@gmail.com";
                cust_id="";
                note="";
            }*/

            try
            {

                if (_session == null) _session = Session;
                if (_server == null) _server = Server;
                if (_request == null) _request = Request;


                if (_session["sessionid"] == null)
                {
                    string[] response_order_id_split= response_order_id.Split('-');
                    LoadSession(response_order_id_split[0]);
                }

                Logger_AddLogMessage(string.Format("MonerisSuccess Begin: ErrorCode={0} ; Token={1} ; Email={2} ; Amount={3} ; Currency={4} ; Guid={5}; ConfigurationID={6}",
                                        "succeeded",
                                        strCardReference,
                                        _session["email"],
                                        _session["amount"],
                                        _session["currency"],
                                        _session["MonerisGuid"],
                                        Configuration_Id
                                        ), LogLevels.logINFO);

                string strGuid = "";

                if (_session["MonerisGuid"] != null)
                {
                    strGuid = _session["MonerisGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid) && Configuration_Id == null)
                {
                    result = "error";
                    errorMessage = "Moneris Guid not found";
                    errorCode = "invalid_configuration";

                }
                else
                {

                    MONERIS_CONFIGURATION oMonerisConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetMonerisConfigurationById(Configuration_Id, out oMonerisConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetMonerisConfiguration(strGuid, out oMonerisConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oMonerisConfiguration == null)
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "Moneris configuration not found";
                        }
                        else
                        {

                            MonerisPayments.MonerisErrorCode eErrorCode;

                            try
                            {
                                eErrorCode = (MonerisPayments.MonerisErrorCode)Convert.ToInt32(response_code);
                            }
                            catch
                            {
                                eErrorCode = MonerisPayments.MonerisErrorCode.InternalError;
                            }

                            errorCode = eErrorCode.ToString();
                            errorMessage = message;

                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                errorMessage = MonerisPayments.ErrorMessageDict[eErrorCode];
                            }

                            if (!MonerisPayments.IsError(eErrorCode))
                            {
                                DateTime dtNow = DateTime.Now;
                                DateTime dtUTCNow = DateTime.UtcNow;
                                int Amount = Convert.ToInt32(_session["amount"]);
                                customersRepository.StartRecharge(oMonerisConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                          email,
                                                          dtUTCNow,
                                                          dtNow,
                                                          Amount,
                                                          infraestructureRepository.GetCurrencyFromIsoCode(_session["currency"].ToString()),
                                                          bank_transaction_id,
                                                          response_order_id,
                                                          txn_num,
                                                          "",
                                                          date_stamp + " " + time_stamp,
                                                          bank_approval_code,
                                                          PaymentMeanRechargeStatus.Committed);                             
                            }

                            /*DateTime dtUTC = (DateTime)_session["utcdate"];


                            if ((oMonerisConfiguration.MONCON_CHECK_DATE_AND_HASH == 1) &&
                                (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) > oMonerisConfiguration.MONCON_CONFIRMATION_TIME))
                            {
                                result = "error";
                                errorMessage = "Invalid DateTime";
                                errorCode = "invalid_datetime";
                                Logger_AddLogMessage(string.Format("MonerisResponse : BeginningDate={0} ; CurrentDate={1}",
                                dtUTC, DateTime.UtcNow), LogLevels.logINFO);
                                if (!MonerisPayments.IsError(eErrorCode))
                                {
                                    customersRepository.FailedRecharge(oMonerisConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                                                 email,
                                                                                 txn_num,
                                                                                 PaymentMeanRechargeStatus.Waiting_Refund);
                                }

                            }
                            else
                            {*/                               
                                if (MonerisPayments.IsError(eErrorCode))
                                {
                                    result = "error";
                                }
                                else
                                {                                   
                                    MonerisPayments oPayments = new MonerisPayments();



                                    string errorMessage2 = "";
                                    oPayments.GetTokenFromTransaction(oMonerisConfiguration.MONCON_API_STORE_ID, oMonerisConfiguration.MONCON_API_STORE_KEY, response_order_id,
                                        txn_num, email, oMonerisConfiguration.MONCON_PROCESING_COUNTRY, oMonerisConfiguration.MONCON_TEST_MODE != 0, out eErrorCode, out errorMessage2, out strCardReference);

                                    if (MonerisPayments.IsError(eErrorCode))
                                    {
                                        result = "error";
                                        errorCode = "null_token";

                                        customersRepository.FailedRecharge(oMonerisConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                                           email,
                                                                           txn_num,
                                                                           PaymentMeanRechargeStatus.Waiting_Refund);


                                        Logger_AddLogMessage(string.Format("MonerisSuccess:GetTokenFromTransaction : errorCode={0} ; errorMessage={1}",
                                                  errorCode, errorMessage2), LogLevels.logINFO);

                                        errorMessage = errorMessage2;

                                        if (string.IsNullOrEmpty(errorMessage))
                                        {
                                            errorMessage = MonerisPayments.ErrorMessageDict[eErrorCode];
                                        }

                                    }
                                    else
                                    {

                                        strReference = response_order_id;
                                        strAuthCode = bank_approval_code;
                                        strAuthResult = bank_transaction_id;
                                        strCardHash = strCardReference;
                                        strCardScheme = card;
                                        strMonerisDateTime = date_stamp + " " + time_stamp;
                                        strPAN = f4l4;
                                        strTransactionId = txn_num;
                                        strExpirationDateMonth = expiry_date.Substring(2,2);
                                        strExpirationDateYear = expiry_date.Substring(0,2);
                                        result = "succeeded";
                                      


                                        /* ---- TEST*/
                                        /*string strTransaction2 = "";
                                        string strAuthCode2 = "";
                                        string strAuthResult2 = "";
                                        string strDateTime2 = "";
                                        string strOrderId2 = MonerisPayments.UserReference();
                                        DateTime dtExpDate = new DateTime(Convert.ToInt32(strExpirationDateYear) + 2000, Convert.ToInt32(strExpirationDateMonth), 1).AddMonths(1).AddSeconds(-1);
                                        string strInlineForm = "";

                                        string strFullURL = Request.Url.AbsoluteUri;
                                        string strBaseURL = strFullURL.Substring(0,strFullURL.LastIndexOf("/"));
                                        string strReturnURL = strBaseURL + "/MonerisMPIResponse";
                                        string strMD = "";



                                        oPayments.AutomaticTransactionMPIStep1(oMonerisConfiguration.MONCON_API_STORE_ID, oMonerisConfiguration.MONCON_API_STORE_KEY, strOrderId2, strCardReference, "" ,  "1.00", oMonerisConfiguration.MONCON_PROCESING_COUNTRY,"",
                                                                       strCardScheme, dtExpDate, strReturnURL, "Mozilla",
                                                                       oMonerisConfiguration.MONCON_CHECK_CARD_STATUS!=0,  oMonerisConfiguration.MONCON_TEST_MODE!=0, out eErrorCode, out errorMessage,
                                                                       out strTransaction2, out strAuthCode2,out strAuthResult2,out strDateTime2, out strInlineForm, out strMD);
                                       

                                        
                                        if (!string.IsNullOrEmpty(strInlineForm))
                                        {
                                            _session["result"] = result;
                                            _session["errorCode"] = errorCode;
                                            _session["errorMessage"] = errorMessage;
                                            _session["cardReference"] = strCardReference;
                                            _session["cardHash"] = strCardHash;
                                            _session["cardScheme"] = strCardScheme;
                                            _session["cardPAN"] = strPAN;
                                            _session["cardExpMonth"] = strExpirationDateMonth;
                                            _session["cardExpYear"] = strExpirationDateYear;
                                            _session["chargeDateTime"] = strMonerisDateTime;
                                            _session["reference"] = strReference;
                                            _session["transactionID"] = strTransactionId;
                                            _session["authCode"] = strAuthCode;
                                            _session["authResult"] = strAuthResult;
                                            _session["sessionid"] = strMD;
                                            SaveSession(strMD);
                                            return Content(strInlineForm); ;
                                        }


                                        string strTransaction3 = "";
                                        string strAuthCode3 = "";
                                        string strAuthResult3 = "";
                                        string strDateTime3 = "";

                                        oPayments.RefundTransaction(oMonerisConfiguration.MONCON_API_STORE_ID, oMonerisConfiguration.MONCON_API_STORE_KEY, response_order_id, strTransactionId, "1.00", oMonerisConfiguration.MONCON_PROCESING_COUNTRY, 
                                                                    oMonerisConfiguration.MONCON_TEST_MODE!=0, out eErrorCode, out errorMessage,
                                                                    out strTransaction3, out strAuthCode3,out strAuthResult3,out strDateTime3);

                                        string strTransaction4 = "";
                                        string strAuthCode4 = "";
                                        string strAuthResult4 = "";
                                        string strDateTime4 = "";

                                        oPayments.RefundTransaction(oMonerisConfiguration.MONCON_API_STORE_ID, oMonerisConfiguration.MONCON_API_STORE_KEY, strOrderId2, strTransaction2, "1.00", oMonerisConfiguration.MONCON_PROCESING_COUNTRY, 
                                                                    oMonerisConfiguration.MONCON_TEST_MODE!=0, out eErrorCode, out errorMessage,
                                                                    out strTransaction4, out strAuthCode4, out strAuthResult4, out strDateTime4);


                                        oPayments.DeleteToken(oMonerisConfiguration.MONCON_API_STORE_ID, oMonerisConfiguration.MONCON_API_STORE_KEY, strCardReference, oMonerisConfiguration.MONCON_PROCESING_COUNTRY, 
                                                              oMonerisConfiguration.MONCON_TEST_MODE!=0, out eErrorCode, out errorMessage);
                                         */
                                        /**TEST*/
                                        

                                    }
                                }
                            //}
                            
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "Moneris configuration not found";
                    }


                }
            }
            catch (Exception e)
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = e.Message;
            }
            finally
            {
                _session["result"] = result;
                _session["errorCode"] = errorCode;
                _session["errorMessage"] = errorMessage;
                _session["cardReference"] = strCardReference;
                _session["cardHash"] = strCardHash;
                _session["cardScheme"] = strCardScheme;
                _session["cardPAN"] = strPAN;
                _session["cardExpMonth"] = strExpirationDateMonth;
                _session["cardExpYear"] = strExpirationDateYear;
                _session["chargeDateTime"] = strMonerisDateTime;
                _session["reference"] = strReference;
                _session["transactionID"] = strTransactionId;
                _session["authCode"] = strAuthCode;
                _session["authResult"] = strAuthResult;
                SaveSession(_session["sessionid"].ToString());

                strRedirectionURLLog = string.Format("MonerisResult?result={0}" +
                                                    "&errorCode={1}" +
                                                    "&errorMessage={2}" +
                                                    "&cardReference={3}" +
                                                    "&cardHash={4}" +
                                                    "&cardScheme={5}" +
                                                    "&cardPAN={6}" +
                                                    "&cardExpMonth={7}" +
                                                    "&cardExpYear={8}" +
                                                    "&chargeDateTime={9}" +
                                                    "&reference={10}" +
                                                    "&transactionID={11}" +
                                                    "&authCode={12}"+
                                                    "&authResult={13}",
                    _server.UrlEncode(result),
                    _server.UrlEncode(errorCode),
                    _server.UrlEncode(errorMessage),
                    _server.UrlEncode(strCardReference),
                    _server.UrlEncode(strCardHash),
                    _server.UrlEncode(strCardScheme),
                    _server.UrlEncode(strPAN),
                    _server.UrlEncode(strExpirationDateMonth),
                    _server.UrlEncode(strExpirationDateYear),
                    _server.UrlEncode(strMonerisDateTime),
                    _server.UrlEncode(strReference),
                    _server.UrlEncode(strTransactionId),
                    _server.UrlEncode(strAuthCode),
                    _server.UrlEncode(strAuthResult));

                Logger_AddLogMessage(string.Format("MonerisSuccess End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        strCardReference,
                                        _session["email"],
                                        _session["amount"],
                                        _session["currency"],
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("MonerisResult?r={0}&s={1}", MonerisResultCalc(), _session["sessionid"]);

            }

            return Redirect(strRedirectionURL);
        }

        [HttpGet]
        public ActionResult MonerisFailure(string response_order_id,
                                           string response_code,
                                           string date_stamp,
                                           string time_stamp,
                                           string message)
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "MonerisResult";
            string strAuthResult = "";
            string strCardHash = "";
            string strCardReference = "";
            string strReference = "";
            string strTransactionId = "";
            string strCardScheme = "";
            string strMonerisDateTime = "";
            string strPAN = "";
            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";
            string strAuthCode = "";


            try
            {
                if (_session == null) _session = Session;
                if (_server == null) _server = Server;
                if (_request == null) _request = Request;

                strAuthResult = _request["moneris_auth_result"];

                if (_session["orderid"] == null)
                {
                    string[] response_order_id_split = response_order_id.Split('-');
                    LoadSession(response_order_id_split[0]);
                }

                Logger_AddLogMessage(string.Format("MonerisFailure Begin: ErrorCode={0} ; Token={1} ; Email={2} ; Amount={3} ; Currency={4}; Guid={5}, ConfigurationID={6}",
                                        "error",
                                        "",
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                        _session["MonerisGuid"],
                                        Configuration_Id
                                        ), LogLevels.logINFO);




                string strGuid = "";

                if (_session["MonerisGuid"] != null)
                {
                    strGuid = _session["MonerisGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid) && Configuration_Id == null)
                {
                    result = "error";
                    errorMessage = "Moneris Guid not found";
                    errorCode = "invalid_configuration";

                }
                else
                {

                    MONERIS_CONFIGURATION oMonerisConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetMonerisConfigurationById(Configuration_Id, out oMonerisConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetMonerisConfiguration(strGuid, out oMonerisConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oMonerisConfiguration == null)
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "Moneris configuration not found";
                        }
                        else
                        {

                            /*DateTime dtUTC = (DateTime)_session["utcdate"];


                            if ((oMonerisConfiguration.MONCON_CHECK_DATE_AND_HASH == 1) &&
                                (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) > oMonerisConfiguration.MONCON_CONFIRMATION_TIME))
                            {
                                result = "error";
                                errorMessage = "Invalid DateTime";
                                errorCode = "invalid_datetime";
                                Logger_AddLogMessage(string.Format("MonerisResponse : BeginningDate={0} ; CurrentDate={1}",
                                dtUTC, DateTime.UtcNow), LogLevels.logINFO);

                            }
                            else
                            {*/
                                result = "error";

                                MonerisPayments.MonerisErrorCode eErrorCode;

                                try
                                {
                                    eErrorCode = (MonerisPayments.MonerisErrorCode)Convert.ToInt32(response_code);
                                }
                                catch
                                {
                                    eErrorCode = MonerisPayments.MonerisErrorCode.InternalError;
                                }

                                if (eErrorCode == MonerisPayments.MonerisErrorCode.CancelledByCardHolder)
                                {

                                    errorCode = "transaction_cancelled";
                                }
                                else
                                {
                                    errorCode = "transaction_failed";
                                }

                                
                                errorMessage = message;

                                if (string.IsNullOrEmpty(errorMessage))
                                {
                                    errorMessage = MonerisPayments.ErrorMessageDict[eErrorCode];
                                }
                            //}
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "Moneris configuration not found";
                    }


                }
            }
            catch (Exception e)
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = e.Message;
            }
            finally
            {
                _session["result"] = result;
                _session["errorCode"] = errorCode;
                _session["errorMessage"] = errorMessage;
                _session["cardReference"] = strCardReference;
                _session["cardHash"] = strCardHash;
                _session["cardScheme"] = strCardScheme;
                _session["cardPAN"] = strPAN;
                _session["cardExpMonth"] = strExpirationDateMonth;
                _session["cardExpYear"] = strExpirationDateYear;
                _session["chargeDateTime"] = strMonerisDateTime;
                _session["reference"] = strReference;
                _session["transactionID"] = strTransactionId;
                _session["authCode"] = strAuthCode;
                _session["authResult"] = strAuthResult;
                SaveSession(_session["sessionid"].ToString());

                strRedirectionURLLog = string.Format("MonerisResult?result={0}" +
                                                    "&errorCode={1}" +
                                                    "&errorMessage={2}" +
                                                    "&cardReference={3}" +
                                                    "&cardHash={4}" +
                                                    "&cardScheme={5}" +
                                                    "&cardPAN={6}" +
                                                    "&cardExpMonth={7}" +
                                                    "&cardExpYear={8}" +
                                                    "&chargeDateTime={9}" +
                                                    "&reference={10}" +
                                                    "&transactionID={11}" +
                                                    "&authCode={12}"+
                                                    "&authResult={13}",
                    _server.UrlEncode(result),
                    _server.UrlEncode(errorCode),
                    _server.UrlEncode(errorMessage),
                    _server.UrlEncode(strCardReference),
                    _server.UrlEncode(strCardHash),
                    _server.UrlEncode(strCardScheme),
                    _server.UrlEncode(strPAN),
                    _server.UrlEncode(strExpirationDateMonth),
                    _server.UrlEncode(strExpirationDateYear),
                    _server.UrlEncode(strMonerisDateTime),
                    _server.UrlEncode(strReference),
                    _server.UrlEncode(strTransactionId),
                    _server.UrlEncode(strAuthCode),
                    _server.UrlEncode(strAuthResult));

                Logger_AddLogMessage(string.Format("MonerisFailure End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        strCardReference,
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("MonerisResult?r={0}&s={1}", MonerisResultCalc(), _session["sessionid"]);

            }

            return Redirect(strRedirectionURL);
        }


        [HttpPost]
        public ActionResult MonerisMPIResponse()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            if (_session["sessionid"] == null)
            {
                LoadSession(_request["MD"]);
            }

            Logger_AddLogMessage(string.Format("MonerisMPIResponse Post: Guid={0}; ConfigurationID={1}", _session["MonerisGuid"], Configuration_Id), LogLevels.logINFO);


            bool bDoFinalStep = false;
            if (_session["DoFinalStep"] != null)
            {
                try
                {

                    bDoFinalStep = Convert.ToBoolean(_session["DoFinalStep"].ToString());
                }
                catch { }

                _session["DoFinalStep"] = null;
            }

            return MonerisMPIResponse(_request["PaRes"],
                                      _request["MD"],
                                      bDoFinalStep);
        }

        [HttpGet]
        public ActionResult MonerisMPIResponse(string PaRes,
                                                string MD,
                                                bool bDoFinalStep)
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "MonerisResult";
            string strCAVV = "";
            string strECI = "";


            string strCardHash = "";
            string strCardReference = "";
            string strReference = "";
            string strTransactionId = "";
            string strCardScheme = "";
            string strMonerisDateTime = "";
            string strPAN = "";
            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";
            string strAuthCode = "";
            string strAuthResult = "";

            try
            {
                if (_session == null) _session = Session;
                if (_server == null) _server = Server;
                if (_request == null) _request = Request;


                if (_session["sessionid"] == null)
                {
                    LoadSession(MD);
                }

                _session["DoFinalStep"] = null;


                Logger_AddLogMessage(string.Format("MonerisMPIResponse Begin: Email={0} ; Guid={1}, MD={2}, PaRes={3}, ",
                                        _session["email"].ToString(),
                                        _session["MonerisGuid"],
                                        MD,
                                        PaRes
                                        ), LogLevels.logINFO);




                string strGuid = "";

                if (_session["MonerisGuid"] != null)
                {
                    strGuid = _session["MonerisGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid) && Configuration_Id == null)
                {
                    result = "error";
                    errorMessage = "Moneris Guid not found";
                    errorCode = "invalid_configuration";

                }
                else
                {

                    MONERIS_CONFIGURATION oMonerisConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetMonerisConfigurationById(Configuration_Id, out oMonerisConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetMonerisConfiguration(strGuid, out oMonerisConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oMonerisConfiguration == null)
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "Moneris configuration not found";
                        }
                        else
                        {

                            DateTime dtUTC = (DateTime)_session["utcdate"];


                            /*if ((oMonerisConfiguration.MONCON_CHECK_DATE_AND_HASH == 1) &&
                                (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) > oMonerisConfiguration.MONCON_CONFIRMATION_TIME))
                            {
                                result = "error";
                                errorMessage = "Invalid DateTime";
                                errorCode = "invalid_datetime";
                                Logger_AddLogMessage(string.Format("MonerisMPIResponse : BeginningDate={0} ; CurrentDate={1}",
                                dtUTC, DateTime.UtcNow), LogLevels.logINFO);

                            }
                            else
                            {*/

                                MonerisPayments oPayments = new MonerisPayments();
                                MonerisPayments.MonerisErrorCode eErrorCode;
                               
                              
                                oPayments.AutomaticTransactionMPIStep2(oMonerisConfiguration.MONCON_API_STORE_ID, oMonerisConfiguration.MONCON_API_STORE_KEY, oMonerisConfiguration.MONCON_PROCESING_COUNTRY,
                                    PaRes, MD, oMonerisConfiguration.MONCON_CHECK_CARD_STATUS != 0, oMonerisConfiguration.MONCON_TEST_MODE != 0, out eErrorCode, out errorMessage,
                                    out strCAVV, out strECI);


                                if (MonerisPayments.IsError(eErrorCode))
                                {
                                    result = "error";
                                    errorCode = "3ds_rejection";



                                    Logger_AddLogMessage(string.Format("MonerisMPIResponse:AutomaticTransactionMPIStep2 : errorCode={0} ; errorMessage={1}",
                                              errorCode, errorMessage), LogLevels.logINFO);

                                    if (string.IsNullOrEmpty(errorMessage))
                                    {
                                        errorMessage = MonerisPayments.ErrorMessageDict[eErrorCode];
                                    }

                                }
                                else
                                {                                    
                                    result = "succeeded";

                                    if (bDoFinalStep)
                                    {

                                        if ((_session["amount"] != null) && (_session["currency"] != null))
                                        {

                                            int Amount = 0;

                                            try
                                            {
                                                Amount = Convert.ToInt32(_session["amount"].ToString());
                                            }
                                            catch { }

                                            NumberFormatInfo provider = new NumberFormatInfo();
                                            string CurrencyISOCODE = _session["currency"].ToString();
                                            string strAmount = (Amount / Convert.ToDouble(infraestructureRepository.GetCurrencyDivisorFromIsoCode(CurrencyISOCODE), provider)).ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(CurrencyISOCODE), provider);

                                            if (Amount > 0)
                                            {


                                                strCardReference = _session["cardReference"].ToString();
                                                string strIssuerId = _session["cardHash"].ToString();
                                                string strOrderId = MonerisPayments.UserReference();
                                                string strDateTime = "";
                                                oPayments.AutomaticTransactionMPIStep3(oMonerisConfiguration.MONCON_API_STORE_ID, oMonerisConfiguration.MONCON_API_STORE_KEY, strOrderId, strCardReference, strIssuerId, strAmount, oMonerisConfiguration.MONCON_PROCESING_COUNTRY, "",
                                                                                   oMonerisConfiguration.MONCON_CHECK_CARD_STATUS != 0, oMonerisConfiguration.MONCON_TEST_MODE != 0, strCAVV, strECI, out eErrorCode, out errorMessage,
                                                                                   out strTransactionId, out strAuthCode, out strAuthResult, out strDateTime);



                                                if (MonerisPayments.IsError(eErrorCode))
                                                {
                                                    result = "error";



                                                    Logger_AddLogMessage(string.Format("MonerisMPIResponse:AutomaticTransactionMPIStep3 : errorCode={0} ; errorMessage={1}",
                                                              errorCode, errorMessage), LogLevels.logINFO);

                                                    if (string.IsNullOrEmpty(errorMessage))
                                                    {
                                                        errorMessage = MonerisPayments.ErrorMessageDict[eErrorCode];
                                                    }
                                                }
                                                else
                                                {
                                                    _session["MD"] = null;
                                                    _session["CAVV"] = null;
                                                    _session["ECI"] = null;
                                                    strReference = strOrderId;
                                                    strCardHash = strIssuerId;
                                                    strCardScheme = _session["cardScheme"].ToString(); ;
                                                    strMonerisDateTime = strDateTime;
                                                    strPAN = _session["cardPAN"].ToString() ;
                                                    strExpirationDateMonth = _session["cardExpMonth"].ToString();
                                                    strExpirationDateYear = _session["cardExpYear"].ToString();
                                                    result = "succeeded";


                                                    DateTime dtNow = DateTime.Now;
                                                    DateTime dtUTCNow = DateTime.UtcNow;

                                                    customersRepository.StartRecharge(oMonerisConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                                              _session["email"].ToString(),
                                                                              dtUTCNow,
                                                                              dtNow,
                                                                              Amount,
                                                                              infraestructureRepository.GetCurrencyFromIsoCode(CurrencyISOCODE),
                                                                              strAuthResult,
                                                                              strOrderId,
                                                                              strTransactionId,
                                                                              "",
                                                                              strDateTime,
                                                                              strAuthCode,
                                                                              PaymentMeanRechargeStatus.Committed);





                                                }
                                            }
                                        }


                                    }
                                    /*TEST*/

                                    /*

                                    string strTransaction3 = "";
                                    string strAuthCode3 = "";
                                    string strAuthResult3 = "";
                                    string strDateTime3 = "";

                                    oPayments.RefundTransaction(oMonerisConfiguration.MONCON_API_STORE_ID, oMonerisConfiguration.MONCON_API_STORE_KEY, strOrderId, strTransactionId, "1.00", oMonerisConfiguration.MONCON_PROCESING_COUNTRY,
                                                                oMonerisConfiguration.MONCON_TEST_MODE != 0, out eErrorCode, out errorMessage,
                                                                out strTransaction3, out strAuthCode3, out strAuthResult3, out strDateTime3);

                                    


                                    oPayments.DeleteToken(oMonerisConfiguration.MONCON_API_STORE_ID, oMonerisConfiguration.MONCON_API_STORE_KEY, strCardReference, oMonerisConfiguration.MONCON_PROCESING_COUNTRY,
                                                          oMonerisConfiguration.MONCON_TEST_MODE != 0, out eErrorCode, out errorMessage);

                                    */
                                     /*TEST */
                                }


                            //}
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "Moneris configuration not found";
                    }


                }
            }
            catch (Exception e)
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = e.Message;
            }
            finally
            {

                if (!bDoFinalStep)
                {

                    _session["result"] = result;
                    _session["errorCode"] = errorCode;
                    _session["errorMessage"] = errorMessage;
                    _session["MD"] = MD;
                    _session["CAVV"] = strCAVV;
                    _session["ECI"] = strECI;
                    SaveSession(_session["sessionid"].ToString());

                    strRedirectionURLLog = string.Format("MonerisResult?result={0}" +
                                                        "&errorCode={1}" +
                                                        "&errorMessage={2}" +
                                                        "&MD={3}" +
                                                        "&CAVV={4}" +
                                                        "&ECI={5}",
                        _server.UrlEncode(result),
                        _server.UrlEncode(errorCode),
                        _server.UrlEncode(errorMessage),
                        _server.UrlEncode(MD),
                        _server.UrlEncode(strCAVV),
                        _server.UrlEncode(strECI));


                    Logger_AddLogMessage(string.Format("MonerisMPIResponse(FinalStep=0) End: Email={0}  ;MD={1} ;CAVV={2}; ECI={3}; ResultURL={4}",
                                            _session["email"].ToString(),
                                            MD, strCAVV, strECI,
                                            strRedirectionURLLog), LogLevels.logINFO);


                }
                else
                {

                    _session["result"] = result;
                    _session["errorCode"] = errorCode;
                    _session["errorMessage"] = errorMessage;
                    _session["cardReference"] = strCardReference;
                    _session["cardHash"] = strCardHash;
                    _session["cardScheme"] = strCardScheme;
                    _session["cardPAN"] = strPAN;
                    _session["cardExpMonth"] = strExpirationDateMonth;
                    _session["cardExpYear"] = strExpirationDateYear;
                    _session["chargeDateTime"] = strMonerisDateTime;
                    _session["reference"] = strReference;
                    _session["transactionID"] = strTransactionId;
                    _session["authCode"] = strAuthCode;
                    _session["authResult"] = strAuthResult;
                    SaveSession(_session["sessionid"].ToString());

                    strRedirectionURLLog = string.Format("MonerisResult?result={0}" +
                                                        "&errorCode={1}" +
                                                        "&errorMessage={2}" +
                                                        "&cardReference={3}" +
                                                        "&cardHash={4}" +
                                                        "&cardScheme={5}" +
                                                        "&cardPAN={6}" +
                                                        "&cardExpMonth={7}" +
                                                        "&cardExpYear={8}" +
                                                        "&chargeDateTime={9}" +
                                                        "&reference={10}" +
                                                        "&transactionID={11}" +
                                                        "&authCode={12}" +
                                                        "&authResult={13}",
                        _server.UrlEncode(result),
                        _server.UrlEncode(errorCode),
                        _server.UrlEncode(errorMessage),
                        _server.UrlEncode(strCardReference),
                        _server.UrlEncode(strCardHash),
                        _server.UrlEncode(strCardScheme),
                        _server.UrlEncode(strPAN),
                        _server.UrlEncode(strExpirationDateMonth),
                        _server.UrlEncode(strExpirationDateYear),
                        _server.UrlEncode(strMonerisDateTime),
                        _server.UrlEncode(strReference),
                        _server.UrlEncode(strTransactionId),
                        _server.UrlEncode(strAuthCode),
                        _server.UrlEncode(strAuthResult));

                    Logger_AddLogMessage(string.Format("MonerisMPIResponse(FinalStep=1) End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                            strCardReference,
                                            _session["email"],
                                            _session["amount"],
                                            _session["currency"],
                                            strRedirectionURLLog), LogLevels.logINFO);

                }



                strRedirectionURL = string.Format("MonerisResult?r={0}&s={1}", MonerisResultCalc(), _session["sessionid"]);

            }

            return Redirect(strRedirectionURL);



        }


        [HttpGet]
        public ActionResult MonerisResult(string r, string s)
        {
            if (string.IsNullOrEmpty(r))
            {
                return new HttpNotFoundResult("");
            }
            else
            {
                if (_session == null) _session = Session;
                if (_server == null) _server = Server;
                if (_request == null) _request = Request;

                if (_session["sessionid"] == null)
                {
                    LoadSession(s);
                }


                string strResultDec = DecryptCryptResult(r, _session["HashSeed"] != null ? _session["HashSeed"].ToString() : string.Empty);
                ViewData["Result"] = strResultDec;
                if (_session["PayerQuantity"] != null) ViewData["PayerQuantity"] = _session["PayerQuantity"];
                if (_session["PayerCurrencyISOCode"] != null) ViewData["PayerCurrencyISOCode"] = _session["PayerCurrencyISOCode"];
                if (_session["UserBalance"] != null) ViewData["UserBalance"] = _session["UserBalance"];


                Logger_AddLogMessage(string.Format("MonerisResult Begin: ReturnURL={0}; ReturnByGet={1} ; ExternalId={2}",
                                        _session["ReturnURL"] != null?_session["ReturnURL"].ToString():"",
                                        _session["ReturnByGet"] != null?_session["ReturnByGet"].ToString():"",
                                        _session["ExternalId"] != null?_session["ExternalId"].ToString():""), LogLevels.logINFO);

                if (_session["ReturnURL"] != null && !string.IsNullOrEmpty(_session["ReturnURL"].ToString()))
                {
                    Dictionary<string, object> postData = new Dictionary<string, object>();
                    if (_session["ReturnByGet"] != null && _session["ReturnByGet"].ToString() == "1")
                    {
                        string url = string.Empty;
                        if (_session["ExternalId"] != null && !string.IsNullOrEmpty(_session["ExternalId"].ToString()))
                        {
                            url = string.Format("{0}?r={1}&ExternalId={2}", _session["ReturnURL"].ToString(), r, _session["ExternalId"].ToString());
                        }
                        else 
                        {
                            url = string.Format("{0}?r={1}", _session["ReturnURL"].ToString(), r);
                        }

                        Logger_AddLogMessage(string.Format("MonerisResult: ReturnURL with QueryString={0};", url), LogLevels.logINFO);
                        
                        return Redirect(url);
                    }
                    else 
                    {                        
                        if (_session["ExternalId"] != null && !string.IsNullOrEmpty(_session["ExternalId"].ToString()))
                        {
                            postData.Add("ExternalId", _session["ExternalId"].ToString());
                        }

                        postData.Add("r", r);

                        RedirectWithData(_session["ReturnURL"].ToString(), postData);
                    }
                }    
            


                else
                {

                    if (_session["PAYMENT_ORIGIN"] != null && _session["PAYMENT_ORIGIN"].ToString() == "AccountController")
                    {
                        _session["result"] = null;
                        _session["errorCode"] = null;
                        _session["errorMessage"] = null;
                        _session["cardReference"] = null;
                        _session["cardHash"] = null;
                        _session["cardScheme"] = null;
                        _session["cardPAN"] = null;
                        _session["cardExpMonth"] = null;
                        _session["cardExpYear"] = null;
                        _session["chargeDateTime"] = null;
                        _session["reference"] = null;
                        _session["transactionID"] = null;
                        _session["authCode"] = null;
                        _session["authResult"] = null;
                        _session["email"] = null;
                        _session["amount"] = null;
                        _session["currency"] = null;
                        _session["utcdate"] = null;
                        _session["MonerisGuid"] = null;
                        _session["ResultURL"] = null;
                        _session["orderid"] = null;
                        _session["MD"] = null;
                        _session["CAVV"] = null;
                        _session["ECI"] = null;
                        _session["ExternalId"] = null;

                        string sSuffix = string.Empty;
                        if (_session["Suffix"] != null)
                        {
                            sSuffix = _session["Suffix"].ToString();
                        }
                        return RedirectToAction("MonerisResult" + sSuffix, "Account", new { r = r });
                    }
                    else if (_session["PAYMENT_ORIGIN"] != null && _session["PAYMENT_ORIGIN"].ToString() == "FineController")
                    {
                        return RedirectToAction("MonerisResult", "Fine", new { r = r });
                    }
                                                            
                }

                return View();
            }

        }

        private string MonerisResultCalc()
        {

            string strRes = "";

            Dictionary<string, object> oDataDict = new Dictionary<string, object>();

            oDataDict["sessionid"] = _session["sessionid"];
            oDataDict["email"] = _session["email"];
            oDataDict["amount"] = _session["amount"];
            oDataDict["currency"] = _session["currency"];
            oDataDict["result"] = _session["result"];
            oDataDict["errorCode"] = _session["errorCode"];
            oDataDict["errorMessage"] = _session["errorMessage"];


            if (_session["AmountCurrencyIsoCode"] != null) oDataDict["PayTickets_AmountCurrencyIsoCode"] = _session["AmountCurrencyIsoCode"];
            if (_session["Total"] != null) oDataDict["PayTickets_Total"] = _session["Total"];
            if (_session["Plate"] != null) oDataDict["PayTickets_Plate"] = _session["Plate"];
            if (_session["QFEE"] != null) oDataDict["PayTickets_QFEE"] = _session["QFEE"];
            if (_session["QVAT"] != null) oDataDict["PayTickets_QVAT"] = _session["QVAT"];
            if (_session["TotalQuantity"] != null) oDataDict["PayTickets_TotalQuantity"] = _session["TotalQuantity"];

            if (_session["result"] != null && _session["result"].ToString() == "succeeded"&& _session["CAVV"]==null)
            {
                            
                oDataDict["moneris_card_reference"] = _session["cardReference"];
                oDataDict["moneris_card_hash"] = _session["cardHash"];
                oDataDict["moneris_card_scheme"] = _session["cardScheme"];
                oDataDict["moneris_masked_card_number"] = _session["cardPAN"];
                oDataDict["moneris_expires_end_month"] = _session["cardExpMonth"];
                oDataDict["moneris_expires_end_year"] = _session["cardExpYear"];
                oDataDict["moneris_date_time_local_fmt"] = _session["chargeDateTime"];
                oDataDict["moneris_reference"] = _session["reference"];
                oDataDict["moneris_transaction_id"] = _session["transactionID"];
                oDataDict["moneris_auth_code"] = _session["authCode"];
                oDataDict["moneris_auth_result"] = _session["authResult"];
            }
            else if (_session["result"] != null && _session["result"].ToString() == "succeeded" && _session["CAVV"] != null)
            {
                string strECI="";
                if (_session["ECI"]!=null)
                {
                    strECI=_session["ECI"].ToString();
                    if (string.IsNullOrEmpty(strECI))
                    {
                        strECI="";
                    }

                }
                oDataDict["moneris_md"] = _session["MD"];
                oDataDict["moneris_cavv"] = _session["CAVV"];
                oDataDict["moneris_eci"] = strECI;
            }


            var json = JsonConvert.SerializeObject(oDataDict);

            Logger_AddLogMessage(string.Format("MonerisResultCalc: {0}",
                                 PrettyJSON(json)), LogLevels.logINFO);

            strRes = CalculateCryptResult(json, _session["HashSeed"] != null ? _session["HashSeed"].ToString() : string.Empty);

            return strRes;


        }
        
        

        private string CalculateHash(string Guid, string Email, int Amount, string CurrencyISOCODE, string Description, string UTCDate, string ReturnURL, string lang, string ExternalId, string strHashSeed)
        {
            string strHashString = Guid + Email + Amount.ToString() + CurrencyISOCODE + Description + UTCDate + ReturnURL + lang + ExternalId;

            Logger_AddLogMessage(string.Format("CalculateHash : HashString = '{0}' - HashSeed = '{1}'", strHashString, strHashSeed), LogLevels.logINFO);

            return CalculateHash(strHashString, strHashSeed);

        }


        private string CalculateHash(string Guid, string id, string MD, string Email, string UTCDate, string lang, string ReturnURL, string ExternalId, string strHashSeed)
        {
            string strHashString = Guid + id + MD + Email + UTCDate + lang + ReturnURL + ExternalId;

            return CalculateHash(strHashString, strHashSeed);

        }

    }
}
