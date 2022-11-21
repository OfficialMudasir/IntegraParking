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
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using integraMobile.Domain;
using integraMobile.Domain.Abstract;
using integraMobile.Infrastructure;
using integraMobile.Infrastructure.Logging.Tools;
using Ninject;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace integraMobile.ExternalWS
{

    public enum ResultTypeStandardFineWS
    {
        Ok = 1,
        InvalidAuthenticationHashExternalService = -1,
        InvalidAuthenticationExternalService = -21,
        InvalidAuthentication = -101,
        TicketNotFound = -211,
        InstallationNotFound = -220,
        TicketNumberNotFound = -224,
        TicketTypeNotPayable = -226,
        TicketPaymentPeriodExpired = -227,
        TicketAlreadyCancelled = -228,
        TicketAlreadyAnulled = -229,
        TicketAlreadyRemitted = -230,
        TicketAlreadyPaid = -231,
        TicketNotClosed = -232,
        InvalidPaymentAmount = -233,
        InvalidExternalProvider = -234,
        InvalidAuthenticationHash = -920,
        ErrorDetected = -990,
        GenericError = -999
    }



    public enum ResultTypeSantBoiFineWS
    {
        Ok = 0,
        TicketAlreadyPaid = 1,
        TicketNotFound = 2,
        GenericError = 3
    }



    public class ThirdPartyFine : ThirdPartyBase
    {
        internal class CustomJsonWriter : JsonTextWriter
        {
            public CustomJsonWriter(TextWriter writer)
                : base(writer)
            {
                Formatting = Newtonsoft.Json.Formatting.Indented;
            }

            public override void WritePropertyName(string name)
            {
                if (name.StartsWith("@") || name.StartsWith("#"))
                {
                    base.WritePropertyName(name.Substring(1));
                }
                else
                {
                    base.WritePropertyName(name);
                }
            }
        }

        internal class GDLGTechnaAPI
        {
            private WebService oWS = null;
            private string strResultString = "";

            public GDLGTechnaAPI(string webserviceEndpoint)
            {
                oWS = new WebService(webserviceEndpoint);
            }

            public int Timeout
            {
                set
                {
                    oWS.Timeout = value;

                }
                get
                {
                    return oWS.Timeout;
                }
            }


            public string Username
            {
                set
                {
                    oWS.Username = value;

                }
            }


            public string Password
            {
                set
                {
                    oWS.Password = value;

                }
            }

            public string ResultXML
            {
                get
                {
                    return strResultString;
                }
            }

            public ResultType GetOutstandingTickets(string strTicketNumber, string strType, string strProvider, string strSecurityToken, out SortedList oTicket)
            {
                ResultType rtRes = ResultType.Result_Error_Generic;
                oTicket = null;
                oWS.PreInvoke();

                oWS.AddParameter("TicketNo", strTicketNumber);
                oWS.AddParameter("Type", strType);
                oWS.AddParameter("Source", strProvider);
                oWS.AddParameter("SecurityToken", strSecurityToken);

                try
                {
                    oWS.Invoke("GetOutstandingTicketsRequest", "GetOutstandingTicketsResponse", "http://soap.payment.seci.cc.gti.com/");
                    strResultString = oWS.ResultString;

                    if (oWS.GetOutputElementCount("Success") == 1)
                    {
                        SortedList oList = null;

                        int iCount = oWS.GetOutputElementCount("Success/OutstandingTickets/Ticket");

                        if (iCount > 1)
                        {
                            int i = 0;

                            while (i < iCount)
                            {
                                oList = oWS.GetOutputElement("Success/OutstandingTickets/Ticket/" + i.ToString());
                                if (oList["TicketNo"].ToString() == strTicketNumber)
                                {
                                    break;
                                }
                                else
                                    oList = null;
                                i++;
                            }
                        }
                        else if (iCount == 1)
                        {
                            oList = oWS.GetOutputElement("Success/OutstandingTickets/Ticket");
                            if (oList != null)
                            {
                                if (oList["TicketNo"].ToString() != strTicketNumber)
                                {
                                    oList = null;
                                    rtRes = ResultType.Result_Error_Fine_Number_Already_Paid;
                                }
                            }
                        }
                        else
                        {
                            rtRes = ResultType.Result_Error_Fine_Number_Already_Paid;
                        }


                        if (oList != null)
                        {
                            oTicket = oList;

                            if (oTicket["TicketStatus"].ToString() == "PA")
                                rtRes = ResultType.Result_Error_Fine_Number_Already_Paid;
                            else if (oTicket["PayableStatus"].ToString() == "Y")
                                rtRes = ResultType.Result_OK;
                            else
                                rtRes = ResultType.Result_Error_Fine_Type_Not_Payable;

                        }
                    }
                    else if (oWS.GetOutputElementCount("InvalidParameters/InvalidParameter") == 1)
                    {

                        SortedList oList = oWS.GetOutputElement("InvalidParameters");

                        if (oList["InvalidParameter"].ToString().ToLower().Contains("invalid"))
                        {
                            rtRes = ResultType.Result_Error_Invalid_Input_Parameter;
                        }
                        else if (oList["InvalidParameter"].ToString().ToLower().Contains("found"))
                        {
                            rtRes = ResultType.Result_Error_Fine_Number_Not_Found;
                        }

                    }
                    else if (oWS.GetOutputElementCount("Error/ErrorMessage") == 1)
                    {
                        rtRes = ResultType.Result_Error_Generic;
                    }


                }
                catch (Exception e)
                {
                    strResultString = e.Message;
                    rtRes = ResultType.Result_Error_Generic;
                }
                finally { oWS.PosInvoke(); }

                return rtRes;
            }

            public ResultType PayTicket(string strTicketNumber, double dAmount, string strPaymentType, DateTime dtPayment, string strProvider,
                                        decimal dTicketId, string strSecurityToken, out SortedList oTransaction)
            {
                ResultType rtRes = ResultType.Result_Error_Generic;
                oTransaction = null;
                oWS.PreInvoke();

                oWS.AddParameter("TicketNo", strTicketNumber);
                oWS.AddParameter("Amount", dAmount.ToString(CultureInfo.InvariantCulture));
                oWS.AddParameter("PaymentType", strPaymentType);
                oWS.AddParameter("PaymentDateTime", dtPayment.ToString("yyyy-MM-dd HH:mm:ss ") + dtPayment.ToString("zzz").Replace(":", ""));
                oWS.AddParameter("TransactionBy", strProvider);
                oWS.AddParameter("Source", strProvider);
                oWS.AddParameter("ReferenceNo", dTicketId.ToString());
                oWS.AddParameter("SecurityToken", strSecurityToken);


                try
                {
                    oWS.Invoke("PayTicketsRequest", "PayTicketsResponse", "http://soap.payment.seci.cc.gti.com/");
                    strResultString = oWS.ResultString;

                    if (oWS.GetOutputElementCount("Success") == 1)
                    {

                        oTransaction = oWS.GetOutputElement("Success/TransactionSet/Transaction");

                        if (oTransaction != null)
                            rtRes = ResultType.Result_OK;

                    }
                    else if (oWS.GetOutputElementCount("InvalidParameters/InvalidParameter") == 1)
                    {

                        SortedList oList = oWS.GetOutputElement("InvalidParameters");

                        if (oList["InvalidParameter"].ToString().ToLower().Contains("invalid"))
                        {
                            rtRes = ResultType.Result_Error_Invalid_Input_Parameter;
                        }
                        else if (oList["InvalidParameter"].ToString().ToLower().Contains("found"))
                        {
                            rtRes = ResultType.Result_Error_Fine_Number_Not_Found;
                        }

                    }
                    else if (oWS.GetOutputElementCount("Error/ErrorMessage") == 1)
                    {
                        rtRes = ResultType.Result_Error_Generic;
                    }


                }
                catch (Exception e)
                {
                    strResultString = e.Message;
                    rtRes = ResultType.Result_Error_Generic;
                }
                finally { oWS.PosInvoke(); }

                return rtRes;
            }
        }

        public ThirdPartyFine() : base()
        {
            m_Log = new CLogWrapper(typeof(ThirdPartyFine));
        }

       /* public ResultType EysaQueryFinePayment(string strFineNumber, DateTime dtFineQuery, USER oUser, INSTALLATION oInstallation,
                                                out int iQuantity, out string strPlate,
                                                out string strArticleType, out string strArticleDescription)
        {
            ResultType rtRes = ResultType.Result_OK;
            iQuantity = 0;
            strPlate = "";
            strArticleDescription = "";
            strArticleType = "";

            try
            {
                SortedList parametersIn = new SortedList();
                SortedList parametersOut = new SortedList();

                parametersIn["f"] = strFineNumber;

                rtRes = EysaQueryFinePaymentQuantity(parametersIn, strFineNumber, dtFineQuery, oUser, oInstallation, ref parametersOut);

                if (rtRes == ResultType.Result_OK)
                {
                    iQuantity = Convert.ToInt32(parametersOut["q"].ToString());
                    strPlate = parametersOut["lp"].ToString();
                    strArticleType = parametersOut["ta"].ToString();
                    strArticleDescription = parametersOut["dta"].ToString();
                }



            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "EysaQueryFinePaymentQuantity::Exception", LogLevels.logERROR);

            }


            return rtRes;


        }

        */



        public ResultType StandardQueryFinePaymentQuantity(string strFineNumber, DateTime dtFineQuery, USER oUser, INSTALLATION oInstallation, int? iWSTimeout, ref SortedList parametersOut)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;


            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                string strURL = oInstallation.INS_FINE_WS_URL + "/querypaymentinfo";
                WebRequest request = WebRequest.Create(strURL);
                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    request.Credentials = new System.Net.NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                }

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                string strFineID = strFineNumber;
                DateTime dtInstallation = dtFineQuery;                
                string strCityID = oInstallation.INS_STANDARD_CITY_ID;
                string strProviderName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();

                Dictionary<string, object> ojsonInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oiparkticketInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oDataObjectDict = new Dictionary<string, object>();
               
                oDataObjectDict["cityid"] = strCityID;
                oDataObjectDict["ticketnumber"] = strFineID;
                oDataObjectDict["date"] = dtInstallation.ToString("HHmmssddMMyyyy");
                oDataObjectDict["provider"] = strProviderName;
                oDataObjectDict["ah"] = CalculateStandardWSHash(oInstallation.INS_FINE_WS_AUTH_HASH_KEY,
                                        string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}", strCityID, strFineID, dtInstallation, strProviderName)); ;
                oiparkticketInObjectDict["iparkticket_in"] = oDataObjectDict;

                ojsonInObjectDict["jsonIn"] = JsonConvert.SerializeObject(oiparkticketInObjectDict).ToString();
                var json = JsonConvert.SerializeObject(ojsonInObjectDict);

                Logger_AddLogMessage(string.Format("StandardQueryFinePaymentQuantity request.url={0}, Timeout={2}, request.json={1}", strURL, json, request.Timeout), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.             

                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.

                        Logger_AddLogMessage(string.Format("StandardQueryFinePaymentQuantity response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.

                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        var strResult = oResponse["Result"];

                        long lResult=Convert.ToInt32(strResult);

                        rtRes = Convert_ResultTypeStandardFineWS_TO_ResultType((ResultTypeStandardFineWS)lResult);

                        if (rtRes == ResultType.Result_OK)
                        {

                            dynamic oData = JsonConvert.DeserializeObject(oResponse["Data"].ToString()); ;
                            lResult = Convert.ToInt64(oData["res"]);
                            rtRes = Convert_ResultTypeStandardFineWS_TO_ResultType((ResultTypeStandardFineWS)lResult);

                            if (rtRes == ResultType.Result_OK)
                            {
                                parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                                parametersOut["q"] = oData["amount"];
                                parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;
                                parametersOut["lp"] = oData["plate"];

                                DateTime dt = DateTime.ParseExact(oData["ticketdate"].ToString(), "HHmmssddMMyyyy",
                                            CultureInfo.InvariantCulture);
                               
                                parametersOut["d"] = dt.ToString("HHmmssddMMyy");

                                try
                                {
                                    dt = DateTime.ParseExact(oData["maximumPayDate"].ToString(), "HHmmssddMMyyyy",
                                            CultureInfo.InvariantCulture);
                                    parametersOut["df"] = dt.ToString("HHmmssddMMyy");
                                }
                                catch
                                {
                                }
                                parametersOut["ta"] = oData["code"];
                                parametersOut["dta"] = oData ["description"];                                

                            }
                            else
                            {

                                try
                                {
                                    parametersOut["lp"] = oData["plate"].ToString().Trim().Replace(" ", "");
                                    DateTime dt = DateTime.ParseExact(oData["ticketdate"].ToString(), "HHmmssddMMyyyy",
                                                CultureInfo.InvariantCulture);
                                    parametersOut["d"] = dt.ToString("HHmmssddMMyy");
                                    try
                                    {
                                        dt = DateTime.ParseExact(oData["maximumPayDate"].ToString(), "HHmmssddMMyyyy",
                                                CultureInfo.InvariantCulture);
                                        parametersOut["df"] = dt.ToString("HHmmssddMMyy");
                                    }
                                    catch
                                    {
                                    } 
                                    parametersOut["ta"] = oData["code"];
                                    parametersOut["dta"] = oData["description"];
                                }
                                catch
                                {

                                }
                            }

                            var strTicketNumber = oData["ticketnumber"].ToString();
                            if (strFineID != strTicketNumber)
                            {
                                parametersOut["fnumber"] = strTicketNumber;
                            }

                        }

                       
                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "StandardQueryFinePaymentQuantity::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rtRes);
                }

               

            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "StandardQueryFinePaymentQuantity::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;          
        }

        public ResultType BilbaoIntegrationQueryFinePaymentQuantity(string strFineNumber, DateTime dtFineQuery, USER oUser, INSTALLATION oInstallation, string strCulturePrefix, int? iWSTimeout, ref SortedList parametersOut)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;


            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);
                AddTLS12Support();
                var oParkWS = new BilbaoParkWsIntegraExternalService.integraExternalServices();
                oParkWS.Url = oInstallation.INS_FINE_WS_URL;
                oParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();
                if (!string.IsNullOrEmpty(oInstallation.INS_PARK_WS_HTTP_USER))
                {
                    oParkWS.Credentials = new NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                }

                string strFineID = strFineNumber;
                DateTime dtInstallation = dtFineQuery;
                string strCityID = oInstallation.INS_STANDARD_CITY_ID;
                string strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();
                string strHashKey = oInstallation.INS_FINE_WS_AUTH_HASH_KEY;
                string strAuthHash = "";
                string strvers = "1.0";
                strAuthHash = CalculateStandardWSHash(strHashKey,
                       string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}",strCityID, strFineID, dtInstallation, strvers, strCompanyName));
                //var strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id><f>{1}</f><d>{2:HHmmssddMMyyyy}</d>" +
                //                              "<ver>{3}</ver><em>{4}</em>" +
                //                              "<ah>{5}</ah></ipark_in>",
                //       strCityID, strFineID, dtInstallation, strvers, strCompanyName, strAuthHash);
                var strMessage = string.Format("<ipark_in><city_id>{0}</city_id><f>{1}</f><d>{2:yyyy-MM-ddThh:mm:ss.fff}</d>" +
                              "<ver>{3}</ver><em>{4}</em>" +
                              "<ah>{5}</ah></ipark_in>",
       "40100", strFineID, dtInstallation, strvers, strCompanyName, "096CQBZox4oN++LHBc11uL9/gvMYD8LitOZLhYjmIg0=");
                sXmlIn = PrettyXml(strMessage);
                Logger_AddLogMessage(string.Format("BilbaoIntegrationQueryFinePaymentQuantity Timeout={1} xmlIn ={0}", sXmlIn, oParkWS.Timeout), LogLevels.logDEBUG);
                string strOut = oParkWS.QueryFinePaymentQuantity(strMessage);
                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");
                sXmlOut = PrettyXml(strOut);
                Logger_AddLogMessage(string.Format("BilbaoIntegrationQueryFinePaymentQuantity xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);
                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {

                    if (Convert.ToInt32(wsParameters["r"].ToString()) == (int)ResultType.Result_OK)
                    {
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                        parametersOut["q"] = wsParameters["q"];
                        parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;
                        parametersOut["lp"] = wsParameters["lp"].ToString().Trim().Replace(" ", "");

                        DateTime dt = DateTime.ParseExact(wsParameters["d"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                     CultureInfo.InvariantCulture);
                        parametersOut["d"] = dt.ToString("HHmmssddMMyy");
                        dt = DateTime.ParseExact(wsParameters["df"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                                             CultureInfo.InvariantCulture);
                        parametersOut["df"] = dt.ToString("HHmmssddMMyy");
                        parametersOut["ta"] = wsParameters["ta"];
                        if (wsParameters.ContainsKey("dta"))
                            parametersOut["dta"] = wsParameters["dta"];
                        else
                        {
                            if (wsParameters.ContainsKey("dta_lang_0_" + strCulturePrefix))
                                parametersOut["dta"] = wsParameters["dta_lang_0_" + strCulturePrefix];
                            else if (wsParameters.ContainsKey("dta_lang_0_es"))
                                parametersOut["dta"] = wsParameters["dta_lang_0_es"];
                            else
                                parametersOut["dta"] = "---------------";

                            if (wsParameters.ContainsKey("dta_sector"))
                                parametersOut["sector"] = wsParameters["dta_sector"];
                            if (wsParameters.ContainsKey("dta_user"))
                                parametersOut["enforcuser"] = wsParameters["dta_user"];
                        }
                        if (wsParameters.ContainsKey("lit"))
                            parametersOut["lit"] = wsParameters["lit"];

                    }
                    else
                    {
                        //denuncia ya remesada = denuncia encontrada pero el plazo de anulación ya ha pasado.
                        parametersOut["r"] = ((Convert.ToInt32(wsParameters["r"])) == -4) ?
                            Convert.ToInt32(ResultType.Result_Error_Fine_Payment_Period_Expired) : Convert.ToInt32(wsParameters["r"]);

                        rtRes = (ResultType)parametersOut["r"];

                        try
                        {
                            parametersOut["lp"] = wsParameters["lp"].ToString().Trim().Replace(" ", "");
                            DateTime dt = DateTime.ParseExact(wsParameters["d"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                         CultureInfo.InvariantCulture);
                            parametersOut["d"] = dt.ToString("HHmmssddMMyy");
                            dt = DateTime.ParseExact(wsParameters["df"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                                                 CultureInfo.InvariantCulture);
                            parametersOut["df"] = dt.ToString("HHmmssddMMyy");
                            parametersOut["ta"] = wsParameters["ta"];
                            if (wsParameters.ContainsKey("dta"))
                                parametersOut["dta"] = wsParameters["dta"];
                            else
                                parametersOut["dta"] = wsParameters["dta_lang_0_es"];
                        }
                        catch
                        {

                        }

                    }

                    if (wsParameters.ContainsKey("fnumber"))
                        parametersOut["fnumber"] = Regex.Replace(wsParameters["fnumber"].ToString(), "[^0-9]", "");
                    else
                    {
                        // *** HB ***
                        //if (oInstallation.INS_ID == 28) parametersOut["fnumber"] = "111111-1";
                        // *** HB ***
                    }

                }
            }
                catch (Exception e)
                {
                oNotificationEx = e;
                Logger_AddLogException(e, "StandardQueryFinePaymentQuantity::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rtRes);
                }
            finally
            {
                watch.Stop();
                watch = null;

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;
        }

        public ResultType StandardQueryFinePaymentQuantityMultiInstallations(string strFineNumber, DateTime dtFineQuery, USER oUser, List<string> oCitiesIds, string sUrl, 
                                                                             string sHashKey, string sHttpUser, string sHttpPassword, string sCurIsoCode, int? iWSTimeout, 
                                                                             ref SortedList parametersOut, out string sCityIDOut)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;

            sCityIDOut = "";

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                string strURL = sUrl + "/TicketSearch";
                WebRequest request = WebRequest.Create(strURL);
                if (!string.IsNullOrEmpty(sHttpUser))
                {
                    request.Credentials = new System.Net.NetworkCredential(sHttpUser, sHttpPassword);
                }

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();


                string strFineID = strFineNumber;
                DateTime dtInstallation = dtFineQuery;
                string strCitiesIDs = string.Join(",", oCitiesIds);
                string strProviderName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();



                Dictionary<string, object> ojsonInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oiparkticketInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oDataObjectDict = new Dictionary<string, object>();

                oDataObjectDict["citiesids"] = strCitiesIDs;
                oDataObjectDict["ticketnumber"] = strFineID;
                oDataObjectDict["date"] = dtInstallation.ToString("HHmmssddMMyyyy");
                oDataObjectDict["provider"] = strProviderName;
                oDataObjectDict["ah"] = CalculateStandardWSHash(sHashKey,
                                        string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}", strCitiesIDs, strFineID, dtInstallation, strProviderName)); ;
                oiparkticketInObjectDict["iparkticket_in"] = oDataObjectDict;

                ojsonInObjectDict["jsonIn"] = JsonConvert.SerializeObject(oiparkticketInObjectDict).ToString();
                var json = JsonConvert.SerializeObject(ojsonInObjectDict);

                Logger_AddLogMessage(string.Format("StandardQueryFinePaymentQuantityMultiInstallations request.url={0}, Timeout={2} request.json={1}", strURL, json, request.Timeout), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.             

                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.

                        Logger_AddLogMessage(string.Format("StandardQueryFinePaymentQuantityMultiInstallations response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.

                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        var strResult = oResponse["Result"];

                        long lResult = Convert.ToInt32(strResult);

                        rtRes = Convert_ResultTypeStandardFineWS_TO_ResultType((ResultTypeStandardFineWS)lResult);

                        if (rtRes == ResultType.Result_OK)
                        {

                            dynamic oData = JsonConvert.DeserializeObject(oResponse["Data"].ToString()); ;
                            lResult = Convert.ToInt64(oData["res"]);
                            rtRes = Convert_ResultTypeStandardFineWS_TO_ResultType((ResultTypeStandardFineWS)lResult);

                            if (rtRes == ResultType.Result_OK)
                            {
                                parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                                parametersOut["q"] = oData["amount"];
                                parametersOut["cur"] = sCurIsoCode;
                                parametersOut["lp"] = oData["plate"];

                                DateTime dt = DateTime.ParseExact(oData["ticketdate"].ToString(), "HHmmssddMMyyyy",
                                            CultureInfo.InvariantCulture);

                                parametersOut["d"] = dt.ToString("HHmmssddMMyy");

                                try
                                {
                                    dt = DateTime.ParseExact(oData["maximumPayDate"].ToString(), "HHmmssddMMyyyy",
                                            CultureInfo.InvariantCulture);
                                    parametersOut["df"] = dt.ToString("HHmmssddMMyy");
                                }
                                catch
                                {
                                }
                                parametersOut["ta"] = oData["code"];
                                parametersOut["dta"] = oData["description"];

                            }
                            else
                            {

                                try
                                {
                                    parametersOut["lp"] = oData["plate"].ToString().Trim().Replace(" ", "");
                                    DateTime dt = DateTime.ParseExact(oData["ticketdate"].ToString(), "HHmmssddMMyyyy",
                                                CultureInfo.InvariantCulture);
                                    parametersOut["d"] = dt.ToString("HHmmssddMMyy");
                                    try
                                    {
                                        dt = DateTime.ParseExact(oData["maximumPayDate"].ToString(), "HHmmssddMMyyyy",
                                                CultureInfo.InvariantCulture);
                                        parametersOut["df"] = dt.ToString("HHmmssddMMyy");
                                    }
                                    catch
                                    {
                                    }
                                    parametersOut["ta"] = oData["code"];
                                    parametersOut["dta"] = oData["description"];
                                }
                                catch
                                {

                                }
                            }

                            var strTicketNumber = oData["ticketnumber"].ToString();
                            if (strFineID != strTicketNumber)
                            {
                                parametersOut["fnumber"] = strTicketNumber;
                            }

                            try
                            {
                                sCityIDOut = oData["cityid"].ToString();
                            }
                            catch { }

                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "StandardQueryFinePaymentQuantityMultiInstallations::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rtRes);
                }



            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "StandardQueryFinePaymentQuantityMultiInstallations::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;
        }

        public ResultType UploadAppealImages(string jsonData, string strFineNumber, DateTime dtFineQuery, USER oUser, List<string> oCitiesIds, string sUrl,
                                                                             string sHashKey, string sHttpUser, string sHttpPassword, string sCurIsoCode, int? iWSTimeout,
                                                                             ref SortedList parametersOut, out string sCityIDOut)
        {
            AppealReturnModel model = new AppealReturnModel();
            model = JsonConvert.DeserializeObject<AppealReturnModel>(jsonData);

            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;

            sCityIDOut = "";

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                string strURL = sUrl + "/uploadappealimages";
                WebRequest request = WebRequest.Create(strURL);
                if (!string.IsNullOrEmpty(sHttpUser))
                {
                    request.Credentials = new System.Net.NetworkCredential(sHttpUser, sHttpPassword);
                }

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();


                string strFineID = strFineNumber;
                DateTime dtInstallation = dtFineQuery;
                string strCitiesIDs = string.Join(",", oCitiesIds);
                string strProviderName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();



                Dictionary<string, object> ojsonInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oiparkticketInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oDataObjectDict = new Dictionary<string, object>();

                oDataObjectDict["citiesids"] = strCitiesIDs;
                oDataObjectDict["ticketnumber"] = strFineID;
                oDataObjectDict["date"] = dtInstallation.ToString("HHmmssddMMyyyy");
                oDataObjectDict["provider"] = strProviderName;
                oDataObjectDict["ah"] = CalculateStandardWSHash(sHashKey,
                                        string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}", strCitiesIDs, strFineID, dtInstallation, strProviderName)); ;
                oDataObjectDict["appealimageslist"] = model.AppealData.AppealImageList;

                oiparkticketInObjectDict["iparkticket_in"] = oDataObjectDict;

                ojsonInObjectDict["jsonIn"] = JsonConvert.SerializeObject(oiparkticketInObjectDict).ToString();
                var json = JsonConvert.SerializeObject(ojsonInObjectDict);

                Logger_AddLogMessage(string.Format("StandardQueryFinePaymentQuantityMultiInstallations request.url={0}, Timeout={2} request.json={1}", strURL, json, request.Timeout), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.             

                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.

                        Logger_AddLogMessage(string.Format("StandardQueryFinePaymentQuantityMultiInstallations response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.

                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        var strResult = oResponse["Result"];

                        long lResult = Convert.ToInt32(strResult);

                        rtRes = Convert_ResultTypeStandardFineWS_TO_ResultType((ResultTypeStandardFineWS)lResult);

                        if (rtRes == ResultType.Result_OK)
                        {

                            dynamic oData = JsonConvert.DeserializeObject(oResponse["Data"].ToString()); ;
                            model.AppealData.AppealImageIds = JsonConvert.DeserializeObject<List<TicketImageReturnModel>>(oData["imageid"].ToString());
                            
                            lResult = Convert.ToInt64(oData["res"]);
                            rtRes = Convert_ResultTypeStandardFineWS_TO_ResultType((ResultTypeStandardFineWS)lResult);

                            if (rtRes == ResultType.Result_OK)
                            {
                                parametersOut["AppealImageIds"] = model.AppealData.AppealImageIds;

                                parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                                parametersOut["q"] = oData["amount"];
                                parametersOut["cur"] = sCurIsoCode;
                                parametersOut["lp"] = oData["plate"];

                                //DateTime dt = DateTime.ParseExact(oData["ticketdate"].ToString(), "HHmmssddMMyyyy",
                                //            CultureInfo.InvariantCulture);

                                //parametersOut["d"] = dt.ToString("HHmmssddMMyy");

                                //try
                                //{
                                //    dt = DateTime.ParseExact(oData["maximumPayDate"].ToString(), "HHmmssddMMyyyy",
                                //            CultureInfo.InvariantCulture);
                                //    parametersOut["df"] = dt.ToString("HHmmssddMMyy");
                                //}
                                //catch
                                //{
                                //}
                                //parametersOut["ta"] = oData["code"];
                                //parametersOut["dta"] = oData["description"];

                            }
                            else
                            {

                                try
                                {
                                    parametersOut["lp"] = oData["plate"].ToString().Trim().Replace(" ", "");
                                    //DateTime dt = DateTime.ParseExact(oData["ticketdate"].ToString(), "HHmmssddMMyyyy",
                                    //            CultureInfo.InvariantCulture);
                                    //parametersOut["d"] = dt.ToString("HHmmssddMMyy");
                                    //try
                                    //{
                                    //    dt = DateTime.ParseExact(oData["maximumPayDate"].ToString(), "HHmmssddMMyyyy",
                                    //            CultureInfo.InvariantCulture);
                                    //    parametersOut["df"] = dt.ToString("HHmmssddMMyy");
                                    //}
                                    //catch
                                    //{
                                    //}
                                    parametersOut["ta"] = oData["code"];
                                    parametersOut["dta"] = oData["description"];
                                }
                                catch
                                {

                                }
                            }

                            

                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "Upload Appeal Images1::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rtRes);
                }



            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "Upload appeal Images2 ::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;
        }

        public ResultType SaveAppealInformation(string jsonData, string strFineNumber, DateTime dtFineQuery, USER oUser, List<string> oCitiesIds, string sUrl,
                                                                             string sHashKey, string sHttpUser, string sHttpPassword, string sCurIsoCode, int? iWSTimeout,
                                                                             ref SortedList parametersOut, out string sCityIDOut)
        {
            AppealReturnModel model = new AppealReturnModel();
            model = JsonConvert.DeserializeObject<AppealReturnModel>(jsonData);

            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;

            sCityIDOut = "";

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                string strURL = sUrl + "/SaveAppeal";
                WebRequest request = WebRequest.Create(strURL);
                if (!string.IsNullOrEmpty(sHttpUser))
                {
                    request.Credentials = new System.Net.NetworkCredential(sHttpUser, sHttpPassword);
                }

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();


                string strFineID = strFineNumber;
                DateTime dtInstallation = dtFineQuery;
                string strCitiesIDs = string.Join(",", oCitiesIds);
                string strProviderName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();

                Dictionary<string, object> ojsonInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oiparkticketInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oDataObjectDict = new Dictionary<string, object>();

                oDataObjectDict["citiesids"] = strCitiesIDs;
                oDataObjectDict["ticketnumber"] = strFineID;
                oDataObjectDict["plateNumber"] = model.Plate;
                oDataObjectDict["firstName"] = model.AppealData.FirstName;
                oDataObjectDict["lastName"] = model.AppealData.LastName;
                oDataObjectDict["address"] = model.AppealData.Address;
                oDataObjectDict["email"] = model.AppealData.Email;
                oDataObjectDict["appealReason"] = model.AppealData.AppealReason;
                oDataObjectDict["appealDescription"] = model.AppealData.AppealDesc;
                oDataObjectDict["appealImages"] = model.AppealData.AppealImageIds;
                oDataObjectDict["device"] = model.AppealData.Device;
                oDataObjectDict["date"] = dtInstallation.ToString("HHmmssddMMyyyy");
                oDataObjectDict["provider"] = strProviderName;
                oDataObjectDict["ah"] = CalculateStandardWSHash(sHashKey,
                                        string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}", strCitiesIDs, strFineID, dtInstallation, strProviderName)); ;

                oiparkticketInObjectDict["iparkticket_in"] = oDataObjectDict;

                ojsonInObjectDict["jsonIn"] = JsonConvert.SerializeObject(oiparkticketInObjectDict).ToString();
                var json = JsonConvert.SerializeObject(ojsonInObjectDict);

                Logger_AddLogMessage(string.Format("StandardQueryFinePaymentQuantityMultiInstallations request.url={0}, Timeout={2} request.json={1}", strURL, json, request.Timeout), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.             

                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);
                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.

                        Logger_AddLogMessage(string.Format("StandardQueryFinePaymentQuantityMultiInstallations response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.

                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        var strResult = oResponse["Result"];

                        long lResult = Convert.ToInt32(strResult);

                        rtRes = Convert_ResultTypeStandardFineWS_TO_ResultType((ResultTypeStandardFineWS)lResult);

                        if (rtRes == ResultType.Result_OK)
                        {

                            dynamic oData = JsonConvert.DeserializeObject(oResponse["Data"].ToString()); ;

                            lResult = Convert.ToInt64(oData["res"]);
                            //rtRes = Convert_ResultTypeStandardFineWS_TO_ResultType((ResultTypeStandardFineWS)lResult);

                            if (rtRes == ResultType.Result_OK)
                            {
                                //parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                                parametersOut["r"] = lResult;
                                rtRes = (ResultType)lResult;
                                parametersOut["lp"] = oData["plate"];
                            }
                            else
                            {
                                try
                                {
                                    parametersOut["lp"] = oData["plate"].ToString().Trim().Replace(" ", "");
                                    
                                    parametersOut["ta"] = oData["code"];
                                    parametersOut["dta"] = oData["description"];
                                }
                                catch
                                {

                                }
                            }

                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "Upload Appeal Images1::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rtRes);
                }

            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "Upload appeal Images2 ::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;
        }



        #region my new chanhes in methods 

        public ResultType StandardQueryTicketFinePaymentQuantityMultiInstallations(string strFineNumber, long? Ticketid, string strplate, DateTime dtFineQuery, USER oUser, List<string> oCitiesIds, string sUrl,
                                                                           string sHashKey, string sHttpUser, string sHttpPassword, string sCurIsoCode, int? iWSTimeout,
                                                                           ref SortedList parametersOut, out string sCityIDOut)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;

            sCityIDOut = "";

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                string strURL = "";
                if (Ticketid != null)
                {
                    strURL = sUrl + "/GetTicketById";
                }
                else
                {
                    strURL = sUrl + "/TicketSearch";
                }
                WebRequest request = WebRequest.Create(strURL);
                if (!string.IsNullOrEmpty(sHttpUser))
                {
                    request.Credentials = new System.Net.NetworkCredential(sHttpUser, sHttpPassword);
                }
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();


                string strFineID = strFineNumber;
                DateTime dtInstallation = dtFineQuery;
                string strCitiesIDs = string.Join(",", oCitiesIds);
                string strProviderName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();



                Dictionary<string, object> ojsonInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oiparkticketInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oDataObjectDict = new Dictionary<string, object>();

                oDataObjectDict["citiesids"] = strCitiesIDs;
                oDataObjectDict["ticketnumber"] = strFineID;
                oDataObjectDict["plate"] = strplate;
                if(Ticketid != null) oDataObjectDict["TicketId"] = Ticketid; //Get Selected for contest Ticket by Ticket Id 
                oDataObjectDict["date"] = dtInstallation.ToString("HHmmssddMMyyyy");
                oDataObjectDict["provider"] = strProviderName;
                oDataObjectDict["ah"] = CalculateStandardWSHash(sHashKey,
                                        string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}", strCitiesIDs, strFineID, dtInstallation, strProviderName)); ;
                oiparkticketInObjectDict["iparkticket_in"] = oDataObjectDict;

                ojsonInObjectDict["jsonIn"] = JsonConvert.SerializeObject(oiparkticketInObjectDict).ToString();
                var json = JsonConvert.SerializeObject(ojsonInObjectDict);

                Logger_AddLogMessage(string.Format("StandardQueryFinePaymentQuantityMultiInstallations request.url={0}, Timeout={2} request.json={1}", strURL, json, request.Timeout), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.             

                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.

                        Logger_AddLogMessage(string.Format("StandardQueryFinePaymentQuantityMultiInstallations response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.

                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        var strResult = oResponse["Result"];

                        long lResult = Convert.ToInt32(strResult);

                        rtRes = Convert_ResultTypeStandardFineWS_TO_ResultType((ResultTypeStandardFineWS)lResult);

                        if (rtRes == ResultType.Result_OK)
                        {

                            dynamic oData = JsonConvert.DeserializeObject(oResponse["Data"].ToString());
                            AppealReturnData appealdata = JsonConvert.DeserializeObject<AppealReturnData>(oResponse["Data"].ToString());
                            
                            lResult = Convert.ToInt64(oData["res"]);
                            rtRes = Convert_ResultTypeStandardFineWS_TO_ResultType((ResultTypeStandardFineWS)lResult);

                            if (rtRes == ResultType.Result_OK)
                            {
                                if(Ticketid != null)
                                {
                                    parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                                    parametersOut["Ticket"] = appealdata.Ticket;
                                    parametersOut["q"] = appealdata.Ticket.TickBalance;
                                    parametersOut["cur"] = sCurIsoCode;
                                    parametersOut["lp"] = appealdata.Ticket.ticketqueryplate;
                                    parametersOut["d"] = appealdata.Ticket.ticketdate;
                                    parametersOut["df"] = appealdata.Ticket.TickMaxpaydate;

                                }
                                else
                                {
                                    parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                                    parametersOut["q"] = appealdata.SearchedTicket.TickBalance;
                                    parametersOut["cur"] = sCurIsoCode;
                                    parametersOut["lp"] = appealdata.SearchedTicket.ticketqueryplate;
                                    parametersOut["SearchedTicket"] = appealdata.SearchedTicket;
                                    parametersOut["AllTickets"] = appealdata.AllTickets;
                                    parametersOut["d"] = appealdata.SearchedTicket.ticketdate;
                                    parametersOut["df"] = appealdata.SearchedTicket.TickMaxpaydate;
                                    try
                                    {
                                        //dt = DateTime.ParseExact(oData["maximumPayDate"].ToString(), "HHmmssddMMyyyy",
                                        //        CultureInfo.InvariantCulture);
                                        //parametersOut["df"] = dt.ToString("HHmmssddMMyy");
                                        parametersOut["df"] = appealdata.SearchedTicket.TickMaxpaydate;
                                    }
                                    catch
                                    {
                                    }
                                    
                                }

                            }
                            else
                            {

                                try
                                {
                                    parametersOut["lp"] = oData["plate"].ToString().Trim().Replace(" ", "");
                                    //DateTime dt = DateTime.ParseExact(oData["ticketdate"].ToString(), "HHmmssddMMyyyy",
                                                //CultureInfo.InvariantCulture);
                                   //parametersOut["d"] = dt.ToString("HHmmssddMMyy");
                                    parametersOut["d"] = oData["ticketdate"].ToString();
                                    try
                                    {
                                        //dt = DateTime.ParseExact(oData["maximumPayDate"].ToString(), "HHmmssddMMyyyy",
                                        //        CultureInfo.InvariantCulture);
                                        //parametersOut["df"] = dt.ToString("HHmmssddMMyy");
                                        parametersOut["df"] = oData["maximumPayDate"].ToString();
                                    }
                                    catch
                                    {
                                    }
                                    parametersOut["ta"] = oData["code"];
                                    parametersOut["dta"] = oData["description"];
                                }
                                catch
                                {

                                }
                            }
                            var strTicketNumber = "";
                            if(Ticketid != null)
                            {
                                strTicketNumber = appealdata.Ticket.ticketnumber;
                                if (strFineID != strTicketNumber)
                                {
                                    parametersOut["fnumber"] = strTicketNumber;
                                }

                                try
                                {
                                    //sCityIDOut = oData["cityid"].ToString();
                                    sCityIDOut = appealdata.Ticket.InstallationID.ToString();
                                }
                                catch { }
                            }
                            else
                            {
                                strTicketNumber = appealdata.ticketnumber;
                                if (strFineID != strTicketNumber)
                                {
                                    parametersOut["fnumber"] = strTicketNumber;
                                }

                                try
                                {
                                    //sCityIDOut = oData["cityid"].ToString();
                                    sCityIDOut = appealdata.SearchedTicket.InstallationID.ToString();
                                }
                                catch { }
                            }

                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, "StandardQueryFinePaymentQuantityMultiInstallations::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rtRes);
                }



            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "StandardQueryFinePaymentQuantityMultiInstallations::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;
        }



        #endregion



        public ResultType StandardConfirmFinePayment(string strFineNumber, DateTime dtOperationDate, int iQuantity, USER oUser, decimal dTicketPaymentID,
                                                     INSTALLATION oInstallation, int? iWSTimeout, 
                                                     ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;


            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                string strURL = oInstallation.INS_FINE_WS_URL + "/pay";
                WebRequest request = WebRequest.Create(strURL);
                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    request.Credentials = new System.Net.NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                }

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();


                string strFineID = strFineNumber;
                DateTime dtInstallation = dtOperationDate;
                string strCityID = oInstallation.INS_STANDARD_CITY_ID;
                string strProviderName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();


                Dictionary<string, object> ojsonInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oiparkticketInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oDataObjectDict = new Dictionary<string, object>();

                oDataObjectDict["cityid"] = strCityID;
                oDataObjectDict["ticketnumber"] = strFineID;
                oDataObjectDict["date"] = dtInstallation.ToString("HHmmssddMMyyyy");
                oDataObjectDict["amount"] = iQuantity;
                oDataObjectDict["provider"] = strProviderName;
                oDataObjectDict["op"] = dTicketPaymentID.ToString();
                oDataObjectDict["payinfo"] = oUser.USR_EMAIL;
                oDataObjectDict["ah"] = CalculateStandardWSHash(oInstallation.INS_FINE_WS_AUTH_HASH_KEY,
                                        string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}", strCityID, strFineID, dtInstallation, iQuantity, strProviderName, dTicketPaymentID.ToString(), oUser.USR_EMAIL)); ;
                oiparkticketInObjectDict["iparkticket_in"] = oDataObjectDict;

                ojsonInObjectDict["jsonIn"] = JsonConvert.SerializeObject(oiparkticketInObjectDict).ToString();
                var json = JsonConvert.SerializeObject(ojsonInObjectDict);

                Logger_AddLogMessage(string.Format("StandardConfirmFinePaymentQuantity request.url={0}, Timeout={2}, request.json={1}", strURL, json, request.Timeout), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.

                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.
                        lEllapsedTime = watch.ElapsedMilliseconds;
                        Logger_AddLogMessage(string.Format("StandardConfirmFinePaymentQuantity response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.

                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        var strResult = oResponse["Result"];

                        long lResult = Convert.ToInt32(strResult);

                        rtRes = Convert_ResultTypeStandardFineWS_TO_ResultType((ResultTypeStandardFineWS)lResult);

                        if (rtRes == ResultType.Result_OK)
                        {

                            dynamic oData = JsonConvert.DeserializeObject(oResponse["Data"].ToString()); ;
                            lResult = Convert.ToInt64(oData["res"]);
                            rtRes = Convert_ResultTypeStandardFineWS_TO_ResultType((ResultTypeStandardFineWS)lResult);

                            if (rtRes == ResultType.Result_OK)
                            {
                                parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                                if (oData["id"] != null)
                                {
                                    str3dPartyOpNum = oData["id"].ToString();
                                }

                            }
                            else
                            {
                                parametersOut["r"] = Convert.ToInt32(rtRes);
                                if (parametersOut.IndexOfKey("autorecharged") >= 0)
                                {
                                    parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                                }
                                if (parametersOut.IndexOfKey("newbal") >= 0)
                                {
                                    parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));
                                }
                            }
                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    if ((watch != null) && (lEllapsedTime == 0))
                    {
                        lEllapsedTime = watch.ElapsedMilliseconds;
                    }
                    Logger_AddLogException(e, "StandardConfirmFinePaymentQuantity::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rtRes);

                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "StandardConfirmFinePaymentQuantity::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;


        }

        public ResultType BilbaoIntegrationConfirmFinePayment(string strFineNumber, DateTime dtOperationDate,string strPlate, int iQuantity, USER oUser, decimal dTicketPaymentID,
                                                     INSTALLATION oInstallation, int? iWSTimeout,string ticketnumber,string ticketType,
                                                     ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;
            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);
                AddTLS12Support();
                var oParkWS = new BilbaoParkWsIntegraExternalService.integraExternalServices();
                oParkWS.Url = oInstallation.INS_FINE_WS_URL;
                oParkWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();
                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oParkWS.Credentials = new NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                }

                string strFineID = strFineNumber;
                DateTime dtInstallation = dtOperationDate;
                string strCityID = oInstallation.INS_STANDARD_CITY_ID;
                string strCompanyName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();
                string strvers = "1.0";
                var strAuthHash = CalculateStandardWSHash(oInstallation.INS_FINE_WS_AUTH_HASH_KEY,
                      string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}{7}{8}{9}", strCityID, strFineID, strPlate, dtInstallation, iQuantity, dTicketPaymentID.ToString(), "", strCompanyName, strvers, ticketnumber, ticketType));
                var strMessage = string.Format("<ipark_in><ins_id>{0}</ins_id><ticket_num>{1}</ticket_num><lic_pla>{2}</lic_pla><date>{2:HHmmssddMMyyyy}</date>" +
                                              "<amou_payed>{3}</amou_payed><oper_id>{4}</oper_id><ext_acc>{5}</ext_acc><prov>{6}</prov><ver>{7}</ver><ticket_num>{8}</ticket_num><ticket_type>{9}</ticket_type>" +
                                              "<ah>{10}</ah></ipark_in>",
                       strCityID, strFineID, strPlate, dtInstallation, iQuantity, dTicketPaymentID.ToString(), "", strCompanyName, strvers, ticketnumber, ticketType, strAuthHash);

                sXmlIn = PrettyXml(strMessage);
                Logger_AddLogMessage(string.Format("BilbaoIntegrationConfirmFinePayment Timeout={1} xmlIn ={0}", sXmlIn, oParkWS.Timeout), LogLevels.logDEBUG);
                string strOut = oParkWS.ExternalticketPaymentParkingmeter(strMessage);
                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");
                sXmlOut = PrettyXml(strOut);
                Logger_AddLogMessage(string.Format("BilbaoIntegrationConfirmFinePayment xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                try
                {
                    SortedList wsParameters = null;

                    rtRes = FindOutParameters(strOut, out wsParameters);

                    if (rtRes == ResultType.Result_OK)
                    {
                        rtRes = Convert_ResultTypeStandardParkingWS_TO_ResultType((ResultTypeStandardParkingWS)Convert.ToInt32(wsParameters["r"].ToString()));

                        if (rtRes == ResultType.Result_OK)
                            {
                                parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                                if (wsParameters["id"] != null)
                                {
                                    str3dPartyOpNum = wsParameters["id"].ToString();
                                }

                            }
                            else
                            {
                                parametersOut["r"] = Convert.ToInt32(rtRes);
                                if (parametersOut.IndexOfKey("autorecharged") >= 0)
                                {
                                    parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                                }
                                if (parametersOut.IndexOfKey("newbal") >= 0)
                                {
                                    parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));
                                }
                            }
                        }
                }
                catch (Exception e)
                {
                    if ((watch != null) && (lEllapsedTime == 0))
                    {
                        lEllapsedTime = watch.ElapsedMilliseconds;
                    }
                    Logger_AddLogException(e, "BilbaoIntegrationConfirmFinePayment::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rtRes);

                }
            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "BilbaoIntegrationConfirmFinePayment::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;
        }

        public ResultType StandardConfirmFinePaymentNonUser(string strFineNumber, DateTime dtOperationDate, int iQuantity, decimal dTicketPaymentID,
                                             INSTALLATION oInstallation, string Email, int? iWSTimeout, 
                                             ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;


            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                string strURL = oInstallation.INS_FINE_WS_URL + "/pay";
                WebRequest request = WebRequest.Create(strURL);
                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    request.Credentials = new System.Net.NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                }

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();


                string strFineID = strFineNumber;
                DateTime dtInstallation = dtOperationDate;
                string strCityID = oInstallation.INS_STANDARD_CITY_ID;
                string strProviderName = ConfigurationManager.AppSettings["STDCompanyName"].ToString();


                Dictionary<string, object> ojsonInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oiparkticketInObjectDict = new Dictionary<string, object>();
                Dictionary<string, object> oDataObjectDict = new Dictionary<string, object>();

                oDataObjectDict["cityid"] = strCityID;
                oDataObjectDict["ticketnumber"] = strFineID;
                oDataObjectDict["date"] = dtInstallation.ToString("HHmmssddMMyyyy");
                oDataObjectDict["amount"] = iQuantity;
                oDataObjectDict["provider"] = strProviderName;
                oDataObjectDict["op"] = dTicketPaymentID.ToString();
                oDataObjectDict["payinfo"] = Email;
                oDataObjectDict["ah"] = CalculateStandardWSHash(oInstallation.INS_FINE_WS_AUTH_HASH_KEY,
                                        string.Format("{0}{1}{2:HHmmssddMMyyyy}{3}{4}{5}{6}", strCityID, strFineID, dtInstallation, iQuantity, strProviderName, dTicketPaymentID.ToString(), Email));
                oiparkticketInObjectDict["iparkticket_in"] = oDataObjectDict;

                ojsonInObjectDict["jsonIn"] = JsonConvert.SerializeObject(oiparkticketInObjectDict).ToString();
                var json = JsonConvert.SerializeObject(ojsonInObjectDict);

                Logger_AddLogMessage(string.Format("StandardConfirmFinePaymentQuantity request.url={0}, Timeout={2}, request.json={1}", strURL, json, request.Timeout), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.

                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.
                        lEllapsedTime = watch.ElapsedMilliseconds;
                        Logger_AddLogMessage(string.Format("StandardConfirmFinePaymentQuantity response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.

                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        var strResult = oResponse["Result"];

                        long lResult = Convert.ToInt32(strResult);

                        rtRes = Convert_ResultTypeStandardFineWS_TO_ResultType((ResultTypeStandardFineWS)lResult);

                        if (rtRes == ResultType.Result_OK)
                        {

                            dynamic oData = JsonConvert.DeserializeObject(oResponse["Data"].ToString()); ;
                            lResult = Convert.ToInt64(oData["res"]);
                            rtRes = Convert_ResultTypeStandardFineWS_TO_ResultType((ResultTypeStandardFineWS)lResult);

                            if (rtRes == ResultType.Result_OK)
                            {
                                parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                                if (oData["id"] != null)
                                {
                                    str3dPartyOpNum = oData["id"].ToString();
                                }

                            }
                            else
                            {
                                parametersOut["r"] = Convert.ToInt32(rtRes);
                                if (parametersOut.IndexOfKey("autorecharged") >= 0)
                                {
                                    parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                                }
                                if (parametersOut.IndexOfKey("newbal") >= 0)
                                {
                                    parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));
                                }
                            }
                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    if ((watch != null) && (lEllapsedTime == 0))
                    {
                        lEllapsedTime = watch.ElapsedMilliseconds;
                    }
                    Logger_AddLogException(e, "StandardConfirmFinePaymentQuantity::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                    parametersOut["r"] = Convert.ToInt32(rtRes);

                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "StandardConfirmFinePaymentQuantity::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;


        }


        public ResultType SantBoiQueryFinePaymentQuantity(string strFineNumber, DateTime dtFineQuery, USER oUser, INSTALLATION oInstallation, ref SortedList parametersOut)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;
            string sXmlOut = "";


         
            Exception oNotificationEx = null;

            try
            {
                watch = Stopwatch.StartNew();
                string strTicketNumber="";
                DateTime dtTicketDate= DateTime.Now;
                string strTicketTypeExtID = "";
                bool bIsPayable=false;
                DateTime? dtMaxPayDate=null;
                int iAmount=0;
                string strDesc1="";
                string strDesc2="";


                string[] fineFields = strFineNumber.Split(new char[] { '#' });

                if (fineFields.Count() < 4)
                {
                    rtRes = ResultType.Result_Error_Fine_Number_Not_Found;
                }
                else
                {

                    strTicketNumber = fineFields[0];
                    strTicketTypeExtID = fineFields[3];
                    dtTicketDate = DateTime.ParseExact(fineFields[1] + " " + fineFields[2], "dd/MM/yyyy HH:mm",
                                                        CultureInfo.InvariantCulture);

                    parametersOut["d"] = dtTicketDate.ToString("HHmmssddMMyy");
                    parametersOut["fnumber"] = strTicketNumber;
                    parametersOut["lp"] = "";

                    if (geograficAndTariffsRepository.ExistTicketPayment(oInstallation.INS_ID, strTicketNumber))
                    {
                        rtRes = ResultType.Result_Error_Fine_Number_Already_Paid;
                    }
                    else
                    {

                        if (geograficAndTariffsRepository.getTicketTypePaymentInfo(oInstallation.INS_ID, dtFineQuery, dtTicketDate, strTicketTypeExtID,
                                                                                   out bIsPayable, out dtMaxPayDate, out iAmount, out strDesc1, out strDesc2))
                        {
                          
                            if (!bIsPayable)
                            {
                                rtRes = ResultType.Result_Error_Fine_Type_Not_Payable;
                            }
                            else
                            {
                                if (dtMaxPayDate.HasValue && dtFineQuery >= dtMaxPayDate.Value)
                                {
                                    rtRes = ResultType.Result_Error_Fine_Payment_Period_Expired;
                                }
                                else
                                {
                                    rtRes = ResultType.Result_OK;
                                    parametersOut["q"] = iAmount;
                                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;

                                    if (dtMaxPayDate.HasValue)
                                    {
                                        parametersOut["df"] = dtMaxPayDate.Value.ToString("HHmmssddMMyy");
                                    }
                                    parametersOut["ta"] = strDesc1;
                                    parametersOut["dta"] = strDesc2;
                                }
                            }

                        }
                        else
                        {
                            rtRes = ResultType.Result_Error_Generic;
                        }

                    }
                }

                parametersOut["r"] = Convert.ToInt32(rtRes);
                          
            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "SantBoiQueryFinePaymentQuantity::Exception", LogLevels.logERROR);
            }
            finally
            {
                watch.Stop();
                watch = null;

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strFineNumber, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;
        }



        public ResultType SantBoiConfirmFinePayment(string strFineNumber, DateTime dtOperationDate, USER oUser, INSTALLATION oInstallation, int? iWSTimeout,
                                                    ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;


            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                integraMobile.ExternalWS.SantBoiThirdPartyFineWS.server oFineWS = new integraMobile.ExternalWS.SantBoiThirdPartyFineWS.server();
                oFineWS.Url = oInstallation.INS_FINE_WS_URL;
                oFineWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                string strDate = dtOperationDate.ToString("yyyyMMddHHmmss");
                string strFineNumberCrypt = EncryptSantBoi(oInstallation.INS_FINE_WS_AUTH_HASH_KEY,oInstallation.INS_FINE_WS_HTTP_PASSWORD, strFineNumber);
                string strDateCrypt = EncryptSantBoi(oInstallation.INS_FINE_WS_AUTH_HASH_KEY,oInstallation.INS_FINE_WS_HTTP_PASSWORD, strDate);


                Logger_AddLogMessage(string.Format("SantBoiConfirmFinePayment strDate={0} ; strFineNumber={1} ; strDateCrypt={2} ; strFineNumberCrypt={3}; Timeout={4}", 
                    strDate, strFineNumber, strDateCrypt, strFineNumberCrypt, oFineWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oFineWS.NuevaAnulacion(strDateCrypt, strFineNumberCrypt);
                
                lEllapsedTime = watch.ElapsedMilliseconds;

                Logger_AddLogMessage(string.Format("SantBoiConfirmFinePayment strOut ={0}", strOut), LogLevels.logDEBUG);

                rtRes = Convert_ResultTypeSantBoiFineWS_TO_ResultType((ResultTypeSantBoiFineWS)Convert.ToInt32(strOut));

                Logger_AddLogMessage(string.Format("SantBoiConfirmFinePayment rt ={0}", rtRes), LogLevels.logDEBUG);

                if (rtRes == ResultType.Result_Error_Fine_Number_Already_Paid)
                {
                    rtRes = ResultType.Result_OK;
                }
                                         
                if (rtRes != ResultType.Result_OK)
                {

                    if (parametersOut.IndexOfKey("autorecharged") >= 0)
                    {
                        parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                    }
                    if (parametersOut.IndexOfKey("newbal") >= 0)
                    {
                        parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));
                    }              
                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "SantBoiConfirmFinePayment::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }


        public ResultType EysaQueryFinePaymentQuantity(string strFineNumber, DateTime dtFineQuery,string strCulturePrefix, USER oUser, INSTALLATION oInstallation, int? iWSTimeout, ref SortedList parametersOut)
        {

            ResultType rtRes = ResultType.Result_OK;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true); 

                integraMobile.ExternalWS.EysaThirdPartyFineWS.Anulaciones oFineWS = new integraMobile.ExternalWS.EysaThirdPartyFineWS.Anulaciones();
                oFineWS.Url = oInstallation.INS_FINE_WS_URL;
                oFineWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oFineWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                }

                integraMobile.ExternalWS.EysaThirdPartyFineWS.ConsolaSoapHeader authentication = new integraMobile.ExternalWS.EysaThirdPartyFineWS.ConsolaSoapHeader();
                authentication.IdContrata = Convert.ToInt32(oInstallation.INS_EYSA_CONTRATA_ID);
                authentication.IdUsuario = oUser.USR_ID.ToString();
                oFineWS.ConsolaSoapHeaderValue = authentication;


                string strFineID = strFineNumber;
                DateTime dtInstallation = dtFineQuery;
                string strvers = "1.0";
                string strCityID = oInstallation.INS_EYSA_CONTRATA_ID;

                string strAuthHash = CalculateEysaWSHash(oInstallation.INS_FINE_WS_AUTH_HASH_KEY,
                    string.Format("{0}{1}{2:yyyy-MM-ddTHH:mm:ss.fff}{3}", strFineID, strCityID, dtInstallation, strvers));
                string strMessage = string.Format("<ipark_in><f>{0}</f><city_id>{1}</city_id><d>{2:yyyy-MM-ddTHH:mm:ss.fff}</d><vers>{3}</vers><ah>{4}</ah></ipark_in>",
                    strFineID, strCityID, dtInstallation, strvers, strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("EysaQueryFinePaymentQuantity Timeout={1} xmlIn={0}", sXmlIn, oFineWS.Timeout), LogLevels.logDEBUG);

                string strOut = oFineWS.rdPQueryFinePaymentQuantity(strMessage);
                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("EysaQueryFinePaymentQuantity xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);

                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {

                    if (Convert.ToInt32(wsParameters["r"].ToString()) == (int)ResultType.Result_OK)
                    {
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                        parametersOut["q"] = wsParameters["q"];
                        parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;
                        parametersOut["lp"] = wsParameters["lp"].ToString().Trim().Replace(" ", "");

                        DateTime dt = DateTime.ParseExact(wsParameters["d"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                     CultureInfo.InvariantCulture);
                        parametersOut["d"] = dt.ToString("HHmmssddMMyy");
                        dt = DateTime.ParseExact(wsParameters["df"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                                             CultureInfo.InvariantCulture);
                        parametersOut["df"] = dt.ToString("HHmmssddMMyy");
                        parametersOut["ta"] = wsParameters["ta"];
                        if (wsParameters.ContainsKey("dta"))
                            parametersOut["dta"] = wsParameters["dta"];
                        else
                        {
                            if (wsParameters.ContainsKey("dta_lang_0_" + strCulturePrefix))
                                parametersOut["dta"] = wsParameters["dta_lang_0_" + strCulturePrefix];
                            else if (wsParameters.ContainsKey("dta_lang_0_es"))
                                parametersOut["dta"] = wsParameters["dta_lang_0_es"];
                            else
                                parametersOut["dta"] = "---------------";

                            if (wsParameters.ContainsKey("dta_sector"))
                                parametersOut["sector"] = wsParameters["dta_sector"];
                            if (wsParameters.ContainsKey("dta_user"))
                                parametersOut["enforcuser"] = wsParameters["dta_user"];
                        }
                        if (wsParameters.ContainsKey("lit"))
                            parametersOut["lit"] = wsParameters["lit"];

                    }
                    else
                    {
                        //denuncia ya remesada = denuncia encontrada pero el plazo de anulación ya ha pasado.
                        parametersOut["r"] = ((Convert.ToInt32(wsParameters["r"])) == -4) ?
                            Convert.ToInt32(ResultType.Result_Error_Fine_Payment_Period_Expired) : Convert.ToInt32(wsParameters["r"]);

                        rtRes = (ResultType)parametersOut["r"];

                        try
                        {
                            parametersOut["lp"] = wsParameters["lp"].ToString().Trim().Replace(" ", "");
                            DateTime dt = DateTime.ParseExact(wsParameters["d"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                         CultureInfo.InvariantCulture);
                            parametersOut["d"] = dt.ToString("HHmmssddMMyy");
                            dt = DateTime.ParseExact(wsParameters["df"].ToString(), "yyyy-MM-ddTHH:mm:ss.fff",
                                                                 CultureInfo.InvariantCulture);
                            parametersOut["df"] = dt.ToString("HHmmssddMMyy");
                            parametersOut["ta"] = wsParameters["ta"];
                            if (wsParameters.ContainsKey("dta"))
                                parametersOut["dta"] = wsParameters["dta"];
                            else
                                parametersOut["dta"] = wsParameters["dta_lang_0_es"];
                        }
                        catch
                        {

                        }

                    }

                    if (wsParameters.ContainsKey("fnumber"))
                        parametersOut["fnumber"] = Regex.Replace(wsParameters["fnumber"].ToString(), "[^0-9]", "");
                    else
                    {
                        // *** HB ***
                        //if (oInstallation.INS_ID == 28) parametersOut["fnumber"] = "111111-1";
                        // *** HB ***
                    }

                }



            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "EysaQueryFinePaymentQuantity::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }


        public ResultType EysaConfirmFinePayment(string strFineNumber, DateTime dtOperationDate, int iQuantity, USER oUser, INSTALLATION oInstallation,int? iWSTimeout,
                                                    ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;


            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                integraMobile.ExternalWS.EysaThirdPartyFineWS.Anulaciones oFineWS = new integraMobile.ExternalWS.EysaThirdPartyFineWS.Anulaciones();
                oFineWS.Url = oInstallation.INS_FINE_WS_URL;
                oFineWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oFineWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                }


                integraMobile.ExternalWS.EysaThirdPartyFineWS.ConsolaSoapHeader authentication = new integraMobile.ExternalWS.EysaThirdPartyFineWS.ConsolaSoapHeader();
                authentication.IdContrata = Convert.ToInt32(oInstallation.INS_EYSA_CONTRATA_ID);
                authentication.IdUsuario = oUser.USR_ID.ToString();
                oFineWS.ConsolaSoapHeaderValue = authentication;

                string strFineID = strFineNumber;
                string strvers = "1.0";
                string strCityID = oInstallation.INS_EYSA_CONTRATA_ID;

                string strAuthHash = CalculateEysaWSHash(oInstallation.INS_FINE_WS_AUTH_HASH_KEY,
                    string.Format("{0}{1}{2:yyyy-MM-ddTHH:mm:ss.fff}{3}{4}", strFineID, strCityID, dtOperationDate, iQuantity, strvers));

                string strMessage = string.Format("<ipark_in><f>{0}</f><city_id>{1}</city_id><d>{2:yyyy-MM-ddTHH:mm:ss.fff}</d><q>{3}</q><vers>{4}</vers><ah>{5}</ah></ipark_in>",
                    strFineID, strCityID, dtOperationDate, iQuantity, strvers, strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("EysaConfirmFinePayment Timeout={1} xmlIn ={0}", sXmlIn, oFineWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oFineWS.rdPConfirmFinePayment(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("EysaConfirmFinePayment xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);


                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {
                    if (Convert.ToInt32(wsParameters["r"].ToString()) > 0)
                    {
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                        if (wsParameters["opnum"] != null)
                        {
                            str3dPartyOpNum = wsParameters["opnum"].ToString();
                        }
                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        if (parametersOut.IndexOfKey("autorecharged") >= 0)
                        {
                            parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                        }
                        if (parametersOut.IndexOfKey("newbal") >= 0)
                        {
                            parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));
                        }

                    }
                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "EysaConfirmFinePayment::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }

        public ResultType EysaConfirmFinePaymentNonUser(string strFineNumber, DateTime dtOperationDate, int iQuantity, INSTALLATION oInstallation, int? iWSTimeout,
                                             ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;


            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                integraMobile.ExternalWS.EysaThirdPartyFineWS.Anulaciones oFineWS = new integraMobile.ExternalWS.EysaThirdPartyFineWS.Anulaciones();
                oFineWS.Url = oInstallation.INS_FINE_WS_URL;
                oFineWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oFineWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                }


                integraMobile.ExternalWS.EysaThirdPartyFineWS.ConsolaSoapHeader authentication = new integraMobile.ExternalWS.EysaThirdPartyFineWS.ConsolaSoapHeader();
                authentication.IdContrata = Convert.ToInt32(oInstallation.INS_EYSA_CONTRATA_ID);
                //authentication.IdUsuario = oUser.USR_ID.ToString();
                oFineWS.ConsolaSoapHeaderValue = authentication;

                string strFineID = strFineNumber;
                string strvers = "1.0";
                string strCityID = oInstallation.INS_EYSA_CONTRATA_ID;

                string strAuthHash = CalculateEysaWSHash(oInstallation.INS_FINE_WS_AUTH_HASH_KEY,
                    string.Format("{0}{1}{2:yyyy-MM-ddTHH:mm:ss.fff}{3}{4}", strFineID, strCityID, dtOperationDate, iQuantity, strvers));

                string strMessage = string.Format("<ipark_in><f>{0}</f><city_id>{1}</city_id><d>{2:yyyy-MM-ddTHH:mm:ss.fff}</d><q>{3}</q><vers>{4}</vers><ah>{5}</ah></ipark_in>",
                    strFineID, strCityID, dtOperationDate, iQuantity, strvers, strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("EysaConfirmFinePayment Timeout={1} xmlIn ={0}", sXmlIn, oFineWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                string strOut = oFineWS.rdPConfirmFinePayment(strMessage);
                lEllapsedTime = watch.ElapsedMilliseconds;

                strOut = strOut.Replace("\r\n  ", "");
                strOut = strOut.Replace("\r\n ", "");
                strOut = strOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strOut);

                Logger_AddLogMessage(string.Format("EysaConfirmFinePayment xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);


                SortedList wsParameters = null;

                rtRes = FindOutParameters(strOut, out wsParameters);

                if (rtRes == ResultType.Result_OK)
                {
                    if (Convert.ToInt32(wsParameters["r"].ToString()) > 0)
                    {
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                        if (wsParameters["opnum"] != null)
                        {
                            str3dPartyOpNum = wsParameters["opnum"].ToString();
                        }
                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        if (parametersOut.IndexOfKey("autorecharged") >= 0)
                        {
                            parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                        }
                        if (parametersOut.IndexOfKey("newbal") >= 0)
                        {
                            parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));
                        }

                    }
                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "EysaConfirmFinePayment::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }

        public bool EysaThirdPartyQueryListOfFines(USER oUser, INSTALLATION oInstallation, DateTime dtinstDateTime, int? iWSTimeout, ref SortedList parametersOut)
        {

            bool bRes = true;

            string sXmlIn = "";
            string sXmlOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true); 

                integraMobile.ExternalWS.EysaThirdPartyFineWS.Anulaciones oFineWS = new integraMobile.ExternalWS.EysaThirdPartyFineWS.Anulaciones();
                oFineWS.Url = oInstallation.INS_FINE_WS_URL;
                oFineWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oFineWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                }

                integraMobile.ExternalWS.EysaThirdPartyFineWS.ConsolaSoapHeader authentication = new integraMobile.ExternalWS.EysaThirdPartyFineWS.ConsolaSoapHeader();
                authentication.IdContrata = Convert.ToInt32(oInstallation.INS_EYSA_CONTRATA_ID);
                authentication.IdUsuario = oUser.USR_ID.ToString();
                oFineWS.ConsolaSoapHeaderValue = authentication;

                string strvers = "1.0";
                string strCityID = oInstallation.INS_EYSA_CONTRATA_ID;

                int iNumPlates = oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).Count();
                string strPlatesList = "";
                string strPlateListForHash = "";
                int iIndex = 0;



                var oUserPlates = oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).OrderBy(r => r.USRP_PLATE).ToArray();
                foreach (USER_PLATE oPlate in oUserPlates)
                {
                    iIndex++;
                    strPlatesList += string.Format("<lp{0}>{1}</lp{0}>", iIndex, oPlate.USRP_PLATE);
                    strPlateListForHash += oPlate.USRP_PLATE;
                    strPlatesList += string.Format("<st{0}>{1}</st{0}>", iIndex, "1");
                    strPlateListForHash += "1";
                }

                string strAuthHash = CalculateEysaWSHash(oInstallation.INS_FINE_WS_AUTH_HASH_KEY,
                    string.Format("{0}{1}{2}{3:yyyy-MM-ddTHH:mm:ss.fff}{4}", strCityID, iNumPlates, strPlateListForHash, dtinstDateTime, strvers));

                string strMessage = string.Format("<ipark_in><city_id>{0}</city_id><nlp>{1}</nlp>{2}<d>{3:yyyy-MM-ddTHH:mm:ss.fff}</d><vers>{4}</vers><ah>{5}</ah></ipark_in>",
                    strCityID, iNumPlates, strPlatesList, dtinstDateTime, strvers, strAuthHash);

                sXmlIn = PrettyXml(strMessage);

                Logger_AddLogMessage(string.Format("EysaThirdPartyQueryListOfFines Timeout={1} xmlIn ={0}", sXmlIn, oFineWS.Timeout), LogLevels.logDEBUG);

                string strWSOut = oFineWS.rdPQueryListOfFines(strMessage);
                strWSOut = strWSOut.Replace("\r\n  ", "");
                strWSOut = strWSOut.Replace("\r\n ", "");
                strWSOut = strWSOut.Replace("\r\n", "");

                sXmlOut = PrettyXml(strWSOut);

                Logger_AddLogMessage(string.Format("EysaThirdPartyQueryListOfFines xmlOut ={0}", sXmlOut), LogLevels.logDEBUG);
                string strXMLOut = "";
                bRes = FindEysaQueryListOfFinesOutParameters(oUserPlates, strWSOut, out strXMLOut);

                parametersOut["userMSG"] = strXMLOut;



            }
            catch (Exception e)
            {
                oNotificationEx = e;
                bRes = false;
                Logger_AddLogException(e, "EysaThirdPartyQueryListOfFines::Exception", LogLevels.logERROR);
            }

            ResultType rtRes = (bRes? ResultType.Result_OK: ResultType.Result_Error_Generic);
            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sXmlIn, sXmlOut, true, oNotificationEx);
            }
            catch
            {

            }

            return bRes;

        }

        private bool FindEysaQueryListOfFinesOutParameters(USER_PLATE[] oUserPlates, string xmlIn, out string strXMLOut)
        {
            bool bRes = true;
            strXMLOut = "";

            try
            {
                XmlDocument xmlInDoc = new XmlDocument();
                try
                {
                    System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        ((sender, certificate, chain, sslPolicyErrors) => true); 

                    xmlInDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + xmlIn);
                    int? iNumInputFines = null;
                    int iNumCountFines = 0;
                    string strCurrentPlate = null;

                    XmlNodeList Nodes = xmlInDoc.SelectNodes("//" + _xmlTagName + OUT_SUFIX + "/*");
                    foreach (XmlNode Node in Nodes)
                    {
                        if (Node.Name == "r")
                        {
                            iNumInputFines = Convert.ToInt32(Node.ChildNodes[0].InnerText.Trim());
                            if (iNumInputFines <= 0)
                                break;
                        }
                        else if (Node.Name.Substring(0, 2) == "lp")
                        {
                            strCurrentPlate = oUserPlates[Convert.ToInt32(Node.Name.Substring(2, Node.Name.Length - 2)) - 1].USRP_PLATE;

                            if (Node.HasChildNodes)
                            {
                                if (Node.ChildNodes[0].HasChildNodes)
                                {
                                    //for each fine fines
                                    foreach (XmlNode xmlFine in Node.ChildNodes)
                                    {
                                        string strXmlFine = string.Format("<usertick><lp>{0}</lp>", strCurrentPlate);
                                        bool bFineOK = true;
                                        int? iFineQuantity = null;

                                        foreach (XmlNode xmlFineData in xmlFine.ChildNodes)
                                        {
                                            /*if ((iFineQuantity.HasValue) &&
                                                (iFineQuantity.Value <= 0))
                                            {
                                                bFineOK = false;
                                                break;
                                            }*/

                                            string strData = xmlFineData.InnerText.Trim();

                                            switch (xmlFineData.Name)
                                            {
                                                case "f":
                                                    strXmlFine += string.Format("<f>{0}</f>", strData);
                                                    break;
                                                case "a":
                                                    iFineQuantity = Convert.ToInt32(strData);
                                                    strXmlFine += string.Format("<q>{0}</q>", strData);
                                                    break;
                                                case "d":
                                                    DateTime dt = DateTime.ParseExact(strData, "yyyy-MM-ddTHH:mm:ss.fff",
                                                                 CultureInfo.InvariantCulture);
                                                    strXmlFine += string.Format("<d>{0}</d>", dt.ToString("HHmmssddMMyy"));
                                                    break;
                                                case "df":
                                                    DateTime dtFinal = DateTime.ParseExact(strData, "yyyy-MM-ddTHH:mm:ss.fff",
                                                                 CultureInfo.InvariantCulture);
                                                    strXmlFine += string.Format("<df>{0}</df>", dtFinal.ToString("HHmmssddMMyy"));
                                                    break;
                                                case "ta":
                                                    strXmlFine += string.Format("<ta>{0}</ta>", strData);
                                                    break;
                                                case "dta":
                                                    strXmlFine += "";//string.Format("<dta>{0}</dta>", strData);
                                                    break;
                                                default:
                                                    break;
                                            }


                                        }


                                        if (bFineOK)
                                        {
                                            strXmlFine += "</usertick>";
                                            strXMLOut += strXmlFine;
                                            iNumCountFines++;
                                        }

                                    }
                                }
                            }
                        }
                    }

                    if (iNumCountFines > 0)
                    {
                        strXMLOut = "<userticks>" + strXMLOut + "</userticks>";
                    }
                    else
                    {
                        strXMLOut = "";

                    }


                    if (Nodes.Count == 0)
                    {
                        Logger_AddLogMessage(string.Format("FindEysaQueryListOfFinesOutParameters: Bad Input XML: xmlIn={0}", PrettyXml(xmlIn)), LogLevels.logERROR);
                        bRes = false;
                        strXMLOut = "";
                    }


                }
                catch (Exception e)
                {
                    Logger_AddLogException(e, string.Format("FindEysaQueryListOfFinesOutParameters: Bad Input XML: xmlIn={0}:Exception", PrettyXml(xmlIn)), LogLevels.logERROR);
                    bRes = false;
                    strXMLOut = "";
                }

            }
            catch (Exception e)
            {
                Logger_AddLogException(e, "FindEysaQueryListOfFinesOutParameters::Exception", LogLevels.logERROR);
                bRes = false;
                strXMLOut = "";

            }


            return bRes;
        }

        public ResultType ValorizaQueryFinePaymentQuantity(string strFineNumber, DateTime dtFineQuery, USER oUser, INSTALLATION oInstallation, int? iWSTimeout, ref SortedList parametersOut)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;

            string json = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                ValorizaWS.Service1 oFineWS = new ValorizaWS.Service1();

                oFineWS.Url = oInstallation.INS_FINE_WS_URL;
                oFineWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oFineWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                }
                                           
                DateTime dtInstallation = dtFineQuery;


                Logger_AddLogMessage(string.Format("ValorizaQueryFinePaymentQuantity SolicitudDenunciasPorExpediente({0},{1},{2},{3})", 
                    oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD, strFineNumber, oInstallation.INS_STANDARD_CITY_ID), LogLevels.logDEBUG);

                XmlNode xmlOut = oFineWS.SolicitudDenunciasPorExpediente(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD, strFineNumber, Convert.ToInt32(oInstallation.INS_STANDARD_CITY_ID)) ;

                XDocument xDocument = XDocument.Load(new XmlNodeReader(xmlOut));
                var builder = new StringBuilder();
                JsonSerializer.Create().Serialize(new CustomJsonWriter(new StringWriter(builder)), xDocument);
                json = builder.ToString();
                Logger_AddLogMessage(string.Format("ValorizaQueryFinePaymentQuantity Timeout={1} SolicitudDenunciasPorExpediente={0}",
                    json, oFineWS.Timeout), LogLevels.logDEBUG);
                dynamic oResponse = JsonConvert.DeserializeObject(json);


                var oRoot = oResponse["R"];
                var oVariablesData = oRoot["F"];
                var oVariablesFineData = oRoot["A"];

                string strVarName = "";
                string strVarValue = "";
                Dictionary<string, string> oData = new Dictionary<string, string>();


                if (oVariablesData != null)
                {
                    foreach (var oElement in oVariablesData)
                    {
                        try
                        {
                            strVarName= oElement["N"];
                            strVarValue = oElement["V"];
                            switch (strVarName)
                            {
                                case "resultado":
                                    oData["res"]=strVarValue;
                                    break;
                                case "descripcion":
                                    oData["resdesc"]=strVarValue;
                                    break;
                                case "expediente":
                                    oData["ticketnumber"] = strVarValue;
                                    break;
                                default:
                                    break;

                            }
                        }
                        catch (Exception e)
                        {
                            Logger_AddLogException(e, "ValorizaQueryFinePaymentQuantity::Exception", LogLevels.logERROR);
                        }

                    }
                }


                if (oVariablesFineData != null)
                {
                    strVarName = oVariablesFineData["N"];
                    if (strVarName == "Denuncia")
                    {
                        var oFineData = oVariablesFineData["B"];


                        foreach (var oElement in oFineData)
                        {
                            try
                            {
                                strVarName = oElement["N"];
                                strVarValue = oElement["V"];

                                switch (strVarName)
                                {
                                    case "precioAnulacion":
                                        oData["amount"] = strVarValue;
                                        break;
                                    case "matricula":
                                        oData["plate"] = strVarValue;
                                        break;
                                    case "fechaDenuncia":
                                        oData["ticketdate"] = strVarValue;
                                        break;
                                    case "fechaMaxAnulacion":
                                        oData["maximumPayDate"] = strVarValue;
                                        break;
                                    case "sancion":
                                        oData["code"] = strVarValue;
                                        break;
                                    case "infraccion":
                                        oData["description"] = strVarValue;
                                        break;
                                    case "anulada":
                                        oData["paid"] = strVarValue;
                                        break;
                                    case "idVSM":
                                        oData["id"] = strVarValue;
                                        break;
                                    case "idzona":
                                        oData["ExtGrpId"] = strVarValue;
                                        break;
                                    default:
                                        break;

                                }

                            }
                            catch (Exception e)
                            {
                                Logger_AddLogException(e, "ValorizaQueryFinePaymentQuantity::Exception", LogLevels.logERROR);
                            }

                        }

                    }

                }

                if (oData["res"] == "0")
                {

                    if (oData.ContainsKey("id"))
                    {

                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        parametersOut["q"] = oData["amount"];
                        parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;
                        parametersOut["lp"] = oData["plate"];
                        parametersOut["AuthId"] = oData["id"];

                        try
                        {
                            DateTime dt = DateTime.ParseExact(oData["ticketdate"].ToString(), "dd/MM/yyyy HH:mm:ss",
                                        CultureInfo.InvariantCulture);

                            parametersOut["d"] = dt.ToString("HHmmssddMMyy");
                        }
                        catch
                        {

                        }

                        DateTime? dtMaxPay = null;

                        try
                        {
                            dtMaxPay = DateTime.ParseExact(oData["maximumPayDate"].ToString(), "dd/MM/yyyy HH:mm:ss",
                                    CultureInfo.InvariantCulture);
                            parametersOut["df"] = dtMaxPay.Value.ToString("HHmmssddMMyy");

                        }
                        catch
                        {

                        }
                        parametersOut["ta"] = oData["code"];
                        parametersOut["dta"] = oData["description"];
                        if(oData.ContainsKey("ExtGrpId") && !string.IsNullOrEmpty(oData["ExtGrpId"]))
                        {
                            parametersOut["ExtGrpId"] = oData["ExtGrpId"];
                        }

                        if (oData["amount"] == "0")
                        {
                            rtRes = ResultType.Result_Error_Fine_Type_Not_Payable;
                        }
                        else if (oData["paid"] != "0")
                        {
                            rtRes = ResultType.Result_Error_Fine_Number_Already_Paid;
                        }
                        else
                        {

                            if (dtMaxPay.HasValue && dtInstallation > dtMaxPay)
                            {

                                rtRes = ResultType.Result_Error_Fine_Payment_Period_Expired;
                            }
                            else
                            {
                                rtRes = ResultType.Result_OK;
                            }

                        }
                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Fine_Type_Not_Payable;
                    }

                    parametersOut["r"] = Convert.ToInt32(rtRes);
                    
                }
                else
                {
                    rtRes = ResultType.Result_Error_Fine_Number_Not_Found;
                    parametersOut["r"] = Convert.ToInt32(rtRes);                   
                }
                

                var strTicketNumber = oData["ticketnumber"].ToString();
                if (strFineNumber != strTicketNumber)
                {
                    parametersOut["fnumber"] = strTicketNumber;
                }
                                    
            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "ValorizaQueryFinePaymentQuantity::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, string.Format("({0},{1},{2},{3})",
                    oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD, strFineNumber, oInstallation.INS_STANDARD_CITY_ID), json, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }


        public ResultType ValorizaConfirmFinePayment(string strIdVSM, USER oUser, INSTALLATION oInstallation, int? iWSTimeout,
                                            ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            str3dPartyOpNum = "";
            string json = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;


           
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                ValorizaWS.Service1 oFineWS = new ValorizaWS.Service1();

                oFineWS.Url = oInstallation.INS_FINE_WS_URL;
                oFineWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oFineWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                }



                Logger_AddLogMessage(string.Format("ValorizaConfirmFinePayment InsertarAnulacion({0},{1},{2},{3})",
                    oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD, oInstallation.INS_STANDARD_CITY_ID, strIdVSM), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                XmlNode xmlOut = oFineWS.InsertarAnulacion(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD, Convert.ToInt32(oInstallation.INS_STANDARD_CITY_ID), strIdVSM);
                lEllapsedTime = watch.ElapsedMilliseconds;

                XDocument xDocument = XDocument.Load(new XmlNodeReader(xmlOut));
                var builder = new StringBuilder();
                JsonSerializer.Create().Serialize(new CustomJsonWriter(new StringWriter(builder)), xDocument);
                json = builder.ToString();
                Logger_AddLogMessage(string.Format("ValorizaConfirmFinePayment Timeout={1} InsertarAnulacion={0}",
                    json, oFineWS.Timeout), LogLevels.logDEBUG);
                dynamic oResponse = JsonConvert.DeserializeObject(json);


                var oRoot = oResponse["R"];
                var oVariablesData = oRoot["F"];

                string strVarName = "";
                string strVarValue = "";
                Dictionary<string, string> oData = new Dictionary<string, string>();


                if (oVariablesData != null)
                {
                    foreach (var oElement in oVariablesData)
                    {
                        try
                        {
                            strVarName = oElement["N"];
                            strVarValue = oElement["V"];
                            switch (strVarName)
                            {
                                case "resultado":
                                    oData["res"] = strVarValue;
                                    break;
                                case "descripcion":
                                    oData["desc"] = strVarValue;
                                    break;                               
                                default:
                                    break;

                            }
                        }
                        catch (Exception e)
                        {
                            Logger_AddLogException(e, "ValorizaConfirmFinePayment::Exception", LogLevels.logERROR);
                        }

                    }
                }

                if (oData["res"] == "1")
                {
                    rtRes = ResultType.Result_OK;
                    str3dPartyOpNum = strIdVSM;
                    parametersOut["r"] = Convert.ToInt32(rtRes);
                }
                else
                {
                    if (oData["desc"].Contains("Denuncia ya anulada anteriormente"))
                    {
                        rtRes = ResultType.Result_OK;
                        str3dPartyOpNum = strIdVSM;
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        if (parametersOut.IndexOfKey("autorecharged") >= 0)
                        {
                            parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                        }
                        if (parametersOut.IndexOfKey("newbal") >= 0)
                        {
                            parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));
                        }
                    }

                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "ValorizaConfirmFinePayment::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, string.Format("({0},{1},{2},{3})",
                    oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD, oInstallation.INS_STANDARD_CITY_ID, strIdVSM), json, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }

        public ResultType ValorizaConfirmFinePaymentNonUser(string strIdVSM, DateTime dtOperationDate, int iQuantity, decimal dTicketPaymentID, 
                                            INSTALLATION oInstallation, int? iWSTimeout, ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_Error_Generic;
            str3dPartyOpNum = "";
            string json = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;

            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                ValorizaWS.Service1 oFineWS = new ValorizaWS.Service1();

                oFineWS.Url = oInstallation.INS_FINE_WS_URL;
                oFineWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oFineWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                }


                Logger_AddLogMessage(string.Format("ValorizaConfirmFinePaymentNonUser InsertarAnulacion({0},{1},{2},{3})",
                    oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD, oInstallation.INS_STANDARD_CITY_ID, strIdVSM), LogLevels.logDEBUG);

                //watch = Stopwatch.StartNew();
                XmlNode xmlOut = oFineWS.InsertarAnulacion(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD, Convert.ToInt32(oInstallation.INS_STANDARD_CITY_ID), strIdVSM);
                //lEllapsedTime = watch.ElapsedMilliseconds;

                XDocument xDocument = XDocument.Load(new XmlNodeReader(xmlOut));
                var builder = new StringBuilder();
                JsonSerializer.Create().Serialize(new CustomJsonWriter(new StringWriter(builder)), xDocument);
                json = builder.ToString();
                Logger_AddLogMessage(string.Format("ValorizaConfirmFinePaymentNonUser Timeout={1} InsertarAnulacion={0}",
                    json, oFineWS.Timeout), LogLevels.logDEBUG);
                dynamic oResponse = JsonConvert.DeserializeObject(json);

                var oRoot = oResponse["R"];
                var oVariablesData = oRoot["F"];

                string strVarName = "";
                string strVarValue = "";
                Dictionary<string, string> oData = new Dictionary<string, string>();


                if (oVariablesData != null)
                {
                    foreach (var oElement in oVariablesData)
                    {
                        try
                        {
                            strVarName = oElement["N"];
                            strVarValue = oElement["V"];
                            switch (strVarName)
                            {
                                case "resultado":
                                    oData["res"] = strVarValue;
                                    break;
                                case "descripcion":
                                    oData["desc"] = strVarValue;
                                    break;
                                default:
                                    break;

                            }
                        }
                        catch (Exception e)
                        {
                            Logger_AddLogException(e, "ValorizaConfirmFinePaymentNonUser::Exception", LogLevels.logERROR);
                        }
                    }
                }

                if (oData["res"] == "1")
                {
                    rtRes = ResultType.Result_OK;
                    str3dPartyOpNum = strIdVSM;
                    parametersOut["r"] = Convert.ToInt32(rtRes);
                }
                else
                {
                    if (oData["desc"].Contains("Denuncia ya anulada anteriormente"))
                    {
                        rtRes = ResultType.Result_OK;
                        str3dPartyOpNum = strIdVSM;
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        if (parametersOut.IndexOfKey("autorecharged") >= 0)
                        {
                            parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                        }
                        if (parametersOut.IndexOfKey("newbal") >= 0)
                        {
                            parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));
                        }
                    }

                }

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "ValorizaConfirmFinePaymentNonUser::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, string.Format("({0},{1},{2},{3})",
                    oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD, oInstallation.INS_STANDARD_CITY_ID, strIdVSM), json, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;



        }


        public bool GtechnaQueryListOfFines(USER oUser, INSTALLATION oInstallation, DateTime dtinstDateTime, int? iWSTimeout, ref SortedList parametersOut)
        {

            bool bRes = true;

            string sParamsIn = "";
            string sParamsOut = "";
            Exception oNotificationEx = null;            

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true); 

                integraMobile.ExternalWS.gTechnaThirdPartyFineWS.PayByPhoneOperationService oFineWS = new integraMobile.ExternalWS.gTechnaThirdPartyFineWS.PayByPhoneOperationService();
                integraMobile.ExternalWS.gTechnaThirdPartyFineWS.ticket_list_request request = new integraMobile.ExternalWS.gTechnaThirdPartyFineWS.ticket_list_request();
                oFineWS.Url = oInstallation.INS_FINE_WS_URL;
                oFineWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oFineWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                }

                int iNumPlates = oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).Count();
                request.plate_query = new integraMobile.ExternalWS.gTechnaThirdPartyFineWS.plate_query[iNumPlates];
                request.city_code = oInstallation.INS_GTECHNA_CITY_CODE;


                string strPlatesListHash = "";
                int iIndex = 0;

                var oUserPlates = oUser.USER_PLATEs.Where(r => r.USRP_ENABLED == 1).OrderBy(r => r.USRP_PLATE).ToArray();
                foreach (USER_PLATE oPlate in oUserPlates)
                {
                    request.plate_query[iIndex] = new integraMobile.ExternalWS.gTechnaThirdPartyFineWS.plate_query();
                    bool bPrefixFound = false;
                    /*foreach (string strProvince in CanadaAndUSAProvinces)
                    {
                        if (oPlate.USRP_PLATE.Substring(0, 2) == strProvince)
                        {
                            bPrefixFound = true;
                            break;
                        }
                    }*/

                    if (bPrefixFound)
                    {
                        request.plate_query[iIndex].plate = oPlate.USRP_PLATE.Substring(2, oPlate.USRP_PLATE.Length - 2);
                        request.plate_query[iIndex].state = oPlate.USRP_PLATE.Substring(0, 2);
                    }
                    else
                    {
                        request.plate_query[iIndex].plate = oPlate.USRP_PLATE;
                        request.plate_query[iIndex].state = "";
                    }

                    strPlatesListHash += request.plate_query[iIndex].plate;
                    strPlatesListHash += request.plate_query[iIndex].state;
                    iIndex++;
                }

                request.date = string.Format("{0:HHmmssddMMyy}", dtinstDateTime);

                string strAuthHash = CalculateGtechnaWSHash(oInstallation.INS_FINE_WS_AUTH_HASH_KEY,
                    string.Format("{0}{1:HHmmssddMMyy}{2}", strPlatesListHash, dtinstDateTime, oInstallation.INS_GTECHNA_CITY_CODE));

                request.ah = strAuthHash;

                sParamsIn = string.Format("GtechnaQueryListOfFines Timeout={1} request ={0}", request.ToString(), oFineWS.Timeout);

                Logger_AddLogMessage(sParamsIn, LogLevels.logDEBUG);

                integraMobile.ExternalWS.gTechnaThirdPartyFineWS.ticket_list_response response = oFineWS.QueryTicketList(request);

                sParamsOut = string.Format("GtechnaQueryListOfFines response ={0}", response.ToString());

                Logger_AddLogMessage(sParamsOut, LogLevels.logDEBUG);


                string strXMLOut = "";
                foreach (integraMobile.ExternalWS.gTechnaThirdPartyFineWS.plate_query plate_query in response.plate_query)
                {

                    if (plate_query.tickets != null)
                    {

                        foreach (integraMobile.ExternalWS.gTechnaThirdPartyFineWS.ticket ticket in plate_query.tickets)
                        {


                            if (ticket.payable)
                            {

                                if (strXMLOut.Length == 0)
                                {
                                    strXMLOut = "<userticks>";
                                }
                                strXMLOut += string.Format("<usertick><lp>{0}{1}</lp>", plate_query.state, plate_query.plate);
                                strXMLOut += string.Format("<f>{0}</f>", ticket.ticketno);
                                DateTime dt = DateTime.ParseExact(ticket.inf_date, "HHmmssddMMyy",
                                            CultureInfo.InvariantCulture);
                                strXMLOut += string.Format("<d>{0}</d>", dt.ToString("HHmmssddMMyy"));
                                if (ticket.exp_date != null)
                                {
                                    dt = DateTime.ParseExact(ticket.exp_date, "HHmmssddMMyy",
                                            CultureInfo.InvariantCulture);

                                }
                                else
                                {

                                    dt = dt.AddDays(1825);
                                }

                                strXMLOut += string.Format("<df>{0}</df>", dt.ToString("HHmmssddMMyy"));

                                strXMLOut += string.Format("<q>{0}</q>", ticket.fine);
                                strXMLOut += string.Format("<ta>{0}</ta>", ticket.article);
                                strXMLOut += string.Format("<dta>{0}</dta>", ticket.infraction);
                                strXMLOut += "</usertick>";

                            }

                        }
                    }
                }


                if (strXMLOut.Length > 0)
                {
                    strXMLOut += "</userticks>";
                }


                parametersOut["userMSG"] = strXMLOut;


            }
            catch (Exception e)
            {
                oNotificationEx = e;
                bRes = false;
                Logger_AddLogException(e, "GtechnaQueryListOfFines::Exception", LogLevels.logERROR);
            }

            ResultType rtRes = (bRes ? ResultType.Result_OK : ResultType.Result_Error_Generic);
            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sParamsIn, sParamsOut, false, oNotificationEx);
            }
            catch
            {
            }

            return bRes;

        }



        /*public ResultType GtechnaQueryFinePayment(string strFineNumber, DateTime dtFineQuery, INSTALLATION oInstallation,
                                                out int iQuantity, out string strPlate,
                                                out string strArticleType, out string strArticleDescription)
        {
            ResultType rtRes = ResultType.Result_OK;
            iQuantity = 0;
            strPlate = "";
            strArticleDescription = "";
            strArticleType = "";

            try
            {
                SortedList parametersIn = new SortedList();
                SortedList parametersOut = new SortedList();

                parametersIn["f"] = strFineNumber;

                rtRes = GtechnaQueryFinePaymentQuantity(parametersIn, strFineNumber, dtFineQuery, oInstallation, ref parametersOut);

                if (rtRes == ResultType.Result_OK)
                {
                    iQuantity = Convert.ToInt32(parametersOut["q"].ToString());
                    strPlate = parametersOut["lp"].ToString();
                    strArticleType = parametersOut["ta"].ToString();
                    strArticleDescription = parametersOut["dta"].ToString();
                }



            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "GtechnaQueryFinePaymentQuantity::Exception", LogLevels.logERROR);

            }


            return rtRes;


        }
        */
        /*
        public ResultType GtechnaQueryFinePaymentQuantity(SortedList parametersIn, string strFineNumber, DateTime dtFineQuery, INSTALLATION oInstallation, ref SortedList parametersOut)
        {

            ResultType rtRes = ResultType.Result_OK;

            string sParamsIn = "";
            string sParamsOut = "";
            Exception oNotificationEx = null;            

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true); 

                integraMobile.ExternalWS.gTechnaThirdPartyFineWS.PayByPhoneOperationService oFineWS = new integraMobile.ExternalWS.gTechnaThirdPartyFineWS.PayByPhoneOperationService();
                integraMobile.ExternalWS.gTechnaThirdPartyFineWS.ticket_status_request request = new integraMobile.ExternalWS.gTechnaThirdPartyFineWS.ticket_status_request();
                oFineWS.Url = oInstallation.INS_FINE_WS_URL;
                oFineWS.Timeout = Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oFineWS.Credentials = new System.Net.NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                }

                string strFineID = strFineNumber;
                DateTime dtInstallation = dtFineQuery;


                string strAuthHash = CalculateGtechnaWSHash(oInstallation.INS_FINE_WS_AUTH_HASH_KEY,
                    string.Format("{0}{1:HHmmssddMMyy}", strFineID, dtInstallation));


                request.ticketno = strFineNumber;
                request.date = string.Format("{0:HHmmssddMMyy}", dtInstallation);
                request.ah = strAuthHash;

                sParamsIn = string.Format("GtechnaQueryFinePaymentQuantity request ={0}", request.ToString());

                Logger_AddLogMessage(sParamsIn, LogLevels.logDEBUG);

                integraMobile.ExternalWS.gTechnaThirdPartyFineWS.ticket_status_response response = oFineWS.QueryTicketStatus(request);

                sParamsOut = string.Format("GtechnaQueryFinePaymentQuantity response ={0}", response.ToString());

                Logger_AddLogMessage(sParamsOut, LogLevels.logDEBUG);


                if (response.result_code > 0)
                {
                    parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                    parametersOut["q"] = response.result_code;
                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;
                    parametersOut["lp"] = response.state + response.plate;
                    parametersOut["d"] = DateTime.ParseExact(response.inf_date, "HHmmssddMMyy",
                                                             CultureInfo.InvariantCulture);

                    if (response.exp_date != null)
                    {
                        parametersOut["df"] = DateTime.ParseExact(response.exp_date, "HHmmssddMMyy",
                                                                CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        parametersOut["df"] = DateTime.ParseExact(response.inf_date, "HHmmssddMMyy",
                                                                  CultureInfo.InvariantCulture).AddDays(1825);
                    }

                    parametersOut["ta"] = response.article;
                    parametersOut["dta"] = response.infraction;
                    parametersOut["lit"] = "";


                }
                else
                {
                    rtRes = (ResultType)response.result_code;
                    parametersOut["r"] = response.result_code;
                    parametersOut["lp"] = "";
                    parametersOut["d"] = "";
                    parametersOut["df"] = "";
                    parametersOut["ta"] = "";
                    parametersOut["dta"] = "";
                }


            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "GtechnaQueryFinePaymentQuantity::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sParamsIn, sParamsOut, false, oNotificationEx);
            }
            catch
            { }

            return rtRes;

        }
        */

        public ResultType GtechnaQueryFinePaymentQuantity(string strFineNumber, DateTime dtFineQuery, INSTALLATION oInstallation, int? iWSTimeout, ref SortedList parametersOut)
        {

            ResultType rtRes = ResultType.Result_OK;

            string sParamsIn = "";
            string sParamsOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                GDLGTechnaAPI oFineWS = new GDLGTechnaAPI(oInstallation.INS_FINE_WS_URL);
                SortedList oTicket = null;
                oFineWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oFineWS.Username = oInstallation.INS_FINE_WS_HTTP_USER;
                    oFineWS.Password = oInstallation.INS_FINE_WS_HTTP_PASSWORD;
                }

                string strFineID = strFineNumber;
                string strCompanyName = ConfigurationManager.AppSettings["GtechnaCompanyName"].ToString();

                sParamsIn = string.Format("GtechnaQueryFinePaymentQuantity Timeout={1} TicketID = {0}", strFineID, oFineWS.Timeout);

                Logger_AddLogMessage(sParamsIn, LogLevels.logDEBUG);

                rtRes= oFineWS.GetOutstandingTickets(strFineID, "P", strCompanyName, oInstallation.INS_FINE_WS_AUTH_HASH_KEY, out oTicket);
                Logger_AddLogMessage(string.Format("GtechnaQueryFinePaymentQuantity ResultXML = {0}", oFineWS.ResultXML), LogLevels.logDEBUG);

                sParamsOut = string.Format("GtechnaQueryFinePaymentQuantity response = {0}\r\n",rtRes);
                if (oTicket != null)
                {
                    IDictionaryEnumerator ide = oTicket.GetEnumerator(); 
	
                    while (ide.MoveNext())
                    {
                        sParamsOut+= string.Format("\t\t{0} = {1}\r\n",ide.Key.ToString(),ide.Value.ToString());
                    }                  

                }

                Logger_AddLogMessage(sParamsOut, LogLevels.logDEBUG);


                if (rtRes == ResultType.Result_OK)
                {
                    parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                    parametersOut["q"] = Convert.ToInt32(Convert.ToDouble(oTicket["Balance"].ToString(),CultureInfo.InvariantCulture) * Math.Pow(10, Convert.ToDouble(oInstallation.CURRENCy.CUR_MINOR_UNIT)));

                    parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;
                    parametersOut["lp"] = oTicket["Plate"].ToString();
                    parametersOut["d"] = DateTime.ParseExact(oTicket["InfractionDate"].ToString(), "yyyy-MM-dd HH:mm:ss zzz",
                                                             CultureInfo.InvariantCulture).ToString("HHmmssddMMyy");

                    

                    parametersOut["ta"] = "";
                    parametersOut["dta"] = "";
                    parametersOut["lit"] = "";


                }
                else
                {
                    parametersOut["r"] = rtRes;
                    parametersOut["lp"] = "";
                    parametersOut["d"] = "";
                    parametersOut["df"] = "";
                    parametersOut["ta"] = "";
                    parametersOut["dta"] = "";
                }

            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "GtechnaQueryFinePaymentQuantity::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sParamsIn, sParamsOut, false, oNotificationEx);
            }
            catch
            { }

            return rtRes;

        }




        public ResultType GtechnaConfirmFinePayment(string strFineNumber, DateTime dtFineQuery, int iQuantity, decimal dTicketPaymentID, INSTALLATION oInstallation,int? iWSTimeout,
                                                        ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            Stopwatch watch = null;
            lEllapsedTime = 0;

            string sParamsIn = "";
            string sParamsOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                GDLGTechnaAPI oFineWS = new GDLGTechnaAPI(oInstallation.INS_FINE_WS_URL);
                SortedList oTransaction = null;
                oFineWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oFineWS.Username = oInstallation.INS_FINE_WS_HTTP_USER;
                    oFineWS.Password = oInstallation.INS_FINE_WS_HTTP_PASSWORD;
                }

                string strFineID = strFineNumber;
                string strCompanyName = ConfigurationManager.AppSettings["GtechnaCompanyName"].ToString();
                double dAmount = Math.Round(Convert.ToDouble(iQuantity) / Math.Pow(10, Convert.ToDouble(oInstallation.CURRENCy.CUR_MINOR_UNIT)), oInstallation.CURRENCy.CUR_MINOR_UNIT ?? 0);

                sParamsIn = string.Format("GtechnaConfirmFinePayment Timeout={1} TicketID = {0}\r\n", strFineID, oFineWS.Timeout);
                sParamsIn += string.Format("\t\tAmount = {0}\r\n", dAmount);
                sParamsIn += string.Format("\t\tDate = {0}\r\n", dtFineQuery);
                sParamsIn += string.Format("\t\tTicket Payment ID = {0}\r\n", dTicketPaymentID);

                Logger_AddLogMessage(sParamsIn, LogLevels.logDEBUG);


                rtRes = oFineWS.PayTicket(strFineID, dAmount, "WALLET", dtFineQuery, strCompanyName, dTicketPaymentID, oInstallation.INS_FINE_WS_AUTH_HASH_KEY, out oTransaction);
                Logger_AddLogMessage(string.Format("GtechnaConfirmFinePayment ResultXML = {0}", oFineWS.ResultXML), LogLevels.logDEBUG);

                sParamsOut = string.Format("GtechnaConfirmFinePayment response = {0}\r\n", rtRes);
                if (oTransaction != null)
                {
                    IDictionaryEnumerator ide = oTransaction.GetEnumerator();

                    while (ide.MoveNext())
                    {
                        sParamsOut += string.Format("\t\t{0} = {1}\r\n", ide.Key.ToString(), ide.Value.ToString());
                    }

                }

                Logger_AddLogMessage(sParamsOut, LogLevels.logDEBUG);


                if (rtRes == ResultType.Result_OK)
                {
                    parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                    str3dPartyOpNum = oTransaction["TransactionID"].ToString();
                }
                else
                {
                    parametersOut["r"] = Convert.ToInt32(rtRes);
                    if (parametersOut.IndexOfKey("autorecharged") >= 0)
                    {
                        parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                    }
                    if (parametersOut.IndexOfKey("newbal") >= 0)
                    {
                        parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));
                    }

                }


            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "GtechnaConfirmFinePayment::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sParamsIn, sParamsOut, false, oNotificationEx);
            }
            catch
            {
            }

            return rtRes;

        }

        public ResultType GtechnaConfirmFinePaymentNonUser(string strFineNumber, DateTime dtFineQuery, int iQuantity, decimal dTicketPaymentID, INSTALLATION oInstallation,int? iWSTimeout,
                                                ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            Stopwatch watch = null;
            lEllapsedTime = 0;

            string sParamsIn = "";
            string sParamsOut = "";
            Exception oNotificationEx = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);


                GDLGTechnaAPI oFineWS = new GDLGTechnaAPI(oInstallation.INS_FINE_WS_URL);
                SortedList oTransaction = null;
                oFineWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oFineWS.Username = oInstallation.INS_FINE_WS_HTTP_USER;
                    oFineWS.Password = oInstallation.INS_FINE_WS_HTTP_PASSWORD;
                }

                string strFineID = strFineNumber;
                string strCompanyName = ConfigurationManager.AppSettings["GtechnaCompanyName"].ToString();
                double dAmount = Math.Round(Convert.ToDouble(iQuantity) / Math.Pow(10, Convert.ToDouble(oInstallation.CURRENCy.CUR_MINOR_UNIT)), oInstallation.CURRENCy.CUR_MINOR_UNIT ?? 0);

                sParamsIn = string.Format("GtechnaConfirmFinePayment Timeout={1} TicketID = {0}\r\n", strFineID, oFineWS.Timeout);
                sParamsIn += string.Format("\t\tAmount = {0}\r\n", dAmount);
                sParamsIn += string.Format("\t\tDate = {0}\r\n", dtFineQuery);
                sParamsIn += string.Format("\t\tTicket Payment ID = {0}\r\n", dTicketPaymentID);

                Logger_AddLogMessage(sParamsIn, LogLevels.logDEBUG);


                rtRes = oFineWS.PayTicket(strFineID, dAmount, "WALLET", dtFineQuery, strCompanyName, dTicketPaymentID, oInstallation.INS_FINE_WS_AUTH_HASH_KEY, out oTransaction);
                Logger_AddLogMessage(string.Format("GtechnaConfirmFinePayment ResultXML = {0}", oFineWS.ResultXML), LogLevels.logDEBUG);

                sParamsOut = string.Format("GtechnaConfirmFinePayment response = {0}\r\n", rtRes);
                if (oTransaction != null)
                {
                    IDictionaryEnumerator ide = oTransaction.GetEnumerator();

                    while (ide.MoveNext())
                    {
                        sParamsOut += string.Format("\t\t{0} = {1}\r\n", ide.Key.ToString(), ide.Value.ToString());
                    }

                }

                Logger_AddLogMessage(sParamsOut, LogLevels.logDEBUG);


                if (rtRes == ResultType.Result_OK)
                {
                    parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                    str3dPartyOpNum = oTransaction["TransactionID"].ToString();
                }
                else
                {
                    parametersOut["r"] = Convert.ToInt32(rtRes);
                    if (parametersOut.IndexOfKey("autorecharged") >= 0)
                    {
                        parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                    }
                    if (parametersOut.IndexOfKey("newbal") >= 0)
                    {
                        parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));
                    }

                }


            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "GtechnaConfirmFinePayment::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sParamsIn, sParamsOut, false, oNotificationEx);
            }
            catch
            {
            }

            return rtRes;

        }

        /*public ResultType MadridPlatformQueryFinePaymentQuantity(string strFineNumber, DateTime dtFineQuery, USER oUser, INSTALLATION oInstallation, ref SortedList parametersOut)
        {

            ResultType rtRes = ResultType.Result_OK;

            string strParamsIn = "";
            string strParamsOut = "";
            Exception oNotificationEx = null;

            MadridPlatform.PublishServiceV12Client oService = null;
            MadridPlatform.AuthSession oAuthSession = null;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true); 

                oService = new MadridPlatform.PublishServiceV12Client();
                // oParkWS.Timeout = Get3rdPartyWSTimeout();

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        ((sender2, certificate, chain, sslPolicyErrors) => true);

                //string strHashKey = "";

                oService.Endpoint.Address = new System.ServiceModel.EndpointAddress(oInstallation.INS_FINE_WS_URL);
                //strHashKey = oGroup.INSTALLATION.INS_PARK_CONFIRM_WS_AUTH_HASH_KEY;
                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oService.ClientCredentials.UserName.UserName = oInstallation.INS_FINE_WS_HTTP_USER;
                    oService.ClientCredentials.UserName.Password = oInstallation.INS_FINE_WS_HTTP_PASSWORD;
                }


                oService.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, Get3rdPartyWSTimeout());

                if (strFineNumber.Length >= 10) strFineNumber = strFineNumber.Substring(0, strFineNumber.Length - 1);

                MadridPlatform.VigTrafficFineFilter oSelector = new MadridPlatform.VigTrafficFineFilter()
                {
                    ExpedientNumber = strFineNumber,                    
                    VigFilter = new MadridPlatform.EntityFilterCity()
                    {
                        CodSystem = oInstallation.INS_PHY_ZONE_COD_SYSTEM,
                        CodGeoZone = oInstallation.INS_PHY_ZONE_COD_GEO_ZONE,
                        CodCity = oInstallation.INS_PHY_ZONE_COD_CITY
                    }
                };

                MadridPlatform.EntityFilterCity oFilterCity = oSelector.VigFilter as MadridPlatform.EntityFilterCity;

                bool? bAuthRetry = null;
                while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                {
                    if (MadridPlatfomStartSession(oService, out oAuthSession))
                    {


                        strParamsIn = string.Format("sessionId={4};userName={5};" +
                                                   "ExpedientNumber={0};" +
                                                   "CodSystem={1};CodGeoZone={2};CodCity={3};",
                                                    oSelector.ExpedientNumber,
                                                    oFilterCity.CodSystem, oFilterCity.CodGeoZone, oFilterCity.CodCity,
                                                    oAuthSession.sessionId, oAuthSession.userName);
                        Logger_AddLogMessage(string.Format("MadridPlatformQueryFinePaymentQuantity.GetTfn parametersIn={0}", strParamsIn), LogLevels.logDEBUG);

                        var oResponse1 = oService.GetTfn(oAuthSession, oSelector);

                        string sAnullationCodes = "";
                        if (oResponse1.Result.Length > 0)
                        {
                            foreach (var sCode in oResponse1.Result[0].AnullationCodes)
                            {
                                sAnullationCodes += "," + sCode;
                            }
                            if (!string.IsNullOrEmpty(sAnullationCodes)) sAnullationCodes = sAnullationCodes.Substring(1);

                            strParamsOut = string.Format("Status={0};errorDetails={1};" +
                                                         "AnulStartDtUTC={2:yyyy-MM-ddTHH:mm:ss.fff};AnullationCodes={3};AnulledDtUTC={4};" +
                                                         "CodPhyZone={5};CreatedUTC={6:yyyy-MM-ddTHH:mm:ss.fff};ExpedientNumber={7};Id={8};" +
                                                         "Infraction.AllowCancel={9};Infraction.AllowCancelMinutes={10};Infraction.Article={11};Infraction.ByLaw={12};Infraction.CancelAmount={13};Infraction.Currency={14};" +
                                                         "InvalidatedDtUTC={15:yyyy-MM-ddTHH:mm:ss.fff};Invalidation={16};LastModificationUTC={17:yyyy-MM-ddTHH:mm:ss.fff};State={18};TicketNumber={19};" +
                                                         "Vehicle.LicensePlate={20};Xml={21}",
                                                         oResponse1.Status.ToString(), oResponse1.errorDetails,
                                                         oResponse1.Result[0].AnulStartDtUTC, sAnullationCodes, oResponse1.Result[0].AnulledDtUTC,
                                                         oResponse1.Result[0].CodPhyZone, oResponse1.Result[0].CreatedUTC, oResponse1.Result[0].ExpedientNumber, oResponse1.Result[0].Id,
                                                         oResponse1.Result[0].Infraction.AllowCancel, oResponse1.Result[0].Infraction.AllowCancelMinutes, oResponse1.Result[0].Infraction.Article, oResponse1.Result[0].Infraction.ByLaw, oResponse1.Result[0].Infraction.CancelAmount, oResponse1.Result[0].Infraction.Currency,
                                                         oResponse1.Result[0].InvalidatedDtUTC, oResponse1.Result[0].Invalidation, oResponse1.Result[0].LastModificationUTC, oResponse1.Result[0].State, oResponse1.Result[0].TicketNumber,
                                                         oResponse1.Result[0].Vehicle.LicensePlate, oResponse1.Result[0].Xml);
                        }
                        else
                        {
                            strParamsOut = string.Format("Status={0};errorDetails={1};" +
                                                         "Result.Length={2}",
                                                         oResponse1.Status.ToString(), oResponse1.errorDetails,
                                                         oResponse1.Result.Length);
                        }
                        Logger_AddLogMessage(string.Format("MadridPlatformQueryFinePaymentQuantity.GetTfn response={0}", strParamsOut), LogLevels.logDEBUG);

                        if (oResponse1.Status != MadridPlatform.PublisherResponse.PublisherStatus.OK &&
                            oResponse1.authError.HasValue && oResponse1.authError.Value == MadridPlatform.PublisherResponseAuthError.EXPIRED_SESSION)
                        {
                            MadridPlatfomEndSession();
                            bAuthRetry = true;
                        }
                        else
                        {
                            bAuthRetry = false;

                            if (oResponse1.Status == MadridPlatform.PublisherResponse.PublisherStatus.OK)
                            {
                                if (oResponse1.Result.Length > 0 && oResponse1.Result[0].AnullationCodes != null && oResponse1.Result[0].AnullationCodes.Length > 0)
                                {
                                    rtRes = ResultType.Result_OK;

                                    TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInstallation.INS_TIMEZONE_ID);
                                    DateTime dtFineQueryUTC = TimeZoneInfo.ConvertTime(dtFineQuery, tzi, TimeZoneInfo.Utc);

                                    var oRequest = new MadridPlatform.FineAnullationAuthRequest()
                                    {
                                        AnullationCode = oResponse1.Result[0].AnullationCodes[0],
                                        City = new MadridPlatform.EntityFilterCity()
                                        {
                                            CodSystem = oInstallation.INS_PHY_ZONE_COD_SYSTEM,
                                            CodGeoZone = oInstallation.INS_PHY_ZONE_COD_GEO_ZONE,
                                            CodCity = oInstallation.INS_PHY_ZONE_COD_CITY
                                        },
                                        DtRequest = dtFineQueryUTC,
                                        IsoLang = "es"
                                    };

                                    strParamsIn = string.Format("sessionId={5};userName={6};" +
                                                               "AnullationCode={7};" +
                                                               "CodSystem={0};CodGeoZone={1};CodCity={2};" +
                                                               "DtRequest={3:yyyy-MM-ddTHH:mm:ss.fff};" +
                                                               "IsoLang={4}",
                                                                oRequest.City.CodSystem, oRequest.City.CodGeoZone, oRequest.City.CodCity,
                                                                oRequest.DtRequest,
                                                                oRequest.IsoLang,
                                                                oAuthSession.sessionId, oAuthSession.userName,
                                                                oRequest.AnullationCode);

                                    Logger_AddLogMessage(string.Format("MadridPlatformQueryFinePaymentQuantity.GetFineAnullationAuthorization parametersIn={0}", strParamsIn), LogLevels.logDEBUG);

                                    var oResponse = oService.GetFineAnullationAuthorization(oAuthSession, oRequest);

                                    strParamsOut = string.Format("Status={0};errorDetails={1};AuthId={2};AuthResult={3};Expedient={4};TotAmo={5};eAuthResult={6}",
                                                                 oResponse.Status.ToString(), oResponse.errorDetails,
                                                                 oResponse.AuthId, oResponse.AuthResult, oResponse.Expedient, oResponse.TotAmo, oResponse.eAuthResult);
                                    Logger_AddLogMessage(string.Format("MadridPlatformQueryFinePaymentQuantity response={0}", strParamsOut), LogLevels.logDEBUG);

                                    if (oResponse.Status == MadridPlatform.PublisherResponse.PublisherStatus.OK)
                                    {
                                        rtRes = Convert_MadridPlatformAuthResult_TO_ResultType(oResponse.AuthResult);

                                        parametersOut["AuthId"] = oResponse.AuthId.ToString();
                                        parametersOut["ExtGrpId"] = oResponse1.Result[0].CodPhyZone;
                                        parametersOut["q"] = ((int)(oResponse.TotAmo * 100)).ToString();
                                        parametersOut["cur"] = oResponse1.Result[0].Infraction.Currency;
                                        parametersOut["exp"] = oResponse.Expedient;
                                        parametersOut["d"] = dtFineQuery.ToString("HHmmssddMMyy");
                                        if (oResponse1.Result[0].AnulStartDtUTC.HasValue)
                                        {
                                            DateTime? dtDf = geograficAndTariffsRepository.ConvertUTCToInstallationDateTime(oInstallation.INS_ID, oResponse1.Result[0].AnulStartDtUTC.Value.AddMinutes(oResponse1.Result[0].Infraction.AllowCancelMinutes ?? 0));
                                            if (dtDf.HasValue) parametersOut["df"] = dtDf.Value;
                                        }
                                        parametersOut["lp"] = oResponse1.Result[0].Vehicle.LicensePlate.Trim().Replace(" ", "");
                                        parametersOut["ta"] = oResponse1.Result[0].Infraction.Article;
                                        parametersOut["dta"] = oResponse1.Result[0].Infraction.ByLaw;
                                        parametersOut["lit"] = "";
                                    }
                                    else
                                    {
                                        rtRes = ResultType.Result_Error_Generic;
                                        parametersOut["d"] = dtFineQuery.ToString("HHmmssddMMyy");
                                        parametersOut["df"] = "";
                                        parametersOut["lp"] = "";
                                        parametersOut["ta"] = "";
                                        parametersOut["dta"] = "";
                                        parametersOut["lit"] = "";
                                    }
                                }
                                else
                                {
                                    rtRes = ResultType.Result_Error_Fine_Type_Not_Payable;
                                    parametersOut["d"] = dtFineQuery.ToString("HHmmssddMMyy");
                                    parametersOut["df"] = "";
                                    parametersOut["lp"] = "";
                                    parametersOut["ta"] = "";
                                    parametersOut["dta"] = "";
                                    parametersOut["lit"] = "";
                                    if (oResponse1.Result.Length > 0)
                                    {
                                        parametersOut["ExtGrpId"] = oResponse1.Result[0].CodPhyZone;
                                        if (oResponse1.Result[0].Infraction != null)
                                        {
                                            parametersOut["cur"] = oResponse1.Result[0].Infraction.Currency;
                                            if (oResponse1.Result[0].AnulStartDtUTC.HasValue)
                                            {
                                                DateTime? dtDf = geograficAndTariffsRepository.ConvertUTCToInstallationDateTime(oInstallation.INS_ID, oResponse1.Result[0].AnulStartDtUTC.Value.AddMinutes(oResponse1.Result[0].Infraction.AllowCancelMinutes ?? 0));
                                                if (dtDf.HasValue) parametersOut["df"] = dtDf.Value;
                                            }
                                            parametersOut["ta"] = oResponse1.Result[0].Infraction.Article;
                                            parametersOut["dta"] = oResponse1.Result[0].Infraction.ByLaw;
                                        }
                                        if (oResponse1.Result[0].Vehicle != null)
                                            parametersOut["lp"] = oResponse1.Result[0].Vehicle.LicensePlate.Trim().Replace(" ", "");
                                    }
                                }
                            }
                            else
                            {
                                if (oResponse1.Status == MadridPlatform.PublisherResponse.PublisherStatus.ZERO_RESULTS)
                                    rtRes = ResultType.Result_Error_Fine_Number_Not_Found;
                                else
                                    rtRes = ResultType.Result_Error_Generic;
                                parametersOut["d"] = dtFineQuery.ToString("HHmmssddMMyy"); // ***???
                                parametersOut["df"] = "";
                                parametersOut["lp"] = "";
                                parametersOut["ta"] = "";
                                parametersOut["dta"] = "";
                                parametersOut["lit"] = "";
                            }
                        }

                        //MadridPlatfomEndSession(oService, oAuthSession);

                    }
                    else
                        rtRes = ResultType.Result_Error_Generic;
                }

                parametersOut["r"] = Convert.ToInt32(rtRes);

            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);                            
                Logger_AddLogException(e, "MadridPlatformQueryFinePaymentQuantity::Exception", LogLevels.logERROR);
            }
            finally
            {
                //if (oService != null && oAuthSession != null)
                //{
                //    MadridPlatfomEndSession(oService, oAuthSession);
                //}
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strParamsIn, strParamsOut, true, oNotificationEx);
            }
            catch
            {
            }

            return rtRes;
        }*/
        public ResultType MadridPlatformQueryFinePaymentQuantity(string strFineNumber, string strPlate, DateTime dtFineQuery, USER oUser, INSTALLATION oInstallation, int? iWSTimeout, ref SortedList parametersOut)
        {

            ResultType rtRes = ResultType.Result_OK;

            string strParamsIn = "";
            string strParamsOut = "";
            Exception oNotificationEx = null;

            MadridPlatform.PublishServiceV12Client oService = null;
            MadridPlatform.AuthSession oAuthSession = null;
            int iWSLocalTimeout = iWSTimeout ?? Get3rdPartyWSTimeout();
            Stopwatch watch = null;


            try
            {

                AddTLS12Support();

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                oService = new MadridPlatform.PublishServiceV12Client();
                // oParkWS.Timeout = Get3rdPartyWSTimeout();

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        ((sender2, certificate, chain, sslPolicyErrors) => true);

                //string strHashKey = "";

                oService.Endpoint.Address = new System.ServiceModel.EndpointAddress(oInstallation.INS_FINE_WS_URL);
                //strHashKey = oGroup.INSTALLATION.INS_PARK_CONFIRM_WS_AUTH_HASH_KEY;
                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oService.ClientCredentials.UserName.UserName = oInstallation.INS_FINE_WS_HTTP_USER;
                    oService.ClientCredentials.UserName.Password = oInstallation.INS_FINE_WS_HTTP_PASSWORD;
                }


                if (!String.IsNullOrEmpty(strFineNumber) && strFineNumber.Length >= 10) strFineNumber = strFineNumber.Substring(0, strFineNumber.Length - 1);

                MadridPlatform.VigTrafficFineFilter oSelector = new MadridPlatform.VigTrafficFineFilter()
                {                    
                    VigFilter = new MadridPlatform.EntityFilterCity()
                    {
                        CodSystem = oInstallation.INS_PHY_ZONE_COD_SYSTEM,
                        CodGeoZone = oInstallation.INS_PHY_ZONE_COD_GEO_ZONE,
                        CodCity = oInstallation.INS_PHY_ZONE_COD_CITY
                    }
                };

                if (string.IsNullOrEmpty(strPlate))
                    oSelector.ExpedientNumber = strFineNumber;
                else
                    oSelector.LicensePlate = strPlate;

                MadridPlatform.EntityFilterCity oFilterCity = oSelector.VigFilter as MadridPlatform.EntityFilterCity;

                bool? bAuthRetry = null;
                long lEllapsedTime = 0;

                while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                {
                    long lEllapsedTimeLocal = 0;
                    if (MadridPlatfomStartSession(oService, out oAuthSession, iWSLocalTimeout, out lEllapsedTimeLocal))
                    {
                        iWSLocalTimeout -= (int)lEllapsedTimeLocal;
                        oService.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, iWSLocalTimeout);

                        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInstallation.INS_TIMEZONE_ID);
                        DateTime dtFineQueryUTC = TimeZoneInfo.ConvertTime(dtFineQuery, tzi, TimeZoneInfo.Utc);

                        var oRequest = new MadridPlatform.FineAnullationAuthRequest()
                        {                            
                            City = new MadridPlatform.EntityFilterCity()
                            {
                                CodSystem = oInstallation.INS_PHY_ZONE_COD_SYSTEM,
                                CodGeoZone = oInstallation.INS_PHY_ZONE_COD_GEO_ZONE,
                                CodCity = oInstallation.INS_PHY_ZONE_COD_CITY
                            },
                            DtRequest = dtFineQueryUTC,
                            IsoLang = "es"
                        };

                        if (string.IsNullOrEmpty(strPlate))
                            oRequest.ExpedientNumber = oSelector.ExpedientNumber;
                        else
                            oRequest.AnullationCode = oSelector.LicensePlate;

                        strParamsIn = string.Format("sessionId={5};userName={6};" +
                                                   "ExpedientNumber={7};AnullationCode={8}" +
                                                   "CodSystem={0};CodGeoZone={1};CodCity={2};" +
                                                   "DtRequest={3:yyyy-MM-ddTHH:mm:ss.fff};" +
                                                   "IsoLang={4}",
                                                    oRequest.City.CodSystem, oRequest.City.CodGeoZone, oRequest.City.CodCity,
                                                    oRequest.DtRequest,
                                                    oRequest.IsoLang,
                                                    oAuthSession.sessionId, oAuthSession.userName,
                                                    oRequest.ExpedientNumber, oRequest.AnullationCode);

                        Logger_AddLogMessage(string.Format("MadridPlatformQueryFinePaymentQuantity.GetFineAnullationAuthorization Timeout={1} parametersIn={0}", strParamsIn, oService.InnerChannel.OperationTimeout.TotalMilliseconds), LogLevels.logDEBUG);

                        watch = Stopwatch.StartNew();

                        var oResponse = oService.GetFineAnullationAuthorization(oAuthSession, oRequest);
                        lEllapsedTime = watch.ElapsedMilliseconds;

                        iWSLocalTimeout -= (int)lEllapsedTime;
                        oService.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, iWSLocalTimeout);


                        strParamsOut = string.Format("Status={0};errorDetails={1};AuthId={2};AuthResult={3};Expedient={4};TotAmo={5};eAuthResult={6}",
                                                     oResponse.Status.ToString(), oResponse.errorDetails,
                                                     oResponse.AuthId, oResponse.AuthResult, oResponse.Expedient, oResponse.TotAmo, oResponse.eAuthResult);
                        Logger_AddLogMessage(string.Format("MadridPlatformQueryFinePaymentQuantity response={0}", strParamsOut), LogLevels.logDEBUG);

                        if (oResponse.Status != MadridPlatform.PublisherResponse.PublisherStatus.OK &&
                            oResponse.authError.HasValue && oResponse.authError.Value == MadridPlatform.PublisherResponseAuthError.EXPIRED_SESSION)
                        {
                            MadridPlatfomEndSession();
                            bAuthRetry = true;
                        }
                        else
                        {
                            bAuthRetry = false;

                            if (oResponse.Status == MadridPlatform.PublisherResponse.PublisherStatus.OK)
                            {
                                rtRes = Convert_MadridPlatformAuthResult_TO_ResultType(oResponse.AuthResult);

                                if (rtRes == ResultType.Result_OK)
                                {
                                    strParamsIn = string.Format("sessionId={5};userName={6};" +
                                                   "ExpedientNumber={0};LicensePlate={1};" +
                                                   "CodSystem={2};CodGeoZone={3};CodCity={4};",
                                                    oSelector.ExpedientNumber, oSelector.LicensePlate,
                                                    oFilterCity.CodSystem, oFilterCity.CodGeoZone, oFilterCity.CodCity,
                                                    oAuthSession.sessionId, oAuthSession.userName);
                                    Logger_AddLogMessage(string.Format("MadridPlatformQueryFinePaymentQuantity.GetTfn Timeout={1} parametersIn={0}", strParamsIn, oService.InnerChannel.OperationTimeout.TotalMilliseconds), LogLevels.logDEBUG);

                                    var oResponse2 = oService.GetTfn(oAuthSession, oSelector);

                                    string sAnullationCodes = "";
                                    if (oResponse2.Result.Length > 0)
                                    {
                                        foreach (var sCode in oResponse2.Result[0].AnullationCodes)
                                        {
                                            sAnullationCodes += "," + sCode;
                                        }
                                        if (!string.IsNullOrEmpty(sAnullationCodes)) sAnullationCodes = sAnullationCodes.Substring(1);

                                        strParamsOut = string.Format("Status={0};errorDetails={1};" +
                                                                     "AnulStartDtUTC={2:yyyy-MM-ddTHH:mm:ss.fff};AnullationCodes={3};AnulledDtUTC={4};" +
                                                                     "CodPhyZone={5};CreatedUTC={6:yyyy-MM-ddTHH:mm:ss.fff};ExpedientNumber={7};Id={8};" +
                                                                     "Infraction.AllowCancel={9};Infraction.AllowCancelMinutes={10};Infraction.Article={11};Infraction.ByLaw={12};Infraction.CancelAmount={13};Infraction.Currency={14};" +
                                                                     "InvalidatedDtUTC={15:yyyy-MM-ddTHH:mm:ss.fff};Invalidation={16};LastModificationUTC={17:yyyy-MM-ddTHH:mm:ss.fff};State={18};TicketNumber={19};" +
                                                                     "Vehicle.LicensePlate={20};Xml={21}",
                                                                     oResponse2.Status.ToString(), oResponse2.errorDetails,
                                                                     oResponse2.Result[0].AnulStartDtUTC, sAnullationCodes, oResponse2.Result[0].AnulledDtUTC,
                                                                     oResponse2.Result[0].CodPhyZone, oResponse2.Result[0].CreatedUTC, oResponse2.Result[0].ExpedientNumber, oResponse2.Result[0].Id,
                                                                     oResponse2.Result[0].Infraction.AllowCancel, oResponse2.Result[0].Infraction.AllowCancelMinutes, oResponse2.Result[0].Infraction.Article, oResponse2.Result[0].Infraction.ByLaw, oResponse2.Result[0].Infraction.CancelAmount, oResponse2.Result[0].Infraction.Currency,
                                                                     oResponse2.Result[0].InvalidatedDtUTC, oResponse2.Result[0].Invalidation, oResponse2.Result[0].LastModificationUTC, oResponse2.Result[0].State, oResponse2.Result[0].TicketNumber,
                                                                     oResponse2.Result[0].Vehicle.LicensePlate, oResponse2.Result[0].Xml);
                                    }
                                    else
                                    {
                                        strParamsOut = string.Format("Status={0};errorDetails={1};" +
                                                                     "Result.Length={2}",
                                                                     oResponse2.Status.ToString(), oResponse2.errorDetails,
                                                                     oResponse2.Result.Length);
                                    }
                                    Logger_AddLogMessage(string.Format("MadridPlatformQueryFinePaymentQuantity.GetTfn response={0}", strParamsOut), LogLevels.logDEBUG);

                                    if (oResponse2.Result.Length > 0 && oResponse2.Result[0].AnullationCodes != null && oResponse2.Result[0].AnullationCodes.Length > 0)
                                    {
                                        parametersOut["AuthId"] = oResponse.AuthId.ToString();
                                        parametersOut["ExtGrpId"] = oResponse2.Result[0].CodPhyZone;
                                        parametersOut["q"] = ((int)(oResponse.TotAmo * 100)).ToString();
                                        parametersOut["cur"] = oResponse2.Result[0].Infraction.Currency;
                                        parametersOut["fnumber"] = oResponse2.Result[0].ExpedientNumber;
                                        parametersOut["d"] = dtFineQuery.ToString("HHmmssddMMyy");
                                        if (oResponse2.Result[0].AnulStartDtUTC.HasValue)
                                        {
                                            DateTime? dtDf = geograficAndTariffsRepository.ConvertUTCToInstallationDateTime(oInstallation.INS_ID, oResponse2.Result[0].AnulStartDtUTC.Value.AddMinutes(oResponse2.Result[0].Infraction.AllowCancelMinutes ?? 0));
                                            if (dtDf.HasValue) parametersOut["df"] = dtDf.Value.ToString("HHmmssddMMyy");
                                        }
                                        parametersOut["lp"] = oResponse2.Result[0].Vehicle.LicensePlate.Trim().Replace(" ", "");
                                        parametersOut["ta"] = oResponse2.Result[0].Infraction.Article;
                                        parametersOut["dta"] = oResponse2.Result[0].Infraction.ByLaw;
                                        parametersOut["lit"] = "";
                                        
                                    }
                                    else
                                    {
                                        rtRes = ResultType.Result_Error_Generic;
                                    }

                                }
                                else
                                {
                                    parametersOut["d"] = dtFineQuery.ToString("HHmmssddMMyy");
                                    parametersOut["df"] = "";
                                    parametersOut["lp"] = "";
                                    parametersOut["ta"] = "";
                                    parametersOut["dta"] = "";
                                    parametersOut["lit"] = "";
                                }

                            }
                            else
                            {
                                rtRes = ResultType.Result_Error_Generic;
                                parametersOut["d"] = dtFineQuery.ToString("HHmmssddMMyy");
                                parametersOut["df"] = "";
                                parametersOut["lp"] = "";
                                parametersOut["ta"] = "";
                                parametersOut["dta"] = "";
                                parametersOut["lit"] = "";
                            }

                        }

                    }
                    else
                        rtRes = ResultType.Result_Error_Generic;
                }

                parametersOut["r"] = Convert.ToInt32(rtRes);

            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "MadridPlatformQueryFinePaymentQuantity::Exception", LogLevels.logERROR);
            }
            finally
            {
                //if (oService != null && oAuthSession != null)
                //{
                //    MadridPlatfomEndSession(oService, oAuthSession);
                //}
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strParamsIn, strParamsOut, true, oNotificationEx);
            }
            catch
            {
            }

            return rtRes;
        }

        public ResultType MadridPlatformConfirmFinePayment(string strFineNumber, DateTime dtFineQuery, DateTime dtUTCInsertionDate, int iQuantity, USER oUser, INSTALLATION oInstallation, 
                                                           decimal dTicketId, decimal dAuthId, GROUP oGroup,int? iWSTimeout,
                                                           ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;


            string strParamsIn = "";
            string strParamsOut = "";
            Exception oNotificationEx = null;

            MadridPlatform.PublishServiceV12Client oService = null;
            MadridPlatform.AuthSession oAuthSession = null;
            int iWSLocalTimeout = iWSTimeout ?? Get3rdPartyWSTimeout();


            try
            {
                AddTLS12Support();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                oService = new MadridPlatform.PublishServiceV12Client();
                // oParkWS.Timeout = Get3rdPartyWSTimeout();

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        ((sender2, certificate, chain, sslPolicyErrors) => true);

                oService.Endpoint.Address = new System.ServiceModel.EndpointAddress(oInstallation.INS_FINE_WS_URL);
                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oService.ClientCredentials.UserName.UserName = oInstallation.INS_FINE_WS_HTTP_USER;
                    oService.ClientCredentials.UserName.Password = oInstallation.INS_FINE_WS_HTTP_PASSWORD;
                }

                string sExtGrpId = "000";
                if (oGroup != null)
                {
                    sExtGrpId = oGroup.GRP_ID_FOR_EXT_OPS;
                }

                

                if (strFineNumber.Length >= 10) strFineNumber = strFineNumber.Substring(0, strFineNumber.Length - 1);

                DateTime? dtUTCFineQuery = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtFineQuery);

                DateTime dtNowUtc = DateTime.UtcNow;                
                DateTime dtNow = geograficAndTariffsRepository.ConvertUTCToInstallationDateTime(oInstallation.INS_ID, dtNowUtc).Value;

                var oRequest = new MadridPlatform.PayFineCancellationTransactionRequest()
                {
                    City = new MadridPlatform.EntityFilterCity()
                    {
                        CodSystem = oInstallation.INS_PHY_ZONE_COD_SYSTEM,
                        CodGeoZone = oInstallation.INS_PHY_ZONE_COD_GEO_ZONE,
                        CodCity = oInstallation.INS_PHY_ZONE_COD_CITY
                    },
                    FineTrans = new MadridPlatform.PayTransactionFineCancellation()
                    {
                        AuthId = Convert.ToInt64(dAuthId),
                        //OperationDateUTC = dtUTCFineQuery.Value,
                        OperationDateUTC = dtNowUtc, // dtUTCInsertionDate,
                        TariffId = 2,
                        TicketNum = string.Format("98{0}00000{1}{2}", sExtGrpId, dtNow.DayOfYear.ToString("000"), dtNow.ToString("HHmm")),
                        TransId = Convert.ToInt64(dTicketId),
                        FineAmount = iQuantity / 100,
                        FineExpedientNumber = strFineNumber
                    }
                };

                bool? bAuthRetry = null;
                while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                {

                    long lEllapsedTimeLocal = 0;
                    if (MadridPlatfomStartSession(oService, out oAuthSession, iWSLocalTimeout, out lEllapsedTimeLocal))
                    {
                        strParamsIn = string.Format("sessionId={10};userName={11};" +
                                                   "CodSystem={0};CodGeoZone={1};CodCity={2};" +
                                                   "AuthId={3};OperationDateUTC={4:yyyy-MM-ddTHH:mm:ss.fff};TariffId={5};TicketNum={6};TransId={7};FineAmount={8};FineExpedientNumber={9}",
                                                    oRequest.City.CodSystem, oRequest.City.CodGeoZone, oRequest.City.CodCity,
                                                    oRequest.FineTrans.AuthId, oRequest.FineTrans.OperationDateUTC, oRequest.FineTrans.TariffId, oRequest.FineTrans.TicketNum, oRequest.FineTrans.TransId,
                                                    oRequest.FineTrans.FineAmount, oRequest.FineTrans.FineExpedientNumber,
                                                    oAuthSession.sessionId, oAuthSession.userName);

                        iWSLocalTimeout -= (int)lEllapsedTimeLocal;
                        oService.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, iWSLocalTimeout);

                        Logger_AddLogMessage(string.Format("MadridPlatformConfirmFinePayment Timeout={1} parametersIn={0}", strParamsIn, oService.InnerChannel.OperationTimeout.TotalMilliseconds), LogLevels.logDEBUG);

                        watch = Stopwatch.StartNew();

                        var oResponse = oService.SetFineAnullationTransaction(oAuthSession, oRequest);

                        lEllapsedTime = watch.ElapsedMilliseconds;

                        iWSLocalTimeout -= (int)lEllapsedTime;

                        strParamsOut = string.Format("Status={0};errorDetails={1}",
                                                     oResponse.Status.ToString(), oResponse.errorDetails);
                        Logger_AddLogMessage(string.Format("MadridPlatformConfirmFinePayment response={0}", strParamsOut), LogLevels.logDEBUG);

                        if (oResponse.Status != MadridPlatform.PublisherResponse.PublisherStatus.OK &&
                            oResponse.authError.HasValue && oResponse.authError.Value == MadridPlatform.PublisherResponseAuthError.EXPIRED_SESSION)
                        {
                            MadridPlatfomEndSession();
                            bAuthRetry = true;
                        }
                        else
                        {
                            bAuthRetry = false;
                            rtRes = (oResponse.Status == MadridPlatform.PublisherResponse.PublisherStatus.OK ? ResultType.Result_OK : ResultType.Result_Error_Generic);
                        }

                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        if (parametersOut.IndexOfKey("autorecharged") >= 0)
                        {
                            parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                        }
                        if (parametersOut.IndexOfKey("newbal") >= 0)
                        {
                            parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));
                        }
                    }
                }

                parametersOut["r"] = Convert.ToInt32(rtRes);

                if (rtRes == ResultType.Result_OK)
                {
                    str3dPartyOpNum = dAuthId.ToString();
                }


            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "MadridPlatformConfirmFinePayment::Exception", LogLevels.logERROR);
            }
            finally
            {
                //if (oService != null && oAuthSession != null)
                //{
                //    MadridPlatfomEndSession(oService, oAuthSession);
                //}
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strParamsIn, strParamsOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }

        public ResultType MadridPlatformConfirmFinePaymentNonUser(string strFineNumber, DateTime dtFineQuery, DateTime dtUTCInsertionDate, int iQuantity, INSTALLATION oInstallation, decimal dTicketId, decimal dAuthId,
                                                                  int? iWSTimeout, ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {

            ResultType rtRes = ResultType.Result_OK;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;
            Stopwatch watch = null;


            string strParamsIn = "";
            string strParamsOut = "";
            Exception oNotificationEx = null;

            MadridPlatform.PublishServiceV12Client oService = null;
            MadridPlatform.AuthSession oAuthSession = null;
            int iWSLocalTimeout = iWSTimeout ?? Get3rdPartyWSTimeout();


            try
            {
                AddTLS12Support();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                oService = new MadridPlatform.PublishServiceV12Client();
                // oParkWS.Timeout = Get3rdPartyWSTimeout();

                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                        ((sender2, certificate, chain, sslPolicyErrors) => true);

                oService.Endpoint.Address = new System.ServiceModel.EndpointAddress(oInstallation.INS_FINE_WS_URL);
                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    oService.ClientCredentials.UserName.UserName = oInstallation.INS_FINE_WS_HTTP_USER;
                    oService.ClientCredentials.UserName.Password = oInstallation.INS_FINE_WS_HTTP_PASSWORD;
                }

                string sExtGrpId = "000";
                /*if (oGroup != null)
                {
                    sExtGrpId = oGroup.GRP_ID_FOR_EXT_OPS;
                }*/

                long lEllapsedTimeLocal = 0;
                if (MadridPlatfomStartSession(oService, out oAuthSession, iWSLocalTimeout, out lEllapsedTimeLocal))
                {
                    if (strFineNumber.Length >= 10) strFineNumber = strFineNumber.Substring(0, strFineNumber.Length - 1);

                    iWSLocalTimeout -= (int)lEllapsedTimeLocal;

                    DateTime? dtUTCFineQuery = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtFineQuery);

                    var oRequest = new MadridPlatform.PayFineCancellationTransactionRequest()
                    {
                        City = new MadridPlatform.EntityFilterCity()
                        {
                            CodSystem = oInstallation.INS_PHY_ZONE_COD_SYSTEM,
                            CodGeoZone = oInstallation.INS_PHY_ZONE_COD_GEO_ZONE,
                            CodCity = oInstallation.INS_PHY_ZONE_COD_CITY
                        },
                        FineTrans = new MadridPlatform.PayTransactionFineCancellation()
                        {
                            AuthId = Convert.ToInt64(dAuthId),
                            //OperationDateUTC = dtUTCFineQuery.Value,
                            OperationDateUTC = dtUTCInsertionDate,
                            TariffId = 2,
                            TicketNum = string.Format("91{0}00000{1}{2}", sExtGrpId, dtFineQuery.DayOfYear.ToString("000"), dtFineQuery.ToString("HHmm")),
                            TransId = Convert.ToInt64(dTicketId),
                            FineAmount = iQuantity / 100,
                            FineExpedientNumber = strFineNumber
                        }
                    };

                    oService.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 0, 0, iWSLocalTimeout);

                    strParamsIn = string.Format("sessionId={10};userName={11};" +
                                               "CodSystem={0};CodGeoZone={1};CodCity={2};" +
                                               "AuthId={3};OperationDateUTC={4:yyyy-MM-ddTHH:mm:ss.fff};TariffId={5};TicketNum={6};TransId={7};FineAmount={8};FineExpedientNumber={9}",
                                                oRequest.City.CodSystem, oRequest.City.CodGeoZone, oRequest.City.CodCity,
                                                oRequest.FineTrans.AuthId, oRequest.FineTrans.OperationDateUTC, oRequest.FineTrans.TariffId, oRequest.FineTrans.TicketNum, oRequest.FineTrans.TransId,
                                                oRequest.FineTrans.FineAmount, oRequest.FineTrans.FineExpedientNumber,
                                                oAuthSession.sessionId, oAuthSession.userName);

                    Logger_AddLogMessage(string.Format("MadridPlatformConfirmFinePayment Timeout={1} parametersIn={0}", strParamsIn, oService.InnerChannel.OperationTimeout.TotalMilliseconds), LogLevels.logDEBUG);

                    watch = Stopwatch.StartNew();

                    var oResponse = oService.SetFineAnullationTransaction(oAuthSession, oRequest);

                    lEllapsedTime = watch.ElapsedMilliseconds;
                    iWSLocalTimeout -= (int)lEllapsedTime;

                    strParamsOut = string.Format("Status={0};errorDetails={1}",
                                                 oResponse.Status.ToString(), oResponse.errorDetails);
                    Logger_AddLogMessage(string.Format("MadridPlatformConfirmFinePayment response={0}", strParamsOut), LogLevels.logDEBUG);

                    if (oResponse.Status == MadridPlatform.PublisherResponse.PublisherStatus.OK)
                    {
                        rtRes = ResultType.Result_OK;

                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Generic;
                    }

                    //MadridPlatfomEndSession(oService, oAuthSession);

                }
                else
                {
                    rtRes = ResultType.Result_Error_Generic;
                    if (parametersOut.IndexOfKey("autorecharged") >= 0)
                    {
                        parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                    }
                    if (parametersOut.IndexOfKey("newbal") >= 0)
                    {
                        parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));
                    }
                }

                parametersOut["r"] = Convert.ToInt32(rtRes);

                if (rtRes == ResultType.Result_OK)
                {
                    str3dPartyOpNum = dAuthId.ToString();
                }


            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "MadridPlatformConfirmFinePayment::Exception", LogLevels.logERROR);
            }
            finally
            {
                if (oService != null && oAuthSession != null)
                {
                    MadridPlatfomEndSession(oService, oAuthSession, iWSLocalTimeout);
                }
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strParamsIn, strParamsOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }

        public ResultType Madrid2PlatformQueryFinePaymentQuantity(string strFineNumber, string strPlate, DateTime dtFineQuery, USER oUser, INSTALLATION oInstallation, int? iWSTimeout, ref SortedList parametersOut)
        {

            ResultType rtRes = ResultType.Result_OK;

            string strParamsIn = "";
            string strParamsOut = "";
            Exception oNotificationEx = null;

            MadridFineErrorResponse oErrorResponse = null;

            int iWSLocalTimeout = iWSTimeout ?? Get3rdPartyWSTimeout();
            Stopwatch watch = null;


            try
            {

                string sBaseUrl = "";
                string sClientId = "";
                string sClientSecret = "";
                string sTokenUrl = "";

                sBaseUrl = oInstallation.INS_FINE_WS_URL;
                sTokenUrl = oInstallation.INS_FINE_WS_AUTH_HASH_KEY;
                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    sClientId = oInstallation.INS_FINE_WS_HTTP_USER;
                    sClientSecret = oInstallation.INS_FINE_WS_HTTP_PASSWORD;
                }

                if (!String.IsNullOrEmpty(strFineNumber) && strFineNumber.Length >= 10) strFineNumber = strFineNumber.Substring(0, strFineNumber.Length - 1);
                

                string sAccessToken;

                bool? bAuthRetry = null;
                long lEllapsedTime = 0;

                while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                {
                    if (!bAuthRetry.HasValue)
                        bAuthRetry = false;

                    if (this.MadridToken(sTokenUrl, sClientId, sClientSecret, out sAccessToken))
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                ((sender, certificate, chain, sslPolicyErrors) => true);

                        string sUrl = string.Format("{0}/fine-annulation-authorization", sBaseUrl);

                        WebRequest oRequest = WebRequest.Create(sUrl);

                        oRequest.Method = "POST";
                        oRequest.ContentType = "application/json";
                        oRequest.Timeout = iWSLocalTimeout;
                        oRequest.Headers.Add("Authorization", string.Format("Bearer {0}", sAccessToken));

                        TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInstallation.INS_TIMEZONE_ID);
                        DateTime dtFineQueryUTC = TimeZoneInfo.ConvertTime(dtFineQuery, tzi, TimeZoneInfo.Utc);

                        var oBodyContent = new MadridQueryFineRequest()
                        {
                            City = new MadridFineCity()
                            {
                                CodSystem = oInstallation.INS_PHY_ZONE_COD_SYSTEM,
                                CodGeoZone = oInstallation.INS_PHY_ZONE_COD_GEO_ZONE,
                                CodCity = oInstallation.INS_PHY_ZONE_COD_CITY
                            },
                            DtRequestUTC = string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-ddTHH:mm:ss}Z", dtFineQueryUTC),
                            IsoLang = "es"
                        };
                        if (string.IsNullOrEmpty(strPlate))
                            oBodyContent.ExpedientNumber = strFineNumber;
                        else
                            oBodyContent.AnnulationCode /*LicensePlate*/ = strPlate;

                        string sJsonIn = JsonConvert.SerializeObject(oBodyContent);

                        Logger_AddLogMessage(string.Format("Madrid2PlatformQueryFinePaymentQuantity request.url={0}, request.authorization={1}, request.jsonIn={2}", sUrl, sAccessToken, PrettyJSON(sJsonIn)), LogLevels.logINFO);

                        byte[] byteArray = Encoding.UTF8.GetBytes(sJsonIn);

                        oRequest.ContentLength = byteArray.Length;
                        Stream dataStream = oRequest.GetRequestStream();
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();

                        MadridQueryFineResponse oResponse = null;

                        try
                        {
                            watch = Stopwatch.StartNew();

                            WebResponse response = oRequest.GetResponse();
                            // Display the status.
                            HttpWebResponse oWebResponse = ((HttpWebResponse)response);

                            lEllapsedTime = watch.ElapsedMilliseconds;

                            bAuthRetry = false;

                            if (oWebResponse.StatusCode == HttpStatusCode.Created)
                            {
                                dataStream = response.GetResponseStream();
                                StreamReader reader = new StreamReader(dataStream);
                                string responseFromServer = reader.ReadToEnd();

                                Logger_AddLogMessage(string.Format("Madrid2PlatformQueryFinePaymentQuantity response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);

                                oResponse = (MadridQueryFineResponse)JsonConvert.DeserializeObject(responseFromServer, typeof(MadridQueryFineResponse));

                                if (oResponse != null)
                                {
                                    rtRes = Convert_MadridPlatformAuthResult_TO_ResultType(oResponse.AuthResult);                                    
                                }
                                else
                                    rtRes = ResultType.Result_Error_Generic;

                                reader.Close();
                                dataStream.Close();
                            }

                            response.Close();

                        }
                        catch (WebException ex)
                        {
                            if ((watch != null) && (lEllapsedTime == 0))
                                lEllapsedTime = watch.ElapsedMilliseconds;

                            if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                            {
                                if (bAuthRetry.Value == false)
                                {
                                    m_sMadridAccessToken = null;
                                    bAuthRetry = true;
                                }
                                else
                                    bAuthRetry = false;
                            }
                            else
                            {
                                bAuthRetry = false;
                                if (ex.Response != null)
                                    oErrorResponse = MadridFineErrorResponse.Load((HttpWebResponse)ex.Response);
                                else
                                    oErrorResponse = new MadridFineErrorResponse()
                                    {
                                        status = (int)ex.Status,
                                        timeout = (ex.Status == WebExceptionStatus.Timeout)
                                    };

                                rtRes = ResultType.Result_Error_Generic;
                            }
                            Logger_AddLogException(ex, "Madrid2PlatformConfirmParking::WebException", LogLevels.logERROR);
                        }
                        catch (Exception e)
                        {
                            if ((watch != null) && (lEllapsedTime == 0))
                                lEllapsedTime = watch.ElapsedMilliseconds;

                            bAuthRetry = false;
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogException(e, "Madrid2PlatformConfirmParking::Exception", LogLevels.logERROR);
                        }

                        if (rtRes == ResultType.Result_OK)
                        {                            
                            sUrl = string.Format("{0}/annulable-fine/{1}", sBaseUrl, oResponse.Expedient /*strPlate*/);

                            oRequest = WebRequest.Create(sUrl);

                            oRequest.Method = "GET";
                            oRequest.ContentType = "application/json";
                            oRequest.Timeout = iWSLocalTimeout;
                            oRequest.Headers.Add("Authorization", string.Format("Bearer {0}", sAccessToken));

                            Logger_AddLogMessage(string.Format("Madrid2PlatformQueryFinePaymentQuantity request.url={0}, request.authorization={1}", sUrl, sAccessToken), LogLevels.logINFO);

                            try
                            {
                                WebResponse response = oRequest.GetResponse();                                
                                HttpWebResponse oWebResponse = ((HttpWebResponse)response);
                                
                                bAuthRetry = false;

                                if (oWebResponse.StatusCode == HttpStatusCode.OK)
                                {
                                    dataStream = response.GetResponseStream();
                                    StreamReader reader = new StreamReader(dataStream);
                                    string responseFromServer = reader.ReadToEnd();

                                    Logger_AddLogMessage(string.Format("Madrid2PlatformQueryFinePaymentQuantity response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);

                                    MadridTrfFineResponse oResponse2 = (MadridTrfFineResponse)JsonConvert.DeserializeObject(responseFromServer, typeof(MadridTrfFineResponse));

                                    if (oResponse2 != null)
                                    {
                                        parametersOut["AuthId"] = oResponse.AuthId.ToString();
                                        parametersOut["ExtGrpId"] = oResponse2.TrfFine.CodPhyZone;
                                        parametersOut["q"] = ((int)(oResponse.TotAmo * 100)).ToString();
                                        parametersOut["cur"] = oResponse2.TrfFine.Infraction.Currency;
                                        parametersOut["fnumber"] = oResponse2.TrfFine.ExpedientNumber;
                                        parametersOut["d"] = dtFineQuery.ToString("HHmmssddMMyy");
                                        if (!string.IsNullOrEmpty(oResponse2.TrfFine.AnulStartDtUTC))
                                        {
                                            DateTime dtAnulStartDtUTC = DateTime.ParseExact(oResponse2.TrfFine.AnulStartDtUTC, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);
                                            DateTime? dtDf = geograficAndTariffsRepository.ConvertUTCToInstallationDateTime(oInstallation.INS_ID, dtAnulStartDtUTC.AddMinutes(oResponse2.TrfFine.Infraction.AllowCancelMinutes));
                                            if (dtDf.HasValue) parametersOut["df"] = dtDf.Value.ToString("HHmmssddMMyy");
                                        }
                                        parametersOut["lp"] = oResponse2.TrfFine.Vehicle.LicensePlate.Trim().Replace(" ", "");
                                        parametersOut["ta"] = oResponse2.TrfFine.Infraction.Article;
                                        parametersOut["dta"] = oResponse2.TrfFine.Infraction.ByLaw;
                                        parametersOut["lit"] = "";
                                    }
                                    else
                                        rtRes = ResultType.Result_Error_Generic;

                                    reader.Close();
                                    dataStream.Close();
                                }

                                response.Close();

                            }
                            catch (WebException ex)
                            {
                                if ((watch != null) && (lEllapsedTime == 0))
                                    lEllapsedTime = watch.ElapsedMilliseconds;

                                if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                                {
                                    if (bAuthRetry.Value == false)
                                    {
                                        m_sMadridAccessToken = null;
                                        bAuthRetry = true;
                                    }
                                    else
                                        bAuthRetry = false;
                                }
                                else
                                {
                                    bAuthRetry = false;
                                    if (ex.Response != null)
                                        oErrorResponse = MadridFineErrorResponse.Load((HttpWebResponse)ex.Response);
                                    else
                                        oErrorResponse = new MadridFineErrorResponse()
                                        {
                                            status = (int)ex.Status,
                                            timeout = (ex.Status == WebExceptionStatus.Timeout)
                                        };

                                    rtRes = ResultType.Result_Error_Generic;
                                }
                                Logger_AddLogException(ex, "Madrid2PlatformConfirmParking::WebException", LogLevels.logERROR);
                            }
                            catch (Exception e)
                            {
                                if ((watch != null) && (lEllapsedTime == 0))
                                    lEllapsedTime = watch.ElapsedMilliseconds;

                                bAuthRetry = false;
                                rtRes = ResultType.Result_Error_Generic;
                                Logger_AddLogException(e, "Madrid2PlatformConfirmParking::Exception", LogLevels.logERROR);
                            }

                        }
                        
                        if (rtRes != ResultType.Result_OK)
                        {
                            parametersOut["d"] = dtFineQuery.ToString("HHmmssddMMyy");
                            parametersOut["df"] = "";
                            parametersOut["lp"] = "";
                            parametersOut["ta"] = "";
                            parametersOut["dta"] = "";
                            parametersOut["lit"] = "";
                        }

                    }
                }

                parametersOut["r"] = Convert.ToInt32(rtRes);

            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "Madrid2PlatformQueryFinePaymentQuantity::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strParamsIn, strParamsOut, true, oNotificationEx);
            }
            catch
            {
            }

            return rtRes;
        }

        public ResultType Madrid2PlatformConfirmFinePayment(string strFineNumber, DateTime dtFineQuery, DateTime dtUTCInsertionDate, int iQuantity, USER oUser, INSTALLATION oInstallation,
                                                            decimal dTicketId, decimal dAuthId, GROUP oGroup, int? iWSTimeout,
                                                            ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            str3dPartyOpNum = "";
            lEllapsedTime = -1;
            Stopwatch watch = null;

            MadridErrorResponse oErrorResponse = null;

            string strParamsIn = "";
            string strParamsOut = "";
            Exception oNotificationEx = null;

            try
            {
                string sBaseUrl = oInstallation.INS_FINE_WS_URL;
                string sClientId = oInstallation.INS_FINE_WS_HTTP_USER;
                string sClientSecret = oInstallation.INS_FINE_WS_HTTP_PASSWORD;
                string sTokenUrl = oInstallation.INS_FINE_WS_AUTH_HASH_KEY;
                string sCodSystem = oInstallation.INS_PHY_ZONE_COD_SYSTEM;
                string sCodGeoZone = oInstallation.INS_PHY_ZONE_COD_GEO_ZONE;
                string sCodCity = oInstallation.INS_PHY_ZONE_COD_CITY;

                // ***
                if (Madrid2AllowedZone(oGroup, 4))
                {
                    Madrid2Params(out sBaseUrl, out sClientId, out sClientSecret, out sTokenUrl, out sCodSystem, out sCodGeoZone, out sCodCity);
                }
                // ***

                string sExtGrpId = "000";
                if (oGroup != null)
                {
                    sExtGrpId = oGroup.GRP_ID_FOR_EXT_OPS;
                }

                if (strFineNumber.Length >= 10) strFineNumber = strFineNumber.Substring(0, strFineNumber.Length - 1);

                DateTime? dtUTCFineQuery = geograficAndTariffsRepository.ConvertInstallationDateTimeToUTC(oInstallation.INS_ID, dtFineQuery);

                DateTime dtNowUtc = DateTime.UtcNow;
                DateTime dtNow = geograficAndTariffsRepository.ConvertUTCToInstallationDateTime(oInstallation.INS_ID, dtNowUtc).Value;

                var oBodyContent = new MadridConfirmFineRequest()
                {
                    City = new MadridFineCity()
                    {
                        CodSystem = sCodSystem,
                        CodGeoZone = sCodGeoZone,
                        CodCity = sCodCity
                    },
                    FineTrans = new MadridFineTrans ()
                    {
                        AuthId = Convert.ToInt32(dAuthId),
                        //OperationDateUTC = dtUTCFineQuery.Value,
                        OperationDateUTC = string.Format(CultureInfo.InvariantCulture, "{0:yyyy-MM-ddTHH:mm:ss}Z", dtNowUtc), // dtUTCInsertionDate,
                        TariffId = 2,
                        TicketNum = string.Format("98{0}00000{1}{2}", sExtGrpId, dtNow.DayOfYear.ToString("000"), dtNow.ToString("HHmm")),
                        TransId = Convert.ToInt32(dTicketId),
                        FineAmount = iQuantity / 100,
                        FineExpedientNumber = strFineNumber
                    }
                };

                string sAccessToken;

                bool? bAuthRetry = null;
                while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                {
                    if (!bAuthRetry.HasValue)
                        bAuthRetry = false;

                    if (this.MadridToken(sTokenUrl, sClientId, sClientSecret, out sAccessToken))
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                ((sender, certificate, chain, sslPolicyErrors) => true);

                        string sUrl = string.Format("{0}/fine-annulation-payment-transaction", sBaseUrl);

                        WebRequest oRequest = WebRequest.Create(sUrl);

                        oRequest.Method = "POST";
                        oRequest.ContentType = "application/json";
                        oRequest.Timeout = Get3rdPartyWSTimeout();
                        oRequest.Headers.Add("Authorization", string.Format("Bearer {0}", sAccessToken));

                        string sJsonIn = JsonConvert.SerializeObject(oBodyContent);

                        Logger_AddLogMessage(string.Format("Madrid2PlatformConfirmFinePayment request.url={0}, request.authorization={1}, request.jsonIn={2}", sUrl, sAccessToken, sJsonIn), LogLevels.logINFO);

                        byte[] byteArray = Encoding.UTF8.GetBytes(sJsonIn);

                        oRequest.ContentLength = byteArray.Length;
                        Stream dataStream = oRequest.GetRequestStream();
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();

                        try
                        {

                            WebResponse response = oRequest.GetResponse();
                            // Display the status.
                            HttpWebResponse oWebResponse = ((HttpWebResponse)response);

                            bAuthRetry = false;

                            if (oWebResponse.StatusCode == HttpStatusCode.Created)
                            {
                                dataStream = response.GetResponseStream();
                                StreamReader reader = new StreamReader(dataStream);
                                string responseFromServer = reader.ReadToEnd();

                                Logger_AddLogMessage(string.Format("Madrid2PlatformConfirmFinePayment response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);

                                MadridPayTransactionResponse oResponse = (MadridPayTransactionResponse)JsonConvert.DeserializeObject(responseFromServer, typeof(MadridPayTransactionResponse));

                                if (oResponse != null)
                                {
                                    str3dPartyOpNum = oResponse.id.ToString();
                                    //str3dPartyOpNum = dAuthId.ToString();  // *****????
                                    rtRes = ResultType.Result_OK;
                                }
                                else
                                    rtRes = ResultType.Result_Error_Generic;

                                reader.Close();
                                dataStream.Close();
                            }

                            response.Close();
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                            {
                                if (bAuthRetry.Value == false)
                                {
                                    m_sMadridAccessToken = null;
                                    bAuthRetry = true;
                                }
                                else
                                    bAuthRetry = false;
                            }
                            else
                            {
                                bAuthRetry = false;
                                if (ex.Response != null)
                                    oErrorResponse = MadridErrorResponse.Load((HttpWebResponse)ex.Response);
                                else
                                    oErrorResponse = new MadridErrorResponse()
                                    {
                                        error = new MadridError()
                                        {
                                            status = (int)ex.Status
                                        },
                                        timeout = (ex.Status == WebExceptionStatus.Timeout)
                                    };

                                rtRes = oErrorResponse.GetResultType();
                            }
                            Logger_AddLogException(ex, "Madrid2PlatformConfirmParking::WebException", LogLevels.logERROR);
                        }
                        catch (Exception e)
                        {
                            bAuthRetry = false;
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogException(e, "Madrid2PlatformConfirmParking::Exception", LogLevels.logERROR);
                        }

                    }
                    else
                    {
                        rtRes = ResultType.Result_Error_Generic;
                        if (parametersOut.IndexOfKey("autorecharged") >= 0)
                        {
                            parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                        }
                        if (parametersOut.IndexOfKey("newbal") >= 0)
                        {
                            parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));
                        }
                    }
                }

                parametersOut["r"] = Convert.ToInt32(rtRes);

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "MadridPlatformConfirmFinePayment::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strParamsIn, strParamsOut, true, oNotificationEx);
            }
            catch
            {
            }


            return rtRes;
        }

        #region Mifas
        public ResultType MifasQueryFinePaymentQuantity(string strFineNumber, DateTime dtFineQuery, USER oUser, INSTALLATION oInstallation, int? iWSTimeout, ref SortedList parametersOut)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            Exception oNotificationEx = null;
            Int32 iResponseWS=-999;
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

                MifasWS.anulaciones oFineWS = new MifasWS.anulaciones();

                oFineWS.Url = oInstallation.INS_FINE_WS_URL;
                oFineWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();
                
                DateTime dtInstallation = dtFineQuery;
                
                long ltFineQuery = Convert.ToInt64(dtFineQuery.ToString("HHmmssddMMyy"));

                Logger_AddLogMessage(string.Format("MifasQueryFinePaymentQuantity EsExpedienteAnulable({0},{1},{2})", Convert.ToInt32(oInstallation.INS_MIFAS_CITY_ID), strFineNumber, ltFineQuery), LogLevels.logDEBUG);

                iResponseWS = oFineWS.EsExpedienteAnulable(Convert.ToInt32(oInstallation.INS_MIFAS_CITY_ID), strFineNumber, ltFineQuery);
                
                Logger_AddLogMessage(string.Format("MifasQueryFinePaymentQuantity EsExpedienteAnulable={0}", iResponseWS), LogLevels.logDEBUG);

                rtRes = Convert_ResultTypeMifasEsExpedienteAnulableWS_TO_ResultType(iResponseWS);

                if (rtRes.Equals(ResultType.Result_OK))
                {

                    Logger_AddLogMessage(string.Format("MifasQueryFinePaymentQuantity ObtenerImporteAnulacion({0},{1},{2})", Convert.ToInt32(oInstallation.INS_MIFAS_CITY_ID), strFineNumber, ltFineQuery), LogLevels.logDEBUG);

                    Int32 iResponseWS2 = oFineWS.ObtenerImporteAnulacion(Convert.ToInt32(oInstallation.INS_MIFAS_CITY_ID), strFineNumber, ltFineQuery);
                    
                    Logger_AddLogMessage(string.Format("MifasQueryFinePaymentQuantity ObtenerImporteAnulacion={0}", iResponseWS2), LogLevels.logDEBUG);
                    
                    // > 0 Importe anulación en céntimos
                    if (iResponseWS2>0)
                    {
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                        parametersOut["q"] = iResponseWS2;
                        parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;
                        parametersOut["lp"] = String.Empty;//oData["plate"];
                        parametersOut["AuthId"] = String.Empty; //oData["id"];
                        parametersOut["d"] = dtFineQuery.ToString("HHmmssddMMyy"); //dt.ToString("HHmmssddMMyy");
                        parametersOut["df"] = String.Empty;//dtMaxPay.Value.ToString("HHmmssddMMyy");

                        parametersOut["ta"] = String.Empty;//oData["code"];
                        parametersOut["dta"] = String.Empty;//oData["description"];
                    }
                    else
                    {
                        rtRes = Convert_ResultTypeMifasEsExpedienteAnulableWS_TO_ResultType(iResponseWS2);

                    }
                }
            }
            catch (Exception e)
            {
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "MifasQueryFinePaymentQuantity::Exception", LogLevels.logERROR);

            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, string.Format("({0},{1},{2},{3})",
                    oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD, strFineNumber, oInstallation.INS_STANDARD_CITY_ID), Convert.ToString(iResponseWS), true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }


        public ResultType MifasConfirmFinePayment(string strFineNumber, USER oUser, INSTALLATION oInstallation, DateTime dtFineQuery, 
                                                  int? iWSTimeout, ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            Exception oNotificationEx = null;
            lEllapsedTime = 0;
            Stopwatch watch = null;
            Int32 iResponseWS=-999;
            str3dPartyOpNum = String.Empty;

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                MifasWS.anulaciones oFineWS = new MifasWS.anulaciones();

                oFineWS.Url = oInstallation.INS_FINE_WS_URL;
                oFineWS.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();

                DateTime dtInstallation = dtFineQuery;
                Int32 ParkingNumber = -1;
                Int32 NumberPrintedTicket = -1;
                
                long ltFineQuery = Convert.ToInt64(dtFineQuery.ToString("HHmmssddMMyy"));

                Logger_AddLogMessage(string.Format("MifasConfirmFinePayment Timeout={5} AnularExpediente({0},{1},{2},{3},{4})", Convert.ToInt32(oInstallation.INS_MIFAS_CITY_ID), strFineNumber, 
                                        ltFineQuery, ParkingNumber, NumberPrintedTicket, oFineWS.Timeout), LogLevels.logDEBUG);

                watch = Stopwatch.StartNew();
                iResponseWS = oFineWS.AnularExpediente(Convert.ToInt32(oInstallation.INS_MIFAS_CITY_ID), strFineNumber, ltFineQuery, ParkingNumber, NumberPrintedTicket);
                lEllapsedTime = watch.ElapsedMilliseconds;

                Logger_AddLogMessage(string.Format("MifasConfirmFinePayment AnularExpediente={0}", iResponseWS), LogLevels.logDEBUG);
                switch (iResponseWS)
                {
                    //1 Expediente anulado
                    case 1:
                        rtRes = ResultType.Result_OK;
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        break;
                    //0 Expediente no anulado
                    case 0:
                        rtRes = ResultType.Result_Error_Generic;
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        break;
                    default:
                        //-1 Error en la anul·lación
                        rtRes = ResultType.Result_Error_Generic;
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        if (parametersOut.IndexOfKey("autorecharged") >= 0)                
                        {
                            parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                        }

                        if (parametersOut.IndexOfKey("newbal") >= 0)
                        {
                            parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "MifasConfirmFinePayment::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, string.Format("({0},{1},{2},{3})",
                    oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD, oInstallation.INS_STANDARD_CITY_ID, strFineNumber), Convert.ToString(iResponseWS), true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;

        }
        #endregion

        #region BSM

        public ResultType BSMQueryFinePaymentQuantity(INSTALLATION oInstallation, string sFineNumber, DateTime dtFineQuery, int? iWSTimeout, ref SortedList parametersOut)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;

            try 
            {

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInstallation.INS_TIMEZONE_ID);

                bool bCancellable = false;
                string sPlate;
                string strSecureId;
                string sNotCancellableReason;
                DateTime? dtUtcMaxCanellationDate;
                decimal dCancellationAmount;
                int iResult;
                int? iSegmentId;
                string sSegmentType;
                BSMErrorResponse oErrorResponse;
                long lEllapsedTime;
                rtRes = BSMAnnulationDetails(oInstallation, sFineNumber,iWSTimeout,
                                             out bCancellable, out sPlate, out strSecureId, out sNotCancellableReason, out dtUtcMaxCanellationDate, out dCancellationAmount, 
                                             out iResult, out iSegmentId, out sSegmentType, out oErrorResponse, out lEllapsedTime);

                if (rtRes == ResultType.Result_OK)
                {
                    if (bCancellable)
                    {
                        parametersOut["r"] = Convert.ToInt32(ResultType.Result_OK);
                        parametersOut["q"] = Convert.ToInt32(dCancellationAmount * 100);
                        parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;
                        parametersOut["lp"] = sPlate;

                        parametersOut["d"] = dtFineQuery.ToString("HHmmssddMMyy");
                        if (dtUtcMaxCanellationDate.HasValue)
                        {
                            parametersOut["df"] = TimeZoneInfo.ConvertTime(dtUtcMaxCanellationDate.Value, TimeZoneInfo.Utc, tzi).ToString("HHmmssddMMyy");
                        }

                        parametersOut["ta"] = "";
                        parametersOut["dta"] = "";
                    }
                    else
                    {
                        switch (sNotCancellableReason)
                        {
                            case "Denuncia No existente": rtRes = ResultType.Result_Error_Fine_Number_Not_Found; break;
                            case "Denuncia ya anulada": rtRes = ResultType.Result_Error_Fine_Number_Already_Paid; break;
                            case "Codigo de infraccion no anulable": rtRes = ResultType.Result_Error_Fine_Type_Not_Payable; break;
                            case "Anulacion fuera del tiempo establecido": rtRes = ResultType.Result_Error_Fine_Payment_Period_Expired; break;
                            default: rtRes = ResultType.Result_Error_Fine_Number_Not_Found; break;
                        }                        
                        parametersOut["r"] = Convert.ToInt32(rtRes);
                        try
                        {
                            parametersOut["lp"] = sPlate.Trim().Replace(" ", "");
                            parametersOut["d"] = dtFineQuery.ToString("HHmmssddMMyy");
                            if (dtUtcMaxCanellationDate.HasValue)
                            {
                                parametersOut["df"] = TimeZoneInfo.ConvertTime(dtUtcMaxCanellationDate.Value, TimeZoneInfo.Utc, tzi).ToString("HHmmssddMMyy");
                            }

                            parametersOut["ta"] = "";
                            parametersOut["dta"] = sNotCancellableReason;
                        }
                        catch { }
                    }

                }
            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "BSMQueryFinePaymentQuantity::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }
        protected ResultType BSMAnnulationDetails(INSTALLATION oInstallation, string sFineNumber, int? iWSTimeout,
                                                  out bool bCancellable, out string sPlate,
                                                  out string strSecureId, out string sNotCancellableReason, out DateTime? dtUtcMaxCanellationDate, 
                                                  out decimal dCancellationAmount, out int iResult, out int? iSegmentId, out string sSegmentType, 
                                                  out BSMErrorResponse oErrorResponse, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            bCancellable = false;
            sPlate = "";
            strSecureId = "";
            sNotCancellableReason = "";
            dtUtcMaxCanellationDate = null;
            dCancellationAmount = 0;
            iResult = 0;
            iSegmentId = null;
            sSegmentType = "";
            oErrorResponse = null;

            lEllapsedTime = 0;
            Stopwatch watch = null;

            string strParamsIn = "";
            string strParamsOut = "";
            Exception oNotificationEx = null;
            int iLocalWSTimeout = iWSTimeout ?? Get3rdPartyWSTimeout();

            try
            {
                string sBaseUrl = oInstallation.INS_FINE_WS_URL;
                string sUser = oInstallation.INS_FINE_WS_HTTP_USER;
                string sPassword = oInstallation.INS_FINE_WS_HTTP_PASSWORD;

                // ***
                /*sBaseUrl = "https://preapi.bsmsa.eu/ext/api";
                sUser = "xL6BxwxOgfXJ7wDEQD7oRT104iMa";
                sPassword = "3jhZBXTsG3jVhN78AsShrGgAoKka";*/
                // ***

                string sAccessToken;

                bool? bAuthRetry = null;
                while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                {
                    if (!bAuthRetry.HasValue)
                        bAuthRetry = false;

                    long lLocalEllapsedTime = 0;
                    if (this.BSMToken(sBaseUrl, sUser, sPassword, iLocalWSTimeout, out sAccessToken, out lLocalEllapsedTime))
                    {
                        iLocalWSTimeout -= (int)lLocalEllapsedTime;

                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                ((sender, certificate, chain, sslPolicyErrors) => true);

                        //NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                        //numberFormatProvider.NumberDecimalSeparator = ".";

                        string sUrl = string.Format("{0}/fines/annulation/{1}", sBaseUrl, sFineNumber);

                        WebRequest oRequest = WebRequest.Create(sUrl);

                        oRequest.Method = "GET";
                        oRequest.ContentType = "application/x-www-form-urlencoded";
                        //oRequest.ContentType = "application/json";
                        oRequest.Timeout = iLocalWSTimeout;
                        oRequest.Headers.Add("Authorization", string.Format("Bearer {0}", sAccessToken));

                        Logger_AddLogMessage(string.Format("BSMAnnulationDetails request.url={0}, Timeout={2} request.authorization={1}", sUrl, sAccessToken, oRequest.Timeout), LogLevels.logINFO);

                        try
                        {

                            WebResponse response = oRequest.GetResponse();                            
                            HttpWebResponse oWebResponse = ((HttpWebResponse)response);

                            bAuthRetry = false;

                            if (oWebResponse.StatusCode == HttpStatusCode.OK)
                            {
                                Stream dataStream = response.GetResponseStream();
                                StreamReader reader = new StreamReader(dataStream);
                                string responseFromServer = reader.ReadToEnd();

                                Logger_AddLogMessage(string.Format("BSMAnnulationDetails response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);

                                dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                                bCancellable = ((int)oResponse.cancellable.Value == 1);
                                sPlate = oResponse.plate;
                                if (oResponse.secureId != null)
                                    strSecureId = (string)oResponse.secureId.Value;
                                sNotCancellableReason = oResponse.notCancellableReason;
                                if (oResponse.maxCancellationDate != null)
                                    dtUtcMaxCanellationDate = (DateTime)oResponse.maxCancellationDate.Value;
                                dCancellationAmount = (decimal)oResponse.cancellationAmount.Value;
                                iResult = (int)oResponse.result.Value;
                                if (oResponse.idTramo != null)
                                    iSegmentId = (int)oResponse.idTramo.Value;
                                sSegmentType = oResponse.tipoTramo;

                                rtRes = ResultType.Result_OK;

                                reader.Close();
                                dataStream.Close();
                            }

                            response.Close();
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                            {
                                if (bAuthRetry.Value == false)
                                {
                                    m_sBSMAccessToken = null;
                                    bAuthRetry = true;
                                }
                                else
                                    bAuthRetry = false;
                            }
                            else
                            {
                                bAuthRetry = false;
                                if (ex.Response != null)
                                    oErrorResponse = BSMErrorResponse.Load((HttpWebResponse)ex.Response);
                                else
                                    oErrorResponse = new BSMErrorResponse()
                                    {
                                        status = ex.Status.ToString(),
                                        timeout = true //(ex.Status == WebExceptionStatus.Timeout)
                                    };
                                rtRes = oErrorResponse.GetResultType();
                            }
                            Logger_AddLogException(ex, "BSMAnnulationDetails::WebException", LogLevels.logERROR);
                        }
                        catch (Exception e)
                        {
                            bAuthRetry = false;
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogException(e, "BSMAnnulationDetails::Exception", LogLevels.logERROR);
                        }

                    }
                    else
                        rtRes = ResultType.Result_Error_BSM_Not_Answering;
                }
            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "BSMAnnulationDetails::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strParamsIn, strParamsOut, true, oNotificationEx);
            }
            catch
            {
            }


            return rtRes;
        }

        public ResultType BSMConfirmFinePayment(INSTALLATION oInstallation, string sFineNumber,  
                                                DateTime dtOperationDate, int? iWSTimeout, 
                                                ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            str3dPartyOpNum = "";
            lEllapsedTime = 0;

            try
            {

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(oInstallation.INS_TIMEZONE_ID);

                int iTicketId;
                string sPlate;
                string sOperationCode;
                BSMErrorResponse oErrorResponse;                
                rtRes = BSMPayAnnulation(oInstallation, sFineNumber, iWSTimeout, out iTicketId, out sPlate, out sOperationCode, out oErrorResponse, out lEllapsedTime);

                parametersOut["r"] = Convert.ToInt32(rtRes);

                if (rtRes == ResultType.Result_OK)
                {
                    str3dPartyOpNum = iTicketId.ToString();
                }
                else
                {
                    if (parametersOut.IndexOfKey("autorecharged") >= 0)
                    {
                        parametersOut.RemoveAt(parametersOut.IndexOfKey("autorecharged"));
                    }
                    if (parametersOut.IndexOfKey("newbal") >= 0)
                    {
                        parametersOut.RemoveAt(parametersOut.IndexOfKey("newbal"));
                    }
                }
            }
            catch (Exception e)
            {
                rtRes = ResultType.Result_Error_Generic;
                parametersOut["r"] = Convert.ToInt32(rtRes);
                Logger_AddLogException(e, "BSMConfirmFinePayment::Exception", LogLevels.logERROR);
            }

            return rtRes;
        }
        protected ResultType BSMPayAnnulation(INSTALLATION oInstallation, string sFineNumber, int ?iWSTimeout,
                                              out int iTicketId, out string sPlate, out string sOperationCode, out BSMErrorResponse oErrorResponse, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            iTicketId = 0;
            sPlate = "";
            sOperationCode = "";
            oErrorResponse = null;

            lEllapsedTime = 0;
            Stopwatch watch = null;

            string strParamsIn = "";
            string strParamsOut = "";
            Exception oNotificationEx = null;
            int iLocalWSTimeout = iWSTimeout ?? Get3rdPartyWSTimeout();

            try
            {
                string sBaseUrl = oInstallation.INS_FINE_WS_URL;
                string sUser = oInstallation.INS_FINE_WS_HTTP_USER;
                string sPassword = oInstallation.INS_FINE_WS_HTTP_PASSWORD;

                // ***
                /*sBaseUrl = "https://preapi.bsmsa.eu/ext/api";
                sUser = "xL6BxwxOgfXJ7wDEQD7oRT104iMa";
                sPassword = "3jhZBXTsG3jVhN78AsShrGgAoKka";*/
                // ***

                string sAccessToken;

                bool? bAuthRetry = null;
                while (!bAuthRetry.HasValue || (bAuthRetry.HasValue && bAuthRetry.Value))
                {
                    if (!bAuthRetry.HasValue)
                        bAuthRetry = false;

                    long lLocalEllapsedTime = 0;
                    if (this.BSMToken(sBaseUrl, sUser, sPassword, iLocalWSTimeout, out sAccessToken, out lLocalEllapsedTime))
                    {
                        iLocalWSTimeout -= (int)lLocalEllapsedTime;
                        System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                ((sender, certificate, chain, sslPolicyErrors) => true);

                        //NumberFormatInfo numberFormatProvider = new NumberFormatInfo();
                        //numberFormatProvider.NumberDecimalSeparator = ".";

                        string sUrl = string.Format("{0}/fines/annulation/{1}", sBaseUrl, sFineNumber);

                        WebRequest oRequest = WebRequest.Create(sUrl);

                        oRequest.Method = "PUT";
                        oRequest.ContentType = "application/x-www-form-urlencoded";
                        //oRequest.ContentType = "application/json";
                        oRequest.Timeout = iLocalWSTimeout;
                        oRequest.Headers.Add("Authorization", string.Format("Bearer {0}", sAccessToken));

                        string sParametersIn = "grant_type=client_credentials";

                        Logger_AddLogMessage(string.Format("BSMPayAnnulation request.url={0}, Timeout={3} request.authorization={1}, request.body={2}", sUrl, sAccessToken, sParametersIn, oRequest.Timeout), LogLevels.logINFO);

                        byte[] byteArray = Encoding.UTF8.GetBytes(sParametersIn);

                        oRequest.ContentLength = byteArray.Length;
                        Stream dataStream = oRequest.GetRequestStream();
                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();

                        try
                        {

                            WebResponse response = oRequest.GetResponse();
                            HttpWebResponse oWebResponse = ((HttpWebResponse)response);

                            bAuthRetry = false;

                            if (oWebResponse.StatusCode == HttpStatusCode.OK)
                            {
                                dataStream = response.GetResponseStream();
                                StreamReader reader = new StreamReader(dataStream);
                                string responseFromServer = reader.ReadToEnd();

                                Logger_AddLogMessage(string.Format("BSMAnnulationDetails response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);

                                dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                                iTicketId = (int)oResponse.ticketId.Value;
                                sPlate = oResponse.plateNumber;
                                sOperationCode = oResponse.operationCode;

                                rtRes = ResultType.Result_OK;

                                reader.Close();
                                dataStream.Close();
                            }

                            response.Close();
                        }
                        catch (WebException ex)
                        {
                            if (ex.Response != null && ((HttpWebResponse)ex.Response).StatusCode == HttpStatusCode.Unauthorized)
                            {
                                if (bAuthRetry.Value == false)
                                {
                                    m_sBSMAccessToken = null;
                                    bAuthRetry = true;
                                }
                                else
                                    bAuthRetry = false;
                            }
                            else
                            {
                                bAuthRetry = false;
                                if (ex.Response != null)
                                    oErrorResponse = BSMErrorResponse.Load((HttpWebResponse)ex.Response);
                                else
                                    oErrorResponse = new BSMErrorResponse()
                                    {
                                        status = ex.Status.ToString(),
                                        timeout = (ex.Status == WebExceptionStatus.Timeout)
                                    };
                                rtRes = oErrorResponse.GetResultType();
                            }
                            Logger_AddLogException(ex, "BSMPayAnnulation::WebException", LogLevels.logERROR);
                        }
                        catch (Exception e)
                        {
                            bAuthRetry = false;
                            rtRes = ResultType.Result_Error_Generic;
                            Logger_AddLogException(e, "BSMPayAnnulation::Exception", LogLevels.logERROR);
                        }

                    }
                    else
                        rtRes = ResultType.Result_Error_BSM_Not_Answering;
                }
            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == 0))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }

                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "BSMPayAnnulation::Exception", LogLevels.logERROR);
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, strParamsIn, strParamsOut, true, oNotificationEx);
            }
            catch
            {
            }


            return rtRes;
        }

        #endregion

        #region Emisalba

        public ResultType EmisalbaQueryFinePaymentQuantity(string strFineNumber, DateTime dtFineQuery, USER oUser, INSTALLATION oInstallation, int? iWSTimeout, ref SortedList parametersOut)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            //Stopwatch watch = null;

            string sJsonIn = "";
            string sJsonOut = "";
            Exception oNotificationEx = null;

            try
            {
                //watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                // PRODUCCIOÓN: http://195.235.246.52:10136/api/Denuncias/Post
                // PRUEBAS: http://195.235.246.52:10137/api/Denuncias/Post

                string strURL = oInstallation.INS_FINE_WS_URL;
                WebRequest request = WebRequest.Create(strURL);

                var oExternUser = new EmisalbaExternUser();
                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    //request.Credentials = new System.Net.NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                    oExternUser.user = oInstallation.INS_FINE_WS_HTTP_USER;
                    oExternUser.password = oInstallation.INS_FINE_WS_HTTP_PASSWORD;
                    oExternUser.empresa = (ConfigurationManager.AppSettings["EmisalbaCompanyName"] ?? "");
                }

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();


                var oMsgRequest = new EmisalbaRequest()
                {
                    action = "GetDenunciaByCodigo",
                    message = new EmisalbaGetDenuncia()
                    {
                        codigo = strFineNumber,
                        ExternUser = oExternUser,
                    }
                };
                
                sJsonIn = JsonConvert.SerializeObject(oMsgRequest);

                Logger_AddLogMessage(string.Format("EmisalbaQueryFinePaymentQuantity request.url={0}, Timeout={2} request.json={1}", strURL, sJsonIn, request.Timeout), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(sJsonIn);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.

                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.
                        //lEllapsedTime = watch.ElapsedMilliseconds;
                        Logger_AddLogMessage(string.Format("EmisalbaQueryFinePaymentQuantity response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.

                        sJsonOut = responseFromServer;

                        var oResponse = (EmisalbaResponse<EmisalbaDenuncias>)JsonConvert.DeserializeObject(responseFromServer, typeof(EmisalbaResponse<EmisalbaDenuncias>));

                        rtRes = Convert_ResultTypeEmisalba_TO_ResultType(Convert.ToInt32(oResponse.error), oResponse.message.error);

                        if (rtRes == ResultType.Result_OK)
                        {
                            if (oResponse.message.denuncias != null && oResponse.message.denuncias.Any())
                            {
                                var oDenuncia = oResponse.message.denuncias[0];

                                DateTime dtTicketDate = dtFineQuery;
                                if (!string.IsNullOrEmpty(oDenuncia.FechaHora))
                                {
                                    dtTicketDate = DateTime.ParseExact(oDenuncia.FechaHora, "dd/MM/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                                }
                                if (dtTicketDate >= dtFineQuery)
                                {
                                    string strTicketTypeExtID = "0";
                                    bool bIsPayable = false;
                                    DateTime? dtMaxPayDate = null;
                                    int iAmount = 0;
                                    string strDesc1 = "";
                                    string strDesc2 = "";

                                    if (geograficAndTariffsRepository.getTicketTypePaymentInfo(oInstallation.INS_ID, dtFineQuery, dtFineQuery, strTicketTypeExtID,
                                                                                       out bIsPayable, out dtMaxPayDate, out iAmount, out strDesc1, out strDesc2))
                                    {                                        
                                        if (!bIsPayable)
                                        {
                                            rtRes = ResultType.Result_Error_Fine_Type_Not_Payable;
                                        }
                                        else
                                        {
                                            if (dtMaxPayDate.HasValue && dtFineQuery >= dtMaxPayDate.Value)
                                            {
                                                rtRes = ResultType.Result_Error_Fine_Payment_Period_Expired;
                                            }
                                            else
                                            {
                                                parametersOut["q"] = iAmount;
                                                parametersOut["cur"] = oInstallation.CURRENCy.CUR_ISO_CODE;
                                                
                                                parametersOut["lp"] = oDenuncia.Matricula;

                                                parametersOut["d"] = dtTicketDate;

                                                parametersOut["ta"] = strDesc1;
                                                parametersOut["dta"] = string.Format("{0}~{1}~{2}~{3}~{4}~{5}",
                                                                                     oDenuncia.Marca,
                                                                                     oDenuncia.Color,
                                                                                     oDenuncia.TipoVehiculo,
                                                                                     oDenuncia.Calle,
                                                                                     oDenuncia.Letra,
                                                                                     oDenuncia.Portal);

                                                parametersOut["fnumber"] = oDenuncia.Codigo;

                                                //Logger_AddLogMessage(string.Format("EmisalbaQueryFinePaymentQuantity:: dta={0}", parametersOut["dta"].ToString()), LogLevels.logINFO);

                                            }
                                        }
                                    }
                                }
                                else
                                    rtRes = ResultType.Result_Error_Fine_Payment_Period_Expired;

                            }
                            else
                                rtRes = ResultType.Result_Error_Fine_Number_Not_Found;

                        }

                        parametersOut["r"] = Convert.ToInt32(rtRes);

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    //if ((watch != null) && (lEllapsedTime == 0))
                    //{
                    //    lEllapsedTime = watch.ElapsedMilliseconds;
                    //}
                    Logger_AddLogException(e, "EmisalbaQueryFinePaymentQuantity::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                }

            }
            catch (Exception e)
            {
                //if ((watch != null) && (lEllapsedTime == 0))
                //{
                //    lEllapsedTime = watch.ElapsedMilliseconds;
                //}
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "EmisalbaQueryFinePaymentQuantity::Exception", LogLevels.logERROR);
            }
            finally
            {
                //watch.Stop();
                //watch = null;
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sJsonIn, sJsonOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;


        }

        public ResultType EmisalbaConfirmFinePayment(string strFineNumber, DateTime dtOperationDate, int iQuantity, USER oUser, INSTALLATION oInstallation, int? iWSTimeout, 
                                                     ref SortedList parametersOut, out string str3dPartyOpNum, out long lEllapsedTime)
        {
            ResultType rtRes = ResultType.Result_Error_Generic;
            Stopwatch watch = null;
            lEllapsedTime = 0;
            str3dPartyOpNum = "";

            string sJsonIn = "";
            string sJsonOut = "";
            Exception oNotificationEx = null;

            try
            {
                //watch = Stopwatch.StartNew();
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                // PRODUCCIOÓN: http:// 195.235.246.52:10136/api/Denuncias/Post
                // PRUEBAS: http:// 195.235.246.52:10137/api/Denuncias/Post

                string strURL = oInstallation.INS_FINE_WS_URL;
                WebRequest request = WebRequest.Create(strURL);

                var oExternUser = new EmisalbaExternUser();
                if (!string.IsNullOrEmpty(oInstallation.INS_FINE_WS_HTTP_USER))
                {
                    //request.Credentials = new System.Net.NetworkCredential(oInstallation.INS_FINE_WS_HTTP_USER, oInstallation.INS_FINE_WS_HTTP_PASSWORD);
                    oExternUser.user = oInstallation.INS_FINE_WS_HTTP_USER;
                    oExternUser.password = oInstallation.INS_FINE_WS_HTTP_PASSWORD;
                    oExternUser.empresa = (ConfigurationManager.AppSettings["EmisalbaCompanyName"] ?? "");
                }

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = iWSTimeout ?? Get3rdPartyWSTimeout();


                var oMsgRequest = new EmisalbaRequest()
                {
                    action = "AnularDenuncia",
                    message = new EmisalbaGetDenuncia()
                    {
                        codigo = strFineNumber,
                        ExternUser = oExternUser,
                    }
                };

                sJsonIn = JsonConvert.SerializeObject(oMsgRequest);

                Logger_AddLogMessage(string.Format("EmisalbaConfirmFinePayment request.url={0}, Timeout={2}, request.json={1}", strURL, sJsonIn, request.Timeout), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(sJsonIn);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.

                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.
                        //lEllapsedTime = watch.ElapsedMilliseconds;
                        Logger_AddLogMessage(string.Format("EmisalbaConfirmFinePayment response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.

                        var oResponse = (EmisalbaResponse<EmisalbaDenuncias>)JsonConvert.DeserializeObject(responseFromServer, typeof(EmisalbaResponse<EmisalbaDenuncias>));

                        rtRes = Convert_ResultTypeEmisalba_TO_ResultType(Convert.ToInt32(oResponse.error), oResponse.message.error);

                        parametersOut["r"] = Convert.ToInt32(rtRes);

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (Exception e)
                {
                    //if ((watch != null) && (lEllapsedTime == 0))
                    //{
                    //    lEllapsedTime = watch.ElapsedMilliseconds;
                    //}
                    Logger_AddLogException(e, "EmisalbaConfirmFinePayment::Exception", LogLevels.logERROR);
                    rtRes = ResultType.Result_Error_Generic;
                }

            }
            catch (Exception e)
            {
                //if ((watch != null) && (lEllapsedTime == 0))
                //{
                //    lEllapsedTime = watch.ElapsedMilliseconds;
                //}
                oNotificationEx = e;
                rtRes = ResultType.Result_Error_Generic;
                Logger_AddLogException(e, "EmisalbaConfirmFinePayment::Exception", LogLevels.logERROR);
            }
            finally
            {
                //watch.Stop();
                //watch = null;
            }

            try
            {
                m_notifications.Notificate(this.GetType().GetMethod(System.Reflection.MethodBase.GetCurrentMethod().Name), rtRes, sJsonIn, sJsonOut, true, oNotificationEx);
            }
            catch
            {

            }

            return rtRes;


        }        

        #endregion

    }

    #region Emisalba Objects

    public class EmisalbaGetDenuncia : EmisalbaRequestMsgBase
    {
        public string codigo { get; set; }
        public string matricula { get; set; }
    }

    public class EmisalbaDenuncias : EmisalbaResponseMsgBase
    {
        public EmisalbaDenuncia[] denuncias { get; set; }
    }

    public class EmisalbaDenuncia
    {
        public string Codigo { get; set; }
        public string Matricula { get; set; }
        public string Marca { get; set; }
        public string Color { get; set; }
        public string TipoVehiculo { get; set; }
        public string FechaHora { get; set; }
        public string Calle { get; set; }
        public string Letra { get; set; }
        public int Portal { get; set; }
    }

    #endregion

    #region Madrid Objects

    public class MadridFineCity
    {
        public string CodCity { get; set; }
        public string CodGeoZone { get; set; }
        public string CodSystem { get; set; }
    }
    public class MadridQueryFineRequest
    {
        public string AnnulationCode { get; set; }
        public MadridFineCity City { get; set; }
        public string ExpedientNumber { get; set; }
        public string IsoLang { get; set; }
        public string DtRequestUTC { get; set; }
    }

    public class MadridQueryFineResponse
    {
        public int? AuthId { get; set; }
        public int AuthResult { get; set; }
        public string eAuthResult { get; set; }
        public string Expedient { get; set; }
        public int Id { get; set; }
        public decimal? TotAmo { get; set; }
    }

    public class MadridFineInfraction
    {
        public bool AllowCancel { get; set; }
        public int AllowCancelMinutes { get; set; }
        public string Article { get; set; }
        public string ByLaw { get; set; }
        public decimal CancelAmount { get; set; }
        public string Currency { get; set; }
    }
    public class MadridFineVehicle
    {
        public string LicensePlate { get; set; }
    }
    public class MadridTrfFine
    {
        public string AnulStartDtUTC { get; set; }
        public string AnnulationCodes { get; set; }
        public string AnnuledDtUTC { get; set; }
        public string CreatedUTC { get; set; }
        public string ExpedientNumber { get; set; }
        public string Id { get; set; }
        public MadridFineInfraction Infraction { get; set; }
        public string InvalidatedDtUTC { get; set; }
        public string Invalidation { get; set; }
        public string LastModificationUTC { get; set; }
        public string State { get; set; }
        public string TicketNumber { get; set; }
        public MadridFineVehicle Vehicle { get; set; }
        public string CodPhyZone { get; set; }
    }
    public class MadridTrfFineResponse
    {
        public MadridTrfFine TrfFine { get; set; }
    }

    public class MadridFineTrans
    {
        public int AuthId { get; set; }
        public int TariffId { get; set; }
        public decimal FineAmount { get; set; }
        public string FineCancellationCode { get; set; }
        public string FineExpedientNumber { get; set; }
        public string OperationDateUTC { get; set; }
        public string TicketNum { get; set; }
        public int TransId { get; set; }
    }
    public class MadridConfirmFineRequest
    {
        public MadridFineCity City { get; set; }
        public MadridFineTrans FineTrans { get; set; }
    }


    public class MadridFineErrorResponse
    {
        protected static readonly CLogWrapper m_oLog = new CLogWrapper(typeof(MadridFineErrorResponse));

        public string type { get; set; }
        public string title { get; set; }
        public int? status { get; set; }
        public string detail { get; set; }
        public string instance { get; set; }
        public string additionalProp1 { get; set; }
        public string additionalProp2 { get; set; }
        public string additionalProp3 { get; set; }

        public bool timeout { get; set; }

        public MadridFineErrorResponse()
        {
            timeout = false;
        }

        public static MadridFineErrorResponse Load(HttpWebResponse oResponse)
        {
            MadridFineErrorResponse oRet = new MadridFineErrorResponse();

            string sJsonResponse = "";

            try
            {
                using (var stream = oResponse.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    sJsonResponse = reader.ReadToEnd();
                }
                oRet = JsonConvert.DeserializeObject<MadridFineErrorResponse>(sJsonResponse);
            }
            catch (Exception ex)
            {
                m_oLog.LogMessage(LogLevels.logERROR, "MadridFineErrorResponse::Load Exception", ex);
                oRet.timeout = true;
            }

            if (oResponse != null)
            {
                try
                {
                    oRet.timeout = (oResponse.StatusCode == HttpStatusCode.RequestTimeout);
                }
                catch (Exception ex) { }
            }

            m_oLog.LogMessage(LogLevels.logINFO, string.Format("MadridFineErrorResponse::Load response={0}", PrettyJSON(sJsonResponse)));

            return oRet;
        }

        protected static string PrettyJSON(string json)
        {

            try
            {
                dynamic parsedJson = JsonConvert.DeserializeObject(json);
                string strRes = JsonConvert.SerializeObject(parsedJson, Newtonsoft.Json.Formatting.Indented);
                return "\r\n\t" + strRes.Replace("\r\n", "\r\n\t") + "\r\n";
            }
            catch
            {
                return "\r\n\t" + json + "\r\n";
            }
        }
    }

    [Serializable()]
    public class AppealReturnData
    {
        public long? TicketId { get; set; }

        public string ticketnumber { get; set; }

        public TicketReturnModel SearchedTicket { get; set; }
        public TicketReturnModel Ticket { get; set; }

        public List<TicketReturnModel> AllTickets { get; set; }

        public List<TicketImageReturnModel> TicketsImages { get; set; }
    }

    [Serializable()]
    public class TicketAddress
    {
        public string road { get; set; }
        public string suburb { get; set; }
        public string city { get; set; }
        public string municipality { get; set; }
        public string state { get; set; }
        public string postcode { get; set; }
        public string country { get; set; }
        public string country_code { get; set; }
    }

    [Serializable()]
    public class TicketReturnModel
    {


        /// <summary>
        /// There are no comments for TickId in the schema.
        /// </summary>
        public long ticketid { get; set; }

        /// <summary>
        /// There are no comments for TickId in the schema.
        /// </summary>
        public List<TicketImageReturnModel> TicketImages { get; set; }


        public string ticketpdfbase64
        {
            get;
            set;
        }
        /// <summary>
        /// There are no comments for TickNumber in the schema.
        /// </summary>
        /// 
        public string ticketnumber
        {
            get;
            set;
        }

        public TicketAddress Address { get; set; }
        /// <summary>
        /// There are no comments for TickImei in the schema.
        /// </summary>
        /// 
        public string tickimei
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickCellWifiMac in the schema.
        /// </summary>
        /// 
        public string tickcellwifimac
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickCellBluetoothMac in the schema.
        /// </summary>
        /// 
        public string tickcellbluetoothmac
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickDate in the schema.
        /// </summary>
        /// 
        public DateTime ticketdate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickUtcDate in the schema.
        /// </summary>
        /// 
        public DateTime? ticketutcdate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickLprPlate in the schema.
        /// </summary>
        /// 
        public string ticketlprplate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickQueryPlate in the schema.
        /// </summary>
        /// 
        public string ticketqueryplate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickQueryEqLprPlate in the schema.
        /// </summary>
        /// 
        public long? ticketqueryeqlprplate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickStrNumber in the schema.
        /// </summary>
        /// 
        public string ticketstrnumber
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickLatitude in the schema.
        /// </summary>
        /// 
        public decimal ticketlatitude
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickLongitude in the schema.
        /// </summary>
        /// 
        public decimal ticketlongitude
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickImageUrl in the schema.
        /// </summary>
        /// 
        public string ticketimageurl
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPayAmount in the schema.
        /// </summary>
        /// 
        public long? ticketpayamount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickStatusDate in the schema.
        /// </summary>
        /// 
        public DateTime? TickStatusDate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickStatusDateUtc in the schema.
        /// </summary>
        /// 
        public DateTime? TickStatusDateUtc
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPayExtproOpId in the schema.
        /// </summary>
        /// 
        public string TickPayExtproOpId
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPayExtproPayEntity in the schema.
        /// </summary>
        /// 
        public string TickPayExtproPayEntity
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickMaxpaydate in the schema.
        /// </summary>
        /// 
        public DateTime? TickMaxpaydate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickParkingInidate in the schema.
        /// </summary>
        /// 
        public DateTime? TickParkingInidate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickParkingEnddate in the schema.
        /// </summary>
        /// 
        public DateTime? TickParkingEnddate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickParkingTime in the schema.
        /// </summary>
        /// 
        public long? TickParkingTime
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickSelectedPlate in the schema.
        /// </summary>
        /// 
        public string TickSelectedPlate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickFineNumber in the schema.
        /// </summary>
        /// 
        public string TickFineNumber
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickStrNumberFrom in the schema.
        /// </summary>
        /// 
        public string TickStrNumberFrom
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickStrNumberTo in the schema.
        /// </summary>
        /// 
        public string TickStrNumberTo
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickAmount in the schema.
        /// </summary>
        /// 
        public double? TickAmount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickSyncManaged in the schema.
        /// </summary>
        /// 
        public bool? TickSyncManaged
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickBalance in the schema.
        /// </summary>
        /// 
        public double? TickBalance
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickInitialAmount in the schema.
        /// </summary>
        /// 
        public long? TickInitialAmount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickAging2Date in the schema.
        /// </summary>
        /// 
        public DateTime? TickAging2Date
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickAging2Utcdate in the schema.
        /// </summary>
        /// 
        public DateTime? TickAging2Utcdate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickAging2Amount in the schema.
        /// </summary>
        /// 
        public long? TickAging2Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickAging3Date in the schema.
        /// </summary>
        /// 
        public DateTime? TickAging3Date
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickAging3Utcdate in the schema.
        /// </summary>
        /// 
        public DateTime? TickAging3Utcdate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickAging3Amount in the schema.
        /// </summary>
        /// 
        public long TickAging3Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickAging4Date in the schema.
        /// </summary>
        /// 
        public DateTime? TickAging4Date
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickAging4Utcdate in the schema.
        /// </summary>
        /// 
        public DateTime? TickAging4Utcdate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickAging4Amount in the schema.
        /// </summary>
        public long? TickAging4Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickAging5Date in the schema.
        /// </summary>
        /// 
        public DateTime? TickAging5Date
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickAging5Utcdate in the schema.
        /// </summary>
        /// 
        public DateTime? TickAging5Utcdate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickAging5Amount in the schema.
        /// </summary>
        /// 
        public long? TickAging5Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPublicNotes in the schema.
        /// </summary>
        /// 
        public string TickPublicNotes
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPrivateNotes in the schema.
        /// </summary>
        /// 
        public string TickPrivateNotes
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickIdType in the schema.
        /// </summary>
        /// 
        public long? TickIdType
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTransitDocumentissuedate in the schema.
        /// </summary>
        /// 
        public DateTime? TickTransitDocumentissuedate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTransitDocumentexpdate in the schema.
        /// </summary>
        /// 
        public DateTime? TickTransitDocumentexpdate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickIsWarning in the schema.
        /// </summary>
        /// 
        public bool? TickIsWarning
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickIsSentbypostalmail in the schema.
        /// </summary>
        /// 
        public bool? TickIsSentbypostalmail
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickFeeInitialAmount in the schema.
        /// </summary>
        /// 
        public long? TickFeeInitialAmount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickFeeAging2Amount in the schema.
        /// </summary>
        /// 
        public long? TickFeeAging2Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickFeeAging3Amount in the schema.
        /// </summary>
        /// 
        public long? TickFeeAging3Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickFeeAging4Amount in the schema.
        /// </summary>
        /// 
        public long? TickFeeAging4Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickFeeAging5Amount in the schema.
        /// </summary>
        /// 
        public long? TickFeeAging5Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTowingInitialAmount in the schema.
        /// </summary>
        /// 
        public long? TickTowingInitialAmount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTowingAging2Amount in the schema.
        /// </summary>
        /// 
        public long? TickTowingAging2Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTowingAging3Amount in the schema.
        /// </summary>
        /// 
        public long? TickTowingAging3Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTowingAging4Amount in the schema.
        /// </summary>
        /// 
        public long? TickTowingAging4Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTowingAging5Amount in the schema.
        /// </summary>
        /// 
        public long? TickTowingAging5Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickFee2InitialAmount in the schema.
        /// </summary>
        /// 
        public long? TickFee2InitialAmount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickFee2Aging2Amount in the schema.
        /// </summary>
        /// 
        public long? TickFee2Aging2Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickFee2Aging3Amount in the schema.
        /// </summary>
        /// 
        public long? TickFee2Aging3Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickFee2Aging4Amount in the schema.
        /// </summary>
        /// 
        public long? TickFee2Aging4Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickFee2Aging5Amount in the schema.
        /// </summary>
        /// 
        public long? TickFee2Aging5Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTotalInitialAmount in the schema.
        /// </summary>
        /// 
        public long? TickTotalInitialAmount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTotalAging2Amount in the schema.
        /// </summary>
        /// 
        public long? TickTotalAging2Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTotalAging3Amount in the schema.
        /// </summary>
        /// 
        public long? TickTotalAging3Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTotalAging4Amount in the schema.
        /// </summary>
        /// 
        public long? TickTotalAging4Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTotalAging5Amount in the schema.
        /// </summary>
        /// 
        public long? TickTotalAging5Amount
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickVin in the schema.
        /// </summary>
        /// 
        public string TickVin
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTransitDocName in the schema.
        /// </summary>
        /// 
        public string TickTransitDocName
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTransitDocAddrStrDesc in the schema.
        /// </summary>
        /// 
        public string TickTransitDocAddrStrDesc
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTransitDocAddrNum in the schema.
        /// </summary>
        /// 
        public string TickTransitDocAddrNum
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTransitDocCity in the schema.
        /// </summary>
        /// 
        public string TickTransitDocCity
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickTransitDocPostalCode in the schema.
        /// </summary>
        /// 
        public string TickTransitDocPostalCode
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickLocationSignDescription in the schema.
        /// </summary>
        /// 
        public string TickLocationSignDescription
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickSituationsDescription in the schema.
        /// </summary>
        /// 
        public string TickSituationsDescription
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickIsTowing in the schema.
        /// </summary>
        /// 
        public bool? TickIsTowing
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickCloseDate in the schema.
        /// </summary>
        /// 
        public DateTime? TickCloseDate
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickCloseDateUtc in the schema.
        /// </summary>
        /// 
        public DateTime? TickCloseDateUtc
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPrintedTicket in the schema.
        /// </summary>
        /// 
        public string TickPrintedTicket
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPrintedTicket2 in the schema.
        /// </summary>
        /// 
        public string TickPrintedTicket2
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPrintedTicket3 in the schema.
        /// </summary>
        /// 
        public string TickPrintedTicket3
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPrintedTicket4 in the schema.
        /// </summary>
        /// 
        public string TickPrintedTicket4
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPrintedTicketPreImg in the schema.
        /// </summary>
        /// 
        public string TickPrintedTicketPreImg
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPrintedTicketPreImg2 in the schema.
        /// </summary>
        /// 
        public string TickPrintedTicketPreImg2
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPrintedTicketPreImg3 in the schema.
        /// </summary>
        /// 
        public string TickPrintedTicketPreImg3
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPrintedTicketPreImg4 in the schema.
        /// </summary>
        /// 
        public string TickPrintedTicketPreImg4
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPrintedTicketOffset in the schema.
        /// </summary>
        /// 

        public string TickPrintedTicketOffset
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPrintedTicketOffset2 in the schema.
        /// </summary>
        /// 
        public string TickPrintedTicketOffset2
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPrintedTicketOffset3 in the schema.
        /// </summary>
        /// 
        public string TickPrintedTicketOffset3
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickPrintedTicketOffset4 in the schema.
        /// </summary>
        /// 
        public string TickPrintedTicketOffset4
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickNumberExt in the schema.
        /// </summary>
        /// 
        public string TickNumberExt
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickStatusExt in the schema.
        /// </summary>
        /// 
        public string TickStatusExt
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickAmountOverride in the schema.
        /// </summary>
        /// 
        public long? TickAmountOverride
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickIsPayable in the schema.
        /// </summary>
        /// 
        public bool? TickIsPayable
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TickWithContextPhoto in the schema.
        /// </summary>
        /// 
        public bool? TickWithContextPhoto
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for AddressSideType in the schema.
        /// </summary>
        /// 
        public long? AddressSideTypeID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for Currency in the schema.
        /// </summary>
        /// 
        public long? CurrencyID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for EnforcQuery in the schema.
        /// </summary>
        /// 
        public long? EnforcQueryID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for ExternalProvider in the schema.
        /// </summary>
        /// 
        public long? ExternalProviderID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for Group in the schema.
        /// </summary>
        /// 
        public long? GroupID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for InstallationPaymentSource in the schema.
        /// </summary>
        /// 
        public long? InstallationPaymentSourceID
        {
            get;
            set;
        }


        /// <summary>
        /// 
        /// 
        /// There are no comments for Installation in the schema.
        /// </summary>
        /// 
        public long? InstallationID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for MobileDevice in the schema.
        /// </summary>
        /// 
        public long? MobileDeviceID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for MobileSession in the schema.
        /// </summary>
        /// 
        public long? MobileSessionID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for Pdm in the schema.
        /// </summary>
        /// 
        public long? PdmID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for RemitGroup in the schema.
        /// </summary>
        /// 
        public long? RemitGroupID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for Street_TICK_STR_ID in the schema.
        /// </summary>
        /// 
        public long? Street_TICK_STR_ID_ID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for Street_TICK_STR_ID_TO in the schema.
        /// </summary>
        /// 
        public long? Street_TICK_STR_ID_TO_ID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for Street_TICK_STR_ID_FROM in the schema.
        /// </summary>
        /// 
        public long? Street_TICK_STR_ID_FROM_ID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for Street_TICK_INTERSECTION_STR_ID in the schema.
        /// </summary>
        public long? Street_TICK_INTERSECTION_STR_ID_ID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for StreetsSection in the schema.
        /// </summary>
        /// 
        public long? StreetsSectionID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TicketCancellations in the schema.
        /// </summary>
        /// 
        public long? TicketCancellationsiD
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TicketLocationSign in the schema.
        /// </summary>
        /// 
        public long? TicketLocationSigniD
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TicketNotifications in the schema.
        /// </summary>
        /// 
        public long? TicketNotificationsID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TicketPrivateNotes in the schema.
        /// </summary>
        /// 
        public long? TicketPrivateNotesID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TicketPublicNotes in the schema.
        /// </summary>
        /// 
        public long? TicketPublicNotesID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TicketSituation in the schema.
        /// </summary>
        /// 
        public long? TicketSituationID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TicketStatus in the schema.
        /// </summary>
        /// 
        public long? TicketStatusID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TicketStatus in the schema.
        /// </summary>
        /// 
        public string TicketStatus
        {
            get;
            set;
        }

        /// <summary>
        /// There are no comments for TicketType in the schema.
        /// </summary>
        /// 
        public long TicketTypeID
        {
            get;
            set;
        }

        public string TicketTypeDesc
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TowingVehicleService in the schema.
        /// </summary>
        /// 
        public long? TowingVehicleServiceID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for User in the schema.
        /// </summary>
        /// 
        public long? UserID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for VehicleBrand in the schema.
        /// </summary>
        /// 
        public long? VehicleBrandID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for VehicleColor in the schema.
        /// </summary>
        /// 
        public long? VehicleColorID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for VehicleEntity in the schema.
        /// </summary>
        /// 
        public long? VehicleEntityID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for VehicleModel in the schema.
        /// </summary>
        /// 
        public long? VehicleModelID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for Vehicle in the schema.
        /// </summary>
        /// 
        public long? VehicleID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for VehicleType in the schema.
        /// </summary>
        /// 
        public long? VehicleTypeID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TicketsEnforcQueriesHis in the schema.
        /// </summary>
        /// 
        public long? TicketsEnforcQueriesHisID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TicketsImages in the schema.
        /// </summary>
        /// 
        public long? TicketsImagesID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TicketsSyncs in the schema.
        /// </summary>
        /// 
        public long? TicketsSyncsID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TicketAppeals in the schema.
        /// </summary>
        /// 
        public long? TicketAppealsID
        {
            get;
            set;
        }


        /// <summary>
        /// There are no comments for TicketAppealImages in the schema.
        /// </summary>
        /// 
        public long? TicketAppealImagesID
        {
            get;
            set;
        }
    }

    [Serializable()]
    public class TicketImageReturnModel
    {
        #region Properties

        public string ImagePath { get; set; }

        public string ImageName { get; set; }

        public long? TicketImageId { get; set; }

        public string image { get; set; }

        public string thumbnail { get; set; }
        public string base64 { get; set; }
        public string pdfticket { get; set; }
        public long? TicketImageNumber { get; set; }

        public long? TicketImageType { get; set; }

        public long? TicketId { get; set; }

        #endregion
    }
    #endregion


    #region new save appeal model 
    [Serializable()]
    public class AppealReturnModel
    {
        public long? TicketId { get; set; }

        public string ticketnumber { get; set; }

        public TicketReturnModel SearchedTicket { get; set; }

        public List<TicketReturnModel> AllTickets { get; set; }

        public List<TicketImageReturnModel> TicketsImages { get; set; }
        public bool isChecked { get; set; }

        public List<TicketReturnModel> SelectedTickets { get; set; }


        public TicketAppealData AppealData { get; set; }


        

        [DataType(DataType.Text)]
        public string Plate { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.EmailAddress)]
        public string ConfirmEmail { get; set; }

        public int InstallationId { get; set; }
        public int Total { get; set; }
        public int Result { get; set; }
        public int NumDecimals { get; set; }
        public DateTime TicketDate { get; set; }
        public string FineDetails { get; set; }
        public string AmountCurrencyIsoCode { get; set; }
        public string InstallationShortDesc { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public decimal CurrencyId { get; set; }
        public DateTime PriceCalculationDate { get; set; }
        public int GrpId { get; set; }
        public string AuthId { get; set; }

        public decimal? ForceInstallationId { get; set; }
        public bool BlockInstallationCombo { get; set; }
        public string ForceCulture { get; set; }
        public string ForceTicketNumber { get; set; }

        [DataType(DataType.Text)]
        public string InstallationList { get; set; }
        public string StandardInstallationList { get; set; }


        [Serializable()]
        public class TicketAppealData
        {
            #region Properties

            public string TicketNumber { get; set; }

            public long? StreetId { get; set; }

            public string StreetNumber { get; set; }

            public long? ModelId { get; set; }

            public long? BrandId { get; set; }

            public long? ColorId { get; set; }

            public long? EntityId { get; set; }

            public long? VehiculeTypeId { get; set; }

            public long? TicketTypeId { get; set; }

            public List<TicketImageModel> Images { get; set; }
            public List<AppealImages> AppealImageList { get; set; }

            public List<TicketImageReturnModel> AppealImageIds { get; set; }
            public long? StreetIdFrom { get; set; }

            public string StreetNumberFrom { get; set; }

            public long? StreetIdTo { get; set; }

            public string StreetNumberTo { get; set; }

            public long? TicketIsSentbyPostalmail { get; set; }


            public int StatusId { get; set; }

            public int UserId { get; set; }

            public string PlateNumber { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Address { get; set; }

            public string Email { get; set; }

            public string AppealReason { get; set; }

            public string AppealDesc { get; set; }

            public string ImagesUrl { get; set; }
            public string Device { get; set; }

            [Serializable()]
            public class TicketImageModel
            {
                #region Properties

                public int Number { get; set; }

                public string Name { get; set; }

                #endregion
            }

            [Serializable()]
            public class AppealImages
            {
                #region Properties

                public string ticketnumber { get; set; }

                public string cityid { get; set; }

                public string imagename { get; set; }
                public long fileSize { get; set; }
                public string date { get; set; }

                [StringLengthAttribute(int.MaxValue)]
                public string image { get; set; }

                public int? Retry { get; set; }

                public int? checkIfExist { get; set; }

                #endregion
            }

            #endregion
        }
    }








    #endregion



}
