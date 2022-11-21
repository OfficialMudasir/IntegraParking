using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Configuration;
using System.Globalization;
using integraMobile.Infrastructure.Logging.Tools;
using Newtonsoft.Json;
using Stripe;


namespace integraMobile.Infrastructure
{
    public class StripePayments : IPayments
    {
        private static readonly CLogWrapper m_Log = new CLogWrapper(typeof(StripePayments));
        protected const int DEFAULT_WS_TIMEOUT = 5000; //ms   

        static public bool PerformCharge(string strSecretKey,
                                  string strEmail, 
                                  string strCardToken,
                                  ref string strCustomerID,
                                  int iQuantity,
                                  string strCurISOCode,
                                  bool bAutoconf,
                                  out string strResult,
                                  out string strErrorCode,
                                  out string strErrorMessage,
                                  out string strCardScheme,
                                  out string strPAN,
                                  out string strExpirationDateMonth,
                                  out string strExpirationDateYear,
                                  out string strChargeID,
                                  out string strStripeDateTime)                            
        {
            bool bRes = false;
            string strIntCustomerId="";
            strResult="error";
            strErrorMessage="";
            strErrorCode="";
            strCardScheme = "";
            strPAN = "";
            strExpirationDateMonth = "";
            strExpirationDateYear = "";
            strChargeID = "";
            strStripeDateTime = "";
           

            AddTLS12Support();

            try
            {

                //StripeConfiguration.SetApiKey(strSecretKey);
                StripeConfiguration.ApiKey = strSecretKey;

                if (string.IsNullOrEmpty(strCustomerID))
                {
                    /*
                    var myCustomer = new StripeCustomerCreateOptions();
                    myCustomer.Email = strEmail;
                    myCustomer.SourceToken = strCardToken;
                    StripeCustomer stripeCustomer = null;
                    */
                    var myCustomer = new CustomerCreateOptions();
                    myCustomer.Email = strEmail;
                    myCustomer.Source = strCardToken;
                    Customer stripeCustomer = null;

                    try
                    {
                        //var customerService = new StripeCustomerService();
                        var customerService = new CustomerService();
                        stripeCustomer = customerService.Create(myCustomer);
                        strIntCustomerId = stripeCustomer.Id;

                    }
                    catch (Exception e)
                    {
                        strErrorCode = "unexpected_failure";
                        strErrorMessage = e.Message;
                        m_Log.LogMessage(LogLevels.logERROR, "StripePayments.PerformCharge: ", e);

                    }
                }
                else
                {
                    strIntCustomerId = strCustomerID;
                }


                if (!string.IsNullOrEmpty(strIntCustomerId))
                {
                    try
                    {
                        /*
                        var myCharge = new StripeChargeCreateOptions();

                        // always set these properties
                        myCharge.Amount = iQuantity;
                        myCharge.Currency = strCurISOCode;

                        // set this property if using a customer - this MUST be set if you are using an existing source!
                        myCharge.CustomerId = strIntCustomerId;

                        myCharge.Capture = bAutoconf;
                        var chargeService = new StripeChargeService();

                        StripeCharge stripeCharge = chargeService.Create(myCharge);
                        */
                        var myCharge = new ChargeCreateOptions();

                        // always set these properties
                        myCharge.Amount = iQuantity;
                        myCharge.Currency = strCurISOCode;

                        // set this property if using a customer - this MUST be set if you are using an existing source!
                        myCharge.Customer = strIntCustomerId;

                        myCharge.Capture = bAutoconf;
                        var chargeService = new ChargeService();

                        Charge stripeCharge = chargeService.Create(myCharge);

                        strResult = stripeCharge.Status;

                        if (stripeCharge.Status != "succeeded")
                        {
                            strErrorMessage = stripeCharge.FailureCode + " - " + stripeCharge.FailureMessage;

                        }
                        else
                        {
                            strCustomerID = strIntCustomerId;
                            /* // v6.3.6
                            strCardScheme = stripeCharge.Source.Card.Brand;
                            strPAN = "************" + stripeCharge.Source.Card.Last4;
                            strExpirationDateMonth = stripeCharge.Source.Card.ExpirationMonth.PadLeft(2, '0');
                            strExpirationDateYear = stripeCharge.Source.Card.ExpirationYear;
                            */
                            strCardScheme = stripeCharge.PaymentMethodDetails.Card.Brand;
                            strPAN = "************" + stripeCharge.PaymentMethodDetails.Card.Last4;
                            strExpirationDateMonth = stripeCharge.PaymentMethodDetails.Card.ExpMonth.ToString().PadLeft(2, '0');
                            strExpirationDateYear = stripeCharge.PaymentMethodDetails.Card.ExpYear.ToString();

                            strChargeID = stripeCharge.Id;
                            strStripeDateTime = stripeCharge.Created.ToString("HHmmssddMMyy");

                            m_Log.LogMessage(LogLevels.logINFO,
                                string.Format("StripePayments.PerformCharge Success: Token={0} ; CustomerID={1} ; Amount={2} ; Currency={3} ; CardScheme={4}; PAN={5}; ExpirationMonth={6}; ExpirationYear={7}; ChargeID={8}; StripeDateTime={9}",
                                                        strCardToken,
                                                        strCustomerID,
                                                        iQuantity,
                                                        strCurISOCode,
                                                        strCardScheme,
                                                        strPAN,
                                                        strExpirationDateMonth,
                                                        strExpirationDateYear,
                                                        strChargeID,
                                                        strStripeDateTime));
                            bRes = true;
                        }
                    }
                    catch (StripeException e)
                    {

                        /*
                            *
                        invalid_parameter
                        invalid_datetime 
                        invalid_hash 
                        configuration_not_found
                        unexpected_failure
                        null_token
                        invalid_email   
                        window_closed
                        invalid_number	The card number is not a valid credit card number.
                        invalid_expiry_month	The card's expiration month is invalid.
                        invalid_expiry_year	The card's expiration year is invalid.
                        invalid_cvc	The card's security code is invalid.
                        incorrect_number	The card number is incorrect.
                        expired_card	The card has expired.
                        incorrect_cvc	The card's security code is incorrect.
                        incorrect_zip	The card's zip code failed validation.
                        card_declined	The card was declined.
                        missing	There is no card on a customer that is being charged.
                        processing_error	An error occurred while processing the card.
                            */

                        strResult = "error";
                        strErrorCode = e.StripeError.Code;
                        strErrorMessage = e.StripeError.Message;
                        m_Log.LogMessage(LogLevels.logERROR, "StripePayments.PerformCharge: ", e);
                    }
                    catch (Exception e)
                    {
                        strResult = "error";
                        strErrorCode = "unexpected_failure";
                        strErrorMessage = e.Message;
                        m_Log.LogMessage(LogLevels.logERROR, "StripePayments.PerformCharge: ", e);

                    }
                }

            }
            catch (Exception e)
            {
                strResult = "error";
                strErrorCode = "unexpected_failure";
                strErrorMessage = e.Message;
                m_Log.LogMessage(LogLevels.logERROR, "StripePayments.PerformCharge: ", e);

            }

            return bRes;
        }


