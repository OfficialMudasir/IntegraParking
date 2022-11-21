using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;

namespace integraMobile.Infrastructure
{
    public class IECISAPayments : IPayments
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(IECISAPayments));
        private static Random m_oRandom = new Random(Convert.ToInt32(DateTime.UtcNow.Ticks % Int32.MaxValue));

        public enum IECISAErrorCode
        {
            OK = 0,
            ValidationError = 1,
            TransactionNotFound = 2,
            PaymentIdNotFound = 3,
            TransactionTypeNotSupported = 4,
            TransactionCancelled = 5,
            TransactionNotCompleted = 6,
            PaymentMeanConfigurationError = 7,
            TokenNotFound = 8,
            PaymentMeanNotRespond = 9,
            TransactionAlreadyUsed = 10,
            RequestNotApplyCurrentTrans = 11,
            TransactionCannotBeRecovered = 12,
            PaymentMeansNotHaveValidToken = 13,
            OriginalTransactionNotFound = 14,
            OriginalTransactionNotCompleted = 15,
            OriginalTransactionRejectedByPaymentMean = 16,
            OriginalAmountLessAmountReturned = 17,
            PaymentMeanNotAcceptRefunds = 18,
            PaymentMeanNotAcceptPartialRefunds = 19,
            TransactionAlreadyCancelled = 20,
            InvalidMACInternal = 81,
            TransactionCancelled2 = 99,
            TokenNotFound2 = 210,
            NotAvailable = 212,
            InternalError = 500,
            BalanceTestCompletedSuccesfully = 543,
            FunctionCompletedSuccesfully = 600,
            AuthFailed = 10002,
            VersionError = 10006,
            SecurityError = 10008,
            MissingParameter = 10014,
            InvalidParameter = 10101,
            MethodUnspecified = 10203,
            ConnectionFailed = 20001,
            EmptyPaymentTypeList = 20002,
            InvalidMAC = 20003,

        }


        private Dictionary<IECISAErrorCode, string> ErrorMessageDict = new Dictionary<IECISAErrorCode, string>()
        {
            {IECISAErrorCode.OK,"Transaction completed successfully (OK)."},
            {IECISAErrorCode.ValidationError,"Validation error."},
            {IECISAErrorCode.TransactionNotFound,"Transaction not found."},
            {IECISAErrorCode.PaymentIdNotFound,"Mean of payment identifier not found."},
            {IECISAErrorCode.TransactionTypeNotSupported,"Transaction type not supported for the mean of payment."},
            {IECISAErrorCode.TransactionCancelled,"Transaction cancelled."},
            {IECISAErrorCode.TransactionNotCompleted,"Transaction not completed."},
            {IECISAErrorCode.PaymentMeanConfigurationError,"Mean of payment configuration error."},
            {IECISAErrorCode.TokenNotFound,"Token not found."},
            {IECISAErrorCode.PaymentMeanNotRespond,"The mean of payment does not respond."},
            {IECISAErrorCode.TransactionAlreadyUsed,"The transaction has already been used and is not valid."},
            {IECISAErrorCode.RequestNotApplyCurrentTrans,"The request does not apply to the current transaction."},
            {IECISAErrorCode.TransactionCannotBeRecovered,"The transaction cannot be recovered."},
            {IECISAErrorCode.PaymentMeansNotHaveValidToken,"The means of payment does not have a valid token for the indicated agreement."},
            {IECISAErrorCode.OriginalTransactionNotFound,"Original transaction not found."},
            {IECISAErrorCode.OriginalTransactionNotCompleted,"Original transaction not completed."},
            {IECISAErrorCode.OriginalTransactionRejectedByPaymentMean,"Original transaction rejected by the mean of payment."},
            {IECISAErrorCode.OriginalAmountLessAmountReturned,"The amount of the original transaction is less than the amount returned / cancelled."},
            {IECISAErrorCode.PaymentMeanNotAcceptRefunds,"The mean of payment does not accept refunds."},
            {IECISAErrorCode.PaymentMeanNotAcceptPartialRefunds,"The mean of payment does not accept partial refunds. "},
            {IECISAErrorCode.TransactionAlreadyCancelled,"The transaction has already been cancelled."},
            {IECISAErrorCode.TransactionCancelled2,"Transaction cancelled. "},
            {IECISAErrorCode.TokenNotFound2,"Token not found."},
            {IECISAErrorCode.NotAvailable,"Not available. "},
            {IECISAErrorCode.InternalError,"Internal error."},
            {IECISAErrorCode.BalanceTestCompletedSuccesfully,"Balance test completed successfully. "},
            {IECISAErrorCode.FunctionCompletedSuccesfully,"Function completed successfully."},
            {IECISAErrorCode.AuthFailed,"Authorization / Authentication failed. "},
            {IECISAErrorCode.VersionError,"Version error."},
            {IECISAErrorCode.SecurityError,"Security error. "},
            {IECISAErrorCode.MissingParameter,"Missing parameter. "},
            {IECISAErrorCode.InvalidParameter,"Invalid parameter. "},
            {IECISAErrorCode.MethodUnspecified,"Method unspecified"},
            {IECISAErrorCode.ConnectionFailed,"Connection Failed"},
            {IECISAErrorCode.EmptyPaymentTypeList,"List of payment types is empty"},
            {IECISAErrorCode.InvalidMAC,"Invalid MAC"},
        };

        public bool StartWebTransaction(string strFormatId,
                                       string strIECISAUser,
                                       string strIECISAMerchantID,
                                       string strIECISAInstance,
                                       string strIECISACentreID,
                                       string strIECISAPosID,
                                       string strIECISAServiceURL,
                                       int iIECISAServiceTimeout,
                                       string strIECISAMACKey,
                                       string strIECISATemplate,
                                       string strIECISACustomerID,
                                       string strAcceptURL,
                                       string strCancelURL,
                                       string strEmail,
                                       string strLang,
                                       int iQuantity,
                                       string strCurISOCode,
                                       string strCurNumISOCode,
                                       bool bCreateToken,
                                       DateTime dtNow,
                                       out IECISAErrorCode eErrorCode,
                                       out string errorMessage,                                    
                                       out string strTransactionId,
                                       out string strOpReference,
                                       out string strCardHash)

        {
            bool bRes = false;
            strTransactionId = null;
            long lEllapsedTime = -1;
            Stopwatch watch = null;
            eErrorCode = IECISAErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            strOpReference = "";
            strCardHash = "";

            AddTLS12Support();

            try
            {
                string strUserReference = UserReference();
                string strURL = strIECISAServiceURL + "/Transaction";
                WebRequest request = WebRequest.Create(strURL);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = iIECISAServiceTimeout;

                Dictionary<string, object> oTransactionRequest = new Dictionary<string, object>();
                Dictionary<string, object> oCardRequestInfo = new Dictionary<string, object>();
                
                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime dtSpain = TimeZoneInfo.ConvertTime(dtNow, TimeZoneInfo.Local, tzi);
               
                oTransactionRequest["Login"] = strIECISAUser;
                oTransactionRequest["TransactionType"] = "V";
                oTransactionRequest["Company"] = strIECISAMerchantID;
                oTransactionRequest["Instance"] = strIECISAInstance;
                oTransactionRequest["AcceptUrl"] = strAcceptURL;
                oTransactionRequest["CancelUrl"] = strCancelURL;
                oTransactionRequest["MerchantTransactionId"] = strUserReference;
                oTransactionRequest["TransactionAmount"] = iQuantity;
                oTransactionRequest["TransactionCurrency"] = strCurNumISOCode;
                oTransactionRequest["CreateToken"] = bCreateToken ? 1 : 0;

                oCardRequestInfo["CF_User"] = strIECISAUser;
                oCardRequestInfo["CF_Date"] = dtSpain.ToString("ddMMyyyy");
                oCardRequestInfo["CF_Time"] = dtSpain.ToString("HHmmss");
                oCardRequestInfo["CF_Lang"] = strLang.ToUpper();
                oCardRequestInfo["CF_Template"] = strIECISATemplate;
                oCardRequestInfo["CF_XtnType"] = "V";
                oCardRequestInfo["Company"] = strIECISAMerchantID;
                oCardRequestInfo["Center"] = strIECISACentreID;
                oCardRequestInfo["Pos"] = strIECISAPosID;
                oCardRequestInfo["CF_TicketNumber"] = strUserReference;
                oCardRequestInfo["CF_Amount"] = iQuantity;
                oCardRequestInfo["CF_Currency"] = strCurNumISOCode;
                oCardRequestInfo["CF_CurrencyCode"] = strCurISOCode;
                strCardHash = TokenReference(strEmail);
                oCardRequestInfo["CF_Ref_Token_Cli"] = strCardHash;
                oCardRequestInfo["CF_TokenUpdate"] = bCreateToken ? 1 : 0;


                if (strFormatId == "02")              
                {
                    oCardRequestInfo["CF_FmtVer"] = strFormatId;
                    oCardRequestInfo["CF_CustomerID"] = strIECISACustomerID;

                    oCardRequestInfo["CF_MAC"] = HashCodeFMT2(strIECISAMACKey, ((string)oCardRequestInfo["CF_FmtVer"]) +
                                                                    ((string)oCardRequestInfo["CF_CustomerID"]) +
                                                                    ((string)oCardRequestInfo["CF_User"]) +
                                                                    ((string)oCardRequestInfo["CF_TicketNumber"]) +
                                                                    ((string)oCardRequestInfo["CF_Date"]) +
                                                                    ((string)oCardRequestInfo["CF_Time"]) +
                                                                    ((string)oCardRequestInfo["CF_XtnType"]) +
                                                                    (Convert.ToString(oCardRequestInfo["CF_Amount"])) +
                                                                    ((string)oCardRequestInfo["CF_Currency"]) +
                                                                    ((string)oCardRequestInfo["CF_Template"]) +
                                                                    (Convert.ToString(oCardRequestInfo["CF_TokenUpdate"])) +
                                                                    ((string)oCardRequestInfo["CF_Ref_Token_Cli"]));
                }
                else
                {
                    oCardRequestInfo["CF_MAC"] = HashCodeFMT1(strIECISAMACKey, ((string)oCardRequestInfo["CF_XtnType"]) +
                            ((string)oCardRequestInfo["CF_User"]) +
                            ((string)oCardRequestInfo["CF_Date"]) +
                            ((string)oCardRequestInfo["CF_Time"]) +
                            (Convert.ToString(oCardRequestInfo["CF_Amount"])) +
                            ((string)oCardRequestInfo["CF_Currency"]) +
                            ((string)oCardRequestInfo["CF_TicketNumber"]) +
                            ((string)oCardRequestInfo["CF_Lang"]));

                }

               
                oTransactionRequest["CardRequestInfo"] = oCardRequestInfo;

                var json = JsonConvert.SerializeObject(oTransactionRequest);

                Logger_AddLogMessage(string.Format("StartWebTransaction request.url={0}, request.json={1}", strURL, PrettyJSON(json)), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                watch = Stopwatch.StartNew();

                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();


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

                        Logger_AddLogMessage(string.Format("StartWebTransaction response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.

                        
                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        try
                        {
                            eErrorCode = (IECISAErrorCode)Convert.ToInt32(oResponse["ResultCode"]);
                        }
                        catch
                        {
                            eErrorCode = IECISAErrorCode.InternalError;
                        }

                        errorMessage = oResponse["ResultDescription"].ToString();

                        if (eErrorCode == IECISAErrorCode.OK)
                        {
                            strTransactionId = oResponse["TransactionId"].ToString();
                            strOpReference = strUserReference;
                            bRes = true;

                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("StartWebTransaction Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "StartWebTransaction::Exception", LogLevels.logERROR);
                    eErrorCode = IECISAErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }


                lEllapsedTime = watch.ElapsedMilliseconds;

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                Logger_AddLogException(e, "StartWebTransaction::Exception", LogLevels.logERROR);

            }

            return bRes;
        }


        public bool StartAutomaticTransaction(string strFormatId,
                                       string strIECISAUser,
                                       string strIECISAMerchantID,
                                       string strIECISAInstance,
                                       string strIECISACentreID,
                                       string strIECISAPosID,
                                       string strIECISAServiceURL,
                                       int iIECISAServiceTimeout,
                                       string strIECISAMACKey,
                                       string strIECISACustomerID,
                                       string strIECISATemplate,
                                       string strCardReference,
                                       string strEmail,
                                       int iQuantity,
                                       string strCurISOCode,
                                       string strCurNumISOCode,
                                       DateTime dtNow,
                                       out IECISAErrorCode eErrorCode,
                                       out string errorMessage,
                                       out string strTransactionId,
                                       out string strOpReference)
        {
            bool bRes = false;
            strTransactionId = null;
            long lEllapsedTime = -1;
            Stopwatch watch = null;
            eErrorCode = IECISAErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            strOpReference = null;

            AddTLS12Support();


            try
            {
                string strUserReference = UserReference();
                string strURL = strIECISAServiceURL + "/Transaction";
                WebRequest request = WebRequest.Create(strURL);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = iIECISAServiceTimeout;

                Dictionary<string, object> oTransactionRequest = new Dictionary<string, object>();
                Dictionary<string, object> oCardRequestInfo = new Dictionary<string, object>();

                oTransactionRequest["Login"] = strIECISAUser;
                oTransactionRequest["TransactionType"] = "V";
                oTransactionRequest["Company"] = strIECISAMerchantID;
                oTransactionRequest["Instance"] = strIECISAInstance;
                oTransactionRequest["MerchantTransactionId"] = strUserReference;
                oTransactionRequest["TransactionAmount"] = iQuantity;
                oTransactionRequest["TransactionCurrency"] = strCurNumISOCode;
                oTransactionRequest["CreateToken"] =  0;
                oTransactionRequest["Token"] = strCardReference;
                oTransactionRequest["AcceptUrl"] = "/NOTEMPTY";
                oTransactionRequest["CancelUrl"] = "/NOTEMPTY";


                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime dtSpain = TimeZoneInfo.ConvertTime(dtNow, TimeZoneInfo.Local, tzi);

                oCardRequestInfo["CF_User"] = strIECISAUser;
                oCardRequestInfo["CF_Date"] = dtSpain.ToString("ddMMyyyy");
                oCardRequestInfo["CF_Time"] = dtSpain.ToString("HHmmss");
                oCardRequestInfo["CF_Lang"] = "ES";
                oCardRequestInfo["CF_XtnType"] = "V";
                oCardRequestInfo["Company"] = strIECISAMerchantID;
                oCardRequestInfo["Center"] = strIECISACentreID;
                oCardRequestInfo["Pos"] = strIECISAPosID;
                oCardRequestInfo["CF_TicketNumber"] = strUserReference;
                oCardRequestInfo["CF_Amount"] = iQuantity;
                oCardRequestInfo["CF_Currency"] = strCurNumISOCode;
                oCardRequestInfo["CF_CurrencyCode"] = strCurISOCode;
                oCardRequestInfo["CF_Ref_Token_Cli"] = TokenReference(strEmail);
                oCardRequestInfo["CF_TokenUpdate"] =  0;
                oCardRequestInfo["CF_Token"] = strCardReference;
                oCardRequestInfo["Support"] = "K";
                oCardRequestInfo["Document"] = strCardReference;
                oCardRequestInfo["CF_Installment"] = "000";

                /*
                */


                if (strFormatId == "02")
                {
                    oCardRequestInfo["CF_FmtVer"] = strFormatId;
                    oCardRequestInfo["CF_CustomerID"] = strIECISACustomerID;
                    oCardRequestInfo["CF_Template"] = strIECISATemplate;



                    oCardRequestInfo["CF_MAC"] = HashCodeFMT2(strIECISAMACKey, ((string)oCardRequestInfo["CF_FmtVer"]) +
                                                                    ((string)oCardRequestInfo["CF_CustomerID"]) +
                                                                    ((string)oCardRequestInfo["CF_User"]) +
                                                                    ((string)oCardRequestInfo["CF_TicketNumber"]) +
                                                                    ((string)oCardRequestInfo["CF_Date"]) +
                                                                    ((string)oCardRequestInfo["CF_Time"]) +
                                                                    (Convert.ToString(oCardRequestInfo["CF_Amount"])) +
                                                                    ((string)oCardRequestInfo["CF_Currency"]) +
                                                                    ((string)oTransactionRequest["Token"]) +
                                                                    ((string)oCardRequestInfo["CF_Installment"]));
                }
                else
                {
                    oCardRequestInfo["CF_MAC"] = HashCodeFMT1(strIECISAMACKey, ((string)oCardRequestInfo["CF_XtnType"]) +
                                   ((string)oCardRequestInfo["Company"]) +
                                   ((string)oCardRequestInfo["Center"]) +
                                   ((string)oCardRequestInfo["Pos"]) +
                                   ((string)oCardRequestInfo["CF_Date"]) +
                                   ((string)oCardRequestInfo["CF_Time"]) +
                                   ((string)oCardRequestInfo["Support"]) +
                                   ((string)oCardRequestInfo["Document"]) +
                                   (Convert.ToString(oCardRequestInfo["CF_Amount"])) +
                                   ((string)oCardRequestInfo["CF_CurrencyCode"]) +
                                   ((string)oCardRequestInfo["CF_Installment"]) +
                                   ((string)oCardRequestInfo["CF_TicketNumber"]));

                }


                oTransactionRequest["CardRequestInfo"] = oCardRequestInfo;

                var json = JsonConvert.SerializeObject(oTransactionRequest);

                Logger_AddLogMessage(string.Format("StartAutomaticTransaction request.url={0}, request.json={1}", strURL, PrettyJSON(json)), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                watch = Stopwatch.StartNew();

                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();


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

                        Logger_AddLogMessage(string.Format("StartAutomaticTransaction response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.


                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        try
                        {
                            eErrorCode = (IECISAErrorCode)Convert.ToInt32(oResponse["ResultCode"]);
                        }
                        catch
                        {
                            eErrorCode = IECISAErrorCode.InternalError;
                        }

                        errorMessage = oResponse["ResultDescription"].ToString();

                        if (eErrorCode == IECISAErrorCode.OK)
                        {
                            strTransactionId = oResponse["TransactionId"].ToString();
                            strOpReference = strUserReference;
                            bRes = true;

                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("StartAutomaticTransaction Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "StartAutomaticTransaction::Exception", LogLevels.logERROR);
                    eErrorCode = IECISAErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }


                lEllapsedTime = watch.ElapsedMilliseconds;

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                Logger_AddLogException(e, "StartAutomaticTransaction::Exception", LogLevels.logERROR);

            }

            return bRes;
        }


        public bool GetWebTransactionPaymentTypes(string strIECISAServiceURL,
                                                  int iIECISAServiceTimeout,
                                                  string strTransactionId,
                                                  out IECISAErrorCode eErrorCode,
                                                  out string errorMessage,
                                                  out string strRedirectURL)
        {
            bool bRes = false;
            long lEllapsedTime = -1;
            Stopwatch watch = null;
            eErrorCode = IECISAErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            strRedirectURL = "";

            AddTLS12Support();

            try
            {
                string strUserReference = UserReference();
                string strURL = strIECISAServiceURL + "/PaymentTypes/" + strTransactionId;
                WebRequest request = WebRequest.Create(strURL);

                request.Method = "GET";
                request.ContentType = "application/json";
                request.Timeout = iIECISAServiceTimeout;

                Logger_AddLogMessage(string.Format("GetWebTransactionPaymentTypes request.url={0}", strURL), LogLevels.logINFO);

                watch = Stopwatch.StartNew();


                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        Stream dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.

                        Logger_AddLogMessage(string.Format("GetWebTransactionPaymentTypes response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.


                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        try
                        {
                            eErrorCode = (IECISAErrorCode)Convert.ToInt32(oResponse["ResultCode"]);
                        }
                        catch
                        {
                            eErrorCode = IECISAErrorCode.InternalError;
                        }

                        errorMessage = oResponse["ResultDescription"].ToString();

                        if (eErrorCode == IECISAErrorCode.OK)
                        {
                            try
                            {
                                if (oResponse["PaymentTypes"][0]["Code"].ToString().ToUpper() == "BANKCARD")
                                {
                                    strRedirectURL = oResponse["PaymentTypes"][0]["Url"].ToString();
                                    bRes = true;
                                }
                            }
                            catch (Exception e)
                            {
                                Logger_AddLogException(e, "GetWebTransactionPaymentTypes::Exception", LogLevels.logERROR);
                                eErrorCode = IECISAErrorCode.EmptyPaymentTypeList;
                                errorMessage = ErrorMessageDict[eErrorCode];

                            }
                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("GetWebTransactionPaymentTypes Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "GetWebTransactionPaymentTypes::Exception", LogLevels.logERROR);
                    eErrorCode = IECISAErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }


                lEllapsedTime = watch.ElapsedMilliseconds;

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                Logger_AddLogException(e, "GetWebTransactionPaymentTypes::Exception", LogLevels.logERROR);

            }

            return bRes;
        }


        public bool CompleteAutomaticTransaction(string strIECISAServiceURL,
                                          int iIECISAServiceTimeout,
                                          string strIECISAMACKey,
                                          string strTransactionId,
                                          out IECISAErrorCode eErrorCode,
                                          out string errorMessage,                                         
                                          out DateTime? dtTransactionDate,
                                          out string strCFTransactionID,
                                          out string strAuthCode)
        {
            bool bRes = false;
            long lEllapsedTime = -1;
            Stopwatch watch = null;
            eErrorCode = IECISAErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            dtTransactionDate = null;
            strAuthCode = "";
            strCFTransactionID = "";


            AddTLS12Support();


            try
            {
                string strUserReference = UserReference();
                string strURL = string.Format("{0}/DoPay/{1}/BANKCARD",strIECISAServiceURL,strTransactionId);
                WebRequest request = WebRequest.Create(strURL);

                request.Method = "GET";
                request.ContentType = "application/json";
                request.Timeout = iIECISAServiceTimeout;

                Logger_AddLogMessage(string.Format("CompleteAutomaticTransaction request.url={0}", strURL), LogLevels.logINFO);

                watch = Stopwatch.StartNew();


                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        Stream dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.

                        Logger_AddLogMessage(string.Format("CompleteAutomaticTransaction response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.


                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        try
                        {
                            eErrorCode = (IECISAErrorCode)Convert.ToInt32(oResponse["ResultCode"]);
                        }
                        catch
                        {
                            eErrorCode = IECISAErrorCode.InternalError;
                        }

                        errorMessage = oResponse["ResultDescription"].ToString();

                        if (eErrorCode == IECISAErrorCode.OK)
                        {

                            string strXtnType = oResponse["CardResultInfo"]["CF_XtnType"];
                            string strUser = oResponse["CardResultInfo"]["CF_User"];
                            string strOpReference = oResponse["CardResultInfo"]["CF_TicketNumber"];
                            strCFTransactionID = oResponse["CardResultInfo"]["CF_TransactionID"];
                            eErrorCode = (IECISAErrorCode)Convert.ToInt32(oResponse["CardResultInfo"]["CF_Result"]);
                            errorMessage = oResponse["CardResultInfo"]["CF_Des_Result"];
                            string strDocType = oResponse["CardResultInfo"]["CF_Des_DocType"];
                            string strProcessor = oResponse["CardResultInfo"]["CF_Des_Processor"];
                            string strMercharnt = oResponse["CardResultInfo"]["CF_Des_Merchant"];
                            strAuthCode = oResponse["CardResultInfo"]["CF_NumAut"];
                            string strBidStore = oResponse["CardResultInfo"]["CF_BIDStore"];
                            string strBidPos = oResponse["CardResultInfo"]["CF_BIDPos"];
                            string strBidTransaction = oResponse["CardResultInfo"]["CF_BIDTransaction"];
                            string strMAC = oResponse["CardResultInfo"]["CF_MAC"];

                            string strCalcMAC = IECISAPayments.HashCodeFMT1(strIECISAMACKey, strXtnType +
                                                                                      strUser +
                                                                                      strOpReference +
                                                                                      strCFTransactionID +
                                                                                      oResponse["CardResultInfo"]["CF_Result"].ToString() +
                                                                                      errorMessage +
                                                                                      strDocType +
                                                                                      strProcessor +
                                                                                      strMercharnt +
                                                                                      strAuthCode +
                                                                                      strBidStore +
                                                                                      strBidPos +
                                                                                      strBidTransaction);

                            /*if (strMAC != strCalcMAC)
                            {
                                eErrorCode = IECISAErrorCode.InvalidMAC;
                                errorMessage = ErrorMessageDict[eErrorCode];

                            }
                            else
                            {*/


                                if (string.IsNullOrEmpty(errorMessage))
                                {
                                    errorMessage = ErrorMessageDict[eErrorCode];
                                }

                                string strTime = oResponse["CardResultInfo"]["CF_Time"];
                                string strDate = oResponse["CardResultInfo"]["CF_Date"];


                                if ((strTime.Length == 6) && (strDate.Length == 8))
                                {
                                    try
                                    {
                                        dtTransactionDate = DateTime.ParseExact(strTime + strDate, "HHmmssddMMyyyy",
                                                      CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        dtTransactionDate = DateTime.Now;
                                    }
                                }

                                bRes = true;
                            //}

                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("CompleteAutomaticTransaction Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "CompleteAutomaticTransaction::Exception", LogLevels.logERROR);
                    eErrorCode = IECISAErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }


                lEllapsedTime = watch.ElapsedMilliseconds;

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                Logger_AddLogException(e, "CompleteAutomaticTransaction::Exception", LogLevels.logERROR);

            }

            return bRes;
        }

        public bool GetTransactionStatus(string strIECISAServiceURL,
                                          int iIECISAServiceTimeout,
                                          string strIECISAMACKey,
                                          string strTransactionId,
                                          out IECISAErrorCode eErrorCode,
                                          out string errorMessage,
                                          out string strMaskedCardNumber,
                                          out string strCardReference,
                                          out DateTime? dtExpDate,
                                          out string strExpMonth,
                                          out string strExpYear,
                                          out DateTime? dtTransactionDate,
                                          out string strOpReference,
                                          out string strCFTransactionID,
                                          out string strAuthCode)
        {
            bool bRes = false;
            long lEllapsedTime = -1;
            Stopwatch watch = null;
            eErrorCode = IECISAErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            strMaskedCardNumber = "";
            strCardReference = "";
            dtExpDate = null;
            dtTransactionDate = null;
            strAuthCode = "";
            strOpReference = "";
            strCFTransactionID = "";
            strExpMonth = "";
            strExpYear = "";

            AddTLS12Support();



            try
            {
                string strUserReference = UserReference();
                string strURL = strIECISAServiceURL + "/TransactionResult/" + strTransactionId;
                WebRequest request = WebRequest.Create(strURL);

                request.Method = "GET";
                request.ContentType = "application/json";
                request.Timeout = iIECISAServiceTimeout;

                Logger_AddLogMessage(string.Format("GetTransactionStatus request.url={0}", strURL), LogLevels.logINFO);

                watch = Stopwatch.StartNew();


                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        Stream dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.

                        Logger_AddLogMessage(string.Format("GetTransactionStatus response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.


                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        try
                        {
                            eErrorCode = (IECISAErrorCode)Convert.ToInt32(oResponse["ResultCode"]);
                        }
                        catch
                        {
                            eErrorCode = IECISAErrorCode.InternalError;
                        }

                        errorMessage = oResponse["ResultDescription"].ToString();

                        if (eErrorCode == IECISAErrorCode.OK)
                        {

                            string strXtnType = oResponse["CardResultInfo"]["CF_XtnType"];
                            string strUser = oResponse["CardResultInfo"]["CF_User"];
                            strOpReference = oResponse["CardResultInfo"]["CF_TicketNumber"];
                            strCFTransactionID = oResponse["CardResultInfo"]["CF_TransactionID"];
                            eErrorCode = (IECISAErrorCode)Convert.ToInt32(oResponse["CardResultInfo"]["CF_Result"]);
                            errorMessage = oResponse["CardResultInfo"]["CF_Des_Result"];
                            string strDocType = oResponse["CardResultInfo"]["CF_Des_DocType"];
                            string strProcessor = oResponse["CardResultInfo"]["CF_Des_Processor"];
                            string strMercharnt = oResponse["CardResultInfo"]["CF_Des_Merchant"];
                            strAuthCode = oResponse["CardResultInfo"]["CF_NumAut"];
                            string strBidStore = oResponse["CardResultInfo"]["CF_BIDStore"];
                            string strBidPos = oResponse["CardResultInfo"]["CF_BIDPos"];
                            string strBidTransaction = oResponse["CardResultInfo"]["CF_BIDTransaction"];
                            string strMAC = oResponse["CardResultInfo"]["CF_MAC"];

                            string strCalcMAC = IECISAPayments.HashCodeFMT1(strIECISAMACKey, strXtnType +
                                                                                      strUser +
                                                                                      strOpReference +
                                                                                      strCFTransactionID +
                                                                                      oResponse["CardResultInfo"]["CF_Result"].ToString() +
                                                                                      errorMessage +
                                                                                      strDocType +
                                                                                      strProcessor +
                                                                                      strMercharnt +
                                                                                      strAuthCode +
                                                                                      strBidStore +
                                                                                      strBidPos +
                                                                                      strBidTransaction);

                            /*if (strMAC != strCalcMAC)
                            {
                                eErrorCode = IECISAErrorCode.InvalidMAC;
                                errorMessage = ErrorMessageDict[eErrorCode];

                            }
                            else
                            {*/


                                if (string.IsNullOrEmpty(errorMessage))
                                {
                                    errorMessage = ErrorMessageDict[eErrorCode];
                                }
                                strMaskedCardNumber = oResponse["CardResultInfo"]["CF_PAN"];
                                strCardReference = oResponse["CardResultInfo"]["CF_Token"];
                                string strExpDate = oResponse["CardResultInfo"]["CF_ExpirationDate"];

                                string strTime = oResponse["CardResultInfo"]["CF_Time"];
                                string strDate = oResponse["CardResultInfo"]["CF_Date"];


                                if (!string.IsNullOrEmpty(strExpDate))
                                {
                                    if (strExpDate.Length == 4)
                                    {
                                        strExpMonth = strExpDate.Substring(0, 2);
                                        strExpYear = strExpDate.Substring(2, 2);
                                        dtExpDate = new DateTime(Convert.ToInt32(strExpYear) + 2000,
                                                                 Convert.ToInt32(strExpMonth), 1).AddMonths(1).AddSeconds(-1);
                                    }
                                }

                                if ((strTime.Length == 6)&&(strDate.Length == 8))
                                {
                                    try
                                    {
                                        dtTransactionDate = DateTime.ParseExact(strTime + strDate, "HHmmssddMMyyyy",
                                                      CultureInfo.InvariantCulture);
                                    }
                                    catch
                                    {
                                        dtTransactionDate = DateTime.Now;
                                    }
                                }

                                bRes = true;
                            //}

                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("GetTransactionStatus Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "GetTransactionStatus::Exception", LogLevels.logERROR);
                    eErrorCode = IECISAErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }


                lEllapsedTime = watch.ElapsedMilliseconds;

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                Logger_AddLogException(e, "GetTransactionStatus::Exception", LogLevels.logERROR);

            }

            return bRes;
        }


        public bool RefundTransaction(string strFormatId,
                               string strIECISAUser,
                               string strIECISAMerchantID,
                               string strIECISAInstance,
                               string strIECISACentreID,
                               string strIECISAPosID,
                               string strIECISAServiceURL,
                               int iIECISAServiceTimeout,
                               string strIECISAMACKey,
                               string strIECISACustomerID,
                               string strIECISATemplate,
                               string strOriginalTransactionId,
                               string strOriginalCFTransactionId,
                               DateTime dtOriginalDate,
                               string strOriginalAuthNumber,
                               int iQuantity,
                               string strCurISOCode,
                               string strCurNumISOCode,
                               DateTime dtNow,
                               out IECISAErrorCode eErrorCode,
                               out string errorMessage,
                               out string strTransactionId)
        {
            bool bRes = false;
            strTransactionId = null;
            long lEllapsedTime = -1;
            Stopwatch watch = null;
            eErrorCode = IECISAErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            AddTLS12Support();


            try
            {
                string strUserReference = UserReference();//"20170428115224078891";//
                string strURL = strIECISAServiceURL + "/RefundTransaction";
                WebRequest request = WebRequest.Create(strURL);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = iIECISAServiceTimeout;

                Dictionary<string, object> oTransactionRequest = new Dictionary<string, object>();
                Dictionary<string, object> oCardRequestInfo = new Dictionary<string, object>();

                oTransactionRequest["Login"] = strIECISAUser;
                oTransactionRequest["TransactionType"] = "D";
                oTransactionRequest["Company"] = strIECISAMerchantID;
                oTransactionRequest["Instance"] = strIECISAInstance;
                oTransactionRequest["AcceptUrl"] = "/NOTEMPTY";
                oTransactionRequest["CancelUrl"] = "/NOTEMPTY";
                oTransactionRequest["MerchantTransactionId"] = strUserReference;
                oTransactionRequest["OriginalTransactionId"] = strOriginalTransactionId;
                oTransactionRequest["TransactionAmount"] = iQuantity;
                oTransactionRequest["TransactionCurrency"] = strCurNumISOCode;

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime dtSpain = TimeZoneInfo.ConvertTime(dtNow, TimeZoneInfo.Local, tzi);

                oCardRequestInfo["CF_User"] = strIECISAUser;
                oCardRequestInfo["CF_Date"] = dtSpain.ToString("ddMMyyyy");//"28042017";//
                oCardRequestInfo["CF_Time"] = dtSpain.ToString("HHmmss");//"135221";//
                oCardRequestInfo["CF_Lang"] = "ES";
                oCardRequestInfo["CF_XtnType"] = "D";
                oCardRequestInfo["CF_Orig_Store"] = strIECISACentreID;
                oCardRequestInfo["CF_Orig_Pos"] = strIECISAPosID;
                oCardRequestInfo["CF_Orig_TransID"] = strOriginalCFTransactionId;
                oCardRequestInfo["CF_Orig_Date"] = dtOriginalDate.ToString("ddMMyyyy");
                oCardRequestInfo["CF_Orig_AuthNum"] = strOriginalAuthNumber;
                oCardRequestInfo["Company"] = strIECISAMerchantID;
                oCardRequestInfo["Center"] = strIECISACentreID;
                oCardRequestInfo["Pos"] = strIECISAPosID;
                oCardRequestInfo["CF_TicketNumber"] = strUserReference;
                oCardRequestInfo["CF_Amount"] = iQuantity;
                oCardRequestInfo["CF_Currency"] = strCurNumISOCode;
                oCardRequestInfo["CF_CurrencyCode"] = strCurISOCode;

               

                if (strFormatId == "02")
                {
                    oCardRequestInfo["CF_FmtVer"] = strFormatId;
                    oCardRequestInfo["CF_CustomerID"] = strIECISACustomerID;
                    oCardRequestInfo["CF_Template"] = strIECISATemplate;

                    oCardRequestInfo["CF_MAC"] = HashCodeFMT2(strIECISAMACKey, ((string)oCardRequestInfo["CF_FmtVer"]) +
                                                                    ((string)oCardRequestInfo["CF_CustomerID"]) +
                                                                    ((string)oCardRequestInfo["CF_XtnType"]) +
                                                                    ((string)oCardRequestInfo["CF_User"]) +
                                                                    ((string)oCardRequestInfo["CF_Date"]) +
                                                                    ((string)oCardRequestInfo["CF_Time"]) +
                                                                    (Convert.ToString(oCardRequestInfo["CF_Amount"])) +
                                                                    ((string)oCardRequestInfo["CF_CurrencyCode"]) +
                                                                    ((string)oCardRequestInfo["CF_TicketNumber"]) +
                                                                    ((string)oCardRequestInfo["CF_Orig_Store"]) +
                                                                    ((string)oCardRequestInfo["CF_Orig_Pos"]) +
                                                                    ((string)oCardRequestInfo["CF_Orig_TransID"]) +
                                                                    ((string)oCardRequestInfo["CF_Orig_Date"]));

                }
                else
                {
                    oCardRequestInfo["CF_MAC"] = HashCodeFMT1(strIECISAMACKey, ((string)oCardRequestInfo["CF_XtnType"]) +
                                                                ((string)oCardRequestInfo["Company"]) +
                                                                ((string)oCardRequestInfo["Center"]) +
                                                                ((string)oCardRequestInfo["Pos"]) +
                                                                ((string)oCardRequestInfo["CF_Date"]) +
                                                                ((string)oCardRequestInfo["CF_Time"]) +
                                                                (Convert.ToString(oCardRequestInfo["CF_Amount"])) +
                                                                ((string)oCardRequestInfo["CF_CurrencyCode"]) +
                                                                ((string)oCardRequestInfo["CF_TicketNumber"]) +
                                                                ((string)oCardRequestInfo["CF_Orig_Store"]) +
                                                                ((string)oCardRequestInfo["CF_Orig_Pos"]) +
                                                                ((string)oCardRequestInfo["CF_Orig_TransID"]) +
                                                                ((string)oCardRequestInfo["CF_Orig_Date"]));
                }


                oTransactionRequest["CardRequestInfo"] = oCardRequestInfo;

                var json = JsonConvert.SerializeObject(oTransactionRequest);

                Logger_AddLogMessage(string.Format("RefundTransaction request.url={0}, request.json={1}", strURL, PrettyJSON(json)), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                watch = Stopwatch.StartNew();

                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();


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

                        Logger_AddLogMessage(string.Format("RefundTransaction response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.


                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        try
                        {
                            eErrorCode = (IECISAErrorCode)Convert.ToInt32(oResponse["ResultCode"]);
                        }
                        catch
                        {
                            eErrorCode = IECISAErrorCode.InternalError;
                        }

                        errorMessage = oResponse["ResultDescription"].ToString();

                        if (eErrorCode == IECISAErrorCode.OK)
                        {
                            strTransactionId = oResponse["TransactionId"].ToString();
                            bRes = true;
                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("RefundTransaction Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "RefundTransaction::Exception", LogLevels.logERROR);
                    eErrorCode = IECISAErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }


                lEllapsedTime = watch.ElapsedMilliseconds;

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                Logger_AddLogException(e, "StartConfirmPreAuth::Exception", LogLevels.logERROR);

            }

            return bRes;
        }



        public bool StartTokenDeletion(string strFormatId,
                                string strIECISAUser,
                              string strIECISAMerchantID,
                              string strIECISAInstance,
                              string strIECISACentreID,
                              string strIECISAPosID,
                              string strIECISAServiceURL,
                              int iIECISAServiceTimeout,
                              string strIECISAMACKey,
                              string strIECISACustomerID,
                              string strIECISATemplate,
                              string strCardReference,
                              string strCardHash,
                              DateTime dtNow,
                              out IECISAErrorCode eErrorCode,
                              out string errorMessage,
                              out string strTransactionId)
        {
            bool bRes = false;
            strTransactionId = null;
            long lEllapsedTime = -1;
            Stopwatch watch = null;
            eErrorCode = IECISAErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            AddTLS12Support();

            try
            {
                string strUserReference = UserReference();
                string strURL = strIECISAServiceURL + "/Transaction";
                WebRequest request = WebRequest.Create(strURL);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Timeout = iIECISAServiceTimeout;

                Dictionary<string, object> oTransactionRequest = new Dictionary<string, object>();
                Dictionary<string, object> oCardRequestInfo = new Dictionary<string, object>();

                oTransactionRequest["Login"] = strIECISAUser;
                oTransactionRequest["TransactionType"] = "B";
                oTransactionRequest["Company"] = strIECISAMerchantID;
                oTransactionRequest["Instance"] = strIECISAInstance;
                oTransactionRequest["AcceptUrl"] = "/NOTEMPTY";
                oTransactionRequest["CancelUrl"] = "/NOTEMPTY";
                oTransactionRequest["MerchantTransactionId"] = strUserReference;
                oTransactionRequest["Token"] = strCardReference;

                TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById("Romance Standard Time");
                DateTime dtSpain = TimeZoneInfo.ConvertTime(dtNow, TimeZoneInfo.Local, tzi);

                oCardRequestInfo["CF_User"] = strIECISAUser;
                oCardRequestInfo["CF_Date"] = dtSpain.ToString("ddMMyyyy");
                oCardRequestInfo["CF_Time"] = dtSpain.ToString("HHmmss");
                oCardRequestInfo["CF_Lang"] = "ES";
                oCardRequestInfo["CF_XtnType"] = "B";
                oCardRequestInfo["Company"] = strIECISAMerchantID;
                oCardRequestInfo["Center"] = strIECISACentreID;
                oCardRequestInfo["Pos"] = strIECISAPosID;
                oCardRequestInfo["CF_TicketNumber"] = strUserReference;
                
                if (strFormatId == "02")
                {
                    oCardRequestInfo["CF_FmtVer"] = strFormatId;
                    oCardRequestInfo["CF_CustomerID"] = strIECISACustomerID;
                    oCardRequestInfo["CF_Template"] = strIECISATemplate;
                    oCardRequestInfo["CF_Ref_Token_Cli"] = strCardHash;
                    oCardRequestInfo["CF_Token"] = strCardReference;



                    oCardRequestInfo["CF_MAC"] = HashCodeFMT2(strIECISAMACKey, ((string)oCardRequestInfo["CF_FmtVer"]) +
                                                                    ((string)oCardRequestInfo["CF_CustomerID"]) +
                                                                    ((string)oCardRequestInfo["CF_User"]) +
                                                                    ((string)oCardRequestInfo["CF_Token"]) +
                                                                    ((string)oCardRequestInfo["CF_Ref_Token_Cli"]));

                }
                else
                {
                    oCardRequestInfo["CF_MAC"] = HashCodeFMT1(strIECISAMACKey, ((string)oCardRequestInfo["CF_XtnType"]) +
                                                                                    ((string)oCardRequestInfo["Company"]) +
                                                                                    ((string)oCardRequestInfo["Center"]) +
                                                                                    ((string)oCardRequestInfo["Pos"]) +
                                                                                    ((string)oCardRequestInfo["CF_Date"]) +
                                                                                    ((string)oCardRequestInfo["CF_Time"]) +
                                                                                    ((string)oTransactionRequest["Token"]) +
                                                                                    ((string)oCardRequestInfo["CF_TicketNumber"]));
                }




                oTransactionRequest["CardRequestInfo"] = oCardRequestInfo;

                var json = JsonConvert.SerializeObject(oTransactionRequest);

                Logger_AddLogMessage(string.Format("StartTokenDeletion request.url={0}, request.json={1}", strURL, PrettyJSON(json)), LogLevels.logINFO);

                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                request.ContentLength = byteArray.Length;
                // Get the request stream.
                watch = Stopwatch.StartNew();

                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();


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

                        Logger_AddLogMessage(string.Format("StartTokenDeletion response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.


                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        try
                        {
                            eErrorCode = (IECISAErrorCode)Convert.ToInt32(oResponse["ResultCode"]);
                        }
                        catch
                        {
                            eErrorCode = IECISAErrorCode.InternalError;
                        }

                        errorMessage = oResponse["ResultDescription"].ToString();

                        if (eErrorCode == IECISAErrorCode.OK)
                        {
                            strTransactionId = oResponse["TransactionId"].ToString();
                            bRes = true;
                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("StartTokenDeletion Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "StartTokenDeletion::Exception", LogLevels.logERROR);
                    eErrorCode = IECISAErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }


                lEllapsedTime = watch.ElapsedMilliseconds;

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                Logger_AddLogException(e, "StartTokenDeletion::Exception", LogLevels.logERROR);

            }

            return bRes;
        }


        public bool CompleteTokenDeletion(string strIECISAServiceURL,
                                          int iIECISAServiceTimeout,
                                          string strIECISAMACKey,
                                          string strTransactionId,
                                          int iCurrentRetries,
                                          int iMaxRetries,
                                          out IECISAErrorCode eErrorCode,
                                          out string errorMessage,                                         
                                          out DateTime? dtTransactionDate,
                                          out string strCFTransactionID,
                                          out string strAuthCode)
        {
            bool bRes = false;
            long lEllapsedTime = -1;
            Stopwatch watch = null;
            eErrorCode = IECISAErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];
            dtTransactionDate = null;
            strAuthCode = "";
            strCFTransactionID = "";


            AddTLS12Support();


            try
            {
                string strUserReference = UserReference();
                string strURL = string.Format("{0}/RemoveToken/BANKCARD/{1}", strIECISAServiceURL, strTransactionId);
                WebRequest request = WebRequest.Create(strURL);

                request.Method = "GET";
                request.ContentType = "application/json";
                request.Timeout = iIECISAServiceTimeout;

                Logger_AddLogMessage(string.Format("CompleteTokenDeletion request.url={0}", strURL), LogLevels.logINFO);

                watch = Stopwatch.StartNew();


                try
                {

                    WebResponse response = request.GetResponse();
                    // Display the status.
                    HttpWebResponse oWebResponse = ((HttpWebResponse)response);


                    if (oWebResponse.StatusDescription == "OK")
                    {
                        // Get the stream containing content returned by the server.
                        Stream dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.

                        Logger_AddLogMessage(string.Format("CompleteTokenDeletion response.json={0}", PrettyJSON(responseFromServer)), LogLevels.logINFO);
                        // Clean up the streams.


                        dynamic oResponse = JsonConvert.DeserializeObject(responseFromServer);

                        try
                        {
                            eErrorCode = (IECISAErrorCode)Convert.ToInt32(oResponse["ResultCode"]);

                        }
                        catch
                        {
                            eErrorCode = IECISAErrorCode.InternalError;
                        }

                        errorMessage = oResponse["ResultDescription"].ToString();

                        if (eErrorCode == IECISAErrorCode.OK)
                        {


                            strCFTransactionID = oResponse["CardResultInfo"]["CF_TransactionID"];
                            eErrorCode = (IECISAErrorCode)Convert.ToInt32(oResponse["CardResultInfo"]["CF_Result"]);
                            errorMessage = oResponse["CardResultInfo"]["CF_Des_Result"];
                            strAuthCode = oResponse["CardResultInfo"]["CF_NumAut"];


                            /*if (strMAC != strCalcMAC)
                            {
                                eErrorCode = IECISAErrorCode.InvalidMAC;
                                errorMessage = ErrorMessageDict[eErrorCode];

                            }
                            else
                            {*/


                            if (string.IsNullOrEmpty(errorMessage))
                            {
                                errorMessage = ErrorMessageDict[eErrorCode];
                            }

                            string strTime = oResponse["CardResultInfo"]["CF_Time"];
                            string strDate = oResponse["CardResultInfo"]["CF_Date"];

                            try
                            {
                                if ((strTime.Length == 6) && (strDate.Length == 8))
                                {

                                    dtTransactionDate = DateTime.ParseExact(strTime + strDate, "HHmmssddMMyyyy",
                                                  CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    dtTransactionDate = DateTime.Now;
                                }

                            }
                            catch
                            {
                                dtTransactionDate = DateTime.Now;
                            }
                            bRes = true;
                            //}

                        }
                        else
                        {
                            string strInternalError = oResponse["CardResultInfo"]["CF_Result"];
                            if ((strInternalError == "090") ||
                                (strInternalError == "092") ||
                                (strInternalError == "093") ||
                                (strInternalError == "094") ||
                                (strInternalError == "095"))
                            {
                                if (iCurrentRetries == iMaxRetries)
                                {
                                    eErrorCode = IECISAErrorCode.OK;
                                    errorMessage = ErrorMessageDict[eErrorCode];
                                    dtTransactionDate = DateTime.Now;
                                }

                            }



                        }

                        reader.Close();
                        dataStream.Close();
                    }

                    response.Close();
                }
                catch (WebException e)
                {
                    Logger_AddLogMessage(string.Format("CompleteTokenDeletion Web Exception HTTP Status={0}", ((HttpWebResponse)e.Response).StatusCode), LogLevels.logINFO);
                    Logger_AddLogException(e, "CompleteTokenDeletion::Exception", LogLevels.logERROR);
                    eErrorCode = IECISAErrorCode.ConnectionFailed;
                    errorMessage = ErrorMessageDict[eErrorCode];
                }


                lEllapsedTime = watch.ElapsedMilliseconds;

            }
            catch (Exception e)
            {
                if ((watch != null) && (lEllapsedTime == -1))
                {
                    lEllapsedTime = watch.ElapsedMilliseconds;
                }
                Logger_AddLogException(e, "CompleteTokenDeletion::Exception", LogLevels.logERROR);

            }

            return bRes;
        }

        public static string UserReference()
        {
            return string.Format("{0:yyyyMMddHHmmssfff}{1:000}", DateTime.Now.ToUniversalTime(), m_oRandom.Next(0, 999));
        }

        public static string TokenReference(string strText)
        {
            Regex rgx = new Regex("[^a-z0-9]");
            string strFilteredText = rgx.Replace(strText.ToLower(), "");
            string strRes=string.Format("{0:yyyyMMddHHmmssfffff}{1:000000000}{2}", DateTime.Now.ToUniversalTime(), m_oRandom.Next(0, 999999999), strFilteredText);
            return strRes.Length <= 64 ? strRes : strRes.Substring(0, 64);
        }


        public static string HashCodeFMT1(string strIECISAMACKey, string strHashCodeSrc)
        {


            string strRes = "";

            if (!string.IsNullOrEmpty(strIECISAMACKey))
            {

                byte[] key = ParseHexString(strIECISAMACKey.ToString());

                byte[] keyA = new byte[8]; ;
                byte[] keyB = new byte[8];
                Array.Copy(key, 0, keyA, 0, 8);
                Array.Copy(key, 8, keyB, 0, 8);

                byte[] data = System.Text.Encoding.ASCII.GetBytes(strHashCodeSrc);
                Array.Resize(ref data, data.Count() + 1);
                data[data.Length - 1] = 0x80;
                PadToMultipleOf(ref data, 8);

                DESCryptoServiceProvider tdesCrypt = new DESCryptoServiceProvider();
                tdesCrypt.Key = keyA;
                tdesCrypt.Mode = CipherMode.ECB;
                tdesCrypt.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransformCrypt = tdesCrypt.CreateEncryptor();

                byte[] byWorkBlock = new byte[8];
                byte[] temp = new byte[8];
                byte[] resultArray = null;

                Array.Copy(data, byWorkBlock, 8);
                int i = 0;
                while (i < data.Length)
                {
                    //transform the specified region of bytes array to resultArray
                    resultArray = cTransformCrypt.TransformFinalBlock(byWorkBlock, 0, byWorkBlock.Length);
                    i += 8;

                    if (i < data.Length)
                    {
                        Array.Copy(data, i, temp, 0, 8);
                        xor(resultArray, temp, ref byWorkBlock);
                        resultArray = null;
                    }
                }

                if (resultArray != null)
                {
                    Array.Resize(ref resultArray, 8);

                    DESCryptoServiceProvider tdesDecrypt = new DESCryptoServiceProvider();
                    tdesDecrypt.Key = keyB;
                    tdesDecrypt.Mode = CipherMode.ECB;
                    tdesDecrypt.Padding = PaddingMode.None;

                    ICryptoTransform cTransformDecrypt = tdesDecrypt.CreateDecryptor();

                    byte[] resultTemp = cTransformDecrypt.TransformFinalBlock(resultArray, 0, resultArray.Length);
                    resultArray = null;
                    resultArray = cTransformCrypt.TransformFinalBlock(resultTemp, 0, resultTemp.Length);

                    tdesDecrypt.Clear();
                    for (i = 0; i < 4; i++)
                    {
                        strRes += resultArray[i].ToString("X2");
                    }

                }

                tdesCrypt.Clear();

            }

            return strRes;
        }

        
        public static string HashCodeFMT2( string strIECISAMACKey, string strHashCodeSrc)
        {


            string strRes = "";

            if (!string.IsNullOrEmpty(strIECISAMACKey))
            {

                byte[] key = ParseHexString(strIECISAMACKey.ToString());

                byte[] keyA = new byte[8]; ;
                byte[] keyB = new byte[8] ;
                Array.Copy(key, 0, keyA, 0, 8);
                Array.Copy(key, 8, keyB, 0, 8);              

                byte[] data = System.Text.Encoding.ASCII.GetBytes(strHashCodeSrc);
                /*Array.Resize(ref data, data.Count() + 1);
                data[data.Length - 1] = 0x80;*/
                PadToMultipleOf(ref data, 8);

                DESCryptoServiceProvider tdesCrypt = new DESCryptoServiceProvider();
                tdesCrypt.Key = keyA;
                tdesCrypt.Mode = CipherMode.ECB;
                tdesCrypt.Padding = PaddingMode.None;

                ICryptoTransform cTransformCrypt = tdesCrypt.CreateEncryptor();

                byte[] byWorkBlock = new byte[8];
                byte[] temp = new byte[8];
                byte[] resultArray=null;

                Array.Copy(data, byWorkBlock, 8);
                int i=0;
                while (i < data.Length)
                {                                    
                    //transform the specified region of bytes array to resultArray
                    resultArray = cTransformCrypt.TransformFinalBlock(byWorkBlock, 0, byWorkBlock.Length);
                    i += 8;

                    if (i < data.Length)
                    {
                        Array.Copy(data,i,temp,0, 8);
                        xor(resultArray, temp, ref byWorkBlock);
                        resultArray = null;
                    }
                }

                if (resultArray != null)
                {
                    Array.Resize(ref resultArray, 8);

                    DESCryptoServiceProvider tdesDecrypt = new DESCryptoServiceProvider();
                    tdesDecrypt.Key = keyB;
                    tdesDecrypt.Mode = CipherMode.ECB;
                    tdesDecrypt.Padding = PaddingMode.None;

                    ICryptoTransform cTransformDecrypt = tdesDecrypt.CreateDecryptor();

                    byte[] resultTemp = cTransformDecrypt.TransformFinalBlock(resultArray, 0, resultArray.Length);
                    resultArray = null;
                    resultArray = cTransformCrypt.TransformFinalBlock(resultTemp, 0, resultTemp.Length);

                    tdesDecrypt.Clear();
                    for (i = 0; i < 8; i++)
                    {
                        strRes += resultArray[i].ToString("X2");
                    }

                }

                tdesCrypt.Clear();

            }

            return strRes;
        }

        static private byte[] ParseHexString(string text)
        {
            if ((text.Length % 2) != 0)
            {
                throw new ArgumentException("Invalid length: " + text.Length);
            }

            if (text.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                text = text.Substring(2);
            }

            int arrayLength = text.Length / 2;
            byte[] byteArray = new byte[arrayLength];
            for (int i = 0; i < arrayLength; i++)
            {
                byteArray[i] = byte.Parse(text.Substring(i * 2, 2), NumberStyles.HexNumber);
            }

            return byteArray;
        }

        static private void PadToMultipleOf(ref byte[] src, int pad)
        {
            int len = (src.Length + pad - 1) / pad * pad;
            Array.Resize(ref src, len);
        }

        static private void xor(byte[] srcArr1, byte[] srcArr2, ref byte[] destArr)
        {
            if (srcArr1.Length < destArr.Length)
                throw new ArgumentException("arr1 < dest");
            if (srcArr2.Length < destArr.Length)
                throw new ArgumentException("arr2 < dest");


            for (int i = 0; i < destArr.Length; i++)
                destArr[i] = Convert.ToByte(srcArr1[i] ^ srcArr2[i]);
        }


        private bool FindParameters(string xmlIn, string xmlSearchTag, out Dictionary<string,string> parameters)
        {
            bool bRes = true;
            parameters = new Dictionary<string,string>();

            try
            {
                XmlDocument xmlInDoc = new XmlDocument();
                try
                {
                    xmlInDoc.LoadXml(xmlIn);
                   
                    XmlNodeList elemList = xmlInDoc.GetElementsByTagName(xmlSearchTag);     
                    for (int i = 0; i < elemList.Count; i++)     
                    {
                        foreach (XmlAttribute attribute in elemList[i].Attributes)
                        {
                            parameters[attribute.Name] = attribute.Value;
                        }

                        foreach (XmlNode Node in elemList[i].ChildNodes)
                        {
                            if (Node.HasChildNodes)
                            {
                                parameters[Node.Name] = Node.InnerText.Trim();
                            }
                            else
                            {
                                parameters[Node.Name] = null;
                            }
                        }
                    }

                    bRes = (parameters.Count() > 0);
                }
                catch
                {
                    bRes = false;
                }
            }
            catch (Exception e)
            {
                bRes = false;
                Console.Write(e.Message);
            }


            return bRes;
        }


        public static void AddTLS12Support()
        {
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Tls12) == 0) //Enable TLs 1.2
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            }
            if (((int)ServicePointManager.SecurityProtocol & (int)SecurityProtocolType.Ssl3) != 0) //Disable SSL3
            {
                ServicePointManager.SecurityProtocol &= ~SecurityProtocolType.Ssl3;
            }
        }
        private string PrettyXml(string xml)
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
                return "\r\n\t" + strRes.Replace("\r\n", "\r\n\t") + "\r\n";
            }
            catch
            {
                return "\r\n\t" + json + "\r\n";
            }
        }

        protected void Logger_AddLogMessage(string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg);
        }

        protected void Logger_AddLogException(Exception ex, string msg, LogLevels nLevel)
        {
            m_Log.LogMessage(nLevel, msg, ex);
        }

        public bool RefundEnabled
        {
            get { return true; }
        }
        public bool PartialRefundEnabled
        {
            get { return true; }
        }

    }
}
