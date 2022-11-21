using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Services;
using System.Configuration;
using System.Xml;
using System.Text;
using System.Web.Security;
using System.Security.Cryptography;
using System.Globalization;
using System.Threading;
using System.Net.NetworkInformation;
using System.IO;
using System.Diagnostics;
using Ninject;
using Ninject.Web;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Domain.Helper;
using integraMobile.Infrastructure;
using integraMobile.ExternalWS;
using integraMobile.WS.Resources;
using integraMobile.WS.Infrastructure;

using Newtonsoft.Json;
using integraMobile.Infrastructure.Logging.Tools;
using log4net;
using Newtonsoft.Json.Linq;
using integraMobile.WS.Tools;
using System.Reflection;

namespace integraMobile.WS
{
    public partial class integraMobileWS
    {

        #region integraMobile.WS Web Methods

        [WebMethod]
        public string QueryParkingNearConfigurations(string xmlIn)
        {
            string xmlOut = "";

            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            xmlOut = TestRedirection(xmlIn, sMethodName);
            if (!string.IsNullOrEmpty(xmlOut))
            {
                return xmlOut;
            }

            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("QueryParkingNearConfigurations: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters2(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["u"] == null) ||
                        (parametersIn["SessionID"] == null) ||
                        (parametersIn["gps_lat"] == null) ||
                        (parametersIn["gps_long"] == null) ||
                        (parametersIn["lang"] == null)
                        )
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("QueryParkingNearConfigurations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                    }
                    else
                    {
                        string strCalculatedHash = CalculateHash(strHashString, strHash);

                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("QueryParkingNearConfigurations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {
                            USER oUser = null;
                            decimal? dInsId = null;
                            string strCulture = "";
                            string strAppVersion = "";

                            rt = GetUserData(ref oUser, parametersIn, true, out dInsId, out strCulture, out strAppVersion);
                            if (rt != ResultType.Result_OK)
                            {
                                xmlOut = GenerateXMLErrorResult(rt);
                                Logger_AddLogMessage(string.Format("QueryParkingNearConfigurations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            decimal? dLatitude = null;
                            decimal? dLongitude = null;
                            GetLatLon(parametersIn, out dLatitude, out dLongitude);

                            string sCulture = "es-ES";
                            string sLocale = "es";
                            try
                            {
                                int iLangIndex = Convert.ToInt32(parametersIn["lang"].ToString());
                                if (iLangIndex <= UserDeviceLangs.Length)
                                {
                                    sCulture = UserDeviceLangs[iLangIndex - 1];
                                }
                            }
                            catch
                            { }
                            if (!string.IsNullOrEmpty(sCulture))
                            {
                                sLocale = sCulture.Split('-')[0].Trim();
                            }

                            INSTALLATION oInstallation = null;
                            DateTime? dtinstDateTime = null;
                            bool bValidCurrency = true;
                            if (!geograficAndTariffsRepository.getInstallation(dInsId,
                                                                         dLatitude,
                                                                         dLongitude, oUser.USR_CUR_ID,
                                                                         ref oInstallation,
                                                                         ref dtinstDateTime,
                                                                         out bValidCurrency))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                                Logger_AddLogMessage(string.Format("QueryParkingNearConfigurations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            BSMConfigurations oConfigurations = null;

                            ThirdPartyOperation oThirParyOperation = new ThirdPartyOperation();


                            int iWSTimeout = infraestructureRepository.GetRateWSTimeout(dInsId.Value);

                            switch ((ParkWSSignatureType)oInstallation.INS_PARK_WS_SIGNATURE_TYPE)
                            {
                                case /*ParkWSSignatureType.pst_standard_amount_steps:*/ ParkWSSignatureType.pst_bsm:
                                    {
                                        long lEllapsedTime;
                                        BSMErrorResponse oErrorResponse = null;
                                        rt = oThirParyOperation.BSMNearConfigurations(oInstallation, dLatitude.Value, dLongitude.Value, sLocale, iWSTimeout, out oConfigurations, out oErrorResponse, out lEllapsedTime);
                                    }
                                    break;
                                default:
                                    rt = ResultType.Result_Error_Generic;
                                    break;
                            }

                            if (rt == ResultType.Result_OK && oConfigurations != null)
                            {                          

                                var oTariffs = oConfigurations.Configurations.Where(c => c.rateId.HasValue)
                                                                             .GroupBy(c => new { rateId = c.rateId, rateDescription = c.rateDescription })
                                                                             .Select(g => new { id = g.Key.rateId, desc = g.Key.rateDescription })
                                                                             .ToList();
                                decimal dTarId;
                                foreach (var oTariff in oTariffs)
                                {
                                    if (infraestructureRepository.InsertTariffIfRequired(oInstallation.INS_ID, oTariff.id.ToString(), oTariff.desc, TariffType.RegularTariff, TariffBehavior.StartStop, out dTarId))
                                    {
                                        oConfigurations.Configurations.Where(c => c.rateId == oTariff.id)
                                                                      .ToList()
                                                                      .ForEach(conf => conf.tarId = dTarId); 

                                    }
                                    else
                                    {
                                        rt = ResultType.Result_Error_Generic;
                                        break;
                                    }
                                }
                            }

                            parametersOut = new SortedList();
                            parametersOut["r"] = Convert.ToInt32(rt).ToString();

                            if (rt == ResultType.Result_OK && oConfigurations != null)
                            {
                                parametersOut["configurations"] = oConfigurations.ToCustomXml();
                            }

                            xmlOut = GenerateXMLOuput(parametersOut, new List<string>() { "configurations" });

                            if (xmlOut.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("QueryParkingNearConfigurations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("QueryParkingNearConfigurations: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                            }
                        }

                    }
                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("QueryParkingNearConfigurations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                }

                if (parametersIn != null)
                {
                    parametersIn.Clear();
                    parametersIn = null;
                }

                if (parametersOut != null)
                {
                    parametersOut.Clear();
                    parametersOut = null;
                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("QueryParkingNearConfigurations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
            }

            return xmlOut;
        }


        [WebMethod]
        public string QueryParkingNearConfigurationsJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = QueryParkingNearConfigurations(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);
                xmlOut = null;

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("QueryParkingNearConfigurationsJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }

        [WebMethod]
        public string StopExpiredOperations(string xmlIn)
        {
            string xmlOut = "";

            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            xmlOut = TestRedirection(xmlIn, sMethodName);
            if (!string.IsNullOrEmpty(xmlOut))
            {
                return xmlOut;
            }

            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("StopExpiredOperations: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters2(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["d"] == null) ||
                        (parametersIn["num_op"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("StopExpiredOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                    }
                    else
                    {
                        string strCalculatedHash = CalculateHash(strHashString, strHash);

                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("StopExpiredOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {
                            DateTime dt;
                            try
                            {
                                dt = DateTime.ParseExact(parametersIn["d"].ToString(), "HHmmssddMMyy", CultureInfo.InvariantCulture);
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("StopExpiredOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            int iNumOperations = 0;
                            try
                            {
                                iNumOperations = Convert.ToInt32(parametersIn["num_op"].ToString());
                            }
                            catch
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                                Logger_AddLogMessage(string.Format("StopExpiredOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            List<decimal> oStoppedOperationsIds = null;
                            if (!customersRepository.StopExpiredOperations(iNumOperations, out oStoppedOperationsIds))
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("StopExpiredOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            foreach (decimal dOpeId in oStoppedOperationsIds)
                            {
                                Email_ConfirmUnParking(null, dOpeId, null, true);
                            }                            

                            parametersOut = new SortedList();
                            parametersOut["r"] = Convert.ToInt32(rt).ToString();

                            parametersOut["num_stopped_op"] = oStoppedOperationsIds.Count;

                            xmlOut = GenerateXMLOuput(parametersOut);

                            if (xmlOut.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("StopExpiredOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("StopExpiredOperations: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                            }
                        }

                    }
                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("StopExpiredOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                }

                if (parametersIn != null)
                {
                    parametersIn.Clear();
                    parametersIn = null;
                }

                if (parametersOut != null)
                {
                    parametersOut.Clear();
                    parametersOut = null;
                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("StopExpiredOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
            }

            return xmlOut;
        }


        [WebMethod]
        public string StopExpiredOperationsJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = StopExpiredOperations(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);
                xmlOut = null;

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("StopExpiredOperationsJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }

        [WebMethod]
        public string QueryCurrentParkingOperations(string xmlIn)
        {
            string xmlOut = "";

            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            xmlOut = TestRedirection(xmlIn, sMethodName);
            if (!string.IsNullOrEmpty(xmlOut))
            {
                return xmlOut;
            }

            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("QueryCurrentParkingOperations: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["u"] == null) ||
                        (parametersIn["SessionID"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("QueryCurrentParkingOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                    }
                    else
                    {
                        string strCalculatedHash = CalculateHash(strHashString, strHash);

                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("QueryCurrentParkingOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {

                            USER oUser = null;
                            decimal? dInsId = null;
                            string strCulture = "";
                            string strAppVersion = "";                            

                            rt = GetUserData(ref oUser, parametersIn, true, out dInsId, out strCulture, out strAppVersion);

                            if (rt != ResultType.Result_OK)
                            {
                                xmlOut = GenerateXMLErrorResult(rt);
                                Logger_AddLogMessage(string.Format("QueryCurrentParkingOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            if (!dInsId.HasValue)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                                Logger_AddLogMessage(string.Format("QueryCurrentParkingOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            ulong ulAppVersion = AppUtilities.AppVersion(strAppVersion);

                            
                            int iParksCount = 0;
                            string sParks = GetUserCurrentParkingOperations(ref oUser, dInsId.Value, strCulture, ulAppVersion);

                            parametersOut = new SortedList();
                            parametersOut["r"] = iParksCount;
                            parametersOut["userparks"] = sParks;

                            xmlOut = GenerateXMLOuput(parametersOut, new List<string> { "userparks" });

                            if (xmlOut.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("QueryCurrentParkingOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("QueryCurrentParkingOperations: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                            }

                            oUser = null;

                        }
                    }

                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("QueryCurrentParkingOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                }

                if (parametersIn != null)
                {
                    parametersIn.Clear();
                    parametersIn = null;
                }

                if (parametersOut != null)
                {
                    parametersOut.Clear();
                    parametersOut = null;
                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("QueryCurrentParkingOperations::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

            }

            return xmlOut;
        }



        [WebMethod]
        public string QueryCurrentParkingOperationsJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                //Logger_AddLogMessage(string.Format("QueryOperationListJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = QueryCurrentParkingOperations(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);
                xmlOut = null;

                Logger_AddLogMessage(string.Format("QueryCurrentParkingOperationsJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("QueryCurrentParkingOperationsJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }

        [WebMethod]
        public string BlinkaySuiteAppLogin(string xmlIn)
        {
            string xmlOut = "";

            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            xmlOut = TestRedirection(xmlIn, sMethodName);
            if (!string.IsNullOrEmpty(xmlOut))
            {
                return xmlOut;
            }

            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("BlinkaySuiteAppLogin: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["u"] == null) ||
                        (parametersIn["pasw"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("BlinkaySuiteAppLogin::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                    }
                    else
                    {
                        string strCalculatedHash = CalculateHash(strHashString, strHash);

                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("BlinkaySuiteAppLogin::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {
                            string strUsername = parametersIn["u"].ToString();
                            string sPwd = parametersIn["pasw"].ToString();

                            ThirdPartyLogin oThirdPartyLogin = new ThirdPartyLogin();

                            parametersOut = new SortedList();

                            List<INSTALLATION> oInstallations = new List<INSTALLATION>(); 
                            Dictionary<decimal, List<string>> oInsRoles =  new Dictionary<decimal, List<string>>();

                            string sPICUrl = infraestructureRepository.GetParameterValue("BlinkaySuiteApp_PICWS_Url");
                            string sHashKey = infraestructureRepository.GetParameterValue("BlinkaySuiteApp_PICWS_HashKey");
                            string sHttpUser = infraestructureRepository.GetParameterValue("BlinkaySuiteApp_PICWS_User");
                            string sHttpPwd = infraestructureRepository.GetParameterValue("BlinkaySuiteApp_PICWS_Pwd");
                            string sProv = infraestructureRepository.GetParameterValue("BlinkaySuiteApp_PICWS_Prov");
                            string sWSUrl = infraestructureRepository.GetParameterValue("BlinkaySuiteApp_WS_Url");

                            oThirdPartyLogin.BlinkaySuiteAppSecurityAuthentication(sPICUrl, sHashKey, sHttpUser, sHttpPwd, sProv,
                                                                                   strUsername, sPwd, ref parametersOut, ref oInstallations, ref oInsRoles);

                                                   

                            List<COUNTRIES_REDIRECTION>  oCountryRedirections = infraestructureRepository.GetCountriesRedirectionsGroupByPICURL();


                            foreach (var oCountryRedirection in oCountryRedirections)
                            {

                               oThirdPartyLogin.BlinkaySuiteAppSecurityAuthentication(oCountryRedirection.COURE_COUNTRY_REDIRECTION_PICWS_URL, oCountryRedirection.COURE_COUNTRY_REDIRECTION_PICWS_HASH_KEY, 
                                                                                            oCountryRedirection.COURE_COUNTRY_REDIRECTION_PICWS_HTTP_USER, oCountryRedirection.COURE_COUNTRY_REDIRECTION_PICWS_PASSWORD,
                                                                                            oCountryRedirection.COURE_COUNTRY_REDIRECTION_PICWS_PROVIDER,
                                                                                            strUsername, sPwd, ref parametersOut, ref oInstallations, ref oInsRoles);

                            }




                            if (oInstallations.Count() > 0)
                            {
                                string sAllowedRoles = "";
                                

                                foreach (var oInstallation in oInstallations)
                                {

                                    sAllowedRoles += string.Format("<installation json:Array='true'>");
                                    sAllowedRoles += string.Format("<id>{0}</id>", oInstallation.INS_ID );
                                    sAllowedRoles += string.Format("<couid>{0}</couid>", oInstallation.INS_COU_ID);

                                    if (oInstallation.COUNTRy.COUNTRIES_REDIRECTIONs.Count() == 0)
                                    {
                                        sAllowedRoles += string.Format("<wsurl>{0}</wsurl>", sWSUrl);
                                    }
                                    else
                                    {
                                        sAllowedRoles += string.Format("<wsurl>{0}</wsurl>", oInstallation.COUNTRy.COUNTRIES_REDIRECTIONs.First().COURE_COUNTRY_REDIRECTION_WS_URL);

                                    }

                                    sAllowedRoles += string.Format("<ins_roles xmlns:json='http://james.newtonking.com/projects/json'>");

                                    foreach (string sRole in oInsRoles[oInstallation.INS_ID])
                                        sAllowedRoles += "<role json:Array='true'>" + sRole.Trim() + "</role>";

                                    sAllowedRoles += "</ins_roles>";
                                    sAllowedRoles += "</installation>";
                                }

                                parametersOut["installations"] = sAllowedRoles;

                                xmlOut = GenerateXMLOuput(parametersOut, new List<string> { "installations", "ins_roles" });

                                if (xmlOut.Length == 0)
                                {
                                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                    Logger_AddLogMessage(string.Format("BlinkaySuiteAppLogin::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                }
                                else
                                {
                                    Logger_AddLogMessage(string.Format("BlinkaySuiteAppLogin: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                                }

                            }
                            else
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthentication);
                                Logger_AddLogMessage(string.Format("BlinkaySuiteAppLogin::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            }

                           

                        }
                    }

                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("BlinkaySuiteAppLogin::Error: xmlIn={0}, xmlOut={1}", xmlIn, xmlOut), LogLevels.logERROR);

                }

                if (parametersIn != null)
                {
                    parametersIn.Clear();
                    parametersIn = null;
                }

                if (parametersOut != null)
                {
                    parametersOut.Clear();
                    parametersOut = null;
                }
            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("BlinkaySuiteAppLogin::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
            }
            return xmlOut;
        }

        [WebMethod]
        public string BlinkaySuiteAppLoginJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                //Logger_AddLogMessage(string.Format("QueryLoginJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = BlinkaySuiteAppLogin(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);
                xmlOut = null;
                Logger_AddLogMessage(string.Format("BlinkaySuiteAppLoginJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("BlinkaySuiteAppLoginJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }

        [WebMethod]
        public string GetUsersDataMovements(string xmlIn)
        {
            string xmlOut = "";

            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            xmlOut = TestRedirection(xmlIn, sMethodName);
            if (!string.IsNullOrEmpty(xmlOut))
            {
                return xmlOut;
            }

            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("GetUsersDataMovements: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {
                    if ((parametersIn["UsrVersion"] == null) ||
                        (parametersIn["CouId"] == null))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("GetUsersDataMovements::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                    }
                    else
                    {
                        string strCalculatedHash = CalculateHash(strHashString, strHash);

                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("GetUsersDataMovements::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {

                            parametersOut = new SortedList();
                            parametersOut["r"] = (int)ResultType.Result_OK;

                            long lCurrVersion = -1;                            
                            if (parametersIn["UsrVersion"] != null)
                            {
                                try
                                {
                                    lCurrVersion = Convert.ToInt64(parametersIn["UsrVersion"].ToString());
                                }
                                catch { }
                            }

                            long lCouId = -1;
                            if (parametersIn["CouId"] != null)
                            {
                                try
                                {
                                    lCouId = Convert.ToInt64(parametersIn["CouId"].ToString());
                                }
                                catch { }
                            }

                            string strInsXML = GetUsersSync(lCurrVersion, lCouId);




                            parametersOut["UsrSync"] = strInsXML;                            



                            xmlOut = GenerateXMLOuput(parametersOut, new List<string> { "UsrSync" });

                            if (xmlOut.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("GetUsersDataMovements::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            }
                            else
                            {
                                string trcMsg = PrettyXml(xmlOut);
                                if (trcMsg.Length > 1000)
                                {
                                    StringBuilder sb = new StringBuilder(trcMsg, 0, 1000, 1200);
                                    sb.Append("\r\n...");
                                    sb.Append("\r\n---------------------------------------------------------------------------");
                                    sb.Append("\r\n...");
                                    Logger_AddLogMessage(string.Format("GetUsersDataMovements: {0}", sb.ToString()), LogLevels.logINFO);


                                }
                                else
                                {
                                    Logger_AddLogMessage(string.Format("GetUsersDataMovements: xmlOut={0}", trcMsg), LogLevels.logINFO);
                                }                                
                            }
                        }
                    }
                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("GetUsersDataMovements::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                }
                if (parametersIn != null)
                {
                    parametersIn.Clear();
                    parametersIn = null;
                }

                if (parametersOut != null)
                {
                    parametersOut.Clear();
                    parametersOut = null;
                }


            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("GetUsersDataMovements::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
            }

            return xmlOut;
        }

        [WebMethod]
        public string GetUsersDataMovementsJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                //Logger_AddLogMessage(string.Format("GetUsersDataMovementsJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = GetUsersDataMovements(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);
                xmlOut = null;

                //Logger_AddLogMessage(string.Format("GetUsersDataMovementsJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("GetUsersDataMovementsJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);

            }

            return jsonOut;
        }

        [WebMethod]
        public string CancelParkingOperation(string xmlIn)
        {
            string xmlOut = "";

            string sMethodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            xmlOut = TestRedirection(xmlIn, sMethodName);
            if (!string.IsNullOrEmpty(xmlOut))
            {
                return xmlOut;
            }

            try
            {
                SortedList parametersIn = null;
                SortedList parametersOut = null;
                string strHash = "";
                string strHashString = "";

                Logger_AddLogMessage(string.Format("CancelParkingOperation: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logINFO);

                ResultType rt = FindInputParameters(xmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if ((parametersIn["u"] == null) ||
                        (parametersIn["opeId"] == null)
                        )
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Missing_Input_Parameter);
                        Logger_AddLogMessage(string.Format("CancelParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);

                    }
                    else
                    {
                        string strCalculatedHash = CalculateHash(strHashString, strHash);

                        if (strCalculatedHash != strHash)
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_InvalidAuthenticationHash);
                            Logger_AddLogMessage(string.Format("CancelParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        }
                        else
                        {

                            string sUsername = parametersIn["u"].ToString();
                            decimal dOpeId = Convert.ToDecimal(parametersIn["opeId"].ToString());

                            bool bRefundCharge = false;

                            try
                            {
                                bRefundCharge = (Convert.ToInt32(parametersIn["refund"]) == 1);
                            }
                            catch { bRefundCharge = false; }



                            USER oUser = null;
                            if (customersRepository.GetUserData(ref oUser, sUsername, bRefundCharge))
                            {
                                decimal? dRechargeId = null;
                                int iFinalAmount = 0;
                                string sOpeExternalId1 = "";
                                string sOpeExternalId2 = "";
                                string sOpeExternalId3 = "";
                                decimal dOpeUserId = 0;
                                INSTALLATION oInstallation = null;

                                OPERATION oOperation = null;
                                HIS_OPERATION oHisOperation = null;
                                if (customersRepository.GetOperationData(dOpeId, out oOperation))
                                {
                                    dRechargeId = oOperation.OPE_CUSPMR_ID;
                                    iFinalAmount = oOperation.OPE_FINAL_AMOUNT;
                                    sOpeExternalId1 = oOperation.OPE_EXTERNAL_ID1;
                                    sOpeExternalId2 = oOperation.OPE_EXTERNAL_ID2;
                                    sOpeExternalId3 = oOperation.OPE_EXTERNAL_ID3;
                                    dOpeUserId = oOperation.USER.USR_ID;
                                    oInstallation = oOperation.INSTALLATION;
                                }
                                else if (customersRepository.GetOperationData(dOpeId, out oHisOperation))
                                {
                                    dRechargeId = oHisOperation.OPE_CUSPMR_ID;
                                    iFinalAmount = oHisOperation.OPE_FINAL_AMOUNT;
                                    sOpeExternalId1 = oHisOperation.OPE_EXTERNAL_ID1;
                                    sOpeExternalId2 = oHisOperation.OPE_EXTERNAL_ID2;
                                    sOpeExternalId3 = oHisOperation.OPE_EXTERNAL_ID3;
                                    dOpeUserId = oHisOperation.USER.USR_ID;
                                    oInstallation = oHisOperation.INSTALLATION;
                                }
                                else
                                    rt = ResultType.Result_Error_OperationNotFound;


                                // Check if input user is the same as operation user
                                if (rt == ResultType.Result_OK)
                                {
                                    if (oUser.USR_ID != dOpeUserId)
                                    {
                                        rt = ResultType.Result_Error_Invalid_User;
                                        Logger_AddLogMessage(string.Format("CancelParkingOperation::The operation user differs from the input user (OpeUserId={0})", dOpeUserId), LogLevels.logERROR);
                                    }
                                }

                                ThirdPartyOperation oThirdPartyOperation = null;

                                long lEllapsedTime = 0;                                

                                if (rt == ResultType.Result_OK && !string.IsNullOrEmpty(sOpeExternalId1))
                                {
                                    oThirdPartyOperation = new ThirdPartyOperation();

                                    ConfirmParkWSSignatureType eParkSignatureType = (ConfirmParkWSSignatureType)oInstallation.INS_PARK_CONFIRM_WS_SIGNATURE_TYPE;

                                    switch (eParkSignatureType)
                                    {
                                        case ConfirmParkWSSignatureType.cpst_nocall:
                                            rt = ResultType.Result_OK;
                                            break;

                                        case ConfirmParkWSSignatureType.cpst_standard:
                                            {
                                                rt = oThirdPartyOperation.StandardCancelParking(1, sOpeExternalId1, oInstallation, null, out lEllapsedTime);
                                            }
                                            break;

                                        default:
                                            rt = ResultType.Result_Error_Generic;
                                            break;
                                    }
                                }
                                if (rt == ResultType.Result_OK && !string.IsNullOrEmpty(sOpeExternalId2))
                                {
                                    if (oThirdPartyOperation == null) oThirdPartyOperation = new ThirdPartyOperation();

                                    ConfirmParkWSSignatureType eParkSignatureType = (ConfirmParkWSSignatureType)oInstallation.INS_PARK_CONFIRM_WS2_SIGNATURE_TYPE;

                                    switch (eParkSignatureType)
                                    {
                                        case ConfirmParkWSSignatureType.cpst_nocall:
                                            rt = ResultType.Result_OK;
                                            break;

                                        case ConfirmParkWSSignatureType.cpst_standard:
                                            {
                                                rt = oThirdPartyOperation.StandardCancelParking(2, sOpeExternalId2, oInstallation, null, out lEllapsedTime);                                                
                                            }
                                            break;

                                        default:
                                            rt = ResultType.Result_Error_Generic;
                                            break;
                                    }
                                }
                                if (rt == ResultType.Result_OK && !string.IsNullOrEmpty(sOpeExternalId3))
                                {
                                    if (oThirdPartyOperation == null) oThirdPartyOperation = new ThirdPartyOperation();

                                    ConfirmParkWSSignatureType eParkSignatureType = (ConfirmParkWSSignatureType)oInstallation.INS_PARK_CONFIRM_WS3_SIGNATURE_TYPE;

                                    switch (eParkSignatureType)
                                    {
                                        case ConfirmParkWSSignatureType.cpst_nocall:
                                            rt = ResultType.Result_OK;
                                            break;

                                        case ConfirmParkWSSignatureType.cpst_standard:
                                            {
                                                rt = oThirdPartyOperation.StandardCancelParking(3, sOpeExternalId3, oInstallation, null, out lEllapsedTime);                                                
                                            }
                                            break;

                                        default:
                                            rt = ResultType.Result_Error_Generic;
                                            break;
                                    }
                                }

                                // Make refund if needed
                                if (rt == ResultType.Result_OK)
                                {
                                    if (bRefundCharge)
                                    {
                                        ResultType rtRefund = RefundChargeParkPayment(ref oUser, dOpeId, dRechargeId, true, false, true);
                                        if (rtRefund == ResultType.Result_OK)
                                        {
                                            Logger_AddLogMessage(string.Format("CancelParkingOperation::Payment Refund of {0}", iFinalAmount), LogLevels.logINFO);
                                        }
                                        else
                                        {
                                            Logger_AddLogMessage(string.Format("CancelParkingOperation::Error in Payment Refund: {0}", rtRefund.ToString()), LogLevels.logERROR);
                                        }
                                    }
                                    else
                                    {
                                        if (!customersRepository.DeleteOperation(dOpeId))
                                        {
                                            rt = ResultType.Result_Error_Generic;
                                        }
                                    }
                                }

                            }
                            else
                            {
                                rt = ResultType.Result_Error_Invalid_User;
                            }

                            parametersOut = new SortedList();
                            parametersOut["r"] = Convert.ToInt32(rt).ToString();
                            xmlOut = GenerateXMLOuput(parametersOut);

                            if (xmlOut.Length == 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("CancelParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            }
                            else
                            {
                                Logger_AddLogMessage(string.Format("CancelParkingOperation: xmlOut={0}", PrettyXml(xmlOut)), LogLevels.logINFO);
                            }
                        }
                    }

                }
                else
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("CancelParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                }

                if (parametersIn != null)
                {
                    parametersIn.Clear();
                    parametersIn = null;
                }

                if (parametersOut != null)
                {
                    parametersOut.Clear();
                    parametersOut = null;
                }

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("CancelParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                Logger_AddLogMessage(string.Format("CancelParkingOperation::Error: '{0}'", e.ToString()), LogLevels.logERROR);

            }
            
            return xmlOut;
        }



        [WebMethod]
        public string CancelParkingOperationJSON(string jsonIn)
        {
            string jsonOut = "";
            try
            {
                Logger_AddLogMessage(string.Format("CancelParkingOperationJSON: jsonIn={0}", PrettyJSON(jsonIn)), LogLevels.logINFO);

                XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(jsonIn);

                string strXmlOut = CancelParkingOperation(xmlIn.OuterXml);

                XmlDocument xmlOut = new XmlDocument();
                xmlOut.LoadXml(strXmlOut);
                xmlOut.RemoveChild(xmlOut.FirstChild);
                jsonOut = JsonConvert.SerializeXmlNode(xmlOut);
                xmlOut = null;

                //Logger_AddLogMessage(string.Format("ConfirmParkingOperationJSON: jsonOut={0}", PrettyJSON(jsonOut)), LogLevels.logINFO);

            }
            catch (Exception e)
            {
                jsonOut = GenerateJSONErrorResult(ResultType.Result_Error_Invalid_Input_Parameter);
                Logger_AddLogException(e, string.Format("CancelParkingOperationJSON::Error: jsonIn={0}, jsonOut={1}", PrettyJSON(jsonIn), PrettyJSON(jsonOut)), LogLevels.logERROR);


            }

            return jsonOut;
        }

        #endregion

        #region Private Methods

        private Dictionary<int, List<string>> GetExtraPlatesFromParamsIn(SortedList parametersIn)
        {
            Dictionary<int, List<string>> oExtraPlates = new Dictionary<int, List<string>>();

            int iExtra = 2;
            string sKey = string.Format("p{0}_1", iExtra);
            string sAddPlate = "";
            while (parametersIn[sKey] != null && parametersIn[sKey].ToString() != "" &&
                   NormalizePlate(parametersIn[sKey].ToString()).Length > 0)
            {
                oExtraPlates.Add(iExtra, new List<string>());
                for (int i = 1; i <= 10; i += 1)
                {
                    sKey = string.Format("p{0}_{1}", iExtra, i);
                    if (parametersIn[sKey] != null && parametersIn[sKey].ToString() != "")
                    {
                        sAddPlate = NormalizePlate(parametersIn[sKey].ToString());
                        if (sAddPlate.Length > 0)
                            oExtraPlates[iExtra].Add(parametersIn[sKey].ToString().Trim().Replace(" ", ""));
                    }
                }
                iExtra += 1;
                sKey = string.Format("p{0}_1", iExtra);
            }

            return oExtraPlates;
        }

        private ResultType ConfirmParkingOperationInternal(ref SortedList parametersIn, ref USER oUser, string sSessionID, decimal? dLatitude, decimal? dLongitude,
                                                           string strPlate, List<string> oAdditionalPlates, TARIFF oTariff, GROUP oGroup, STREET_SECTION oStreetSection,
                                                           double dChangeToApply, decimal? dAuthId, DateTime dtSavedInstallationTime, DateTime dtSavedUtcTime, ChargeOperationsType operationType, 
                                                           DateTime dtIni, DateTime dtEnd, int iTime, int iTimeBalUsed, int iQuantity, int iRealQuantity, int iQuantityWithoutBon, string strQPlusVATQs,
                                                           decimal dPercBonus, string sBonusId, string sBonusMarca, int? iBonusType, 
                                                           string strAppVersion, string strPlaceString, int iPostpay, bool bIsShopKeeperOperation, string sBackOfficeUsr, bool bPermitAutomaticRenewal,
                                                           decimal? dBonMlt, decimal? dBonExtMlt, string sVehicleType,
                                                           DateTime? dtExpirationDate, string sAdditionalParams, decimal? dCampaignId, int? indexCampaing,int? iCampaignAmountToSubstract,
                                                           bool bPerformRecharge, int? iTotalRechargeQuantity, bool bConfirmIfRequired,
                                                           bool bSendConfirmParkingMail, int iOSType,
                                                           string xmlIn, string xmlOut,string strMD,string strCAVV,string strECI, string strBSRedsys3DSTransID,
                                                           string strBSRedsys3DSPares,string strBSRedsys3DSCres,string strBSRedsys3DSMethodData, string strMercadoPagoToken,
                                                           string strMPProTransactionId,
                                                           string strMPProReference,
                                                           string strMPProCardHash,
                                                           string strMPProCardReference,
                                                           string strMPProCardScheme,
                                                           string strMPProGatewayDate,
                                                           string strMPProMaskedCardNumber,
                                                           string strMPProExpMonth,
                                                           string strMPProExpYear,
                                                           string strMPProCardType,
                                                           string strMPProDocumentID,
                                                           string strMPProDocumentType,
                                                           string strMPProInstallaments,
                                                           string strMPProCVVLength,
                                                           decimal dSourceApp, bool bPaymentInPerson, CardPayment_Mode eCardPaymentMode, 
                                                           out SortedList parametersOut, out decimal? dRechargeId, out bool bRestoreBalanceInCaseOfRefund)
        {

            ResultType rt = ResultType.Result_OK;
            parametersOut = new SortedList();

            Dictionary<int, int> oDictQs = new Dictionary<int, int>();

            if (!string.IsNullOrEmpty(strQPlusVATQs))
            {
                string[] strTuples = strQPlusVATQs.Split(new char[] { '|' });

                foreach (string strtupla in strTuples)
                {
                    string[] strQs = strtupla.Split(new char[] { ';' });

                    if (strQs.Length == 2)
                    {
                        oDictQs[Convert.ToInt32(strQs[0])] = Convert.ToInt32(strQs[1]);
                    }

                }
            }


            
            parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();

            int iCurrencyChargedQuantity = 0;
            decimal dOperationID = -1;
            string str3dPartyOpNum = "";
            string str3dPartyBaseOpNum = "";
            DateTime? dtIniModified = null;
            DateTime? dtUtcIniModified = null;
            DateTime? dtEndModified = null;
            DateTime? dtUtcEndModified = null;
            bool bInsertNoAnswered = false;
            //decimal? dRechargeId;
            //bool bRestoreBalanceInCaseOfRefund = true;
            int? iBalanceAfterRecharge = null;
            DateTime? dtUTCInsertionDate = null;

            decimal dUsrId = oUser.USR_ID;
            var session = oUser.MOBILE_SESSIONs.Where(r => r.MOSE_SESSIONID == sSessionID && r.MOSE_USR_ID == dUsrId).First();

            int iQT;
            int iQC;
            int iIVA;
            int iQTIVA;

            IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;
            int? iPaymentTypeId = null;
            int? iPaymentSubtypeId = null;
            if (oUser.CUSTOMER_PAYMENT_MEAN != null)
            {
                iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
            }

            int? iTariffType = null;
            if (oTariff != null)
            {
                iTariffType = oTariff.TAR_TYPE;
            }

            bool bExistBon = (iQuantity != iQuantityWithoutBon);

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
            int iPartialBonusFEE;
            int iPartialBonusFEEVAT;

            customersRepository.GetFinantialParams(oUser, oGroup.GRP_INS_ID, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, iPaymentTypeId, iPaymentSubtypeId, operationType, iTariffType,
                                                             out dPercVAT1, out dPercVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode);


            if (iQuantity > 0)
            {

                //m_Log.LogMessage(LogLevels.logERROR,string.Format("ConfirmParkingOperation {0} {1} {2}", iQuantity, oDictQs[iQuantity], eTaxMode));

                if ((oDictQs.ContainsKey(iQuantity)) && (eTaxMode == IsTAXMode.IsNotTaxVATBackward))
                {

                    iQuantity = oDictQs[iQuantity];
                    iTotalQuantity = customersRepository.CalculateFEE(ref iQuantity, dPercVAT1, dPercVAT2, dPercFEE, dPercFEETopped, dFixedFEE, dPercBonus, eTaxMode, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE, out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);

                }
                else
                {
                    iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, dPercFEETopped, dFixedFEE, dPercBonus,
                                                                out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE,
                                                                out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);
                }
            }
            else
            {
                iTotalQuantity = iQuantity;
                iPartialVAT1 = 0;
                iPartialPercFEE = 0;
                iPartialFixedFEE = 0;
                iPartialBonusFEE = 0;
                iPartialPercFEEVAT = 0;
                iPartialFixedFEEVAT = 0;
                iPartialBonusFEEVAT = 0;
            }

            iQT = (iPartialPercFEE - iPartialPercFEEVAT) + (iPartialFixedFEE - iPartialFixedFEEVAT);
            iQC = iPartialBonusFEE - iPartialBonusFEEVAT;
            iIVA = iPartialPercFEEVAT + iPartialFixedFEEVAT - iPartialBonusFEEVAT;
            iQTIVA = iQuantity;


            if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
            {
                iQTIVA += iPartialVAT1;
            }

            int iQTIVAWithoutBon = iQTIVA;
            if (bExistBon)
            {
                int iPartialVAT1_WithoutBon;
                int iPartialPercFEE_WithoutBon;
                int iPartialFixedFEE_WithoutBon;
                int iPartialPercFEEVAT_WithoutBon;
                int iPartialFixedFEEVAT_WithoutBon;
                int iTotalQuantity_WithoutBon;
                int iPartialBonusFEE_WithoutBon;
                int iPartialBonusFEEVAT_WithoutBon;

                int iQT_WithoutBon;
                int iQC_WithoutBon;
                int iIVA_WithoutBon;

                if (iQuantityWithoutBon > 0)
                {
                    if ((oDictQs.ContainsKey(iQuantityWithoutBon)) && (eTaxMode == IsTAXMode.IsNotTaxVATBackward))
                    {

                        iQuantityWithoutBon = oDictQs[iQuantityWithoutBon];
                        iTotalQuantity_WithoutBon = customersRepository.CalculateFEE(ref iQuantityWithoutBon, dPercVAT1, dPercVAT2, dPercFEE, dPercFEETopped, dFixedFEE, dPercBonus, eTaxMode,
                                                                                     out iPartialVAT1_WithoutBon, out iPartialPercFEE_WithoutBon, out iPartialFixedFEE_WithoutBon, out iPartialBonusFEE_WithoutBon, out iPartialPercFEEVAT_WithoutBon, out iPartialFixedFEEVAT_WithoutBon, out iPartialBonusFEEVAT_WithoutBon);

                    }
                    else
                    {
                        iTotalQuantity_WithoutBon = customersRepository.CalculateFEE(iQuantityWithoutBon, dPercVAT1, dPercVAT2, dPercFEE, dPercFEETopped, dFixedFEE, dPercBonus,
                                                                    out iPartialVAT1_WithoutBon, out iPartialPercFEE_WithoutBon, out iPartialFixedFEE_WithoutBon, out iPartialBonusFEE_WithoutBon,
                                                                    out iPartialPercFEEVAT_WithoutBon, out iPartialFixedFEEVAT_WithoutBon, out iPartialBonusFEEVAT_WithoutBon);
                    }
                }
                else
                {
                    iTotalQuantity_WithoutBon = iQuantity;
                    iPartialVAT1_WithoutBon = 0;
                    iPartialPercFEE_WithoutBon = 0;
                    iPartialFixedFEE_WithoutBon = 0;
                    iPartialBonusFEE_WithoutBon = 0;
                    iPartialPercFEEVAT_WithoutBon = 0;
                    iPartialFixedFEEVAT_WithoutBon = 0;
                    iPartialBonusFEEVAT_WithoutBon = 0;
                }

                iQT_WithoutBon = (iPartialPercFEE_WithoutBon - iPartialPercFEEVAT_WithoutBon) + (iPartialFixedFEE_WithoutBon - iPartialFixedFEEVAT_WithoutBon);
                iQC_WithoutBon = iPartialBonusFEE_WithoutBon - iPartialBonusFEEVAT_WithoutBon;
                iIVA_WithoutBon = iPartialPercFEEVAT_WithoutBon + iPartialFixedFEEVAT_WithoutBon - iPartialBonusFEEVAT_WithoutBon;
                iQTIVAWithoutBon = iQuantityWithoutBon;

                if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                {
                    iQTIVAWithoutBon += iPartialVAT1_WithoutBon;
                }
            }

            int iPercFEETopped = Convert.ToInt32(Math.Round(dPercFEETopped, MidpointRounding.AwayFromZero));
            int iFixedFEE = Convert.ToInt32(Math.Round(dFixedFEE, MidpointRounding.AwayFromZero));

            string str3DSURL = null;
            long lEllapsedTime = 0;
            int iWSTimeout = infraestructureRepository.GetRateWSTimeout(oGroup.INSTALLATION.INS_ID);


            rt = ChargeParkingOperation(ref parametersIn, oGroup.INSTALLATION, operationType, strPlate, oAdditionalPlates, dChangeToApply, iQuantity, 
                                        iTime, iRealQuantity, iTimeBalUsed, dtSavedInstallationTime, dtIni, dtEnd, oTariff,
                                        oGroup, oStreetSection, ref oUser, session.MOSE_OS.Value, session.MOSE_ID, dLatitude, dLongitude, strAppVersion, dAuthId,
                                        dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, dPercBonus,
                                        iPartialVAT1, iPartialPercFEE, iPartialFixedFEE, iPartialBonusFEE, iTotalQuantity,
                                        sBonusId, sBonusMarca, iBonusType, strPlaceString, iPostpay, bIsShopKeeperOperation,
                                        iQuantityWithoutBon, (dBonMlt ?? 1), dBonExtMlt, sVehicleType, sBackOfficeUsr, bPermitAutomaticRenewal, dtExpirationDate,
                                        bPerformRecharge, iTotalRechargeQuantity, !bConfirmIfRequired, sAdditionalParams,strMD, strCAVV,strECI,
                                        strBSRedsys3DSTransID,strBSRedsys3DSPares,strBSRedsys3DSCres,strBSRedsys3DSMethodData, strMercadoPagoToken,
                                        strMPProTransactionId,
                                        strMPProReference,
                                        strMPProCardHash,
                                        strMPProCardReference,
                                        strMPProCardScheme,
                                        strMPProGatewayDate,
                                        strMPProMaskedCardNumber,
                                        strMPProExpMonth,
                                        strMPProExpYear,
                                        strMPProCardType,
                                        strMPProDocumentID,
                                        strMPProDocumentType,
                                        strMPProInstallaments,
                                        strMPProCVVLength,
                                        dSourceApp, bPaymentInPerson, eCardPaymentMode,
                                        ref parametersOut,
                                        out iCurrencyChargedQuantity, out dOperationID, out dtUTCInsertionDate, out dRechargeId, out iBalanceAfterRecharge,
                                        out bRestoreBalanceInCaseOfRefund,out str3DSURL, out lEllapsedTime);

            if (rt != ResultType.Result_OK)
            {
                //DeleteConfirmLockInformation(strLockDictionaryString);
                //xmlOut = GenerateXMLErrorResult(rt);
                //Logger_AddLogMessage(string.Format("ConfirmParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                //return xmlOut;

                if (rt == ResultType.Result_3DS_Validation_Needed)
                {
                    parametersOut["ThreeDSURL"] = str3DSURL;
                    CUSTOMER_PAYMENT_MEAN oUserPaymentMean = customersRepository.GetUserPaymentMean(ref oUser, (INSTALLATION)null);
                    parametersOut["cc_provider"] = oUserPaymentMean.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.CPTGC_PROVIDER;

                }
                return rt;
            }

            integraMobile.Domain.CAMPAING oCampaing = null;
            int? iQTIVAWihtBon = null;

            if (dCampaignId != null)
            {
                customersRepository.GetCampaign(dCampaignId.Value, out oCampaing);
                switch ((CampaingShema)oCampaing.CAMP_SCHEMA)
                {
                    case CampaingShema.DiscountCampaignByZoneForDailyFirstParkingWithTimeMoreThanX:
                        {
                            iQTIVAWihtBon = iQTIVA;
                        }
                        break;

                    default:
                        break;

                }
            }

            iWSTimeout -= (int)lEllapsedTime;

            parametersOut["operationid"] = dOperationID;
            ThirdPartyOperation oThirdPartyOperation = null;

            bool? bConfirm1NotNeeded = null;

            if (((oGroup.INSTALLATION.INS_OPT_OPERATIONCONFIRM_MODE ?? 0) == (int)OperationConfirmMode.online || 
                 (oGroup.INSTALLATION.INS_OPT_OPERATIONCONFIRM_MODE ?? 0) == (int)OperationConfirmMode.first_online) && 
                bConfirmIfRequired)
            {
                oThirdPartyOperation = new ThirdPartyOperation();

                ConfirmParkWSSignatureType eParkSignatureType = (ConfirmParkWSSignatureType)oGroup.INSTALLATION.INS_PARK_CONFIRM_WS_SIGNATURE_TYPE;

                // ***
                if (eParkSignatureType == ConfirmParkWSSignatureType.cpst_madridplatform &&
                    oThirdPartyOperation.Madrid2AllowedZone(oGroup, 1))
                    eParkSignatureType = ConfirmParkWSSignatureType.cpst_madrid2platform;
                // ***

                switch (eParkSignatureType)
                {
                    case ConfirmParkWSSignatureType.cpst_nocall:
                        rt = ResultType.Result_OK;
                        break;
                    case ConfirmParkWSSignatureType.cpst_test:
                        break;

                    case ConfirmParkWSSignatureType.cpst_eysa:
                        {
                            rt = oThirdPartyOperation.EysaConfirmParking(1, strPlate, dtUTCInsertionDate.Value, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oTariff.TAR_ID, iQuantity, iTime, dtIni, dtEnd,
                                                                         iQT, iQC, iIVA, sBonusMarca, iBonusType, dLatitude, dLongitude,iWSTimeout,
                                                                         ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                        }
                        break;

                    case ConfirmParkWSSignatureType.cpst_internal:
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                        break;

                    case ConfirmParkWSSignatureType.cpst_standard:
                        {
                            rt = oThirdPartyOperation.StandardConfirmParking(1, strPlate, oAdditionalPlates, dtSavedInstallationTime, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oStreetSection?.STRSE_ID, oTariff.TAR_ID, iQTIVAWithoutBon,
                                                                             iTime, dtIni, dtEnd,dOperationID, strPlaceString, iPostpay, dAuthId, Helpers.BonificationLogic(dBonMlt,dBonExtMlt), dBonExtMlt, iQTIVAWihtBon, iWSTimeout,
                                                                             ref parametersOut, out str3dPartyOpNum, out str3dPartyBaseOpNum, out lEllapsedTime);
                        }
                        break;

                    case ConfirmParkWSSignatureType.cpst_gtechna:
                        {
                            rt = oThirdPartyOperation.GtechnaConfirmParking(1, session.MOSE_ID, strPlate, dtSavedInstallationTime, oGroup.INSTALLATION, oGroup.GRP_ID, oTariff.TAR_ID, iQTIVA, iTime, dtIni, dtEnd,
                                                                                   dOperationID, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                        }
                        break;

                    case ConfirmParkWSSignatureType.cpst_standardmadrid:
                        {
                            //rt = oThirdPartyOperation.MadridPlatformConfirmParking(1, strPlate, dtSavedInstallationTime, dtUTCInsertionDate.Value, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oGroup.GRP_ID, iQTIVA, iTime, dtIni, dtEnd, dOperationID, dAuthId ?? 0,
                            //                                                    /*dBonExtMlt,*/ ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);

                            // Enviar amount només amb la bonificació plataforma (bonext_mlt) i bon_mlt que no sigui de la plataforma 
                            if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                            {
                                iTotalQuantity += iQTIVA;
                            }

                            rt = oThirdPartyOperation.StandardConfirmParking(1, strPlate, oAdditionalPlates, dtSavedInstallationTime, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oStreetSection?.STRSE_ID, oTariff.TAR_ID, 
                                                                             Helpers.ApplyPercentageBonExtMlt(dBonMlt, dBonExtMlt, iTotalQuantity), iTime, dtIni, dtEnd,
                                                                             dOperationID, strPlaceString, iPostpay, dAuthId, Helpers.BonificationLogic(dBonMlt, dBonExtMlt), dBonExtMlt, iQTIVAWihtBon, iWSTimeout,
                                                                             ref parametersOut, out str3dPartyOpNum, out str3dPartyBaseOpNum, out lEllapsedTime);
                        }
                        break;

                    case ConfirmParkWSSignatureType.cpst_madridplatform:
                        {
                            // Enviar amount només amb la bonificació plataforma (bonext_mlt) i bon_mlt que no sigui de la plataforma 
                            if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                            {
                                iTotalQuantity += iQTIVA;
                            }
                            rt = oThirdPartyOperation.MadridPlatformConfirmParking(1, strPlate, dtSavedInstallationTime, dtUTCInsertionDate.Value, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, 
                                                                                  oTariff.TAR_ID, Helpers.ApplyPercentageBonExtMlt(dBonMlt, dBonExtMlt, iTotalQuantity), iTime, dtIni, dtEnd, 
                                                                                  dOperationID, dAuthId ?? 0,/*dBonExtMlt,*/iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                        }
                        break;

                    case ConfirmParkWSSignatureType.cpst_madrid2platform:
                        {
                            // Enviar amount només amb la bonificació plataforma (bonext_mlt) i bon_mlt que no sigui de la plataforma 
                            if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                            {
                                iTotalQuantity += iQTIVA;
                            }
                            rt = oThirdPartyOperation.Madrid2PlatformConfirmParking(1, strPlate, dtSavedInstallationTime, dtUTCInsertionDate.Value, oUser, oGroup.INSTALLATION, oGroup.GRP_ID,
                                                                                  oTariff.TAR_ID, Helpers.ApplyPercentageBonExtMlt(dBonMlt, dBonExtMlt, iTotalQuantity), iTime, dtIni, dtEnd,
                                                                                  dOperationID, dAuthId ?? 0,/*dBonExtMlt,*/iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                        }
                        break;

                    case ConfirmParkWSSignatureType.cpst_bsm:
                        {
                            BSMConfiguration oBSMConfiguration = null;
                            if (!string.IsNullOrEmpty(sAdditionalParams))
                            {
                                try
                                {
                                    oBSMConfiguration = (BSMConfiguration)JsonConvert.DeserializeObject(sAdditionalParams, typeof(BSMConfiguration));
                                }
                                catch (Exception)
                                {
                                    rt = ResultType.Result_Error_Generic;
                                }                                
                            }
                            else
                                rt = ResultType.Result_Error_Generic;

                            if (!dLatitude.HasValue || !dLongitude.HasValue)
                            {                                
                                rt = ResultType.Result_Error_Invalid_Input_Parameter;
                                Logger_AddLogMessage("ConfirmParkingOperationInternal::Gps position required", LogLevels.logERROR);
                            }
                            if (rt == ResultType.Result_OK)
                            {
                                int? iInstallationTimeMinOffset = geograficAndTariffsRepository.GetInstallationUTCOffSetInMinutes(oGroup.GRP_INS_ID);

                                rt = oThirdPartyOperation.BSMStart(1, oGroup.INSTALLATION, strPlate, oBSMConfiguration, dtSavedUtcTime, dLatitude.Value, dLongitude.Value, iQTIVA, 
                                                                   Helpers.BonificationLogic(dBonMlt,dBonExtMlt), dBonExtMlt, iWSTimeout, ref parametersOut, 
                                                                   out str3dPartyOpNum, out str3dPartyBaseOpNum, out dtUtcIniModified, out dtUtcEndModified, out lEllapsedTime, out bInsertNoAnswered);
                                if (rt == ResultType.Result_OK)
                                {
                                    if (iInstallationTimeMinOffset.HasValue)
                                        dtSavedInstallationTime = DateTime.UtcNow.AddMinutes(iInstallationTimeMinOffset.Value);
                                    if (dtUtcIniModified.HasValue)
                                        dtIniModified = geograficAndTariffsRepository.ConvertUTCToInstallationDateTime(oGroup.GRP_INS_ID, dtUtcIniModified.Value);
                                    if (dtUtcEndModified.HasValue)
                                        dtEndModified = geograficAndTariffsRepository.ConvertUTCToInstallationDateTime(oGroup.GRP_INS_ID, dtUtcEndModified.Value);

                                    if (dtIniModified.HasValue && dtIniModified.Value != dtSavedInstallationTime && Math.Abs((dtSavedInstallationTime - dtIniModified.Value).TotalSeconds) < 60)
                                        dtSavedInstallationTime = dtIniModified.Value;
                                }
                            }
                        }
                        break;
                    case ConfirmParkWSSignatureType.cpst_bilbao_integration:
                        {
                            //rt = oThirdPartyOperation.MadridPlatformConfirmParking(1, strPlate, dtSavedInstallationTime, dtUTCInsertionDate.Value, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oGroup.GRP_ID, iQTIVA, iTime, dtIni, dtEnd, dOperationID, dAuthId ?? 0,
                            //                                                    /*dBonExtMlt,*/ ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);

                            // Enviar amount només amb la bonificació plataforma (bonext_mlt) i bon_mlt que no sigui de la plataforma 
                            if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                            {
                                iTotalQuantity += iQTIVA;
                            }

                            rt = oThirdPartyOperation.BilbaoIntegrationConfirmParking(1, strPlate, oAdditionalPlates, dtSavedInstallationTime, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oTariff.TAR_ID,
                                                                             Helpers.ApplyPercentageBonExtMlt(dBonMlt, dBonExtMlt, iTotalQuantity), iTime, dtIni, dtEnd,
                                                                             dOperationID, strPlaceString, iPostpay, dAuthId, Helpers.BonificationLogic(dBonMlt, dBonExtMlt), dBonExtMlt, iWSTimeout,
                                                                             ref parametersOut, out str3dPartyOpNum, out str3dPartyBaseOpNum, out lEllapsedTime);
                        }
                        break;
                    case ConfirmParkWSSignatureType.cpst_SIR:
                        {
                            if ((oTariff.TAR_BEHAVIOR ?? 0) == (int)ParkingMode.StartStop ||
                                (oTariff.TAR_BEHAVIOR ?? 0) == (int)ParkingMode.StartStopHybrid)
                            {
                                bConfirm1NotNeeded = true;
                            }
                            else
                            {
                                rt = ResultType.Result_Error_Generic;
                                CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge = null;
                                if (dRechargeId.HasValue)
                                {
                                    if (customersRepository.GetRechargeData(ref oUser, dRechargeId.Value, out oRecharge))
                                    {
                                        if (oRecharge != null && 
                                            (oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG?.CPTGC_PROVIDER == (int)PaymentMeanCreditCardProviderType.pmccpMercadoPago ||
                                            oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG?.CPTGC_PROVIDER == (int)PaymentMeanCreditCardProviderType.pmccpMercadoPagoPro) && 
                                            oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION != null)
                                        {
                                            SIRResponse<SIRCobroResponse> oSIRResponse = null;
                                            rt = oThirdPartyOperation.SIRConfirmPayment(1, oGroup.INSTALLATION, oRecharge.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG.MERCADOPAGO_CONFIGURATION.MEPACON_CUENTAMP, dtSavedInstallationTime, oRecharge.CUSPMR_OP_REFERENCE, Convert.ToInt32(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED), iWSTimeout,
                                                                                        out str3dPartyOpNum, out oSIRResponse, out lEllapsedTime);
                                        }
                                    }
                                }
                            }
                            
                        }
                        break;

                    default:
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                        break;
                }
            }

            if (rt != ResultType.Result_OK)
            {
                try
                {
                    if (parametersOut.IndexOfKey("autorecharged") >= 0)
                        parametersOut.Remove("autorecharged");
                    if (parametersOut.IndexOfKey("newbal") >= 0)
                        parametersOut.Remove("newbal");
                    if (parametersOut.IndexOfKey("new_time_bal") >= 0)
                        parametersOut.Remove("newtime_bal");
                }
                catch { }

                ResultType rtRefund = RefundChargeParkPayment(ref oUser, dOperationID, dRechargeId, bRestoreBalanceInCaseOfRefund, bInsertNoAnswered);
                if (rtRefund == ResultType.Result_OK)
                {
                    Logger_AddLogMessage(string.Format("ConfirmParkingOperation::Payment Refund of {0}", iCurrencyChargedQuantity), LogLevels.logERROR);
                }
                else
                {
                    Logger_AddLogMessage(string.Format("ConfirmParkingOperation::Error in Payment Refund: {0}", rtRefund.ToString()), LogLevels.logERROR);
                }


                //DeleteConfirmLockInformation(strLockDictionaryString);
                //xmlOut = GenerateXMLErrorResult(rt);
                //Logger_AddLogMessage(string.Format("ConfirmParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                //return xmlOut;
                return rt;
            }
            else
            {
                parametersOut["utc_offset"] = geograficAndTariffsRepository.GetInstallationUTCOffSetInMinutes(oGroup.INSTALLATION.INS_ID);

                if (str3dPartyOpNum.Length > 0 || str3dPartyBaseOpNum.Length > 0 ||
                    dtIniModified.HasValue || dtUtcIniModified.HasValue || dtEndModified.HasValue || dtUtcEndModified.HasValue)
                {
                    customersRepository.UpdateThirdPartyIDAndDatesInParkingOperation(ref oUser, 1, dOperationID, str3dPartyOpNum, str3dPartyBaseOpNum, dtIniModified, dtUtcIniModified, dtEndModified, dtUtcEndModified);
                }

                if (bConfirm1NotNeeded.HasValue && bConfirm1NotNeeded.Value)
                {
                    customersRepository.UpdateThirdPartyConfirmedInParkingOperation(ref oUser, dOperationID, false, true, true);
                }

                parametersOut["d"] = dtSavedInstallationTime.ToString("HHmmssddMMyy");

                parametersOut["di"] = dtIni.ToString("HHmmssddMMyy");
                if (dtIniModified.HasValue)
                    parametersOut["di"] = dtIniModified.Value.ToString("HHmmssddMMyy");

                ulong ulAppVersion = AppUtilities.AppVersion(strAppVersion);
                if (ulAppVersion < _VERSION_3_6)
                {
                    bool bShowOutOfRateTime = (operationType == ChargeOperationsType.ParkingOperation && dtSavedInstallationTime < (dtIniModified ?? dtIni));
                    parametersOut["showOutOfRateTimeMsg"] = (bShowOutOfRateTime ? 1 : 0);
                }
                else
                {
                    parametersOut["NoticeChargesNow"] = oTariff.TAR_NOTICE_CHARGES_NOW;
                    String sLiteral=string.Empty;

                    if ((oTariff.TAR_NOTICE_CHARGES_NOW != (int)NoticChargesNow.NotShowMessage) && (operationType == ChargeOperationsType.ParkingOperation && dtSavedInstallationTime < (dtIniModified ?? dtIni)))
                    {
                        DateTime? dFecha = (dtIniModified ?? dtIni);
                        string sCulture = "en-US";
                        if (session != null && !string.IsNullOrEmpty(session.MOSE_CULTURE_LANG))
                        {
                            sCulture = session.MOSE_CULTURE_LANG;
                        }

                        if (oTariff.TAR_NOTICE_CHARGES_NOW_LIT_ID.HasValue)
                        {
                            sLiteral = infraestructureRepository.GetLiteral(oTariff.TAR_NOTICE_CHARGES_NOW_LIT_ID.Value, sCulture);
                            sLiteral = sLiteral.Replace("#", dFecha.Value.ToShortTimeString() + " | " + dFecha.Value.ToShortDateString());
                        }
                    }
                    else
                    {
                        parametersOut["NoticeChargesNow"] = (int)NoticChargesNow.NotShowMessage;
                    }
                     parametersOut["NoticeChargesNowLiteral"]=sLiteral;

                }
                if (dtEndModified.HasValue && dtEndModified.Value != dtEnd)
                {
                    parametersOut["dexp"] = dtEndModified.Value.ToString("HHmmssddMMyy");
                }

                PaymentSuscryptionType eUserSuscryptionType = (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE.Value;
                if (oGroup.INSTALLATION.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG != null)
                    eUserSuscryptionType = PaymentSuscryptionType.pstPerTransaction;

                if (dRechargeId != null)
                {
                    customersRepository.ConfirmRecharge(ref oUser, dRechargeId.Value, false);

                    if (eUserSuscryptionType == PaymentSuscryptionType.pstPrepay)
                    {
                        try
                        {
                            CUSTOMER_PAYMENT_MEANS_RECHARGE oRecharge = null;
                            if (customersRepository.GetRechargeData(ref oUser, dRechargeId.Value, out oRecharge))
                            {
                                string culture = oUser.USR_CULTURE_LANG;
                                CultureInfo ci = new CultureInfo(culture);
                                Thread.CurrentThread.CurrentUICulture = ci;
                                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                                integraMobile.WS.Properties.Resource.Culture = ci;


                                iQuantity = oRecharge.CUSPMR_AMOUNT;
                                dPercVAT1 = oRecharge.CUSPMR_PERC_VAT1 ?? 0;
                                dPercVAT2 = oRecharge.CUSPMR_PERC_VAT2 ?? 0;
                                dPercFEE = oRecharge.CUSPMR_PERC_FEE ?? 0;
                                dPercFEETopped = (int)(oRecharge.CUSPMR_PERC_FEE_TOPPED ?? 0);
                                dFixedFEE = (int)(oRecharge.CUSPMR_FIXED_FEE ?? 0);

                                iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, dPercFEETopped, dFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialPercFEEVAT, out iPartialFixedFEEVAT);

                                iPercFEETopped = Convert.ToInt32(Math.Round(dPercFEETopped, MidpointRounding.AwayFromZero));
                                iFixedFEE = Convert.ToInt32(Math.Round(dFixedFEE, MidpointRounding.AwayFromZero));


                                int iQFEE = Convert.ToInt32(Math.Round(iQuantity * dPercFEE, MidpointRounding.AwayFromZero));
                                if (dPercFEETopped > 0 && iQFEE > dPercFEETopped) iQFEE = iPercFEETopped;
                                iQFEE += iFixedFEE;
                                int iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT;
                                int iQSubTotal = iQuantity + iQFEE;

                                int iLayout = 0;
                                if (iQFEE != 0 || iQVAT != 0)
                                {
                                    OPERATOR oOperator = customersRepository.GetDefaultOperator();
                                    if (oOperator != null) iLayout = oOperator.OPR_FEE_LAYOUT;
                                }


                                string sLayoutSubtotal = "";
                                string sLayoutTotal = "";

                                string sCurIsoCode = infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oRecharge.CUSPMR_CUR_ID));
                                string strSourceAppEmailPrefix = GetEmailSourceAppEmailPrefix(dSourceApp);


                                if (iLayout == 2)
                                {
                                    sLayoutSubtotal = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Email_LayoutSubtotal"),
                                                                    string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQSubTotal) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                                    (oRecharge.CUSPMR_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oRecharge.CUSPMR_PERC_VAT1 * 100) : "") +
                                                                    (oRecharge.CUSPMR_PERC_VAT2 != 0 && oRecharge.CUSPMR_PERC_VAT1 != oRecharge.CUSPMR_PERC_VAT2 ? string.Format("{0:0.00#}%", oRecharge.CUSPMR_PERC_VAT2 * 100) : ""),
                                                                    string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                                }
                                else if (iLayout == 1)
                                {
                                    sLayoutTotal = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Email_LayoutTotal"),
                                                                 string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQuantity) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                                 string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQFEE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                                 (oRecharge.CUSPMR_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oRecharge.CUSPMR_PERC_VAT1 * 100) : "") +
                                                                 (oRecharge.CUSPMR_PERC_VAT2 != 0 && oRecharge.CUSPMR_PERC_VAT1 != oRecharge.CUSPMR_PERC_VAT2 ? string.Format("{0:0.00#}%", oRecharge.CUSPMR_PERC_VAT2 * 100) : ""),
                                                                 string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                                }

                                string strRechargeEmailSubject = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmAutomaticRecharge_EmailHeader");
                                /*
                                    ID: {0}<br>
                                 *  Fecha de recarga: {1:HH:mm:ss dd/MM/yyyy}<br>
                                 *  Cantidad Recargada: {2} 
                                 */
                                string strRechargeEmailBody = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmRecharge_EmailBody"),
                                    oRecharge.CUSPMR_ID,
                                    oRecharge.CUSPMR_DATE,
                                    string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(oRecharge.CUSPMR_TOTAL_AMOUNT_CHARGED) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode),
                                                                  infraestructureRepository.GetCurrencySymbolOrIsoCode(Convert.ToInt32(oRecharge.CUSPMR_CUR_ID))),
                                    string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) + "} {1}", Convert.ToDouble(iBalanceAfterRecharge) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                                        infraestructureRepository.GetCurrencySymbolOrIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                    ConfigurationManager.AppSettings["EmailSignatureURL"],
                                    ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                    sLayoutSubtotal, sLayoutTotal,
                                    GetEmailFooter(ref oUser, dSourceApp));

                                SendEmail(ref oUser, strRechargeEmailSubject, strRechargeEmailBody, dSourceApp);

                            }
                        }
                        catch { }
                    }
                }

                if (eUserSuscryptionType == PaymentSuscryptionType.pstPrepay)
                {
                    int iDiscountValue = 0;
                    string strDiscountCurrencyISOCode = "";

                    try
                    {
                        iDiscountValue = Convert.ToInt32(ConfigurationManager.AppSettings["SuscriptionType1_DiscountValue"]);
                        strDiscountCurrencyISOCode = ConfigurationManager.AppSettings["SuscriptionType1_DiscountCurrency"];
                    }
                    catch
                    { }


                    if (iDiscountValue > 0)
                    {
                        double dDiscountChangeApplied = 0;
                        double dDiscountChangeFee = 0;
                        int iCurrencyDiscountQuantity = ChangeQuantityFromCurToUserCur(iDiscountValue, strDiscountCurrencyISOCode, oUser,
                                                                                        out dDiscountChangeApplied, out dDiscountChangeFee);

                        if (iCurrencyDiscountQuantity > 0)
                        {
                            DateTime? dtUTCTime = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oGroup.GRP_INS_ID, dtSavedInstallationTime.AddSeconds(1));

                            customersRepository.AddDiscountToParkingOperation(ref oUser, session.MOSE_OS.Value, PaymentSuscryptionType.pstPrepay,
                                    dtSavedInstallationTime.AddSeconds(1), dtUTCTime.Value, iDiscountValue,
                                    infraestructureRepository.GetCurrencyFromIsoCode(strDiscountCurrencyISOCode),
                                    oUser.CURRENCy.CUR_ID, dDiscountChangeApplied, dDiscountChangeFee, iCurrencyDiscountQuantity, dOperationID,
                                    dLatitude, dLongitude, strAppVersion);

                            parametersOut["newbal"] = oUser.USR_BALANCE;

                        }
                    }

                }


            }


            if (Convert.ToInt32(parametersOut["r"]) == Convert.ToInt32(ResultType.Result_OK))
            {
                customersRepository.DeleteSessionOperationInfo(ref oUser, sSessionID, strPlate);
                int amountDiscountExt;
                decimal amountDiscount = iQuantityWithoutBon - iQuantity;
                if ((dBonExtMlt.HasValue)&&(dBonMlt!=1))
                {
                    int iOpeAmountBonification;
                    //En el caso de que sea Madrid obtenemos el descuento que se hace
                    if (iQuantityWithoutBon > 0)
                    {
                        iOpeAmountBonification = iQuantityWithoutBon;
                    }
                    else
                    {
                        iOpeAmountBonification = iTotalQuantity;
                    }
                    amountDiscountExt = Helpers.ApplyPercentageBonExtMlt(dBonMlt, dBonExtMlt, iOpeAmountBonification);
                    if (dBonMlt.HasValue)
                    {
                        Logger_AddLogMessage(string.Format("ConfirmParkingOperationInternal::ApplyPercentageBonExtMlt::  dBonMlt={0}, dBonExtMlt={1}, iQuantityWithoutBon={2}, amountDiscount={3}, amountDiscountExt={4}",
                                                            (dBonMlt.HasValue ? dBonMlt.Value : 0), (dBonExtMlt.HasValue ? dBonExtMlt.Value : 0), iOpeAmountBonification, amountDiscount, amountDiscountExt), LogLevels.logINFO);

                        decimal? dBonificationCampainReal=null;
                        decimal? dBonificationCampain = Helpers.BonificationLogic(dBonMlt, dBonExtMlt);

                        Logger_AddLogMessage(string.Format("ConfirmParkingOperationInternal::ApplyPercentageBonExtMlt::  dBonificationCampainReal={0}, dBonificationCampain={1}",
                                   dBonificationCampainReal, dBonificationCampain), LogLevels.logINFO);

                        
                        if (dBonificationCampain.HasValue)
                        {
                            dBonificationCampainReal = Math.Abs(dBonificationCampain.Value * 100 - 100);
                        }
                        if (dBonificationCampainReal.HasValue)
                        {
                            //Al descuento de madrid se aplica el de la campaña
                            decimal resultado = ((amountDiscountExt * dBonificationCampainReal.Value) / 100);
                            //el resultado se guarda en la tabla de campaña CAMP_TOTAL_DISCOUNT_DELIVERED
                            amountDiscountExt = Convert.ToInt32(Math.Round(resultado, MidpointRounding.AwayFromZero));
                            Logger_AddLogMessage(string.Format("ConfirmParkingOperationInternal::ApplyPercentageBonCampaing::  dBonificationCampain={0}, dBonificationCampainReal={1}, amountDiscountExt={2}",
                                                            (dBonificationCampain.HasValue ? dBonificationCampain.Value : 0), (dBonificationCampainReal.HasValue ? dBonificationCampainReal.Value : 0), amountDiscountExt), LogLevels.logINFO);
                        }
                        Logger_AddLogMessage(string.Format("ConfirmParkingOperationInternal::ApplyPercentageBonExtMlt::  dBonificationCampainReal={0}, dBonificationCampain={1}, amountDiscount={2} amountDiscountExt={3}",
                                dBonificationCampainReal, dBonificationCampain, amountDiscount, amountDiscountExt), LogLevels.logINFO);
                    }
                    

                    amountDiscount = amountDiscountExt;



                }
                else
                {
                    if ((dBonMlt.HasValue)&&(dBonMlt!=1))
                    {
                        int iOpeAmountBonification;
                        if (iQuantityWithoutBon > 0)
                        {
                            iOpeAmountBonification = iQuantityWithoutBon;
                        }
                        else
                        {
                            iOpeAmountBonification = iTotalQuantity;
                        }
                        //amountDiscount = Convert.ToInt32(Math.Round((iQuantityWithoutBon * dBonMlt.Value), MidpointRounding.AwayFromZero));
                        //amountDiscount = iQuantityWithoutBon * iQuantity;

                        //Al descuento de madrid se aplica el de la campaña
                        decimal resultado = ((iOpeAmountBonification * dBonMlt.Value) / 100);
                        //el resultado se guarda en la tabla de campaña CAMP_TOTAL_DISCOUNT_DELIVERED
                        amountDiscountExt = Convert.ToInt32(Math.Round(resultado, MidpointRounding.AwayFromZero));
                        Logger_AddLogMessage(string.Format("ConfirmParkingOperationInternal::ApplyPercentageBonCampaing::  dBonificationCampain={0},  amountDiscount={1}",
                                                        (dBonMlt.HasValue ? dBonMlt.Value : 0),  amountDiscountExt), LogLevels.logINFO);

                    }
                }



                if (dCampaignId != null)
                {
                    switch ((CampaingShema)oCampaing.CAMP_SCHEMA)
                    {
                        case CampaingShema.DiscountCampaignByZone:
                            {

                                bool bApplyCampaingSchemaFour = SaveCampaingSchemaFour(oUser, oGroup, dOperationID, dtSavedInstallationTime, amountDiscount, oCampaing, indexCampaing.Value, ref xmlOut);
                                if (bApplyCampaingSchemaFour)
                                {                                    
                                    customersRepository.UpdateOperationByCampaign(dOperationID, oCampaing.CAMP_ID, Convert.ToDecimal(indexCampaing.Value));
                                }
                            }
                            break;

                        case CampaingShema.DiscountCampaignByZoneForFirstParking:
                            {
                                DateTime? dtUTCTime = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oGroup.GRP_INS_ID, dtSavedInstallationTime);
                                if (SaveCampaingSchemaSix(oUser, oGroup, session, dOperationID, dtSavedInstallationTime, dtUTCTime.Value, amountDiscount, oCampaing, ref xmlOut))
                                {
                                    customersRepository.UpdateOperationByCampaign(dOperationID, oCampaing.CAMP_ID, null);
                                }

                            }
                            break;

                        case CampaingShema.DiscountCampaignByZoneForDailyFirstParkingWithTimeMoreThanX:
                            {
                                DateTime? dtUTCTime = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oGroup.GRP_INS_ID, dtSavedInstallationTime);
                                if (SaveCampaingSchemaEight(oUser, oGroup, session, dOperationID, dtSavedInstallationTime, dtUTCTime.Value, iTime,iCampaignAmountToSubstract, oCampaing, ref xmlOut))
                                {
                                    customersRepository.UpdateOperationByCampaign(dOperationID, oCampaing.CAMP_ID, null);
                                }

                            }
                            break;


                        default:
                            break;

                    }
                    
                   
                }


                bool bSendEmail = true;
                if (parametersIn["sendemail"] != null)
                {
                    try
                    {
                        bSendEmail = (Convert.ToInt32(parametersIn["sendemail"].ToString()) == 1);
                    }
                    catch { }
                }

                if ((bSendConfirmParkingMail)&&(bSendEmail))
                    Email_ConfirmParking(ref oUser, dOperationID);
            }



            if ((oGroup.INSTALLATION.INS_OPT_OPERATIONCONFIRM_MODE ?? 0) == (int)OperationConfirmMode.online && bConfirmIfRequired)
            {

                if (Convert.ToInt32(parametersOut["r"]) == Convert.ToInt32(ResultType.Result_OK))
                {
                    bool bConfirmed1 = true;
                    bool bConfirmed2 = true;
                    bool bConfirmed3 = true;


                    if (oGroup.INSTALLATION.INS_PARK_CONFIRM_WS2_SIGNATURE_TYPE.HasValue)
                    {
                        iWSTimeout -= (int)lEllapsedTime;
                        SortedList parametersOutTemp = new SortedList();
                        ResultType rt2= ResultType.Result_OK;

                        ConfirmParkWSSignatureType eParkSignatureType = (ConfirmParkWSSignatureType)oGroup.INSTALLATION.INS_PARK_CONFIRM_WS2_SIGNATURE_TYPE;

                        // ***
                        if (eParkSignatureType == ConfirmParkWSSignatureType.cpst_madridplatform &&
                            oThirdPartyOperation.Madrid2AllowedZone(oGroup, 2))
                            eParkSignatureType = ConfirmParkWSSignatureType.cpst_madrid2platform;
                        // ***

                        switch (eParkSignatureType)
                        {

                            case ConfirmParkWSSignatureType.cpst_nocall:
                                rt2 = ResultType.Result_OK;
                                break;

                            case ConfirmParkWSSignatureType.cpst_eysa:
                                {

                                  
                                    rt2 = oThirdPartyOperation.EysaConfirmParking(2, strPlate, dtUTCInsertionDate.Value, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oTariff.TAR_ID, iQuantity, iTime, dtIni, dtEnd,
                                                                                 iQT, iQC, iIVA, sBonusMarca, iBonusType, dLatitude, dLongitude,iWSTimeout,
                                                                                 ref parametersOutTemp, out str3dPartyOpNum, out lEllapsedTime);
                                }
                                break;


                            case ConfirmParkWSSignatureType.cpst_standard:
                                {

                                    rt2 = oThirdPartyOperation.StandardConfirmParking(2, strPlate, oAdditionalPlates, dtSavedInstallationTime, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oStreetSection?.STRSE_ID, oTariff.TAR_ID, iQTIVAWithoutBon, 
                                                                                     iTime, dtIni, dtEnd,dOperationID, strPlaceString, iPostpay, dAuthId, Helpers.BonificationLogic(dBonMlt, dBonExtMlt), dBonExtMlt, iQTIVAWihtBon, iWSTimeout, 
                                                                                     ref parametersOutTemp, out str3dPartyOpNum, out str3dPartyBaseOpNum, out lEllapsedTime);
                                }
                                break;

                            case ConfirmParkWSSignatureType.cpst_gtechna:
                                {

                                    rt2 = oThirdPartyOperation.GtechnaConfirmParking(2, session.MOSE_ID, strPlate, dtSavedInstallationTime, oGroup.INSTALLATION, oGroup.GRP_ID, oTariff.TAR_ID, iQTIVA, iTime, dtIni, dtEnd,
                                                                                           dOperationID, iWSTimeout, ref parametersOutTemp, out str3dPartyOpNum, out lEllapsedTime);
                                }
                                break;

                            case ConfirmParkWSSignatureType.cpst_standardmadrid:
                                {


                                    //rt = oThirdPartyOperation.MadridPlatformConfirmParking(2, strPlate, dtSavedInstallationTime, dtUTCInsertionDate.Value, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oTariff.TAR_ID, iQTIVA, iTime, dtIni, dtEnd, dOperationID, dAuthId ?? 0,
                                    //                                                    ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                                    // Enviar amount només amb la bonificació plataforma (bonext_mlt) i bon_mlt que no sigui de la plataforma 
                                    if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                                    {
                                        iTotalQuantity += iQTIVA;
                                    }
                                    rt2 = oThirdPartyOperation.StandardConfirmParking(2, strPlate, oAdditionalPlates, dtSavedInstallationTime, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oStreetSection?.STRSE_ID, oTariff.TAR_ID, 
                                                                                     Helpers.ApplyPercentageBonExtMlt(dBonMlt, dBonExtMlt, iTotalQuantity), iTime, dtIni, dtEnd,dOperationID, strPlaceString, 
                                                                                     iPostpay, dAuthId, Helpers.BonificationLogic(dBonMlt, dBonExtMlt), dBonExtMlt, iQTIVAWihtBon, iWSTimeout, ref parametersOut, 
                                                                                     out str3dPartyOpNum, out str3dPartyBaseOpNum, out lEllapsedTime);
                                }
                                break;

                            case ConfirmParkWSSignatureType.cpst_madridplatform:
                                {
                                    // Enviar amount només amb la bonificació plataforma (bonext_mlt) i bon_mlt que no sigui de la plataforma 
                                    if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                                    {
                                        iTotalQuantity += iQTIVA;
                                    }
                                    rt2 = oThirdPartyOperation.MadridPlatformConfirmParking(2, strPlate, dtSavedInstallationTime, dtUTCInsertionDate.Value, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oTariff.TAR_ID, 
                                                                                          Helpers.ApplyPercentageBonExtMlt(dBonMlt, dBonExtMlt, iTotalQuantity), iTime, dtIni, dtEnd, dOperationID, dAuthId ?? 0,
                                                                                          /*dBonExtMlt,*/ iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                                }
                                break;

                            case ConfirmParkWSSignatureType.cpst_madrid2platform:
                                {
                                    // Enviar amount només amb la bonificació plataforma (bonext_mlt) i bon_mlt que no sigui de la plataforma 
                                    if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                                    {
                                        iTotalQuantity += iQTIVA;
                                    }
                                    rt2 = oThirdPartyOperation.Madrid2PlatformConfirmParking(2, strPlate, dtSavedInstallationTime, dtUTCInsertionDate.Value, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oTariff.TAR_ID,
                                                                                            Helpers.ApplyPercentageBonExtMlt(dBonMlt, dBonExtMlt, iTotalQuantity), iTime, dtIni, dtEnd, dOperationID, dAuthId ?? 0,
                                                                                            /*dBonExtMlt,*/ iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                                }
                                break;

                            case ConfirmParkWSSignatureType.cpst_bsm:
                                rt2 = ResultType.Result_Error_Generic;
                                break;

                            default:
                                break;
                        }



                        if (rt2 != ResultType.Result_OK)
                        {
                            bConfirmed2 = false;
                            Logger_AddLogMessage(string.Format("ConfirmParkingOperation::Error in WS 2 Confirmation"), LogLevels.logWARN);
                        }
                        else
                        {
                            if (str3dPartyOpNum.Length > 0 || str3dPartyBaseOpNum.Length > 0)
                            {
                                customersRepository.UpdateThirdPartyIDInParkingOperation(ref oUser, 2, dOperationID, str3dPartyOpNum, str3dPartyBaseOpNum);
                            }

                        }
                    }


                    if (oGroup.INSTALLATION.INS_PARK_CONFIRM_WS3_SIGNATURE_TYPE.HasValue)
                    {
                        ResultType rt2 = ResultType.Result_OK;
                        SortedList parametersOutTemp = new SortedList();
                        iWSTimeout -= (int)lEllapsedTime;

                        ConfirmParkWSSignatureType eParkSignatureType = (ConfirmParkWSSignatureType)oGroup.INSTALLATION.INS_PARK_CONFIRM_WS3_SIGNATURE_TYPE;

                        // ***
                        if (eParkSignatureType == ConfirmParkWSSignatureType.cpst_madridplatform &&
                            oThirdPartyOperation.Madrid2AllowedZone(oGroup, 3))
                            eParkSignatureType = ConfirmParkWSSignatureType.cpst_madrid2platform;
                        // ***

                        switch (eParkSignatureType)
                        {
                            case ConfirmParkWSSignatureType.cpst_nocall:
                                rt2 = ResultType.Result_OK;
                                break;

                            case ConfirmParkWSSignatureType.cpst_eysa:
                                {

                                    rt2 = oThirdPartyOperation.EysaConfirmParking(3, strPlate, dtUTCInsertionDate.Value, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oTariff.TAR_ID, iQuantity, iTime, dtIni, dtEnd,
                                                                                 iQT, iQC, iIVA, sBonusMarca, iBonusType, dLatitude, dLongitude, iWSTimeout,
                                                                                 ref parametersOutTemp, out str3dPartyOpNum, out lEllapsedTime);
                                }
                                break;


                            case ConfirmParkWSSignatureType.cpst_standard:
                                {


                                    rt2 = oThirdPartyOperation.StandardConfirmParking(3, strPlate, oAdditionalPlates, dtSavedInstallationTime, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oStreetSection?.STRSE_ID, oTariff.TAR_ID, iQTIVAWithoutBon, 
                                                                                     iTime, dtIni, dtEnd,dOperationID, strPlaceString, iPostpay, dAuthId, Helpers.BonificationLogic(dBonMlt, dBonExtMlt), dBonExtMlt, iQTIVAWihtBon, iWSTimeout,
                                                                                     ref parametersOutTemp, out str3dPartyOpNum, out str3dPartyBaseOpNum, out lEllapsedTime);
                                }
                                break;


                            case ConfirmParkWSSignatureType.cpst_gtechna:
                                {

                                    rt2 = oThirdPartyOperation.GtechnaConfirmParking(3, session.MOSE_ID, strPlate, dtSavedInstallationTime, oGroup.INSTALLATION, oGroup.GRP_ID, oTariff.TAR_ID, iQTIVA, iTime, dtIni, dtEnd,
                                                                                    dOperationID, iWSTimeout, ref parametersOutTemp, out str3dPartyOpNum, out lEllapsedTime);
                                }
                                break;

                            case ConfirmParkWSSignatureType.cpst_standardmadrid:
                                {
                                    //rt = oThirdPartyOperation.MadridPlatformConfirmParking(3, strPlate, dtSavedInstallationTime, dtUTCInsertionDate.Value, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oTariff.TAR_ID, iQTIVA, iTime, dtIni, dtEnd, dOperationID, dAuthId ?? 0,
                                    //                                                    ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                                    // Enviar amount només amb la bonificació plataforma (bonext_mlt) i bon_mlt que no sigui de la plataforma 
                                    if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                                    {
                                        iTotalQuantity += iQTIVA;
                                    }
                                    rt2 = oThirdPartyOperation.StandardConfirmParking(3, strPlate, oAdditionalPlates, dtSavedInstallationTime, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oStreetSection?.STRSE_ID, oTariff.TAR_ID, 
                                                                                     Helpers.ApplyPercentageBonExtMlt(dBonMlt, dBonExtMlt, iTotalQuantity), iTime, dtIni, dtEnd,
                                                                                     dOperationID, strPlaceString, iPostpay, dAuthId, Helpers.BonificationLogic(dBonMlt, dBonExtMlt), dBonExtMlt, iQTIVAWihtBon, iWSTimeout,
                                                                                     ref parametersOut, out str3dPartyOpNum, out str3dPartyBaseOpNum, out lEllapsedTime);
                                }
                                break;

                            case ConfirmParkWSSignatureType.cpst_madridplatform:
                                {
                                    // Enviar amount només amb la bonificació plataforma (bonext_mlt) i bon_mlt que no sigui de la plataforma 
                                    if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                                    {
                                        iTotalQuantity += iQTIVA;
                                    }
                                    rt2 = oThirdPartyOperation.MadridPlatformConfirmParking(3, strPlate, dtSavedInstallationTime, dtUTCInsertionDate.Value, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oTariff.TAR_ID, 
                                                                                           Helpers.ApplyPercentageBonExtMlt(dBonMlt, dBonExtMlt, iTotalQuantity), iTime, dtIni, dtEnd, dOperationID, dAuthId ?? 0,
                                                                                           iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                                }
                                break;

                            case ConfirmParkWSSignatureType.cpst_madrid2platform:
                                {
                                    // Enviar amount només amb la bonificació plataforma (bonext_mlt) i bon_mlt que no sigui de la plataforma 
                                    if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                                    {
                                        iTotalQuantity += iQTIVA;
                                    }
                                    rt2 = oThirdPartyOperation.Madrid2PlatformConfirmParking(3, strPlate, dtSavedInstallationTime, dtUTCInsertionDate.Value, oUser, oGroup.INSTALLATION, oGroup.GRP_ID, oTariff.TAR_ID,
                                                                                             Helpers.ApplyPercentageBonExtMlt(dBonMlt, dBonExtMlt, iTotalQuantity), iTime, dtIni, dtEnd, dOperationID, dAuthId ?? 0,
                                                                                             iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                                }
                                break;

                            case ConfirmParkWSSignatureType.cpst_bsm:
                                rt2 = ResultType.Result_Error_Generic;
                                break;

                            default:
                                break;
                        }



                        if (rt2 != ResultType.Result_OK)
                        {
                            bConfirmed3 = false;
                            Logger_AddLogMessage(string.Format("ConfirmParkingOperation::Error in WS 3 Confirmation"), LogLevels.logWARN);
                        }
                        else
                        {
                            if (str3dPartyOpNum.Length > 0 || str3dPartyBaseOpNum.Length > 0)
                            {
                                customersRepository.UpdateThirdPartyIDInParkingOperation(ref oUser, 3, dOperationID, str3dPartyOpNum, str3dPartyBaseOpNum);
                            }

                        }
                    }

                    if ((!bConfirmed2) || (!bConfirmed3))
                    {
                        customersRepository.UpdateThirdPartyConfirmedInParkingOperation(ref oUser, dOperationID, bConfirmed1, bConfirmed2, bConfirmed3);
                    }
                }
            }

            if (oGroup != null)
            {
                DateTime? dInsDateTime = geograficAndTariffsRepository.getInstallationDateTime(oGroup.GRP_INS_ID);

                if (dInsDateTime.HasValue)
                {
                    parametersOut["cityDatetime"] = dInsDateTime.Value.ToString("HHmmssddMMyy");
                }
            }

            if (rt == ResultType.Result_OK)
            {
                //ConfirmParkingOperation
                decimal? dIdOperation = Helpers.ValidateInputParameterToDecimal(parametersOut, "operationid");
                Campaing(oUser, dLatitude, dLongitude, iOSType, dIdOperation, null, oGroup, xmlIn, xmlOut, dSourceApp);
            }

            return rt;
        }


        //private decimal? BonificationLogic(decimal? bonMlt, decimal? bonExtMlt)
        //{
        //    decimal? dBonMlt = null;
        //    if (bonMlt.HasValue && bonExtMlt.HasValue)
        //    {
        //        dBonMlt = bonMlt / bonExtMlt;
        //        if (dBonMlt == 1)
        //        {
        //            dBonMlt = null; ;
        //        }
        //    }
        //    else if (bonMlt.HasValue && !bonExtMlt.HasValue)
        //    {
        //        dBonMlt = bonMlt;
        //    }
        //    return dBonMlt;
        //}

        //private int ApplyPercentageBonExtMlt(decimal? bonMlt, decimal? bonExtMlt, int totalAmount)
        //{
        //    decimal? dBonMltTemp = BonificationLogic(bonMlt, bonExtMlt);
        //    if (dBonMltTemp.HasValue)
        //    {
        //        if (bonExtMlt.HasValue)
        //        {
        //            return Convert.ToInt32(Math.Round((totalAmount * bonExtMlt.Value), MidpointRounding.AwayFromZero));
        //        }
                
        //    }
        //    return totalAmount;
        //}

        private bool Email_ConfirmParking(ref USER oUser, decimal dOperationID, string strEmailRecipient=null)
        {
            bool bRet = false;
            string strParkingEmailBody = string.Empty;
            try
            {
                OPERATION oParkOp = null;
                
                if (customersRepository.GetOperationData(dOperationID, out oParkOp))
                {
                    string culture = oUser.USR_CULTURE_LANG;
                    CultureInfo ci = new CultureInfo(culture);
                    Thread.CurrentThread.CurrentUICulture = ci;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                    integraMobile.WS.Properties.Resource.Culture = ci;


                    decimal dSourceApp = oParkOp.OPE_SOAPP_ID.Value;
                    int iQuantity = oParkOp.OPE_AMOUNT;
                    decimal dPercVAT1 = oParkOp.OPE_PERC_VAT1 ?? 0;
                    decimal dPercVAT2 = oParkOp.OPE_PERC_VAT2 ?? 0;
                    decimal dPercFEE = oParkOp.OPE_PERC_FEE ?? 0;
                    decimal dPercBonus = oParkOp.OPE_PERC_BONUS ?? 0;
                    decimal dPercFEETopped = (int)(oParkOp.OPE_PERC_FEE_TOPPED ?? 0);
                    decimal dFixedFEE = (int)(oParkOp.OPE_FIXED_FEE ?? 0);

                    int iPartialVAT1;
                    int iPartialPercFEE;
                    int iPartialFixedFEE;
                    int iPartialPercFEEVAT;
                    int iPartialFixedFEEVAT;                    
                    int iPartialBonusFEE;
                    int iPartialBonusFEEVAT;

                    //iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialPercFEEVAT, out iPartialFixedFEEVAT);
                    int iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, dPercFEETopped, dFixedFEE, dPercBonus,
                                                                      out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE,
                                                                      out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);


                    int iPercFEETopped = Convert.ToInt32(Math.Round(dPercFEETopped, MidpointRounding.AwayFromZero));
                    int iFixedFEE = Convert.ToInt32(Math.Round(dFixedFEE, MidpointRounding.AwayFromZero));

                    int iQFEE = Convert.ToInt32(Math.Round(iQuantity * dPercFEE, MidpointRounding.AwayFromZero));
                    if (dPercFEETopped > 0 && iQFEE > dPercFEETopped) iQFEE = iPercFEETopped;
                    iQFEE += iFixedFEE;
                    int iQBonus = iPartialBonusFEE - iPartialBonusFEEVAT;
                    int iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT - iPartialBonusFEEVAT;
                    int iQSubTotal = iQuantity + iQFEE;

                    int iLayout = 0;
                    if (dPercBonus == 0)
                    {
                        if (iQFEE != 0 || iQVAT != 0)
                        {
                            iLayout = oParkOp.INSTALLATION.INS_FEE_LAYOUT;
                        }
                        
                    }
                    else
                        iLayout = 3;

                    string sEmailType = "";
                    if (oParkOp.TARIFF != null)
                    {
                        int iMailTypeBehavior = (oParkOp.TARIFF.TAR_BEHAVIOR ?? 0);
                        if (iMailTypeBehavior == (int)ParkingMode.StartStopHybrid) iMailTypeBehavior = (int)ParkingMode.StartStop;

                        sEmailType = string.Format("_{0}_{1}", oParkOp.TARIFF.TAR_TYPE, iMailTypeBehavior);
                    }

                    string sLayoutSubtotal = "";
                    string sLayoutTotal = "";

                    string strSpaceSection = "";

                    string strSourceAppEmailPrefix = GetEmailSourceAppEmailPrefix(dSourceApp);

                    if (!string.IsNullOrEmpty(oParkOp.OPE_SPACE_STRING))
                    {
                        strSpaceSection = ResourceExtension.GetLiteral(string.Format(strSourceAppEmailPrefix+"ConfirmParking{0}_EmailBody_SpaceSection", sEmailType));
                        if (string.IsNullOrEmpty(strSpaceSection))
                            strSpaceSection = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmParking_EmailBody_SpaceSection");
                        strSpaceSection = string.Format(strSpaceSection, oParkOp.OPE_SPACE_STRING);
                    }

                    string sCurIsoCode = infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oParkOp.OPE_AMOUNT_CUR_ID));

                    if (iLayout == 2)
                    {
                        sLayoutSubtotal = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Email_LayoutSubtotal"),
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQSubTotal) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                        (oParkOp.OPE_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oParkOp.OPE_PERC_VAT1 * 100) : "") +
                                                        (oParkOp.OPE_PERC_VAT2 != 0 && oParkOp.OPE_PERC_VAT1 != oParkOp.OPE_PERC_VAT2 ? string.Format("{0:0.00#}%", oParkOp.OPE_PERC_VAT2 * 100) : ""),
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                    }
                    else if (iLayout == 1)
                    {
                        sLayoutTotal = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Email_LayoutTotal"),
                                                     string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQuantity) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                     string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQFEE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                     (oParkOp.OPE_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oParkOp.OPE_PERC_VAT1 * 100) : "") +
                                                     (oParkOp.OPE_PERC_VAT2 != 0 && oParkOp.OPE_PERC_VAT1 != oParkOp.OPE_PERC_VAT2 ? string.Format("{0:0.00#}%", oParkOp.OPE_PERC_VAT2 * 100) : ""),
                                                     string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                    }


                    string strParkingEmailSubject = ResourceExtension.GetLiteral(string.Format(strSourceAppEmailPrefix+"ConfirmParking{0}_EmailHeader", sEmailType));
                    if (string.IsNullOrEmpty(strParkingEmailSubject))
                        strParkingEmailSubject = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmParking_EmailHeader");

                    /*
                        * ID: {0}<br>
                        * Matr&iacute;cula: {1}<br>
                        * Ciudad: {2}<br>
                        * Zona: {3}<br>
                        * Tarifa: {4}<br>
                        * Fecha de emisi&ocuate;: {5:HH:mm:ss dd/MM/yyyy}<br>
                        * Aparcamiento Comienza fecha:  {6:dd MMM yyyy}<br><b>
                        * Aparcamiento Finaliza fecha:  {7:dd MMM yyyy}</b><br>
                        * Cantidad Pagada: {8} 
                        * Aparcamiento Comienza hora:  {6:HH:mm:ss}<br><b>
                        * Aparcamiento Finaliza hora:  {7:HH:mm:ss}</b><br>
                        */
                    INSTALLATION oInstallation = geograficAndTariffsRepository.GetSuperInstallation(oParkOp.INSTALLATION.INS_ID);
                    if (oInstallation == null) oInstallation = oParkOp.INSTALLATION;

                    string sParkingEmailPlates = ResourceExtension.GetLiteral(string.Format(strSourceAppEmailPrefix+"ConfirmParking{0}_EmailBody_Plates", sEmailType));
                    if (string.IsNullOrEmpty(sParkingEmailPlates))
                        sParkingEmailPlates = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmParking_EmailBody_Plates");
                    if (string.IsNullOrEmpty(sParkingEmailPlates))
                        sParkingEmailPlates = "{1}";

                    string sPlates = oParkOp.USER_PLATE.USRP_PLATE;
                    if (oParkOp.USER_PLATE1 != null) sPlates += string.Format("<br>{0}", oParkOp.USER_PLATE1.USRP_PLATE);
                    if (oParkOp.USER_PLATE2 != null) sPlates += string.Format("<br>{0}", oParkOp.USER_PLATE2.USRP_PLATE);
                    if (oParkOp.USER_PLATE3 != null) sPlates += string.Format("<br>{0}", oParkOp.USER_PLATE3.USRP_PLATE);
                    if (oParkOp.USER_PLATE4 != null) sPlates += string.Format("<br>{0}", oParkOp.USER_PLATE4.USRP_PLATE);
                    if (oParkOp.USER_PLATE5 != null) sPlates += string.Format("<br>{0}", oParkOp.USER_PLATE5.USRP_PLATE);
                    if (oParkOp.USER_PLATE6 != null) sPlates += string.Format("<br>{0}", oParkOp.USER_PLATE6.USRP_PLATE);
                    if (oParkOp.USER_PLATE7 != null) sPlates += string.Format("<br>{0}", oParkOp.USER_PLATE7.USRP_PLATE);
                    if (oParkOp.USER_PLATE8 != null) sPlates += string.Format("<br>{0}", oParkOp.USER_PLATE8.USRP_PLATE);
                    if (oParkOp.USER_PLATE9 != null) sPlates += string.Format("<br>{0}", oParkOp.USER_PLATE9.USRP_PLATE);

                    sPlates = string.Format(sParkingEmailPlates, "", sPlates);

                    strParkingEmailBody = ResourceExtension.GetLiteral(string.Format(strSourceAppEmailPrefix+"ConfirmParking{0}_EmailBody", sEmailType));
                    if (string.IsNullOrEmpty(strParkingEmailBody))
                        strParkingEmailBody = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmParking_EmailBody");
                    string sParkingEmailBalance = ResourceExtension.GetLiteral(string.Format(strSourceAppEmailPrefix+"Confirm{0}_EmailBody_Balance", sEmailType));
                    if (string.IsNullOrEmpty(sParkingEmailBalance))
                        sParkingEmailBalance = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Confirm_EmailBody_Balance");

                    int iLayoutCampaign = customersRepository.GetLayoutCampaignOperationByOperationId(dOperationID);
                    decimal? dDiscount = null;
                    string sLayoutDiscountCampaign = string.Empty;
                    
                    if (iLayoutCampaign != -1)
                    {
                        //**************************************************************************************
                        //En caso de que aplica una campaña a la operación se debe mostrar el descuento aplicado
                        //**************************************************************************************
                        iLayout = iLayoutCampaign;
                        dDiscount = customersRepository.GetDiscountCampaignOperationByOperationId(dOperationID);
                        string sVerificacionLayaout = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Email_LayoutDiscountCampaign");
                        if(string.IsNullOrEmpty(sVerificacionLayaout))
                        {
                            m_Log.LogMessage(LogLevels.logERROR, "Error:: Email_ConfirmParking -->  No Found: Email_LayoutDiscountCampaign.html");
                        }
                        sLayoutDiscountCampaign = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Email_LayoutDiscountCampaign"), ConvertToDecimalToStringDiscountCampaign(dDiscount.Value));
                    }

                    string sCurIsoCode2 = oParkOp.CURRENCy.CUR_ISO_CODE;
                    string sCurIsoCode3 = oParkOp.CURRENCy1.CUR_ISO_CODE;

                    string sGroupDesc = oParkOp.GROUP.GRP_DESCRIPTION;
                    if (oParkOp.STREET_SECTION != null && !string.IsNullOrEmpty(oParkOp.STREET_SECTION.STRSE_DESCRIPTION))
                        sGroupDesc = string.Format("{0}, {1}", oParkOp.STREET_SECTION.STRSE_DESCRIPTION, sGroupDesc); 

                    strParkingEmailBody = string.Format(strParkingEmailBody,
                                                        oParkOp.OPE_ID,/*{0}*/
                                                        sPlates,/*{1}*/
                                                        oInstallation.INS_DESCRIPTION,/*{2}*/
                                                        sGroupDesc,/*{3}*/
                                                        oParkOp.TARIFF.TAR_DESCRIPTION,/*{4}*/
                                                        oParkOp.OPE_DATE,/*{5}*/
                                                        oParkOp.OPE_INIDATE,/*{6}*/
                                                        oParkOp.OPE_ENDDATE,/*{7}*/
                                                        (oParkOp.OPE_AMOUNT_CUR_ID == oParkOp.OPE_BALANCE_CUR_ID ?
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode2) + "} {1}", Convert.ToDouble(oParkOp.OPE_TOTAL_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode2), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode2)) :
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode2) + "} {1} / {2:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode3) + "} {3}", Convert.ToDouble(oParkOp.OPE_TOTAL_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode2), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode2),
                                                                                                        Convert.ToDouble(oParkOp.OPE_FINAL_AMOUNT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode3), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode3))),
                        /*{8}*/
                                                        (oParkOp.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay || oUser.USR_BALANCE > 0) ?
                                                                string.Format(sParkingEmailBalance, string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) + "} {1}",
                                                                            Convert.ToDouble(oUser.USR_BALANCE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                                                            infraestructureRepository.GetCurrencySymbolOrIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)))) : "",
                        /*{9}*/
                                                        ConfigurationManager.AppSettings["EmailSignatureURL"],/*{10}*/
                                                        ConfigurationManager.AppSettings["EmailSignatureGraphic"],/*{11}*/
                                                        sLayoutSubtotal,/*{12}*/
                                                        sLayoutTotal,/*{13}*/
                                                        strSpaceSection,/*{14}*/
                                                        GetEmailFooter(ref oInstallation,dSourceApp),/*{15}*/
                                                        GetEmailInvoiceHeader(ref oInstallation, dSourceApp),/*{16}*/
                                                        sLayoutDiscountCampaign/*{17}*/                                                        
                                                        );


                    if (string.IsNullOrEmpty(strEmailRecipient))
                    {
                        bRet = SendEmail(ref oUser, strParkingEmailSubject, strParkingEmailBody, dSourceApp);
                    }
                    else
                    {
                        bRet = SendEmail(strEmailRecipient, strParkingEmailSubject, strParkingEmailBody, dSourceApp);
                    }
                }
            }
            catch(Exception ex)
            {
                string error = string.Format("Error:: OperationId: {0} - UserId: {1} - NameEmail{2}-  Exception: {3}", dOperationID, oUser.USR_ID, strParkingEmailBody , ex.Message);
                m_Log.LogMessage(LogLevels.logERROR, string.Format("Error:: Email_ConfirmParking - {0}", error));
            }

            return bRet;
        }

        private bool Email_ConfirmMultiParking(ref USER oUser, decimal dRelOperationID)
        {
            bool bRet = false;
            string strParkingEmailBody = "";

            try
            {                
                List<OPERATION> oParkOps = null;
                if (customersRepository.GetRelatedOperationsData(dRelOperationID, out oParkOps))
                {
                    string culture = oUser.USR_CULTURE_LANG;
                    CultureInfo ci = new CultureInfo(culture);
                    Thread.CurrentThread.CurrentUICulture = ci;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                    integraMobile.WS.Properties.Resource.Culture = ci;


                    oParkOps = oParkOps.OrderBy(t => t.OPE_ID).ToList();

                    int iTotalQuantity = 0;
                    int iTotalQFEE = 0;                    
                    int iTotalQVAT = 0;
                    int iTotalQSubTotal = 0;
                    int iTotalAmount = 0;
                    int iTotalFinalAmount = 0;

                    foreach (var oParkOp in oParkOps)
                    {
                        
                        int iQuantity = oParkOp.OPE_AMOUNT;
                        decimal dPercVAT1 = oParkOp.OPE_PERC_VAT1 ?? 0;
                        decimal dPercVAT2 = oParkOp.OPE_PERC_VAT2 ?? 0;
                        decimal dPercFEE = oParkOp.OPE_PERC_FEE ?? 0;
                        decimal dPercBonus = oParkOp.OPE_PERC_BONUS ?? 0;
                        decimal dPercFEETopped = (int)(oParkOp.OPE_PERC_FEE_TOPPED ?? 0);
                        decimal dFixedFEE = (int)(oParkOp.OPE_FIXED_FEE ?? 0);

                        int iPartialVAT1;
                        int iPartialPercFEE;
                        int iPartialFixedFEE;
                        int iPartialPercFEEVAT;
                        int iPartialFixedFEEVAT;
                        int iPartialBonusFEE;
                        int iPartialBonusFEEVAT;

                        //iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialPercFEEVAT, out iPartialFixedFEEVAT);
                        int iTQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, dPercFEETopped, dFixedFEE, dPercBonus,
                                                                          out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE,
                                                                          out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);

                        int iPercFEETopped = Convert.ToInt32(Math.Round(dPercFEETopped, MidpointRounding.AwayFromZero));
                        int iFixedFEE = Convert.ToInt32(Math.Round(dFixedFEE, MidpointRounding.AwayFromZero));

                        int iQFEE = Convert.ToInt32(Math.Round(iQuantity * dPercFEE, MidpointRounding.AwayFromZero));
                        if (dPercFEETopped > 0 && iQFEE > dPercFEETopped) iQFEE = iPercFEETopped;
                        iQFEE += iFixedFEE;
                        int iQBonus = iPartialBonusFEE - iPartialBonusFEEVAT;
                        int iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT - iPartialBonusFEEVAT;
                        int iQSubTotal = iQuantity + iQFEE;

                        iTotalQuantity += iQuantity;
                        iTotalQFEE += iQFEE;
                        iTotalQVAT += iQVAT;
                        iTotalQSubTotal += iQSubTotal;
                        iTotalAmount += (oParkOp.OPE_TOTAL_AMOUNT ?? oParkOp.OPE_AMOUNT);
                        iTotalFinalAmount += oParkOp.OPE_FINAL_AMOUNT;
                    }

                    OPERATION oLastParkOp = oParkOps.Last();
                    decimal dSourceApp = oLastParkOp.OPE_SOAPP_ID.Value;
                    
                    int iLayout = 0;
                    //if (dPercBonus == 0)
                    if ((oLastParkOp.OPE_PERC_BONUS ?? 0) == 0)
                    {
                        if (iTotalQFEE != 0 || iTotalQVAT != 0)
                        {
                            iLayout = oLastParkOp.INSTALLATION.INS_FEE_LAYOUT;
                        }
                    }
                    else
                        iLayout = 3;

                    string sEmailType = "";
                    if (oLastParkOp.TARIFF != null)
                        sEmailType = string.Format("_{0}", oLastParkOp.TARIFF.TAR_TYPE);

                    string sLayoutSubtotal = "";
                    string sLayoutTotal = "";

                    string strSpaceSection = "";

                    string strSourceAppEmailPrefix = GetEmailSourceAppEmailPrefix(dSourceApp);


                    if (!string.IsNullOrEmpty(oLastParkOp.OPE_SPACE_STRING))
                    {
                        strSpaceSection = ResourceExtension.GetLiteral(string.Format(strSourceAppEmailPrefix+"ConfirmParking{0}_EmailBody_SpaceSection", sEmailType));
                        if (string.IsNullOrEmpty(strSpaceSection))
                            strSpaceSection = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmParking_EmailBody_SpaceSection");
                        strSpaceSection = string.Format(strSpaceSection, oLastParkOp.OPE_SPACE_STRING);
                    }

                    string sCurIsoCode = infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oLastParkOp.OPE_AMOUNT_CUR_ID));

                    if (iLayout == 2)
                    {
                        sLayoutSubtotal = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Email_LayoutSubtotal"),
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iTotalQSubTotal) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                        (oLastParkOp.OPE_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oLastParkOp.OPE_PERC_VAT1 * 100) : "") +
                                                        (oLastParkOp.OPE_PERC_VAT2 != 0 && oLastParkOp.OPE_PERC_VAT1 != oLastParkOp.OPE_PERC_VAT2 ? string.Format("{0:0.00#}%", oLastParkOp.OPE_PERC_VAT2 * 100) : ""),
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iTotalQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                    }
                    else if (iLayout == 1)
                    {
                        sLayoutTotal = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Email_LayoutTotal"),
                                                     string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iTotalQuantity) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                     string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iTotalQFEE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                     (oLastParkOp.OPE_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oLastParkOp.OPE_PERC_VAT1 * 100) : "") +
                                                     (oLastParkOp.OPE_PERC_VAT2 != 0 && oLastParkOp.OPE_PERC_VAT1 != oLastParkOp.OPE_PERC_VAT2 ? string.Format("{0:0.00#}%", oLastParkOp.OPE_PERC_VAT2 * 100) : ""),
                                                     string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iTotalQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                    }


                    string strParkingEmailSubject = ResourceExtension.GetLiteral(string.Format(strSourceAppEmailPrefix+"ConfirmParking{0}_EmailHeader", sEmailType));
                    if (string.IsNullOrEmpty(strParkingEmailSubject))
                        strParkingEmailSubject = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmParking_EmailHeader");

                    /*
                        * ID: {0}<br>
                        * Matr&iacute;cula: {1}<br>
                        * Ciudad: {2}<br>
                        * Zona: {3}<br>
                        * Tarifa: {4}<br>
                        * Fecha de emisi&ocuate;: {5:HH:mm:ss dd/MM/yyyy}<br>
                        * Aparcamiento Comienza fecha:  {6:dd MMM yyyy}<br><b>
                        * Aparcamiento Finaliza fecha:  {7:dd MMM yyyy}</b><br>
                        * Cantidad Pagada: {8} 
                        * Aparcamiento Comienza hora:  {16:HH:mm:ss}<br><b>
                        * Aparcamiento Finaliza hora:  {17:HH:mm:ss}</b><br>
                        */
                    INSTALLATION oInstallation = geograficAndTariffsRepository.GetSuperInstallation(oLastParkOp.INSTALLATION.INS_ID);
                    if (oInstallation == null) oInstallation = oLastParkOp.INSTALLATION;

                    string sPlates = "";
                    string sParkingEmailPlates = ResourceExtension.GetLiteral(string.Format(strSourceAppEmailPrefix+"ConfirmParking{0}_EmailBody_Plates", sEmailType));
                    if (string.IsNullOrEmpty(sParkingEmailPlates))
                        sParkingEmailPlates = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmParking_EmailBody_Plates");
                    if (string.IsNullOrEmpty(sParkingEmailPlates))
                        sParkingEmailPlates = "{1}";

                    int iParkingIndex = 1;
                    foreach (var oParkOp in oParkOps)
                    {
                        string sOpPlates = oParkOp.USER_PLATE.USRP_PLATE;
                        if (oParkOp.USER_PLATE1 != null) sOpPlates += string.Format("<br>{0}", oParkOp.USER_PLATE1.USRP_PLATE);
                        if (oParkOp.USER_PLATE2 != null) sOpPlates += string.Format("<br>{0}", oParkOp.USER_PLATE2.USRP_PLATE);
                        if (oParkOp.USER_PLATE3 != null) sOpPlates += string.Format("<br>{0}", oParkOp.USER_PLATE3.USRP_PLATE);
                        if (oParkOp.USER_PLATE4 != null) sOpPlates += string.Format("<br>{0}", oParkOp.USER_PLATE4.USRP_PLATE);
                        if (oParkOp.USER_PLATE5 != null) sOpPlates += string.Format("<br>{0}", oParkOp.USER_PLATE5.USRP_PLATE);
                        if (oParkOp.USER_PLATE6 != null) sOpPlates += string.Format("<br>{0}", oParkOp.USER_PLATE6.USRP_PLATE);
                        if (oParkOp.USER_PLATE7 != null) sOpPlates += string.Format("<br>{0}", oParkOp.USER_PLATE7.USRP_PLATE);
                        if (oParkOp.USER_PLATE8 != null) sOpPlates += string.Format("<br>{0}", oParkOp.USER_PLATE8.USRP_PLATE);
                        if (oParkOp.USER_PLATE9 != null) sOpPlates += string.Format("<br>{0}", oParkOp.USER_PLATE9.USRP_PLATE);

                        //if (!string.IsNullOrEmpty(sPlates)) sPlates += "<br>";
                        sPlates += string.Format(sParkingEmailPlates, " " + iParkingIndex.ToString(), sOpPlates);

                        iParkingIndex += 1;
                    }

                    strParkingEmailBody = ResourceExtension.GetLiteral(string.Format(strSourceAppEmailPrefix+"ConfirmParking{0}_EmailBody", sEmailType));
                    if (string.IsNullOrEmpty(strParkingEmailBody))
                        strParkingEmailBody = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmParking_EmailBody");
                    string sParkingEmailBalance = ResourceExtension.GetLiteral(string.Format(strSourceAppEmailPrefix+"Confirm{0}_EmailBody_Balance", sEmailType));
                    if (string.IsNullOrEmpty(sParkingEmailBalance))
                        sParkingEmailBalance = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Confirm_EmailBody_Balance");


                    string sCurIsoCode2 = oLastParkOp.CURRENCy.CUR_ISO_CODE;
                    string sCurIsoCode3 = oLastParkOp.CURRENCy1.CUR_ISO_CODE;

                    string sGroupDesc = oLastParkOp.GROUP.GRP_DESCRIPTION;
                    if (oLastParkOp.STREET_SECTION != null && !string.IsNullOrEmpty(oLastParkOp.STREET_SECTION.STRSE_DESCRIPTION))
                        sGroupDesc = string.Format("{0}, {1}", oLastParkOp.STREET_SECTION.STRSE_DESCRIPTION, sGroupDesc);

                    strParkingEmailBody = string.Format(strParkingEmailBody,
                                                        oLastParkOp.OPE_ID,
                                                        sPlates,
                                                        oInstallation.INS_DESCRIPTION,
                                                        sGroupDesc,
                                                        oLastParkOp.TARIFF.TAR_DESCRIPTION,
                                                        oLastParkOp.OPE_DATE,
                                                        oLastParkOp.OPE_INIDATE,
                                                        oLastParkOp.OPE_ENDDATE,
                                                        (oLastParkOp.OPE_AMOUNT_CUR_ID == oLastParkOp.OPE_BALANCE_CUR_ID ?
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode2) + "} {1}", Convert.ToDouble(iTotalAmount) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode2), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode2)) :
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode2) + "} {1} / {2:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode3) + "} {3}", Convert.ToDouble(iTotalAmount) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode2), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode2),
                                                                                                        Convert.ToDouble(iTotalFinalAmount) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode3), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode3))),
                                                        (oLastParkOp.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay || oUser.USR_BALANCE > 0) ?
                                                                string.Format(sParkingEmailBalance, string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) + "} {1}",
                                                                            Convert.ToDouble(oUser.USR_BALANCE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                                                            infraestructureRepository.GetCurrencySymbolOrIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)))) : "",
                                                        ConfigurationManager.AppSettings["EmailSignatureURL"],
                                                        ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                                                        sLayoutSubtotal,
                                                        sLayoutTotal,
                                                        strSpaceSection,
                                                        GetEmailFooter(ref oInstallation,dSourceApp),
                                                        GetEmailInvoiceHeader(ref oInstallation, dSourceApp));


                    bRet = SendEmail(ref oUser, strParkingEmailSubject, strParkingEmailBody, dSourceApp);
                }
            }
            catch(Exception ex) 
            {
                string error = string.Format("Error:: OperationId: {0} - UserId: {1} - NameEmail{2}-  Exception: {3}", dRelOperationID, oUser.USR_ID, strParkingEmailBody, ex.Message);
                m_Log.LogMessage(LogLevels.logERROR, string.Format("Error:: Email_ConfirmParking - {0}", error));
            }

            return bRet;
        }

        private bool Email_ConfirmUnParking(USER oUser, decimal dOperationID, int? iQuantityRem = null, bool bAutomaticStop = false)
        {
            bool bRet = false;
            string strUnParkingEmailBody = "";

            try
            {
                OPERATION oParkOp = null;
                if (customersRepository.GetOperationData(dOperationID, out oParkOp))
                {
                    if (oUser == null)
                        oUser = oParkOp.USER;



                    decimal dSourceApp = oParkOp.OPE_SOAPP_ID.Value;

                    string culture = oUser.USR_CULTURE_LANG;
                    CultureInfo ci = new CultureInfo(culture);
                    Thread.CurrentThread.CurrentUICulture = ci;
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
                    integraMobile.WS.Properties.Resource.Culture = ci;


                    double dChangeToApply = Convert.ToDouble(oParkOp.OPE_CHANGE_APPLIED);

                    int iQuantity;
                    if (!iQuantityRem.HasValue)
                        iQuantity = oParkOp.OPE_AMOUNT;
                    else
                        iQuantity = iQuantityRem.Value;
                    decimal dPercVAT1 = oParkOp.OPE_PERC_VAT1 ?? 0;
                    decimal dPercVAT2 = oParkOp.OPE_PERC_VAT2 ?? 0;
                    decimal dPercFEE = oParkOp.OPE_PERC_FEE ?? 0;
                    int iPercFEETopped = (int)(oParkOp.OPE_PERC_FEE_TOPPED ?? 0);
                    int iFixedFEE = (int)(oParkOp.OPE_FIXED_FEE ?? 0);
                    decimal dPercBonus = oParkOp.OPE_PERC_BONUS ?? 0;

                    int iPartialVAT1;
                    int iPartialPercFEE;
                    int iPartialFixedFEE;
                    int iPartialPercFEEVAT;
                    int iPartialFixedFEEVAT;
                    int iPartialBonusFEE;
                    int iPartialBonusFEEVAT;

                    int iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, dPercBonus, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE, out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);

                    int iQFEE = Convert.ToInt32(Math.Round(iQuantity * dPercFEE, MidpointRounding.AwayFromZero));
                    if (iPercFEETopped > 0 && iQFEE > iPercFEETopped) iQFEE = iPercFEETopped;
                    iQFEE += iFixedFEE;
                    int iQBonus = iPartialBonusFEE - iPartialBonusFEEVAT;
                    int iQVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT - iPartialBonusFEEVAT;
                    int iQSubTotal = iQuantity + iQFEE - iQBonus;

                    int iLayout = 0;
                    if (dPercBonus == 0)
                    {
                        if (iQFEE != 0 || iQVAT != 0)
                        {
                            iLayout = oParkOp.INSTALLATION.INS_FEE_LAYOUT;
                        }
                    }
                    else
                        iLayout = 3;

                    ParkingMode eParkingMode = ParkingMode.Normal;
                    string sEmailType = "";
                    if (oParkOp.TARIFF != null)
                    {
                        int iMailTypeBehavior = (oParkOp.TARIFF.TAR_BEHAVIOR ?? 0);
                        if (iMailTypeBehavior == (int)ParkingMode.StartStopHybrid) iMailTypeBehavior = (int)ParkingMode.StartStop;

                        sEmailType = string.Format("_{0}_{1}", oParkOp.TARIFF.TAR_TYPE, iMailTypeBehavior);
                        eParkingMode = (ParkingMode)(oParkOp.TARIFF.TAR_BEHAVIOR ?? 0);
                    }

                    string sLayoutSubtotal = "";
                    string sLayoutTotal = "";

                    string sCurIsoCode = oParkOp.CURRENCy.CUR_ISO_CODE;

                    string strSourceAppEmailPrefix = GetEmailSourceAppEmailPrefix(dSourceApp);

                    if (iLayout == 2)
                    {
                        sLayoutSubtotal = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Email_LayoutSubtotal"),
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}",
                                                        Convert.ToDouble(iQSubTotal) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                        (oParkOp.OPE_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oParkOp.OPE_PERC_VAT1 * 100) : "") +
                                                        (oParkOp.OPE_PERC_VAT2 != 0 && oParkOp.OPE_PERC_VAT1 != oParkOp.OPE_PERC_VAT2 ? string.Format("{0:0.00#}%", oParkOp.OPE_PERC_VAT2 * 100) : ""),
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}",
                                                        Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                    }
                    else if (iLayout == 1)
                    {
                        sLayoutTotal = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Email_LayoutTotal"),
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}",
                                                        Convert.ToDouble(iQuantity) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}",
                                                        Convert.ToDouble(iQFEE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                        (oParkOp.OPE_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oParkOp.OPE_PERC_VAT1 * 100) : "") +
                                                        (oParkOp.OPE_PERC_VAT2 != 0 && oParkOp.OPE_PERC_VAT1 != oParkOp.OPE_PERC_VAT2 ? string.Format("{0:0.00#}%", oParkOp.OPE_PERC_VAT2 * 100) : ""),
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}",
                                                        Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                    }
                    else if (iLayout == 3)
                    {
                        sLayoutTotal = string.Format(ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Email_LayoutTotalBonus"),
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQuantity) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQFEE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", -Convert.ToDouble(iQBonus) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)),
                                                        (oParkOp.OPE_PERC_VAT1 != 0 ? string.Format("{0:0.00#}% ", oParkOp.OPE_PERC_VAT1 * 100) : "") +
                                                        (oParkOp.OPE_PERC_VAT2 != 0 && oParkOp.OPE_PERC_VAT1 != oParkOp.OPE_PERC_VAT2 ? string.Format("{0:0.00#}%", oParkOp.OPE_PERC_VAT2 * 100) : ""),
                                                        string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iQVAT) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)));
                    }

                    string strUnParkingEmailSubject = null;
                    if (bAutomaticStop)
                        strUnParkingEmailSubject = ResourceExtension.GetLiteral(string.Format(strSourceAppEmailPrefix+"ConfirmUnParkingExpired{0}_EmailHeader", sEmailType));
                    if (string.IsNullOrEmpty(strUnParkingEmailSubject))
                        strUnParkingEmailSubject = ResourceExtension.GetLiteral(string.Format(strSourceAppEmailPrefix+"ConfirmUnParking{0}_EmailHeader", sEmailType));
                    if (string.IsNullOrEmpty(strUnParkingEmailSubject))
                        strUnParkingEmailSubject = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmUnParking_EmailHeader");
                    /*
                        * ID: {0}<br>
                        * Matr&iacute;cula: {1}<br>
                        * Ciudad: {2}<br>
                        * Fecha de emisi&ocuate;: {5:HH:mm:ss dd/MM/yyyy}<br>
                        * Aparcamiento Comienza:  {6:HH:mm:ss dd/MM/yyyy}<br><b>
                        * Aparcamiento Finaliza:  {7:HH:mm:ss dd/MM/yyyy}</b><br>
                        * Cantidad Pagada: {8} 
                        */
                    INSTALLATION oInst = oParkOp.INSTALLATION;

                    string strAmountToShow = "";
                    if (oUser.USR_REFUND_BALANCE_TYPE == (int)RefundBalanceType.rbtTime)
                    {
                        strAmountToShow = oParkOp.OPE_TIME.ToString();
                    }
                    else
                    {
                        int iFinalQuantity = oParkOp.OPE_FINAL_AMOUNT;
                        if (oParkOp.OPE_AMOUNT_CUR_ID != oParkOp.OPE_BALANCE_CUR_ID)
                        {
                            double dChangeFee = 0;
                            iFinalQuantity = ChangeQuantityFromInstallationCurToUserCur(iTotalQuantity, dChangeToApply, oInst, oUser, out dChangeFee);
                        }

                        strAmountToShow = (oParkOp.OPE_AMOUNT_CUR_ID == oParkOp.OPE_BALANCE_CUR_ID ?
                                    string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1}", Convert.ToDouble(iTotalQuantity/*oParkOp.OPE_TOTAL_AMOUNT*/) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode)) :
                                    string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(sCurIsoCode) + "} {1} / {2:" + infraestructureRepository.GetDecimalFormatFromIsoCode(oParkOp.CURRENCy1.CUR_ISO_CODE) + "} {3}", Convert.ToDouble(iTotalQuantity/*oParkOp.OPE_TOTAL_AMOUNT*/) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(sCurIsoCode), infraestructureRepository.GetCurSymbolFromIsoCode(sCurIsoCode),
                                                                                    Convert.ToDouble(iFinalQuantity/*oParkOp.OPE_FINAL_AMOUNT*/) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(oParkOp.CURRENCy1.CUR_ISO_CODE), infraestructureRepository.GetCurSymbolFromIsoCode(oParkOp.CURRENCy1.CUR_ISO_CODE)));
                    }




                    strUnParkingEmailBody = ResourceExtension.GetLiteral(string.Format(strSourceAppEmailPrefix+"ConfirmUnParking{0}_EmailBody", sEmailType));
                    if (string.IsNullOrEmpty(strUnParkingEmailBody))
                        strUnParkingEmailBody = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"ConfirmUnParking_EmailBody");
                    string sUnParkingEmailBalance = ResourceExtension.GetLiteral(string.Format(strSourceAppEmailPrefix+"Confirm{0}_EmailBody_Balance", sEmailType));
                    if (string.IsNullOrEmpty(sUnParkingEmailBalance))
                        sUnParkingEmailBalance = ResourceExtension.GetLiteral(strSourceAppEmailPrefix+"Confirm_EmailBody_Balance");

                    if (eParkingMode != ParkingMode.StartStop && eParkingMode != ParkingMode.StartStopHybrid)
                    {
                        DateTime? dtPrevEnd = oParkOp.OPE_REFUND_PREVIOUS_ENDDATE;

                        strUnParkingEmailBody = string.Format(strUnParkingEmailBody,
                            oParkOp.OPE_ID,
                            oParkOp.USER_PLATE.USRP_PLATE,
                            oParkOp.INSTALLATION.INS_DESCRIPTION,
                            oParkOp.OPE_DATE,
                            oParkOp.OPE_INIDATE,
                            oParkOp.OPE_ENDDATE,
                            strAmountToShow,
                            (oParkOp.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay || oUser.USR_BALANCE > 0) ?
                                    string.Format(sUnParkingEmailBalance, string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) + "} {1}",
                                                Convert.ToDouble(oUser.USR_BALANCE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                                infraestructureRepository.GetCurrencySymbolOrIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)))) : "",
                            ConfigurationManager.AppSettings["EmailSignatureURL"],
                            ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                            sLayoutSubtotal,
                            sLayoutTotal,
                            GetEmailFooter(ref oInst, dSourceApp), GetEmailInvoiceHeader(ref oInst, dSourceApp), dtPrevEnd);
                    }
                    else
                    {
                        string sGroupDesc = "";
                        if (oParkOp.GROUP != null) sGroupDesc = oParkOp.GROUP.GRP_DESCRIPTION;
                        string sTariffDesc = "";
                        if (oParkOp.TARIFF != null) sTariffDesc = oParkOp.TARIFF.TAR_DESCRIPTION;
                        
                        if (oParkOp.STREET_SECTION != null && !string.IsNullOrEmpty(oParkOp.STREET_SECTION.STRSE_DESCRIPTION))
                            sGroupDesc = string.Format("{0}, {1}", oParkOp.STREET_SECTION.STRSE_DESCRIPTION, sGroupDesc);

                        strUnParkingEmailBody = string.Format(strUnParkingEmailBody,
                            oParkOp.OPE_ID,
                            oParkOp.USER_PLATE.USRP_PLATE,
                            oParkOp.INSTALLATION.INS_DESCRIPTION,
                            sGroupDesc, sTariffDesc,
                            oParkOp.OPE_DATE,
                            oParkOp.OPE_INIDATE,
                            oParkOp.OPE_ENDDATE,
                            strAmountToShow,
                            (oParkOp.OPE_SUSCRIPTION_TYPE == (int)PaymentSuscryptionType.pstPrepay || oUser.USR_BALANCE > 0) ?
                                    string.Format(sUnParkingEmailBalance, string.Format("{0:" + infraestructureRepository.GetDecimalFormatFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))) + "} {1}",
                                                Convert.ToDouble(oUser.USR_BALANCE) / infraestructureRepository.GetCurrencyDivisorFromIsoCode(infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID))),
                                                infraestructureRepository.GetCurrencySymbolOrIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)))) : "",
                            ConfigurationManager.AppSettings["EmailSignatureURL"],
                            ConfigurationManager.AppSettings["EmailSignatureGraphic"],
                            sLayoutSubtotal,
                            sLayoutTotal,
                            "",
                            GetEmailFooter(ref oInst,dSourceApp),
                            GetEmailInvoiceHeader(ref oInst, dSourceApp));

                    }

                    SendEmail(ref oUser, strUnParkingEmailSubject, strUnParkingEmailBody, dSourceApp);
                }
            }
            catch (Exception ex)
            {
                string error = string.Format("Error:: OperationId: {0} - UserId: {1} - NameEmail:{2}-  Exception: {3}", dOperationID, oUser.USR_ID, strUnParkingEmailBody, ex.Message);
                m_Log.LogMessage(LogLevels.logERROR, string.Format("Error:: Email_ConfirmParking - {0}", error));
            }

            return bRet;
        }

        private TARIFF GetTariff(decimal dGroupId, decimal dTariffId)
        {
            TARIFF oTariff = null;
            GROUP oGroup = null;
            DateTime? dtInstDateTime = null;
            if (geograficAndTariffsRepository.getGroup(dGroupId, ref oGroup, ref dtInstDateTime))
                oTariff = GetTariff(oGroup, dTariffId);

            return oTariff; 
        }
        private TARIFF GetTariff(GROUP oGroup, decimal dTariffId)
        {
            TARIFF oTariff = null;

            if (oGroup.INSTALLATION.INS_MAP_SCREEN_TYPE != 1)
            {
                try
                {
                    oTariff = oGroup.TARIFFS_IN_GROUPs.Where(r => r.TARGR_TAR_ID == dTariffId).First().TARIFF;
                }
                catch
                {
                    foreach (GROUPS_TYPES_ASSIGNATION oAssigns in oGroup.GROUPS_TYPES_ASSIGNATIONs)
                    {
                        try
                        {
                            oTariff = oAssigns.GROUPS_TYPE.TARIFFS_IN_GROUPs.Where(r => r.TARGR_TAR_ID == dTariffId).First().TARIFF;
                            break;
                        }
                        catch { }
                    }
                }
            }
            else
            {
                oTariff = oGroup.INSTALLATION.TARIFFs.Where(t => t.TAR_ID == dTariffId).FirstOrDefault();
            }

            return oTariff;
        }

        private string SpecifyMessageErrorRefundNotPossible(string literalUnparkNotAllowed, SortedList parametersOut, ResultType rt)
        {
            string xmlOut = "";
            if (rt == ResultType.Result_Error_RefundNotPossible && !string.IsNullOrEmpty(literalUnparkNotAllowed))
            {
                parametersOut["literal_unpark_not_allowed"] = literalUnparkNotAllowed;
                xmlOut = GenerateXMLOuput(parametersOut);
            }
            else
            {
                xmlOut = GenerateXMLErrorResult(rt);
            }
            return xmlOut;
        }
        
        private string QueryUnParkingOperation(string xmlIn, 
                                                ref SortedList parametersIn,
                                                ref USER oUser,
                                                decimal? dInsId,
                                                string strCulture,
                                                ulong ulAppVersion,
                                                string strPlate,
                                                decimal? dGroupId, decimal? dStreetSectionId, decimal? dTariffId,
                                                int iWSTimeout,
                                                ref List<SortedList> lstRefunds,
                                                ref SortedList parametersOut,
                                                ref ResultType rt,
                                                out long lEllapsedTime)
        {
            string xmlOut = "";
            lEllapsedTime = 0;
            try
            {

                INSTALLATION oInst = null;
                DateTime? dtinstDateTime = null;
                string sLiteralUnparkNotAllowed = string.Empty;

                if (!geograficAndTariffsRepository.getInstallation(dInsId, null, null, ref oInst, ref dtinstDateTime))
                {
                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                    Logger_AddLogMessage(string.Format("QueryUnParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                    return xmlOut;
                }
                
                if (oInst != null)
                {
                    sLiteralUnparkNotAllowed = infraestructureRepository.GetLiteral(oInst.INS_LIT_UNPARK_NOT_ALLOWED.Value, strCulture);
                }

                //if (dtinstDateTime.HasValue)
                //{
                //    string sQueryParkingOffset = infraestructureRepository.GetParameterValueNoCache(string.Format("QueryParkingOffset_{0}", dInsId));
                //    if (string.IsNullOrEmpty(sQueryParkingOffset))
                //        sQueryParkingOffset = infraestructureRepository.GetParameterValueNoCache("QueryParkingOffset");
                //    if (!string.IsNullOrEmpty(sQueryParkingOffset))
                //    {
                //        try
                //        {
                //            int iOffset = Convert.ToInt32(sQueryParkingOffset);
                //            dtinstDateTime = dtinstDateTime.Value.AddMinutes(iOffset);
                //            Logger_AddLogMessage(string.Format("QueryUnParkingOperation datetime offset applied: new date={0:yyyy/MM/dd hh:mm:ss}", dtinstDateTime), LogLevels.logINFO);
                //        }
                //        catch (Exception)
                //        {
                //            Logger_AddLogMessage(string.Format("QueryUnParkingOperation::Error applying datetime offset: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logWARN);
                //        }
                //    }
                //}

                if (strPlate.Length > 0)
                {
                    if (customersRepository.ExistUnConfirmedParkingOperationFor(dInsId.Value, strPlate))
                    {
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_RefundNotPossible).ToString();
                        rt = (ResultType)Convert.ToInt32(parametersOut["r"].ToString());
                        xmlOut = SpecifyMessageErrorRefundNotPossible(sLiteralUnparkNotAllowed, parametersOut, rt);
                        Logger_AddLogMessage(string.Format("QueryUnParkingOperation::Error: ExistUnConfirmedParkingOperation xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        return xmlOut;
                    }
                }

                string sAdditionalParams = null;

                ThirdPartyOperation oThirdPartyOperation = new ThirdPartyOperation();

                List<INSTALLATION> oLstInsts= geograficAndTariffsRepository.GetChildInstallations(dInsId.Value, (DateTime?)null, false);


                if (oLstInsts.Count() == 0)
                {
                    oLstInsts = new List<INSTALLATION>();
                    oLstInsts.Add(oInst);
                }
                else
                {
                    oLstInsts = oLstInsts.Where(r => r.INS_OPT_UNPARK != 0).ToList();

                }

             
                foreach (var oInstallation in oLstInsts)
                {
                    switch ((UnParkWSSignatureType)oInstallation.INS_UNPARK_WS_SIGNATURE_TYPE)
                    {
                        case UnParkWSSignatureType.upst_test:
                            {
                                TimeSpan ts = new TimeSpan(0, 50, 0);

                                int iAmount = 50;
                                int iTime = 50;
                                DateTime dtInitialTime = dtinstDateTime.Value - ts;
                                DateTime dtEndTime = dtinstDateTime.Value;

                                parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();
                                SortedList oRefund = new SortedList();
                                oRefund["ins_id"] = oInstallation.INS_ID;
                                oRefund["d1"] = dtInitialTime.ToString("HHmmssddMMyy");
                                oRefund["d2"] = dtEndTime.ToString("HHmmssddMMyy");
                                oRefund["q"] = iAmount.ToString();
                                oRefund["t"] = iTime.ToString();
                                oRefund["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;
                                oRefund["p"] = oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).First().USRP_PLATE;
                                lstRefunds.Add(oRefund);

                                rt = (ResultType)Convert.ToInt32(parametersOut["r"].ToString());
                            }
                            break;

                        case UnParkWSSignatureType.upst_eysa:
                            {


                                if (strPlate.Length > 0)
                                {
                                    rt = oThirdPartyOperation.EysaQueryUnParking(0, oUser, strPlate, dtinstDateTime.Value, oInstallation, ulAppVersion, iWSTimeout, ref parametersOut,
                                                                                 ref lstRefunds, out lEllapsedTime);
                                    if (rt != ResultType.Result_OK)
                                    {
                                        xmlOut = GenerateXMLErrorResult(rt);
                                        Logger_AddLogMessage(string.Format("QueryParkingUnOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                        return xmlOut;
                                    }
                                }
                                else
                                {
                                    rt = ResultType.Result_Error_RefundNotPossible;

                                    foreach (USER_PLATE oPlate in oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1))
                                    {

                                        ResultType rtTemp = oThirdPartyOperation.EysaQueryUnParking(0, oUser, oPlate.USRP_PLATE, dtinstDateTime.Value, oInstallation, ulAppVersion, iWSTimeout, 
                                                                                                    ref parametersOut, ref lstRefunds, out lEllapsedTime);
                                        if ((rtTemp != ResultType.Result_OK) && (rtTemp != ResultType.Result_Error_RefundNotPossible))
                                        {
                                            xmlOut = GenerateXMLErrorResult(rt);
                                            Logger_AddLogMessage(string.Format("QueryParkingUnOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                            return xmlOut;
                                        }

                                        if (rtTemp == ResultType.Result_OK)
                                        {
                                            rt = ResultType.Result_OK;
                                        }
                                    }

                                    if (rt == ResultType.Result_OK)
                                    {
                                        parametersOut["r"] = Convert.ToInt32(rt).ToString();
                                    }
                                    else
                                    {
                                        xmlOut = SpecifyMessageErrorRefundNotPossible(sLiteralUnparkNotAllowed, parametersOut, rt);
                                        Logger_AddLogMessage(string.Format("QueryParkingUnOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                        return xmlOut;
                                    }

                                }

                            }
                            break;

                        case UnParkWSSignatureType.upst_internal:
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                            rt = (ResultType)Convert.ToInt32(parametersOut["r"].ToString());
                            break;

                        case UnParkWSSignatureType.upst_standard:
                            {

                                if (strPlate.Length > 0)
                                {

                                    rt = oThirdPartyOperation.StandardQueryUnParking(0, oUser, strPlate, dTariffId, dtinstDateTime.Value, oInstallation, ulAppVersion, iWSTimeout, ref parametersOut,
                                                                                     ref lstRefunds, out lEllapsedTime);

                                    if (rt == ResultType.Result_OK)
                                    {
                                        if (dGroupId.HasValue && dTariffId.HasValue && !dStreetSectionId.HasValue)
                                        {
                                            lstRefunds.RemoveAll(refund => refund["g"] == null || Convert.ToDecimal(refund["g"]) != dGroupId.Value ||
                                                                           refund["ad"] == null || Convert.ToDecimal(refund["ad"]) != dTariffId.Value);

                                        }
                                        else if (dGroupId.HasValue && dTariffId.HasValue && dStreetSectionId.HasValue)
                                        {
                                            lstRefunds.RemoveAll(refund => refund["g"] == null || Convert.ToDecimal(refund["g"]) != dGroupId.Value ||
                                                                           refund["ad"] == null || Convert.ToDecimal(refund["ad"]) != dTariffId.Value ||
                                                                           refund["sts"] == null || Convert.ToDecimal(refund["sts"]) != dStreetSectionId.Value);

                                        }
                                        if (lstRefunds.Count > 1)
                                            lstRefunds.RemoveRange(1, lstRefunds.Count - 1);
                                    }
                                    else
                                    {
                                        xmlOut = SpecifyMessageErrorRefundNotPossible(sLiteralUnparkNotAllowed, parametersOut, rt);
                                        //xmlOut = GenerateXMLErrorResult(rt);
                                        Logger_AddLogMessage(string.Format("QueryParkingUnOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                        return xmlOut;
                                    }
                                }
                                else
                                {


                                    rt = ResultType.Result_Error_RefundNotPossible;

                                    List<SortedList> lstRefundsPlate = null;

                                    foreach (USER_PLATE oPlate in oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1))
                                    {
                                        iWSTimeout -= (int)lEllapsedTime;
                                        lstRefundsPlate = new List<SortedList>();

                                        ResultType rtTemp = oThirdPartyOperation.StandardQueryUnParking(0, oUser, oPlate.USRP_PLATE, dTariffId, dtinstDateTime.Value, oInstallation, ulAppVersion, iWSTimeout, 
                                                                                                        ref parametersOut, ref lstRefundsPlate, out lEllapsedTime);

                                        
                                        if ((rtTemp != ResultType.Result_OK) && (rtTemp != ResultType.Result_Error_RefundNotPossible))
                                        {
                                            xmlOut = SpecifyMessageErrorRefundNotPossible(sLiteralUnparkNotAllowed, parametersOut, rt);
                                            //xmlOut = GenerateXMLErrorResult(rt);
                                            Logger_AddLogMessage(string.Format("QueryParkingUnOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                            return xmlOut;
                                        }
                                        else
                                        {
                                            if (dGroupId.HasValue && dTariffId.HasValue && !dStreetSectionId.HasValue)
                                            {
                                                lstRefundsPlate.RemoveAll(refund => refund["g"] == null || Convert.ToDecimal(refund["g"]) != dGroupId.Value ||
                                                                                    refund["ad"] == null || Convert.ToDecimal(refund["ad"]) != dTariffId.Value);

                                            }
                                            else if (dGroupId.HasValue && dTariffId.HasValue && dStreetSectionId.HasValue)
                                            {
                                                lstRefundsPlate.RemoveAll(refund => refund["g"] == null || Convert.ToDecimal(refund["g"]) != dGroupId.Value ||
                                                                                    refund["ad"] == null || Convert.ToDecimal(refund["ad"]) != dTariffId.Value ||
                                                                                    refund["sts"] == null || Convert.ToDecimal(refund["sts"]) != dStreetSectionId.Value);

                                            }
                                            if (lstRefundsPlate.Count > 1)
                                                lstRefundsPlate.RemoveRange(1, lstRefundsPlate.Count - 1);

                                            lstRefunds.AddRange(lstRefundsPlate);
                                        }

                                        if (rtTemp == ResultType.Result_OK)
                                        {
                                            rt = ResultType.Result_OK;
                                        }
                                    }

                                    if (rt == ResultType.Result_OK)
                                    {
                                        parametersOut["r"] = Convert.ToInt32(rt).ToString();
                                    }
                                    else
                                    {
                                        xmlOut = SpecifyMessageErrorRefundNotPossible(sLiteralUnparkNotAllowed, parametersOut, rt);
                                        Logger_AddLogMessage(string.Format("QueryParkingUnOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                        return xmlOut;
                                    }
                                }
                            }
                            break;

                        case UnParkWSSignatureType.upst_bsm:
                            {
                                if (dGroupId.HasValue && dTariffId.HasValue)
                                {
                                    OPERATION oStartOperation = null;
                                    if (customersRepository.ExistStartedParkingOperation(oUser.USR_ID, strPlate, dGroupId.Value, dTariffId.Value, dtinstDateTime.Value, out oStartOperation))
                                    {
                                        string sLocale = "ca";
                                        if (!string.IsNullOrEmpty(strCulture))
                                        {
                                            sLocale = strCulture.Split('-')[0].Trim();
                                        }
                                        BSMConfiguration oBSMConfiguration = null;
                                        try
                                        {
                                            oBSMConfiguration = (BSMConfiguration)JsonConvert.DeserializeObject(oStartOperation.OPE_ADDITIONAL_PARAMS, typeof(BSMConfiguration));
                                        }
                                        catch (Exception) { }

                                        if (oBSMConfiguration != null)
                                        {
                                            oBSMConfiguration.latitude = oStartOperation.OPE_LATITUDE;
                                            oBSMConfiguration.longitude = oStartOperation.OPE_LONGITUDE;

                                            decimal dVAT1;
                                            decimal dVAT2;
                                            decimal dPercFEE;
                                            decimal dPercFEETopped;
                                            decimal dFixedFEE;
                                            int? iPaymentTypeId = null;
                                            int? iPaymentSubtypeId = null;
                                            int? iTariffType = null;
                                            IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;

                                            if (oUser.CUSTOMER_PAYMENT_MEAN != null)
                                            {
                                                iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                                                iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
                                            }
                                            if (oStartOperation.TARIFF != null)
                                                iTariffType = oStartOperation.TARIFF.TAR_TYPE;
                                            if (customersRepository.GetFinantialParams(oUser, oStartOperation.OPE_INS_ID, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.ParkingRefund, iTariffType,
                                                                                        out dVAT1, out dVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode))
                                            {

                                                int iAmount = oStartOperation.OPE_AMOUNT;
                                                int iAmountWithoutBon = oStartOperation.OPE_AMOUNT_WITHOUT_BON ?? oStartOperation.OPE_AMOUNT;

                                                if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                                                    iAmount += Convert.ToInt32((oStartOperation.OPE_PARTIAL_VAT1 ?? 0));

                                                BSMModifiers oModifiers = null;

                                                rt = oThirdPartyOperation.BSMPriceUnParking(oUser, oStartOperation.GROUP, oStartOperation.TARIFF, strPlate, oBSMConfiguration, oStartOperation.OPE_INIDATE, 
                                                                                            dtinstDateTime.Value, oStartOperation.OPE_ENDDATE, iAmount, iAmountWithoutBon, oStartOperation.OPE_BON_MLT, 
                                                                                            oStartOperation.OPE_EXTERNAL_BASE_ID1, strCulture, sLocale, iWSTimeout, 
                                                                                            out oModifiers, ref parametersOut, ref lstRefunds, out lEllapsedTime);

                                                oBSMConfiguration.Modifiers = oModifiers;
                                                if ((lstRefunds != null) && (lstRefunds.Count == 1))
                                                {
                                                    oBSMConfiguration.parkingBaseQuantityLbl = lstRefunds[0]["ServiceParkingBaseQuantityLbl"] != null ? lstRefunds[0]["ServiceParkingBaseQuantityLbl"].ToString() : "";
                                                    oBSMConfiguration.parkingVariableQuantityLbl = lstRefunds[0]["ServiceParkingVariableQuantityLbl"] != null ? lstRefunds[0]["ServiceParkingVariableQuantityLbl"].ToString() : "";

                                                }

                                                JsonSerializerSettings oJsonSettings = new JsonSerializerSettings();
                                                oJsonSettings.NullValueHandling = NullValueHandling.Ignore;
                                                sAdditionalParams = JsonConvert.SerializeObject(oBSMConfiguration, oJsonSettings);

                                            }
                                            else
                                            {
                                                rt = ResultType.Result_Error_Generic;
                                                Logger_AddLogMessage("QueryParkingUnOperation::Error getting finantial parameters", LogLevels.logERROR);
                                            }
                                        }
                                        else
                                        {
                                            rt = ResultType.Result_Error_Generic;
                                            Logger_AddLogMessage("QueryParkingUnOperation::BSM missing operation additional parameters (BSMcityID, BSMconfigurationId, BSMzoneTypeId)", LogLevels.logERROR);
                                        }

                                    }
                                    else
                                    {
                                        rt = ResultType.Result_Error_StartedOperation_NotExist;
                                    }
                                }
                                else
                                {
                                    rt = ResultType.Result_Error_Generic;
                                    Logger_AddLogMessage("QueryParkingUnOperation::BSM missing input parameters (groupId, tariffId)", LogLevels.logERROR);
                                }

                                if (rt != ResultType.Result_OK)
                                {
                                    xmlOut = SpecifyMessageErrorRefundNotPossible(sLiteralUnparkNotAllowed, parametersOut, rt);
                                    Logger_AddLogMessage(string.Format("QueryParkingUnOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                    return xmlOut;
                                }
                            }
                            break;
                        case UnParkWSSignatureType.upst_bilbao_integration:
                            {

                                if (strPlate.Length > 0)
                                {

                                    rt = oThirdPartyOperation.BilbaoQueryUnParking(0, oUser, strPlate, dTariffId, dtinstDateTime.Value, oInstallation, ulAppVersion, iWSTimeout, ref parametersOut,
                                                                                     ref lstRefunds, out lEllapsedTime);

                                    if (rt == ResultType.Result_OK)
                                    {
                                        if (dGroupId.HasValue && dTariffId.HasValue)
                                        {
                                            lstRefunds.RemoveAll(refund => refund["g"] == null || Convert.ToDecimal(refund["g"]) != dGroupId.Value ||
                                                                           refund["ad"] == null || Convert.ToDecimal(refund["ad"]) != dTariffId.Value);

                                        }
                                        if (lstRefunds.Count > 1)
                                            lstRefunds.RemoveRange(1, lstRefunds.Count - 1);
                                    }
                                    else
                                    {
                                        xmlOut = SpecifyMessageErrorRefundNotPossible(sLiteralUnparkNotAllowed, parametersOut, rt);
                                        //xmlOut = GenerateXMLErrorResult(rt);
                                        Logger_AddLogMessage(string.Format("QueryParkingUnOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                        return xmlOut;
                                    }
                                }
                                else
                                {


                                    rt = ResultType.Result_Error_RefundNotPossible;

                                    List<SortedList> lstRefundsPlate = null;

                                    foreach (USER_PLATE oPlate in oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1))
                                    {
                                        iWSTimeout -= (int)lEllapsedTime;
                                        lstRefundsPlate = new List<SortedList>();

                                        ResultType rtTemp = oThirdPartyOperation.StandardQueryUnParking(0, oUser, oPlate.USRP_PLATE, dTariffId, dtinstDateTime.Value, oInstallation, ulAppVersion, iWSTimeout,
                                                                                                        ref parametersOut, ref lstRefundsPlate, out lEllapsedTime);


                                        if ((rtTemp != ResultType.Result_OK) && (rtTemp != ResultType.Result_Error_RefundNotPossible))
                                        {
                                            xmlOut = SpecifyMessageErrorRefundNotPossible(sLiteralUnparkNotAllowed, parametersOut, rt);
                                            //xmlOut = GenerateXMLErrorResult(rt);
                                            Logger_AddLogMessage(string.Format("QueryParkingUnOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                            return xmlOut;
                                        }
                                        else
                                        {                                            
                                            if (dGroupId.HasValue && dTariffId.HasValue && !dStreetSectionId.HasValue)
                                            {
                                                lstRefundsPlate.RemoveAll(refund => refund["g"] == null || Convert.ToDecimal(refund["g"]) != dGroupId.Value ||
                                                                                    refund["ad"] == null || Convert.ToDecimal(refund["ad"]) != dTariffId.Value);

                                            }
                                            else if (dGroupId.HasValue && dTariffId.HasValue && dStreetSectionId.HasValue)
                                            {
                                                lstRefundsPlate.RemoveAll(refund => refund["g"] == null || Convert.ToDecimal(refund["g"]) != dGroupId.Value ||
                                                                                    refund["ad"] == null || Convert.ToDecimal(refund["ad"]) != dTariffId.Value ||
                                                                                    refund["sts"] == null || Convert.ToDecimal(refund["sts"]) != dStreetSectionId.Value);

                                            }
                                            if (lstRefundsPlate.Count > 1)
                                                lstRefundsPlate.RemoveRange(1, lstRefundsPlate.Count - 1);

                                            lstRefunds.AddRange(lstRefundsPlate);
                                        }

                                        if (rtTemp == ResultType.Result_OK)
                                        {
                                            rt = ResultType.Result_OK;
                                        }
                                    }

                                    if (rt == ResultType.Result_OK)
                                    {
                                        parametersOut["r"] = Convert.ToInt32(rt).ToString();
                                    }
                                    else
                                    {
                                        xmlOut = SpecifyMessageErrorRefundNotPossible(sLiteralUnparkNotAllowed, parametersOut, rt);
                                        Logger_AddLogMessage(string.Format("QueryParkingUnOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                        return xmlOut;
                                    }
                                }
                            }
                            break;
                        default:
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                            rt = (ResultType)Convert.ToInt32(parametersOut["r"].ToString());
                            break;
                    }

                }


                double dChangeToApply = 1.0;

                if (ulAppVersion >= _VERSION_2_3)
                {
                    parametersOut["refunds"] = "";
                }

                string strQPlusVATQs = "";

                if (rt == ResultType.Result_OK)
                {
                    int i = 0;
                    foreach (SortedList oRefund in lstRefunds)
                    {
                        decimal dInstallationId =Convert.ToDecimal(oRefund["ins_id"]);
                        INSTALLATION oInstallation = oLstInsts.Where(r => r.INS_ID == dInstallationId).First();

                        oRefund["time_bal"] = oUser.USR_TIME_BALANCE.ToString();
                        oRefund["refund_balance_type"] = oUser.USR_REFUND_BALANCE_TYPE.ToString();
                        int iAmount = Convert.ToInt32(oRefund["q"]);
                        int? iAmountRem = null;
                        int iTime = Convert.ToInt32(oRefund["t"]);
                        dGroupId = oRefund.ContainsKey("g") ? Convert.ToDecimal(oRefund["g"]) : (decimal?)null;
                        dTariffId = oRefund.ContainsKey("ad") ? Convert.ToDecimal(oRefund["ad"]) : (decimal?)null;
                        DateTime dtInitialTime = DateTime.ParseExact(oRefund["d1"].ToString(), "HHmmssddMMyy",
                                                CultureInfo.InvariantCulture);
                        DateTime dtEndTime = DateTime.ParseExact(oRefund["d2"].ToString(), "HHmmssddMMyy",
                                                CultureInfo.InvariantCulture);
                        decimal dPercBonus = 0;
                        string sBonusId = null;
                        if (oRefund["bonusper"] != null)
                        {
                            dPercBonus = Convert.ToDecimal(oRefund["bonusper"]) / Convert.ToDecimal(100);
                            sBonusId = oRefund["bonusid"].ToString();
                        }


                        decimal dVAT1;
                        decimal dVAT2;
                        decimal dPercFEE;
                        decimal dPercFEETopped;
                        decimal dFixedFEE;
                        int? iPaymentTypeId = null;
                        int? iPaymentSubtypeId = null;
                        IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;
                        if (oUser.CUSTOMER_PAYMENT_MEAN != null)
                        {
                            iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                            iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
                        }
                        int? iTariffType = null;
                        ParkingMode eTariffBehavior = ParkingMode.Normal;
                        if (dTariffId.HasValue && dGroupId.HasValue)
                        {
                            TARIFF oTariff = GetTariff(dGroupId.Value, dTariffId.Value);
                            if (oTariff != null)
                            {
                                iTariffType = oTariff.TAR_TYPE;
                                eTariffBehavior = (ParkingMode)(oTariff.TAR_BEHAVIOR ?? 0);
                            }
                        }
                        if (!customersRepository.GetFinantialParams(oUser, oInstallation.INS_ID, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.ParkingRefund, iTariffType,
                                                                         out dVAT1, out dVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode))
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                            Logger_AddLogMessage(string.Format("QueryParkingUnOperation::Error getting installation FEE parameters: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            return xmlOut;
                        }

                        int iPartialVAT1;
                        int iPartialPercFEE;
                        int iPartialFixedFEE;
                        int iPartialPercFEEVAT;
                        int iPartialFixedFEEVAT;
                        int iPartialBonusFEE;
                        int iPartialBonusFEEVAT;

                        int iTotalAmount = customersRepository.CalculateFEE(ref iAmount, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE, dPercBonus, eTaxMode,
                                                                            out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE,
                                                                            out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);

                        //iTotalAmount = iAmount - iPartialVAT1 - iPartialPercFEE - iPartialFixedFEE;

                        decimal dAmountFEE = Math.Round(iAmount * dPercFEE, MidpointRounding.AwayFromZero);
                        if (dPercFEETopped > 0 && dAmountFEE > dPercFEETopped) dAmountFEE = dPercFEETopped;
                        dAmountFEE += dFixedFEE;
                        int iAmountFEE = Convert.ToInt32(Math.Round(dAmountFEE, MidpointRounding.AwayFromZero));

                        int iBonus = Convert.ToInt32(Math.Round(dAmountFEE * dPercBonus, MidpointRounding.AwayFromZero));

                        int iAmountVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT - iPartialBonusFEEVAT;
                        int iSubTotalAmount = iAmount + iAmountFEE - iBonus;

                        int iQPlusIVA = iAmount + iPartialVAT1;
                        int iFeePlusIVA = iPartialPercFEE + iPartialFixedFEE;

                        if (ulAppVersion >= _VERSION_1_4)
                        {

                            oRefund["q"] = iAmount;
                            oRefund["layout"] = oInstallation.INS_FEE_LAYOUT;
                            oRefund["q_fee_lbl"] = infraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_FEE_LIT_ID ?? 0, strCulture);
                            oRefund["q_fee_vat_lbl"] = infraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_FEE_PLUS_VAT_LIT_ID ?? 0, strCulture);
                            oRefund["q_vat_lbl"] = infraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_VAT_LIT_ID ?? 0, strCulture);
                            oRefund["q_subtotalLbl"] = infraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_SUBTOTAL_LIT_ID ?? 0, strCulture);
                            oRefund["q_total_lbl"] = infraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_TOTAL_LIT_ID ?? 0, strCulture);
                            oRefund["q_lbl"] = infraestructureRepository.GetLiteral(oInstallation.INS_SERVICE_PARK_LIT_ID ?? 0, strCulture);
                            oRefund["q_fee"] = iAmountFEE;
                            oRefund["q_vat"] = iAmountVAT;
                            oRefund["q_subtotal"] = iSubTotalAmount;
                            oRefund["q_total"] = iTotalAmount;
                            oRefund["q_plus_vat"] = iQPlusIVA;
                            oRefund["q_fee_plus_vat"] = iFeePlusIVA;

                            if (i > 0)
                            {
                                strQPlusVATQs += "|";
                            }
                            strQPlusVATQs += string.Format("{0};{1}", iAmount, iQPlusIVA);

                            i++;

                            if (eTariffBehavior == ParkingMode.StartStopHybrid)
                            {                                
                                if (oRefund["q_rem"] != null)
                                {
                                    SortedList oSelRefund = oRefund;

                                    int iAmountTmp = Convert.ToInt32(oSelRefund["q_rem"]);
                                    RecalcRefundAmount(ref iAmountTmp, ref oUser, ref oSelRefund, oInstallation, ulAppVersion);
                                    oSelRefund.Remove("q_rem");
                                    iAmountRem = iAmountTmp;
                                    if (oSelRefund["q_rem_without_bon"] != null)
                                    {
                                        SortedList oSelRefundTmp = new SortedList();
                                        iAmountTmp = Convert.ToInt32(oSelRefund["q_rem_without_bon"]);
                                        RecalcRefundAmount(ref iAmountTmp, ref oUser, ref oSelRefundTmp, oInstallation, ulAppVersion);
                                        oSelRefund["q_without_bon"] = iAmountTmp;
                                        oSelRefund.Remove("q_rem_without_bon");
                                    }

                                }
                                if (oRefund["t_rem"] != null)
                                {
                                    oRefund["t"] = oRefund["t_rem"];
                                    oRefund.Remove("t_rem");
                                }
                            }
                        }

                        if (oInstallation.CURRENCy.CUR_ISO_CODE != infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)))
                        {
                            double dChangeFee = 0;

                            dChangeToApply = GetChangeToApplyFromInstallationCurToUserCur(oInstallation, oUser);
                            if (dChangeToApply < 0)
                            {
                                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                                Logger_AddLogMessage(string.Format("QueryParkingUnOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                                return xmlOut;
                            }

                            NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                            numberFormatProvider.NumberDecimalSeparator = ".";
                            oRefund["chng"] = dChangeToApply.ToString(numberFormatProvider);

                            int iQChange = ChangeQuantityFromInstallationCurToUserCur(Convert.ToInt32(oRefund["q"]),
                                            dChangeToApply, oInstallation, oUser, out dChangeFee);

                            oRefund["qch"] = iQChange.ToString();

                            int iAmountFEEChange = ChangeQuantityFromInstallationCurToUserCur(iAmountFEE, dChangeToApply, oInstallation, oUser, out dChangeFee);
                            oRefund["qch_fee"] = iAmountFEEChange.ToString();

                            int iAmountVATChange = ChangeQuantityFromInstallationCurToUserCur(iAmountVAT, dChangeToApply, oInstallation, oUser, out dChangeFee);
                            oRefund["qch_vat"] = iAmountVATChange.ToString();


                            int iSubTotalAmountChange = ChangeQuantityFromInstallationCurToUserCur(iSubTotalAmount, dChangeToApply, oInstallation, oUser, out dChangeFee);
                            oRefund["qch_subtotal"] = iSubTotalAmountChange.ToString();

                            int iTotalAmountChange = ChangeQuantityFromInstallationCurToUserCur(iTotalAmount, dChangeToApply, oInstallation, oUser, out dChangeFee);
                            oRefund["qch_total"] = iTotalAmountChange.ToString();

                            int iQPlusIVAChange = ChangeQuantityFromInstallationCurToUserCur(iQPlusIVA, dChangeToApply, oInstallation, oUser, out dChangeFee);
                            oRefund["qch_plus_vat"] = iQPlusIVAChange.ToString();

                            int iFeePlusIVAChange = ChangeQuantityFromInstallationCurToUserCur(iFeePlusIVA, dChangeToApply, oInstallation, oUser, out dChangeFee);
                            oRefund["qch_fee_plus_vat"] = iFeePlusIVAChange.ToString();


                        }

                        ChargeOperationsType operationType = ChargeOperationsType.ParkingRefund;

                        DateTime? dtUTCDateTime = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtinstDateTime.Value);
                        DateTime? dtUTCIniDateTime = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtInitialTime);
                        DateTime? dtUTCEndDateTime = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtEndTime);

                        int iPercFEETopped = Convert.ToInt32(Math.Round(dPercFEETopped, MidpointRounding.AwayFromZero));
                        int iFixedFEE = Convert.ToInt32(Math.Round(dFixedFEE, MidpointRounding.AwayFromZero));

                        string strBaseOpeId = null;
                        if (oRefund["base_oper_id"] != null)
                            strBaseOpeId = oRefund["base_oper_id"].ToString();

                        decimal? dBonMlt = null;
                        decimal? dBonExtMlt = null;
                        string sVehicleType = null;
                        int? iAmountWithoutBon = null;
                        if (oRefund["per_bon"] != null)
                        {
                            NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                            numberFormatProvider.NumberDecimalSeparator = ".";                            
                            try
                            {
                                string sBonMlt = oRefund["per_bon"].ToString();
                                if (sBonMlt.IndexOf(",") > 0) numberFormatProvider.NumberDecimalSeparator = ",";
                                decimal dTryBonMlt = Convert.ToDecimal(sBonMlt, numberFormatProvider);
                                dBonMlt = dTryBonMlt;
                            }
                            catch
                            {
                                Logger_AddLogMessage(string.Format("QueryUnParkingOperation::Error parsing per_bon '{0}'", parametersIn["per_bon"].ToString()), LogLevels.logERROR);
                                dBonMlt = null;
                            }
                        }
                        if (oRefund["vehicletype"] != null)
                            sVehicleType = oRefund["vehicletype"].ToString();
                        if (oRefund["q_without_bon"] != null)
                        {
                            try
                            {

                                iAmountWithoutBon = Int32.Parse(oRefund["q_without_bon"].ToString(), CultureInfo.InvariantCulture);
                               
                                if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                                {

                                    int iAmountWithoutBonRef = iAmountWithoutBon.Value;
                                    int iPartialVAT1Temp = 0;
                                    int iPartialPercFEETemp = 0;
                                    int iPartialFixedFEETemp = 0;
                                    int iPartialPercFEEVATTemp = 0;
                                    int iPartialFixedFEEVATTemp = 0;


                                    int iQWithoutBonTotal = customersRepository.CalculateFEE(ref iAmountWithoutBonRef, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE, eTaxMode,
                                                                                out iPartialVAT1Temp, out iPartialPercFEETemp, out iPartialFixedFEETemp,
                                                                                out iPartialPercFEEVATTemp, out iPartialFixedFEEVATTemp);

                                    iAmountWithoutBon = iAmountWithoutBonRef;

                                    oRefund["q_without_bon"] = iAmountWithoutBon;

                                }



                                
                            }
                            catch //(Exception ex)
                            {
                                Logger_AddLogMessage(string.Format("QueryUnParkingOperation::Error parsing q_without_bon '{0}'", parametersIn["q_without_bon"].ToString()), LogLevels.logERROR);
                                iAmountWithoutBon = null;
                            }
                        }

                        if (!customersRepository.AddSessionOperationUnParkInfo(ref oUser, parametersIn["SessionID"].ToString(), operationType,
                                        dtinstDateTime.Value, dtUTCDateTime.Value, oRefund["p"].ToString(), iAmount, iAmountRem, iTime, dGroupId, dTariffId, dtUTCIniDateTime.Value, dtUTCEndDateTime.Value, dChangeToApply,
                                        dVAT1, dVAT2, dPercFEE, iPercFEETopped, iFixedFEE, dPercBonus, sBonusId, strQPlusVATQs,
                                        dBonMlt, dBonExtMlt, sVehicleType, iAmountWithoutBon,
                                        strBaseOpeId, sAdditionalParams))
                        {
                            xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                            Logger_AddLogMessage(string.Format("QueryParkingUnOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                            return xmlOut;
                        }

                        if (ulAppVersion >= _VERSION_2_3)
                        {
                            StringBuilder sb = new StringBuilder();

                            sb.Append("<refund json:Array='true'>");
                            foreach (string key in oRefund.Keys)
                            {
                                sb.AppendFormat("<{0}>{1}</{0}>", key, oRefund[key]);
                            }

                            sb.Append("</refund>");
                            parametersOut["refunds"] += sb.ToString();
                        }
                        else
                        {
                            foreach (string key in oRefund.Keys)
                            {
                                parametersOut[key] = oRefund[key];
                            }

                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("QueryUnParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
            }
            return xmlOut;
        }

       


        private string ConfirmUnParkingOperation(string xmlIn, 
                                                ref SortedList parametersIn,
                                                ref USER oUser,
                                                decimal? dInsId,
                                                decimal? dLatitude,
                                                decimal? dLongitude,
                                                string strAppVersion,
                                                string strPlate,
                                                int iQuantity,
                                                int? iQuantityRem,
                                                decimal? dGroupId,
                                                decimal? dStreetSectionId,
                                                decimal? dTariffId,
                                                decimal dSourceApp,
                                                string strSessionCulture,
                                                int iWSTimeout,
                                                ref SortedList parametersOut,
                                                ref ResultType rt,
                                                out DateTime? dtEndModified)
        {
            string xmlOut = "";
            dtEndModified = null;

            try
            {
                INSTALLATION oInst = null;
                DateTime? dtinstDateTime = null;
                decimal? dLatitudeInst = null;
                decimal? dLongitudeInst = null;

                if (!geograficAndTariffsRepository.getInstallation(dInsId.Value,
                                                             dLatitudeInst,
                                                             dLongitudeInst,
                                                             ref oInst,
                                                             ref dtinstDateTime))
                {
                   
                    xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Invalid_City);
                    Logger_AddLogMessage(string.Format("ConfirmUnParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                    return xmlOut;
                }


                double dChangeToApply = 1.0;
                DateTime dtSavedInstallationTime = DateTime.UtcNow;
                DateTime dtUtcSavedTime = DateTime.UtcNow;
                ChargeOperationsType operationType = ChargeOperationsType.ParkingRefund;
                int iTime = 0;
                DateTime dtUTCInitialTime = dtinstDateTime.Value;
                DateTime dtUTCEndTime = dtinstDateTime.Value;
                decimal dPercVAT1;
                decimal dPercVAT2;
                decimal dPercFEE;
                int iPercFEETopped;
                int iFixedFEE;
                int iPartialVAT1;
                int iPartialPercFEE;
                int iPartialFixedFEE;
                int iPartialPercFEEVAT;
                int iPartialFixedFEEVAT;
                int iPartialBonusFEE;
                int iTotalQuantity;
                decimal dPercBonus = 0;
                string sBonusId = null;
                decimal? dBonMlt = null;
                string sVehicleType = null;
                int? iAmountWithoutBon = null;
                string sBaseOpeId = null;
                string sAdditionalParams = null;


                int iQTIVA;

                IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;
                int? iPaymentTypeId = null;
                int? iPaymentSubtypeId = null;
                if (oUser.CUSTOMER_PAYMENT_MEAN != null)
                {
                    iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                    iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
                }

               
                parametersOut["autorecharged"] = "0";
                string strQPlusVATQs = "";

                int? iQuantityToRefund = null;
                int? iQuantityRemaining = null;

                TARIFF oTariff = null;

                if (dGroupId.HasValue && dTariffId.HasValue)
                {
                    bool bCheckAmountRemaining = false;

                    oTariff = GetTariff(dGroupId.Value, dTariffId.Value);
                    if (oTariff != null && oTariff.TAR_BEHAVIOR == (int)ParkingMode.StartStopHybrid)
                    {
                        bCheckAmountRemaining = true;
                    }

                    if (!customersRepository.CheckSessionOperationUnParkInfo(ref oUser, parametersIn["SessionID"].ToString(), strPlate, iQuantity, bCheckAmountRemaining, dGroupId.Value,dTariffId.Value, out dtSavedInstallationTime, out dtUtcSavedTime,
                        out iTime, out dtUTCInitialTime, out dtUTCEndTime, out operationType, out dChangeToApply,
                        out dPercVAT1, out dPercVAT2, out dPercFEE, out iPercFEETopped, out iFixedFEE, out dPercBonus, out sBonusId, out strQPlusVATQs,
                        out dBonMlt, out sVehicleType, out iAmountWithoutBon,
                        out sBaseOpeId, out sAdditionalParams,
                        out iQuantityToRefund, out iQuantityRemaining))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_OperationExpired);
                        Logger_AddLogMessage(string.Format("ConfirmUnParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        return xmlOut;
                    }

                }
                else
                {


                    if (!customersRepository.CheckSessionOperationUnParkInfo(ref oUser, parametersIn["SessionID"].ToString(), strPlate, iQuantity, false, out dtSavedInstallationTime, out dtUtcSavedTime,
                        out iTime, out dGroupId, out dTariffId, out dtUTCInitialTime, out dtUTCEndTime, out operationType, out dChangeToApply,
                        out dPercVAT1, out dPercVAT2, out dPercFEE, out iPercFEETopped, out iFixedFEE, out dPercBonus, out sBonusId, out strQPlusVATQs,
                        out dBonMlt, out sVehicleType, out iAmountWithoutBon,
                        out sBaseOpeId, out sAdditionalParams,
                        out iQuantityToRefund, out iQuantityRemaining))
                    {
                        xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_OperationExpired);
                        Logger_AddLogMessage(string.Format("ConfirmUnParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                        return xmlOut;
                    }
                }

                if (iQuantityToRefund.HasValue)
                    iQuantity = iQuantityToRefund.Value;

                if (iQuantityRemaining.HasValue && !iQuantityRem.HasValue)
                    iQuantityRem = iQuantityRemaining;

                decimal dPercFEETopped = 0;
                decimal dFixedFEE = 0;


                //TARIFF oTariff = null;
                DateTime? dtgroupDateTime=null;
                GROUP oGroup = null;
                geograficAndTariffsRepository.getGroup(dGroupId, ref oGroup, ref dtgroupDateTime);
                int? iTariffType = null;
                if (dTariffId.HasValue && dGroupId.HasValue)
                {
                    oTariff = GetTariff(dGroupId.Value, dTariffId.Value);
                    if (oTariff != null)
                    {
                        iTariffType = oTariff.TAR_TYPE;
                    }
                }



                INSTALLATION oInstallation = oGroup.INSTALLATION;

                customersRepository.GetFinantialParams(oUser, oInstallation.INS_ID, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.ParkingRefund, iTariffType,
                                                                    out dPercVAT1, out dPercVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode);


                Dictionary<int, int> oDictQs = new Dictionary<int, int>();

                string[] strTuples = strQPlusVATQs.Split(new char[] { '|' });

                foreach (string strtupla in strTuples)
                {
                    string[] strQs = strtupla.Split(new char[] { ';' });

                    if (strQs.Length == 2)
                    {
                        oDictQs[Convert.ToInt32(strQs[0])] = Convert.ToInt32(strQs[1]);
                    }

                }

                DateTime? dtInitialTime = geograficAndTariffsRepository.ConvertUTCToInstallationDateTime(oInstallation.INS_ID, dtUTCInitialTime);
                DateTime? dtEndTime = geograficAndTariffsRepository.ConvertUTCToInstallationDateTime(oInstallation.INS_ID, dtUTCEndTime);


                parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK).ToString();

                int iCurrencyChargedQuantity = 0;
                decimal dOperationID = -1;
                string str3dPartyOpNum = "";
                DateTime? dtUTCInsertionDate = null;
                int iPartialBonusFEEVAT;


                decimal dUserID = oUser.USR_ID;
                string strSessionID = parametersIn["SessionID"].ToString();

                var session = oUser.MOBILE_SESSIONs.Where(r => r.MOSE_SESSIONID == strSessionID
                                                    && r.MOSE_USR_ID == dUserID).First();


                DateTime? dtPrevEnd = null;

                if (parametersIn.ContainsKey("d_prev_end"))
                {
                    try
                    {
                        dtPrevEnd = DateTime.ParseExact(parametersIn["d_prev_end"].ToString(), "HHmmssddMMyy",
                            CultureInfo.InvariantCulture);
                    }
                    catch { }
                }



                if ((oDictQs.ContainsKey(iQuantity)) && (eTaxMode == IsTAXMode.IsNotTaxVATBackward))
                {

                    iQuantity = oDictQs[iQuantity];
                    iTotalQuantity = customersRepository.CalculateFEE(ref iQuantity, dPercVAT1, dPercVAT2, dPercFEE, dPercFEETopped, dFixedFEE, dPercBonus, eTaxMode, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE, out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);
                }
                else
                {
                    iTotalQuantity = customersRepository.CalculateFEE(iQuantity, dPercVAT1, dPercVAT2, dPercFEE, dPercFEETopped, dFixedFEE, dPercBonus, out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE);
                }

                //iTotalQuantity = iQuantity - iPartialVAT1 - iPartialPercFEE - iPartialFixedFEE;


                iQTIVA = iQuantity;


                if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
                {
                    iQTIVA += iPartialVAT1;
                }

                rt = ChargeUnParkingOperation(strPlate, iQuantity, iTime, dtSavedInstallationTime, dtInitialTime.Value, dtEndTime.Value, dtPrevEnd, dChangeToApply, dGroupId, dStreetSectionId, oTariff,
                                            dPercVAT1, dPercVAT2, dPercFEE, iPercFEETopped, iFixedFEE, dPercBonus,
                                            iPartialVAT1, iPartialPercFEE, iPartialFixedFEE, iPartialBonusFEE, iTotalQuantity, sBonusId,
                                            dBonMlt, sVehicleType, iAmountWithoutBon,
                                            parametersIn, oInstallation, ref oUser, session.MOSE_OS.Value, session.MOSE_ID, dLatitude, dLongitude, strAppVersion, sBaseOpeId, 
                                            sAdditionalParams, dSourceApp,
                                            ref parametersOut, out dOperationID, out dtUTCInsertionDate, out iCurrencyChargedQuantity);


                if (rt != ResultType.Result_OK)
                {
                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("ConfirmUnParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                    return xmlOut;
                }

                if (!dtPrevEnd.HasValue)
                {
                    dtPrevEnd = DateTime.Now;

                }

                //DateTime? dtEndModified = null;
                DateTime? dtUtcEndModified = null;
                bool bInsertNoAnswered = false;

                ThirdPartyOperation oThirdPartyOperation = new ThirdPartyOperation();
                long lEllapsedTime = 0;


                if ((oInstallation.INS_OPT_OPERATIONCONFIRM_MODE ?? 0) == (int)OperationConfirmMode.online ||
                    (oInstallation.INS_OPT_OPERATIONCONFIRM_MODE ?? 0) == (int)OperationConfirmMode.first_online)
                {


                    switch ((ConfirmParkWSSignatureType)oInstallation.INS_PARK_CONFIRM_WS_SIGNATURE_TYPE)
                    {
                        case ConfirmParkWSSignatureType.cpst_nocall:
                            rt = ResultType.Result_OK;
                            break;
                        case ConfirmParkWSSignatureType.cpst_test:
                            break;

                        case ConfirmParkWSSignatureType.cpst_eysa:
                            {
                                rt = oThirdPartyOperation.EysaConfirmUnParking(1, strPlate, dtSavedInstallationTime, oUser, oInstallation,
                                                                            iQTIVA, iTime, dGroupId, dTariffId, dtInitialTime.Value, dtEndTime.Value, iWSTimeout, 
                                                                            ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                            }
                            break;

                        case ConfirmParkWSSignatureType.cpst_internal:
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                            break;

                        case ConfirmParkWSSignatureType.cpst_standard:
                            {

                                rt = oThirdPartyOperation.StandardConfirmUnParking(1, strPlate, dtSavedInstallationTime, oUser, oInstallation,
                                                                                    iQTIVA, iTime, dGroupId.Value, dStreetSectionId, dTariffId.Value, dtInitialTime.Value,
                                                                                    dtEndTime.Value, dOperationID, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                            }
                            break;
                            
                        case ConfirmParkWSSignatureType.cpst_gtechna:
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                            break;

                        case ConfirmParkWSSignatureType.cpst_bsm:
                            {
                                BSMConfiguration oBSMConfiguration = null;
                                if (!string.IsNullOrEmpty(sAdditionalParams))
                                {
                                    try
                                    {
                                        oBSMConfiguration = (BSMConfiguration)JsonConvert.DeserializeObject(sAdditionalParams, typeof(BSMConfiguration));
                                        if (oBSMConfiguration.latitude.HasValue) dLatitude = oBSMConfiguration.latitude;
                                        if (oBSMConfiguration.longitude.HasValue) dLongitude = oBSMConfiguration.longitude;
                                    }
                                    catch (Exception)
                                    {
                                        rt = ResultType.Result_Error_Generic;
                                    }
                                }
                                else
                                    rt = ResultType.Result_Error_Generic;

                                if (!dLatitude.HasValue || !dLongitude.HasValue)
                                {
                                    rt = ResultType.Result_Error_Invalid_Input_Parameter;
                                    Logger_AddLogMessage("ConfirmUnParkingOperation::Gps position required", LogLevels.logERROR);
                                }
                                else
                                {
                                    int iQuantityModified;
                                    rt = oThirdPartyOperation.BSMStop(1, oInstallation, Convert.ToInt32(sBaseOpeId), dtUtcSavedTime, dLatitude.Value, dLongitude.Value, 
                                                                      iAmountWithoutBon??iQTIVA, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out dtUtcEndModified, 
                                                                      out iQuantityModified, out lEllapsedTime, out bInsertNoAnswered);
                                    if (rt == ResultType.Result_OK)
                                    {
                                        if (dtUtcEndModified.HasValue)
                                            dtEndModified = geograficAndTariffsRepository.ConvertUTCToInstallationDateTime(oInstallation.INS_ID, dtUtcEndModified.Value);
                                        if (iQuantityModified != (iAmountWithoutBon??iQTIVA))
                                        {
                                            // ...

                                        }
                                    }
                                }
                            }
                            break;
                        case ConfirmParkWSSignatureType.cpst_bilbao_integration:
                            {

                                rt = oThirdPartyOperation.BilbaoIntegrationConfirmUnParking(1, strPlate, dtSavedInstallationTime, oUser, oInstallation,
                                                                                    iQTIVA, iTime, dGroupId.Value, dTariffId.Value, dtInitialTime.Value,
                                                                                    dtEndTime.Value, dOperationID, iWSTimeout, ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                            }
                            break;

                        case ConfirmParkWSSignatureType.cpst_SIR:
                            rt = ResultType.Result_OK;
                            break;

                        default:
                            parametersOut["r"] = Convert.ToInt32(ResultType.Result_Error_Generic).ToString();
                            break;
                    }

                }

                if (rt != ResultType.Result_OK)
                {

                    try
                    {
                        if (parametersOut.IndexOfKey("autorecharged") >= 0)
                            parametersOut.Remove("autorecharged");
                        if (parametersOut.IndexOfKey("newbal") >= 0)
                            parametersOut.Remove("newbal");
                        if (parametersOut.IndexOfKey("new_time_bal") >= 0)
                            parametersOut.Remove("newtime_bal");
                    }
                    catch { }


                    ResultType rtRefund = BackUnParkPayment(ref oUser, dOperationID, bInsertNoAnswered, sBaseOpeId, oTariff);
                    if (rtRefund == ResultType.Result_OK)
                    {
                        Logger_AddLogMessage(string.Format("ConfirmUnParkingOperation::Payment Refund of {0}", iCurrencyChargedQuantity), LogLevels.logERROR);
                    }
                    else
                    {
                        Logger_AddLogMessage(string.Format("ConfirmUnParkingOperation::Error in Payment Refund: {0}", rtRefund.ToString()), LogLevels.logERROR);
                    }


                    xmlOut = GenerateXMLErrorResult(rt);
                    Logger_AddLogMessage(string.Format("ConfirmUnParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);
                    return xmlOut;
                }
                else
                {
                    parametersOut["utc_offset"] = geograficAndTariffsRepository.GetInstallationUTCOffSetInMinutes(oInstallation.INS_ID);

                    if (str3dPartyOpNum.Length > 0 || dtEndModified.HasValue || dtUtcEndModified.HasValue)
                    {
                        customersRepository.UpdateThirdPartyIDAndDatesInParkingOperation(ref oUser, 1, dOperationID, str3dPartyOpNum, sBaseOpeId, null, null, dtEndModified, dtUtcEndModified);
                    }

                }


                if (Convert.ToInt32(parametersOut["r"]) == Convert.ToInt32(ResultType.Result_OK))
                {
                    customersRepository.DeleteSessionOperationInfo(ref oUser, parametersIn["SessionID"].ToString(), strPlate);

                    Email_ConfirmUnParking(oUser, dOperationID, iQuantityRem, false);
                }


                if ((oInstallation.INS_OPT_OPERATIONCONFIRM_MODE ?? 0) == (int)OperationConfirmMode.online)
                {
                    iWSTimeout -= (int)lEllapsedTime;

                    if (Convert.ToInt32(parametersOut["r"]) == Convert.ToInt32(ResultType.Result_OK))
                    {
                        bool bConfirmed1 = true;
                        bool bConfirmed2 = true;
                        bool bConfirmed3 = true;


                        if (oInstallation.INS_PARK_CONFIRM_WS2_SIGNATURE_TYPE.HasValue)
                        {
                            ResultType rt2 = ResultType.Result_OK;
                            SortedList parametersOutTemp = new SortedList();

                            switch ((ConfirmParkWSSignatureType)oInstallation.INS_PARK_CONFIRM_WS2_SIGNATURE_TYPE)
                            {

                                case ConfirmParkWSSignatureType.cpst_nocall:
                                    rt2 = ResultType.Result_OK;
                                    break;

                                case ConfirmParkWSSignatureType.cpst_eysa:
                                    {

                                        rt2 = oThirdPartyOperation.EysaConfirmUnParking(2, strPlate, dtSavedInstallationTime, oUser, oInstallation,
                                                                                    iQTIVA, iTime, dGroupId, dTariffId, dtInitialTime.Value, dtEndTime.Value, iWSTimeout,
                                                                                    ref parametersOutTemp, out str3dPartyOpNum, out lEllapsedTime);
                                    }
                                    break;


                                case ConfirmParkWSSignatureType.cpst_standard:
                                    {
                                        rt2 = oThirdPartyOperation.StandardConfirmUnParking(2, strPlate, dtSavedInstallationTime, oUser, oInstallation,
                                                                                            iQTIVA, iTime, dGroupId.Value, dStreetSectionId, dTariffId.Value, dtInitialTime.Value,
                                                                                            dtEndTime.Value, dOperationID, iWSTimeout,
                                                                                            ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                                    }
                                    break;

                                case ConfirmParkWSSignatureType.cpst_gtechna:
                                    {
                                        rt2 = ResultType.Result_Error_Generic;
                                    }
                                    break;
                                case ConfirmParkWSSignatureType.cpst_SIR:
                                    rt2 = ResultType.Result_OK;
                                    break;

                                default:
                                    break;
                            }



                            if (rt2 != ResultType.Result_OK)
                            {
                                bConfirmed2 = false;
                                Logger_AddLogMessage(string.Format("ConfirmUnParkingOperation::Error in WS 2 Confirmation"), LogLevels.logWARN);
                            }
                            else
                            {
                                if (str3dPartyOpNum.Length > 0)
                                {
                                    customersRepository.UpdateThirdPartyIDInParkingOperation(ref oUser, 2, dOperationID, str3dPartyOpNum);
                                }

                            }
                        }


                        if (oInstallation.INS_PARK_CONFIRM_WS3_SIGNATURE_TYPE.HasValue)
                        {
                            iWSTimeout -= (int)lEllapsedTime;

                            SortedList parametersOutTemp = new SortedList();
                            ResultType rt2 = ResultType.Result_OK;

                            switch ((ConfirmParkWSSignatureType)oInstallation.INS_PARK_CONFIRM_WS3_SIGNATURE_TYPE)
                            {
                                case ConfirmParkWSSignatureType.cpst_nocall:
                                    rt2 = ResultType.Result_OK;
                                    break;

                                case ConfirmParkWSSignatureType.cpst_eysa:
                                    {
                                        rt2 = oThirdPartyOperation.EysaConfirmUnParking(3, strPlate, dtSavedInstallationTime, oUser, oInstallation,
                                                                                    iQTIVA, iTime, dGroupId, dTariffId, dtInitialTime.Value, dtEndTime.Value, iWSTimeout,
                                                                                    ref parametersOutTemp, out str3dPartyOpNum, out lEllapsedTime);
                                    }
                                    break;


                                case ConfirmParkWSSignatureType.cpst_standard:
                                    {
                                        rt2 = oThirdPartyOperation.StandardConfirmUnParking(3, strPlate, dtSavedInstallationTime, oUser, oInstallation,
                                                                                            iQTIVA, iTime, dGroupId.Value, dStreetSectionId, dTariffId.Value, dtInitialTime.Value,
                                                                                            dtEndTime.Value, dOperationID, iWSTimeout,
                                                                                            ref parametersOut, out str3dPartyOpNum, out lEllapsedTime);
                                    }
                                    break;


                                case ConfirmParkWSSignatureType.cpst_gtechna:
                                    {
                                        rt2 = ResultType.Result_Error_Generic;
                                    }
                                    break;
                                case ConfirmParkWSSignatureType.cpst_SIR:
                                    rt2 = ResultType.Result_OK;
                                    break;

                                default:
                                    break;
                            }



                            if (rt2 != ResultType.Result_OK)
                            {
                                bConfirmed3 = false;
                                Logger_AddLogMessage(string.Format("ConfirmUnParkingOperation::Error in WS 3 Confirmation"), LogLevels.logWARN);
                            }
                            else
                            {
                                if (str3dPartyOpNum.Length > 0)
                                {
                                    customersRepository.UpdateThirdPartyIDInParkingOperation(ref oUser, 3, dOperationID, str3dPartyOpNum);
                                }

                            }
                        }

                        if ((!bConfirmed2) || (!bConfirmed3))
                        {
                            customersRepository.UpdateThirdPartyConfirmedInParkingOperation(ref oUser, dOperationID, bConfirmed1, bConfirmed2, bConfirmed3);
                        }
                    }
                }

                if (dInsId.HasValue)
                {
                    DateTime? dInsDateTime = geograficAndTariffsRepository.getInstallationDateTime(dInsId.Value);

                    if (dInsDateTime.HasValue)
                    {
                        parametersOut["cityDatetime"] = dtinstDateTime.Value.ToString("HHmmssddMMyy");
                    }
                }

                string strCulture = strSessionCulture;
                if (parametersIn["lang"] != null)
                {
                    try
                    {
                        int iLangIndex = Convert.ToInt32(parametersIn["lang"].ToString());
                        if (iLangIndex <= UserDeviceLangs.Length)
                        {
                            strCulture = UserDeviceLangs[iLangIndex - 1];
                        }
                    }
                    catch
                    { }
                }
                ulong ulAppVersion = AppUtilities.AppVersion(strAppVersion);
                parametersOut["userparks"] = GetUserCurrentParkingOperations(ref oUser, dInsId.Value, strCulture, ulAppVersion);

            }
            catch (Exception e)
            {
                xmlOut = GenerateXMLErrorResult(ResultType.Result_Error_Generic);
                Logger_AddLogException(e, string.Format("ConfirmUnParkingOperation::Error: xmlIn={0}, xmlOut={1}", PrettyXml(xmlIn), PrettyXml(xmlOut)), LogLevels.logERROR);


            }

            return xmlOut;

        }

        private ResultType RecalcRefundAmount(ref int iAmount, ref USER oUser, ref SortedList oRefund, INSTALLATION oInstallation, ulong ulAppVersion)
        {
            ResultType rtRes = ResultType.Result_OK;
            
            decimal dPercBonus = 0;
            if (oRefund["bonusper"] != null)                        
                dPercBonus = Convert.ToDecimal(oRefund["bonusper"]) / Convert.ToDecimal(100);

            decimal dVAT1;
            decimal dVAT2;
            decimal dPercFEE;
            decimal dPercFEETopped;
            decimal dFixedFEE;
            int? iPaymentTypeId = null;
            int? iPaymentSubtypeId = null;
            IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;
            if (oUser.CUSTOMER_PAYMENT_MEAN != null)
            {
                iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
            }
            decimal? dGroupId = oRefund.ContainsKey("g") ? Convert.ToDecimal(oRefund["g"]) : (decimal?)null;
            decimal? dTariffId = oRefund.ContainsKey("ad") ? Convert.ToDecimal(oRefund["ad"]) : (decimal?)null;
            int? iTariffType = null;            
            if (dTariffId.HasValue && dGroupId.HasValue)
            {
                TARIFF oTariff = GetTariff(dGroupId.Value, dTariffId.Value);
                if (oTariff != null)                
                    iTariffType = oTariff.TAR_TYPE;                
            }
            if (!customersRepository.GetFinantialParams(oUser, oInstallation.INS_ID, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.ParkingRefund, iTariffType,
                                                             out dVAT1, out dVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode))
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogMessage(string.Format("RecalcRefundAmount::Error getting installation FEE parameters"), LogLevels.logERROR);
                return rtRes;
            }

            int iPartialVAT1;
            int iPartialPercFEE;
            int iPartialFixedFEE;
            int iPartialPercFEEVAT;
            int iPartialFixedFEEVAT;
            int iPartialBonusFEE;
            int iPartialBonusFEEVAT;

            int iTotalAmount = customersRepository.CalculateFEE(ref iAmount, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE, dPercBonus, eTaxMode,
                                                                out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE,
                                                                out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);

            //iTotalAmount = iAmount - iPartialVAT1 - iPartialPercFEE - iPartialFixedFEE;

            decimal dAmountFEE = Math.Round(iAmount * dPercFEE, MidpointRounding.AwayFromZero);
            if (dPercFEETopped > 0 && dAmountFEE > dPercFEETopped) dAmountFEE = dPercFEETopped;
            dAmountFEE += dFixedFEE;
            int iAmountFEE = Convert.ToInt32(Math.Round(dAmountFEE, MidpointRounding.AwayFromZero));

            int iBonus = Convert.ToInt32(Math.Round(dAmountFEE * dPercBonus, MidpointRounding.AwayFromZero));

            int iAmountVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT - iPartialBonusFEEVAT;
            int iSubTotalAmount = iAmount + iAmountFEE - iBonus;

            int iQPlusIVA = iAmount + iPartialVAT1;
            int iFeePlusIVA = iPartialPercFEE + iPartialFixedFEE;

            if (ulAppVersion >= _VERSION_1_4)
            {
                oRefund["q"] = iAmount;
                oRefund["layout"] = oInstallation.INS_FEE_LAYOUT;                
                oRefund["q_fee"] = iAmountFEE;
                oRefund["q_vat"] = iAmountVAT;
                oRefund["q_subtotal"] = iSubTotalAmount;
                oRefund["q_total"] = iTotalAmount;
                oRefund["q_plus_vat"] = iQPlusIVA;
                oRefund["q_fee_plus_vat"] = iFeePlusIVA;
                
                /*if (strQPlusVATQs != "")
                {
                    strQPlusVATQs += "|";
                }
                strQPlusVATQs += string.Format("{0};{1}", iAmount, iQPlusIVA);*/
                
            }

            if (oInstallation.CURRENCy.CUR_ISO_CODE != infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)))
            {
                double dChangeToApply = 1.0;
                double dChangeFee = 0;

                dChangeToApply = GetChangeToApplyFromInstallationCurToUserCur(oInstallation, oUser);
                if (dChangeToApply < 0)
                {
                    rtRes = ResultType.Result_Error_Generic;
                    Logger_AddLogMessage(string.Format("RecalcRefundAmount::Error"), LogLevels.logERROR);
                    return rtRes;
                }

                NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                numberFormatProvider.NumberDecimalSeparator = ".";
                oRefund["chng"] = dChangeToApply.ToString(numberFormatProvider);

                int iQChange = ChangeQuantityFromInstallationCurToUserCur(Convert.ToInt32(oRefund["q"]),
                                dChangeToApply, oInstallation, oUser, out dChangeFee);

                oRefund["qch"] = iQChange.ToString();

                int iAmountFEEChange = ChangeQuantityFromInstallationCurToUserCur(iAmountFEE, dChangeToApply, oInstallation, oUser, out dChangeFee);
                oRefund["qch_fee"] = iAmountFEEChange.ToString();

                int iAmountVATChange = ChangeQuantityFromInstallationCurToUserCur(iAmountVAT, dChangeToApply, oInstallation, oUser, out dChangeFee);
                oRefund["qch_vat"] = iAmountVATChange.ToString();


                int iSubTotalAmountChange = ChangeQuantityFromInstallationCurToUserCur(iSubTotalAmount, dChangeToApply, oInstallation, oUser, out dChangeFee);
                oRefund["qch_subtotal"] = iSubTotalAmountChange.ToString();

                int iTotalAmountChange = ChangeQuantityFromInstallationCurToUserCur(iTotalAmount, dChangeToApply, oInstallation, oUser, out dChangeFee);
                oRefund["qch_total"] = iTotalAmountChange.ToString();

                int iQPlusIVAChange = ChangeQuantityFromInstallationCurToUserCur(iQPlusIVA, dChangeToApply, oInstallation, oUser, out dChangeFee);
                oRefund["qch_plus_vat"] = iQPlusIVAChange.ToString();

                int iFeePlusIVAChange = ChangeQuantityFromInstallationCurToUserCur(iFeePlusIVA, dChangeToApply, oInstallation, oUser, out dChangeFee);
                oRefund["qch_fee_plus_vat"] = iFeePlusIVAChange.ToString();

            }

            return rtRes;
        }

        private string OperationQueryTags(OPERATION oOperation, string strCulture)
        {
            StringBuilder sb = new StringBuilder();

            USER oUser = oOperation.USER;

            decimal dPercBonus = (oOperation.OPE_PERC_BONUS ?? 0);

            decimal dVAT1 = oOperation.OPE_PERC_VAT1 ?? 0;
            decimal dVAT2 = oOperation.OPE_PERC_VAT2 ?? 0;
            decimal dPercFEE = oOperation.OPE_PERC_FEE ?? 0;
            decimal dPercFEETopped = oOperation.OPE_PERC_FEE_TOPPED ?? 0;
            decimal dFixedFEE = oOperation.OPE_FIXED_FEE ?? 0;

            NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
            numberFormatProvider.NumberDecimalSeparator = ".";

            /*int? iPaymentTypeId = null;
            int? iPaymentSubtypeId = null;
            IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;
            if (oUser.CUSTOMER_PAYMENT_MEAN != null)
            {
                iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
            }            
            int? iTariffType = null;
            if (oOperation.TARIFF != null)
                iTariffType = oOperation.TARIFF.TAR_TYPE;

            
            if (!customersRepository.GetFinantialParams(oUser, oOperation.OPE_INS_ID, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, iPaymentTypeId, iPaymentSubtypeId, ChargeOperationsType.ParkingRefund, iTariffType,
                                                             out dVAT1, out dVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode))
            {                
                Logger_AddLogMessage(string.Format("OperationQueryTags::Error getting installation FEE parameters"), LogLevels.logERROR);
                return sRes;
            }*/

            int iPartialVAT1;
            int iPartialPercFEE;
            int iPartialFixedFEE;
            int iPartialPercFEEVAT;
            int iPartialFixedFEEVAT;
            int iPartialBonusFEE;
            int iPartialBonusFEEVAT;

            int iAmount = oOperation.OPE_AMOUNT;
            int iAmountWithoutBon = (oOperation.OPE_AMOUNT_WITHOUT_BON ?? 0);
            if (oOperation.OPE_PARKING_MODE == (int)ParkingMode.StartStop && oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund)
            {
                // Calculate amount remaining using start operation
                OPERATION oStartOp = null;
                if (!string.IsNullOrEmpty(oOperation.OPE_EXTERNAL_BASE_ID1))                
                    customersRepository.GetStartOperationByExternalBaseId(1, oOperation.OPE_EXTERNAL_BASE_ID1, out oStartOp);
                if (oStartOp == null && !string.IsNullOrEmpty(oOperation.OPE_EXTERNAL_BASE_ID2))
                    customersRepository.GetStartOperationByExternalBaseId(2, oOperation.OPE_EXTERNAL_BASE_ID2, out oStartOp);
                if (oStartOp == null && !string.IsNullOrEmpty(oOperation.OPE_EXTERNAL_BASE_ID3))
                    customersRepository.GetStartOperationByExternalBaseId(3, oOperation.OPE_EXTERNAL_BASE_ID3, out oStartOp);

                if (oStartOp != null)
                {
                    iAmount = oStartOp.OPE_AMOUNT - oOperation.OPE_AMOUNT;
                    iAmountWithoutBon = (oStartOp.OPE_AMOUNT_WITHOUT_BON ?? 0) - (oOperation.OPE_AMOUNT_WITHOUT_BON ?? 0);
                }
            }

            int iTotalAmount = customersRepository.CalculateFEE(ref iAmount, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE, dPercBonus, IsTAXMode.IsTax,
                                                                out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE,
                                                                out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);

            //iTotalAmount = iAmount - iPartialVAT1 - iPartialPercFEE - iPartialFixedFEE;

            decimal dAmountFEE = Math.Round(iAmount * dPercFEE, MidpointRounding.AwayFromZero);
            if (dPercFEETopped > 0 && dAmountFEE > dPercFEETopped) dAmountFEE = dPercFEETopped;
            dAmountFEE += dFixedFEE;
            int iAmountFEE = Convert.ToInt32(Math.Round(dAmountFEE, MidpointRounding.AwayFromZero));

            int iBonus = Convert.ToInt32(Math.Round(dAmountFEE * dPercBonus, MidpointRounding.AwayFromZero));

            int iAmountVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT - iPartialBonusFEEVAT;
            int iSubTotalAmount = iAmount + iAmountFEE - iBonus;

            int iQPlusIVA = iAmount + iPartialVAT1;
            int iFeePlusIVA = iPartialPercFEE + iPartialFixedFEE;

            sb.AppendFormat("<q>{0}</q>" +
                            "<layout>{1}</layout>" +
                            "<q_fee>{2}</q_fee>" +
                            "<q_vat>{3}</q_vat>" +
                            "<q_subtotal>{4}</q_subtotal>" +
                            "<q_total>{5}</q_total>" +
                            //"<real_q>850</real_q>" +
                            "<q_plus_vat>{6}</q_plus_vat>" +
                            "<q_fee_plus_vat>{7}</q_fee_plus_vat>" +
                            "<q_without_bon>{8}</q_without_bon>" +
                            "<per_bon>{9}</per_bon>",
                            iAmount,
                            oOperation.INSTALLATION.INS_FEE_LAYOUT,
                            iAmountFEE,
                            iAmountVAT,
                            iSubTotalAmount,
                            iTotalAmount,
                            iQPlusIVA,
                            iFeePlusIVA,
                            iAmountWithoutBon,
                            oOperation.OPE_BON_MLT.HasValue?oOperation.OPE_BON_MLT.Value.ToString(numberFormatProvider) : "0");

            if (!string.IsNullOrEmpty(oOperation.OPE_VEHICLE_TYPE))
                sb.AppendFormat("<vehicletype>{0}</vehicletype>", oOperation.OPE_VEHICLE_TYPE);

            if (oOperation.INSTALLATION.CURRENCy.CUR_ISO_CODE != infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)))
            {
                double dChangeToApply = 1.0;
                double dChangeFee = 0;

                dChangeToApply = Convert.ToDouble(oOperation.OPE_CHANGE_APPLIED);/// GetChangeToApplyFromInstallationCurToUserCur(oOperation.INSTALLATION, oUser);
                if (dChangeToApply >= 0)
                {
                    
                    string sChng = dChangeToApply.ToString(numberFormatProvider);

                    int iQChange = ChangeQuantityFromInstallationCurToUserCur(iAmount,
                                    dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);

                    int iAmountFEEChange = ChangeQuantityFromInstallationCurToUserCur(iAmountFEE, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                    int iAmountVATChange = ChangeQuantityFromInstallationCurToUserCur(iAmountVAT, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                    int iSubTotalAmountChange = ChangeQuantityFromInstallationCurToUserCur(iSubTotalAmount, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                    int iTotalAmountChange = ChangeQuantityFromInstallationCurToUserCur(iTotalAmount, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                    int iQPlusIVAChange = ChangeQuantityFromInstallationCurToUserCur(iQPlusIVA, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                    int iFeePlusIVAChange = ChangeQuantityFromInstallationCurToUserCur(iFeePlusIVA, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);

                    sb.AppendFormat("<chng>{0}</chng>" +
                                    "<qch>{1}</qch>" +
                                    "<qch_fee>{2}</qch_fee>" +
                                    "<qch_vat>{3}</qch_vat>" +
                                    "<qch_subtotal>{4}</qch_subtotal>" +
                                    "<qch_total>{5}</qch_total>" +                        
                                    "<qch_plus_vat>{6}</qch_plus_vat>" +
                                    "<qch_fee_plus_vat>{7}</qch_fee_plus_vat>",
                                    sChng,
                                    iQChange,
                                    iAmountFEEChange, 
                                    iAmountVATChange,
                                    iSubTotalAmountChange,
                                    iTotalAmountChange,
                                    iQPlusIVAChange,
                                    iFeePlusIVAChange);

                }
            }
           
            sb.AppendFormat("<ServiceParkingLbl>{0}</ServiceParkingLbl>" +
                            "<ServiceFeeLbl>{1}</ServiceFeeLbl>" +
                            "<ServiceFeeVATLbl>{2}</ServiceFeeVATLbl>" +
                            "<ServiceVATLbl>{3}</ServiceVATLbl>" +
                            "<ServiceTotalLbl>{4}</ServiceTotalLbl>" +
                            "<ServiceParkingBaseLbl>{5}</ServiceParkingBaseLbl>" +
                            "<ServiceParkingVariableLbl>{6}</ServiceParkingVariableLbl>",
                            infraestructureRepository.GetLiteral(oOperation.INSTALLATION.INS_SERVICE_PARK_LIT_ID ?? 0, strCulture),
                            infraestructureRepository.GetLiteral(oOperation.INSTALLATION.INS_SERVICE_FEE_LIT_ID ?? 0, strCulture),
                            infraestructureRepository.GetLiteral(oOperation.INSTALLATION.INS_SERVICE_FEE_PLUS_VAT_LIT_ID ?? 0, strCulture),
                            infraestructureRepository.GetLiteral(oOperation.INSTALLATION.INS_SERVICE_VAT_LIT_ID ?? 0, strCulture),
                            infraestructureRepository.GetLiteral(oOperation.INSTALLATION.INS_SERVICE_TOTAL_LIT_ID ?? 0, strCulture),
                            infraestructureRepository.GetLiteral(oOperation.INSTALLATION.INS_SERVICE_PARK_BASE_LIT_ID ?? 0, strCulture),
                            infraestructureRepository.GetLiteral(oOperation.INSTALLATION.INS_SERVICE_PARK_VARIABLE_LIT_ID ?? 0, strCulture));


            sb.AppendFormat("<ReceiptHeaderLbl>{0}</ReceiptHeaderLbl>", infraestructureRepository.GetLiteralFromKey(string.Format("{0}_{1}", Convert.ToInt32(oOperation.INSTALLATION.INS_ID), "RECEIPT_HEADER"), strCulture));
            sb.AppendFormat("<ExternalID>{0}</ExternalID>", oOperation.OPE_EXTERNAL_BASE_ID1);

            if ((ParkWSSignatureType)oOperation.INSTALLATION.INS_PARK_WS_SIGNATURE_TYPE == ParkWSSignatureType.pst_bsm &&
                !string.IsNullOrEmpty(oOperation.OPE_ADDITIONAL_PARAMS))
            {
                BSMConfiguration oBSMConfiguration = null;
                try
                {
                    oBSMConfiguration = (BSMConfiguration)JsonConvert.DeserializeObject(oOperation.OPE_ADDITIONAL_PARAMS, typeof(BSMConfiguration));
                }
                catch (Exception) { }

                if (oBSMConfiguration != null)
                {
                    sb.AppendFormat("<ServiceParkingBaseQuantityLbl>{0}</ServiceParkingBaseQuantityLbl>", oBSMConfiguration.parkingBaseQuantityLbl);
                    sb.AppendFormat("<ServiceParkingVariableQuantityLbl>{0}</ServiceParkingVariableQuantityLbl>", oBSMConfiguration.parkingVariableQuantityLbl);
                }

                if (oBSMConfiguration != null && oBSMConfiguration.Modifiers != null && oBSMConfiguration.Modifiers.Modifiers != null && oBSMConfiguration.Modifiers.Modifiers.Any())
                {
                    sb.AppendFormat("<modifiers>{0}</modifiers>", oBSMConfiguration.Modifiers.ToCustomXml());
                }
            }
            else if (!string.IsNullOrEmpty(oOperation.OPE_ADDITIONAL_PARAMS))
            {

                dynamic oParams = JsonConvert.DeserializeObject(oOperation.OPE_ADDITIONAL_PARAMS);

                var oRDetail = oParams["rdetail"];

                if (oRDetail != null)
                {
                    int iRDetail = Convert.ToInt32(oRDetail);
                    string strMessage = infraestructureRepository.GetCustomErrorMessageForOperation(oOperation.OPE_DATE, iRDetail, oOperation.OPE_GRP_ID.Value, oOperation.OPE_TAR_ID, strCulture);
                    sb.AppendFormat("<ParkingWarningLbl>{0}</ParkingWarningLbl>", strMessage);
                }
            }

            sb.AppendFormat("<g_id>{0}</g_id>" +
                            "<ad_id>{1}</ad_id>" +
                            "<isshopkeeperoperation>{2}</isshopkeeperoperation>", 
                            oOperation.OPE_GRP_ID ?? 0,
                            oOperation.OPE_TAR_ID ?? 0,
                            oOperation.OPE_SHOPKEEPER_OP ?? 0);

            if (oOperation.OPE_STRSE_ID.HasValue)
                sb.AppendFormat("<sts_id>{0}</sts_id>", oOperation.OPE_STRSE_ID.Value);

            bool bStop = false;
            bool bExtension = false;
            bool bRefund = false;

            if (oOperation.TARIFF != null)
            {
                bStop = (oOperation.OPE_PARKING_MODE == (int)ParkingMode.StartStop && 
                         oOperation.OPE_PARKING_MODE_STATUS == (int)ParkingModeStatus.Opened);

                if (oOperation.OPE_PARKING_MODE != (int)ParkingMode.StartStop &&
                    oOperation.OPE_UTC_ENDDATE >= DateTime.UtcNow)
                {
                    IQueryable<OPERATION> oOperations = null;
                    if (customersRepository.GetOperationsByExternalBaseId(1, oOperation.OPE_EXTERNAL_BASE_ID1, out oOperations))
                    {
                        var oLastOp = oOperations.OrderByDescending(o => o.OPE_DATE).FirstOrDefault();
                        if (oLastOp != null && oLastOp.OPE_ID == oOperation.OPE_ID)
                        {
                            bExtension = (oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation ||
                                          oOperation.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation ||
                                          oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund);
                            bRefund = ((oOperation.INSTALLATION.INS_OPT_UNPARK == 1 ||
                                        oOperation.OPE_PARKING_MODE == (int)ParkingMode.StartStopHybrid) &&
                                       (oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation ||
                                        oOperation.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation));
                        }
                    }
                }

                /*if (oOperation.OPE_PARKING_MODE != (int)ParkingMode.StartStop &&
                    (oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation ||
                     oOperation.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation))
                {
                    bExtension = (oOperation.OPE_UTC_ENDDATE >= DateTime.UtcNow);
                }

                if (oOperation.OPE_PARKING_MODE != (int)ParkingMode.StartStop &&
                    oOperation.INSTALLATION.INS_OPT_UNPARK == 1)
                {
                    IQueryable<OPERATION> oOperations = null;
                    if (customersRepository.GetOperationsByExternalBaseId(1, oOperation.OPE_EXTERNAL_BASE_ID1, out oOperations))
                    {
                        bRefund = !oOperations.Where(o => o.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund).Any();
                    }
                }*/
            }

            sb.AppendFormat("<stop>{0}</stop>" +
                            "<extension>{1}</extension>" +
                            "<refund>{2}</refund>",
                            bStop ? 1 : 0,
                            bExtension ? 1 : 0,
                            bRefund ? 1 : 0);

            return sb.ToString();
        }


        private string OperationQueryTags(HIS_OPERATION oOperation, ref  List<ALL_OPERATION> oListOperations, string strCulture, List<decimal> oRemovedOps)
        {
            StringBuilder sb = new StringBuilder();

            USER oUser = oOperation.USER;

            decimal dPercBonus = (oOperation.OPE_PERC_BONUS ?? 0);

            decimal dVAT1 = oOperation.OPE_PERC_VAT1 ?? 0;
            decimal dVAT2 = oOperation.OPE_PERC_VAT2 ?? 0;
            decimal dPercFEE = oOperation.OPE_PERC_FEE ?? 0;
            decimal dPercFEETopped = oOperation.OPE_PERC_FEE_TOPPED ?? 0;
            decimal dFixedFEE = oOperation.OPE_FIXED_FEE ?? 0;

            NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
            numberFormatProvider.NumberDecimalSeparator = ".";

            int? iPaymentTypeId = null;
            int? iPaymentSubtypeId = null;
            IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;
            if (oUser.CUSTOMER_PAYMENT_MEAN != null)
            {
                iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
            }            
            int? iTariffType = null;
            if (oOperation.TARIFF != null)
                iTariffType = oOperation.TARIFF.TAR_TYPE;

            
            if (!customersRepository.GetFinantialParams(oUser, oOperation.OPE_INS_ID, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, iPaymentTypeId, iPaymentSubtypeId, (ChargeOperationsType)oOperation.OPE_TYPE, iTariffType, oOperation.OPE_DATE,
                                                             out dVAT1, out dVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode))
            {                
                Logger_AddLogMessage(string.Format("OperationQueryTags::Error getting installation FEE parameters"), LogLevels.logERROR);               
            }

            int iPartialVAT1;
            int iPartialPercFEE;
            int iPartialFixedFEE;
            int iPartialPercFEEVAT;
            int iPartialFixedFEEVAT;
            int iPartialBonusFEE;
            int iPartialBonusFEEVAT;

            int iAmount = oOperation.OPE_AMOUNT;
            int iAmountWithoutBon = (oOperation.OPE_AMOUNT_WITHOUT_BON ?? 0);

            int iTime = oOperation.OPE_TIME;

            if (oOperation.OPE_PARKING_MODE == (int)ParkingMode.StartStop && oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund)
            {
                // Calculate amount remaining using start operation
                OPERATION oStartOp = null;
                if (!string.IsNullOrEmpty(oOperation.OPE_EXTERNAL_BASE_ID1))
                    customersRepository.GetStartOperationByExternalBaseId(1, oOperation.OPE_EXTERNAL_BASE_ID1, out oStartOp);
                if (oStartOp == null && !string.IsNullOrEmpty(oOperation.OPE_EXTERNAL_BASE_ID2))
                    customersRepository.GetStartOperationByExternalBaseId(2, oOperation.OPE_EXTERNAL_BASE_ID2, out oStartOp);
                if (oStartOp == null && !string.IsNullOrEmpty(oOperation.OPE_EXTERNAL_BASE_ID3))
                    customersRepository.GetStartOperationByExternalBaseId(3, oOperation.OPE_EXTERNAL_BASE_ID3, out oStartOp);

                if (oStartOp != null)
                {
                    iAmount = oStartOp.OPE_AMOUNT - oOperation.OPE_AMOUNT;
                    iAmountWithoutBon = (oStartOp.OPE_AMOUNT_WITHOUT_BON ?? 0) - (oOperation.OPE_AMOUNT_WITHOUT_BON ?? 0);
                }
            }
            else if (oOperation.OPE_PARKING_MODE == (int)ParkingMode.StartStopHybrid && oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund)
            {
                // Calculate amount remaining using start operation
                List<OPERATION> oStartExtsOps = new List<OPERATION>();
                if (!string.IsNullOrEmpty(oOperation.OPE_EXTERNAL_BASE_ID1))
                    customersRepository.GetStartStopHybridChainOperationsByExternalBaseId(1, oOperation.OPE_EXTERNAL_BASE_ID1, out oStartExtsOps);
                if (!oStartExtsOps.Any() && !string.IsNullOrEmpty(oOperation.OPE_EXTERNAL_BASE_ID2))
                    customersRepository.GetStartStopHybridChainOperationsByExternalBaseId(2, oOperation.OPE_EXTERNAL_BASE_ID2, out oStartExtsOps);
                if (!oStartExtsOps.Any() && !string.IsNullOrEmpty(oOperation.OPE_EXTERNAL_BASE_ID3))
                    customersRepository.GetStartStopHybridChainOperationsByExternalBaseId(3, oOperation.OPE_EXTERNAL_BASE_ID3, out oStartExtsOps);

                if (oStartExtsOps.Any())
                {
                    iAmount = 0;
                    iAmountWithoutBon = 0;
                    iTime = 0;
                    foreach (var oChainOp in oStartExtsOps.Where(o => o.OPE_DATE < oOperation.OPE_DATE).OrderByDescending(o => o.OPE_DATE))
                    {
                        if (oChainOp.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund && oChainOp.OPE_ID != oOperation.OPE_ID)
                            break;
                        else
                        {
                            iAmount += oChainOp.OPE_AMOUNT;
                            iAmountWithoutBon += (oChainOp.OPE_AMOUNT_WITHOUT_BON ?? 0);
                            iTime += oChainOp.OPE_TIME;
                        }
                    }
                    //iAmount = oStartExtsOps.Sum(o => o.OPE_AMOUNT) - oOperation.OPE_AMOUNT;
                    //iAmountWithoutBon = oStartExtsOps.Sum(o => o.OPE_AMOUNT_WITHOUT_BON ?? 0) - (oOperation.OPE_AMOUNT_WITHOUT_BON ?? 0);
                    iAmount -= oOperation.OPE_AMOUNT;
                    iAmountWithoutBon -= (oOperation.OPE_AMOUNT_WITHOUT_BON ?? 0);
                    iTime -= oOperation.OPE_TIME;
                }
            }

            if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
            {
                iAmount = iAmount + (int)(oOperation.OPE_PARTIAL_VAT1 ?? 0);
            }


            int iTotalAmount = customersRepository.CalculateFEE(ref iAmount, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE, dPercBonus, eTaxMode,
                                                                out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE,
                                                                out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);

            //iTotalAmount = iAmount - iPartialVAT1 - iPartialPercFEE - iPartialFixedFEE;

            decimal dAmountFEE = Math.Round(iAmount * dPercFEE, MidpointRounding.AwayFromZero);
            if (dPercFEETopped > 0 && dAmountFEE > dPercFEETopped) dAmountFEE = dPercFEETopped;
            dAmountFEE += dFixedFEE;
            int iAmountFEE = Convert.ToInt32(Math.Round(dAmountFEE, MidpointRounding.AwayFromZero));

            int iBonus = Convert.ToInt32(Math.Round(dAmountFEE * dPercBonus, MidpointRounding.AwayFromZero));

            int iAmountVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT - iPartialBonusFEEVAT;
            int iSubTotalAmount = iAmount + iAmountFEE - iBonus;

            int iQPlusIVA = iAmount + iPartialVAT1;
            int iFeePlusIVA = iPartialPercFEE + iPartialFixedFEE;

            //Verificar en la tabla de CAMPA si existe el IDOperation
            int iLayout = oOperation.INSTALLATION.INS_FEE_LAYOUT;
            int iLayoutCampaign= customersRepository.GetLayoutCampaignOperationByOperationId(oOperation.OPE_ID);
            if ((iLayoutCampaign != -1)&&(string.IsNullOrEmpty(oOperation.INSTALLATION.INS_BSM_CITY_ID)))
            {
                iLayout = iLayoutCampaign;
            }

            sb.AppendFormat("<q>{0}</q>" +
                            "<layout>{1}</layout>" +
                            "<q_fee>{2}</q_fee>" +
                            "<q_vat>{3}</q_vat>" +
                            "<q_subtotal>{4}</q_subtotal>" +
                            "<q_total>{5}</q_total>" +
                //"<real_q>850</real_q>" +
                            "<q_plus_vat>{6}</q_plus_vat>" +
                            "<q_fee_plus_vat>{7}</q_fee_plus_vat>" +
                            "<q_without_bon>{8}</q_without_bon>" +
                            "<per_bon>{9}</per_bon>" +
                            "<t>{10}</t>",
                            iAmount,
                            iLayout,
                            iAmountFEE,
                            iAmountVAT,
                            iSubTotalAmount,
                            iTotalAmount,
                            iQPlusIVA,
                            iFeePlusIVA,
                            iAmountWithoutBon,
                            oOperation.OPE_BON_MLT.HasValue ? oOperation.OPE_BON_MLT.Value.ToString(numberFormatProvider) : "0",
                            iTime);

            if (!string.IsNullOrEmpty(oOperation.OPE_VEHICLE_TYPE))
                sb.AppendFormat("<vehicletype>{0}</vehicletype>", oOperation.OPE_VEHICLE_TYPE);

            if (oOperation.INSTALLATION.CURRENCy.CUR_ISO_CODE != infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)))
            {
                double dChangeToApply = 1.0;
                double dChangeFee = 0;

                dChangeToApply = Convert.ToDouble(oOperation.OPE_CHANGE_APPLIED);/// GetChangeToApplyFromInstallationCurToUserCur(oOperation.INSTALLATION, oUser);
                if (dChangeToApply >= 0)
                {

                    string sChng = dChangeToApply.ToString(numberFormatProvider);

                    int iQChange = ChangeQuantityFromInstallationCurToUserCur(iAmount,
                                    dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);

                    int iAmountFEEChange = ChangeQuantityFromInstallationCurToUserCur(iAmountFEE, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                    int iAmountVATChange = ChangeQuantityFromInstallationCurToUserCur(iAmountVAT, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                    int iSubTotalAmountChange = ChangeQuantityFromInstallationCurToUserCur(iSubTotalAmount, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                    int iTotalAmountChange = ChangeQuantityFromInstallationCurToUserCur(iTotalAmount, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                    int iQPlusIVAChange = ChangeQuantityFromInstallationCurToUserCur(iQPlusIVA, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                    int iFeePlusIVAChange = ChangeQuantityFromInstallationCurToUserCur(iFeePlusIVA, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);

                    sb.AppendFormat("<chng>{0}</chng>" +
                                    "<qch>{1}</qch>" +
                                    "<qch_fee>{2}</qch_fee>" +
                                    "<qch_vat>{3}</qch_vat>" +
                                    "<qch_subtotal>{4}</qch_subtotal>" +
                                    "<qch_total>{5}</qch_total>" +
                                    "<qch_plus_vat>{6}</qch_plus_vat>" +
                                    "<qch_fee_plus_vat>{7}</qch_fee_plus_vat>",
                                    sChng,
                                    iQChange,
                                    iAmountFEEChange,
                                    iAmountVATChange,
                                    iSubTotalAmountChange,
                                    iTotalAmountChange,
                                    iQPlusIVAChange,
                                    iFeePlusIVAChange);

                }
            }

            sb.AppendFormat("<ServiceParkingLbl>{0}</ServiceParkingLbl>" +
                            "<ServiceFeeLbl>{1}</ServiceFeeLbl>" +
                            "<ServiceFeeVATLbl>{2}</ServiceFeeVATLbl>" +
                            "<ServiceVATLbl>{3}</ServiceVATLbl>" +
                            "<ServiceTotalLbl>{4}</ServiceTotalLbl>" +
                            "<ServiceParkingBaseLbl>{5}</ServiceParkingBaseLbl>" +
                            "<ServiceParkingVariableLbl>{6}</ServiceParkingVariableLbl>",
                            infraestructureRepository.GetLiteral(oOperation.INSTALLATION.INS_SERVICE_PARK_LIT_ID ?? 0, strCulture),
                            infraestructureRepository.GetLiteral(oOperation.INSTALLATION.INS_SERVICE_FEE_LIT_ID ?? 0, strCulture),
                            infraestructureRepository.GetLiteral(oOperation.INSTALLATION.INS_SERVICE_FEE_PLUS_VAT_LIT_ID ?? 0, strCulture),
                            infraestructureRepository.GetLiteral(oOperation.INSTALLATION.INS_SERVICE_VAT_LIT_ID ?? 0, strCulture),
                            infraestructureRepository.GetLiteral(oOperation.INSTALLATION.INS_SERVICE_TOTAL_LIT_ID ?? 0, strCulture),
                            infraestructureRepository.GetLiteral(oOperation.INSTALLATION.INS_SERVICE_PARK_BASE_LIT_ID ?? 0, strCulture),
                            infraestructureRepository.GetLiteral(oOperation.INSTALLATION.INS_SERVICE_PARK_VARIABLE_LIT_ID ?? 0, strCulture));


            sb.AppendFormat("<ReceiptHeaderLbl>{0}</ReceiptHeaderLbl>", infraestructureRepository.GetLiteralFromKey(string.Format("{0}_{1}", Convert.ToInt32(oOperation.INSTALLATION.INS_ID), "RECEIPT_HEADER"), strCulture));
            sb.AppendFormat("<ExternalID>{0}</ExternalID>", oOperation.OPE_EXTERNAL_BASE_ID1);

            if ((ParkWSSignatureType)oOperation.INSTALLATION.INS_PARK_WS_SIGNATURE_TYPE == ParkWSSignatureType.pst_bsm &&
                !string.IsNullOrEmpty(oOperation.OPE_ADDITIONAL_PARAMS))
            {
                BSMConfiguration oBSMConfiguration = null;
                try
                {
                    oBSMConfiguration = (BSMConfiguration)JsonConvert.DeserializeObject(oOperation.OPE_ADDITIONAL_PARAMS, typeof(BSMConfiguration));
                }
                catch (Exception) { }

                if (oBSMConfiguration != null)
                {
                    sb.AppendFormat("<ServiceParkingBaseQuantityLbl>{0}</ServiceParkingBaseQuantityLbl>", oBSMConfiguration.parkingBaseQuantityLbl);
                    sb.AppendFormat("<ServiceParkingVariableQuantityLbl>{0}</ServiceParkingVariableQuantityLbl>", oBSMConfiguration.parkingVariableQuantityLbl);
                }
                
                if (oBSMConfiguration != null && oBSMConfiguration.Modifiers != null && oBSMConfiguration.Modifiers.Modifiers != null && oBSMConfiguration.Modifiers.Modifiers.Any())
                {
                    sb.AppendFormat("<modifiers>{0}</modifiers>", oBSMConfiguration.Modifiers.ToCustomXml());
                }

            }
            else if (!string.IsNullOrEmpty(oOperation.OPE_ADDITIONAL_PARAMS))
            {

                dynamic oParams = JsonConvert.DeserializeObject(oOperation.OPE_ADDITIONAL_PARAMS);

                var oRDetail = oParams["rdetail"];

                if (oRDetail != null)
                {
                    int iRDetail = Convert.ToInt32(oRDetail);
                    string strMessage = infraestructureRepository.GetCustomErrorMessageForOperation(oOperation.OPE_DATE, iRDetail, oOperation.OPE_GRP_ID.Value, oOperation.OPE_TAR_ID, strCulture);
                    sb.AppendFormat("<ParkingWarningLbl>{0}</ParkingWarningLbl>", strMessage);
                }
            }







            sb.AppendFormat("<g_id>{0}</g_id>" +
                            "<ad_id>{1}</ad_id>" +
                            "<isshopkeeperoperation>{2}</isshopkeeperoperation>",
                            oOperation.OPE_GRP_ID ?? 0,
                            oOperation.OPE_TAR_ID ?? 0,
                            oOperation.OPE_SHOPKEEPER_OP ?? 0);

            if (oOperation.OPE_STRSE_ID.HasValue)
                sb.AppendFormat("<sts_id>{0}</sts_id>", oOperation.OPE_STRSE_ID.Value);

            bool bStop = false;
            bool bExtension = false;
            bool bRefund = false;
            bool bRepeat = false;

            if (oOperation.TARIFF != null)
            {
                bStop = (oOperation.OPE_PARKING_MODE == (int)ParkingMode.StartStop &&
                         oOperation.OPE_PARKING_MODE_STATUS == (int)ParkingModeStatus.Opened);

                if (oOperation.OPE_PARKING_MODE != (int)ParkingMode.StartStop &&
                    oOperation.OPE_UTC_ENDDATE >= DateTime.UtcNow)
                {
                    var oLastOp = oListOperations.Where(r => !oRemovedOps.Contains(r.OPE_ID) && r.OPE_EXTERNAL_BASE_ID1 == oOperation.OPE_EXTERNAL_BASE_ID1).OrderByDescending(r => r.OPE_UTC_DATE).FirstOrDefault();
                    if (oLastOp != null && oLastOp.OPE_ID == oOperation.OPE_ID)
                    {
                        bExtension = (oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation ||
                                        oOperation.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation ||
                                        oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund);
                        bRefund = (oOperation.INSTALLATION.INS_OPT_UNPARK == 1 &&
                                    (oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation ||
                                    oOperation.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation));
                    }
                }


                bRepeat = !bStop && !bExtension && !bRefund &&
                        (oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation || oOperation.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation ||
                            (oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund && oOperation.OPE_PARKING_MODE == (int)ParkingMode.StartStop));
            }

            sb.AppendFormat("<stop>{0}</stop>" +
                            "<extension>{1}</extension>" +
                            "<refund>{2}</refund>"+
                            "<repeat>{3}</repeat>",
                            bStop ? 1 : 0,
                            bExtension ? 1 : 0,
                            bRefund ? 1 : 0,
                            bRepeat ? 1 : 0                           
                            );



           
            if ((oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund && oOperation.OPE_PARKING_MODE == (int)ParkingMode.StartStop))
            {
                IQueryable<OPERATION> oStartStopOperations = null;
                if (customersRepository.GetOperationsByExternalBaseId(1, oOperation.OPE_EXTERNAL_BASE_ID1, out oStartStopOperations))
                {
                    var vFirstOp = oStartStopOperations.Where(r => r.OPE_EXTERNAL_BASE_ID1 == oOperation.OPE_EXTERNAL_BASE_ID1).OrderBy(r => r.OPE_UTC_DATE).FirstOrDefault();

                    if (vFirstOp != null && vFirstOp.OPE_LATITUDE.HasValue && vFirstOp.OPE_LONGITUDE.HasValue)
                    {
                        sb.AppendFormat("<gps_lat>{0}</gps_lat>" +
                                        "<gps_lon>{1}</gps_lon>",
                                        vFirstOp.OPE_LATITUDE.Value.ToString(CultureInfo.InvariantCulture),
                                        vFirstOp.OPE_LONGITUDE.Value.ToString(CultureInfo.InvariantCulture));
                    }
                }
          
            }
            else
            {

                if (oOperation.OPE_LATITUDE.HasValue && oOperation.OPE_LONGITUDE.HasValue)
                {

                    sb.AppendFormat("<gps_lat>{0}</gps_lat>" +
                                    "<gps_lon>{1}</gps_lon>",
                                    oOperation.OPE_LATITUDE.Value.ToString(CultureInfo.InvariantCulture),
                                    oOperation.OPE_LONGITUDE.Value.ToString(CultureInfo.InvariantCulture));
                }
            }

            


            return sb.ToString();
        }

        private void OperationsRemoveStarts(ref List<ALL_OPERATION> oOperations)
        {
            List<decimal> oRemoveStarts = new List<decimal>();

            HIS_OPERATION oStopHisOp = null;
            foreach (ALL_OPERATION oStopOp in oOperations.Where(o => o.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund))
            {
                if (customersRepository.GetOperationData(oStopOp.OPE_ID, out oStopHisOp))
                {
                    if (oStopHisOp.OPE_PARKING_MODE == (int)ParkingMode.StartStop)
                    {
                        OPERATION oStartOp = null;
                        if (!string.IsNullOrEmpty(oStopHisOp.OPE_EXTERNAL_BASE_ID1))
                            customersRepository.GetStartOperationByExternalBaseId(1, oStopHisOp.OPE_EXTERNAL_BASE_ID1, out oStartOp);
                        if (oStartOp == null && !string.IsNullOrEmpty(oStopHisOp.OPE_EXTERNAL_BASE_ID2))
                            customersRepository.GetStartOperationByExternalBaseId(2, oStopHisOp.OPE_EXTERNAL_BASE_ID2, out oStartOp);
                        if (oStartOp == null && !string.IsNullOrEmpty(oStopHisOp.OPE_EXTERNAL_BASE_ID3))
                            customersRepository.GetStartOperationByExternalBaseId(3, oStopHisOp.OPE_EXTERNAL_BASE_ID3, out oStartOp);
                        if (oStartOp != null)
                        {
                            oRemoveStarts.Add(oStartOp.OPE_ID);
                        }
                    }
                    else if (oStopHisOp.OPE_PARKING_MODE == (int)ParkingMode.StartStopHybrid)
                    {
                        List<OPERATION> oStartExtsOps = new List<OPERATION>();
                        if (!string.IsNullOrEmpty(oStopHisOp.OPE_EXTERNAL_BASE_ID1))
                            customersRepository.GetStartAndExtensionsOperationByExternalBaseId(1, oStopHisOp.OPE_EXTERNAL_BASE_ID1, out oStartExtsOps);
                        if (!oStartExtsOps.Any() && !string.IsNullOrEmpty(oStopHisOp.OPE_EXTERNAL_BASE_ID2))
                            customersRepository.GetStartAndExtensionsOperationByExternalBaseId(2, oStopHisOp.OPE_EXTERNAL_BASE_ID2, out oStartExtsOps);
                        if (!oStartExtsOps.Any() && !string.IsNullOrEmpty(oStopHisOp.OPE_EXTERNAL_BASE_ID3))
                            customersRepository.GetStartAndExtensionsOperationByExternalBaseId(3, oStopHisOp.OPE_EXTERNAL_BASE_ID3, out oStartExtsOps);
                        if (oStartExtsOps.Any())
                        {
                            oRemoveStarts.AddRange(oStartExtsOps.Select(o => o.OPE_ID).ToList());
                        }
                    }
                }
            }

            oOperations.RemoveAll(o => oRemoveStarts.Contains(o.OPE_ID));
        }

        private string QueryParkingOperationButtons(SortedList oPars, List<StandardQueryParkingStep> oSteps, TARIFFS_IN_GROUP oTariffInGroup, ulong ulAppVersion, string strCulture, out int? iLayout)
        {
            string sButtonsXml = "";
            iLayout = null;

            List<ParkingOperationButton> oButtons = null;
            ParkingOperationButtonsList oButtonsList = null;

            if (oPars["buttons"] != null)
            {
                oButtonsList = ParkingOperationButtonsList.FromCustomXml(oPars["buttons"].ToString());
                if (oButtonsList != null)
                    oButtons = oButtonsList.Buttons.ToList();
            }

            string sLiteral = "";

            if (oButtons != null)
            {
                if (ulAppVersion < _VERSION_2_7_1 && oButtons.Count > 3)
                {
                    oButtons.RemoveRange(3, oButtons.Count - 3);
                }

                List<string> oButtonsToRemove = new List<string>();

                oButtons.ForEach(button =>
                {
                    button.Literal = infraestructureRepository.GetLiteralFromKey(button.Id, strCulture);
                    if (!string.IsNullOrEmpty(button.Literal))
                    {
                        if (oSteps != null && oSteps.Any() &&
                            (button.Type == ParkingOperationButtonType.RateStep ||
                             button.Type == ParkingOperationButtonType.RateMaximum ||
                             button.Type == ParkingOperationButtonType.Reset))
                        {
                            if ((button.Type == ParkingOperationButtonType.RateStep)|| (button.Type == ParkingOperationButtonType.Reset))
                            {
                                var oStep = oSteps.Where(step => step.Time == button.Minutes).FirstOrDefault();
                                if (oStep != null)
                                {
                                    button.Literal = string.Format(new System.Globalization.CultureInfo(strCulture), button.Literal, Convert.ToDouble(oStep.QPlusIVA) / 100);
                                }
                                else
                                    oButtonsToRemove.Add(button.Id);


                                if (button.Type == ParkingOperationButtonType.Reset)
                                {
                                    button.Type = ParkingOperationButtonType.RateStep;
                                }
                                
                            }
                            else if (button.Type == ParkingOperationButtonType.RateMaximum)
                            {
                                var oStep = oSteps.Last();
                                if (oStep != null)
                                {
                                    button.Literal = string.Format(new System.Globalization.CultureInfo(strCulture), button.Literal, Convert.ToDouble(oStep.QPlusIVA) / 100, oStep.Dt);
                                }
                            }
                        }
                        
                    }
                    else
                        button.Literal = button.Id;
                });

                oButtons.RemoveAll(button => string.IsNullOrEmpty(button.Literal) || oButtonsToRemove.Contains(button.Id));

                if (ulAppVersion < _VERSION_2_7_1)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (oButtons.Count > i)
                        {
                            oPars[string.Format("btn{0}step", i + 1)] = oButtons[i].Minutes;
                            oPars[string.Format("btn{0}steplit", i + 1)] = oButtons[i].Literal;
                        }
                        else
                        {
                            oPars[string.Format("btn{0}step", i + 1)] = "";
                            oPars[string.Format("btn{0}steplit", i + 1)] = "";
                        }
                    }
                    oButtons = null;
                }                
            }
            else
            {
                if (ulAppVersion < _VERSION_2_7_1)
                {
                    if (oTariffInGroup != null)
                    {
                        oPars["btn1step"] = (oTariffInGroup.TARGR_STEP1_MIN.HasValue ? oTariffInGroup.TARGR_STEP1_MIN.Value.ToString() : "");
                        if (oTariffInGroup.TARGR_STEP1_LIT_ID.HasValue)
                            oPars["btn1steplit"] = infraestructureRepository.GetLiteral(oTariffInGroup.TARGR_STEP1_LIT_ID.Value, strCulture);
                        else
                            oPars["btn1steplit"] = "";
                        oPars["btn2step"] = (oTariffInGroup.TARGR_STEP2_MIN.HasValue ? oTariffInGroup.TARGR_STEP2_MIN.Value.ToString() : "");
                        if (oTariffInGroup.TARGR_STEP2_LIT_ID.HasValue)
                            oPars["btn2steplit"] = infraestructureRepository.GetLiteral(oTariffInGroup.TARGR_STEP2_LIT_ID.Value, strCulture);
                        else
                            oPars["btn2steplit"] = "";
                        oPars["btn3step"] = (oTariffInGroup.TARGR_STEP3_MIN.HasValue ? oTariffInGroup.TARGR_STEP3_MIN.Value.ToString() : "");
                        if (oTariffInGroup.TARGR_STEP3_LIT_ID.HasValue)
                        {
                            sLiteral = infraestructureRepository.GetLiteral(oTariffInGroup.TARGR_STEP3_LIT_ID.Value, strCulture);
                            if (oSteps != null)
                            {
                                if (oSteps.Any())
                                    sLiteral = string.Format(new System.Globalization.CultureInfo(strCulture), sLiteral, Convert.ToDouble(oSteps.Last().QPlusIVA) / 100);
                                else
                                    sLiteral = string.Format(new System.Globalization.CultureInfo(strCulture), sLiteral, 0);
                            }
                            oPars["btn3steplit"] = sLiteral;
                        }
                        else
                            oPars["btn3steplit"] = "";
                    }
                }
                else if ((ulAppVersion < _VERSION_3_0) || ((ulAppVersion >= _VERSION_3_0) && (oTariffInGroup != null) && (oTariffInGroup.TARGR_LAYOUT>0)))
                {
                    oButtons = new List<ParkingOperationButton>();

                    if (oTariffInGroup != null)
                    {
                        // Increments buttons                                    
                        if (oTariffInGroup.TARGR_STEP1_ENABLED == 1 && oTariffInGroup.TARGR_STEP1_LIT_ID.HasValue)
                        {
                            oButtons.Add(new ParkingOperationButton()
                            {
                                Type = ParkingOperationButtonType.Increment,
                                Minutes = oTariffInGroup.TARGR_STEP1_MIN,
                                Literal = infraestructureRepository.GetLiteral(oTariffInGroup.TARGR_STEP1_LIT_ID.Value, strCulture)
                            });
                        }
                        if (oTariffInGroup.TARGR_STEP2_ENABLED == 1 && oTariffInGroup.TARGR_STEP2_LIT_ID.HasValue)
                        {
                            oButtons.Add(new ParkingOperationButton()
                            {
                                Type = ParkingOperationButtonType.Increment,
                                Minutes = oTariffInGroup.TARGR_STEP2_MIN,
                                Literal = infraestructureRepository.GetLiteral(oTariffInGroup.TARGR_STEP2_LIT_ID.Value, strCulture)
                            });
                        }
                        if (oTariffInGroup.TARGR_STEP3_ENABLED == 1 && oTariffInGroup.TARGR_STEP3_LIT_ID.HasValue)
                        {
                            oButtons.Add(new ParkingOperationButton()
                            {
                                Type = ParkingOperationButtonType.Increment,
                                Minutes = oTariffInGroup.TARGR_STEP3_MIN,
                                Literal = infraestructureRepository.GetLiteral(oTariffInGroup.TARGR_STEP3_LIT_ID.Value, strCulture)
                            });
                        }
                        if (oTariffInGroup.TARGR_STEP4_ENABLED == 1 && oTariffInGroup.TARGR_STEP4_LIT_ID.HasValue)
                        {
                            oButtons.Add(new ParkingOperationButton()
                            {
                                Type = ParkingOperationButtonType.Increment,
                                Minutes = oTariffInGroup.TARGR_STEP4_MIN,
                                Literal = infraestructureRepository.GetLiteral(oTariffInGroup.TARGR_STEP4_LIT_ID.Value, strCulture)
                            });
                        }
                        if (oTariffInGroup.TARGR_STEP5_ENABLED == 1 && oTariffInGroup.TARGR_STEP5_LIT_ID.HasValue)
                        {
                            oButtons.Add(new ParkingOperationButton()
                            {
                                Type = ParkingOperationButtonType.Increment,
                                Minutes = oTariffInGroup.TARGR_STEP5_MIN,
                                Literal = infraestructureRepository.GetLiteral(oTariffInGroup.TARGR_STEP5_LIT_ID.Value, strCulture)
                            });
                        }
                        if (oTariffInGroup.TARGR_STEP6_ENABLED == 1 && oTariffInGroup.TARGR_STEP6_LIT_ID.HasValue)
                        {
                            oButtons.Add(new ParkingOperationButton()
                            {
                                Type = ParkingOperationButtonType.Increment,
                                Minutes = oTariffInGroup.TARGR_STEP6_MIN,
                                Literal = infraestructureRepository.GetLiteral(oTariffInGroup.TARGR_STEP6_LIT_ID.Value, strCulture)
                            });
                        }
                        if (oTariffInGroup.TARGR_STEP7_ENABLED == 1 && oTariffInGroup.TARGR_STEP7_LIT_ID.HasValue)
                        {
                            oButtons.Add(new ParkingOperationButton()
                            {
                                Type = ParkingOperationButtonType.Increment,
                                Minutes = oTariffInGroup.TARGR_STEP7_MIN,
                                Literal = infraestructureRepository.GetLiteral(oTariffInGroup.TARGR_STEP7_LIT_ID.Value, strCulture)
                            });
                        }
                        if (oTariffInGroup.TARGR_STEP8_ENABLED == 1 && oTariffInGroup.TARGR_STEP8_LIT_ID.HasValue)
                        {
                            oButtons.Add(new ParkingOperationButton()
                            {
                                Type = ParkingOperationButtonType.Increment,
                                Minutes = oTariffInGroup.TARGR_STEP8_MIN,
                                Literal = infraestructureRepository.GetLiteral(oTariffInGroup.TARGR_STEP8_LIT_ID.Value, strCulture)
                            });
                        }
                        if (oTariffInGroup.TARGR_STEP9_ENABLED == 1 && oTariffInGroup.TARGR_STEP9_LIT_ID.HasValue)
                        {
                            oButtons.Add(new ParkingOperationButton()
                            {
                                Type = ParkingOperationButtonType.Increment,
                                Minutes = oTariffInGroup.TARGR_STEP9_MIN,
                                Literal = infraestructureRepository.GetLiteral(oTariffInGroup.TARGR_STEP9_LIT_ID.Value, strCulture)
                            });
                        }
                    }

                    // Rate steps buttons
                    if (oSteps != null)
                    {
                        foreach (var oStep in oSteps.Where(s => !string.IsNullOrEmpty(s.Label)))
                        {
                            sLiteral = infraestructureRepository.GetLiteralFromKey(oStep.Label, strCulture);
                            if (!string.IsNullOrEmpty(sLiteral))
                            {
                                sLiteral = string.Format(new System.Globalization.CultureInfo(strCulture), sLiteral, Convert.ToDouble(oStep.QPlusIVA) / 100);
                                oButtons.Add(new ParkingOperationButton()
                                {
                                    Type = ParkingOperationButtonType.RateStep,
                                    Minutes = oStep.Time,
                                    Literal = sLiteral
                                });
                            }
                        }
                    }

                    if (oTariffInGroup != null)
                    {
                        // Rate max button
                        if (oTariffInGroup.TARGR_STEP10_ENABLED == 1 && oTariffInGroup.TARGR_STEP10_LIT_ID.HasValue)
                        {
                            sLiteral = infraestructureRepository.GetLiteral(oTariffInGroup.TARGR_STEP10_LIT_ID.Value, strCulture);
                            if (oSteps != null)
                            {
                                if (oSteps.Any())
                                    sLiteral = string.Format(new System.Globalization.CultureInfo(strCulture), sLiteral, Convert.ToDouble(oSteps.Last().QPlusIVA) / 100);
                                else
                                    sLiteral = string.Format(new System.Globalization.CultureInfo(strCulture), sLiteral, 0);
                            }
                            oButtons.Add(new ParkingOperationButton()
                            {
                                Type = ParkingOperationButtonType.RateMaximum,
                                Minutes = oTariffInGroup.TARGR_STEP10_MIN,
                                Literal = sLiteral

                            });
                        }
                    }
                }
            }

            if (oButtons != null)
            {
                oButtonsList = new ParkingOperationButtonsList()
                {
                    Buttons = oButtons.ToArray()
                };
                sButtonsXml = oButtonsList.ToCustomXml();

                if (oButtons.Count >= 6)
                    iLayout = 3; // -----  ------
                                 // -----  ------
                                 // -----  ------
                else if (oButtons.Count >= 4)
                    iLayout = 2; // -----  ------
                                 // -----  ------
                                 // -------------
                else if (oButtons.Count > 0)
                    iLayout = 1; // -------------
                                 // -------------
                                 // -------------
                else 
                    iLayout = 0;
            }
            else
            {
                iLayout = 0;
            }

            //if (oTariffInGroup != null && oTariffInGroup.TARGR_LAYOUT.HasValue)
            //    oPars["buttons_layout"] = oTariffInGroup.TARGR_LAYOUT.Value;

            return sButtonsXml;
        }

        private string GetUserCurrentParkingOperations(ref USER oUser, decimal dInsId, string strCulture, ulong ulAppVersion, Stopwatch watch=null)
        {
            StringBuilder sb = new StringBuilder();

            DateTime? dInsDateTime = geograficAndTariffsRepository.getInstallationDateTime(dInsId);
            INSTALLATION oInst=null;


            string strTodayColour = infraestructureRepository.GetParameterValue("ParkingScreenTodayColour");
            string strTomorrowColour = infraestructureRepository.GetParameterValue("ParkingScreenTomorrowColour");
            string strMoreThanTomorrowColour = infraestructureRepository.GetParameterValue("ParkingScreenMoreThanTomorrowColour");


            geograficAndTariffsRepository.getInstallation(dInsId,
                                                        null,
                                                        null,
                                                        ref oInst,
                                                        ref dInsDateTime);

            List<decimal> oChildInstallations = geograficAndTariffsRepository.GetChildInstallationsIds(dInsId, dInsDateTime, true);


            var oCurrentParkingOperations = customersRepository.GetUserCurrentParkings(ref oUser, oChildInstallations)
                                                               .ToList();

            if (oCurrentParkingOperations.Any())
            {


                ParkingTags oConsolidateParking;
                List<OPERATION> oParkings = null;
                OPERATION oFirst;
                OPERATION oLast;
                string sPlate = "";
                string sGrpDescription = "";
                string sStsDescription = "";
                string sTarDescription = "";                
                string sOpeRefundPreviousEndDate = "";
                int iPostPay = 0;
                int iTariffBehavior = 0;

                NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                numberFormatProvider.NumberDecimalSeparator = ".";

                var oExternalBaseIDs = oCurrentParkingOperations.Where(o => !string.IsNullOrEmpty(o.OPE_EXTERNAL_BASE_ID1 ?? o.OPE_EXTERNAL_ID1)) 
                                                                .GroupBy(o => (o.OPE_EXTERNAL_BASE_ID1 ?? o.OPE_EXTERNAL_ID1),
                                                                         (k, o) => new { ExternalBaseID = k, MinDate = o.Select(t => t.OPE_DATE).Min() })
                                                                .ToList();



                Dictionary<string, List<decimal>> oVirtualExternalBaseIDs = new Dictionary<string, List<decimal>>();

                var oNoBaseIdParkings = oCurrentParkingOperations.Where(o => string.IsNullOrEmpty(o.OPE_EXTERNAL_BASE_ID1 ?? o.OPE_EXTERNAL_ID1))
                                                                 .OrderBy(o => o.OPE_UTC_DATE)
                                                                 .ToList();


                System.Random rand = new System.Random(DateTime.UtcNow.GetHashCode());


                foreach (OPERATION oNoBaseOpe in oNoBaseIdParkings)
                {
                    if (oNoBaseOpe.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation)
                    {                        
                        string sBaseId = string.Format("###{0}{1}###", DateTime.UtcNow.Ticks, rand.Next());
                        oVirtualExternalBaseIDs.Add(sBaseId, new List<decimal>() { oNoBaseOpe.OPE_ID });
                        oExternalBaseIDs.Add(new { ExternalBaseID = sBaseId, MinDate = oNoBaseOpe.OPE_DATE });
                    }
                    else if (oNoBaseOpe.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation)
                    {
                        var oOpe = oCurrentParkingOperations.Where(o => o.OPE_ENDDATE == oNoBaseOpe.OPE_INIDATE &&
                                                                         o.USER_PLATE != null && oNoBaseOpe.USER_PLATE != null && o.USER_PLATE.USRP_PLATE == oNoBaseOpe.USER_PLATE.USRP_PLATE &&
                                                                         o.OPE_TAR_ID == oNoBaseOpe.OPE_TAR_ID &&
                                                                         (o.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation || o.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation || o.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund))
                                                            .FirstOrDefault();
                        if (oOpe != null)
                        {
                            if (!string.IsNullOrEmpty(oOpe.OPE_EXTERNAL_BASE_ID1))
                            {                                
                                if (!oVirtualExternalBaseIDs.ContainsKey(oOpe.OPE_EXTERNAL_BASE_ID1)) 
                                    oVirtualExternalBaseIDs.Add(oOpe.OPE_EXTERNAL_BASE_ID1, new List<decimal>());
                                oVirtualExternalBaseIDs[oOpe.OPE_EXTERNAL_BASE_ID1].Add(oNoBaseOpe.OPE_ID);
                            }
                            else
                            {                             
                                foreach (string sBaseId in oVirtualExternalBaseIDs.Keys)
                                {
                                    if (oVirtualExternalBaseIDs[sBaseId].Contains(oOpe.OPE_ID))
                                    {
                                        oVirtualExternalBaseIDs[sBaseId].Add(oNoBaseOpe.OPE_ID);
                                        break;
                                    }
                                }                                                              
                            }
                        }
                        else
                        {
                            string sBaseId = string.Format("###{0}{1}###", DateTime.UtcNow.Ticks, rand.Next());
                            oVirtualExternalBaseIDs.Add(sBaseId, new List<decimal>() { oNoBaseOpe.OPE_ID });
                            oExternalBaseIDs.Add(new { ExternalBaseID = sBaseId, MinDate = oNoBaseOpe.OPE_DATE });
                        }
                    }
                    else if (oNoBaseOpe.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund)
                    {
                        var oOpe = oCurrentParkingOperations.Where(o => o.OPE_INIDATE == oNoBaseOpe.OPE_INIDATE && o.OPE_ID != oNoBaseOpe.OPE_ID &&
                                                                         o.USER_PLATE != null && oNoBaseOpe.USER_PLATE != null && o.USER_PLATE.USRP_PLATE == oNoBaseOpe.USER_PLATE.USRP_PLATE &&
                                                                         o.OPE_TAR_ID == oNoBaseOpe.OPE_TAR_ID &&
                                                                         (o.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation || o.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation))
                                                            .FirstOrDefault();
                        if (oOpe != null)
                        {
                            if (!string.IsNullOrEmpty(oOpe.OPE_EXTERNAL_BASE_ID1))
                            {                                
                                if (!oVirtualExternalBaseIDs.ContainsKey(oOpe.OPE_EXTERNAL_BASE_ID1))
                                    oVirtualExternalBaseIDs.Add(oOpe.OPE_EXTERNAL_BASE_ID1, new List<decimal>());
                                oVirtualExternalBaseIDs[oOpe.OPE_EXTERNAL_BASE_ID1].Add(oNoBaseOpe.OPE_ID);
                            }
                            else
                            {
                                bool bAdded = false;
                                foreach (string sBaseId in oVirtualExternalBaseIDs.Keys)
                                {
                                    if (oVirtualExternalBaseIDs[sBaseId].Contains(oOpe.OPE_ID))
                                    {
                                        oVirtualExternalBaseIDs[sBaseId].Add(oNoBaseOpe.OPE_ID);
                                        bAdded = true;
                                        break;
                                    }
                                }

                                if (!bAdded)
                                {
                                    string sNewBaseId = string.Format("###{0}{1}###", DateTime.UtcNow.Ticks, rand.Next());
                                    oVirtualExternalBaseIDs.Add(sNewBaseId, new List<decimal>() { oNoBaseOpe.OPE_ID });
                                    oExternalBaseIDs.Add(new { ExternalBaseID = sNewBaseId, MinDate = oNoBaseOpe.OPE_DATE });
                                }
                            }
                        }
                        else
                        {
                            string sBaseId = string.Format("###{0}{1}###", DateTime.UtcNow.Ticks, rand.Next());
                            oVirtualExternalBaseIDs.Add(sBaseId, new List<decimal>() { oNoBaseOpe.OPE_ID });
                            oExternalBaseIDs.Add(new { ExternalBaseID = sBaseId, MinDate = oNoBaseOpe.OPE_DATE });
                        }

                    }

                }


                foreach (string sExternalBaseID in oExternalBaseIDs.OrderBy(o => o.MinDate).Select(e => e.ExternalBaseID))
                {


                    oParkings = oCurrentParkingOperations.Where(o => o.OPE_EXTERNAL_BASE_ID1 == sExternalBaseID || o.OPE_EXTERNAL_ID1 == sExternalBaseID)
                                                         .ToList();
                    if (oVirtualExternalBaseIDs.ContainsKey(sExternalBaseID))
                        oParkings = oParkings.Union(oCurrentParkingOperations.Where(o => oVirtualExternalBaseIDs[sExternalBaseID].Contains(o.OPE_ID)))                                             
                                             .ToList();
                    oParkings = oParkings.OrderBy(o => o.OPE_DATE).ToList();


                    
                    if (oParkings.Any())
                    {                       
                        oConsolidateParking = new ParkingTags();
                        for (int i = 0; i < oParkings.Count; i += 1)
                        {
                            ParkingConsolidate(i, oParkings, ref oConsolidateParking);
                        }

                        oFirst = oParkings.First();
                        oLast = oParkings.Last();

                        sPlate = "";
                        if (oFirst.USER_PLATE != null)
                            sPlate = oFirst.USER_PLATE.USRP_PLATE;

                        sGrpDescription = "";
                        if (oFirst.GROUP != null)
                            sGrpDescription = oFirst.GROUP.GRP_DESCRIPTION;

                        sStsDescription = "";
                        if (oFirst.STREET_SECTION != null)
                            sStsDescription = oFirst.STREET_SECTION.STRSE_DESCRIPTION;
                        
                        sTarDescription = "";
                        iTariffBehavior = 0;
                        if (oFirst.TARIFF != null)
                        {
                            sTarDescription = oFirst.TARIFF.TAR_DESCRIPTION;
                            iTariffBehavior = (oFirst.TARIFF.TAR_BEHAVIOR ?? 0);
                        }

                        
                        sOpeRefundPreviousEndDate = "";
                        if (oLast.OPE_REFUND_PREVIOUS_ENDDATE.HasValue)
                        {
                            sOpeRefundPreviousEndDate = oLast.OPE_REFUND_PREVIOUS_ENDDATE.Value.ToString("HHmmssddMMyy");
                        }

                        iPostPay = 0;
                        if (oParkings.Where(o => o.OPE_POSTPAY == 1).Any())
                            iPostPay = 1;


                    
                        if (dInsDateTime.Value < oLast.OPE_ENDDATE && oLast.OPE_INIDATE != oLast.OPE_ENDDATE)
                        {
                            sb.Append("<userpark json:Array='true'>");

                            if (ulAppVersion >= _VERSION_3_4)
                            {
                                string ccpan = "";
                                int? cctype = null;
                                int? ccprovider = null;

                                if ((PaymentSuscryptionType)oFirst.OPE_SUSCRIPTION_TYPE == PaymentSuscryptionType.pstPerTransaction)
                                {
                                    if (oFirst.CUSTOMER_PAYMENT_MEANS_RECHARGE != null)
                                    {
                                        if ((oFirst.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSTOMER_PAYMENT_MEAN != null) &&
                                            ((PaymentMeanType)oFirst.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID == PaymentMeanType.pmtDebitCreditCard))
                                        {
                                            ccpan = oFirst.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_MASKED_CARD_NUMBER; //oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_TOKEN_MASKED_CARD_NUMBER;
                                            cctype = (int)customersRepository.GetCreditCardType(oFirst.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSTOMER_PAYMENT_MEAN);
                                            ccprovider = oFirst.CUSTOMER_PAYMENT_MEANS_RECHARGE.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG?.CPTGC_PROVIDER;  //oUser.CUSTOMER_PAYMENT_MEAN.CURRENCIES_PAYMENT_TYPE_GATEWAY_CONFIG?.CPTGC_PROVIDER;
                                        }
                                    }
                                }


                                int iTotalDays = Convert.ToInt32((oLast.OPE_ENDDATE.Date - dInsDateTime.Value.Date).TotalDays);
                                if (iTotalDays < 0)
                                    iTotalDays = 0;


                                sb.AppendFormat("<p>{0}</p>" +
                                                "<cityID>{1}</cityID>" +
                                                "<cityShortDesc>{2}</cityShortDesc>" +
                                                "<g>{3}</g>" +
                                                "<sts>{29}</sts>" +
                                                "<ad>{4}</ad>" +
                                                "<d>{5}</d>" +
                                                "<q_city>{6}</q_city>" +
                                                "<cur_city>{7}</cur_city>" +
                                                "<q_user>{8}</q_user>" +
                                                "<cur_user>{9}</cur_user>" +
                                                "<t>{10}</t>" +
                                                "<bd>{11}</bd>	" +
                                                "<ed>{12}</ed>	" +
                                                "<bal_before>{13}</bal_before>" +
                                                "<st>{14}</st>" +
                                                "<srcType>{15}</srcType>" +
                                                "<srcIdent>{16}</srcIdent>" +
                                                "<postpay>{17}</postpay>" +
                                                "<time_bal_used>{18}</time_bal_used>" +
                                                "<time_bal_before>{19}</time_bal_before>" +
                                                "<d_prev_end>{20}</d_prev_end>" +
                                                "<behavior>{21}</behavior>" +
                                                "<lat>{22}</lat>" +
                                                "<long>{23}</long>" +
                                                "<ccpan>{24}</ccpan>" +
                                                "<cctype>{25}</cctype>" +
                                                "<ccprovider>{26}</ccprovider>" +
                                                "<num_days>{27}</num_days>" +
                                                "<num_days_colour>{28}</num_days_colour>",
                                                sPlate,
                                                dInsId,
                                                oInst.INS_SHORTDESC,
                                                sGrpDescription,
                                                sTarDescription,
                                                oFirst.OPE_DATE.ToString("HHmmssddMMyy"),
                                                oConsolidateParking.FinalAmount,
                                                oFirst.CURRENCy.CUR_ISO_CODE,
                                                oConsolidateParking.FinalAmount,
                                                oLast.CURRENCy1.CUR_ISO_CODE,
                                                oConsolidateParking.Time,
                                                oFirst.OPE_INIDATE.ToString("HHmmssddMMyy"),
                                                oLast.OPE_ENDDATE.ToString("HHmmssddMMyy"),
                                                oLast.OPE_BALANCE_BEFORE,
                                                oLast.OPE_SUSCRIPTION_TYPE,
                                                1,
                                                "-1",
                                                iPostPay,
                                                oLast.OPE_TIME_BALANCE_USED,
                                                oLast.OPE_TIME_BALANCE_BEFORE,
                                                sOpeRefundPreviousEndDate,
                                                iTariffBehavior,
                                                oFirst.OPE_LATITUDE,
                                                oFirst.OPE_LONGITUDE,
                                                ccpan,
                                                cctype.HasValue ? cctype.Value.ToString() : "",
                                                ccprovider.HasValue ? ccprovider.Value.ToString() : "",
                                                iTotalDays,
                                                (iTotalDays <= 0) ? strTodayColour : ((iTotalDays == 1) ? strTomorrowColour : strMoreThanTomorrowColour),
                                                sStsDescription);
                            }
                            else
                            {
                                sb.AppendFormat("<p>{0}</p>" +
                                               "<cityID>{1}</cityID>" +
                                               "<cityShortDesc>{2}</cityShortDesc>" +
                                               "<g>{3}</g>" +
                                               "<sts>{22}</sts>" +
                                               "<ad>{4}</ad>" +
                                               "<d>{5}</d>" +
                                               "<q_city>{6}</q_city>" +
                                               "<cur_city>{7}</cur_city>" +
                                               "<q_user>{8}</q_user>" +
                                               "<cur_user>{9}</cur_user>" +
                                               "<t>{10}</t>" +
                                               "<bd>{11}</bd>	" +
                                               "<ed>{12}</ed>	" +
                                               "<bal_before>{13}</bal_before>" +
                                               "<st>{14}</st>" +
                                               "<srcType>{15}</srcType>" +
                                               "<srcIdent>{16}</srcIdent>" +
                                               "<postpay>{17}</postpay>" +
                                               "<time_bal_used>{18}</time_bal_used>" +
                                               "<time_bal_before>{19}</time_bal_before>" +
                                               "<d_prev_end>{20}</d_prev_end>" +
                                               "<behavior>{21}</behavior>",
                                               sPlate,
                                               dInsId,
                                               oInst.INS_SHORTDESC,
                                               sGrpDescription,
                                               sTarDescription,
                                               oFirst.OPE_DATE.ToString("HHmmssddMMyy"),
                                               oConsolidateParking.FinalAmount,
                                               oFirst.CURRENCy.CUR_ISO_CODE,
                                               oConsolidateParking.FinalAmount,
                                               oLast.CURRENCy1.CUR_ISO_CODE,
                                               oConsolidateParking.Time,
                                               oFirst.OPE_INIDATE.ToString("HHmmssddMMyy"),
                                               oLast.OPE_ENDDATE.ToString("HHmmssddMMyy"),
                                               oLast.OPE_BALANCE_BEFORE,
                                               oLast.OPE_SUSCRIPTION_TYPE,
                                               1,
                                               "-1",
                                               iPostPay,
                                               oLast.OPE_TIME_BALANCE_USED,
                                               oLast.OPE_TIME_BALANCE_BEFORE,
                                               sOpeRefundPreviousEndDate,
                                               iTariffBehavior,
                                               sStsDescription);
                             }


                            sb.AppendFormat("<q>{0}</q>" +
                                            "<layout>{1}</layout>" +
                                            "<q_fee>{2}</q_fee>" +
                                            "<q_vat>{3}</q_vat>" +
                                            "<q_subtotal>{4}</q_subtotal>" +
                                            "<q_total>{5}</q_total>" +
                                //"<real_q>850</real_q>" +
                                            "<q_plus_vat>{6}</q_plus_vat>" +
                                            "<q_fee_plus_vat>{7}</q_fee_plus_vat>" +
                                            "<q_without_bon>{8}</q_without_bon>" +
                                            "<per_bon>{9}</per_bon>",
                                            oConsolidateParking.Amount,
                                            ((oConsolidateParking.LayoutCampaign.HasValue && string.IsNullOrEmpty(oInst.INS_BSM_CITY_ID)) ? oConsolidateParking.LayoutCampaign.Value : oFirst.INSTALLATION.INS_FEE_LAYOUT),
                                            oConsolidateParking.AmountFEE,
                                            oConsolidateParking.AmountVAT,
                                            oConsolidateParking.SubTotalAmount,
                                            oConsolidateParking.TotalAmount,
                                            oConsolidateParking.QPlusIVA,
                                            oConsolidateParking.FeePlusIVA,
                                            oConsolidateParking.AmountWithoutBon,
                                            oConsolidateParking.BonMlt.ToString(numberFormatProvider));

                            if (oConsolidateParking.QChange.HasValue)
                            {
                                sb.AppendFormat("<chng>{0}</chng>" +
                                                "<qch>{1}</qch>" +
                                                "<qch_fee>{2}</qch_fee>" +
                                                "<qch_vat>{3}</qch_vat>" +
                                                "<qch_subtotal>{4}</qch_subtotal>" +
                                                "<qch_total>{5}</qch_total>" +
                                                "<qch_plus_vat>{6}</qch_plus_vat>" +
                                                "<qch_fee_plus_vat>{7}</qch_fee_plus_vat>",
                                                oConsolidateParking.Chng,
                                                oConsolidateParking.QChange,
                                                oConsolidateParking.AmountFEEChange,
                                                oConsolidateParking.AmountVATChange,
                                                oConsolidateParking.SubTotalAmountChange,
                                                oConsolidateParking.TotalAmountChange,
                                                oConsolidateParking.QPlusIVAChange,
                                                oConsolidateParking.FeePlusIVAChange);
                            }

                            sb.AppendFormat("<ServiceParkingLbl>{0}</ServiceParkingLbl>" +
                                            "<ServiceFeeLbl>{1}</ServiceFeeLbl>" +
                                            "<ServiceFeeVATLbl>{2}</ServiceFeeVATLbl>" +
                                            "<ServiceVATLbl>{3}</ServiceVATLbl>" +
                                            "<ServiceTotalLbl>{4}</ServiceTotalLbl>" +
                                            "<ServiceParkingBaseLbl>{5}</ServiceParkingBaseLbl>" +
                                            "<ServiceParkingVariableLbl>{6}</ServiceParkingVariableLbl>",
                                            infraestructureRepository.GetLiteral(oFirst.INSTALLATION.INS_SERVICE_PARK_LIT_ID ?? 0, strCulture),
                                            infraestructureRepository.GetLiteral(oFirst.INSTALLATION.INS_SERVICE_FEE_LIT_ID ?? 0, strCulture),
                                            infraestructureRepository.GetLiteral(oFirst.INSTALLATION.INS_SERVICE_FEE_PLUS_VAT_LIT_ID ?? 0, strCulture),
                                            infraestructureRepository.GetLiteral(oFirst.INSTALLATION.INS_SERVICE_VAT_LIT_ID ?? 0, strCulture),
                                            infraestructureRepository.GetLiteral(oFirst.INSTALLATION.INS_SERVICE_TOTAL_LIT_ID ?? 0, strCulture),
                                            infraestructureRepository.GetLiteral(oFirst.INSTALLATION.INS_SERVICE_PARK_BASE_LIT_ID ?? 0, strCulture),
                                            infraestructureRepository.GetLiteral(oFirst.INSTALLATION.INS_SERVICE_PARK_VARIABLE_LIT_ID ?? 0, strCulture)                                                                                                                                   
                                            );


                            sb.AppendFormat("<ReceiptHeaderLbl>{0}</ReceiptHeaderLbl>", infraestructureRepository.GetLiteralFromKey(string.Format("{0}_{1}", Convert.ToInt32(oFirst.INSTALLATION.INS_ID), "RECEIPT_HEADER"), strCulture));
                            if (sExternalBaseID != null && !sExternalBaseID.StartsWith("###") && !sExternalBaseID.EndsWith("###"))
                                sb.AppendFormat("<ExternalID>{0}</ExternalID>", sExternalBaseID);

                            if ((ParkWSSignatureType)oFirst.INSTALLATION.INS_PARK_WS_SIGNATURE_TYPE == ParkWSSignatureType.pst_bsm &&
                                !string.IsNullOrEmpty(oFirst.OPE_ADDITIONAL_PARAMS))
                            {
                                BSMConfiguration oBSMConfiguration = null;
                                try
                                {
                                    oBSMConfiguration = (BSMConfiguration)JsonConvert.DeserializeObject(oFirst.OPE_ADDITIONAL_PARAMS, typeof(BSMConfiguration));
                                }
                                catch (Exception) { }

                                if (oBSMConfiguration != null)
                                {
                                    sb.AppendFormat("<ServiceParkingBaseQuantityLbl>{0}</ServiceParkingBaseQuantityLbl>", oBSMConfiguration.parkingBaseQuantityLbl);
                                    sb.AppendFormat("<ServiceParkingVariableQuantityLbl>{0}</ServiceParkingVariableQuantityLbl>", oBSMConfiguration.parkingVariableQuantityLbl);
                                }


                                if (oBSMConfiguration != null && oBSMConfiguration.Modifiers != null && oBSMConfiguration.Modifiers.Modifiers != null && oBSMConfiguration.Modifiers.Modifiers.Any())
                                {
                                    sb.AppendFormat("<modifiers>{0}</modifiers>", oBSMConfiguration.Modifiers.ToCustomXml());
                                }
                            }
                            else if (!string.IsNullOrEmpty(oFirst.OPE_ADDITIONAL_PARAMS))
                            {

                                dynamic oParams = JsonConvert.DeserializeObject(oFirst.OPE_ADDITIONAL_PARAMS);

                                var oRDetail = oParams["rdetail"];

                                if (oRDetail != null)
                                {
                                    int iRDetail = Convert.ToInt32(oRDetail);
                                    string strMessage = infraestructureRepository.GetCustomErrorMessageForOperation(oFirst.OPE_DATE, iRDetail, oFirst.OPE_GRP_ID.Value, oFirst.OPE_TAR_ID, strCulture);
                                    sb.AppendFormat("<ParkingWarningLbl>{0}</ParkingWarningLbl>", strMessage);
                                }
                            }

                            sb.AppendFormat("<g_id>{0}</g_id>" +
                                            "<ad_id>{1}</ad_id>" +
                                            "<isshopkeeperoperation>{2}</isshopkeeperoperation>",
                                            oFirst.OPE_GRP_ID ?? 0,
                                            oFirst.OPE_TAR_ID ?? 0,
                                            oFirst.OPE_SHOPKEEPER_OP ?? 0);

                            if (oFirst.OPE_STRSE_ID.HasValue)
                                sb.AppendFormat("<sts_id>{0}</sts_id>", oFirst.OPE_STRSE_ID.Value);

                            bool bStop = false;
                            bool bExtension = false;
                            bool bRefund = false;

                            if (oLast.TARIFF != null)
                            {
                                bStop = (oLast.OPE_PARKING_MODE == (int)ParkingMode.StartStop &&
                                         oLast.OPE_PARKING_MODE_STATUS == (int)ParkingModeStatus.Opened);

                                if (oLast.OPE_PARKING_MODE != (int)ParkingMode.StartStop &&
                                    oLast.OPE_UTC_ENDDATE >= DateTime.UtcNow)
                                {
                                    bExtension = (oLast.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation ||
                                                    oLast.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation ||
                                                    oLast.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund);
                                    bRefund = ((oLast.INSTALLATION.INS_OPT_UNPARK == 1 || oLast.OPE_PARKING_MODE == (int)ParkingMode.StartStopHybrid)  &&
                                                (oLast.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation ||
                                                oLast.OPE_TYPE == (int)ChargeOperationsType.ExtensionOperation));
                                }
                            }

                            sb.AppendFormat("<stop>{0}</stop>" +
                                            "<extension>{1}</extension>" +
                                            "<refund>{2}</refund>",
                                            bStop ? 1 : 0,
                                            bExtension ? 1 : 0,
                                            bRefund ? 1 : 0);

                            sb.Append("</userpark>");
                        }

                    }

                }


            }

            return sb.ToString();
        }

        private void ParkingConsolidate(int iOperationIndex, List<OPERATION> oParkings, ref ParkingTags oConsolidateParking)
        {
            OPERATION oOperation = oParkings[iOperationIndex];

            USER oUser = oOperation.USER;

            decimal dPercBonus = (oOperation.OPE_PERC_BONUS ?? 0);

            decimal dVAT1 = oOperation.OPE_PERC_VAT1 ?? 0;
            decimal dVAT2 = oOperation.OPE_PERC_VAT2 ?? 0;
            decimal dPercFEE = oOperation.OPE_PERC_FEE ?? 0;
            decimal dPercFEETopped = oOperation.OPE_PERC_FEE_TOPPED ?? 0;
            decimal dFixedFEE = oOperation.OPE_FIXED_FEE ?? 0;

            NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
            numberFormatProvider.NumberDecimalSeparator = ".";


            int iPartialVAT1;
            int iPartialPercFEE;
            int iPartialFixedFEE;
            int iPartialPercFEEVAT;
            int iPartialFixedFEEVAT;
            int iPartialBonusFEE;
            int iPartialBonusFEEVAT;

            int iAmount = oOperation.OPE_AMOUNT;
            int iAmountWithoutBon = (oOperation.OPE_AMOUNT_WITHOUT_BON ?? 0);
            int iFinalAmount = oOperation.OPE_FINAL_AMOUNT;
            int iTime = oOperation.OPE_TIME;
            /*if (oOperation.OPE_PARKING_MODE == (int)ParkingMode.StartStop && oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund)
            {
                // Calculate amount remaining using start operation
                OPERATION oStartOp = oParkings.Where(o => o.OPE_TYPE == (int)ChargeOperationsType.ParkingOperation &&
                                                          o.OPE_PARKING_MODE == (int)ParkingMode.StartStop)
                                              .FirstOrDefault();
                if (oStartOp != null)
                {
                    iAmount = oStartOp.OPE_AMOUNT - oOperation.OPE_AMOUNT;
                    iAmountWithoutBon = (oStartOp.OPE_AMOUNT_WITHOUT_BON ?? 0) - (oOperation.OPE_AMOUNT_WITHOUT_BON ?? 0);
                    iFinalAmount = oStartOp.OPE_FINAL_AMOUNT - oOperation.OPE_FINAL_AMOUNT;
                    iTime = oStartOp.OPE_TIME - oOperation.OPE_TIME;
                }
            }*/



            int? iPaymentTypeId = null;
            int? iPaymentSubtypeId = null;
            IsTAXMode eTaxMode = IsTAXMode.IsNotTaxVATForward;
            if (oUser.CUSTOMER_PAYMENT_MEAN != null)
            {
                iPaymentTypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAT_ID;
                iPaymentSubtypeId = oUser.CUSTOMER_PAYMENT_MEAN.CUSPM_PAST_ID;
            }
            int? iTariffType = null;
            if (oOperation.TARIFF != null)
                iTariffType = oOperation.TARIFF.TAR_TYPE;


            if (!customersRepository.GetFinantialParams(oUser, oOperation.OPE_INS_ID, (PaymentSuscryptionType)oUser.USR_SUSCRIPTION_TYPE, iPaymentTypeId, iPaymentSubtypeId, (ChargeOperationsType)oOperation.OPE_TYPE, iTariffType, oOperation.OPE_DATE,
                                                             out dVAT1, out dVAT2, out dPercFEE, out dPercFEETopped, out dFixedFEE, out eTaxMode))
            {
                Logger_AddLogMessage(string.Format("OperationQueryTags::Error getting installation FEE parameters"), LogLevels.logERROR);
            }


            if (eTaxMode == IsTAXMode.IsNotTaxVATBackward)
            {
                iAmount = iAmount + (int)(oOperation.OPE_PARTIAL_VAT1 ?? 0);
            }


            int iTotalAmount = customersRepository.CalculateFEE(ref iAmount, dVAT1, dVAT2, dPercFEE, dPercFEETopped, dFixedFEE, dPercBonus, eTaxMode,
                                                                out iPartialVAT1, out iPartialPercFEE, out iPartialFixedFEE, out iPartialBonusFEE,
                                                                out iPartialPercFEEVAT, out iPartialFixedFEEVAT, out iPartialBonusFEEVAT);

            //iTotalAmount = iAmount - iPartialVAT1 - iPartialPercFEE - iPartialFixedFEE;

            decimal dAmountFEE = Math.Round(iAmount * dPercFEE, MidpointRounding.AwayFromZero);
            if (dPercFEETopped > 0 && dAmountFEE > dPercFEETopped) dAmountFEE = dPercFEETopped;
            dAmountFEE += dFixedFEE;
            int iAmountFEE = Convert.ToInt32(Math.Round(dAmountFEE, MidpointRounding.AwayFromZero));

            int iBonus = Convert.ToInt32(Math.Round(dAmountFEE * dPercBonus, MidpointRounding.AwayFromZero));

            int iAmountVAT = iPartialVAT1 + iPartialPercFEEVAT + iPartialFixedFEEVAT - iPartialBonusFEEVAT;
            int iSubTotalAmount = iAmount + iAmountFEE - iBonus;

            int iQPlusIVA = iAmount + iPartialVAT1;
            int iFeePlusIVA = iPartialPercFEE + iPartialFixedFEE;

            if (oOperation.OPE_BON_MLT.HasValue)
                oConsolidateParking.BonMlt = oOperation.OPE_BON_MLT.Value;

            if (!string.IsNullOrEmpty(oOperation.OPE_VEHICLE_TYPE))
                oConsolidateParking.VehicleType = oOperation.OPE_VEHICLE_TYPE;                

            if (oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund) 
            {
                iAmount = -iAmount;
                iAmountFEE = -iAmountFEE;
                iAmountVAT = -iAmountVAT;
                iSubTotalAmount = -iSubTotalAmount;
                iTotalAmount = -iTotalAmount;
                iQPlusIVA = -iQPlusIVA;
                iFeePlusIVA = -iFeePlusIVA;
                iAmountWithoutBon = -iAmountWithoutBon;
                iFinalAmount = -iFinalAmount;
                iTime = -iTime;
            }

            oConsolidateParking.Amount += iAmount;
            oConsolidateParking.AmountFEE += iAmountFEE;
            oConsolidateParking.AmountVAT += iAmountVAT;
            oConsolidateParking.SubTotalAmount += iSubTotalAmount;
            oConsolidateParking.TotalAmount += iTotalAmount;
            oConsolidateParking.QPlusIVA += iQPlusIVA;
            oConsolidateParking.FeePlusIVA += iFeePlusIVA;
            oConsolidateParking.AmountWithoutBon += iAmountWithoutBon;
            oConsolidateParking.FinalAmount += iFinalAmount;
            oConsolidateParking.Time += iTime;
            if(oOperation.OPE_CAMP_ID.HasValue)
            {
                oConsolidateParking.LayoutCampaign = Convert.ToInt32(oOperation.CAMPAING.CAMP_LAYOUT_ID);
            }

            if (oOperation.INSTALLATION.CURRENCy.CUR_ISO_CODE != infraestructureRepository.GetCurrencyIsoCode(Convert.ToInt32(oUser.USR_CUR_ID)))
            {
                double dChangeToApply = 1.0;
                double dChangeFee = 0;

                dChangeToApply = Convert.ToDouble(oOperation.OPE_CHANGE_APPLIED);/// GetChangeToApplyFromInstallationCurToUserCur(oOperation.INSTALLATION, oUser);
                if (dChangeToApply >= 0)
                {

                    string sChng = dChangeToApply.ToString(numberFormatProvider);

                    int iQChange = ChangeQuantityFromInstallationCurToUserCur(iAmount,
                                    dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);

                    int iAmountFEEChange = ChangeQuantityFromInstallationCurToUserCur(iAmountFEE, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                    int iAmountVATChange = ChangeQuantityFromInstallationCurToUserCur(iAmountVAT, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                    int iSubTotalAmountChange = ChangeQuantityFromInstallationCurToUserCur(iSubTotalAmount, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                    int iTotalAmountChange = ChangeQuantityFromInstallationCurToUserCur(iTotalAmount, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                    int iQPlusIVAChange = ChangeQuantityFromInstallationCurToUserCur(iQPlusIVA, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                    int iFeePlusIVAChange = ChangeQuantityFromInstallationCurToUserCur(iFeePlusIVA, dChangeToApply, oOperation.INSTALLATION, oUser, out dChangeFee);
                   
                    oConsolidateParking.Chng = sChng;

                    if (oOperation.OPE_TYPE == (int)ChargeOperationsType.ParkingRefund)
                    {
                        iQChange = -iQChange;
                        iAmountFEEChange = -iAmountFEEChange;
                        iAmountVATChange = -iAmountVATChange;
                        iSubTotalAmountChange = -iSubTotalAmountChange;
                        iTotalAmountChange = -iTotalAmountChange;
                        iQPlusIVAChange = -iQPlusIVAChange;
                        iFeePlusIVAChange = -iFeePlusIVAChange;
                    }

                    oConsolidateParking.QChange = (oConsolidateParking.QChange ?? 0) + iQChange;
                    oConsolidateParking.AmountFEEChange = (oConsolidateParking.AmountFEEChange ?? 0) + iAmountFEEChange;
                    oConsolidateParking.AmountVATChange = (oConsolidateParking.AmountVATChange ?? 0) + iAmountVATChange;
                    oConsolidateParking.SubTotalAmountChange = (oConsolidateParking.SubTotalAmountChange ?? 0) + iSubTotalAmountChange;
                    oConsolidateParking.TotalAmountChange = (oConsolidateParking.TotalAmountChange ?? 0) + iTotalAmountChange;
                    oConsolidateParking.QPlusIVAChange = (oConsolidateParking.QPlusIVAChange ?? 0) + iQPlusIVAChange;
                    oConsolidateParking.FeePlusIVAChange = (oConsolidateParking.FeePlusIVAChange ?? 0) + iFeePlusIVAChange;

                }
            }

        }

        private string GetUsersSync(long lCurrVersion, decimal dCouId)
        {
            string strRes = "";
            StringBuilder strRegs = new StringBuilder();

            try
            {
                NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                numberFormatProvider.NumberDecimalSeparator = ".";

                USERS_SYNC[] oArrSync;
                if (customersRepository.GetSyncUsers(lCurrVersion, dCouId, infraestructureRepository, out oArrSync))
                {
                    foreach (USERS_SYNC oSync in oArrSync)
                    {
                        strRegs.Append("<reg json:Array='true'>");
                        strRegs.Append(string.Format("<version>{0}</version>", oSync.USR_VERSION));
                        strRegs.Append(string.Format("<movtype>{0}</movtype>", oSync.USR_MOV_TYPE));
                        strRegs.Append(string.Format("<id>{0}</id>", oSync.USR_ID));
                        strRegs.Append(string.Format("<email>{0}</email>", oSync.USR_EMAIL));
                        strRegs.Append(string.Format("<cou>{0}</cou>", oSync.USR_COU_ID));                        
                        strRegs.Append("</reg>");
                    }
                }

                strRes = strRegs.ToString();

            }
            catch (Exception)
            {
                strRes = "";
            }

            return strRes;
        }

        private bool CheckCardPaymentMode(CUSTOMER_PAYMENT_MEAN oUserPaymentMean, CardPayment_Mode eTariffCardPaymentMode, out CardPayment_Mode eCardPaymentModeToApply)
        {
            bool bRet = false;
            eCardPaymentModeToApply = eTariffCardPaymentMode;

            if (oUserPaymentMean != null && oUserPaymentMean.CUSPM_ENABLED == 1 && oUserPaymentMean.CUSPM_VALID == 1)
            {
                switch ((PaymentMeanCreditCardProviderType)oUserPaymentMean.CUSPM_CREDIT_CARD_PAYMENT_PROVIDER)
                {
                    case PaymentMeanCreditCardProviderType.pmccpMercadoPago:
                        {
                            if (eTariffCardPaymentMode == CardPayment_Mode.Charge)
                            {
                                bRet = true;
                                eCardPaymentModeToApply = CardPayment_Mode.Charge;
                            }
                            else
                            {
                                bool bAllowAuthorizationAndCapture = MercadoPagoPayments.AllowAuthorizationAndCapture(oUserPaymentMean.CUSPM_TOKEN_CARD_SCHEMA,
                                                                                                                      oUserPaymentMean.CUSPM_TOKEN_CARD_TYPE);
                                bRet = (eTariffCardPaymentMode == CardPayment_Mode.AuthorizationPreferably ||
                                        (eTariffCardPaymentMode == CardPayment_Mode.Authorization && bAllowAuthorizationAndCapture));
                                eCardPaymentModeToApply = (bAllowAuthorizationAndCapture ? CardPayment_Mode.Authorization : CardPayment_Mode.Charge);
                            }
                        }
                        break;

                    default:
                        {
                            bRet = (eTariffCardPaymentMode == CardPayment_Mode.Charge ||
                                    eTariffCardPaymentMode == CardPayment_Mode.AuthorizationPreferably);
                            eCardPaymentModeToApply = CardPayment_Mode.Charge;
                        }
                        break;
                }
            }

            return bRet;
        }

        private string TestRedirection(string sInput, string sWebMethodName, bool bIsJson = false)
        {
            string sOut = "";

            string sXmlIn = "";
            SortedList parametersIn = null;
            string strHash = "";
            string strHashString = "";

            bool bRedirect = false;

            try
            {

                if (m_oTestRedirections == null)
                {
                    m_oTestRedirections = new Dictionary<string, List<decimal>>();
                    List<string> oRedirectionConfig = (ConfigurationManager.AppSettings["TestRedirections"] ?? "").Split(';').ToList();
                    string[] oConfig;
                    string sMethod = "";
                    string sInstallations = "";
                    List<decimal> oInstallations = null;
                    foreach (string sConfig in oRedirectionConfig)
                    {
                        if (!string.IsNullOrEmpty(sConfig.Trim()))
                        {
                            oConfig = sConfig.Trim().Split(':');
                            sMethod = oConfig[0].Trim();
                            sInstallations = oConfig[1].Trim();
                            if (!string.IsNullOrEmpty(sInstallations))
                                oInstallations = oConfig[1].Split(new char[] { ',' }).Select(id => Convert.ToDecimal(id)).ToList();
                            else
                                oInstallations = new List<decimal>();
                            if (!m_oTestRedirections.ContainsKey(sMethod))
                                m_oTestRedirections.Add(sMethod, oInstallations);
                            else
                                m_oTestRedirections[sMethod] = oInstallations;
                        }
                    }
                }

                if (bIsJson)
                {
                    XmlDocument xmlIn = (XmlDocument)JsonConvert.DeserializeXmlNode(sInput);
                    sXmlIn = xmlIn.OuterXml;
                }
                else
                    sXmlIn = sInput;

                ResultType rt = FindInputParameters(sXmlIn, out parametersIn, out strHash, out strHashString);

                if (rt == ResultType.Result_OK)
                {

                    if (m_oTestRedirections.ContainsKey(sWebMethodName))
                    {
                        switch (sWebMethodName)
                        {
                            case "QueryLoginCity":
                                {
                                    decimal dInsId = decimal.Parse(parametersIn["cityID"].ToString());
                                    bRedirect = (!m_oTestRedirections[sWebMethodName].Any() || m_oTestRedirections[sWebMethodName].Contains(dInsId));
                                }
                                break;

                            default:
                                {
                                    bRedirect = !m_oTestRedirections[sWebMethodName].Any();
                                    if (!bRedirect && parametersIn["u"] != null && parametersIn["SessionID"] != null)
                                    {
                                        USER oUser = null;
                                        decimal? dInsId;
                                        string strCulture;
                                        string strAppVersion;
                                        GetUserData(ref oUser, parametersIn, false, out dInsId, out strCulture, out strAppVersion);
                                        if (dInsId.HasValue)
                                        {
                                            bRedirect = (m_oTestRedirections[sWebMethodName].Contains(dInsId.Value));
                                        }
                                    }
                                }
                                break;
                        }
                    }

                    if (bRedirect)
                    {                        

                        COUNTRIES_REDIRECTION oRedirectionConfig = new COUNTRIES_REDIRECTION
                        {
                            COURE_COUNTRY_REDIRECTION_WS_URL = (ConfigurationManager.AppSettings["TestRedirections_Url"] ?? "http://localhost:6150/integraMobileWSTest.asmx"),
                            COURE_COUNTRY_REDIRECTION_HTTP_USER = (ConfigurationManager.AppSettings["TestRedirections_HttpUser"] ?? ""),
                            COURE_COUNTRY_REDIRECTION_PASSWORD = (ConfigurationManager.AppSettings["TestRedirections_HttpPassword"] ?? "")
                        };

                        ExternalIntegraMobileWS.integraMobileWS oExternalIntegraMobileWS = StarExternalIntegraMobileWS2(oRedirectionConfig);
                        if (oExternalIntegraMobileWS != null)
                        {
                            MethodInfo oWebMethod = typeof(ExternalIntegraMobileWS.integraMobileWS).GetMethod(sWebMethodName);
                            if (oWebMethod != null)
                            {
                                Logger_AddLogMessage(string.Format("TestRedirection({0})::Input={1}", sWebMethodName, sInput), LogLevels.logINFO);
                                sOut = (string)oWebMethod.Invoke(oExternalIntegraMobileWS, new object[] { sInput });
                                Logger_AddLogMessage(string.Format("TestRedirection({0})::Response={1}", sWebMethodName, sOut), LogLevels.logINFO);
                            }
                            //xmlOut = oExternalIntegraMobileWS.QueryLoginCity(xmlIn);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                sOut = "";
                Logger_AddLogException(ex, string.Format("TestRedirection::Error: webMethodName={0}, Input={1}", sWebMethodName, (bIsJson ? PrettyJSON(sInput) : PrettyXml(sInput))), LogLevels.logERROR);
            }
            return sOut;
        }

        #endregion

        private class ParkingTags
        {
            public int Amount;                            
            public int AmountFEE;
            public int AmountVAT;
            public int SubTotalAmount;
            public int TotalAmount;
            public int QPlusIVA;
            public int FeePlusIVA;
            public int AmountWithoutBon;
            public decimal BonMlt;
            public string VehicleType;
            
            public string Chng;
            public int? QChange;
            public int? AmountFEEChange;
            public int? AmountVATChange;
            public int? SubTotalAmountChange;
            public int? TotalAmountChange;
            public int? QPlusIVAChange;
            public int? FeePlusIVAChange;

            public int FinalAmount;
            public int Time;

            public int? LayoutCampaign;
        }

        protected static Dictionary<string, List<decimal>> m_oTestRedirections = null;

    }
}