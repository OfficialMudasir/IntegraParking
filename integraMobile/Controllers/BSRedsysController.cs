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
using integraMobile.Web.Resources;
using integraMobile.Models;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Invoicing;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;
using integraMobile.Infrastructure.RedsysAPI;

namespace integraMobile.Controllers
{
    [HandleError]
    [NoCache]
    public class BSRedsysController : BaseCCController
    {

        public BSRedsysController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;          
        }

        public BSRedsysController(ICustomersRepository customersRepository, IInfraestructureRepository infraestructureRepository, System.Web.HttpRequestBase iRequest, System.Web.HttpServerUtilityBase iServer, System.Web.HttpSessionStateBase iSession, decimal? Config_id)
        {
            this.customersRepository = customersRepository;
            this.infraestructureRepository = infraestructureRepository;            
            _request = iRequest;
            _server = iServer;
            _session = iSession;
            bAvoidHashCheck = true;
            Configuration_Id = Config_id;
        }

        /*
          http://localhost:4091/BSRedsys/BSRedsysRequest?Guid=e53f878f-a15b-4343-9545-2fd6c69b171f&Email=febermejo@gmail.com&Amount=5&CurrencyISOCODE=EUR&Description=fasdfasddfa&UTCDate=231200111217&Hash=32423
          https://www.iparkme.com/PreProd/integraMobile/Moneris/MonerisRequest?Guid=3db29743-d641-4bf7-a23f-d9ecfdf6481f&Email=febermejo@gmail.com&Amount=100&CurrencyISOCODE=CAD&Description=fasdfasddfa&UTCDate=231200111217&Hash=32423
          https://www.iparkme.com/Dev/integraMobile/Moneris/MonerisRequest?Guid=9ff9dba3-8a3a-4db5-aa64-39dd421b80a0&Email=febermejo@gmail.com&Amount=100&CurrencyISOCODE=CAD&Description=fasdfasddfa&UTCDate=231200111217&Hash=32423
         */


