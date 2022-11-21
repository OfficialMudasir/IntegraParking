using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Web.Security;
using System.Security.Cryptography;
using System.Globalization;
using System.Threading;
using Ninject;
using Ninject.Web;
using Newtonsoft.Json;
using log4net;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.WS.Resources;
using System.Data.SqlClient;
using System.Data;
using integraMobile.ExternalWS;

namespace integraExternalServices.WS
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class integraExternalServices : Ninject.Web.WebServiceBase
    {

        #region Properties

        //Log4net Wrapper class
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(integraExternalServices));

        static string _hMacKey = null;
        static byte[] _normKey = null;
        static string _xmlTagName = "ipark";
        private const long BIG_PRIME_NUMBER = 2147483647;
        private const string IN_SUFIX = "_in";
        private const string OUT_SUFIX = "_out";

        private const decimal MEXICO_INS = 10005;
        private static SortedList<string, string> _MexicoPlates = null;

        private enum ResultType
        {
            Result_OK = 1,
            Result_Error_InvalidAuthenticationHash = -1,
            Result_Error_PlateNotExist = -2,
            Result_Error_Invalid_Input_Parameter = -3,
            Result_Error_Missing_Input_Parameter = -4,
            Result_Error_Invalid_City = -5,
            Result_Error_Invalid_ExternalProvider = -6,
            Result_Error_Generic = -9,
            Result_Error_OperationAlreadyClosed = -39,
            Result_Error_OperationEntryAlreadyExists = -40,
            Result_Error_OperationExitNotExist = -41,
            Result_Error_OperationNotExist = -42,
            Result_Error_MultipleUsersforPlate = 2,
            
            //Result_Error_PaymentDeclined = 0,
            Result_Error_Recharge_Failed = -23,
            Result_Error_Recharge_Not_Possible = -24,
            Result_Error_Invalid_Payment_Mean = -29,
            Result_Error_Not_Enough_Balance = -33,

            Result_Error_Coupon_NoExist = -43,
            Result_Error_Coupon_Used = -44,
            Result_Error_Coupon_NotAvailable = -45,

            Result_Toll_is_Not_from_That_installation = -46

        }

        static integraExternalServices()
        {
            InitializeStatic();
        }

        [Inject]
        public ICustomersRepository customersRepository { get; set; }
        [Inject]
        public IInfraestructureRepository infraestructureRepository { get; set; }
        [Inject]
        public IGeograficAndTariffsRepository geograficAndTariffsRepository { get; set; }
        [Inject]
        public IFineRepository fineRepository { get; set; }

        #endregion

        #region integraExternalServices.WS Web Methods


        [WebMethod]
        public string NotifyPlateFine(string xmlIn)
        {
            string xmlOut = "";
            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("3rdPNotifyPlateFine: xmlIn={0}",PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["city_id"] == null) ||
                        (parametersIn["lp"] == null) ||
                        (parametersIn["d"] == null)||
                        (parametersIn["f"] == null)||
                        (parametersIn["q"] == null) ||
                        (parametersIn["df"] == null) ||
                        (parametersIn["ta"] == null)||
                        (parametersIn["vers"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("3rdPNotifyPlateFine::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);

                    }
                    else
                    {
                        string strCalculatedHash = CalculateHash(strHashString, strHash);

                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("3rdPNotifyPlateFine::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {
                            
                            DateTime dt;
                            try
                            {
                                try
                                {
                                    dt = DateTime.ParseExact(parametersIn["d"].ToString(), "HHmmssddMMyy",
                                        CultureInfo.InvariantCulture);

                                }
                                catch
                                {
                                    dt = DateTime.ParseExact(parametersIn["d"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                        CultureInfo.InvariantCulture);
                                }

                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateFine::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;

                            }

                            DateTime df;
                            try
                            {
                                try
                                {
                                    df = DateTime.ParseExact(parametersIn["df"].ToString(), "HHmmssddMMyy",
                                      CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    df = DateTime.ParseExact(parametersIn["df"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                      CultureInfo.InvariantCulture);
                                }
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateFine::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;

                            }

                            decimal? dInstallationId = null;

                            try
                            {
                                decimal dTryInstallationId = Convert.ToDecimal(parametersIn["city_id"].ToString());
                                dInstallationId = dTryInstallationId;
                            }
                            catch
                            {
                                dInstallationId = null;
                            }

                            INSTALLATION oInstallation = null;
                            DateTime? dtinstDateTime = null;

                            if (!geograficAndTariffsRepository.getInstallationById(dInstallationId,
                                                                         ref oInstallation,
                                                                         ref dtinstDateTime))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateFine::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            int iQuantity = 0;
                            try
                            {
                                iQuantity = Convert.ToInt32(parametersIn["q"].ToString());
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateFine::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;

                            }
                            
                            DateTime? dtUTC = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(dInstallationId.Value, dt);
                            DateTime? dfUTC = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(dInstallationId.Value, df);


                            if ((string.IsNullOrEmpty(parametersIn["lp"].ToString())) ||
                                (string.IsNullOrEmpty(parametersIn["f"].ToString())) ||
                                (string.IsNullOrEmpty(parametersIn["ta"].ToString())))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateFine::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            string strPlate = parametersIn["lp"].ToString();
                            strPlate = strPlate.Trim().Replace(" ", "").Replace("-", "").ToUpper();

                            string strFineNumber = parametersIn["f"].ToString();
                            strFineNumber = strFineNumber.Trim().Replace(" ", "");
                            
                            if (!infraestructureRepository.ExistPlateInSystem(strPlate))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_PlateNotExist);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateFine::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            string strDta = "";
                            if (parametersIn["dta"] != null)
                            {
                                strDta = parametersIn["dta"].ToString();
                            }
                           

                          
                            IEnumerable<USER> oUsersList = null;
                            bool bExistAUser=false;
                            if (customersRepository.GetUsersWithPlate(strPlate, out oUsersList))
                            {
                                FineNotificationType oFineNotificationType = oInstallation.INS_FINE_NOTIFICATION_TYPE.HasValue ? (FineNotificationType)oInstallation.INS_FINE_NOTIFICATION_TYPE.Value : FineNotificationType.Nocontrol;


                                foreach (USER user in oUsersList)
                                {
                                    USER oUser = user;

                                    if (oFineNotificationType == FineNotificationType.Nocontrol ||
                                        (oUser.USR_FIRST_OPERATION_INS_ID.HasValue && oUser.USR_FIRST_OPERATION_INS_ID.Value == dInstallationId))
                                    {

                                        bExistAUser = true;
                                        string culture = user.USR_CULTURE_LANG;
                                        CultureInfo ci = new CultureInfo(culture);
                                        Thread.CurrentThread.CurrentUICulture = ci;
                                        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                                        integraMobile.WS.Properties.Resource.Culture = ci;

                                        decimal dSourceApp = user.USR_LAST_SOAPP_ID.Value;

                                        string strCulturePrefix = culture.ToLower().Substring(0, 2);
                                        string strZone = "";
                                        string strSector = "";
                                        string strEnforcuser = "";
                                        string strDtaLang = strDta;
                                        if (string.IsNullOrEmpty(strDta))
                                        {
                                            try
                                            {
                                                strDtaLang = parametersIn["dta_lang_" + strCulturePrefix].ToString();
                                            }
                                            catch
                                            {
                                                strDtaLang = parametersIn["dta_lang_es"].ToString();
                                            }

                                            if (parametersIn.ContainsKey("dta_user"))
                                                strEnforcuser = parametersIn["dta_user"].ToString();
                                            if (parametersIn.ContainsKey("dta_sector"))
                                            {
                                                strZone = ResourceExtension.GetLiteral("Zone_" + parametersIn["dta_sector"].ToString());
                                                strSector = ResourceExtension.GetLiteral("Sector_" + parametersIn["dta_sector"].ToString());
                                            }


                                        }


                                        string strSourceAppEmailPrefix = GetEmailSourceAppEmailPrefix(dSourceApp);

                                        string strExtTicketEmailHeader = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ExternalTicket_EmailHeader"), strPlate, strFineNumber);

                                        /*Bolet&iacute;n: {0}<br>
                                         * Matr&iacute;cula: {1}<br>
                                         * Ciudad: {2}<br>
                                         * Fecha: {3:HH:mm:ss dd/MM/yyyy}<br>
                                         * Descripci&oacute;n: {4}<br>
                                         * Cantidad A Pagar: {5}<br><br>
                                         * <a href="{6}"><img src="{7}" border="0"></a><br><br>*/

                                        string strExtTicketEmailBody = "";

                                        /*SortedList oTemplateVariables = new SortedList();

                                        oTemplateVariables.Add("FineNumber", strFineNumber);
                                        oTemplateVariables.Add("Plate", strPlate);
                                        oTemplateVariables.Add("InstallationDescription", oInstallation.INS_DESCRIPTION);
                                        oTemplateVariables.Add("Date", dt);
                                        oTemplateVariables.Add("Type", parametersIn["ta"].ToString());
                                        oTemplateVariables.Add("Description", parametersIn["dta"].ToString());
                                        oTemplateVariables.Add("Quantity", Convert.ToDouble(iQuantity) / 100);
                                        oTemplateVariables.Add("Currency", oInstallation.CURRENCy.CUR_ISO_CODE);

                                        strExtTicketEmailBody = ContentTemplates.SubstituteVariablesInTemplate(culture, Resource.ExternalTicket_EmailBody, oTemplateVariables);*/


                                        if (iQuantity > 0)
                                        {

                                            strExtTicketEmailBody = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ExternalTicket_EmailBody"),
                                                    strFineNumber,
                                                    strPlate,
                                                    oInstallation.INS_DESCRIPTION,
                                                    dt,
                                                    parametersIn["ta"].ToString() + " - " + strDtaLang,
                                                    string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(oInstallation.CURRENCy.CUR_ISO_CODE) + "} {1}", 
                                                    Convert.ToDouble(iQuantity) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oInstallation.CURRENCy.CUR_ISO_CODE), infraestructureRepository.GetCurSymbolFromIsoCode(oInstallation.CURRENCy.CUR_ISO_CODE)),
                                                    GetEmailFooter(ref oUser), strZone, strSector, strEnforcuser);
                                        }
                                        else
                                        {
                                            strExtTicketEmailBody = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ExternalTicketNotPayable_EmailBody"),
                                                    strFineNumber,
                                                    strPlate,
                                                    oInstallation.INS_DESCRIPTION,
                                                    dt,
                                                    parametersIn["ta"].ToString() + " - " + strDtaLang,
                                                    GetEmailFooter(ref oUser), strZone, strSector, strEnforcuser);


                                        }
                                        SendEmail(ref oUser, strExtTicketEmailHeader, strExtTicketEmailBody, dSourceApp);
                                    }
                                }
                            }

                            parametersOut = new SortedList();
                            if (bExistAUser)
                            {
                                if (!infraestructureRepository.AddExternalPlateFine(dInstallationId.Value, strPlate, dt, dtUTC.Value,
                                                                              strFineNumber, iQuantity,
                                                                              df, dfUTC.Value,
                                                                              parametersIn["ta"].ToString(),
                                                                              strDta))
                                {
                                    Logger_AddLogMessage(string.Format("3rdPNotifyPlateFine::Error: AddExternalPlateFine: {0}", PrettyXml(xmlIn)), LogLevels.logERROR);
                                }
                                parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();
                            }
                            else
                            {
                                parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_PlateNotExist).ToString();
                                
                            }

                            xmlOut = GenerateXMLOuput(parametersOut);

                            if (xmlOut.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateFine::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateFine: xmlOut={0}",PrettyXml(xmlOut)), LogLevels.logINFO);
                            }

                        }
                    }

                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("3rdPNotifyPlateFine::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("3rdPNotifyPlateFine::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);

            }

            return xmlOut;
        }

        [WebMethod]
        public string NotifyPlateFineJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                Logger_AddLogMessage(string.Format("3rdPNotifyPlateFineJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = NotifyPlateFine(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

                Logger_AddLogMessage(string.Format("3rdPNotifyPlateFineJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("3rdPNotifyPlateFineJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }

        

        [WebMethod]
        public string NotifyPlateParking(string xmlIn)
        {
            string xmlOut = "";
            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking: xmlIn={0}",PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["city_id"] == null) ||
                        (parametersIn["p"] == null) ||
                        (parametersIn["ed"] == null) ||
                        (parametersIn["vers"] == null) ||
                        (parametersIn["prov_name"] == null) ||
                        (parametersIn["d"] == null) ||
                        (parametersIn["srcType"] == null) ||                        
                        (parametersIn["type"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);

                    }
                    else
                    {
                        string strCalculatedHash = CalculateHash(strHashString, strHash);

                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {

                            if (string.IsNullOrEmpty(parametersIn["p"].ToString()))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            string strPlate = parametersIn["p"].ToString();
                            strPlate = strPlate.Trim().Replace(" ", "");

                            DateTime ed;
                            DateTime? edUTC;
                            try
                            {
                                try
                                {
                                    ed = DateTime.ParseExact(parametersIn["ed"].ToString(), "HHmmssddMMyy",
                                    CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    ed = DateTime.ParseExact(parametersIn["ed"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                    CultureInfo.InvariantCulture);
                                }
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;

                            }

                            decimal? dInstallationId = null;

                            try
                            {
                                decimal dTryInstallationId = Convert.ToDecimal(parametersIn["city_id"].ToString());
                                dInstallationId = dTryInstallationId;
                            }
                            catch
                            {
                                dInstallationId = null;
                            }

                            INSTALLATION oInstallation = null;
                            DateTime? dtinstDateTime = null;
                            decimal? dLatitude = null;
                            decimal? dLongitude = null;

                            if (!geograficAndTariffsRepository.getInstallation(dInstallationId,
                                                                         dLatitude,
                                                                         dLongitude,
                                                                         ref oInstallation,
                                                                         ref dtinstDateTime))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }



                            DateTime? bd = null;
                            DateTime? bdUTC = null;
                            if (parametersIn["bd"] != null)
                            {
                                try
                                {
                                    try
                                    {
                                        bd = DateTime.ParseExact(parametersIn["bd"].ToString(), "HHmmssddMMyy",
                                        CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        bd = DateTime.ParseExact(parametersIn["bd"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                        CultureInfo.InvariantCulture);
                                    }

                                }
                                catch
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                    Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;

                                }
                            }


                            int? iQuantity = null;
                            if (parametersIn["q"] != null)
                            {
                                try
                                {
                                    iQuantity = Convert.ToInt32(parametersIn["q"].ToString());
                                }
                                catch
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                    Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;

                                }
                            }

                            int? iTime = null;
                            if (parametersIn["t"] != null)
                            {
                                try
                                {
                                    iTime = Convert.ToInt32(parametersIn["t"].ToString());
                                }
                                catch
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                    Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;

                                }
                            }


                            decimal? dGroup = null;
                            decimal? dTariff = null;
                            string strGroupExtId = "";
                            string strTariffExtId = "";

                            strGroupExtId = parametersIn["g"].ToString();
                            strTariffExtId = parametersIn["ad"].ToString();

                            if (!geograficAndTariffsRepository.GetGroupAndTariffFromExternalId(4, dtinstDateTime.Value, oInstallation, strGroupExtId, strTariffExtId, ref dGroup, ref dTariff))
                            {
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::GetGroupAndTariffFromExternalId.Error: xmlIn={0}, Group or Tariff not found", PrettyXml(xmlIn)), LogLevels.logERROR);
                            }


                            edUTC = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(dInstallationId.Value, ed);
                            if (bd != null)
                            {
                                bdUTC = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(dInstallationId.Value, bd.Value);
                            }

                            if (!infraestructureRepository.ExistPlateInSystem(strPlate))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_PlateNotExist);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }


                            EXTERNAL_PROVIDER oExternalProvider = null;
                            if (!geograficAndTariffsRepository.getExternalProvider(parametersIn["prov_name"].ToString(), 
                                                                                   ref oExternalProvider))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_ExternalProvider);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            DateTime d;
                            DateTime? dUTC;
                            try
                            {
                                try
                                {
                                    d = DateTime.ParseExact(parametersIn["d"].ToString(), "HHmmssddMMyy",
                                                            CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    d = DateTime.ParseExact(parametersIn["d"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                                            CultureInfo.InvariantCulture);
                                }
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;

                            }
                            dUTC = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(dInstallationId.Value, d);

                            OperationSourceType srcType;
                            try
                            {
                                srcType = (OperationSourceType)Convert.ToInt32(parametersIn["srcType"].ToString());
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            string strSrcIdent = null;
                            if (parametersIn["srcIdent"] != null) strSrcIdent = parametersIn["srcIdent"].ToString();

                            ChargeOperationsType chargeType;
                            try
                            {
                                chargeType = (ChargeOperationsType)Convert.ToInt32(parametersIn["type"].ToString());
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            string strOperationId1 = null;
                            if (parametersIn["operation_id1"] != null) strOperationId1 = parametersIn["operation_id1"].ToString();
                            string strOperationId2 = null;
                            if (parametersIn["operation_id2"] != null) strOperationId2 = parametersIn["operation_id2"].ToString();

                            decimal dOperationId;

                            if (!infraestructureRepository.AddExternalPlateParking(dInstallationId.Value, strPlate, 
                                                                                   d, dUTC.Value, ed, edUTC.Value, dGroup, dTariff, bd, bdUTC, iQuantity, iTime,
                                                                                   oExternalProvider.EXP_ID, srcType, strSrcIdent, chargeType, strOperationId1, strOperationId2, out dOperationId))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }


                            parametersOut = new SortedList();
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();
                            parametersOut["operation_id"] = Convert.ToInt32(dOperationId);

                            xmlOut = GenerateXMLOuput(parametersOut);

                            if (xmlOut.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking: xmlOut={0}",PrettyXml(xmlOut)), LogLevels.logINFO);
                            }


                        }
                    }

                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);

                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("3rdPNotifyPlateParking::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);

            }

            return xmlOut;
        }

        [WebMethod]
        public string NotifyPlateParkingJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                Logger_AddLogMessage(string.Format("3rdPNotifyPlateParkingJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = NotifyPlateParking(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

                Logger_AddLogMessage(string.Format("3rdPNotifyPlateParkingJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("3rdPNotifyPlateParkingJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }


        [WebMethod]
        public string QueryPlateLastParkingOperation(string xmlIn)
        {
            string xmlOut = "";
            try
            {

                if (_MexicoPlates == null)
                {
                    _MexicoPlates = new SortedList<string, string>();

                    try { _MexicoPlates.Add("CRN1550", "-HONDA-FEBO-CREDITO CONSTRUCTOR-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("931UWZ", "PORSCHE-VOLKSWAGEN-BARRANCA DEL MUERTO-CREDITO CONSTRUCTOR-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("981SBX", "RAV-TOYOTA-POSEIDON-CREDITO CONSTRUCTOR-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("932SDK", "TOWN & COUNTRY-CHRYSLER-CDA NEZAHUALCOYOTL-CREDITO CONSTRUCTOR-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("368ZAW", "MATIZ-CHEVROLET-HERA-CREDITO CONSTRUCTOR-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("339ZDJ", "SONIC-SONIC-FEBO-CREDITO CONSTRUCTOR-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("649WAT", "VERNA (IMPORTADO)-CHRYSLER-FEBO-CREDITO CONSTRUCTOR-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MEH2444", "BORA-VOLKSWAGEN-HERA-CREDITO CONSTRUCTOR-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("748PVM", "DART-CHYRSLER-MOSQUETA-CREDITO CONSTRUCTOR-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("261XFB", "CR-V-HONDA-HERA-CREDITO CONSTRUCTOR-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("703YUT", "AVEO-GM CHEVROLET-RIO MIXCOAC-CREDITO CONSTRUCTOR-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("120XEY", "JEEP-CHRYSLER-HERA-CREDITO CONSTRUCTOR-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MGD2945", "JETTA-VOLKSWAGEN-HERA-CREDITO CONSTRUCTOR-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("DPN8943", "AVANZA-TOYOTA-RIO MIXCOAC-CREDITO CONSTRUCTOR-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("549TCF", "GM FIAT-PALIO-FEBO-CREDITO CONSTRUCTOR-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("LWZ7239", "ECONOLINE-FORD-POSEIDON-CREDITO CONSTRUCTOR-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MXE5966", "AUDI-AUDI--CREDITO CONSTRUCTOR -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("938UKJ", "STRATUS-DODGE--CREDITO CONSTRUCTOR -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("DE750", "DART-DODGE--CREDITO CONSTRUCTOR -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MUZ3317", "FOCUS-FORD--CREDITO CONSTRUCTOR -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("DSH7340", "AVANZA-TOYOTA--CREDITO CONSTRUCTOR -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("414VYE", "SERIE 1-BMW-FEBO-CREDITO CONSTRUCTOR -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("507XMP", "OUTLANDER-MITSUBISHI-MOSQUETA-CREDITO CONSTRUCTOR -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("PYR7260", "CLASE C-MERCEDS BENZ-CDA NEZAHUALCOYOTL-CREDITO CONSTRUCTOR -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("L69AGY", "SONIC-CHEVROLET -PERUGINO-EXTREMADURA INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("220ZPX", "CLIO-RENAULT-RERUGINO-EXTREMADURA INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("420XCR", "-NISSAN-EMPRESA-EXTREMADURA INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("567XJB", "-CHRYSLER-PORFIRIO DIAZ-EXTREMADURA INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("B45AEW", "SONIC-CHEVROLET--EXTREMADURA INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("555YPV", "RAV 4-TOYOTA--EXTREMADURA INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MTC1589", "TIIDA-NISSAN-PERUGINO-EXTREMADURA INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("958ZFK", "JETTA-VOLKSWAGEN-CADIZ NORTE-EXTREMADURA INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("UKA415F", "SEAT-VOLKSWAGEN -COROT-EXTREMADURA INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MSU5315", "BEATLE-VOLKSWAGEN-PERUGINO-EXTREMADURA INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MAM4455", "MEGANE-RENAULT-CDA. PERUGINO-EXTREMADURA INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("912SWN", "JEEP-CHRYSLER -CADIZ NORTE-EXTREMADURA INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("779ZYM", "SONIC (NACIONAL)-GM CHEVROLET -PERUGINO-EXTREMADURA INSURGENTES-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("L38AAM", "AVANZA-TOYOTA-AUGUSTO RODIN-EXTREMADURA INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("908RYE", "X-TRAIL-NISSAN -CDA MILLET-EXTREMADURA INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("513YZR", "AVEO NACIONAL-GM CHEVROLET-PERUGINIO-EXTREMADURA INSURGENTES-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("474PTH", "SPIRIT-CHRYSLER -CDA MILLET-EXTREMADURA INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("309MZD", "PONTIAC-GM-CARRACI-EXTREMADURA INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("284MYC", "VERNA-CHRYSLER-C. CARRACI-EXTREMADURA INSURGENTES-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("217XVH", "COROLLA-TOYOTA-PORFIRIO DIAZ-EXTREMADURA INSURGENTES-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("882XPM", "-CHRYSLER -VIZCAYA-EXTREMADURA INSURGENTES -EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("145RUX", "POINTER-VOLKSWAGEN-MILLET-EXTRENADURA INSURGENTES-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("384XXC", "MAZDA 3-MAZDA-EXTREMADURA -INSURGENTES MIXCOAC-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("432XVC", "JEEP-CHRYSLER -EXTREMADURA -INSURGENTES MIXCOAC-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("MYL9856", "-SEAT-EXTREMADURA-INSURGENTES MIXCOAC-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("MKF4547", "MARCH-NISSAN-SANTANDER-INSURGENTES MIXCOAC-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("R60AHV", "VERSA-NISSAN-CAMPANA-INSURGENTES MIXCOAC-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("B04ACH", "MATIZ-PONTIAC -RIO MIXCOAC-INSURGENTES MIXCOAC-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("KS94880 ", "PICKUP-DOGDE-SANTANDER-INSURGENTES MIXCOAC-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("LSV2118 ", "FIESTA-FORD -EXTREMADURA-INSURGENTES MIXCOAC-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("684UBW", "-VOLKSWAGEN SEAT-EXTREMADURA-INSURGENTES MIXCOAC-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("MAN5746", "PLATINA-NISSAN -JEREZ-INSURGENTES MIXCOAC-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("MDX6738", "-VOLKSWAGEN-GOYA-INSURGENTES MIXCOAC-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("GST7245", "GOL-VOLKSWAGEN--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("P11AAN", "POLO-VOLKSWAGEN--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("323TTF", "CIVIC-HONDA--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("M86AED", "FIESTA-FORD--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MXP2235", "BEETLE-VOLKSWAGEN--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("V70AGB", "VENTO-VOLKSWAGEN--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("530TTD", "206-PEUGEOT--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("981XFT", "CHEVY-GENERAL MOTORS--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MPR6019", "-CHEVROLET--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("211YLL", "SEAT-VOLKSWAGEN--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("Y08AFT", "MAZDA3-MAZDA--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("451SSX", "CONVERTIBLE-MG--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("Y45AEC", "JETTA-VOLKSWAGEN--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("N20AFK", "VERSA-NISSAN --INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("798WMG", "SAFRANE-RENAULT--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("174XBM", "SUBURBAN-CHEVROLET--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("YKL9828", "TSURU-NISSAN--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("Z25AEG", "SONIC-CHEVROLET--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("334WYA", "2010-SUBARU--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MNJ8272", "CHEVY-CHEVROLET--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("311ZFM", "301-PEUGEOT--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("M60ADH", "CLIO-RENAULT--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MNV3726", "CRV-HONDA--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("590YLE", "BEETLE-VOLKSWAGEN -MALAGA-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MTU4170", "FIESTA-FORD-AUGUSTO RODIN-INSURGENTES MIXCOAC-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("830ZPE", "GOL-VOLKSWAGEN-SANTANDER-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MRB8788", "JETTA-VOLKSWAGEN-PATRIOTISMO-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("375PFB", "SHADOW-CHRYSLER-CAMPANA-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("263TMP", "COURIER-FORD-ACTIPAN-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("608NPB", "CIVIC-HONDA -MALAGA-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("879MSH", "TSURU-NISSAN-ANTONIO CANOVA-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("LXZ1521", "PLATINA-NISSAN--INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("USA933B", "BORA-VOLKSWAGEN-GOYA-INSURGENTES MIXCOAC-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("SMT2523", "-BMW-DONATELLO-INSURGENTES MIXCOAC-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MSM4175", "CIVIC-HONDA-SANTANDER-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("888XPF", "CORSA-GM-CAMPANA-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("217TDF", "X-TRAIL-NISSAN-CADIZ-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("398THF", "POINTER-VOLKSWAGEN-SANTANDER-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("501ZDV", "PATHFINDER-NIISSAN-DONATELLO-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("E16ABZ", "SONIC-GM CHEVROLET-SANTANDER-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("765SDR", "VERNA-CHRYSLER-AGUSTO RODIN -INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MAG7392", "COMBI-VOLKSWAGEN-ACTIPAN -INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("587UGY", "SEDAN-VOLKSWAGEN-MALAGA-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("365ZAN", "JETTA-VOLKSWAGEN-CAMPANA -INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("140XXA", "CR-V-HONDA-VALENCIA-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("319YBU", "DODGE ATTITUDE-CHRYSLER-VALENCIA-INSURGENTES MIXCOAC-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("MHJ8180", "MEGANE SCENIC 4 PTAS-RENAULT-ASTURIAS-INSURGENTES MIXCOAC-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("373SUU", "IKON-FORD-MALAGA -INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("780RDV", "SEAT-VOLKSWAGEN-GOYA-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("625MCD", "SENTRA-NISSAN -CADIZ-INSURGENTES MIXCOAC-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("899ZLZ", "CX-7-MAZDA-JEREZ-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("SS76151", "RANGER XLT-FORD -GOYA-INSURGENTES MIXCOAC-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MMR9264", "F-150-FORD -DONATELLO-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("364XRV", "CHEVY-GM-CAMPANA-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("424RBE", "SEAT-VOLKSWAGEN-ALGECIRAS-INSURGENTES MIXCOAC-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("436WKK", "TIDA-NISSAN -C. MALAGA-INSURGENTES MIXCOAC-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("676SCP", "X-TRAIL-NISSAN-FLORIDA-NOCHE BUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("928YKR", "HONDA CITY-HONDA-FLORIDA-NOCHE BUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MPB5823", "CAPTIVA SPORT-CHEVROLET-HOLBEIN-NOCHE BUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("433ZSU", "ESCAPE-FORD-PORFIRIO DIAZ-NOCHE BUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MBN1490", "TRAILBLAZER-CHEVROLET-DETROIT-NOCHE BUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("185UYD", "CUTLASS-GM-HOLBEIN-NOCHE BUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("871UFC", "CHEVY-GM -HOLBEIN-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("LYZ7642", "DERBY-VOLKSWAGEN-FLORIDA-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("V18AHT", "--DETROIT-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("PYN1693", "-JEEP-CLEVELAND-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("456ZWW", "CIVIC-HONDA-COROT-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("304WEP", "CIVIC-HONDA -BOSTON-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("147XGV", "CHEVY-GM -HOLBEIN-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("V95AAZ", "COROLLA-TOYOTA-FLORIDA-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("588TZW", "KA-FORD -FLORIDA-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("258ZVM ", "FIAT500-FIAT-ATLANTA-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("R51AFH ", "-CHRYSLER-FLORIDA-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("923YPS", "ACCORD-HONDA -CAROLINA-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("875YFD", "-VOLKSWAGEN SEAT-DETROIT-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("461ZNC", "FIESTA-FORD-HOLBEIN-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("505YAD", "-VOLKSWAGEN SEAT-FLORIDA -NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("714ZYV", "-FORD-BOSTON-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("MVV5856", "JEEP-CHRYSLER--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("H50AEU", "SWIFT-SUZUKI--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("844XJK", "207-PEUGEOT--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("894WAD", "206-PEUGEOT--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("EKA8587", "GOLF-VOLKSWAGEN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("951WUJ", "NUEVO GOL-VOLKSWAGEN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MSK6415", "TOWN & COUNTRY-CHRYSLER--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("751XVV", "SWIFT-SUZUKI--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("A22AHC", "TIDA-NISSAN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("JKR2563", "TIGUAN-VOLKSWAGEN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("300YDP", "CIVIC-HONDA--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("EKC7872", "FIT-HONDA--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("200YBY", "JETTA-VOLKSWAGEN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("426YJL", "CX-7-MAZDA--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("W08AGD", "MAZDA 5-MAZDA--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("671ZUT", "CITY-HONDA--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("F66AAZ", "SERIE 1-BMW--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("P18AFU", "CROSSFOX-VOLKSWAGEN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("626YUA", "DUSTER-RENAULT--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("K78AFM", "VERSA-NISSAN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("F05AET", "SEAT-VOLKSWAGEN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("LYK5958", "MONZA-CHEVROLET--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("H28AFL", "VENTO-VOLKSWAGEN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("582ZXA", "SEAT-VOLKSWAGEN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("109WKE", "ATITTUDE-CHEVROLET--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("323XSV", "CROSSFOX-VOLKSWAGEN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("511ZYF", "NOTE-NISSAN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("D14ACX", "VENTO-VOLKSWAGEN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("765XFG", "MINI COOPER-BMW--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("336SEE", "DERBY-VOLKSWAGEN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("705PAC", "TRACKER-GM CHEVROLET--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("L74AED", "FIT-HONDA--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("L21ADZ", "PRIUS-TOYOTA--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("351ZVR", "MARCH-NISSAN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("681YGM", "FOCUS-FORD--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("507YJG", "FIT-HONDA--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("Z34ACL", "6-MAZDA--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("301WLD", "LUPO-VOLKSWAGEN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MUC6240", "MINI-MINI COOPER--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("464ZAV", "COROLLA-TOYOTA--NOCHEBUENA-CANCELADO"); }
                    catch { };
                    try { _MexicoPlates.Add("C76ADR", "MARCH-NISSAN--NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("253XMG", "CHEVROLET-GM-DETROIT-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("369XEU", "CHEVROLET-GM-PORFIRIO DIAZ-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("L90ABR", "XTRAIL-NISSAN -BOSTON-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("722TNK", "FIESTA-FORD-BOSTON-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("408VDF", "KA-FORD-BOSTON-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("544XLW", "STEPWAY-RENAULT-CINCINNATI-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("155NHW", "GOLF-VOLKSWAGEN-AV. INSURGENTES SUR-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("766VVV", "TIIDA SEDAN-NISSAN-FLORIDA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("637YKG", "SEDAN-VOLKSWAGEN-FLORIDA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("195UZA", "CHARGER-CHRYSLER-CAROLINA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("ACL3705", "KANGOO-RENAULT-HOLBEIN-NOCHEBUENA-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("822WCY", "YARIS-TOYOTA-CLEVELAND-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("601ZZX", "FORESTER-SUBARU-HOLBEIN-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("660YYK", "DODGE DURANGO-CHRYSLER-DETROIT-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("YGB1459", "SANDERO-REANULT-FLORIDA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("474ZSP", "FIESTA NOTCH-FORD-HOLBEIN-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("971THJ", "CHEVY-GM CHEVROLET-ATLANTA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("713ZGS", "FOCUA-FORD-BOSTON-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("531NKB", "POINTER-VOLKSWAGEN-CINCINNATI-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("720THD", "LUPO-VOLKSWAGEN-BOSTON-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("LRJ5536", "ALTIMA SEDAN-NISSAN-BOSTON-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("646ZPJ", "CRUZE IMPORTADO-GM CHEVROLET-BOSTON-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("429TMJ", "MONDEO-FORD-BOSTON-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("730XZY", "COROLLA-TOYOTA -CINCINATTI-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("283XND", "JETTA CLASICO-VOLKSWAGEN-BALTIMORE-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("952PFP", "SPIRIT-CHRYSLER-FLORIDA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("774ZPF", "CROSS FOX-VOLKSWAGEN-CLEVELAND-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("228WZG", "CR-V-HONDA-BALTIMORE-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("423NXB", "CIVIC-HONDA-HOLBEIN-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("634YXJ", "X-TRAIL-NISSAN-BOSTON-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("EEZ1875", "GOLF-VOLKSWAGEN-CAROLINA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("288TCS", "JETTA-VOLKSWAGEN-DETROIT-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("718XCK", "SCALA-RENAULT-BOSTON -NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("453ZZN", "VERSA-NISSAN-PORFIRIO DIAZ-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("812YWF", "SWIFT (IMPORTADO)-SUZUKI-FLORIDA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("553YPD", "DUSTER-RENAULT-BALTIMORE-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("377TZS", "ALTIMA-NISSAN-FLORIDA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MER5058", "COROLLA-TOYOTA-CINCINNATI-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MUN1210", "SONATA-HYUNDAI-BOSTON-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("390XCL", "NUEVO GOL-VOLKSWAGEN-CINCINATTI-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("341MBA", "TSURU SEDAN-NISSAN-FLORIDA-NOCHEBUENA-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("960WTD", "RANGER-FORD-FLORIDA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("763UGR", "TORNADO-GM CHEVROLET-DETROIT-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("558XBE", "EXPLORER-FORD-BALTIMORE-NOCHEBUENA-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("779XGJ", "FIESTA IKON-FORD-FLORIDA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("699UCC", "FIESTA NOTCH-FORD-CINCINATTI-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("816MDS", "POINTER-VOLKSWAGEN-FLORIDA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("174XPN", "SHADOW-CHRYSLER-FLORIDA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("694SPH", "SEAT-VOLKSWAGEN-FLORIDA-NOCHEBUENA-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("725RVJ", "JETTA-VOLKSWAGEN-ATLANTA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("232TBD", "FIESTA-FORD-FLORIDA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("598YJV", "TIDA-NISSAN-CINCINATTI-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("840SRU", "SENTRA-NISSAN-CAROLINA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("735PBD", "ESCORT-FORD-CAROLINA-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("621WWN", "CHEVY-GM-HOLBEIN-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("162XWV", "VERSA-NISSAN-HOLBEIN-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("529TDH", "PALIO-GM FIAT-DETROIT-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("992SZY", "DODGE STRATUS-CHRYSLER-DETROIT-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("AFC6317", "SENTRA SEDAN-NISSAN-FLORIDA-NOCHEBUENA-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("170MNX", "JETTA-VOLKSWAGEN-PORFIRIO DIAZ-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("321MTG", "ACCORD-HONDA-BALTIMORE-NOCHEBUENA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("801YLP", "MAZDA 3-MAZDA-DETROIT-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("764YHL", "FIESTA-FORD-HOLBEIN-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("799ZUR", "JETTA-VOLKSWAGEN-CINCINNATTI-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("143ZEZ", "MAZDA 6-MAZDA-CINCINATTI-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("447PHF", "GM-CHEVY-FLORIDA-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("858ZJX", "AUDI-VOLKSWAGEN-PORFIRIO DIAZ-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("HNH5232", "AVEO-CHEVROLET-FLORIDA-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("953TTC", "CLIO-RENAULT-FLORIDA-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("817WUD", "AVEO-CHEVROLET-HOLBEIN-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("100ZMD", "FRONTIER-NISSAN-ATLANTA-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("PFG7521", "TROOOPER VAGONETA-ISUZU-BOSTON-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("SMV7479", "BEETLE-VOLKSWAGEN-FLORIDA-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("974ZTR", "MARCH-NISSAN -FLORIDA-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MRN9922", "CAPTIVA SPORT-CHEVROLET-ATLANTA-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("PYK2447", "MINI COOPER-BMW-CLEVELAND-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("768WAB", "CAPTIVA SPORT-CHEVROLET-BOSTON-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MDF5228", "MATIZ-PONTIAC-FLORIDA-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("992SPU", "CIVIC-HONDA-DETROIT-NOCHEBUENA -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("241XZN", "TIGUAN-VOLKSWAGEN-JOSE MARIA IBARRAGAN-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("PYP5175", "FORTE-KIA -ANDRES DE LA CONCHA-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("391YAD", "-VOLKSWAGEN SEAT-MIGUEL NOREÑA-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("MWL2853", "MINIVAN-NISSAN -SAGREDO-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("MXW6673", "POLO-VOLKSWAGEN-SALOMA PIÑA-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("634NVV", "SERIE 3-BMW-MERCADERES-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("585YET ", "VERSA-NISSAN -CDA. PERPETUA-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("240ZWJ ", "FOCUS-FORD-SAGREDO-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("114XRH", "MAZDA 3-MAZDA -JOSE MARIA IBARRARAN-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("887YTA", "FEAT-CHRYSLER -DEL ANGEL-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("771WBD", "-CHEVROLET-DEL ANGEL-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("400XFL", "-TOYOTA-JOSE MARIA VELASCO-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("VGX2274", "-RENAULT-JOSE MARIA VELASCO-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("683ZAP", "CORSA-GM-CERVERO RODRIGUEZ-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("279SKL", "POINTER-VOLKSWAGEN-CORREGIO-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("MYR2324", "CRUZE-CHEVROLET--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("985XJZ", "COROLLA-TOYOTA--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("237YEG", "AVEO-CHEVROLET--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MTH4794", "-PEUGEOT--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("475WDL", "CROSSFOX-VOLKSWAGEN--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MKX8500", "PASSAT-VOLKSWAGEN--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MDR3169", "-SEDAN--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("134WUT", "CRV-HONDA--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("299YHZ", "KANGOO-RENAULT--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("503ZVU", "ODISSEY-HONDA--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("328ZPS", "CITY-HONDA--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("867ZSS", "SONIC-CHEVROLET--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("M53AFB", "SPARK-CHEVROLET--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("924ZNV", "SPARK-HEVROLET--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("Z54AEC", "MAZDA 3-MAZDA--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("J20ADS", "MITSUBISHI-CHRYSLER--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("662WTX", "X-TRAIL-NISSAN--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("693VCE", "DODGE-CHRYSLER--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("P86AER", "MAZDA 6-MAZDA--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MCE3079", "YARIS-TOYOTA--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("F69ADZ", "JETTA-VOLKSWAGEN--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MNX8933", "JETTA-VOLKSWAGEN--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("223YMN", "FIAT-CHRYSLER--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MCJ7565", "COROLLA-TOYOTA--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("810XJS", "AVEO -CHEVROLET--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("R76AEN", "JETTA-VOLKSWAGEN--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MMY1257", "CLIO-RENAULT--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("432WHC", "6-MAZDA--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("J90ADZ", "MARCH-NISSAN --SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("E95ADV", "JETTA-VOLKSWAGEN--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("627WLN", "CLIO-RENAULT--SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("231ZGC", "BEETLE-VOLKSWAGEN-SAGREDO -SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("977YNP", "PRIUS-TOYOTA-PERPETUA-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("495WHK", "GOL-VOLKSWAGEN-SAGREDO-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("H50ABC", "SPARK-CHEVROLET-SALOME PIÑA-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("844VBX", "LUV-GM-JOSE MARIA IBARRAGAN-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MRP6276", "CX-5-MAZDA-SAGREDO-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("862UUJ", "CHEVROLET-GM-SAGREDO-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("560MEJ", "DODGE NEON-CHRYSLER-DAMAS-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("597XAW", "ATTITUD-DODGE-SAGREDO-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("976XVV", "GRAND VITARA-SUZUKI-FELIX PARRA-SAN JOSE INSURGENTES-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("LYM6532", "GRAND CHEROKEE-JEEP-CORDOBANES-SAN JOSE INSURGENTES-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MSM8891", "MAZDA-MAZDA-ANGEL-SAN JOSE INSURGENTES-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MTS6996", "A1 COUPÉ-AUDI-MERCADERES-SAN JOSE INSURGENTES-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("783TEA", "SENTRA-NISSAN-MATEO HERRERA-SAN JOSE INSURGENTES-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("529ZZJ", "CHRYSLER 200-CHRYSLER-DAMAS-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("139ZCK", "TRAX-GM CHEVROLET-CORDOBANES -SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("494ZVU", "SENTRA-NISSAN -SAGREDO-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("772YHV", "AUDI-VOLKSWAGEN-ANGEL-SAN JOSE INSURGENTES-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("918WZP", "JEEP-CHRYSLER-FELIZ PARRA-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("486ZJS", "ACCORD-HONDA-JOSE MARIA IBARRARAN-SAN JOSE INSURGENTES-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("577ZWJ", "CIVIC-HONDA-MATIAS HERRERA-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("990YTP", "JETTA CLASICO-VOLKSWAGEN-SAGREDO-SAN JOSE INSURGENTES-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("576XTN", "ATHOS-CHRYSLER-SAGREDO-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("A68426", "TIIDA-NISSAN-AV. REVOLUCIÓN-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("Z98ABL", "MITSUBISHI-CHRYSLER-JOSE MARIA IBARRARAN-SAN JOSE INSURGENTES-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("654WSN", "NUEVO GOL-VOLKSWAGEN-SAGREDO-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("461XUX", "DODGE STRATUS-CHRYSLER-SAGREDO-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("616YEC", "JETTA-VOLKSWAGEN -SALOME PIÑA-SAN JOSE INSURGENTES-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("391NAW", "GOLF-VOLKSWAGEN-DAMAS-SAN JOSE INSURGENTES-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("710ZSY", "GRAND I10-HYUNDAI-FLAMENCOS-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("686WLB", "ACCORD-HONDA -BARRANCA DEL MUERTO-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("341UUJ", "CRV-HONDA -JOSE MARIA VELASCO -SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("786XFL", "CAPTIVA SPORT (NACIONAL)-GM CHEVROLET-JOSE MARIA VELASCO-SAN JOSE INSURGENTES-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("292VEF", "POINTER-VOLKSWAGEN-MERCADERES-SAN JOSE INSURGENTES-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("262YRF", "SONIC-CHEVROLET-FELIX PARRA-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("849WAL", "ECO SPORT-FORD-CDA.MERCADERES-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("187PCT", "FORD-FORD-CERRADA DE PERPETUA -SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("943VCM", "BLAZER-GM-PRIVADA DE VENECIA-SAN JOSE INSURGENTES-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("925XLY", "GOL-VOLKSWAGEN-DIEGO BECERRA-SAN JOSE INSURGENTES-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("403YUA", "CR-V-HONDA-JOSE MARIA VELASCO-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("412SAH", "POINTER-VOLKSWAGEN-MERCADERES-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("113XJT", "LINEA INDEFINIDA-FORD-FELIX PARRA-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("760WPZ", "ESCAPE-FORD-DIEGO BECERRA-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("256VVR", "TSURU SEDAN-NISSAN-SATURNINO HERRAN-SAN JOSE INSURGENTES-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("B13243", "PLATINA-NISSAN-DIAGO BECERRA -SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("J78ACW", "TOWN & COUNTRY-CHRYSLER-ANDRES DE LA CONCHA-SAN JOSE INSURGENTES-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("KZ22008", "TRAFIC-RENAULT-ANDRES DE LA CONCHA-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("732TDM", "VERNA-CHRYSLER-SALOME -SAN JOSE INSURGENTES-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("264TCT", "POLO-VOLKSWAGEN-FELIX PARRA -SAN JOSE INSURGENTES-DENEGADO"); }
                    catch { };
                    try { _MexicoPlates.Add("723YKS", "X-TRAIL-NISSAN -SAGREDO-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("615WNK", "CROSSFOX-VOLKSWAGEN-SATURNINO HERRAN-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("SLH9703", "CHEVY-CHEVROLET-PARQUE DEL CONDE -SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("914XLH", "TIDA SEDAN -NISSAN -SAGREDO-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("380VWE", "MAZDA 3-MAZDA-CDA MIGUEL NOREÑA -SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("864UNZ", "SPORTVAN-VOLKSWAGEN-SAGREDO-SAN JOSE INSURGENTES-EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("503ZYP", "CRUZE-CHEVROLET-JOSE IBARRARAN-SAN JOSE INSURGENTES-RENOVACIÓN"); }
                    catch { };
                    try { _MexicoPlates.Add("117SRC", "SILVERADO-GM-RODRIGO CIFUENTES-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("346RVY", "CHEVY-GM-LORENZO RODRIGUEZ-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("212ZND", "TOWN AND COUNTRY-CHRYSLER-CDA MERCADERES-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("712WMP", "OUTLANDER-MITSUBISHI-C. SAGREDO-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("545RDV", "ASTRA-GM-C. MATEO HERRERA-SAN JOSE INSURGENTES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("162YRA", "COROLLA-TOYOTA-SAGREDO-SAN JOSE INSURGENTES -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("650SLN", "CIVIC-HONDA-SAGREDO-SAN JOSE INSURGENTES -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("918NJY", "ODISSEY MINIVAN-HONDA-ANDRES DE LA CONCHA -SAN JOSE INSURGENTES -EN PROCESO"); }
                    catch { };
                    try { _MexicoPlates.Add("JFB6143", "SIENNA-TOYOTA-SAGREDO-SAN JOSE INSURGENTES -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("141NMB", "ESCORD-FORD-SATURNINO HERRAN -SAN JOSE INSURGENTES -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("Y54ABU", "SPARK-CHEVROLET--SAN JOSE INSURGENTES 6-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("214NMP", "GRAND MARQUIS-FORD-RODRIGO CIFUENTES-SAN JOSE INSURGENTRES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("133RRS", "SUBURBAN-DELEGACION-DONATELLO-DENEGADO COCHERA"); }
                    catch { };
                    try { _MexicoPlates.Add("136YCX", "FIAT-DELEGACION-SAGREDO -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("149RWP", "CAVALIER-DELEGACION-MOSQUETA-DENEGADO CUENTA CON TARJETON"); }
                    catch { };
                    try { _MexicoPlates.Add("173UUR", "-DELEGACION-MOSQUETA-NO HAY SOLICITUD"); }
                    catch { };
                    try { _MexicoPlates.Add("174ZMH", "IBIZA GRIS-MARFER-I. MIXCOAC-MARFER"); }
                    catch { };
                    try { _MexicoPlates.Add("184ZPG", "BEETLE-DELEGACION-FEDERICO-EN TRAMITE"); }
                    catch { };
                    try { _MexicoPlates.Add("212SUA", "-DELEGACION--NO HAY SOLICITUD"); }
                    catch { };
                    try { _MexicoPlates.Add("282YYD", "JETTA-TEODORO-TEODORO W-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("287TZZ", "PASSAT AZUL-DELEGACION-MERCADERES-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("333WRM", "POLO--DIEGO BECERRA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("335YAH", "XTERRA-DELEGACION-MOSQUETA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("380UFH", "PASSAT-DELEGACION-DONATELLO-DENEGADO COCHERA"); }
                    catch { };
                    try { _MexicoPlates.Add("440TFK", "RAV4-DELEGACION-DONATELLO-DENEGADO COCHERA"); }
                    catch { };
                    try { _MexicoPlates.Add("447MMD", "CHEVY-ANGEL LUNA-PERPETUA-DENEGADO COCHERA"); }
                    catch { };
                    try { _MexicoPlates.Add("496ZMM", "XTRAIL-DELEGACION-MOSQUETA-DENEGADO COCHERA"); }
                    catch { };
                    try { _MexicoPlates.Add("502PVZ", "GRAND MARQUIS--VALENCIA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("511YZZ", "VOLKSWAGEN VENTO-AEP-DAMAS 118-NO HAY SOLICITUD"); }
                    catch { };
                    try { _MexicoPlates.Add("519UZC", "-DELEGACION-MOSQUETA-NO HAY SOLICITUD"); }
                    catch { };
                    try { _MexicoPlates.Add("523SZC", "MATRIX--CATALUÑA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("532WTT", "ATTITUD -DELEGACION-PLATEROS MONICA BASOL-DENEGADO COCHERA"); }
                    catch { };
                    try { _MexicoPlates.Add("541YYL", "ATTITUD-AEP-RIO MIXCOAC-AMLO"); }
                    catch { };
                    try { _MexicoPlates.Add("572WEJ", "TIIDA BLANCO-DELEGACION-MOSQUETA-NO HAY SOLICITUD"); }
                    catch { };
                    try { _MexicoPlates.Add("599RUZ", "CHEVY NEGRO-DELEGACION-MARINO-NO HAY SOLICITUD"); }
                    catch { };
                    try { _MexicoPlates.Add("652UKN", "SPOTVAN--DON JUAN MANUEL -AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("697RXX", "MALIBU 2000--CARRACCI 102-4-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("701VHJ", "FORD EXPLORER-DELEGACION-MATEO HERRERA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("749RWP", "CAVALIER-AEP-OMAR-NO HAY SOLICITUD"); }
                    catch { };
                    try { _MexicoPlates.Add("759VYS", "MATIZ PLATA-ANGEL LUNA-PERPETUA-NO HAY SOLICITUD"); }
                    catch { };
                    try { _MexicoPlates.Add("767VLC", "PLATINA-DELEGACION-MOSQUETA-DENEGADO COCHERA"); }
                    catch { };
                    try { _MexicoPlates.Add("795XBW", "ECO SPORT --EMPRESA 168 DPTO 2-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("846NJP", "EXPLORER-DELEGACION-DONATELLO-DENEGADO COCHERA"); }
                    catch { };
                    try { _MexicoPlates.Add("851YRZ", "CROSS FOX--SATURNINO HERRAN (RESIDENTE)-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("866YVT", "FIESTA-DELEGACION-DONATELLO 46-3-DENEGADO COCHERA"); }
                    catch { };
                    try { _MexicoPlates.Add("895TUX", "BEETLE -DELEGACION-VALENCIA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("918NJY", "ODISSEY--ANDRES DE LA CONCHA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("932SDK", "TOWN N COUNTRY-DELEGACION-MOSQUETA-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("988PGN", "NEON-DELEGACION-DONATELLO-DENEGADO COCHERA"); }
                    catch { };
                    try { _MexicoPlates.Add("MNX7078", "HONDA ODYSSEY--MURCIA 10-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("MRF2175", "VOLKSWAGEN JETTA 2014, PLATA-AEP-DAMAS 118-NO HAY SOLICITUD"); }
                    catch { };
                    try { _MexicoPlates.Add("MUL6966", "CAPTIVA PLATA-DELEGACION-MARINO-NO HAY SOLICITUD"); }
                    catch { };
                    try { _MexicoPlates.Add("MWU2871", "TRAX GRIS-AEP-DAMAS 118-NO HAY SOLICITUD"); }
                    catch { };
                    try { _MexicoPlates.Add("PXZ7925", "JETTA-DELEGACION-DONATELLO-DENEGADO COCHERA"); }
                    catch { };
                    try { _MexicoPlates.Add("PYS6456", "-DELEGACION-MOSQUETA-NO HAY SOLICITUD"); }
                    catch { };
                    try { _MexicoPlates.Add("SMT2523", "BMW-DELEGACION-DONATELLO-DENEGADO COCHERA"); }
                    catch { };
                    try { _MexicoPlates.Add("SPN1480", "CROSS FOX AMARILLO--SANTANDER-AUTORIZADO"); }
                    catch { };
                    try { _MexicoPlates.Add("UVC5019", "JEEP-DELEGACION-DONATELLO-DENEGADO COCHERA"); }
                    catch { };
                    try { _MexicoPlates.Add("Y63AEM", "DUSTER NEGRA-DELEGACION-CJON 2 DE ABRIL-NO HAY SOLICITUD"); }
                    catch { };
                    try { _MexicoPlates.Add("Z05ABV", "SPARK AZUL-ANGEL LUNA-PERPETUA-NO HAY SOLICITUD"); }
                    catch { };

                }

                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("3rdQueryPlateLastParkingOperation: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["city_id"] == null) ||
                        (parametersIn["p"] == null) ||
                        (parametersIn["vers"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("3rdQueryPlateLastParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                    }
                    else
                    {
                        /*string strCalculatedHash = CalculateHash(strHashString, strHash);
                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("3rdQueryPlateLastParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }*
                        else
                        {*/

                            

                            decimal? dInstallationId = null;

                            try
                            {
                                decimal dTryInstallationId = Convert.ToDecimal(parametersIn["city_id"].ToString());
                                dInstallationId = dTryInstallationId;
                            }
                            catch
                            {
                                dInstallationId = null;
                            }

                            INSTALLATION oInstallation = null;
                            DateTime? dtinstDateTime = null;
                            decimal? dLatitude = null;
                            decimal? dLongitude = null;

                            if (!geograficAndTariffsRepository.getInstallation(dInstallationId,
                                                                         dLatitude,
                                                                         dLongitude,
                                                                         ref oInstallation,
                                                                         ref dtinstDateTime))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                                Logger_AddLogMessage(string.Format("3rdQueryPlateLastParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }



                            if (string.IsNullOrEmpty(parametersIn["p"].ToString()))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdQueryPlateLastParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            string strPlate = parametersIn["p"].ToString();
                            strPlate = strPlate.Trim().Replace(" ", "").Replace("-", "").ToUpper();

                           
                            parametersOut = new SortedList();
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();

                            if (dInstallationId == MEXICO_INS)
                            {
                                try
                                {

                                    parametersOut["mensaje_changada"] = _MexicoPlates[strPlate];
                                    parametersOut["mensaje_changada"] = _MexicoPlates[strPlate].Replace("&", "");                                    
                                }
                                catch
                                {
                                    if (!infraestructureRepository.ExistPlateInSystem(strPlate))
                                    {
                                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_PlateNotExist);
                                        Logger_AddLogMessage(string.Format("3rdQueryPlateLastParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                        return xmlOut;
                                    }
                                }
                            }
                            else
                            {
                                if (!infraestructureRepository.ExistPlateInSystem(strPlate))
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_PlateNotExist);
                                    Logger_AddLogMessage(string.Format("3rdQueryPlateLastParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;
                                }
                            }

                            
                            OPERATION oOperation = null;
                            if (customersRepository.GetLastOperationWithPlate(strPlate, dInstallationId.Value, out oOperation))
                            {
                                if (oOperation == null)
                                {
                                    parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_OperationNotExist).ToString();
                                }
                                else
                                {
                                    string strExtGroupId = "";
                                    string strExtTariffId = "";
                                     if  ((oOperation.GROUP!=null)&&( oOperation.TARIFF!=null)&&(!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(0, oOperation.GROUP, oOperation.TARIFF,
                                                    ref strExtGroupId, ref strExtTariffId)))
                                    {
                                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                                        Logger_AddLogMessage("3rdQueryPlateLastParkingOperation::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                                    }
                                    else
                                    {
                                        parametersOut["id"] = oOperation.OPE_ID.ToString();
                                        parametersOut["u"] = oOperation.USER.USR_ID.ToString();
                                        parametersOut["g"] = strExtGroupId;
                                        parametersOut["tar_id"] = strExtTariffId;
                                        parametersOut["d"] = string.Format("{0:yyyy-MM-ddTHH:mm:ss.fff}", oOperation.OPE_DATE);
                                        parametersOut["bd"] = string.Format("{0:yyyy-MM-ddTHH:mm:ss.fff}", oOperation.OPE_INIDATE);
                                        parametersOut["ed"] = string.Format("{0:yyyy-MM-ddTHH:mm:ss.fff}", oOperation.OPE_ENDDATE);
                                        parametersOut["q"] = oOperation.OPE_AMOUNT.ToString();
                                        parametersOut["t"] = oOperation.OPE_TIME.ToString();
                                    }

                                }
                            }
                            else
                            {
                                parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_OperationNotExist).ToString();
                            }


                           

                            xmlOut = GenerateXMLOuput(parametersOut);

                            if (xmlOut.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdQueryPlateLastParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("3rdQueryPlateLastParkingOperation: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                            }

                        //}
                    }

                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("3rdQueryPlateLastParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("3rdQueryPlateLastParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

            }

            return xmlOut;
        }

        [WebMethod]
        public string QueryPlateLastParkingOperationJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                Logger_AddLogMessage(string.Format("3rdQueryPlateLastParkingOperationJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = QueryPlateLastParkingOperation(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

                Logger_AddLogMessage(string.Format("3rdQueryPlateLastParkingOperationJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("3rdQueryPlateLastParkingOperationJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }


        [WebMethod]
        public string QueryParkingOperations(string xmlIn)
        {
            string xmlOut = "";
            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("3rdQueryParkingOperations: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["curr_id"] == null) ||
                        (parametersIn["vers"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("3rdQueryParkingOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                    }
                    else
                    {
                        /*string strCalculatedHash = CalculateHash(strHashString, strHash);
                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("3rdQueryParkingOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {*/

                            int iMaxRows=50;

                            try
                            {
                                iMaxRows = Convert.ToInt32(ConfigurationManager.AppSettings["MaxReturnRowsInQueryParkingOperations"].ToString());
                            }
                            catch
                            {
                                iMaxRows=50;
                            }

                            int? iWSSignatureType = -1;

                            try
                            {
                                int iTryWSSignatureType = Convert.ToInt32(parametersIn["sign_type"].ToString());
                                iWSSignatureType = iTryWSSignatureType;
                            }
                            catch
                            {
                                iWSSignatureType=null;
                            }

                            decimal? dCurrId = null;

                            try
                            {
                                int dTryCurrId = Convert.ToInt32(parametersIn["curr_id"].ToString());
                                dCurrId = dTryCurrId;
                            }
                            catch
                            {
                                dCurrId = null;
                            }

                            if (!dCurrId.HasValue)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdQueryParkingOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            string strCompanyName = "";
                            if (iWSSignatureType.HasValue)
                            {
                                switch ((ConfirmParkWSSignatureType)iWSSignatureType)
                                {
                                    case ConfirmParkWSSignatureType.cpst_standard:
                                        strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();
                                        break;
                                    case ConfirmParkWSSignatureType.cpst_eysa:
                                        strCompanyName = ConfigurationManager.AppSettings["EYSACompanyName"].ToString();
                                        break;
                                    case ConfirmParkWSSignatureType.cpst_gtechna:
                                        strCompanyName = ConfigurationManager.AppSettings["GtechnaCompanyName"].ToString();
                                        break;

                                }
                            }
                            

                            parametersOut = new SortedList();
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();

                            if (!customersRepository.ConfirmUnConfirmedParkingOperations(dCurrId.Value, iWSSignatureType))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdQueryParkingOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;

                            }


                            IEnumerable<OPERATION> oOperationList = null;
                            if (customersRepository.QueryUnConfirmedParkingOperations(dCurrId.Value, iWSSignatureType, iMaxRows, out oOperationList))
                            {
                                if (oOperationList == null)
                                {
                                    parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                                }
                                else
                                {
                                    
                                    StringBuilder strBuildRows = new StringBuilder();
                                    int iNumOpers = 0;

                                    foreach (OPERATION oOperation in oOperationList)
                                    {

                                        string strExtGroupId = "";
                                        string strExtTariffId = "";
                                        int iConfirmationWSNumber = 0;

                                        if ((oOperation.OPE_CONFIRMED_IN_WS1 == 0) && 
                                            ((oOperation.INSTALLATION.INS_PARK_CONFIRM_WS_SIGNATURE_TYPE == iWSSignatureType)||(!iWSSignatureType.HasValue)))
                                        {
                                            iConfirmationWSNumber = 1;
                                        }
                                        else if ((oOperation.OPE_CONFIRMED_IN_WS2 == 0) && 
                                            ((oOperation.INSTALLATION.INS_PARK_CONFIRM_WS2_SIGNATURE_TYPE == iWSSignatureType)||(!iWSSignatureType.HasValue)))
                                        {
                                            iConfirmationWSNumber = 2;
                                        }
                                        else if ((oOperation.OPE_CONFIRMED_IN_WS3 == 0) &&
                                            ((oOperation.INSTALLATION.INS_PARK_CONFIRM_WS3_SIGNATURE_TYPE == iWSSignatureType) || (!iWSSignatureType.HasValue)))
                                        {
                                            iConfirmationWSNumber = 3;
                                        }


                                        string strCityID = "";

                                        if (!iWSSignatureType.HasValue)
                                        {
                                            switch (iConfirmationWSNumber)
                                            {
                                                case 1:
                                                    {
                                                        switch ((ConfirmParkWSSignatureType)oOperation.INSTALLATION.INS_PARK_CONFIRM_WS_SIGNATURE_TYPE)
                                                        {
                                                            case ConfirmParkWSSignatureType.cpst_standard:
                                                                strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();
                                                                strCityID = oOperation.INSTALLATION.INS_STANDARD_CITY_ID;
                                                                break;
                                                            case ConfirmParkWSSignatureType.cpst_eysa:
                                                                strCompanyName = ConfigurationManager.AppSettings["EYSACompanyName"].ToString();
                                                                strCityID = oOperation.INSTALLATION.INS_EYSA_CONTRATA_ID;
                                                                break;
                                                            case ConfirmParkWSSignatureType.cpst_gtechna:
                                                                strCompanyName = ConfigurationManager.AppSettings["GtechnaCompanyName"].ToString();
                                                                strCityID = oOperation.INSTALLATION.INS_GTECHNA_CITY_CODE;
                                                                break;

                                                        }
                                                    }
                                                    break;
                                                case 2:
                                                    {
                                                        switch ((ConfirmParkWSSignatureType)oOperation.INSTALLATION.INS_PARK_CONFIRM_WS2_SIGNATURE_TYPE)
                                                        {
                                                            case ConfirmParkWSSignatureType.cpst_standard:
                                                                strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();
                                                                strCityID = oOperation.INSTALLATION.INS_STANDARD_CITY_ID;
                                                                break;
                                                            case ConfirmParkWSSignatureType.cpst_eysa:
                                                                strCompanyName = ConfigurationManager.AppSettings["EYSACompanyName"].ToString();
                                                                strCityID = oOperation.INSTALLATION.INS_EYSA_CONTRATA_ID;
                                                                break;
                                                            case ConfirmParkWSSignatureType.cpst_gtechna:
                                                                strCompanyName = ConfigurationManager.AppSettings["GtechnaCompanyName"].ToString();
                                                                strCityID = oOperation.INSTALLATION.INS_GTECHNA_CITY_CODE;
                                                                break;

                                                        }
                                                    }
                                                    break;
                                                case 3:
                                                    {
                                                        switch ((ConfirmParkWSSignatureType)oOperation.INSTALLATION.INS_PARK_CONFIRM_WS3_SIGNATURE_TYPE)
                                                        {
                                                            case ConfirmParkWSSignatureType.cpst_standard:
                                                                strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();
                                                                strCityID = oOperation.INSTALLATION.INS_STANDARD_CITY_ID;
                                                                break;
                                                            case ConfirmParkWSSignatureType.cpst_eysa:
                                                                strCompanyName = ConfigurationManager.AppSettings["EYSACompanyName"].ToString();
                                                                strCityID = oOperation.INSTALLATION.INS_EYSA_CONTRATA_ID;
                                                                break;
                                                            case ConfirmParkWSSignatureType.cpst_gtechna:
                                                                strCompanyName = ConfigurationManager.AppSettings["GtechnaCompanyName"].ToString();
                                                                strCityID = oOperation.INSTALLATION.INS_GTECHNA_CITY_CODE;
                                                                break;

                                                        }
                                                    }

                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            switch ((ConfirmParkWSSignatureType)iWSSignatureType)
                                            {
                                                case ConfirmParkWSSignatureType.cpst_standard:
                                                    strCityID = oOperation.INSTALLATION.INS_STANDARD_CITY_ID;
                                                    break;
                                                case ConfirmParkWSSignatureType.cpst_eysa:
                                                    strCityID = oOperation.INSTALLATION.INS_EYSA_CONTRATA_ID;
                                                    break;
                                                case ConfirmParkWSSignatureType.cpst_gtechna:
                                                    strCityID = oOperation.INSTALLATION.INS_GTECHNA_CITY_CODE;
                                                    break;

                                            }


                                        }


                                        if  ((oOperation.GROUP!=null)&&( oOperation.TARIFF!=null)&&
                                            (!geograficAndTariffsRepository.GetGroupAndTariffExternalTranslation(iConfirmationWSNumber, oOperation.GROUP, oOperation.TARIFF,
                                                        ref strExtGroupId, ref strExtTariffId)))
                                        {
                                            Logger_AddLogMessage("3rdQueryParkingOperations::GetGroupAndTariffExternalTranslation Error", LogLevels.logERROR);
                                        }
                                        else
                                        {


                                            switch ((ChargeOperationsType)oOperation.OPE_TYPE)
                                            {
                                                case ChargeOperationsType.ParkingOperation:
                                                case ChargeOperationsType.ExtensionOperation:
                                                    strBuildRows.Append(string.Format("<oper><id>{9}</id><ot>{10}</ot><u>{11}</u><city_id>{0}</city_id><p>{1}</p><d>{2:yyyy-MM-ddTHH:mm:ss.fff}</d><g>{3}</g><tar_id>{4}</tar_id><q>{5}</q><t>{6}</t>" +
                                                                                      "<bd>{7:yyyy-MM-ddTHH:mm:ss.fff}</bd><ed>{8:yyyy-MM-ddTHH:mm:ss.fff}</ed></oper>",
                                                                                    strCityID, oOperation.USER_PLATE.USRP_PLATE, oOperation.OPE_DATE, strExtGroupId, strExtTariffId, oOperation.OPE_AMOUNT,
                                                                                    oOperation.OPE_TIME, oOperation.OPE_INIDATE, oOperation.OPE_ENDDATE, oOperation.OPE_ID, oOperation.OPE_TYPE, oOperation.OPE_USR_ID));
                                                    iNumOpers++;
                                                    break;
                                                case ChargeOperationsType.ParkingRefund:
                                                    strBuildRows.Append(string.Format("<oper><id>{9}</id><ot>{10}</ot><u>{11}</u><city_id>{0}</city_id><p>{1}</p><d>{2:yyyy-MM-ddTHH:mm:ss.fff}</d><g>{3}</g><tar_id>{4}</tar_id><q>{5}</q><t>{6}</t>" +
                                                                                      "<bd>{7:yyyy-MM-ddTHH:mm:ss.fff}</bd><ed>{8:yyyy-MM-ddTHH:mm:ss.fff}</ed></oper>",
                                                                                    strCityID, oOperation.USER_PLATE.USRP_PLATE, oOperation.OPE_DATE, strExtGroupId, strExtTariffId, oOperation.OPE_AMOUNT,
                                                                                    oOperation.OPE_TIME, oOperation.OPE_INIDATE, oOperation.OPE_ENDDATE, oOperation.OPE_ID, oOperation.OPE_TYPE, oOperation.OPE_USR_ID));
                                                    iNumOpers++;
                                                    break;

                                            }

 
                                        }
                                    }

                                    parametersOut["num_opers"] = iNumOpers.ToString();
                                    parametersOut["opers"] = strBuildRows.ToString();
                                    parametersOut["em"] = strCompanyName;

                                }
                            }
                            else
                            {
                                parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                            }

                            xmlOut = GenerateXMLOuput(parametersOut);

                            if (xmlOut.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdQueryParkingOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("3rdQueryParkingOperations: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                            }

                        //}
                    }

                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("3rdQueryParkingOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("3rdQueryParkingOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

            }

            return xmlOut;
        }

        [WebMethod]
        public string QueryParkingOperationsJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                Logger_AddLogMessage(string.Format("3rdQueryParkingOperationsJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);
                    
                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = QueryParkingOperations(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

                Logger_AddLogMessage(string.Format("3rdQueryParkingOperationsJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("3rdQueryParkingOperationsJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }


        [WebMethod]
        public string QueryFinePayments(string xmlIn)
        {
            string xmlOut = "";
            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("3rdQueryFinePayments: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["curr_id"] == null) ||
                        (parametersIn["vers"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("3rdQueryFinePayments::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                    }
                    else
                    {
                        /*string strCalculatedHash = CalculateHash(strHashString, strHash);
                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("3rdQueryFinePayments::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {*/

                        int iMaxRows = 50;

                        try
                        {
                            iMaxRows = Convert.ToInt32(ConfigurationManager.AppSettings["MaxReturnRowsInQueryFinePayments"].ToString());
                        }
                        catch
                        {
                            iMaxRows = 50;
                        }

                        int? iWSSignatureType = -1;

                        try
                        {
                            int iTryWSSignatureType = Convert.ToInt32(parametersIn["sign_type"].ToString());
                            iWSSignatureType = iTryWSSignatureType;
                        }
                        catch
                        {
                            iWSSignatureType = null;
                        }

                        decimal? dCurrId = null;

                        try
                        {
                            int dTryCurrId = Convert.ToInt32(parametersIn["curr_id"].ToString());
                            dCurrId = dTryCurrId;
                        }
                        catch
                        {
                            dCurrId = null;
                        }

                        if (!dCurrId.HasValue)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                            Logger_AddLogMessage(string.Format("3rdQueryFinePayments::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            return xmlOut;
                        }

                        string strCompanyName = "";
                        if (iWSSignatureType.HasValue)
                        {
                            switch ((FineWSSignatureType)iWSSignatureType)
                            {
                                case FineWSSignatureType.fst_standard:
                                    strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();
                                    break;
                                case FineWSSignatureType.fst_eysa:
                                    strCompanyName = ConfigurationManager.AppSettings["EYSACompanyName"].ToString();
                                    break;
                                case FineWSSignatureType.fst_gtechna:
                                    strCompanyName = ConfigurationManager.AppSettings["GtechnaCompanyName"].ToString();
                                    break;

                            }
                        }


                        parametersOut = new SortedList();
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();

                        if (!customersRepository.ConfirmUnConfirmedFinePayments(dCurrId.Value, iWSSignatureType))
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                            Logger_AddLogMessage(string.Format("3rdQueryFinePayments::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            return xmlOut;

                        }


                        IEnumerable<TICKET_PAYMENT> oTicketPaymentList = null;
                        if (customersRepository.QueryUnConfirmedFinePayments(dCurrId.Value, iWSSignatureType, iMaxRows, out oTicketPaymentList))
                        {
                            if (oTicketPaymentList == null)
                            {
                                parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                            }
                            else
                            {

                                StringBuilder strBuildRows = new StringBuilder();
                                int iNumOpers = 0;

                                foreach (TICKET_PAYMENT oTicketPayment in oTicketPaymentList)
                                {                                  
                                    string strCityID = "";

                                    if (!iWSSignatureType.HasValue)
                                    {
                                        switch ((FineWSSignatureType)oTicketPayment.INSTALLATION.INS_FINE_WS_SIGNATURE_TYPE)
                                        {
                                            case FineWSSignatureType.fst_standard:
                                                strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();
                                                strCityID = oTicketPayment.INSTALLATION.INS_STANDARD_CITY_ID;
                                                break;
                                            case FineWSSignatureType.fst_eysa:
                                                strCompanyName = ConfigurationManager.AppSettings["EYSACompanyName"].ToString();
                                                strCityID = oTicketPayment.INSTALLATION.INS_EYSA_CONTRATA_ID;
                                                break;
                                            case FineWSSignatureType.fst_gtechna:
                                                strCompanyName = ConfigurationManager.AppSettings["GtechnaCompanyName"].ToString();
                                                strCityID = oTicketPayment.INSTALLATION.INS_GTECHNA_CITY_CODE;
                                                break;

                                        }
                                                                                               
                                    }
                                    else
                                    {
                                        switch ((FineWSSignatureType)iWSSignatureType)
                                        {
                                            case FineWSSignatureType.fst_standard:
                                                strCityID = oTicketPayment.INSTALLATION.INS_STANDARD_CITY_ID;
                                                break;
                                            case FineWSSignatureType.fst_eysa:
                                                strCityID = oTicketPayment.INSTALLATION.INS_EYSA_CONTRATA_ID;
                                                break;
                                            case FineWSSignatureType.fst_gtechna:
                                                strCityID = oTicketPayment.INSTALLATION.INS_GTECHNA_CITY_CODE;
                                                break;

                                        }
                                    }

                                    strBuildRows.Append(string.Format("<ticket><id>{4}</id><u>{5}</u><f>{0}</f><city_id>{1}</city_id><d>{2:yyyy-MM-ddTHH:mm:ss.fff}</d><q>{3}</q></ticket>",
                                                                        oTicketPayment.TIPA_TICKET_NUMBER, strCityID, oTicketPayment.TIPA_DATE, oTicketPayment.TIPA_AMOUNT, oTicketPayment.TIPA_ID, oTicketPayment.TIPA_USR_ID));
                                    iNumOpers++;
                                               

                                }

                                parametersOut["num_tickets"] = iNumOpers.ToString();
                                parametersOut["tickets"] = strBuildRows.ToString();
                                parametersOut["em"] = strCompanyName;

                            }
                        }
                        else
                        {
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                        }

                        xmlOut = GenerateXMLOuput(parametersOut);

                        if (xmlOut.Length == 0)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                            Logger_AddLogMessage(string.Format("3rdQueryFinePayments::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {
                            Logger_AddLogMessage(string.Format("3rdQueryFinePayments: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                        }

                        //}
                    }

                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("3rdQueryFinePayments::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("3rdQueryFinePayments::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

            }

            return xmlOut;
        }

        [WebMethod]
        public string QueryFinePaymentsJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                Logger_AddLogMessage(string.Format("3rdQueryFinePaymentsJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = QueryFinePayments(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

                Logger_AddLogMessage(string.Format("3rdQueryFinePaymentsJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("3rdQueryFinePaymentsJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }

        [WebMethod]
        public string QueryCoupon(string xmlIn)
        {
            string xmlOut = "";
            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("3rdQueryCoupon: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["prov_name"] == null) ||
                        (parametersIn["qr_code"] == null) ||
                        (parametersIn["d"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("3rdQueryCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                    }
                    else
                    {
                        string strCalculatedHash = CalculateHash(strHashString, strHash);

                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("3rdQueryCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {

                            DateTime dtDateUTC;                            
                            try
                            {
                                try
                                {
                                    dtDateUTC = DateTime.ParseExact(parametersIn["d"].ToString(), "HHmmssddMMyy",
                                    CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    dtDateUTC = DateTime.ParseExact(parametersIn["d"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                    CultureInfo.InvariantCulture);
                                }
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdQueryCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;

                            }

                            EXTERNAL_PROVIDER oExternalProvider = null;
                            if (!geograficAndTariffsRepository.getExternalProvider(parametersIn["prov_name"].ToString(),
                                                                                   ref oExternalProvider))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_ExternalProvider);
                                Logger_AddLogMessage(string.Format("3rdQueryCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            string sQrCode = parametersIn["qr_code"].ToString();

                            bool bKeyQrCode = false;
                            if (parametersIn.ContainsKey("keycode"))
                            {
                                try
                                {
                                    bKeyQrCode = (Convert.ToInt32(parametersIn["keycode"].ToString()) == 1);
                                }
                                catch
                                {
                                    bKeyQrCode = false;
                                }
                            }

                            parametersOut = new SortedList();
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();

                            bool bAvailable = false;
                            RECHARGE_COUPON oCoupon = null;

                            int iTimeoutSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["ExternalCouponsLock_Timeout"] ?? "0");

                            if (!customersRepository.QueryCouponAndLock(sQrCode, bKeyQrCode, dtDateUTC, oExternalProvider.EXP_ID, iTimeoutSeconds, out bAvailable, out oCoupon))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdQueryCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            rt = ResultType.Result_OK;

                            if (!bAvailable || oCoupon == null)
                            {
                                if (oCoupon == null)
                                    rt = ResultType.Result_Error_Coupon_NoExist;
                                else if (oCoupon.RCOUP_COUPS_ID == Convert.ToInt32(RechargeCouponsStatus.Used))
                                    rt = ResultType.Result_Error_Coupon_Used;
                                else
                                    rt = ResultType.Result_Error_Coupon_NotAvailable;

                                xmlOut = GenerateXMLErrorResult(rt);
                                Logger_AddLogMessage(string.Format("3rdQueryCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            parametersOut["cur_iso_code"] = oCoupon.CURRENCy.CUR_ISO_CODE;
                            parametersOut["coupval"] = oCoupon.RCOUP_VALUE;

                            xmlOut = GenerateXMLOuput(parametersOut);

                            if (xmlOut.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdQueryCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("3rdQueryCoupon: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                            }

                        }
                    }

                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("3rdQueryCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("3rdQueryCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

            }

            return xmlOut;
        }

        [WebMethod]
        public string QueryCouponJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                Logger_AddLogMessage(string.Format("3rdQueryCouponJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = QueryCoupon(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

                Logger_AddLogMessage(string.Format("3rdQueryCouponJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("3rdQueryCouponJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }

        [WebMethod]
        public string ConfirmCoupon(string xmlIn)
        {
            string xmlOut = "";
            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("3rdConfirmCoupon: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["prov_name"] == null) ||
                        (parametersIn["qr_code"] == null) ||
                        (parametersIn["type"] == null) ||
                        (parametersIn["d"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("3rdConfirmCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                    }
                    else
                    {
                        string strCalculatedHash = CalculateHash(strHashString, strHash);

                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("3rdConfirmCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {

                            DateTime dtDateUTC;
                            try
                            {
                                try
                                {
                                    dtDateUTC = DateTime.ParseExact(parametersIn["d"].ToString(), "HHmmssddMMyy",
                                    CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    dtDateUTC = DateTime.ParseExact(parametersIn["d"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                    CultureInfo.InvariantCulture);
                                }
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdConfirmCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            EXTERNAL_PROVIDER oExternalProvider = null;
                            if (!geograficAndTariffsRepository.getExternalProvider(parametersIn["prov_name"].ToString(),
                                                                                   ref oExternalProvider))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_ExternalProvider);
                                Logger_AddLogMessage(string.Format("3rdConfirmCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            string sQrCode = parametersIn["qr_code"].ToString();

                            RechargeCouponsConfirmType oConfirmType = RechargeCouponsConfirmType.Cancel;
                            try
                            {
                                oConfirmType = (RechargeCouponsConfirmType)Convert.ToInt32(parametersIn["type"].ToString());
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdConfirmCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            bool bKeyQrCode = false;
                            if (parametersIn.ContainsKey("keycode"))
                            {
                                try
                                {
                                    bKeyQrCode = (Convert.ToInt32(parametersIn["keycode"].ToString()) == 1);
                                }
                                catch
                                {
                                    bKeyQrCode = false;
                                }
                            }

                            parametersOut = new SortedList();
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();

                            RECHARGE_COUPON oCoupon = null;

                            if (!customersRepository.ConfirmCoupon(sQrCode, bKeyQrCode, dtDateUTC, oExternalProvider.EXP_ID, oConfirmType, out oCoupon))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdConfirmCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            if (oCoupon == null)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Coupon_NoExist);
                                Logger_AddLogMessage(string.Format("3rdQueryCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }
                            else
                            {
                                parametersOut["coupid"] = oCoupon.RCOUP_ID.ToString();
                            }

                            xmlOut = GenerateXMLOuput(parametersOut);

                            if (xmlOut.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdConfirmCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("3rdConfirmCoupon: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                            }

                        }
                    }

                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("3rdConfirmCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("3rdConfirmCoupon::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

            }

            return xmlOut;
        }

        [WebMethod]
        public string ConfirmCouponJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                Logger_AddLogMessage(string.Format("3rdConfirmCouponJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = ConfirmCoupon(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

                Logger_AddLogMessage(string.Format("3rdConfirmCouponJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("3rdConfirmCouponJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }




        [WebMethod]
        public string NotifyCarEntry(string xmlIn) {

            string xmlOut = "";
            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["parking_id"] == null) ||
                        (parametersIn["ope_id"] == null) ||
                        (parametersIn["ope_id_type"] == null) ||
                        (parametersIn["p"] == null) ||
                        (parametersIn["d"] == null) ||
                        //(parametersIn["gate_id"] == null) ||
                        //(parametersIn["tar_id"] == null) ||
                        (parametersIn["vers"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                    }
                    else
                    {
                        string strCalculatedHash = CalculateHash(strHashString, strHash);

                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {
                            string sExtParkingId = parametersIn["parking_id"].ToString();

                            GROUPS_OFFSTREET_WS_CONFIGURATION oParkingConfiguration = null;
                            DateTime? dtgroupDateTime = null;
                            if (!geograficAndTariffsRepository.getOffStreetConfigurationByExtOpsId(sExtParkingId, ref oParkingConfiguration, ref dtgroupDateTime))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            GROUP oGroup = null;
                            DateTime? dtinstDateTime = null;
                            if (!geograficAndTariffsRepository.getGroupByExtOpsId(sExtParkingId, ref oGroup, ref dtinstDateTime))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }
                            if (oGroup.GRP_TYPE != (int)GroupType.OffStreet)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }


                            string sOpeId = parametersIn["ope_id"].ToString();
                            OffstreetOperationIdType oOpeType = OffstreetOperationIdType.MeyparId;
                            try
                            {
                                oOpeType = (OffstreetOperationIdType)Convert.ToInt32(parametersIn["ope_id_type"].ToString());
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            string sPlate = parametersIn["p"].ToString();
                            sPlate = sPlate.Trim().Replace(" ", "").ToUpper();
                            if (sPlate.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }
                            if (!infraestructureRepository.ExistPlateInSystem(sPlate))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_PlateNotExist);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            USER oUser = null;
                            IEnumerable<USER> oUsersList = null;
                            if (!customersRepository.GetUsersWithPlate(sPlate, out oUsersList))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            if (oUsersList.Count() > 0)
                            {
                                if (oUsersList.Count() == 1)
                                {
                                    oUser = oUsersList.First();
                                }
                                else
                                {
                                    parametersOut = new SortedList();
                                    parametersOut["r"] = oUsersList.Count().ToString();
                                    xmlOut = GenerateXMLOuput(parametersOut);                                    
                                    Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;
                                }
                            }
                            else
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_PlateNotExist);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }
                            
                            DateTime dtCurrentDate;
                            try
                            {
                                dtCurrentDate = DateTime.ParseExact(parametersIn["d"].ToString(), "HHmmssddMMyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            string sTariff = (parametersIn["tar_id"] ?? "").ToString();
                            string sGate = (parametersIn["gate_id"] ?? "").ToString();

                            // Get last offstreet operation with the same group id and logical id (<g> and <ope_id>)
                            OPERATIONS_OFFSTREET oLastParkOp = null;
                            if (!customersRepository.GetLastOperationOffstreetData(oGroup.GRP_ID, sOpeId, out oLastParkOp))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            if (oLastParkOp != null && oLastParkOp.OPEOFF_TYPE != (int)OffstreetOperationType.Entry)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_OperationAlreadyClosed);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            // Check if entry operation already exists
                            bool bEntryExist = (oLastParkOp != null && oLastParkOp.OPEOFF_TYPE == (int)OffstreetOperationType.Entry);

                            parametersOut = new SortedList();
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();

                            decimal dOperationID = -1;

                            if (!bEntryExist)
                            {
                                int iCurrencyChargedQuantity = 0;                                
                                decimal? dRechargeId;
                                bool bRestoreBalanceInCaseOfRefund = true;
                                int? iBalanceAfterRecharge = null;

                                integraMobile.WS.integraCommonService oCommonService = new integraMobile.WS.integraCommonService(customersRepository, infraestructureRepository, geograficAndTariffsRepository);

                            	bool bPaymentInPerson = false;
                            	string str3DSURL = null;

                                decimal dSourceApp = geograficAndTariffsRepository.GetDefaultSourceApp();
                                long lEllapsedTime = 0;

                                integraMobile.ExternalWS.ResultType rtIntegraMobileWS =
                                    oCommonService.ChargeOffstreetOperation(OffstreetOperationType.Entry, sPlate, 1.0, 0, 0,
                                                                              dtCurrentDate, dtCurrentDate, null, null, null,
                                                                              oParkingConfiguration, oGroup, sOpeId, sTariff, sGate, "", true,
                                                                              ref oUser, (int) MobileOS.None, null, null, null, "",
                                                                              0, 0, 0, 0, 0,
                                                                              0, 0, 0, 0,
                                                                              null,"","","", "", "", "","","",
                                                                              "", "", "", "", "", "", "", 
                                                                              "", "", "", "", "", "", "", dSourceApp, bPaymentInPerson, null,
                                                                              ref parametersOut, out iCurrencyChargedQuantity, out dOperationID,
                                                                              out dRechargeId, out iBalanceAfterRecharge, out bRestoreBalanceInCaseOfRefund,out str3DSURL, out lEllapsedTime);

                                rt = Convert_integraMobileExternalWSResultType_TO_ResultType(rtIntegraMobileWS);
                                if (rt != ResultType.Result_OK)
                                {                                    
                                    xmlOut = GenerateXMLErrorResult(rt);
                                    Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;
                                }
                            }
                            else
                            {
                                rt = ResultType.Result_Error_OperationEntryAlreadyExists;
                            }

                            parametersOut["r"] = Convert.ToInt32(rt).ToString();
                            parametersOut["u"] = oUser.USR_USERNAME;
                            parametersOut["opnum"] = dOperationID.ToString();

                            xmlOut = GenerateXMLOuput(parametersOut);

                            if (xmlOut.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                            }


                        }
                    }

                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("3rdPNotifyCarEntry::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

            }

            return xmlOut;
        }

        [WebMethod]
        public string NotifyCarEntryJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntryJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = NotifyCarEntry(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

                Logger_AddLogMessage(string.Format("3rdPNotifyCarEntryJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("3rdPNotifyCarEntryJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }

        [WebMethod]
        public string TryCarPayment(string xmlIn)
        {
            string xmlOut = "";
            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("3rdPTryCarPayment: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["parking_id"] == null) ||
                        (parametersIn["ope_id"] == null) ||
                        (parametersIn["ope_id_type"] == null) ||
                        (parametersIn["p"] == null) ||
                        (parametersIn["q"] == null) ||
                        (parametersIn["cur"] == null) ||
                        (parametersIn["t"] == null) ||
                        (parametersIn["bd"] == null) ||
                        (parametersIn["ed"] == null) ||
                        (parametersIn["med"] == null) ||
                        //(parametersIn["gate_id"] == null) ||
                        //(parametersIn["tar_id"] == null) ||
                        (parametersIn["vers"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                    }
                    else
                    {
                        string strCalculatedHash = CalculateHash(strHashString, strHash);

                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {
                            string sExtParkingId = parametersIn["parking_id"].ToString();

                            GROUPS_OFFSTREET_WS_CONFIGURATION oParkingConfiguration = null;
                            DateTime? dtgroupDateTime = null;
                            if (!geograficAndTariffsRepository.getOffStreetConfigurationByExtOpsId(sExtParkingId, ref oParkingConfiguration, ref dtgroupDateTime))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            GROUP oGroup = null;
                            DateTime? dtinstDateTime = null;
                            if (!geograficAndTariffsRepository.getGroupByExtOpsId(sExtParkingId,
                                                                                    ref oGroup,
                                                                                    ref dtinstDateTime))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }
                            if (oGroup.GRP_TYPE != (int)GroupType.OffStreet)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            string sOpeId = parametersIn["ope_id"].ToString();
                            OffstreetOperationIdType oOpeIdType = OffstreetOperationIdType.MeyparId;
                            try
                            {
                                oOpeIdType = (OffstreetOperationIdType)Convert.ToInt32(parametersIn["ope_id_type"].ToString());
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            string sPlate = parametersIn["p"].ToString();
                            sPlate = sPlate.Trim().Replace(" ", "").ToUpper();
                            if (sPlate.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }
                            if (!infraestructureRepository.ExistPlateInSystem(sPlate))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_PlateNotExist);
                                Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            USER oUser = null;
                            IEnumerable<USER> oUsersList = null;
                            if (!customersRepository.GetUsersWithPlate(sPlate, out oUsersList))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            if (oUsersList.Count() > 0)
                            {
                                if (oUsersList.Count() == 1)
                                {
                                    oUser = oUsersList.First();
                                }
                                else
                                {
                                    parametersOut = new SortedList();
                                    parametersOut["r"] = oUsersList.Count().ToString();
                                    xmlOut = GenerateXMLOuput(parametersOut);
                                    Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;
                                }
                            }
                            else
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_PlateNotExist);
                                Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            int iAmount = 0;
                            int iTime = 0;
                            string sCurIsoCode = (parametersIn["cur"] ?? "").ToString();
                            try
                            {
                                iAmount = Convert.ToInt32(parametersIn["q"].ToString());
                                iTime = Convert.ToInt32(parametersIn["t"].ToString());
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            DateTime dtEntryDate;
                            DateTime dtEndDate;
                            DateTime dtExitLimitDate;
                            try
                            {
                                dtEntryDate = DateTime.ParseExact(parametersIn["bd"].ToString(), "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                dtEndDate = DateTime.ParseExact(parametersIn["ed"].ToString(), "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                dtExitLimitDate = DateTime.ParseExact(parametersIn["med"].ToString(), "HHmmssddMMyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            string sTariff = (parametersIn["tar_id"] ?? "").ToString();
                            string sGate = (parametersIn["gate_id"] ?? "").ToString();

                            int iOp = Convert.ToInt32((parametersIn["op"] ?? "0").ToString());
                            OffstreetOperationType operationType = (iOp == 0 ? OffstreetOperationType.Exit : OffstreetOperationType.OverduePayment);

                            parametersOut = new SortedList();
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();

                            integraMobile.WS.integraCommonService oCommonService = CommonService();

                            double dChangeToApply = 1.0;

                            if (sCurIsoCode.ToUpper() != oParkingConfiguration.GROUP.INSTALLATION.CURRENCy.CUR_ISO_CODE)
                            {
                                // ...
                            }

                            if (oParkingConfiguration.GROUP.INSTALLATION.CURRENCy.CUR_ISO_CODE != infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)))
                            {
                                dChangeToApply = oCommonService.GetChangeToApplyFromInstallationCurToUserCur(oParkingConfiguration.GROUP.INSTALLATION, oUser);
                                if (dChangeToApply < 0)
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                    Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;
                                }

                                /*NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                                numberFormatProvider.NumberDecimalSeparator = ".";
                                parametersOut["chng"] = dChangeToApply.ToString(numberFormatProvider);

                                double dChangeFee = 0;
                                int iQChange = oIntegraMobileWS.ChangeQuantityFromInstallationCurToUserCur(Convert.ToInt32(parametersOut["q1"]),
                                                                                                           dChangeToApply, oParkingConfiguration.GROUP.INSTALLATION, oUser, out dChangeFee);

                                parametersOut["qch"] = iQChange.ToString();*/
                            }

                            decimal dPercVAT1;
                            decimal dPercVAT2;
                            decimal dPercFEE;
                            decimal dPercFEETopped;
                            decimal dFixedFEE;
                            int iPartialVAT1;
                            int iPartialPercFEE;
                            int iPartialFixedFEE;
                            int iPartialPercFEEVAT;
                            int iPartialFixedFEEVAT;
                            int iTotalQuantity;
                            string sDiscounts = "";

                            int? iPaymentTypeId = null;
                            int? iPaymentSubtypeId = null;
                            if (oUser.CUSTOMER_PAYMENT_MEAN != null)
                            {
                                iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                                iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
                            }
                            if (!customersRepository.GetFinantialParams(oUser, oGroup, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, iPaymentTypeId, iPaymentSubtypeId,
                                                                        out dPercVAT1, out dPercVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error getting installation FEE parameters: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            iTotalQuantity = customersRepository.CalculateFEE(iAmount, dPercVAT1, dPercVAT2, dPercFEE, dPercFEETopped, dFixedFEE,
                                                                                                          out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE,
                                                                                                          out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                            int iPercFEETopped = Convert.ToInt32(Math.Round(dPercFEETopped, MidpointRounding.AwayFromZero));
                            int iFixedFEE = Convert.ToInt32(Math.Round(dFixedFEE, MidpointRounding.AwayFromZero));

                            bool bPaymentInPerson = false;
                            string str3DSURL = null;

                            decimal dSourceApp = geograficAndTariffsRepository.GetDefaultSourceApp();

                            integraMobile.ExternalWS.ResultType rtIntegraMobileWS =
                                oCommonService.ConfirmCarPayment(oParkingConfiguration, oGroup, oUser,
                                                                  sOpeId, oOpeIdType, sPlate, sTariff, sGate,
                                                                  operationType, iAmount, iTime, dChangeToApply, sCurIsoCode,
                                                                  dtEntryDate, dtgroupDateTime.Value, dtEndDate, dtExitLimitDate,
                                                                  (int)MobileOS.None, null, null, null, "",
                                                                  dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE,
                                                                  iPartialVAT1, iPartialPercFEE, iPartialFixedFEE, iTotalQuantity,
                                                                  sDiscounts,"","","", "", "", "", "", "",
                                                                  "", "", "", "", "", "", "", "", "", "", "", "", "", "", dSourceApp, bPaymentInPerson,
                                                                  null, null,
                                                                  ref parametersOut, out str3DSURL);

                            rt = Convert_integraMobileExternalWSResultType_TO_ResultType(rtIntegraMobileWS);

                            if (rt != ResultType.Result_OK)
                            {
                                xmlOut = GenerateXMLErrorResult(rt);
                                Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }
                            
                            xmlOut = GenerateXMLOuput(parametersOut);

                            if (xmlOut.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("3rdPTryCarPayment: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                            }
                        }
                    }
                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("3rdPTryCarPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

            }

            return xmlOut;
        }

        [WebMethod]
        public string TryCarPaymentJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                Logger_AddLogMessage(string.Format("3rdPTryCarPaymentJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = TryCarPayment(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

                Logger_AddLogMessage(string.Format("3rdPTryCarPaymentJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("3rdPTryCarPaymentJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }

        [WebMethod]
        public string NotifyCarExit(string xmlIn)
        {

            string xmlOut = "";
            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["parking_id"] == null) ||
                        (parametersIn["ope_id"] == null) ||
                        (parametersIn["ope_id_type"] == null) ||
                        //(parametersIn["p"] == null) ||
                        (parametersIn["exd"] == null) ||
                        (parametersIn["vers"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                    }
                    else
                    {
                        string strCalculatedHash = CalculateHash(strHashString, strHash);

                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {
                            string sExtParkingId = parametersIn["parking_id"].ToString();

                            GROUPS_OFFSTREET_WS_CONFIGURATION oParkingConfiguration = null;
                            DateTime? dtgroupDateTime = null;
                            if (!geograficAndTariffsRepository.getOffStreetConfigurationByExtOpsId(sExtParkingId, ref oParkingConfiguration, ref dtgroupDateTime))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            GROUP oGroup = null;
                            DateTime? dtinstDateTime = null;
                            if (!geograficAndTariffsRepository.getGroupByExtOpsId(sExtParkingId,
                                                                                    ref oGroup,
                                                                                    ref dtinstDateTime))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }
                            if (oGroup.GRP_TYPE != (int)GroupType.OffStreet)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }


                            string sOpeId = parametersIn["ope_id"].ToString();
                            OffstreetOperationIdType oOpeIdType = OffstreetOperationIdType.MeyparId;
                            try
                            {
                                oOpeIdType = (OffstreetOperationIdType)Convert.ToInt32(parametersIn["ope_id_type"].ToString());
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            string sPlate = (parametersIn["p"] ?? "").ToString();
                            sPlate = sPlate.Trim().Replace(" ", "").ToUpper();
                            /*if (sPlate.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }
                            if (!infraestructureRepository.ExistPlateInSystem(sPlate))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_PlateNotExist);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            USER oUser = null;
                            IEnumerable<USER> oUsersList = null;
                            if (!customersRepository.GetUsersWithPlate(sPlate, out oUsersList))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            if (oUsersList.Count() > 0)
                            {
                                if (oUsersList.Count() == 1)
                                {
                                    oUser = oUsersList.First();
                                }
                                else
                                {
                                    parametersOut = new SortedList();
                                    parametersOut["r"] = oUsersList.Count().ToString();
                                    xmlOut = GenerateXMLOuput(parametersOut);
                                    Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;
                                }
                            }
                            else
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_PlateNotExist);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }*/

                            DateTime dtExitDate;
                            try
                            {
                                dtExitDate = DateTime.ParseExact(parametersIn["exd"].ToString(), "HHmmssddMMyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }
                            
                            string sGate = (parametersIn["gate_id"] ?? "").ToString();

                            // Get last offstreet operation with the same group id and logical id (<g> and <ope_id>)
                            OPERATIONS_OFFSTREET oLastParkOp = null;
                            if (!customersRepository.GetLastOperationOffstreetData(oGroup.GRP_ID, sOpeId, out oLastParkOp))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            if (oLastParkOp == null)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_OperationExitNotExist);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            if (oLastParkOp != null && oLastParkOp.OPEOFF_TYPE != (int)OffstreetOperationType.Exit && oLastParkOp.OPEOFF_TYPE != (int)OffstreetOperationType.OverduePayment)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_OperationExitNotExist);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            if (oLastParkOp.OPEOFF_EXIT_DATE.HasValue)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_OperationAlreadyClosed);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            if (sPlate.Length > 0 && oLastParkOp.USER_PLATE.USRP_PLATE != sPlate)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_OperationExitNotExist);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            // Update offstreet operation exit date and check must notify
                            DateTime? dtUTCExitDate = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oGroup.GRP_INS_ID, dtExitDate);
                            if (!customersRepository.UpdateOperationOffstreetExitData(oLastParkOp.OPEOFF_ID, dtExitDate, dtUTCExitDate.Value, true, out oLastParkOp))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            parametersOut = new SortedList();
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();
                            parametersOut["ope_id"] = sOpeId;
                            parametersOut["opnum"] = oLastParkOp.OPEOFF_ID.ToString();

                            xmlOut = GenerateXMLOuput(parametersOut);

                            if (xmlOut.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("3rdPNotifyCarExit: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                            }


                        }
                    }

                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("3rdPNotifyCarExit::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

            }

            return xmlOut;
        }

        [WebMethod]
        public string NotifyCarExitJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                Logger_AddLogMessage(string.Format("3rdPNotifyCarExitJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = NotifyCarExit(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

                Logger_AddLogMessage(string.Format("3rdPNotifyCarExitJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("3rdPNotifyCarExitJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }

        [WebMethod]
        public bool SendPushWPNotificationsTo(string strUser,
                                              string strToastText1,
                                              string strToastText2,
                                              string strToastParam,
                                              string strTileTitle,
                                              int iTileCount,
                                              string strBackgroundImage)            
        {
            bool bRes=true;
            try
            {

                Logger_AddLogMessage(string.Format("SendPushWPNotificationsTo:\r\n\tstrUser={0},\r\n\tstrToastText1= {1},\r\n\tstrToastText2= {2},\r\n\t" +
                                                "strToastParam= {3},\r\n\tstrTileTitle= {4},\r\n\tiTileCount= {5},\r\n\tstrBackgroundImage= {6}\r\n\r\n",
                                                strUser,
                                                strToastText1,
                                                strToastText2,
                                                strToastParam,
                                                strTileTitle,
                                                iTileCount,
                                                strBackgroundImage), LogLevels.logINFO);


                USER oUser = null;

                if (customersRepository.GetUserData(ref oUser, strUser))
                {
                    if (!customersRepository.AddWPPushIDNotification(ref oUser,
                                                                    strToastText1,
                                                                    strToastText2,
                                                                    strToastParam,
                                                                    strTileTitle,
                                                                    iTileCount,
                                                                    strBackgroundImage))
                    {
                        Logger_AddLogMessage(string.Format("SendPushWPNotificationsTo Error Inserting notification:\r\n\tstrUser= {0},\r\n\tstrToastText1= {1},\r\n\tstrToastText2= {2},\r\n\t" +
                                                        "strToastParam= {3},\r\n\tstrTileTitle= {4},\r\n\tiTileCount= {5},\r\n\tstrBackgroundImage= {6}\r\n\r\n",
                                                        strUser,
                                                        strToastText1,
                                                        strToastText2,
                                                        strToastParam,
                                                        strTileTitle,
                                                        iTileCount,
                                                        strBackgroundImage), LogLevels.logERROR);
                        bRes = false;

                    }
                    else
                    {
                        Logger_AddLogMessage(string.Format("SendPushWPNotificationsTo. Notification inserted OK:\r\n\tstrUser= {0},\r\n\tstrToastText1= {1},\r\n\tstrToastText2= {2},\r\n\t" +
                                                        "strToastParam={3},\r\n\tstrTileTitle= {4},\r\n\tiTileCount= {5},\r\n\tstrBackgroundImage= {6}\r\n\r\n",
                                                        strUser,
                                                        strToastText1,
                                                        strToastText2,
                                                        strToastParam,
                                                        strTileTitle,
                                                        iTileCount,
                                                        strBackgroundImage), LogLevels.logINFO);


                    }


                }
                else
                {
                    Logger_AddLogMessage(string.Format("SendPushWPNotificationsTo User Not found: strUser= {0}",
                                                    strUser), LogLevels.logERROR);
                    bRes = false;
                }


            }
            catch (Exception e)
            {
                bRes = false;
                Logger_AddLogException(e, string.Format("SendPushWPNotificationsTo Error:\r\n\tstrUser= {0},\r\n\tstrToastText1= {1},\r\n\tstrToastText2= {2},\r\n\t" +
                                                "strToastParam= {3},\r\n\tstrTileTitle= {4},\r\n\tiTileCount= {5},\r\n\tstrBackgroundImage= {6}\r\n\r\n",
                                                strUser,
                                                strToastText1,
                                                strToastText2,
                                                strToastParam,
                                                strTileTitle,
                                                iTileCount,
                                                strBackgroundImage), LogLevels.logERROR);

            }

            return bRes;

        }


        [WebMethod]
        public string NotifyTollPayment(string xmlIn)
        {
            string xmlOut = "";
            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("3rdNotifyTollPayment: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["ope_succeed"] == null) ||
                        (parametersIn["opeqr"] == null) ||
                        (parametersIn["tolldate"] == null) ||
                        (parametersIn["tollid"] == null) ||
                        (parametersIn["tolliddesc"] == null) ||
                        (parametersIn["tollrailid"] == null) ||
                        (parametersIn["tollrailiddesc"] == null) ||
                        //(parametersIn["tolltarif"] == null) ||
                        (parametersIn["tolltardesc"] == null) ||
                        //(parametersIn["tollq"] == null) ||
                        //(parametersIn["tollqvat"] == null) ||
                        (parametersIn["tollqtotal"] == null) ||
                        (parametersIn["tollcur"] == null) ||
                        (parametersIn["vers"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("3rdNotifyTollPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                    }
                    else
                    {
                        string strCalculatedHash = CalculateHash(strHashString, strHash);

                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("3rdNotifyTollPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {

                            bool bSucceed = false;
                            string sExternalId = null;

                            decimal dMovementID = -1;
                            string sQr;
                            INSTALLATION oInstallation = null;
                            string sPlate = "";
                            DateTime dtOpeDate;
                            USER oUser = null;
                            int iBalance = 0;
                            decimal dBalanceCurrencyId = 0;
                            DateTime dtBalanceDate;
                            int iBalanceAverage = 0;
                            TollPaymentMode oPaymentMode = TollPaymentMode.Online;
                            bool bPaymentStatus = false;
                            int iPaymentBalDue = 0;
                            string sIMEI = "";
                            string sWIFIMAC = "";
                            int iOSID = (int)MobileOS.Web;
                            string sTollTariff = null;
                            TOLL oToll = null;
                            string sTollDescription = "";
                            int iTollAmount = 0;
                            int iTollVAT = 0;
                            int iTollTotalAmount = 0;

                            try
                            {
                                bSucceed = (parametersIn["ope_succeed"].ToString() == "1");

                                if (parametersIn["opeid"] != null)
                                    sExternalId = parametersIn["opeid"].ToString();

                                sQr = parametersIn["opeqr"].ToString();
                                integraMobile.Infrastructure.QrDecoder.QrTollData oQrData = null;
                                if (!integraMobile.Infrastructure.QrDecoder.QrDecoderUtil.QRDecode(sQr, out oQrData))
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                    Logger_AddLogMessage(string.Format("3rdNotifyTollPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;
                                }


                                DateTime? dtInstallationDate = null;
                                if (!geograficAndTariffsRepository.getInstallation(oQrData.InsId, null, null, ref oInstallation, ref dtInstallationDate))
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                    Logger_AddLogMessage(string.Format("3rdNotifyTollPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;
                                }

                                sPlate = NormalizePlate(oQrData.Plate);
                                if (sPlate.Length == 0)
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                    Logger_AddLogMessage(string.Format("3rdNotifyTollPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;
                                }

                                try
                                {
                                    dtOpeDate = DateTime.ParseExact(parametersIn["tolldate"].ToString(), "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                }
                                catch
                                {
                                    dtOpeDate = DateTime.ParseExact(parametersIn["tolldate"].ToString(), "HHmmssfffddMMyy", CultureInfo.InvariantCulture);
                                }

                                if (!customersRepository.GetUserDataById(ref oUser, oQrData.UsrId))
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                    Logger_AddLogMessage(string.Format("3rdNotifyTollPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;
                                }
                                iBalance = oQrData.Balance; // Convert.ToInt32(parametersIn["baluser"].ToString());
                                dBalanceCurrencyId = oQrData.BalanceCudId; // Convert.ToDecimal(parametersIn["balcur"].ToString());
                                string sCurIsoCode = infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(dBalanceCurrencyId));
                                if (string.IsNullOrEmpty(sCurIsoCode))
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                    Logger_AddLogMessage(string.Format("3rdNotifyTollPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;
                                }
                                dtBalanceDate = oQrData.BalanceDate; // DateTime.ParseExact(parametersIn["baldate"].ToString(), "HHmmssddMMyy", CultureInfo.InvariantCulture);
                                iBalanceAverage = oQrData.BalanceAvg; // Convert.ToInt32(parametersIn["balaver"].ToString());
                                oPaymentMode = (TollPaymentMode)oQrData.TollPaymentMode; // Convert.ToInt32(parametersIn["paymode"].ToString());
                                //int iPaymentStatus = Convert.ToInt32(parametersIn["paystatus"].ToString());
                                bPaymentStatus = oQrData.PaymentStatus; // (iPaymentStatus == 1);
                                iPaymentBalDue = oQrData.PaymentBalDue; // Convert.ToInt32(parametersIn["paybaldue"].ToString());
                                sIMEI = oQrData.IMEI; // parametersIn["devimei"].ToString();
                                sWIFIMAC = oQrData.WIFIMAC; // parametersIn["devmac"].ToString();
                                iOSID = oQrData.OSID; // Convert.ToInt32(parametersIn["OSID"].ToString());
                                if (parametersIn["tolltarif"] != null)
                                    sTollTariff = parametersIn["tolltarif"].ToString();

                                decimal dTollId = Convert.ToDecimal(parametersIn["tollid"].ToString());
                                oToll = customersRepository.GetToll(dTollId);
                                if (oToll == null)
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                    Logger_AddLogMessage(string.Format("3rdNotifyTollPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;
                                }
                                else if (oToll.TOL_INS_ID != oInstallation.INS_ID)
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Toll_is_Not_from_That_installation);
                                    Logger_AddLogMessage(string.Format("3rdNotifyTollPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;
                                }

                                sTollDescription = parametersIn["tolliddesc"].ToString();

                                //iTollAmount = Convert.ToInt32(parametersIn["tollq"].ToString());
                                //dTollVAT = Convert.ToDecimal(parametersIn["tollqvat"].ToString());
                                iTollTotalAmount = Convert.ToInt32(parametersIn["tollqtotal"].ToString());

                                dMovementID = oQrData.BlockingId;

                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("3rdNotifyTollPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            /*if (!customersRepository.IsPlateOfUser(ref oUser, sPlate))
                            {
                                if (!customersRepository.AddPlateToUser(ref oUser, sPlate))
                                {                                    
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                    Logger_AddLogMessage(string.Format("TryTollPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;
                                }
                            }*/

                            ResultType rtRes = ResultType.Result_OK;


                            TOLL_MOVEMENT oTollMovement = null;

                            var oMovements = customersRepository.GetTollMovementsByQr(sQr);

                            oTollMovement = oMovements.Where(t => t.TOLM_TYPE == (int)ChargeOperationsType.TollPayment).FirstOrDefault();
                            if (oTollMovement != null)
                            {
                                dMovementID = oTollMovement.TOLM_ID;
                                rtRes = ResultType.Result_Error_OperationEntryAlreadyExists;
                                Logger_AddLogMessage(string.Format("3rdNotifyTollPayment::Toll movement already paid (id:{0})", oTollMovement.TOLM_ID), LogLevels.logWARN);
                            }
                            else if (!oMovements.Where(t => t.TOLM_TYPE == (int)ChargeOperationsType.TollUnlock).Any())
                            {
                                oTollMovement = oMovements.Where(t => t.TOLM_TYPE == (int)ChargeOperationsType.TollLock)
                                                          .OrderByDescending(t => t.TOLM_DATE)
                                                          .FirstOrDefault();
                                if (oTollMovement == null)
                                    customersRepository.GetTollMovementById(dMovementID, out oTollMovement);

                            }

                            parametersOut = new SortedList();

                            if (rtRes == ResultType.Result_OK)
                            {

                                if (oTollMovement != null)
                                {
                                    dMovementID = oTollMovement.TOLM_ID;
                                    // Check toll movement status
                                    rtRes = ((ChargeOperationsType)oTollMovement.TOLM_TYPE) == ChargeOperationsType.TollLock ? ResultType.Result_OK : ResultType.Result_Error_OperationEntryAlreadyExists;
                                    if (rtRes != ResultType.Result_OK)
                                        Logger_AddLogMessage(string.Format("3rdNotifyTollPayment::Toll movement already paid (id:{0})", dMovementID), LogLevels.logWARN);
                                }

                                if (rtRes == ResultType.Result_OK)
                                {

                                    double dChangeToApply = 1.0;
                                    //DateTime dtSavedInstallationTime = DateTime.UtcNow;                            
                                    decimal dVAT1;
                                    decimal dVAT2;
                                    decimal dPercFEE;
                                    decimal dPercFEETopped;
                                    decimal dFixedFEE;
                                    int iPartialVAT1;
                                    int iPartialPercFEE;
                                    int iPartialFixedFEE;
                                    int iPartialPercFEEVAT;
                                    int iPartialFixedFEEVAT;
                                    int iTotalQuantity;
                                    decimal dPercBonus = 0;
                                    int iPartialBonusFEE;
                                    int iPartialBonusFEEVAT;

                                    int? iPaymentTypeId = null;
                                    int? iPaymentSubtypeId = null;
                                    if (oUser.CUSTOMER_PAYMENT_MEAN != null)
                                    {
                                        iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                                        iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
                                    }
                                    if (!customersRepository.GetFinantialParams(oUser, oToll, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, iPaymentTypeId, iPaymentSubtypeId,
                                                                                out dVAT1, out dVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE))
                                    {
                                        rtRes = ResultType.Result_Error_Generic;
                                        Logger_AddLogMessage("3rdNotifyTollPayment::Error getting finantial parameters", LogLevels.logERROR);
                                    }

                                    iTollVAT = Convert.ToInt32(Math.Round((dVAT1 * (iTollTotalAmount)) / (1 + dVAT1), MidpointRounding.AwayFromZero));
                                    iTollAmount = iTollTotalAmount - iTollVAT;

                                    if (rtRes == ResultType.Result_OK)
                                    {

                                        int iCurrencyChargedQuantity = 0;
                                        decimal? dRechargeId;
                                        bool bRestoreBalanceInCaseOfRefund = true;
                                        int? iBalanceAfterRecharge = null;
                                        DateTime? dtUTCInsertionDate = null;


                                        iTotalQuantity = customersRepository.CalculateFEE(iTollAmount, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE, dPercBonus,
                                                                                          out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE,
                                                                                          out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);

                                        int iQT = (iPartialPercFEE - iPartialPercFEEVAT) + (iPartialFixedFEE - iPartialFixedFEEVAT);
                                        int iQC = iPartialBonusFEE - iPartialBonusFEEVAT;
                                        int iIVA = iPartialPercFEEVAT + iPartialFixedFEEVAT - iPartialBonusFEEVAT;

                                        integraMobile.WS.integraCommonService oCommonService = CommonService();

                                        integraMobile.ExternalWS.ResultType rtIntegraMobileWS = integraMobile.ExternalWS.ResultType.Result_OK;

                                        DateTime? dtOpeDateUTC = DateTime.UtcNow;

                                        if (oTollMovement != null && oTollMovement.TOLM_TYPE == (int)ChargeOperationsType.TollLock)
                                        {
                                            TOLL_MOVEMENT oTollLockMovement = null;
                                            if (!customersRepository.GetTollMovementByLockId(oTollMovement.TOLM_ID, out oTollLockMovement))
                                            {
                                                string sPlateUnlock = null;
                                                if (oTollMovement.USER_PLATE != null) sPlateUnlock = oTollMovement.USER_PLATE.USRP_PLATE;

                                                decimal dSourceApp = geograficAndTariffsRepository.GetDefaultSourceApp();

                                                rtIntegraMobileWS = oCommonService.ChargeTollMovement(sPlateUnlock, Convert.ToDouble(oTollMovement.TOLM_CHANGE_APPLIED), oTollMovement.TOLM_AMOUNT, dtOpeDate, oTollMovement.TOLM_TOL_TARIFF,
                                                                                                        oInstallation, oTollMovement.TOLL, ref oUser, iOSID,
                                                                                                        oTollMovement.TOLM_PERC_VAT1 ?? 0, oTollMovement.TOLM_PERC_VAT2 ?? 0, oTollMovement.TOLM_PERC_FEE ?? 0, Convert.ToInt32(oTollMovement.TOLM_PERC_FEE_TOPPED ?? 0), Convert.ToInt32(oTollMovement.TOLM_FIXED_FEE ?? 0),
                                                                                                        Convert.ToInt32(oTollMovement.TOLM_PARTIAL_VAT1 ?? 0), Convert.ToInt32(oTollMovement.TOLM_PARTIAL_PERC_FEE ?? 0), Convert.ToInt32(oTollMovement.TOLM_PARTIAL_FIXED_FEE ?? 0), oTollMovement.TOLM_TOTAL_AMOUNT ?? 0,
                                                                                                        oTollMovement.TOLM_EXTERNAL_ID, false, ChargeOperationsType.TollUnlock, sQr,
                                                                                                        oTollMovement.TOLM_ID, dSourceApp,
                                                                                                        ref parametersOut,
                                                                                                        out iCurrencyChargedQuantity, out dMovementID, out dtUTCInsertionDate, out dRechargeId, out iBalanceAfterRecharge,
                                                                                                        out bRestoreBalanceInCaseOfRefund, out dtOpeDateUTC);

                                                /*rtIntegraMobileWS = oCommonService.ModifyTollMovement(dMovementID, dChangeToApply, iTollAmount, sTollTariff,
                                                                            oToll, ref oUser, iOSID,
                                                                            dVAT1, dVAT2, dPercFEE, iPercFEETopped, iFixedFEE,
                                                                            iPartialVAT1, iPartialPercFEE, iPartialFixedFEE, iTotalQuantity, oTollMovement.TOLM_TOTAL_AMOUNT ?? 0,
                                                                            sExternalId, false, TollMovementStatus.Confirmed, sQr,
                                                                            out dRechargeId, out iBalanceAfterRecharge,
                                                                            out bRestoreBalanceInCaseOfRefund);*/
                                            }
                                        }

                                        if (rtIntegraMobileWS == integraMobile.ExternalWS.ResultType.Result_OK)
                                        {
                                            int iPercFEETopped = Convert.ToInt32(Math.Round(dPercFEETopped, MidpointRounding.AwayFromZero));
                                            int iFixedFEE = Convert.ToInt32(Math.Round(dFixedFEE, MidpointRounding.AwayFromZero));

                                            decimal dSourceApp = geograficAndTariffsRepository.GetDefaultSourceApp();

                                            rtIntegraMobileWS = oCommonService.ChargeTollMovement(sPlate, dChangeToApply, iTollAmount, dtOpeDate, sTollTariff,
                                                                                                    oInstallation, oToll, ref oUser, iOSID,
                                                                                                    dVAT1, dVAT2, dPercFEE, iPercFEETopped, iFixedFEE,
                                                                                                    iPartialVAT1, iPartialPercFEE, iPartialFixedFEE, iTotalQuantity,
                                                                                                    sExternalId, false, ChargeOperationsType.TollPayment, sQr, null, dSourceApp, 
                                                                                                    ref parametersOut,
                                                                                                    out iCurrencyChargedQuantity, out dMovementID, out dtUTCInsertionDate, out dRechargeId, out iBalanceAfterRecharge,
                                                                                                    out bRestoreBalanceInCaseOfRefund, out dtOpeDateUTC);
                                        }

                                        rt = Convert_integraMobileExternalWSResultType_TO_ResultType(rtIntegraMobileWS);

                                        if (rt != ResultType.Result_OK)
                                        {
                                            xmlOut = GenerateXMLErrorResult(rt);
                                            Logger_AddLogMessage(string.Format("3rdNotifyTollPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                            return xmlOut;
                                        }

                                    }

                                }

                            }

                            parametersOut["r"] = Convert.ToInt32(rtRes).ToString();
                            parametersOut["opeid"] = dMovementID;
                            xmlOut = GenerateXMLOuput(parametersOut);

                            if (xmlOut.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("3rdNotifyTollPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("3rdNotifyTollPayment: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                            }

                        }

                    }
                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("3rdNotifyTollPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("3rdNotifyTollPayment::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

            }

            return xmlOut;
        }

        [WebMethod]
        public string NotifyTollPaymentJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                Logger_AddLogMessage(string.Format("3rdNotifyTollPaymentJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = NotifyTollPayment(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

                Logger_AddLogMessage(string.Format("3rdNotifyTollPaymentJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("3rdNotifyTollPaymentJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }

        [WebMethod]
        public string CalculateOperationAmounts(string xmlIn)
        {
            string xmlOut = "";
            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("CalculateOperationAmounts: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["city_id"] == null) ||
                        (parametersIn["q"] == null) ||
                        (parametersIn["d"] == null) ||
                        (parametersIn["o"] == null) ||
                        (parametersIn["pm"] == null) ||
                        (parametersIn["lang"]== null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("CalculateOperationAmounts::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                    }
                    else
                    {
                        parametersOut = CalculateOperationAmountsPrivate(xmlIn, strHashString, strHash, Convert.ToString(parametersIn["city_id"]), Convert.ToString(parametersIn["d"]), Convert.ToString(parametersIn["q"]),
                                                         Convert.ToString(parametersIn["o"]), Convert.ToString(parametersIn["pm"]), Convert.ToString(parametersIn["lang"]),rt, ref xmlOut);
                        xmlOut = GenerateXMLOuput(parametersOut);
                    }
                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("CalculateOperationAmounts::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                }
            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("CalculateOperationAmounts::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

            }

            return xmlOut;
        }

        [WebMethod]
        public string CalculateOperationAmountsJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                Logger_AddLogMessage(string.Format("CalculateOperationAmountsJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = CalculateOperationAmounts(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

                Logger_AddLogMessage(string.Format("CalculateOperationAmountsJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("CalculateOperationAmountsJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }

        [WebMethod]
        public string ConfirmFinePaymentNonUser(string xmlIn)
        {
            string xmlOut = "";
            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("ConfirmFinePaymentNonUser: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {
                    if ((parametersIn["city_id"] == null) ||
                        (parametersIn["ticketnumber"] == null) ||
                        (parametersIn["q"] == null) ||
                        (parametersIn["d"] == null) ||
                        (parametersIn["o"] == null) ||
                        (parametersIn["pm"] == null) ||
                        (parametersIn["lic_pla"] == null) ||
                        (parametersIn["grp_id"] == null) ||
                        (parametersIn["backoffice_usr_id"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("ConfirmFinePaymentNonUser::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        return xmlOut;
                    }
                    else
                    {
                        string strInstallationId = parametersIn["city_id"].ToString();
                        string TicketNumber = Convert.ToString(parametersIn["ticketnumber"]);


                        if (String.IsNullOrEmpty(TicketNumber) || String.IsNullOrEmpty(strInstallationId))
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                            Logger_AddLogMessage(string.Format("ConfirmFinePaymentNonUser: Null Installation or TicketNumber", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            return xmlOut;
                        }
                        TicketNumber = TicketNumber.Trim();
                        parametersOut = CalculateOperationAmountsPrivate(xmlIn, strHashString, strHash, strInstallationId, Convert.ToString(parametersIn["d"]), Convert.ToString(parametersIn["q"]),
                                 Convert.ToString(parametersIn["o"]), Convert.ToString(parametersIn["pm"]), Convert.ToString(parametersIn["land"]), rt, ref xmlOut);


                        decimal dInstallationId = Convert.ToDecimal(strInstallationId);
                                                
                        DateTime dtTickectPayment = DateTime.ParseExact(parametersIn["d"].ToString(), "HHmmssddMMyyyy",CultureInfo.InvariantCulture);
                        decimal? dPaymentType = Convert.ToDecimal(parametersIn["pm"]);
                        string strPlate = Convert.ToString(parametersIn["lic_pla"]);
                        
                        decimal dGrpId = Convert.ToDecimal(parametersIn["grp_id"]);
                        GROUP oGROUP=null;
                        DateTime? dtgroup = null;


                        geograficAndTariffsRepository.getGroupByExtOpsId(parametersIn["grp_id"].ToString(), ref oGROUP, ref dtgroup);
                        decimal? dGroupId=null;
                        if (oGROUP != null)
                        {
                            dGroupId = oGROUP.GRP_ID;
                        }
                        decimal? dBackofficeUsrId = Convert.ToDecimal(parametersIn["backoffice_usr_id"]);

                        string sAdditionalinfo = Convert.ToString(parametersIn["additional_info"]);
                        
                        string strAppVersion = "";
                        decimal? dLatitude = null;
                        decimal? dLongitude = null;
                        double dChangeToApply = 1.0;

                        int iQuantity = Convert.ToInt32(parametersOut["q"]);
                        int iTotalQuantity = Convert.ToInt32(parametersOut["q_total"]);
                        int iFixedFEE = Convert.ToInt32(parametersOut["q_fee"]);
                        decimal dPercVAT1 = Convert.ToDecimal(parametersOut["q_VAT1"]);
                        decimal dPercVAT2 = Convert.ToDecimal(parametersOut["q_VAT2"]);
                        decimal dPercFEE = Convert.ToDecimal(parametersOut["q_PercFEE"]);

                        int iPercFEETopped = Convert.ToInt32(parametersOut["q_PercFEETopped"]);
                        int iPartialVAT1 = Convert.ToInt32(parametersOut["q_PartialVAT1"]);
                        int iPartialPercFEE = Convert.ToInt32(parametersOut["q_PartialPercFEE"]);
                        int iPartialFixedFEE = Convert.ToInt32(parametersOut["q_PartialFixedFEE"]);

                        int iCurrencyChargedQuantity = 0;

                        iCurrencyChargedQuantity = iTotalQuantity;

                        if (iCurrencyChargedQuantity < 0)
                        {
                            rt = (ResultType)iCurrencyChargedQuantity;
                            Logger_AddLogMessage(string.Format("ChargeFinePayment::Error Changing Quantity {0} ", rt.ToString()), LogLevels.logERROR);
                            xmlOut = GenerateXMLErrorResult(rt);
                            return xmlOut;
                        }


                        decimal dTicketPaymentID = -1;
                        decimal? dRechargeId = null;
                        double dChangeFee = 0;
                        DateTime? dtUTCInsertionDate = null;
                        INSTALLATION oInstallation=null;
                        DateTime? dtInsDateTime = null;
                        
                        if (!geograficAndTariffsRepository.getInstallationByStandardIdWebPortal(strInstallationId, ref oInstallation, ref dtInsDateTime))
                        {
                            oInstallation = null;
                        }

                        string strArticleType = string.Empty;
                        string strArticleDescription = string.Empty;
                        string strTicketData = string.Format("{0} ({1})", strArticleType, strArticleDescription);

                        DateTime? dtUTCDateTime = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtInsDateTime.Value);
                        
                        decimal dCurrencyId =oInstallation.CURRENCy.CUR_ID;
                        decimal dBalanceCurID = dCurrencyId;
                        bool bConfirmedWs = ((oInstallation.INS_OPT_FINECONFIRM_MODE ?? 0) == 0);
                        string strTransactionId = string.Empty;
                        string strCFTransactionId = string.Empty;
                        string strReference = string.Empty;
                        string strGatewayDate= string.Empty;
                        string strAuthCode= string.Empty;
                        string strAuthResult= string.Empty;
                        string strCardHash= string.Empty;
                        string strCardReference= string.Empty;
                        string strCardScheme= string.Empty;
                        string strMaskedCardNumber = string.Empty;
                        string strCurrencyISO = oInstallation.CURRENCy.CUR_ISO_CODE;
                        decimal dGatewayConfigId=0;
                        string authId = string.Empty;
                        DateTime? dtExpDate = null;
                        string strPaypal3tToken = "";
                        string strPaypal3tPayerId = "";

                        if (!fineRepository.ChargeFinePaymentNonUser(true, PaymentSuscryptionType.pstPerTransaction, oInstallation.INS_ID,
                                                                    dtTickectPayment, dtUTCDateTime.Value, strPlate, TicketNumber, strTicketData, iQuantity,
                                                                    dCurrencyId, dBalanceCurID, dChangeToApply, dChangeFee, iCurrencyChargedQuantity, dPercVAT1, dPercVAT2,
                                                                    iPartialVAT1, dPercFEE, iPercFEETopped, iPartialPercFEE, iFixedFEE, iPartialFixedFEE, iTotalQuantity, dRechargeId,
                                                                    bConfirmedWs, dLatitude, dLongitude, strAppVersion, dGroupId, dBackofficeUsrId, dPaymentType, sAdditionalinfo,
                                                                    out dTicketPaymentID, out dtUTCInsertionDate, strReference, strTransactionId, strCFTransactionId, strGatewayDate,
                                                                    strAuthCode, strAuthResult, strCardHash, strCardReference, strCardScheme, strMaskedCardNumber,
                                                                    strPaypal3tPayerId,strPaypal3tToken,dtExpDate,                                                                    
                                                                    oInstallation, PaymentMeanCreditCardProviderType.pmccpUndefined, strCurrencyISO, dGatewayConfigId, authId))
                        {
                            Logger_AddLogMessage(string.Format("ConfirmFinePaymentNonUser::Error Inserting Ticket Payment {0} ", TicketNumber), LogLevels.logERROR);
                            rt = ResultType.Result_Error_Generic;
                        }

                        if (rt != ResultType.Result_OK)
                        {
                            xmlOut = GenerateXMLErrorResult(rt);
                            m_Log.LogMessage(LogLevels.logERROR, "ConfirmFinePaymentNonUser: Null Installation or TicketNumber");
                            return xmlOut;
                        }
                        else if (Convert.ToInt32(rt) == 1)
                        {
                            parametersOut = new SortedList();
                            parametersOut["ConfirmFinePayment"] = 1;
                        }
                        
                        parametersOut["r"] = Convert.ToInt32(rt).ToString();
                        xmlOut = GenerateXMLOuput(parametersOut);

                        if (xmlOut.Length == 0)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                            m_Log.LogMessage(LogLevels.logERROR, "ConfirmFinePaymentNonUser: Null Installation or TicketNumber");
                        }
                        else
                        {
                            Logger_AddLogMessage(string.Format("ConfirmFinePayment: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                        }

                        if (parametersOut != null)
                        {
                            parametersOut.Clear();
                            parametersOut = null;
                        }
                    }
                }
            }
            catch (Exception)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                m_Log.LogMessage(LogLevels.logERROR, "ConfirmFinePaymentNonUser: Null Installation or TicketNumber");
            }

            return xmlOut;
        }

        [WebMethod]
        public string ConfirmFinePaymentNonUserJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                Logger_AddLogMessage(string.Format("ChargeFinePaymentNonUserJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = ConfirmFinePaymentNonUser(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

                Logger_AddLogMessage(string.Format("ChargeFinePaymentNonUserJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("ChargeFinePaymentNonUserJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);
            }

            return jsonOut;
        }

        [WebMethod]
        public bool SendPushAndroidNotificationsTo(string strUser,
                                                   string strAndroidRawData)
        {
            bool bRes = true;
            try
            {

                Logger_AddLogMessage(string.Format("SendPushAndroidNotificationsTo: strUser= {0}, strAndroidRawData=\n{1}",
                                                strUser,
                                                PrettyJSON(strAndroidRawData)), LogLevels.logINFO);


                USER oUser = null;

                if (customersRepository.GetUserData(ref oUser, strUser))
                {
                    if (!customersRepository.AddAndroidPushIDNotification(ref oUser,
                                                                        strAndroidRawData))
                    {
                        Logger_AddLogMessage(string.Format("SendPushAndroidNotificationsTo Error Inserting notification: strUser= {0}, strAndroidRawData=\n{1}",
                                                        strUser,
                                                        strAndroidRawData), LogLevels.logERROR);
                        bRes = false;

                    }
                    else
                    {
                        Logger_AddLogMessage(string.Format("SendPushAndroidNotificationsTo. Notification inserted OK: strUser= {0}, strAndroidRawData=\n{1}",
                                                        strUser,
                                                        strAndroidRawData), LogLevels.logINFO);

                    }


                }
                else
                {
                    Logger_AddLogMessage(string.Format("SendPushAndroidNotificationsTo User Not found: strUser= {0}",
                                                    strUser), LogLevels.logERROR);
                    bRes = false;
                }


            }
            catch (Exception e)
            {
                bRes = false;
                Logger_AddLogException(e, string.Format("SendPushAndroidNotificationsTo Error: strUser= {0}, strAndroidRawData=\n{1}",
                                                strUser,
                                                strAndroidRawData), LogLevels.logERROR);

            }

            return bRes;

        }

        //[WebMethod]
        public string CalculateMessageWithHash(string xmlIn)
        {
            string xmlOut = "";
            try
            {
                SortedList parametersIn = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("CalculateMessageWithHash: xmlIn={0}",PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    string strCalculatedHash = CalculateHash(strHashString, strHash);
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(xmlIn);


                    XmlNodeList Nodes = xmldoc.SelectNodes("//" + _xmlTagName + IN_SUFIX);
                    XmlElement node = xmldoc.CreateElement("ah");
                    node.InnerXml = strCalculatedHash;
                    Nodes[0].AppendChild(node);

                    xmlOut = xmldoc.OuterXml;


                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("CalculateMessageWithHash::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);

                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("CalculateMessageWithHash::Error: xmlIn={0}, xmlOut={1}",PrettyXml(xmlIn),PrettyXml(xmlOut)), LogLevels.logERROR);


            }

            return xmlOut;

        }
        //[WebMethod]
        public string CalculateMessageWithHashJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                Logger_AddLogMessage(string.Format("CalculateMessageWithHashJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = CalculateMessageWithHash(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

                Logger_AddLogMessage(string.Format("CalculateMessageWithHashJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("CalculateMessageWithHashJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);


            }

            return jsonOut;
        }

        //[WebMethod]
        public string CalculateHash(string strInput, string strHash)
        {
            string strRes = "";
            try
            {
                if ((ConfigurationManager.AppSettings["CheckSessionAndHash"].ToString() == "0") && (strHash.Length > 0))
                {
                    strRes = strHash;
                }
                else
                {

                    byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(strInput);
                    byte[] hash = null;

                    MACTripleDES _mac3des = null;
                    HMACSHA256 _hmacsha256 = null;
                    if (ConfigurationManager.AppSettings["AuthHashAlgorithmExternal"].ToString() == "HMACSHA256")
                    {
                        _hmacsha256 = new HMACSHA256(_normKey);
                    }
                    else if (ConfigurationManager.AppSettings["AuthHashAlgorithmExternal"].ToString() == "MACTripleDES")
                    {
                        _mac3des = new MACTripleDES(_normKey);
                    }

                    if (_mac3des != null)
                    {
                        hash = _mac3des.ComputeHash(inputBytes);

                    }
                    else if (_hmacsha256 != null)
                    {
                        hash = _hmacsha256.ComputeHash(inputBytes);
                    }


                    if (hash.Length >= 8)
                    {
                        StringBuilder sb = new StringBuilder();
                        for (int i = hash.Length - 8; i < hash.Length; i++)
                        {
                            sb.Append(hash[i].ToString("X2"));
                        }
                        strRes = sb.ToString();
                    }
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "CalculateHash::Exception", LogLevels.logERROR);

            }


            return strRes;
        }

        [WebMethod]
        public String GetLastOperations(String xmlIn)
        {
            string urlsubstring = "iparkme.com";
            int iTimeout = 30;
            String xmlOut = String.Empty;
            SortedList parametersOut = null;

            String strHash = String.Empty;
            String strHashString = String.Empty;

            integraMobile.ExternalWS.ResultType rtRes = integraMobile.ExternalWS.ResultType.Result_OK;

            SortedList parametersIn = null;

            Logger_AddLogMessage(string.Format("GetLastOperations: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

            ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

            if (rt == ResultType.Result_OK)
            {

                if (parametersIn["date"] == null)
                {
                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                    Logger_AddLogMessage(string.Format("GetLastOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                }
                else
                {
                    string strCalculatedHash = CalculateHash(strHashString, strHash);

                    if (strCalculatedHash != strHash)
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                        Logger_AddLogMessage(string.Format("GetLastOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                    }
                    else
                    {

                        try
                        {
                            iTimeout = int.Parse(parametersIn["timeout"].ToString());
                        }
                        catch
                        {

                        }

                        try
                        {
                            urlsubstring = parametersIn["urlsubstring"].ToString();
                        }
                        catch
                        {

                        }


                        try
                        {
                            DateTime df;
                            try
                            {
                                DateTime dDateTimeNowUTC = DateTime.UtcNow;
                                df = DateTime.ParseExact(parametersIn["date"].ToString(), "HHmmssddMMyy",
                                      CultureInfo.InvariantCulture);
                                if ((dDateTimeNowUTC - df).TotalSeconds > 30)
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                    Logger_AddLogMessage(string.Format("getLastOperations: parametersIn[date]={0}, DateTimeNowUTC={1}", df, dDateTimeNowUTC), LogLevels.logERROR);
                                }
                                else
                                {
                                    OPERATION oOperations = customersRepository.GetLastOperations(iTimeout, urlsubstring);
                                    if (oOperations != null)
                                    {
                                        Int32 iWSNumber = 0;

                                        bool bExit = false;
                                        do
                                        {
                                            switch (iWSNumber)
                                            {
                                                case 0:
                                                    bExit = (((ConfirmParkWSSignatureType)(oOperations.INSTALLATION.INS_PARK_CONFIRM_WS_SIGNATURE_TYPE)) == ConfirmParkWSSignatureType.cpst_standard);
                                                    break;
                                                case 1:
                                                    bExit = (((ConfirmParkWSSignatureType)(oOperations.INSTALLATION.INS_PARK_CONFIRM_WS2_SIGNATURE_TYPE ?? 0)) == ConfirmParkWSSignatureType.cpst_standard);
                                                    break;
                                                case 2:
                                                    bExit = (((ConfirmParkWSSignatureType)(oOperations.INSTALLATION.INS_PARK_CONFIRM_WS3_SIGNATURE_TYPE ?? 0)) == ConfirmParkWSSignatureType.cpst_standard);
                                                    break;
                                                default:
                                                    bExit = true;
                                                    break;

                                            }

                                            if (!bExit)
                                                iWSNumber++;

                                        }
                                        while (!bExit);


                                        if (iWSNumber <= 2)
                                        {

                                            String sExtGroupId = String.Empty;
                                            String sExtTariffId = String.Empty;

                                            ThirdPartyOperation oThirdPartyOperation = new ThirdPartyOperation();
                                            Logger_AddLogMessage(string.Format("ThirdPartyOperation.GetLastOperations: xmlIn={0}, xmlIn={1}, xmlIn={2}", iWSNumber, oOperations.OPE_GRP_ID.Value, oOperations.OPE_TAR_ID.Value), LogLevels.logINFO);
                                            rtRes = oThirdPartyOperation.GetLastOperations(iWSNumber, oOperations.OPE_GRP_ID.Value, oOperations.OPE_TAR_ID.Value, ref sExtGroupId, ref sExtTariffId);

                                            parametersOut = new SortedList();
                                            parametersOut["ins_id"] = oOperations.INSTALLATION.INS_STANDARD_CITY_ID;
                                            parametersOut["grp_id"] = sExtGroupId;
                                            parametersOut["tar_id"] = sExtTariffId;
                                            parametersOut["lic_pla"] = oOperations.USER_PLATE.USRP_PLATE;
                                            parametersOut["d_end"] = string.Format("{0:HHmmssfffddMMyy}", oOperations.OPE_ENDDATE);

                                            xmlOut = GenerateXMLOuput(parametersOut);
                                        }
                                        else
                                        {
                                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                        }
                                    }
                                    else
                                    {
                                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                    }

                                    Logger_AddLogMessage(string.Format("GetLastOperations: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                                }
                            }
                            catch (Exception e)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogException(e, string.Format("GetLastOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                            }

                        }
                        catch (Exception e)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                            Logger_AddLogException(e, string.Format("GetLastOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }

                    }
                }
            }
            return xmlOut;
        }

        [WebMethod]
        public string GetLastOperationsJSON(String jsonIn)
        {
            string jsonOut = String.Empty; 
            try
            {
                Logger_AddLogMessage(string.Format("GetLastOperationsJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                String strXmlOut = GetLastOperations(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

                Logger_AddLogMessage(string.Format("GetLastOperationsJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("GetLastOperationsJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }

        [WebMethod]
        public String RechargePayments()
        {
            /*//List<TICKET_PAYMENTS_NON_USER> oListFines = fineRepository.GetTicketsPaymentNonUser(new DateTime(2018, 12, 21)).ToList();
            List<TICKET_PAYMENTS_NON_USER> oListFines = fineRepository.GetTicketsPaymentNonUser(223,241).ToList();


            foreach (TICKET_PAYMENTS_NON_USER oFine in oListFines)
            {

                MonerisPayments cardPayment = new MonerisPayments();
                string errorMessage = "";
                string strUserReference = null;
                string strAuthCode = null;
                string strAuthResult = null;
                string strGatewayDate = null;
                string strTransactionId = null;
                MonerisPayments.MonerisErrorCode eErrorCode = MonerisPayments.MonerisErrorCode.InternalError;
                int iTotalQuantity = 0;

                NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                numberFormatProvider.NumberDecimalSeparator = ".";
                decimal dQuantity = 0;
                decimal dQuantityToCharge = 0;


                iTotalQuantity = oFine.TIPANU_TOTAL_AMOUNT.Value;

                dQuantity = Convert.ToDecimal(iTotalQuantity, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oFine.CURRENCy.CUR_ISO_CODE);
                dQuantityToCharge = Convert.ToDecimal(iTotalQuantity, numberFormatProvider) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oFine.CURRENCy.CUR_ISO_CODE);

                NumberFormatInfo provider = new NumberFormatInfo();
                string strAmount = dQuantityToCharge.ToString("#" + infraestructureRepository.GetDecimalFormatFromIsoCode(oFine.CURRENCy.CUR_ISO_CODE), provider);

                strUserReference = MonerisPayments.UserReference();
                bool bPayIsCorrect = cardPayment.AutomaticTransaction(oFine.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_ID,
                                               oFine.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_API_STORE_KEY,
                                               strUserReference,
                                               oFine.TIPANU_CARD_REFERENCE,
                                               strAmount,
                                               oFine.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_PROCESING_COUNTRY,
                                               oFine.TIPANU_TICKET_NUMBER,
                                               oFine.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_CHECK_CARD_STATUS != 0,
                                               oFine.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MONERIS_CONFIGURATION.MONCON_TEST_MODE != 0,
                                               out eErrorCode, out errorMessage, out strTransactionId, out strAuthCode, out strAuthResult, out strGatewayDate);

                if (bPayIsCorrect)
                {
                    fineRepository.UpdateFine(oFine.TIPANU_ID, strUserReference, strTransactionId, strGatewayDate, strAuthCode, strAuthResult);
                }
            
            }*/



            return "ok";

                                   
        }

        #endregion

        #region Private methods

        private string GetEmailFooter(ref USER oUser)
        {
            string strFooter = "";

            try
            {
                strFooter = ResourceExtension.GetLiteral(string.Format("footer_CUR_{0}_{1}", infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)), oUser.COUNTRy.COU_CODE));
                if (string.IsNullOrEmpty(strFooter))
                {
                    strFooter = ResourceExtension.GetLiteral(string.Format("footer_CUR_{0}", infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))));
                }
            }
            catch
            {

            }

            return strFooter;
        }
        
        private static void InitializeStatic()
        {

            int iKeyLength = 24;

            if (_hMacKey == null)
            {
                _hMacKey = ConfigurationManager.AppSettings["AuthHashKeyExternal"].ToString();
            }


            if (ConfigurationManager.AppSettings["AuthHashAlgorithmExternal"].ToString() == "HMACSHA256")
            {
                iKeyLength = 64;
            }
            else if (ConfigurationManager.AppSettings["AuthHashAlgorithmExternal"].ToString() == "MACTripleDES")
            {
                iKeyLength = 24;
            }



            if (_normKey == null)
            {
                byte[] keyBytes = System.Text.Encoding.UTF8.GetBytes(_hMacKey);
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
            }

          

        }

        private ResultType FindInputParameters(string xmlIn, out SortedList parameters, out string strHash, out string strHashString)
        {
            ResultType rtRes = ResultType.Result_OK;
            parameters = new SortedList();
            strHash = "";
            strHashString = "";

            try
            {
                XmlDocument xmlInDoc = new XmlDocument();
                try
                {
                    xmlInDoc.LoadXml(xmlIn);
                    XmlNodeList Nodes = xmlInDoc.SelectNodes("//" + _xmlTagName + IN_SUFIX + "/*");
                    foreach (XmlNode Node in Nodes)
                    {
                        switch (Node.Name)
                        {
                            case "ah":
                                strHash = Node.InnerText;
                                break;
                            default:

                                if (Node.HasChildNodes)
                                {
                                    if (Node.ChildNodes[0].HasChildNodes)
                                    {
                                        foreach (XmlNode ChildNode in Node.ChildNodes)
                                        {                                            
                                            if (!ChildNode.ChildNodes[0].HasChildNodes)
                                            {
                                                strHashString += ChildNode.InnerText;
                                                parameters[Node.Name + "_" + ChildNode.Name] = ChildNode.InnerText.Trim();
                                            }
                                            else
                                            {
                                                
                                                foreach (XmlNode ChildNode2 in ChildNode.ChildNodes)
                                                {
                                                    strHashString += ChildNode2.InnerText;
                                                    parameters[Node.Name + "_" + ChildNode.Name + "_" + ChildNode2.Name] = ChildNode2.InnerText.Trim();
                                                }
                                            }


                                        }
                                    }
                                    else
                                    {
                                        strHashString += Node.InnerText;
                                        parameters[Node.Name] = Node.InnerText.Trim();
                                    }
                                }
                                else
                                {
                                    parameters[Node.Name] = null;
                                }

                                break;

                        }

                    }

                    if (Nodes.Count == 0)
                    {
                        Logger_AddLogMessage(string.Format("FindInputParameters: Bad Input XML: xmlIn={0}",PrettyXml(xmlIn)), LogLevels.logERROR);
                        rtRes = ResultType.Result_Error_Invalid_Input_Parameter;

                    }


                }
                catch
                {
                    Logger_AddLogMessage(string.Format("FindInputParameters: Bad Input XML: xmlIn={0}",PrettyXml(xmlIn)), LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Invalid_Input_Parameter;
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "FindInputParameters::Exception", LogLevels.logERROR);

            }


            return rtRes;
        }

        private string GenerateXMLOuput(SortedList parametersOut)
        {
            string strRes = "";
            try
            {
                XmlDocument xmlOutDoc = new XmlDocument();

                XmlDeclaration xmldecl;
                xmldecl = xmlOutDoc.CreateXmlDeclaration("1.0", null, null);
                xmldecl.Encoding = "UTF-8";
                xmlOutDoc.AppendChild(xmldecl);

                XmlElement root = xmlOutDoc.CreateElement(_xmlTagName + OUT_SUFIX);
                xmlOutDoc.AppendChild(root);
                XmlNode rootNode = xmlOutDoc.SelectSingleNode(_xmlTagName + OUT_SUFIX);

                foreach (DictionaryEntry item in parametersOut)
                {
                    try
                    {
                        XmlElement node = xmlOutDoc.CreateElement(item.Key.ToString());
                        node.InnerXml = item.Value.ToString().Trim();
                        rootNode.AppendChild(node);
                    }
                    catch (Exception e)
                    {
                        Logger_AddLogException(e, "GenerateXMLOuput::Exception", LogLevels.logERROR);
                    }
                }

                strRes = xmlOutDoc.OuterXml;

                if (parametersOut["r"] != null)
                {
                    try
                    {
                        int ir = Convert.ToInt32(parametersOut["r"].ToString());
                        ResultType rt = (ResultType)ir;

                        if (ir < 0)
                        {
                            Logger_AddLogMessage(string.Format("Error = {0}", rt.ToString()), LogLevels.logERROR);
                        }
                    }
                    catch
                    {

                    }


                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "GenerateXMLOuput::Exception", LogLevels.logERROR);

            }


            return strRes;
        }

        private string GenerateXMLErrorResult(ResultType rt)
        {
            string strRes = "";
            try
            {
                Logger_AddLogMessage(string.Format("Error = {0}", rt.ToString()), LogLevels.logERROR);

                XmlDocument xmlOutDoc = new XmlDocument();

                XmlDeclaration xmldecl;
                xmldecl = xmlOutDoc.CreateXmlDeclaration("1.0", null, null);
                xmldecl.Encoding = "UTF-8";
                xmlOutDoc.AppendChild(xmldecl);

                XmlElement root = xmlOutDoc.CreateElement(_xmlTagName + OUT_SUFIX);
                xmlOutDoc.AppendChild(root);
                XmlNode rootNode = xmlOutDoc.SelectSingleNode(_xmlTagName + OUT_SUFIX);
                XmlElement result = xmlOutDoc.CreateElement("r");
                result.InnerText = ((int)rt).ToString();
                rootNode.AppendChild(result);
                strRes = xmlOutDoc.OuterXml;

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "GenerateXMLErrorResult::Exception", LogLevels.logERROR);

            }


            return strRes;
        }

        private string GenerateJSONErrorResult(ResultType rt)
        {
            string jsonOut = "";
            try
            {

                string strXmlOut = GenerateXMLErrorResult(rt);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "GenerateJSONErrorResult::Exception", LogLevels.logERROR);

            }


            return jsonOut;
        }

        private bool SendEmail(ref USER oUser, string strEmailSubject, string strEmailBody, decimal dSourceApp)
        {
            bool bRes = true;
            try
            {

                long lSenderId = infraestructureRepository.SendEmailTo(oUser.USR_EMAIL, strEmailSubject, strEmailBody, dSourceApp);

                if (lSenderId > 0)
                {
                    customersRepository.InsertUserEmail(ref oUser, oUser.USR_EMAIL, strEmailSubject, strEmailBody, lSenderId);
                }

            }
            catch
            {
                bRes = false;
            }

            return bRes;
        }

        private ResultType Convert_integraMobileExternalWSResultType_TO_ResultType(integraMobile.ExternalWS.ResultType oExtResultType)
        {
            ResultType rtResultType = ResultType.Result_Error_Generic;

            switch (oExtResultType)
            {
                case integraMobile.ExternalWS.ResultType.Result_OK:
                    rtResultType = ResultType.Result_OK;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_InvalidAuthenticationHash:
                    rtResultType = ResultType.Result_Error_InvalidAuthenticationHash;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_Invalid_Input_Parameter:
                    rtResultType = ResultType.Result_Error_Invalid_Input_Parameter;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_Missing_Input_Parameter:
                    rtResultType = ResultType.Result_Error_Missing_Input_Parameter;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_Generic:
                    rtResultType = ResultType.Result_Error_Generic;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_OperationAlreadyClosed:
                    rtResultType = ResultType.Result_Error_OperationAlreadyClosed;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_OperationEntryAlreadyExists:
                    rtResultType = ResultType.Result_Error_OperationEntryAlreadyExists;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_Recharge_Failed:
                    rtResultType = ResultType.Result_Error_Recharge_Failed;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_Recharge_Not_Possible:
                    rtResultType = ResultType.Result_Error_Recharge_Not_Possible;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_Invalid_Payment_Mean:
                    rtResultType = ResultType.Result_Error_Invalid_Payment_Mean;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Error_Not_Enough_Balance:
                    rtResultType = ResultType.Result_Error_Not_Enough_Balance;
                    break;
                case integraMobile.ExternalWS.ResultType.Result_Toll_is_Not_from_That_installation:
                    rtResultType = ResultType.Result_Toll_is_Not_from_That_installation;
                    break;

                default:
                    break;
            }
            return rtResultType;
        }

        private integraMobile.WS.integraCommonService CommonService()
        {
            return new integraMobile.WS.integraCommonService(customersRepository, infraestructureRepository, geograficAndTariffsRepository);
        }

        private static void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }

        private static void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }

        static string PrettyXml(string xml)
        {

            try
            {
                var stringBuilder = new StringBuilder();

                var element = XElement.Parse(xml);

                var settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;
                settings.Indent = true;
                settings.NewLineOnAttributes = true;

                using (var xmlWriter = XmlWriter.Create(stringBuilder, settings))
                {
                    element.Save(xmlWriter);
                }

                return "\r\n\t" + stringBuilder.ToString().Replace("\r\n", "\r\n\t") + "\r\n";
            }
            catch
            {
                return "\r\n\t" + xml + "\r\n";
            }
        }

        static string PrettyJSON(string json)
        {

            try
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(json);
                string strRes = JsonConvert.SerializeObject(parsedJson, Newtonsoft.Json.Formatting.Indented);
                return "\r\n\t" + strRes.Replace("\r\n", "\r\n\t");
            }
            catch
            {
                return "\r\n\t" + json + "\r\n";
            }
        }

        private string NormalizePlate(string strPlate)
        {
            string strResPlate = "";
            strResPlate = strPlate.Trim().Replace(" ", "").ToUpper();
            strResPlate = new string(strResPlate.Where(c => char.IsLetterOrDigit(c)).ToArray());
            return strResPlate;
        }

        private int? MaxAmountAllowedToPay(ref USER oUser, decimal dInsId)
        {
            int? iAmount = oUser.USR_BALANCE;


            try
            {
                PaymentSuscryptionType? eUserSuscrytpionType = null;
                if (oUser.USR_SUSCRIPTION_TYPE.HasValue)
                    eUserSuscrytpionType = (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE.Value;

                INSTALLATION oInstallation = null;
                DateTime? dtInsDateTime = null;
                if (geograficAndTariffsRepository.getInstallation(dInsId, null, null, ref oInstallation, ref dtInsDateTime))
                {
                    if (oInstallation.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG != null)
                    {
                        iAmount = 0;
                        eUserSuscrytpionType = PaymentSuscryptionType.pstPerTransaction;
                    }
                }

                CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, oInstallation);

                if ((oUserPaymentMean != null) &&
                    (oUserPaymentMean.CUSPM_ENABLED == 1) &&
                     (oUserPaymentMean.CUSPM_VALID == 1))
                {
                    if (((eUserSuscrytpionType == PaymentSuscryptionType.pstPrepay) &&
                        (oUserPaymentMean.CUSPM_AUTOMATIC_RECHARGE == 1) &&
                        (oUserPaymentMean.CUSPM_AMOUNT_TO_RECHARGE > 0)) ||
                        (eUserSuscrytpionType == PaymentSuscryptionType.pstPerTransaction))
                    {
                        iAmount = null;
                    }
                }
            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "MaxAmountAllowedToPay::Exception", LogLevels.logERROR);

            }

            return iAmount;
        }

        private SortedList CalculateOperationAmountsPrivate(string xmlIn, string strHashString, string strHash, string sCity_id, string sDateTime, string sQuantity,
                                                             string sOperationType, string sPaymentMethod, string sLanguage, ResultType rt, ref string xmlOut)
        {
            SortedList parametersOut = new SortedList();
            string strCalculatedHash = CalculateHash(strHashString, strHash);

            if (strCalculatedHash != strHash)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                Logger_AddLogMessage(string.Format("CalculateOperationAmounts::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
            }
            else
            {
                ResultType rtRes = ResultType.Result_OK;

                string strQPlusVATQs = "";

                if (rt == ResultType.Result_OK)
                {
                    int iQ = 0;
                    int iQFEE = 0;
                    decimal dQFEE = 0;
                    int iQVAT = 0;
                    int iQTotal = 0;
                    int iQSubTotal = 0;

                    decimal dVAT1;
                    decimal dVAT2;
                    int iPartialVAT1;
                    decimal dPercFEE;
                    decimal dPercFEETopped;
                    int iPartialPercFEE;
                    decimal dFixedFEE;
                    int iPartialFixedFEE;
                    int iPartialPercFEEVAT;
                    int iPartialFixedFEEVAT;

                    int? iPaymentTypeId = Convert.ToInt32(sPaymentMethod);
                    int? iPaymentSubtypeId = null;
                    IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;

                    INSTALLATION oInstallation = null;
                    DateTime? dtinstDateTime = null;

                    if (!geograficAndTariffsRepository.getInstallationByStandardIdWebPortal(sCity_id, ref oInstallation, ref dtinstDateTime))
                    {
                        oInstallation = null;
                    }

                    if (oInstallation != null)
                    {
                        LANGUAGE oLANGUAGE=null;
                        if (String.IsNullOrEmpty(sLanguage))
                            sLanguage = "en-US";

                        decimal dLanguage = infraestructureRepository.GetLanguage(sLanguage);
                        if (dLanguage == -1)
                        {
                            dLanguage = 2; 
                        };
                        infraestructureRepository.GetLanguage(dLanguage, out oLANGUAGE);
                        int? iOperationType = Convert.ToInt32(sOperationType);
                        
                        string strCulture = oLANGUAGE.LAN_CULTURE;
                        if (!customersRepository.GetFinantialParams(oInstallation.INS_ID, iPaymentTypeId, iPaymentSubtypeId, (ChargeOperationsType)iOperationType, null,
                            out dVAT1, out dVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode))
                        {
                            rt = ResultType.Result_Error_Generic;
                            Logger_AddLogMessage("CalculateOperationAmounts::Error getting finantial parameters", LogLevels.logERROR);
                        }


                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();

                        iQ = Convert.ToInt32(sQuantity);

                        iQTotal = customersRepository.CalculateFEE(ref iQ, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE, eTaxMode,
                                                                    out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE,
                                                                    out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                        dQFEE = Math.Round(iQ * dPercFEE, MidpointRounding.AwayFromZero);
                        if (dPercFEETopped > 0 && iQFEE > dPercFEETopped) dQFEE = dPercFEETopped;
                        dQFEE += dFixedFEE;
                        iQFEE = Convert.ToInt32(Math.Round(dQFEE, MidpointRounding.AwayFromZero));

                        iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;
                        iQSubTotal = iQ + iQFEE;

                        int iQPlusIVA = iQ + iPartialVAT1;
                        int iFeePlusIVA = iPartialPercFEE + iPartialFixedFEE;

                        parametersOut["q"] = iQ;
                        parametersOut["q_lbl"] = infraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_FINE_LIT_ID ?? 0, strCulture);
                        parametersOut["layout"] = oInstallation.INS_FEE_LAYOUT;
                        parametersOut["q_fee_lbl"] = infraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_FEE_LIT_ID ?? 0, strCulture);
                        parametersOut["q_fee_vat_lbl"] = infraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_FEE_PLUS_VAT_LIT_ID ?? 0, strCulture);
                        parametersOut["q_vat_lbl"] = infraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_VAT_LIT_ID ?? 0, strCulture);
                        parametersOut["q_subtotalLbl"] = infraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_SUBTOTAL_LIT_ID ?? 0, strCulture);
                        parametersOut["q_total_lbl"] = infraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_TOTAL_LIT_ID ?? 0, strCulture);
                        parametersOut["df_lbl"] = infraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_FINE_MAX_DATE_LIT_ID ?? 0, strCulture);
                        parametersOut["q_fee"] = iQFEE;
                        parametersOut["q_vat"] = iQVAT;
                        parametersOut["q_subtotal"] = iQSubTotal;
                        parametersOut["q_total"] = iQTotal;
                        parametersOut["q_plus_vat"] = iQPlusIVA;
                        parametersOut["q_fee_plus_vat"] = iFeePlusIVA;
                        parametersOut["q_VAT1"] = dVAT1;
                        parametersOut["q_VAT2"] = dVAT2;
                        parametersOut["q_PercFEE"] = dPercFEE;
                        parametersOut["q_PercFEETopped"] = dPercFEETopped;
                        parametersOut["q_TaxMode"] = eTaxMode;
                        parametersOut["q_PartialVAT1"] = iPartialVAT1;
                        parametersOut["q_PartialPercFEE"] = iPartialPercFEE;
                        parametersOut["q_PartialFixedFEE"] = iPartialFixedFEE;

                        strQPlusVATQs += string.Format("{0};{1}", iQ, iQPlusIVA);

                        DateTime? dtUTCDateTime = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtinstDateTime.Value);

                        decimal? dAuthId = null;
                        if (parametersOut["AuthId"] != null)
                        {
                            try
                            {
                                decimal dTryAuthId = Convert.ToDecimal(parametersOut["AuthId"].ToString());
                                dAuthId = dTryAuthId;
                            }
                            catch
                            {
                                dAuthId = null;
                            }
                        }

                        string sExtGrpId = "";
                        decimal? dGrpId = null;
                        if (parametersOut["ExtGrpId"] != null) sExtGrpId = parametersOut["ExtGrpId"].ToString();
                        if (!string.IsNullOrWhiteSpace(sExtGrpId))
                        {
                            GROUP oGroup = null;
                            DateTime? dtGroupDateTime = null;
                            if (geograficAndTariffsRepository.getGroupByExtOpsId(sExtGrpId, ref oGroup, ref dtGroupDateTime))
                            {
                                dGrpId = oGroup.GRP_ID;
                            }
                        }
                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Invalid_City;
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                        Logger_AddLogMessage(string.Format("CalculateOperationAmounts::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                    }

                }
                parametersOut["r"] = Convert.ToInt32(rtRes).ToString();
            }
            return parametersOut;
        }


        private string GetEmailSourceAppEmailPrefix(decimal dSourceApp)
        {
            string strRes = "";

            decimal dDefaultSourceApp = geograficAndTariffsRepository.GetDefaultSourceApp();
            if (dSourceApp != dDefaultSourceApp)
            {
                strRes = geograficAndTariffsRepository.GetSourceAppCode(dSourceApp) + "_";
            }

            return strRes;
        }
        #endregion

    }
}