        static public bool CaptureCharge(string strSecretKey,
                                          string strChargeID,
                                          out string strResult,
                                          out string strErrorCode,
                                          out string strErrorMessage,
                                          out string strBalanceTransactionId)
        {
            bool bRes = false;
            strResult = "error";
            strErrorMessage = "";
            strErrorCode = "";
            strBalanceTransactionId = "";


            AddTLS12Support();

            try
            {
                /* // v6.3.6
                StripeConfiguration.SetApiKey(strSecretKey);
                */
                StripeConfiguration.ApiKey = strSecretKey;

                if (!string.IsNullOrEmpty(strChargeID))
                {

                    try
                    {
                        /* // v6.3.6
                        var chargeService = new StripeChargeService();
                        StripeCharge stripeCharge = chargeService.Capture(strChargeID);
                        */

                        var chargeService = new ChargeService();
                        Charge stripeCharge = chargeService.Capture(strChargeID);

                        if (stripeCharge.Status == "succeeded" && stripeCharge.Captured)
                        {
                            strBalanceTransactionId = stripeCharge.BalanceTransactionId;
                            bRes = true;
                            m_Log.LogMessage(LogLevels.logINFO, "StripePayments.CaptureCharge: Succeeded Capturing ChargeID " + strChargeID);
                        }
                        

                    }
                    catch (StripeException e)
                    {
                        strErrorCode = "unexpected_failure";
                        strErrorMessage = e.Message;
                        m_Log.LogMessage(LogLevels.logERROR, "StripePayments.CaptureCharge: ", e);


                        if (strErrorMessage.Contains(" has already been captured."))
                        {
                            m_Log.LogMessage(LogLevels.logINFO, "StripePayments.CaptureCharge: Succeeded Capturing ChargeID " + strChargeID);
                            bRes = true;
                        }

                    }
                    catch (Exception e)
                    {
                        strErrorCode = "unexpected_failure";
                        strErrorMessage = e.Message;
                        m_Log.LogMessage(LogLevels.logERROR, "StripePayments.CaptureCharge: ", e);

                    }
                }
                else
                {
                    strErrorCode = "invalid_parameter";
                    strErrorMessage = "Missing Charge ID";
                    m_Log.LogMessage(LogLevels.logERROR, "StripePayments.CaptureCharge: "+ strErrorCode + " "+ strErrorMessage);
                }
           
            }
            catch (Exception e)
            {
                strResult = "error";
                strErrorCode = "unexpected_failure";
                strErrorMessage = e.Message;
                m_Log.LogMessage(LogLevels.logERROR, "StripePayments.CaptureCharge: ", e);

            }

            return bRes;
        }



