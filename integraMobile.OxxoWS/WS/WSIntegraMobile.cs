using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using System.Linq;
using System.Web;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using integraMobile.Domain.Abstract;

namespace integraMobile.OxxoWS.WS
{
    public class WSIntegraMobile : WSBase
    {

        public WSIntegraMobile()
            : base()
        {
            m_Log = new CLogWrapper(typeof(WSIntegraMobile));
        }


        public ResultType GetListOfCities(decimal? dLatitude, decimal? dLongitude, out Dictionary<int, string> oCities, ref SortedList parametersOut)
        {            
            ResultType rtRes = ResultType.Result_OK;
            oCities = new Dictionary<int, string>();

            string sXmlIn = "";
            string sXmlOut = "";            

            try
            {

                integraMobileWS.integraMobileWS oIntegraMobileWS = new integraMobileWS.integraMobileWS();
                oIntegraMobileWS.Url = GetWSUrl();
                oIntegraMobileWS.Timeout = GetWSTimeout();

                if (!string.IsNullOrEmpty(GetWSHttpUser()))
                {
                    oIntegraMobileWS.Credentials = new System.Net.NetworkCredential(GetWSHttpUser(), GetWSHttpPassword());
                }

                string strvers = "1.0";

                string strMessage = "";
                string strAuthHash = "";

                string sLatLon = "";
                string sLatLonXml = LatLonXml(dLatitude, dLongitude, out sLatLon);

                strAuthHash = CalculateWSHash(string.Format("{0}{1}{2}", OSID, sLatLon, strvers));

                strMessage = string.Format("<ipark_in><OSID>{0}</OSID>{1}<vers>{2}</vers><ah>{3}</ah></ipark_in>",
                                           OSID, sLatLonXml, strvers, strAuthHash);


                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("GetListOfCities xmlIn={0}", sXmlIn), LogLevels.logDEBUG);

                string strOut = "";
                strOut = oIntegraMobileWS.GetListOfCities(strMessage);

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("GetListOfCities xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (parametersOut != null)
                {
                    foreach (var key in wsParameters.Keys)
                        parametersOut.Add(key, wsParameters[key]);
                }
                

                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = (ResultType)Convert.ToInt32(wsParameters["r"].ToString());

                    if (rtRes == ResultType.Result_OK)
                    {
                        int iNum = Convert.ToInt32(wsParameters["cities_city_num"]);
                        for (int i = 0; i < iNum; i++)
                        {
                            oCities.Add(Convert.ToInt32(wsParameters[string.Format("cities_city_{0}_id", i)]),  wsParameters[string.Format("cities_city_{0}", i)].ToString());
                        }

                    }

                }


            }
            catch (Exception e)
            {                
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "GetListOfCities::Exception", LogLevels.logERROR);

            }

