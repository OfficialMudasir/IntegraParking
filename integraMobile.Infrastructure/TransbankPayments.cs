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
using Webpay.Transbank.Library;
using Webpay.Transbank.Library.Wsdl.OneClick;



namespace integraMobile.Infrastructure
{
    public class TransBankPayments : IPayments
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(TransBankPayments));
        private static Random m_oRandom = new Random(Convert.ToInt32(DateTime.UtcNow.Ticks % Int32.MaxValue));
        private const int TokenEndInscriptionErrorOffset = -100;

        public enum TransBankErrorCode
        {
            Approved = 0,
            Rejected = -1,
            Rejected2 = -2,
            Rejected3 = -3,
            Rejected4 = -4,
            Rejected5 = -5,
            Rejected6 = -6,
            Rejected7 = -7,
            Rejected8 = -8,
            DailyAMaxmountExceeded = -97,
            TransactionMaxAmountExceeded = -98,
            DailyMaxNumberOfPaymentsExceeded = -99,
            Rejected9 = -101,
            RejectedMustRetry = -102,
            Rejected10 = -103,
            Rejected11 = -104,
            Rejected12 = -105,
            MonthlyMaxmountExceeded = -106,
            TransactionMaxAmountExceeded2 = -107,
            UnAuthorizedTitle = -108,
            CancelledByCardHolder = -198,
            InternalError = -200,

        }


        static readonly public Dictionary<TransBankErrorCode, string> ErrorMessageDict = new Dictionary<TransBankErrorCode, string>()
        {
            {TransBankErrorCode.Approved,"Transaction Approved"},
            {TransBankErrorCode.Rejected,"Transaction Rejected"},
            {TransBankErrorCode.Rejected2,"Transaction Rejected"},
            {TransBankErrorCode.Rejected3,"Transaction Rejected"},
            {TransBankErrorCode.Rejected4,"Transaction Rejected"},
            {TransBankErrorCode.Rejected5,"Transaction Rejected"},
            {TransBankErrorCode.Rejected6,"Transaction Rejected"},
            {TransBankErrorCode.Rejected7,"Transaction Rejected"},
            {TransBankErrorCode.Rejected8,"Transaction Rejected"},
            {TransBankErrorCode.DailyAMaxmountExceeded,"Daily maximum amount exceeded"},
            {TransBankErrorCode.TransactionMaxAmountExceeded,"Transaction maximum amount exceeded"},
            {TransBankErrorCode.DailyMaxNumberOfPaymentsExceeded,"Daily max number of payments exceeded"},
            {TransBankErrorCode.Rejected9,"Transaction Rejected"},
            {TransBankErrorCode.RejectedMustRetry,"Rejected (Must Retry)"},
            {TransBankErrorCode.Rejected10,"Transaction Error"},
            {TransBankErrorCode.Rejected11,"Transaction Rejected"},
            {TransBankErrorCode.Rejected12,"Transaction Rejected"},
            {TransBankErrorCode.MonthlyMaxmountExceeded,"Monthly maximum amount exceeded"},
            {TransBankErrorCode.TransactionMaxAmountExceeded2,"Transaction maximum amount exceeded"},
            {TransBankErrorCode.UnAuthorizedTitle,"Unauthorized Title"},                                 
            {TransBankErrorCode.CancelledByCardHolder,"Cancelled By Card Holder"},            
            {TransBankErrorCode.InternalError,"Internal error."},
        };

        public bool TokenInitInscription(string TRBACON_ENVIRONMENT,
                                        string TRBACON_COMMERCECODE,
                                        string TRBACON_PUBLICCERT,
                                        string TRBACON_WEBPAYCERT,
                                        string TRBACON_PASSWORD,
                                        string strEmail,
                                        string strResponseURL,
                                        out TransBankErrorCode eErrorCode,
                                        out string errorMessage,
                                        out string strToken,
                                        out string strURLRedirect)
        {

            bool bRes = false;
            eErrorCode = TransBankErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            strToken = "";
            strURLRedirect = "";

            
            try
            {

               

                Webpay.Transbank.Library.Webpay oWebPay = getInstance(TRBACON_ENVIRONMENT,
                                                                    TRBACON_COMMERCECODE,
                                                                    TRBACON_PUBLICCERT,
                                                                    TRBACON_WEBPAYCERT,
                                                                    TRBACON_PASSWORD);


                Webpay.Transbank.Library.Wsdl.OneClick.oneClickInscriptionOutput resultOneClick = oWebPay.getOneClickTransaction().initInscription(strEmail, strEmail, strResponseURL);

                strToken = resultOneClick.token;
                strURLRedirect = resultOneClick.urlWebpay;

                if (!string.IsNullOrEmpty(strToken))
                {
                    eErrorCode = TransBankErrorCode.Approved;
                }
                else
                {
                    eErrorCode = TransBankErrorCode.Rejected;
                }

                errorMessage = ErrorMessageDict[eErrorCode];

                m_Log.LogMessage(LogLevels.logINFO, "TransBankPayments.TokenInitInscription: ");

                m_Log.LogMessage(LogLevels.logINFO, "Commerce Code = " + TRBACON_COMMERCECODE);
                m_Log.LogMessage(LogLevels.logINFO, "Token = " + strToken);
                m_Log.LogMessage(LogLevels.logINFO, "URL Webpay = " + strURLRedirect);

                bRes = true;


            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "TransBankPayments.TokenInitInscription: ", e);

            }

            return bRes;
            
        }


        public bool TokenEndInscription(string TRBACON_ENVIRONMENT,
                                        string TRBACON_COMMERCECODE,
                                        string TRBACON_PUBLICCERT,
                                        string TRBACON_WEBPAYCERT,
                                        string TRBACON_PASSWORD,
                                        string strToken,
                                        out TransBankErrorCode eErrorCode,
                                        out string errorMessage,
                                        out string strTokenUser,
                                        out string strPAN,
                                        out string strAuthCode,
                                        out string strCardType)
        {

            bool bRes = false;
            eErrorCode = TransBankErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            strTokenUser = "";
            strPAN = "";
            strAuthCode = "";
            strCardType = "";


            try
            {

                Webpay.Transbank.Library.Webpay oWebPay = getInstance(TRBACON_ENVIRONMENT,
                                                                     TRBACON_COMMERCECODE,
                                                                     TRBACON_PUBLICCERT,
                                                                     TRBACON_WEBPAYCERT,
                                                                     TRBACON_PASSWORD);

                Webpay.Transbank.Library.Wsdl.OneClick.oneClickFinishInscriptionOutput resultOneClick = oWebPay.getOneClickTransaction().finishInscription(strToken);

                strTokenUser = resultOneClick.tbkUser;
                strPAN = "************"+resultOneClick.last4CardDigits;
                strAuthCode = resultOneClick.authCode;
                strCardType = resultOneClick.creditCardType.ToString();

                int iErrorCode = Convert.ToInt32(resultOneClick.responseCode);

                if (iErrorCode == 0)
                {
                    eErrorCode = TransBankErrorCode.Approved;
                }
                else
                {
                    eErrorCode = (TransBankErrorCode)(iErrorCode + TokenEndInscriptionErrorOffset);

                }
                
                errorMessage = ErrorMessageDict[eErrorCode];

                m_Log.LogMessage(LogLevels.logINFO, "TransBankPayments.TokenEndInscription: ");

                m_Log.LogMessage(LogLevels.logINFO, "Commerce Code = " + TRBACON_COMMERCECODE);
                m_Log.LogMessage(LogLevels.logINFO, "Token = " + strToken);        
                m_Log.LogMessage(LogLevels.logINFO, "Token User = " + strTokenUser);
                m_Log.LogMessage(LogLevels.logINFO, "PAN = " + strPAN);
                m_Log.LogMessage(LogLevels.logINFO, "AuthCode = " + strAuthCode);
                m_Log.LogMessage(LogLevels.logINFO, "CardType = " + strCardType);
                m_Log.LogMessage(LogLevels.logINFO, "ResponseCode = " + resultOneClick.responseCode);
                m_Log.LogMessage(LogLevels.logINFO, "ResponseMessage = " + errorMessage);

                bRes = true;

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "TransBankPayments.TokenInitInscription: ", e);

            }

            return bRes;

        }



        public bool AutomaticTransaction(string TRBACON_ENVIRONMENT,
                                       string TRBACON_COMMERCECODE,
                                       string TRBACON_PUBLICCERT,
                                       string TRBACON_WEBPAYCERT,
                                       string TRBACON_PASSWORD,
                                       string strEmail,
                                       string strTokenUser,
                                       string strBuyOrder,
                                       string strAmount,
                                       out TransBankErrorCode eErrorCode,
                                       out string errorMessage,
                                       out string strTransaction,
                                       out string strAuthCode,
                                       out string strDateTime)
        {

            bool bRes = false;
            eErrorCode = TransBankErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            strTransaction = "";
            strAuthCode = "";
            strDateTime = "";


            try
            {

                Webpay.Transbank.Library.Webpay oWebPay = getInstance(TRBACON_ENVIRONMENT,
                                                                     TRBACON_COMMERCECODE,
                                                                     TRBACON_PUBLICCERT,
                                                                     TRBACON_WEBPAYCERT,
                                                                     TRBACON_PASSWORD);

                Webpay.Transbank.Library.Wsdl.OneClick.oneClickPayOutput resultOneClick = oWebPay.getOneClickTransaction().authorize(strBuyOrder, strTokenUser, strEmail, strAmount);

                eErrorCode = (TransBankErrorCode)resultOneClick.responseCode;
                errorMessage = ErrorMessageDict[eErrorCode];
                string strPAN = "************" + resultOneClick.last4CardDigits;
                strAuthCode = resultOneClick.authorizationCode;
                string strCardType = resultOneClick.creditCardType.ToString();
                if (eErrorCode == TransBankErrorCode.Approved)
                {
                    strTransaction = resultOneClick.transactionId.ToString();
                    strDateTime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                }



                m_Log.LogMessage(LogLevels.logINFO, "TransBankPayments.AutomaticTransaction: ");

                m_Log.LogMessage(LogLevels.logINFO, "Commerce Code = " + TRBACON_COMMERCECODE);
                m_Log.LogMessage(LogLevels.logINFO, "Token User = " + strTokenUser);
                m_Log.LogMessage(LogLevels.logINFO, "User = " + strEmail);
                m_Log.LogMessage(LogLevels.logINFO, "Buy Order = " + strBuyOrder);
                m_Log.LogMessage(LogLevels.logINFO, "Transaction = " + strTransaction);
                m_Log.LogMessage(LogLevels.logINFO, "PAN = " + strPAN);
                m_Log.LogMessage(LogLevels.logINFO, "AuthCode = " + strAuthCode);
                m_Log.LogMessage(LogLevels.logINFO, "CardType = " + strCardType);
                m_Log.LogMessage(LogLevels.logINFO, "ResponseCode = " + resultOneClick.responseCode);
                m_Log.LogMessage(LogLevels.logINFO, "ResponseMessage = " + errorMessage);

                bRes = true;

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "TransBankPayments.AutomaticTransaction: ", e);

            }

            return bRes;

        }



        public bool RefundTransaction(string TRBACON_ENVIRONMENT,
                                       string TRBACON_COMMERCECODE,
                                       string TRBACON_PUBLICCERT,
                                       string TRBACON_WEBPAYCERT,
                                       string TRBACON_PASSWORD,
                                       string strBuyOrder,
                                       out TransBankErrorCode eErrorCode,
                                       out string errorMessage,
                                       out string strRefundTransactionID)
        {

            bool bRes = false;
            eErrorCode = TransBankErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            strRefundTransactionID = "";


            try
            {

                Webpay.Transbank.Library.Webpay oWebPay = getInstance(TRBACON_ENVIRONMENT,
                                                                     TRBACON_COMMERCECODE,
                                                                     TRBACON_PUBLICCERT,
                                                                     TRBACON_WEBPAYCERT,
                                                                     TRBACON_PASSWORD);

                Webpay.Transbank.Library.Wsdl.OneClick.oneClickReverseOutput resultOneClick = oWebPay.getOneClickTransaction().reverseTransaction(strBuyOrder);

                if (resultOneClick.reversed)
                {
                    eErrorCode = TransBankErrorCode.Approved;
                    strRefundTransactionID = resultOneClick.reverseCode.ToString();
                }
                else
                {
                    eErrorCode = TransBankErrorCode.Rejected;
                }

                errorMessage = ErrorMessageDict[eErrorCode];


                m_Log.LogMessage(LogLevels.logINFO, "TransBankPayments.RefundTransaction: ");

                m_Log.LogMessage(LogLevels.logINFO, "Commerce Code = " + TRBACON_COMMERCECODE);
                m_Log.LogMessage(LogLevels.logINFO, "Buy Order = " + strBuyOrder);
                m_Log.LogMessage(LogLevels.logINFO, "Transaction ID = " + strRefundTransactionID);
                m_Log.LogMessage(LogLevels.logINFO, "Reversed = " + resultOneClick.reversed);
                m_Log.LogMessage(LogLevels.logINFO, "ResponseMessage = " + errorMessage);

                bRes = true;

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "TransBankPayments.RefundTransaction: ", e);

            }

            return bRes;

        }

        public bool DeleteToken(string TRBACON_ENVIRONMENT,
                                string TRBACON_COMMERCECODE,
                                string TRBACON_PUBLICCERT,
                                string TRBACON_WEBPAYCERT,
                                string TRBACON_PASSWORD,
                                string strEmail,
                                string strTokenUser,
                                out TransBankErrorCode eErrorCode,
                                out string errorMessage)
        {

            bool bRes = false;
            eErrorCode = TransBankErrorCode.InternalError;
            errorMessage = ErrorMessageDict[eErrorCode];

            try
            {

                Webpay.Transbank.Library.Webpay oWebPay = getInstance(TRBACON_ENVIRONMENT,
                                                                     TRBACON_COMMERCECODE,
                                                                     TRBACON_PUBLICCERT,
                                                                     TRBACON_WEBPAYCERT,
                                                                     TRBACON_PASSWORD);

                bool resultOneClick = oWebPay.getOneClickTransaction().oneClickremoveUserOutput(strTokenUser, strEmail);

                if (resultOneClick)
                {
                    eErrorCode = TransBankErrorCode.Approved;
                }
                else
                {
                    eErrorCode = TransBankErrorCode.Rejected;
                }

                errorMessage = ErrorMessageDict[eErrorCode];


                m_Log.LogMessage(LogLevels.logINFO, "TransBankPayments.DeleteToken: ");
                m_Log.LogMessage(LogLevels.logINFO, "Commerce Code = " + TRBACON_COMMERCECODE);
                m_Log.LogMessage(LogLevels.logINFO, "Token User = " + strTokenUser);
                m_Log.LogMessage(LogLevels.logINFO, "User = " + strEmail);
                m_Log.LogMessage(LogLevels.logINFO, "Result = " + resultOneClick);
                m_Log.LogMessage(LogLevels.logINFO, "ResponseMessage = " + errorMessage);

                bRes = true;

            }
            catch (Exception e)
            {
                m_Log.LogMessage(LogLevels.logERROR, "TransBankPayments.DeleteToken: ", e);

            }

            return bRes;

        }



        protected Webpay.Transbank.Library.Webpay getInstance(string TRBACON_ENVIRONMENT,
                                       string TRBACON_COMMERCECODE,
                                       string TRBACON_PUBLICCERT,
                                       string TRBACON_WEBPAYCERT,
                                       string TRBACON_PASSWORD)
        {
            AddTLS12Support();

            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).Replace("file:\\","")+"\\TransbankCerts\\";

            m_Log.LogMessage(LogLevels.logINFO, "Public Cert File: " + path + TRBACON_PUBLICCERT);
            m_Log.LogMessage(LogLevels.logINFO, "WebPay Cert File: " + path + TRBACON_WEBPAYCERT);


            Webpay.Transbank.Library.Configuration configuration = new Webpay.Transbank.Library.Configuration();
            configuration.Environment = TRBACON_ENVIRONMENT;
            configuration.CommerceCode = TRBACON_COMMERCECODE;
            configuration.PublicCert = path+TRBACON_PUBLICCERT;
            configuration.WebpayCert = path+TRBACON_WEBPAYCERT;
            configuration.Password = TRBACON_PASSWORD;

            return new Webpay.Transbank.Library.Webpay(configuration);
        }




        public static bool IsError(TransBankErrorCode eErrorCode)
        {
            return eErrorCode != TransBankErrorCode.Approved;
        }

        public static string UserReference()
        {
            return string.Format("{0:yyyyMMddHHmmss}{1:000}", DateTime.Now.ToUniversalTime(), m_oRandom.Next(0, 999));
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
            get { return false; }
        }
    }
}
