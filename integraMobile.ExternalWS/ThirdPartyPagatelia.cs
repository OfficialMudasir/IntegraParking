using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;
using System.Globalization;
using System.Diagnostics;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using Ninject;

namespace integraMobile.ExternalWS
{
    public class ThirdPartyPagatelia : ThirdPartyBase
    {

        public ThirdPartyPagatelia() : base()
        {
            m_Log = new CLogWrapper(typeof(ThirdPartyPagatelia));
        }

        public ResultType QueryLogin(string sUser, string sPwd, DateTime dtQuery, decimal? dLatitude, decimal? dLongitude, out string sSessionID, out decimal? dBalance, out string sCurIsoCode)
        {

            ResultType rtRes = ResultType.Result_OK;
            sSessionID = "";
            dBalance = null;
            sCurIsoCode = "";

            string sIn = "";
            string sOut = "";
            Exception oNotificationEx = null;

            try
            {
                integraMobile.ExternalWS.PagateliaWS.ApiPayment oPagateliaWS = new PagateliaWS.ApiPayment();

                if (ConfigurationManager.AppSettings["PagateliaWsUrl"] != null)
                    oPagateliaWS.Url = ConfigurationManager.AppSettings["PagateliaWsUrl"].ToString();
                oPagateliaWS.Timeout = Get3rdPartyWSTimeout();
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["PagateliaWsHttpUser"]??""))
                    oPagateliaWS.Credentials = new System.Net.NetworkCredential((ConfigurationManager.AppSettings["PagateliaWsHttpUser"] ?? ""), ConfigurationManager.AppSettings["PagateliaWsHttpPassword"] ?? "");
 
                PagateliaWS.GPS oGPS = new PagateliaWS.GPS();
                oGPS.Latitude = dLatitude;
                oGPS.LatitudeSpecified = dLatitude.HasValue;
                oGPS.Longitude = dLongitude;
                oGPS.LongitudeSpecified = dLongitude.HasValue;
                                
                
                string strvers = "1.0";

                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";
                //nfi.NumberGroupSeparator = ",";

                string strAuthHash = CalculatePagateliaWsHash(string.Format("{0}{1}{2:HHmmssddMMyy}{3}{4}{5}", 
                                                              sUser, 
                                                              sPwd, 
                                                              dtQuery, 
                                                              (oGPS.Latitude.HasValue?oGPS.Latitude.Value.ToString(nfi):""),
                                                              (oGPS.Longitude.HasValue ? oGPS.Longitude.Value.ToString(nfi) : ""),
                                                              strvers));

                sIn = string.Format("userLogin:{0},password:{1},date:{2:HHmmssddMMyy},dateSpecified:{3},gps:{4},version:{5},hash:{6}",
                                                  sUser, 
                                                  sPwd, 
                                                  dtQuery, true,
                                                  (oGPS.Latitude.HasValue ? oGPS.Latitude.Value.ToString(nfi) : "") + " " + (oGPS.Longitude.HasValue ? oGPS.Longitude.Value.ToString(nfi) : ""),
                                                  strvers, strAuthHash);
                

                Logger_AddLogMessage(string.Format("PagateliaDoLogin In={0}", sIn), LogLevels.logDEBUG);

                var oRes = oPagateliaWS.DoLogin(sUser, sPwd, dtQuery, true, oGPS, strvers, strAuthHash);
                                
                rtRes = Convert_ResultTypePagateliaWS_TO_ResultType(oRes.CodeResult);

                sOut = string.Format("CodeResult:{0},IdSession:{1},Balance:{2},Currency:{3}",
                                           oRes.CodeResult, oRes.IdSession, oRes.Balance, oRes.Currency);

                Logger_AddLogMessage(string.Format("PagateliaDoLogin Out ={0}", sOut), LogLevels.logDEBUG);

                if (rtRes == ResultType.Result_OK)
                {
                    sSessionID = oRes.IdSession;
                    if (oRes.BalanceSpecified && oRes.Balance.HasValue) dBalance = oRes.Balance.Value;
                    sCurIsoCode = oRes.Currency;
                }

            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "PagateliaDoLogin::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sIn, sOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }

        public ResultType ConfirmRecharge(string sUser, string sSessionID, decimal dAmount, decimal? dLatitude, decimal? dLongitude, string sRechargeId, out decimal? dNewBalance)
        {

            ResultType rtRes = ResultType.Result_OK;            
            dNewBalance = null;

            string strIn = "";
            string strOut = "";
            Exception oNotificationEx = null;

            try
            {
                integraMobile.ExternalWS.PagateliaWS.ApiPayment oPagateliaWS = new PagateliaWS.ApiPayment();

                if (ConfigurationManager.AppSettings["PagateliaWsUrl"] != null)
                    oPagateliaWS.Url = ConfigurationManager.AppSettings["PagateliaWsUrl"].ToString();
                oPagateliaWS.Timeout = Get3rdPartyWSTimeout();
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["PagateliaWsHttpUser"] ?? ""))
                    oPagateliaWS.Credentials = new System.Net.NetworkCredential((ConfigurationManager.AppSettings["PagateliaWsHttpUser"] ?? ""), ConfigurationManager.AppSettings["PagateliaWsHttpPassword"] ?? "");

                PagateliaWS.GPS oGPS = new PagateliaWS.GPS();
                oGPS.Latitude = dLatitude;
                oGPS.LatitudeSpecified = dLatitude.HasValue;
                oGPS.Longitude = dLongitude;
                oGPS.LongitudeSpecified = dLongitude.HasValue;


                string strvers = "1.0";

                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberDecimalSeparator = ".";
                //nfi.NumberGroupSeparator = ",";

                string strAuthHash = CalculatePagateliaWsHash(string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}", 
                                                                            sUser, 
                                                                            "", "", 
                                                                            sSessionID, 
                                                                            sRechargeId,
                                                                            (oGPS.Latitude.HasValue?oGPS.Latitude.Value.ToString(nfi):""),
                                                                            (oGPS.Longitude.HasValue ? oGPS.Longitude.Value.ToString(nfi) : ""),
                                                                            dAmount,
                                                                            strvers));

                strIn = string.Format("userLogin:{0},imei:{1},wifimac:{2},idSession:{3},idRecharge:{4},gps:{5},amount:{6},amountSpecified:{7},version:{8},hash:{9}",
                                                    sUser,
                                                    "", "",
                                                    sSessionID,
                                                    sRechargeId,
                                                    (oGPS.Latitude.HasValue ? oGPS.Latitude.Value.ToString(nfi) : "") + " " + (oGPS.Longitude.HasValue ? oGPS.Longitude.Value.ToString(nfi) : ""),
                                                    dAmount, true,
                                                    strvers,
                                                    strAuthHash);
                
                Logger_AddLogMessage(string.Format("PagateliaDoRecharge In={0}", strIn), LogLevels.logDEBUG);

                var oRes = oPagateliaWS.DoRecharge(sUser, null, null, sSessionID, sRechargeId, oGPS, dAmount, true, strvers, strAuthHash);

                rtRes = Convert_ResultTypePagateliaWS_TO_ResultType(oRes.CodeResult);

                strOut = string.Format("CodeResult:{0},Balance:{1}",
                                            oRes.CodeResult, oRes.Balance);

                Logger_AddLogMessage(string.Format("PagateliaDoRecharge Out ={0}", strOut), LogLevels.logDEBUG);

                if (rtRes == ResultType.Result_OK)
                {
                    dNewBalance = oRes.Balance;
                }

            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "PagateliaDoRecharge::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strIn, strOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }

    }
}