            return rtRes;

        }

        public ResultType QueryLogin(string sUser, string sPassword, string sCulture, bool bSessionKeepAlive, int iCityID, decimal? dLatitude, decimal? dLongitude, out string sSessionID, ref SortedList parametersOut)
        {
            ResultType rtRes = ResultType.Result_OK;
            sSessionID = "";
            
            string sXmlIn = "";
            string sXmlOut = "";

            try
            {

                integraMobileWS.integraMobileWS oIntegraMobileWS = new integraMobileWS.integraMobileWS();
                oIntegraMobileWS.Url = GetWSUrl();
                oIntegraMobileWS.Timeout = GetWSTimeout();

                if (!string.IsNullOrEmpty(GetWSHttpUser()))
                {
                    oIntegraMobileWS.Credentials = new System.Net.NetworkCredential(GetWSHttpUser(), GetWSHttpPassword());
                }

                string strvers = "1.0";

                string strMessage = "";
                string strAuthHash = "";

                string sLatLon = "";
                string sLatLonXml = LatLonXml(dLatitude, dLongitude, out sLatLon);

                string sUserIdent = sUser + "_" + DateTime.UtcNow.Ticks.ToString();

                DateTime dtDate = DateTime.Now;
                DateTime dtUtcDate = DateTime.UtcNow;

                strAuthHash = CalculateWSHash(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10:HHmmssddMMyy}{11:HHmmssddMMyy}{12}", sUser, m_oLanguages[sCulture], OSID, sPassword, iCityID, sLatLon, "1.5", sUserIdent, sUserIdent, sUserIdent, dtDate, dtUtcDate, strvers));

                strMessage = string.Format("<ipark_in><u>{0}</u><lang>{1}</lang><OSID>{2}</OSID><pasw>{3}</pasw><cityID>{4}</cityID>{5}<appvers>{6}</appvers><IMEI>{7}</IMEI><WIFIMAC>{8}</WIFIMAC><pushID>{9}</pushID><date>{10:HHmmssddMMyy}</date><utc_date>{11:HHmmssddMMyy}</utc_date><vers>{12}</vers><ah>{13}</ah></ipark_in>",
                                           sUser, m_oLanguages[sCulture], OSID, sPassword, iCityID, sLatLonXml, "1.5",
                                           sUserIdent, sUserIdent, sUserIdent,
                                           dtDate, dtUtcDate,
                                           strvers, strAuthHash);


                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("QueryLogin xmlIn={0}", sXmlIn), LogLevels.logDEBUG);

                string strOut = "";
                strOut = oIntegraMobileWS.QueryLogin(strMessage);

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("QueryLogin xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                //rtRes = FindOutParameters2(strOut, out wsParameters);

                rtRes = FindOutParameters3(strOut, out wsParameters);                

                if (parametersOut != null)
                {
                    foreach (var key in wsParameters.Keys)
                        parametersOut.Add(key, wsParameters[key]);
                }


                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = (ResultType)Convert.ToInt32(wsParameters["r"].ToString());

                    if (rtRes == ResultType.Result_OK)
                    {
                        sSessionID = wsParameters["SessionID"].ToString();
                    }

                }


            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "QueryLogin::Exception", LogLevels.logERROR);

            }

            return rtRes;

        }

        public ResultType QueryCity(string sUser, string sSessionID, int iCityID, string sLicenseTermsVersion,  ref SortedList parametersOut)
        {
            ResultType rtRes = ResultType.Result_OK;            

            string sXmlIn = "";
            string sXmlOut = "";

            try
            {

                integraMobileWS.integraMobileWS oIntegraMobileWS = new integraMobileWS.integraMobileWS();
                oIntegraMobileWS.Url = GetWSUrl();
                oIntegraMobileWS.Timeout = GetWSTimeout();

                if (!string.IsNullOrEmpty(GetWSHttpUser()))
                {
                    oIntegraMobileWS.Credentials = new System.Net.NetworkCredential(GetWSHttpUser(), GetWSHttpPassword());
                }

                string strvers = "1.0";

                string strMessage = "";
                string strAuthHash = "";

                strAuthHash = CalculateWSHash(string.Format("{0}{1}{2}{3}{4}", sUser, sSessionID, iCityID, sLicenseTermsVersion, strvers));

                strMessage = string.Format("<ipark_in><u>{0}</u><SessionID>{1}</SessionID><cityID>{2}</cityID><legaltermsver>{3}</legaltermsver><vers>{4}</vers><ah>{5}</ah></ipark_in>",
                                           sUser, sSessionID, iCityID, sLicenseTermsVersion, strvers, strAuthHash);


                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("QueryCity xmlIn={0}", sXmlIn), LogLevels.logDEBUG);

                string strOut = "";
                strOut = oIntegraMobileWS.QueryCity(strMessage);

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("QueryCity xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters2(strOut, out wsParameters);

                if (parametersOut != null)
                {
                    foreach (var key in wsParameters.Keys)
                        parametersOut.Add(key, wsParameters[key]);
                }


                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = (ResultType)Convert.ToInt32(wsParameters["r"].ToString());

                    if (rtRes == ResultType.Result_OK)
                    {

                    }

                }


            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "QueryCity::Exception", LogLevels.logERROR);

            }

            return rtRes;

        }

        public ResultType QueryParkingOperationWithTimeSteps(string sUser, string sSessionID, string sPlate, DateTime dtQueryDate, int iSector, int iTariff, ref SortedList parametersOut)
        {
            ResultType rtRes = ResultType.Result_OK;

            string sXmlIn = "";
            string sXmlOut = "";

            try
            {

                integraMobileWS.integraMobileWS oIntegraMobileWS = new integraMobileWS.integraMobileWS();
                oIntegraMobileWS.Url = GetWSUrl();
                oIntegraMobileWS.Timeout = GetWSTimeout();

                if (!string.IsNullOrEmpty(GetWSHttpUser()))
                {
                    oIntegraMobileWS.Credentials = new System.Net.NetworkCredential(GetWSHttpUser(), GetWSHttpPassword());
                }

                string strvers = "1.0";

                string strMessage = "";
                string strAuthHash = "";

                strAuthHash = CalculateWSHash(string.Format("{0}{1}{2}{3:HHmmssddMMyy}{4}{5}{6}", sUser, sSessionID, sPlate, dtQueryDate, iSector, iTariff, strvers));

                strMessage = string.Format("<ipark_in><u>{0}</u><SessionID>{1}</SessionID><p>{2}</p><d>{3:HHmmssddMMyy}</d><g>{4}</g><ad>{5}</ad><vers>{6}</vers><ah>{7}</ah></ipark_in>",
                                           sUser, sSessionID, sPlate, dtQueryDate, iSector, iTariff, strvers, strAuthHash);


                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("QueryParkingOperationWithTimeSteps xmlIn={0}", sXmlIn), LogLevels.logDEBUG);

                string strOut = "";
                strOut = oIntegraMobileWS.QueryParkingOperationWithTimeSteps(strMessage);

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("QueryParkingOperationWithTimeSteps xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters2(strOut, out wsParameters);

                if (parametersOut != null)
                {
                    foreach (var key in wsParameters.Keys)
                        parametersOut.Add(key, wsParameters[key]);
                }


                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = (ResultType)Convert.ToInt32(wsParameters["r"].ToString());

                    if (rtRes == ResultType.Result_OK)
                    {

                    }

                }


            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "QueryParkingOperationWithTimeSteps::Exception", LogLevels.logERROR);

            }

            return rtRes;

        }

        public ResultType ConfirmParkingOperation(string sUser, string sSessionID, string sPlate, DateTime dtDate, int iSector, int iTariff, int iAmount, int iFEE, int iVAT, int iTotal, int iTime, DateTime dtIniDate, DateTime dtEndDate, bool bMadTarInfo, ref SortedList parametersOut)
        {
            ResultType rtRes = ResultType.Result_OK;

            string sXmlIn = "";
            string sXmlOut = "";

            try
            {

                integraMobileWS.integraMobileWS oIntegraMobileWS = new integraMobileWS.integraMobileWS();
                oIntegraMobileWS.Url = GetWSUrl();
                oIntegraMobileWS.Timeout = GetWSTimeout();

                if (!string.IsNullOrEmpty(GetWSHttpUser()))
                {
                    oIntegraMobileWS.Credentials = new System.Net.NetworkCredential(GetWSHttpUser(), GetWSHttpPassword());
                }

                string strvers = "1.0";

                string strMessage = "";
                string strAuthHash = "";

                strAuthHash = CalculateWSHash(string.Format("{0}{1}{2}{3}{4}{5:HHmmssddMMyy}{6}{7}{8}{9}{10}{11:HHmmssddMMyy}{12:HHmmssddMMyy}{13}{14}",
                                              sUser, sSessionID, sPlate, iSector, iTariff, dtDate, iAmount, iFEE, iVAT, iTotal, iTime, dtIniDate, dtEndDate, (bMadTarInfo ? 1 : 0), strvers));

                strMessage = string.Format("<ipark_in><u>{0}</u><SessionID>{1}</SessionID><p>{2}</p><g>{3}</g><ad>{4}</ad><d>{5:HHmmssddMMyy}</d><q>{6}</q><q_fee>{7}</q_fee><q_vat>{8}</q_vat><q_total>{9}</q_total><t>{10}</t><bd>{11:HHmmssddMMyy}</bd><ed>{12:HHmmssddMMyy}</ed><madtarinfo>{13}</madtarinfo><vers>{14}</vers><ah>{15}</ah></ipark_in>",
                                           sUser, sSessionID, sPlate, iSector, iTariff, dtDate, iAmount, iFEE, iVAT, iTotal, iTime, dtIniDate, dtEndDate, (bMadTarInfo ? 1 : 0), strvers, strAuthHash);


                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("ConfirmParkingOperation xmlIn={0}", sXmlIn), LogLevels.logDEBUG);

                string strOut = "";
                strOut = oIntegraMobileWS.ConfirmParkingOperation(strMessage);

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("ConfirmParkingOperation xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters2(strOut, out wsParameters);

                if (parametersOut != null)
                {
                    foreach (var key in wsParameters.Keys)
                        parametersOut.Add(key, wsParameters[key]);
                }


                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = (ResultType)Convert.ToInt32(wsParameters["r"].ToString());

                    if (rtRes == ResultType.Result_OK)
                    {

                    }

                }


            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "ConfirmParkingOperation::Exception", LogLevels.logERROR);

            }

            return rtRes;

        }

        public ResultType TransferBalance(string sUser, string sSessionID, string sPassword, int iAmount, string sDestinationEmail, ref SortedList parametersOut)
        {
            ResultType rtRes = ResultType.Result_OK;

            string sXmlIn = "";
            string sXmlOut = "";

            try
            {

                integraMobileWS.integraMobileWS oIntegraMobileWS = new integraMobileWS.integraMobileWS();
                oIntegraMobileWS.Url = GetWSUrl();
                oIntegraMobileWS.Timeout = GetWSTimeout();

                if (!string.IsNullOrEmpty(GetWSHttpUser()))
                {
                    oIntegraMobileWS.Credentials = new System.Net.NetworkCredential(GetWSHttpUser(), GetWSHttpPassword());
                }

                string strvers = "1.0";

                string strMessage = "";
                string strAuthHash = "";

                strAuthHash = CalculateWSHash(string.Format("{0}{1}{2}{3}{4}{5}",
                                              sUser, sPassword, sSessionID, sDestinationEmail, iAmount, strvers));

                strMessage = string.Format("<ipark_in><u>{0}</u><pass>{1}</pass><SessionID>{2}</SessionID><emailrecipient>{3}</emailrecipient><amountrecipient>{4}</amountrecipient><vers>{5}</vers><ah>{6}</ah></ipark_in>",
                                           sUser, sPassword, sSessionID, sDestinationEmail, iAmount, strvers, strAuthHash);


                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("TransferBalance xmlIn={0}", sXmlIn), LogLevels.logDEBUG);

                string strOut = "";
                strOut = oIntegraMobileWS.TransferBalance(strMessage);

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("TransferBalance xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters2(strOut, out wsParameters);

                if (parametersOut != null)
                {
                    foreach (var key in wsParameters.Keys)
                        parametersOut.Add(key, wsParameters[key]);
                }


                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = (ResultType)Convert.ToInt32(wsParameters["r"].ToString());

                    if (rtRes == ResultType.Result_OK)
                    {

                    }

                }


            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "TransferBalance::Exception", LogLevels.logERROR);

            }

            return rtRes;

        }

        public ResultType ConfirmRecharge(string sUser, string sSessionID, int iAmount, 
                                          string sOxxoToken, int? iOxxoCashMachine, string sOxxoEntryMode, decimal dOxxoTicket, decimal dOxxoFolio, DateTime dtOxxoAdminDate, string sOxxoStore, string sOxxoPartial,
                                          ref SortedList parametersOut)
        {
            ResultType rtRes = ResultType.Result_OK;

            string sXmlIn = "";
            string sXmlOut = "";

            try
            {

                integraMobileWS.integraMobileWS oIntegraMobileWS = new integraMobileWS.integraMobileWS();
                oIntegraMobileWS.Url = GetWSUrl();
                oIntegraMobileWS.Timeout = GetWSTimeout();

                if (!string.IsNullOrEmpty(GetWSHttpUser()))
                {
                    oIntegraMobileWS.Credentials = new System.Net.NetworkCredential(GetWSHttpUser(), GetWSHttpPassword());
                }

                string sUserIdent = sUser + "_" + DateTime.UtcNow.Ticks.ToString();
                string strvers = "1.0";
                int iAutRech = 0;                
                int iRechargeType = (int)PaymentMeanRechargeType.Oxxo;
                int iOSID = 5;
                
                string strMessage = "";
                string strAuthHash = "";


                strAuthHash = CalculateWSHash(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12:yyyyMMdd}{13}{14}{15}",
                                                            sUser, iAmount, iRechargeType, sSessionID, iOSID, sUserIdent, sUserIdent, sOxxoToken, iOxxoCashMachine, sOxxoEntryMode, dOxxoTicket, dOxxoFolio, dtOxxoAdminDate, sOxxoStore, sOxxoPartial, strvers));

                strMessage = string.Format("<ipark_in><u>{0}</u><q>{1}</q><t>{2}</t><SessionID>{3}</SessionID><OSID>{4}</OSID><IMEI>{5}</IMEI><WIFIMAC>{6}</WIFIMAC><oxxo_token>{7}</oxxo_token><oxxo_cashmachine>{8}</oxxo_cashmachine><oxxo_entrymode>{9}</oxxo_entrymode><oxxo_ticket>{10}</oxxo_ticket><oxxo_folio>{11}</oxxo_folio><oxxo_admindate>{12:yyyyMMdd}</oxxo_admindate><oxxo_store>{13}</oxxo_store><oxxo_partial>{14}</oxxo_partial><vers>{15}</vers><ah>{16}</ah></ipark_in>",
                                           sUser, iAmount, iRechargeType, sSessionID, iOSID, sUserIdent, sUserIdent, sOxxoToken, iOxxoCashMachine, sOxxoEntryMode, dOxxoTicket, dOxxoFolio, dtOxxoAdminDate, sOxxoStore, sOxxoPartial, strvers, strAuthHash);


                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("ConfirmRecharge xmlIn={0}", sXmlIn), LogLevels.logDEBUG);

                string strOut = "";
                strOut = oIntegraMobileWS.ConfirmRecharge(strMessage);

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("ConfirmRecharge xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters2(strOut, out wsParameters);

                if (parametersOut != null)
                {
                    foreach (var key in wsParameters.Keys)
                        parametersOut.Add(key, wsParameters[key]);
                }


                if (rtRes == ResultType.Result_OK)
                {

                    rtRes = (ResultType)Convert.ToInt32(wsParameters["r"].ToString());

                    if (rtRes == ResultType.Result_OK)
                    {

                    }

                }


            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "ConfirmRecharge::Exception", LogLevels.logERROR);

            }

            return rtRes;

        }

    }
}