        static public bool RefundCharge(string strSecretKey,
                                        string strChargeID,
                                        int iAmount,
                                        out string strResult,
                                        out string strErrorCode,
                                        out string strErrorMessage,
                                        out string strBalanceTransactionId)
        {
            bool bRes = false;
            strResult = "error";
            strErrorMessage = "";
            strErrorCode = "";
            strBalanceTransactionId = "";


            AddTLS12Support();

            try
            {

                StripeConfiguration.SetApiKey(strSecretKey);

                if (!string.IsNullOrEmpty(strChargeID))
                {

                    try
                    {
                        /* // v6.3.6
                        var refundService = new StripeRefundService();

                        StripeRefund refund = refundService.Create(strChargeID, new StripeRefundCreateOptions()
                        {
                            Amount = iAmount,
                            Reason = StripeRefundReasons.RequestedByCustomer
                        });
                        */

                        var refundService = new RefundService();

                        var options = new RefundCreateOptions
                        {
                            Charge = strChargeID,
                            Amount = iAmount,
                            Reason = RefundReasons.RequestedByCustomer
                        };

                        Refund refund = refundService.Create(options);

                        if ((refund.Status == "succeeded"))
                        {
                            strBalanceTransactionId = refund.Id;
                            bRes = true;
                            m_Log.LogMessage(LogLevels.logINFO, "StripePayments.RefundCharge: Succeeded Refunding ChargeID " + strChargeID);

                        }
                        


                    }
                    catch (Exception e)
                    {
                        strErrorCode = "unexpected_failure";
                        strErrorMessage = e.Message;
                        m_Log.LogMessage(LogLevels.logERROR, "StripePayments.RefundCharge: ", e);

                    }
                }
                else
                {
                    strErrorCode = "invalid_parameter";
                    strErrorMessage = "Missing Charge ID";
                    m_Log.LogMessage(LogLevels.logERROR, "StripePayments.RefundCharge: " + strErrorCode + " " + strErrorMessage);
                }

            }
            catch (Exception e)
            {
                strResult = "error";
                strErrorCode = "unexpected_failure";
                strErrorMessage = e.Message;
                m_Log.LogMessage(LogLevels.logERROR, "StripePayments.RefundCharge: ", e);

            }

            return bRes;
        }



        static protected int Get3rdPartyWSTimeout()
        {
            int iRes = DEFAULT_WS_TIMEOUT;
            try
            {
                iRes = Convert.ToInt32(ConfigurationManager.AppSettings["3rdPartyWSTimeout"].ToString());
            }
            catch
            {
                iRes = DEFAULT_WS_TIMEOUT;
            }

            return iRes;

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