        [HttpPost]
        public ActionResult BSRedsysRequest()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            return BSRedsysRequest(_request["Guid"],
                                _request["Email"],
                                (_request["Amount"] != null ? Convert.ToInt32(_request["Amount"]) : (int?)null),                                
                                _request["CurrencyISOCode"],
                                _request["Description"],
                                _request["UTCDate"],
                                _request["Culture"],
                                _request["ReturnURL"],                                
                                _request["Hash"]);
        }

        [HttpGet]
        public ActionResult BSRedsysRequest(string Guid, string Email, int? Amount, string CurrencyISOCode, string Description, string UTCDate, string Culture, string ReturnURL, string Hash)
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";

            try
            {
                if (_session == null) _session = Session;
                if (_server == null) _server = Server;
                if (_request == null) _request = Request;                

                _session["result"] = null;
                _session["errorCode"] = null;
                _session["errorMessage"] = null;
                _session["email"] = null;
                _session["amount"] = null;
                _session["currency"] = null;
                _session["utcdate"] = null;
                _session["BSRedsysGuid"] = null;
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
                _session["threeDSTransId"] = null;
                _session["threeDSPARes"] = null;
                _session["threeDSCres"] = null;
                _session["threeDSMethodData"] = null;


                Logger_AddLogMessage(string.Format("BSRedsysRequest Begin: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}; ReturnURL={6}; culture= {7}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCode,
                                        UTCDate,
                                        Description,
                                        ReturnURL,
                                        Culture), LogLevels.logINFO);


                _session["ReturnURL"] = ReturnURL;                

                if ((string.IsNullOrEmpty(Guid) && Configuration_Id == null) ||
                    (string.IsNullOrEmpty(Email)) ||
                    (!Amount.HasValue) || (Amount.Value < 0) ||                    
                    (string.IsNullOrEmpty(CurrencyISOCode)) ||
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
                    int bsredsys_lang = 1;
                    if (!string.IsNullOrEmpty(Culture) && Culture.Length >= 2)
                    {
                        switch (Culture.ToLower().Substring(0, 2))
                        {
                            case "ca": bsredsys_lang = 3; break;
                            case "es": bsredsys_lang = 1; break;
                            case "en": bsredsys_lang = 2; break;
                            case "fr": bsredsys_lang = 4; break;
                            case "it": bsredsys_lang = 7; break;
                            case "gl": bsredsys_lang = 12; break;
                            case "de": bsredsys_lang = 5; break;
                            case "nl": bsredsys_lang = 6; break;
                            case "eu": bsredsys_lang = 13; break;
                            case "sv": bsredsys_lang = 8; break;
                            case "pt": bsredsys_lang = 9; break;
                            case "pl": bsredsys_lang = 11; break;
                        }
                    }

                    BSREDSYS_CONFIGURATION oConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetBSRedsysConfigurationById(Configuration_Id, out oConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetBSRedsysConfiguration(Guid, out oConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oConfiguration != null)
                        {
                            _session["HashSeed"] = oConfiguration.BSRCON_HASH_SEED;

                            string strCalcHash = CalculateHash(oConfiguration.BSRCON_GUID, Email, Amount.Value, CurrencyISOCode, Description, UTCDate, ReturnURL, Culture, oConfiguration.BSRCON_HASH_SEED);

                            if ((oConfiguration.BSRCON_CHECK_DATE_AND_HASH == 0) ||
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
                                    Logger_AddLogMessage(string.Format("BSRedsysRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                         UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                }

                                if (string.IsNullOrEmpty(result))
                                {
                                    if ((oConfiguration.BSRCON_CHECK_DATE_AND_HASH == 0) ||
                                        (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oConfiguration.BSRCON_CONFIRMATION_TIME) ||
                                        (bAvoidHashCheck == true))
                                    {

                                        var uri = new Uri(_request.Url.AbsoluteUri);
                                        string strURLPath = _request.Url.AbsoluteUri.Substring(0, _request.Url.AbsoluteUri.LastIndexOf("/"));

                                        if (!strURLPath.ToLower().EndsWith("/bsredsys"))
                                            strURLPath = strURLPath.Substring(0, strURLPath.LastIndexOf("/")) + "/BSRedsys";

                                        var sCurrencyISONum = infraestructureRepository.GetCurrencyIsoCodeNumericFromIsoCode(CurrencyISOCode);

                                        string orderId = BSRedsysPayments.OrderId();

                                        RedsysAPI r = new RedsysAPI();

                                        r.SetParameter("DS_MERCHANT_AMOUNT", Amount.Value.ToString());
                                        r.SetParameter("DS_MERCHANT_ORDER", orderId);
                                        r.SetParameter("DS_MERCHANT_MERCHANTCODE", oConfiguration.BSRCON_MERCHANT_CODE.ToString());
                                        r.SetParameter("DS_MERCHANT_CURRENCY", sCurrencyISONum);
                                        r.SetParameter("DS_MERCHANT_TRANSACTIONTYPE", "0");
                                        r.SetParameter("DS_MERCHANT_TERMINAL", oConfiguration.BSRCON_MERCHANT_TERMINAL);
                                        r.SetParameter("DS_MERCHANT_MERCHANTURL", "");
                                        r.SetParameter("DS_MERCHANT_URLOK", strURLPath + "/BSRedsysSuccess");
                                        r.SetParameter("DS_MERCHANT_URLKO", strURLPath + "/BSRedsysFailure");
                                        r.SetParameter("DS_MERCHANT_IDENTIFIER", "REQUIRED");
                                        r.SetParameter("DS_MERCHANT_CONSUMERLANGUAGE", bsredsys_lang.ToString());
                                        r.SetParameter("DS_MERCHANT_COF_INI", "S");
                                        r.SetParameter("DS_MERCHANT_COF_TYPE", "C");


                                        if (oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().COMMON_TOKEN_GROUP != null)
                                        {
                                            if (!string.IsNullOrEmpty(oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().COMMON_TOKEN_GROUP.CTG_BS_MERCHANT_GROUP))
                                            {
                                                r.SetParameter("DS_MERCHANT_GROUP", oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().COMMON_TOKEN_GROUP.CTG_BS_MERCHANT_GROUP);
                                            }
                                        }


                                        ViewData["email"] = Email;
                                        ViewData["bsredsys_form_url"] = oConfiguration.BSRCON_FORM_URL;
                                        ViewData["bsredsys_signature_version"] = "HMAC_SHA256_V1";
                                        ViewData["bsredsys_merchant_parameters"] = r.createMerchantParameters();
                                        ViewData["bsredsys_signature"] = r.createMerchantSignature(oConfiguration.BSRCON_MERCHANT_SIGNATURE);

                                        _session["email"] = Email;
                                        _session["amount"] = Amount;
                                        _session["currency"] = CurrencyISOCode;
                                        _session["utcdate"] = dtUTC;
                                        Guid = oConfiguration.BSRCON_GUID;
                                        _session["BSRedsysGuid"] = Guid;
                                        _session["sessionid"] = orderId;


                                        Logger_AddLogMessage(string.Format("BSRedsysRequest : Email={0} ; MerchantParametersBase64 ={1} ; MerchantParametersJSON={2}",
                                                 Email, ViewData["bsredsys_merchant_parameters"].ToString(), r.GetParameterAll(false)), LogLevels.logINFO);

                                    }
                                    else
                                    {
                                        result = "error";
                                        errorMessage = "Invalid DateTime";
                                        errorCode = "invalid_datetime";
                                        Logger_AddLogMessage(string.Format("BSRedsysRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }
                                }

                            }
                            else
                            {
                                result = "error";
                                errorCode = "invalid_hash";
                                errorMessage = "Invalid Hash";

                                Logger_AddLogMessage(string.Format("BSRedsysRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                       Hash, strCalcHash), LogLevels.logINFO);
                            }

                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "BSRedsys configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "BSRedsys configuration not found";
                    }
                }
            }
            catch
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = "BSRedsysRequest Method Exception";
            }

            if (!string.IsNullOrEmpty(errorCode))
            {

                _session["result"] = result;
                _session["errorCode"] = errorCode;
                _session["errorMessage"] = errorMessage;
                //SaveSession(_session["sessionid"].ToString());


                string strRedirectionURLLog = string.Format("BSRedsysResult?result={0}&errorCode={1}&errorMessage={2}", _server.UrlEncode(result), _server.UrlEncode(errorCode), _server.UrlEncode(errorMessage));
                string strRedirectionURL = string.Format("BSRedsysResult?r={0}", BSRedsysResultCalc());
                Logger_AddLogMessage(string.Format("BSRedsysRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; ResultURL={4}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCode,
                                        strRedirectionURLLog), LogLevels.logINFO);
                return Redirect(strRedirectionURL);
            }
            else
            {
                SaveSession(_session["sessionid"].ToString());

                Logger_AddLogMessage(string.Format("BSRedsysRequest End: Guid={0}; Email={1} ; Amount={2} ; Currency={3}; UTCDate = {4} ; Description={5}",
                                        Guid,
                                        Email,
                                        Amount,
                                        CurrencyISOCode,
                                        UTCDate,
                                        Description), LogLevels.logINFO);
                return View();
            }

        }

        [HttpGet]
        public ActionResult BSRedsysSuccess()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            return BSRedsysReturn("succeeded",
                                  _request["Ds_SignatureVersion"],
                                  _request["Ds_MerchantParameters"],
                                  _request["Ds_Signature"]);
        }

        [HttpGet]
        public ActionResult BSRedsysFailure()
        {

            if (_session == null) _session = Session;
            if (_server == null) _server = Server;
            if (_request == null) _request = Request;

            return BSRedsysReturn("error",
                                  _request["Ds_SignatureVersion"],
                                  _request["Ds_MerchantParameters"],
                                  _request["Ds_Signature"]);
        }

        [HttpGet]
        public ActionResult BSRedsysReturn(string result, string Ds_SignatureVersion, string Ds_MerchantParameters, string Ds_Signature)
        {

            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "BSRedsysResult";
            string strCardHash = "";
            string strCardReference = "";
            string strReference = "";
            string strTransactionId = "";
            string strCardScheme = "";
            string strBSRedsysDateTime = "";
            string strPAN = "";
            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";
            string strAuthCode = "";
            string strAuthResult = "";
            string strCOFTxnID = "";

            try
            {

                if (_session == null) _session = Session;
                if (_server == null) _server = Server;
                if (_request == null) _request = Request;

                Logger_AddLogMessage(string.Format("BSRedsysReturn Begin: Result={0} ; Email={1} ; Amount={2} ; Currency={3}; Ds_SignatureVersion={4},  Ds_MerchantParameters={5},  Ds_Signature={6} ",
                                        result,
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                         Ds_SignatureVersion,  
                                         Ds_MerchantParameters,  
                                         Ds_Signature
                                        ), LogLevels.logINFO);

                string strGuid = "";

                if (_session["BSRedsysGuid"] != null)
                {
                    strGuid = _session["BSRedsysGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid) && Configuration_Id == null)
                {
                    result = "error";
                    errorMessage = "BSRedsys Guid not found";
                    errorCode = "invalid_configuration";
                }
                else
                {

                    BSREDSYS_CONFIGURATION oConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetBSRedsysConfigurationById(Configuration_Id, out oConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetBSRedsysConfiguration(strGuid, out oConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oConfiguration == null)
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "BSRedsys configuration not found";
                        }
                        else
                        {
                            BSRedsysPayments.BSRedsysErrorCode eErrorCode = BSRedsysPayments.BSRedsysErrorCode.ERROR;
                            errorCode = "-1";

                            if (!string.IsNullOrEmpty(Ds_MerchantParameters))
                            {
                                RedsysAPI r = new RedsysAPI();

                                string sSignature = r.createMerchantSignatureNotif(oConfiguration.BSRCON_MERCHANT_SIGNATURE, Ds_MerchantParameters);

                                Logger_AddLogMessage(string.Format("BSRedsysReturn: Result='{0}'", r.GetParameterAll(false)), LogLevels.logINFO);

                                if (Ds_Signature == sSignature)
                                {
                                    //ViewData["Result"] = r.GetParameterAll(true);

                                    errorCode = r.GetParameter("Ds_Response");
                                    eErrorCode = BSRedsysPayments.GetErrorInfo(errorCode, out errorMessage);

                                    if (!BSRedsysPayments.IsError(eErrorCode))
                                    {
                                        strReference = r.GetParameter("Ds_Order");
                                        strTransactionId = r.GetParameter("Ds_AuthorisationCode");
                                        strBSRedsysDateTime = r.GetParameter("Ds_Date") + " " + r.GetParameter("Ds_Hour");
                                        strCardHash = r.GetParameter("Ds_Merchant_Identifier");
                                        strCardReference = r.GetParameter("Ds_Merchant_Identifier");
                                        strCardScheme = "";
                                        strPAN = r.GetParameter("Ds_Card_Number");
                                        strCOFTxnID = r.GetParameter("Ds_Merchant_Cof_Txnid");

                                        if (strCOFTxnID != null)
                                        {
                                            strCOFTxnID = strCOFTxnID.Trim();
                                        }

                                        if (!string.IsNullOrEmpty(strCOFTxnID))
                                        {
                                            strCardHash = strCOFTxnID;
                                        }
                                        else
                                        {
                                            Logger_AddLogMessage(string.Format("BSRedsysReturn: Ds_Merchant_Cof_Txnid empty or with spaces strCOFTxnID=strCardReference (Ds_Merchant_Identifier)"), LogLevels.logERROR);
                                        }

                                        if (string.IsNullOrEmpty(strPAN))
                                        {
                                            strPAN = "****";
                                        }

                                        strExpirationDateYear = r.GetParameter("Ds_ExpiryDate").Substring(0, 2);
                                        strExpirationDateMonth = r.GetParameter("Ds_ExpiryDate").Substring(2, 2);
                                    }
                                    else
                                        Logger_AddLogMessage(string.Format("BSRedsysReturn: Error='{0}-{1}' ; Result='{2}'", errorCode, errorMessage, r.GetParameterAll(false)), LogLevels.logERROR);
                                }
                                else
                                    Logger_AddLogMessage("BSRedsysReturn: Received signature is not equal to calculated signature", LogLevels.logERROR);
                            }

                            if (string.IsNullOrEmpty(errorMessage))
                                eErrorCode = BSRedsysPayments.GetErrorInfo(errorCode, out errorMessage);
                            if (BSRedsysPayments.IsError(eErrorCode))
                            {
                                result = "error";
                            }

                            if (!BSRedsysPayments.IsError(eErrorCode))
                            {
                                DateTime dtNow = DateTime.Now;
                                DateTime dtUTCNow = DateTime.UtcNow;

                                /*DateTime dtUTC = (DateTime)_session["utcdate"];

                                if ((oConfiguration.BSRCON_CHECK_DATE_AND_HASH == 1) &&
                                    (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) > oConfiguration.BSRCON_CONFIRMATION_TIME))
                                {
                                    result = "error";
                                    errorMessage = "Invalid DateTime";
                                    errorCode = "invalid_datetime";
                                    Logger_AddLogMessage(string.Format("BSRedsysSuccess : BeginningDate={0} ; CurrentDate={1}",
                                                                       dtUTC, DateTime.UtcNow), LogLevels.logINFO);


                                }
                                else
                                {*/


                                    int Amount = (int)_session["amount"];

                                    if (Amount > 0)
                                    {


                                        customersRepository.StartRecharge(oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                                  _session["email"].ToString(),
                                                                  dtUTCNow,
                                                                  dtNow,
                                                                  Amount,
                                                                  infraestructureRepository.GetCurrencyFromIsoCode(_session["currency"].ToString()),
                                                                  "",
                                                                  strReference,
                                                                  strTransactionId,
                                                                  "",
                                                                  strBSRedsysDateTime,
                                                                  strTransactionId,
                                                                  PaymentMeanRechargeStatus.Committed);
                                    }





                                    
                                    /*string sCurrencyIsoCode = oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CURRENCy.CUR_ISO_CODE;
                                    string sCurrencyIsoCodeNum = oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CURRENCy.CUR_ISO_CODE_NUM;
                                    //int iQuantityToRechargeBSRedsys = Convert.ToInt32(Convert.ToDecimal(Amount) * infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurrencyIsoCode));
                                    var oCardPayments = new BSRedsysPayments();
                                    string strMerchantGroup = null;

                                    if (oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().COMMON_TOKEN_GROUP != null)
                                    {
                                        if (!string.IsNullOrEmpty(oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().COMMON_TOKEN_GROUP.CTG_BS_MERCHANT_GROUP))
                                        {
                                            strMerchantGroup = oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().COMMON_TOKEN_GROUP.CTG_BS_MERCHANT_GROUP;
                                        }
                                    }

                                    BSRedsysPayments.BSRedsysErrorCode eResult = BSRedsysPayments.BSRedsysErrorCode.ERROR;
                                    string strUserReference = "";
                                    string strGatewayDate = "";
                                    string sErrorMessage = "";

                                    string strFullURL = Request.Url.AbsoluteUri;
                                    string strBaseURL = strFullURL.Substring(0, strFullURL.LastIndexOf("/"));
                                    string strReturnURL = strBaseURL + "/BSRedsys3DSResponse";
                                    string strMD = "";
                                    string strPaReq = "";
                                    string strCreq = "";
                                    string strInlineForm = "";
                                    string strProtocolVersion = "";
                                    string strthreeDSServerTransID = "";

                                    if (oCardPayments.StandardPayment3DSStep1(oConfiguration.BSRCON_WS_URL,
                                                                     oConfiguration.BSRCON_MERCHANT_CODE,
                                                                     oConfiguration.BSRCON_MERCHANT_SIGNATURE,
                                                                     oConfiguration.BSRCON_MERCHANT_TERMINAL,
                                                                     oConfiguration.BSRCON_SERVICE_TIMEOUT,
                                                                     strCardReference,
                                                                     5,
                                                                     sCurrencyIsoCodeNum, strMerchantGroup, strCardHash,
                                                                     strReturnURL,
                                                                     out eResult, out sErrorMessage,
                                                                     out strUserReference,
                                                                     out strTransactionId,
                                                                     out strGatewayDate,
                                                                     out strInlineForm,
                                                                     out strMD,
                                                                     out strPaReq,
                                                                     out strCreq,
                                                                     out strProtocolVersion,
                                                                     out strthreeDSServerTransID))
                                    {

                                        if (!string.IsNullOrEmpty(strInlineForm))
                                        {
                                            
                                            decimal? dTransId = null;
                                            string strTransId = (string.IsNullOrEmpty(strMD) ? strthreeDSServerTransID : strMD);

                                            DateTime utcnow = DateTime.UtcNow;
                                            infraestructureRepository.AddBSRedsys3DSTransaction(oConfiguration.BSRCON_ID, strTransId, strUserReference,
                                                _session["email"].ToString(), 5, utcnow, strInlineForm, strProtocolVersion, out  dTransId);

                                            string str3DSURL = string.Format("{0}/BSRedsys3DSRequest?Guid={1}&id={2}&threeDSTransId={3}&Email={4}&UTCDate={5}&Culture={6}&Hash={7}",
                                                strBaseURL,
                                                oConfiguration.BSRCON_GUID,
                                                dTransId.ToString(),
                                                HttpUtility.UrlEncode(strTransId),
                                                HttpUtility.UrlEncode(_session["email"].ToString()),
                                                utcnow.ToString("HHmmssddMMyy"),
                                                HttpUtility.UrlEncode(_session["Culture"].ToString()),
                                                "DDDDD"
                                                );
                                      
                                      

                                            return Redirect(str3DSURL);
                                        }


                                        if (Amount > 0)
                                        {
                                            customersRepository.StartRecharge(oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                                          _session["email"].ToString(),
                                                                          dtUTCNow,
                                                                          dtNow,
                                                                          Amount,
                                                                          infraestructureRepository.GetCurrencyFromIsoCode(_session["currency"].ToString()),
                                                                          "",
                                                                          strReference,
                                                                          strTransactionId,
                                                                          "",
                                                                          strBSRedsysDateTime,
                                                                          strTransactionId,
                                                                          PaymentMeanRechargeStatus.Committed);
                                        }
                                    }
                                    else
                                    {
                                        result = "error";
                                        errorCode = eResult.ToString();
                                        errorMessage = sErrorMessage;
                                    }
                                 */    
                                //}
                            }

                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "BSRedsys configuration not found";
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
                _session["chargeDateTime"] = strBSRedsysDateTime;
                _session["reference"] = strReference;
                _session["transactionID"] = strTransactionId;
                _session["authCode"] = strAuthCode;
                _session["authResult"] = strAuthResult;



                strRedirectionURLLog = string.Format("BSRedsysResult?result={0}" +
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
                    _server.UrlEncode(strBSRedsysDateTime),
                    _server.UrlEncode(strReference),
                    _server.UrlEncode(strTransactionId),
                    _server.UrlEncode(strAuthCode),
                    _server.UrlEncode(strAuthResult));

                Logger_AddLogMessage(string.Format("BSRedsysSuccess End: Token={0} ; Email={1} ; Amount={2} ; Currency={3} ; ResultURL={4}",
                                        strCardReference,
                                        _session["email"].ToString(),
                                        _session["amount"].ToString(),
                                        _session["currency"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("BSRedsysResult?r={0}", BSRedsysResultCalc());

            }

            return Redirect(strRedirectionURL);
        }


        [HttpGet]
        public ActionResult BSRedsys3DSRequest(string Guid, string id, string threeDSTransId, string Email, string UTCDate, string Culture, string ReturnURL, string Hash)
        {
            string result = "";
            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURL = "";


            try
            {
                if (_session == null) _session = Session;
                BSRedsysPayments oPayments = new BSRedsysPayments();


                if (_server == null) _server = Server;
                if (_request == null) _request = Request;

                Logger_AddLogMessage(string.Format("BSRedsys3DSRequest Begin: Guid={0}; id={1} ; threeDSTransId={2}; Email={3} ;UTCDate = {4} ; culture= {5}; ReturnURL={6}",
                                        Guid,
                                        id,
                                        threeDSTransId,
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
                _session["BSRedsysGuid"] = null;
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
                _session["threeDSTransId"] = null;
                _session["threeDSPARes"] = null;
                _session["threeDSCres"] = null;
                _session["threeDSMethodData"] = null;



                _session["ReturnURL"] = ReturnURL;

                if ((string.IsNullOrEmpty(Guid) && Configuration_Id == null) ||
                    (string.IsNullOrEmpty(Email)) ||
                    (string.IsNullOrEmpty(id)) ||
                    (string.IsNullOrEmpty(threeDSTransId)) ||
                    (string.IsNullOrEmpty(UTCDate)) ||
                    (string.IsNullOrEmpty(Hash) && bAvoidHashCheck == false))
                {
                    result = "error";
                    errorMessage = "Invalid or missing parameter";
                    errorCode = "invalid_parameter";
                }

                else
                {

                    BSREDSYS_CONFIGURATION oConfiguration = null;
                    bool Configuration_OK = false;

                    if (Configuration_Id != null)
                    {
                        Configuration_OK = infraestructureRepository.GetBSRedsysConfigurationById(Configuration_Id, out oConfiguration);
                    }
                    else
                    {
                        Configuration_OK = infraestructureRepository.GetBSRedsysConfiguration(Guid, out oConfiguration);
                    }

                    if (Configuration_OK)
                    {
                        if (oConfiguration != null)
                        {

                            _session["HashSeed"] = oConfiguration.BSRCON_HASH_SEED;

                            string strCalcHash = CalculateHash(oConfiguration.BSRCON_GUID, id, threeDSTransId, Email, UTCDate, Culture, ReturnURL, oConfiguration.BSRCON_HASH_SEED);

                            if ((oConfiguration.BSRCON_CHECK_DATE_AND_HASH == 0) ||
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
                                    Logger_AddLogMessage(string.Format("BSRedsys3DSRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                       UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                }

                                if (string.IsNullOrEmpty(result))
                                {

                                    if ((oConfiguration.BSRCON_CHECK_DATE_AND_HASH == 0) ||
                                        (Math.Abs((DateTime.UtcNow - dtUTC).TotalSeconds) <= oConfiguration.BSRCON_CONFIRMATION_TIME) ||
                                        (bAvoidHashCheck == true))
                                    {

                                        string strInlineForm="";
                                        infraestructureRepository.GetBSRedsys3DSTransactionInlineForm(Convert.ToDecimal(id), threeDSTransId, Email, out strInlineForm);


                                        if (!string.IsNullOrEmpty(strInlineForm))
                                        {
                                            _session["result"] = result;
                                            _session["errorCode"] = errorCode;
                                            _session["errorMessage"] = errorMessage;
                                            _session["sessionid"] = threeDSTransId;
                                            _session["email"] = Email;
                                            _session["utcdate"] = dtUTC;
                                            Guid = oConfiguration.BSRCON_GUID;
                                            _session["BSRedsysGuid"] = Guid;
                                            Logger_AddLogMessage(string.Format("BSRedsys3DSRequest : InLineForm:{0}", strInlineForm),  LogLevels.logINFO);
                                            SaveSession(threeDSTransId);

                                            HttpResponse response = HttpContext.ApplicationInstance.Response;
                                            response.Clear();                                         
                                            response.Write(strInlineForm);
                                            response.End();
                                            //return Content(strInlineForm); ;
                                        }
                                        else
                                        {
                                            result = "error";
                                            errorMessage = "empty inline form";
                                            errorCode = "invalid_transaction";
                                            Logger_AddLogMessage(string.Format("BSRedsys3DSRequest : Empty inline form",
                                                               UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                        }



                                    }
                                    else
                                    {
                                        result = "error";
                                        errorMessage = "Invalid DateTime";
                                        errorCode = "invalid_datetime";
                                        Logger_AddLogMessage(string.Format("BSRedsys3DSRequest : ReceivedDate={0} ; CurrentDate={1}",
                                                           UTCDate, DateTime.UtcNow), LogLevels.logINFO);
                                    }
                                }
                            }
                            else
                            {
                                result = "error";
                                errorCode = "invalid_hash";
                                errorMessage = "Invalid Hash";

                                Logger_AddLogMessage(string.Format("BSRedsys3DSRequest : ReceivedHash={0} ; CalculatedHash={1}",
                                                       Hash, strCalcHash), LogLevels.logINFO);


                            }

                        }
                        else
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "BSRedsys configuration not found";
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "BSRedsys configuration not found";
                    }
                }
            }
            catch
            {
                result = "error";
                errorCode = "unexpected_failure";
                errorMessage = "BSRedsys3DSRequest Method Exception";
            }

            if (!string.IsNullOrEmpty(errorCode))
            {

                _session["result"] = result;
                _session["errorCode"] = errorCode;
                _session["errorMessage"] = errorMessage;

                //SaveSession(_session["sessionid"].ToString());

                string strRedirectionURLLog = string.Format("BSRedsysResult?result={0}&errorCode={1}&errorMessage={2}", _server.UrlEncode(result), _server.UrlEncode(errorCode), _server.UrlEncode(errorMessage));
                strRedirectionURL = string.Format("BSRedsysResult?r={0}&s={1}", BSRedsysResultCalc(), _session["sessionid"]);
                Logger_AddLogMessage(string.Format("MonerisRequest End: Guid={0}; Email={1} ; ResultURL={2}",
                                        Guid,
                                        Email,
                                        strRedirectionURLLog), LogLevels.logINFO);
                return Redirect(strRedirectionURL);
            }

            return View();
        }




        [HttpPost]
        public ActionResult BSRedsys3DSResponse(string PaRes, string MD, string cres, string threeDSMethodData)
        {

            string errorMessage = "";
            string errorCode = "";
            string strRedirectionURLLog = "";
            string strRedirectionURL = "BSRedsysResult";
            string strCardHash = "";
            string strCardReference = "";
            string strReference = "";
            string strTransactionId = "";
            string strCardScheme = "";
            string strBSRedsysDateTime = "";
            string strPAN = "";
            string strExpirationDateMonth = "";
            string strExpirationDateYear = "";
            string strAuthCode = "";
            string strAuthResult = "";
            string strJSON = "";
            string result = "";
            string threeDSServerTransID = "";

            try
            {


                if (_session == null) _session = Session;
                if (_server == null) _server = Server;
                if (_request == null) _request = Request;


                if (!string.IsNullOrEmpty(threeDSMethodData))
                {
                    Logger_AddLogMessage(string.Format("BSRedsys3DSResponse Begin:  (!string.IsNullOrEmpty(threeDSMethodData) --> threeDSMethodData={0} ",
                                           threeDSMethodData
                                           ), LogLevels.logINFO);

                    strJSON = Base64Decode(threeDSMethodData);

                    dynamic othreeDSMethodData = JsonConvert.DeserializeObject(strJSON);

                    try
                    {
                        threeDSServerTransID = othreeDSMethodData["threeDSServerTransID"];
                    }
                    catch { }

                }
                else if (!string.IsNullOrEmpty(cres))
                {
                    Logger_AddLogMessage(string.Format("BSRedsys3DSResponse Begin:  (!string.IsNullOrEmpty(cres) --> cres={0} ",
                                           cres
                                           ), LogLevels.logINFO);

                    strJSON = Base64Decode(cres);

                    dynamic oCres = JsonConvert.DeserializeObject(strJSON);

                    try
                    {
                        threeDSServerTransID = oCres["threeDSServerTransID"];
                    }
                    catch { }
                }


                Logger_AddLogMessage(string.Format("BSRedsys3DSResponse Begin:  MD={0}; threeDSMethodData={1}; threeDSServerTransID={2}; JSON={3}; cres={4}; PaRes={5}",
                                       MD,
                                       threeDSMethodData,
                                       threeDSServerTransID,
                                       strJSON,
                                       cres,
                                       PaRes
                                       ), LogLevels.logINFO);

                if ((_session["sessionid"] == null) && (!string.IsNullOrEmpty(MD)))
                {
                    LoadSession(MD);
                }
                else if ((_session["sessionid"] == null) && (!string.IsNullOrEmpty(threeDSMethodData)))
                {
                    LoadSession(threeDSServerTransID);

                }
                else if ((_session["sessionid"] == null) && (!string.IsNullOrEmpty(cres)))
                {
                    LoadSession(threeDSServerTransID);
                }

                Logger_AddLogMessage(string.Format("BSRedsys3DSResponse Begin: Result={0} ; Email={1} ; MD={2} ; threeDSServerTransID={3}",
                                        result,
                                        Session["email"].ToString(),
                                        MD,
                                        threeDSServerTransID
                                        ), LogLevels.logINFO);

                string strGuid = "";

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


                if (_session["BSRedsysGuid"] != null)
                {
                    strGuid = _session["BSRedsysGuid"].ToString();
                }

                if (string.IsNullOrEmpty(strGuid))
                {
                    result = "error";
                    errorMessage = "BSRedsys Guid not found";
                    errorCode = "invalid_configuration";
                }
                else
                {

                    BSREDSYS_CONFIGURATION oConfiguration = null;
                    bool Configuration_OK = false;


                    Configuration_OK = infraestructureRepository.GetBSRedsysConfiguration(strGuid, out oConfiguration);


                    if (Configuration_OK)
                    {
                        if (oConfiguration == null)
                        {
                            result = "error";
                            errorCode = "configuration_not_found";
                            errorMessage = "BSRedsys configuration not found";
                        }
                        else
                        {
                            result = "succeeded";
                            if (!bDoFinalStep)
                            {
                                if (!string.IsNullOrEmpty(MD))
                                {
                                    _session["threeDSTransId"]= MD;
                                }
                                else
                                {
                                    _session["threeDSTransId"] = threeDSServerTransID;
                                }
                                _session["threeDSPARes"] = (PaRes==null)?"":PaRes;
                                _session["threeDSCres"] = (cres == null) ? "" : cres;
                                _session["threeDSMethodData"] = (threeDSMethodData == null) ? "" : threeDSMethodData;

                            }
                            else
                            {

                                BSRedsysPayments.BSRedsysErrorCode eErrorCode = BSRedsysPayments.BSRedsysErrorCode.ERROR;
                                string sErrorMessage = "";
                                errorCode = "-1";
                                string strGatewayDate = "";

                                string strOrderId = _session["orderid"].ToString();
                                int Amount = (int)_session["amount"];
                                string strProtocolVersion = _session["ProtocolVersion"].ToString();
                                strCardReference = _session["cardReference"].ToString();
                                strCardHash = _session["cardHash"].ToString();
                                string strMerchantGroup = null;

                                if (oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().COMMON_TOKEN_GROUP != null)
                                {
                                    if (!string.IsNullOrEmpty(oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().COMMON_TOKEN_GROUP.CTG_BS_MERCHANT_GROUP))
                                    {
                                        strMerchantGroup = oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().COMMON_TOKEN_GROUP.CTG_BS_MERCHANT_GROUP;
                                    }
                                }

                                string sCurrencyIsoCode = oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CURRENCy.CUR_ISO_CODE;
                                string sCurrencyIsoCodeNum = oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CURRENCy.CUR_ISO_CODE_NUM;
                                var oCardPayments = new BSRedsysPayments();

                                if ((string.IsNullOrEmpty(MD)) && (string.IsNullOrEmpty(cres)))
                                {
                                    string strFullURL = Request.Url.AbsoluteUri;
                                    string strBaseURL = strFullURL.Substring(0, strFullURL.LastIndexOf("/"));
                                    string strReturnURL = strBaseURL + "/BSRedsys3DSResponse";
                                    string strMD = "";
                                    string strPaReq = "";
                                    string strCreq = "";
                                    string strInlineForm = "";
                                    string strEmail="";
                                    infraestructureRepository.GetBSRedsys3DSTransactionEmail(threeDSServerTransID, out strEmail);

                                    if (oCardPayments.StandardPayment3DSStep2(oConfiguration.BSRCON_WS_URL,
                                                                             oConfiguration.BSRCON_MERCHANT_CODE,
                                                                             oConfiguration.BSRCON_MERCHANT_SIGNATURE,
                                                                             oConfiguration.BSRCON_MERCHANT_TERMINAL,
                                                                             oConfiguration.BSRCON_SERVICE_TIMEOUT,
                                                                             strCardReference,
                                                                             Amount,
                                                                             sCurrencyIsoCodeNum, strMerchantGroup, strCardHash,
                                                                             strOrderId, strReturnURL, strProtocolVersion, threeDSServerTransID, "Y",
                                                                             out eErrorCode, out sErrorMessage,
                                                                             out strTransactionId,
                                                                             out strGatewayDate,
                                                                             out  strInlineForm,
                                                                             out  strMD,
                                                                             out strPaReq,
                                                                             out strCreq))
                                    {
                                        if (!string.IsNullOrEmpty(strInlineForm))
                                        {
                                            _session["cardReference"] = strCardReference;
                                            _session["cardHash"] = strCardHash;
                                            _session["cardScheme"] = strCardScheme;
                                            _session["cardPAN"] = strPAN;
                                            _session["cardExpMonth"] = strExpirationDateMonth;
                                            _session["cardExpYear"] = strExpirationDateYear;
                                            _session["chargeDateTime"] = strBSRedsysDateTime;

                                            if (!string.IsNullOrEmpty(strMD))
                                            {
                                                _session["sessionid"] = strMD;
                                                SaveSession(strMD);
                                            }

                                            else if (!string.IsNullOrEmpty(threeDSServerTransID))
                                            {
                                                _session["sessionid"] = threeDSServerTransID;
                                                SaveSession(threeDSServerTransID);
                                            }

                                            decimal? dTransId = null;
                                            string strTransId = (string.IsNullOrEmpty(strMD) ? threeDSServerTransID : strMD);

                                            DateTime utcnow = DateTime.UtcNow;
                                            infraestructureRepository.UpdateBSRedsys3DSTransaction(threeDSServerTransID, strEmail, strInlineForm, utcnow, out dTransId);


                                            string strCalcHash = CalculateHash(oConfiguration.BSRCON_GUID,
                                                dTransId.ToString(),
                                                strTransId,
                                                Session["email"].ToString(),
                                                utcnow.ToString("HHmmssddMMyy"),
                                                Session["Culture"].ToString(),
                                                "", oConfiguration.BSRCON_HASH_SEED);

                                            string str3DSURL = string.Format("{0}/BSRedsys3DSRequest?Guid={1}&id={2}&threeDSTransId={3}&Email={4}&UTCDate={5}&Culture={6}&Hash={7}",
                                                strBaseURL,
                                                oConfiguration.BSRCON_GUID,
                                                dTransId.ToString(),
                                                HttpUtility.UrlEncode(strTransId),
                                                HttpUtility.UrlEncode(_session["email"].ToString()),
                                                utcnow.ToString("HHmmssddMMyy"),
                                                HttpUtility.UrlEncode(_session["Culture"].ToString()),
                                                strCalcHash
                                                );



                                            return Redirect(str3DSURL);

                                        }
                                    }
                                    else
                                    {
                                        result = "error";
                                        errorCode = eErrorCode.ToString();
                                        errorMessage = sErrorMessage;
                                    }


                                }
                                else
                                {

                                    if (!oCardPayments.StandardPayment3DSStep3(oConfiguration.BSRCON_WS_URL,
                                                                             oConfiguration.BSRCON_MERCHANT_CODE,
                                                                             oConfiguration.BSRCON_MERCHANT_SIGNATURE,
                                                                             oConfiguration.BSRCON_MERCHANT_TERMINAL,
                                                                             oConfiguration.BSRCON_SERVICE_TIMEOUT,
                                                                             strCardReference,
                                                                             Amount,
                                                                             sCurrencyIsoCodeNum, null, strCardHash,
                                                                             strOrderId, MD, PaRes, cres, ref strProtocolVersion,
                                                                             out eErrorCode, out sErrorMessage,
                                                                             out strTransactionId,
                                                                             out strGatewayDate))
                                    {
                                        result = "error";
                                        errorCode = eErrorCode.ToString();
                                        errorMessage = sErrorMessage;
                                    }
                                    else
                                    {
                                        _session["threeDSTransId"] = null;
                                        _session["threeDSPARes"] = null;
                                        _session["threeDSCres"] = null;
                                        _session["threeDSMethodData"] = null;

                                      
                                        DateTime dtNow = DateTime.Now;
                                        DateTime dtUTCNow = DateTime.UtcNow;

                                        customersRepository.StartRecharge(oConfiguration.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIGs.First().CPTGC_ID,
                                                                        _session["email"].ToString(),
                                                                        dtUTCNow,
                                                                        dtNow,
                                                                        Amount,
                                                                        infraestructureRepository.GetCurrencyFromIsoCode(_session["currency"].ToString()),
                                                                        "",
                                                                        strReference,
                                                                        strTransactionId,
                                                                        "",
                                                                        strBSRedsysDateTime,
                                                                        strTransactionId,
                                                                        PaymentMeanRechargeStatus.Committed);
                                        
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        result = "error";
                        errorCode = "configuration_not_found";
                        errorMessage = "BSRedsys configuration not found";
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
                _session["chargeDateTime"] = strBSRedsysDateTime;
                _session["reference"] = strReference;
                _session["transactionID"] = strTransactionId;
                _session["authCode"] = strAuthCode;
                _session["authResult"] = strAuthResult;

             

                if (_session["threeDSTransId"] == null)
                {

                    strRedirectionURLLog = string.Format("BSRedsysResult?result={0}" +
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
                                                        "&authResult={13}" +
                                                        "&threeDSTransID={14}" +
                                                        "&threeDSPares={15}" +
                                                        "&threeDSCres={16}" +
                                                        "&threeDSMethodData={17}",

                        Server.UrlEncode(result),
                        Server.UrlEncode(errorCode),
                        Server.UrlEncode(errorMessage),
                        Server.UrlEncode(strCardReference),
                        Server.UrlEncode(strCardHash),
                        Server.UrlEncode(strCardScheme),
                        Server.UrlEncode(strPAN),
                        Server.UrlEncode(strExpirationDateMonth),
                        Server.UrlEncode(strExpirationDateYear),
                        Server.UrlEncode(strBSRedsysDateTime),
                        Server.UrlEncode(strReference),
                        Server.UrlEncode(strTransactionId),
                        Server.UrlEncode(strAuthCode),
                        Server.UrlEncode(strAuthResult));
                }
                else
                {

   
                    strRedirectionURLLog = string.Format("BSRedsysResult?result={0}" +
                                                                          "&errorCode={1}" +
                                                                          "&errorMessage={2}" +
                                                                          "&threeDSTransID={3}" +
                                                                          "&threeDSPares={4}" +
                                                                          "&threeDSCres={5}" +
                                                                          "&threeDSMethodData={6}",

                                          Server.UrlEncode(result),
                                          Server.UrlEncode(errorCode),
                                          Server.UrlEncode(errorMessage),
                                          Server.UrlEncode(_session["threeDSTransId"].ToString()),
                                          Server.UrlEncode(_session["threeDSPARes"].ToString()),
                                          Server.UrlEncode(_session["threeDSCres"].ToString()),
                                          Server.UrlEncode(_session["threeDSMethodData"].ToString()));
                }

                Logger_AddLogMessage(string.Format("BSRedsys3DSResponse End: Email={0} ; ResultURL={1}",
                                        _session["email"].ToString(),
                                        strRedirectionURLLog), LogLevels.logINFO);

                strRedirectionURL = string.Format("BSRedsysResult?r={0}", BSRedsysResultCalc());

            }

            return Redirect(strRedirectionURL);
        }


        string Base64Decode(string toDecode)
        {
            try
            {
                string toDecodePadded = toDecode;

                int mod4 = toDecode.Length % 4;
                if (mod4 > 0)
                {
                    toDecodePadded += new string('=', 4 - mod4);
                    Logger_AddLogMessage(string.Format("Base64Decode Padding: Length1:{0} Length2:{1} string1={2} string2={3}",
                                        toDecode.Length, toDecodePadded.Length, toDecode, toDecodePadded), LogLevels.logINFO);
                }

                var base64EncodedBytes = System.Convert.FromBase64String(toDecodePadded);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch (FormatException ex)
            {

                Logger_AddLogException(ex, "Base64Decode", LogLevels.logERROR);
                return "";
            }
        }


        [HttpGet]
        public ActionResult BSRedsysResult(string r)
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

                string strResultDec = DecryptCryptResult(r, _session["HashSeed"].ToString());
                ViewData["Result"] = strResultDec;
                if (_session["PayerQuantity"] != null) ViewData["PayerQuantity"] = _session["PayerQuantity"];
                if (_session["PayerCurrencyISOCode"] != null) ViewData["PayerCurrencyISOCode"] = _session["PayerCurrencyISOCode"];
                if (_session["UserBalance"] != null) ViewData["UserBalance"] = _session["UserBalance"];



                if (_session["ReturnURL"] != null && !string.IsNullOrEmpty(_session["ReturnURL"].ToString()))
                {
                    Dictionary<string, object> postData = new Dictionary<string, object>();
                    postData.Add("r", r);

                    RedirectWithData(_session["ReturnURL"].ToString(), postData);
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
                        _session["BSRedsysGuid"] = null;
                        _session["ResultURL"] = null;
                        _session["threeDSTransId"] = null;
                        _session["threeDSPARes"] = null;
                        _session["threeDSCres"] = null;
                        _session["threeDSMethodData"] = null;


                        string sSuffix = string.Empty;
                        if (_session["Suffix"] != null)
                        {
                            sSuffix = _session["Suffix"].ToString();
                        }
                        return RedirectToAction("BSRedsysResult" + sSuffix, "Account", new { r = r });
                    }
                    else if (_session["PAYMENT_ORIGIN"] != null && _session["PAYMENT_ORIGIN"].ToString() == "FineController")
                    {
                        return RedirectToAction("BSRedsysResult", "Fine", new { r = r });
                    }
                                                            
                }

                return View();
            }

        }

        private string BSRedsysResultCalc()
        {

            string strRes = "";

            Dictionary<string, object> oDataDict = new Dictionary<string, object>();

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


            if (_session["result"] != null && _session["result"].ToString() == "succeeded" && _session["threeDSTransId"] == null)
            {
                oDataDict["bsredsys_card_reference"] = _session["cardReference"];
                oDataDict["bsredsys_card_hash"] = _session["cardHash"];
                oDataDict["bsredsys_card_scheme"] = _session["cardScheme"];
                oDataDict["bsredsys_masked_card_number"] = _session["cardPAN"];
                oDataDict["bsredsys_expires_end_month"] = _session["cardExpMonth"];
                oDataDict["bsredsys_expires_end_year"] = _session["cardExpYear"];
                oDataDict["bsredsys_date_time_local_fmt"] = _session["chargeDateTime"];
                oDataDict["bsredsys_reference"] = _session["reference"];
                oDataDict["bsredsys_transaction_id"] = _session["transactionID"];
                oDataDict["bsredsys_auth_code"] = _session["authCode"];
                oDataDict["bsredsys_auth_result"] = _session["authResult"];
            }
            else if (_session["result"] != null && _session["result"].ToString() == "succeeded" && _session["threeDSTransId"] != null)
            {
                oDataDict["bsredsys_3ds_trans_id"] = _session["threeDSTransId"];
                oDataDict["bsredsys_3ds_pares"] = _session["threeDSPARes"];
                oDataDict["bsredsys_3ds_cres"] = _session["threeDSCres"];
                oDataDict["bsredsys_3ds_methoddata"] = _session["threeDSMethodData"];                
            }

            var json = JsonConvert.SerializeObject(oDataDict);

            Logger_AddLogMessage(string.Format("BSRedsysResultCalc: {0}",
                                               PrettyJSON(json)), LogLevels.logINFO);

            strRes = CalculateCryptResult(json, _session["HashSeed"].ToString());

            return strRes;


        }



        private string CalculateHash(string Guid, string Email, int Amount, string CurrencyISOCODE, string Description, string UTCDate, string ReturnURL, string lang, string strHashSeed)
        {
            string strHashString = Guid + Email + Amount.ToString() + CurrencyISOCODE + Description + UTCDate + ReturnURL + lang;

            return CalculateHash(strHashString, strHashSeed);

        }


        private string CalculateHash(string Guid, string id, string TransId, string Email, string UTCDate, string lang, string ReturnURL, string strHashSeed)
        {
            string strHashString = Guid + id + TransId + Email + UTCDate + lang + ReturnURL;

            return CalculateHash(strHashString, strHashSeed);

        }

    }
}
